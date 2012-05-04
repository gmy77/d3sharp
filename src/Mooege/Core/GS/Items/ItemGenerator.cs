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

using System;
using System.Data.SQLite;
using System.Linq;
using System.Collections.Generic;
using Mooege.Common.Helpers.Hash;
using Mooege.Common.Helpers.Math;
using Mooege.Common.Logging;
using Mooege.Common.Storage;
using Mooege.Core.GS.Players;
using Mooege.Net.GS.Message;
using Mooege.Common.MPQ.FileFormats;
using Mooege.Common.MPQ;
using Mooege.Core.GS.Common.Types.SNO;
using System.Reflection;
using World = Mooege.Core.GS.Map.World;

// FIXME: Most of this stuff should be elsewhere and not explicitly generate items to the player's GroundItems collection / komiga?

namespace Mooege.Core.GS.Items
{
    public static class ItemGenerator
    {
        public static readonly Logger Logger = LogManager.CreateLogger();

        private static readonly Dictionary<int, ItemTable> Items = new Dictionary<int, ItemTable>();
        private static readonly Dictionary<int, Type> GBIDHandlers = new Dictionary<int, Type>();
        private static readonly Dictionary<int, Type> TypeHandlers = new Dictionary<int, Type>();
        private static readonly HashSet<int> AllowedItemTypes = new HashSet<int>();
        
        //private static readonly Dictionary<Player, List<Item>> DbItems = new Dictionary<Player, List<Item>>(); //we need this list to delete item_instances from items which have no owner anymore.
        //private static readonly Dictionary<int, Item> CachedItems = new Dictionary<int, Item>();



        public static int TotalItems
        {
            get { return Items.Count; }
        }

        static ItemGenerator()
        {
            LoadItems();
            LoadHandlers();
            SetAllowedTypes();
        }

        private static void LoadHandlers()
        {
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (!type.IsSubclassOf(typeof(Item))) continue;

                var attributes = (HandledItemAttribute[])type.GetCustomAttributes(typeof(HandledItemAttribute), true);
                if (attributes.Length != 0)
                {
                    foreach (var name in attributes.First().Names)
                    {
                        GBIDHandlers.Add(StringHashHelper.HashItemName(name), type);
                    }
                }

                var typeAttributes = (HandledTypeAttribute[])type.GetCustomAttributes(typeof(HandledTypeAttribute), true);
                if (typeAttributes.Length != 0)
                {
                    foreach (var typeName in typeAttributes.First().Types)
                    {
                        TypeHandlers.Add(StringHashHelper.HashItemName(typeName), type);
                    }
                }
            }
        }

        private static void LoadItems()
        {
            foreach (var asset in MPQStorage.Data.Assets[SNOGroup.GameBalance].Values)
            {
                GameBalance data = asset.Data as GameBalance;
                if (data != null && data.Type == BalanceType.Items)
                {
                    foreach (var itemDefinition in data.Item)
                    {
                        Items.Add(itemDefinition.Hash, itemDefinition);
                    }
                }
            }
        }

        private static void SetAllowedTypes()
        {
            foreach (int hash in ItemGroup.SubTypesToHashList("Weapon"))
                AllowedItemTypes.Add(hash);
            foreach (int hash in ItemGroup.SubTypesToHashList("Armor"))
                AllowedItemTypes.Add(hash);
            foreach (int hash in ItemGroup.SubTypesToHashList("Offhand"))
                AllowedItemTypes.Add(hash);
            foreach (int hash in ItemGroup.SubTypesToHashList("Jewelry"))
                AllowedItemTypes.Add(hash);
            foreach (int hash in ItemGroup.SubTypesToHashList("Utility"))
                AllowedItemTypes.Add(hash);
            foreach (int hash in ItemGroup.SubTypesToHashList("CraftingPlan"))
                AllowedItemTypes.Add(hash);
            foreach (int hash in TypeHandlers.Keys)
            {
                if (AllowedItemTypes.Contains(hash))
                {
                    // already added structure
                    continue;
                }
                foreach (int subhash in ItemGroup.SubTypesToHashList(ItemGroup.FromHash(hash).Name))
                {
                    if (AllowedItemTypes.Contains(subhash))
                    {
                        // already added structure
                        continue;
                    }
                    AllowedItemTypes.Add(subhash);
                }
            }

        }

