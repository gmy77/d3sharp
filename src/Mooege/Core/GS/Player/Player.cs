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
using System.Threading;
using Mooege.Common;
using Mooege.Core.Common.Toons;
using Mooege.Core.Common.Items;
using Mooege.Core.GS.Objects;
using Mooege.Core.GS.Map;
using Mooege.Core.GS.Actors;
using Mooege.Core.GS.Skills;
using Mooege.Net.GS;
using Mooege.Net.GS.Message;
using Mooege.Net.GS.Message.Definitions.ACD;
using Mooege.Net.GS.Message.Definitions.Actor;
using Mooege.Net.GS.Message.Definitions.Misc;
using Mooege.Net.GS.Message.Definitions.World;
using Mooege.Net.GS.Message.Fields;
using Mooege.Net.GS.Message.Definitions.Hero;
using Mooege.Net.GS.Message.Definitions.Player;
using Mooege.Net.GS.Message.Definitions.Skill;
using Mooege.Net.GS.Message.Definitions.Effect;


// TODO: When the player moves, it will set the Position property which will bounce back to the player again.
//       That is unnecessary and we should exclude the player from receiving it in that case. /komiga

namespace Mooege.Core.GS.Player
{
    public class Player : Actor, IMessageConsumer
    {
        private static readonly Logger Logger = LogManager.CreateLogger();

        public override ActorType ActorType { get { return ActorType.Player; } }
        
        public GameClient InGameClient { get; set; }

        public int PlayerIndex { get; private set; } 

        public Toon Properties { get; private set; }
        public SkillSet SkillSet;
        public Inventory Inventory;

        public Dictionary<uint, IRevealable> RevealedObjects { get; private set; }

        // Collection of items that only the player can see. This is only used when items drop from killing an actor
        // TODO: Might want to just have a field on the item itself to indicate whether it is visible to only one player
        public Dictionary<uint, Item> GroundItems { get; private set; }

