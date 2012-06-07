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
using Mooege.Core.GS.Common.Types.TagMap;

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
        private bool _spawned;  // using my own spawn flag cause Actor.Spawned isn't being used right now

        public Projectile(PowerContext context, int actorSNO, Vector3D position)
            : base(context.World, actorSNO)
        {
            this.Field2 = 0x8;
            this.Field7 = 0x00000001;  // TODO: test if this is necessary

            if (this.Scale == 0f)
                this.Scale = 1.00f;

            this.Context = context;
            this.Position = new Vector3D(position);
            // offset position by mpq collision data
            this.Position.Z += this.ActorData.Cylinder.Ax1 - this.ActorData.Cylinder.Position.Z;
            // 2 second default timeout for projectiles
            this.Timeout = new SecondsTickTimer(context.World.Game, 2f);

            // copy in important effect params from user
            this.Attributes[GameAttribute.Rune_A, context.PowerSNO] = context.User.Attributes[GameAttribute.Rune_A, context.PowerSNO];
            this.Attributes[GameAttribute.Rune_B, context.PowerSNO] = context.User.Attributes[GameAttribute.Rune_B, context.PowerSNO];
            this.Attributes[GameAttribute.Rune_C, context.PowerSNO] = context.User.Attributes[GameAttribute.Rune_C, context.PowerSNO];
            this.Attributes[GameAttribute.Rune_D, context.PowerSNO] = context.User.Attributes[GameAttribute.Rune_D, context.PowerSNO];
            this.Attributes[GameAttribute.Rune_E, context.PowerSNO] = context.User.Attributes[GameAttribute.Rune_E, context.PowerSNO];

            _prevUpdatePosition = null;
            _mover = new ActorMover(this);
            _spawned = false;
        }

        public void Launch(Vector3D targetPosition, float speed)
        {
            _onArrivalCalled = false;
            _prevUpdatePosition = this.Position;

            this.TranslateFacing(targetPosition, true);
            targetPosition = new Vector3D(targetPosition);
            targetPosition.Z += this.ActorData.Cylinder.Ax1 - this.ActorData.Cylinder.Position.Z;
            if (!_spawned)
            {
                this.EnterWorld(this.Position);
                _spawned = true;
            }

            _mover.MoveFixed(targetPosition, speed, new ACDTranslateFixedMessage
            {
                Field2 = 0x00800000,
                AnimationTag = AnimationSetKeys.IdleDefault.ID,
                Field4 = -1
            });
        }

        public void LaunchArc(Vector3D destination, float arcHeight, float arcGravity, float visualBounce = 0f)
        {
            _onArrivalCalled = false;
            _prevUpdatePosition = this.Position;

            this.TranslateFacing(destination, true);
            if (!_spawned)
            {
                this.EnterWorld(this.Position);
                _spawned = true;
            }

            _mover.MoveArc(destination, arcHeight, arcGravity, new ACDTranslateArcMessage
            {
                Field3 = 0x00800000,
                FlyingAnimationTagID = AnimationSetKeys.IdleDefault.ID,
                LandingAnimationTagID = -1,
                PowerSNO = this.Context.PowerSNO,
                Bounce = visualBounce
            });
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
            if (!_spawned) return;

            // gotta make sure the actor hasn't been deleted after processing each handler

            if (this.World != null)
                _CheckCollisions();

            // doing updates after collision tests
            if (this.World != null)
            {
                _prevUpdatePosition = this.Position;
                _mover.Update();
            }

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
