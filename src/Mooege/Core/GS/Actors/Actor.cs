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

using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Mooege.Common.Logging;
using Mooege.Core.GS.Actors.Implementations;
using Mooege.Core.GS.Common.Types.Math;
using Mooege.Core.GS.Common.Types.Misc;
using Mooege.Core.GS.Common.Types.SNO;
using Mooege.Core.GS.Games;
using Mooege.Core.GS.Items;
using Mooege.Core.GS.Objects;
using Mooege.Core.GS.Players;
using Mooege.Core.GS.Map;
using Mooege.Net.GS.Message;
using Mooege.Net.GS.Message.Fields;
using Mooege.Net.GS.Message.Definitions.ACD;
using Mooege.Net.GS.Message.Definitions.Misc;
using Mooege.Core.GS.Common.Types.TagMap;
using System;
using Mooege.Core.GS.Powers;
using Mooege.Net.GS.Message.Definitions.Effect;
using Mooege.Net.GS.Message.Definitions.Animation;

namespace Mooege.Core.GS.Actors
{
    public abstract class Actor : WorldObject
    {
        private static readonly Logger Logger = LogManager.CreateLogger();

        /// <summary>
        /// ActorSNO.
        /// </summary>
        public SNOHandle ActorSNO { get; private set; }

        /// <summary>
        /// Gets or sets the sno of the actor used to identify the actor to the player
        /// This is usually the same as actorSNO except for actors that have a GBHandle
        /// There are few exceptions though like the Inn_Zombies that have both.
        /// Used by ACDEnterKnown to name the actor.
        /// </summary>
        public int NameSNOId { get; set; }

        /// <summary>
        /// The actor type.
        /// </summary>
        public abstract ActorType ActorType { get; }

        /// <summary>
        /// Current scene for the actor.
        /// </summary>
        public virtual Scene CurrentScene
        {
            get { return this.World.QuadTree.Query<Scene>(this.Bounds).FirstOrDefault(); }
        }

        /// <summary>
        /// Returns true if actor is already spawned in the world.
        /// </summary>
        public bool Spawned { get; private set; }

        /// <summary>
        /// Default lenght value for region based queries.
        /// </summary>
        public const int DefaultQueryProximityLenght = 240;

        /// <summary>
        /// Default lenght value for range based queries.
        /// </summary>
        public const int DefaultQueryProximityRadius = 120;

        /// <summary>
        /// PRTransform for the actor.
        /// </summary>
        public virtual PRTransform Transform
        {
            get { return new PRTransform { Quaternion = new Quaternion { W = this.RotationW, Vector3D = this.RotationAxis }, Vector3D = this.Position }; }
        }

        /// <summary>
        /// Replaces the actor's rotation with one that rotates along the Z-axis by the specified "facing" angle. 
        /// </summary>
        /// <param name="facingAngle">The angle in radians.</param>
        public void SetFacingRotation(float facingAngle)
        {
            Quaternion q = Quaternion.FacingRotation(facingAngle);
            this.RotationW = q.W;
            this.RotationAxis = q.Vector3D;
        }

        /// <summary>
        /// Tags read from MPQ's for the actor.
        /// </summary>
        public TagMap Tags { get; private set; }

        /// <summary>
        /// Attribute map.
        /// </summary>
        public GameAttributeMap Attributes { get; private set; }

        /// <summary>
        /// Affix list.
        /// </summary>
        public List<Affix> AffixList { get; set; }

        /// <summary>
        /// GBHandle.
        /// </summary>
        public GBHandle GBHandle { get; private set; }

        /// <summary>
        /// Collision flags.
        /// </summary>
        public int CollFlags { get; set; }

        /// <summary>
        /// Gets whether the actor is visible by questrange, privately set on quest progress
        /// </summary>
        public bool Visible { get; private set; }

        /// <summary>
        /// The QuestRange specifies the visibility of an actor, depending on quest progress
        /// </summary>
        private Mooege.Common.MPQ.FileFormats.QuestRange _questRange;

