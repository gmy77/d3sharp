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

using System.Collections.Generic;
using Mooege.Common.Helpers.Math;
using Mooege.Core.GS.Actors;
using Mooege.Core.GS.Actors.Movement;
using Mooege.Core.GS.Common.Types.Math;
using Mooege.Core.GS.Players;

namespace Mooege.Core.GS.AI.Brains
{
    public class MonsterBrain:Brain
    {
        /// <summary>
        /// Hostile actors in range.
        /// </summary>
        public List<Player> EnemiesInRange { get; protected set; }
        private Pathfinder pather;
        private List<Vector3D> path;
        private int nextPlayerSearch = 0;
        public MonsterBrain(Actor body)
            : base(body)
        {
            pather = new Pathfinder(this.Body.World);
        }

        private List<Player> GetPlayersInRange(Mooege.Core.GS.Map.World world)
        {
            // Not as clean and fancy as quadtreee, but the cost is like 1/10th or less. - Darklotus
            List<Player> playerList = new List<Player>();
            foreach (var p in world.Players.Values)
            {
                if (MovementHelpers.GetDistance(this.Body.Position, p.Position) < 240f)
                {
                    playerList.Add(p);
                }
            }
            return playerList;
        }
        public override void Think(int tickCounter)
        {
            if (this.Body is NPC) return;

            // Reading enemies in range from quad map is quite expensive, so read it once at the very start. /raist.
            //Temp fix till someone smart fixes it - DarkLotus
            if (nextPlayerSearch < tickCounter)
            {
                this.EnemiesInRange = this.GetPlayersInRange(Body.World);
                nextPlayerSearch = tickCounter + 5000;
            }
            

            if (this.EnemiesInRange.Count > 0) // if there are enemies around
                if((MovementHelpers.GetDistance(this.Body.Position, this.EnemiesInRange[0].Position) < 50f))
                {
                this.State = BrainState.Combat; // attack them
                }
            //else
                this.State = BrainState.Wander; // else just wander around.

            if (this.CurrentAction == null)
            {
                switch (this.State)
                { 
                    case BrainState.Wander:
                        this.Wander();
                        break;
                    case BrainState.Combat:
                        this.Combat();
                        break;
                }
                
            }
        }

        private void Wander()
        {
            var heading = new Vector3D(this.Body.Position.X + FastRandom.Instance.Next(-40, 40), this.Body.Position.Y + FastRandom.Instance.Next(-40, 40), this.Body.Position.Z);
            if (MovementHelpers.GetDistance(this.Body.Position, heading) > 5f && MovementHelpers.GetDistance(this.Body.Position, heading) < 50f)
            {
                if (path == null)
                {
                    path = Pather.GetPath(pather, this.Body, this.Body.Position, heading);
                    if (path == null) { return; } //Path should be waiting in cache when we tick around again - DarkLotus
                    path.Reverse();
                }
                if (path.Count < 1)
                {
                    //this.CurrentAction = new MoveToPointAction(this.Body, new Vector3D(this.path[1]));
                    path = null; return;
                }
                this.CurrentAction = new FollowPathAction(this.Body, this.path);

                return;
            }
            //if (this.Body.Position.DistanceSquared(ref heading) > this.Body.WalkSpeed * this.Body.World.Game.TickRate) // just skip the movements that can be accomplished in a single game.update(). /raist.
             //   this.CurrentAction = new MoveToPointAction(this.Body, heading);
        }
        

        private void Combat()
        {
            // Mobs should discard path their following if target destination changes. - DarkLotus
            var heading = new Vector3D(this.EnemiesInRange[0].Position);

            if (MovementHelpers.GetDistance(this.Body.Position, heading) > 5f && MovementHelpers.GetDistance(this.Body.Position, heading) < 30f)
            {
                if (path == null)
                {
                    path = Pather.GetPath(pather, this.Body, this.Body.Position, heading);
                    if (path == null) { return; }
                    path.Reverse();
                }
                if (path.Count < 1)
                {
                    //this.CurrentAction = new MoveToPointAction(this.Body, new Vector3D(this.path[1]));
                    path = null; return;
                }
                this.CurrentAction = new FollowPathAction(this.Body,this.path);
                
                return;
            }
        }
    }
}
