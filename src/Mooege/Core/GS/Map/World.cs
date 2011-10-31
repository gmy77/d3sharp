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
using Mooege.Core.GS.Actors.Implementations;
using Mooege.Core.GS.Common.Types;
using Mooege.Core.GS.Common.Types.Math;
using Mooege.Core.GS.Games;
using Mooege.Core.GS.Objects;
using Mooege.Core.GS.Actors;
using Mooege.Core.Common.Items;
using Mooege.Core.GS.Players;
using Mooege.Net.GS.Message;
using Mooege.Net.GS.Message.Definitions.World;

namespace Mooege.Core.GS.Map
{
    public sealed class World : DynamicObject, IRevealable
    {
        static readonly Logger Logger = LogManager.CreateLogger();

        /// <summary>
        /// Game that the world belongs to.
        /// </summary>
        public Game Game { get; private set; }

        /// <summary>
        /// The SNOId for the world.
        /// </summary>
        public int SNOId { get; set; }

        /// <summary>
        /// QuadTree that contains scenes & actors.
        /// </summary>
        public QuadTree QuadTree { get; private set; }

        /// <summary>
        /// List of scenes contained in the world.
        /// </summary>
        public ConcurrentDictionary<uint, Scene> Scenes;

        /// <summary>
        /// List of actors contained in the world.
        /// </summary>
        public readonly ConcurrentDictionary<uint, Actor> Actors;

        /// <summary>
        /// List of players contained in the world.
        /// </summary>
        public readonly ConcurrentDictionary<uint, Player> Players;

        /// <summary>
        /// Returns true if the world has players in.
        /// </summary>
        public bool HasPlayersIn { get { return this.Players.Count > 0; } }


        /// <summary>
        /// Returns a new dynamicId for scenes.
        /// </summary>
        public uint NewSceneID { get { return this.Game.NewSceneID; } }

        // Returns a new dynamicId for actors.
        public uint NewActorID { get { return this.Game.NewObjectID; } }

        /// <summary>
        /// Returns a new dynamicId for players.
        /// </summary>
        public uint NewPlayerID { get { return this.Game.NewObjectID; } }

        /// <summary>
        /// Scene revealing proximity for players.
        /// </summary>
        private const int SceneProximity = 240;

        /// <summary>
        /// Actors revealing proximity for player.
        /// </summary>
        private const int ActorProximity = 240;

        /// <summary>
        /// Returns list of available starting points.
        /// </summary>
        public List<StartingPoint> StartingPoints
        {
            get { return this.Actors.Values.OfType<StartingPoint>().Select(actor => actor).ToList(); }
        }

        /// <summary>
        /// Creates a new world for the given game with given snoId.
        /// </summary>
        /// <param name="game">The parent game.</param>
        /// <param name="snoId">The snoId for the world.</param>
        public World(Game game, int snoId)
            : base(game.NewWorldID)
        {
            this.Game = game;
            this.SNOId = snoId; // NOTE: WorldSNO must be valid before adding it to the game
            this.Game.StartTracking(this); // start tracking the dynamicId for the world.            
            this.Scenes = new ConcurrentDictionary<uint, Scene>();
            this.Actors = new ConcurrentDictionary<uint, Actor>();
            this.Players = new ConcurrentDictionary<uint, Player>();
            this.QuadTree = new QuadTree(new Size(60, 60), 0);
            this.Game.AddWorld(this); 
        }

        #region update & tick logic

        public override void Update()
        {
            // update actors.
            foreach (var pair in this.Actors) { pair.Value.Update(); }

            // update players.
            foreach (var pair in this.Players) { pair.Value.Update(); }
        }

        #endregion

        #region message broadcasting

        // NOTE: Scenes are actually laid out in cells with Subscenes filling in certain areas under a Scene.
        // We can use this design feature to track Actors' current scene and send updates to it and neighboring
        // scenes instead of distance checking for broadcasting messages. / komiga
        // I'll be soon adding that feature /raist.

