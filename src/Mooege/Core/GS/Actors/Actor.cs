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
using System.Linq;
using System.Windows;
using Mooege.Common;
using Mooege.Common.MPQ.FileFormats.Types;
using Mooege.Core.GS.Actors.Implementations;
using Mooege.Core.GS.Common.Types.Math;
using Mooege.Core.GS.Common.Types.Misc;
using Mooege.Core.GS.Common.Types.SNO;
using Mooege.Core.GS.Markers;
using Mooege.Core.GS.Objects;
using Mooege.Core.GS.Map;
using Mooege.Core.GS.Players;
using Mooege.Net.GS.Message;
using Mooege.Net.GS.Message.Definitions.World;
using Mooege.Net.GS.Message.Fields;
using Mooege.Net.GS.Message.Definitions.ACD;
using Mooege.Net.GS.Message.Definitions.Misc;
using Mooege.Core.Common.Items;

namespace Mooege.Core.GS.Actors
{
    public abstract class Actor : WorldObject
    {
        private static readonly Logger Logger = LogManager.CreateLogger();

        /// <summary>
        /// SNO Id of the actor.
        /// </summary>
        public int SNOId { get; protected set; }

        /// <summary>
        /// SNOName - TODO: we can handle this better /raist.
        /// </summary>
        public SNOName SNOName { get; private set; }

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
        /// Default query radius value.
        /// </summary>
        protected const int DefaultQueryProximity = 240;

        /// <summary>
        /// PRTransform for the actor.
        /// </summary>
        public virtual PRTransform Transform
        {
            get { return new PRTransform { Quaternion = new Quaternion { W = this.RotationAmount, Vector3D = this.RotationAxis }, Vector3D = this.Position }; }
        }

        /// <summary>
        /// Tags read from MPQ's for the actor.
        /// </summary>
        public Dictionary<int, TagMapEntry> Tags { get; private set; }

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
        /// Returns true if actor has world location. TODO: I belive this belongs to WorldObject.cs /raist.
        /// </summary>
        public virtual bool HasWorldLocation
        {
            get { return true; }
        }

        // Some ACD uncertainties /komiga.
        public int Field2 = 0x00000000; // TODO: Probably flags or actor type. 0x8==monster, 0x1a==item, 0x10=npc, 0x01=other player, 0x09=player-itself /komiga & /raist.
        public int Field3 = 0x00000001; // TODO: What dis? <-- I guess its just 0 for WorldItem and 1 for InventoryItem // Farmy
        public int Field7 = -1;
        public int Field8 = -1; // Animation set SNO?
        public int Field9; // SNOName.Group?
        public byte Field10 = 0x00;
        public int? Field11 = null;
        public int? Field12 = null;
        public int? Field13 = null;

