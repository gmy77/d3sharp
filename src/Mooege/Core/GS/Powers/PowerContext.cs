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
using Mooege.Core.GS.Common.Types.Misc;
using Mooege.Core.GS.Ticker;
using Mooege.Core.GS.Common.Types.TagMap;
using Mooege.Common.Logging;

namespace Mooege.Core.GS.Powers
{
    public class PowerContext
    {
        public static readonly Logger Logger = LogManager.CreateLogger();

        private static ThreadLocal<Random> _threadRand = new ThreadLocal<Random>(() => new Random());
        public static Random Rand { get { return _threadRand.Value; } }

        public int PowerSNO;
        public World World;
        public Actor User;
        public Actor Target;

        // helper variables
        private TickTimer _defaultEffectTimeout;

        public void SetDefaultEffectTimeout(TickTimer timeout)
        {
            _defaultEffectTimeout = timeout;
        }

        public TickTimer WaitSeconds(float seconds)
        {
            return new SecondsTickTimer(World.Game, seconds);
        }

        public TickTimer WaitTicks(int ticks)
        {
            return new RelativeTickTimer(World.Game, ticks);
        }

        public TickTimer WaitInfinite()
        {
            return new TickTimer(World.Game, int.MaxValue);
        }

        public void StartCooldown(TickTimer timeout)
        {
            if (User is Player)
            {
                User.Attributes[GameAttribute.Power_Cooldown_Start, PowerSNO] = World.Game.TickCounter;
                User.Attributes[GameAttribute.Power_Cooldown, PowerSNO] = timeout.TimeoutTick;
                User.Attributes.BroadcastChangedIfRevealed();
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

            if (User is Player)
            {
                (User as Player).InGameClient.SendMessage(new FloatingNumberMessage
                {
                    ActorID = target.DynamicID,
                    Number = damageAmount,
                    Type = FloatingNumberMessage.FloatType.White
                });
            }

            if (!hitEffectOverridden)
                target.PlayHitEffect((int)damageType.HitEffect, User);

            // Update hp, kill if Monster and 0hp
            float new_hp = Math.Max(target.Attributes[GameAttribute.Hitpoints_Cur] - damageAmount, 0f);
            target.Attributes[GameAttribute.Hitpoints_Cur] = new_hp;
            target.Attributes.BroadcastChangedIfRevealed();

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
            if (angle == -1)
                angle = (float)(Rand.NextDouble() * (Math.PI * 2));
            if (timeout == null)
            {
                if (_defaultEffectTimeout == null)
                    _defaultEffectTimeout = new SecondsTickTimer(World.Game, 2f); // default timeout of 2 seconds for now

                timeout = _defaultEffectTimeout;
            }

            var actor = new EffectActor(this, actorSNO, position);
            actor.Timeout = timeout;
            actor.Spawn(angle);
            return actor;
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

        public IList<Actor> GetEnemiesInRange(Vector3D center, float range, int maxCount = -1)
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

        public void Knockback(Actor target, Vector3D from, float amount)
        {
            if (target == null) return;

            var move = PowerMath.ProjectAndTranslate2D(from, target.Position, target.Position, amount);
            target.TranslateNormal(move, 1f);
        }

        public void Knockback(Actor target, float amount)
        {
            Knockback(target, User.Position, amount);
        }

        public bool ValidTarget(Actor target)
        {
            return Target != null && Target.World != null; // TODO: check if world is same as powers?
        }

        public bool ValidTarget()
        {
            return ValidTarget(Target);
        }

        public float ScriptFormula(int index)
        {
            float result;
            if (!PowerFormulaScript.Evaluate(this.PowerSNO, PowerFormulaScript.GenerateTagForScriptFormula(index),
                                             User.Attributes, Rand, out result))
                return 0;

            return result;
        }

        public int EvalTag(TagKeyInt key)
        {
            TagMap tagmap = _FindTagMapWithKey(key);
            if (tagmap != null)
                return tagmap[key];
            else
                return 0;
        }

        public int EvalTag(TagKeySNO key)
        {
            TagMap tagmap = _FindTagMapWithKey(key);
            if (tagmap != null)
                return tagmap[key].Id;
            else
                return -1;
        }

        public float EvalTag(TagKeyFloat key)
        {
            TagMap tagmap = _FindTagMapWithKey(key);
            if (tagmap != null)
                return tagmap[key];
            else
                return 0;
        }

        public float EvalTag(TagKeyScript key)
        {
            float result;
            if (!PowerFormulaScript.Evaluate(this.PowerSNO, key,
                                             User.Attributes, Rand, out result))
                return 0;

            return result;
        }

        private TagMap _FindTagMapWithKey(TagKey key)
        {
            TagMap tagmap = PowerTag.FindTagMapWithKey(PowerSNO, key);
            if (tagmap != null)
                return tagmap;
            else
            {
                Logger.Error("could not find tag key {0} in power {1}", key.ID, PowerSNO);
                return null;
            }
        }

        public int Rune_A { get { return User.Attributes[GameAttribute.Rune_A, PowerSNO]; } }
        public int Rune_B { get { return User.Attributes[GameAttribute.Rune_B, PowerSNO]; } }
        public int Rune_C { get { return User.Attributes[GameAttribute.Rune_C, PowerSNO]; } }
        public int Rune_D { get { return User.Attributes[GameAttribute.Rune_D, PowerSNO]; } }
        public int Rune_E { get { return User.Attributes[GameAttribute.Rune_E, PowerSNO]; } }

        public T RuneSelect<T>(T none, T runeA, T runeB, T runeC, T runeD, T runeE)
        {
            if (Rune_A > 0)
                return runeA;
            else if (Rune_B > 0)
                return runeB;
            else if (Rune_C > 0)
                return runeC;
            else if (Rune_D > 0)
                return runeD;
            else if (Rune_E > 0)
                return runeE;
            else
                return none;
        }

        public bool AddBuff(Actor target, Buff buff)
        {
            return target.World.BuffManager.AddBuff(User, target, buff);
        }
    }
}
