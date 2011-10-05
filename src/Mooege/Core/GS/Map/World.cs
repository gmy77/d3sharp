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
using Mooege.Core.GS.NPC;
using Mooege.Core.GS.Data.SNO;
using Mooege.Core.Common.Items;
using Mooege.Net.GS.Message.Definitions.Map;
using Mooege.Net.GS.Message.Definitions.Misc;
using Mooege.Net.GS.Message.Definitions.Scene;
using Mooege.Net.GS.Message.Definitions.World;

namespace Mooege.Core.GS.Map
{
    public class World : IDynamicObject
    {
        static readonly Logger Logger = LogManager.CreateLogger();

        public Mooege.Core.GS.Game.Game Game { get; private set; }
        public uint DynamicID { get; private set; }

        private Dictionary<uint, Scene> Scenes;
        private Dictionary<uint, Portal> Portals;
        private Dictionary<uint, Actor> Actors;
        private Dictionary<uint, Player> Players;
        private Dictionary<uint, Item> Items;

        public int WorldSNO { get; set; }

        public World(Mooege.Core.GS.Game.Game game)
        {
            this.Game = game;
            this.DynamicID = this.Game.NewWorldID;
            this.Game.AddWorld(this);

            this.Scenes = new Dictionary<uint, Scene>();
            this.Portals = new Dictionary<uint, Portal>();
            this.Actors = new Dictionary<uint, Actor>();
            this.Players = new Dictionary<uint, Player>();
            this.Items = new Dictionary<uint, Item>();
        }

        #region Collections

        // Adding
        public void AddScene(Scene obj)
        {
            if (obj.DynamicID == 0 || this.Scenes.ContainsKey(obj.DynamicID))
                throw new Exception(String.Format("Object has an invalid ID or was already present ({0})", obj.DynamicID));
            this.Scenes.Add(obj.DynamicID, obj);
        }

        public void AddPortal(Portal obj)
        {
            if (obj.DynamicID == 0 || this.Portals.ContainsKey(obj.DynamicID))
                throw new Exception(String.Format("Object has an invalid ID or was already present ({0})", obj.DynamicID));
            this.Portals.Add(obj.DynamicID, obj);
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
        public void RemoveScene(Scene obj)
        {
            if (obj.DynamicID == 0 || !this.Scenes.ContainsKey(obj.DynamicID))
                throw new Exception(String.Format("Object has an invalid ID or was not present ({0})", obj.DynamicID));
            this.Scenes.Remove(obj.DynamicID);
        }

        public void RemovePortal(Portal obj)
        {
            if (obj.DynamicID == 0 || !this.Portals.ContainsKey(obj.DynamicID))
                throw new Exception(String.Format("Object has an invalid ID or was not present ({0})", obj.DynamicID));
            this.Portals.Remove(obj.DynamicID);
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
        public Scene GetScene(uint dynamicID)
        {
            Scene scene;
            this.Scenes.TryGetValue(dynamicID, out scene);
            return scene;
        }

        public Portal GetPortal(uint dynamicID)
        {
            Portal portal;
            this.Portals.TryGetValue(dynamicID, out portal);
            return portal;
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
        public bool HasScene(uint dynamicID)
        {
            return this.Scenes.ContainsKey(dynamicID);
        }

        public bool HasPortal(uint dynamicID)
        {
            return this.Portals.ContainsKey(dynamicID);
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

        // get actor by snoid and x,y,z position - this is a helper function to prune duplicate entries from the packet data
        public Actor GetActor(int snoID, float x, float y, float z)
        {
            foreach (var actor in this.Actors.Values)
            {
                if (actor.AppearanceSNO == snoID &&
                    actor.Position.X == x &&
                    actor.Position.Y == y &&
                    actor.Position.Z == z)
                    return actor;
            }
            return null;
        }

        public List<Actor> GetActorsInRange(int snoID, float x, float y, float z, float range)
        {
            List<Actor> result = new List<Actor>();
            foreach (var actor in this.Actors.Values)
            {
                if (actor.AppearanceSNO == snoID &&
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

        public bool ActorExists(Actor actor)
        {
            return Actors.Any(
                t =>
                    t.Value.AppearanceSNO == actor.AppearanceSNO &&
                    t.Value.Position.X == actor.Position.X &&
                    t.Value.Position.Y == actor.Position.Y &&
                    t.Value.Position.Z == actor.Position.Z);
        }

        // Why do we need to sort scenes?
        /*private int SceneSorter(Scene x, Scene y)
        {
            if (x.SceneData.DynamicID != y.SceneData.DynamicID) return x.SceneData.DynamicID - y.SceneData.DynamicID;
            if (x.SceneData.ParentChunkID != y.SceneData.ParentChunkID) return x.SceneData.ParentChunkID - y.SceneData.ParentChunkID;
            return 0;
        }

        public void SortScenes()
        {
            // this makes sure no scene is referenced before it is revealed to a player
            Scenes.Sort(SceneSorter);
        }*/

        #endregion // Collections

        public void Reveal(Player player)
        {
            bool found=player.RevealedWorlds.ContainsKey(this.DynamicID);

            if (!found)
            {
                // reveal world to player
                player.InGameClient.SendMessage(new RevealWorldMessage()
                {
                    WorldID = this.DynamicID,
                    WorldSNO = this.WorldSNO,
                });
                player.RevealedWorlds.Add(this.DynamicID, this);
            }

            // player enters world
            player.InGameClient.SendMessage(new EnterWorldMessage()
            {
                EnterPosition = player.Position,
                WorldID = this.DynamicID,
                WorldSNO = this.WorldSNO,
            });

            // just reveal the whole thing to the player for now
            foreach (var scene in Scenes.Values)
                scene.Reveal(player);

            if (!found)
            {
                // reveal actors
                foreach (var actor in Actors.Values)
                {
                    if (SNODatabase.Instance.IsOfGroup(actor.AppearanceSNO, SNOGroup.Blacklist)) continue;
                    if (SNODatabase.Instance.IsOfGroup(actor.AppearanceSNO, SNOGroup.NPCs)) continue;
                    // Commenting this out since it will lag the client to hell with all actors revealed
                    // TODO: We need proper location-aware reveal logic for _all_ objects, even scenes
                    //actor.Reveal(player);
                }

                // reveal portals
                Logger.Info("Revealing portals for world " + DynamicID);
                foreach (Portal portal in Portals.Values)
                {
                    portal.Reveal(player);
                }
            }
        }

        public void DestroyWorld(Player player)
        {
            // TODO: Destroy and release all objects in the world
            //foreach (var actor in player.RevealedActors.Values)
            //    if (actor.RevealMessage.Field4.Field2 == this.DynamicID) actor.Destroy(player);

            //foreach (var scene in player.RevealedScenes.Values)
            //    if (scene.SceneData.DynamicID == this.DynamicID) scene.Destroy(player);

            //player.Owner.LoggedInBNetClient.InGameClient.SendMessage(new WorldDeletedMessage() { Id = 0xd9, Field0 = DynamicID, });
            player.InGameClient.FlushOutgoingBuffer();
        }

        public void Tick()
        {
            //world time based code comes here later, call this X times a second (where x is around 20 imo)
        }
    }
}
