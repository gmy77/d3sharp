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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mooege.Core.GS.Actors;
using Mooege.Core.GS.Common.Types.Math;
using Mooege.Core.GS.Ticker;
using Mooege.Net.GS.Message.Definitions.ACD;
using Mooege.Net.GS.Message.Definitions.Actor;
using Mooege.Core.GS.Common.Types.Misc;

namespace Mooege.Core.GS.Powers
{
    public class ActorMover
    {
        public Actor Target;

        public Vector3D Velocity { get; private set; }
        public TickTimer ArrivalTime { get; private set; }
        public bool Arrived { get { return ArrivalTime.TimedOut; } }

        private Vector3D _startPosition;
        private Vector3D _endPosition;
        private int _startTick;
        private float _arcGravity;

        private enum MoveCommandType
        {
            Normal,
            Fixed,
            Arc
        }
        private MoveCommandType _moveCommand;

        public ActorMover(Actor target)
        {
            this.Target = target;
        }

        public void Move(Vector3D destination, float speed, ACDTranslateNormalMessage baseMessage = null)
        {
            _SetupMove(destination, speed);
            _moveCommand = MoveCommandType.Normal;

            if (baseMessage == null)
                baseMessage = new ACDTranslateNormalMessage();

            baseMessage.ActorId = (int)this.Target.DynamicID;
            baseMessage.Position = destination;
            baseMessage.Angle = (float)Math.Acos(this.Target.RotationW) * 2f;
            baseMessage.Speed = speed;

            this.Target.World.BroadcastIfRevealed(baseMessage, this.Target);
        }

        public void MoveFixed(Vector3D targetPosition, float speed, ACDTranslateFixedMessage baseMessage = null)
        {
            _SetupMove(targetPosition, speed);
            _moveCommand = MoveCommandType.Fixed;

            if (baseMessage == null)
                baseMessage = new ACDTranslateFixedMessage();

            baseMessage.ActorId = (int)this.Target.DynamicID;
            baseMessage.Velocity = this.Velocity;

            this.Target.World.BroadcastIfRevealed(baseMessage, this.Target);
        }

        public void MoveArc(Vector3D destination, float height, float gravity, ACDTranslateArcMessage baseMessage = null)
        {
            _SetupArcMove(destination, height, gravity);
            _moveCommand = MoveCommandType.Arc;

            if (baseMessage == null)
                baseMessage = new ACDTranslateArcMessage();

            baseMessage.ActorId = (int)this.Target.DynamicID;
            baseMessage.Start = this.Target.Position;
            baseMessage.Velocity = this.Velocity;
            baseMessage.Gravity = gravity;
            baseMessage.DestinationZ = destination.Z;

            this.Target.World.BroadcastIfRevealed(baseMessage, this.Target);
        }

        public bool Update()
        {
            _UpdatePosition();
            return this.Arrived;
        }

        private void _SetupMove(Vector3D destination, float speed)
        {
            Vector3D dir_normal = PowerMath.Normalize(new Vector3D(destination.X - this.Target.Position.X,
                                                                   destination.Y - this.Target.Position.Y,
                                                                   destination.Z - this.Target.Position.Z));

            this.Velocity = new Vector3D(dir_normal.X * speed,
                                         dir_normal.Y * speed,
                                         dir_normal.Z * speed);

            this.ArrivalTime = new RelativeTickTimer(this.Target.World.Game,
                                                     (int)(PowerMath.Distance2D(this.Target.Position, destination) / speed));
            _startPosition = this.Target.Position;
            _endPosition = destination;
            _startTick = this.Target.World.Game.TickCounter;
        }

        private void _SetupArcMove(Vector3D destination, float crestHeight, float gravity)
        {
            // TODO: handle when target and destination heights differ
            float absGravity = Math.Abs(gravity);
            float arcLength = (float)Math.Sqrt(2f * crestHeight / absGravity);
            int arrivalTicks = (int)(arcLength * 2f);

            float distance = PowerMath.Distance2D(this.Target.Position, destination);
            Vector3D normal = PowerMath.Normalize(new Vector3D(destination.X - this.Target.Position.X,
                                                               destination.Y - this.Target.Position.Y,
                                                               0f));

            this.Velocity = new Vector3D(normal.X * (distance / arrivalTicks),
                                         normal.Y * (distance / arrivalTicks),
                                         absGravity * arcLength);

            this.ArrivalTime = new RelativeTickTimer(this.Target.World.Game, arrivalTicks);
            _startPosition = this.Target.Position;
            _endPosition = destination;
            _startTick = this.Target.World.Game.TickCounter;
            _arcGravity = gravity;
        }

        private void _UpdatePosition()
        {
            if (_moveCommand != MoveCommandType.Fixed && this.Arrived)
            {
                this.Target.Position = _endPosition;
                return;
            }

            int moveTicks = this.Target.World.Game.TickCounter - _startTick;

            if (_moveCommand == MoveCommandType.Arc)
            {
                this.Target.Position = new Vector3D(_startPosition.X + this.Velocity.X * moveTicks,
                    _startPosition.Y + this.Velocity.Y * moveTicks,
                    _startPosition.Z + 0.5f * _arcGravity * (moveTicks * moveTicks) + this.Velocity.Z * moveTicks);
            }
            else
            {
                this.Target.Position = new Vector3D(_startPosition.X + this.Velocity.X * moveTicks,
                    _startPosition.Y + this.Velocity.Y * moveTicks,
                    _startPosition.Z + this.Velocity.Z * moveTicks);
            }
        }
    }
}
