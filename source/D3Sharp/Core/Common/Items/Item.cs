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
using System.Collections.Generic;
namespace D3Sharp.Core.Common.Items
{
    

    public enum ItemType
    {
        Helm, ChestArmor, Gloves, Boots, Shoulders, Belt, Pants, Bracers, Shield, Quiver, Orb,
        Axe_1H, Axe_2H, CombatStaff_2H, Dagger, FistWeapon, Mace_1H, Mace_2H, Sword_1H,
        Sword_2H, Bow, Crossbow, Spear, Staff, Polearm, ThrownWeapon, ThrowingAxe, Wand, Ring
    }

    public class Item
    {
        public Dictionary<int, int> Attributes = new Dictionary<int, int>();
        public int Gbid { get; set; }
        public int Count { get; set;  }
        public ItemType ItemType { get; set; }
        public Item(int gbid)
        {
            Gbid = gbid;
            Count = 1;
        }

    }
}
