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
using System.Linq;
using Mooege.Common;
using Mooege.Core.GS.Game;
using Mooege.Core.GS.Objects;
using Mooege.Core.GS.Actors;
using Mooege.Core.GS.Player;
using Mooege.Core.GS.NPC;
using Mooege.Core.GS.Data.SNO;
using Mooege.Core.Common.Items;
using Mooege.Net.GS.Message;
using Mooege.Net.GS.Message.Fields;
using Mooege.Net.GS.Message.Definitions.Map;
using Mooege.Net.GS.Message.Definitions.Misc;
using Mooege.Net.GS.Message.Definitions.Scene;
using Mooege.Net.GS.Message.Definitions.World;

// NOTE: Scenes are actually laid out in cells with Subscenes filling in certain areas under a Scene.
//  We can use this design feature to track Actors' current scene and send updates to it and neighboring
//  scenes instead of distance checking for broadcasting messages.

namespace Mooege.Core.GS.Map
{
    public sealed class World : DynamicObject, IRevealable
    {
        static readonly Logger Logger = LogManager.CreateLogger();

        public Mooege.Core.GS.Game.Game Game { get; private set; }

        private Dictionary<uint, Scene> Scenes;
        private Dictionary<uint, Actor> Actors;
        private Dictionary<uint, Mooege.Core.GS.Player.Player> Players; // Temporary for fast iteration for now since move/enter/leave handling is at the world level instead of the scene level

        public int WorldSNO { get; set; }

        public uint NewSceneID { get { return this.Game.NewSceneID; } }
        public uint NewActorID { get { return this.Game.NewObjectID; } }
        public uint NewPlayerID { get { return this.Game.NewObjectID; } }

        public World(Mooege.Core.GS.Game.Game game, int worldSNO)
            : base(game.NewWorldID)
        {
            this.Game = game;
            this.Game.StartTracking(this);
            this.Scenes = new Dictionary<uint, Scene>();
            this.Actors = new Dictionary<uint, Actor>();
            this.Players = new Dictionary<uint, Mooege.Core.GS.Player.Player>();

            // NOTE: WorldSNO must be valid before adding it to the game
            this.WorldSNO = worldSNO;
            this.Game.AddWorld(this);
        }

        public void BroadcastInclusive(GameMessage message, Actor actor)
        {
            var players = this.GetPlayersInRange(actor.Position, 480.0f);
            foreach (var player in players)
            {
                player.InGameClient.SendMessageNow(message);
            }
        }

        public void BroadcastExclusive(GameMessage message, Actor actor)
        {
            var players = this.GetPlayersInRange(actor.Position, 480.0f);
            foreach (var player in players)
            {
                if (player != actor)
                {
                    player.InGameClient.SendMessageNow(message);
                }
            }
        }

        public void OnActorMove(Actor actor, Vector3D prevPosition)
        {
            // TODO: Unreveal from players that are now outside the actor's range
            BroadcastExclusive(actor.ACDWorldPositionMessage, actor);
        }

        // TODO: NewPlayer messages should be broadcasted at the Game level, which means we may have to track players separately from objects in Game
        public void Enter(Actor actor)
        {
            this.AddActor(actor);
            actor.OnEnter(this);
            // Broadcast reveal
            var players = this.GetPlayersInRange(actor.Position, 480.0f);
            foreach (var player in players)
            {
                actor.Reveal(player);
            }
        }

        public void Leave(Actor actor)
        {
            actor.OnLeave(this);
            // Broadcast unreveal
            var players = this.GetPlayersInRange(actor.Position, 480.0f);
            foreach (var player in players)
            {
                actor.Unreveal(player);
            }
            this.RemoveActor(actor);
        }

        public void Reveal(Mooege.Core.GS.Player.Player player)
        {
            if (!player.RevealedObjects.ContainsKey(this.DynamicID))
            {
                // Reveal world to player
                player.InGameClient.SendMessage(new RevealWorldMessage()
                {
                    WorldID = this.DynamicID,
                    WorldSNO = this.WorldSNO,
                });
                player.RevealedObjects.Add(this.DynamicID, this);

                // Revealing all scenes for now..
                Logger.Info("Revealing scenes for world {0}", this.DynamicID);
                foreach (var scene in this.Scenes.Values)
                {
                    scene.Reveal(player);
                }

                // Reveal all actors
                // TODO: We need proper location-aware reveal logic for _all_ objects
                //       This can be done on the scene level once that bit is in
                Logger.Info("Revealing all actors for world {0}", this.DynamicID);
                foreach (var actor in Actors.Values)
                {
                    actor.Reveal(player);
                }
            }
        }

