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
        public IList<ClientObjectId> ids;
    }

    public class PowersManager
    {
        // temporary testing helper
        private PowersMobTester _mobtester;

        private Universe.Universe _universe;

        private List<Effect> _effects = new List<Effect>();
        private Random _rand = new Random();
        private List<Actor> _channelingActors = new List<Actor>();
        // TODO: multiple proxies per user needed?
        private Dictionary<Actor, Effect> _proxies = new Dictionary<Actor, Effect>();

        // channeled spells fire extremely fast, we need some way to slow down damage and graphic effects,
        // while still updating position/targeting data.
        private Dictionary<Actor, DateTime> _castDelays = new Dictionary<Actor, DateTime>();

        // supplies Powers with all available targets.
        private IEnumerable<IPowerTarget> Targets
        {
            get
            {
                return PowersMobTester.PowerTargets;
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
            IPowerTarget target;

            if (targetId == -1)
            {
                target = null;
            }
            else if (GetPowerTargetFromId(targetId) != null)
            {
                target = GetPowerTargetFromId(targetId);
                targetPos = target.GetPosition();
            }
            else
            {
                return;
            }

            if (targetPos == null)
                targetPos = new Vector3D(0, 0, 0);

            // do casting delay if using channeled power
            if (_channelingActors.Contains(user))
            {
                bool delayed = CastDelay(user);
                if (delayed)
                {
                    // update proxy if needed
                    if (target == null)
                    {
                        GetProxyEffectFor(user, targetPos);
                        SendDWordTick();
                        FlushOutgoingBuffer();
                    }
                    return;
                }
            }

            #region Conceptual power implementations
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
                        foreach (ClientObjectId clid in target.GetIds())
                        {
                            clid.client.SendMessage(new PlayHitEffectMessage()
                            {
                                Id = 0x7b,
                                Field0 = clid.id,
                                Field1 = user.DynamicId,
                                Field2 = 4,
                                Field3 = false
                            });
                        }
                        DoDamage(user, target, 25f, 0);
                        SendDWordTick();
                        FlushOutgoingBuffer();
                    }

                }
            }
            else if (powerId == Skills.Skills.Wizard.Utility.FrostNova) // testing skill
            {
                //if (target == null) break;
            }
            else if (powerId == Skills.Skills.Wizard.Signature.Electrocute) // electrocute
            {
                // channeled power
                if (!_channelingActors.Contains(user))
                    _channelingActors.Add(user);

                LookAt(user, targetPos);

                IList<IPowerTarget> targets;
                if (target == null)
                {
                    targets = new List<IPowerTarget>();
                    PlayRopeEffectToActor(0x78c0, GetIdsForActor(user), GetProxyEffectFor(user, targetPos).ids);
                    SendDWordTick();
                }
                else
                {
                    targets = FindTargetsInRadius(targetPos, 15f, 1);
                    targets.Insert(0, target);
                    IList<ClientObjectId> effect_source = GetIdsForActor(user);
                    foreach (IPowerTarget tar in targets)
                    {
                        PlayHitEffect(2, effect_source, tar.GetIds());
                        PlayRopeEffectToActor(0x78c0, effect_source, tar.GetIds());
                        SendDWordTick();

                        effect_source = tar.GetIds();
                    }
                }

                foreach (IPowerTarget t in targets)
                {
                    DoDamage(user, t, 12, 0);
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

                    IList<IPowerTarget> hits = FindTargetsInRadius(spos, 6f);
                    foreach (IPowerTarget x in hits)
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

                IList<IPowerTarget> hits = FindTargetsInRadius(targetPos, 10f);
                foreach (IPowerTarget x in hits)
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
                SpawnEffect(user, 90364, targetPos, -1, 4000);

                IList<IPowerTarget> hits = FindTargetsInRadius(targetPos, 13f);
                foreach (IPowerTarget x in hits)
                {
                    DoDamage(user, x, 150f, 0);
                }
                FlushOutgoingBuffer();
            }
            else if (powerId == Skills.Skills.Wizard.Offensive.Disintegrate)
            {
                Effect pid = GetProxyEffectFor(user, targetPos);
                if (!_channelingActors.Contains(user))
                {
                    _channelingActors.Add(user);
                    PlayRopeEffectToActor(30888, GetIdsForActor(user), pid.ids);
                }
                SendDWordTick();

                //DoDamage(user, target, 12, 0);
                FlushOutgoingBuffer();
            }
            else if (powerId == Skills.Skills.Monk.SpiritSpenders.SevenSidedStrike)
            {
                Vector3D startpos;
                if (target == null)
                    startpos = user.Position;
                else
                    startpos = targetPos;

                IList<IPowerTarget> nearby = FindTargetsInRadius(startpos, 20f, 7);
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
            #endregion // conceptual power implementations
        }

        private bool CastDelay(Actor user)
        {
            if (!_castDelays.ContainsKey(user) || DateTime.Now > _castDelays[user])
            {
                _castDelays[user] = DateTime.Now.AddMilliseconds(150); // TODO: might need to make this set per-power
                return false;
            }
            else
            {
                return true;
            }
        }

        public void CancelChanneledPower(Universe.Hero hero, int powerId)
        {
            if (_channelingActors.Contains(hero))
                _channelingActors.Remove(hero);

            if (_proxies.ContainsKey(hero))
            {
                KillSpawnedEffect(_proxies[hero]);
                _proxies.Remove(hero);
            }
        }

        private void PlayHitEffect(int effectId, IList<ClientObjectId> from, IList<ClientObjectId> target)
        {
            foreach (ClientObjectId clid in target)
            {
                clid.client.SendMessage(new PlayHitEffectMessage()
                {
                    Id = 0x7b,
                    Field0 = clid.id,
                    Field1 = from.First(clo => clo.client == clid.client).id,
                    Field2 = effectId,
                    Field3 = false
                });
            }
        }

        private void PlayRopeEffectToActor(int effectId, IList<ClientObjectId> from, IList<ClientObjectId> target)
        {
            foreach (ClientObjectId clid in target)
            {
                clid.client.SendMessage(new RopeEffectMessageACDToACD()
                {
                    Id = 0x00ab,
                    Field0 = effectId,
                    Field1 = from.First(clo => clo.client == clid.client).id,
                    Field2 = 4,
                    Field3 = clid.id,
                    Field4 = 1
                });
            }
        }

        private void LookAt(Actor actor, Vector3D targetPos)
        {
            float facing = (float)Math.Atan2(targetPos.Y - actor.Position.Y, targetPos.X - actor.Position.X);
            foreach (ClientObjectId clid in GetIdsForActor(actor))
            {
                clid.client.SendMessage(new ACDTranslateFacingMessage
                {
                    Id = 0x70,
                    Field0 = clid.id,
                    Field1 = facing,
                    Field2 = false
                });
            }
        }

        private IList<ClientObjectId> GetIdsForActor(Actor actor)
        {
            // HACK: actor doesn't current track per-client ids so we just hard wire it to player0
            return new List<ClientObjectId>()
            {
                new ClientObjectId()
                {
                    id = actor.DynamicId,
                    client = _universe.PlayerManager.Players[0].Client
                }
            };
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
                return _universe.PlayerManager.Players.Select(p => p.Client);
            }
        }

        private Effect SpawnEffectObject(Actor from, int effectId, Vector3D position, float angle = -1f /*random*/, int timeout = 2000)
        {
            Effect effect = new Effect()
            {
                ids = new List<ClientObjectId>(),
                position = position,
                timeout = DateTime.Now.AddMilliseconds(timeout)
            };

            if (angle == -1f)
                angle = (float)_rand.NextDouble() * 360f;

            float quat_w = (float)Math.Cos(angle / 2f);
            float quat_z = (float)Math.Sin(angle / 2f);

            foreach (GameClient client in _clients)
            {
                ClientObjectId clid = ClientObjectId.GenerateNewId(client);
                effect.ids.Add(clid);

                #region ACDEnterKnown Hittable Zombie
                client.SendMessage(new ACDEnterKnownMessage()
                {
                    Id = 0x003B,
                    Field0 = clid.id,
                    Field1 = effectId,
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
                    Field8 = effectId,
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

                // removed attribute set code
                /////////////////////////////

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
                    Field1 = (float)(RandomHelper.NextDouble() * 2.0 * Math.PI), // TODO: make this use angle
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
                        Field1 = effectId
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
            foreach (ClientObjectId clid in effect.ids)
            {
                clid.client.SendMessage(new ANNDataMessage()
                {
                    Id = 0x3c,
                    Field0 = clid.id,
                });

                clid.client.PacketId += 10 * 2;
                clid.client.SendMessage(new DWordDataMessage()
                {
                    Id = 0x89,
                    Field0 = clid.client.PacketId,
                });
                clid.client.FlushOutgoingBuffer();
            }
        }

        private IList<IPowerTarget> FindTargetsInRadius(Vector3D center, float radius, int maxCount = -1)
        {
            List<IPowerTarget> hits = new List<IPowerTarget>();
            foreach (IPowerTarget d in Targets)
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

        private Effect GetProxyEffectFor(Actor from, Vector3D pos)
        {
            if (!_proxies.ContainsKey(from))
            {
                _proxies[from] = SpawnEffectObject(from, 187359, pos);
            }
            else
            {
                foreach (ClientObjectId clid in _proxies[from].ids)
                {
                    // not working
                    //clid.client.SendMessage(new ACDTranslateNormalMessage()
                    //{
                    //    Id = 0x6e,
                    //    Field0 = clid.id,
                    //    Position = pos,
                    //    Field2 = 5.5f,
                    //    Field5 = 1032
                    //});
                    clid.client.SendMessage(new ACDWorldPositionMessage()
                    {
                        Id = 0x003f,
                        Field0 = clid.id,
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
                            Field2 = from.WorldId,
                        }
                    });
                }
            }

            return _proxies[from];
        }

        private void DoDamage(Actor from, IPowerTarget target, float amount, int type)
        {
            foreach (ClientObjectId clid in target.GetIds())
            {
                clid.client.SendMessage(new FloatingNumberMessage()
                {
                    Id = 0xd0,
                    Field0 = clid.id,
                    Field1 = amount,
                    Field2 = type,
                });
            }
            SendDWordTick();
            target.ReceiveDamage(from, amount, type);
            FlushOutgoingBuffer();
        }

        private void DoDamage(Actor from, IList<IPowerTarget> target_list, float amount, int type)
        {
            foreach (IPowerTarget target in target_list)
            {
                foreach (ClientObjectId clid in target.GetIds())
                {
                    clid.client.SendMessage(new FloatingNumberMessage()
                    {
                        Id = 0xd0,
                        Field0 = clid.id,
                        Field1 = amount,
                        Field2 = type,
                    });
                }
                SendDWordTick();
                target.ReceiveDamage(from, amount, type);
            }
            FlushOutgoingBuffer();
        }

        private IPowerTarget GetPowerTargetFromId(int id)
        {
            return Targets.FirstOrDefault(t => t.GetIds().Any(clid => clid.id == id));
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
                client.SendMessage(new DWordDataMessage()
                {
                    Id = 0x89,
                    Field0 = client.PacketId,
                });
            }
        }
    }
}
