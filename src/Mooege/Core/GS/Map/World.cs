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
using System.Collections.Generic;
using System.Linq;
using Mooege.Common;
using Mooege.Common.Helpers;
using Mooege.Core.GS.Objects;
using Mooege.Core.GS.Actors;
using Mooege.Core.Common.Items;
using Mooege.Net.GS.Message;
using Mooege.Net.GS.Message.Fields;
using Mooege.Net.GS.Message.Definitions.World;

// NOTE: Scenes are actually laid out in cells with Subscenes filling in certain areas under a Scene.
//  We can use this design feature to track Actors' current scene and send updates to it and neighboring
//  scenes instead of distance checking for broadcasting messages.

namespace Mooege.Core.GS.Map
{
    public sealed class World : DynamicObject, IRevealable
    {
        static readonly Logger Logger = LogManager.CreateLogger();

        public Game.Game Game { get; private set; }

        private Dictionary<uint, Scene> Scenes;
        //private List<Scene> Scenes;
        private readonly ConcurrentDictionary<uint, Actor> _actors;
        private readonly ConcurrentDictionary<uint, Player.Player> _players; // Temporary for fast iteration for now since move/enter/leave handling is currently at the world level instead of the scene level

        public bool HasPlayersIn { get { return this._players.Count > 0; } }

        public int WorldSNO { get; set; }
        public Vector3D StartPosition { get; private set; }

        public uint NewSceneID { get { return this.Game.NewSceneID; } }
        public uint NewActorID { get { return this.Game.NewObjectID; } }
        public uint NewPlayerID { get { return this.Game.NewObjectID; } }

        public World(Game.Game game, int worldSNO)
            : base(game.NewWorldID)
        {
            this.Game = game;
            this.Game.StartTracking(this);
            this.Scenes = new Dictionary<uint, Scene>();
            //this.Scenes = new List<Scene>();
            this._actors = new ConcurrentDictionary<uint, Actor>();
            this._players = new ConcurrentDictionary<uint, Player.Player>();

            // NOTE: WorldSNO must be valid before adding it to the game
            this.WorldSNO = worldSNO;
            this.StartPosition = new Vector3D();
            this.Game.AddWorld(this);
        }

        public override void Update()
        {
            // update actors.
            foreach(var pair in this._actors) { pair.Value.Update(); }

            // update players.
            foreach (var pair in this._players) { pair.Value.Update(); }
        }

        public void BroadcastIfRevealed(GameMessage message, Actor actor)
        {
            foreach (var player in this._players.Values)
            {
                if (player.RevealedObjects.ContainsKey(actor.DynamicID))
                {
                    player.InGameClient.SendMessage(message);
                }
            }
        }

        public void BroadcastGlobal(GameMessage message)
        {
            foreach (var player in this._players.Values)
            {
                player.InGameClient.SendMessage(message);
            }
        }

        public void BroadcastInclusive(GameMessage message, Actor actor)
        {
            var players = this.GetPlayersInRange(actor.Position, 480.0f);
            foreach (var player in players)
            {
                player.InGameClient.SendMessage(message);
            }
        }

        public void BroadcastExclusive(GameMessage message, Actor actor)
        {
            var players = this.GetPlayersInRange(actor.Position, 480.0f);
            foreach (var player in players.Where(player => player != actor))
            {
                player.InGameClient.SendMessage(message);
            }
        }

        public void OnActorMove(Actor actor, Vector3D prevPosition)
        {
            // TODO: Unreveal from players that are now outside the actor's range                        
        }

        public void OnActorPositionChange(Actor actor, Vector3D prevPosition)
        {
            // Okay we need this here for positioning actors on world (like when item drops)
            // but we shouldn't be using it for movement of actors (like players) -- they should be instead using NotifyActorMovementMessage /raist.

            if (!actor.HasWorldLocation) return;
            if (actor is Player.Player) return; // don't send position ACDWorldPositionMessage for players, else it'll breake movement for them.  /raist.

            BroadcastIfRevealed(actor.ACDWorldPositionMessage, actor);            
        }

        // TODO: NewPlayer messages should be broadcasted at the Game level, which means we may have to track players separately from objects in Game
        public void Enter(Actor actor)
        {
            this.AddActor(actor);
            actor.OnEnter(this);
            // Broadcast reveal
            // NOTE: Revealing to all right now since the flow results in actors that have initial positions that are not within the range of the player. /komiga

            var players = this._players.Values; //this.GetPlayersInRange(actor.Position, 480.0f);
            foreach (var player in players)
            {
                actor.Reveal(player);
            }
        }

        public void Leave(Actor actor)
        {
            actor.OnLeave(this);
            // Broadcast unreveal
            //Logger.Debug("Leave {0}, unreveal to {1} players", actor.DynamicID, this.Players.Count);
            foreach (var player in this._players.Values)
            {
                actor.Unreveal(player);
            }
            this.RemoveActor(actor);
        }

