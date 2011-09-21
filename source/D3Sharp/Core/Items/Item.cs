using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace D3Sharp.Core.Items
{

    public enum ItemType
    {
        Helm, ChestArmor, Gloves, Boots, Shoulders, Belt, Pants, Bracers, Shield, Quiver, Orb, 
        Axe_1H, Axe_2H, CombatStaff_2H, Dagger, FistWeapon, Mace_1H, Mace_2H, Sword_1H, 
        Sword_2H, Bow, Crossbow, Spear, Staff, Polearm, ThrownWeapon, ThrowingAxe, Wand, Ring
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
