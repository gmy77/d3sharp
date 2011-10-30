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

using Mooege.Core.GS.Common.Types.Math;
using Mooege.Core.GS.Map;
using Mooege.Net.GS.Message;
using Mooege.Net.GS.Message.Definitions.Trade;
using Mooege.Net.GS.Message.Definitions.World;
using System.Collections.Generic;
using Mooege.Core.Common.Items;

namespace Mooege.Core.GS.Actors.Implementations
{
    [HandledSNO(178396 /* Fence_In_Town_01? */)] //TODO this is just a test, do it properly for all vendors?
    public class Vendor : InteractiveNPC
    {
        private List<Item> _vendorInventory;

        public Vendor(World world, int actorSNO, Vector3D position)
            : base(world, actorSNO, position)
        {
            this.Attributes[GameAttribute.MinimapActive] = true;
            _vendorInventory = new List<Item>();
            LoadItems();
        }

        private void LoadItems()
        {
            var def = new ItemDefinition(189846,"Crafting_Tier_01A", null);
            _vendorInventory.Add(ItemGenerator.CreateItem(this, def));
            _vendorInventory.Add(ItemGenerator.CreateItem(this, def));
            def = new ItemDefinition(189846, "Crafting_Tier_01B", null);
            _vendorInventory.Add(ItemGenerator.CreateItem(this, def));
            _vendorInventory.Add(ItemGenerator.CreateItem(this, def));
            def = new ItemDefinition(189846, "Crafting_Tier_01C", null);
            _vendorInventory.Add(ItemGenerator.CreateItem(this, def));
            _vendorInventory.Add(ItemGenerator.CreateItem(this, def));
            def = new ItemDefinition(189846, "Crafting_Tier_01D", null);
            _vendorInventory.Add(ItemGenerator.CreateItem(this, def));
            _vendorInventory.Add(ItemGenerator.CreateItem(this, def));
            _vendorInventory.Add(ItemGenerator.GenerateRandom(this, ItemType.Sword_1H));
            //Item 
            for (int i = 0; i < _vendorInventory.Count; i++)
            {
                _vendorInventory[i].Owner = this;
                _vendorInventory[i].Field3 = 1; // Holy shit, what is this, forged in gods... 
                _vendorInventory[i].SetInventoryLocation(20, i, 0);
            }
        }

        public override bool Reveal(Player.Player player)
        {
            if (!base.Reveal(player))
                return false;

            RevealVendorInventory(player);
            return true;
        }

        private void RevealVendorInventory(Player.Player player)
        {
            foreach (var item in _vendorInventory)
            {
                item.Reveal(player);
            }
        }

        public override void OnTargeted(Mooege.Core.GS.Player.Player player, TargetMessage message)
        {
            player.InGameClient.SendMessage(new OpenTradeWindowMessage((int)this.DynamicID));
        }
    }
}
