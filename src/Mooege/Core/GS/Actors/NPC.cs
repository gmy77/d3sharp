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
using Mooege.Common.MPQ.FileFormats.Types;
using Mooege.Core.GS.Map;
using Mooege.Net.GS.Message;

namespace Mooege.Core.GS.Actors
{
    public class NPC : Living
    {
        public override ActorType ActorType { get { return ActorType.Monster; } }

        public NPC(World world, int snoId, Dictionary<int, TagMapEntry> tags)
            : base(world, snoId, tags)
        {
            this.Field2 = 0x9;
            this.Field7 = 1;
            this.Field8 = snoId; //TODO check if this is not true for every actor / living? /fasbat
            this.Attributes[GameAttribute.TeamID] = 1;
            this.Attributes[GameAttribute.Is_NPC] = true;
        }
    }
}