        /// <summary>
        /// Broadcasts a message for a given actor to only players that actor has been revealed.
        /// </summary>
        /// <param name="message">The message to broadcast.</param>
        /// <param name="actor">The actor.</param>
        public void BroadcastIfRevealed(GameMessage message, Actor actor)
        {
            foreach (var player in this.Players.Values)
            {
                if (player.RevealedObjects.ContainsKey(actor.DynamicID))
                    player.InGameClient.SendMessage(message);
            }
        }

        /// <summary>
        /// Broadcasts a message to all players in the world.
        /// </summary>
        /// <param name="message"></param>
        public void BroadcastGlobal(GameMessage message)
        {
            foreach (var player in this.Players.Values)
            {
                player.InGameClient.SendMessage(message);
            }
        }

        /// <summary>
        /// Broadcasts a message to all players in the range of given actor.
        /// </summary>
        /// <param name="message">The message to broadcast.</param>
        /// <param name="actor">The actor.</param>
        public void BroadcastInclusive(GameMessage message, Actor actor)
        {
            var players = this.GetPlayersInRange(actor.Position, 480.0f);
            foreach (var player in players)
            {
                player.InGameClient.SendMessage(message);
            }
        }

        /// <summary>
        /// Broadcasts a message to all players in the range of given actor, but not the player itself if actor is the player.
        /// </summary>
        /// <param name="message">The message to broadcast.</param>
        /// <param name="actor">The actor.</param>
        public void BroadcastExclusive(GameMessage message, Actor actor)
        {
            var players = this.GetPlayersInRange(actor.Position, 480.0f);
            foreach (var player in players.Where(player => player != actor))
            {
                player.InGameClient.SendMessage(message);
            }
        }

        #endregion

        #region reveal logic

        /// <summary>
        /// Reveals the world to given player.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <returns></returns>
        public bool Reveal(Player player)
        {
            if (player.RevealedObjects.ContainsKey(this.DynamicID))
                return false;

            player.InGameClient.SendMessage(new RevealWorldMessage() // Reveal world to player
            {
                WorldID = this.DynamicID,
                WorldSNO = this.SNOId,
            });

            player.InGameClient.SendMessage(new EnterWorldMessage()
            {
                EnterPosition = player.Position,
                WorldID = this.DynamicID,
                WorldSNO = this.SNOId,
            });

            this.RevealScenesInProximity(player); // reveal scenes in players proximity.
            this.RevealActorsInProximity(player); // reveal actors in players proximity.

            player.RevealedObjects.Add(this.DynamicID, this);

            return true;
        }

        /// <summary>
        /// Reveals scenes in player's proximity.
        /// </summary>
        /// <param name="player">The player</param>
        public void RevealScenesInProximity(Player player)
        {
            var proximity = new Rect(player.Position.X - SceneProximity / 2, player.Position.Y - SceneProximity / 2, SceneProximity, SceneProximity);
            var scenesInProximity = this.QuadTree.Query<Scene>(proximity);

            foreach (var scene in scenesInProximity) // reveal scenes in player's proximity.
            {
                scene.Reveal(player);
            }

            // var alreadyRevealedScenes = player.GetRevealedObjects<Scene>(); - disabled for time being /raist
            //foreach(var scene in alreadyRevealedScenes) // unreveal scenes that are not in player's proximity.
            //{
            //    if (!scenesInProximity.Contains(scene))
            //        scene.Unreveal(player);
            //}
        }

        /// <summary>
        /// Revals actors in player's proximity.
        /// </summary>
        /// <param name="player"></param>
        public void RevealActorsInProximity(Player player)
        {
            var proximity = new Rect(player.Position.X - ActorProximity / 2, player.Position.Y - ActorProximity / 2, ActorProximity, ActorProximity);
            var actorsInProximity = this.QuadTree.Query<Actor>(proximity);

            foreach (var actor in actorsInProximity) // reveal actors in player's proximity.
            {
                if (actor.ActorType == ActorType.Gizmo || actor.ActorType == ActorType.Player || actor.ActorType == ActorType.Monster)
                    actor.Reveal(player);
            }

            // var alreadyRevealedActors = player.GetRevealedObjects<Actor>(); - disabled for time being - crashing on monster kills /raist.
            //foreach (var actor in alreadyRevealedActors) // unreveal actors that are not in player's proximity.
            //{
            //    if (!actorsInProximity.Contains(actor))
            //        actor.Unreveal(player);
            //}
        }

