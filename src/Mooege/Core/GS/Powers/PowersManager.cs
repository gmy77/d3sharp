using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mooege.Net.GS.Message.Fields;
using Mooege.Core.GS.Actors;
using Mooege.Net.GS;
using Mooege.Net.GS.Message.Definitions.Combat;
using Mooege.Core.GS.Skills;
using Mooege.Net.GS.Message.Definitions.ACD;
using Mooege.Net.GS.Message.Definitions.Misc;
using Mooege.Common.Helpers;
using Mooege.Net.GS.Message.Definitions.Animation;
using Mooege.Net.GS.Message;
using Mooege.Net.GS.Message.Definitions.Effect;
using Mooege.Net.GS.Message.Definitions.Attribute;
using Mooege.Net.GS.Message.Definitions.Player;

namespace Mooege.Core.GS.Powers
{
    internal class Effect
    {
        public Vector3D position;
        public DateTime timeout;
        public List<int> ids;
    }

    public interface IDamageable
    {
        void ReceiveDamage(Actor from, float amount, int type);
        Vector3D GetPosition();
        int GetId();
    }

    public class PowersManager
    {
        // temporary testing helper
        private PowersMobTester _mobtester;

        private Universe.Universe _universe;
        private List<Effect> _effects = new List<Effect>();
        private Random _rand = new Random();
        private Dictionary<Actor, Effect> _proxies = new Dictionary<Actor, Effect>();

        private DateTime _castTimeout = DateTime.MinValue;

        private IEnumerable<IDamageable> Damageables
        {
            get
            {
                return PowersMobTester.Damageables;
            }
        }

        public PowersManager(Universe.Universe universe)
        {
            _mobtester = new PowersMobTester(universe);
            _universe = universe;
        }

        public void Tick()
        {
            CleanUpEffects();
            _mobtester.Tick();
        }