        // generates a random item.
        public static Item GenerateRandom(Mooege.Core.GS.Actors.Actor owner)
        {
            var itemDefinition = GetRandom(Items.Values.ToList());
            return CreateItem(owner, itemDefinition);
        }

        // generates a random item from given type category.
        // we can also set a difficulty mode parameter here, but it seems current db doesnt have nightmare or hell-mode items with valid snoId's /raist.
        public static Item GenerateRandom(Mooege.Core.GS.Actors.Actor player, ItemTypeTable type)
        {
            var itemDefinition = GetRandom(Items.Values
                .Where(def => ItemGroup
                    .HierarchyToHashList(ItemGroup.FromHash(def.ItemType1)).Contains(type.Hash)).ToList());
            return CreateItem(player, itemDefinition);
        }

        private static ItemTable GetRandom(List<ItemTable> pool)
        {
            var found = false;
            ItemTable itemDefinition = null;

            while (!found)
            {
                itemDefinition = pool[RandomHelper.Next(0, pool.Count() - 1)];

                if (itemDefinition.SNOActor == -1) continue;

                // if ((itemDefinition.ItemType1 == StringHashHelper.HashItemName("Book")) && (itemDefinition.BaseGoldValue != 0)) return itemDefinition; // testing books /xsochor
                // if (itemDefinition.ItemType1 != StringHashHelper.HashItemName("Book")) continue; // testing books /xsochor
                // if (!ItemGroup.SubTypesToHashList("SpellRune").Contains(itemDefinition.ItemType1)) continue; // testing spellrunes /xsochor

                // ignore gold and healthglobe, they should drop only when expect, not randomly
                if (itemDefinition.Name.ToLower().Contains("gold")) continue;
                if (itemDefinition.Name.ToLower().Contains("healthglobe")) continue;
                if (itemDefinition.Name.ToLower().Contains("pvp")) continue;
                if (itemDefinition.Name.ToLower().Contains("unique")) continue;
                if (itemDefinition.Name.ToLower().Contains("crafted")) continue;
                if (itemDefinition.Name.ToLower().Contains("debug")) continue;
                if (itemDefinition.Name.ToLower().Contains("missing")) continue; //I believe I've seen a missing item before, may have been affix though. //Wetwlly
                if ((itemDefinition.ItemType1 == StringHashHelper.HashItemName("Book")) && (itemDefinition.BaseGoldValue == 0)) continue; // i hope it catches all lore with npc spawned /xsochor

                if (!GBIDHandlers.ContainsKey(itemDefinition.Hash) &&
                    !AllowedItemTypes.Contains(itemDefinition.ItemType1)) continue;

                found = true;
            }

            return itemDefinition;
        }

        public static Type GetItemClass(ItemTable definition)
        {
            Type type = typeof(Item);

            if (GBIDHandlers.ContainsKey(definition.Hash))
            {
                type = GBIDHandlers[definition.Hash];
            }
            else
            {
                foreach (var hash in ItemGroup.HierarchyToHashList(ItemGroup.FromHash(definition.ItemType1)))
                {
                    if (TypeHandlers.ContainsKey(hash))
                    {
                        type = TypeHandlers[hash];
                        break;
                    }
                }
            }

            return type;
        }

        public static Item CloneItem(Item originalItem)
        {
            var clonedItem = CreateItem(originalItem.Owner, originalItem.ItemDefinition);
            AffixGenerator.CloneIntoItem(originalItem, clonedItem);
            return clonedItem;
        }

        // Creates an item based on supplied definition.
        public static Item CreateItem(Mooege.Core.GS.Actors.Actor owner, ItemTable definition)
        {
            // Logger.Trace("Creating item: {0} [sno:{1}, gbid {2}]", definition.Name, definition.SNOActor, StringHashHelper.HashItemName(definition.Name));

            Type type = GetItemClass(definition);

            var item = (Item)Activator.CreateInstance(type, new object[] { owner.World, definition });

            return item;
        }