        /// <summary>
        /// Creates a new actor.
        /// </summary>
        /// <param name="world">The world that initially belongs to.</param>
        /// <param name="snoId">SNOId of the actor.</param>
        /// <param name="tags">TagMapEntry dictionary read for the actor from MPQ's..</param>           
        protected Actor(World world, int snoId, Dictionary<int, TagMapEntry> tags)
            : base(world, world.NewActorID)
        {
            this.SNOId = snoId;
            this.SNOName = new SNOName { Group = SNOGroup.Actor, SNOId = this.SNOId };
            this.Spawned = false;
            this.Size = new Size(1, 1);
            this.Attributes = new GameAttributeMap();
            this.AffixList = new List<Affix>();
            this.GBHandle = new GBHandle { Type = -1, GBID = -1 }; // Seems to be the default. /komiga
            this.CollFlags = 0x00000000;

            this.Tags = tags;
            this.ReadTags();
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
        /// Creates a new world.
        /// </summary>
        /// <param name="world">The world that initially belongs to.</param>
        protected Actor(World world)
            : this(world, -1, null)
        { }

        #region enter-world, change-world, teleport helpers

        public void EnterWorld(Vector3D position)
        {
            if (this.Spawned)
                return;

            this.Position = position;

            if (this.World != null) // if actor got into a new world.
                this.World.Enter(this); // let him enter first.
        }

        public void ChangeWorld(World world, Vector3D position)
        {
            if (this.World == world)
                return;

            this.Position = position;

            if (this.World != null) // if actor is already in a existing-world
                this.World.Leave(this); // make him leave it first.

            this.World = world;
            if (this.World != null) // if actor got into a new world.
                this.World.Enter(this); // let him enter first.

            world.BroadcastIfRevealed(this.ACDWorldPositionMessage, this);
        }

        public void ChangeWorld(World world, StartingPoint startingPoint)
        {
            this.RotationAxis = startingPoint.RotationAxis;
            this.RotationAmount = startingPoint.RotationAmount;

            this.ChangeWorld(world, startingPoint.Position);
        }

        public void Teleport(Vector3D position)
        {
            this.Position = position;
            this.OnTeleport();
            this.World.BroadcastIfRevealed(this.ACDWorldPositionMessage, this);
        }

        #endregion

        #region reveal & unreveal handling

        /// <summary>
        /// Returns true if the actor is revealed to player.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <returns><see cref="bool"/></returns>
        public bool IsRevealedToPlayer(Player player)
        {
            return player.RevealedObjects.ContainsKey(this.DynamicID);
        }

        /// <summary>
        /// Reveals an actor to a player.
        /// </summary>
        /// <returns>true if the actor was revealed or false if the actor was already revealed.</returns>
        public override bool Reveal(Player player)
        {
            if (player.RevealedObjects.ContainsKey(this.DynamicID)) return false; // already revealed
            player.RevealedObjects.Add(this.DynamicID, this);

            var msg = new ACDEnterKnownMessage
            {
                ActorID = this.DynamicID,
                ActorSNO = this.SNOId,
                Field2 = Field2,
                Field3 = Field3, // this.hasWorldLocation ? 0 : 1;
                WorldLocation = this.HasWorldLocation ? this.WorldLocationMessage : null,
                InventoryLocation = this.HasWorldLocation ? null : this.InventoryLocationMessage,
                GBHandle = this.GBHandle,
                Field7 = Field7,
                Field8 = Field8,
                Field9 = Field9,
                Field10 = Field10,
                Field11 = Field11,
                Field12 = Field12,
                Field13 = Field13,
            };

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
            Attributes.SendMessage(player.InGameClient, DynamicID);

            // Actor group
            player.InGameClient.SendMessage(new ACDGroupMessage
            {
                ActorID = DynamicID,
                Field1 = -1,
                Field2 = -1,
            });

            // Reveal actor (creates actor and makes it visible to the player)
            player.InGameClient.SendMessage(new ACDCreateActorMessage(this.DynamicID));

            // This is always sent even though it doesn't identify the actor. /komiga
            player.InGameClient.SendMessage(new SNONameDataMessage
                                                {
                Name = this.SNOName
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

        public List<Player> GetPlayersInRange(float radius = DefaultQueryProximity)
        {
            return this.GetObjectsInRange<Player>(radius);
        }

        public List<Item> GetItemsInRange(float radius = DefaultQueryProximity)
        {
            return this.GetObjectsInRange<Item>(radius);
        }

        public List<Monster> GetMonstersInRange(float radius = DefaultQueryProximity)
        {
            return this.GetObjectsInRange<Monster>(radius);
        }

        public List<Actor> GetActorsInRange(float radius = DefaultQueryProximity)
        {
            return this.GetObjectsInRange<Actor>(radius);
        }

        public List<T> GetActorsInRange<T>(float radius = DefaultQueryProximity) where T : Actor
        {
            return this.GetObjectsInRange<T>(radius);
        }

        public List<Scene> GetScenesInRange(float radius = DefaultQueryProximity)
        {
            return this.GetObjectsInRange<Scene>(radius);
        }

        public List<WorldObject> GetObjectsInRange(float radius = DefaultQueryProximity)
        {
            return this.GetObjectsInRange<WorldObject>(radius);
        }

        public List<T> GetObjectsInRange<T>(float radius=DefaultQueryProximity) where T : WorldObject
        {
            var proximityCircle = new Circle(this.Position.X, this.Position.Y, radius);
            return this.World.QuadTree.Query<T>(proximityCircle);
        }

        #endregion

        #region rectangluar region queries

        public List<Player> GetPlayersInRegion(int lenght = DefaultQueryProximity)
        {
            return this.GetObjectsInRegion<Player>(lenght);
        }

        public List<Item> GetItemsInRegion(int lenght = DefaultQueryProximity)
        {
            return this.GetObjectsInRegion<Item>(lenght);
        }

        public List<Monster> GetMonstersInRegion(int lenght = DefaultQueryProximity)
        {
            return this.GetObjectsInRegion<Monster>(lenght);
        }

        public List<Actor> GetActorsInRegion(int lenght = DefaultQueryProximity)
        {
            return this.GetObjectsInRegion<Actor>(lenght);
        }

        public List<T> GetActorsInRegion<T>(int lenght = DefaultQueryProximity) where T : Actor
        {
            return this.GetObjectsInRegion<T>(lenght);
        }

        public List<Scene> GetScenesInRegion(int lenght = DefaultQueryProximity)
        {
            return this.GetObjectsInRegion<Scene>(lenght);
        }

        public List<WorldObject> GetObjectsInRegion(int lenght = DefaultQueryProximity)
        {
            return this.GetObjectsInRegion<WorldObject>(lenght);
        }

        public List<T> GetObjectsInRegion<T>(int lenght = DefaultQueryProximity) where T : WorldObject
        {
            var proximityRectangle = new Rect(this.Position.X - lenght / 2, this.Position.Y - lenght / 2, lenght, lenght);
            return this.World.QuadTree.Query<T>(proximityRectangle);
        }

        #endregion

        #endregion

        #region events

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

        public virtual void OnTargeted(Player player, TargetMessage message)
        {

        }

        public virtual void OnTeleport()
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

            if (this.Tags.ContainsKey((int)MarkerTagTypes.Scale))
                this.Scale = this.Tags[(int)MarkerTagTypes.Scale].Float0;
        }

        #endregion

        public override string ToString()
        {
            return string.Format("[Actor] [Type: {0}] SNOId:{1} DynamicId: {2} Position: {3} Name: {4}", this.ActorType, this.SNOId, this.DynamicID, this.Position, this.SNOName.Name);
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