        public void UsePower(Actor user, int powerId, int targetId = -1, Vector3D targetPos = null)
        {
            IDamageable target;

            if (targetId == -1)
            {
                target = null;
            }
            else if (GetDamagableFromId(targetId) != null)
            {
                target = GetDamagableFromId(targetId);
                targetPos = target.GetPosition();
            }
            else
            {
                return;
            }

            if (targetPos == null)
                targetPos = new Vector3D(0, 0, 0);

            // TODO: needs to be done in out-of-band thread eventually
            Tick();

            // HACK: some spells (electrocute, disintegrate) have insane cast speeds, not
            // sure how server handles this, maybe its just because ticks aren't implemented.
            if (DateTime.Now < _castTimeout)
                return;
            _castTimeout = DateTime.Now.AddMilliseconds(100);

            // TODO: need to split this out into multiple files eventually
            if (powerId == Skills.Skills.Monk.SpiritSpenders.BlindingFlash) // used for spawning stuff right now
            {
                _mobtester.SpawnMob();
            }
            else if (powerId == Skills.Skills.BasicAttack) // TODO: actual generic basic attack? or always melee?
            {
                if (target != null)
                {
                    if (Math.Abs(user.Position.X - targetPos.X) < 15f &&
                        Math.Abs(user.Position.Y - targetPos.Y) < 15f)
                    {
                        SendMessage(new PlayHitEffectMessage()
                        {
                            Id = 0x7b,
                            Field0 = target.GetId(),
                            Field1 = user.Id,
                            Field2 = 0,
                            Field3 = false
                        });
                        DoDamage(user, target, 25f, 0);
                        SendDWordTick();
                        FlushOutgoingBuffer();
                    }

                }
            }
            else if (powerId == Skills.Skills.Wizard.Utility.FrostNova) // testing skill
            {
                //if (target == null) break;

                // try to level up!

                SendMessage(new PlayerLevel()
                {
                    Id = 0x98,
                    Field0 = 0,
                    Field1 = 2
                });
                SendMessage(new PlayConvLineMessage()
                {
                    Id = 0xba,
                    Field0 = user.Id,
                    Field1 = new int[9]
                    {
                        user.Id, -1, -1, -1, -1, -1, -1, -1, -1
                    },
                    Field2 = new PlayLineParams()
                    {
                        snoConversation = 0x2a777,
                        Field1 = 0,
                        Field2 = false,
                        Field3 = 0,
                        Field4 = 0,
                        Field5 = -1,
                        Field6 = 2,
                        Field7 = 1,
                        Field8 = 2,
                        snoSpeakerActor = 0x0000197e,
                        Field10 = "aaa",
                        Field11 = 2,
                        Field12 = -1,
                        Field13 = 0x32,
                        Field14 = 0x5a,
                        Field15 = 0x32
                    },
                    Field3 = 0x32
                });
                SendDWordTick();

                FlushOutgoingBuffer();
            }
            else if (powerId == Skills.Skills.Wizard.Signature.Electrocute) // electrocute
            {
                // TODO: refactor this targetting style and effect messages out of here
                IList<IDamageable> targets;
                List<int> effect_targets = new List<int>();
                if (target == null)
                {
                    effect_targets.Add(GetProxyIdFor(user, targetPos));
                    targets = new List<IDamageable>();
                }
                else
                {
                    targets = FindTargetsInRadius(targetPos, 15f, 1);
                    targets.Insert(0, target);
                    foreach (var d in targets)
                        effect_targets.Add(d.GetId());
                }

                // look at target
                float facing = (float)Math.Atan2(targetPos.Y - user.Position.Y, targetPos.X - user.Position.X);
                SendMessage(new ACDTranslateFacingMessage
                {
                    Id = 0x70,
                    Field0 = user.Id,
                    Field1 = facing,
                    Field2 = false
                });

                int effect_source = user.Id;
                foreach (int id in effect_targets)
                {
                    //SendMessage(new PlayEffectMessage() // sound effect
                    //{
                    //    Id = 0x7a,
                    //    Field0 = id,
                    //    Field1 = 0,
                    //    Field2 = 0x07
                    //});
                    //SendMessage(new PlayHitEffectMessage() // hit lightining
                    //{
                    //    Id = 0x7b,
                    //    Field0 = id,
                    //    Field1 = user.Id,
                    //    Field2 = 2,
                    //    Field3 = false
                    //});
                    SendMessage(new RopeEffectMessageACDToACD() // bolt
                    {
                        Id = 0x00ab,
                        Field0 = 0x078C0,
                        Field1 = effect_source,
                        Field2 = 4,
                        Field3 = id,
                        Field4 = 1
                    });
                    SendDWordTick();

                    effect_source = id;
                }

                foreach (IDamageable d in targets)
                {
                    DoDamage(user, d, 12, 0);
                }
                FlushOutgoingBuffer();
            }
            else if (powerId == Skills.Skills.Wizard.Signature.MagicMissile)
            {
                for (int step = 1; step < 10; ++step)
                {
                    var spos = new Vector3D();
                    spos.X = user.Position.X + ((targetPos.X - user.Position.X) * (step * 0.10f));
                    spos.Y = user.Position.Y + ((targetPos.Y - user.Position.Y) * (step * 0.10f));
                    spos.Z = user.Position.Z + ((targetPos.Z - user.Position.Z) * (step * 0.10f));

                    SpawnEffect(user, 61419, spos);

                    IList<IDamageable> hits = FindTargetsInRadius(spos, 6f);
                    foreach (IDamageable x in hits)
                    {
                        DoDamage(user, x, 20f, 0);
                    }
                    FlushOutgoingBuffer();
                    System.Threading.Thread.Sleep(100);
                }
            }
            else if (powerId == Skills.Skills.Wizard.Offensive.Hydra) // demonic meteor
            {
                SpawnEffect(user, 185366, targetPos);
                FlushOutgoingBuffer();

                System.Threading.Thread.Sleep(400);

                IList<IDamageable> hits = FindTargetsInRadius(targetPos, 10f);
                foreach (IDamageable x in hits)
                {
                    DoDamage(user, x, 100f, 0);
                }
                FlushOutgoingBuffer();
            }
            else if (powerId == Skills.Skills.Wizard.Offensive.Meteor)
            {
                SpawnEffect(user, 86790, targetPos);
                FlushOutgoingBuffer();
                System.Threading.Thread.Sleep(2000);
                SpawnEffect(user, 86769, targetPos);
                SpawnEffect(user, 90364, targetPos); //, -1.0f, 5000);

                IList<IDamageable> hits = FindTargetsInRadius(targetPos, 13f);
                foreach (IDamageable x in hits)
                {
                    DoDamage(user, x, 150f, 0);
                }
                FlushOutgoingBuffer();
            }
            else if (powerId == Skills.Skills.Wizard.Offensive.Disintegrate)
            {
                //SpawnEffect(user, 52687, targetPos);
            }
            else if (powerId == Skills.Skills.Monk.SpiritSpenders.SevenSidedStrike)
            {
                Vector3D startpos;
                if (target == null)
                    startpos = user.Position;
                else
                    startpos = targetPos;

                IList<IDamageable> nearby = FindTargetsInRadius(startpos, 20f, 7);
                for (int n = 0; n < 7; ++n)
                {
                    if (nearby.Count > 0)
                    {
                        SpawnEffect(user, 99063, nearby[0].GetPosition());
                        DoDamage(user, nearby[0], 100f, 0);
                        nearby.RemoveAt(0);
                    }
                    FlushOutgoingBuffer();
                    System.Threading.Thread.Sleep(100);
                }
            }
        }