        public bool Reveal(Mooege.Core.GS.Player.Player player)
        {
            if (player.RevealedObjects.ContainsKey(this.DynamicID))
                return false;

            // Reveal world to player
            player.InGameClient.SendMessage(new RevealWorldMessage()
            {
                WorldID = this.DynamicID,
                WorldSNO = this.WorldSNO,
            });
            player.InGameClient.SendMessage(new EnterWorldMessage()
            {
                EnterPosition = player.Position,
                WorldID = this.DynamicID,
                WorldSNO = this.WorldSNO,
            });

            // Revealing all scenes for now..
            Logger.Info("Revealing scenes for world {0}", this.DynamicID);
            foreach (var scene in this.Scenes.Values)
            {
                scene.Reveal(player);
            }

            // Reveal all actors
            // TODO: We need proper location-aware reveal logic for _all_ objects. This can be done on the scene level once that bit is in. /komiga
            Logger.Info("Revealing all actors for world {0}", this.DynamicID);
            foreach (var actor in _actors.Values)
            {
                actor.Reveal(player);                
            }

            player.RevealedObjects.Add(this.DynamicID, this);

            return true;
        }

        public bool Unreveal(Mooege.Core.GS.Player.Player player)
        {
            if (!player.RevealedObjects.ContainsKey(this.DynamicID))
                return false;

            // TODO: Unreveal all objects in the world? I think the client will do this on its own when it gets a WorldDeletedMessage /komiga
            //foreach (var obj in player.RevealedObjects.Values)
            //    if (obj.DynamicID == this.DynamicID) obj.Unreveal(player);

            player.InGameClient.SendMessage(new WorldDeletedMessage() { WorldID = DynamicID });
            return true;
        }

        public override void Destroy()
        {
            // TODO: Destroy all objects
            Mooege.Core.GS.Game.Game game = this.Game;
            this.Game = null;
            game.EndTracking(this);
        }

        public void SpawnMob(Mooege.Core.GS.Player.Player player, int actorSNO, Vector3D position)
        {
            new Monster(player.World, actorSNO, position);
        }

        public void SpawnRandomDrop(Mooege.Core.GS.Player.Player player, Vector3D position)
        {
            var item = ItemGenerator.GenerateRandom(player);

            item.Drop(null, position); // NOTE: The owner field for an item is only set when it is in the owner's inventory. /komiga
            player.GroundItems[item.DynamicID] = item; // FIXME: Hacky. /komiga
        }

        public void SpawnGold(Mooege.Core.GS.Player.Player player, Vector3D position)
        {
            var item = ItemGenerator.CreateGold(player, RandomHelper.Next(1, 3)); // somehow the actual ammount is not shown on ground /raist.
            item.Drop(null, position);
            player.GroundItems[item.DynamicID] = item;
        }

        #region Collections

        // Adding
        public void AddScene(Scene obj)
        {
            if (obj.DynamicID == 0 || HasScene(obj.DynamicID))
                throw new Exception(String.Format("Object has an invalid ID or was already present (ID = {0})", obj.DynamicID));
            this.Scenes.Add(obj.DynamicID, obj);
            //this.Scenes.Add(obj);
        }

        private void AddActor(Actor obj)
        {
            if (obj.DynamicID == 0 || HasActor(obj.DynamicID))
                throw new Exception(String.Format("Object has an invalid ID or was already present (ID = {0})", obj.DynamicID));
            this._actors.TryAdd(obj.DynamicID, obj);

            if (obj.ActorType == ActorType.Player) // temp
                this.AddPlayer((Mooege.Core.GS.Player.Player)obj);
        }

        private void AddPlayer(Mooege.Core.GS.Player.Player obj)
        {
            if (obj.DynamicID == 0 || HasPlayer(obj.DynamicID))
                throw new Exception(String.Format("Object has an invalid ID or was already present (ID = {0})", obj.DynamicID));

            this._players.TryAdd(obj.DynamicID, obj);
        }

        // Removing
        public void RemoveScene(Scene obj)
        {
            if (obj.DynamicID == 0 || !HasScene(obj.DynamicID))
                throw new Exception(String.Format("Object has an invalid ID or was not present (ID = {0})", obj.DynamicID));
            this.Scenes.Remove(obj.DynamicID);
            //this.Scenes.Remove(obj);
        }

        private void RemoveActor(Actor obj)
        {
            if (obj.DynamicID == 0 || !this._actors.ContainsKey(obj.DynamicID))
                throw new Exception(String.Format("Object has an invalid ID or was not present (ID = {0})", obj.DynamicID));
            Actor actor;
            this._actors.TryRemove(obj.DynamicID, out actor);

            if (obj.ActorType == ActorType.Player) // temp
                this.RemovePlayer((Mooege.Core.GS.Player.Player)obj);
        }

