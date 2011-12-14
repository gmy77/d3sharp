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
using Mooege.Net.GS.Message.Definitions.ACD;
using Mooege.Core.GS.Common.Types.Misc;
using Mooege.Core.GS.Objects;
using Mooege.Core.GS.Ticker;
using Mooege.Common.Logging;
using Mooege.Net.GS.Message;

namespace Mooege.Core.GS.Powers
{
    public class Projectile : Actor, IUpdateable
    {
        public static readonly Logger Logger = LogManager.CreateLogger();

        public override ActorType ActorType { get { return ActorType.Projectile; } }

        public PowerContext Context;

        public Func<Actor, bool> CollisionFilter = null;
        public Action<Actor> OnCollision = null;
        public Action OnUpdate = null;
        public Action OnArrival = null;
        public Action OnTimeout = null;
        public TickTimer Timeout = null;
        public bool DestroyOnArrival = false;
        public TickTimer ArrivalTime { get { return _mover.ArrivalTime; } }
        public bool Arrived { get { return _mover.Arrived; } }

        private ActorMover _mover;
        private Vector3D _prevUpdatePosition;
        private bool _onArrivalCalled;

        public Projectile(PowerContext context, int actorSNO, Vector3D position)
            : base(context.World, actorSNO)
        {
            this.Field2 = 0x8;
            this.Scale = 1.35f;
            this.Field7 = 0x00000001;  // TODO: test if this is necessary

            this.Context = context;
            this.Position = new Vector3D(position);
            this.Timeout = new SecondsTickTimer(context.World.Game, 2f);  // 2 second default timeout for projectiles

            // copy in important effect params from user
            this.Attributes[GameAttribute.Rune_A, context.PowerSNO] = context.User.Attributes[GameAttribute.Rune_A, context.PowerSNO];
            this.Attributes[GameAttribute.Rune_B, context.PowerSNO] = context.User.Attributes[GameAttribute.Rune_B, context.PowerSNO];
            this.Attributes[GameAttribute.Rune_C, context.PowerSNO] = context.User.Attributes[GameAttribute.Rune_C, context.PowerSNO];
            this.Attributes[GameAttribute.Rune_D, context.PowerSNO] = context.User.Attributes[GameAttribute.Rune_D, context.PowerSNO];
            this.Attributes[GameAttribute.Rune_E, context.PowerSNO] = context.User.Attributes[GameAttribute.Rune_E, context.PowerSNO];

            _prevUpdatePosition = null;
            _mover = new ActorMover(this);

            // offset position by mpq collision data
            this.Position.Z += this.ActorData.Cylinder.Ax1 - this.ActorData.Cylinder.Position.Z;
        }

        public void Launch(Vector3D targetPosition, float speed)
        {
            if (!this.Spawned)
                this.EnterWorld(this.Position);

            _mover.MoveFixed(targetPosition, speed, new ACDTranslateFixedMessage
            {
                Field2 = 0x00800000,
                AnimationTag = 0x00011000,  // seems all projectiles use walk ani
                Field4 = unchecked((int)0xFFFFFFFF)
            });

            _onArrivalCalled = false;
        }

        public void LaunchArc(Vector3D destination, float arcHeight, float arcGravity)
        {
            if (!this.Spawned)
                this.EnterWorld(this.Position);

            _mover.MoveArc(destination, arcHeight, arcGravity);
            _onArrivalCalled = false;
        }

        private void _CheckCollisions()
        {
            if (OnCollision == null) return;

            // check if we collided with anything since last update

            float radius = this.ActorData.Cylinder.Ax2;
            Circle startCircle = new Circle(_prevUpdatePosition.X, _prevUpdatePosition.Y, radius);
            // make a velocity representing the change to the current position
            Vector2F velocity = PowerMath.VectorWithoutZ(this.Position - _prevUpdatePosition);
            
            Actor hit = null;
            TargetList targets = this.Context.GetEnemiesInRadius(this.Position, radius + 25f);
            if (CollisionFilter != null)
                targets.Actors.RemoveAll(actor => !CollisionFilter(actor));
            targets.SortByDistanceFrom(_prevUpdatePosition);

            foreach (Actor target in targets.Actors)
            {
                float targetRadius = 1.5f; // TODO: use target.ActorData.Cylinder.Ax2 ?
                if (PowerMath.MovingCircleCollides(startCircle, velocity, new Circle(target.Position.X, target.Position.Y, targetRadius)))
                {
                    hit = target;
                    break;
                }
            }

            if (hit != null)
                OnCollision(hit);
        }
        
        public void Update(int tickCounter)
        {
            _prevUpdatePosition = this.Position;
            _mover.Update();

            // gotta make sure the actor hasn't been deleted after processing each handler

            if (OnUpdate != null)
                OnUpdate();

            if (this.World != null && this.Arrived)
            {
                if (OnArrival != null && _onArrivalCalled == false)
                {
                    _onArrivalCalled = true;
                    OnArrival();
                }
                if (this.World != null && this.DestroyOnArrival &&
                    this.Arrived) // double check arrival in case OnArrival() re-launched
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
