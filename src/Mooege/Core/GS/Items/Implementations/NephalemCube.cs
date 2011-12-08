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
using System;
using System.Collections.Generic;
using System.Text;
using Mooege.Net.GS.Message;
using Mooege.Common.MPQ.FileFormats;
using Mooege.Net.GS.Message.Definitions.Misc;

namespace Mooege.Core.GS.Items.Implementations
{
    [HandledItem("NephalemCube")]
    public class NephalemCube : Item
    {
        public NephalemCube(GS.Map.World world, Mooege.Common.MPQ.FileFormats.ItemTable definition)
            : base(world, definition)
        {
        }

        public override void OnTargeted(Players.Player player, Net.GS.Message.Definitions.World.TargetMessage message)
        {
            player.EnableCubeOfNephalem();
            this.Destroy();
        }

        public static void OnUse(GS.Players.Player player, Item salvageItem)
        {           
            if (salvageItem == null) return;
            player.Inventory.DestroyInventoryItem(salvageItem);

            List<Item> craftingMaterials = TreasureClassManager.CreateLoot(player, salvageItem.ItemDefinition.SNOComponentTreasureClass);
            if (salvageItem.Attributes[GameAttribute.Item_Quality_Level] >= (int)ItemTable.ItemQuality.Magic1)
                craftingMaterials.AddRange(TreasureClassManager.CreateLoot(player, salvageItem.ItemDefinition.SNOComponentTreasureClassMagic));
            if (salvageItem.Attributes[GameAttribute.Item_Quality_Level] >= (int)ItemTable.ItemQuality.Rare4)
                craftingMaterials.AddRange(TreasureClassManager.CreateLoot(player, salvageItem.ItemDefinition.SNOComponentTreasureClassRare));

            List<int> craftigItemsGbids = new List<int>();
            foreach (Item crafingItem in craftingMaterials)
            {
                craftigItemsGbids.Add(crafingItem.GBHandle.GBID);
                // reveal new item to player                  
                player.Inventory.PickUp(crafingItem);
            }

            // TODO: This Message doesn't work. I think i should produce an entry in the chat window like "Salvaging Gloves gave you Common scrap!" - angerwin
            //SalvageResultsMessage message = new SalvageResultsMessage
            //{
            //    gbidNewItems = craftigItemsGbids.ToArray(),
            //    gbidOriginalItem = salvageItem.GBHandle.GBID,
            //    Field1 = 0, // Unkown
            //    Field2 = 0, // Unkown 

            //};
            //player.InGameClient.SendMessage(message);
        }
        
    }
}