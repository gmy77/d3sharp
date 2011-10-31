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
using Mooege.Core.GS.Players;
using Mooege.Core.Common.Items.ItemCreation;
using Wintellect.PowerCollections;
using Mooege.Net.GS.Message;

// FIXME: Most of this stuff should be elsewhere and not explicitly generate items to the player's GroundItems collection / komiga?

namespace Mooege.Core.Common.Items
{
    public static class ItemGenerator
    {
        public static readonly Logger Logger = LogManager.CreateLogger();

        // All item definitions from db.
        private static readonly MultiDictionary<ItemType, ItemDefinition> ItemDefinitions =
            new MultiDictionary<ItemType, ItemDefinition>(true);

        // List of all valid item definitions (which has snoId!=0).
        private static readonly List<ItemDefinition> ValidDefinitions;

        public static int TotalItems
        {
            get { return ItemDefinitions.Keys.ToList().Sum(key => ItemDefinitions[key].Count); }
        }

        static ItemGenerator()
        {
            LoadItems();
            ValidDefinitions = ItemDefinitions.Values.Where(definition => definition.SNOId != 0).ToList();
        }

        private static void LoadItems()
        {
            const string query = "SELECT snoId, itemname, itemdescription from items";
            var cmd = new SQLiteCommand(query, GameDataDBManager.Connection);
            var reader = cmd.ExecuteReader();

            if (!reader.HasRows) return;

            while (reader.Read())
            {
                var itemDefinition = new ItemDefinition(reader.GetInt32(0), reader.GetString(1),reader.IsDBNull(2) ? string.Empty : reader.GetString(2));
                ItemDefinitions.Add(itemDefinition.Type, itemDefinition);
            }            
        }

        // generates a random item.
        public static Item GenerateRandom(Player player)
        {
            var itemDefinition = GetRandom(ValidDefinitions);
            return CreateItem(player, itemDefinition);
        }

        // generates a random item from given type category.
        // we can also set a difficulty mode parameter here, but it seems current db doesnt have nightmare or hell-mode items with valid snoId's /raist.
        public static Item GenerateRandom(Player player, ItemType type)
        {
            var validDefinitions = ItemDefinitions[type].Where(definition => definition.SNOId != 0).ToList(); // only find item definitions with snoId!=0 for given itemtype.
            var itemDefinition = GetRandom(validDefinitions);

            return CreateItem(player, itemDefinition);
        }

        private static ItemDefinition GetRandom(List<ItemDefinition> pool)
        {
            var found = false;
            ItemDefinition itemDefinition = null;

            while (!found)
            {
                itemDefinition = pool[RandomHelper.Next(0, pool.Count() - 1)];

                // ignore gold and healthglobe, they should drop only when expect, not randomly
                if (itemDefinition.Type == ItemType.Gold) continue;
                if (itemDefinition.Type == ItemType.HealthGlobe) continue;
                // ignore items that mostly produce bad gbid's which crashes client on pickup eventually. 
                if (itemDefinition.Type == ItemType.Unknown) continue;
                if (itemDefinition.Name.ToLower().Contains("pvp")) continue;
                if (itemDefinition.Name.ToLower().Contains("unique")) continue;
                if (itemDefinition.Name.ToLower().Contains("crafted")) continue;

                found = true;
            }

            return itemDefinition;
        }

        // Creates an item based on supplied definition.
        public static Item CreateItem(Player player, ItemDefinition definition)
        {
            //Logger.Trace("Creating item: {0} [type: {1}, mode: {2}, sno:{3}, gbid {4}]", definition.Name, definition.Type, definition.DifficultyMode, definition.SNOId, definition.GBId);

            var item = new Item(player.World, definition.SNOId, definition.GBId, definition.Type);

            var attributeCreators = new AttributeCreatorFactory().Create(definition.Type);
            foreach (IItemAttributeCreator creator in attributeCreators)
            {
                creator.CreateAttributes(item);
            }

            return item;
        }

        // Allows cooking a custom item.
        public static Item Cook(Player player, string name, int snoId, ItemType type)
        {
            var item = new Item(player.World, snoId, StringHashHelper.HashItemName(name), type);
            //player.GroundItems[item.DynamicID] = item;

            var attributeCreators = new AttributeCreatorFactory().Create(type);
            foreach (IItemAttributeCreator creator in attributeCreators)
            {
                creator.CreateAttributes(item);
            }

            return item;
        }

        public static Item CreateGold(Player player, int amount)
        {
            var item = Cook(player, "Gold1", 0x00000178, ItemType.Gold);
            item.Attributes[GameAttribute.Gold] = amount;

            var attributeMap = new GameAttributeMap();
            attributeMap[GameAttribute.Gold] = amount;
            attributeMap.SendMessage(player.InGameClient, item.DynamicID);
            return item;
        }

        public static Item CreateGlobe(Player player, int amount)
        {
            int snoid;

            if (amount > 10)
                amount = 10 + ((amount - 10) * 5);

            if (amount < 50)
                snoid = 4267;
            else
                snoid = 85798;

            var item = Cook(player, "HealthGlobe" + amount, snoid, ItemType.HealthGlobe);
            item.Attributes[GameAttribute.Health_Globe_Bonus_Health] = amount;

            return item;
        }
    }

    public class ItemDefinition
    {
        public ItemType Type { get; private set; }
        public int SNOId { get; private set; }
        public int GBId { get; private set; } // not sure on if this should be actually uint /raist
        public string Name { get; private set; }
        public string Description { get; private set; }
        public ItemDifficultyMode DifficultyMode { get; private set; }

        public ItemDefinition(int snoId, string name, string description)
        {
            this.SNOId = snoId;
            this.Name = name;
            this.GBId = StringHashHelper.HashItemName(name); // FIXME: Our item name hasher seems to be problematic, for some items even with valid snoId's, bad gbId's are hashed which crashes client on pickup. /raist.
            this.Description = description;
            this.Type = this.GetTypeFromName(name);
            this.DifficultyMode = this.GetDifficultyModeFromName(name);
        }

        private ItemType GetTypeFromName(string name)
        {
            return Enum.GetValues(typeof(ItemType)).Cast<object>().Where(type => name.Contains(type.ToString())).Cast<ItemType>().FirstOrDefault();
        }

        private ItemDifficultyMode GetDifficultyModeFromName(string name)
        {
            if (name.IndexOf('_') == -1) return ItemDifficultyMode.Normal;

            int modeIndex;
            return Int32.TryParse(name.Substring(name.IndexOf('_') + 1, 1), out modeIndex)
                       ? (ItemDifficultyMode)modeIndex
                       : ItemDifficultyMode.Normal;
        }
    }

    public enum ItemDifficultyMode
    {
        Normal,
        Nightmare,
        Hell
    }
}