        public Player(World world, GameClient client, Toon bnetToon)
            : base(world, world.NewPlayerID)
        {
            this.InGameClient = client;
            this.PlayerIndex = Interlocked.Increment(ref this.InGameClient.Game.PlayerIndexCounter); // make it atomic.

            this.Properties = bnetToon;
            this.Inventory = new Inventory(this);
            this.SkillSet = new SkillSet(this.Properties.Class);

            this.RevealedObjects = new Dictionary<uint, IRevealable>();
            this.GroundItems = new Dictionary<uint, Item>();

            // actor values
            this.ActorSNO = this.ClassSNO;
            this.Field2 = 0x00000009;
            this.Field3 = 0x00000000;
            this.Scale = ModelScale;
            this.RotationAmount = 0.05940768f;
            this.RotationAxis = new Vector3D(0f, 0f, 0.9982339f);
            this.CollFlags = 0x00000000;

            this.Position.X = 3143.75f;
            this.Position.Y = 2828.75f;
            this.Position.Z = 59.075588f;

            // den of evil: this.Position.X = 2526.250000f; this.Position.Y = 2098.750000f; this.Position.Z = -5.381495f;
            // inn: this.Position.X = 2996.250000f; this.Position.Y = 2793.750000f; this.Position.Z = 24.045330f;
            // adrias hut: this.Position.X = 1768.750000f; this.Position.Y = 2921.250000f; this.Position.Z = 20.333143f;
            // cemetery of the forsaken: this.Position.X = 2041.250000f; this.Position.Y = 1778.750000f; this.Position.Z = 0.426203f;
            // defiled crypt level 2: this.WorldId = 2000289804; this.Position.X = 158.750000f; this.Position.Y = 76.250000f; this.Position.Z = 0.100000f;

            this.GBHandle.Type = (int)GBHandleType.Player;
            this.GBHandle.GBID = this.Properties.ClassID;

            this.Field7 = -1;
            this.Field8 = -1;
            this.Field9 = 0x00000000;
            this.Field10 = 0x0;

            #region Attributes
            this.Attributes[GameAttribute.SkillKit] = this.SkillKit;
            this.Attributes[GameAttribute.Buff_Active, 0x33C40] = true;
            this.Attributes[GameAttribute.Skill, 0x7545] = 1;
            this.Attributes[GameAttribute.Skill_Total, 0x7545] = 1;
            this.Attributes[GameAttribute.Resistance_Total, 0x226] = 0.5f;
            this.Attributes[GameAttribute.Resistance, 0x226] = 0.5f;
            this.Attributes[GameAttribute.Immobolize] = true;
            this.Attributes[GameAttribute.Untargetable] = true;
            this.Attributes[GameAttribute.Skill_Total, 0x76B7] = 1;
            this.Attributes[GameAttribute.Skill, 0x76B7] = 1;
            this.Attributes[GameAttribute.Skill, 0x6DF] = 1;
            this.Attributes[GameAttribute.Buff_Active, 0xCE11] = true;
            this.Attributes[GameAttribute.CantStartDisplayedPowers] = true;
            this.Attributes[GameAttribute.Skill_Total, 0x216FA] = 1;
            this.Attributes[GameAttribute.Skill, 0x176C4] = 1;
            this.Attributes[GameAttribute.Skill, 0x216FA] = 1;
            this.Attributes[GameAttribute.Skill_Total, 0x176C4] = 1;
            this.Attributes[GameAttribute.Skill_Total, 0x6DF] = 1;
            this.Attributes[GameAttribute.Resistance, 0xDE] = 0.5f;
            this.Attributes[GameAttribute.Resistance_Total, 0xDE] = 0.5f;
            this.Attributes[GameAttribute.Get_Hit_Recovery] = 6f;
            this.Attributes[GameAttribute.Get_Hit_Recovery_Per_Level] = 1f;
            this.Attributes[GameAttribute.Get_Hit_Recovery_Base] = 5f;
            this.Attributes[GameAttribute.Skill, 0x7780] = 1;
            this.Attributes[GameAttribute.Get_Hit_Max] = 60f;
            this.Attributes[GameAttribute.Skill_Total, 0x7780] = 1;
            this.Attributes[GameAttribute.Get_Hit_Max_Per_Level] = 10f;
            this.Attributes[GameAttribute.Get_Hit_Max_Base] = 50f;
            this.Attributes[GameAttribute.Resistance_Total, 0] = 3.051758E-05f; // im pretty sure key = 0 doesnt do anything since the lookup is (attributeId | (key << 12)), maybe this is some base resistance? /cm
            this.Attributes[GameAttribute.Resistance_Total, 1] = 3.051758E-05f;
            this.Attributes[GameAttribute.Resistance_Total, 2] = 3.051758E-05f;
            this.Attributes[GameAttribute.Resistance_Total, 3] = 3.051758E-05f;
            this.Attributes[GameAttribute.Resistance_Total, 4] = 3.051758E-05f;
            this.Attributes[GameAttribute.Resistance_Total, 5] = 3.051758E-05f;
            this.Attributes[GameAttribute.Resistance_Total, 6] = 3.051758E-05f;
            this.Attributes[GameAttribute.Dodge_Rating_Total] = 3.051758E-05f;
            this.Attributes[GameAttribute.IsTrialActor] = true;
            this.Attributes[GameAttribute.Buff_Visual_Effect, 0xFFFFF] = true;
            this.Attributes[GameAttribute.Crit_Percent_Cap] = 0x3F400000;
            this.Attributes[GameAttribute.Resource_Cur, this.ResourceID] = 200f;
            this.Attributes[GameAttribute.Resource_Max, this.ResourceID] = 200f;
            this.Attributes[GameAttribute.Resource_Max_Total, this.ResourceID] = 200f;
            this.Attributes[GameAttribute.Damage_Weapon_Min_Total_All] = 2f;
            this.Attributes[GameAttribute.Damage_Weapon_Delta_Total_All] = 1f;
            this.Attributes[GameAttribute.Resource_Regen_Total, this.ResourceID] = 3.051758E-05f;
            this.Attributes[GameAttribute.Resource_Effective_Max, this.ResourceID] = 200f;
            this.Attributes[GameAttribute.Damage_Min_Subtotal, 0xFFFFF] = 3.051758E-05f;
            this.Attributes[GameAttribute.Damage_Min_Total, 0xFFFFF] = 3.051758E-05f;
            this.Attributes[GameAttribute.Damage_Weapon_Min_Total_CurrentHand, 0xFFFFF] = 3.051758E-05f;
            this.Attributes[GameAttribute.Attacks_Per_Second_Item_CurrentHand] = 1.199219f;
            this.Attributes[GameAttribute.Attacks_Per_Second_Item_Total_MainHand] = 1.199219f;
            this.Attributes[GameAttribute.Attacks_Per_Second_Total] = 1.199219f;
            this.Attributes[GameAttribute.Attacks_Per_Second] = 1f;
            this.Attributes[GameAttribute.Attacks_Per_Second_Item_MainHand] = 1.199219f;
            this.Attributes[GameAttribute.Attacks_Per_Second_Item_Total] = 1.199219f;
            this.Attributes[GameAttribute.Buff_Icon_End_Tick0, 0x00033C40] = 0x000003FB;
            this.Attributes[GameAttribute.Attacks_Per_Second_Item_Subtotal] = 3.051758E-05f;
            this.Attributes[GameAttribute.Attacks_Per_Second_Item] = 3.051758E-05f;
            this.Attributes[GameAttribute.Buff_Icon_Start_Tick0, 0x00033C40] = 0x00000077;
            this.Attributes[GameAttribute.Hit_Chance] = 1f;
            this.Attributes[GameAttribute.Casting_Speed_Total] = 1f;
            this.Attributes[GameAttribute.Casting_Speed] = 1f;
            this.Attributes[GameAttribute.Movement_Scalar_Total] = 1f;
            this.Attributes[GameAttribute.Skill_Total, 0x0002EC66] = 0;
            this.Attributes[GameAttribute.Movement_Scalar_Capped_Total] = 1f;
            this.Attributes[GameAttribute.Movement_Scalar_Subtotal] = 1f;
            this.Attributes[GameAttribute.Strafing_Rate_Total] = 3.051758E-05f;
            this.Attributes[GameAttribute.Sprinting_Rate_Total] = 3.051758E-05f;
            this.Attributes[GameAttribute.Running_Rate_Total] = 0.3598633f;
            this.Attributes[GameAttribute.Damage_Weapon_Min_Total_MainHand, 0] = 2f;
            this.Attributes[GameAttribute.Walking_Rate_Total] = 0.2797852f;
            this.Attributes[GameAttribute.Damage_Weapon_Delta_Total_MainHand, 0] = 1f;
            this.Attributes[GameAttribute.Damage_Delta_Total, 1] = 3.051758E-05f;
            this.Attributes[GameAttribute.Damage_Delta_Total, 2] = 3.051758E-05f;
            this.Attributes[GameAttribute.Damage_Delta_Total, 3] = 3.051758E-05f;
            this.Attributes[GameAttribute.Damage_Delta_Total, 4] = 3.051758E-05f;
            this.Attributes[GameAttribute.Damage_Delta_Total, 5] = 3.051758E-05f;
            this.Attributes[GameAttribute.Damage_Delta_Total, 6] = 3.051758E-05f;
            this.Attributes[GameAttribute.Damage_Delta_Total, 0] = 1f;
            this.Attributes[GameAttribute.Running_Rate] = 0.3598633f;
            this.Attributes[GameAttribute.Damage_Weapon_Min_Total_CurrentHand, 1] = 3.051758E-05f;
            this.Attributes[GameAttribute.Damage_Weapon_Min_Total_CurrentHand, 2] = 3.051758E-05f;
            this.Attributes[GameAttribute.Damage_Weapon_Min_Total_CurrentHand, 3] = 3.051758E-05f;
            this.Attributes[GameAttribute.Damage_Weapon_Min_Total_CurrentHand, 4] = 3.051758E-05f;
            this.Attributes[GameAttribute.Damage_Weapon_Min_Total_CurrentHand, 5] = 3.051758E-05f;
            this.Attributes[GameAttribute.Damage_Weapon_Min_Total_CurrentHand, 6] = 3.051758E-05f;
            this.Attributes[GameAttribute.Damage_Weapon_Min_Total_CurrentHand, 0] = 2f;
            this.Attributes[GameAttribute.Walking_Rate] = 0.2797852f;
            this.Attributes[GameAttribute.Damage_Min_Total, 1] = 3.051758E-05f;
            this.Attributes[GameAttribute.Damage_Min_Total, 2] = 3.051758E-05f;
            this.Attributes[GameAttribute.Damage_Min_Total, 3] = 3.051758E-05f;
            this.Attributes[GameAttribute.Damage_Min_Total, 4] = 3.051758E-05f;
            this.Attributes[GameAttribute.Damage_Min_Total, 5] = 3.051758E-05f;
            this.Attributes[GameAttribute.Damage_Min_Total, 6] = 3.051758E-05f;
            this.Attributes[GameAttribute.Damage_Weapon_Delta_Total_CurrentHand, 1] = 3.051758E-05f;
            this.Attributes[GameAttribute.Damage_Weapon_Delta_Total_CurrentHand, 2] = 3.051758E-05f;
            this.Attributes[GameAttribute.Damage_Weapon_Delta_Total_CurrentHand, 3] = 3.051758E-05f;
            this.Attributes[GameAttribute.Damage_Weapon_Delta_Total_CurrentHand, 4] = 3.051758E-05f;
            this.Attributes[GameAttribute.Damage_Weapon_Delta_Total_CurrentHand, 5] = 3.051758E-05f;
            this.Attributes[GameAttribute.Damage_Weapon_Delta_Total_CurrentHand, 6] = 3.051758E-05f;
            this.Attributes[GameAttribute.Damage_Min_Total, 0] = 2f;
            this.Attributes[GameAttribute.Damage_Weapon_Delta_Total_CurrentHand, 0] = 1f;
            this.Attributes[GameAttribute.Movement_Scalar] = 1f;
            this.Attributes[GameAttribute.Damage_Min_Subtotal, 1] = 3.051758E-05f;
            this.Attributes[GameAttribute.Damage_Min_Subtotal, 2] = 3.051758E-05f;
            this.Attributes[GameAttribute.Damage_Min_Subtotal, 3] = 3.051758E-05f;
            this.Attributes[GameAttribute.Damage_Min_Subtotal, 4] = 3.051758E-05f;
            this.Attributes[GameAttribute.Damage_Min_Subtotal, 5] = 3.051758E-05f;
            this.Attributes[GameAttribute.Damage_Min_Subtotal, 6] = 3.051758E-05f;
            this.Attributes[GameAttribute.Damage_Min_Subtotal, 0] = 2f;
            this.Attributes[GameAttribute.Damage_Weapon_Delta, 0] = 1f;
            this.Attributes[GameAttribute.Damage_Weapon_Delta_SubTotal, 0] = 1f;
            this.Attributes[GameAttribute.Damage_Weapon_Max, 0] = 3f;
            this.Attributes[GameAttribute.Damage_Weapon_Max_Total, 0] = 3f;
            this.Attributes[GameAttribute.Damage_Weapon_Delta_Total, 0] = 1f;
            this.Attributes[GameAttribute.Trait, 0x0000CE11] = 1;
            this.Attributes[GameAttribute.Damage_Weapon_Min, 0] = 2f;
            this.Attributes[GameAttribute.Damage_Weapon_Min_Total, 0] = 2f;
            this.Attributes[GameAttribute.Skill, 0x0000CE11] = 1;
            this.Attributes[GameAttribute.Skill_Total, 0x0000CE11] = 1;
            this.Attributes[GameAttribute.Resource_Type_Primary] = this.ResourceID;
            this.Attributes[GameAttribute.Hitpoints_Max_Total] = 76f;
            this.Attributes[GameAttribute.Hitpoints_Max] = 40f;
            this.Attributes[GameAttribute.Hitpoints_Total_From_Level] = 3.051758E-05f;
            this.Attributes[GameAttribute.Hitpoints_Total_From_Vitality] = 36f;
            this.Attributes[GameAttribute.Hitpoints_Factor_Vitality] = 4f;
            this.Attributes[GameAttribute.Hitpoints_Factor_Level] = 4f;
            this.Attributes[GameAttribute.Hitpoints_Cur] = 76f;
            //this.Attributes[GameAttribute.Disabled] = true;
            //this.Attributes[GameAttribute.Loading] = true;
            //this.Attributes[GameAttribute.Invulnerable] = true;
            this.Attributes[GameAttribute.TeamID] = 2;
            this.Attributes[GameAttribute.Skill_Total, 0xFFFFF] = 1;
            this.Attributes[GameAttribute.Skill, 0xFFFFF] = 1;
            this.Attributes[GameAttribute.Buff_Icon_Count0, 0x0000CE11] = 1;
            this.Attributes[GameAttribute.Hidden] = false;
            this.Attributes[GameAttribute.Level_Cap] = 13;
            this.Attributes[GameAttribute.Level] = this.Properties.Level;
            this.Attributes[GameAttribute.Experience_Next] = 1200;
            this.Attributes[GameAttribute.Experience_Granted] = 1000;
            this.Attributes[GameAttribute.Armor_Total] = 0;
            this.Attributes[GameAttribute.Defense] = 10.0f;
            this.Attributes[GameAttribute.Buff_Icon_Count0, 0x00033C40] = 1;
            this.Attributes[GameAttribute.Vitality] = 9.0f;
            this.Attributes[GameAttribute.Precision] = 11.0f;
            this.Attributes[GameAttribute.Attack] = 10.0f;
            this.Attributes[GameAttribute.Shared_Stash_Slots] = 14;
            this.Attributes[GameAttribute.Backpack_Slots] = 60;
            this.Attributes[GameAttribute.General_Cooldown] = 0;
            #endregion // Attributes            
        }

