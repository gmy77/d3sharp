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

using Mooege.Common;
using Mooege.Core.Common.Toons;
using Mooege.Net.GS;
using Mooege.Net.GS.Message;
using Mooege.Net.GS.Message.Definitions.ACD;
using Mooege.Net.GS.Message.Definitions.Act;
using Mooege.Net.GS.Message.Definitions.Attribute;
using Mooege.Net.GS.Message.Definitions.Connection;
using Mooege.Net.GS.Message.Definitions.Game;
using Mooege.Net.GS.Message.Definitions.Hero;
using Mooege.Net.GS.Message.Definitions.Misc;
using Mooege.Net.GS.Message.Definitions.Player;
using Mooege.Net.GS.Message.Fields;

namespace Mooege.Core.GS.Universe
{
    public class Player
    {
        private static readonly Logger Logger = LogManager.CreateLogger();

        public GameClient Client { get; set; }
        public Hero Hero { get; set; }
        public Universe Universe;
        
        public Player(GameClient client, Universe universe, Toon bnetToon)
        {
            this.Client = client;
            this.Universe = universe;
            this.Hero = new Hero(client, universe, bnetToon);

        }

        /// <summary>
        /// Greets the player and sends the client initial data it needs to get-ingame.
        /// </summary>
        /// <param name="message"></param>
        public void Greet(JoinBNetGameMessage message)
        {
            Logger.Trace("Greeting player with toon-name: {0} and positioning him to {1}", this.Hero.Properties.Name, Hero.Position);

            // send versions message
            Client.SendMessageNow(new VersionsMessage(message.SNOPackHash));

            // send connection established message.
            Client.SendMessage(new ConnectionEstablishedMessage()
                                   {
                                       Field0 = 0x00000000,
                                       Field1 = 0x4BB91A16,
                                       SNOPackHash = message.SNOPackHash,
                                   });

            // game setup message.
            Client.SendMessage(new GameSetupMessage()
                                   {
                                       Field0 = 0x00000077,
                                   });

            Client.SendMessage(new SavePointInfoMessage()
                                   {
                                       snoLevelArea = -1,
                                   });

            Client.SendMessage(new HearthPortalInfoMessage()
                                   {
                                       snoLevelArea = -1,
                                       Field1 = -1,
                                   });

            // transition player to act so client can load act related data? /raist
            Client.SendMessage(new ActTransitionMessage()
                                   {
                                       Field0 = 0x00000000,
                                       Field1 = true,
                                   });
            
            //reveal world to the toon
            if (Hero.CurrentWorld != null)
                Hero.CurrentWorld.Reveal(this.Hero);

            // send newplayermessage.
            Client.SendMessage(new NewPlayerMessage()
                                   {
                                       Id = 0x0031,
                                       Field0 = 0x00000000, //Party frame (0x00000000 hide, 0x00000001 show)
                                       Field1 = "", //Owner name?
                                       ToonName = this.Hero.Properties.Name,
                                       Field3 = 0x00000002, //party frame class 
                                       Field4 = 0x00000004, //party frame level
                                       snoActorPortrait = this.Hero.ClassSNO, //party frame portrait
                                       Field6 = 0x00000001,
                                       Field7 = this.Hero.GetStateData(),
                                       Field8 = false, //announce party join
                                       Field9 = 0x00000001,
                                       Field10 = this.Hero.DynamicId,
                                   });            

            // reveal the hero
            Hero.Reveal(this.Hero);

            Client.SendMessage(new ACDCollFlagsMessage()
                                   {
                                       Field0 = this.Hero.DynamicId,
                                       Field1 = 0x00000000,
                                   });

            GameAttributeMap attribs = new GameAttributeMap();
            attribs[GameAttribute.SkillKit] = Client.Player.Hero.SkillKit;
            attribs[GameAttribute.Buff_Active, 0x33C40] = true;
            attribs[GameAttribute.Skill, 0x7545] = 1;
            attribs[GameAttribute.Skill_Total, 0x7545] = 1;
            attribs[GameAttribute.Resistance_Total, 0x226] = 0.5f;
            attribs[GameAttribute.Resistance, 0x226] = 0.5f;
            attribs[GameAttribute.Immobolize] = true;
            attribs[GameAttribute.Untargetable] = true;
            attribs[GameAttribute.Skill_Total, 0x76B7] = 1;
            attribs[GameAttribute.Skill, 0x76B7] = 1;
            attribs[GameAttribute.Skill, 0x6DF] = 1;
            attribs[GameAttribute.Buff_Active, 0xCE11] = true;
            attribs[GameAttribute.CantStartDisplayedPowers] = true;
            attribs[GameAttribute.Skill_Total, 0x216FA] = 1;
            attribs[GameAttribute.Skill, 0x176C4] = 1;
            //--
            attribs[GameAttribute.Skill, 0x216FA] = 1;
            attribs[GameAttribute.Skill_Total, 0x176C4] = 1;
            attribs[GameAttribute.Skill_Total, 0x6DF] = 1;
            attribs[GameAttribute.Resistance, 0xDE] = 0.5f;
            attribs[GameAttribute.Resistance_Total, 0xDE] = 0.5f;
            attribs[GameAttribute.Get_Hit_Recovery] = 6f;
            attribs[GameAttribute.Get_Hit_Recovery_Per_Level] = 1f;
            attribs[GameAttribute.Get_Hit_Recovery_Base] = 5f;
            attribs[GameAttribute.Skill, 0x7780] = 1;
            attribs[GameAttribute.Get_Hit_Max] = 60f;
            attribs[GameAttribute.Skill_Total, 0x7780] = 1;
            attribs[GameAttribute.Get_Hit_Max_Per_Level] = 10f;
            attribs[GameAttribute.Get_Hit_Max_Base] = 50f;
            attribs[GameAttribute.Resistance_Total, 0] = 3.051758E-05f; // im pretty sure key = 0 doesnt do anything since the lookup is (attributeId | (key << 12)), maybe this is some base resistance? /cm
            attribs[GameAttribute.Resistance_Total, 1] = 3.051758E-05f;
            //--
            attribs[GameAttribute.Resistance_Total, 2] = 3.051758E-05f;
            attribs[GameAttribute.Resistance_Total, 3] = 3.051758E-05f;
            attribs[GameAttribute.Resistance_Total, 4] = 3.051758E-05f;
            attribs[GameAttribute.Resistance_Total, 5] = 3.051758E-05f;
            attribs[GameAttribute.Resistance_Total, 6] = 3.051758E-05f;
            attribs[GameAttribute.Dodge_Rating_Total] = 3.051758E-05f;
            attribs[GameAttribute.IsTrialActor] = true;
            attribs[GameAttribute.Buff_Visual_Effect, 0xFFFFF] = true;
            attribs[GameAttribute.Crit_Percent_Cap] = 0x3F400000;
            attribs[GameAttribute.Resource_Cur, Client.Player.Hero.ResourceID] = 200f;
            attribs[GameAttribute.Resource_Max, Client.Player.Hero.ResourceID] = 200f;
            attribs[GameAttribute.Resource_Max_Total, Client.Player.Hero.ResourceID] = 200f;
            attribs[GameAttribute.Damage_Weapon_Min_Total_All] = 2f;
            attribs[GameAttribute.Damage_Weapon_Delta_Total_All] = 1f;
            attribs[GameAttribute.Resource_Regen_Total, Client.Player.Hero.ResourceID] = 3.051758E-05f;
            //--
            attribs[GameAttribute.Resource_Effective_Max, Client.Player.Hero.ResourceID] = 200f;
            attribs[GameAttribute.Damage_Min_Subtotal, 0xFFFFF] = 3.051758E-05f;
            attribs[GameAttribute.Damage_Min_Total, 0xFFFFF] = 3.051758E-05f;
            attribs[GameAttribute.Damage_Weapon_Min_Total_CurrentHand, 0xFFFFF] = 3.051758E-05f;
            attribs[GameAttribute.Attacks_Per_Second_Item_CurrentHand] = 1.199219f;
            attribs[GameAttribute.Attacks_Per_Second_Item_Total_MainHand] = 1.199219f;
            attribs[GameAttribute.Attacks_Per_Second_Total] = 1.199219f;
            attribs[GameAttribute.Attacks_Per_Second] = 1f;
            attribs[GameAttribute.Attacks_Per_Second_Item_MainHand] = 1.199219f;
            attribs[GameAttribute.Attacks_Per_Second_Item_Total] = 1.199219f;
            attribs[GameAttribute.Buff_Icon_End_Tick0, 0x00033C40] = 0x000003FB;
            attribs[GameAttribute.Attacks_Per_Second_Item_Subtotal] = 3.051758E-05f;
            attribs[GameAttribute.Attacks_Per_Second_Item] = 3.051758E-05f;
            attribs[GameAttribute.Buff_Icon_Start_Tick0, 0x00033C40] = 0x00000077;
            attribs[GameAttribute.Hit_Chance] = 1f;
            //--
            attribs[GameAttribute.Casting_Speed_Total] = 1f;
            attribs[GameAttribute.Casting_Speed] = 1f;
            attribs[GameAttribute.Movement_Scalar_Total] = 1f;
            attribs[GameAttribute.Skill_Total, 0x0002EC66] = 0;
            attribs[GameAttribute.Movement_Scalar_Capped_Total] = 1f;
            attribs[GameAttribute.Movement_Scalar_Subtotal] = 1f;
            attribs[GameAttribute.Strafing_Rate_Total] = 3.051758E-05f;
            attribs[GameAttribute.Sprinting_Rate_Total] = 3.051758E-05f;
            attribs[GameAttribute.Running_Rate_Total] = 0.3598633f;
            attribs[GameAttribute.Damage_Weapon_Min_Total_MainHand, 0] = 2f;
            attribs[GameAttribute.Walking_Rate_Total] = 0.2797852f;
            attribs[GameAttribute.Damage_Weapon_Delta_Total_MainHand, 0] = 1f;
            attribs[GameAttribute.Damage_Delta_Total, 1] = 3.051758E-05f;
            attribs[GameAttribute.Damage_Delta_Total, 2] = 3.051758E-05f;
            attribs[GameAttribute.Damage_Delta_Total, 3] = 3.051758E-05f;
            //--
            attribs[GameAttribute.Damage_Delta_Total, 4] = 3.051758E-05f;
            attribs[GameAttribute.Damage_Delta_Total, 5] = 3.051758E-05f;
            attribs[GameAttribute.Damage_Delta_Total, 6] = 3.051758E-05f;
            attribs[GameAttribute.Damage_Delta_Total, 0] = 1f;
            attribs[GameAttribute.Running_Rate] = 0.3598633f;
            attribs[GameAttribute.Damage_Weapon_Min_Total_CurrentHand, 1] = 3.051758E-05f;
            attribs[GameAttribute.Damage_Weapon_Min_Total_CurrentHand, 2] = 3.051758E-05f;
            attribs[GameAttribute.Damage_Weapon_Min_Total_CurrentHand, 3] = 3.051758E-05f;
            attribs[GameAttribute.Damage_Weapon_Min_Total_CurrentHand, 4] = 3.051758E-05f;
            attribs[GameAttribute.Damage_Weapon_Min_Total_CurrentHand, 5] = 3.051758E-05f;
            attribs[GameAttribute.Damage_Weapon_Min_Total_CurrentHand, 6] = 3.051758E-05f;
            attribs[GameAttribute.Damage_Weapon_Min_Total_CurrentHand, 0] = 2f;
            attribs[GameAttribute.Walking_Rate] = 0.2797852f;
            attribs[GameAttribute.Damage_Min_Total, 1] = 3.051758E-05f;
            attribs[GameAttribute.Damage_Min_Total, 2] = 3.051758E-05f;
            //--
            attribs[GameAttribute.Damage_Min_Total, 3] = 3.051758E-05f;
            attribs[GameAttribute.Damage_Min_Total, 4] = 3.051758E-05f;
            attribs[GameAttribute.Damage_Min_Total, 5] = 3.051758E-05f;
            attribs[GameAttribute.Damage_Min_Total, 6] = 3.051758E-05f;
            attribs[GameAttribute.Damage_Weapon_Delta_Total_CurrentHand, 1] = 3.051758E-05f;
            attribs[GameAttribute.Damage_Weapon_Delta_Total_CurrentHand, 2] = 3.051758E-05f;
            attribs[GameAttribute.Damage_Weapon_Delta_Total_CurrentHand, 3] = 3.051758E-05f;
            attribs[GameAttribute.Damage_Weapon_Delta_Total_CurrentHand, 4] = 3.051758E-05f;
            attribs[GameAttribute.Damage_Weapon_Delta_Total_CurrentHand, 5] = 3.051758E-05f;
            attribs[GameAttribute.Damage_Weapon_Delta_Total_CurrentHand, 6] = 3.051758E-05f;
            attribs[GameAttribute.Damage_Min_Total, 0] = 2f;
            attribs[GameAttribute.Damage_Weapon_Delta_Total_CurrentHand, 0] = 1f;
            attribs[GameAttribute.Movement_Scalar] = 1f;
            attribs[GameAttribute.Damage_Min_Subtotal, 1] = 3.051758E-05f;
            attribs[GameAttribute.Damage_Min_Subtotal, 2] = 3.051758E-05f;
            //--
            attribs[GameAttribute.Damage_Min_Subtotal, 3] = 3.051758E-05f;
            attribs[GameAttribute.Damage_Min_Subtotal, 4] = 3.051758E-05f;
            attribs[GameAttribute.Damage_Min_Subtotal, 5] = 3.051758E-05f;
            attribs[GameAttribute.Damage_Min_Subtotal, 6] = 3.051758E-05f;
            attribs[GameAttribute.Damage_Min_Subtotal, 0] = 2f;
            attribs[GameAttribute.Damage_Weapon_Delta, 0] = 1f;
            attribs[GameAttribute.Damage_Weapon_Delta_SubTotal, 0] = 1f;
            attribs[GameAttribute.Damage_Weapon_Max, 0] = 3f;
            attribs[GameAttribute.Damage_Weapon_Max_Total, 0] = 3f;
            attribs[GameAttribute.Damage_Weapon_Delta_Total, 0] = 1f;
            attribs[GameAttribute.Trait, 0x0000CE11] = 1;
            attribs[GameAttribute.Damage_Weapon_Min, 0] = 2f;
            attribs[GameAttribute.Damage_Weapon_Min_Total, 0] = 2f;
            attribs[GameAttribute.Skill, 0x0000CE11] = 1;
            attribs[GameAttribute.Skill_Total, 0x0000CE11] = 1;
            //--
            attribs[GameAttribute.Resource_Type_Primary] = Client.Player.Hero.ResourceID;
            attribs[GameAttribute.Hitpoints_Max_Total] = 76f;
            attribs[GameAttribute.Hitpoints_Max] = 40f;
            attribs[GameAttribute.Hitpoints_Total_From_Level] = 3.051758E-05f;
            attribs[GameAttribute.Hitpoints_Total_From_Vitality] = 36f;
            attribs[GameAttribute.Hitpoints_Factor_Vitality] = 4f;
            attribs[GameAttribute.Hitpoints_Factor_Level] = 4f;
            attribs[GameAttribute.Hitpoints_Cur] = 76f;
            attribs[GameAttribute.Disabled] = true;
            attribs[GameAttribute.Loading] = true;
            attribs[GameAttribute.Invulnerable] = true;
            attribs[GameAttribute.TeamID] = 2;
            attribs[GameAttribute.Skill_Total, 0xFFFFF] = 1;
            attribs[GameAttribute.Skill, 0xFFFFF] = 1;
            attribs[GameAttribute.Buff_Icon_Count0, 0x0000CE11] = 1;
            //--
            attribs[GameAttribute.Hidden] = true;
            attribs[GameAttribute.Level_Cap] = 13;
            attribs[GameAttribute.Level] = Client.Player.Hero.Properties.Level;
            attribs[GameAttribute.Experience_Next] = 1200;
            attribs[GameAttribute.Experience_Granted] = 1000;
            attribs[GameAttribute.Armor_Total] = 0;
            attribs[GameAttribute.Defense] = 10f;
            attribs[GameAttribute.Buff_Icon_Count0, 0x00033C40] = 1;
            attribs[GameAttribute.Vitality] = 9f;
            attribs[GameAttribute.Precision] = 11f;
            attribs[GameAttribute.Attack] = 10f;
            attribs[GameAttribute.Shared_Stash_Slots] = 14;
            attribs[GameAttribute.Backpack_Slots] = 60;
            attribs[GameAttribute.General_Cooldown] = 0;

            attribs.SendMessage(Client, this.Hero.DynamicId);
            
            Client.SendMessage(new ACDGroupMessage()
                                   {
                                       Id = 0x00B8,
                                       Field0 = this.Hero.DynamicId,
                                       Field1 = -1,
                                       Field2 = -1,
                                   });

            Client.SendMessage(new ANNDataMessage()
                                   {
                                       Id = 0x003E,
                                       Field0 = this.Hero.DynamicId,
                                   });

            Client.SendMessage(new ACDTranslateFacingMessage()
                                   {
                                       Id = 0x0070,
                                       Field0 = this.Hero.DynamicId,
                                       Field1 = 3.022712f,
                                       Field2 = false,
                                   });

            Client.SendMessage(new PlayerEnterKnownMessage()
                                   {
                                       Field0 = 0x00000000,
                                       Field1 = this.Hero.DynamicId,
                                   });
            
            Client.SendMessage(new PlayerActorSetInitialMessage()
                                   {
                                       Field0 = this.Hero.DynamicId,
                                       Field1 = 0x00000000,
                                   });

            Client.SendMessage(new SNONameDataMessage()
                                   {
                                       Id = 0x00D3,
                                       Field0 = new SNOName()
                                                    {
                                                        Field0 = 0x00000001,
                                                        Field1 = Client.Player.Hero.ClassSNO,
                                                    },
                                   });

            Client.FlushOutgoingBuffer();

            Client.SendMessage(new DWordDataMessage() // TICK
                                   {
                                       Id = 0x0089,
                                       Field0 = 0x00000077,
                                   });

            Client.FlushOutgoingBuffer();

            Client.SendMessage(new AttributeSetValueMessage()
                                   {
                                       Field0 = this.Hero.DynamicId,
                                       Field1 = new NetAttributeKeyValue()
                                                    {
                                                        Attribute = GameAttribute.Attributes[0x005B], // Hitpoints_Healed_Target
                                                        Int = 0x00000000,
                                                        Float = 76f,
                                                    },
                                   });
            Client.SendMessage(new DWordDataMessage() // TICK
                                   {
                                       Id = 0x0089,
                                       Field0 = 0x0000007D,
                                   });

            Client.FlushOutgoingBuffer();
        }
    }
}