        /// <summary>
        /// Unreveals the world to player.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <returns></returns>
        public bool Unreveal(Player player)
        {
            if (!player.RevealedObjects.ContainsKey(this.DynamicID))
                return false;
            
            player.InGameClient.SendMessage(new WorldDeletedMessage() { WorldID = DynamicID });

            // TODO: Unreveal all objects in the world? I think the client will do this on its own when it gets a WorldDeletedMessage /komiga
            //foreach (var obj in player.RevealedObjects.Values)
            //    if (obj.DynamicID == this.DynamicID) obj.Unreveal(player);

            return true;
        }

        #endregion

        #region actor enter & leave functionality

        /// <summary>
        /// Allows an actor to enter the world.
        /// </summary>
        /// <param name="actor">The actor entering the world.</param>
        public void Enter(Actor actor)
        {
            // NOTE: Revealing to all right now since the flow results in actors that have initial positions that are not within the range of the player. /komiga

            this.AddActor(actor);
            actor.OnEnter(this);

            var players = this.Players.Values; //this.GetPlayersInRange(actor.Position, 480.0f);
            foreach (var player in players)
            {
                actor.Reveal(player);
            }
        }

        /// <summary>
        /// Allows an actor to leave the world.
        /// </summary>
        /// <param name="actor">The actor leaving the world.</param>
        public void Leave(Actor actor)
        {
            actor.OnLeave(this);

            foreach (var player in this.Players.Values)
            {
                if (actor != player)
                    actor.Unreveal(player);
            }

            this.RemoveActor(actor);

            if (!(actor is Player)) return;

            // if the leaving actors is a player, unreveal the actors revealed to him contained in the world.
            var revealedToPlayer = (actor as Player).RevealedObjects.Values.ToList(); // list of revealed actors.
            foreach (IRevealable revealable in revealedToPlayer)
                if (revealable is Actor)
                    if (revealable != actor)
                        revealable.Unreveal(actor as Player);
        }

        #endregion

        #region monster spawning & item drops

        /// <summary>
        /// Spawns a monster with given SNOId in given position.
        /// </summary>
        /// <param name="monsterSNOId">The SNOId of the monster.</param>
        /// <param name="position">The position to spawn it.</param>
        public void SpawnMonster(int monsterSNOId, Vector3D position)
        {
            var monster = new Monster(this, monsterSNOId, position, new Dictionary<int, Mooege.Common.MPQ.FileFormats.Types.TagMapEntry>()) { Scale = 1.35f };
            this.Enter(monster);
        }

        /// <summary>
        /// Spawns a random item drop for given player.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <param name="position">The position for drop.</param>
        public void SpawnRandomItemDrop(Player player, Vector3D position)
        {
            var item = ItemGenerator.GenerateRandom(player);

            item.Drop(null, position); // NOTE: The owner field for an item is only set when it is in the owner's inventory. /komiga
            player.GroundItems[item.DynamicID] = item; // FIXME: Hacky. /komiga
        }

        /// <summary>
        /// Spanws gold for given player.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <param name="position">The position for drop.</param>
        public void SpawnGold(Player player, Vector3D position)
        {
<<<<<<< HEAD
            var item = ItemGenerator.CreateGold(player, RandomHelper.Next(1000, 3000)); // somehow the actual ammount is not shown on ground /raist.
=======
            // TODO: Gold should be spawned for all players in range. /raist.
            var item = ItemGenerator.CreateGold(player, RandomHelper.Next(1, 3)); // somehow the actual ammount is not shown on ground /raist.
>>>>>>> mooege/master
            item.Drop(null, position);
        }

