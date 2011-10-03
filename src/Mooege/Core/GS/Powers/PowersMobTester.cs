using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mooege.Net.GS.Message.Fields;
using Mooege.Core.GS.Actors;
using Mooege.Net.GS;
using Mooege.Common.Helpers;
using Mooege.Net.GS.Message.Definitions.ACD;
using Mooege.Net.GS.Message.Definitions.Misc;
using Mooege.Net.GS.Message.Definitions.Attribute;
using Mooege.Net.GS.Message;
using Mooege.Net.GS.Message.Definitions.Animation;
using Mooege.Net.GS.Message.Definitions.Effect;
using Mooege.Core.GS.Universe;

namespace Mooege.Core.GS.Powers
{
    public class SimpleMob : IPowerTarget
    {
        public IList<ClientObjectId> ids;
        public Vector3D position;
        public PowersMobTester owner;
        public float hp;
        public DateTime deleteTimeout;
        public bool dead;

        public void ReceiveDamage(Actor from, float amount, int type)
        {
            hp -= amount;
            if (!dead && hp <= 0.0f)
                owner.KillMob(this);
        }

        public Vector3D GetPosition()
        {
            return position;
        }

        public IList<ClientObjectId> GetIds()
        {
            return ids;
        }
    }

    // simple hackish mob spawner, all mobs created by all instances of it are available via the PowerTargets
    // property, horribly un-threadsafe :)
    public class PowersMobTester
    {
        public static IEnumerable<IPowerTarget> PowerTargets
        {
            get
            {
                foreach (var moblist in _mobtesters)
                    foreach (var mob in moblist._mobs)
                        yield return mob;
            }
        }

        public static List<PowersMobTester> _mobtesters = new List<PowersMobTester>();

        private List<SimpleMob> _mobs = new List<SimpleMob>();
        private Universe.Universe _universe;
        // list of mobs that are to be deleted, can't do it right away because it causes crashes
        private List<SimpleMob> _mobsToDelete = new List<SimpleMob>();

        public PowersMobTester(Universe.Universe universe)
        {
            _mobtesters.Add(this);
            this._universe = universe;
        }

        public void Destroy()
        {
            _mobtesters.Remove(this);
            _universe = null;
        }

