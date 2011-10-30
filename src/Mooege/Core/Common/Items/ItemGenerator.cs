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
using System.Data.SQLite;
using System.Linq;
using Mooege.Common;
using Mooege.Common.Helpers;
using System.Collections.Generic;
using Mooege.Core.Common.Storage;
using Mooege.Core.GS.Player;
using Mooege.Net.GS;
using Mooege.Core.Common.Items.ItemCreation;
using Wintellect.PowerCollections;
using Mooege.Net.GS.Message;
using Mooege.Common.MPQ.FileFormats;
using Mooege.Common.MPQ;
using Mooege.Core.GS.Common.Types.SNO;

// FIXME: Most of this stuff should be elsewhere and not explicitly generate items to the player's GroundItems collection / komiga?

namespace Mooege.Core.Common.Items
{
    public static class ItemGenerator
    {
        public static readonly Logger Logger = LogManager.CreateLogger();

        private static readonly Dictionary<int, ItemTable> Items = new Dictionary<int, ItemTable>();

        public static int TotalItems
        {
            get { return Items.Count; }
        }

        static ItemGenerator()
        {
            LoadItems();
        }

        private static void LoadItems()
        {
            foreach (var asset in MPQStorage.Data.Assets[SNOGroup.GameBalance].Values)
            {
                GameBalance data = asset.Data as GameBalance;
                if (data != null && data.Type == BalanceType.Items)
                {
                    foreach (var itemdef in data.Item)
                    {
                        Items.Add(itemdef.Hash, itemdef);
                    }
                }
            }
        }

        // generates a random item.
        public static Item GenerateRandom(Player player)
        {
            var itemDefinition = GetRandom(Items.Values.ToList());
            return CreateItem(player, itemDefinition);
        }

        // generates a random item from given type category.
        // we can also set a difficulty mode parameter here, but it seems current db doesnt have nightmare or hell-mode items with valid snoId's /raist.
        public static Item GenerateRandom(Player player, ItemTypeTable type)
        {
            // TODO
            var itemDefinition = GetRandom(Items.Values.ToList());
            return CreateItem(player, itemDefinition);
        }

        private static ItemTable GetRandom(List<ItemTable> pool)
        {
            var found = false;
            ItemTable itemDefinition = null;

            while (!found)
            {
                itemDefinition = pool[RandomHelper.Next(0, pool.Count() - 1)];

                // ignore gold and healthglobe, they should drop only when expect, not randomly
                if (itemDefinition.Name.ToLower().Contains("gold")) continue;
                if (itemDefinition.Name.ToLower().Contains("healthglobe")) continue;
                if (itemDefinition.Name.ToLower().Contains("pvp")) continue;
                if (itemDefinition.Name.ToLower().Contains("unique")) continue;
                if (itemDefinition.Name.ToLower().Contains("crafted")) continue;

                if (itemDefinition.SNOActor == -1) continue;

                found = true;
            }

            return itemDefinition;
        }

        // Creates an item based on supplied definition.
        public static Item CreateItem(Player player, ItemTable definition)
        {
            Logger.Trace("Creating item: {0} [sno:{1}, gbid {2}]", definition.Name,
                         definition.SNOActor, StringHashHelper.HashItemName(definition.Name));

            var item = new Item(player.World, definition);

            return item;
        }

        // Allows cooking a custom item.
        public static Item Cook(Player player, string name)
        {
            int hash = StringHashHelper.HashItemName(name);
            ItemTable definition = Items[hash];
            var item = new Item(player.World, definition);
            //player.GroundItems[item.DynamicID] = item;

            return item;
        }

        public static Item CreateGold(Player player, int amount)
        {
            var item = Cook(player, "Gold1");
            item.Attributes[GameAttribute.Gold] = amount;

            var attributeMap = new GameAttributeMap();
            attributeMap[GameAttribute.Gold] = amount;
            attributeMap.SendMessage(player.InGameClient, item.DynamicID);
            return item;
        }

        public static Item CreateGlobe(Player player, int amount)
        {
            if (amount > 10)
                amount = 10 + ((amount - 10) * 5);

            var item = Cook(player, "HealthGlobe" + amount);
            item.Attributes[GameAttribute.Health_Globe_Bonus_Health] = amount;

            return item;
        }
    }

}

