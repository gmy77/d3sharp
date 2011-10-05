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
using Mooege.Common;

namespace Mooege.Core.GS.Powers
{
    public class Effect : Actor
    {
        public DateTime timeout;
    }

    public class PowersManager
    {
        static readonly Logger Logger = LogManager.CreateLogger();

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

        // list of all waiting to execute powers
        class WaitingPower
        {
            public IEnumerator<int> PowerEnumerator;
            public DateTime Timeout;
            public Actor User;
        }
        List<WaitingPower> _waitingPowers = new List<WaitingPower>();

        // supplies Powers with all available targets.
        private IEnumerable<Actor> Targets
        {
            get
            {
                return PowersMobTester.AllMobs;
            }
        }

        public PowersManager(Universe.Universe universe)
        {
            _mobtester = new PowersMobTester(universe);
            _universe = universe;
        }

        public void Tick()
        {
            UpdateWaitingPowers();
            CleanUpEffects();
            _mobtester.Tick();
        }

        public void UsePower(Actor user, int powerId, int targetId = -1, Vector3D targetPos = null, TargetMessage message = null)
        {
            Actor target;

            if (targetId == -1)
            {
                target = null;
            }
            else if (GetActorFromId(targetId) != null)
            {
                target = GetActorFromId(targetId);
                targetPos = target.Position;
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
                        SendDWordTickFor(user);
                    }
                    return;
                }
            }

