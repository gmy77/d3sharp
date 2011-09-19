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

        public Item generate(TYPE itemType)
        {             
          try
            {
                String query = String.Format("SELECT itemname from items where itemname like '{0}_%' limit 1", itemType.ToString());
                var cmd = new SQLiteCommand(query, Storage.GameDataDBManager.Connection);
                var reader = cmd.ExecuteReader();

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