        /// <summary>
        /// Spanws a health-globe for given player.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <param name="position">The position for drop.</param>
        public void SpawnHealthGlobe(Player player, Vector3D position)
        {
            // TODO: Health-globe should be spawned for all players in range. /raist.
            var item = ItemGenerator.CreateGlobe(player, RandomHelper.Next(1, 28)); // somehow the actual ammount is not shown on ground /raist.
            item.Drop(null, position);
        }

        #endregion

        #region collections managemnet

        /// <summary>
        /// Adds given scene to world.
        /// </summary>
        /// <param name="scene">The scene to add.</param>
        public void AddScene(Scene scene)
        {
            if (scene.DynamicID == 0 || HasScene(scene.DynamicID))
                throw new Exception(String.Format("Object has an invalid ID or was already present (ID = {0})", scene.DynamicID));

            this.Scenes.TryAdd(scene.DynamicID, scene); // add to scenes collection.
            this.QuadTree.Insert(scene); // add it to quad-tree too.
        }

        /// <summary>
        /// Removes given scene from world.
        /// </summary>
        /// <param name="scene">The scene to remove.</param>
        public void RemoveScene(Scene scene)
        {
            if (scene.DynamicID == 0 || !HasScene(scene.DynamicID))
                throw new Exception(String.Format("Object has an invalid ID or was not present (ID = {0})", scene.DynamicID));

            Scene remotedScene;
            this.Scenes.TryRemove(scene.DynamicID, out remotedScene); // remove it from scenes collection.
            this.QuadTree.Remove(scene); // remove from quad-tree too.
        }

        /// <summary>
        /// Returns the scene with given dynamicId.
        /// </summary>
        /// <param name="dynamicID">The dynamicId of the scene.</param>
        /// <returns></returns>
        public Scene GetScene(uint dynamicID)
        {
            Scene scene;
            this.Scenes.TryGetValue(dynamicID, out scene);
            return scene;
        }

        /// <summary>
        /// Returns true if world contains a scene with given dynamicId.
        /// </summary>
        /// <param name="dynamicID">The dynamicId of the scene.</param>
        /// <returns><see cref="bool"/></returns>
        public bool HasScene(uint dynamicID)
        {
            return this.Scenes.ContainsKey(dynamicID);
        }

        /// <summary>
        /// Adds given actor to world.
        /// </summary>
        /// <param name="actor">The actor to add.</param>
        private void AddActor(Actor actor)
        {
            if (actor.DynamicID == 0 || HasActor(actor.DynamicID))
                throw new Exception(String.Format("Object has an invalid ID or was already present (ID = {0})", actor.DynamicID));

            this.Actors.TryAdd(actor.DynamicID, actor); // add to actors collection.
            this.QuadTree.Insert(actor); // add it to quad-tree too.

            if (actor.ActorType == ActorType.Player) // if actor is a player, add it to players collection too.
                this.AddPlayer((Player)actor);
        }

        /// <summary>
        /// Removes given actor from world.
        /// </summary>
        /// <param name="actor">The actor to remove.</param>
        private void RemoveActor(Actor actor)
        {
            if (actor.DynamicID == 0 || !this.Actors.ContainsKey(actor.DynamicID))
                throw new Exception(String.Format("Object has an invalid ID or was not present (ID = {0})", actor.DynamicID));
            
            Actor removedActor;
            this.Actors.TryRemove(actor.DynamicID, out removedActor); // remove it from actors collection.
            this.QuadTree.Remove(actor); // remove from quad-tree too.

            if (actor.ActorType == ActorType.Player) // if actors is a player, remove it from players collection too.
                this.RemovePlayer((Player)actor);
        }

        /// <summary>
        /// Returns the actor with given dynamicId.
        /// </summary>
        /// <param name="dynamicID">The dynamicId of the actor.</param>
        /// <returns></returns>
        public Actor GetActor(uint dynamicID)
        {
            Actor actor;
            this.Actors.TryGetValue(dynamicID, out actor);
            return actor;
        }

