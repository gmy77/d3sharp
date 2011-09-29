using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using D3Sharp.Core.Common.Items;
using System.Data.SQLite;
using D3Sharp.Core.Common.Storage;
using D3Sharp.Net.Game;

namespace D3Sharp.Core.Inventory
{
    class InventoryManager
    {
        
        public Dictionary<String, Item> inventory = new Dictionary<String, Item>();

        private static int INV_MAX_X = 9;
        private static int INV_MAX_Y = 5;
        private int playerId;
        
        public InventoryManager(int playerId)
        {          
            this.playerId = playerId;      
        }        

        public ICollection<Item> GetInventoryItems()
        {
            return inventory.Values;
        }

        public void AddToInventory(Item item)
        {
               
            // TODO: handle items with two spaces            
            for (int iY = 0; iY < INV_MAX_Y; iY++)
            {
                for (int iX = 0; iX < INV_MAX_X; iX++)
                {
                    String coords = iX + ":" + iY;
                    if (!inventory.ContainsKey(coords))
                    {
                        inventory[coords] = item;
                        item.PlayerId = this.playerId;
                        item.InvLoc.x = iX;
                        item.InvLoc.y = iY;
                        return;
                    }
                }
            }
            throw new Exception("No free Space in Inventory");
        }

        internal Item GetItem(int itemId)
        {
            foreach (Item item in GetInventoryItems())
            {
                if (item.ItemId.Equals(itemId))
                {
                    return item;
                }
            }
            throw new Exception("Requested item is not in th inventory");
        }
    }
}
