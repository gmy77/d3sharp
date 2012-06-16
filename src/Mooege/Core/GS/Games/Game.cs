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

using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using Mooege.Common.Logging;
using Mooege.Core.GS.Objects;
using Mooege.Core.GS.Generators;
using Mooege.Core.GS.Map;
using Mooege.Core.GS.Players;
using Mooege.Net.GS;
using Mooege.Net.GS.Message;
using Mooege.Net.GS.Message.Definitions.Game;
using Mooege.Net.GS.Message.Definitions.Misc;
using Mooege.Net.GS.Message.Definitions.Player;
using Mooege.Net.GS.Message.Fields;

namespace Mooege.Core.GS.Games
{
    public class Game : IMessageConsumer
    {
        private static readonly Logger Logger = LogManager.CreateLogger();

        /// <summary>
        /// The game id.
        /// </summary>
        public int GameId { get; private set; }

        /// <summary>
        /// Dictionary that maps gameclient's to players.
        /// </summary>
        public ConcurrentDictionary<GameClient, Player> Players { get; private set; }

        /// <summary>
        /// Dictionary that tracks objects and maps them to dynamicId's.
        /// </summary>
        private readonly ConcurrentDictionary<uint, DynamicObject> _objects;

        /// <summary>
        /// Dictionary that tracks world.
        /// NOTE: This tracks by WorldSNO rather than by DynamicID; this.Objects _does_ still contain the world since it is a DynamicObject
        /// </summary>
        private readonly ConcurrentDictionary<int, World> _worlds;

        /// <summary>
        /// Starting world's sno id.
        /// </summary>
        public int StartingWorldSNOId { get; private set; }

        /// <summary>
        /// Starting world for the game.
        /// </summary>
        public World StartingWorld { get { return GetWorld(this.StartingWorldSNOId); } }

        /// <summary>
        /// Player index counter.
        /// </summary>
        public int PlayerIndexCounter = -1;

        /// <summary>
        /// Update frequency for the game - 100 ms.
        /// </summary>
        public readonly long UpdateFrequency = 100;

        /// <summary>
        /// Incremented tick value on each Game.Update().
        /// </summary>
        public readonly int TickRate = 6;

        /// <summary>
        /// Tick counter.
        /// </summary>
        private int _tickCounter;

        /// <summary>
        /// Returns the latest tick count.
        /// </summary>
        public int TickCounter
        {
            get { return _tickCounter; }
        }

        /// <summary>
        /// Stopwatch that measures time takent to get a full Game.Update(). 
        /// </summary>
        private readonly Stopwatch _tickWatch;

        /// <summary>
        /// DynamicId counter for objects.
        /// </summary>
        private uint _lastObjectID = 0x00000001;

        /// <summary>
        /// Returns a new dynamicId for objects.
        /// </summary>
        public uint NewObjectID { get { return _lastObjectID++; } }

        /// <summary>
        /// DynamicId counter for scene.
        /// </summary>
        private uint _lastSceneID = 0x04000000;

        /// <summary>
        /// Returns a new dynamicId for scenes.
        /// </summary>
        public uint NewSceneID { get { return _lastSceneID++; } }

        /// <summary>
        /// DynamicId counter for worlds.
        /// </summary>
        private uint _lastWorldID = 0x07000000;

        /// <summary>
        /// Returns a new dynamicId for worlds.
        /// </summary>
        public uint NewWorldID { get { return _lastWorldID++; } }

        public QuestManager Quests { get; private set; }
        public AI.Pather Pathfinder { get; private set; }

        /// <summary>
        /// Creates a new game with given gameId.
        /// </summary>
        /// <param name="gameId"></param>
        public Game(int gameId)
        {
            this.GameId = gameId;
            this.Players = new ConcurrentDictionary<GameClient, Player>();
            this._objects = new ConcurrentDictionary<uint, DynamicObject>();
            this._worlds = new ConcurrentDictionary<int, World>();
            this.StartingWorldSNOId = 71150; // FIXME: This must be set according to the game settings (start quest/act). Better yet, track the player's save point and toss this stuff. /komiga
            this.Quests = new QuestManager(this);

            this._tickWatch = new Stopwatch();
            var loopThread = new Thread(Update) { IsBackground = true, CurrentCulture = CultureInfo.InvariantCulture }; ; // create the game update thread.
            loopThread.Start();
            Pathfinder = new Mooege.Core.GS.AI.Pather(this); //Creates the "Game"s single Pathfinder thread, Probably could be pushed further up and have a single thread handling all path req's for all running games. - DarkLotus
            var patherThread = new Thread(Pathfinder.UpdateLoop) { IsBackground = true, CurrentCulture = CultureInfo.InvariantCulture };
            patherThread.Start();
        }

        #region update & tick managment

