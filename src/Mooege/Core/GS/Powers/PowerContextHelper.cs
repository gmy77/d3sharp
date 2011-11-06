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
using System.Collections.Generic;
using System.Threading;
using Mooege.Core.GS.Actors;
using Mooege.Core.GS.Common.Types.Math;
using Mooege.Core.GS.Map;
using Mooege.Core.GS.Players;
using Mooege.Net.GS.Message;
using Mooege.Net.GS.Message.Definitions.Misc;
using Mooege.Net.GS.Message.Definitions.World;
using Mooege.Core.GS.Common.Types.Misc;
using Mooege.Core.GS.Ticker.Helpers;

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
                GameAttributeMap map = User.Attributes;
                map[GameAttribute.Power_Cooldown_Start, PowerSNO] = World.Game.TickCounter;
                map[GameAttribute.Power_Cooldown, PowerSNO] = timeout.TimeoutTick;
                map.SendChangedMessage((User as Player).InGameClient, User.DynamicID);
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
        
        public void WeaponDamage(Actor target, float percentage, DamageType damageType, bool hitEffectOverridden = false)
        {
            if (target == null || target.World == null) return;

            // TODO: this all needs to really be forwarded to a real combat system
            // just use hardcoded weapon damage range for now
            float damageAmount = Rand.Next(20, 35) * percentage;

            World.BroadcastIfRevealed(new FloatingNumberMessage
            {
                ActorID = target.DynamicID,
                Number = damageAmount,
                Type = FloatingNumberMessage.FloatType.White
            }, target);

            if (!hitEffectOverridden)
                target.PlayHitEffect((int)damageType.HitEffect, User);

            // Update hp, kill if Monster and 0hp
            float new_hp = Math.Max(target.Attributes[GameAttribute.Hitpoints_Cur] - damageAmount, 0f);
            target.Attributes[GameAttribute.Hitpoints_Cur] = new_hp;
            foreach (var msg in target.Attributes.GetChangedMessageList(target.DynamicID))
                World.BroadcastIfRevealed(msg, target);

            if (new_hp == 0f && target is Monster && User is Player)
                (target as Monster).Die(User as Player);
        }

        public void WeaponDamage(IList<Actor> target_list, float percentage, DamageType damageType, bool hitEffectOverridden = false)
        {
            foreach (Actor target in target_list)
            {
                WeaponDamage(target, percentage, damageType);
            }
        }

        public EffectActor SpawnEffect(int actorSNO, Vector3D position, float angle = 0, TickTimer timeout = null)
        {
            if (angle == -1f)
                angle = (float)(Rand.NextDouble() * (Math.PI * 2));
            if (timeout == null)
            {
                if (_defaultEffectTimeout == null)
                    _defaultEffectTimeout = new TickSecondsTimer(World.Game, 2f); // default timeout of 2 seconds for now

                timeout = _defaultEffectTimeout;
            }

            return new EffectActor(World, actorSNO, position, angle, timeout);
        }

        public EffectActor SpawnEffect(int actorSNO, Vector3D position, Actor facingTarget, TickTimer timeout = null)
        {
            float angle = (facingTarget != null) ? PowerMath.AngleLookAt(User.Position, facingTarget.Position) : -1f;
            return SpawnEffect(actorSNO, position, angle, timeout);
        }

        public EffectActor SpawnEffect(int actorSNO, Vector3D position, Vector3D facingTarget, TickTimer timeout = null)
        {
            float angle = PowerMath.AngleLookAt(User.Position, facingTarget);
            return SpawnEffect(actorSNO, position, angle, timeout);
        }

        public EffectActor SpawnProxy(Vector3D position, TickTimer timeout = null)
        {
            return SpawnEffect(187359, position, 0, timeout);
        }

        public IList<Actor> GetTargetsInRange(Vector3D center, float range, int maxCount = -1)
        {
            List<Actor> hits = new List<Actor>();
            foreach (Actor actor in World.QuadTree.Query<Actor>(new Circle(center.X, center.Y, range)))
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
            target.TranslateNormal(move, 1f);
        }
    }
}
