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
using Mooege.Common.MPQ.FileFormats.Types;
using Mooege.Core.GS.Common.Types.Math;
using Mooege.Core.GS.Map;
using Mooege.Net.GS.Message;

namespace Mooege.Core.GS.Actors.Implementations.Hirelings
{
    [HandledSNO(4538 /* Templar.acr */)]
    public class Templar : Hireling
    {
        public Templar(World world, int snoId, Dictionary<int, TagMapEntry> tags)
            : base(world, snoId, tags)
        {
            //enable this for some spectacular crashes /fasbat
            //hirelingSNO = 0x0000CDD5;  
            this.Attributes[GameAttribute.Hireling_Class] = 1;

            #region test attribs

            this.Attributes[GameAttribute.Buff_Active, 0x20c51] = true;
            this.Attributes[GameAttribute.SkillKit] = 0x8AFB;
            this.Attributes[GameAttribute.Skill_Total, 0x76B7] = 1;
            this.Attributes[GameAttribute.Skill, 0x76B7] = 1;
            this.Attributes[GameAttribute.Skill_Total, 0x7780] = 1;
            this.Attributes[GameAttribute.Skill, 0x7780] = 1;
            //

            this.Attributes[GameAttribute.Get_Hit_Damage] = 20;
            this.Attributes[GameAttribute.Get_Hit_Recovery] = 3.051758E-05f;
            this.Attributes[GameAttribute.Get_Hit_Max] = 3.051758E-05f;
            this.Attributes[GameAttribute.Dodge_Rating_Total] = 3.051758E-05f;
            this.Attributes[GameAttribute.Callout_Cooldown, 0x1618a] = 743;
            this.Attributes[GameAttribute.Block_Amount_Item_Delta] = 4;
            this.Attributes[GameAttribute.Buff_Visual_Effect, 0x000FFFFF] = true;
            this.Attributes[GameAttribute.Block_Amount_Item_Min] = 6;
            this.Attributes[GameAttribute.Block_Amount_Total_Max] = 10;

            this.Attributes[GameAttribute.Block_Amount_Total_Min] = 6;
            this.Attributes[GameAttribute.Block_Chance_Item_Total] = 0.1099854f;
            this.Attributes[GameAttribute.Block_Chance_Item] = 3.051758E-05f;
            this.Attributes[GameAttribute.Block_Chance_Total] = 0.1099854f;
            this.Attributes[GameAttribute.Conversation_Icon, 0] = 0;
            this.Attributes[GameAttribute.Resource_Cur, 0] = 0x3f800000;
            this.Attributes[GameAttribute.Resource_Max, 0] = 1;
            this.Attributes[GameAttribute.Resource_Max_Total, 0] = 0x3F800000;
            this.Attributes[GameAttribute.Damage_Weapon_Min_Total_All] = 6;
            this.Attributes[GameAttribute.Damage_Weapon_Delta_Total_All] = 2;
            this.Attributes[GameAttribute.Resource_Regen_Total, 0] = 3.051758E-05f;
            this.Attributes[GameAttribute.Resource_Effective_Max, 0] = 1;
            this.Attributes[GameAttribute.Attacks_Per_Second_Item_CurrentHand] = 1.199219f;
            this.Attributes[GameAttribute.Attacks_Per_Second_Item_Total_MainHand] = 1.199219f;

            this.Attributes[GameAttribute.Attacks_Per_Second_Total] = 1.199219f;
            this.Attributes[GameAttribute.Attacks_Per_Second_Item_MainHand] = 1.199219f;
            this.Attributes[GameAttribute.Attacks_Per_Second] = 1;
            this.Attributes[GameAttribute.Attacks_Per_Second_Item_Total] = 1.199219f;
            this.Attributes[GameAttribute.Attacks_Per_Second_Item_Subtotal] = 3.051758E-05f;
            this.Attributes[GameAttribute.Attacks_Per_Second_Item] = 3.051758E-05f;
            this.Attributes[GameAttribute.Hit_Chance] = 1;
            this.Attributes[GameAttribute.Casting_Speed_Total] = 1;
            this.Attributes[GameAttribute.Casting_Speed] = 1;
            this.Attributes[GameAttribute.Movement_Scalar_Total] = 1;
            this.Attributes[GameAttribute.Movement_Scalar_Capped_Total] = 1;
            this.Attributes[GameAttribute.Movement_Scalar_Subtotal] = 1;
            this.Attributes[GameAttribute.Strafing_Rate_Total] = 0.1799316f;

            this.Attributes[GameAttribute.Sprinting_Rate_Total] = 0.3598633f;
            this.Attributes[GameAttribute.Damage_Weapon_Min_Total_MainHand, 0] = 6;
            this.Attributes[GameAttribute.Running_Rate_Total] = 0.3598633f;
            this.Attributes[GameAttribute.Walking_Rate_Total] = 0.3598633f;
            this.Attributes[GameAttribute.Damage_Weapon_Delta_Total_MainHand, 0] = 2;
            this.Attributes[GameAttribute.Strafing_Rate] = 0.1799316f;
            this.Attributes[GameAttribute.Damage_Delta_Total, 0] = 2;
            this.Attributes[GameAttribute.Sprinting_Rate] = 0.3598633f;
            this.Attributes[GameAttribute.Damage_Min, 0] = 0.8115234f;
            this.Attributes[GameAttribute.Running_Rate] = 0.3598633f;
            this.Attributes[GameAttribute.Damage_Weapon_Min_Total_CurrentHand, 0] = 6;
            this.Attributes[GameAttribute.Walking_Rate] = 0.3598633f;
            this.Attributes[GameAttribute.Damage_Weapon_Delta_Total_CurrentHand, 0] = 2;
            this.Attributes[GameAttribute.Damage_Min_Total, 0] = 6.808594f;
            this.Attributes[GameAttribute.Movement_Scalar] = 1;

            this.Attributes[GameAttribute.Damage_Min_Subtotal, 0] = 6.808594f;
            this.Attributes[GameAttribute.Damage_Weapon_Delta, 0] = 2;
            this.Attributes[GameAttribute.Damage_Weapon_Delta_SubTotal, 0] = 2;
            this.Attributes[GameAttribute.Damage_Weapon_Max, 0] = 8;
            this.Attributes[GameAttribute.Damage_Weapon_Max_Total, 0] = 8;
            this.Attributes[GameAttribute.Damage_Weapon_Delta_Total, 0] = 2;
            this.Attributes[GameAttribute.Damage_Weapon_Min, 0] = 6;
            this.Attributes[GameAttribute.Damage_Weapon_Min_Total, 0] = 6;
            this.Attributes[GameAttribute.Resource_Type_Primary] = 0;
            this.Attributes[GameAttribute.Callout_Cooldown, 0x000FFFFF] = 0x00000797;
            //
            this.Attributes[GameAttribute.Hitpoints_Max_Total] = 308.25f;
            this.Attributes[GameAttribute.Hitpoints_Max] = 216.25f;
            //

            this.Attributes[GameAttribute.Hitpoints_Total_From_Level] = 3.051758E-05f;
            this.Attributes[GameAttribute.Hitpoints_Total_From_Vitality] = 92;
            //
            this.Attributes[GameAttribute.Hitpoints_Factor_Vitality] = 4;
            //
            //
            this.Attributes[GameAttribute.Hitpoints_Cur] = 308.25f;
            //
            //
            //
            this.Attributes[GameAttribute.Experience_Next] = 0x003C19;
            this.Attributes[GameAttribute.Experience_Granted] = 0x28;
            this.Attributes[GameAttribute.Armor_Total] = 0x40E00000;
            this.Attributes[GameAttribute.Armor_Item_Total] = 0x40E00000;
            this.Attributes[GameAttribute.Armor_Item_SubTotal] = 0x40E00000;

            this.Attributes[GameAttribute.Armor_Item] = 0;
            this.Attributes[GameAttribute.Defense] = 23;
            this.Attributes[GameAttribute.Vitality] = 23;
            this.Attributes[GameAttribute.Precision] = 23;
            this.Attributes[GameAttribute.Attack] = 23;
            this.Attributes[GameAttribute.General_Cooldown] = 0;
            this.Attributes[GameAttribute.Level] = 7;
            this.Attributes[GameAttribute.Level_Cap] = 60;

            #endregion
        }
    }
}