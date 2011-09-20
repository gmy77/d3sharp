using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace D3Sharp.Core.Items
{

    public enum ItemType
    {
        Helm, ChestArmor, Gloves, Boots, Shoulders, Belt
    }

    class Item
    {
       
        public int Gbid { get; set; }

        public ItemType ItemType { get; set; }
       
        public Item(int gbid, ItemType itemType)
        {
            Gbid = gbid;
            ItemType = itemType;
        }

    }
}
