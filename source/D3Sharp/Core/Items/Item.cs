using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace D3Sharp.Core.Items
{

    public enum TYPE
    {
        Helm, ChestArmor, Gloves, Boots, Shoulders, Belt
    }

    class Item
    {
       
        public int Gbid { get; set; }

        public TYPE ItemType { get; set; }
       
        public Item(int gbid, TYPE itemType)
        {
            Gbid = gbid;
            ItemType = itemType;
        }

    }
}
