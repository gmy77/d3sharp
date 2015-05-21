﻿/*
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
using Mooege.Core.GS.Map;
using Mooege.Core.GS.Players;
using Mooege.Net.GS.Message;
using Mooege.Core.GS.Actors.Interactions;
using Mooege.Core.GS.Common.Types.TagMap;

namespace Mooege.Core.GS.Actors.Implementations
{
    [HandledSNO(3533 /* Ho-ho-horadrim */)]
    public class Cain : InteractiveNPC
    {
        public Cain(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
            this.Attributes[GameAttribute.MinimapActive] = true;
            Conversations.Add(new ConversationInteraction(72416));
            Conversations.Add(new ConversationInteraction(198588));
            Conversations.Add(new ConversationInteraction(73171));
            Interactions.Add(new IdentifyAllInteraction());
        }

        public override bool Reveal(Player player)
        {
            if (!base.Reveal(player))
                return false;

            return true;
        }
    }
}