        public void Consume(GameClient client, GameMessage message)
        {
            if (message is AssignActiveSkillMessage) OnAssignActiveSkill(client, (AssignActiveSkillMessage)message);
            else if (message is AssignPassiveSkillMessage) OnAssignPassiveSkill(client, (AssignPassiveSkillMessage)message);
            else if (message is PlayerChangeHotbarButtonMessage) OnPlayerChangeHotbarButtonMessage(client, (PlayerChangeHotbarButtonMessage)message);
            else if (message is TargetMessage) OnObjectTargeted(client, (TargetMessage)message);
            else if (message is PlayerMovementMessage) OnPlayerMovement(client, (PlayerMovementMessage)message);
            else return;

            UpdateState();
        }

        public override void Update()
        {
            this.InGameClient.SendTick(); // if there's available messages to send, will handle ticking and flush the outgoing buffer.
        }

        private void OnPlayerMovement(GameClient client, PlayerMovementMessage message)
        {
            // here we should also be checking the position and see if it's valid. If not we should be resetting player to a good position with ACDWorldPositionMessage 
            // so we can have a basic precaution for hacks & exploits /raist.

            if (message.Position != null)
                this.Position = message.Position; 

            var msg = new NotifyActorMovementMessage
                             {
                                 ActorId = message.ActorId,
                                 Position = this.Position,
                                 Angle = message.Angle,
                                 Field3 = false,
                                 Field4 = message.Field4,
                                 Field5 = message.Field5,
                                 Field6 = message.Field6
                             };

            this.World.BroadcastExclusive(msg, this); // TODO: We should be instead notifying currentscene we're in. /raist.

            this.CollectGold();
        }

