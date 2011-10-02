using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mooege.Net.GS.Message.Fields;
using Mooege.Net.GS.Message;

namespace Mooege.Core.Common.Items.ItemCreation
{
    class WeaponAttributesCreator : IItemAttributesCreator
    {
        public void CreateAttributes(Item item)
        {
           

            item.AttributeList.Add(new NetAttributeKeyValue{ Attribute = GameAttribute.Attributes[0x0041], Int = 0x00000001, }); // Skill 
            item.AttributeList.Add(new NetAttributeKeyValue{ Attribute = GameAttribute.Attributes[0x0124], Int = 0x00000001, }); // IdentifyCost 

        

            float heroAttackspeed = 1.2f;   // musst be calculated by Skills + passives + affixes  + ...
            float heroMaxDmg = 50f; // musst be calculated by Skills + passives + affixes  + ...
            float heroMinDmg = 10f; // musst be calculated by Skills + passives + affixes  + ...
            float offHandMultiplikator = 0.8f;

            Random random = new Random();

            float weaponDmg = (float)(random.NextDouble() * 100) + 10;
            float attackspeed = (float)(random.NextDouble() + 0.5);
            float weaponDmg_min = weaponDmg -((float)(random.NextDouble() * 20) + 10);
           

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
            item.AttributeList.Add(new NetAttributeKeyValue{ Attribute = GameAttribute.Attributes[0x009B], Float = weaponDmg_min, Field0 = 0x00000000,});      //Damage_Weapon_Min
            item.AttributeList.Add(new NetAttributeKeyValue{ Attribute = GameAttribute.Attributes[0x009C], Float = weaponDmg_min + heroMinDmg, Field0 = 0x00000000,});//Damage_Weapon_Min_Total
            item.AttributeList.Add(new NetAttributeKeyValue{ Attribute = GameAttribute.Attributes[0x009C], Float = weaponDmg_min + heroMinDmg, Field0 = 0x000FFFFF,});//Damage_Weapon_Min_Total            
            item.AttributeList.Add(new NetAttributeKeyValue{ Attribute = GameAttribute.Attributes[0x009D], Float = 2f,});                  //Damage_Weapon_Min_Total_All

            item.AttributeList.Add(new NetAttributeKeyValue{ Attribute = GameAttribute.Attributes[0x0187], Float = attackspeed,});        // Attacks_Per_Second_Item_MainHand 
            item.AttributeList.Add(new NetAttributeKeyValue{ Attribute = GameAttribute.Attributes[0x0188], Float = attackspeed+1,});    // Attacks_Per_Second_Item_OffHand 
            item.AttributeList.Add(new NetAttributeKeyValue{ Attribute = GameAttribute.Attributes[0x0189], Float = attackspeed,});        // Attacks_Per_Second_Item_Total_MainHand 
            item.AttributeList.Add(new NetAttributeKeyValue{ Attribute = GameAttribute.Attributes[0x018A], Float = attackspeed+1,});    // Attacks_Per_Second_Item_Total_OffHand            
            item.AttributeList.Add(new NetAttributeKeyValue{ Attribute = GameAttribute.Attributes[0x018B], Float = (weaponDmg_min + heroMinDmg), Field0 = 0x00000000,});   //Damage_Weapon_Min_Total_MainHand
            item.AttributeList.Add(new NetAttributeKeyValue{ Attribute = GameAttribute.Attributes[0x018B], Float = (weaponDmg_min + heroMinDmg), Field0 = 0x000FFFFF,});   //Damage_Weapon_Min_Total_MainHand
            item.AttributeList.Add(new NetAttributeKeyValue{ Attribute = GameAttribute.Attributes[0x018C], Float = (weaponDmg_min + heroMinDmg) * offHandMultiplikator, Field0 = 0x00000000,});   //Damage_Weapon_Min_Total_OffHand
            item.AttributeList.Add(new NetAttributeKeyValue{ Attribute = GameAttribute.Attributes[0x018C], Float = (weaponDmg_min + heroMinDmg) * offHandMultiplikator, Field0 = 0x000FFFFF,});   //Damage_Weapon_Min_Total_OffHand
            item.AttributeList.Add(new NetAttributeKeyValue{ Attribute = GameAttribute.Attributes[0x018D], Float = 1f, Field0 = 0x00000000,});              //Damage_Weapon_Delta_Total_MainHand
            item.AttributeList.Add(new NetAttributeKeyValue{ Attribute = GameAttribute.Attributes[0x018E], Float = 3.051758E-05f, Field0 = 0x00000000,});   //Damage_Weapon_Delta_Total_OffHand


            int equipped = 0;// (item.InvLoc.x + item.InvLoc.y == 0) ? 0 : 1;

            item.AttributeList.Add(new NetAttributeKeyValue { Attribute = GameAttribute.Attributes[0x0117], Int = equipped, }); //Item_Equipped

            /*
            item.AttributeList.Add(new NetAttribute(0x0113, 0x00000000)); //Durability_Max
            item.AttributeList.Add(new NetAttribute(0x0112, 0x00000000)); //Durability_Cur
            */        
        }
    }
}
