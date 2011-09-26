using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using D3Sharp.Net.Game;
using D3Sharp.Core.Map;
using D3Sharp.Core.Actors;
using D3Sharp.Utils;
using D3Sharp.Utils.Extensions;
using D3Sharp.Core.NPC;
using D3Sharp.Core.Actors;

namespace D3Sharp.Core.Map
{
    public class World
    {
        private GameClient Game;
        static readonly Logger Logger = LogManager.CreateLogger();

        private List<BasicNPC> NPCs;
        private List<Actor> Actors;
        private List<Scene> Scenes;

        public World(GameClient g)
        {
            Game = g;
            NPCs = new List<BasicNPC>();
            Actors = new List<Actors.Actor>();
            Scenes = new List<Scene>();
        }

        public  float posx, posy, posz;

        public List<Scene> MapChunkDB;
        public int WorldID = 0x772E0000;

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

            if (s!=null) return;

            s.MapLine = Line;
            s.Map = new MapRevealSceneMessage(data.Skip(2).ToArray(), WorldID);
        }

        public void AddActor(string Line)
        {
            string[] data = Line.Split(' ');

        }


        //private  int CompareMapChunks(Scene x, Scene y)
        //{
        //    if (x.SceneData.ParentChunkID != y.SceneData.ParentChunkID) return x.SceneData.ParentChunkID - y.SceneData.ParentChunkID;
        //    return x.SceneData.ChunkID - y.SceneData.ChunkID;
        //}
        //private  int OriginalSort(Scene x, Scene y)
        //{
        //    if (x.SceneData.WorldID != y.SceneData.WorldID) return x.SceneData.WorldID - y.SceneData.WorldID;
        //    if (x.SceneData.ParentChunkID != y.SceneData.ParentChunkID) return x.SceneData.ParentChunkID - y.SceneData.ParentChunkID;
        //    return x.originalsortorder - y.originalsortorder;
        //}

        //public  void LoadMapChunkDatabase()
        //{
        //    MapChunkDB = new List<Scene>();

        //    string filePath = "Assets\\!onlymap.txt";
        //    string line, line2;
        //    if (File.Exists(filePath))
        //    {
        //        int x = 0;
        //        StreamReader file = null;
        //        try
        //        {
        //            System.Text.RegularExpressions.Regex rx = new System.Text.RegularExpressions.Regex(@"\s+");

        //            file = new StreamReader(filePath);
        //            while ((line = file.ReadLine()) != null)
        //            {
        //                string line_ = rx.Replace(line, @" ");
        //                string[] data = line_.Split(' ');

        //                if ((line2 = file.ReadLine()) != null)
        //                {
        //                    string line2_ = rx.Replace(line2, @" ");
        //                    string[] data2 = line2_.Split(' ');

        //                    int worldid = int.Parse(data[2]);

        //                    Scene m = new Scene();

        //                    m.SceneData = new RevealSceneMessage(data.Skip(2).ToArray(), worldid);
        //                    m.Map = new MapRevealSceneMessage(data2.Skip(2).ToArray(), worldid);
        //                    m.SceneLine = line;
        //                    m.MapLine = line2;
        //                    m.originalsortorder = x++;

        //                    MapChunkDB.Add(m);
        //                }
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            Logger.DebugException(e, "ReadMapDatabase");
        //        }
        //        finally
        //        {
        //            if (file != null)
        //                file.Close();
        //        }
        //        MapChunkDB.Sort(CompareMapChunks);
        //        StreamWriter f = new StreamWriter("Assets\\mapdboutput.txt");

        //        for (x = 1; x < MapChunkDB.Count; x++)
        //        {
        //            Scene m0 = MapChunkDB[x - 1];
        //            Scene m1 = MapChunkDB[x];
        //            if (CompareMapChunks(m0, m1) == 0)
        //            {
        //                MapChunkDB.RemoveAt(x);
        //                x--;
        //            }
        //        }
        //        MapChunkDB.Sort(OriginalSort);


        //        Scene[] mdb = MapChunkDB.ToArray();

        //        foreach (Scene m in mdb)
        //        {
        //            f.WriteLine(m.SceneLine);
        //            f.WriteLine(m.MapLine);
        //        }
        //        f.Close();


        //    }
        //}

        void CreateNPC(int objectId)
        {
            NPCs.Add(new BasicNPC(objectId, ref Game));
        }

        public void Tick()
        {
            //world time based code comes here later, call this X times a second (where x is around 20 imo)
        }

        public void RevealWorld()
        {

        }

