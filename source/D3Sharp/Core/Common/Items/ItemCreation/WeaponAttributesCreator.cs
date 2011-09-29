using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using D3Sharp.Core.Common.Items;

namespace D3Sharp.Core.Items.ItemCreation
{
    class WeaponAttributesCreator : IItemAttributesCreator
    {
        public void CreateAttributes(Item item)
        {
            item.AttributeList.Add(new NetAttribute(0x0041, 0x00000001)); // Skill 
            item.AttributeList.Add(new NetAttribute(0x0124, 0x00000001)); // IdentifyCost 




            float heroAttackspeed = 1.2f;   // musst be calculated by Skills + passives + affixes  + ...
            float heroMaxDmg = 50f; // musst be calculated by Skills + passives + affixes  + ...
            float heroMinDmg = 10f; // musst be calculated by Skills + passives + affixes  + ...
            float offHandMultiplikator = 0.8f;

            Random random = new Random();

            float weaponDmg = (float)(random.NextDouble() * 100) + 10;
            float attackspeed = (float)(random.NextDouble() + 0.5);
            float weaponDmg_min = weaponDmg -((float)(random.NextDouble() * 20) + 10);
           

            item.AttributeList.Add(new NetAttribute(0x0082, attackspeed)); // Attacks_Per_Second_Item 
            item.AttributeList.Add(new NetAttribute(0x0084, attackspeed)); // Attacks_Per_Second_Item_Subtotal
            item.AttributeList.Add(new NetAttribute(0x0086, attackspeed)); // Attacks_Per_Second_Item_Total
            item.AttributeList.Add(new NetAttribute(0x0089, attackspeed * heroAttackspeed)); // Attacks_Per_Second_Total              

            item.AttributeList.Add(new NetAttribute(0x0094, 1f, 0x00000000));       //Damage_Weapon_Delta
            item.AttributeList.Add(new NetAttribute(0x0095, 1f, 0x00000000));       //Damage_Weapon_Delta_SubTotal
            item.AttributeList.Add(new NetAttribute(0x0096, weaponDmg, 0x00000000)); //Damage_Weapon_Max
            item.AttributeList.Add(new NetAttribute(0x0096, weaponDmg, 0x000FFFFF)); //Damage_Weapon_Max            
            item.AttributeList.Add(new NetAttribute(0x0097, weaponDmg + heroMaxDmg, 0x00000000)); //Damage_Weapon_Max_Total
            item.AttributeList.Add(new NetAttribute(0x0097, weaponDmg + heroMaxDmg, 0x000FFFFF)); //Damage_Weapon_Max_Total
            item.AttributeList.Add(new NetAttribute(0x0098, 1f, 0x00000000));       //Damage_Weapon_Delta_Total
            item.AttributeList.Add(new NetAttribute(0x0099, 1f));                   //Damage_Weapon_Delta_Total_All            
            item.AttributeList.Add(new NetAttribute(0x009B, weaponDmg_min, 0x00000000));       //Damage_Weapon_Min
            item.AttributeList.Add(new NetAttribute(0x009C, weaponDmg_min + heroMinDmg, 0x00000000)); //Damage_Weapon_Min_Total
            item.AttributeList.Add(new NetAttribute(0x009C, weaponDmg_min + heroMinDmg, 0x000FFFFF)); //Damage_Weapon_Min_Total            
            item.AttributeList.Add(new NetAttribute(0x009D, 2f));                   //Damage_Weapon_Min_Total_All

            item.AttributeList.Add(new NetAttribute(0x0187, attackspeed));        // Attacks_Per_Second_Item_MainHand 
            item.AttributeList.Add(new NetAttribute(0x0188, attackspeed+1));    // Attacks_Per_Second_Item_OffHand 
            item.AttributeList.Add(new NetAttribute(0x0189, attackspeed));        // Attacks_Per_Second_Item_Total_MainHand 
            item.AttributeList.Add(new NetAttribute(0x018A, attackspeed+1));    // Attacks_Per_Second_Item_Total_OffHand            
            item.AttributeList.Add(new NetAttribute(0x018B, (weaponDmg_min + heroMinDmg), 0x00000000));    //Damage_Weapon_Min_Total_MainHand
            item.AttributeList.Add(new NetAttribute(0x018B, (weaponDmg_min + heroMinDmg), 0x000FFFFF));    //Damage_Weapon_Min_Total_MainHand
            item.AttributeList.Add(new NetAttribute(0x018C, (weaponDmg_min + heroMinDmg) * offHandMultiplikator, 0x00000000));    //Damage_Weapon_Min_Total_OffHand
            item.AttributeList.Add(new NetAttribute(0x018C, (weaponDmg_min + heroMinDmg) * offHandMultiplikator, 0x000FFFFF));    //Damage_Weapon_Min_Total_OffHand
            item.AttributeList.Add(new NetAttribute(0x018D, 1f, 0x00000000));               //Damage_Weapon_Delta_Total_MainHand
            item.AttributeList.Add(new NetAttribute(0x018E, 3.051758E-05f, 0x00000000));    //Damage_Weapon_Delta_Total_OffHand


            int equipped = 0;// (item.InvLoc.x + item.InvLoc.y == 0) ? 0 : 1;

            item.AttributeList.Add(new NetAttribute(0x0117, equipped)); //Item_Equipped

            /*
            item.AttributeList.Add(new NetAttribute(0x0113, 0x00000000)); //Durability_Max
            item.AttributeList.Add(new NetAttribute(0x0112, 0x00000000)); //Durability_Cur
            */        
        }
    }
}
