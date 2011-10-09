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
using Mooege.Core.GS.Generators;
using Mooege.Core.GS.Map;
using Mooege.Net.GS;
using Mooege.Net.GS.Message;
using Mooege.Net.GS.Message.Fields;

// TODO: Move scene stuff into a Map class (which can also handle the efficiency stuff and object grouping)

namespace Mooege.Core.GS.Game
{
    public class Game : IMessageConsumer
    {
        static readonly Logger Logger = LogManager.CreateLogger();

        public PlayerManager PlayerManager { get; private set; }

        private Dictionary<uint, DynamicObject> Objects;
        // NOTE: This tracks by WorldSNO rather than by DynamicID; this.Objects _does_ still contain the world since it is a DynamicObject
        private Dictionary<int, World> Worlds;

        public int StartWorldSNO { get; private set; }
        public World StartWorld { get { return GetWorld(this.StartWorldSNO); } }

        private readonly WorldGenerator WorldGenerator;

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
            this.WorldGenerator = new WorldGenerator(this);
            // FIXME: This must be set according to the game settings (start quest/act). Better yet, track the player's save point and toss this stuff
            this.StartWorldSNO = 71150;
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
            // If it doesn't exist, try to load it
            if (world == null)
            {
                world = this.WorldGenerator.GenerateWorld(worldSNO);
                if (world == null)
                    Logger.Warn(String.Format("Failed to generate world (SNO = {0})", worldSNO));
            }
            return world;
        }

        public bool WorldExists(int worldSNO)
        {
            return this.Worlds.ContainsKey(worldSNO);
        }

        #endregion // World collection
    }
}