            if (powerId == Skills.Skills.Monk.SpiritSpenders.BlindingFlash) // HACK: intercepted to use for spawning test mobs
            {
                _mobtester.SpawnMob(user);
            }
            else
            {
                // find and run an implementation
                var implementation = PowerImplementation.ImplementationForId(powerId);
                if (implementation != null)
                {
                    IEnumerable<int> powerExe = implementation.Run(new PowerParameters()
                    {
                        User = user,
                        Target = target,
                        TargetPosition = targetPos,
                        Message = message
                    },
                    this);

                    var powerEnum = powerExe.GetEnumerator();
                    // actual power will first run here, if it yielded a value process it in the waiting list
                    if (powerEnum.MoveNext())
                    {
                        AddWaitingPower(_waitingPowers, powerEnum, user);
                    }
                    // send tick after executing power
                    SendDWordTickFor(user);
                }
            }                
        }

        private void AddWaitingPower(IList<WaitingPower> list, IEnumerator<int> powerEnum, Actor user)
        {
            WaitingPower wait = new WaitingPower();
            wait.PowerEnumerator = powerEnum;
            wait.User = user;
            if (powerEnum.Current == 0)
                wait.Timeout = DateTime.MinValue;
            else
                wait.Timeout = DateTime.Now.AddMilliseconds(powerEnum.Current);

            list.Add(wait);
        }

        private void UpdateWaitingPowers()
        {
            List<WaitingPower> newWaitList = new List<WaitingPower>();
            foreach (WaitingPower wait in _waitingPowers)
            {
                if (DateTime.Now > wait.Timeout)
                {
                    if (wait.PowerEnumerator.MoveNext())
                    {
                        // re-add with new timeout
                        AddWaitingPower(newWaitList, wait.PowerEnumerator, wait.User);
                    }
                    // else did not request another wait

                    // send dword tick after every power execute
                    SendDWordTickFor(wait.User);
                }
                else
                {
                    // re-add with same timeout
                    newWaitList.Add(wait);
                }
            }
            _waitingPowers = newWaitList;

            // HACK: just flush all clients right now
            foreach (GameClient client in _universe.PlayerManager.Players.Select(p => p.Client))
            {
                client.FlushOutgoingBuffer();
            }
        }

        public void SendDWordTickFor(Actor user)
        {
            foreach (GameClient client in _clientsInSameWorld(user))
                SendDWordTick(client);
        }

        public bool CastDelay(Actor user)
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

        public void CancelChanneledPower(Actor user, int powerId)
        {
            if (_channelingActors.Contains(user))
                _channelingActors.Remove(user);

            if (_proxies.ContainsKey(user))
            {
                KillSpawnedEffect(_proxies[user]);
                _proxies.Remove(user);
            }
        }

        public void PlayHitEffect(int effectId, Actor from, Actor target)
        {
            foreach (GameClient client in _clientsInSameWorld(target))
            {
                client.SendMessage(new PlayHitEffectMessage()
                {
                    Id = 0x7b,
                    Field0 = target.DynamicId,
                    Field1 = from.DynamicId,
                    Field2 = effectId,
                    Field3 = false
                });
            }
        }

        public void PlayRopeEffectActorToActor(int effectId, Actor from, Actor target)
        {
            foreach (GameClient client in _clientsInSameWorld(target))
            {
                client.SendMessage(new RopeEffectMessageACDToACD()
                {
                    Id = 0x00ab,
                    Field0 = effectId,
                    Field1 = from.DynamicId,
                    Field2 = 4,
                    Field3 = target.DynamicId,
                    Field4 = 1
                });
            }
        }

        public void PlayEffectGroupActorToActor(int effectId, Actor from, Actor target)
        {
            foreach (GameClient client in _clientsInSameWorld(target))
            {
                client.SendMessage(new EffectGroupACDToACDMessage()
                {
                    Id = 0xaa,
                    Field0 = effectId,
                    Field1 = from.DynamicId,
                    Field2 = target.DynamicId
                });
            }
        }

        public void LookAt(Actor actor, Vector3D targetPos)
        {
            float facing = (float)Math.Atan2(targetPos.Y - actor.Position.Y, targetPos.X - actor.Position.X);
            foreach (GameClient client in _clientsInSameWorld(actor))
            {
                client.SendMessage(new ACDTranslateFacingMessage
                {
                    Id = 0x70,
                    Field0 = actor.DynamicId,
                    Field1 = facing,
                    Field2 = false
                });
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

        private IEnumerable<GameClient> _clientsInSameWorld(Actor actor)
        {
            return _universe.PlayerManager.Players
                                          .FindAll(p => p.Hero.WorldId == actor.WorldId)
                                          .Select(p => p.Client);
        }

        public Effect SpawnEffectObject(Actor from, int effectId, Vector3D position, float angle = -1f /*random*/, int timeout = 2000)
        {
            if (angle == -1f)
                angle = (float)_rand.NextDouble() * 360f;

            Effect effect = new Effect()
            {
                DynamicId = _universe.NextObjectId,
                Position = position,
                SnoId = effectId,
                Scale = 1.35f,
                RotationAmount = (float)Math.Cos(angle / 2f),
                RotationAxis = new Vector3D(0, 0, (float)Math.Sin(angle / 2f)),
                WorldId = from.WorldId,
                GBHandle = new GBHandle()
                {
                    Field0 = 1,
                    Field1 = 1,
                },
                Field2 = 0x8,
                Field3 = 0x0,
                //Field7 = 0x01,
                //Field8 = effectId,
                // TODO might need more
                timeout = DateTime.Now.AddMilliseconds(timeout)
            };
            
            foreach (GameClient client in _clientsInSameWorld(from))
            {
                effect.Reveal(client.Player.Hero);

                client.SendMessage(new AffixMessage()
                {
                    Id = 0x48,
                    Field0 = effect.DynamicId,
                    Field1 = 0x1,
                    aAffixGBIDs = new int[0]
                });
                client.SendMessage(new AffixMessage()
                {
                    Id = 0x48,
                    Field0 = effect.DynamicId,
                    Field1 = 0x2,
                    aAffixGBIDs = new int[0]
                });
                client.SendMessage(new ACDCollFlagsMessage
                {
                    Id = 0xa6,
                    Field0 = effect.DynamicId,
                    Field1 = 0x1
                });

                // removed attribute set code
                /////////////////////////////

                client.SendMessage(new ACDGroupMessage
                {
                    Id = 0xb8,
                    Field0 = effect.DynamicId,
                    Field1 = unchecked((int)0xb59b8de4),
                    Field2 = unchecked((int)0xffffffff)
                });

                client.SendMessage(new ANNDataMessage
                {
                    Id = 0x3e,
                    Field0 = effect.DynamicId
                });

                client.SendMessage(new SetIdleAnimationMessage
                {
                    Id = 0xa5,
                    Field0 = effect.DynamicId,
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

        public void SpawnEffect(Actor from, int effectId, Vector3D position, float angle = -1f /*random*/, int timeout = 2000)
        {
            _effects.Add(SpawnEffectObject(from, effectId, position, angle, timeout));
        }

        public void KillSpawnedEffect(Effect effect)
        {
            foreach (GameClient client in _clientsInSameWorld(effect))
            {
                client.SendMessage(new ANNDataMessage()
                {
                    Id = 0x3c,
                    Field0 = effect.DynamicId,
                });

                client.PacketId += 10 * 2;
                client.SendMessage(new DWordDataMessage()
                {
                    Id = 0x89,
                    Field0 = client.PacketId,
                });
                client.FlushOutgoingBuffer();
            }
        }

        public IList<Actor> FindActorsInRadius(Vector3D center, float radius, int maxCount = -1)
        {
            List<Actor> hits = new List<Actor>();
            foreach (Actor actor in Targets)
            {
                if (hits.Count == maxCount)
                    break;

                if (Math.Abs(actor.Position.X - center.X) < radius &&
                    Math.Abs(actor.Position.Y - center.Y) < radius &&
                    Math.Abs(actor.Position.Z - center.Z) < radius)
                {
                    hits.Add(actor);
                }
            }
            return hits;
        }

        public Effect GetProxyEffectFor(Actor from, Vector3D pos)
        {
            if (!_proxies.ContainsKey(from))
            {
                _proxies[from] = SpawnEffectObject(from, 187359, pos);
            }
            else
            {
                RepositionActor(_proxies[from], pos);
            }

            return _proxies[from];
        }

        public void RepositionActor(Actor from, Vector3D pos)
        {
            from.Position = pos;
            foreach (GameClient client in _clientsInSameWorld(from))
            {
                client.SendMessage(new ACDWorldPositionMessage()
                {
                    Id = 0x003f,
                    Field0 = from.DynamicId,
                    Field1 = new WorldLocationMessageData()
                    {
                        Field0 = from.Scale,
                        Field1 = new PRTransform()
                        {
                            Field0 = new Quaternion()
                            {
                                Amount = from.RotationAmount,
                                Axis = from.RotationAxis
                            },
                            ReferencePoint = pos
                        },
                        Field2 = from.WorldId,
                    }
                });
            }
        }

        public void MoveActorNormal(Actor actor, Vector3D pos)
        {
            actor.Position = pos;
            foreach (GameClient client in _clientsInSameWorld(actor))
            {
                client.SendMessage(new ACDTranslateNormalMessage()
                {
                    Id = 0x6e,
                    Field0 = actor.DynamicId,
                    Position = pos,
                    Field2 = 0f, // TODO: convert quaternion rotation for this?
                    Field3 = false,
                    Field4 = 1.0f,
                });
            }
        }
        public void DoDamage(Actor from, Actor target, float amount, int type)
        {
            foreach (GameClient client in _clientsInSameWorld(target))
            {
                client.SendMessage(new FloatingNumberMessage()
                {
                    Id = 0xd0,
                    Field0 = target.DynamicId,
                    Field1 = amount,
                    Field2 = type,
                });
                SendDWordTick(client);
                client.FlushOutgoingBuffer();
            }

            // TODO: handling more damagable types
            if (target is SimpleMob)
                ((SimpleMob)target).ReceiveDamage(from, amount, type);
        }

        public void DoDamage(Actor from, IList<Actor> target_list, float amount, int type)
        {
            foreach (Actor target in target_list)
            {
                foreach (GameClient client in _clientsInSameWorld(target))
                {
                    client.SendMessage(new FloatingNumberMessage()
                    {
                        Id = 0xd0,
                        Field0 = target.DynamicId,
                        Field1 = amount,
                        Field2 = type,
                    });
                    SendDWordTick(client);
                    client.FlushOutgoingBuffer();
                }

                // TODO: handling more damagable types
                if (target is SimpleMob)
                    ((SimpleMob)target).ReceiveDamage(from, amount, type);
            }
        }

        public Actor GetActorFromId(int id)
        {
            return Targets.FirstOrDefault(t => t.DynamicId == id);
        }

        public void SendDWordTick(GameClient client)
        {
            client.PacketId += 10 * 2;
            client.SendMessage(new DWordDataMessage()
            {
                Id = 0x89,
                Field0 = client.PacketId,
            });
        }

        public void AddChannelingActor(Actor actor)
        {
            if (!_channelingActors.Contains(actor))
                _channelingActors.Add(actor);
        }
    }
}