        private void RemovePlayer(Mooege.Core.GS.Player.Player obj)
        {
            if (obj.DynamicID == 0 || !this._players.ContainsKey(obj.DynamicID))
                throw new Exception(String.Format("Object has an invalid ID or was not present (ID = {0})", obj.DynamicID));

            Player.Player player;
            this._players.TryRemove(obj.DynamicID,out player);
        }

        // Getters
        public Scene GetScene(uint dynamicID)
        {
            Scene scene;
            this.Scenes.TryGetValue(dynamicID, out scene);
            return scene;
            //return this.Scenes.Where(scene => scene.DynamicID == dynamicID).FirstOrDefault();
        }

        public Actor GetActor(uint dynamicID)
        {
            Actor actor;
            this._actors.TryGetValue(dynamicID, out actor);
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

        public Mooege.Core.GS.Player.Player GetPlayer(uint dynamicID)
        {
            Mooege.Core.GS.Player.Player player;
            this._players.TryGetValue(dynamicID, out player);
            return player;
        }

        public Portal GetPortal(uint dynamicID)
        {
            return (Portal)GetActor(dynamicID, ActorType.Portal);
        }

        public Item GetItem(uint dynamicID)
        {
            return (Item)GetActor(dynamicID, ActorType.Item);
        }

        // Existence
        public bool HasScene(uint dynamicID)
        {
            return this.Scenes.ContainsKey(dynamicID);
            //return this.Scenes.Any(scene => scene.DynamicID == dynamicID);
        }

        public bool HasActor(uint dynamicID)
        {
            return this._actors.ContainsKey(dynamicID);
        }

        public bool HasActor(uint dynamicID, ActorType matchType)
        {
            var actor = GetActor(dynamicID, matchType);
            return actor != null;
        }

        public bool HasPortal(uint dynamicID)
        {
            return HasActor(dynamicID, ActorType.Portal);
        }

        public bool HasPlayer(uint dynamicID)
        {
            return this._players.ContainsKey(dynamicID);
        }

        public bool HasMonster(uint dynamicID)
        {
            return HasActor(dynamicID, ActorType.Monster);
        }

        public bool HasItem(uint dynamicID)
        {
            return HasActor(dynamicID, ActorType.Item);
        }

        private int SceneSorter(Scene x, Scene y)
        {
            if (x.World.DynamicID != y.World.DynamicID) return unchecked((int)x.World.DynamicID) - unchecked((int)y.World.DynamicID);
            if (x.ParentChunkID != y.ParentChunkID) return unchecked((int)x.ParentChunkID) - unchecked((int)y.ParentChunkID);
            return 0;
        }

        public void SortScenes()
        {
            // "this makes sure no scene is referenced before it is revealed to a player"
            //this.Scenes.Sort(SceneSorter);
        }

        public List<Actor> GetActorsInRange(int SNO, float x, float y, float z, float range)
        {
            var result = new List<Actor>();
            foreach (var actor in this._actors.Values)
            {
                if (actor.ActorSNO == SNO &&
                    (Math.Sqrt(
                        Math.Pow(actor.Position.X - x, 2) +
                        Math.Pow(actor.Position.Y - y, 2) +
                        Math.Pow(actor.Position.Z - z, 2)) <= range))
                {
                    result.Add(actor);
                }
            }
            return result;
        }

        public List<Actor> GetActorsInRange(int SNO, Vector3D vec, float range)
        {
            return GetActorsInRange(SNO, vec.X, vec.Y, vec.Z, range);
        }

        public List<Actor> GetActorsInRange(float x, float y, float z, float range)
        {
            var result = new List<Actor>();
            foreach (var actor in this._actors.Values)
            {
                if (Math.Sqrt(
                        Math.Pow(actor.Position.X - x, 2) +
                        Math.Pow(actor.Position.Y - y, 2) +
                        Math.Pow(actor.Position.Z - z, 2)) <= range)
                {
                    result.Add(actor);
                }
            }
            return result;
        }

        public List<Actor> GetActorsInRange(Vector3D vec, float range)
        {
            return GetActorsInRange(vec.X, vec.Y, vec.Z, range);
        }

        public List<Mooege.Core.GS.Player.Player> GetPlayersInRange(float x, float y, float z, float range)
        {
            var result = new List<Mooege.Core.GS.Player.Player>();
            foreach (var player in this._players.Values)
            {
                if (Math.Sqrt(
                        Math.Pow(player.Position.X - x, 2) +
                        Math.Pow(player.Position.Y - y, 2) +
                        Math.Pow(player.Position.Z - z, 2)) <= range)
                {
                    result.Add(player);
                }
            }
            return result;
        }

        public List<Mooege.Core.GS.Player.Player> GetPlayersInRange(Vector3D vec, float range)
        {
            return GetPlayersInRange(vec.X, vec.Y, vec.Z, range);
        }

        #endregion // Collections
    }
}
