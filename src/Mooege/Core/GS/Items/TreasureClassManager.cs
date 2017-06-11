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
using Mooege.Common.MPQ;
using Mooege.Core.GS.Common.Types.SNO;
using Mooege.Common.MPQ.FileFormats;
using Mooege.Core.GS.Players;
using Mooege.Net.GS.Message;

namespace Mooege.Core.GS.Items
{

    // This Implementation just supports salavging items.
    public class TreasureClassManager
    {
        public static List<Item> CreateLoot(Player owner, int treasureClassId)
        {

            List<Item> items = new List<Item>();
            if (MPQStorage.Data.Assets[SNOGroup.TreasureClass].ContainsKey(treasureClassId))
            {
                TreasureClass treasureClass = (TreasureClass)MPQStorage.Data.Assets[SNOGroup.TreasureClass][treasureClassId].Data;
                foreach (LootDropModifier modifier in treasureClass.LootDropModifiers)
                {
                    ItemTable definition = ItemGenerator.GetItemDefinition(modifier.ItemSpecifier.ItemGBId);
                    if (definition != null)
                    {
                        Item item = ItemGenerator.CreateItem(owner, definition);
                        item.Attributes[GameAttribute.Item_Quality_Level] = (modifier.GBIdQualityClass > 0) ? modifier.GBIdQualityClass : 0;
                        items.Add(item);
                    }
                }
            }

            return items;
        }
    }
}