using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using D3Sharp.Utils.Helpers;
using D3Sharp.Core.Common.Items;

namespace D3Sharp.Core.Items
{
    class ItemGenerator
    {

        private static List<int> usedItemIds = new List<int>();
        private static Random idGenerator = new Random();

        private List<int> vaildItemIds = new List<int>();        
           
        
        public ItemGenerator(){

            // couldn't figure out how to create random ids which are valid on clientside 
            // workaraound: use this list           
            vaildItemIds.Add(0x789F00E1);            
            vaildItemIds.Add(0x78A000E5);
            vaildItemIds.Add(0x78A000E6);
            vaildItemIds.Add(0x78A000E7);
            vaildItemIds.Add(0x78A000E8);
            vaildItemIds.Add(0x78A000E9);
            vaildItemIds.Add(0x78A000EA);
            vaildItemIds.Add(0x78A000EB);
            vaildItemIds.Add(0x78A000EC);
            vaildItemIds.Add(0x78A000ED);
            vaildItemIds.Add(0x78A000EE);
            vaildItemIds.Add(0x78A000EF); 
        }

        public Item Generate(String itemName, int itemId, ItemType itemType)
        {                  
            uint gbid = StringHashHelper.HashItemName(itemName);
            Item item = new Item(itemId, gbid, itemType);

            return item;
        }

        public Item Generate(String itemName, ItemType itemType){
           
            int itemId = CreateUniqueItemId();
            uint gbid = StringHashHelper.HashItemName(itemName);
            Item item = new Item(itemId, gbid, itemType);            
            return item;
        }





        private int CreateUniqueItemId(){

            // random number does not work as item id
            //int itemId = idGenerator.Next(0x78A00000, 0x78A000FF);            

            int index = 0;
            int itemId = vaildItemIds[index];
            while ( usedItemIds.Contains(itemId)){
                itemId = vaildItemIds[index++];
            }
            usedItemIds.Add(itemId);
            return itemId;
        }


    }
}
