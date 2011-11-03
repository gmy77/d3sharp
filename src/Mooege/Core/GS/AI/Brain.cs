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
using Mooege.Common;
using Mooege.Core.GS.Actors;
using Mooege.Core.GS.Actors.Helpers;
using Mooege.Core.GS.Common.Types.Math;

namespace Mooege.Core.GS.AI
{
    public class Brain
    {
        private static readonly Logger Logger = LogManager.CreateLogger();

        public Actor Body { get; private set; }
        public Vector3D Heading { get; private set; }
        public Actor Target { get; private set; }

        public Brain(Actor body)
        {
            this.Body = body;
        }

        public virtual void Think(int tickCounter)
        {
            var players = this.Body.GetPlayersInRange();
            if (players.Count == 0) return;

            this.Chase(players.First());
        }

        public void Chase(Actor actor)
        {
            if (this.Heading == actor.Position && Math.Abs(this.Body.RotationAmount - ActorHelpers.GetFacingAngle(this.Body, actor)) < 0.1)
                return;

            this.Target = actor;
            this.Heading = this.Target.Position;

            Logger.Trace("Chasing " + actor);

            this.Move(this.Target);
        }

        public void Move(Vector3D position, float facingAngle)
        {
            this.Body.RotationAmount = facingAngle;
            this.Body.Move(position, facingAngle);
        }

        public void Move(Actor actor)
        {
            this.Move(actor.Position, ActorHelpers.GetFacingAngle(this.Body, actor));            
        }
    }
}
