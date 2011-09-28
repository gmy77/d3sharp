/*
 * Copyright (C) 2011 D3Sharp Project
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

using System.Collections.Generic;
using System.Linq;
using D3Sharp.Core.Ingame.Actors;
using D3Sharp.Core.Ingame.NPC;
using D3Sharp.Core.Ingame.Universe;
using D3Sharp.Net.Game.Message.Definitions.ACD;
using D3Sharp.Net.Game.Message.Definitions.Map;
using D3Sharp.Net.Game.Message.Definitions.Scene;
using D3Sharp.Net.Game.Message.Definitions.World;
using D3Sharp.Net.Game.Message.Fields;
using D3Sharp.Utils;

namespace D3Sharp.Core.Ingame.Map
{
    public class World
    {
        static readonly Logger Logger = LogManager.CreateLogger();
        private List<BasicNPC> NPCs;
        private List<Actor> Actors;
        private List<Scene> Scenes;

        public int WorldID;
        public int WorldSNO;

        public World(int ID)
        {
            NPCs = new List<BasicNPC>();
            Actors = new List<Actor>();
            Scenes = new List<Scene>();
            WorldID = ID;
        }

        public Scene GetScene(int ID)
        {
            for (int x = 0; x < Scenes.Count; x++)
                if (Scenes[x].ID == ID) return Scenes[x];
            return null;
        }

        //this is a helper function to prune duplicate entries from the packet data
        public bool ActorExists(Actor actor)
        {
            return Actors.Any(
                t =>
                    t.SnoId == actor.SnoId && 
                    t.Position.X == actor.Position.X && 
                    t.Position.Y == actor.Position.Y &&
                    t.Position.Z == actor.Position.Z);
        }

        public void AddScene(string Line)
        {
            string[] data = Line.Split(' ');
            int SceneID=int.Parse(data[46]);

            Scene s = GetScene(SceneID);
            if (s != null) return;

            s = new Scene();
            s.ID = SceneID;

            s.SceneLine = Line;
            s.SceneData = new RevealSceneMessage(data.Skip(2).ToArray(),WorldID);

            Scenes.Add(s);
        }

        public void AddMapScene(string Line)
        {
            string[] data = Line.Split(' ');
            int SceneID = int.Parse(data[2]);

            Scene s = GetScene(SceneID);

            if (s==null) return;

            s.MapLine = Line;
            s.Map = new MapRevealSceneMessage(data.Skip(2).ToArray(), WorldID);
        }

        public void AddActor(string line)
        {
            var actor = new Actor();
            if (!actor.ParseFrom(this.WorldID, line)) return; // if not valid actor (inventory using items), just don't add it to list.            
            if(!this.ActorExists(actor)) Actors.Add(actor); // filter duplicate actors.
        }

        private int SceneSorter(Scene x, Scene y)
        {
            if (x.SceneData.WorldID != y.SceneData.WorldID) return x.SceneData.WorldID - y.SceneData.WorldID;
            if (x.SceneData.ParentChunkID != y.SceneData.ParentChunkID) return x.SceneData.ParentChunkID - y.SceneData.ParentChunkID;
            return 0;
        }

        public void SortScenes()
        {   
            //this makes sure no scene is referenced before it is revealed to a player
            Scenes.Sort(SceneSorter);
        }

        public void Reveal(Hero hero)
        {
            //reveal world to player
            hero.InGameClient.SendMessage(new RevealWorldMessage()
            {
                Id = 0x0037,
                Field0 = WorldID,
                Field1 = WorldSNO,
            });

            //player enters world
            hero.InGameClient.SendMessage(new EnterWorldMessage()
            {
                Id = 0x0033,
                Field0 = hero.Position,
                Field1 = WorldID,
                Field2 = WorldSNO,
            });

            //just reveal the whole thing to the player for now
            foreach (var scene in Scenes)
                scene.Reveal(hero);

            //reveal actors
            foreach (var actor in Actors)
            {
                if (ActorDB.isBlackListed(actor.SnoId)) continue;
                if (ActorDB.isNPC(actor.SnoId)) continue;
                actor.Reveal(hero);
            }
        }

        void CreateNPC(int objectId)
        {
            //NPCs.Add(new BasicNPC(objectId, ref Client));
        }

        public void Tick()
        {
            //world time based code comes here later, call this X times a second (where x is around 20 imo)
        }

        public void HandleTarget(int ID)
        {
            List<BasicNPC> removelist = new List<BasicNPC>();
            foreach (BasicNPC b in NPCs)
            {
                if (b.ID == ID)
                {
                    b.Die(0);
                    removelist.Add(b);
                }
            }
            foreach (BasicNPC b in removelist)
            {
                NPCs.Remove(b);
            }
        }
    }
}
