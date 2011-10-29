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
using System.Windows;
using Mooege.Common;
using Mooege.Common.Helpers;
using Mooege.Core.GS.Common.Types.Collision;
using Mooege.Core.GS.Common.Types.Math;
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

        public Dictionary<uint, Scene> Scenes = new Dictionary<uint, Scene>();
        public QuadTree QuadTree;

        public readonly ConcurrentDictionary<uint, Player.Player> Players;
        private readonly ConcurrentDictionary<uint, Actor> _actors;

        public bool HasPlayersIn { get { return this.Players.Count > 0; } }

        public int WorldSNO { get; set; }

        public uint NewSceneID { get { return this.Game.NewSceneID; } }
        public uint NewActorID { get { return this.Game.NewObjectID; } }
        public uint NewPlayerID { get { return this.Game.NewObjectID; } }

        private const int SceneProximity = 240;
        private const int ActorProximity = 240;

        public World(Game.Game game, int worldSNO)
            : base(game.NewWorldID)
        {
            this.Game = game;
            this.Game.StartTracking(this);
            this._actors = new ConcurrentDictionary<uint, Actor>();
            this.Players = new ConcurrentDictionary<uint, Player.Player>();

            // NOTE: WorldSNO must be valid before adding it to the game
            this.WorldSNO = worldSNO;

            this.QuadTree = new QuadTree(new Size(60, 60), 0, false);
            this.Game.AddWorld(this);
        }

        /// <summary>
        /// Returns a list of scenes that's player is spawnable.
        /// </summary>
        public List<Scene> SpawnableScenes
        {
            get
            {
                return (from pair in this.Scenes where pair.Value.StartPosition != null select pair.Value).ToList();
            }
        }

        public override void Update()
        {
            // update actors.
            foreach(var pair in this._actors) { pair.Value.Update(); }

            // update players.
            foreach (var pair in this.Players) { pair.Value.Update(); }
        }

        public void BroadcastIfRevealed(GameMessage message, Actor actor)
        {
            foreach (var player in this.Players.Values)
            {
                if (player.RevealedObjects.ContainsKey(actor.DynamicID))
                {
                    player.InGameClient.SendMessage(message);
                }
            }
        }

        public void BroadcastGlobal(GameMessage message)
        {
            foreach (var player in this.Players.Values)
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

        // TODO: NewPlayer messages should be broadcasted at the Game level, which means we may have to track players separately from objects in Game
        public void Enter(Actor actor)
        {
            this.AddActor(actor);
            actor.OnEnter(this);
            // Broadcast reveal
            // NOTE: Revealing to all right now since the flow results in actors that have initial positions that are not within the range of the player. /komiga

            var players = this.Players.Values; //this.GetPlayersInRange(actor.Position, 480.0f);
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
            foreach (var player in this.Players.Values)
            {
                if(actor != player)
                    actor.Unreveal(player);
            }
            this.RemoveActor(actor);

            
            if(actor is Player.Player)
            {
                List<IRevealable> revealedToPlayer = new List<IRevealable>();
                foreach (IRevealable revealable in (actor as Player.Player).RevealedObjects.Values)
                    revealedToPlayer.Add(revealable);

                foreach (IRevealable revealable in revealedToPlayer)
                    if(revealable is Actor)
                        if(revealable != actor)
                            revealable.Unreveal(actor as Player.Player);
            }            
        }

        public void RevealScenesInProximity(Player.Player player)
        {
            var proximity = new Rect(player.Position.X - SceneProximity / 2, player.Position.Y - SceneProximity / 2, SceneProximity, SceneProximity);
            var scenes = this.QuadTree.Query<Scene>(proximity);

            foreach (var scene in scenes)
            {
                scene.Reveal(player);
            }
        }

        public void RevealActorsInProximity(Player.Player player)
        {
            var proximity = new Rect(player.Position.X - ActorProximity / 2, player.Position.Y - ActorProximity / 2, ActorProximity, ActorProximity);
            var actors = this.QuadTree.Query<Actor>(proximity);

            foreach(var actor in actors)
            {
                if (actor.ActorType == ActorType.Gizmo || actor.ActorType == ActorType.Player || actor.ActorType == ActorType.Monster)
                    actor.Reveal(player);
            }
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

            this.RevealScenesInProximity(player); // reveal scenes in players proximity.
            this.RevealActorsInProximity(player); // reveal actors in players proximity.

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
            var monster = new Monster(player.World, actorSNO, position, new Dictionary<int,Mooege.Common.MPQ.FileFormats.Types.TagMapEntry>());
            this.Enter(monster);
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
        }

        public void SpawnGlobe(Mooege.Core.GS.Player.Player player, Vector3D position)
        {
            var item = ItemGenerator.CreateGlobe(player, RandomHelper.Next(1, 28)); // somehow the actual ammount is not shown on ground /raist.
            item.Drop(null, position);
        }

        #region Collections

        // Adding
        public void AddScene(Scene scene)
        {
            if (scene.DynamicID == 0 || HasScene(scene.DynamicID))
                throw new Exception(String.Format("Object has an invalid ID or was already present (ID = {0})", scene.DynamicID));
            this.Scenes.Add(scene.DynamicID, scene);
            this.QuadTree.Insert(scene);
        }

        private void AddActor(Actor actor)
        {
            if (actor.DynamicID == 0 || HasActor(actor.DynamicID))
                throw new Exception(String.Format("Object has an invalid ID or was already present (ID = {0})", actor.DynamicID));
            this._actors.TryAdd(actor.DynamicID, actor);
            this.QuadTree.Insert(actor);

            if (actor.ActorType == ActorType.Player) // temp
                this.AddPlayer((Mooege.Core.GS.Player.Player)actor);
        }

        private void AddPlayer(Mooege.Core.GS.Player.Player obj)
        {
            if (obj.DynamicID == 0 || HasPlayer(obj.DynamicID))
                throw new Exception(String.Format("Object has an invalid ID or was already present (ID = {0})", obj.DynamicID));

            this.Players.TryAdd(obj.DynamicID, obj);
        }

        // Removing
        public void RemoveScene(Scene obj)
        {
            if (obj.DynamicID == 0 || !HasScene(obj.DynamicID))
                throw new Exception(String.Format("Object has an invalid ID or was not present (ID = {0})", obj.DynamicID));
            this.Scenes.Remove(obj.DynamicID);
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
            if (obj.DynamicID == 0 || !this.Players.ContainsKey(obj.DynamicID))
                throw new Exception(String.Format("Object has an invalid ID or was not present (ID = {0})", obj.DynamicID));

            Player.Player player;
            this.Players.TryRemove(obj.DynamicID,out player);
        }

        // Getters
        public Scene GetScene(uint dynamicID)
        {
            Scene scene;
            this.Scenes.TryGetValue(dynamicID, out scene);
            return scene;
            //return this.Scenes.Where(scene => scene.DynamicID == dynamicID).FirstOrDefault();
        }

        public Actor GetActorByTag(int tag)
        {
            return (from Actor a in _actors.Values where a.Tag == tag select a).FirstOrDefault();
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

        public Player.Player GetPlayer(uint dynamicID)
        {
            Player.Player player;
            this.Players.TryGetValue(dynamicID, out player);
            return player;
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

        public bool HasPlayer(uint dynamicID)
        {
            return this.Players.ContainsKey(dynamicID);
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
            foreach (var player in this.Players.Values)
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