        private void CleanUpEffects()
        {
            List<Effect> survivors = new List<Effect>();
            var curtime = DateTime.Now;
            foreach (Effect effect in _effects)
            {
                if (curtime > effect.timeout)
                    KillSpawnedEffect(effect);
                else
                    survivors.Add(effect);
            }

            _effects = survivors;
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

        private Effect SpawnEffectObject(Actor from, int effectId, Vector3D position, float angle = -1f /*random*/, int timeout = 2000)
        {
            Effect effect = new Effect();
            effect.ids = new List<int>();
            effect.position = position;
            effect.timeout = DateTime.Now.AddMilliseconds(timeout);

            if (angle == -1f)
                angle = (float)_rand.NextDouble() * 360f;

            float quat_w = (float)Math.Cos(angle / 2f);
            float quat_z = (float)Math.Sin(angle / 2f);

            foreach (GameClient client in _clients)
            {
                int nId = effectId;

                client.ObjectId++;
                effect.ids.Add(client.ObjectId);

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
                                Amount = quat_w,
                                Axis = new Vector3D()
                                {
                                    X = 0f,
                                    Y = 0f,
                                    Z = quat_z,
                                },
                            },
                            ReferencePoint = new Vector3D()
                            {
                                X = position.X,
                                Y = position.Y,
                                Z = position.Z,
                            },
                        },
                        Field2 = from.WorldId, // 0x772E0000,
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

                // removed attribute set code
                /////////////////////////////

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
                    Field1 = (float)(RandomHelper.NextDouble() * 2.0 * Math.PI), // TODO: make this use angle
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

            return effect;
        }

        private void SpawnEffect(Actor from, int effectId, Vector3D position, float angle = -1f /*random*/, int timeout = 2000)
        {
            _effects.Add(SpawnEffectObject(from, effectId, position, angle, timeout));
        }

        private void KillSpawnedEffect(Effect effect)
        {
            foreach (GameClient client in _clients)
            {
                foreach (int id in effect.ids)
                {
                    var killAni = new int[]
                    {
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
                        Field0 = id,
                        Field1 = 0x0,
                        Field2 = 0x2,
                    });
                    client.SendMessage(new PlayEffectMessage()
                    {
                        Id = 0x7a,
                        Field0 = id,
                        Field1 = 0xc,
                    });
                    client.SendMessage(new PlayHitEffectMessage()
                    {
                        Id = 0x7b,
                        Field0 = id,
                        Field1 = 0x789E00E2,
                        Field2 = 0x2,
                        Field3 = false,
                    });

                    //client.SendMessage(new FloatingNumberMessage()
                    //{
                    //    Id = 0xd0,
                    //    Field0 = id,
                    //    Field1 = 9001.0f,
                    //    Field2 = 0,
                    //});

                    client.SendMessage(new ANNDataMessage()
                    {
                        Id = 0x6d,
                        Field0 = id,
                    });

                    int ani = killAni[RandomHelper.Next(killAni.Length)];
                    //Logger.Info("Ani used: " + ani);

                    client.SendMessage(new PlayAnimationMessage()
                    {
                        Id = 0x6c,
                        Field0 = id,
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
                        Field0 = id,
                    });

                    client.SendMessage(new AttributeSetValueMessage
                    {
                        Id = 0x4c,
                        Field0 = id,
                        Field1 = new NetAttributeKeyValue
                        {
                            Attribute = GameAttribute.Attributes[0x4d],
                            Float = 0
                        }
                    });

                    client.SendMessage(new AttributeSetValueMessage
                    {
                        Id = 0x4c,
                        Field0 = id,
                        Field1 = new NetAttributeKeyValue
                        {
                            Attribute = GameAttribute.Attributes[0x1c2],
                            Int = 1
                        }
                    });

                    client.SendMessage(new AttributeSetValueMessage
                    {
                        Id = 0x4c,
                        Field0 = id,
                        Field1 = new NetAttributeKeyValue
                        {
                            Attribute = GameAttribute.Attributes[0x1c5],
                            Int = 1
                        }
                    });
                    client.SendMessage(new PlayEffectMessage()
                    {
                        Id = 0x7a,
                        Field0 = id,
                        Field1 = 0xc,
                    });
                    client.SendMessage(new PlayEffectMessage()
                    {
                        Id = 0x7a,
                        Field0 = id,
                        Field1 = 0x37,
                    });
                    client.SendMessage(new PlayHitEffectMessage()
                    {
                        Id = 0x7b,
                        Field0 = id,
                        Field1 = 0x789E00E2,
                        Field2 = 0x2,
                        Field3 = false,
                    });

                    client.SendMessage(new ANNDataMessage()
                    {
                        Id = 0x3c,
                        Field0 = id,
                    });

                    client.PacketId += 10 * 2;
                    client.SendMessage(new DWordDataMessage()
                    {
                        Id = 0x89,
                        Field0 = client.PacketId,
                    });
                }
                client.FlushOutgoingBuffer();
            }
        }