        private void CollectGold()
        {
            var actorList = this.World.GetActorsInRange(this.Position.X, this.Position.Y, this.Position.Z, 20f);
            foreach (var actor in actorList)
            {
                Item item;
                if (!this.GroundItems.TryGetValue(actor.DynamicID, out item) || item.ItemType != ItemType.Gold) continue;

                this.InGameClient.SendMessage(new FloatingAmountMessage()
                {
                    Place = new WorldPlace()
                    {
                        Position = this.Position,
                        WorldID = this.World.DynamicID,
                    },

                    Amount = item.Attributes[GameAttribute.Gold],
                    Type = FloatingAmountMessage.FloatType.Gold,
                });

                // NOTE: ANNDataMessage6 is probably "AddToInventory"
                this.InGameClient.SendMessage(new ANNDataMessage(Opcodes.ANNDataMessage6)
                {
                    ActorID = actor.DynamicID,
                });

                this.Inventory.PickUpGold(actor.DynamicID);

                this.GroundItems.Remove(actor.DynamicID); // should delete from World also
            }
        }

        // FIXME: Hardcoded crap
        public override void OnEnter(World world)
        {
            this.World.Reveal(this);

            // FIXME: hackedy hack
            var attribs = new GameAttributeMap();
            attribs[GameAttribute.Hitpoints_Healed_Target] = 76f;
            attribs.SendMessage(InGameClient, this.DynamicID);
        }

