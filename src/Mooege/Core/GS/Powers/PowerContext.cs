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
using Mooege.Net.GS.Message.Definitions.Animation;
using Mooege.Net.GS.Message.Fields;
using Mooege.Core.GS.Powers.Payloads;
using Mooege.Net.GS.Message.Definitions.Actor;
using Mooege.Core.GS.Actors.Movement;
using Mooege.Net.GS.Message.Definitions.ACD;

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
            AddBuff(User, new Implementations.CooldownBuff(PowerSNO, timeout));
        }

        public void StartCooldown(float seconds)
        {
            StartCooldown(WaitSeconds(seconds));
        }

        public void StartDefaultCooldown()
        {
            float seconds = EvalTag(PowerKeys.CooldownTime);
            if (seconds > 0f)
                StartCooldown(seconds);
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

        public void WeaponDamage(Actor target, float damageMultiplier, DamageType damageType)
        {
            AttackPayload payload = new AttackPayload(this);
            payload.SetSingleTarget(target);
            payload.AddWeaponDamage(damageMultiplier, damageType);
            payload.Apply();
        }

        public void WeaponDamage(TargetList targets, float damageMultiplier, DamageType damageType)
        {
            AttackPayload payload = new AttackPayload(this);
            payload.Targets = targets;
            payload.AddWeaponDamage(damageMultiplier, damageType);
            payload.Apply();
        }

        public void Damage(Actor target, float minDamage, float damageDelta, DamageType damageType)
        {
            AttackPayload payload = new AttackPayload(this);
            payload.SetSingleTarget(target);
            payload.AddDamage(minDamage, damageDelta, damageType);
            payload.Apply();
        }

        public void Damage(TargetList targets, float minDamage, float damageDelta, DamageType damageType)
        {
            AttackPayload payload = new AttackPayload(this);
            payload.Targets = targets;
            payload.AddDamage(minDamage, damageDelta, damageType);
            payload.Apply();
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
            float angle = (facingTarget != null) ? MovementHelpers.GetFacingAngle(User.Position, facingTarget.Position) : -1f;
            return SpawnEffect(actorSNO, position, angle, timeout);
        }

        public EffectActor SpawnEffect(int actorSNO, Vector3D position, Vector3D facingTarget, TickTimer timeout = null)
        {
            float angle = MovementHelpers.GetFacingAngle(User.Position, facingTarget);
            return SpawnEffect(actorSNO, position, angle, timeout);
        }

        public EffectActor SpawnProxy(Vector3D position, TickTimer timeout = null)
        {
            return SpawnEffect(187359, position, 0, timeout);
        }

        public TargetList GetEnemiesInRadius(Vector3D center, float radius, int maxCount = -1)
        {
            return _GetTargetsInRadiusHelper(center, radius, maxCount, (actor) => true, _EnemyActorFilter);
        }

        public TargetList GetAlliesInRadius(Vector3D center, float radius, int maxCount = -1)
        {
            return _GetTargetsInRadiusHelper(center, radius, maxCount, (actor) => true, _AllyActorFilter);
        }

        public TargetList GetEnemiesInBeamDirection(Vector3D startPoint, Vector3D direction,
                                                    float length, float thickness = 0f)
        {
            Vector3D beamEnd = PowerMath.TranslateDirection2D(startPoint, direction, startPoint, length);

            float fixedActorRadius = 1.5f;  // TODO: calculate based on actor.ActorData.Cylinder.Ax2 ?
            return _GetTargetsInRadiusHelper(startPoint, length + thickness, -1,
                actor => PowerMath.CircleInBeam(new Circle(actor.Position.X, actor.Position.Y, fixedActorRadius),
                                                startPoint, beamEnd, thickness),
                _EnemyActorFilter);
        }

        public TargetList GetEnemiesInArcDirection(Vector3D center, Vector3D direction, float radius, float lengthDegrees)
        {
            Vector2F arcCenter2D = PowerMath.VectorWithoutZ(center);
            Vector2F arcDirection2D = PowerMath.VectorWithoutZ(direction);
            float arcLength = lengthDegrees * PowerMath.DegreesToRadians;

            float fixedActorRadius = 1.5f;  // TODO: calculate based on actor.ActorData.Cylinder.Ax2 ?
            return _GetTargetsInRadiusHelper(center, radius, -1,
                actor => PowerMath.ArcCircleCollides(arcCenter2D, arcDirection2D, radius, arcLength,
                                                     new Circle(actor.Position.X, actor.Position.Y, fixedActorRadius)),
                _EnemyActorFilter);
        }

        private TargetList _GetTargetsInRadiusHelper(Vector3D center, float radius, int maxCount,
            Func<Actor, bool> filter, Func<Actor, bool> targetFilter)
        {
            // Query() needs to gather using circle-circle collision, until then just extend the search radius by the default
            // actor radius currently used.
            float actorRadiusCompensation = 1.5f;

            TargetList targets = new TargetList();
            int count = 0;
            foreach (Actor actor in World.QuadTree.Query<Actor>(new Circle(center.X, center.Y, radius + actorRadiusCompensation)))
            {
                if (filter(actor) && !actor.Attributes[GameAttribute.Untargetable] && !World.PowerManager.IsDeletingActor(actor) &&
                    actor != User)
                {
                    if (targetFilter(actor))
                    {
                        if (count != maxCount)
                        {
                            targets.Actors.Add(actor);
                            count += 1;
                        }
                    }
                    else
                    {
                        targets.ExtraActors.Add(actor);
                    }
                }
            }

            return targets;
        }

        private Func<Actor, bool> _EnemyActorFilter
        {
            get
            {
                if (User is Player || User is Minion)
                    return (actor) => actor is Monster;
                else
                    return (actor) => actor is Player || actor is Minion;
            }
        }

        private Func<Actor, bool> _AllyActorFilter
        {
            get
            {
                if (User is Player || User is Minion)
                    return (actor) => actor is Player || actor is Minion;
                else
                    return (actor) => actor is Monster;
            }
        }

        public void TranslateEffect(Actor actor, Vector3D destination, float speed)
        {
            actor.Position = destination;

            if (actor.World == null) return;

            actor.World.BroadcastIfRevealed(new ACDTranslateNormalMessage
            {
                ActorId = (int)actor.DynamicID,
                Position = destination,
                Angle = (float)Math.Acos(actor.RotationW) * 2f,  // convert z-axis quat to radians
                TurnImmediately = true,
                Speed = speed,
            }, actor);
        }

        public TickTimer Knockback(Actor target, float magnitude, float arcHeight = 3.0f, float arcGravity = -0.03f)
        {
            var buff = new Implementations.KnockbackBuff(magnitude, arcHeight, arcGravity);
            AddBuff(target, buff);
            return buff.ArrivalTime;
        }

        public TickTimer Knockback(Vector3D from, Actor target, float magnitude, float arcHeight = 3.0f, float arcGravity = -0.03f)
        {
            var buff = new Implementations.KnockbackBuff(magnitude, arcHeight, arcGravity);
            AddBuff(SpawnProxy(from), target, buff);
            return buff.ArrivalTime;
        }

        public static bool ValidTarget(Actor target)
        {
            return target != null && target.World != null;
        }

        public bool ValidTarget()
        {
            return ValidTarget(Target);
        }

        public float ScriptFormula(int index)
        {
            float result;
            if (!ScriptFormulaEvaluator.Evaluate(this.PowerSNO, PowerTagHelper.GenerateTagForScriptFormula(index),
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
                return -1;
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
            if (!ScriptFormulaEvaluator.Evaluate(this.PowerSNO, key,
                                             User.Attributes, Rand, out result))
                return 0;

            return result;
        }

        private TagMap _FindTagMapWithKey(TagKey key)
        {
            TagMap tagmap = PowerTagHelper.FindTagMapWithKey(PowerSNO, key);
            if (tagmap != null)
                return tagmap;
            else
            {
                //Logger.Error("could not find tag key {0} in power {1}", key.ID, PowerSNO);
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
            if (Rune_A > 0) return runeA;
            else if (Rune_B > 0) return runeB;
            else if (Rune_C > 0) return runeC;
            else if (Rune_D > 0) return runeD;
            else if (Rune_E > 0) return runeE;
            else return none;
        }

        public bool AddBuff(Actor target, Buff buff)
        {
            return AddBuff(User, target, buff);
        }

        public bool AddBuff(Actor user, Actor target, Buff buff)
        {
            return target.World.BuffManager.AddBuff(user, target, buff);
        }

        public bool HasBuff<BuffType>(Actor target) where BuffType : Buff
        {
            return target.World.BuffManager.HasBuff<BuffType>(target);
        }

        public Vector3D RandomDirection(Vector3D position, float radius)
        {
            return RandomDirection(position, radius, radius);
        }

        public Vector3D RandomDirection(Vector3D position, float minRadius, float maxRadius)
        {
            float angle = (float)(Rand.NextDouble() * Math.PI * 2);
            float radius = minRadius + (float)Rand.NextDouble() * (maxRadius - minRadius);
            return new Vector3D(position.X + (float)Math.Cos(angle) * radius,
                                position.Y + (float)Math.Sin(angle) * radius,
                                position.Z);
        }
    }
}
