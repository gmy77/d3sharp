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
    public class SimpleMob : Actor
    {
        public PowersMobTester owner;
        public DateTime deleteTimeout;
        public float hp;
        public bool dead;

        public void ReceiveDamage(Actor from, float amount, int type)
        {
            hp -= amount;
            if (!dead && hp <= 0.0f)
                owner.KillMob(from, this);
        }
    }

    // simple hackish mob spawner, all mobs created by all instances of it are available via the AllMobs
    // property
    public class PowersMobTester
    {
        public static IEnumerable<SimpleMob> AllMobs
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
        // list of mobs that are to be deleted, can't do it right away because
        // it causes crashes when ropes are attached to the actor
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

        public IList<SimpleMob> SpawnMob(Actor user, int count = 10, int mobcode = -1)
        {
            // mob id list to select from when spawning if mobcode == -1
            int[] mobids = { 4282, 3893, 6652, 5428, 5346, 6024, 5393, 5433, 5467 };

            IList<SimpleMob> created = new List<SimpleMob>();

            for (int n = 0; n < count; ++n)
            {
                Vector3D position = new Vector3D();
                position.X = user.Position.X;
                position.Y = user.Position.Y;
                position.Z = user.Position.Z;
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

                int nId;
                if (mobcode == -1)
                    nId = mobids[RandomHelper.Next(mobids.Length - 1)];
                else
                    nId = mobcode;

                SimpleMob mob = new SimpleMob()
                {
                    DynamicId = _universe.NextObjectId,
                    Position = position,
                    SnoId = nId,
                    Scale = 1.35f,
                    RotationAmount = 1f,
                    RotationAxis = new Vector3D(0, 0, 0),
                    WorldId = user.WorldId,
                    GBHandle = new GBHandle()
                    {
                        Field0 = 1,
                        Field1 = 1,
                    },
                    Field2 = 0x8,
                    Field3 = 0x0,
                    // TODO might need more
                    hp = 50,
                    owner = this,
                    dead = false,
                };
                _mobs.Add(mob);
                created.Add(mob);

                foreach (GameClient client in _clientsInSameWorld(user))
                {
                    mob.Reveal(client.Player.Hero);

                    client.SendMessage(new AffixMessage()
                    {
                        Id = 0x48,
                        Field0 = mob.DynamicId,
                        Field1 = 0x1,
                        aAffixGBIDs = new int[0]
                    });
                    client.SendMessage(new AffixMessage()
                    {
                        Id = 0x48,
                        Field0 = mob.DynamicId,
                        Field1 = 0x2,
                        aAffixGBIDs = new int[0]
                    });
                    client.SendMessage(new ACDCollFlagsMessage
                    {
                        Id = 0xa6,
                        Field0 = mob.DynamicId,
                        Field1 = 0x1
                    });

                    GameAttributeMap attribs = new GameAttributeMap();
                    attribs[GameAttribute.Untargetable] = false;
                    attribs[GameAttribute.Uninterruptible] = true;
                    attribs[GameAttribute.Buff_Visual_Effect, 1048575] = true;
                    attribs[GameAttribute.Buff_Icon_Count0, 30582] = 1;
                    attribs[GameAttribute.Buff_Icon_Count0, 30286] = 1;
                    attribs[GameAttribute.Buff_Icon_Count0, 30285] = 1;
                    attribs[GameAttribute.Buff_Icon_Count0, 30284] = 1;
                    attribs[GameAttribute.Buff_Icon_Count0, 30283] = 1;
                    attribs[GameAttribute.Buff_Icon_Count0, 30290] = 1;
                    attribs[GameAttribute.Buff_Icon_Count0, 79486] = 1;
                    attribs[GameAttribute.Buff_Active, 30286] = true;
                    attribs[GameAttribute.Buff_Active, 30285] = true;
                    attribs[GameAttribute.Buff_Active, 30284] = true;
                    attribs[GameAttribute.Buff_Active, 30283] = true;
                    attribs[GameAttribute.Buff_Active, 30290] = true;

                    attribs[GameAttribute.Hitpoints_Max_Total] = 4.546875f;
                    attribs[GameAttribute.Buff_Active, 79486] = true;
                    attribs[GameAttribute.Hitpoints_Max] = 4.546875f;
                    attribs[GameAttribute.Hitpoints_Total_From_Level] = 0f;
                    attribs[GameAttribute.Hitpoints_Cur] = 4.546875f;
                    attribs[GameAttribute.Invulnerable] = true;
                    attribs[GameAttribute.Buff_Active, 30582] = true;
                    attribs[GameAttribute.TeamID] = 10;
                    attribs[GameAttribute.Level] = 1;

                    attribs.SendMessage(client, mob.DynamicId);

                    client.SendMessage(new ACDGroupMessage
                    {
                        Id = 0xb8,
                        Field0 = mob.DynamicId,
                        Field1 = unchecked((int)0xb59b8de4),
                        Field2 = unchecked((int)0xffffffff)
                    });

                    client.SendMessage(new ANNDataMessage
                    {
                        Id = 0x3e,
                        Field0 = mob.DynamicId
                    });

                    client.SendMessage(new ACDTranslateFacingMessage
                    {
                        Id = 0x70,
                        Field0 = mob.DynamicId,
                        Field1 = (float)(RandomHelper.NextDouble() * 2.0 * Math.PI),
                        Field2 = false
                    });

                    client.SendMessage(new SetIdleAnimationMessage
                    {
                        Id = 0xa5,
                        Field0 = mob.DynamicId,
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

                    client.FlushOutgoingBuffer();
                }
            }
            return created;
        }

        public void KillMob(Actor user, SimpleMob mob)
        {
            mob.dead = true;
            _mobs.Remove(mob);

            foreach (GameClient client in _clientsInSameWorld(user))
            {
                // HACK: I guess I'll throw in the item spawn code here, with a random chance instead of every time
                if (RandomHelper.Next(1, 5) == 1)
                    _universe.SpawnRandomDrop((Hero)user, mob.Position);
                _universe.SpawnGold((Hero)user, mob.Position);

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
                    Field0 = mob.DynamicId,
                    Field1 = 0x0,
                    Field2 = 0x2,
                });
                client.SendMessage(new PlayEffectMessage()
                {
                    Id = 0x7a,
                    Field0 = mob.DynamicId,
                    Field1 = 0xc,
                });

                client.SendMessage(new ANNDataMessage()
                {
                    Id = 0x6d,
                    Field0 = mob.DynamicId,
                });

                int ani = killAni[RandomHelper.Next(killAni.Length)];
                //Logger.Info("Ani used: " + ani);

                client.SendMessage(new PlayAnimationMessage()
                {
                    Id = 0x6c,
                    Field0 = mob.DynamicId,
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
                    Field0 = mob.DynamicId,
                });

                client.SendMessage(new AttributeSetValueMessage
                {
                    Id = 0x4c,
                    Field0 = mob.DynamicId,
                    Field1 = new NetAttributeKeyValue
                    {
                        Attribute = GameAttribute.Attributes[0x4d],
                        Float = 0
                    }
                });

                client.SendMessage(new AttributeSetValueMessage
                {
                    Id = 0x4c,
                    Field0 = mob.DynamicId,
                    Field1 = new NetAttributeKeyValue
                    {
                        Attribute = GameAttribute.Attributes[0x1c2],
                        Int = 1
                    }
                });

                client.SendMessage(new AttributeSetValueMessage
                {
                    Id = 0x4c,
                    Field0 = mob.DynamicId,
                    Field1 = new NetAttributeKeyValue
                    {
                        Attribute = GameAttribute.Attributes[0x1c5],
                        Int = 1
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
                    foreach (GameClient client in _clientsInSameWorld(mob))
                    {
                        mob.Destroy(client.Player.Hero);
                    }
                }
                else
                {
                    survivors.Add(mob);
                }
            }
            _mobsToDelete = survivors;
        }

        private IEnumerable<GameClient> _clientsInSameWorld(Actor actor)
        {
            return _universe.PlayerManager.Players
                                          .FindAll(p => p.Hero.WorldId == actor.WorldId)
                                          .Select(p => p.Client);
        }
    }
}
