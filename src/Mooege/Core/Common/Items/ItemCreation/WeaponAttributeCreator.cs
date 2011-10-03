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

using Mooege.Common.Helpers;
using Mooege.Net.GS.Message.Fields;
using Mooege.Net.GS.Message;

namespace Mooege.Core.Common.Items.ItemCreation
{
    class WeaponAttributeCreator : IItemAttributeCreator
    {
        public void CreateAttributes(Item item)
        {
            item.AttributeList.Add(new NetAttributeKeyValue{ Attribute = GameAttribute.Attributes[0x0041], Int = 0x00000001, }); // Skill
            item.AttributeList.Add(new NetAttributeKeyValue{ Attribute = GameAttribute.Attributes[0x0124], Int = 0x00000001, }); // IdentifyCost

            const float heroAttackspeed = 1.2f; // must be calculated by Skills + passives + affixes  + ...
            const float heroMaxDmg = 50f; // must be calculated by Skills + passives + affixes  + ...
            const float heroMinDmg = 10f; // must be calculated by Skills + passives + affixes  + ...
            const float offhandMultiplier = 0.8f;

            var weaponDmg = (float)(RandomHelper.NextDouble() * 100) + 10;
            var attackspeed = (float)(RandomHelper.NextDouble() + 0.5);
            var minWeaponDmg = weaponDmg - ((float)(RandomHelper.NextDouble() * 20) + 10);

            item.AttributeList.Add(new NetAttributeKeyValue{ Attribute = GameAttribute.Attributes[0x0082], Float = attackspeed,}); // Attacks_Per_Second_Item
            item.AttributeList.Add(new NetAttributeKeyValue{ Attribute = GameAttribute.Attributes[0x0084], Float = attackspeed,}); // Attacks_Per_Second_Item_Subtotal
            item.AttributeList.Add(new NetAttributeKeyValue{ Attribute = GameAttribute.Attributes[0x0086], Float = attackspeed,}); // Attacks_Per_Second_Item_Total
            item.AttributeList.Add(new NetAttributeKeyValue{ Attribute = GameAttribute.Attributes[0x0089], Float = attackspeed * heroAttackspeed,}); // Attacks_Per_Second_Total

            item.AttributeList.Add(new NetAttributeKeyValue{ Attribute = GameAttribute.Attributes[0x0094], Float = 1f, Field0 = 0x00000000,});      //Damage_Weapon_Delta
            item.AttributeList.Add(new NetAttributeKeyValue{ Attribute = GameAttribute.Attributes[0x0095], Float = 1f, Field0 = 0x00000000,});      //Damage_Weapon_Delta_SubTotal
            item.AttributeList.Add(new NetAttributeKeyValue{ Attribute = GameAttribute.Attributes[0x0096], Float = weaponDmg, Field0 =0x00000000,});//Damage_Weapon_Max
            item.AttributeList.Add(new NetAttributeKeyValue{ Attribute = GameAttribute.Attributes[0x0096], Float = weaponDmg, Field0 = 0x000FFFFF,});//Damage_Weapon_Max
            item.AttributeList.Add(new NetAttributeKeyValue{ Attribute = GameAttribute.Attributes[0x0097], Float = weaponDmg + heroMaxDmg, Field0 = 0x00000000,});//Damage_Weapon_Max_Total
            item.AttributeList.Add(new NetAttributeKeyValue{ Attribute = GameAttribute.Attributes[0x0097], Float = weaponDmg + heroMaxDmg, Field0 = 0x000FFFFF,});//Damage_Weapon_Max_Total
            item.AttributeList.Add(new NetAttributeKeyValue{ Attribute = GameAttribute.Attributes[0x0098], Float = 1f, Field0 = 0x00000000,});      //Damage_Weapon_Delta_Total
            item.AttributeList.Add(new NetAttributeKeyValue{ Attribute = GameAttribute.Attributes[0x0099], Float = 1f,});                  //Damage_Weapon_Delta_Total_All
            item.AttributeList.Add(new NetAttributeKeyValue{ Attribute = GameAttribute.Attributes[0x009B], Float = minWeaponDmg, Field0 = 0x00000000,});      //Damage_Weapon_Min
            item.AttributeList.Add(new NetAttributeKeyValue{ Attribute = GameAttribute.Attributes[0x009C], Float = minWeaponDmg + heroMinDmg, Field0 = 0x00000000,});//Damage_Weapon_Min_Total
            item.AttributeList.Add(new NetAttributeKeyValue{ Attribute = GameAttribute.Attributes[0x009C], Float = minWeaponDmg + heroMinDmg, Field0 = 0x000FFFFF,});//Damage_Weapon_Min_Total
            item.AttributeList.Add(new NetAttributeKeyValue{ Attribute = GameAttribute.Attributes[0x009D], Float = 2f,});                  //Damage_Weapon_Min_Total_All

            item.AttributeList.Add(new NetAttributeKeyValue{ Attribute = GameAttribute.Attributes[0x0187], Float = attackspeed,});        // Attacks_Per_Second_Item_MainHand
            item.AttributeList.Add(new NetAttributeKeyValue{ Attribute = GameAttribute.Attributes[0x0188], Float = attackspeed+1,});    // Attacks_Per_Second_Item_OffHand
            item.AttributeList.Add(new NetAttributeKeyValue{ Attribute = GameAttribute.Attributes[0x0189], Float = attackspeed,});        // Attacks_Per_Second_Item_Total_MainHand
            item.AttributeList.Add(new NetAttributeKeyValue{ Attribute = GameAttribute.Attributes[0x018A], Float = attackspeed+1,});    // Attacks_Per_Second_Item_Total_OffHand
            item.AttributeList.Add(new NetAttributeKeyValue{ Attribute = GameAttribute.Attributes[0x018B], Float = (minWeaponDmg + heroMinDmg), Field0 = 0x00000000,});   //Damage_Weapon_Min_Total_MainHand
            item.AttributeList.Add(new NetAttributeKeyValue{ Attribute = GameAttribute.Attributes[0x018B], Float = (minWeaponDmg + heroMinDmg), Field0 = 0x000FFFFF,});   //Damage_Weapon_Min_Total_MainHand
            item.AttributeList.Add(new NetAttributeKeyValue{ Attribute = GameAttribute.Attributes[0x018C], Float = (minWeaponDmg + heroMinDmg) * offhandMultiplier, Field0 = 0x00000000,});   //Damage_Weapon_Min_Total_OffHand
            item.AttributeList.Add(new NetAttributeKeyValue{ Attribute = GameAttribute.Attributes[0x018C], Float = (minWeaponDmg + heroMinDmg) * offhandMultiplier, Field0 = 0x000FFFFF,});   //Damage_Weapon_Min_Total_OffHand
            item.AttributeList.Add(new NetAttributeKeyValue{ Attribute = GameAttribute.Attributes[0x018D], Float = 1f, Field0 = 0x00000000,});              //Damage_Weapon_Delta_Total_MainHand
            item.AttributeList.Add(new NetAttributeKeyValue{ Attribute = GameAttribute.Attributes[0x018E], Float = 3.051758E-05f, Field0 = 0x00000000,});   //Damage_Weapon_Delta_Total_OffHand

            int equipped = 0; // (item.InvLoc.x + item.InvLoc.y == 0) ? 0 : 1;

            item.AttributeList.Add(new NetAttributeKeyValue { Attribute = GameAttribute.Attributes[0x0117], Int = equipped, }); //Item_Equipped

            /* item.AttributeList.Add(new NetAttribute(0x0113, 0x00000000)); //Durability_Max
               item.AttributeList.Add(new NetAttribute(0x0112, 0x00000000)); //Durability_Cur */
        }
    }
}
