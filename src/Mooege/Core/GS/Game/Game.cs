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
using System.IO;
using System.Collections.Generic;
using Mooege.Common;
using Mooege.Common.Helpers;
using Mooege.Core.GS.Objects;
using Mooege.Core.GS.Map;
using Mooege.Core.GS.Actors;
using Mooege.Core.GS.NPC;
using Mooege.Core.GS.Data.SNO;
using Mooege.Core.Common.Items;
using Mooege.Net.GS;
using Mooege.Net.GS.Message;
using Mooege.Net.GS.Message.Definitions.ACD;
using Mooege.Net.GS.Message.Definitions.Animation;
using Mooege.Net.GS.Message.Definitions.Attribute;
using Mooege.Net.GS.Message.Definitions.Combat;
using Mooege.Net.GS.Message.Definitions.Effect;
using Mooege.Net.GS.Message.Definitions.Misc;
using Mooege.Net.GS.Message.Definitions.Player;
using Mooege.Net.GS.Message.Fields;

namespace Mooege.Core.GS.Game
{
    public class Game : IMessageConsumer
    {
        static readonly Logger Logger = LogManager.CreateLogger();

        public PlayerManager PlayerManager { get; private set; }

        private Dictionary<uint, DynamicObject> Objects;
        // NOTE: This tracks by WorldSNO rather than by DynamicID; this.Objects _does_ still contain the world since it is a DynamicObject
        private Dictionary<int, World> Worlds;

        public World StartWorld { get; private set; }

        private uint _lastObjectID = 0x00000001;
        private uint _lastSceneID  = 0x04000000;
        private uint _lastWorldID  = 0x07000000;

        // TODO: Need overrun handling and existence checking
        public uint NewObjectID { get { return _lastObjectID++; } }
        public uint NewSceneID { get { return _lastSceneID++; } }
        public uint NewWorldID { get { return _lastWorldID++; } }

        public Game()
        {
            this.Objects = new Dictionary<uint, DynamicObject>();
            this.Worlds = new Dictionary<int, World>();
            this.PlayerManager = new PlayerManager(this);
            // FIXME: This is a stub for the moment
            this.StartWorld = new World(this, 1);
        }

        public void Route(GameClient client, GameMessage message)
        {
            switch (message.Consumer)
            {
                case Consumers.Game:
                    this.Consume(client, message);
                    break;
                case Consumers.Inventory:
                    client.Player.Inventory.Consume(client, message);
                    break;
                case Consumers.PlayerManager:
                    this.PlayerManager.Consume(client, message);
                    break;
                case Consumers.Player:
                    client.Player.Consume(client, message);
                    break;
              }
        }

        public void Consume(GameClient client, GameMessage message)
        {
        }

        #region Tracking

        public void StartTracking(DynamicObject obj)
        {
            if (obj.DynamicID == 0 || IsTracking(obj))
                throw new Exception(String.Format("Object has an invalid ID or was already being tracked (ID = {0})", obj.DynamicID));
            this.Objects.Add(obj.DynamicID, obj);
        }

        public void EndTracking(DynamicObject obj)
        {
            if (obj.DynamicID == 0 || !IsTracking(obj))
                throw new Exception(String.Format("Object has an invalid ID or was not being tracked (ID = {0})", obj.DynamicID));
            this.Objects.Remove(obj.DynamicID);
        }

        public DynamicObject GetObject(uint dynamicID)
        {
            DynamicObject obj;
            this.Objects.TryGetValue(dynamicID, out obj);
            return obj;
        }

        public bool IsTracking(uint dynamicID)
        {
            return this.Objects.ContainsKey(dynamicID);
        }

        public bool IsTracking(DynamicObject obj)
        {
            return this.Objects.ContainsKey(obj.DynamicID);
        }

        #endregion // Tracking

        #region World collection

