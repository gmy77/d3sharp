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
using Mooege.Common.Helpers.Math;
using Mooege.Common.Logging;
using Mooege.Core.GS.Common.Types.Math;
using Mooege.Core.GS.Items;
using Mooege.Core.GS.Objects;
using Mooege.Core.GS.Map;
using Mooege.Core.GS.Actors;
using Mooege.Core.GS.Skills;
using Mooege.Core.MooNet.Toons;
using Mooege.Net.GS;
using Mooege.Net.GS.Message;
using Mooege.Net.GS.Message.Definitions.Actor;
using Mooege.Net.GS.Message.Definitions.Conversation;
using Mooege.Net.GS.Message.Definitions.Misc;
using Mooege.Net.GS.Message.Definitions.Pet;
using Mooege.Net.GS.Message.Definitions.Waypoint;
using Mooege.Net.GS.Message.Definitions.World;
using Mooege.Net.GS.Message.Fields;
using Mooege.Net.GS.Message.Definitions.Hero;
using Mooege.Net.GS.Message.Definitions.Player;
using Mooege.Net.GS.Message.Definitions.Skill;
using Mooege.Net.GS.Message.Definitions.Effect;
using Mooege.Net.GS.Message.Definitions.Trade;
using Mooege.Core.GS.Actors.Implementations;
using Mooege.Net.GS.Message.Definitions.Artisan;
using Mooege.Core.GS.Actors.Implementations.Artisans;
using Mooege.Core.GS.Actors.Implementations.Hirelings;
using Mooege.Net.GS.Message.Definitions.Hireling;
using System;
using Mooege.Common.Helpers;
using Mooege.Net.GS.Message.Definitions.ACD;

namespace Mooege.Core.GS.Players
{
    public class Player : Actor, IMessageConsumer, IUpdateable
    {
        private static readonly Logger Logger = LogManager.CreateLogger();

        /// <summary>
        /// The ingame-client for player.
        /// </summary>
        public GameClient InGameClient { get; set; }

        /// <summary>
        /// The player index.
        /// </summary>
        public int PlayerIndex { get; private set; }

        /// <summary>
        /// The player's toon.
        /// We need a better name /raist.
        /// </summary>
        public Toon Toon { get; private set; }

        /// <summary>
        /// Skillset for the player (or actually for player's toons class).
        /// </summary>
        public SkillSet SkillSet { get; private set; }

        /// <summary>
        /// The inventory of player's toon.
        /// </summary>
        public Inventory Inventory { get; private set; }

        /// <summary>
        /// ActorType = Player.
        /// </summary>
        public override ActorType ActorType { get { return ActorType.Player; } }

        /// <summary>
        /// Revealed objects to player.
        /// </summary>
        public Dictionary<uint, IRevealable> RevealedObjects = new Dictionary<uint, IRevealable>();

        public ConversationManager Conversations { get; private set; }

        // Collection of items that only the player can see. This is only used when items drop from killing an actor
        // TODO: Might want to just have a field on the item itself to indicate whether it is visible to only one player
        /// <summary>
        /// Dropped items for the player
        /// </summary>
        public Dictionary<uint, Item> GroundItems { get; private set; }

        /// <summary>
        /// Everything connected to ExpBonuses.
        /// </summary>
        public ExpBonusData ExpBonusData { get; private set; }

        /// <summary>
        /// NPC currently interaced with
        /// </summary>
        public InteractiveNPC SelectedNPC { get; set; }

        private Hireling _activeHireling = null;
        public Hireling ActiveHireling
        {
            get { return _activeHireling; }
            set
            {
                if (value == _activeHireling)
                    return;

                if (_activeHireling != null)
                {
                    _activeHireling.Dismiss(this);
                }

                _activeHireling = value;

                if (value != null)
                {
                    InGameClient.SendMessage(new PetMessage()
                    {
                        Field0 = 0,
                        Field1 = 0,
                        PetId = value.DynamicID,
                        Field3 = 0,
                    });
                }
            }
        }

        private Hireling _activeHirelingProxy = null;
        public Hireling ActiveHirelingProxy
        {
            get { return _activeHirelingProxy; }
            set
            {
                if (value == _activeHirelingProxy)
                    return;

                if (_activeHirelingProxy != null)
                {
                    _activeHirelingProxy.Dismiss(this);
                }

                _activeHirelingProxy = value;

                if (value != null)
                {
                    InGameClient.SendMessage(new PetMessage()
                    {
                        Field0 = 0,
                        Field1 = 0,
                        PetId = value.DynamicID,
                        Field3 = 22,
                    });
                }
            }
        }

