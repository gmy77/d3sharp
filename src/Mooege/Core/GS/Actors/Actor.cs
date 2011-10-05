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
using Mooege.Core.GS.Game;
using Mooege.Core.GS.Objects;
using Mooege.Core.GS.Map;
using Mooege.Net.GS;
using Mooege.Net.GS.Message;
using Mooege.Net.GS.Message.Fields;
using Mooege.Net.GS.Message.Definitions.ACD;
using Mooege.Net.GS.Message.Definitions.Misc;
using Mooege.Net.GS.Message.Definitions.Attribute;

// TODO: Actor needs to use a nullable object for world position and a getter for inventory position (which is only used by Item)
//       Or just a boolean parameter in Reveal to specify which location member is to be sent/nulled

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
        GenericActor, // TODO: Should remove this when everything is properly laid out and Actor should be abstract
        Player, // Message structure and dumped data suggests that the actor is a player
        NPC,
        Monster,
        Item,
        Portal,
        PowerEffect
    }

    // Base actor
    public class Actor : IDynamicObject, IWorldObject
    {
        public Mooege.Core.GS.Game.Game Game { get; private set; }
        public uint DynamicID { get; private set; }

        public virtual ActorType ActorType { get { return ActorType.GenericActor; } }

        public World World { get; set; }

        public float Scale { get; set; }
        public float RotationAmount { get; set; }
        public Vector3D RotationAxis { get; set; }
        public Vector3D Position { get; set; }

        public GameAttributeMap Attributes { get; private set; }

        public int AppearanceSNO { get; set; }
        public GBHandle GBHandle { get; set; }

        // Some ACD uncertainties
        public int Field2 = 0x00000000; // TODO: Probably flags or actor type. 0x8==monster, 0x1a==item, 0x10=npc
        public int Field3 = 0x00000001; // TODO: What dis?
        public int Field7 = -1;
        public int Field8 = -1; // Animation set SNO?
        public int Field9; // SNOName.Group?
        public byte Field10 = 0x00;
        public int? /* sno */ Field11 = null;
        public int? Field12 = null;
        public int? Field13 = null;

        public virtual WorldLocationMessageData WorldLocationMessage
        {
            get { return new WorldLocationMessageData { Scale = this.Scale, Transform = this.Transform, WorldID = this.World.DynamicID }; }
        }

        // NOTE: May want pack all of the location stuff into a PRTransform field called Position or Transform
        public virtual PRTransform Transform
        {
            get { return new PRTransform { Rotation = new Quaternion { Amount = this.RotationAmount, Axis = this.RotationAxis }, ReferencePoint = this.Position }; }
        }

        // Only used in Item
        public virtual InventoryLocationMessageData InventoryLocationMessage
        {
            get { return null; }
        }

        public Actor(World world, uint dynamicID)
        {
            // NOTE: Base class does _not_ add the actor to the game since deriving classes may want to place themselves in a separate collection (like Item)
            if (world == null)
                throw new Exception("World cannot be null");
            this.Game = world.Game;
            this.DynamicID = dynamicID;
            this.World = world;
            this.RotationAxis = new Vector3D();
            this.Position = new Vector3D();
            this.Attributes = new GameAttributeMap();
            this.AppearanceSNO = -1;
            this.GBHandle = new GBHandle();
        }

        public virtual void Reveal(Player player)
        {
            if (player.RevealedActors.ContainsKey(this.DynamicID)) return; // already revealed
            player.RevealedActors.Add(this.DynamicID, this);

            var msg = new ACDEnterKnownMessage
            {
                ActorID = this.DynamicID,
                AppearanceSNO = AppearanceSNO,
                Field2 = Field2,
                Field3 = Field3,
                WorldLocation = this.WorldLocationMessage,
                InventoryLocation = this.InventoryLocationMessage,
                GBHandle = GBHandle,
                Field7 = Field7,
                Field8 = Field8,
                Field9 = Field9,
                Field10 = Field10,
                Field11 = Field11,
                Field12 = Field12,
                Field13 = Field13,
            };
            player.InGameClient.SendMessageNow(msg);
        }

        public virtual void Destroy(Player player)
        {
            if (!player.RevealedActors.ContainsKey(this.DynamicID)) return; // not revealed yet

            player.InGameClient.SendMessageNow(new ANNDataMessage(Opcodes.ANNDataMessage1) { ActorID = this.DynamicID, });
            player.RevealedActors.Remove(this.DynamicID);
        }

        public void SendWorldPosition(Player player)
        {
            player.InGameClient.SendMessage(new ACDWorldPositionMessage
            {
                ActorID = this.DynamicID,
                WorldLocation = this.WorldLocationMessage
            });
            player.InGameClient.FlushOutgoingBuffer();
        }

        private void SendAttributes(List<NetAttributeKeyValue> netAttributesList, Player player)
        {
            GameClient client = player.InGameClient;
            // Attributes can't be send all together
            // must be split up to part of max 15 attributes at once
            var tempList = new List<NetAttributeKeyValue>(netAttributesList);

            while (tempList.Count > 0)
            {
                int selectCount = (tempList.Count > 15) ? 15 : tempList.Count;
                client.SendMessage(new AttributesSetValuesMessage()
                {
                    ActorID = this.DynamicID,
                    atKeyVals = tempList.GetRange(0, selectCount).ToArray(),
                });
                tempList.RemoveRange(0, selectCount);
            }
        }
    }
}
