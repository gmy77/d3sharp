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
using Mooege.Core.GS.Items;
using Mooege.Core.GS.Map;
using Mooege.Core.GS.Players;
using Mooege.Net.GS.Message;
using Mooege.Net.GS.Message.Definitions.Trade;
using Mooege.Net.GS.Message.Definitions.World;
using Mooege.Core.GS.Common;
using Mooege.Core.GS.Common.Types.TagMap;

namespace Mooege.Core.GS.Actors.Implementations
{
    // TODO: this is just a test, do it properly for all vendors?
    [HandledSNO(
        // Miner_InTown + variations
        177320, 178396, 178401, 178403, 229372, 229373, 229374, 229375, 229376,
        // Fence_InTown + variations
        177319, 178388, 178390, 178392, 229367, 229368, 229369, 229370, 229371,
        // Collector_InTown + variations
        107535, 178362, 178383, 178385, 229362, 229363, 229364, 229365, 229366)]
    public class Vendor : InteractiveNPC
    {
        private InventoryGrid _vendorGrid;

        public Vendor(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
            this.Attributes[GameAttribute.MinimapActive] = true;
            _vendorGrid = new InventoryGrid(this, 1, 20, (int)EquipmentSlotId.Vendor);
            PopulateItems();
        }


        // TODO: Proper item loading from droplist?
        protected virtual List<Item> GetVendorItems()
        {
            var list = new List<Item>
            {
                ItemGenerator.GenerateRandom(this),
                ItemGenerator.GenerateRandom(this),
                ItemGenerator.GenerateRandom(this),
                ItemGenerator.GenerateRandom(this),
                ItemGenerator.GenerateRandom(this),
                ItemGenerator.GenerateRandom(this)
            };

            return list;
        }

        private void PopulateItems()
        {
            var items = GetVendorItems();
            if (items.Count > _vendorGrid.Columns)
            {
                _vendorGrid.ResizeGrid(1, items.Count);
            }

            foreach (var item in items)
            {
                _vendorGrid.AddItem(item);
            }

        }

        public override bool Reveal(Player player)
        {
            if (!base.Reveal(player))
                return false;

            _vendorGrid.Reveal(player);
            return true;
        }

        public override bool Unreveal(Player player)
        {
            if (!base.Unreveal(player))
                return false;

            _vendorGrid.Unreveal(player);
            return true;
        }

        public override void OnTargeted(Player player, TargetMessage message)
        {
            base.OnTargeted(player, message);
            player.InGameClient.SendMessage(new OpenTradeWindowMessage((int)this.DynamicID));
        }


        public virtual void OnRequestBuyItem(Players.Player player, uint itemId)
        {
            // TODO: Check gold here
            Item item = _vendorGrid.GetItem(itemId);
            if (item == null)
                return;

            if (!player.Inventory.HasInventorySpace(item))
            {
                return;
            }

            // TODO: Remove the gold
            player.Inventory.BuyItem(item);
        }
    }
}
