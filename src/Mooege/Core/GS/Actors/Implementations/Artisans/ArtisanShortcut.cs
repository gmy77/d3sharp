/*
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
using Mooege.Core.GS.Common.Types.TagMap;
using Mooege.Net.GS.Message;
using Mooege.Net.GS.Message.Definitions.Artisan;
using Mooege.Net.GS.Message.Definitions.World;
using Mooege.Core.GS.Players;

namespace Mooege.Core.GS.Actors.Implementations.Artisans
{
    [HandledSNO(0x0002FA63 /* PT_Blacksmith_ForgeWeaponShortcut.acr */,
        0x0002FA64 /*PT_Blacksmith_ForgeArmorShortcut.acr */,
        0x0002FA62 /*PT_Blacksmith_RepairShortcut.acr */,
        212519 /* Actor PT_Jeweler_AddSocketShortcut */,
        212517 /* Actor PT_Jeweler_CombineShortcut */,
        212521 /* Actor PT_Jeweler_RemoveGemShortcut */,
        212511 /* Actor PT_Mystic_EnhanceShortcut */,
        212510 /* Actor PT_Mystic_IdentifyShortcut */)]
    public class ArtisanShortcut : InteractiveNPC
    {
        public ArtisanShortcut(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
            Attributes[GameAttribute.MinimapActive] = false;
            Attributes[GameAttribute.Conversation_Icon, 0] = 0;
        }

        public override void OnTargeted(Player player, TargetMessage message)
        {
            player.InGameClient.SendMessage(new OpenArtisanWindowMessage() { ArtisanID = this.DynamicID });
        }
    }
}