        public override void OnLeave(World world)
        {
        }

        public override bool Reveal(Mooege.Core.GS.Player.Player player)
        {
            if (!base.Reveal(player))
                return false;

            if (this == player) // only send this when player's own actor being is revealed. /raist.
            {
                player.InGameClient.SendMessage(new PlayerWarpedMessage()
                                                    {
                                                        Field0 = 9,
                                                        Field1 = 0f,
                                                    });
            }

            player.InGameClient.SendMessage(new PlayerEnterKnownMessage()
            {
                PlayerIndex = this.PlayerIndex,
                ActorId = this.DynamicID,
            });

            this.Inventory.SendVisualInventory(player); 

            if (this == player) // only send this to player itself. Warning: don't remove this check or you'll make the game start crashing! /raist.
            {
                player.InGameClient.SendMessage(new PlayerActorSetInitialMessage()
                {
                    ActorId = this.DynamicID,
                    PlayerIndex = this.PlayerIndex,
                });
            }
            
            return true;
        }

        // Message handlers
        private void OnObjectTargeted(GameClient client, TargetMessage message)
        {
            Actor actor = this.World.GetActor(message.TargetID);
            if (actor != null)
            {
                actor.OnTargeted(this, message);
            }
            else
            {
                //Logger.Warn("Player targeted an invalid object (ID = {0})", message.TargetID);
            }
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
        }

