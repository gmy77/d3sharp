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

using System;
using System.IO;
using System.Collections.Generic;
using D3Sharp.Core.Helpers;
using D3Sharp.Core.Ingame.Map;
using D3Sharp.Net.Game.Message;
using D3Sharp.Net.Game.Message.Definitions.Animation;
using D3Sharp.Net.Game.Message.Definitions.Combat;
using D3Sharp.Net.Game.Message.Definitions.Effect;
using D3Sharp.Net.Game.Message.Definitions.Map;
using D3Sharp.Net.Game.Message.Definitions.Scene;
using D3Sharp.Utils;
using D3Sharp.Net.Game;
using D3Sharp.Net.Game.Message.Fields;
using D3Sharp.Net.Game.Message.Definitions.ACD;
using D3Sharp.Net.Game.Message.Definitions.Misc;
using D3Sharp.Net.Game.Message.Definitions.Player;
using D3Sharp.Net.Game.Message.Definitions.World;
using D3Sharp.Net.Game.Message.Definitions.Attribute;

namespace D3Sharp.Core.Ingame.Universe
{
    public class Universe: IMessageConsumer
    {
        static readonly Logger Logger = LogManager.CreateLogger();

        private readonly List<World> _worlds;

        public PlayerManager PlayerManager { get; private set; }

        public InventoryMenager InventoryMenager { get; private set; }

        public Universe()
        {
            this._worlds = new List<World>();
            this.PlayerManager = new PlayerManager(this);
            this.InventoryMenager = new InventoryMenager();
            InitializeUniverse();
        }

        public void Route(GameClient client, GameMessage message)
        {
            switch(message.Consumer)
            {
                case Consumers.Universe:
                    this.Consume(client, message);
                    break;
                case Consumers.Inventory:
                    this.InventoryMenager.Consume(client, message);
                    break;
                case Consumers.PlayerManager:
                    this.PlayerManager.Consume(client, message);
                    break;
                case Consumers.Skillset:
                    client.Player.Hero.Skillset.Consume(client, message);
                    break;
            }
        }

        public void Consume(GameClient client, GameMessage message)
        {
            if (message is TargetMessage) OnToonTargetChange(client, (TargetMessage)message);
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
                                        World w = GetWorld(int.Parse(data[2]));
                                        w.AddScene(line);
                                        w.WorldSNO = int.Parse(data[21]); //snoPresetWorld
                                    }
                                    break;

                                case 0x3b: //new actor                            
                                    GetWorld(int.Parse(data[16])).AddActor(line);
                                    break;

                                case 0x44: //new map scene                            
                                    GetWorld(int.Parse(data[11])).AddMapScene(line);
                                    break;

                                default:
                                    Logger.Error("Unimplemented packet type encountered in universe file: " + packettype);
                                    break;
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

