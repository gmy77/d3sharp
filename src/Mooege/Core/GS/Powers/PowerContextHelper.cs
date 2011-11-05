using System;
using System.Collections.Generic;
using System.Threading;
using Mooege.Core.GS.Actors;
using Mooege.Core.GS.Common.Types;
using Mooege.Core.GS.Common.Types.Math;
using Mooege.Core.GS.Map;
using Mooege.Core.GS.Players;
using Mooege.Net.GS.Message;
using Mooege.Net.GS.Message.Definitions.Misc;
using Mooege.Net.GS.Message.Definitions.World;

namespace Mooege.Core.GS.Powers
{
    public class PowerContextHelper
    {
        private static ThreadLocal<Random> _threadRand = new ThreadLocal<Random>(() => new Random());
        public static Random Rand { get { return _threadRand.Value; } }

        public PowerManager PowerManager;
        public int PowerSNO;
        public World World;
        public Actor User;
        public Actor Target;
        public Vector3D TargetPosition;
        public float TargetZ;
        public TargetMessage Message;

        // helper variables
        private TickTimer _defaultEffectTimeout;

        public void SetDefaultEffectTimeout(TickTimer timeout)
        {
            _defaultEffectTimeout = timeout;
        }

        public TickTimer WaitSeconds(float seconds)
        {
            return new TickSecondsTimer(World.Game, seconds);
        }

        public TickTimer WaitTicks(int ticks)
        {
            return new TickRelativeTimer(World.Game, ticks);
        }

        public TickTimer WaitInfinite()
        {
            return new TickTimer(World.Game, int.MaxValue);
        }

        public void StartCooldown(TickTimer timeout)
        {
            if (User is Player)
            {
                // TODO: update User.Attribute instead of creating temp map
                GameAttributeMap map = new GameAttributeMap();
                map[GameAttribute.Power_Cooldown_Start, PowerSNO] = World.Game.Tick;
                map[GameAttribute.Power_Cooldown, PowerSNO] = timeout.TimeoutTick;
                map.SendMessage((User as Player).InGameClient, User.DynamicID);
            }
        }

        public void GeneratePrimaryResource(float amount)
        {
            if (User is Player)
            {
                (User as Player).GeneratePrimaryResource(amount);
            }
        }

        public void UsePrimaryResource(float amount)
        {
            if (User is Player)
            {
                (User as Player).UsePrimaryResource(amount);
            }
        }

        public void GenerateSecondaryResource(float amount)
        {
            if (User is Player)
            {
                (User as Player).GenerateSecondaryResource(amount);
            }
        }

        public void UseSecondaryResource(float amount)
        {
            if (User is Player)
            {
                (User as Player).UseSecondaryResource(amount);
            }
        }
        
        public void Damage(Actor target, float amount, int type)
        {
            if (target == null || target.World == null) return;

            World.BroadcastIfRevealed(new FloatingNumberMessage
            {
                ActorID = target.DynamicID,
                Number = amount,
                Type = FloatingNumberMessage.FloatType.White//type,
            }, target);

            // Update hp, kill if Monster and 0hp
            float new_hp = Math.Max(target.Attributes[GameAttribute.Hitpoints_Cur] - amount, 0f);
            target.Attributes[GameAttribute.Hitpoints_Cur] = new_hp;
            GameAttributeMap map = new GameAttributeMap();
            map[GameAttribute.Hitpoints_Cur] = new_hp;
            foreach (var msg in map.GetMessageList(target.DynamicID))
                World.BroadcastIfRevealed(msg, target);

            if (new_hp == 0f && target is Monster && User is Player)
                (target as Monster).Die(User as Player);
        }

        public void Damage(IList<Actor> target_list, float amount, int type)
        {
            foreach (Actor target in target_list)
            {
                Damage(target, amount, type);
            }
        }

        public ClientEffect SpawnEffect(int actorSNO, Vector3D position, float angle = 0, TickTimer timeout = null)
        {
            if (angle == -1f)
                angle = (float)(Rand.NextDouble() * (Math.PI * 2));
            if (timeout == null)
            {
                if (_defaultEffectTimeout == null)
                    _defaultEffectTimeout = new TickSecondsTimer(World.Game, 2f); // default timeout of 2 seconds for now

                timeout = _defaultEffectTimeout;
            }

            return new ClientEffect(World, actorSNO, position, angle, timeout);
        }

        public ClientEffect SpawnEffect(int actorSNO, Vector3D position, Actor point_to_actor, TickTimer timeout = null)
        {
            float angle = (point_to_actor != null) ? PowerMath.AngleLookAt(User.Position, point_to_actor.Position) : -1f;
            return SpawnEffect(actorSNO, position, angle, timeout);
        }

        public ClientEffect SpawnProxy(Vector3D position, TickTimer timeout = null)
        {
            return SpawnEffect(187359, position, 0, timeout);
        }

        public IList<Actor> GetTargetsInRange(Vector3D center, float range, int maxCount = -1)
        {
            List<Actor> hits = new List<Actor>();
            foreach (Actor actor in World.GetActorsInRange(center, range))
            {
                if (hits.Count == maxCount)
                    break;

                if (actor is Monster)
                    hits.Add(actor);
            }

            return hits;
        }

        public bool CanHitMeleeTarget(Actor target, float range = 13f)
        {
            if (target == null) return false;

            return (Math.Sqrt(
                        Math.Pow(User.Position.X - target.Position.X, 2) +
                        Math.Pow(User.Position.Y - target.Position.Y, 2) +
                        Math.Pow(User.Position.Z - target.Position.Z, 2)) <= range);
        }

        public void Knockback(Actor target, float amount)
        {
            if (target == null) return;

            var move = PowerMath.ProjectAndTranslate2D(User.Position, target.Position, target.Position, amount);
            target.MoveNormal(move);
        }
    }
}
