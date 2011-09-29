using System;
using System.Collections.Generic;
using D3Sharp.Net.Game;
using D3Sharp.Core.Helpers;
using D3Sharp.Utils;
using D3Sharp.Utils.Helpers;
using System.Data.SQLite;
using D3Sharp.Net.Game.Message.Definitions.Misc;
using D3Sharp.Net.Game.Message.Fields;
using D3Sharp.Net.Game.Message.Definitions.Animation;
using D3Sharp.Net.Game.Message.Definitions.Player;
using D3Sharp.Net.Game.Message.Definitions.Inventory;
using D3Sharp.Net.Game.Message.Definitions.ACD;
using D3Sharp.Core.Common.Items;

namespace D3Sharp.Core.Common.Items
{

    class ItemGenerator
    {
        public static readonly Logger Logger = LogManager.CreateLogger();

        public Item Potion(GameClient Client,int count=1)
        {
            Dictionary<int, int> Atribiutes = new Dictionary<int, int>();
           
            var Gbid = (int)StringHashHelper.HashItemName("HealthPotion");
            var item = new Item(Gbid);

            item.Count = count;
            item.Attributes.Add(0x0121, 100);

            return item;

        }

        public Item Sword(GameClient Client)
        {
            Dictionary<int, int> Atribiutes = new Dictionary<int, int>();
            try
            {


                String querypart = String.Format("from items where itemname like 'SWORD_%'");
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
                    var Gbid = (int)StringHashHelper.HashItemName(itemName);
                    var item = new Item(Gbid);
                    item.Attributes.Add(274, 20);
                    item.Attributes.Add(275, 50);
                    item.Attributes.Add(155, 5);
                    
                    return item;

                }

            }
            catch (Exception e)
            {
                Logger.ErrorException(e, "Error generating Item");
            }

            return null ;
        }
    }
}
