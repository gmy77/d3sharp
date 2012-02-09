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

using Mooege.Core.GS.Items;
using Mooege.Core.GS.Map;
using Mooege.Net.GS.Message;
using Mooege.Core.GS.Common.Types.TagMap;

namespace Mooege.Core.GS.Actors.Implementations.Artisans
{
    [HandledSNO(56949 /* PT_Jewler.acr */)]
    public class Jeweler : Artisan
    {
        public Jeweler(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
        }

        public void OnAddSocket(Players.Player player, Item item)
        {
            // TODO: Animate Jeweler? Who knows. /fasbat
            item.Attributes[GameAttribute.Sockets] += 1;
            // Why this not work? :/
            item.Attributes.SendChangedMessage(player.InGameClient);
        }
    }
}