        protected Mooege.Common.MPQ.FileFormats.ConversationList ConversationList;
        public Vector3D CheckPointPosition { get; set; }

        /// <summary>
        /// Returns true if actor has world location.
        /// TODO: I belive this belongs to WorldObject.cs /raist.
        /// </summary>
        public virtual bool HasWorldLocation
        {
            get { return true; }
        }

        public Mooege.Common.MPQ.FileFormats.Actor ActorData { get; private set; }

        /// <summary>
        /// The animation set for actor.
        /// </summary>
        public Mooege.Common.MPQ.FileFormats.AnimSet AnimationSet { get; private set; }

        // TODO: read them from MPQ data's instead /raist.
        public float WalkSpeed = 0.108f;//0.2797852f;
        public float RunSpeed = 0.3598633f;

        // Some ACD uncertainties /komiga.
        public int Field2 = 0x00000000; // TODO: Probably flags or actor type. 0x8==monster, 0x1a==item, 0x10=npc, 0x01=other player, 0x09=player-itself /komiga & /raist.
        public int Field7 = -1;         // Either -1 when ActorNameSNO is -1 or 1 if ActorNameSno is set

        /// <summary>
        /// Quality of the actor as presented to the client. This is either ItemQualityLevel
        /// or LevelArea.SpawnType for monsters or GameBalance.ItemQuality for Items and 0 for all other actors
        /// TODO ACDEnterKnown.Quality seems to be overridden by actor.attributes anyways, at least for items, so im not sure if it maybe has some other purpose -farmy
        /// </summary>
        public virtual int Quality { get; set; }

        public byte Field10 = 0x00;     // always 0 except for a few loots corpses (not all)... 
        public int? Field11 = null;     // never used at all?

        /// <summary>
        /// Gets or sets the MarkerSet from which this item has been loaded.
        /// TODO MarkerSetData for actors is currently never sent with ACDEnterKnown and it is unclear what it is used for
        /// </summary>
        public int? MarkerSetSNO { get; private set; }

        /// <summary>
        /// Gets or sets hte index within the markerset actor list which was the
        /// source of this actor (markerset may point to an encounter which
        /// creates this actor or this actor directly)
        /// TODO MarkerSetData for actors is currently never sent with ACDEnterKnown and it is unclear what it is used for
        /// </summary>
        public int? MarkerSetIndex { get; private set; }

        private int snoTriggeredConversation = -1;

        /// <summary>
        /// Creates a new actor.
        /// </summary>
        /// <param name="world">The world that initially belongs to.</param>
        /// <param name="snoId">SNOId of the actor.</param>
        /// <param name="tags">TagMapEntry dictionary read for the actor from MPQ's..</param>           
        protected Actor(World world, int snoId, TagMap tags)
            : base(world, world.NewActorID)
        {
            this.Attributes = new GameAttributeMap(this);
            this.AffixList = new List<Affix>();

            this.ActorData = (Mooege.Common.MPQ.FileFormats.Actor)Mooege.Common.MPQ.MPQStorage.Data.Assets[SNOGroup.Actor][snoId].Data;
            if (this.ActorData.AnimSetSNO != -1)
                this.AnimationSet = (Mooege.Common.MPQ.FileFormats.AnimSet)Mooege.Common.MPQ.MPQStorage.Data.Assets[SNOGroup.AnimSet][this.ActorData.AnimSetSNO].Data;

            this.ActorSNO = new SNOHandle(SNOGroup.Actor, snoId);
            this.NameSNOId = snoId;
            this.Quality = 0;

            if (ActorData.TagMap.ContainsKey(ActorKeys.TeamID))
                this.Attributes[GameAttribute.TeamID] = ActorData.TagMap[ActorKeys.TeamID];
            this.Spawned = false;
            this.Size = new Size(1, 1);
            this.GBHandle = new GBHandle { Type = -1, GBID = -1 }; // Seems to be the default. /komiga
            this.CollFlags = this.ActorData.ActorCollisionData.ColFlags.I3;

            this.Tags = tags;
            this.ReadTags();

            // Listen for quest progress if the actor has a QuestRange attached to it
            foreach (var quest in World.Game.Quests)
                if (_questRange != null)
                    quest.OnQuestProgress += new Games.Quest.QuestProgressDelegate(quest_OnQuestProgress);
            UpdateQuestRangeVisbility();
        }

