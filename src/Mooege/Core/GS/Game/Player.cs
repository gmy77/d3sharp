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

using System.Linq;
using System.Collections.Generic;
using Mooege.Common;
using Mooege.Core.Common.Toons;
using Mooege.Core.Common.Items;
using Mooege.Core.GS.Game;
using Mooege.Core.GS.Map;
using Mooege.Core.GS.Actors;
using Mooege.Core.GS.Skills;
using Mooege.Net.GS;
using Mooege.Net.GS.Message;
using Mooege.Net.GS.Message.Fields;
using Mooege.Net.GS.Message.Definitions.ACD;
using Mooege.Net.GS.Message.Definitions.Act;
using Mooege.Net.GS.Message.Definitions.Attribute;
using Mooege.Net.GS.Message.Definitions.Connection;
using Mooege.Net.GS.Message.Definitions.Game;
using Mooege.Net.GS.Message.Definitions.Hero;
using Mooege.Net.GS.Message.Definitions.Misc;
using Mooege.Net.GS.Message.Definitions.Player;
using Mooege.Net.GS.Message.Definitions.Skill;
using Mooege.Net.GS.Message.Definitions.Inventory;

// NOTE: Merged Hero into Player since dumped structures imply that they are the same thing

namespace Mooege.Core.GS.Game
{
    public class Player : Actor
    {
        private static readonly Logger Logger = LogManager.CreateLogger();

        public override ActorType ActorType { get { return ActorType.Player; } }

        public GameClient InGameClient { get; set; }

        public Toon Properties { get; private set; }

        public SkillSet SkillSet;
        public Inventory Inventory;

        public Dictionary<uint, World> RevealedWorlds;
        public Dictionary<uint, Scene> RevealedScenes; // Will have to be a list if scene IDs are per-world
        public Dictionary<uint, Actor> RevealedActors;

        public Player(World world, GameClient client, Toon bnetToon)
            : base(world, world.Game.NewPlayerID)
        {
            this.Game.AddPlayer(this);
            this.World.AddPlayer(this);
            this.InGameClient = client;

            this.Properties = bnetToon;
            this.Inventory = new Inventory(this);
            this.SkillSet = new Skills.SkillSet(this.Properties.Class);

            RevealedWorlds = new Dictionary<uint, World>();
            RevealedScenes = new Dictionary<uint, Scene>();
            RevealedActors = new Dictionary<uint, Actor>();

            // actor values
            this.AppearanceSNO = this.ClassSNO;
            this.Field2 = 0x00000009;
            this.Field3 = 0x00000000;
            this.Scale = ModelScale;
            this.RotationAmount = 0.05940768f;
            this.RotationAxis = new Vector3D(0f, 0f, 0.9982339f);

            this.Position.X = 3143.75f;
            this.Position.Y = 2828.75f;
            this.Position.Z = 59.075588f;

            //den of evil: this.Position.X = 2526.250000f; this.Position.Y = 2098.750000f; this.Position.Z = -5.381495f;
            //inn: this.Position.X = 2996.250000f; this.Position.Y = 2793.750000f; this.Position.Z = 24.045330f;
            // adrias hut: this.Position.X = 1768.750000f; this.Position.Y = 2921.250000f; this.Position.Z = 20.333143f;
            // cemetry of forsaken: this.Position.X = 2041.250000f; this.Position.Y = 1778.750000f; this.Position.Z = 0.426203f;
            //defiled crypt level 2: this.WorldId = 2000289804; this.Position.X = 158.750000f; this.Position.Y = 76.250000f; this.Position.Z = 0.100000f;

            this.GBHandle = new GBHandle()
            {
                Type = (int)GBHandleType.Player,
                GBID = this.Properties.ClassID,
            };

            this.Field7 = -1;
            this.Field8 = -1;
            this.Field9 = 0x00000000;
            this.Field10 = 0x0;
        }

        public void Consume(GameClient client, GameMessage message)
        {
            if (message is AssignActiveSkillMessage) OnAssignActiveSkill(client, (AssignActiveSkillMessage)message);
            else if (message is AssignPassiveSkillMessage) OnAssignPassiveSkill(client, (AssignPassiveSkillMessage)message);
            else if (message is PlayerChangeHotbarButtonMessage) OnPlayerChangeHotbarButtonMessage(client, (PlayerChangeHotbarButtonMessage)message);
            else return;

            UpdateState();
            client.FlushOutgoingBuffer();
        }

