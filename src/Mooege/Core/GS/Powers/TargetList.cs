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

namespace Mooege.Core.GS.Powers
{
    public class TargetList
    {
        // list of actors that are the primary targets
        public List<Actor> Actors { get; private set; }

        // list of extra actors that are near the targets, i.e. destructables like barrels, tombstones etc.
        public List<Actor> ExtraActors { get; private set; }

        public TargetList()
        {
            this.Actors = new List<Actor>();
            this.ExtraActors = new List<Actor>();
        }

        public void SortByDistanceFrom(Vector3D position)
        {
            this.Actors = this.Actors.OrderBy(actor => PowerMath.Distance2D(actor.Position, position)).ToList();
        }

        public Actor GetClosestTo(Vector3D position)
        {
            Actor closest = null;
            float closestDistance = float.MaxValue;
            foreach (Actor actor in this.Actors)
            {
                float distance = PowerMath.Distance2D(actor.Position, position);
                if (distance < closestDistance)
                {
                    closest = actor;
                    closestDistance = distance;
                }
            }

            return closest;
        }
    }
}