        /// <summary>
        /// Creates a new actor.
        /// </summary>
        /// <param name="world">The world that initially belongs to.</param>
        /// <param name="snoId">SNOId of the actor.</param>
        protected Actor(World world, int snoId)
            : this(world, snoId, null)
        { }

        /// <summary>
        /// Unregister from quest events when object is destroyed 
        /// </summary>
        public override void Destroy()
        {
            if (_questRange != null)
                if (World == null)
                    Logger.Debug("World is null? {0}", this.GetType());
                else if (World.Game == null)
                    Logger.Debug("Game is null? {0}", this.GetType());
                else if (World.Game.Quests != null)
                    foreach (var quest in World.Game.Quests)
                        quest.OnQuestProgress -= quest_OnQuestProgress;

            base.Destroy();
        }


        #region enter-world, change-world, teleport helpers

        public void EnterWorld(Vector3D position)
        {
            if (this.Spawned)
                return;

            this.Position = position;
            this.CheckPointPosition = position;

            if (this.World != null) // if actor got into a new world.
                this.World.Enter(this); // let him enter first.
        }

        public virtual void BeforeChangeWorld()
        {

        }

        public virtual void AfterChangeWorld()
        {

        }


        public void ChangeWorld(World world, Vector3D position)
        {
            if (this.World == world)
                return;

            this.Position = position;

            if (this.World != null) // if actor is already in a existing-world
                this.World.Leave(this); // make him leave it first.

            BeforeChangeWorld();

            this.World = world;
            if (this.World != null) // if actor got into a new world.
                this.World.Enter(this); // let him enter first.

            AfterChangeWorld();
            this.CheckPointPosition = position;
            world.BroadcastIfRevealed(this.ACDWorldPositionMessage, this);
        }

        public void ChangeWorld(World world, StartingPoint startingPoint)
        {
            this.RotationAxis = startingPoint.RotationAxis;
            this.RotationW = startingPoint.RotationW;

            this.ChangeWorld(world, startingPoint.Position);
        }

        public void Teleport(Vector3D position)
        {
            this.Position = position;
            this.OnTeleport();
            this.World.BroadcastIfRevealed(this.ACDWorldPositionMessage, this);
        }

        #endregion

        #region Movement/Translation

        public void TranslateFacing(Vector3D target, bool immediately = false)
        {
            float facingAngle = Movement.MovementHelpers.GetFacingAngle(this, target);
            this.SetFacingRotation(facingAngle);

            if (this.World == null) return;

            this.World.BroadcastIfRevealed(new ACDTranslateFacingMessage
            {
                ActorId = DynamicID,
                Angle = facingAngle,
                TurnImmediately = immediately
            }, this);
        }

        #endregion

        #region Effects

        public void PlayEffectGroup(int effectGroupSNO)
        {
            PlayEffect(Effect.PlayEffectGroup, effectGroupSNO);
        }

        public void PlayEffectGroup(int effectGroupSNO, Actor target)
        {
            if (target == null || this.World == null) return;

            World.BroadcastIfRevealed(new EffectGroupACDToACDMessage
            {
                ActorID = this.DynamicID,
                TargetID = target.DynamicID,
                EffectSNOId = effectGroupSNO
            }, this);
        }

        public void PlayHitEffect(int hitEffect, Actor hitDealer)
        {
            if (hitDealer.World == null || this.World == null) return;

            World.BroadcastIfRevealed(new PlayHitEffectMessage
            {
                ActorID = DynamicID,
                HitDealer = hitDealer.DynamicID,
                Field2 = hitEffect,
                Field3 = false
            }, this);
        }

