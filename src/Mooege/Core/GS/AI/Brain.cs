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
using Mooege.Core.GS.Ticker.Helpers;
using System.Collections.Generic;

namespace Mooege.Core.GS.AI
{
    public class Brain
    {
        private static readonly Logger Logger = LogManager.CreateLogger();
        private List<Vector3D> path = new List<Vector3D>();

        public Actor Body { get; private set; }
        public Vector3D Heading { get; private set; }
        public Actor Target { get; private set; }

        public Brain(Actor body)
        {
            this.Body = body;
        }
       
        public virtual void Think(int tickCounter)
        {
            if (this.Body == null || this.Body.World == null) return; // hack fix until i get brain-states in. /raist.

            var players = this.Body.GetPlayersInRange();
            if (players.Count == 0) return;

            this.Chase(players.First());
        }

        public void Chase(Actor actor)
        {
            if (this.Heading == actor.Position)
                return;
            if (path.Count < 2)
            {
                path = Pathfinding.FindPath(this.Body, this.Body.Position, actor.Position);
                path.Reverse();
            }
            if (path.Count < 2) { return; }

            this.path.RemoveAt(0); // Each move will be 2f as we skip moves, first move removed as its only to move to our current loc

            this.Target = actor;
            this.Heading = this.path[0];// this.Target.Position;
            this.Move(path[0], ActorHelpers.GetFacingAngle(this.Body.Position,path[0]));
            this.Body.Position = path[0]; //Fix me i guess, actor bounces around without. -DarkLotus
            this.path.RemoveAt(0);
            //this.Move(this.Target);
        }

        public void Move(Vector3D position, float facingAngle)
        {
            this.Body.RotationAmount = facingAngle;
            this.Body.Move(position, facingAngle);
        }

        public void Move(Actor actor)
        {
            this.Move(actor.Position, ActorHelpers.GetFacingAngle(this.Body, actor));
            this.Body.Position = actor.Position; // hack /raist.
        }
    }
}
