﻿/*
 * Copyright (C) 2011 - 2012 mooege project - http://www.mooege.org
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

using Mooege.Core.GS.Map;
using Mooege.Core.GS.Players;
using Mooege.Core.GS.Common.Types.TagMap;
using Mooege.Net.GS.Message.Definitions.World;
using Mooege.Net.GS.Message.Definitions.Misc;
using Mooege.Net.GS.Message;

namespace Mooege.Core.GS.Actors.Implementations
{
    [HandledSNO(220930 /* Global_Nephalem_Altar.acr */)]
    public sealed class NephalemAltar : Gizmo
    {
        public NephalemAltar(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
            Attributes[GameAttribute.MinimapActive] = true;
        }

        public override void OnTargeted(Player player, TargetMessage message)
        {
            player.InGameClient.SendMessage(new ANNDataMessage(Opcodes.UseNephalemAltarMessage)
            {
                ActorID = this.DynamicID
            });

            player.Attributes[GameAttribute.Last_Altar_ANN] = (int)this.DynamicID;
            player.Attributes.BroadcastChangedIfRevealed();
            Attributes[GameAttribute.Gizmo_Has_Been_Operated] = true;
            Attributes.BroadcastChangedIfRevealed();
        }
    }
}
