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
using System.IO;
using System.Collections.Generic;
using Mooege.Common;
using Mooege.Common.Helpers;
using Mooege.Core.GS.Actors;
using Mooege.Core.GS.Map;
using Mooege.Net.GS;
using Mooege.Net.GS.Message;
using Mooege.Net.GS.Message.Definitions.ACD;
using Mooege.Net.GS.Message.Definitions.Animation;
using Mooege.Net.GS.Message.Definitions.Attribute;
using Mooege.Net.GS.Message.Definitions.Combat;
using Mooege.Net.GS.Message.Definitions.Effect;
using Mooege.Net.GS.Message.Definitions.Misc;
using Mooege.Net.GS.Message.Definitions.Player;
using Mooege.Net.GS.Message.Fields;
using Mooege.Core.GS.Powers;
using System.Threading;

namespace Mooege.Core.GS.Universe
{
    public class Universe : IMessageConsumer
    {
        static readonly Logger Logger = LogManager.CreateLogger();

        private readonly List<World> _worlds;

        public PlayerManager PlayerManager { get; private set; }

        public PowersManager PowersManager;

        public readonly int TicksPerSecond = 30;
        private Thread _tickThread;

        public Universe()
        {
            _tickThread = new Thread(() => _tickThread_Run());

            this._worlds = new List<World>();
            this.PlayerManager = new PlayerManager(this);

            this.PowersManager = new PowersManager(this);

            InitializeUniverse();

            _tickThread.Start();
        }

        public void Route(GameClient client, GameMessage message)
        {
            switch (message.Consumer)
            {
                case Consumers.Universe:
                    this.Consume(client, message);
                    break;
                case Consumers.PlayerManager:
                    this.PlayerManager.Consume(client, message);
                    break;
                case Consumers.Hero:
                    client.Player.Hero.Consume(client, message);
                    break;
            }
        }

        public void Consume(GameClient client, GameMessage message)
        {
            if (message is TargetMessage) OnToonTargetChange(client, (TargetMessage)message);
        }

        public void _tickThread_Run()
        {
            // TODO: needs to have exit condition, probably either PlayerManager.Players.Count or a manual shutdown flag
            while (true)
            {
                lock (this)
                {
                    PowersManager.Tick();
                }
                Thread.Sleep(1000 / TicksPerSecond);
            }
        }

        void InitializeUniverse()
        {
            LoadUniverseData("Assets/Maps/universe.txt");
        }

        private void LoadUniverseData(string Filename)
        {
            if (File.Exists(Filename))
            {
                StreamReader file = null;
                try
                {
                    var rx = new System.Text.RegularExpressions.Regex(@"\s+");

                    string line;
                    file = new StreamReader(Filename);
                    while ((line = file.ReadLine()) != null)
                    {
                        line = rx.Replace(line, @" ");
                        string[] data = line.Split(' ');

                        if (data.Length == 0) continue; //check only lines with data in them

                        //packet data
                        if (data[0].Equals("p") && data.Length >= 2)
                        {
                            int packettype = int.Parse(data[1]);
                            switch (packettype)
                            {
                                case 0x34: //new scene
                                    {
                                        int WorldID = int.Parse(data[2]);
                                        World w = GetWorld(WorldID);
                                        w.AddScene(line);
                                    }
                                    break;

                                case 0x37: //reveal world
                                    {
                                        int WorldID = int.Parse(data[2]);
                                        World w = GetWorld(WorldID);
                                        w.WorldSNO = int.Parse(data[3]);
                                    }
                                    break;

                                case 0x3b: //new actor     
                                    {
                                        int WorldID = int.Parse(data[16]);
                                        World w = GetWorld(WorldID);
                                        w.AddActor(line);
                                    }
                                    break;

                                case 0x44: //new map scene 
                                    {
                                        int WorldID = int.Parse(data[11]);
                                        World w = GetWorld(WorldID);
                                        w.AddMapScene(line);
                                    }
                                    break;

                                case 0x4b: //new portal
                                    {
                                        Actor a = GetActor(int.Parse(data[2]));
                                        if (a != null)
                                        {
                                            World w = GetWorld(a.WorldId);
                                            if (w != null) w.AddPortal(line);
                                        }
                                    }
                                    break;

                                default:
                                    //Logger.Info("Unimplemented packet type encountered in universe file: " + packettype);
                                    break;
                            }
                        }

                        //manual portal description
                        if (data[0].Equals("o") && data.Length >= 6)
                        {
                            Portal p = GetPortal(int.Parse(data[1]));
                            if (p != null)
                            {
                                p.TargetPos = new Vector3D();
                                p.TargetPos.X = float.Parse(data[2], System.Globalization.CultureInfo.InvariantCulture);
                                p.TargetPos.Y = float.Parse(data[3], System.Globalization.CultureInfo.InvariantCulture);
                                p.TargetPos.Z = float.Parse(data[4], System.Globalization.CultureInfo.InvariantCulture);
                                p.TargetWorldID = int.Parse(data[5]);
                            }
                        }


                        ////spawn point
                        //if (data[0].Equals("s") && data.Length >= 4)
                        //{
                        //    posx = float.Parse(data[1], System.Globalization.CultureInfo.InvariantCulture);
                        //    posy = float.Parse(data[2], System.Globalization.CultureInfo.InvariantCulture);
                        //    posz = float.Parse(data[3], System.Globalization.CultureInfo.InvariantCulture);
                        //}

                    }
                }
                catch (Exception e)
                {
                    Logger.DebugException(e, "LoadUniverseData");
                }
                finally
                {
                    if (file != null)
                        file.Close();
                }
            }
            else
            {
                Logger.Error("Universe file {0} not found!", Filename);
            }


            foreach (World w in _worlds)
                w.SortScenes();
        }

