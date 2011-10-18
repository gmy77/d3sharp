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
using Mooege.Core.GS.Objects;
using Mooege.Core.GS.Map;
using Mooege.Net.GS.Message;
using Mooege.Net.GS.Message.Definitions.World;
using Mooege.Net.GS.Message.Fields;
using Mooege.Net.GS.Message.Definitions.ACD;
using Mooege.Net.GS.Message.Definitions.Misc;
using Mooege.Core.Common.Items;

// TODO: Need to move all of the remaining ACD fields into Actor (such as the affix list)

namespace Mooege.Core.GS.Actors
{
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
        Player,
        NPC,
        Monster,
        Item,
        Portal
    }

    // Base actor
    public abstract class Actor : WorldObject
    {
        // Actors can change worlds and have a specific addition/removal scheme
        // We'll just override the setter to handle all of this automagically
        public override World World
        {
            set
            {
                if (this._world != value)
                {
                    if (this._world != null)
                        this._world.Leave(this);
                    this._world = value;
                    if (this._world != null)
                        this._world.Enter(this);
                }
            }
        }

        public sealed override Vector3D Position
        {
            set
            {
                var old = new Vector3D(this._position);
                this._position.Set(value);
                this.OnMove(old);
                this.World.OnActorPositionChange(this, old); // TODO: Should notify its scene instead
            }
        }

        public abstract ActorType ActorType { get; }

        public GameAttributeMap Attributes { get; private set; }
        public List<Affix> AffixList { get; set; }

        protected int _actorSNO;
        public int ActorSNO
        {
            get { return _actorSNO; }
            set
            {
                this._actorSNO = value;
                this.SNOName.Handle = this.ActorSNO;
            }
        }

        public int CollFlags { get; set; }
        public GBHandle GBHandle { get; private set; }
        public SNOName SNOName { get; private set; }

        // Some ACD uncertainties
        public int Field2 = 0x00000000; // TODO: Probably flags or actor type. 0x8==monster, 0x1a==item, 0x10=npc, 0x01=other player, 0x09=player-itself
        public int Field3 = 0x00000001; // TODO: What dis? <-- I guess its just 0 for WorldItem and 1 for InventoryItem // Farmy
        public int Field7 = -1;
        public int Field8 = -1; // Animation set SNO?
        public int Field9; // SNOName.Group?
        public byte Field10 = 0x00;
        public int? /* sno */ Field11 = null;
        public int? Field12 = null;
        public int? Field13 = null;

        public virtual WorldLocationMessageData WorldLocationMessage
        {
            get
            {
                return new WorldLocationMessageData { Scale = this.Scale, Transform = this.Transform, WorldID = this.World.DynamicID };
            }
        }

        public virtual bool HasWorldLocation
        {
            get { return true; }
        }

        public virtual PRTransform Transform
        {
            get { return new PRTransform { Rotation = new Quaternion { Amount = this.RotationAmount, Axis = this.RotationAxis }, ReferencePoint = this.Position }; }
        }

        // Only used in Item; stubbed here to prevent an overrun in some cases. /komiga
        public virtual InventoryLocationMessageData InventoryLocationMessage
        {
            get { return new InventoryLocationMessageData{ OwnerID = 0, EquipmentSlot = 0, InventoryLocation = new IVector2D() }; }
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
                    ItemID = this.DynamicID,
                    InventoryLocation = this.InventoryLocationMessage,
                    Field2 = 1 // TODO: find out what this is and why it must be 1...is it an enum?
                };
            }
        }

        protected Actor(World world, uint dynamicID)
            : base(world, dynamicID)
        {
            this.Attributes = new GameAttributeMap();
            this.AffixList = new List<Affix>();
            this.GBHandle = new GBHandle() { Type = -1, GBID = -1 }; // Seems to be the default. /komiga
            this.SNOName = new SNOName() { Group = 0x00000001, Handle = this.ActorSNO };
            this.ActorSNO = -1;
            this.CollFlags = 0x00000000;
            this.Scale = 1.0f;
            this.RotationAmount = 0.0f;
            this.RotationAxis.Set(0.0f, 0.0f, 1.0f);
        }

        // NOTE: When using this, you should *not* set the actor's world. It is done for you
        public void TransferTo(World targetWorld, Vector3D pos)
        {
            this.Position = pos;
            this.World = targetWorld; // Will Leave() from its current world and then Enter() to the target world
        }

        public virtual void OnEnter(World world)
        {
        }

        public virtual void OnLeave(World world)
        {
        }

        protected virtual void OnMove(Vector3D prevPosition)
        {
        }

        public virtual void OnTargeted(Mooege.Core.GS.Player.Player player, TargetMessage message)
        {
        }

        /// <summary>
        /// Reveals an actor to a player.
        /// </summary>
        /// <returns>true if the actor was revealed or false if the actor was already revealed.</returns>
        public override bool Reveal(Mooege.Core.GS.Player.Player player)
        {
            if (player.RevealedObjects.ContainsKey(this.DynamicID)) return false; // already revealed
            player.RevealedObjects.Add(this.DynamicID, this);

            var msg = new ACDEnterKnownMessage
            {
                ActorID = this.DynamicID,
                ActorSNO = this.ActorSNO,
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
            if ((this is Player.Player) && this != player) 
                msg.Field2 = 0x01; 

            player.InGameClient.SendMessage(msg);

            // Affixes of the actor, two messages with 1 and 2,i guess prefix and suffix so it does not
            // make sense to send the same list twice. server does not do this
            var affixGbis = new int[AffixList.Count];
            for (int i = 0; i < AffixList.Count; i++)
            {
                affixGbis[i] = AffixList[i].AffixGbid;
            }

            player.InGameClient.SendMessage(new AffixMessage()
            {
                ActorID = DynamicID,
                Field1 = 0x00000001,
                aAffixGBIDs = affixGbis,
            });

            player.InGameClient.SendMessage(new AffixMessage()
            {
                ActorID = DynamicID,
                Field1 = 0x00000002,
                aAffixGBIDs = affixGbis,
            });

            // Collision Flags
            player.InGameClient.SendMessage(new ACDCollFlagsMessage()
            {
                ActorID = DynamicID,
                CollFlags = this.CollFlags
            });

            // Send Attributes
            Attributes.SendMessage(player.InGameClient, DynamicID);

            // Actor group
            player.InGameClient.SendMessage(new ACDGroupMessage()
            {
                ActorID = DynamicID,
                Field1 = -1,
                Field2 = -1,
            });

            // Reveal actor (creates actor and makes it visible to the player)
            player.InGameClient.SendMessage(new ANNDataMessage(Opcodes.ANNDataMessage7)
            {
                ActorID = DynamicID
            });

            // This is always sent even though it doesn't identify the actor. /komiga
            player.InGameClient.SendMessage(new SNONameDataMessage()
            {
                Name = this.SNOName
            });

            return true;
        }

        /// <summary>
        /// Unreveals an actor from a player.
        /// </summary>
        /// <returns>true if the actor was unrevealed or false if the actor wasn't already revealed.</returns>
        public override bool Unreveal(Mooege.Core.GS.Player.Player player)
        {
            if (!player.RevealedObjects.ContainsKey(this.DynamicID)) return false; // not revealed yet
            // NOTE: This message ID is probably "DestroyActor". ANNDataMessage7 is used for addition/creation
            player.InGameClient.SendMessage(new ANNDataMessage(Opcodes.ANNDataMessage6) { ActorID = this.DynamicID });
            player.RevealedObjects.Remove(this.DynamicID);
            return true;
        }
    }
}