        public void Unreveal(Mooege.Core.GS.Player.Player player)
        {
            // TODO: Unreveal all objects in the world? I think the client will do this on its own when it gets a WorldDeletedMessage
            //foreach (var obj in player.RevealedObjects.Values)
            //    if (obj.DynamicID == this.DynamicID) obj.Unreveal(player);

            player.InGameClient.SendMessage(new WorldDeletedMessage() { WorldID = DynamicID });
            player.InGameClient.FlushOutgoingBuffer();
        }

        public override void Destroy()
        {
            // TODO: Destroy all objects
            Mooege.Core.GS.Game.Game game = this.Game;
            this.Game = null;
            game.EndTracking(this);
        }

        // TODO: All the things.
        public void Tick()
        {
        }

        #region Collections

        // Adding
        public void AddScene(Scene obj)
        {
            if (obj.DynamicID == 0 || this.Scenes.ContainsKey(obj.DynamicID))
                throw new Exception(String.Format("Object has an invalid ID or was already present (ID = {0})", obj.DynamicID));
            this.Scenes.Add(obj.DynamicID, obj);
        }

        private void AddActor(Actor obj)
        {
            if (obj.DynamicID == 0 || this.Actors.ContainsKey(obj.DynamicID))
                throw new Exception(String.Format("Object has an invalid ID or was already present (ID = {0})", obj.DynamicID));
            this.Actors.Add(obj.DynamicID, obj);
            if (obj.ActorType == ActorType.Player) // temp
                this.AddPlayer((Mooege.Core.GS.Player.Player)obj);
        }

        private void AddPlayer(Mooege.Core.GS.Player.Player obj)
        {
            if (obj.DynamicID == 0 || this.Players.ContainsKey(obj.DynamicID))
                throw new Exception(String.Format("Object has an invalid ID or was already present (ID = {0})", obj.DynamicID));
            this.Players.Add(obj.DynamicID, obj);
        }

        // Removing
        public void RemoveScene(Scene obj)
        {
            if (obj.DynamicID == 0 || !this.Scenes.ContainsKey(obj.DynamicID))
                throw new Exception(String.Format("Object has an invalid ID or was not present (ID = {0})", obj.DynamicID));
            this.Scenes.Remove(obj.DynamicID);
        }

        private void RemoveActor(Actor obj)
        {
            if (obj.DynamicID == 0 || !this.Actors.ContainsKey(obj.DynamicID))
                throw new Exception(String.Format("Object has an invalid ID or was not present (ID = {0})", obj.DynamicID));
            this.Actors.Remove(obj.DynamicID);
            if (obj.ActorType == ActorType.Player) // temp
                this.RemovePlayer((Mooege.Core.GS.Player.Player)obj);
        }

        private void RemovePlayer(Mooege.Core.GS.Player.Player obj)
        {
            if (obj.DynamicID == 0 || !this.Players.ContainsKey(obj.DynamicID))
                throw new Exception(String.Format("Object has an invalid ID or was not present (ID = {0})", obj.DynamicID));
            this.Players.Remove(obj.DynamicID);
        }

        // Getters
        public Scene GetScene(uint dynamicID)
        {
            Scene scene;
            this.Scenes.TryGetValue(dynamicID, out scene);
            return scene;
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

        public Mooege.Core.GS.Player.Player GetPlayer(uint dynamicID)
        {
            Mooege.Core.GS.Player.Player player;
            this.Players.TryGetValue(dynamicID, out player);
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
        }

        public bool HasActor(uint dynamicID)
        {
            return this.Actors.ContainsKey(dynamicID);
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

        public List<Actor> GetActorsInRange(int SNO, float x, float y, float z, float range)
        {
            var result = new List<Actor>();
            foreach (var actor in this.Actors.Values)
            {
                if (actor.AppearanceSNO == SNO &&
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
            foreach (var actor in this.Actors.Values)
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
