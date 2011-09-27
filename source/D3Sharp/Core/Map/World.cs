using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using D3Sharp.Net.Game;
using D3Sharp.Core.Map;
using D3Sharp.Core.Actors;
using D3Sharp.Net.Game.Messages;
using D3Sharp.Net.Game.Messages.ACD;
using D3Sharp.Net.Game.Messages.Map;
using D3Sharp.Net.Game.Messages.Scene;
using D3Sharp.Net.Game.Messages.World;
using D3Sharp.Utils;
using D3Sharp.Utils.Extensions;
using D3Sharp.Core.NPC;
using D3Sharp.Core.Actors;
using D3Sharp.Core.Toons;

namespace D3Sharp.Core.Map
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
            Actors = new List<Actors.Actor>();
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
                    Actors[i].PosX==x &&
                    Actors[i].PosY==y &&
                    Actors[i].PosZ==z) return Actors[i];
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
            a.RevealMessage = new ACDEnterKnownMessage(data.Skip(2).ToArray(),WorldID);
            a.PosX = x;
            a.PosY = y;
            a.PosZ = z;

            Actors.Add(a);
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

        public void RevealWorld(Toon t)
        {
            //reveal world to player
            t.client.SendMessage(new RevealWorldMessage()
            {
                Id = 0x0037,
                Field0 = WorldID,
                Field1 = WorldSNO,
            });

            //player enters world
            t.client.SendMessage(new EnterWorldMessage()
            {
                Id = 0x0033,
                Field0 = new Vector3D()
                {
                    Field0 = t.PosX,
                    Field1 = t.PosY,
                    Field2 = t.PosZ
                },
                Field1 = WorldID,
                Field2 = WorldSNO,
            });

            //just reveal the whole thing to the player for now
            foreach (Scene s in Scenes)
                s.Reveal(t);

            //reveal actors
            foreach (Actor a in Actors)
            {
                if (ActorDB.isBlackListed(a.snoID)) continue;
                if (ActorDB.isNPC(a.snoID)) continue;
                a.Reveal(t);
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