        /// <summary>
        /// Returns the actor with given dynamicId.
        /// </summary>
        /// <param name="dynamicID">The dynamicId of the actor.</param>
        /// <param name="matchType">The actor-type.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Returns true if the world has an actor with given dynamicId.
        /// </summary>
        /// <param name="dynamicID">The dynamicId of the actor.</param>
        /// <returns><see cref="bool"/></returns>
        public bool HasActor(uint dynamicID)
        {
            return this.Actors.ContainsKey(dynamicID);
        }

        /// <summary>
        /// Returns true if the world has an actor with given dynamicId and type.
        /// </summary>
        /// <param name="dynamicID">The dynamicId of the actor.</param>
        /// <param name="matchType">The type of the actor.</param>
        /// <returns></returns>
        public bool HasActor(uint dynamicID, ActorType matchType)
        {
            var actor = GetActor(dynamicID, matchType);
            return actor != null;
        }

        /// <summary>
        /// Adds given player to world.
        /// </summary>
        /// <param name="player">The player to add.</param>
        private void AddPlayer(Player player)
        {
            if (player.DynamicID == 0 || HasPlayer(player.DynamicID))
                throw new Exception(String.Format("Object has an invalid ID or was already present (ID = {0})", player.DynamicID));

            this.Players.TryAdd(player.DynamicID, player); // add it to players collection.
        }

        /// <summary>
        /// Removes given player from world.
        /// </summary>
        /// <param name="player"></param>
        private void RemovePlayer(Player player)
        {
            if (player.DynamicID == 0 || !this.Players.ContainsKey(player.DynamicID))
                throw new Exception(String.Format("Object has an invalid ID or was not present (ID = {0})", player.DynamicID));

            Player removedPlayer;
            this.Players.TryRemove(player.DynamicID, out removedPlayer); // remove it from players collection.
        }

        /// <summary>
        /// Returns player with given dynamicId.
        /// </summary>
        /// <param name="dynamicID">The dynamicId of the player.</param>
        /// <returns></returns>
        public Player GetPlayer(uint dynamicID)
        {
            Player player;
            this.Players.TryGetValue(dynamicID, out player);
            return player;
        }

        /// <summary>
        /// Returns true if world contains a player with given dynamicId.
        /// </summary>
        /// <param name="dynamicID">The dynamicId of the player.</param>
        /// <returns><see cref="bool"/></returns>
        public bool HasPlayer(uint dynamicID)
        {
            return this.Players.ContainsKey(dynamicID);
        }

        /// <summary>
        /// Returns item with given dynamicId.
        /// </summary>
        /// <param name="dynamicID">The dynamicId of the item.</param>
        /// <returns></returns>
        public Item GetItem(uint dynamicID)
        {
            return (Item)GetActor(dynamicID, ActorType.Item);
        }

        /// <summary>
        /// Returns true if world contains a monster with given dynamicId.
        /// </summary>
        /// <param name="dynamicID">The dynamicId of the monster.</param>
        /// <returns><see cref="bool"/></returns>
        public bool HasMonster(uint dynamicID)
        {
            return HasActor(dynamicID, ActorType.Monster);
        }

        /// <summary>
        /// Returns true if world contains an item with given dynamicId.
        /// </summary>
        /// <param name="dynamicID">The dynamicId of the item.</param>
        /// <returns><see cref="bool"/></returns>
        public bool HasItem(uint dynamicID)
        {
            return HasActor(dynamicID, ActorType.Item);
        }

        #endregion

        #region range-based queries

        // TODO: replace this stuff with quad-tree based queries /raist.

