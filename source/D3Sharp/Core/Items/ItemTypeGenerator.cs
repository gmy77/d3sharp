using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using D3Sharp.Utils;
using D3Sharp.Utils.Helpers;

namespace D3Sharp.Core.Items
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
                String querypart = String.Format("from items where itemname like '{0}_%'",itemType.ToString());
                String countQuery = String.Format("SELECT count(*) {0}", querypart);
                var cmd = new SQLiteCommand(countQuery, Storage.GameDataDBManager.Connection);
                var reader = cmd.ExecuteReader();
                reader.Read();
                int itemsCount = reader.GetInt32(0);

                // Now select random element 
                Random rand = new Random();
                int selectedElementNr = rand.Next(itemsCount);
                String selectRandom = String.Format("SELECT itemname {0} limit {1},1",querypart, selectedElementNr);
                cmd = new SQLiteCommand(selectRandom, Storage.GameDataDBManager.Connection);
                reader = cmd.ExecuteReader();

                if (!reader.HasRows)
                {
                    throw new Exception("Missing Data in DB");
                }              

                while (reader.Read())
                {
                    var itemName = (String)reader.GetString(0);
                    int id = (int)StringHashHelper.HashString2(itemName);
                    Item item = new Item(id, itemType);
                    return item;
                }

            }catch(Exception e)
            {
                Logger.ErrorException(e, "Error generating Item");
            }
           
            return null;
        }

      
    }

}