        public void PlayEffect(Effect effect, int? param = null)
        {
            if (this.World == null) return;

            this.World.BroadcastIfRevealed(new PlayEffectMessage
            {
                ActorId = this.DynamicID,
                Effect = effect,
                OptionalParameter = param
            }, this);
        }

        public void AddRopeEffect(int ropeSNO, Actor target)
        {
            if (target == null || target.World == null || this.World == null) return;

            this.World.BroadcastIfRevealed(new RopeEffectMessageACDToACD
            {
                RopeSNO = ropeSNO,
                StartSourceActorId = (int)DynamicID,
                Field2 = 4,
                DestinationActorId = (int)target.DynamicID,
                Field4 = 1
            }, this);
        }

        public void AddRopeEffect(int ropeSNO, Vector3D target)
        {
            if (this.World == null) return;

            this.World.BroadcastIfRevealed(new RopeEffectMessageACDToPlace
            {
                RopeSNO = ropeSNO,
                StartSourceActorId = (int)this.DynamicID,
                Field2 = 4,
                EndPosition = new WorldPlace { Position = target, WorldID = this.World.DynamicID }
            }, this);
        }

        public void AddComplexEffect(int effectGroupSNO, Actor target)
        {
            if (target == null || target.World == null || this.World == null) return;

            this.World.BroadcastIfRevealed(new ComplexEffectAddMessage
            {
                EffectId = (int)this.World.NewActorID, // TODO: maybe not use actor ids?
                Field1 = 1,
                EffectSNO = effectGroupSNO,
                SourceActorId = (int)this.DynamicID,
                TargetActorId = (int)target.DynamicID,
                Field5 = 0,
                Field6 = 0
            }, target);
        }

        public void PlayAnimation(int animationType, int animationSNO, float speed = 1.0f, int? ticksToPlay = null)
        {
            if (this.World == null) return;

            this.World.BroadcastIfRevealed(new PlayAnimationMessage
            {
                ActorID = this.DynamicID,
                Field1 = animationType,
                Field2 = 0,
                tAnim = new PlayAnimationMessageSpec[]
                {
                    new PlayAnimationMessageSpec
                    {
                        Duration = ticksToPlay.HasValue ? ticksToPlay.Value : -2,  // -2 = play animation once through
                        AnimationSNO = animationSNO,
                        PermutationIndex = 0x0,  // TODO: implement variations?
                        Speed = speed,
                    }
                }
            }, this);
        }

        public void PlayActionAnimation(int animationSNO, float speed = 1.0f, int? ticksToPlay = null)
        {
            PlayAnimation(3, animationSNO, speed, ticksToPlay);
        }
        #endregion

        #region reveal & unreveal handling

        private void UpdateQuestRangeVisbility()
        {
            if (_questRange != null)
                Visible = World.Game.Quests.IsInQuestRange(_questRange);
            else
                Visible = true;
        }

        /// <summary>
        /// Returns true if the actor is revealed to player.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <returns><see cref="bool"/></returns>
        public bool IsRevealedToPlayer(Player player)
        {
            return player.RevealedObjects.ContainsKey(this.DynamicID);
        }

        public ACDEnterKnownMessage ACDEnterKnown()
        {
            return new ACDEnterKnownMessage
            {
                ActorID = this.DynamicID,
                ActorSNOId = this.ActorSNO.Id,
                Field2 = this.Field2,
                Field3 = this.HasWorldLocation ? 0 : 1,
                WorldLocation = this.HasWorldLocation ? this.WorldLocationMessage : null,
                InventoryLocation = this.HasWorldLocation ? null : this.InventoryLocationMessage,
                GBHandle = this.GBHandle,
                Field7 = this.Field7,
                NameSNOId = this.NameSNOId,
                Quality = this.Quality,
                Field10 = this.Field10,
                Field11 = this.Field11,
                MarkerSetSNO = this.MarkerSetSNO,
                MarkerSetIndex = this.MarkerSetIndex,
            };
        }