        public IList<SimpleMob> SpawnMob(int count = 10)
        {
            // HACK: use player0's properties for spawning center
            Player player0 = _universe.PlayerManager.Players[0];

            // mob id list to select from when spawning
            int[] mobids = { 4282, 3893, 6652, 5428, 5346, 6024, 5393, 5433, 5467 };

            IList<SimpleMob> created = new List<SimpleMob>();

            for (int n = 0; n < count; ++n)
            {
                Vector3D position = new Vector3D();
                position.X = player0.Hero.Position.X;
                position.Y = player0.Hero.Position.Y;
                position.Z = player0.Hero.Position.Z;
                if ((n % 2) == 0)
                {
                    position.X += (float)(RandomHelper.NextDouble() * 20);
                    position.Y += (float)(RandomHelper.NextDouble() * 20);
                }
                else
                {
                    position.X -= (float)(RandomHelper.NextDouble() * 20);
                    position.Y -= (float)(RandomHelper.NextDouble() * 20);
                }

                int nId = mobids[RandomHelper.Next(mobids.Length - 1)];

                SimpleMob mob = new SimpleMob()
                {
                    ids = new List<ClientObjectId>(),
                    hp = 50,
                    position = position,
                    owner = this,
                    dead = false,
                };
                _mobs.Add(mob);
                created.Add(mob);

                foreach (GameClient client in _clients)
                {
                    ClientObjectId clid = ClientObjectId.GenerateNewId(client);
                    mob.ids.Add(clid);

                    #region ACDEnterKnown Hittable Zombie
                    client.SendMessage(new ACDEnterKnownMessage()
                    {
                        Id = 0x003B,
                        Field0 = clid.id,
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
                                    X = position.X + 5,
                                    Y = position.Y + 5,
                                    Z = position.Z,
                                },
                            },
                            // HACK: always spawns in player0's world
                            Field2 = player0.Hero.WorldId,
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
                        Field0 = clid.id,
                        Field1 = 0x1,
                        aAffixGBIDs = new int[0]
                    });
                    client.SendMessage(new AffixMessage()
                    {
                        Id = 0x48,
                        Field0 = clid.id,
                        Field1 = 0x2,
                        aAffixGBIDs = new int[0]
                    });
                    client.SendMessage(new ACDCollFlagsMessage
                    {
                        Id = 0xa6,
                        Field0 = clid.id,
                        Field1 = 0x1
                    });

                    client.SendMessage(new AttributesSetValuesMessage
                    {
                        Id = 0x4d,
                        Field0 = clid.id,
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
                        Field0 = clid.id,
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
                        Field0 = clid.id,
                        Field1 = unchecked((int)0xb59b8de4),
                        Field2 = unchecked((int)0xffffffff)
                    });

                    client.SendMessage(new ANNDataMessage
                    {
                        Id = 0x3e,
                        Field0 = clid.id
                    });

                    client.SendMessage(new ACDTranslateFacingMessage
                    {
                        Id = 0x70,
                        Field0 = clid.id,
                        Field1 = (float)(RandomHelper.NextDouble() * 2.0 * Math.PI),
                        Field2 = false
                    });

                    client.SendMessage(new SetIdleAnimationMessage
                    {
                        Id = 0xa5,
                        Field0 = clid.id,
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

            foreach (GameClient client in _clients)
                client.FlushOutgoingBuffer();

            return created;
        }

        public void KillMob(SimpleMob mob)
        {
            mob.dead = true;
            _mobs.Remove(mob);

            foreach (ClientObjectId clid in mob.ids)
            {
                // HACK: I guess I'll throw in the item spawn code here, with a random chance instead of every time
                if (RandomHelper.Next(1, 5) == 1)
                    _universe.SpawnRandomDrop(clid.client.Player.Hero, mob.position);

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
                clid.client.SendMessage(new PlayEffectMessage()
                {
                    Id = 0x7a,
                    Field0 = clid.id,
                    Field1 = 0x0,
                    Field2 = 0x2,
                });
                clid.client.SendMessage(new PlayEffectMessage()
                {
                    Id = 0x7a,
                    Field0 = clid.id,
                    Field1 = 0xc,
                });

                clid.client.SendMessage(new ANNDataMessage()
                {
                    Id = 0x6d,
                    Field0 = clid.id,
                });

                int ani = killAni[RandomHelper.Next(killAni.Length)];
                //Logger.Info("Ani used: " + ani);

                clid.client.SendMessage(new PlayAnimationMessage()
                {
                    Id = 0x6c,
                    Field0 = clid.id,
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

                clid.client.PacketId += 10 * 2;
                clid.client.SendMessage(new DWordDataMessage()
                {
                    Id = 0x89,
                    Field0 = clid.client.PacketId,
                });

                clid.client.SendMessage(new ANNDataMessage()
                {
                    Id = 0xc5,
                    Field0 = clid.id,
                });

                clid.client.SendMessage(new AttributeSetValueMessage
                {
                    Id = 0x4c,
                    Field0 = clid.id,
                    Field1 = new NetAttributeKeyValue
                    {
                        Attribute = GameAttribute.Attributes[0x4d],
                        Float = 0
                    }
                });

                clid.client.SendMessage(new AttributeSetValueMessage
                {
                    Id = 0x4c,
                    Field0 = clid.id,
                    Field1 = new NetAttributeKeyValue
                    {
                        Attribute = GameAttribute.Attributes[0x1c2],
                        Int = 1
                    }
                });

                clid.client.SendMessage(new AttributeSetValueMessage
                {
                    Id = 0x4c,
                    Field0 = clid.id,
                    Field1 = new NetAttributeKeyValue
                    {
                        Attribute = GameAttribute.Attributes[0x1c5],
                        Int = 1
                    }
                });

                clid.client.PacketId += 10 * 2;
                clid.client.SendMessage(new DWordDataMessage()
                {
                    Id = 0x89,
                    Field0 = clid.client.PacketId,
                });
                clid.client.FlushOutgoingBuffer();
            }

            mob.deleteTimeout = DateTime.Now.AddMilliseconds(100);
            _mobsToDelete.Add(mob);
        }

        public void Tick()
        {
            List<SimpleMob> survivors = new List<SimpleMob>();
            foreach (SimpleMob mob in _mobsToDelete)
            {
                if (DateTime.Now > mob.deleteTimeout)
                {
                    foreach (ClientObjectId clid in mob.ids)
                    {
                        clid.client.SendMessage(new ANNDataMessage()
                        {
                            Id = 0x3c,
                            Field0 = clid.id,
                        });
                    }
                }
                else
                {
                    survivors.Add(mob);
                }
            }
            _mobsToDelete = survivors;
        }

        private IEnumerable<GameClient> _clients
        {
            get
            {
                foreach (GameClient client in _universe.PlayerManager.Players.Select(p => p.Client))
                {
                    yield return client;
                }
            }
        }
    }
}
