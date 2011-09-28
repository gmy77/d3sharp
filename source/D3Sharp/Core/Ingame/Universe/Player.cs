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

using System;
using D3Sharp.Core.Common.Toons;
using D3Sharp.Net.Game;
using D3Sharp.Net.Game.Message;
using D3Sharp.Net.Game.Message.Definitions.ACD;
using D3Sharp.Net.Game.Message.Definitions.Act;
using D3Sharp.Net.Game.Message.Definitions.Attribute;
using D3Sharp.Net.Game.Message.Definitions.Connection;
using D3Sharp.Net.Game.Message.Definitions.Game;
using D3Sharp.Net.Game.Message.Definitions.Hero;
using D3Sharp.Net.Game.Message.Definitions.Inventory;
using D3Sharp.Net.Game.Message.Definitions.Misc;
using D3Sharp.Net.Game.Message.Definitions.Player;
using D3Sharp.Net.Game.Message.Fields;
using D3Sharp.Utils;

namespace D3Sharp.Core.Ingame.Universe
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
            Logger.Debug("Greeting player with toon-name: {0} and positioning him to {1}", this.Hero.Properties.Name,
                         Hero.Position);

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
                                       Field10 = 0x789E00E2,
                                   });            

            Hero.Reveal(this.Hero);

            Client.SendMessage(new ACDCollFlagsMessage()
                                   {
                                       Id = 0x00A6,
                                       Field0 = 0x789E00E2,
                                       Field1 = 0x00000000,
                                   });

            Client.SendMessage(new AttributesSetValuesMessage()
                                   {
                                       Id = 0x004D,
                                       Field0 = 0x789E00E2,
                                       atKeyVals = new NetAttributeKeyValue[15]
                                                       {
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Attribute = GameAttribute.Attributes[0x01F8],
                                                                   // SkillKit 
                                                                   Int = Client.Player.Hero.SkillKit,
                                                                   Float = 0f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = 0x00033C40,
                                                                   Attribute = GameAttribute.Attributes[0x01CC],
                                                                   // Buff_Active 
                                                                   Int = 0x00000001,
                                                                   Float = 0f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = 0x00007545,
                                                                   Attribute = GameAttribute.Attributes[0x0041],
                                                                   // Skill 
                                                                   Int = 0x00000001,
                                                                   Float = 0f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = 0x00007545,
                                                                   Attribute = GameAttribute.Attributes[0x0042],
                                                                   // Skill_Total 
                                                                   Int = 0x00000001,
                                                                   Float = 0f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = 0x00000226,
                                                                   Attribute = GameAttribute.Attributes[0x003E],
                                                                   // Resistance_Total 
                                                                   Int = 0x00000000,
                                                                   Float = 0.5f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = 0x00000226,
                                                                   Attribute = GameAttribute.Attributes[0x003C],
                                                                   // Resistance 
                                                                   Int = 0x00000000,
                                                                   Float = 0.5f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Attribute = GameAttribute.Attributes[0x00D7],
                                                                   // Immobolize 
                                                                   Int = 0x00000001,
                                                                   Float = 0f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Attribute = GameAttribute.Attributes[0x00D6],
                                                                   // Untargetable 
                                                                   Int = 0x00000001,
                                                                   Float = 0f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = 0x000076B7,
                                                                   Attribute = GameAttribute.Attributes[0x0042],
                                                                   // Skill_Total 
                                                                   Int = 0x00000001,
                                                                   Float = 0f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = 0x000076B7,
                                                                   Attribute = GameAttribute.Attributes[0x0041],
                                                                   // Skill 
                                                                   Int = 0x00000001,
                                                                   Float = 0f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = 0x000006DF,
                                                                   Attribute = GameAttribute.Attributes[0x0041],
                                                                   // Skill 
                                                                   Int = 0x00000001,
                                                                   Float = 0f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = 0x0000CE11,
                                                                   Attribute = GameAttribute.Attributes[0x01CC],
                                                                   // Buff_Active 
                                                                   Int = 0x00000001,
                                                                   Float = 0f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Attribute = GameAttribute.Attributes[0x01D2],
                                                                   // CantStartDisplayedPowers 
                                                                   Int = 0x00000001,
                                                                   Float = 0f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = 0x000216FA,
                                                                   Attribute = GameAttribute.Attributes[0x0042],
                                                                   // Skill_Total 
                                                                   Int = 0x00000001,
                                                                   Float = 0f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = 0x000176C4,
                                                                   Attribute = GameAttribute.Attributes[0x0041],
                                                                   // Skill 
                                                                   Int = 0x00000001,
                                                                   Float = 0f,
                                                               },
                                                       },
                                   });

            Client.SendMessage(new AttributesSetValuesMessage()
                                   {
                                       Id = 0x004D,
                                       Field0 = 0x789E00E2,
                                       atKeyVals = new NetAttributeKeyValue[15]
                                                       {
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = 0x000216FA,
                                                                   Attribute = GameAttribute.Attributes[0x0041],
                                                                   // Skill 
                                                                   Int = 0x00000001,
                                                                   Float = 0f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = 0x000176C4,
                                                                   Attribute = GameAttribute.Attributes[0x0042],
                                                                   // Skill_Total 
                                                                   Int = 0x00000001,
                                                                   Float = 0f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = 0x000006DF,
                                                                   Attribute = GameAttribute.Attributes[0x0042],
                                                                   // Skill_Total 
                                                                   Int = 0x00000001,
                                                                   Float = 0f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = 0x000000DE,
                                                                   Attribute = GameAttribute.Attributes[0x003C],
                                                                   // Resistance 
                                                                   Int = 0x00000000,
                                                                   Float = 0.5f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = 0x000000DE,
                                                                   Attribute = GameAttribute.Attributes[0x003E],
                                                                   // Resistance_Total 
                                                                   Int = 0x00000000,
                                                                   Float = 0.5f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Attribute = GameAttribute.Attributes[0x00C8],
                                                                   // Get_Hit_Recovery 
                                                                   Int = 0x00000000,
                                                                   Float = 6f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Attribute = GameAttribute.Attributes[0x00C7],
                                                                   // Get_Hit_Recovery_Per_Level 
                                                                   Int = 0x00000000,
                                                                   Float = 1f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Attribute = GameAttribute.Attributes[0x00C6],
                                                                   // Get_Hit_Recovery_Base 
                                                                   Int = 0x00000000,
                                                                   Float = 5f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = 0x00007780,
                                                                   Attribute = GameAttribute.Attributes[0x0041],
                                                                   // Skill 
                                                                   Int = 0x00000001,
                                                                   Float = 0f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Attribute = GameAttribute.Attributes[0x00C5],
                                                                   // Get_Hit_Max 
                                                                   Int = 0x00000000,
                                                                   Float = 60f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = 0x00007780,
                                                                   Attribute = GameAttribute.Attributes[0x0042],
                                                                   // Skill_Total 
                                                                   Int = 0x00000001,
                                                                   Float = 0f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Attribute = GameAttribute.Attributes[0x00C4],
                                                                   // Get_Hit_Max_Per_Level 
                                                                   Int = 0x00000000,
                                                                   Float = 10f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Attribute = GameAttribute.Attributes[0x00C3],
                                                                   // Get_Hit_Max_Base 
                                                                   Int = 0x00000000,
                                                                   Float = 50f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = 0x00000000,
                                                                   Attribute = GameAttribute.Attributes[0x003E],
                                                                   // Resistance_Total 
                                                                   Int = 0x00000000,
                                                                   Float = 3.051758E-05f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = 0x00000001,
                                                                   Attribute = GameAttribute.Attributes[0x003E],
                                                                   // Resistance_Total 
                                                                   Int = 0x00000000,
                                                                   Float = 3.051758E-05f,
                                                               },
                                                       },
                                   });

            Client.SendMessage(new AttributesSetValuesMessage()
                                   {
                                       Id = 0x004D,
                                       Field0 = 0x789E00E2,
                                       atKeyVals = new NetAttributeKeyValue[15]
                                                       {
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = 0x00000002,
                                                                   Attribute = GameAttribute.Attributes[0x003E],
                                                                   // Resistance_Total 
                                                                   Int = 0x00000000,
                                                                   Float = 3.051758E-05f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = 0x00000003,
                                                                   Attribute = GameAttribute.Attributes[0x003E],
                                                                   // Resistance_Total 
                                                                   Int = 0x00000000,
                                                                   Float = 3.051758E-05f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = 0x00000004,
                                                                   Attribute = GameAttribute.Attributes[0x003E],
                                                                   // Resistance_Total 
                                                                   Int = 0x00000000,
                                                                   Float = 3.051758E-05f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = 0x00000005,
                                                                   Attribute = GameAttribute.Attributes[0x003E],
                                                                   // Resistance_Total 
                                                                   Int = 0x00000000,
                                                                   Float = 3.051758E-05f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = 0x00000006,
                                                                   Attribute = GameAttribute.Attributes[0x003E],
                                                                   // Resistance_Total 
                                                                   Int = 0x00000000,
                                                                   Float = 3.051758E-05f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Attribute = GameAttribute.Attributes[0x00BE],
                                                                   // Dodge_Rating_Total 
                                                                   Int = 0x00000000,
                                                                   Float = 3.051758E-05f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Attribute = GameAttribute.Attributes[0x02BA],
                                                                   // IsTrialActor 
                                                                   Int = 0x00000001,
                                                                   Float = 0f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = 0x000FFFFF,
                                                                   Attribute = GameAttribute.Attributes[0x01B9],
                                                                   // Buff_Visual_Effect 
                                                                   Int = 0x00000001,
                                                                   Float = 0f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Attribute = GameAttribute.Attributes[0x00A8],
                                                                   // Crit_Percent_Cap 
                                                                   Int = 0x3F400000,
                                                                   Float = 0f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = Client.Player.Hero.ResourceID,
                                                                   Attribute = GameAttribute.Attributes[0x005E],
                                                                   // Resource_Cur 
                                                                   Int = 0x43480000,
                                                                   Float = 0f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = Client.Player.Hero.ResourceID,
                                                                   Attribute = GameAttribute.Attributes[0x005F],
                                                                   // Resource_Max 
                                                                   Int = 0x00000000,
                                                                   Float = 200f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = Client.Player.Hero.ResourceID,
                                                                   Attribute = GameAttribute.Attributes[0x0061],
                                                                   // Resource_Max_Total 
                                                                   Int = 0x43480000,
                                                                   Float = 0f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Attribute = GameAttribute.Attributes[0x009D],
                                                                   // Damage_Weapon_Min_Total_All 
                                                                   Int = 0x00000000,
                                                                   Float = 2f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Attribute = GameAttribute.Attributes[0x0099],
                                                                   // Damage_Weapon_Delta_Total_All 
                                                                   Int = 0x00000000,
                                                                   Float = 1f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = Client.Player.Hero.ResourceID,
                                                                   Attribute = GameAttribute.Attributes[0x0068],
                                                                   // Resource_Regen_Total 
                                                                   Int = 0x00000000,
                                                                   Float = 3.051758E-05f,
                                                               },
                                                       },
                                   });

            Client.SendMessage(new AttributesSetValuesMessage()
                                   {
                                       Id = 0x004D,
                                       Field0 = 0x789E00E2,
                                       atKeyVals = new NetAttributeKeyValue[15]
                                                       {
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = Client.Player.Hero.ResourceID,
                                                                   Attribute = GameAttribute.Attributes[0x006B],
                                                                   // Resource_Effective_Max 
                                                                   Int = 0x00000000,
                                                                   Float = 200f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = 0x000FFFFF,
                                                                   Attribute = GameAttribute.Attributes[0x0092],
                                                                   // Damage_Min_Subtotal 
                                                                   Int = 0x00000000,
                                                                   Float = 3.051758E-05f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = 0x000FFFFF,
                                                                   Attribute = GameAttribute.Attributes[0x0091],
                                                                   // Damage_Min_Total 
                                                                   Int = 0x00000000,
                                                                   Float = 3.051758E-05f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = 0x000FFFFF,
                                                                   Attribute = GameAttribute.Attributes[0x0190],
                                                                   // Damage_Weapon_Min_Total_CurrentHand 
                                                                   Int = 0x00000000,
                                                                   Float = 3.051758E-05f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Attribute = GameAttribute.Attributes[0x018F],
                                                                   // Attacks_Per_Second_Item_CurrentHand 
                                                                   Int = 0x00000000,
                                                                   Float = 1.199219f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Attribute = GameAttribute.Attributes[0x0189],
                                                                   // Attacks_Per_Second_Item_Total_MainHand 
                                                                   Int = 0x00000000,
                                                                   Float = 1.199219f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Attribute = GameAttribute.Attributes[0x0089],
                                                                   // Attacks_Per_Second_Total 
                                                                   Int = 0x00000000,
                                                                   Float = 1.199219f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Attribute = GameAttribute.Attributes[0x0087],
                                                                   // Attacks_Per_Second 
                                                                   Int = 0x00000000,
                                                                   Float = 1f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Attribute = GameAttribute.Attributes[0x0187],
                                                                   // Attacks_Per_Second_Item_MainHand 
                                                                   Int = 0x00000000,
                                                                   Float = 1.199219f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Attribute = GameAttribute.Attributes[0x0086],
                                                                   // Attacks_Per_Second_Item_Total 
                                                                   Int = 0x00000000,
                                                                   Float = 1.199219f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = 0x00033C40,
                                                                   Attribute = GameAttribute.Attributes[0x01BE],
                                                                   // Buff_Icon_End_Tick0 
                                                                   Int = 0x000003FB,
                                                                   Float = 0f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Attribute = GameAttribute.Attributes[0x0084],
                                                                   // Attacks_Per_Second_Item_Subtotal 
                                                                   Int = 0x00000000,
                                                                   Float = 3.051758E-05f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Attribute = GameAttribute.Attributes[0x0082],
                                                                   // Attacks_Per_Second_Item 
                                                                   Int = 0x00000000,
                                                                   Float = 3.051758E-05f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = 0x00033C40,
                                                                   Attribute = GameAttribute.Attributes[0x01BA],
                                                                   // Buff_Icon_Start_Tick0 
                                                                   Int = 0x00000077,
                                                                   Float = 0f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Attribute = GameAttribute.Attributes[0x0081],
                                                                   // Hit_Chance 
                                                                   Int = 0x00000000,
                                                                   Float = 1f,
                                                               },
                                                       },
                                   });

            Client.SendMessage(new AttributesSetValuesMessage()
                                   {
                                       Id = 0x004D,
                                       Field0 = 0x789E00E2,
                                       atKeyVals = new NetAttributeKeyValue[15]
                                                       {
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Attribute = GameAttribute.Attributes[0x007F],
                                                                   // Casting_Speed_Total 
                                                                   Int = 0x00000000,
                                                                   Float = 1f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Attribute = GameAttribute.Attributes[0x007D],
                                                                   // Casting_Speed 
                                                                   Int = 0x00000000,
                                                                   Float = 1f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Attribute = GameAttribute.Attributes[0x007B],
                                                                   // Movement_Scalar_Total 
                                                                   Int = 0x00000000,
                                                                   Float = 1f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = 0x0002EC66,
                                                                   Attribute = GameAttribute.Attributes[0x0042],
                                                                   // Skill_Total 
                                                                   Int = 0x00000000,
                                                                   Float = 0f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Attribute = GameAttribute.Attributes[0x0079],
                                                                   // Movement_Scalar_Capped_Total 
                                                                   Int = 0x00000000,
                                                                   Float = 1f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Attribute = GameAttribute.Attributes[0x0078],
                                                                   // Movement_Scalar_Subtotal 
                                                                   Int = 0x00000000,
                                                                   Float = 1f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Attribute = GameAttribute.Attributes[0x0076],
                                                                   // Strafing_Rate_Total 
                                                                   Int = 0x00000000,
                                                                   Float = 3.051758E-05f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Attribute = GameAttribute.Attributes[0x0075],
                                                                   // Sprinting_Rate_Total 
                                                                   Int = 0x00000000,
                                                                   Float = 3.051758E-05f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Attribute = GameAttribute.Attributes[0x0074],
                                                                   // Running_Rate_Total 
                                                                   Int = 0x00000000,
                                                                   Float = 0.3598633f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = 0x00000000,
                                                                   Attribute = GameAttribute.Attributes[0x018B],
                                                                   // Damage_Weapon_Min_Total_MainHand 
                                                                   Int = 0x00000000,
                                                                   Float = 2f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Attribute = GameAttribute.Attributes[0x0073],
                                                                   // Walking_Rate_Total 
                                                                   Int = 0x00000000,
                                                                   Float = 0.2797852f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = 0x00000000,
                                                                   Attribute = GameAttribute.Attributes[0x018D],
                                                                   // Damage_Weapon_Delta_Total_MainHand 
                                                                   Int = 0x00000000,
                                                                   Float = 1f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = 0x00000001,
                                                                   Attribute = GameAttribute.Attributes[0x008E],
                                                                   // Damage_Delta_Total 
                                                                   Int = 0x00000000,
                                                                   Float = 3.051758E-05f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = 0x00000002,
                                                                   Attribute = GameAttribute.Attributes[0x008E],
                                                                   // Damage_Delta_Total 
                                                                   Int = 0x00000000,
                                                                   Float = 3.051758E-05f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = 0x00000003,
                                                                   Attribute = GameAttribute.Attributes[0x008E],
                                                                   // Damage_Delta_Total 
                                                                   Int = 0x00000000,
                                                                   Float = 3.051758E-05f,
                                                               },
                                                       },
                                   });

            Client.SendMessage(new AttributesSetValuesMessage()
                                   {
                                       Id = 0x004D,
                                       Field0 = 0x789E00E2,
                                       atKeyVals = new NetAttributeKeyValue[15]
                                                       {
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = 0x00000004,
                                                                   Attribute = GameAttribute.Attributes[0x008E],
                                                                   // Damage_Delta_Total 
                                                                   Int = 0x00000000,
                                                                   Float = 3.051758E-05f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = 0x00000005,
                                                                   Attribute = GameAttribute.Attributes[0x008E],
                                                                   // Damage_Delta_Total 
                                                                   Int = 0x00000000,
                                                                   Float = 3.051758E-05f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = 0x00000006,
                                                                   Attribute = GameAttribute.Attributes[0x008E],
                                                                   // Damage_Delta_Total 
                                                                   Int = 0x00000000,
                                                                   Float = 3.051758E-05f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = 0x00000000,
                                                                   Attribute = GameAttribute.Attributes[0x008E],
                                                                   // Damage_Delta_Total 
                                                                   Int = 0x00000000,
                                                                   Float = 1f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Attribute = GameAttribute.Attributes[0x0070],
                                                                   // Running_Rate 
                                                                   Int = 0x00000000,
                                                                   Float = 0.3598633f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = 0x00000001,
                                                                   Attribute = GameAttribute.Attributes[0x0190],
                                                                   // Damage_Weapon_Min_Total_CurrentHand 
                                                                   Int = 0x00000000,
                                                                   Float = 3.051758E-05f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = 0x00000002,
                                                                   Attribute = GameAttribute.Attributes[0x0190],
                                                                   // Damage_Weapon_Min_Total_CurrentHand 
                                                                   Int = 0x00000000,
                                                                   Float = 3.051758E-05f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = 0x00000003,
                                                                   Attribute = GameAttribute.Attributes[0x0190],
                                                                   // Damage_Weapon_Min_Total_CurrentHand 
                                                                   Int = 0x00000000,
                                                                   Float = 3.051758E-05f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = 0x00000004,
                                                                   Attribute = GameAttribute.Attributes[0x0190],
                                                                   // Damage_Weapon_Min_Total_CurrentHand 
                                                                   Int = 0x00000000,
                                                                   Float = 3.051758E-05f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = 0x00000005,
                                                                   Attribute = GameAttribute.Attributes[0x0190],
                                                                   // Damage_Weapon_Min_Total_CurrentHand 
                                                                   Int = 0x00000000,
                                                                   Float = 3.051758E-05f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = 0x00000006,
                                                                   Attribute = GameAttribute.Attributes[0x0190],
                                                                   // Damage_Weapon_Min_Total_CurrentHand 
                                                                   Int = 0x00000000,
                                                                   Float = 3.051758E-05f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = 0x00000000,
                                                                   Attribute = GameAttribute.Attributes[0x0190],
                                                                   // Damage_Weapon_Min_Total_CurrentHand 
                                                                   Int = 0x00000000,
                                                                   Float = 2f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Attribute = GameAttribute.Attributes[0x006F],
                                                                   // Walking_Rate 
                                                                   Int = 0x00000000,
                                                                   Float = 0.2797852f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = 0x00000001,
                                                                   Attribute = GameAttribute.Attributes[0x0091],
                                                                   // Damage_Min_Total 
                                                                   Int = 0x00000000,
                                                                   Float = 3.051758E-05f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = 0x00000002,
                                                                   Attribute = GameAttribute.Attributes[0x0091],
                                                                   // Damage_Min_Total 
                                                                   Int = 0x00000000,
                                                                   Float = 3.051758E-05f,
                                                               },
                                                       },
                                   });

            Client.SendMessage(new AttributesSetValuesMessage()
                                   {
                                       Id = 0x004D,
                                       Field0 = 0x789E00E2,
                                       atKeyVals = new NetAttributeKeyValue[15]
                                                       {
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = 0x00000003,
                                                                   Attribute = GameAttribute.Attributes[0x0091],
                                                                   // Damage_Min_Total 
                                                                   Int = 0x00000000,
                                                                   Float = 3.051758E-05f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = 0x00000004,
                                                                   Attribute = GameAttribute.Attributes[0x0091],
                                                                   // Damage_Min_Total 
                                                                   Int = 0x00000000,
                                                                   Float = 3.051758E-05f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = 0x00000005,
                                                                   Attribute = GameAttribute.Attributes[0x0091],
                                                                   // Damage_Min_Total 
                                                                   Int = 0x00000000,
                                                                   Float = 3.051758E-05f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = 0x00000006,
                                                                   Attribute = GameAttribute.Attributes[0x0091],
                                                                   // Damage_Min_Total 
                                                                   Int = 0x00000000,
                                                                   Float = 3.051758E-05f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = 0x00000001,
                                                                   Attribute = GameAttribute.Attributes[0x0191],
                                                                   // Damage_Weapon_Delta_Total_CurrentHand 
                                                                   Int = 0x00000000,
                                                                   Float = 3.051758E-05f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = 0x00000002,
                                                                   Attribute = GameAttribute.Attributes[0x0191],
                                                                   // Damage_Weapon_Delta_Total_CurrentHand 
                                                                   Int = 0x00000000,
                                                                   Float = 3.051758E-05f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = 0x00000003,
                                                                   Attribute = GameAttribute.Attributes[0x0191],
                                                                   // Damage_Weapon_Delta_Total_CurrentHand 
                                                                   Int = 0x00000000,
                                                                   Float = 3.051758E-05f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = 0x00000004,
                                                                   Attribute = GameAttribute.Attributes[0x0191],
                                                                   // Damage_Weapon_Delta_Total_CurrentHand 
                                                                   Int = 0x00000000,
                                                                   Float = 3.051758E-05f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = 0x00000005,
                                                                   Attribute = GameAttribute.Attributes[0x0191],
                                                                   // Damage_Weapon_Delta_Total_CurrentHand 
                                                                   Int = 0x00000000,
                                                                   Float = 3.051758E-05f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = 0x00000006,
                                                                   Attribute = GameAttribute.Attributes[0x0191],
                                                                   // Damage_Weapon_Delta_Total_CurrentHand 
                                                                   Int = 0x00000000,
                                                                   Float = 3.051758E-05f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = 0x00000000,
                                                                   Attribute = GameAttribute.Attributes[0x0091],
                                                                   // Damage_Min_Total 
                                                                   Int = 0x00000000,
                                                                   Float = 2f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = 0x00000000,
                                                                   Attribute = GameAttribute.Attributes[0x0191],
                                                                   // Damage_Weapon_Delta_Total_CurrentHand 
                                                                   Int = 0x00000000,
                                                                   Float = 1f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Attribute = GameAttribute.Attributes[0x006E],
                                                                   // Movement_Scalar 
                                                                   Int = 0x00000000,
                                                                   Float = 1f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = 0x00000001,
                                                                   Attribute = GameAttribute.Attributes[0x0092],
                                                                   // Damage_Min_Subtotal 
                                                                   Int = 0x00000000,
                                                                   Float = 3.051758E-05f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = 0x00000002,
                                                                   Attribute = GameAttribute.Attributes[0x0092],
                                                                   // Damage_Min_Subtotal 
                                                                   Int = 0x00000000,
                                                                   Float = 3.051758E-05f,
                                                               },
                                                       },
                                   });

            Client.SendMessage(new AttributesSetValuesMessage()
                                   {
                                       Id = 0x004D,
                                       Field0 = 0x789E00E2,
                                       atKeyVals = new NetAttributeKeyValue[15]
                                                       {
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = 0x00000003,
                                                                   Attribute = GameAttribute.Attributes[0x0092],
                                                                   // Damage_Min_Subtotal 
                                                                   Int = 0x00000000,
                                                                   Float = 3.051758E-05f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = 0x00000004,
                                                                   Attribute = GameAttribute.Attributes[0x0092],
                                                                   // Damage_Min_Subtotal 
                                                                   Int = 0x00000000,
                                                                   Float = 3.051758E-05f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = 0x00000005,
                                                                   Attribute = GameAttribute.Attributes[0x0092],
                                                                   // Damage_Min_Subtotal 
                                                                   Int = 0x00000000,
                                                                   Float = 3.051758E-05f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = 0x00000006,
                                                                   Attribute = GameAttribute.Attributes[0x0092],
                                                                   // Damage_Min_Subtotal 
                                                                   Int = 0x00000000,
                                                                   Float = 3.051758E-05f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = 0x00000000,
                                                                   Attribute = GameAttribute.Attributes[0x0092],
                                                                   // Damage_Min_Subtotal 
                                                                   Int = 0x00000000,
                                                                   Float = 2f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = 0x00000000,
                                                                   Attribute = GameAttribute.Attributes[0x0094],
                                                                   // Damage_Weapon_Delta 
                                                                   Int = 0x00000000,
                                                                   Float = 1f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = 0x00000000,
                                                                   Attribute = GameAttribute.Attributes[0x0095],
                                                                   // Damage_Weapon_Delta_SubTotal 
                                                                   Int = 0x00000000,
                                                                   Float = 1f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = 0x00000000,
                                                                   Attribute = GameAttribute.Attributes[0x0096],
                                                                   // Damage_Weapon_Max 
                                                                   Int = 0x00000000,
                                                                   Float = 3f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = 0x00000000,
                                                                   Attribute = GameAttribute.Attributes[0x0097],
                                                                   // Damage_Weapon_Max_Total 
                                                                   Int = 0x00000000,
                                                                   Float = 3f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = 0x00000000,
                                                                   Attribute = GameAttribute.Attributes[0x0098],
                                                                   // Damage_Weapon_Delta_Total 
                                                                   Int = 0x00000000,
                                                                   Float = 1f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = 0x0000CE11,
                                                                   Attribute = GameAttribute.Attributes[0x027B],
                                                                   // Trait 
                                                                   Int = 0x00000001,
                                                                   Float = 0f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = 0x00000000,
                                                                   Attribute = GameAttribute.Attributes[0x009B],
                                                                   // Damage_Weapon_Min 
                                                                   Int = 0x00000000,
                                                                   Float = 2f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = 0x00000000,
                                                                   Attribute = GameAttribute.Attributes[0x009C],
                                                                   // Damage_Weapon_Min_Total 
                                                                   Int = 0x00000000,
                                                                   Float = 2f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = 0x0000CE11,
                                                                   Attribute = GameAttribute.Attributes[0x0041],
                                                                   // Skill 
                                                                   Int = 0x00000001,
                                                                   Float = 0f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = 0x0000CE11,
                                                                   Attribute = GameAttribute.Attributes[0x0042],
                                                                   // Skill_Total 
                                                                   Int = 0x00000001,
                                                                   Float = 0f,
                                                               },
                                                       },
                                   });

            Client.SendMessage(new AttributesSetValuesMessage()
                                   {
                                       Id = 0x004D,
                                       Field0 = 0x789E00E2,
                                       atKeyVals = new NetAttributeKeyValue[15]
                                                       {
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Attribute = GameAttribute.Attributes[0x005C],
                                                                   // Resource_Type_Primary 
                                                                   Int = Client.Player.Hero.ResourceID,
                                                                   Float = 0f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Attribute = GameAttribute.Attributes[0x0056],
                                                                   // Hitpoints_Max_Total 
                                                                   Int = 0x00000000,
                                                                   Float = 76f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Attribute = GameAttribute.Attributes[0x0054],
                                                                   // Hitpoints_Max 
                                                                   Int = 0x00000000,
                                                                   Float = 40f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Attribute = GameAttribute.Attributes[0x0051],
                                                                   // Hitpoints_Total_From_Level 
                                                                   Int = 0x00000000,
                                                                   Float = 3.051758E-05f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Attribute = GameAttribute.Attributes[0x0050],
                                                                   // Hitpoints_Total_From_Vitality 
                                                                   Int = 0x00000000,
                                                                   Float = 36f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Attribute = GameAttribute.Attributes[0x004F],
                                                                   // Hitpoints_Factor_Vitality 
                                                                   Int = 0x00000000,
                                                                   Float = 4f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Attribute = GameAttribute.Attributes[0x004E],
                                                                   // Hitpoints_Factor_Level 
                                                                   Int = 0x00000000,
                                                                   Float = 4f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Attribute = GameAttribute.Attributes[0x004D],
                                                                   // Hitpoints_Cur 
                                                                   Int = 0x00000000,
                                                                   Float = 76f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Attribute = GameAttribute.Attributes[0x024C],
                                                                   // Disabled 
                                                                   Int = 0x00000001,
                                                                   Float = 0f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Attribute = GameAttribute.Attributes[0x0046],
                                                                   // Loading 
                                                                   Int = 0x00000001,
                                                                   Float = 0f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Attribute = GameAttribute.Attributes[0x0045],
                                                                   // Invulnerable 
                                                                   Int = 0x00000001,
                                                                   Float = 0f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Attribute = GameAttribute.Attributes[0x0043],
                                                                   // TeamID 
                                                                   Int = 0x00000002,
                                                                   Float = 0f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = 0x000FFFFF,
                                                                   Attribute = GameAttribute.Attributes[0x0042],
                                                                   // Skill_Total 
                                                                   Int = 0x00000001,
                                                                   Float = 0f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = 0x000FFFFF,
                                                                   Attribute = GameAttribute.Attributes[0x0041],
                                                                   // Skill 
                                                                   Int = 0x00000001,
                                                                   Float = 0f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = 0x0000CE11,
                                                                   Attribute = GameAttribute.Attributes[0x0230],
                                                                   // Buff_Icon_Count0 
                                                                   Int = 0x00000001,
                                                                   Float = 0f,
                                                               },
                                                       },
                                   });

            Client.SendMessage(new AttributesSetValuesMessage()
                                   {
                                       Id = 0x004D,
                                       Field0 = 0x789E00E2,
                                       atKeyVals = new NetAttributeKeyValue[14]
                                                       {
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Attribute = GameAttribute.Attributes[0x012C],
                                                                   // Hidden 
                                                                   Int = 0x00000001,
                                                                   Float = 0f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Attribute = GameAttribute.Attributes[0x0027],
                                                                   // Level_Cap 
                                                                   Int = 0x0000000D,
                                                                   Float = 0f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Attribute = GameAttribute.Attributes[0x0026],
                                                                   // Level 
                                                                   Int = Client.Player.Hero.Properties.Level,
                                                                   Float = 0f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Attribute = GameAttribute.Attributes[0x0022],
                                                                   // Experience_Next 
                                                                   Int = 0x000004B0,
                                                                   Float = 0f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Attribute = GameAttribute.Attributes[0x0021],
                                                                   // Experience_Granted 
                                                                   Int = 0x000003E8,
                                                                   Float = 0f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Attribute = GameAttribute.Attributes[0x0020],
                                                                   // Armor_Total 
                                                                   Int = 0x00000000,
                                                                   Float = 0f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Attribute = GameAttribute.Attributes[0x000C],
                                                                   // Defense 
                                                                   Int = 0x00000000,
                                                                   Float = 10f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Field0 = 0x00033C40,
                                                                   Attribute = GameAttribute.Attributes[0x0230],
                                                                   // Buff_Icon_Count0 
                                                                   Int = 0x00000001,
                                                                   Float = 0f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Attribute = GameAttribute.Attributes[0x000B],
                                                                   // Vitality 
                                                                   Int = 0x00000000,
                                                                   Float = 9f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Attribute = GameAttribute.Attributes[0x000A],
                                                                   // Precision 
                                                                   Int = 0x00000000,
                                                                   Float = 11f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Attribute = GameAttribute.Attributes[0x0009],
                                                                   // Attack 
                                                                   Int = 0x00000000,
                                                                   Float = 10f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Attribute = GameAttribute.Attributes[0x0008],
                                                                   // Shared_Stash_Slots 
                                                                   Int = 0x0000000E,
                                                                   Float = 0f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Attribute = GameAttribute.Attributes[0x0007],
                                                                   // Backpack_Slots 
                                                                   Int = 0x0000003C,
                                                                   Float = 0f,
                                                               },
                                                           new NetAttributeKeyValue()
                                                               {
                                                                   Attribute = GameAttribute.Attributes[0x0103],
                                                                   // General_Cooldown 
                                                                   Int = 0x00000000,
                                                                   Float = 0f,
                                                               },
                                                       },
                                   });

            Client.SendMessage(new ACDGroupMessage()
                                   {
                                       Id = 0x00B8,
                                       Field0 = 0x789E00E2,
                                       Field1 = -1,
                                       Field2 = -1,
                                   });

            Client.SendMessage(new ANNDataMessage()
                                   {
                                       Id = 0x003E,
                                       Field0 = 0x789E00E2,
                                   });

            Client.SendMessage(new ACDTranslateFacingMessage()
                                   {
                                       Id = 0x0070,
                                       Field0 = 0x789E00E2,
                                       Field1 = 3.022712f,
                                       Field2 = false,
                                   });

            Client.SendMessage(new PlayerEnterKnownMessage()
                                   {
                                       Id = 0x003D,
                                       Field0 = 0x00000000,
                                       Field1 = 0x789E00E2,
                                   });

            Client.SendMessage(new VisualInventoryMessage()
                                   {
                                       Id = 0x004E,
                                       Field0 = 0x789E00E2,
                                       Field1 = new VisualEquipment()
                                                    {
                                                        Field0 = new VisualItem[8]
                                                                     {
                                                                         new VisualItem() //Head
                                                                             {
                                                                                 Field0 =
                                                                                     Client.Player.Hero.Properties.
                                                                                     Equipment.VisualItemList[0].Gbid,
                                                                                 Field1 = 0x00000000,
                                                                                 Field2 = 0x00000000,
                                                                                 Field3 = -1,
                                                                             },
                                                                         new VisualItem() //Chest
                                                                             {
                                                                                 Field0 =
                                                                                     Client.Player.Hero.Properties.
                                                                                     Equipment.VisualItemList[1].Gbid,
                                                                                 Field1 = 0x00000000,
                                                                                 Field2 = 0x00000000,
                                                                                 Field3 = -1,
                                                                             },
                                                                         new VisualItem() //Feet
                                                                             {
                                                                                 Field0 =
                                                                                     Client.Player.Hero.Properties.
                                                                                     Equipment.VisualItemList[2].Gbid,
                                                                                 Field1 = 0x00000000,
                                                                                 Field2 = 0x00000000,
                                                                                 Field3 = -1,
                                                                             },
                                                                         new VisualItem() //Hands
                                                                             {
                                                                                 Field0 =
                                                                                     Client.Player.Hero.Properties.
                                                                                     Equipment.VisualItemList[3].Gbid,
                                                                                 Field1 = 0x00000000,
                                                                                 Field2 = 0x00000000,
                                                                                 Field3 = -1,
                                                                             },
                                                                         new VisualItem() //Main hand
                                                                             {
                                                                                 Field0 =
                                                                                     Client.Player.Hero.Properties.
                                                                                     Equipment.VisualItemList[4].Gbid,
                                                                                 Field1 = 0x00000000,
                                                                                 Field2 = 0x00000000,
                                                                                 Field3 = -1,
                                                                             },
                                                                         new VisualItem() //Offhand
                                                                             {
                                                                                 Field0 =
                                                                                     Client.Player.Hero.Properties.
                                                                                     Equipment.VisualItemList[5].Gbid,
                                                                                 Field1 = 0x00000000,
                                                                                 Field2 = 0x00000000,
                                                                                 Field3 = -1,
                                                                             },
                                                                         new VisualItem() //Shoulders
                                                                             {
                                                                                 Field0 =
                                                                                     Client.Player.Hero.Properties.
                                                                                     Equipment.VisualItemList[6].Gbid,
                                                                                 Field1 = 0x00000000,
                                                                                 Field2 = 0x00000000,
                                                                                 Field3 = -1,
                                                                             },
                                                                         new VisualItem() //Legs
                                                                             {
                                                                                 Field0 =
                                                                                     Client.Player.Hero.Properties.
                                                                                     Equipment.VisualItemList[7].Gbid,
                                                                                 Field1 = 0x00000000,
                                                                                 Field2 = 0x00000000,
                                                                                 Field3 = -1,
                                                                             },
                                                                     },
                                                    },
                                   });

            Client.SendMessage(new PlayerActorSetInitialMessage()
                                   {
                                       Id = 0x0039,
                                       Field0 = 0x789E00E2,
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
                                       Id = 0x004C,
                                       Field0 = 0x789E00E2,
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
