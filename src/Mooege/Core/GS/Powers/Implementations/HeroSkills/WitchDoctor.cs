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
using Mooege.Net.GS.Message.Definitions.Animation;
using Mooege.Net.GS.Message.Fields;
using Mooege.Core.GS.Actors;
using Mooege.Core.GS.Common.Types.Math;
using Mooege.Net.GS.Message;
using Mooege.Core.GS.Ticker;

namespace Mooege.Core.GS.Powers.Implementations
{
    [ImplementsPowerSNO(Skills.Skills.WitchDoctor.PhysicalRealm.PoisonDart)]
    public class WitchDoctorPoisonDart : PowerScript
    {
        public override IEnumerable<TickTimer> Run()
        {
            int numProjectiles = (int)ScriptFormula(4);
            for (int n = 0; n < numProjectiles; ++n)
            {
                var proj = new Projectile(this,
                                          RuneSelect(107011, 107030, 107035, 107223, 107265, 107114),
                                          User.Position);
                proj.Position.Z += 3f;
                proj.OnHit = (hit) =>
                {
                    // TODO: fix positioning of hit actors. possibly increase model scale?
                    SpawnEffect(RuneSelect(112327, 112338, 112327, 112345, 112347, 112311), proj.Position);

                    proj.Destroy();

                    if (Rune_E > 0 && Rand.NextDouble() < ScriptFormula(11))
                        hit.PlayEffectGroup(107163);

                    WeaponDamage(hit, (ScriptFormula(13) + 0.16f * Rune_A), (Rune_A > 0 ? DamageType.Fire : DamageType.Poison));
                };
                proj.Launch(TargetPosition, 1f);

                yield return WaitSeconds(ScriptFormula(17));
            }
        }
    }

    [ImplementsPowerSNO(Skills.Skills.WitchDoctor.PhysicalRealm.ZombieCharger)]
    public class WitchDoctorZombieCharger : PowerScript
    {
        public override IEnumerable<TickTimer> Run()
        {
            // HACK: made up garggy spell :)

            Vector3D inFrontOfTarget = PowerMath.ProjectAndTranslate2D(TargetPosition, User.Position, TargetPosition, 11f);
            inFrontOfTarget.Z = User.Position.Z;
            var garggy = SpawnEffect(122305, inFrontOfTarget, TargetPosition, WaitInfinite());

            _PlayAni(garggy, 155988);

            yield return WaitSeconds(2f);

            for (int n = 0; n < 3; ++n)
            {
                _PlayAni(garggy, 211382);

                yield return WaitSeconds(0.5f);

                SpawnEffect(192210, TargetPosition);
                WeaponDamage(GetEnemiesInRange(TargetPosition, 12f), 1.00f, DamageType.Poison);

                yield return WaitSeconds(0.4f);
            }

            _PlayAni(garggy, 155536); //mwhaha
            yield return WaitSeconds(1.5f);

            _PlayAni(garggy, 171024);
            yield return WaitSeconds(2f);

            garggy.Destroy();

            yield break;       
        }

        // hackish animation player until a centralized one can be made
        private void _PlayAni(Actor actor, int aniSNO)
        {
            World.BroadcastIfRevealed(new PlayAnimationMessage()
            {
                ActorID = actor.DynamicID,
                Field1 = 0x3,
                Field2 = 0,
                tAnim = new PlayAnimationMessageSpec[1]
                {
                    new PlayAnimationMessageSpec()
                    {
                        Field0 = -2,
                        Field1 = aniSNO,
                        Field2 = 0x0,
                        Field3 = 1f
                    }
                }
            }, actor);
        }
    }

    [ImplementsPowerSNO(Skills.Skills.WitchDoctor.PhysicalRealm.PlagueOfToads)]
    public class WitchDoctorPlagueOfToads : PowerScript
    {
        public override IEnumerable<TickTimer> Run()
        {
            // NOTE: not normal plague of toads right now but Obsidian runed "Toad of Hugeness"

            Vector3D userCastPosition = new Vector3D(User.Position);
            Vector3D inFrontOfUser = PowerMath.ProjectAndTranslate2D(User.Position, TargetPosition, User.Position, 7f);
            var bigtoad = SpawnEffect(109906, inFrontOfUser, TargetPosition, WaitInfinite());
            
            // HACK: holy hell there is alot of hardcoded animation timings here
            
            _PlayAni(bigtoad, 110766); // spawn ani
            yield return WaitSeconds(1f);

            _PlayAni(bigtoad, 110520); // attack ani
            TickTimer waitAttackEnd = WaitSeconds(1.5f);
            yield return WaitSeconds(0.3f); // wait for attack ani to play a bit

            var tongueEnd = SpawnProxy(TargetPosition, WaitInfinite());
            bigtoad.AddRopeEffect(107892, tongueEnd);

            // calculate time it will take for actors to reach toad
            const float tongueSpeed = 4f;
            int waitMoveTicks = (int)(PowerMath.Distance(bigtoad.Position, TargetPosition) / tongueSpeed);
            
            yield return WaitSeconds(0.3f); // have tongue hang there for a bit
            
            tongueEnd.TranslateNormal(bigtoad.Position, tongueSpeed);
            if (ValidTarget())
                Target.TranslateNormal(bigtoad.Position, tongueSpeed);

            yield return WaitTicks(waitMoveTicks);
            tongueEnd.Destroy();

            if (ValidTarget())
            {
                _SetHiddenAttribute(Target, true);

                if (!waitAttackEnd.TimedOut)
                    yield return waitAttackEnd;

                _PlayAni(bigtoad, 110636); // disgest ani, 5 seconds
                for (int n = 0; n < 5 && ValidTarget(); ++n)
                {
                    WeaponDamage(Target, 0.39f, DamageType.Poison, true);
                    yield return WaitSeconds(1f);
                }

                if (ValidTarget())
                {
                    _SetHiddenAttribute(Target, false);

                    _PlayAni(bigtoad, 110637); // regurgitate ani
                    Knockback(Target, userCastPosition, 6f);
                    Target.PlayEffectGroup(18281); // actual regurgitate efg isn't working so use generic acid effect
                    yield return WaitSeconds(0.9f);
                }
            }

            _PlayAni(bigtoad, 110764); // despawn ani
            yield return WaitSeconds(0.7f);
            bigtoad.Destroy();
        }
        
        private void _SetHiddenAttribute(Actor actor, bool active)
        {
            actor.Attributes[GameAttribute.Hidden] = active;
            foreach (var msg in actor.Attributes.GetChangedMessageList(actor.DynamicID))
                World.BroadcastIfRevealed(msg, actor);
        }

        // hackish animation player until a centralized one can be made
        private void _PlayAni(Actor actor, int aniSNO)
        {
            World.BroadcastIfRevealed(new PlayAnimationMessage()
            {
                ActorID = actor.DynamicID,
                Field1 = 0x3,
                Field2 = 0,
                tAnim = new PlayAnimationMessageSpec[1]
                {
                    new PlayAnimationMessageSpec()
                    {
                        Field0 = -2,
                        Field1 = aniSNO,
                        Field2 = 0x0,
                        Field3 = 1f
                    }
                }
            }, actor);
        }
    }
}