        private void OnToonTargetChange(GameClient client, TargetMessage message)
        {
            if (message.Field1 == 0x77F20036)
            {
                this.EnterInn(client);
                return;
            }
            else if (client.ObjectIdsSpawned == null || !client.ObjectIdsSpawned.Contains(message.Field1)) return;

            client.ObjectIdsSpawned.Remove(message.Field1);

            var killAni = new int[]{
                    0x2cd7,
                    0x2cd4,
                    0x01b378,
                    0x2cdc,
                    0x02f2,
                    0x2ccf,
                    0x2cd0,
                    0x2cd1,
                    0x2cd2,
                    0x2cd3,
                    0x2cd5,
                    0x01b144,
                    0x2cd6,
                    0x2cd8,
                    0x2cda,
                    0x2cd9
            };
            client.SendMessage(new PlayEffectMessage()
            {
                Id = 0x7a,
                Field0 = message.Field1,
                Field1 = 0x0,
                Field2 = 0x2,
            });
            client.SendMessage(new PlayEffectMessage()
            {
                Id = 0x7a,
                Field0 = message.Field1,
                Field1 = 0xc,
            });
            client.SendMessage(new PlayHitEffectMessage()
            {
                Id = 0x7b,
                Field0 = message.Field1,
                Field1 = 0x789E00E2,
                Field2 = 0x2,
                Field3 = false,
            });

            client.SendMessage(new FloatingNumberMessage()
            {
                Id = 0xd0,
                Field0 = message.Field1,
                Field1 = 9001.0f,
                Field2 = 0,
            });

            client.SendMessage(new ANNDataMessage()
            {
                Id = 0x6d,
                Field0 = message.Field1,
            });

            int ani = killAni[RandomHelper.Next(killAni.Length)];
            Logger.Info("Ani used: " + ani);

            client.SendMessage(new PlayAnimationMessage()
            {
                Id = 0x6c,
                Field0 = message.Field1,
                Field1 = 0xb,
                Field2 = 0,
                tAnim = new PlayAnimationMessageSpec[1]
                {
                    new PlayAnimationMessageSpec()
                    {
                        Field0 = 0x2,
                        Field1 = ani,
                        Field2 = 0x0,
                        Field3 = 1f
                    }
                }
            });

            client.PacketId += 10 * 2;
            client.SendMessage(new DWordDataMessage()
            {
                Id = 0x89,
                Field0 = client.PacketId,
            });

            client.SendMessage(new ANNDataMessage()
            {
                Id = 0xc5,
                Field0 = message.Field1,
            });

            client.SendMessage(new AttributeSetValueMessage
            {
                Id = 0x4c,
                Field0 = message.Field1,
                Field1 = new NetAttributeKeyValue
                {
                    Attribute = GameAttribute.Attributes[0x4d],
                    Float = 0
                }
            });

            client.SendMessage(new AttributeSetValueMessage
            {
                Id = 0x4c,
                Field0 = message.Field1,
                Field1 = new NetAttributeKeyValue
                {
                    Attribute = GameAttribute.Attributes[0x1c2],
                    Int = 1
                }
            });

            client.SendMessage(new AttributeSetValueMessage
            {
                Id = 0x4c,
                Field0 = message.Field1,
                Field1 = new NetAttributeKeyValue
                {
                    Attribute = GameAttribute.Attributes[0x1c5],
                    Int = 1
                }
            });
            client.SendMessage(new PlayEffectMessage()
            {
                Id = 0x7a,
                Field0 = message.Field1,
                Field1 = 0xc,
            });
            client.SendMessage(new PlayEffectMessage()
            {
                Id = 0x7a,
                Field0 = message.Field1,
                Field1 = 0x37,
            });
            client.SendMessage(new PlayHitEffectMessage()
            {
                Id = 0x7b,
                Field0 = message.Field1,
                Field1 = 0x789E00E2,
                Field2 = 0x2,
                Field3 = false,
            });
            client.PacketId += 10 * 2;
            client.SendMessage(new DWordDataMessage()
            {
                Id = 0x89,
                Field0 = client.PacketId,
            });
        }

