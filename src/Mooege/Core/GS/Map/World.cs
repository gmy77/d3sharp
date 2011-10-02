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

using System.Collections.Generic;
using System.Linq;
using Mooege.Common;
using Mooege.Core.GS.Actors;
using Mooege.Core.GS.Data.SNO;
using Mooege.Core.GS.NPC;
using Mooege.Core.GS.Universe;
using Mooege.Net.GS.Message.Definitions.Map;
using Mooege.Net.GS.Message.Definitions.Misc;
using Mooege.Net.GS.Message.Definitions.Scene;
using Mooege.Net.GS.Message.Definitions.World;

namespace Mooege.Core.GS.Map
{
    public class World
    {
        static readonly Logger Logger = LogManager.CreateLogger();
        private List<BasicNPC> NPCs;
        private List<Actor> Actors;
        private List<Scene> Scenes;
        private List<Portal> Portals;

        public int WorldID;
        public int WorldSNO;

        public World(int ID)
        {
            NPCs = new List<BasicNPC>();
            Actors = new List<Actor>();
            Scenes = new List<Scene>();
            Portals = new List<Portal>();
            WorldID = ID;
        }

        public Scene GetScene(int ID)
        {
            for (int x = 0; x < Scenes.Count; x++)
                if (Scenes[x].ID == ID) return Scenes[x];
            return null;
        }
        

        public BasicNPC GetNpc(int ID)
        {
            for (int x = 0; x < NPCs.Count; x++)
                if (NPCs[x].ID == ID) return NPCs[x];
            return null;
        }

        public Actor GetActor(int ID)
        {
            for (int x = 0; x < Actors.Count; x++)
                if (Actors[x].Id == ID) return Actors[x];
            return null;
        }

        public Portal GetPortal(int ID)
        {
            for (int x = 0; x < Portals.Count; x++)
                if (Portals[x].ActorRef.Id == ID) return Portals[x];
            return null;
        }

        //get actor by snoid and x,y,z position - this is a helper function to prune duplicate entries from the packet data
        public Actor GetActor(int snoID, float x, float y, float z)
        {
            for (int i = 0; i < Actors.Count; i++)
                if (Actors[i].SnoId == snoID &&
                    Actors[i].Position.X == x &&
                    Actors[i].Position.Y == y &&
                    Actors[i].Position.Z == z) return Actors[i];
            return null;
        }

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

            s.SceneData = new RevealSceneMessage(data.Skip(2).ToArray(),WorldID);

            Scenes.Add(s);
        }

        public void AddMapScene(string Line)
        {
            string[] data = Line.Split(' ');
            int SceneID = int.Parse(data[2]);

            Scene s = GetScene(SceneID);

            if (s==null) return;

            s.Map = new MapRevealSceneMessage(data.Skip(2).ToArray(), WorldID);
        }


        public void AddActor(Actor actor)
        {
            Actors.Add(actor);                 
        }

        public void AddNpc(BasicNPC npc)
        {
            NPCs.Add(npc);
        }


        public void AddActor(string line)
        {
            var actor = new Actor();
            if (!actor.ParseFrom(this.WorldID, line)) return; // if not valid actor (inventory using items), just don't add it to list.            
            if(!this.ActorExists(actor)) Actors.Add(actor); // filter duplicate actors.
        }

        public void AddPortal(string Line)
        {
            string[] data = Line.Split(' ');

            int ActorID = int.Parse(data[2]);

            //this check is here so no duplicate portals are in a world and thus a portal actor isn't revealed twice to a player
            foreach (var portal in Portals)
                if (portal.ActorRef.Id == ActorID) return;

            Portal p = new Portal();
            p.PortalMessage = new PortalSpecifierMessage(data.Skip(2).ToArray());
            p.ActorRef=GetActor(ActorID);

            Portals.Add(p);
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
            bool found=hero.RevealedWorlds.Contains(this);

            if (!found)
            {
                //reveal world to player
                hero.InGameClient.SendMessage(new RevealWorldMessage()
                {
                    Id = 0x0037,
                    Field0 = WorldID,
                    Field1 = WorldSNO,
                });
                hero.RevealedWorlds.Add(this);
            }

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

            if (!found)
            {
                //reveal actors
                foreach (var actor in Actors)
                {
                    if (SNODatabase.Instance.IsOfGroup(actor.SnoId, SNOGroup.Blacklist)) continue;
                    if (SNODatabase.Instance.IsOfGroup(actor.SnoId, SNOGroup.NPCs)) continue;
                    
                    if(actor.Id == 2065563791)
                        actor.Reveal(hero);
                }

                //reveal portals
                Logger.Info("Revealing portals for world " + WorldID);
                foreach (Portal portal in Portals)
                {
                    portal.Reveal(hero);
                }
            }
        }

        public void DestroyWorld(Hero hero)
        {
            //for (int x = hero.RevealedActors.Count - 1; x >= 0; x-- )
            //    if (hero.RevealedActors[x].RevealMessage.Field4.Field2 == WorldID) hero.RevealedActors[x].Destroy(hero);

            //for (int x = hero.RevealedScenes.Count - 1; x >= 0; x--)
            //    if (hero.RevealedScenes[x].SceneData.WorldID == WorldID) hero.RevealedScenes[x].Destroy(hero);

            //hero.Owner.LoggedInBNetClient.InGameClient.SendMessage(new WorldDeletedMessage() { Id = 0xd9, Field0 = WorldID, });
            hero.InGameClient.FlushOutgoingBuffer();
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