        /// <summary>
        /// Reveals an actor to a player.
        /// </summary>
        /// <returns>true if the actor was revealed or false if the actor was already revealed.</returns>
        public override bool Reveal(Player player)
        {
            if (player.RevealedObjects.ContainsKey(this.DynamicID)) return false; // already revealed
            player.RevealedObjects.Add(this.DynamicID, this);

            var msg = ACDEnterKnown();

            // normaly when we send acdenterknown for players own actor it's set to 0x09. But while sending the acdenterknown for another player's actor we should set it to 0x01. /raist
            if ((this is Player) && this != player)
                msg.Field2 = 0x01;

            player.InGameClient.SendMessage(msg);

            // Collision Flags
            player.InGameClient.SendMessage(new ACDCollFlagsMessage
            {
                ActorID = DynamicID,
                CollFlags = this.CollFlags
            });

            // Send Attributes
            Attributes.SendMessage(player.InGameClient);

            // Actor group
            player.InGameClient.SendMessage(new ACDGroupMessage
            {
                ActorID = DynamicID,
                Group1Hash = -1,
                Group2Hash = -1,
            });

            // Reveal actor (creates actor and makes it visible to the player)
            player.InGameClient.SendMessage(new ACDCreateActorMessage(this.DynamicID));

            // This is always sent even though it doesn't identify the actor. /komiga
            player.InGameClient.SendMessage(new SNONameDataMessage
            {
                Name = this.ActorSNO
            });

            return true;
        }

        /// <summary>
        /// Unreveals an actor from a player.
        /// </summary>
        /// <returns>true if the actor was unrevealed or false if the actor wasn't already revealed.</returns>
        public override bool Unreveal(Player player)
        {
            if (!player.RevealedObjects.ContainsKey(this.DynamicID)) return false; // not revealed yet
            player.InGameClient.SendMessage(new ACDDestroyActorMessage(this.DynamicID));
            player.RevealedObjects.Remove(this.DynamicID);
            return true;
        }

        #endregion

        #region proximity-based query helpers

        #region circurlar region queries

        public List<Player> GetPlayersInRange(float radius = DefaultQueryProximityRadius)
        {
            return this.GetObjectsInRange<Player>(radius);
        }

        public List<Item> GetItemsInRange(float radius = DefaultQueryProximityRadius)
        {
            return this.GetObjectsInRange<Item>(radius);
        }

        public List<Monster> GetMonstersInRange(float radius = DefaultQueryProximityRadius)
        {
            return this.GetObjectsInRange<Monster>(radius);
        }

        public List<Actor> GetActorsInRange(float radius = DefaultQueryProximityRadius)
        {
            return this.GetObjectsInRange<Actor>(radius);
        }

        public List<T> GetActorsInRange<T>(float radius = DefaultQueryProximityRadius) where T : Actor
        {
            return this.GetObjectsInRange<T>(radius);
        }

        public List<Scene> GetScenesInRange(float radius = DefaultQueryProximityRadius)
        {
            return this.GetObjectsInRange<Scene>(radius);
        }

        public List<WorldObject> GetObjectsInRange(float radius = DefaultQueryProximityRadius)
        {
            return this.GetObjectsInRange<WorldObject>(radius);
        }

        public List<T> GetObjectsInRange<T>(float radius = DefaultQueryProximityRadius) where T : WorldObject
        {
            var proximityCircle = new Circle(this.Position.X, this.Position.Y, radius);
            return this.World.QuadTree.Query<T>(proximityCircle);
        }

        #endregion

        #region rectangluar region queries

        public List<Player> GetPlayersInRegion(int lenght = DefaultQueryProximityLenght)
        {
            return this.GetObjectsInRegion<Player>(lenght);
        }

        public List<Item> GetItemsInRegion(int lenght = DefaultQueryProximityLenght)
        {
            return this.GetObjectsInRegion<Item>(lenght);
        }

        public List<Monster> GetMonstersInRegion(int lenght = DefaultQueryProximityLenght)
        {
            return this.GetObjectsInRegion<Monster>(lenght);
        }