        /// <summary>
        /// The main game loop.
        /// </summary>
        public void Update()
        {
            while (true)
            {
                this._tickWatch.Restart();
                Interlocked.Add(ref this._tickCounter, this.TickRate); // +6 ticks per 100ms. Verified by setting LogoutTickTimeMessage.Ticks to 600 which eventually renders a 10 sec logout timer on client. /raist

                // Lock Game instance to prevent incoming messages from modifying state while updating
                lock (this)
                {
                    // only update worlds with active players in it - so mob brain()'s in empty worlds doesn't get called and take actions for nothing. /raist.
                    foreach (var pair in this._worlds.Where(pair => pair.Value.HasPlayersIn))
                    {
                        pair.Value.Update(this._tickCounter);
                    }
                }

                this._tickWatch.Stop();

                var compensation = (int)(this.UpdateFrequency - this._tickWatch.ElapsedMilliseconds); // the compensation value we need to sleep in order to get consistent 100 ms Game.Update().

                if (this._tickWatch.ElapsedMilliseconds > this.UpdateFrequency)
                    Logger.Warn("Game.Update() took [{0}ms] more than Game.UpdateFrequency [{1}ms].", this._tickWatch.ElapsedMilliseconds, this.UpdateFrequency); // TODO: We may need to eventually use dynamic tickRate / updateFrenquencies. /raist.
                else
                    Thread.Sleep(compensation); // sleep until next Update().
            }
        }

        #endregion

        #region game-message handling & routing

        /// <summary>
        /// Routers incoming GameMessage to it's proper consumer.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="message"></param>
        public void Route(GameClient client, GameMessage message)
        {
            lock (this)
            {
                try
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

                        case Consumers.Conversations:
                            client.Player.Conversations.Consume(client, message);
                            break;

                        case Consumers.SelectedNPC:
                            if (client.Player.SelectedNPC != null)
                                client.Player.SelectedNPC.Consume(client, message);
                            break;

                    }
                }
                catch (Exception e)
                {
                    Logger.DebugException(e, "Unhandled exception caught:");
                }
            }
        }

        public void Consume(GameClient client, GameMessage message)
        { } // for possile future messages consumed by game. /raist.

        #endregion

        #region player-handling

        /// <summary>
        /// Allows a player to join the game.
        /// </summary>
        /// <param name="joinedPlayer">The new player.</param>
        public void Enter(Player joinedPlayer)
        {
            this.Players.TryAdd(joinedPlayer.InGameClient, joinedPlayer);

            // send all players in the game to new player that just joined (including him)
            foreach (var pair in this.Players)
            {
                this.SendNewPlayerMessage(joinedPlayer, pair.Value);
            }

            // notify other players about our new player too.
            foreach (var pair in this.Players.Where(pair => pair.Value != joinedPlayer))
            {
                this.SendNewPlayerMessage(pair.Value, joinedPlayer);
            }

            joinedPlayer.InGameClient.SendMessage(new GameSyncedDataMessage
            {
                Field0 = new GameSyncedData
                            {
                                Field0 = 0x0,
                                Field1 = 0x0,
                                Field2 = 0x0,
                                Field3 = 0x0,
                                Field4 = 0x0,
                                Field5 = 0x0,
                                Field6 = 0x0,
                                Field7 = new[] { 0x0, 0x0 },
                                Field8 = new[] { 0x0, 0x0 }
                            }
            });

            //joinedPlayer.EnterWorld(this.StartingWorld.StartingPoints.First().Position);
            joinedPlayer.EnterWorld(this.StartingWorld.StartingPoints.Find(x => x.ActorSNO.Name == "Start_Location_Team_0").Position);

            joinedPlayer.InGameClient.TickingEnabled = true; // it seems bnet-servers only start ticking after player is completely in-game. /raist
        }

        /// <summary>
        /// Sends NewPlayerMessage to players when a new player joins the game. 
        /// </summary>
        /// <param name="target">Target player to send the message.</param>
        /// <param name="joinedPlayer">The new joined player.</param>
        private void SendNewPlayerMessage(Player target, Player joinedPlayer)
        {
            target.InGameClient.SendMessage(new NewPlayerMessage
            {
                PlayerIndex = joinedPlayer.PlayerIndex, // player index
                ToonId = new EntityId() { High = (long)joinedPlayer.Toon.D3EntityID.IdHigh, Low = (long)joinedPlayer.Toon.D3EntityID.IdLow }, //Toon
                GameAccountId = new EntityId() { High = (long)joinedPlayer.Toon.GameAccount.BnetEntityId.High, Low = (long)joinedPlayer.Toon.GameAccount.BnetEntityId.Low }, //GameAccount
                ToonName = joinedPlayer.Toon.Name,
                Field3 = 0x00000002, //party frame class
                Field4 = target != joinedPlayer ? 0x2 : 0x4, //party frame level /boyc - may mean something different /raist.
                snoActorPortrait = joinedPlayer.ClassSNO, //party frame portrait
                Field6 = joinedPlayer.Toon.Level,
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

        #endregion

        #region object dynamicId tracking

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

        #endregion

        #region world collection

        public void AddWorld(World world)
        {
            if (world.WorldSNO.Id == -1 || WorldExists(world.WorldSNO.Id))
                throw new Exception(String.Format("World has an invalid SNO or was already being tracked (ID = {0}, SNO = {1})", world.DynamicID, world.WorldSNO.Id));
            this._worlds.TryAdd(world.WorldSNO.Id, world);
        }

        public void RemoveWorld(World world)
        {
            if (world.WorldSNO.Id == -1 || !WorldExists(world.WorldSNO.Id))
                throw new Exception(String.Format("World has an invalid SNO or was not being tracked (ID = {0}, SNO = {1})", world.DynamicID, world.WorldSNO.Id));

            World removed;
            this._worlds.TryRemove(world.WorldSNO.Id, out removed);
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

        #endregion

    }
}
