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

using System.Collections.Generic;
using System.Diagnostics;
using Mooege.Net.GS.Message;
using Mooege.Core.GS.Players;
using Mooege.Net.GS.Message.Fields;
using Mooege.Core.GS.Map;

namespace Mooege.Core.GS.Items.Implementations
{
    // This is how Dyes should be implemented ;) /fasbat
    [HandledType("Dye")]
    public class Dye : Item
    {
        private static Dictionary<int, int> DyeColorMap = new Dictionary<int, int>();

        public Dye(World world, Mooege.Common.MPQ.FileFormats.ItemTable definition)
            : base(world, definition)
        {
        }

        public override void OnRequestUse(Player player, Item target, int actionId, WorldPlace worldPlace)
        {
            Debug.Assert(target != null);

            target.Attributes[GameAttribute.DyeType] = this.Attributes[GameAttribute.DyeType];
            player.Inventory.DestroyInventoryItem(this);
            player.Inventory.SendVisualInventory(player); // TODO: Send it to all who see! /fasbat
        }
    }
}
