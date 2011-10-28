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
using Mooege.Core.GS.Actors.Implementations;
using Mooege.Core.GS.Common.Types.Math;
using Mooege.Core.GS.Objects;
using Mooege.Core.GS.Map;
using Mooege.Core.GS.Actors;
using Mooege.Core.GS.Skills;
using Mooege.Net.GS;
using Mooege.Net.GS.Message;
using Mooege.Net.GS.Message.Definitions.Actor;
using Mooege.Net.GS.Message.Definitions.Misc;
using Mooege.Net.GS.Message.Definitions.Waypoint;
using Mooege.Net.GS.Message.Definitions.World;
using Mooege.Net.GS.Message.Fields;
using Mooege.Net.GS.Message.Definitions.Hero;
using Mooege.Net.GS.Message.Definitions.Player;
using Mooege.Net.GS.Message.Definitions.Skill;
using Mooege.Net.GS.Message.Definitions.Effect;
using Mooege.Net.GS.Message.Definitions.Conversation;
using Mooege.Common.Helpers;
using Mooege.Net.GS.Message.Definitions.Combat;
using System;


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

        // Used for Exp-Bonuses
        private int _killstreakTickTime;
        private int _killstreakPlayer;
        private int _killstreakEnvironment;
        private int _lastMonsterKillTick;
        private int _lastMonsterAttackTick;
        private int _lastMonsterAttackKills;
        private int _lastEnvironmentDestroyTick;
        private int _lastEnvironmentDestroyMonsterKills;
        private int _lastEnvironmentDestroyMonsterKillTick;

        public Dictionary<uint, IRevealable> RevealedObjects { get; private set; }

        // Collection of items that only the player can see. This is only used when items drop from killing an actor
        // TODO: Might want to just have a field on the item itself to indicate whether it is visible to only one player
        public Dictionary<uint, Item> GroundItems { get; private set; }

        public List<OpenConversation> OpenConversations { get; set; }

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

            this.OpenConversations = new List<OpenConversation>();

            this._killstreakTickTime = 400;
            this._killstreakPlayer = 0;
            this._killstreakEnvironment = 0;
            this._lastMonsterKillTick = 0;
            this._lastMonsterAttackTick = 0;
            this._lastMonsterAttackKills = 0;
            this._lastEnvironmentDestroyTick = 0;
            this._lastEnvironmentDestroyMonsterKills = 0;
            this._lastEnvironmentDestroyMonsterKillTick = 0;

            // actor values
            this.ActorSNO = this.ClassSNO;
            this.Field2 = 0x00000009;
            this.Field3 = 0x00000000;
            this.Scale = ModelScale;
            this.RotationAmount = 0.05940768f;
            this.RotationAxis = new Vector3D(0f, 0f, 0.9982339f);
            this.CollFlags = 0x00000000;

            this.CurrentScene = this.World.SpawnableScenes.First();
            this.Position.X = this.CurrentScene.StartPosition.X;
            this.Position.Y = this.CurrentScene.StartPosition.Y;
            this.Position.Z = this.CurrentScene.StartPosition.Z;

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
            //Skills
            this.Attributes[GameAttribute.SkillKit] = this.SkillKit;
            this.Attributes[GameAttribute.Skill_Total, 0x7545] = 1; //Axe Operate Gizmo
            this.Attributes[GameAttribute.Skill, 0x7545] = 1;
            this.Attributes[GameAttribute.Skill_Total, 0x76B7] = 1; //Punch!
            this.Attributes[GameAttribute.Skill, 0x76B7] = 1;
            this.Attributes[GameAttribute.Skill_Total, 0x216FA] = 1; //Monk's Blinding Flash
            this.Attributes[GameAttribute.Skill, 0x216FA] = 1;
            this.Attributes[GameAttribute.Skill_Total, 0x176C4] = 1; //Monk's Fist of Thunder
            this.Attributes[GameAttribute.Skill, 0x176C4] = 1;
            this.Attributes[GameAttribute.Skill_Total, 0x6DF] = 1; //Use Item
            this.Attributes[GameAttribute.Skill, 0x6DF] = 1;
            this.Attributes[GameAttribute.Skill_Total, 0x7780] = 1; //Basic Attack
            this.Attributes[GameAttribute.Skill, 0x7780] = 1;
            this.Attributes[GameAttribute.Skill_Total, 0x0000CE11] = 1;  //Monk Spirit Trait
            this.Attributes[GameAttribute.Skill, 0x0000CE11] = 1;
            this.Attributes[GameAttribute.Skill_Total, 0x0002EC66] = 0; //stone of recall
            this.Attributes[GameAttribute.Skill_Total, 0xFFFFF] = 1;
            this.Attributes[GameAttribute.Skill, 0xFFFFF] = 1;

            //Buffs
            this.Attributes[GameAttribute.Buff_Active, 0x33C40] = true;
            this.Attributes[GameAttribute.Buff_Icon_End_Tick0, 0x00033C40] = 0x000003FB;
            this.Attributes[GameAttribute.Buff_Icon_Start_Tick0, 0x00033C40] = 0x00000077;
            this.Attributes[GameAttribute.Buff_Icon_Count0, 0x00033C40] = 1;
            this.Attributes[GameAttribute.Buff_Active, 0xCE11] = true;
            this.Attributes[GameAttribute.Buff_Icon_Count0, 0x0000CE11] = 1;
            this.Attributes[GameAttribute.Buff_Visual_Effect, 0xFFFFF] = true;

            //Resistance
            this.Attributes[GameAttribute.Resistance, 0xDE] = 0.5f;
            this.Attributes[GameAttribute.Resistance, 0x226] = 0.5f;
            this.Attributes[GameAttribute.Resistance_Total, 0] = 10f; // im pretty sure key = 0 doesnt do anything since the lookup is (attributeId | (key << 12)), maybe this is some base resistance? /cm
            // likely the physical school of damage, it probably doesn't actually do anything in this case (or maybe just not for the player's hero) 
            // but exists for the sake of parity with weapon damage schools
            this.Attributes[GameAttribute.Resistance_Total, 1] = 10f; //Fire
            this.Attributes[GameAttribute.Resistance_Total, 2] = 10f; //Lightning
            this.Attributes[GameAttribute.Resistance_Total, 3] = 10f; //Cold
            this.Attributes[GameAttribute.Resistance_Total, 4] = 10f; //Poison
            this.Attributes[GameAttribute.Resistance_Total, 5] = 10f; //Arcane
            this.Attributes[GameAttribute.Resistance_Total, 6] = 10f; //Holy
            this.Attributes[GameAttribute.Resistance_Total, 0xDE] = 0.5f;
            this.Attributes[GameAttribute.Resistance_Total, 0x226] = 0.5f;

            //Damage
            this.Attributes[GameAttribute.Damage_Delta_Total, 0] = 1f;
            this.Attributes[GameAttribute.Damage_Delta_Total, 1] = 3.051758E-05f;
            this.Attributes[GameAttribute.Damage_Delta_Total, 2] = 3.051758E-05f;
            this.Attributes[GameAttribute.Damage_Delta_Total, 3] = 3.051758E-05f;
            this.Attributes[GameAttribute.Damage_Delta_Total, 4] = 3.051758E-05f;
            this.Attributes[GameAttribute.Damage_Delta_Total, 5] = 3.051758E-05f;
            this.Attributes[GameAttribute.Damage_Delta_Total, 6] = 3.051758E-05f;
            this.Attributes[GameAttribute.Damage_Min_Total, 0] = 2f;
            this.Attributes[GameAttribute.Damage_Min_Total, 1] = 3.051758E-05f;
            this.Attributes[GameAttribute.Damage_Min_Total, 2] = 3.051758E-05f;
            this.Attributes[GameAttribute.Damage_Min_Total, 3] = 3.051758E-05f;
            this.Attributes[GameAttribute.Damage_Min_Total, 4] = 3.051758E-05f;
            this.Attributes[GameAttribute.Damage_Min_Total, 5] = 3.051758E-05f;
            this.Attributes[GameAttribute.Damage_Min_Total, 6] = 3.051758E-05f;
            this.Attributes[GameAttribute.Damage_Min_Total, 0xFFFFF] = 3.051758E-05f;
            this.Attributes[GameAttribute.Damage_Min_Subtotal, 0] = 2f;
            this.Attributes[GameAttribute.Damage_Min_Subtotal, 1] = 3.051758E-05f;
            this.Attributes[GameAttribute.Damage_Min_Subtotal, 2] = 3.051758E-05f;
            this.Attributes[GameAttribute.Damage_Min_Subtotal, 3] = 3.051758E-05f;
            this.Attributes[GameAttribute.Damage_Min_Subtotal, 4] = 3.051758E-05f;
            this.Attributes[GameAttribute.Damage_Min_Subtotal, 5] = 3.051758E-05f;
            this.Attributes[GameAttribute.Damage_Min_Subtotal, 6] = 3.051758E-05f;
            this.Attributes[GameAttribute.Damage_Min_Subtotal, 0xFFFFF] = 3.051758E-05f;
            this.Attributes[GameAttribute.Damage_Weapon_Min_Total_CurrentHand, 0] = 2f;
            this.Attributes[GameAttribute.Damage_Weapon_Min_Total_CurrentHand, 1] = 3.051758E-05f;
            this.Attributes[GameAttribute.Damage_Weapon_Min_Total_CurrentHand, 2] = 3.051758E-05f;
            this.Attributes[GameAttribute.Damage_Weapon_Min_Total_CurrentHand, 3] = 3.051758E-05f;
            this.Attributes[GameAttribute.Damage_Weapon_Min_Total_CurrentHand, 4] = 3.051758E-05f;
            this.Attributes[GameAttribute.Damage_Weapon_Min_Total_CurrentHand, 5] = 3.051758E-05f;
            this.Attributes[GameAttribute.Damage_Weapon_Min_Total_CurrentHand, 6] = 3.051758E-05f;
            this.Attributes[GameAttribute.Damage_Weapon_Min, 0] = 2f;
            this.Attributes[GameAttribute.Damage_Weapon_Min_Total, 0] = 2f;
            this.Attributes[GameAttribute.Damage_Weapon_Min_Total_All] = 2f;
            this.Attributes[GameAttribute.Damage_Weapon_Min_Total_MainHand, 0] = 2f;
            this.Attributes[GameAttribute.Damage_Weapon_Min_Total_CurrentHand, 0xFFFFF] = 3.051758E-05f;
            this.Attributes[GameAttribute.Damage_Weapon_Delta, 0] = 1f;
            this.Attributes[GameAttribute.Damage_Weapon_Delta_SubTotal, 0] = 1f;
            this.Attributes[GameAttribute.Damage_Weapon_Delta_Total_CurrentHand, 0] = 1f;
            this.Attributes[GameAttribute.Damage_Weapon_Delta_Total_CurrentHand, 1] = 3.051758E-05f;
            this.Attributes[GameAttribute.Damage_Weapon_Delta_Total_CurrentHand, 2] = 3.051758E-05f;
            this.Attributes[GameAttribute.Damage_Weapon_Delta_Total_CurrentHand, 3] = 3.051758E-05f;
            this.Attributes[GameAttribute.Damage_Weapon_Delta_Total_CurrentHand, 4] = 3.051758E-05f;
            this.Attributes[GameAttribute.Damage_Weapon_Delta_Total_CurrentHand, 5] = 3.051758E-05f;
            this.Attributes[GameAttribute.Damage_Weapon_Delta_Total_CurrentHand, 6] = 3.051758E-05f;
            this.Attributes[GameAttribute.Damage_Weapon_Delta_Total, 0] = 1f;
            this.Attributes[GameAttribute.Damage_Weapon_Delta_Total_All] = 1f;
            this.Attributes[GameAttribute.Damage_Weapon_Delta_Total_MainHand, 0] = 1f;
            this.Attributes[GameAttribute.Damage_Weapon_Max, 0] = 3f;
            this.Attributes[GameAttribute.Damage_Weapon_Max_Total, 0] = 3f;

            //Bonus stats
            this.Attributes[GameAttribute.Get_Hit_Recovery] = 6f;
            this.Attributes[GameAttribute.Get_Hit_Recovery_Per_Level] = 1f;
            this.Attributes[GameAttribute.Get_Hit_Recovery_Base] = 5f;
            this.Attributes[GameAttribute.Get_Hit_Max] = 60f;
            this.Attributes[GameAttribute.Get_Hit_Max_Per_Level] = 10f;
            this.Attributes[GameAttribute.Get_Hit_Max_Base] = 50f;
            this.Attributes[GameAttribute.Hit_Chance] = 1f;
            this.Attributes[GameAttribute.Dodge_Rating_Total] = 3.051758E-05f;
            this.Attributes[GameAttribute.Attacks_Per_Second_Item_CurrentHand] = 1.199219f;
            this.Attributes[GameAttribute.Attacks_Per_Second_Item_Total_MainHand] = 1.199219f;
            this.Attributes[GameAttribute.Attacks_Per_Second_Total] = 1.199219f;
            this.Attributes[GameAttribute.Attacks_Per_Second] = 1f;
            this.Attributes[GameAttribute.Attacks_Per_Second_Item_MainHand] = 1.199219f;
            this.Attributes[GameAttribute.Attacks_Per_Second_Item_Total] = 1.199219f;
            this.Attributes[GameAttribute.Attacks_Per_Second_Item_Subtotal] = 3.051758E-05f;
            this.Attributes[GameAttribute.Attacks_Per_Second_Item] = 3.051758E-05f;
            this.Attributes[GameAttribute.Crit_Percent_Cap] = 0x3F400000;
            this.Attributes[GameAttribute.Casting_Speed_Total] = 1f;
            this.Attributes[GameAttribute.Casting_Speed] = 1f;

            //Basic stats
            this.Attributes[GameAttribute.Level_Cap] = 13;
            this.Attributes[GameAttribute.Level] = this.Properties.Level;
            this.Attributes[GameAttribute.Experience_Next] = LevelBorders[this.Properties.Level];
            this.Attributes[GameAttribute.Experience_Granted] = 1000;
            this.Attributes[GameAttribute.Armor_Total] = 0;
            this.Attributes[GameAttribute.Attack] = this.InitialAttack;
            this.Attributes[GameAttribute.Precision] = this.InitialPrecision;
            this.Attributes[GameAttribute.Defense] = this.InitialDefense;
            this.Attributes[GameAttribute.Vitality] = this.InitialVitality;

            //Hitpoints have to be calculated after Vitality
            this.Attributes[GameAttribute.Hitpoints_Factor_Level] = 4f;
            this.Attributes[GameAttribute.Hitpoints_Factor_Vitality] = 4f;
            //this.Attributes[GameAttribute.Hitpoints_Total_From_Level] = 3.051758E-05f;
            this.Attributes[GameAttribute.Hitpoints_Total_From_Level] = 40f; // For now, this just adds 40 hitpoints to the hitpoints gained from vitality
            this.Attributes[GameAttribute.Hitpoints_Total_From_Vitality] = this.Attributes[GameAttribute.Vitality] * this.Attributes[GameAttribute.Hitpoints_Factor_Vitality];
            this.Attributes[GameAttribute.Hitpoints_Max] = GetMaxTotalHitpoints();
            this.Attributes[GameAttribute.Hitpoints_Max_Total] = GetMaxTotalHitpoints();
            this.Attributes[GameAttribute.Hitpoints_Cur] = this.Attributes[GameAttribute.Hitpoints_Max_Total];

            //Resource
            this.Attributes[GameAttribute.Resource_Cur, this.ResourceID] = 200f;
            this.Attributes[GameAttribute.Resource_Max, this.ResourceID] = 200f;
            this.Attributes[GameAttribute.Resource_Max_Total, this.ResourceID] = 200f;
            this.Attributes[GameAttribute.Resource_Effective_Max, this.ResourceID] = 200f;
            this.Attributes[GameAttribute.Resource_Regen_Total, this.ResourceID] = 3.051758E-05f;
            this.Attributes[GameAttribute.Resource_Type_Primary] = this.ResourceID;
            //Secondary Resource for the Demon Hunter
            if (this.Properties.Class == ToonClass.DemonHunter)
            {
                int Discipline = this.ResourceID + 1; //0x00000006
                this.Attributes[GameAttribute.Resource_Cur, Discipline] = 30f;
                this.Attributes[GameAttribute.Resource_Max, Discipline] = 30f;
                this.Attributes[GameAttribute.Resource_Max_Total, Discipline] = 30f;
                this.Attributes[GameAttribute.Resource_Effective_Max, Discipline] = 30f;
                this.Attributes[GameAttribute.Resource_Regen_Total, Discipline] = 3.051758E-05f;
                this.Attributes[GameAttribute.Resource_Type_Secondary] = Discipline;
            }

            //Movement
            this.Attributes[GameAttribute.Movement_Scalar_Total] = 1f;
            this.Attributes[GameAttribute.Movement_Scalar_Capped_Total] = 1f;
            this.Attributes[GameAttribute.Movement_Scalar_Subtotal] = 1f;
            this.Attributes[GameAttribute.Movement_Scalar] = 1f;
            this.Attributes[GameAttribute.Walking_Rate_Total] = 0.2797852f;
            this.Attributes[GameAttribute.Walking_Rate] = 0.2797852f;
            this.Attributes[GameAttribute.Running_Rate_Total] = 0.3598633f;
            this.Attributes[GameAttribute.Running_Rate] = 0.3598633f;
            this.Attributes[GameAttribute.Sprinting_Rate_Total] = 3.051758E-05f;
            this.Attributes[GameAttribute.Strafing_Rate_Total] = 3.051758E-05f;

            //Miscellaneous
            //this.Attributes[GameAttribute.Disabled] = true;
            //this.Attributes[GameAttribute.Loading] = true;
            //this.Attributes[GameAttribute.Invulnerable] = true;
            this.Attributes[GameAttribute.Hidden] = false;
            this.Attributes[GameAttribute.Immobolize] = true;
            this.Attributes[GameAttribute.Untargetable] = true;
            this.Attributes[GameAttribute.CantStartDisplayedPowers] = true;
            this.Attributes[GameAttribute.IsTrialActor] = true;
            this.Attributes[GameAttribute.Trait, 0x0000CE11] = 1;
            this.Attributes[GameAttribute.TeamID] = 2;
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
            else if (message is TryWaypointMessage) OnTryWaypoint(client, (TryWaypointMessage)message);
            else return;
        }

        public override void Update()
        {
            // Check the Killstreaks
            CheckExpBonus(0);
            CheckExpBonus(1);
            // Check if there is an conversation to close in this tick
            CheckOpenConversations();
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
                                 Speed = message.Speed,
                                 Field5 = message.Field5,
                                 AnimationTag = message.AnimationTag
                             };

            this.World.BroadcastExclusive(msg, this); // TODO: We should be instead notifying currentscene we're in. /raist.

            this.CollectGold();
            this.CollectHealthGlobe();
        }

        private void CollectGold()
        {
            var actorList = this.World.GetActorsInRange(this.Position.X, this.Position.Y, this.Position.Z, 5f);
            foreach (var actor in actorList)
            {
                Item item;
                if (! (actor is Item)) continue;
                item = (Item)actor;
                if (item.ItemType != ItemType.Gold) continue;

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

                this.Inventory.PickUpGold(item.DynamicID);
              

                item.Destroy();
            }
        }

        private void CollectHealthGlobe()
        {
            var actorList = this.World.GetActorsInRange(this.Position.X, this.Position.Y, this.Position.Z, 5f);
            foreach (Actor actor in actorList)
            {
                Item item;
                if (!(actor is Item)) continue;
                item = (Item)actor;
                if (item.ItemType != ItemType.HealthGlobe) continue;

                this.InGameClient.SendMessage(new PlayEffectMessage() //Remember, for PlayEffectMessage, field1=7 are globes picking animation.
                {
                    ActorId = this.DynamicID,
                    Effect = Effect.HealthOrbPickup
                });

                foreach(var pair in this.World.Players) // should be actually checking for players in proximity. /raist
                {
                    pair.Value.AddPercentageHP((int)item.Attributes[GameAttribute.Health_Globe_Bonus_Health]);
                }

                item.Destroy();

            }
        }

        public void AddPercentageHP(int percentage)
        {
            float quantity = (percentage * this.Attributes[GameAttribute.Hitpoints_Max]) / 100;
            this.AddHP(quantity);
        }

        public void AddHP(float quantity)
        {
            if (this.Attributes[GameAttribute.Hitpoints_Cur] + quantity >= this.Attributes[GameAttribute.Hitpoints_Max])
                this.Attributes[GameAttribute.Hitpoints_Cur] = this.Attributes[GameAttribute.Hitpoints_Max];
            else
                this.Attributes[GameAttribute.Hitpoints_Cur] = this.Attributes[GameAttribute.Hitpoints_Cur] + quantity;
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
            Logger.Trace("Leaving world!");
        }

        protected override void OnPositionChange(Vector3D prevPosition)
        {
            // check here for current-scene change.
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
                if ((actor.GBHandle.Type == 1) && (actor.Attributes[GameAttribute.TeamID] == 10))
                {
                    this._lastMonsterAttackTick = this.InGameClient.Game.Tick;
                }

                actor.OnTargeted(this, message);
                CheckExpBonus(2);
            }
            else
            {
                //Logger.Warn("Player targeted an invalid object (ID = {0})", message.TargetID);
            }
        }

        private void OnTryWaypoint(GameClient client, TryWaypointMessage tryWaypointMessage)
        {
            Vector3D position;

            if (Waypoint.Waypoints.ContainsKey(tryWaypointMessage.Field1)) // TODO handle other worlds! it's easy! /fasbat
                position = Waypoint.Waypoints[tryWaypointMessage.Field1].Position;
            else
                return;

            this.Position = position;
            InGameClient.SendMessage(ACDWorldPositionMessage);
        }

        private void OnPlayerChangeHotbarButtonMessage(GameClient client, PlayerChangeHotbarButtonMessage message)
        {
            this.SkillSet.HotBarSkills[message.BarIndex] = message.ButtonData;
        }

        private void OnAssignPassiveSkill(GameClient client, AssignPassiveSkillMessage message)
        {
            this.SkillSet.PassiveSkills[message.SkillIndex] = message.SNOSkill;
            this.UpdateHeroState();
        }

        private void OnAssignActiveSkill(GameClient client, AssignActiveSkillMessage message)
        {
            var oldSNOSkill = this.SkillSet.ActiveSkills[message.SkillIndex]; // find replaced skills SNO.

            foreach (HotbarButtonData button in this.SkillSet.HotBarSkills.Where(button => button.SNOSkill == oldSNOSkill)) // loop through hotbar and replace the old skill with new one
            {
                button.SNOSkill = message.SNOSkill;
            }

            this.SkillSet.ActiveSkills[message.SkillIndex] = message.SNOSkill;
            this.UpdateHeroState();
        }

        /// <summary>
        /// Allows you to send a hero state message when you update hero's some property.
        /// </summary>
        public void UpdateHeroState()
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
                Field3 = 0x7FFFFFFF,

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

        // Defines the Max Total hitpoints for the current level
        // May want to move this into a property if it has to made class-specific
        // This is still a work in progress on getting the right algorithm for all the classes
        private float GetMaxTotalHitpoints()
        {
            return (this.Attributes[GameAttribute.Hitpoints_Total_From_Vitality]) +
                    (this.Attributes[GameAttribute.Hitpoints_Total_From_Level]);
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
                this.Properties.LevelUp();
                if (this.Attributes[GameAttribute.Level] < this.Attributes[GameAttribute.Level_Cap]) { this.Attributes[GameAttribute.Experience_Next] = this.Attributes[GameAttribute.Experience_Next] + LevelBorders[this.Attributes[GameAttribute.Level]]; }
                else { this.Attributes[GameAttribute.Experience_Next] = 0; }

                // 4 main attributes are incremented according to class
                this.Attributes[GameAttribute.Attack] += this.AttackIncrement;
                this.Attributes[GameAttribute.Precision] += this.PrecisionIncrement;
                this.Attributes[GameAttribute.Vitality] += this.VitalityIncrement;
                this.Attributes[GameAttribute.Defense] += this.DefenseIncrement;

                // Hitpoints from level may actually change. This needs to be verified by someone with the beta.
                //this.Attributes[GameAttribute.Hitpoints_Total_From_Level] = this.Attributes[GameAttribute.Level] * this.Attributes[GameAttribute.Hitpoints_Factor_Level];
                
                // For now, hit points are based solely on vitality and initial hitpoints received.
                // This will have to change when hitpoint bonuses from items are implemented.
                this.Attributes[GameAttribute.Hitpoints_Total_From_Vitality] = this.Attributes[GameAttribute.Vitality] * this.Attributes[GameAttribute.Hitpoints_Factor_Vitality];
                this.Attributes[GameAttribute.Hitpoints_Max] = GetMaxTotalHitpoints();
                this.Attributes[GameAttribute.Hitpoints_Max_Total] = GetMaxTotalHitpoints();

                // On level up, health is set to max
                this.Attributes[GameAttribute.Hitpoints_Cur] = this.Attributes[GameAttribute.Hitpoints_Max_Total];

                attribs[GameAttribute.Level] = this.Attributes[GameAttribute.Level];
                attribs[GameAttribute.Defense] = this.Attributes[GameAttribute.Defense];
                attribs[GameAttribute.Vitality] = this.Attributes[GameAttribute.Vitality];
                attribs[GameAttribute.Precision] = this.Attributes[GameAttribute.Precision];
                attribs[GameAttribute.Attack] = this.Attributes[GameAttribute.Attack];
                attribs[GameAttribute.Experience_Next] = this.Attributes[GameAttribute.Experience_Next];
                attribs[GameAttribute.Hitpoints_Total_From_Vitality] = this.Attributes[GameAttribute.Hitpoints_Total_From_Vitality];
                attribs[GameAttribute.Hitpoints_Max_Total] = this.Attributes[GameAttribute.Hitpoints_Max_Total];
                attribs[GameAttribute.Hitpoints_Max] = this.Attributes[GameAttribute.Hitpoints_Max];
                attribs[GameAttribute.Hitpoints_Cur] = this.Attributes[GameAttribute.Hitpoints_Cur];

                attribs.SendMessage(this.InGameClient, this.DynamicID);

                this.InGameClient.SendMessage(new PlayerLevel()
                {
                    Id = 0x98,
                    Field0 = 0x00000000,
                    Field1 = this.Attributes[GameAttribute.Level],
                });

                this.InGameClient.SendMessage(new PlayEffectMessage()
                {
                    ActorId = this.DynamicID,
                    Effect = Effect.LevelUp,
                });

                this.World.BroadcastGlobal(new PlayEffectMessage()
                {
                    ActorId = this.DynamicID,
                    Effect = Effect.PlayEffectGroup,
                    OptionalParameter = LevelUpEffects[this.Attributes[GameAttribute.Level]],
                });
            }

            // constant 0 exp at Level_Cap
            if (this.Attributes[GameAttribute.Experience_Next] < 0) { this.Attributes[GameAttribute.Experience_Next] = 0; }

            attribs[GameAttribute.Experience_Next] = this.Attributes[GameAttribute.Experience_Next];
            attribs.SendMessage(this.InGameClient, this.DynamicID);

            //this.Attributes.SendMessage(this.InGameClient, this.DynamicID); kills the player atm
        }

        public void UpdateExpBonusData(int attackerActorType, int defeatedActorType)
        {
            if (attackerActorType == 7) // Player
            {
                if (defeatedActorType == 1) // Monster
                {
                    // Massacre
                    if (this._lastMonsterKillTick + this._killstreakTickTime > this.InGameClient.Game.Tick)
                    {
                        this._killstreakPlayer++;
                    }
                    else
                    {
                        this._killstreakPlayer = 1;
                    }

                    // MightyBlow
                    if (Math.Abs(this._lastMonsterAttackTick - this.InGameClient.Game.Tick) <= 20)
                    {
                        this._lastMonsterAttackKills++;
                    }
                    else
                    {
                        this._lastMonsterAttackKills = 1;
                    }

                    this._lastMonsterKillTick = this.InGameClient.Game.Tick;
                }
                else if (defeatedActorType == 5) // Environment
                {
                    // Destruction
                    if (this._lastEnvironmentDestroyTick + this._killstreakTickTime > this.InGameClient.Game.Tick)
                    {
                        this._killstreakEnvironment++;
                    }
                    else
                    {
                        this._killstreakEnvironment = 1;
                    }

                    this._lastEnvironmentDestroyTick = this.InGameClient.Game.Tick;
                }
            }
            else if (attackerActorType == 5) // Environment
            {
                // Pulverized
                if (Math.Abs(this._lastEnvironmentDestroyMonsterKillTick - this.InGameClient.Game.Tick) <= 20)
                {
                    this._lastEnvironmentDestroyMonsterKills++;
                }
                else
                {
                    this._lastEnvironmentDestroyMonsterKills = 1;
                }

                this._lastEnvironmentDestroyMonsterKillTick = this.InGameClient.Game.Tick;
            }
        }

        public void CheckExpBonus(byte BonusType)
        {
            int defeated = 0;
            int expBonus = 0;

            switch (BonusType)
            {
                case 0: // Massacre
                    {
                        if ((this._killstreakPlayer > 5) && (this._lastMonsterKillTick + this._killstreakTickTime <= this.InGameClient.Game.Tick))
                        {
                            defeated = this._killstreakPlayer;
                            expBonus = (this._killstreakPlayer - 5) * 10;

                            this._killstreakPlayer = 0;
                        }
                        break;
                    }
                case 1: // Destruction
                    {
                        if ((this._killstreakEnvironment > 5) && (this._lastEnvironmentDestroyTick + this._killstreakTickTime <= this.InGameClient.Game.Tick))
                        {
                            defeated = this._killstreakEnvironment;
                            expBonus = (this._killstreakEnvironment - 5) * 5;

                            this._killstreakEnvironment = 0;
                        }
                        break;
                    }
                case 2: // Mighty Blow
                    {
                        if (this._lastMonsterAttackKills > 5)
                        {
                            defeated = this._lastMonsterAttackKills;
                            expBonus = (this._lastMonsterAttackKills - 5) * 5;
                        }
                        this._lastMonsterAttackKills = 0;
                        break;
                    }
                case 3: // Pulverized
                    {
                        if (this._lastEnvironmentDestroyMonsterKills > 3)
                        {
                            defeated = this._lastEnvironmentDestroyMonsterKills;
                            expBonus = (this._lastEnvironmentDestroyMonsterKills - 3) * 10;
                        }
                        this._lastEnvironmentDestroyMonsterKills = 0;
                        break;
                    }
                default:
                    {
                        Logger.Warn("Invalid Exp-Bonus-Type was checked.");
                        return;
                    }
            }

            if (expBonus > 0)
            {
                this.InGameClient.SendMessage(new KillCounterUpdateMessage()
                {
                    Id = 0xcd,
                    Field0 = BonusType,
                    Field1 = defeated,
                    Field2 = expBonus,
                    Field3 = false,
                });

                this.UpdateExp(expBonus);
                PlayHeroConversation(0x0002A73F, RandomHelper.Next(0, 8));
            }
        }

        public void PlayHeroConversation(int snoConversation, int lineID)
        {
            this.InGameClient.SendMessage(new PlayConvLineMessage()
            {
                Id = 0xba,
                ActorID = this.DynamicID,
                Field1 = new uint[9]
                    {
                        this.DynamicID, 0xFFFFFFFF, 0xFFFFFFFF, 0xFFFFFFFF, 0xFFFFFFFF, 0xFFFFFFFF, 0xFFFFFFFF, 0xFFFFFFFF, 0xFFFFFFFF
                    },

                Params = new PlayLineParams()
                {
                    SNOConversation = snoConversation,
                    Field1 = 0x00000001,
                    Field2 = false,
                    LineID = lineID,
                    Field4 = 0x00000000,
                    Field5 = -1,
                    TextClass = (Class)this.Properties.VoiceClassID,
                    Gender = (this.Properties.Gender == 0) ? VoiceGender.Male : VoiceGender.Female,
                    AudioClass = (Class)this.Properties.VoiceClassID,
                    SNOSpeakerActor = this._actorSNO,
                    Name = this.Properties.Name,
                    Field11 = 0x00000002,
                    Field12 = -1,
                    Field13 = 0x00000069,
                    Field14 = 0x0000006E,
                    Field15 = 0x00000032
                },
                Field3 = 0x00000069,
            });

            this.OpenConversations.Add(new OpenConversation(
                new EndConversationMessage()
                {
                    ActorId = this.DynamicID,
                    Field0 = 0x0000006E,
                    SNOConversation = snoConversation
                },
                this.InGameClient.Game.Tick + 400
            ));
        }

        public void CheckOpenConversations()
        {
            if(this.OpenConversations.Count > 0)
            {
                foreach(OpenConversation openConversation in this.OpenConversations)
                {
                    if(openConversation.endTick == this.InGameClient.Game.Tick)
                    {
                        this.InGameClient.SendMessage(openConversation.endConversationMessage);
                    }
                }
            }
        }

        public struct OpenConversation
        {
            public EndConversationMessage endConversationMessage;
            public int endTick;

            public OpenConversation(EndConversationMessage endConversationMessage, int endTick)
            {
                this.endConversationMessage = endConversationMessage;
                this.endTick = endTick;
            }
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

        #region PlayerAttributeHandling
        public float InitialAttack // Defines the amount of attack points with which a player starts
        {
            get
            {
                switch (this.Properties.Class)
                {
                    case ToonClass.Barbarian:
                        return 10f + ((this.Properties.Level - 1) * 2);
                    case ToonClass.DemonHunter:
                        return 10f + ((this.Properties.Level - 1) * 2);
                    case ToonClass.Monk:
                        return 10f + ((this.Properties.Level - 1) * 2);
                    case ToonClass.WitchDoctor:
                        return 10f + ((this.Properties.Level - 1) * 2);
                    case ToonClass.Wizard:
                        return 10f + ((this.Properties.Level - 1) * 2);
                }
                return 10f + (this.Properties.Level - 1) * 2;
            }
        }

        public float InitialPrecision // Defines the amount of precision points with which a player starts
        {
            get
            {
                switch (this.Properties.Class)
                {
                    case ToonClass.Barbarian:
                        return 9f + (this.Properties.Level - 1);
                    case ToonClass.DemonHunter:
                        return 11f + ((this.Properties.Level - 1) * 2);
                    case ToonClass.Monk:
                        return 11f + ((this.Properties.Level - 1) * 2);
                    case ToonClass.WitchDoctor:
                        return 9f + ((this.Properties.Level - 1) * 2);
                    case ToonClass.Wizard:
                        return 10f + ((this.Properties.Level - 1) * 2);
                }
                return 10f + ((this.Properties.Level - 1) * 2);
            }
        }

        public float InitialDefense // Defines the amount of defense points with which a player starts
        {
            get
            {
                switch (this.Properties.Class)
                {
                    case ToonClass.Barbarian:
                        return 11f + ((this.Properties.Level - 1) * 2);
                    case ToonClass.DemonHunter:
                        // For DH and Wizard, half the levels (starting with the first) give 2 defense => (Level / 2) * 2
                        // and half give 1 defense => ((Level - 1) / 2) * 1
                        // Note: We can't cancel the twos in ((Level - 1) / 2) * 2 because of integer divison
                        return 9f + (((this.Properties.Level / 2) * 2) + ((this.Properties.Level - 1) / 2));
                    case ToonClass.Monk:
                        return 10f + ((this.Properties.Level - 1) * 2);
                    case ToonClass.WitchDoctor:
                        return 9f + ((this.Properties.Level - 1) * 2);
                    case ToonClass.Wizard:
                        return 8f + (((this.Properties.Level / 2) * 2) + ((this.Properties.Level - 1) / 2));
                }
                return 10f + ((this.Properties.Level - 1) * 2);
            }
        }

        public float InitialVitality // Defines the amount of vitality points with which a player starts
        {
            get
            {
                switch (this.Properties.Class)
                {
                    case ToonClass.Barbarian:
                        return 11f + ((this.Properties.Level - 1) * 2);
                    case ToonClass.DemonHunter:
                        // For DH and Wizard, half the levels give 2 vit => ((Level - 1) / 2) * 2
                        // and half (starting with the first) give 1 vit => (Level / 2) * 1
                        // Note: We can't cancel the twos in ((Level - 1) / 2) * 2 because of integer divison
                        return 9f + ((((this.Properties.Level - 1) / 2) * 2) + (this.Properties.Level / 2));
                    case ToonClass.Monk:
                        return 9f + (this.Properties.Level - 1);
                    case ToonClass.WitchDoctor:
                        return 10f + (this.Properties.Level - 1);
                    case ToonClass.Wizard:
                        return 9f + ((((this.Properties.Level - 1) / 2) * 2) + (this.Properties.Level / 2));
                }
                return 10f + ((this.Properties.Level - 1) * 2);
            }
        }

        // Notes on attribute increment algorithm:
        // Precision: Barbarian => +1, else => +2
        // Defense:   Wizard or Demon Hunter => (lvl+1)%2+1, else => +2
        // Vitality:  Wizard or Demon Hunter => lvl%2+1, Barbarian => +2, else +1
        // Attack:    All +2
        public float AttackIncrement
        {
            get
            {
                switch (this.Properties.Class)
                {
                    case ToonClass.Barbarian:
                        return 2f;
                    case ToonClass.DemonHunter:
                        return 2f;
                    case ToonClass.Monk:
                        return 2f;
                    case ToonClass.WitchDoctor:
                        return 2f;
                    case ToonClass.Wizard:
                        return 2f;
                }
                return 2f;
            }
        }

        public float VitalityIncrement
        {
            get
            {
                switch (this.Properties.Class)
                {
                    case ToonClass.Barbarian:
                        return 2f;
                    case ToonClass.DemonHunter:
                        return (this.Attributes[GameAttribute.Level] % 2) + 1f;
                    case ToonClass.Monk:
                        return 1f;
                    case ToonClass.WitchDoctor:
                        return 1f;
                    case ToonClass.Wizard:
                        return (this.Attributes[GameAttribute.Level] % 2) + 1f;
                }
                return 1f;
            }
        }

        public float DefenseIncrement
        {
            get
            {
                switch (this.Properties.Class)
                {
                    case ToonClass.Barbarian:
                        return 2f;
                    case ToonClass.DemonHunter:
                        return ((this.Attributes[GameAttribute.Level] + 1) % 2) + 1f;
                    case ToonClass.Monk:
                        return 2f;
                    case ToonClass.WitchDoctor:
                        return 2f;
                    case ToonClass.Wizard:
                        return ((this.Attributes[GameAttribute.Level] + 1) % 2) + 1f;
                }
                return 2f;
            }
        }

        public float PrecisionIncrement
        {
            get
            {
                switch (this.Properties.Class)
                {
                    case ToonClass.Barbarian:
                        return 1f;
                    case ToonClass.DemonHunter:
                        return 2f;
                    case ToonClass.Monk:
                        return 2f;
                    case ToonClass.WitchDoctor:
                        return 2f;
                    case ToonClass.Wizard:
                        return 2f;
                }
                return 2f;
            }
        }
        #endregion // #region PlayerAttributeHandling

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