        public World GetWorld(int WorldID)
        {
            for (int x = 0; x < _worlds.Count; x++)
                if (_worlds[x].WorldID == WorldID) return _worlds[x];

            var world = new World(WorldID);
            _worlds.Add(world);
            return world;
        }

        Actor GetActor(int ActorID)
        {
            for (int x = 0; x < _worlds.Count; x++)
            {
                Actor a = _worlds[x].GetActor(ActorID);
                if (a != null) return a;
            }
            return null;
        }

        Portal GetPortal(int ActorID)
        {
            for (int x = 0; x < _worlds.Count; x++)
            {
                Portal p = _worlds[x].GetPortal(ActorID);
                if (p != null) return p;
            }
            return null;
        }

        public void ChangeToonWorld(GameClient client, int WorldID, Vector3D Pos)
        {
            Hero hero = client.Player.Hero;

            World newworld = null;
            //don't use getworld() here as that'd create a new empty world anyway
            foreach (var x in _worlds)
                if (x.WorldID == WorldID)
                    newworld = x;

            World currentworld = null;
            //don't use getworld() here as that'd create a new empty world anyway
            foreach (var x in _worlds)
                if (x.WorldID == hero.WorldId)
                    currentworld = x;

            if (newworld == null || currentworld == null) return; //don't go to a world we don't have in the universe

            currentworld.DestroyWorld(hero);

            hero.WorldId = newworld.WorldID;
            hero.CurrentWorldSNO = newworld.WorldSNO;
            hero.Position.X = Pos.X;
            hero.Position.Y = Pos.Y;
            hero.Position.Z = Pos.Z;

            newworld.Reveal(hero);

            client.SendMessage(new ACDWorldPositionMessage
            {
                Id = 0x3f,
                Field0 = 0x789E00E2,
                Field1 = new WorldLocationMessageData
                {
                    Field0 = 1.43f,
                    Field1 = new PRTransform
                    {
                        Field0 = new Quaternion
                        {
                            Amount = 0.05940768f,
                            Axis = new Vector3D
                            {
                                X = 0f,
                                Y = 0f,
                                Z = 0.9982339f,
                            }
                        },
                        ReferencePoint = hero.Position,
                    },
                    Field2 = newworld.WorldID,
                }
            });

            client.FlushOutgoingBuffer();

            client.SendMessage(new PlayerWarpedMessage()
            {
                Id = 0x0B1,
                Field0 = 9,
                Field1 = 0f,
            });

            client.PacketId += 40 * 2;
            client.SendMessage(new DWordDataMessage()
            {
                Id = 0x89,
                Field0 = client.PacketId,
            });

            client.FlushOutgoingBuffer();
        }

        private void OnToonTargetChange(GameClient client, TargetMessage message)
        {
            //Logger.Info("Player interaction with " + message.AsText());

            Portal p = GetPortal(message.Field1);

            if (p != null)
            {
                //we have a transition between worlds here
                ChangeToonWorld(client, p.TargetWorldID, p.TargetPos); //targetpos will always be valid as otherwise the portal wouldn't be targetable
                return;
            }
            else
            {
                PowersManager.UsePower(client.Player.Hero, message.snoPower, message.Field1, message.Field2.Field0);
            }
        }

    }
}