        /// <summary>
        /// Greets the player and sends the client initial data it needs to get in-game.
        /// </summary>
        /// <param name="message"></param>
        public void Greet(JoinBNetGameMessage message)
        {
            Logger.Trace("Greeting player {0} and positioning him to {1}", this.Properties.Name, this.Position);

            // send versions message
            InGameClient.SendMessageNow(new VersionsMessage(message.SNOPackHash));

            // send connection established message.
            InGameClient.SendMessage(new ConnectionEstablishedMessage
            {
                Field0 = 0x00000000,
                Field1 = 0x4BB91A16,
                SNOPackHash = message.SNOPackHash,
            });

            // game setup message.
            InGameClient.SendMessage(new GameSetupMessage
            {
                Field0 = 0x00000077,
            });

            InGameClient.SendMessage(new SavePointInfoMessage
            {
                snoLevelArea = -1,
            });

            InGameClient.SendMessage(new HearthPortalInfoMessage
            {
                snoLevelArea = -1,
                Field1 = -1,
            });

            // transition player to act so client can load act related data? /raist
            InGameClient.SendMessage(new ActTransitionMessage
            {
                Field0 = 0x00000000,
                Field1 = true,
            });

            //reveal world to the toon
            if (this.World != null)
                this.World.Reveal(this);

            // send newplayermessage.
            InGameClient.SendMessage(new NewPlayerMessage
            {
                Field0 = 0x00000000, //Party frame (0x00000000 hide, 0x00000001 show)
                Field1 = "", //Owner name?
                ToonName = this.Properties.Name,
                Field3 = 0x00000002, //party frame class
                Field4 = 0x00000004, //party frame level
                snoActorPortrait = this.ClassSNO, //party frame portrait
                Field6 = 0x00000001,
                StateData = this.GetStateData(),
                Field8 = false, //announce party join
                Field9 = 0x00000001,
                ActorID = this.DynamicID,
            });

            // reveal the hero
            Reveal(this);

            InGameClient.SendMessage(new ACDCollFlagsMessage
            {
                ActorID = this.DynamicID,
                CollFlags = 0x00000000,
            });

            GameAttributeMap attribs = new GameAttributeMap();
            attribs[GameAttribute.SkillKit] = this.SkillKit;
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
            attribs[GameAttribute.Resource_Cur, this.ResourceID] = 200f;
            attribs[GameAttribute.Resource_Max, this.ResourceID] = 200f;
            attribs[GameAttribute.Resource_Max_Total, this.ResourceID] = 200f;
            attribs[GameAttribute.Damage_Weapon_Min_Total_All] = 2f;
            attribs[GameAttribute.Damage_Weapon_Delta_Total_All] = 1f;
            attribs[GameAttribute.Resource_Regen_Total, this.ResourceID] = 3.051758E-05f;
            //--
            attribs[GameAttribute.Resource_Effective_Max, this.ResourceID] = 200f;
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
            attribs[GameAttribute.Resource_Type_Primary] = this.ResourceID;
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
            attribs[GameAttribute.Level] = this.Properties.Level;
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

            attribs.SendMessage(InGameClient, this.DynamicID);

            InGameClient.SendMessage(new ACDGroupMessage()
            {
                ActorID = this.DynamicID,
                Field1 = -1,
                Field2 = -1,
            });

            InGameClient.SendMessage(new ANNDataMessage(Opcodes.ANNDataMessage1)
            {
                ActorID = this.DynamicID,
            });

            InGameClient.SendMessage(new ACDTranslateFacingMessage(Opcodes.ACDTranslateFacingMessage1)
            {
                ActorID = this.DynamicID,
                Angle = 3.022712f,
                Field2 = false,
            });

            InGameClient.SendMessage(new PlayerEnterKnownMessage()
            {
                Field0 = 0x00000000,
                PlayerID = this.DynamicID,
            });

            InGameClient.SendMessage(new PlayerActorSetInitialMessage()
            {
                PlayerID = this.DynamicID,
                Field1 = 0x00000000,
            });

            InGameClient.SendMessage(new SNONameDataMessage()
            {
                Name = new SNOName()
                {
                    Group = 0x00000001,
                    Handle = this.ClassSNO,
                },
            });

            InGameClient.FlushOutgoingBuffer();

            InGameClient.SendMessage(new DWordDataMessage() // TICK
            {
                Id = 0x0089,
                Field0 = 0x00000077,
            });

            InGameClient.FlushOutgoingBuffer();

            attribs = new GameAttributeMap();
            attribs[GameAttribute.Hitpoints_Healed_Target] = 76f;
            attribs.SendMessage(InGameClient, this.DynamicID);

            InGameClient.SendMessage(new DWordDataMessage() // TICK
            {
                Id = 0x0089,
                Field0 = 0x0000007D,
            });

            InGameClient.FlushOutgoingBuffer();
        }