        /// <summary>
        /// Creates a new player.
        /// </summary>
        /// <param name="world">The initial world player joins in.</param>
        /// <param name="client">The gameclient for the player.</param>
        /// <param name="bnetToon">Toon of the player.</param>
        public Player(World world, GameClient client, Toon bnetToon)
            : base(world, GetClassSNOId(bnetToon.Gender, bnetToon.Class))
        {
            this.InGameClient = client;
            this.PlayerIndex = Interlocked.Increment(ref this.InGameClient.Game.PlayerIndexCounter); // get a new playerId for the player and make it atomic.
            this.Toon = bnetToon;
            this.GBHandle.Type = (int)GBHandleType.Player;
            this.GBHandle.GBID = this.Toon.ClassID;

            this.Field2 = 0x00000009;
            this.Scale = this.ModelScale;
            this.RotationW = 0.05940768f;
            this.RotationAxis = new Vector3D(0f, 0f, 0.9982339f);
            this.Field7 = -1;
            this.NameSNOId = -1;
            this.Field10 = 0x0;

            this.SkillSet = new SkillSet(this.Toon.Class);
            this.GroundItems = new Dictionary<uint, Item>();
            this.Conversations = new ConversationManager(this, this.World.Game.Quests);
            this.ExpBonusData = new ExpBonusData(this);
            this.SelectedNPC = null;

            // TODO SavePoint from DB
            this.SavePointData = new SavePointData() { snoWorld = -1, SavepointId = -1 };

            #region Attributes

            //Skills
            this.Attributes[GameAttribute.SkillKit] = this.SkillKit;
            this.Attributes[GameAttribute.Skill_Total, 0x7545] = 1; //Axe Operate Gizmo
            this.Attributes[GameAttribute.Skill, 0x7545] = 1;
            this.Attributes[GameAttribute.Skill_Total, 0x76B7] = 1; //Punch!
            this.Attributes[GameAttribute.Skill, 0x76B7] = 1;
            this.Attributes[GameAttribute.Skill_Total, 0x6DF] = 1; //Use Item
            this.Attributes[GameAttribute.Skill, 0x6DF] = 1;
            this.Attributes[GameAttribute.Skill_Total, 0x7780] = 1; //Basic Attack
            this.Attributes[GameAttribute.Skill, 0x7780] = 1;
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
            this.Attributes[GameAttribute.Level] = this.Toon.Level;
            this.Attributes[GameAttribute.Experience_Next] = LevelBorders[this.Toon.Level];
            this.Attributes[GameAttribute.Experience_Granted] = 1000;
            this.Attributes[GameAttribute.Armor_Total] = 0;
            this.Attributes[GameAttribute.Attack] = (int)this.InitialAttack;
            this.Attributes[GameAttribute.Precision] = (int)this.InitialPrecision;
            this.Attributes[GameAttribute.Defense] = (int)this.InitialDefense;
            this.Attributes[GameAttribute.Vitality] = (int)this.InitialVitality;

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

            // Class specific
            switch (this.Toon.Class)
            {
                case ToonClass.Barbarian:
                    this.Attributes[GameAttribute.Skill_Total, 30078] = 1;  //Fury Trait
                    this.Attributes[GameAttribute.Skill, 30078] = 1;
                    this.Attributes[GameAttribute.Trait, 30078] = 1;
                    this.Attributes[GameAttribute.Buff_Active, 30078] = true;
                    this.Attributes[GameAttribute.Buff_Icon_Count0, 30078] = 1;
                    break;
                case ToonClass.DemonHunter:
                    /* // unknown
                    this.Attributes[GameAttribute.Skill_Total, ] = 1;  // Hatred Trait
                    this.Attributes[GameAttribute.Skill, ] = 1;
                    this.Attributes[GameAttribute.Trait, ] = 1;
                    this.Attributes[GameAttribute.Buff_Active, ] = true;
                    this.Attributes[GameAttribute.Buff_Icon_Count0, ] = 1;
                    this.Attributes[GameAttribute.Skill_Total, ] = 1;  // Discipline Trait
                    this.Attributes[GameAttribute.Skill, ] = 1;
                    this.Attributes[GameAttribute.Trait, ] = 1;
                    this.Attributes[GameAttribute.Buff_Active, ] = true;
                    this.Attributes[GameAttribute.Buff_Icon_Count0, ] = 1;
                     */
                    //Secondary Resource for the Demon Hunter
                    int Discipline = this.ResourceID + 1; //0x00000006
                    this.Attributes[GameAttribute.Resource_Cur, Discipline] = 30f;
                    this.Attributes[GameAttribute.Resource_Max, Discipline] = 30f;
                    this.Attributes[GameAttribute.Resource_Max_Total, Discipline] = 30f;
                    this.Attributes[GameAttribute.Resource_Effective_Max, Discipline] = 30f;
                    this.Attributes[GameAttribute.Resource_Type_Secondary] = Discipline;
                    break;
                case ToonClass.Monk:
                    this.Attributes[GameAttribute.Skill_Total, 0x0000CE11] = 1;  //Spirit Trait
                    this.Attributes[GameAttribute.Skill, 0x0000CE11] = 1;
                    this.Attributes[GameAttribute.Trait, 0x0000CE11] = 1;
                    this.Attributes[GameAttribute.Buff_Active, 0xCE11] = true;
                    this.Attributes[GameAttribute.Buff_Icon_Count0, 0x0000CE11] = 1;
                    break;
                case ToonClass.WitchDoctor:
                    /* // unknown
                    this.Attributes[GameAttribute.Skill_Total, ] = 1;  //Mana Trait
                    this.Attributes[GameAttribute.Skill, ] = 1;
                    this.Attributes[GameAttribute.Buff_Active, ] = true;
                    this.Attributes[GameAttribute.Buff_Icon_Count0, ] = 1;
                     */
                    break;
                case ToonClass.Wizard:
                    /* // unknown
                    this.Attributes[GameAttribute.Skill_Total, ] = 1;  //Arcane Power Trait
                    this.Attributes[GameAttribute.Skill, ] = 1;
                    this.Attributes[GameAttribute.Trait, ] = 1;
                    this.Attributes[GameAttribute.Buff_Active, ] = true;
                    this.Attributes[GameAttribute.Buff_Icon_Count0, ] = 1;
                     */
                    break;
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

            //this.Attributes[GameAttribute.Disabled] = true; // we should be making use of these ones too /raist.
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


            // TODO: Fix this shit! Should be actually done so after loading is complete /raist.
            this.Attributes[GameAttribute.Banter_Cooldown, 0xFFFFF] = 0x000007C9;
            this.Attributes[GameAttribute.Buff_Active, 0x20CBE] = true;
            this.Attributes[GameAttribute.Buff_Active, 0x33C40] = false;
            this.Attributes[GameAttribute.Immobolize] = false;
            this.Attributes[GameAttribute.Untargetable] = false;
            this.Attributes[GameAttribute.CantStartDisplayedPowers] = false;
            this.Attributes[GameAttribute.Buff_Icon_Start_Tick0, 0x20CBE] = 0xC1;
            this.Attributes[GameAttribute.Disabled] = false;
            this.Attributes[GameAttribute.Hidden] = false;
            this.Attributes[GameAttribute.Buff_Icon_Count0, 0x33C40] = 0;
            this.Attributes[GameAttribute.Buff_Icon_End_Tick0, 0x20CBE] = 0x7C9;
            this.Attributes[GameAttribute.Loading] = false;
            this.Attributes[GameAttribute.Buff_Icon_End_Tick0, 0x33C40] = 0;
            this.Attributes[GameAttribute.Invulnerable] = false;
            this.Attributes[GameAttribute.Buff_Icon_Count0, 0x20CBE] = 1;
            this.Attributes[GameAttribute.Buff_Icon_Start_Tick0, 0x33C40] = 0;

            #endregion // Attributes

            // unlocking assigned skills
            for (int i = 0; i < this.SkillSet.ActiveSkills.Length; i++)
            {
                this.Attributes[GameAttribute.Skill, this.SkillSet.ActiveSkills[i]] = 1;
                this.Attributes[GameAttribute.Skill_Total, this.SkillSet.ActiveSkills[i]] = 1;
            }

            this.Inventory = new Inventory(this); // Here because it needs attributes /fasbat
        }

        #region game-message handling & consumers

        /// <summary>
        /// Consumes the given game-message.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="message">The GameMessage.</param>
        public void Consume(GameClient client, GameMessage message)
        {
            if (message is AssignActiveSkillMessage) OnAssignActiveSkill(client, (AssignActiveSkillMessage)message);
            else if (message is AssignPassiveSkillMessage) OnAssignPassiveSkill(client, (AssignPassiveSkillMessage)message);
            else if (message is PlayerChangeHotbarButtonMessage) OnPlayerChangeHotbarButtonMessage(client, (PlayerChangeHotbarButtonMessage)message);
            else if (message is TargetMessage) OnObjectTargeted(client, (TargetMessage)message);
            else if (message is PlayerMovementMessage) OnPlayerMovement(client, (PlayerMovementMessage)message);
            else if (message is TryWaypointMessage) OnTryWaypoint(client, (TryWaypointMessage)message);
            else if (message is RequestBuyItemMessage) OnRequestBuyItem(client, (RequestBuyItemMessage)message);
            else if (message is RequestAddSocketMessage) OnRequestAddSocket(client, (RequestAddSocketMessage)message);
            else if (message is HirelingDismissMessage) OnHirelingDismiss();
            else if (message is SocketSpellMessage) OnSocketSpell(client, (SocketSpellMessage)message);
            else if (message is PlayerTranslateFacingMessage) OnTranslateFacing(client, (PlayerTranslateFacingMessage)message);
            else return;
        }

        private void OnTranslateFacing(GameClient client, PlayerTranslateFacingMessage message)
        {
            this.SetFacingRotation(message.Angle);

            World.BroadcastExclusive(new ACDTranslateFacingMessage
            {
                ActorId = this.DynamicID,
                Angle = message.Angle,
                TurnImmediately = message.TurnImmediately
            }, this);
        }

        private void OnAssignActiveSkill(GameClient client, AssignActiveSkillMessage message)
        {
            var oldSNOSkill = this.SkillSet.ActiveSkills[message.SkillIndex]; // find replaced skills SNO.
            if (oldSNOSkill != -1)
            {
                // if old power was socketted, pickup rune
                Item oldRune = this.Inventory.RemoveRune(message.SkillIndex);
                if (oldRune != null)
                {
                    if (!this.Inventory.PickUp(oldRune))
                    {
                        // full inventory, cancel socketting
                        this.Inventory.SetRune(oldRune, oldSNOSkill, message.SkillIndex); // readd old rune
                        return;
                    }
                }
                // switch off old skill in hotbar
                this.Attributes[GameAttribute.Skill, oldSNOSkill] = 0;
                this.Attributes[GameAttribute.Skill_Total, oldSNOSkill] = 0;
            }
            // switch on new skill in hotbar
            this.Attributes[GameAttribute.Skill, message.SNOSkill] = 1;
            this.Attributes[GameAttribute.Skill_Total, message.SNOSkill] = 1;
            this.Attributes.BroadcastChangedIfRevealed();

            foreach (HotbarButtonData button in this.SkillSet.HotBarSkills.Where(button => button.SNOSkill == oldSNOSkill)) // loop through hotbar and replace the old skill with new one
            {
                button.SNOSkill = message.SNOSkill;
            }

            this.SkillSet.ActiveSkills[message.SkillIndex] = message.SNOSkill;
            this.UpdateHeroState();
        }

        private void OnAssignPassiveSkill(GameClient client, AssignPassiveSkillMessage message)
        {
            var oldSNOSkill = this.SkillSet.PassiveSkills[message.SkillIndex]; // find replaced skills SNO.
            if (oldSNOSkill != -1)
            {
                // switch off old passive skill
                this.Attributes[GameAttribute.Trait, oldSNOSkill] = 0;
                this.Attributes[GameAttribute.Skill, oldSNOSkill] = 0;
                this.Attributes[GameAttribute.Skill_Total, oldSNOSkill] = 0;
            }
            // switch on new passive skill
            this.Attributes[GameAttribute.Trait, message.SNOSkill] = 1;
            this.Attributes[GameAttribute.Skill, message.SNOSkill] = 1;
            this.Attributes[GameAttribute.Skill_Total, message.SNOSkill] = 1;
            this.Attributes.BroadcastChangedIfRevealed();
            this.SkillSet.PassiveSkills[message.SkillIndex] = message.SNOSkill;
            this.UpdateHeroState();
        }

        private void OnPlayerChangeHotbarButtonMessage(GameClient client, PlayerChangeHotbarButtonMessage message)
        {
            this.SkillSet.HotBarSkills[message.BarIndex] = message.ButtonData;
        }

        /// <summary>
        /// Sockets skill with rune.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="socketSpellMessage"></param>
        private void OnSocketSpell(GameClient client, SocketSpellMessage socketSpellMessage)
        {
            Item rune = this.Inventory.GetItem(unchecked((uint)socketSpellMessage.RuneDynamicId));
            int PowerSNOId = socketSpellMessage.PowerSNOId;
            int skillIndex = -1; // find index of power in skills
            for (int i = 0; i < this.SkillSet.ActiveSkills.Length; i++)
            {
                if (this.SkillSet.ActiveSkills[i] == PowerSNOId)
                {
                    skillIndex = i;
                    break;
                }
            }
            if (skillIndex == -1)
            {
                // validity of message is controlled on client side, this shouldn't happen
                return;
            }
            Item oldRune = this.Inventory.RemoveRune(skillIndex); // removes old rune (if present)
            if (rune.Attributes[GameAttribute.Rune_Rank] != 0)
            {
                // unattuned rune: pick random color, create new rune, set attunement to new rune and destroy unattuned one
                int rank = rune.Attributes[GameAttribute.Rune_Rank];
                int colorIndex = RandomHelper.Next(0, 5);
                Item newRune = ItemGenerator.Cook(this, "Runestone_" + (char)('A' + colorIndex) + "_0" + rank); // TODO: quite of hack, find better solution /xsochor
                newRune.Attributes[GameAttribute.Rune_Attuned_Power] = PowerSNOId;
                switch (colorIndex)
                {
                    case 0:
                        newRune.Attributes[GameAttribute.Rune_A] = rank;
                        break;
                    case 1:
                        newRune.Attributes[GameAttribute.Rune_B] = rank;
                        break;
                    case 2:
                        newRune.Attributes[GameAttribute.Rune_C] = rank;
                        break;
                    case 3:
                        newRune.Attributes[GameAttribute.Rune_D] = rank;
                        break;
                    case 4:
                        newRune.Attributes[GameAttribute.Rune_E] = rank;
                        break;
                }
                newRune.Owner = this;
                newRune.InventoryLocation.X = rune.InventoryLocation.X; // sets position of original
                newRune.InventoryLocation.Y = rune.InventoryLocation.Y; // sets position of original
                this.Inventory.DestroyInventoryItem(rune); // destroy unattuned rune
                newRune.EnterWorld(this.Position);
                newRune.Reveal(this);
                this.Inventory.SetRune(newRune, PowerSNOId, skillIndex);
            }
            else
            {
                this.Inventory.SetRune(rune, PowerSNOId, skillIndex);
            }
            if (oldRune != null)
            {
                this.Inventory.PickUp(oldRune); // pick removed rune
            }
            this.Attributes.BroadcastChangedIfRevealed();
            UpdateHeroState();
        }

        private void OnObjectTargeted(GameClient client, TargetMessage message)
        {
            Actor actor = this.World.GetActorByDynamicId(message.TargetID);
            if (actor == null) return;

            if ((actor.GBHandle.Type == 1) && (actor.Attributes[GameAttribute.TeamID] == 10))
            {
                this.ExpBonusData.MonsterAttacked(this.InGameClient.Game.TickCounter);
            }

            actor.OnTargeted(this, message);
            this.ExpBonusData.Check(2);
        }

        private void OnPlayerMovement(GameClient client, PlayerMovementMessage message)
        {
            // here we should also be checking the position and see if it's valid. If not we should be resetting player to a good position with ACDWorldPositionMessage
            // so we can have a basic precaution for hacks & exploits /raist.

            if (message.Position != null)
                this.Position = message.Position;

            if (message.Angle != null)
                this.SetFacingRotation(message.Angle.Value);


            var msg = new NotifyActorMovementMessage
            {
                ActorId = message.ActorId,
                Position = this.Position,
                Angle = message.Angle,
                TurnImmediately = false,
                Speed = message.Speed,
                Field5 = message.Field5,
                AnimationTag = message.AnimationTag
            };

            this.RevealScenesToPlayer();
            this.RevealActorsToPlayer();

            this.World.BroadcastExclusive(msg, this); // TODO: We should be instead notifying currentscene we're in. /raist.

            foreach (var actor in GetActorsInRange())
                actor.OnPlayerApproaching(this);

            this.CollectGold();
            this.CollectHealthGlobe();
        }

        private void OnTryWaypoint(GameClient client, TryWaypointMessage tryWaypointMessage)
        {
            var wayPoint = this.World.GetWayPointById(tryWaypointMessage.Field1);
            if (wayPoint == null) return;

            this.Teleport(wayPoint.Position);
        }

        private void OnRequestBuyItem(GameClient client, RequestBuyItemMessage requestBuyItemMessage)
        {
            var vendor = this.SelectedNPC as Vendor;
            if (vendor == null)
                return;
            vendor.OnRequestBuyItem(this, requestBuyItemMessage.ItemId);
        }

        private void OnRequestAddSocket(GameClient client, RequestAddSocketMessage requestAddSocketMessage)
        {
            var item = World.GetItem(requestAddSocketMessage.ItemID);
            if (item == null || item.Owner != this)
                return;
            var jeweler = World.GetActorInstance<Jeweler>();
            if (jeweler == null)
                return;

            jeweler.OnAddSocket(this, item);
        }

        private void OnHirelingDismiss()
        {
            ActiveHireling = null;
        }

        #endregion

        #region update-logic

        public void Update(int tickCounter)
        {
            // Check the Killstreaks
            this.ExpBonusData.Check(0);
            this.ExpBonusData.Check(1);

            // Check if there is an conversation to close in this tick
            Conversations.Update(this.World.Game.TickCounter);

            this.InGameClient.SendTick(); // if there's available messages to send, will handle ticking and flush the outgoing buffer.
        }

        #endregion

        #region enter, leave, reveal handling

        /// <summary>
        /// Revals scenes in player's proximity.
        /// </summary>
        public void RevealScenesToPlayer()
        {
            var scenes = this.GetScenesInRegion(DefaultQueryProximityLenght * 2);

            foreach (var scene in scenes) // reveal scenes in player's proximity.
            {
                if (scene.IsRevealedToPlayer(this)) // if the actors is already revealed skip it.
                    continue; // if the scene is already revealed, skip it.

                if (scene.Parent != null) // if it's a subscene, always make sure it's parent get reveals first and then it reveals his childs.
                    scene.Parent.Reveal(this); 
                else 
                    scene.Reveal(this);
            }
        }

        /// <summary>
        /// Reveals actors in player's proximity.
        /// </summary>
        public void RevealActorsToPlayer()
        {
            var actors = this.GetActorsInRange();

            foreach (var actor in actors) // reveal actors in player's proximity.
            {
                if (actor.Visible == false || actor.IsRevealedToPlayer(this)) // if the actors is already revealed, skip it.
                    continue;

                if (actor.ActorType == ActorType.Gizmo || actor.ActorType == ActorType.Player 
                    || actor.ActorType == ActorType.Monster || actor.ActorType == ActorType.Enviroment 
                    || actor.ActorType == ActorType.Critter || actor.ActorType == ActorType.Item || actor.ActorType == ActorType.ServerProp)
                    actor.Reveal(this);
            }
        }

        public override void OnEnter(World world)
        {
            this.World.Reveal(this);

            this.RevealScenesToPlayer(); // reveal scenes in players proximity.
            this.RevealActorsToPlayer(); // reveal actors in players proximity.
        }

        public override void OnTeleport()
        {
            this.RevealScenesToPlayer(); // reveal scenes in players proximity.
            this.RevealActorsToPlayer(); // reveal actors in players proximity.
        }

        public override void OnLeave(World world)
        {
            this.Conversations.StopAll();
        }

        public override bool Reveal(Player player)
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

            this.Inventory.Reveal(player);

            return true;
        }

