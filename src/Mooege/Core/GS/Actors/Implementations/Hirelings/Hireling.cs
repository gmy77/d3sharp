/*
 * Copyright (C) 2011 - 2012 mooege project - http://www.mooege.org
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

using Mooege.Core.GS.Common.Types.Math;
using Mooege.Core.GS.Map;
using Mooege.Core.GS.Players;
using Mooege.Net.GS.Message;
using Mooege.Core.GS.Actors.Interactions;
using Mooege.Net.GS.Message.Definitions.Inventory;
using Mooege.Net.GS.Message.Fields;
using System;
using Mooege.Core.GS.Common.Types.TagMap;

namespace Mooege.Core.GS.Actors.Implementations.Hirelings
{
    public class Hireling : InteractiveNPC
    {
        protected int mainSNO = -1;
        protected int hirelingSNO = -1;
        protected int proxySNO = -1;
        protected int skillKit = -1;
        protected int hirelingGBID = -1;


        private World originalWorld = null;
        private PRTransform originalPRT = null;

        protected Player owner = null;

        public bool IsProxy { get { return ActorSNO.Id == proxySNO; } }
        public bool IsHireling { get { return ActorSNO.Id == hirelingSNO; } }
        public bool HasHireling { get { return this.hirelingSNO != -1; } }
        public bool HasProxy { get { return this.proxySNO != -1; } }
        public int PetType { get { return IsProxy ? 22 : 0; } }


        public Hireling(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
            //this.Attributes[GameAttribute.TeamID] = 2;
            Interactions.Add(new HireInteraction());
            Interactions.Add(new InventoryInteraction());
        }

        private void SetUpAttributes(Player player)
        {
            this.owner = player;

            var info = player.HirelingInfo[this.Attributes[GameAttribute.Hireling_Class]];

            // TODO: fix this hardcoded crap
            if (!IsProxy)
                this.Attributes[GameAttribute.Buff_Visual_Effect, 0x000FFFFF] = true;

            //scripted //this.Attributes[GameAttribute.Hitpoints_Max_Total] = 308.25f;
            //scripted //this.Attributes[GameAttribute.Hitpoints_Total_From_Level] = 3.051758E-05f;            

            if (!IsHireling && !IsProxy) // original doesn't need more attribs
            {
                this.Attributes[GameAttribute.Level] = info.Level;
                return;
            }

            this.Attributes[GameAttribute.Level] = info.Level;

            if (info.Skill1SNOId != -1)
            {
                //scripted //this.Attributes[GameAttribute.Skill_Total, info.Skill1SNOId] = 1;
                this.Attributes[GameAttribute.Skill, info.Skill1SNOId] = 1;
            }

            if (info.Skill2SNOId != -1)
            {
                //scripted //this.Attributes[GameAttribute.Skill_Total, info.Skill2SNOId] = 1;
                this.Attributes[GameAttribute.Skill, info.Skill2SNOId] = 1;
            }

            if (info.Skill3SNOId != -1)
            {
                //scripted //this.Attributes[GameAttribute.Skill_Total, info.Skill3SNOId] = 1;
                this.Attributes[GameAttribute.Skill, info.Skill3SNOId] = 1;
            }

            if (info.Skill4SNOId != -1)
            {
                //scripted //this.Attributes[GameAttribute.Skill_Total, info.Skill4SNOId] = 1;
                this.Attributes[GameAttribute.Skill, info.Skill4SNOId] = 1;
            }

            this.Attributes[GameAttribute.SkillKit] = skillKit;

            #region hardcoded attribs :/

            this.Attributes[GameAttribute.Armor_Item] = 20;
            //scripted //this.Attributes[GameAttribute.Armor_Item_SubTotal] = 20;
            //scripted //this.Attributes[GameAttribute.Armor_Item_Total] = 20;
            //scripted //this.Attributes[GameAttribute.Armor_Total] = 20;
            this.Attributes[GameAttribute.Attack] = 23;
            this.Attributes[GameAttribute.Attacks_Per_Second] = 1f;
            this.Attributes[GameAttribute.Attacks_Per_Second_Item] = 1.199219f;
            //scripted //this.Attributes[GameAttribute.Attacks_Per_Second_Item_CurrentHand] = 1.199219f;
            //scripted //this.Attributes[GameAttribute.Attacks_Per_Second_Item_MainHand] = 1.199219f;
            //scripted //this.Attributes[GameAttribute.Attacks_Per_Second_Item_Subtotal] = 3.051758E-05f;
            //scripted //this.Attributes[GameAttribute.Attacks_Per_Second_Item_Total] = 1.199219f;
            //scripted //this.Attributes[GameAttribute.Attacks_Per_Second_Item_Total_MainHand] = 1.199219f;
            //scripted //this.Attributes[GameAttribute.Attacks_Per_Second_Total] = 1.199219f;
            this.Attributes[GameAttribute.Block_Amount_Item_Delta] = 4;
            this.Attributes[GameAttribute.Block_Amount_Item_Min] = 6;
            //scripted //this.Attributes[GameAttribute.Block_Amount_Total_Min] = 6;
            this.Attributes[GameAttribute.Block_Chance_Item] = 3.051758E-05f;
            //scripted //this.Attributes[GameAttribute.Block_Chance_Item_Total] = 0.1099854f;
            //scripted //this.Attributes[GameAttribute.Block_Chance_Total] = 0.1099854f;
            this.Attributes[GameAttribute.Casting_Speed] = 1;
            //scripted //this.Attributes[GameAttribute.Casting_Speed_Total] = 1;
            this.Attributes[GameAttribute.Damage_Delta, 0] = 2;
            //scripted //this.Attributes[GameAttribute.Damage_Delta_Total, 0] = 2;
            this.Attributes[GameAttribute.Damage_Min, 0] = 6.808594f;
            //scripted //this.Attributes[GameAttribute.Damage_Min_Subtotal, 0] = 6.808594f;
            //scripted //this.Attributes[GameAttribute.Damage_Min_Total, 0] = 6.808594f;
            this.Attributes[GameAttribute.Damage_Weapon_Delta, 0] = 2;
            //scripted //this.Attributes[GameAttribute.Damage_Weapon_Delta_SubTotal, 0] = 2;
            //scripted //this.Attributes[GameAttribute.Damage_Weapon_Delta_Total, 0] = 2;
            //scripted //this.Attributes[GameAttribute.Damage_Weapon_Delta_Total_All] = 2;
            //scripted //this.Attributes[GameAttribute.Damage_Weapon_Delta_Total_CurrentHand, 0] = 2;
            //scripted //this.Attributes[GameAttribute.Damage_Weapon_Delta_Total_MainHand, 0] = 2;
            //scripted //this.Attributes[GameAttribute.Damage_Weapon_Max, 0] = 8;
            //scripted //this.Attributes[GameAttribute.Damage_Weapon_Max_Total, 0] = 8;
            this.Attributes[GameAttribute.Damage_Weapon_Min, 0] = 6;
            //scripted //this.Attributes[GameAttribute.Damage_Weapon_Min_Total, 0] = 6;
            //scripted //this.Attributes[GameAttribute.Damage_Weapon_Min_Total_All] = 6;
            //scripted //this.Attributes[GameAttribute.Damage_Weapon_Min_Total_CurrentHand, 0] = 6;
            //scripted //this.Attributes[GameAttribute.Damage_Weapon_Min_Total_MainHand, 0] = 6;
            this.Attributes[GameAttribute.Defense] = 23;
            this.Attributes[GameAttribute.Experience_Granted] = 0x28;
            this.Attributes[GameAttribute.Experience_Next] = 0x003C19;
            this.Attributes[GameAttribute.General_Cooldown] = 0;
            this.Attributes[GameAttribute.Get_Hit_Damage] = 20;
            //scripted //this.Attributes[GameAttribute.Get_Hit_Max] = 3.051758E-05f;
            //scripted //this.Attributes[GameAttribute.Get_Hit_Recovery] = 3.051758E-05f;
            this.Attributes[GameAttribute.Hit_Chance] = 1;
            this.Attributes[GameAttribute.Hitpoints_Factor_Vitality] = 4f;  // TODO: check if this is suppose to be 10 like player's
            this.Attributes[GameAttribute.Hitpoints_Max] = 216.25f;
            //scripted //this.Attributes[GameAttribute.Hitpoints_Max_Total] = 308.25f;
            //scripted //this.Attributes[GameAttribute.Hitpoints_Total_From_Level] = 3.051758E-05f;
            //scripted //this.Attributes[GameAttribute.Hitpoints_Total_From_Vitality] = 92;
            this.Attributes[GameAttribute.Hitpoints_Cur] = 308.25f;
            this.Attributes[GameAttribute.Level_Cap] = 60;
            this.Attributes[GameAttribute.Movement_Scalar] = 1;
            //scripted //this.Attributes[GameAttribute.Movement_Scalar_Capped_Total] = 1;
            //scripted //this.Attributes[GameAttribute.Movement_Scalar_Subtotal] = 1;
            //scripted //this.Attributes[GameAttribute.Movement_Scalar_Total] = 1;
            this.Attributes[GameAttribute.Precision] = 23;
            //scripted //this.Attributes[GameAttribute.Resource_Effective_Max, 0] = 1;
            this.Attributes[GameAttribute.Resource_Max, 0] = 1.0f;
            //scripted //this.Attributes[GameAttribute.Resource_Max_Total, 0] = 1.0f;
            //scripted //this.Attributes[GameAttribute.Resource_Regen_Total, 0] = 3.051758E-05f;
            this.Attributes[GameAttribute.Resource_Cur, 0] = 1.0f;
            this.Attributes[GameAttribute.Resource_Type_Primary] = 0;
            this.Attributes[GameAttribute.Running_Rate] = 0.3598633f;
            //scripted //this.Attributes[GameAttribute.Running_Rate_Total] = 0.3598633f;
            this.Attributes[GameAttribute.Sprinting_Rate] = 0.3598633f;
            //scripted //this.Attributes[GameAttribute.Sprinting_Rate_Total] = 0.3598633f;
            this.Attributes[GameAttribute.Strafing_Rate] = 0.1799316f;
            //scripted //this.Attributes[GameAttribute.Strafing_Rate_Total] = 0.1799316f;
            this.Attributes[GameAttribute.Vitality] = 23;
            this.Attributes[GameAttribute.Walking_Rate] = 0.3598633f;
            //scripted //this.Attributes[GameAttribute.Walking_Rate_Total] = 0.3598633f;

            if (IsProxy)
                return;

            this.Attributes[GameAttribute.Callout_Cooldown, 0x000FFFFF] = 0x00000797;
            this.Attributes[GameAttribute.Buff_Visual_Effect, 0x000FFFFF] = true;
            this.Attributes[GameAttribute.Buff_Icon_Count0, 0x000075C1] = 1;
            this.Attributes[GameAttribute.Buff_Active, 0x000075C1] = true;
            this.Attributes[GameAttribute.Conversation_Icon, 0] = 0;
            this.Attributes[GameAttribute.Buff_Active, 0x20c51] = true;
            this.Attributes[GameAttribute.Buff_Icon_End_Tick0, 0x00020C51] = 0x00000A75;
            this.Attributes[GameAttribute.Buff_Icon_Start_Tick0, 0x00020C51] = 0x00000375;
            this.Attributes[GameAttribute.Buff_Icon_Count0, 0x00020C51] = 3;
            this.Attributes[GameAttribute.Callout_Cooldown, 0x1618a] = 743;
            this.Attributes[GameAttribute.Callout_Cooldown, 0x01CAB6] = 743;
            #endregion

        }

        public virtual Hireling CreateHireling(World world, int snoId, TagMap tags)
        {
            throw new NotImplementedException();
        }

        public override void OnHire(Player player)
        {
            if (hirelingSNO == -1)
                return;

            if (IsHireling || IsProxy)
                return; // This really shouldn't happen.. /fasbat

            this.Unreveal(player);
            var hireling = CreateHireling(this.World, hirelingSNO, this.Tags);
            hireling.SetUpAttributes(player);
            hireling.originalWorld = this.World;
            hireling.originalPRT = this.Transform;
            hireling.GBHandle.Type = 4;
            hireling.GBHandle.GBID = hirelingGBID;

            hireling.Attributes[GameAttribute.Pet_Creator] = player.PlayerIndex;
            hireling.Attributes[GameAttribute.Pet_Type] = 0;
            hireling.Attributes[GameAttribute.Pet_Owner] = player.PlayerIndex;

            hireling.RotationW = this.RotationW;
            hireling.RotationAxis = this.RotationAxis;

            hireling.EnterWorld(this.Position);
            player.ActiveHireling = hireling;
            this.Destroy();
            player.SelectedNPC = null;
        }

        public override void OnInventory(Player player)
        {
            if (proxySNO == -1)
                return;

            if (IsHireling || IsProxy)
                return;

            if (player.ActiveHireling != null &&
                player.ActiveHireling.Attributes[GameAttribute.Hireling_Class] == this.Attributes[GameAttribute.Hireling_Class])
                return;

            if (player.ActiveHirelingProxy != null &&
                player.ActiveHirelingProxy.Attributes[GameAttribute.Hireling_Class] == this.Attributes[GameAttribute.Hireling_Class])
                return;
            var hireling = CreateHireling(this.World, proxySNO, this.Tags);
            hireling.SetUpAttributes(player);
            hireling.originalWorld = this.World;
            hireling.originalPRT = this.Transform;
            hireling.GBHandle.Type = 4;
            hireling.GBHandle.GBID = hirelingGBID;
            hireling.Attributes[GameAttribute.Is_NPC] = false;
            hireling.Attributes[GameAttribute.NPC_Is_Operatable] = false;
            hireling.Attributes[GameAttribute.NPC_Has_Interact_Options, 0] = false;
            hireling.Attributes[GameAttribute.Buff_Visual_Effect, 0x00FFFFF] = false;
            hireling.Attributes[GameAttribute.Pet_Creator] = player.PlayerIndex;
            hireling.Attributes[GameAttribute.Pet_Type] = 22;
            hireling.Attributes[GameAttribute.Pet_Owner] = player.PlayerIndex;

            hireling.RotationW = this.RotationW;
            hireling.RotationAxis = this.RotationAxis;

            hireling.EnterWorld(this.Position);
            player.ActiveHirelingProxy = hireling;
        }

        public void Dismiss(Player player)
        {
            this.Unreveal(player);
            if (this.IsHireling)
            {
                var original = CreateHireling(originalWorld, mainSNO, this.Tags);
                original.SetUpAttributes(player);
                original.RotationW = this.originalPRT.Quaternion.W;
                original.RotationAxis = this.originalPRT.Quaternion.Vector3D;
                original.EnterWorld(this.originalPRT.Vector3D);
            }
            this.Destroy();
        }

        public override bool Reveal(Player player)
        {
            if (owner == null)
                SetUpAttributes(player);
            else if (IsProxy && owner != player)
                return false;

            if (!base.Reveal(player))
                return false;

            if (IsProxy)
            {
                player.InGameClient.SendMessage(new VisualInventoryMessage()
                {
                    ActorID = this.DynamicID,
                    EquipmentList = new VisualEquipment()
                    {
                        Equipment = new VisualItem[]
                    {
                        new VisualItem()
                        {
                            GbId = -1,
                            Field1 = 0,
                            Field2 = 0,
                            Field3 = 0,
                        },
                        new VisualItem()
                        {
                            GbId = -1,
                            Field1 = 0,
                            Field2 = 0,
                            Field3 = 0,
                        },
                        new VisualItem()
                        {
                            GbId = -1,
                            Field1 = 0,
                            Field2 = 0,
                            Field3 = 0,
                        },
                        new VisualItem()
                        {
                            GbId = -1,
                            Field1 = 0,
                            Field2 = 0,
                            Field3 = 0,
                        },
                        new VisualItem()
                        {
                            GbId = -1,
                            Field1 = 0,
                            Field2 = 0,
                            Field3 = 0,
                        },
                        new VisualItem()
                        {
                            GbId = -1,
                            Field1 = 0,
                            Field2 = 0,
                            Field3 = 0,
                        },
                        new VisualItem()
                        {
                            GbId = -1,
                            Field1 = 0,
                            Field2 = 0,
                            Field3 = 0,
                        },
                        new VisualItem()
                        {
                            GbId = -1,
                            Field1 = 0,
                            Field2 = 0,
                            Field3 = 0,
                        },
                    }
                    }
                });
                return true;
            }

            player.InGameClient.SendMessage(new VisualInventoryMessage()
            {
                ActorID = this.DynamicID,
                EquipmentList = new VisualEquipment()
                {
                    Equipment = new VisualItem[]
                    {
                        new VisualItem()
                        {
                            GbId = -1,
                            Field1 = 0,
                            Field2 = 0,
                            Field3 = 0,
                        },
                        new VisualItem()
                        {
                            GbId = -1,
                            Field1 = 0,
                            Field2 = 0,
                            Field3 = 0,
                        },
                        new VisualItem()
                        {
                            GbId = -1,
                            Field1 = 0,
                            Field2 = 0,
                            Field3 = 0,
                        },
                        new VisualItem()
                        {
                            GbId = -1,
                            Field1 = 0,
                            Field2 = 0,
                            Field3 = 0,
                        },
                        new VisualItem()
                        {
                            GbId = unchecked((int)0xF9F6170B),
                            Field1 = 0,
                            Field2 = 0,
                            Field3 = -1,
                        },
                        new VisualItem()
                        {
                            GbId = 0x6C3B0389,
                            Field1 = 0,
                            Field2 = 0,
                            Field3 = -1,
                        },
                        new VisualItem()
                        {
                            GbId = -1,
                            Field1 = 0,
                            Field2 = 0,
                            Field3 = 0,
                        },
                        new VisualItem()
                        {
                            GbId = -1,
                            Field1 = 0,
                            Field2 = 0,
                            Field3 = 0,
                        },
                    }
                }
            });

            return true;
        }
    }
}
