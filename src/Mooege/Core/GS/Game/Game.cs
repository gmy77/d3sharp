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
    public class Game : IDynamicObjectManager, IMessageConsumer
    {
        static readonly Logger Logger = LogManager.CreateLogger();

        public PlayerManager PlayerManager { get; private set; }

        private Dictionary<uint, World> Worlds;
        private Dictionary<uint, Actor> Actors;
        private Dictionary<uint, Player> Players;
        private Dictionary<uint, Item> Items;

        public World StartWorld { get; private set; }

        // These are split up just so that we can tell what an object's type is by its ID
        private uint _lastWorldID   = 0x01000000;
        private uint _lastSceneID   = 0x03000000;
        private uint _lastPlayerID  = 0x00000001;
        private uint _lastActorID   = 0x00100000;
        private uint _lastNPCID     = 0x00400000;
        private uint _lastMonsterID = 0x00600000;
        private uint _lastItemID    = 0x00800000;

        // TODO: Need overrun handling and existence checking
        public uint NewWorldID { get { return _lastWorldID++; } }
        public uint NewSceneID { get { return _lastSceneID++; } }
        public uint NewPlayerID { get { return _lastPlayerID++; } }
        public uint NewActorID { get { return _lastActorID++; } }
        public uint NewNPCID { get { return _lastNPCID++; } }
        public uint NewMonsterID { get { return _lastMonsterID++; } }
        public uint NewItemID { get { return _lastItemID++; } }

        public Game()
        {
            this.Worlds = new Dictionary<uint, World>();
            this.Actors = new Dictionary<uint, Actor>();
            this.Players = new Dictionary<uint, Player>();
            this.Items = new Dictionary<uint, Item>();
            this.PlayerManager = new PlayerManager(this);
            // FIXME: This is a stub for the moment
            this.StartWorld = new World(this);
        }

        public void Route(GameClient client, GameMessage message)
        {
            switch (message.Consumer)
            {
                case Consumers.Universe:
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
            if (message is TargetMessage) OnTargeted(client, (TargetMessage)message);
        }

        #region Collections

        // Adding
        public void AddWorld(World obj)
        {
            if (obj.DynamicID == 0 || this.Worlds.ContainsKey(obj.DynamicID))
                throw new Exception(String.Format("Object has an invalid ID or was already present ({0})", obj.DynamicID));
            this.Worlds.Add(obj.DynamicID, obj);
        }

        public void AddActor(Actor obj)
        {
            if (obj.DynamicID == 0 || this.Actors.ContainsKey(obj.DynamicID))
                throw new Exception(String.Format("Object has an invalid ID or was already present ({0})", obj.DynamicID));
            this.Actors.Add(obj.DynamicID, obj);
        }

        public void AddPlayer(Player obj)
        {
            if (obj.DynamicID == 0 || this.Players.ContainsKey(obj.DynamicID))
                throw new Exception(String.Format("Object has an invalid ID or was already present ({0})", obj.DynamicID));
            this.Players.Add(obj.DynamicID, obj);
        }

        public void AddItem(Item obj)
        {
            if (obj.DynamicID == 0 || this.Items.ContainsKey(obj.DynamicID))
                throw new Exception(String.Format("Object has an invalid ID or was already present ({0})", obj.DynamicID));
            this.Items.Add(obj.DynamicID, obj);
        }

        // Removing
        public void RemoveWorld(World obj)
        {
            if (obj.DynamicID == 0 || !this.Worlds.ContainsKey(obj.DynamicID))
                throw new Exception(String.Format("Object has an invalid ID or was not present ({0})", obj.DynamicID));
            this.Worlds.Remove(obj.DynamicID);
        }

        public void RemoveActor(Actor obj)
        {
            if (obj.DynamicID == 0 || !this.Actors.ContainsKey(obj.DynamicID))
                throw new Exception(String.Format("Object has an invalid ID or was not present ({0})", obj.DynamicID));
            this.Actors.Remove(obj.DynamicID);
        }

        public void RemovePlayer(Player obj)
        {
            if (obj.DynamicID == 0 || !this.Players.ContainsKey(obj.DynamicID))
                throw new Exception(String.Format("Object has an invalid ID or was not present ({0})", obj.DynamicID));
            this.Players.Remove(obj.DynamicID);
        }

        public void RemoveItem(Item obj)
        {
            if (obj.DynamicID == 0 || !this.Items.ContainsKey(obj.DynamicID))
                throw new Exception(String.Format("Object has an invalid ID or was not present ({0})", obj.DynamicID));
            this.Items.Remove(obj.DynamicID);
        }

        // Getters
        public World GetWorld(uint dynamicID)
        {
            World world;
            this.Worlds.TryGetValue(dynamicID, out world);
            return world;
        }

        public Actor GetActor(uint dynamicID)
        {
            Actor actor;
            this.Actors.TryGetValue(dynamicID, out actor);
            return actor;
        }

        public Actor GetActor(uint dynamicID, ActorType matchType)
        {
            var actor = GetActor(dynamicID);
            if (actor != null)
            {
                if (actor.ActorType == matchType)
                    return actor;
                else
                    Logger.Warn("Attempted to get actor ID {0} as a {1}, whereas the actor is type {2}",
                        dynamicID, Enum.GetName(typeof(ActorType), matchType), Enum.GetName(typeof(ActorType), actor.ActorType));
            }
            return null;
        }

        public Player GetPlayer(uint dynamicID)
        {
            Player player;
            this.Players.TryGetValue(dynamicID, out player);
            return player;
        }

        public Item GetItem(uint dynamicID)
        {
            Item item;
            this.Items.TryGetValue(dynamicID, out item);
            return item;
        }

        // Existence
        public bool HasActor(uint dynamicID, ActorType matchType)
        {
            var actor = GetActor(dynamicID, matchType);
            return actor != null;
        }

        public bool HasWorld(uint dynamicID)
        {
            return this.Worlds.ContainsKey(dynamicID);
        }

        public bool HasPlayer(uint dynamicID)
        {
            return this.Players.ContainsKey(dynamicID);
        }

        public bool HasActor(uint dynamicID)
        {
            return this.Actors.ContainsKey(dynamicID);
        }

        public bool HasNPC(uint dynamicID)
        {
            return HasActor(dynamicID, ActorType.NPC);
        }

        public bool HasMonster(uint dynamicID)
        {
            return HasActor(dynamicID, ActorType.Monster);
        }

        public bool HasItem(uint dynamicID)
        {
            return this.Items.ContainsKey(dynamicID);
        }

        #endregion // Collections

        public void ChangeToonWorld(GameClient client, uint worldID, Vector3D pos)
        {
            Player player = client.Player;
            World newWorld = GetWorld(worldID);
            World currentWorld = player.World;

            if (newWorld == null/* || currentWorld==null*/) return; //don't go to a world we don't have in the universe

            // TODO: Should be renamed to differentiate between actual world destruction and removing the player from the world
            if (currentWorld != null)
                currentWorld.DestroyWorld(player);

            player.World = newWorld;
            player.Position.X = pos.X;
            player.Position.Y = pos.Y;
            player.Position.Z = pos.Z;

            newWorld.Reveal(player);
            player.SendWorldPosition(client.Player);
            client.FlushOutgoingBuffer();

            client.SendMessage(new PlayerWarpedMessage()
            {
                Field0 = 9,
                Field1 = 0f,
            });

            client.PacketId += 40 * 2;
            client.SendMessage(new DWordDataMessage()
            {
                Id = 0x89,
                Field0 = client.PacketId,
            });

            client.FlushOutgoingBuffer();
        }

        private void OnTargeted(GameClient client, TargetMessage message)
        {
            //Logger.Info("Player interaction with " + message.AsText());

            Portal p = client.Player.World.GetPortal(message.TargetID);
            if (p != null)
            {
                //we have a transition between worlds here
                ChangeToonWorld(client, p.TargetWorldID, p.TargetPos); //targetpos will always be valid as otherwise the portal wouldn't be targetable
                return;
            }

            // Check if it is an Interaction with an item....
            Actor a = this.GetActor(message.TargetID);
            if (a != null)
            {
                if (client.Items.ContainsKey(message.TargetID))
                {
                    client.Player.Inventory.PickUp(message);
                    return;
                }
            }
        }

        private void SpawnRandomDrop(Player player, Vector3D postition)
        {
            ItemTypeGenerator itemGenerator = new ItemTypeGenerator(player.InGameClient);
            // randomize ItemType
            ItemType[] allValues = (ItemType[])Enum.GetValues(typeof(ItemType));
            ItemType type = allValues[RandomHelper.Next(allValues.Length)];
            Item item = itemGenerator.GenerateRandomElement(type);
            DropItem(player, item, postition);
        }

        private void SpawnGold(Player player, Vector3D position)
        {
            ItemTypeGenerator itemGenerator = new ItemTypeGenerator(player.InGameClient);
            Item item = itemGenerator.CreateItem("Gold1", 0x00000178, ItemType.Gold);
            item.Count = RandomHelper.Next(1, 3);
            DropItem(player, item, position);
        }

        // TODO: Redo this
        public void DropItem(Player player, Item item, Vector3D postition)
        {}
        /*public void DropItem(Player player, Item item, Vector3D postition)
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

            client.SendMessage(new ANNDataMessage(Opcodes.ANNDataMessage1)
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
