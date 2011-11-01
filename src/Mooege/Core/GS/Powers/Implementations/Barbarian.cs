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
using Mooege.Core.GS.Actors;
using Mooege.Core.GS.Actors.Buffs;
using Mooege.Core.GS.Common.Types.Math;
using Mooege.Net.GS.Message.Definitions.ACD;

namespace Mooege.Core.GS.Powers.Implementations
{
    [ImplementsPowerSNO(Skills.Skills.Barbarian.FuryGenerators.Bash)]
    public class BarbarianBash : ContinuableEffect
    {
        public override IEnumerable<TickTimer> Continue()
        {
            yield return WaitSeconds(0.25f); // wait for swing animation

            User.PlayEffectGroup(18662);

            if (CanHitMeleeTarget(Target))
            {
                GeneratePrimaryResource(6f);
                
                Knockback(Target, 4f);
                Damage(Target, 35, 0);
            }

            yield break;
        }
    }

    [ImplementsPowerSNO(Skills.Skills.Barbarian.FuryGenerators.LeapAttack)]
    public class BarbarianLeap : ContinuableEffect
    {
        public override IEnumerable<TickTimer> Continue()
        {
            //StartCooldown(WaitSeconds(10f));

            Vector3D delta = new Vector3D(TargetPosition.X - User.Position.X, TargetPosition.Y - User.Position.Y,
                                          TargetPosition.Z - User.Position.Z);
            float delta_length = (float)Math.Sqrt(delta.X * delta.X + delta.Y * delta.Y);
            Vector3D delta_normal = new Vector3D(delta.X / delta_length, delta.Y / delta_length, delta.Z / delta_length);
            float unitsMovedPerTick = 30f;
            Vector3D ramp = new Vector3D(delta_normal.X * (delta_length / unitsMovedPerTick),
                                         delta_normal.Y * (delta_length / unitsMovedPerTick),
                                         1.48324f); // usual leap height, possibly different when jumping up/down?

            // TODO: Generalize this and put it in Actor
            User.World.BroadcastIfRevealed(new ACDTranslateArcMessage()
            {
                Id = 114,
                ActorId = (int)User.DynamicID,
                Start = User.Position,
                Velocity = ramp,
                Field3 = 303110, // used for male barb leap
                FlyingAnimationTagID = 69792, // used for male barb leap
                LandingAnimationTagID = -1,
                Field6 = -0.1f, // leap falloff
                Field7 = Skills.Skills.Barbarian.FuryGenerators.LeapAttack,
                Field8 = 0
            }, User);
            User.Position.Set(TargetPosition);

            // wait for leap to hit
            yield return WaitSeconds(0.65f);

            User.PlayEffectGroup(18688);

            bool hitAnything = false;
            foreach (Actor actor in GetTargetsInRange(TargetPosition, 8f))
            {
                hitAnything = true;
                actor.PlayHitEffect(0, User);
                Damage(actor, 55, 0);
            }

            if (hitAnything)
                GeneratePrimaryResource(15f);

            yield break;
        }
    }

    [ImplementsPowerSNO(Skills.Skills.Barbarian.FurySpenders.Whirlwind)]
    public class BarbarianWhirlwind : ContinuableEffect
    {
        public override IEnumerable<TickTimer> Continue()
        {
            User.AddBuff(new WhirlWindEffectBuff(WaitSeconds(0.250f)));

            foreach (Actor target in GetTargetsInRange(User.Position, 9f))
            {
                target.PlayHitEffect(0, User);
                Damage(target, 10, 0);
            }

            yield break;
        }
    }

    [ImplementsPowerSNO(Skills.Skills.Barbarian.FuryGenerators.AncientSpear)]
    public class BarbarianAncientSpear : ContinuableEffect
    {
        public override IEnumerable<TickTimer> Continue()
        {
            //StartCooldown(WaitSeconds(10f));
            
            var projectile = new PowerProjectile(User.World, 74636, User.Position, TargetPosition, 2f, 500f, 1f, 3f, 5f, 0f);

            User.AddRopeEffect(79402, projectile);

            projectile.OnHit = () =>
            {
                GeneratePrimaryResource(20f);

                var inFrontOfUser = PowerMath.ProjectAndTranslate2D(User.Position, projectile.hittedActor.Position,
                    User.Position, 5f);

                _setupReturnProjectile(projectile.hittedActor.Position, 5f);

                // GET OVER HERE
                projectile.hittedActor.MoveNormal(inFrontOfUser, 2f);
                projectile.hittedActor.PlayHitEffect(0, User);
                Damage(projectile.hittedActor, 15f, 0);

                projectile.Destroy();
            };

            projectile.OnTimeout = () =>
            {
                _setupReturnProjectile(projectile.getCurrentPosition(), 0f);
            };

            yield break;
        }

        private void _setupReturnProjectile(Vector3D spawnPosition, float heightOffset)
        {
            var return_proj = new PowerProjectile(User.World, 79400, spawnPosition,
                User.Position, 2f, 500f, 1f, 3f, heightOffset, 0f);

            User.AddRopeEffect(79402, return_proj);

            return_proj.OnUpdate = () =>
            {
                if (PowerMath.Distance(return_proj.getCurrentPosition(), User.Position) < 15f) // TODO: make this tick based distance?
                {
                    return_proj.Destroy();
                    return false;
                }
                return true;
            };
        }
    }
}