        private void OnPlayerChangeHotbarButtonMessage(GameClient client, PlayerChangeHotbarButtonMessage message)
        {
            this.SkillSet.HotBarSkills[message.BarIndex] = message.ButtonData;
        }

        private void OnAssignPassiveSkill(GameClient client, AssignPassiveSkillMessage message)
        {
            this.SkillSet.PassiveSkills[message.SkillIndex] = message.SNOSkill;
        }

        private void OnAssignActiveSkill(GameClient client, AssignActiveSkillMessage message)
        {
            var oldSNOSkill = this.SkillSet.ActiveSkills[message.SkillIndex]; // find replaced skills SNO.

            foreach (HotbarButtonData button in this.SkillSet.HotBarSkills.Where(button => button.SNOSkill == oldSNOSkill)) // loop through hotbar and replace the old skill with new one
            {
                button.SNOSkill = message.SNOSkill;
            }

            this.SkillSet.ActiveSkills[message.SkillIndex] = message.SNOSkill;
        }

        public void UpdateState()
        {
            this.InGameClient.SendMessage(new HeroStateMessage
            {
                State = this.GetStateData()
            });

            this.InGameClient.PacketId += 10 * 2;
            this.InGameClient.SendMessage(new DWordDataMessage()
            {
                Id = 0x89,
                Field0 = this.InGameClient.PacketId,
            });
        }

        public HeroStateData GetStateData()
        {
            return new HeroStateData()
            {
                Field0 = 0x00000000,
                Field1 = 0x00000000,
                Field2 = 0x00000000,
                Gender = Properties.Gender,
                PlayerSavedData = this.GetSavedData(),
                Field5 = 0x00000000,
                tQuestRewardHistory = QuestRewardHistory,
            };
        }

        private PlayerSavedData GetSavedData()
        {
            return new PlayerSavedData()
            {
                HotBarButtons = this.SkillSet.HotBarSkills,
                SkilKeyMappings = this.SkillKeyMappings,

                Field2 = 0x00000000,
                Field3 = 0x00000001,

                Field4 = new HirelingSavedData()
                {
                    HirelingInfos = this.HirelingInfo,
                    Field1 = 0x00000000,
                    Field2 = 0x00000000,
                },

                Field5 = 0x00000000,

                LearnedLore = this.LearnedLore,
                snoActiveSkills = this.SkillSet.ActiveSkills,
                snoTraits = this.SkillSet.PassiveSkills,
                Field9 = new SavePointData() { snoWorld = -1, Field1 = -1, },
                m_SeenTutorials = this.SeenTutorials,
            };
        }

        public VisualInventoryMessage GetVisualInventory()
        {
            return new VisualInventoryMessage
            {
                ActorID = this.DynamicID,
                EquipmentList =
                    new VisualEquipment
                    {
                        Equipment =
                            Properties.Equipment.VisualItemList.Select(
                                equipment =>
                                new VisualItem
                                {
                                    GbId = equipment.Gbid,
                                    Field1 = 0x0,
                                    Field2 = 0x0,
                                    Field3 = -1
                                }).ToArray()
                    }
            };
        }

        public int ClassSNO
        {
            get
            {
                if (this.Properties.Gender == 0)
                {
                    switch (this.Properties.Class)
                    {
                        case ToonClass.Barbarian:
                            return 0x0CE5;
                        case ToonClass.DemonHunter:
                            return 0x0125C7;
                        case ToonClass.Monk:
                            return 0x1271;
                        case ToonClass.WitchDoctor:
                            return 0x1955;
                        case ToonClass.Wizard:
                            return 0x1990;
                    }
                }
                else
                {
                    switch (this.Properties.Class)
                    {
                        case ToonClass.Barbarian:
                            return 0x0CD5;
                        case ToonClass.DemonHunter:
                            return 0x0123D2;
                        case ToonClass.Monk:
                            return 0x126D;
                        case ToonClass.WitchDoctor:
                            return 0x1951;
                        case ToonClass.Wizard:
                            return 0x197E;
                    }
                }
                return 0x0;
            }
        }

        public float ModelScale
        {
            get
            {
                //dummy values, need confirmation from dump
                switch (this.Properties.Class)
                {
                    case ToonClass.Barbarian:
                        return 1.22f;
                    case ToonClass.DemonHunter:
                        return 1.43f;
                    case ToonClass.Monk:
                        return 1.43f;
                    case ToonClass.WitchDoctor:
                        return 1.43f;
                    case ToonClass.Wizard:
                        return 1.43f;
                }
                return 1.43f;
            }
        }

