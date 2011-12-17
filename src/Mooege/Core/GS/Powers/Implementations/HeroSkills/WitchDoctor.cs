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

using System.Collections.Generic;
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
            int numProjectiles = Rune_B > 0 ? (int)ScriptFormula(4) : 1;
            for (int n = 0; n < numProjectiles; ++n)
            {
                var proj = new Projectile(this,
                                          RuneSelect(107011, 107030, 107035, 107223, 107265, 107114),
                                          User.Position);
                proj.Position.Z += 3f;
                proj.OnCollision = (hit) =>
                {
                    // TODO: fix positioning of hit actors. possibly increase model scale?
                    SpawnEffect(RuneSelect(112327, 112338, 112327, 112345, 112347, 112311), proj.Position);

                    proj.Destroy();

                    if (Rune_E > 0 && Rand.NextDouble() < ScriptFormula(11))
                        hit.PlayEffectGroup(107163);

                    if (Rune_A > 0)
                        WeaponDamage(hit, ScriptFormula(2), DamageType.Fire);
                    else
                        WeaponDamage(hit, ScriptFormula(0), DamageType.Poison);
                };
                proj.Launch(TargetPosition, 1f);

                if (Rune_B > 0)
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

            Vector3D inFrontOfTarget = PowerMath.TranslateDirection2D(TargetPosition, User.Position, TargetPosition, 11f);
            inFrontOfTarget.Z = User.Position.Z;
            var garggy = SpawnEffect(122305, inFrontOfTarget, TargetPosition, WaitInfinite());

            garggy.PlayActionAnimation(155988);

            yield return WaitSeconds(2f);

            for (int n = 0; n < 3; ++n)
            {
                garggy.PlayActionAnimation(211382);

                yield return WaitSeconds(0.5f);

                SpawnEffect(192210, TargetPosition);
                WeaponDamage(GetEnemiesInRadius(TargetPosition, 12f), 1.00f, DamageType.Poison);

                yield return WaitSeconds(0.4f);
            }

            garggy.PlayActionAnimation(155536); //mwhaha
            yield return WaitSeconds(1.5f);

            garggy.PlayActionAnimation(171024);
            yield return WaitSeconds(2f);

            garggy.Destroy();

            yield break;       
        }
    }

    [ImplementsPowerSNO(Skills.Skills.WitchDoctor.PhysicalRealm.PlagueOfToads)]
    public class WitchDoctorPlagueOfToads : PowerScript
    {
        public override IEnumerable<TickTimer> Run()
        {
            // NOTE: not normal plague of toads right now but Obsidian runed "Toad of Hugeness"

            Vector3D userCastPosition = new Vector3D(User.Position);
            Vector3D inFrontOfUser = PowerMath.TranslateDirection2D(User.Position, TargetPosition, User.Position, 7f);
            var bigtoad = SpawnEffect(109906, inFrontOfUser, TargetPosition, WaitInfinite());
            
            // HACK: holy hell there is alot of hardcoded animation timings here
            
            bigtoad.PlayActionAnimation(110766); // spawn ani
            yield return WaitSeconds(1f);

            bigtoad.PlayActionAnimation(110520); // attack ani
            TickTimer waitAttackEnd = WaitSeconds(1.5f);
            yield return WaitSeconds(0.3f); // wait for attack ani to play a bit

            var tongueEnd = SpawnProxy(TargetPosition, WaitInfinite());
            bigtoad.AddRopeEffect(107892, tongueEnd);

            yield return WaitSeconds(0.3f); // have tongue hang there for a bit
            
            var tongueMover = new Implementations.KnockbackBuff(-0.01f, 3f, -0.1f);
            this.World.BuffManager.AddBuff(bigtoad, tongueEnd, tongueMover);
            if (ValidTarget())
                this.World.BuffManager.AddBuff(bigtoad, Target, new Implementations.KnockbackBuff(-0.01f, 3f, -0.1f));

            yield return tongueMover.ArrivalTime;
            tongueEnd.Destroy();

            if (ValidTarget())
            {
                _SetHiddenAttribute(Target, true);

                if (!waitAttackEnd.TimedOut)
                    yield return waitAttackEnd;

                bigtoad.PlayActionAnimation(110636); // disgest ani, 5 seconds
                for (int n = 0; n < 5 && ValidTarget(); ++n)
                {
                    WeaponDamage(Target, 0.039f, DamageType.Poison);
                    yield return WaitSeconds(1f);
                }

                if (ValidTarget())
                {
                    _SetHiddenAttribute(Target, false);

                    bigtoad.PlayActionAnimation(110637); // regurgitate ani
                    this.World.BuffManager.AddBuff(bigtoad, Target, new Implementations.KnockbackBuff(36f));
                    Target.PlayEffectGroup(18281); // actual regurgitate efg isn't working so use generic acid effect
                    yield return WaitSeconds(0.9f);
                }
            }

            bigtoad.PlayActionAnimation(110764); // despawn ani
            yield return WaitSeconds(0.7f);
            bigtoad.Destroy();
        }
        
        private void _SetHiddenAttribute(Actor actor, bool active)
        {
            actor.Attributes[GameAttribute.Hidden] = active;
            actor.Attributes.BroadcastChangedIfRevealed();
        }
    }
}