        // Properties

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

        public static int[] LevelBorders = 
        {
            0, 1200, 2250, 4000, 6050, 8500, 11700, 15400, 19500, 24000, /* Level 1-10 */
            28900, 34200, 39900, 44100, 45000, 46200, 48300, 50400, 52500, 54600, /* Level 11-20 */
            56700, 58800, 60900, 63000, 65100, 67200, 69300, 71400, 73500, 75600, /* Level 21-30 */
            77700, 81700, 85800, 90000, 94300, 98700, 103200, 107800, 112500, 117300, /* Level 31-40 */
            122200, 127200, 132300, 137500, 142800, 148200, 153700, 159300, 165000, 170800, /* Level 41-50 */
            176700, 182700, 188800, 195000, 201300, 207700, 214200, 220800, 227500, 234300, /* Level 51-60 */
            241200, 248200, 255300, 262500, 269800, 277200, 284700, 292300, 300000, 307800, /* Level 61-70 */
            315700, 323700, 331800, 340000, 348300, 356700, 365200, 373800, 382500, 391300, /* Level 71-80 */
            400200, 409200, 418300, 427500, 436800, 446200, 455700, 465300, 475000, 484800, /* Level 81-90 */
            494700, 504700, 514800, 525000, 535300, 545700, 556200, 566800, 577500 /* Level 91-99 */
        };

        public static int[] LevelUpEffects =
        {
            85186, 85186, 85186, 85186, 85186, 85190, 85190, 85190, 85190, 85190, /* Level 1-10 */
            85187, 85187, 85187, 85187, 85187, 85187, 85187, 85187, 85187, 85187, /* Level 11-20 */
            85192, 85192, 85192, 85192, 85192, 85192, 85192, 85192, 85192, 85192, /* Level 21-30 */
            85192, 85192, 85192, 85192, 85192, 85192, 85192, 85192, 85192, 85192, /* Level 31-40 */
            85192, 85192, 85192, 85192, 85192, 85192, 85192, 85192, 85192, 85192, /* Level 41-50 */
            85194, 85194, 85194, 85194, 85194, 85194, 85194, 85194, 85194, 85194, /* Level 51-60 */
            85194, 85194, 85194, 85194, 85194, 85194, 85194, 85194, 85194, 85194, /* Level 61-70 */
            85194, 85194, 85194, 85194, 85194, 85194, 85194, 85194, 85194, 85194, /* Level 71-80 */
            85195, 85195, 85195, 85195, 85195, 85195, 85195, 85195, 85195, 85195, /* Level 81-90 */
            85195, 85195, 85195, 85195, 85195, 85195, 85195, 85195, 85195, 85195 /* Level 91-99 */
        };

