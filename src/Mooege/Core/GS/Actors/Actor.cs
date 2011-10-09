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

using System;
using System.Collections.Generic;
using Mooege.Core.GS.Player;
using Mooege.Core.GS.Objects;
using Mooege.Core.GS.Map;
using Mooege.Net.GS;
using Mooege.Net.GS.Message;
using Mooege.Net.GS.Message.Fields;
using Mooege.Net.GS.Message.Definitions.ACD;
using Mooege.Net.GS.Message.Definitions.Misc;
using Mooege.Net.GS.Message.Definitions.Attribute;
using Mooege.Core.Common.Items;

// TODO: Actor needs to use a nullable object for world position and a getter for inventory position (which is only used by Item)
//       Or just a boolean parameter in Reveal to specify which location member is to be sent/nulled

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
                this.World.OnActorMove(this, old); // TODO: Should notify its scene instead
            }
        }

        public abstract ActorType ActorType { get; }

        public GameAttributeMap Attributes { get; private set; }
        public List<Affix> AffixList { get; set; }

        public int ActorSNO { get; set; }
        public GBHandle GBHandle { get; set; }

        // Some ACD uncertainties
        public int Field2 = 0x00000000; // TODO: Probably flags or actor type. 0x8==monster, 0x1a==item, 0x10=npc
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


        // NOTE: May want pack all of the location stuff into a PRTransform field called Position or Transform
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
            this.ActorSNO = -1;
            this.GBHandle = new GBHandle();
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

        public virtual void OnTargeted(Mooege.Core.GS.Player.Player player)
        {
        }

        /// <summary>
        /// Reveals an actor to the client.
        /// </summary>
        public override void Reveal(Mooege.Core.GS.Player.Player player)
        {
            // The server always sends all of these messages together when revealing an actor.
            // To completly reveal all properties of an actor, the client needs more than just ACDEnterKnown

            if (player.RevealedObjects.ContainsKey(this.DynamicID)) return; // already revealed
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
                CollFlags = 0x00000080, // TODO: ACtor needs collision flags property
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

            // Reveal actor (Creates actor and makes him visible in the client)
            player.InGameClient.SendMessage(new ANNDataMessage(Opcodes.ANNDataMessage7)
            {
                ActorID = DynamicID
            });
        }

        public override void Unreveal(Mooege.Core.GS.Player.Player player)
        {
            if (!player.RevealedObjects.ContainsKey(this.DynamicID)) return; // not revealed yet
            // NOTE: This message ID is probably "DestroyActor". ANNDataMessage7 is used for addition/creation
            player.InGameClient.SendMessageNow(new ANNDataMessage(Opcodes.ANNDataMessage6) { ActorID = this.DynamicID });
            player.RevealedObjects.Remove(this.DynamicID);
        }
    }
}