        // Allows cooking a custom item.
        public static Item Cook(Player player, string name)
        {
            int hash = StringHashHelper.HashItemName(name);
            ItemTable definition = Items[hash];
            return CookFromDefinition(player, definition);
        }

        // Allows cooking a custom item.
        public static Item CookFromDefinition(Player player, ItemTable definition)
        {
            Type type = GetItemClass(definition);

            var item = (Item)Activator.CreateInstance(type, new object[] { player.World, definition });
            //player.GroundItems[item.DynamicID] = item;

            return item;
        }

        public static ItemTable GetItemDefinition(int gbid)
        {
            return (Items.ContainsKey(gbid)) ? Items[gbid] : null;
        }

        public static Item CreateGold(Player player, int amount)
        {
            var item = Cook(player, "Gold1");
            item.Attributes[GameAttribute.Gold] = amount;

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

        public static bool IsValidItem(string name)
        {
            return Items.ContainsKey(StringHashHelper.HashItemName(name));
        }

        public static void SaveToDB(Item item)
        {
            var timestart = DateTime.Now;


            if (item.DBId < 0)
            {
                //item not in db, creating new
                var affixSer = SerializeAffixList(item.AffixList);
                var attributesSer = item.Attributes.Serialize();

                var cmd = DBManager.Connection.CreateCommand();
                cmd.CommandText = string.Format("INSERT INTO item_entities (item_gbid,item_attributes,item_affixes) VALUES ({0},@item_attributes,@item_affixes);select last_insert_rowid(); ", item.GBHandle.GBID);
                cmd.Parameters.Add(new SQLiteParameter("@item_attributes", attributesSer));
                cmd.Parameters.Add(new SQLiteParameter("@item_affixes", affixSer));
                var insertID = cmd.ExecuteScalar();
                item.DBId = Convert.ToInt32(insertID);
            }
            else
            {
                //item in db, updating

                if (
                    serAffixesHashes.ContainsKey(item.DBId) && serAffixesHashes[item.DBId] == item.AffixList.GetHashCode() &&
                    serAttributesHashes.ContainsKey(item.DBId) && serAttributesHashes[item.DBId] == item.Attributes.GetHashCode()
                    )
                {
                    Logger.Debug("Item not changed. skipping db-update.");
                }
                else
                {
                    var affixSer = SerializeAffixList(item.AffixList);
                    var attributesSer = item.Attributes.Serialize();

                    var cmd = DBManager.Connection.CreateCommand();
                    cmd.CommandText = string.Format("UPDATE item_entities SET item_gbid={0},item_attributes=@item_attributes,item_affixes=@item_affixes WHERE id={1}", item.GBHandle.GBID, item.DBId);
                    cmd.Parameters.Add(new SQLiteParameter("@item_attributes", attributesSer));
                    cmd.Parameters.Add(new SQLiteParameter("@item_affixes", affixSer));
                    cmd.ExecuteNonQuery();
                    if (item.World.CachedItems.ContainsKey(item.DBId))
                        item.World.CachedItems.Remove(item.DBId);//clearing cache to reRead item from database to find errors earlier.
                }
            }


            var timeTaken = DateTime.Now - timestart;
            Logger.Debug("Save item instance #{0}, took {1} msec", item.DBId, timeTaken.TotalMilliseconds);
            
        }


        public static void DeleteFromDB(Item item)
        {
            if (item.DBId < 0)
                return;
            Logger.Debug("Deleting Item instance #{0} from DB", item.DBId);
            if (item.World.CachedItems.ContainsKey(item.DBId))
                item.World.CachedItems.Remove(item.DBId);
            var deletequery = string.Format("DELETE FROM item_entities WHERE id={0}", item.DBId);
            var cmd = new SQLiteCommand(deletequery, DBManager.Connection);
            cmd.ExecuteNonQuery();
            item.DBId = -1;
        }

        private static Dictionary<int, int> serAffixesHashes = new Dictionary<int, int>();
        private static Dictionary<int, int> serAttributesHashes = new Dictionary<int, int>();
        public static Item LoadFromDB(Player owner, int dbID)
        {

            if (owner.World.CachedItems.ContainsKey(dbID))
            {
                Logger.Debug("Getting item instance #{0} from Cache", dbID);
                return owner.World.CachedItems[dbID];
            }
            var timestart = DateTime.Now;

            var query = string.Format("SELECT * FROM item_entities WHERE id={0}", dbID);
            var cmd = new SQLiteCommand(query, DBManager.Connection);
            var reader = cmd.ExecuteReader();

            if (!reader.HasRows)
                return null;

            reader.Read();

            var gbid = Convert.ToInt32(reader["item_gbid"]);
            var attributesSer = (string)reader["item_attributes"];
            var affixesSer = (string)reader["item_affixes"];

            var itm = LoadFromValues(owner, dbID, gbid, attributesSer, affixesSer);

            var timeTaken = DateTime.Now - timestart;
            Logger.Debug("Loaded item_instance #{0} from Database, took {1} msec", dbID, timeTaken.TotalMilliseconds);
            return itm;
        }
        public static Item LoadFromValues(Player owner, int dbID, int gbid, string attributesSer, string affixesSer)
        {
            var table = Items[gbid];
            var itm = new Item(owner.World, table, DeSerializeAffixList(affixesSer), attributesSer);
            itm.DBId = dbID;
            if (!owner.World.DbItems.ContainsKey(owner.World))
                owner.World.DbItems.Add(owner.World, new List<Item>());
            if (!owner.World.DbItems[owner.World].Contains(itm))
                owner.World.DbItems[owner.World].Add(itm);

            owner.World.CachedItems[itm.DBId] = itm;
            serAffixesHashes[dbID] = itm.AffixList.GetHashCode();
            serAttributesHashes[dbID] = itm.Attributes.GetHashCode();
            return itm;
        }

        public static Item[] LoadFromDB(Player owner, int[] dbIDs)
        {
            var timestart = DateTime.Now;
            var results = new List<Item>();
            results.AddRange(owner.World.CachedItems.Where(citm => dbIDs.Contains(citm.Key)).Select(citm => citm.Value));



            var idsToSelect = dbIDs.Where(id => !results.Any(r => r.DBId == id));
            var idsToSelectInQuery = idsToSelect.Aggregate("", (current, id) => current + ((current.Length > 0 ? "," : "") + id));

            var query = string.Format("SELECT * FROM item_entities WHERE id IN ({0})", idsToSelectInQuery);
            var cmd = new SQLiteCommand(query, DBManager.Connection);

            var reader = cmd.ExecuteReader();

            if (!reader.HasRows)
                return results.ToArray();

            while (reader.Read())
            {
                var gbid = Convert.ToInt32(reader["item_gbid"]);
                var attributesSer = (string)reader["item_attributes"];
                var affixesSer = (string)reader["item_affixes"];


                var dbId = Convert.ToInt32(reader["id"]); ;
                var itm = LoadFromValues(owner, dbId, gbid, attributesSer, affixesSer);
                results.Add(itm);
            }

            var timeTaken = DateTime.Now - timestart;
            Logger.Debug("Loaded {0} item_instances from Database, took {1} msec", results.Count, timeTaken.TotalMilliseconds);
            return results.ToArray();
        }

        public static string SerializeAffixList(List<Affix> affixList)
        {
            var affixgbIdList = affixList.Select(af => af.AffixGbid);
            var affixSer = affixgbIdList.Aggregate(",", (current, affixId) => current + (affixId + ",")).Trim(new[] { ',' });
            return affixSer;
        }

        public static List<Affix> DeSerializeAffixList(string serializedAffixList)
        {
            var affixListStr = serializedAffixList.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            var affixList = affixListStr.Select(int.Parse).Select(affixId => new Affix(affixId)).ToList();
            return affixList;
        }
    }

}