        public override bool Unreveal(Player player)
        {
            if (!base.Unreveal(player))
                return false;

            this.Inventory.Unreveal(player);

            return true;
        }

        public override void BeforeChangeWorld()
        {
            this.Inventory.Unreveal(this);
        }

        public override void AfterChangeWorld()
        {
            this.Inventory.Reveal(this);
        }

        #endregion

        #region hero-state

        /// <summary>
        /// Allows hero state message to be sent when hero's some property get's updated.
        /// </summary>
        public void UpdateHeroState()
        {
            this.InGameClient.SendMessage(new HeroStateMessage
            {
                State = this.GetStateData()
            });
        }

        public HeroStateData GetStateData()
        {
            return new HeroStateData()
            {
                Field0 = 0x00000000,
                Field1 = 0x00000000,
                Field2 = 0x00000000,
                Gender = Toon.Gender,
                PlayerSavedData = this.GetSavedData(),
                QuestRewardHistoryEntriesCount = 0x00000000,
                tQuestRewardHistory = QuestRewardHistory,
            };
        }

        #endregion

        #region player attribute handling

        public float InitialAttack // Defines the amount of attack points with which a player starts
        {
            get
            {
                switch (this.Toon.Class)
                {
                    case ToonClass.Barbarian:
                        return 10f + ((this.Toon.Level - 1) * 2);
                    case ToonClass.DemonHunter:
                        return 10f + ((this.Toon.Level - 1) * 2);
                    case ToonClass.Monk:
                        return 10f + ((this.Toon.Level - 1) * 2);
                    case ToonClass.WitchDoctor:
                        return 10f + ((this.Toon.Level - 1) * 2);
                    case ToonClass.Wizard:
                        return 10f + ((this.Toon.Level - 1) * 2);
                }
                return 10f + (this.Toon.Level - 1) * 2;
            }
        }

