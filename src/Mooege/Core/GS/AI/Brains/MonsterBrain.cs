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
using Mooege.Core.GS.Actors;
using Mooege.Core.GS.Players;
using Mooege.Core.GS.Ticker;

namespace Mooege.Core.GS.AI.Brains
{
    public class MonsterBrain:Brain
    {
        /// <summary>
        /// Hostile actors in range.
        /// </summary>
        public List<Player> EnemiesInRange { get; protected set; }

        public MonsterBrain(Actor body)
            : base(body)
        {
        }

        public override void Think(int tickCounter)
        {
            if (this.Body is NPC) return;

            // Reading enemies in range from quad map is quite expensive, so read it once at the very start. /raist.
            this.EnemiesInRange = this.Body.GetPlayersInRange();

            if (this.EnemiesInRange.Count > 0) // if there are enemies around
                this.State = BrainState.Combat; // attack them.
            else
                this.State = BrainState.Wander; // else just wander around.            
        }
    }
}
