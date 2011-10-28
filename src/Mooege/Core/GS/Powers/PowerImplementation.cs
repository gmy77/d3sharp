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
using System.Linq;
using System.Text;
using Mooege.Core.GS.Actors;
using Mooege.Net.GS.Message.Fields;
using Mooege.Net.GS.Message.Definitions.World;
using Mooege.Net.GS.Message.Definitions.Misc;
using Mooege.Common;
using Mooege.Net.GS.Message;
using Mooege.Core.GS.Map;
using Mooege.Core.GS.Common.Types.Math;

namespace Mooege.Core.GS.Powers
{
    public abstract class PowerImplementation
    {
        public static readonly Logger Logger = LogManager.CreateLogger();
        public static Random Rand = new Random();
        
        public PowerManager PowerManager;
        public int PowerSNO;
        public World World;
        public Actor User;
        public Actor Target;
        public Vector3D TargetPosition;
        public TargetMessage Message;
        public bool UserIsChanneling;
        public bool ThrottledCast;

        // Called to start executing a power
        // Yields timers that signify when to continue execution.
        public abstract IEnumerable<TickTimer> Run();

        // token instance that can be yielded by Run() to indicate the power manager should stop
        // running a power implementation.
        public static TickTimer StopExecution = new TickTimer(null, 0);

        #region Implementation helpers

        // helper variables
        private TickTimer _defaultEffectTimeout;

        public void SetDefaultEffectTimeout(TickTimer timeout)
        {
            _defaultEffectTimeout = timeout;
        }

        public TickTimer WaitSeconds(float seconds)
        {
            return new TickSecondsTimer(User.World.Game, seconds);
        }

        public TickTimer WaitTicks(int ticks)
        {
            return new TickRelativeTimer(User.World.Game, ticks);
        }

        public void StartCooldown(TickTimer timeout)
        {
            if (User is Player.Player)
            {
                // TODO: update User.Attribute instead of creating temp map
                GameAttributeMap map = new GameAttributeMap();
                map[GameAttribute.Power_Cooldown_Start, PowerSNO] = User.World.Game.Tick;
                map[GameAttribute.Power_Cooldown, PowerSNO] = timeout.TimeoutTick;
                map.SendMessage((User as Player.Player).InGameClient, User.DynamicID);
            }
        }

        public void GeneratePrimaryResource(float amount)
        {
            if (User is Player.Player)
            {
                (User as Player.Player).GeneratePrimaryResource(amount);
            }
        }

        public void UsePrimaryResource(float amount)
        {
            if (User is Player.Player)
            {
                (User as Player.Player).UsePrimaryResource(amount);
            }
        }

        public void GenerateSecondaryResource(float amount)
        {
            if (User is Player.Player)
            {
                (User as Player.Player).GenerateSecondaryResource(amount);
            }
        }

        public void UseSecondaryResource(float amount)
        {
            if (User is Player.Player)
            {
                (User as Player.Player).UseSecondaryResource(amount);
            }
        }
        
        public void RegisterChannelingPower(TickTimer channelCastDelay = null)
        {
            PowerManager.RegisterChannelingPower(channelCastDelay);
        }

        public void Damage(Actor target, float amount, int type)
        {
            if (target == null) return;
            if (target.World == null)
            {
                // WTF is world null sometimes for? Probably race condition due to lack of GS locks
                return;
            }

            target.World.BroadcastIfRevealed(new FloatingNumberMessage
            {
                Id = 0xd0,
                ActorID = target.DynamicID,
                Number = amount,
                Type = FloatingNumberMessage.FloatType.White//type,
            }, target);

            // TODO: handling more damagable types
            if (target is PowersTestMonster)
            {
                ((PowersTestMonster)target).ReceiveDamage(User, amount, type);
            }
            else if (target is Monster && User is Player.Player)
            {
                ((Monster)target).Die((Player.Player)User);
            }
        }

        public void Damage(IList<Actor> target_list, float amount, int type)
        {
            foreach (Actor target in target_list)
            {
                Damage(target, amount, type);
            }
        }

        public EffectActor SpawnEffect(int actorSNO, Vector3D position, float angle = -1f /*random*/, TickTimer timeout = null)
        {
            if (angle == -1f)
                angle = (float)(Rand.NextDouble() * (Math.PI * 2));
            if (timeout == null)
            {
                if (_defaultEffectTimeout == null)
                    _defaultEffectTimeout = new TickSecondsTimer(User.World.Game, 2f); // default timeout of 2 seconds for now

                timeout = _defaultEffectTimeout;
            }

            return new EffectActor(User.World, actorSNO, position, angle, timeout);
        }

        public EffectActor SpawnEffect(int actorSNO, Vector3D position, Actor point_to_actor, TickTimer timeout = null)
        {
            float angle = (point_to_actor != null) ? PowerMath.AngleLookAt(User.Position, point_to_actor.Position) : -1f;
            return SpawnEffect(actorSNO, position, angle, timeout);
        }

        public EffectActor SpawnProxy(Vector3D position, TickTimer timeout = null)
        {
            return SpawnEffect(187359, position, 0, timeout);
        }

        public EffectActor GetChanneledEffect(int index, int actorSNO, Vector3D position, bool snapped = false)
        {
            return PowerManager.GetChanneledEffect(User, index, actorSNO, position, snapped);
        }

        public EffectActor GetChanneledProxy(int index, Vector3D position, bool snapped = false)
        {
            return GetChanneledEffect(index, 187359, position, snapped);
        }

        public IList<Actor> GetTargetsInRange(Vector3D center, float range, int maxCount = -1)
        {
            List<Actor> hits = new List<Actor>();
            foreach (Actor actor in User.World.GetActorsInRange(center, range))
            {
                if (hits.Count == maxCount)
                    break;

                if (actor is PowersTestMonster || actor is Monster)
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

        #endregion

    }
}