        /// <summary>
        /// Returns list of actors in range with given snoId.
        /// </summary>
        /// <param name="actorSNOId">The actor SNOId.</param>
        /// <param name="x">The position's x-coordinate</param>
        /// <param name="y">The position's y-coordinate</param>
        /// <param name="z">The position's z-coordinate</param>
        /// <param name="range">The range.</param>
        /// <returns>List of actors.</returns>
        public List<Actor> GetActorsInRange(int actorSNOId, float x, float y, float z, float range)
        {
            var result = new List<Actor>();

            foreach (var actor in this.Actors.Values)
            {
                if (actor.SNOId == actorSNOId && (Math.Sqrt(Math.Pow(actor.Position.X - x, 2) + Math.Pow(actor.Position.Y - y, 2) + Math.Pow(actor.Position.Z - z, 2)) <= range))
                {
                    result.Add(actor);
                }
            }

            return result;
        }

        /// <summary>
        /// Returns list of actors in range with given snoId.
        /// </summary>
        /// <param name="actorSNOId">The actor snoId.</param>
        /// <param name="position">The position.</param>
        /// <param name="range">The range.</param>
        /// <returns>List of actors.</returns>
        public List<Actor> GetActorsInRange(int actorSNOId, Vector3D position, float range)
        {
            return GetActorsInRange(actorSNOId, position.X, position.Y, position.Z, range);
        }

        /// <summary>
        /// Returns list of actors.
        /// </summary>
        /// <param name="x">The position's x-coordinate</param>
        /// <param name="y">The position's y-coordinate</param>
        /// <param name="z">The position's z-coordinate</param>
        /// <param name="range">The range.</param>
        /// <returns>List of actors.</returns>
        public List<Actor> GetActorsInRange(float x, float y, float z, float range)
        {
            var result = new List<Actor>();

            foreach (var actor in this.Actors.Values)
            {
                if (Math.Sqrt(Math.Pow(actor.Position.X - x, 2) + Math.Pow(actor.Position.Y - y, 2) + Math.Pow(actor.Position.Z - z, 2)) <= range)
                {
                    result.Add(actor);
                }
            }

            return result;
        }

        /// <summary>
        /// Returns list of actors.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="range">The range.</param>
        /// <returns>List of actors.</returns>
        public List<Actor> GetActorsInRange(Vector3D position, float range)
        {
            return GetActorsInRange(position.X, position.Y, position.Z, range);
        }

        /// <summary>
        /// Returns list of players in range.
        /// </summary>
        /// <param name="x">The position's x-coordinate</param>
        /// <param name="y">The position's y-coordinate</param>
        /// <param name="z">The position's z-coordinate</param>
        /// <param name="range">The range.</param>
        /// <returns>List of players.</returns>
        public List<Player> GetPlayersInRange(float x, float y, float z, float range)
        {
            var result = new List<Player>();
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

        /// <summary>
        /// Returns list of players in range.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="range">The range.</param>
        /// <returns>List of actors.</returns>
        public List<Player> GetPlayersInRange(Vector3D position, float range)
        {
            return GetPlayersInRange(position.X, position.Y, position.Z, range);
        }

        #endregion

        #region misc-queries

        /// <summary>
        /// Returns StartingPoint with given id.
        /// </summary>
        /// <param name="id">The id of the StartingPoint.</param>
        /// <returns><see cref="StartingPoint"/></returns>
        public StartingPoint GetStartingPointById(int id)
        {
            return Actors.Values.OfType<StartingPoint>().FirstOrDefault(startingPoint => startingPoint.TargetId == id);
        }

        /// <summary>
        /// Returns WayPoint with given id.
        /// </summary>
        /// <param name="id">The id of the WayPoint</param>
        /// <returns><see cref="WayPoint"/></returns>
        public Waypoint GetWayPointById(int id)
        {
            return Actors.Values.OfType<Waypoint>().FirstOrDefault(waypoint => waypoint.WaypointId == id);
        }

        #endregion

        #region destroy, ctor, finalizer

        public override void Destroy()
        {
            // TODO: Destroy all objects /raist
            Game game = this.Game;
            this.Game = null;
            game.EndTracking(this);
        }

        #endregion
    }
}
