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
using System.Linq;
using Mooege.Common.Helpers;
using Mooege.Common.Helpers.Math;
using Mooege.Common.MPQ.FileFormats.Types;
using Mooege.Core.GS.Map;
using Mooege.Core.GS.Objects;
using Mooege.Core.GS.Players;
using Mooege.Net.GS.Message;
using Mooege.Net.GS.Message.Definitions.World;
using Mooege.Net.GS.Message.Fields;
using Mooege.Net.GS.Message.Definitions.Animation;
using Mooege.Net.GS.Message.Definitions.Effect;
using Mooege.Net.GS.Message.Definitions.Misc;
using Mooege.Common.MPQ;
using Mooege.Core.GS.Common.Types.SNO;
using System;
using Mooege.Core.GS.Common.Types.TagMap;
using MonsterFF = Mooege.Common.MPQ.FileFormats.Monster;
using ActorFF = Mooege.Common.MPQ.FileFormats.Actor;
using Mooege.Core.GS.AI.Brains;
using Mooege.Core.GS.Ticker;


namespace Mooege.Core.GS.Actors
{
    public class Minion : Living, IUpdateable
    {
        public Actor Master; //The player who summoned the minion.

        public override ActorType ActorType { get { return ActorType.Monster; } }

        public override int Quality
        {
            get
            {
                return (int)Mooege.Common.MPQ.FileFormats.SpawnType.Normal; //Seems like this was never implemented on the clientside, so using 0 is fine.
            }
            set
            {
                // Not implemented
            }
        }

        public Minion(World world, int snoId, Actor master, TagMap tags)
            : base(world, snoId, tags)
        {
            // The following two seems to be shared with monsters. One wonders why there isn't a specific actortype for minions.
            this.Master = master;
            this.Field2 = 0x8;
            this.GBHandle.Type = (int)GBHandleType.Monster; this.GBHandle.GBID = 1;
            this.Attributes[GameAttribute.Summoned_By_ACDID] = (int)master.DynamicID;
            this.Attributes[GameAttribute.TeamID] = master.Attributes[GameAttribute.TeamID];
        }

        public override void OnTargeted(Player player, TargetMessage message)
        {
        }

        public void Update(int tickCounter)
        {
            if (this.Brain == null)
                return;

            this.Brain.Update(tickCounter);
        }

        public void SetBrain(Mooege.Core.GS.AI.Brain brain)
        {
            this.Brain = brain;
        }

        public void AddPresetPower(int powerSNO)
        {
            (Brain as MinionBrain).AddPresetPower(powerSNO);
        }
    }
}