        public List<Actor> GetActorsInRegion(int lenght = DefaultQueryProximityLenght)
        {
            return this.GetObjectsInRegion<Actor>(lenght);
        }

        public List<T> GetActorsInRegion<T>(int lenght = DefaultQueryProximityLenght) where T : Actor
        {
            return this.GetObjectsInRegion<T>(lenght);
        }

        public List<Scene> GetScenesInRegion(int lenght = DefaultQueryProximityLenght)
        {
            return this.GetObjectsInRegion<Scene>(lenght);
        }

        public List<WorldObject> GetObjectsInRegion(int lenght = DefaultQueryProximityLenght)
        {
            return this.GetObjectsInRegion<WorldObject>(lenght);
        }

        public List<T> GetObjectsInRegion<T>(int lenght = DefaultQueryProximityLenght) where T : WorldObject
        {
            var proximityRectangle = new Rect(this.Position.X - lenght / 2, this.Position.Y - lenght / 2, lenght, lenght);
            return this.World.QuadTree.Query<T>(proximityRectangle);
        }

        #endregion

        #endregion

        #region events

        private void quest_OnQuestProgress(Quest quest)
        {
            UpdateQuestRangeVisbility();
        }

        public virtual void OnEnter(World world)
        {

        }

        public virtual void OnLeave(World world)
        {

        }

        public void OnActorMove(Actor actor, Vector3D prevPosition)
        {
            // TODO: Unreveal from players that are now outside the actor's range. /komiga
        }

        public virtual void OnTargeted(Player player, Mooege.Net.GS.Message.Definitions.World.TargetMessage message)
        {

        }

        public virtual void OnTeleport()
        {

        }

        /// <summary>
        /// Called when a player moves close to the actor
        /// </summary>
        public virtual void OnPlayerApproaching(Player player)
        {
        }

        #endregion

        #region cooked messages

        public virtual InventoryLocationMessageData InventoryLocationMessage
        {
            // Only used in Item; stubbed here to prevent an overrun in some cases. /komiga
            get { return new InventoryLocationMessageData { OwnerID = 0, EquipmentSlot = 0, InventoryLocation = new Vector2D() }; }
        }

        public virtual ACDWorldPositionMessage ACDWorldPositionMessage
        {
            get { return new ACDWorldPositionMessage { ActorID = this.DynamicID, WorldLocation = this.WorldLocationMessage }; }
        }

        public virtual ACDInventoryPositionMessage ACDInventoryPositionMessage
        {
            get
            {
                return new ACDInventoryPositionMessage()
                {
                    ItemId = this.DynamicID,
                    InventoryLocation = this.InventoryLocationMessage,
                    Field2 = 1 // TODO: find out what this is and why it must be 1...is it an enum?
                };
            }
        }

        public virtual WorldLocationMessageData WorldLocationMessage
        {
            get
            {
                return new WorldLocationMessageData { Scale = this.Scale, Transform = this.Transform, WorldID = this.World.DynamicID };
            }
        }

        #endregion

        #region tag-readers