        public void AddWorld(World world)
        {
            if (world.WorldSNO == -1 || WorldExists(world.WorldSNO))
                throw new Exception(String.Format("World has an invalid SNO or was already being tracked (ID = {0}, SNO = {1})", world.DynamicID, world.WorldSNO));
            this.Worlds.Add(world.WorldSNO, world);
        }

        public void RemoveWorld(World world)
        {
            if (world.WorldSNO == -1 || !WorldExists(world.WorldSNO))
                throw new Exception(String.Format("World has an invalid SNO or was not being tracked (ID = {0}, SNO = {1})", world.DynamicID, world.WorldSNO));
            this.Worlds.Remove(world.WorldSNO);
        }

        public World GetWorld(int worldSNO)
        {
            World world;
            this.Worlds.TryGetValue(worldSNO, out world);
            return world;
        }

        public bool WorldExists(int worldSNO)
        {
            return this.Worlds.ContainsKey(worldSNO);
        }

        #endregion // World collection

        private void SpawnRandomDrop(Mooege.Core.GS.Player.Player player, Vector3D postition)
        {
            ItemTypeGenerator itemGenerator = new ItemTypeGenerator(player.InGameClient);
            // randomize ItemType
            ItemType[] allValues = (ItemType[])Enum.GetValues(typeof(ItemType));
            ItemType type = allValues[RandomHelper.Next(allValues.Length)];
            Item item = itemGenerator.GenerateRandomElement(type);
            DropItem(player, item, postition);
        }

        private void SpawnGold(Mooege.Core.GS.Player.Player player, Vector3D position)
        {
            ItemTypeGenerator itemGenerator = new ItemTypeGenerator(player.InGameClient);
            Item item = itemGenerator.CreateItem("Gold1", 0x00000178, ItemType.Gold);
            item.Count = RandomHelper.Next(1, 3);
            DropItem(player, item, position);
        }

        // TODO: Redo this
        public void DropItem(Mooege.Core.GS.Player.Player player, Item item, Vector3D postition)
        {}
        /*public void DropItem(Mooege.Core.GS.Player.Player player, Item item, Vector3D postition)
        {
            // Items are actors; shouldn't do this
            Actor itemActor = new Actor(player.World)
            {
                GBHandle = new GBHandle()
                {
                    Type = 2,
                    GBId = item.GBID,
                },
                InventoryLocationData = null,
                Scale = 1.35f,
                Position = postition,
                World = player.World,
                RotationAmount = 0.768145f,
                RotationAxis = new Vector3D()
                {
                    X = 0f,
                    Y = 0f,
                    Z = (float)RandomHelper.NextDouble(),
                },
                AppearanceSNO = item.AppearanceSNO,
                DynamicID = item.DynamicID,
            };

            itemActor.Reveal(player);
            item.Reveal(player);

            player.InGameClient.PacketId += 10 * 2;
            player.InGameClient.SendMessage(new DWordDataMessage()
            {
                Id = 0x89,
                Field0 = player.InGameClient.PacketId,
            });
            player.InGameClient.FlushOutgoingBuffer();
        }*/