        public float InitialPrecision // Defines the amount of precision points with which a player starts
        {
            get
            {
                switch (this.Toon.Class)
                {
                    case ToonClass.Barbarian:
                        return 9f + (this.Toon.Level - 1);
                    case ToonClass.DemonHunter:
                        return 11f + ((this.Toon.Level - 1) * 2);
                    case ToonClass.Monk:
                        return 11f + ((this.Toon.Level - 1) * 2);
                    case ToonClass.WitchDoctor:
                        return 9f + ((this.Toon.Level - 1) * 2);
                    case ToonClass.Wizard:
                        return 10f + ((this.Toon.Level - 1) * 2);
                }
                return 10f + ((this.Toon.Level - 1) * 2);
            }
        }

        public float InitialDefense // Defines the amount of defense points with which a player starts
        {
            get
            {
                switch (this.Toon.Class)
                {
                    case ToonClass.Barbarian:
                        return 11f + ((this.Toon.Level - 1) * 2);
                    case ToonClass.DemonHunter:
                        // For DH and Wizard, half the levels (starting with the first) give 2 defense => (Level / 2) * 2
                        // and half give 1 defense => ((Level - 1) / 2) * 1
                        // Note: We can't cancel the twos in ((Level - 1) / 2) * 2 because of integer divison
                        return 9f + (((this.Toon.Level / 2) * 2) + ((this.Toon.Level - 1) / 2));
                    case ToonClass.Monk:
                        return 10f + ((this.Toon.Level - 1) * 2);
                    case ToonClass.WitchDoctor:
                        return 9f + ((this.Toon.Level - 1) * 2);
                    case ToonClass.Wizard:
                        return 8f + (((this.Toon.Level / 2) * 2) + ((this.Toon.Level - 1) / 2));
                }
                return 10f + ((this.Toon.Level - 1) * 2);
            }
        }

