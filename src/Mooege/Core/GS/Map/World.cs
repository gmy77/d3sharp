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
using Mooege.Core.GS.Common.Types.Math;
using Mooege.Core.GS.Common.Types.QuadTrees;
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
        private readonly ConcurrentDictionary<uint, Scene> _scenes;

        /// <summary>
        /// List of actors contained in the world.
        /// </summary>
        private readonly ConcurrentDictionary<uint, Actor> _actors;

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
        /// Returns list of available starting points.
        /// </summary>
        public List<StartingPoint> StartingPoints
        {
            get { return this._actors.Values.OfType<StartingPoint>().Select(actor => actor).ToList(); }
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
            this._scenes = new ConcurrentDictionary<uint, Scene>();
            this._actors = new ConcurrentDictionary<uint, Actor>();
            this.Players = new ConcurrentDictionary<uint, Player>();
            this.QuadTree = new QuadTree(new Size(60, 60), 0);
            this.Game.AddWorld(this); 
        }

        #region update & tick logic

        public override void Update()
        {
            // TODO: we should be skipping child-scenes, so actors contained doesn't get updated() twice.
            foreach(var scene in this._scenes.Values)
            {
                if (!scene.HasPlayers) 
                    continue; // if scene has no players in, just skip the scene.

                scene.Update();
            }
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
            var players=actor.GetPlayersInRange();
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
            var players = actor.GetPlayersInRange();
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

            player.RevealedObjects.Add(this.DynamicID, this);

            return true;
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

            // player.InGameClient.SendMessage(new WorldDeletedMessage() { WorldID = DynamicID });/ / don't delete the old world or beta-client will be crashing! /raist.
            player.RevealedObjects.Remove(this.DynamicID);            
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
            this.AddActor(actor);
            actor.OnEnter(this);

            // reveal actor to player's in-range.
            foreach(var player in  actor.GetPlayersInRange())
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

            if (!(actor is Player)) return; // if the leaving actors is a player, unreveal the actors revealed to him contained in the world.
            var revealedObjects = (actor as Player).RevealedObjects.Values.ToList(); // list of revealed actors.
            foreach (var @object in revealedObjects)
                    if(@object!=actor) // do not unreveal the player itself.
                        @object.Unreveal(actor as Player);
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
            var monster = new Monster(this, monsterSNOId, new Dictionary<int, Mooege.Common.MPQ.FileFormats.Types.TagMapEntry>()) { Scale = 1.35f };
            monster.EnterWorld(position);
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
            var item = ItemGenerator.CreateGold(player, RandomHelper.Next(1000, 3000)); // somehow the actual ammount is not shown on ground /raist.
            // TODO: Gold should be spawned for all players in range. /raist.
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

            this._scenes.TryAdd(scene.DynamicID, scene); // add to scenes collection.
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
            this._scenes.TryRemove(scene.DynamicID, out remotedScene); // remove it from scenes collection.
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
            this._scenes.TryGetValue(dynamicID, out scene);
            return scene;
        }

        /// <summary>
        /// Returns true if world contains a scene with given dynamicId.
        /// </summary>
        /// <param name="dynamicID">The dynamicId of the scene.</param>
        /// <returns><see cref="bool"/></returns>
        public bool HasScene(uint dynamicID)
        {
            return this._scenes.ContainsKey(dynamicID);
        }

        /// <summary>
        /// Adds given actor to world.
        /// </summary>
        /// <param name="actor">The actor to add.</param>
        private void AddActor(Actor actor)
        {
            if (actor.DynamicID == 0 || HasActor(actor.DynamicID))
                throw new Exception(String.Format("Object has an invalid ID or was already present (ID = {0})", actor.DynamicID));

            this._actors.TryAdd(actor.DynamicID, actor); // add to actors collection.
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
            if (actor.DynamicID == 0 || !this._actors.ContainsKey(actor.DynamicID))
                throw new Exception(String.Format("Object has an invalid ID or was not present (ID = {0})", actor.DynamicID));
            
            Actor removedActor;
            this._actors.TryRemove(actor.DynamicID, out removedActor); // remove it from actors collection.
            this.QuadTree.Remove(actor); // remove from quad-tree too.

            if (actor.ActorType == ActorType.Player) // if actors is a player, remove it from players collection too.
                this.RemovePlayer((Player)actor);
        }

        // TODO: We should be instead using actor queries. /raist.
        /// <summary>
        /// Returns the actor with given dynamicId.
        /// </summary>
        /// <param name="dynamicID">The dynamicId of the actor.</param>
        /// <returns></returns>
        public Actor GetActorByDynamicId(uint dynamicID)
        {
            Actor actor;
            this._actors.TryGetValue(dynamicID, out actor);
            return actor;
        }

        /// <summary>
        /// Returns the actor with given dynamicId.
        /// </summary>
        /// <param name="dynamicID">The dynamicId of the actor.</param>
        /// <param name="matchType">The actor-type.</param>
        /// <returns></returns>
        public Actor GetActorByDynamicId(uint dynamicID, ActorType matchType)
        {
            var actor = this.GetActorByDynamicId(dynamicID);
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
            return this._actors.ContainsKey(dynamicID);
        }

        /// <summary>
        /// Returns true if the world has an actor with given dynamicId and type.
        /// </summary>
        /// <param name="dynamicID">The dynamicId of the actor.</param>
        /// <param name="matchType">The type of the actor.</param>
        /// <returns></returns>
        public bool HasActor(uint dynamicID, ActorType matchType)
        {
            var actor = this.GetActorByDynamicId(dynamicID, matchType);
            return actor != null;
        }

        /// <summary>
        /// Returns actor instance with given type.
        /// </summary>
        /// <typeparam name="T">Type of the actor.</typeparam>
        /// <returns>Actor</returns>
        public T GetActorInstance<T>() where T: Actor
        {
            return this._actors.Values.OfType<T>().FirstOrDefault();
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
            return (Item)GetActorByDynamicId(dynamicID, ActorType.Item);
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

        #region misc-queries

        /// <summary>
        /// Returns StartingPoint with given id.
        /// </summary>
        /// <param name="id">The id of the StartingPoint.</param>
        /// <returns><see cref="StartingPoint"/></returns>
        public StartingPoint GetStartingPointById(int id)
        {
            return _actors.Values.OfType<StartingPoint>().FirstOrDefault(startingPoint => startingPoint.TargetId == id);
        }

        /// <summary>
        /// Returns WayPoint with given id.
        /// </summary>
        /// <param name="id">The id of the WayPoint</param>
        /// <returns><see cref="Waypoint"/></returns>
        public Waypoint GetWayPointById(int id)
        {
            return _actors.Values.OfType<Waypoint>().FirstOrDefault(waypoint => waypoint.WaypointId == id);
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

        public override string ToString()
        {
            return string.Format("[World] SNOId: {0} DynamicId: {1}", this.SNOId, this.DynamicID);
        }
    }
}
