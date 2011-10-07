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
using Mooege.Common;
using Mooege.Common.Helpers;
using System.Collections.Generic;
using Mooege.Net.GS;
using Mooege.Core.Common.Items.ItemCreation;

// FIXME: Most of this stuff should be elsewhere and not explicitly generate items to the player's GroundItems collection

namespace Mooege.Core.Common.Items
{
    class ItemTypeGenerator
    {
        public static readonly Logger Logger = LogManager.CreateLogger();

        // This is an incredibly confusing design choice..
        private readonly GameClient _client;

        public ItemTypeGenerator(GameClient client)
        {
            this._client = client;
        }

        public Item GenerateRandomElement(ItemType itemType)
        {
            try
            {
                // select count of Items with correct Type
                // the itemname structure Itemtype_ModeNumber example: BOOTS_001 , BELT_104
                // where mode is 0 = normal , 1 = nightmare, 2 = hell
                // there are missing snoId for nightmare and hell so just use items from normal mode
                const string modeId = "0";

                String querypart = String.Format("from items where itemname like '{0}_{1}%'", itemType.ToString(), modeId);
                String countQuery = String.Format("SELECT count(*) {0}", querypart);
                var cmd = new SQLiteCommand(countQuery, Storage.GameDataDBManager.Connection);
                var reader = cmd.ExecuteReader();
                reader.Read();
                int itemsCount = reader.GetInt32(0);

                if (itemsCount == 0)
                {
                    querypart = String.Format("from items where itemname like '{0}%'", itemType.ToString());
                    countQuery = String.Format("SELECT count(*) {0}", querypart);
                    cmd = new SQLiteCommand(countQuery, Storage.GameDataDBManager.Connection);
                    reader = cmd.ExecuteReader();
                    reader.Read();
                    itemsCount = reader.GetInt32(0);
                }

                // Now select random element
                int selectedElementNr = RandomHelper.Next(itemsCount);
                String selectRandom = String.Format("SELECT itemname, snoId {0} limit {1},1", querypart, selectedElementNr);
                cmd = new SQLiteCommand(selectRandom, Storage.GameDataDBManager.Connection);
                reader = cmd.ExecuteReader();

                if (!reader.HasRows)
                {
                    throw new Exception("Missing Data in DB");
                }

                while (reader.Read())
                {
                    var itemName = (String)reader.GetString(0);
                    var snoId = (int)reader.GetInt32(1);
                    return CreateItem(itemName, snoId, itemType);
                }
            }
            catch (Exception e)
            {
                Logger.ErrorException(e, "Error generating item of type: {0}", itemType.ToString());
            }
            return null;
        }

        public Item CreateItem(String itemName, int actorSNO, ItemType itemType)
        {
            Item item = Generate(itemName, actorSNO, itemType);
            List<IItemAttributeCreator> attributeCreators = new AttributeCreatorFactory().Create(itemType);
            foreach (IItemAttributeCreator creator in attributeCreators)
            {
                creator.CreateAttributes(item);
            }
            return item;
        }

        private Item Generate(String itemName, int actorSNO, ItemType itemType)
        {
            int gbid = StringHashHelper.HashItemName(itemName);
            var item = new Item(_client.Player.World, gbid, itemType) { ActorSNO = actorSNO };
            _client.Player.GroundItems[item.DynamicID] = item;
            return item;
        }
    }
}

