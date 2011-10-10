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
using Mooege.Common;
using Mooege.Core.GS.Objects;
using Mooege.Core.GS.Generators;
using Mooege.Core.GS.Map;
using Mooege.Net.GS;
using Mooege.Net.GS.Message;

// TODO: Move scene stuff into a Map class (which can also handle the efficiency stuff and object grouping)

namespace Mooege.Core.GS.Game
{
    public class Game : IMessageConsumer
    {
        static readonly Logger Logger = LogManager.CreateLogger();

        public int GameId { get; private set; }

        public Dictionary<GameClient, Player.Player> Players = new Dictionary<GameClient, Player.Player>();

        private readonly Dictionary<uint, DynamicObject> _objects;
        private readonly Dictionary<int, World> _worlds; // NOTE: This tracks by WorldSNO rather than by DynamicID; this.Objects _does_ still contain the world since it is a DynamicObject

        public int StartWorldSNO { get; private set; }
        public World StartWorld { get { return GetWorld(this.StartWorldSNO); } }

        private readonly WorldGenerator _worldGenerator;

        private uint _lastObjectID = 0x00000001;
        private uint _lastSceneID  = 0x04000000;
        private uint _lastWorldID  = 0x07000000;

        // TODO: Need overrun handling and existence checking
        public uint NewObjectID { get { return _lastObjectID++; } }
        public uint NewSceneID { get { return _lastSceneID++; } }
        public uint NewWorldID { get { return _lastWorldID++; } }

        public Game(int gameId)
        {
            this.GameId = gameId;
            this._objects = new Dictionary<uint, DynamicObject>();
            this._worlds = new Dictionary<int, World>();
            this._worldGenerator = new WorldGenerator(this);
            this.StartWorldSNO = 71150; // FIXME: This must be set according to the game settings (start quest/act). Better yet, track the player's save point and toss this stuff
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
                case Consumers.Player:
                    client.Player.Consume(client, message);
                    break;
              }
        }

        public void Consume(GameClient client, GameMessage message)
        {
            // for possile future messages consumed by game.
        }

        public void Enter(Player.Player player)
        {
            this.Players.Add(player.InGameClient, player);
        }        

        #region Tracking

        public void StartTracking(DynamicObject obj)
        {
            if (obj.DynamicID == 0 || IsTracking(obj))
                throw new Exception(String.Format("Object has an invalid ID or was already being tracked (ID = {0})", obj.DynamicID));
            this._objects.Add(obj.DynamicID, obj);
        }

        public void EndTracking(DynamicObject obj)
        {
            if (obj.DynamicID == 0 || !IsTracking(obj))
                throw new Exception(String.Format("Object has an invalid ID or was not being tracked (ID = {0})", obj.DynamicID));
            this._objects.Remove(obj.DynamicID);
        }

        public DynamicObject GetObject(uint dynamicID)
        {
            DynamicObject obj;
            this._objects.TryGetValue(dynamicID, out obj);
            return obj;
        }

        public bool IsTracking(uint dynamicID)
        {
            return this._objects.ContainsKey(dynamicID);
        }

        public bool IsTracking(DynamicObject obj)
        {
            return this._objects.ContainsKey(obj.DynamicID);
        }

        #endregion // Tracking

        #region World collection

        public void AddWorld(World world)
        {
            if (world.WorldSNO == -1 || WorldExists(world.WorldSNO))
                throw new Exception(String.Format("World has an invalid SNO or was already being tracked (ID = {0}, SNO = {1})", world.DynamicID, world.WorldSNO));
            this._worlds.Add(world.WorldSNO, world);
        }

        public void RemoveWorld(World world)
        {
            if (world.WorldSNO == -1 || !WorldExists(world.WorldSNO))
                throw new Exception(String.Format("World has an invalid SNO or was not being tracked (ID = {0}, SNO = {1})", world.DynamicID, world.WorldSNO));
            this._worlds.Remove(world.WorldSNO);
        }

        public World GetWorld(int worldSNO)
        {
            World world;
            this._worlds.TryGetValue(worldSNO, out world);
            // If it doesn't exist, try to load it
            if (world == null)
            {
                world = this._worldGenerator.GenerateWorld(worldSNO);
                if (world == null)
                    Logger.Warn(String.Format("Failed to generate world (SNO = {0})", worldSNO));
            }
            return world;
        }

        public bool WorldExists(int worldSNO)
        {
            return this._worlds.ContainsKey(worldSNO);
        }

        #endregion // World collection
    }
}
