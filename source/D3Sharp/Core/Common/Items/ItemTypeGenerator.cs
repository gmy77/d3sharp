/*
 * Copyright (C) 2011 D3Sharp Project
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
using D3Sharp.Core.Helpers;
using D3Sharp.Utils;
using D3Sharp.Utils.Helpers;

namespace D3Sharp.Core.Common.Items
{
    class ItemTypeGenerator
    {
        public static readonly Logger Logger = LogManager.CreateLogger();

        public Item generateRandomElement(ItemType itemType)
        {
            try
            {

                // select count of Items with correct Type
                // the itemname structure ITEMTYPE_NUMBER example: BOOTS_001 , BELT_004
                String querypart = String.Format("from items where itemname like '{0}_%'", itemType.ToString());
                String countQuery = String.Format("SELECT count(*) {0}", querypart);
                var cmd = new SQLiteCommand(countQuery, Storage.GameDataDBManager.Connection);
                var reader = cmd.ExecuteReader();
                reader.Read();
                int itemsCount = reader.GetInt32(0);

                // Now select random element 
                int selectedElementNr = RandomHelper.Next(itemsCount);
                String selectRandom = String.Format("SELECT itemname {0} limit {1},1", querypart, selectedElementNr);
                cmd = new SQLiteCommand(selectRandom, Storage.GameDataDBManager.Connection);
                reader = cmd.ExecuteReader();

                if (!reader.HasRows)
                {
                    throw new Exception("Missing Data in DB");
                }

                while (reader.Read())
                {
                    var itemName = (String)reader.GetString(0);
                    var id = (int)StringHashHelper.HashItemName(itemName);
                    var item = new Item(id, itemType);
                    return item;
                }

            }
            catch (Exception e)
            {
                Logger.ErrorException(e, "Error generating Item");
            }

            return null;
        }


    }

}