        private IList<IDamageable> FindTargetsInRadius(Vector3D center, float radius, int maxCount = -1)
        {
            List<IDamageable> hits = new List<IDamageable>();
            foreach (IDamageable d in Damageables)
            {
                if (hits.Count == maxCount)
                    break;

                if (Math.Abs(d.GetPosition().X - center.X) < radius &&
                    Math.Abs(d.GetPosition().Y - center.Y) < radius &&
                    Math.Abs(d.GetPosition().Z - center.Z) < radius)
                {
                    hits.Add(d);
                }
            }
            return hits;
        }

        private int GetProxyIdFor(Actor from, Vector3D pos)
        {
            if (!_proxies.ContainsKey(from))
            {
                _proxies[from] = SpawnEffectObject(from, 187359, pos);
            }
            foreach (GameClient client in _clients)
            {
                SendMessage(new ACDWorldPositionMessage()
                {
                    Id = 0x003f,
                    Field0 = _proxies[from].ids[0], // HACK: make ids associative
                    Field1 = new WorldLocationMessageData()
                    {
                        Field0 = 1f,
                        Field1 = new PRTransform()
                        {
                            Field0 = new Quaternion()
                            {
                                Amount = 0.768145f,
                                Axis = new Vector3D(0, 0, -0.640276f)
                            },
                            ReferencePoint = pos
                        },
                        Field2 = from.WorldId, // 0x772E0000,
                    }
                });
            }

            return _proxies[from].ids[0]; // HACK: make ids associative
        }

        private void DoDamage(Actor from, IDamageable to, float amount, int type)
        {
            SendMessage(new FloatingNumberMessage()
            {
                Id = 0xd0,
                Field0 = to.GetId(),
                Field1 = amount,
                Field2 = type,
            });
            to.ReceiveDamage(from, amount, type);
            FlushOutgoingBuffer();
        }

        private void DoDamage(Actor from, IList<IDamageable> to_list, float amount, int type)
        {
            foreach (IDamageable dam in to_list)
            {
                SendMessage(new FloatingNumberMessage()
                {
                    Id = 0xd0,
                    Field0 = dam.GetId(),
                    Field1 = amount,
                    Field2 = type,
                });
                dam.ReceiveDamage(from, amount, type);
            }
            FlushOutgoingBuffer();
        }

        private IDamageable GetDamagableFromId(int id)
        {
            foreach (IDamageable d in Damageables)
            {
                if (d.GetId() == id)
                    return d;
            }
            return null;
        }

        private void SendMessage(GameMessage msg)
        {
            foreach (GameClient client in _clients)
            {
                client.SendMessage(msg);
            }
        }

        private void FlushOutgoingBuffer()
        {
            foreach (GameClient client in _clients)
            {
                client.FlushOutgoingBuffer();
            }
        }

        private void SendDWordTick()
        {
            foreach (GameClient client in _clients)
            {
                client.PacketId += 10 * 2;
                SendMessage(new DWordDataMessage()
                {
                    Id = 0x89,
                    Field0 = client.PacketId,
                });
            }
        }
    }
}