        public void UpdateExp(int addedExp)
        {
            GameAttributeMap attribs = new GameAttributeMap();

            this.Attributes[GameAttribute.Experience_Next] -= addedExp;

            // Levelup
            if ((this.Attributes[GameAttribute.Experience_Next] <= 0) && (this.Attributes[GameAttribute.Level] < this.Attributes[GameAttribute.Level_Cap]))
            {
                this.Attributes[GameAttribute.Level]++;
                if (this.Attributes[GameAttribute.Level] < this.Attributes[GameAttribute.Level_Cap]) { this.Attributes[GameAttribute.Experience_Next] = this.Attributes[GameAttribute.Experience_Next] + LevelBorders[this.Attributes[GameAttribute.Level]]; }
                else { this.Attributes[GameAttribute.Experience_Next] = 0; }

                // todo: not always adding 2/2/2/1 on Levelup
                this.Attributes[GameAttribute.Defense] = 10f + (this.Attributes[GameAttribute.Level] - 1f) * 2f;
                this.Attributes[GameAttribute.Vitality] = 9f + (this.Attributes[GameAttribute.Level] - 1f) * 1f;
                this.Attributes[GameAttribute.Precision] = 11f + (this.Attributes[GameAttribute.Level] - 1f) * 2f;
                this.Attributes[GameAttribute.Attack] = 10f + (this.Attributes[GameAttribute.Level] - 1f) * 2f;
                attribs[GameAttribute.Level] = this.Attributes[GameAttribute.Level];
                attribs[GameAttribute.Defense] = this.Attributes[GameAttribute.Defense];
                attribs[GameAttribute.Vitality] = this.Attributes[GameAttribute.Vitality];
                attribs[GameAttribute.Precision] = this.Attributes[GameAttribute.Precision];
                attribs[GameAttribute.Attack] = this.Attributes[GameAttribute.Attack];
                attribs[GameAttribute.Experience_Next] = this.Attributes[GameAttribute.Experience_Next];
                attribs.SendMessage(this.InGameClient, this.DynamicID);

                this.InGameClient.SendMessage(new PlayerLevel()
                {
                    Id = 0x98,
                    Field0 = 0x00000000,
                    Field1 = this.Attributes[GameAttribute.Level],
                });

                this.InGameClient.SendMessage(new PlayEffectMessage()
                {
                    Id = 0x7a,
                    ActorID = this.DynamicID,
                    Field1 = 6,
                });
                this.InGameClient.SendMessage(new PlayEffectMessage()
                {
                    Id = 0x7a,
                    ActorID = this.DynamicID,
                    Field1 = 32,
                    Field2 = LevelUpEffects[this.Attributes[GameAttribute.Level]],
                });
            }

            // constant 0 exp at Level_Cap
            if (this.Attributes[GameAttribute.Experience_Next] < 0) { this.Attributes[GameAttribute.Experience_Next] = 0; }

            attribs[GameAttribute.Experience_Next] = this.Attributes[GameAttribute.Experience_Next];
            attribs.SendMessage(this.InGameClient, this.DynamicID);

            //this.Attributes.SendMessage(this.InGameClient, this.DynamicID); kills the player atm
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
                switch (this.Properties.Class)
                {
                    case ToonClass.Barbarian:
                        return 1.2f;
                    case ToonClass.DemonHunter:
                        return 1.35f;
                    case ToonClass.Monk:
                        return 1.43f;
                    case ToonClass.WitchDoctor:
                        return 1.1f;
                    case ToonClass.Wizard:
                        return 1.3f;
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

        public GenericBlobMessage GetPlayerBanner()
        {
            var playerBanner = D3.GameMessage.PlayerBanner.CreateBuilder()
                .SetPlayerIndex((uint) this.PlayerIndex)
                .SetBanner(this.Properties.Owner.BannerConfiguration)
                .Build();

            return new GenericBlobMessage(Opcodes.GenericBlobMessage6) {Data = playerBanner.ToByteArray()};
        }
    }
}
