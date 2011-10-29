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
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using Mooege.Common;
using Mooege.Core.GS.Objects;
using Mooege.Core.GS.Generators;
using Mooege.Core.GS.Map;
using Mooege.Net.GS;
using Mooege.Net.GS.Message;
using Mooege.Net.GS.Message.Definitions.Game;
using Mooege.Net.GS.Message.Definitions.Player;
using Mooege.Net.GS.Message.Fields;

// TODO: Move scene stuff into a Map class (which can also handle the efficiency stuff and object grouping)

namespace Mooege.Core.GS.Game
{
    public class Game : IMessageConsumer
    {
        static readonly Logger Logger = LogManager.CreateLogger();

        public int GameId { get; private set; }

        public ConcurrentDictionary<GameClient, Player.Player> Players = new ConcurrentDictionary<GameClient, Player.Player>();

        public int PlayerIndexCounter = -1;

        private readonly ConcurrentDictionary<uint, DynamicObject> _objects;
        private readonly ConcurrentDictionary<int, World> _worlds; // NOTE: This tracks by WorldSNO rather than by DynamicID; this.Objects _does_ still contain the world since it is a DynamicObject

        public int StartWorldSNO { get; private set; }
        public World StartWorld { get { return GetWorld(this.StartWorldSNO); } }

        public readonly int UpdateFrequency=100; // updates game every 100ms - still not sure if we should be updating this frequent / raist.
        private int _tickCounter;
        
        public int Tick
        {
            get { return _tickCounter; }
        }

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
            this._objects = new ConcurrentDictionary<uint, DynamicObject>();
            this._worlds = new ConcurrentDictionary<int, World>();
            this.StartWorldSNO = 71150; // FIXME: This must be set according to the game settings (start quest/act). Better yet, track the player's save point and toss this stuff
            var loopThread=new Thread(Update) { IsBackground = true };
            loopThread.Start();
        }

        public void Update() // the main game-loop.
        {
            while (true)
            {
                Interlocked.Add(ref this._tickCounter, 6); // +6 ticks per 100ms. Verified by setting LogoutTickTimeMessage.Ticks to 600 which eventually renders a 10 sec logout timer on client. /raist

                // only update worlds with active players in it - so mob's brain() in empty worlds doesn't get called and take actions for nothing. /raist.
                foreach (var pair in this._worlds.Where(pair => pair.Value.HasPlayersIn)) 
                {
                    pair.Value.Update();
                }

                Thread.Sleep(UpdateFrequency);
            }
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

        public void Enter(Player.Player joinedPlayer)
        {
            this.Players.TryAdd(joinedPlayer.InGameClient, joinedPlayer);

            // send all players in the game to new player that just joined (including him)
            foreach (var pair in this.Players)
            {
                this.SendNewPlayerMessage(joinedPlayer, pair.Value);
            }

            // notify other players about or new player too.
            foreach (var pair in this.Players.Where(pair => pair.Value != joinedPlayer))
            {
                this.SendNewPlayerMessage(pair.Value, joinedPlayer);
            }

            joinedPlayer.InGameClient.SendMessage(new GameSyncedDataMessage
            {
                Field0 = new GameSyncedData
                            {
                                Field0 = false,
                                Field1 = 0x0,
                                Field2 = 0x0,
                                Field3 = 0x0,
                                Field4 = 0x0,
                                Field5 = 0x0,
                                Field6 = new[] {0x0, 0x0},
                                Field7 = new[] {0x0, 0x0}
                            }
            });

            joinedPlayer.World.Enter(joinedPlayer); // Enter only once all fields have been initialized to prevent a run condition
            joinedPlayer.InGameClient.TickingEnabled = true; // it seems bnet-servers only start ticking after player is completely in-game. /raist
        }

        private void SendNewPlayerMessage(Player.Player target, Player.Player joinedPlayer)
        {
            target.InGameClient.SendMessage(new NewPlayerMessage
            {
                PlayerIndex = joinedPlayer.PlayerIndex, // player index
                Field1 = "", //Owner name?
                ToonName = joinedPlayer.Properties.Name,
                Field3 = 0x00000002, //party frame class
                Field4 = target!=joinedPlayer? 0x2 : 0x4, //party frame level /boyc - may mean something different /raist.
                snoActorPortrait = joinedPlayer.ClassSNO, //party frame portrait
                Field6 = 0x0000000A,
                StateData = joinedPlayer.GetStateData(),
                Field8 = this.Players.Count != 1, //announce party join
                Field9 = 0x00000001,
                ActorID = joinedPlayer.DynamicID,
            });

            target.InGameClient.SendMessage(joinedPlayer.GetPlayerBanner()); // send player banner proto - D3.GameMessage.PlayerBanner
            target.InGameClient.SendMessage(joinedPlayer.GetBlacksmithData()); // send player artisan proto /fasbat
            target.InGameClient.SendMessage(joinedPlayer.GetJewelerData());
            target.InGameClient.SendMessage(joinedPlayer.GetMysticData());
        }


        #region Tracking

        public void StartTracking(DynamicObject obj)
        {
            if (obj.DynamicID == 0 || IsTracking(obj))
                throw new Exception(String.Format("Object has an invalid ID or was already being tracked (ID = {0})", obj.DynamicID));
            this._objects.TryAdd(obj.DynamicID, obj);
        }

        public void EndTracking(DynamicObject obj)
        {
            if (obj.DynamicID == 0 || !IsTracking(obj))
                throw new Exception(String.Format("Object has an invalid ID or was not being tracked (ID = {0})", obj.DynamicID));

            DynamicObject removed;
            this._objects.TryRemove(obj.DynamicID, out removed);
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
            this._worlds.TryAdd(world.WorldSNO, world);
        }

        public void RemoveWorld(World world)
        {
            if (world.WorldSNO == -1 || !WorldExists(world.WorldSNO))
                throw new Exception(String.Format("World has an invalid SNO or was not being tracked (ID = {0}, SNO = {1})", world.DynamicID, world.WorldSNO));

            World removed;
            this._worlds.TryRemove(world.WorldSNO, out removed);
        }

        public World GetWorld(int worldSNO)
        {
            World world;
            this._worlds.TryGetValue(worldSNO, out world);

            if (world == null) // If it doesn't exist, try to load it
            {
                world = WorldGenerator.Generate(this, worldSNO);
                if (world == null) Logger.Warn("Failed to generate world with sno: {0}", worldSNO);
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