        // this shouldn't even rely on client or its position though; i know this is just a hack atm ;) /raist.
        // FIXME: Horriblest thing ever
        /*public void SpawnMob(GameClient client, int mobId)
        {
            int nId = mobId;
            if (client.Player.Position == null)
                return;

            if (client.ObjectIdsSpawned == null)
            {
                client.ObjectIdsSpawned = new List<int>(); // This should be initialized looooong before we ever get into this function
                client.ObjectIdsSpawned.Add(this.NextObjectId); // Oh let's just add a new object ID..
            }

            int objectId = this.NextObjectId;
            client.ObjectIdsSpawned.Add(objectId); // But.. you just added a generated ID! Now there's a void ID lurking around..

            #region ACDEnterKnown Hittable Zombie
            Vector3D pos = client.Player.Position;
            BasicNPC mob = new BasicNPC(objectId, mobId, new WorldPlace { Field0 = new Vector3D(pos.X - 5, pos.Y - 5, pos.Z), });
            GetWorld(client.Player.World).AddMonster(mob);
            mob.Reveal(client);

            client.SendMessage(new AffixMessage()
            {
                ActorID = objectId,
                Field1 = 0x1,
                aAffixGBIDs = new int[0]
            });
            client.SendMessage(new AffixMessage()
            {
                ActorID = objectId,
                Field1 = 0x2,
                aAffixGBIDs = new int[0]
            });
            client.SendMessage(new ACDCollFlagsMessage
            {
                Field0 = objectId,
                Field1 = 0x1
            });

            GameAttributeMap attribs = new GameAttributeMap();
            attribs[GameAttribute.Untargetable] = false;
            attribs[GameAttribute.Uninterruptible] = true;
            attribs[GameAttribute.Buff_Visual_Effect, 1048575] = true;
            attribs[GameAttribute.Buff_Icon_Count0, 30582] = 1;
            attribs[GameAttribute.Buff_Icon_Count0, 30286] = 1;
            attribs[GameAttribute.Buff_Icon_Count0, 30285] = 1;
            attribs[GameAttribute.Buff_Icon_Count0, 30284] = 1;
            attribs[GameAttribute.Buff_Icon_Count0, 30283] = 1;
            attribs[GameAttribute.Buff_Icon_Count0, 30290] = 1;
            attribs[GameAttribute.Buff_Icon_Count0, 79486] = 1;
            attribs[GameAttribute.Buff_Active, 30286] = true;
            attribs[GameAttribute.Buff_Active, 30285] = true;
            attribs[GameAttribute.Buff_Active, 30284] = true;
            attribs[GameAttribute.Buff_Active, 30283] = true;
            attribs[GameAttribute.Buff_Active, 30290] = true;

            attribs[GameAttribute.Hitpoints_Max_Total] = 4.546875f;
            attribs[GameAttribute.Buff_Active, 79486] = true;
            attribs[GameAttribute.Hitpoints_Max] = 4.546875f;
            attribs[GameAttribute.Hitpoints_Total_From_Level] = 0f;
            attribs[GameAttribute.Hitpoints_Cur] = 4.546875f;
            attribs[GameAttribute.Invulnerable] = true;
            attribs[GameAttribute.Buff_Active, 30582] = true;
            attribs[GameAttribute.TeamID] = 10;
            attribs[GameAttribute.Level] = 1;

            attribs.SendMessage(client, objectId);

            client.SendMessage(new ACDGroupMessage
            {
                ActorID = objectId,
                Field1 = unchecked((int)0xb59b8de4),
                Field2 = unchecked((int)0xffffffff)
            });

            client.SendMessage(new ANNDataMessage(Opcodes.ANNDataMessage24)
            {
                ActorID = objectId
            });

            client.SendMessage(new ACDTranslateFacingMessage(Opcodes.ACDTranslateFacingMessage1)
            {
                ActorID = objectId,
                Angle = (float)(RandomHelper.NextDouble() * 2.0 * Math.PI),
                Field2 = false
            });

            client.SendMessage(new SetIdleAnimationMessage
            {
                Id = 0xa5,
                Field0 = objectId,
                Field1 = 0x11150
            });

            client.SendMessage(new SNONameDataMessage
            {
                Id = 0xd3,
                Field0 = new SNOName
                {
                    Field0 = 0x1,
                    Field1 = nId
                }
            });
            #endregion

            client.PacketId += 30 * 2;
            client.SendMessage(new DWordDataMessage()
            {
                Id = 0x89,
                Field0 = client.PacketId,
            });
            client.Tick += 20;
            client.SendMessage(new EndOfTickMessage()
            {
                Id = 0x008D,
                Field0 = client.Tick - 20,
                Field1 = client.Tick
            });
        }*/
    }
}