        public float InitialVitality // Defines the amount of vitality points with which a player starts
        {
            get
            {
                switch (this.Toon.Class)
                {
                    case ToonClass.Barbarian:
                        return 11f + ((this.Toon.Level - 1) * 2);
                    case ToonClass.DemonHunter:
                        // For DH and Wizard, half the levels give 2 vit => ((Level - 1) / 2) * 2
                        // and half (starting with the first) give 1 vit => (Level / 2) * 1
                        // Note: We can't cancel the twos in ((Level - 1) / 2) * 2 because of integer divison
                        return 9f + ((((this.Toon.Level - 1) / 2) * 2) + (this.Toon.Level / 2));
                    case ToonClass.Monk:
                        return 9f + (this.Toon.Level - 1);
                    case ToonClass.WitchDoctor:
                        return 10f + (this.Toon.Level - 1);
                    case ToonClass.Wizard:
                        return 9f + ((((this.Toon.Level - 1) / 2) * 2) + (this.Toon.Level / 2));
                }
                return 10f + ((this.Toon.Level - 1) * 2);
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
                switch (this.Toon.Class)
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
                switch (this.Toon.Class)
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
                switch (this.Toon.Class)
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
                switch (this.Toon.Class)
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
        #endregion

        #region saved-data

        private PlayerSavedData GetSavedData()
        {
            return new PlayerSavedData()
            {
                HotBarButtons = this.SkillSet.HotBarSkills,
                SkilKeyMappings = this.SkillKeyMappings,

                PlaytimeTotal = 0x00000000,
                WaypointFlags = 0x7FFFFFFF,

                Field4 = new HirelingSavedData()
                {
                    HirelingInfos = this.HirelingInfo,
                    Field1 = 0x00000000,
                    Field2 = 0x00000002,
                },

                Field5 = 0x00006A770,

                LearnedLore = this.LearnedLore,
                snoActiveSkills = this.SkillSet.ActiveSkills,
                snoTraits = this.SkillSet.PassiveSkills,
                SavePointData = new SavePointData { snoWorld = -1, SavepointId = -1, },
                m_SeenTutorials = this.SeenTutorials,
            };
        }

        public SavePointData SavePointData { get; set; }

        public LearnedLore LearnedLore = new LearnedLore()
        {
            Count = 0x00000000,
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
            new HirelingInfo { HirelingIndex = 0x00000000, Field1 = -1, Level = 0x00000000, Field3 = 0x0000, Field4 = false, Skill1SNOId = -1, Skill2SNOId = -1, Skill3SNOId = -1, Skill4SNOId = -1, },
            new HirelingInfo { HirelingIndex = 0x00000001, Field1 = -1, Level = 20, Field3 = 0x00003C19, Field4 = false, Skill1SNOId = 0x000006D3, Skill2SNOId = -1, Skill3SNOId = -1, Skill4SNOId = -1, },
            new HirelingInfo { HirelingIndex = 0x00000002, Field1 = -1, Level = 25, Field3 = 0x00003C19, Field4 = false, Skill1SNOId = -1, Skill2SNOId = -1, Skill3SNOId = -1, Skill4SNOId = -1, },
            new HirelingInfo { HirelingIndex = 0x00000003, Field1 = -1, Level = 30, Field3 = 0x00003C19, Field4 = false, Skill1SNOId = -1, Skill2SNOId = -1, Skill3SNOId = -1, Skill4SNOId = -1, },
        };

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

        #endregion

        #region cooked messages

        public PlayerBannerMessage GetPlayerBanner()
        {
            var playerBanner = D3.GameMessage.PlayerBanner.CreateBuilder()
                .SetPlayerIndex((uint)this.PlayerIndex)
                .SetBanner(this.Toon.Owner.BannerConfiguration)
                .Build();

            return new PlayerBannerMessage() { PlayerBanner = playerBanner };
        }

        public BlacksmithDataInitialMessage GetBlacksmithData()
        {
            var blacksmith = D3.ItemCrafting.CrafterData.CreateBuilder()
                .SetLevel(45)
                .SetCooldownEnd(0)
                .Build();
            return new BlacksmithDataInitialMessage() { CrafterData = blacksmith };
        }

        public JewelerDataInitialMessage GetJewelerData()
        {
            var jeweler = D3.ItemCrafting.CrafterData.CreateBuilder()
                .SetLevel(9)
                .SetCooldownEnd(0)
                .Build();
            return new JewelerDataInitialMessage() { CrafterData = jeweler };
        }

        public MysticDataInitialMessage GetMysticData()
        {
            var mystic = D3.ItemCrafting.CrafterData.CreateBuilder()
                .SetLevel(45)
                .SetCooldownEnd(0)
                .Build();
            return new MysticDataInitialMessage() { CrafterData = mystic };
        }

        #endregion

        #region generic properties

        public static int GetClassSNOId(int gender, ToonClass @class)
        {
            if(gender==0) // male
            {
                switch(@class)
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
            else // female
            {
                switch (@class)
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

        public int ClassSNO
        {
            get
            {
                if (this.Toon.Gender == 0)
                {
                    switch (this.Toon.Class)
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
                    switch (this.Toon.Class)
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
                switch (this.Toon.Class)
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
                switch (this.Toon.Class)
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
                switch (this.Toon.Class)
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

        #endregion

        #region queries

        public List<T> GetRevealedObjects<T>() where T : class, IRevealable
        {
            return this.RevealedObjects.Values.OfType<T>().Select(@object => @object).ToList();
        }

        #endregion

        #region experience handling

        private float GetMaxTotalHitpoints()
        {
            // Defines the Max Total hitpoints for the current level
            // May want to move this into a property if it has to made class-specific
            // This is still a work in progress on getting the right algorithm for all the classes

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

            this.Attributes[GameAttribute.Experience_Next] -= addedExp;

            // Levelup
            if ((this.Attributes[GameAttribute.Experience_Next] <= 0) && (this.Attributes[GameAttribute.Level] < this.Attributes[GameAttribute.Level_Cap]))
            {
                this.Attributes[GameAttribute.Level]++;
                this.Toon.LevelUp();
                if (this.Attributes[GameAttribute.Level] < this.Attributes[GameAttribute.Level_Cap]) { this.Attributes[GameAttribute.Experience_Next] = this.Attributes[GameAttribute.Experience_Next] + LevelBorders[this.Attributes[GameAttribute.Level]]; }
                else { this.Attributes[GameAttribute.Experience_Next] = 0; }

                // 4 main attributes are incremented according to class
                this.Attributes[GameAttribute.Attack] += (int)this.AttackIncrement;
                this.Attributes[GameAttribute.Precision] += (int)this.PrecisionIncrement;
                this.Attributes[GameAttribute.Vitality] += (int)this.VitalityIncrement;
                this.Attributes[GameAttribute.Defense] += (int)this.DefenseIncrement;

                // Hitpoints from level may actually change. This needs to be verified by someone with the beta.
                //this.Attributes[GameAttribute.Hitpoints_Total_From_Level] = this.Attributes[GameAttribute.Level] * this.Attributes[GameAttribute.Hitpoints_Factor_Level];

                // For now, hit points are based solely on vitality and initial hitpoints received.
                // This will have to change when hitpoint bonuses from items are implemented.
                this.Attributes[GameAttribute.Hitpoints_Total_From_Vitality] = this.Attributes[GameAttribute.Vitality] * this.Attributes[GameAttribute.Hitpoints_Factor_Vitality];
                this.Attributes[GameAttribute.Hitpoints_Max] = GetMaxTotalHitpoints();
                this.Attributes[GameAttribute.Hitpoints_Max_Total] = GetMaxTotalHitpoints();

                // On level up, health is set to max
                this.Attributes[GameAttribute.Hitpoints_Cur] = this.Attributes[GameAttribute.Hitpoints_Max_Total];

                this.Attributes.BroadcastChangedIfRevealed();

                this.InGameClient.SendMessage(new PlayerLevel()
                {
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
            if (this.Attributes[GameAttribute.Experience_Next] < 0) 
            { 
                this.Attributes[GameAttribute.Experience_Next] = 0;

            }
            this.Attributes.BroadcastChangedIfRevealed();
            //this.Attributes.SendMessage(this.InGameClient, this.DynamicID); kills the player atm
        }

        #endregion

        #region gold, heath-glob collection

        private void CollectGold()
        {
            List<Item> itemList = this.GetItemsInRange(5f);
            foreach (Item item in itemList)
            {
                if (!Item.IsGold(item.ItemType)) continue;

                List<Player> playersAffected = this.GetPlayersInRange(26f);
                int amount = (int)Math.Max(1, Math.Round((double)item.Attributes[GameAttribute.Gold] / playersAffected.Count, 0));
                item.Attributes[GameAttribute.Gold] = amount;
                foreach (Player player in playersAffected)
                {
                    player.InGameClient.SendMessage(new FloatingAmountMessage()
                    {
                        Place = new WorldPlace()
                        {
                            Position = player.Position,
                            WorldID = player.World.DynamicID,
                        },

                        Amount = amount,
                        Type = FloatingAmountMessage.FloatType.Gold,
                    });

                    player.Inventory.PickUpGold(item.DynamicID);
                }


                item.Destroy();
            }
        }

        private void CollectHealthGlobe()
        {
            var itemList = this.GetItemsInRange(5f);
            foreach (Item item in itemList)
            {
                if (!Item.IsHealthGlobe(item.ItemType)) continue;

                var playersAffected = this.GetPlayersInRange(26f);
                foreach (Player player in playersAffected)
                {
                    foreach (Player targetAffected in playersAffected)
                    {
                        player.InGameClient.SendMessage(new PlayEffectMessage()
                        {
                            ActorId = targetAffected.DynamicID,
                            Effect = Effect.HealthOrbPickup
                        });
                    }

                    //every summon and mercenary owned by you must broadcast their green text to you /H_DANILO
                    player.AddPercentageHP((int)item.Attributes[GameAttribute.Health_Globe_Bonus_Health]);
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

            this.InGameClient.SendMessage(new FloatingNumberMessage()
            {
                ActorID = this.DynamicID,
                Number = quantity,
                Type = FloatingNumberMessage.FloatType.Green
            });

        }

        #endregion

        #region lore

        /// <summary>
        /// Checks if player has lore
        /// </summary>
        /// <param name="loreSNOId"></param>
        /// <returns></returns>
        public bool HasLore(int loreSNOId)
        {
            return LearnedLore.m_snoLoreLearned.Contains(loreSNOId);
        }

        /// <summary>
        /// Plays lore to player
        /// </summary>
        /// <param name="loreSNOId"></param>
        /// <param name="immediately">if false, lore will have new lore button</param>
        public void PlayLore(int loreSNOId, bool immediately)
        {
            // TODO: Fixme
            // play lore to player
            //InGameClient.SendMessage(new Mooege.Net.GS.Message.Definitions.Quest.LoreMessage
            //{
            //    Id = (int)(immediately ? Opcodes.PlayLoreImmediately : Opcodes.PlayLoreWithButton),
            //    LoreSNOId = loreSNOId
            //});
            //if (!HasLore(loreSNOId))
            //{
            //    AddLore(loreSNOId);
            //}
        }

        /// <summary>
        /// Adds lore to player's state
        /// </summary>
        /// <param name="loreSNOId"></param>
        public void AddLore(int loreSNOId) {
            if (this.LearnedLore.Count < this.LearnedLore.m_snoLoreLearned.Length)
            {
                LearnedLore.m_snoLoreLearned[LearnedLore.Count] = loreSNOId;
                LearnedLore.Count++; // Count
                UpdateHeroState();
            }
        }

        #endregion

        #region StoneOfRecall, CubeOfNephalem, CauldonOfJourdan

        public void EnableStoneOfRecall()
        {
            Attributes[GameAttribute.Skill, 0x0002EC66] = 1;
            Attributes[GameAttribute.Skill_Total, 0x0002EC66] = 1;
            Attributes.SendChangedMessage(this.InGameClient);
        }

        public void EnableCauldronOfJordan()
        {         
            Attributes[GameAttribute.ItemMeltUnlocked] = true;
            Attributes.SendChangedMessage(this.InGameClient);
        }

        public void EnableCubeOfNephalem()
        {
            Attributes[GameAttribute.SalvageUnlocked] = true;
            Attributes.SendChangedMessage(this.InGameClient);
        }

        #endregion

    }
}