        public void ReadAndSendMap()
        {
            ActorDB.Init();
            //LoadMapChunkDatabase();

            string filePath = D3Sharp.Net.Game.Config.Instance.Map;
            string line, line2;

            //avarage = double.Parse("0.0", NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);

            bool versiondetermined = false;
            int version = 0;

            if (File.Exists(filePath))
            {
                StreamReader file = null;
                try
                {

                    System.Text.RegularExpressions.Regex rx = new System.Text.RegularExpressions.Regex(@"\s+");

                    file = new StreamReader(filePath);
                    while ((line = file.ReadLine()) != null)
                    {
                        line = rx.Replace(line, @" ");
                        string[] data = line.Split(' ');

                        if (!versiondetermined)
                            if (data.Length > 0)
                                if (data[0].Equals("v"))
                                {
                                    version = int.Parse(data[1]);
                                    Logger.Info("Map file version: " + version);
                                }
                                else
                                {
                                    //reveal world here if fallback - updated map files have world reveal data
                                    #region Interstitial,RevealWorld,WorldStatus,EnterWorld
                                    Game.SendMessage(new RevealWorldMessage()
                                    {
                                        Id = 0x0037,
                                        Field0 = WorldID,
                                        Field1 = 0x000115EE,
                                    });

                                    Game.SendMessage(new EnterWorldMessage()
                                    {
                                        Id = 0x0033,
                                        Field0 = new Vector3D() { Field0 = 3143.75f, Field1 = 2828.75f, Field2 = 59.07559f },
                                        Field1 = WorldID,
                                        Field2 = 0x000115EE,
                                    });
                                    #endregion
                                }
                        versiondetermined = true;

                        if (version == 0)
                        {
                            //fallback to the original version of the text files because people WILL mix them with the new :(

                            if ((line2 = file.ReadLine()) != null)
                            {
                                string[] data2 = line2.Split(' ');

                                RevealSceneMessage r = new RevealSceneMessage(data, WorldID);
                                MapRevealSceneMessage r2 = new MapRevealSceneMessage(data2, WorldID);

                                posx = (r.SceneSpec.tCachedValues.Field3.Field0.Field0 + r.SceneSpec.tCachedValues.Field3.Field1.Field0) / 2.0f + r.Position.Field1.Field0;
                                posy = (r.SceneSpec.tCachedValues.Field3.Field0.Field1 + r.SceneSpec.tCachedValues.Field3.Field1.Field1) / 2.0f + r.Position.Field1.Field1;
                                posz = (r.SceneSpec.tCachedValues.Field3.Field0.Field2 + r.SceneSpec.tCachedValues.Field3.Field1.Field2) / 2.0f + r.Position.Field1.Field2;

                                Game.SendMessage(r);
                                Game.SendMessage(r2);
                            }
                        }
                        else
                            if (data.Length >= 1) //check only lines with data in them
                            {
                                //packet data
                                if (data[0].Equals("p") && data.Length >= 2)
                                {
                                    int packettype = int.Parse(data[1]);
                                    switch (packettype)
                                    {
                                        case 0x34: //revealscenemessage
                                            //if (int.Parse(data[56]) == -1)
                                                Game.SendMessage(new RevealSceneMessage(data.Skip(2).ToArray(), WorldID));
                                            break;
                                        case 0x33: //enterworldmessage
                                            WorldID = int.Parse(data[5]);
                                            Game.SendMessage(new EnterWorldMessage()
                                            {
                                                Id = 0x0033,
                                                Field0 = new Vector3D()
                                                {
                                                    Field0 = float.Parse(data[2], System.Globalization.CultureInfo.InvariantCulture),
                                                    Field1 = float.Parse(data[3], System.Globalization.CultureInfo.InvariantCulture),
                                                    Field2 = float.Parse(data[4], System.Globalization.CultureInfo.InvariantCulture)
                                                },
                                                Field1 = WorldID,
                                                Field2 = int.Parse(data[6]),
                                            });
                                            break;
                                        case 0x37: //revealworldmessage
                                            Game.SendMessage(new RevealWorldMessage()
                                            {
                                                Id = 0x0037,
                                                Field0 = int.Parse(data[2]),
                                                Field1 = int.Parse(data[3]),
                                            });
                                            break;
                                        case 0x3b: //acdenterknownmessage
                                            {
                                                ACDEnterKnownMessage wm = new ACDEnterKnownMessage(data.Skip(2).ToArray(), WorldID);
                                                if (!ActorDB.isBlackListed(wm.Field1))
                                                {
                                                    Game.SendMessage(wm);
                                                    Logger.Info(String.Format("{0,7}", wm.Field1) + ": " + ActorDB.GetActorName(wm.Field1));
                                                    if (ActorDB.isNPC(wm.Field1))
                                                    {
                                                        //SEND NPC DATA HERE
                                                        CreateNPC(wm.Field0);
                                                    }
                                                    Game.FlushOutgoingBuffer();
                                                }
                                            }
                                            break;
                                        case 0x44: //maprevealscenemessage
                                            Game.SendMessage(new MapRevealSceneMessage(data.Skip(2).ToArray(), WorldID));
                                            break;
                                        default:
                                            Logger.Error("Unimplemented packet type encountered in map file: " + packettype);
                                            break;
                                    }
                                }

                                //spawn point
                                if (data[0].Equals("s") && data.Length >= 4)
                                {
                                    posx = float.Parse(data[1], System.Globalization.CultureInfo.InvariantCulture);
                                    posy = float.Parse(data[2], System.Globalization.CultureInfo.InvariantCulture);
                                    posz = float.Parse(data[3], System.Globalization.CultureInfo.InvariantCulture);
                                }

                            }

                    }
                }
                catch (Exception e)
                {
                    Logger.DebugException(e, "ReadAndSendMap");
                }
                finally
                {
                    if (file != null)
                        file.Close();
                }
            }
            else
            {
                Logger.Error("Map file {0} not found!", filePath);
            }
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
