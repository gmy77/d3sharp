/*
 * Copyright (C) 2011 mooege project
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */

using System;
using System.Linq;
using Mooege.Core.GS.Actors;
using Mooege.Core.GS.Common.Types.Math;
using Mooege.Core.GS.Map;
using Mooege.Net.GS.Message.Definitions.ACD;
using Mooege.Core.GS.Common.Types.Misc;
using Mooege.Core.GS.Objects;
using Mooege.Net.GS.Message.Definitions.Actor;
using Mooege.Core.GS.Ticker;

namespace Mooege.Core.GS.Powers
{
    public class Projectile : Actor, IUpdateable
    {
        public override ActorType ActorType { get { return ActorType.Projectile; } }

        public PowerContext Context;
        public Vector3D Velocity { get; private set; }

        public Action<Actor> OnHit;
        public Action OnUpdate;
        public Action OnArrival;
        public Action OnTimeout;
        public TickTimer Timeout = null;
        public bool DestroyOnArrival = false;

        private TickTimer _arrivalTime;
        private int _lastUpdateTick;
        private Vector3D _prevUpdatePosition;
        private bool _onArrivalCalled;

        public Projectile(PowerContext context, int actorSNO, Vector3D position)
            : base(context.World, actorSNO)
        {
            this.Field2 = 0x8;
            this.Scale = 1.35f;
            // just use default? GBHandle.Projectile is 10, but most projectiles I see use 17
            //this.GBHandle.Type = (int)GBHandleType.Projectile; this.GBHandle.GBID = 1;
            this.Field7 = 0x00000001;
            // these no longer needed?
            //this.Field10 = 0x1;
            //this.Field11 = 0x1;
            //this.Field12 = 0x1;
            //this.Field13 = 0x1;
            //this.CollFlags = 0x4;
            
            this.Context = context;
            this.Position = new Vector3D(position);
            this.Timeout = new SecondsTickTimer(context.World.Game, 2f);  // 2 second default timeout for projectiles

            _arrivalTime = null;
            _lastUpdateTick = 0;
            _prevUpdatePosition = null;
            _onArrivalCalled = false;

            // offset position by mpq collision data
            this.Position.Z += this.ActorData.Cylinder.Ax1 - this.ActorData.Cylinder.Position.Z;
        }

        public void Launch(Vector3D targetPosition, float speed)
        {
            _SetupLaunch(targetPosition, speed);

            if (this.World == null)
                return;

            this.World.BroadcastIfRevealed(new ACDTranslateFixedMessage
            {
                Id = 113,
                ActorId = (int)this.DynamicID,
                Velocity = this.Velocity,
                Field2 = 0x00800000,
                AnimationTag = 0x00011000, // walk tag
                Field4 = unchecked((int)0xFFFFFFFF),
            }, this);
        }

        public TickTimer WaitForArrival()
        {
            return _arrivalTime;
        }
        
        private void _SetupLaunch(Vector3D target, float speed)
        {
            float facing = PowerMath.AngleLookAt(this.Position, target);
            this.FacingAngle = (float)Math.Cos(facing / 2f);
            this.RotationAxis = new Vector3D(0, 0, (float)Math.Sin(facing / 2f));
            
            Vector3D dir_normal = PowerMath.Normalize(new Vector3D(target.X - this.Position.X,
                                                                   target.Y - this.Position.Y,
                                                                   0f));  // were not moving in 3d for now
            
            this.Velocity = new Vector3D(dir_normal.X * speed,
                                         dir_normal.Y * speed,
                                         dir_normal.Z * speed);

            _arrivalTime = new RelativeTickTimer(this.World.Game, (int)(PowerMath.Distance2D(this.Position, target) / speed));
            _lastUpdateTick = this.World.Game.TickCounter;
            _prevUpdatePosition = this.Position;
            _onArrivalCalled = false;

            if (!this.Spawned)
                this.EnterWorld(this.Position);

            // TODO: do full update processing after launch?
            _CheckCollisions();
        }

        private void _UpdatePosition()
        {
            int deltaTick = this.World.Game.TickCounter - _lastUpdateTick;
            _lastUpdateTick = this.World.Game.TickCounter;
            _prevUpdatePosition = this.Position;
            this.Position = new Vector3D(this.Position.X + this.Velocity.X * deltaTick,
                                         this.Position.Y + this.Velocity.Y * deltaTick,
                                         this.Position.Z + this.Velocity.Z * deltaTick);
        }

        private void _CheckCollisions()
        {
            if (OnHit == null) return;

            // check if we collided with anything since last update

            float radius = this.ActorData.Cylinder.Ax2;
            Circle startCircle = new Circle(_prevUpdatePosition.X, _prevUpdatePosition.Y, radius);
            // make a velocity representing the change to the current position
            PowerMath.Vec2D velocity = PowerMath.Vec2D.WithoutZ(this.Position - _prevUpdatePosition);
            
            Actor hit = null;
            foreach (Actor target in this.GetMonstersInRange(radius + 25f))
            {
                float targetRadius = 1.5f; // target.ActorData.Cylinder.Ax2;
                if (PowerMath.MovingCircleCollides(startCircle, velocity, new Circle(target.Position.X, target.Position.Y, targetRadius)))
                {
                    hit = target;
                    break;
                }
            }

            if (hit != null)
                OnHit(hit);
        }

        private bool _ReckonArrivalTimedOut()
        {
            return _arrivalTime.TimedOut;// || this.World.Game.TickCounter + this.World.Game.TickRate >= _arrivalTime.TimeoutTick;
        }

        public void Update(int tickCounter)
        {
            _UpdatePosition();

            // gotta make sure the actor hasn't been deleted after processing each handler

            if (OnUpdate != null)
                OnUpdate();

            if (this.World != null && _ReckonArrivalTimedOut())
            {
                if (OnArrival != null && _onArrivalCalled == false)
                {
                    _onArrivalCalled = true;
                    OnArrival();
                }
                if (this.World != null && this.DestroyOnArrival &&
                    _ReckonArrivalTimedOut()) // double check arrival in case OnArrival() re-launched
                    Destroy();
            }

            if (this.World != null)
                _CheckCollisions();

            if (this.World != null)
            {
                if (Timeout.TimedOut)
                {
                    if (OnTimeout != null)
                        OnTimeout();

                    Destroy();
                }
            }
        }
    }
}