        private void EnterInn(GameClient client)
        {
            client.SendMessage(new RevealWorldMessage()
            {
                Id = 0x037,
                Field0 = 0x772F0001,
                Field1 = 0x0001AB32,
            });

            client.SendMessage(new WorldStatusMessage()
            {
                Id = 0x0B4,
                Field0 = 0x772F0001,
                Field1 = false,
            });

            client.SendMessage(new EnterWorldMessage()
            {
                Id = 0x0033,
                Field0 = new Vector3D()
                {
                    X = 83.75f,
                    Y = 123.75f,
                    Z = 0.2000023f,
                },
                Field1 = 0x772F0001,
                Field2 = 0x0001AB32,
            });

            client.FlushOutgoingBuffer();

            client.SendMessage(new RevealSceneMessage()
            {
                Id = 0x0034,
                WorldID = 0x772F0001,
                SceneSpec = new SceneSpecification()
                {
                    Field0 = 0x00000000,
                    Field1 = new IVector2D()
                    {
                        Field0 = 0x00000000,
                        Field1 = 0x00000000,
                    },
                    arSnoLevelAreas = new int[] { 0x0001AB91, unchecked((int)0xFFFFFFFF), unchecked((int)0xFFFFFFFF), unchecked((int)0xFFFFFFFF) },
                    snoPrevWorld = 0x000115EE,
                    Field4 = 0x000000DF,
                    snoPrevLevelArea = unchecked((int)0xFFFFFFFF),
                    snoNextWorld = 0x00015348,
                    Field7 = 0x000000AC,
                    snoNextLevelArea = unchecked((int)0xFFFFFFFF),
                    snoMusic = 0x000206F8,
                    snoCombatMusic = unchecked((int)0xFFFFFFFF),
                    snoAmbient = 0x0002C68A,
                    snoReverb = 0x00021ABA,
                    snoWeather = 0x00017869,
                    snoPresetWorld = 0x0001AB32,
                    Field15 = 0x00000000,
                    Field16 = 0x00000000,
                    Field17 = 0x00000000,
                    Field18 = unchecked((int)0xFFFFFFFF),
                    tCachedValues = new SceneCachedValues()
                    {
                        Field0 = 0x0000003F,
                        Field1 = 0x00000060,
                        Field2 = 0x00000060,
                        Field3 = new AABB()
                        {
                            Field0 = new Vector3D()
                            {
                                X = 120f,
                                Y = 120f,
                                Z = 26.61507f,
                            },
                            Field1 = new Vector3D()
                            {
                                X = 120f,
                                Y = 120f,
                                Z = 36.06968f,
                            }
                        },
                        Field4 = new AABB()
                        {
                            Field0 = new Vector3D()
                            {
                                X = 120f,
                                Y = 120f,
                                Z = 26.61507f,
                            },
                            Field1 = new Vector3D()
                            {
                                X = 120f,
                                Y = 120f,
                                Z = 36.06968f,
                            }
                        },
                        Field5 = new int[] { 0x00000267, 0x00000000, 0x00000000, 0x00000000, },
                        Field6 = 0x00000001
                    }
                },
                ChunkID = 0x78740120,
                snoScene = 0x0001AB2F,
                Position = new PRTransform()
                {
                    Field0 = new Quaternion()
                    {
                        Amount = 1f,
                        Axis = new Vector3D()
                        {
                            X = 0f,
                            Y = 0f,
                            Z = 0f,
                        }
                    },
                    ReferencePoint = new Vector3D()
                    {
                        X = 0f,
                        Y = 0f,
                        Z = 0f,
                    }
                },
                ParentChunkID = unchecked((int)0xFFFFFFFF),
                snoSceneGroup = unchecked((int)0xFFFFFFFF),
                arAppliedLabels = new int[0]

            });
            client.FlushOutgoingBuffer();

            client.SendMessage(new MapRevealSceneMessage()
            {
                Id = 0x044,
                ChunkID = 0x78740120,
                snoScene = 0x0001AB2F,
                Field2 = new PRTransform()
                {
                    Field0 = new Quaternion()
                    {
                        Amount = 1f,
                        Axis = new Vector3D()
                        {
                            X = 0f,
                            Y = 0f,
                            Z = 0f,
                        }
                    },
                    ReferencePoint = new Vector3D()
                    {
                        X = 0f,
                        Y = 0f,
                        Z = 0f,
                    }
                },
                Field3 = 0x772F0001,
                MiniMapVisibility = 0
            });
            client.FlushOutgoingBuffer();

            client.SendMessage(new ACDEnterKnownMessage
            {
                Id = 0x3B,
                Field0 = 0x7A0800A2,
                Field1 = 0x0000157F,
                Field2 = 8,
                Field3 = 0,
                Field4 = new WorldLocationMessageData
                {
                    Field0 = 1f,
                    Field1 = new PRTransform
                    {
                        Field0 = new Quaternion
                        {
                            Amount = 0.9909708f,
                            Axis = new Vector3D
                            {
                                X = 0f,
                                Y = 0f,
                                Z = 0.1340775f
                            }
                        },
                        ReferencePoint = new Vector3D
                        {
                            X = 82.15131f,
                            Y = 122.2867f,
                            Z = 0.1000366f
                        }
                    },
                    Field2 = 0x772F0001
                },
                Field6 = new GBHandle
                {
                    Field0 = -1,
                    Field1 = unchecked((int)0xFFFFFFFF)
                },
                Field7 = 0x00000001,
                Field8 = 0x0000157F,
                Field9 = 0,
                Field10 = 0x0,
                Field12 = 0x0001AB8C,
                Field13 = 0x00000000
            });

            client.FlushOutgoingBuffer();
            client.SendMessage(new AffixMessage()
            {
                Id = 0x48,
                Field0 = 0x7A0800A2,
                Field1 = 1,
                aAffixGBIDs = new int[0]
            });
            client.FlushOutgoingBuffer();

            client.SendMessage(new AffixMessage()
            {
                Id = 0x48,
                Field0 = 0x7A0800A2,
                Field1 = 2,
                aAffixGBIDs = new int[0]
            });
            client.FlushOutgoingBuffer();

            client.SendMessage(new PlayerWarpedMessage()
            {
                Id = 0x0B1,
                Field0 = 9,
                Field1 = 0xf,
            });
            client.FlushOutgoingBuffer();

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
                        ReferencePoint = new Vector3D
                        {
                            X = 82.15131f,
                            Y = 122.2867f,
                            Z = 0.1000366f
                        }
                    },
                    Field2 = 0x772F0001
                }
            });

            client.PacketId += 10 * 2;
            client.SendMessage(new DWordDataMessage()
            {
                Id = 0x89,
                Field0 = client.PacketId,
            });
            client.FlushOutgoingBuffer();
        }

        public void SpawnMob(GameClient client, int mobId) // this shoudn't even rely on client or it's position though i know this is just a hack atm ;) /raist.
        {
            int nId = mobId;
            if (client.Player.Hero.Position == null)
                return;

            if (client.ObjectIdsSpawned == null)
            {
                client.ObjectIdsSpawned = new List<int>();
                client.ObjectIdsSpawned.Add(client.ObjectId - 100);
                client.ObjectIdsSpawned.Add(client.ObjectId);
            }

            client.ObjectId++;
            client.ObjectIdsSpawned.Add(client.ObjectId);

            #region ACDEnterKnown Hittable Zombie
            client.SendMessage(new ACDEnterKnownMessage()
            {
                Id = 0x003B,
                Field0 = client.ObjectId,
                Field1 = nId,
                Field2 = 0x8,
                Field3 = 0x0,
                Field4 = new WorldLocationMessageData()
                {
                    Field0 = 1.35f,
                    Field1 = new PRTransform()
                    {
                        Field0 = new Quaternion()
                        {
                            Amount = 0.768145f,
                            Axis = new Vector3D()
                            {
                                X = 0f,
                                Y = 0f,
                                Z = -0.640276f,
                            },
                        },
                        ReferencePoint = new Vector3D()
                        {
                            X = client.Player.Hero.Position.X + 5,
                            Y = client.Player.Hero.Position.Y + 5,
                            Z = client.Player.Hero.Position.Z,
                        },
                    },
                    Field2 = 0x772E0000,
                },
                Field5 = null,
                Field6 = new GBHandle()
                {
                    Field0 = 1,
                    Field1 = 1,
                },
                Field7 = 0x00000001,
                Field8 = nId,
                Field9 = 0x0,
                Field10 = 0x0,
                Field11 = 0x0,
                Field12 = 0x0,
                Field13 = 0x0
            });
            client.SendMessage(new AffixMessage()
            {
                Id = 0x48,
                Field0 = client.ObjectId,
                Field1 = 0x1,
                aAffixGBIDs = new int[0]
            });
            client.SendMessage(new AffixMessage()
            {
                Id = 0x48,
                Field0 = client.ObjectId,
                Field1 = 0x2,
                aAffixGBIDs = new int[0]
            });
            client.SendMessage(new ACDCollFlagsMessage
            {
                Id = 0xa6,
                Field0 = client.ObjectId,
                Field1 = 0x1
            });

            client.SendMessage(new AttributesSetValuesMessage
            {
                Id = 0x4d,
                Field0 = client.ObjectId,
                atKeyVals = new NetAttributeKeyValue[15] {
                    new NetAttributeKeyValue {
                        Attribute = GameAttribute.Attributes[214],
                        Int = 0
                    },
                    new NetAttributeKeyValue {
                        Attribute = GameAttribute.Attributes[464],
                        Int = 1
                    },
                    new NetAttributeKeyValue {
                        Field0 = 1048575,
                        Attribute = GameAttribute.Attributes[441],
                        Int = 1
                    },
                    new NetAttributeKeyValue {
                        Field0 = 30582,
                        Attribute = GameAttribute.Attributes[560],
                        Int = 1
                    },
                    new NetAttributeKeyValue {
                        Field0 = 30286,
                        Attribute = GameAttribute.Attributes[560],
                        Int = 1
                    },
                    new NetAttributeKeyValue {
                        Field0 = 30285,
                        Attribute = GameAttribute.Attributes[560],
                        Int = 1
                    },
                    new NetAttributeKeyValue {
                        Field0 = 30284,
                        Attribute = GameAttribute.Attributes[560],
                        Int = 1
                    },
                    new NetAttributeKeyValue {
                        Field0 = 30283,
                        Attribute = GameAttribute.Attributes[560],
                        Int = 1
                    },
                    new NetAttributeKeyValue {
                        Field0 = 30290,
                        Attribute = GameAttribute.Attributes[560],
                        Int = 1
                    },
                    new NetAttributeKeyValue {
                        Field0 = 79486,
                        Attribute = GameAttribute.Attributes[560],
                        Int = 1
                    },
                    new NetAttributeKeyValue {
                        Field0 = 30286,
                        Attribute = GameAttribute.Attributes[460],
                        Int = 1
                    },
                    new NetAttributeKeyValue {
                        Field0 = 30285,
                        Attribute = GameAttribute.Attributes[460],
                        Int = 1
                    },
                    new NetAttributeKeyValue {
                        Field0 = 30284,
                        Attribute = GameAttribute.Attributes[460],
                        Int = 1
                    },
                    new NetAttributeKeyValue {
                        Field0 = 30283,
                        Attribute = GameAttribute.Attributes[460],
                        Int = 1
                    },
                    new NetAttributeKeyValue {
                        Field0 = 30290,
                        Attribute = GameAttribute.Attributes[460],
                        Int = 1
                    }
                }

            });

            client.SendMessage(new AttributesSetValuesMessage
            {
                Id = 0x4d,
                Field0 = client.ObjectId,
                atKeyVals = new NetAttributeKeyValue[9] {
                    new NetAttributeKeyValue {
                        Attribute = GameAttribute.Attributes[86],
                        Float = 4.546875f
                    },
                    new NetAttributeKeyValue {
                        Field0 = 79486,
                        Attribute = GameAttribute.Attributes[460],
                        Int = 1
                    },
                    new NetAttributeKeyValue {
                        Attribute = GameAttribute.Attributes[84],
                        Float = 4.546875f
                    },
                    new NetAttributeKeyValue {
                        Attribute = GameAttribute.Attributes[81],
                        Int = 0
                    },
                    new NetAttributeKeyValue {
                        Attribute = GameAttribute.Attributes[77],
                        Float = 4.546875f
                    },
                    new NetAttributeKeyValue {
                        Attribute = GameAttribute.Attributes[69],
                        Int = 1
                    },
                    new NetAttributeKeyValue {
                        Field0 = 30582,
                        Attribute = GameAttribute.Attributes[460],
                        Int = 1
                    },
                    new NetAttributeKeyValue {
                        Attribute = GameAttribute.Attributes[67],
                        Int = 10
                    },
                    new NetAttributeKeyValue {
                        Attribute = GameAttribute.Attributes[38],
                        Int = 1
                    }
                }

            });


            client.SendMessage(new ACDGroupMessage
            {
                Id = 0xb8,
                Field0 = client.ObjectId,
                Field1 = unchecked((int)0xb59b8de4),
                Field2 = unchecked((int)0xffffffff)
            });

            client.SendMessage(new ANNDataMessage
            {
                Id = 0x3e,
                Field0 = client.ObjectId
            });

            client.SendMessage(new ACDTranslateFacingMessage
            {
                Id = 0x70,
                Field0 = client.ObjectId,
                Field1 = (float)(RandomHelper.NextDouble() * 2.0 * Math.PI),
                Field2 = false
            });

            client.SendMessage(new SetIdleAnimationMessage
            {
                Id = 0xa5,
                Field0 = client.ObjectId,
                Field1 = 0x11150
            });

            client.SendMessage(new SNONameDataMessage
            {
                Id = 0xd3,
                Field0 = new SNOName
                {
                    Field0 = 0x1,
                    Field1 = nId
                }
            });
            #endregion

            client.PacketId += 30 * 2;
            client.SendMessage(new DWordDataMessage()
            {
                Id = 0x89,
                Field0 = client.PacketId,
            });
            client.Tick += 20;
            client.SendMessage(new EndOfTickMessage()
            {
                Id = 0x008D,
                Field0 = client.Tick - 20,
                Field1 = client.Tick
            });
        }
    }
}