        public int ResourceID
        {
            get
            {
                switch (this.Properties.Class)
                {
                    case ToonClass.Barbarian:
                        return 0x00000002;
                    case ToonClass.DemonHunter:
                        return 0x00000005;
                    case ToonClass.Monk:
                        return 0x00000003;
                    case ToonClass.WitchDoctor:
                        return 0x00000000;
                    case ToonClass.Wizard:
                        return 0x00000001;
                }
                return 0x00000000;
            }
        }

        public int SkillKit
        {
            get
            {
                switch (this.Properties.Class)
                {
                    case ToonClass.Barbarian:
                        return 0x00008AF4;
                    case ToonClass.DemonHunter:
                        return 0x00008AFC;
                    case ToonClass.Monk:
                        return 0x00008AFA;
                    case ToonClass.WitchDoctor:
                        return 0x00008AFF;
                    case ToonClass.Wizard:
                        return 0x00008B00;
                }
                return 0x00000001;
            }
        }

        public SkillKeyMapping[] SkillKeyMappings = new SkillKeyMapping[15]
        {
            new SkillKeyMapping { Power = -1, Field1 = -1, Field2 = 0x00000000, },
            new SkillKeyMapping { Power = -1, Field1 = -1, Field2 = 0x00000000, },
            new SkillKeyMapping { Power = -1, Field1 = -1, Field2 = 0x00000000, },
            new SkillKeyMapping { Power = -1, Field1 = -1, Field2 = 0x00000000, },
            new SkillKeyMapping { Power = -1, Field1 = -1, Field2 = 0x00000000, },
            new SkillKeyMapping { Power = -1, Field1 = -1, Field2 = 0x00000000, },
            new SkillKeyMapping { Power = -1, Field1 = -1, Field2 = 0x00000000, },
            new SkillKeyMapping { Power = -1, Field1 = -1, Field2 = 0x00000000, },
            new SkillKeyMapping { Power = -1, Field1 = -1, Field2 = 0x00000000, },
            new SkillKeyMapping { Power = -1, Field1 = -1, Field2 = 0x00000000, },
            new SkillKeyMapping { Power = -1, Field1 = -1, Field2 = 0x00000000, },
            new SkillKeyMapping { Power = -1, Field1 = -1, Field2 = 0x00000000, },
            new SkillKeyMapping { Power = -1, Field1 = -1, Field2 = 0x00000000, },
            new SkillKeyMapping { Power = -1, Field1 = -1, Field2 = 0x00000000, },
            new SkillKeyMapping { Power = -1, Field1 = -1, Field2 = 0x00000000, },
        };

        public LearnedLore LearnedLore = new LearnedLore()
        {
            Field0 = 0x00000000,
            m_snoLoreLearned = new int[256]
             {
                0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,
                0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,
                0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,
                0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,
                0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,
                0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,
                0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,
                0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,
                0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,
                0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,
                0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,
                0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,
                0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,
                0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,
                0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,
                0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,
                0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,
                0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,
                0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,
                0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,
                0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,
                0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,
                0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,
                0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,
                0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,
                0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,
                0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,
                0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,
                0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,
                0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,
                0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,
                0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000
             },
        };

        public int[] SeenTutorials = new int[64]
        {
            -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
        };

        public PlayerQuestRewardHistoryEntry[] QuestRewardHistory = new PlayerQuestRewardHistoryEntry[0] { };

        public HirelingInfo[] HirelingInfo = new HirelingInfo[4]
        {
            new HirelingInfo { Field0 = 0x00000000, Field1 = -1, Field2 = 0x00000000, Field3 = 0x00000000, Field4 = false, Field5 = -1, Field6 = -1, Field7 = -1, Field8 = -1, },
            new HirelingInfo { Field0 = 0x00000000, Field1 = -1, Field2 = 0x00000000, Field3 = 0x00000000, Field4 = false, Field5 = -1, Field6 = -1, Field7 = -1, Field8 = -1, },
            new HirelingInfo { Field0 = 0x00000000, Field1 = -1, Field2 = 0x00000000, Field3 = 0x00000000, Field4 = false, Field5 = -1, Field6 = -1, Field7 = -1, Field8 = -1, },
            new HirelingInfo { Field0 = 0x00000000, Field1 = -1, Field2 = 0x00000000, Field3 = 0x00000000, Field4 = false, Field5 = -1, Field6 = -1, Field7 = -1, Field8 = -1, },
        };
    }
}
