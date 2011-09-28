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

        public Actor GetActor(int ID)
        {
            for (int x = 0; x < Actors.Count; x++)
                if (Actors[x].ID == ID) return Actors[x];
            return null;
        }

        //get actor by snoid and x,y,z position - this is a helper function to prune duplicate entries from the packet data
        public Actor GetActor(int snoID, float x, float y, float z)
        {
            for (int i = 0; i < Actors.Count; i++)
                if (Actors[i].snoID == snoID &&
                    Actors[i].Position.X ==x &&
                    Actors[i].Position.Y == y &&
                    Actors[i].Position.Z == z) return Actors[i];
            return null;
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

        public void AddActor(string Line)
        {
            string[] data = Line.Split(' ');

            //skip inventory using items as their use is unknown
            if (int.Parse(data[2]) == 0) return;

            int ActorID = int.Parse(data[4]);
            int snoID = int.Parse(data[5]);

            float x, y, z;
            x = float.Parse(data[13], System.Globalization.CultureInfo.InvariantCulture);
            y = float.Parse(data[14], System.Globalization.CultureInfo.InvariantCulture);
            z = float.Parse(data[15], System.Globalization.CultureInfo.InvariantCulture);

            Actor a = GetActor(snoID,x,y,z);
            if (a != null) return;

            a = new Actor();
            a.ID = ActorID;
            a.snoID = snoID;
            a.WorldId = this.WorldID;

            a.RevealMessage = new ACDEnterKnownMessage(data.Skip(2).ToArray(),WorldID);
            this.ReadActor(a, data.ToArray().Skip(2).ToArray());

            a.Position.X = x;
            a.Position.Y = y;
            a.Position.Z = z;

            Actors.Add(a);
        }

        private void ReadActor(Actor actor, string[] Data)
        {
            actor.Field0 = int.Parse(Data[2]);
            actor.snoID = int.Parse(Data[3]);
            actor.Field2 = int.Parse(Data[4]);
            actor.Field3 = int.Parse(Data[5]);

            actor.Scale = float.Parse(Data[6], System.Globalization.CultureInfo.InvariantCulture);
            actor.RotationAmount = float.Parse(Data[10], System.Globalization.CultureInfo.InvariantCulture);
            actor.RotationAxis = new Vector3D()
                                     {
                                         X = float.Parse(Data[7], System.Globalization.CultureInfo.InvariantCulture),
                                         Y = float.Parse(Data[8], System.Globalization.CultureInfo.InvariantCulture),
                                         Z = float.Parse(Data[9], System.Globalization.CultureInfo.InvariantCulture),
                                     };

            actor.Position = new Vector3D()
                                 {
                                     X = float.Parse(Data[11], System.Globalization.CultureInfo.InvariantCulture),
                                     Y = float.Parse(Data[12], System.Globalization.CultureInfo.InvariantCulture),
                                     Z = float.Parse(Data[13], System.Globalization.CultureInfo.InvariantCulture),
                                 };

            actor.InventoryLocationData = new InventoryLocationMessageData()
                                              {
                                                  Field0 = int.Parse(Data[15]),
                                                  Field1 = int.Parse(Data[16]),
                                                  Field2 = new IVector2D()
                                                               {
                                                                   Field0 = int.Parse(Data[17]),
                                                                   Field1 = int.Parse(Data[18]),
                                                               }
                                              };

            actor.GBHandle = new GBHandle()
                                 {
                                     Field0 = int.Parse(Data[19]),
                                     Field1 = int.Parse(Data[20]),
                                 };

            actor.Field7 = int.Parse(Data[21]);
            actor.Field8 = int.Parse(Data[22]);
            actor.Field9 = int.Parse(Data[23]);
            actor.Field10 = byte.Parse(Data[24]);           
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
                if (ActorDB.isBlackListed(actor.snoID)) continue;
                if (ActorDB.isNPC(actor.snoID)) continue;
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