        /// <summary>
        /// Reads known tags from TagMapEntry and set the proper values.
        /// </summary>
        protected virtual void ReadTags()
        {
            if (this.Tags == null) return;

            // load scale from actor data and override it with marker tags if one is set
            this.Scale = ActorData.TagMap.ContainsKey(ActorKeys.Scale) ? ActorData.TagMap[ActorKeys.Scale] : 1;
            this.Scale = Tags.ContainsKey(MarkerKeys.Scale) ? Tags[MarkerKeys.Scale] : this.Scale;


            if (Tags.ContainsKey(MarkerKeys.QuestRange))
            {
                int snoQuestRange = Tags[MarkerKeys.QuestRange].Id;
                if (Mooege.Common.MPQ.MPQStorage.Data.Assets[SNOGroup.QuestRange].ContainsKey(snoQuestRange))
                    _questRange = Mooege.Common.MPQ.MPQStorage.Data.Assets[SNOGroup.QuestRange][snoQuestRange].Data as Mooege.Common.MPQ.FileFormats.QuestRange;
                else Logger.Warn("Actor {0} is tagged with unknown QuestRange {1}", NameSNOId, snoQuestRange);
            }

            if (Tags.ContainsKey(MarkerKeys.ConversationList))
            {
                int snoConversationList = Tags[MarkerKeys.ConversationList].Id;
                if (Mooege.Common.MPQ.MPQStorage.Data.Assets[SNOGroup.ConversationList].ContainsKey(snoConversationList))
                    ConversationList = Mooege.Common.MPQ.MPQStorage.Data.Assets[SNOGroup.ConversationList][snoConversationList].Data as Mooege.Common.MPQ.FileFormats.ConversationList;
                else Logger.Warn("Actor {0} is tagged with unknown ConversationList {1}", NameSNOId, snoConversationList);
            }


            if (this.Tags.ContainsKey(MarkerKeys.TriggeredConversation))
                snoTriggeredConversation = Tags[MarkerKeys.TriggeredConversation].Id;
        }

        #endregion

        #region movement

        public void Move(Vector3D point, float facingAngle)
        {
            this.Position = point;  // TODO: will need update Position over time, not instantly.
            this.SetFacingRotation(facingAngle);

            // find suitable movement animation
            int aniTag;
            if (this.AnimationSet == null)
                aniTag = -1;
            else if (this.AnimationSet.TagExists(Mooege.Common.MPQ.FileFormats.AnimationTags.Walk))
                aniTag = this.AnimationSet.GetAnimationTag(Mooege.Common.MPQ.FileFormats.AnimationTags.Walk);
            else if (this.AnimationSet.TagExists(Mooege.Common.MPQ.FileFormats.AnimationTags.Run))
                aniTag = this.AnimationSet.GetAnimationTag(Mooege.Common.MPQ.FileFormats.AnimationTags.Run);
            else
                aniTag = -1;

            var movementMessage = new ACDTranslateNormalMessage
            {
                ActorId = (int)this.DynamicID,
                Position = point,
                Angle = facingAngle,
                TurnImmediately = false,
                Speed = this.WalkSpeed,
                Field5 = 0,
                AnimationTag = aniTag
            };

            this.World.BroadcastIfRevealed(movementMessage, this);
        }

        public void MoveSnapped(Vector3D point, float facingAngle)
        {
            this.Position = point;
            this.SetFacingRotation(facingAngle);

            this.World.BroadcastIfRevealed(new ACDTranslateSnappedMessage
            {
                ActorId = (int)this.DynamicID,
                Position = point,
                Angle = facingAngle,
                Field3 = false,
                Field4 = 0x900  // TODO: figure out when to use this field
            }, this);
        }

        #endregion

        public override string ToString()
        {
            return string.Format("[Actor] [Type: {0}] SNOId:{1} DynamicId: {2} Position: {3} Name: {4}", this.ActorType, this.ActorSNO.Id, this.DynamicID, this.Position, this.ActorSNO.Name);
        }
    }

    // This is used for GBHandle.Type; uncertain if used elsewhere
    public enum GBHandleType : int
    {
        Invalid = 0,
        Monster = 1,
        Gizmo = 2,
        ClientEffect = 3,
        ServerProp = 4,
        Environment = 5,
        Critter = 6,
        Player = 7,
        Item = 8,
        AxeSymbol = 9,
        Projectile = 10,
        CustomBrain = 11
    }

    // This should probably be the same as GBHandleType (probably merge them once all actor classes are created)
    public enum ActorType
    {
        Invalid = 0,
        Monster = 1,
        Gizmo = 2,
        ClientEffect = 3,
        ServerProp = 4,
        Enviroment = 5,
        Critter = 6,
        Player = 7,
        Item = 8,
        AxeSymbol = 9,
        Projectile = 10,
        CustomBrain = 11
    }
}
