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

namespace D3Sharp.Core.Common.Items
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
