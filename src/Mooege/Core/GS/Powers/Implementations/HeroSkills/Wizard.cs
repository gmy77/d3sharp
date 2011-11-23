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
using Mooege.Core.GS.Actors;
using Mooege.Core.GS.Common.Types.Math;
using Mooege.Net.GS.Message.Definitions.ACD;
using Mooege.Core.GS.Ticker;

namespace Mooege.Core.GS.Powers.Implementations
{
    [ImplementsPowerSNO(Skills.Skills.Wizard.Offensive.Meteor)]
    public class WizardMeteor : PowerScript
    {
        public override IEnumerable<TickTimer> Run()
        {
            UsePrimaryResource(ScriptFormula(8));

            // cast effect
            User.PlayEffectGroup(RuneSelect(71141, 71141, 71141, 92222, 217377, 217461));

            TickTimer waitForImpact = WaitSeconds(ScriptFormula(4));

            List<Vector3D> impactPositions = new List<Vector3D>();
            int meteorCount = Rune_B > 0 ? (int)ScriptFormula(9) : 1;

            // pending effect + meteor
            for (int n = 0; n < meteorCount; ++n)
            {
                Vector3D impactPos;
                if (meteorCount > 1)
                    impactPos = new Vector3D(TargetPosition.X + ((float)Rand.NextDouble() - 0.5f) * 25,
                                             TargetPosition.Y + ((float)Rand.NextDouble() - 0.5f) * 25,
                                             TargetPosition.Z);
                else
                    impactPos = TargetPosition;

                SpawnEffect(RuneSelect(86790, 215853, 91440, 92030, 217142, 217457), impactPos, 0, WaitSeconds(5f));
                impactPositions.Add(impactPos);

                if (meteorCount > 1)
                    yield return WaitSeconds(0.1f);
            }

            // wait for meteor impact(s)
            yield return waitForImpact;

            // impact effects
            foreach (var impactPos in impactPositions)
            {
                // impact
                SpawnEffect(RuneSelect(86769, 215809, 91441, 92031, 217139, 217458), impactPos);

                IList<Actor> hits = GetEnemiesInRange(impactPos, ScriptFormula(3));
                WeaponDamage(hits, ScriptFormula(0),
                    RuneSelect(DamageType.Fire, DamageType.Fire, DamageType.Fire, DamageType.Cold, DamageType.Arcane, DamageType.Fire));

                // pool effect
                if (Rune_B == 0)
                {
                    SpawnEffect(RuneSelect(90364, 90364, -1, 92032, 217307, 217459), impactPos, 0,
                        WaitSeconds(ScriptFormula(5)));
                }

                if (meteorCount > 1)
                    yield return WaitSeconds(0.1f);
            }
        }
    }

    [ImplementsPowerSNO(Skills.Skills.Wizard.Signature.Electrocute)]
    public class WizardElectrocute : ChanneledPower
    {
        public override void OnChannelOpen()
        {
            RunDelay = 0.5f;
        }

        public override IEnumerable<TickTimer> RunChannel()
        {
            User.TranslateFacing(TargetPosition);

            UsePrimaryResource(10f);

            if (Target == null)
            {
                // no target, just zap the air with miss effect rope
                User.AddRopeEffect(30913, SpawnProxy(TargetPosition));
            }
            else
            {
                IList<Actor> targets = new List<Actor>() { Target };
                Actor ropeSource = User;
                Actor curTarget = Target;
                float damage = 0.4f;
                while (targets.Count < 4) // original target + bounce 2 times
                {
                    // replace source with proxy if it died while doing bounce delay
                    if (ropeSource.World == null)
                        ropeSource = SpawnProxy(ropeSource.Position);

                    if (curTarget.World != null)
                    {
                        ropeSource.AddRopeEffect(0x78c0, curTarget);
                        ropeSource = curTarget;

                        WeaponDamage(curTarget, damage, DamageType.Lightning);
                        damage *= 0.7f;
                    }
                    else
                    {
                        // early out if monster to be bounced died prematurely
                        break;
                    }

                    curTarget = GetEnemiesInRange(curTarget.Position, 15f, 3).FirstOrDefault(t => !targets.Contains(t));
                    if (curTarget != null)
                    {
                        targets.Add(curTarget);
                        yield return WaitSeconds(0.150f);
                    }
                    else
                    {
                        break;
                    }
                }             
            }
        }
    }

    [ImplementsPowerSNO(Skills.Skills.Wizard.Signature.MagicMissile)]
    public class WizardMagicMissile : PowerScript
    {
        public override IEnumerable<TickTimer> Run()
        {
            UsePrimaryResource(20f);

            User.PlayEffectGroup(19305); // cast effect
            
            var projectile = new Projectile(this, 99567, User.Position);
            projectile.OnHit = (hit) =>
            {
                SpawnEffect(99572, hit.Position); // impact effect
                projectile.Destroy();
                WeaponDamage(hit, 1.10f, DamageType.Arcane);
            };
            projectile.Launch(TargetPosition, 1f);

            yield break;
        }
    }

    [ImplementsPowerSNO(Skills.Skills.Wizard.Offensive.Hydra)]
    public class WizardHydra : PowerScript
    {
        public override IEnumerable<TickTimer> Run()
        {
            UsePrimaryResource(60f);

            // HACK: made up demonic meteor spell, not real hydra
            SpawnEffect(185366, TargetPosition);
            yield return WaitSeconds(0.4f);

            IList<Actor> hits = GetEnemiesInRange(TargetPosition, 10f);
            WeaponDamage(hits, 10f, DamageType.Fire);
        }
    }

    [ImplementsPowerSNO(Skills.Skills.Wizard.Offensive.Disintegrate)]
    public class WizardDisintegrate : ChanneledPower
    {
        const float BeamLength = 40f;

        private Actor _target = null;

        private void _calcTargetPosition()
        {
            // project beam end to always be a certain length
            TargetPosition = PowerMath.ProjectAndTranslate2D(User.Position, TargetPosition,
                                                             new Vector3D(User.Position.X, User.Position.Y, TargetPosition.Z),
                                                             BeamLength);
        }

        public override void OnChannelOpen()
        {
            RunDelay = 0.1f;

            // spawn target effect a little bit above the ground target
            _calcTargetPosition();
            _target = SpawnEffect(52687, TargetPosition, 0, WaitInfinite());
            User.AddComplexEffect(18792, _target);
        }

        public override void OnChannelClose()
        {
            if (_target != null)
                _target.Destroy();
        }

        public override void OnChannelUpdated()
        {
            _calcTargetPosition();
            User.TranslateFacing(TargetPosition);
            // client updates target actor position
        }

        public override IEnumerable<TickTimer> RunChannel()
        {
            UsePrimaryResource(23f * RunDelay);

            foreach (Actor actor in GetEnemiesInRange(User.Position, BeamLength + 10f))
            {
                if (PowerMath.PointInBeam(actor.Position, User.Position, TargetPosition, 3f))
                {  
                    actor.PlayEffectGroup(18793);
                    WeaponDamage(actor, 1.35f * RunDelay, DamageType.Arcane, true);
                }
            }

            yield break;
        }
    }

    [ImplementsPowerSNO(Skills.Skills.Wizard.Offensive.WaveOfForce)]
    public class WizardWaveOfForce : PowerScript
    {
        public override IEnumerable<TickTimer> Run()
        {
            UsePrimaryResource(25f);
            StartCooldown(WaitSeconds(15f));

            yield return WaitSeconds(0.350f); // wait for wizard to land
            User.PlayEffectGroup(19356);

            IList<Actor> hits = GetEnemiesInRange(User.Position, 20);
            foreach (Actor actor in hits)
            {
                Knockback(actor, 5f);
                WeaponDamage(actor, 2.05f, DamageType.Physical);
            }
            yield break;
        }
    }

    [ImplementsPowerSNO(Skills.Skills.Wizard.Offensive.ArcaneTorrent)]
    public class WizardArcaneTorrent : ChanneledPower
    {
        private Actor _targetProxy = null;
        private Actor _userProxy = null;

        public override void OnChannelOpen()
        {
            RunDelay = 0.2f;

            _targetProxy = SpawnProxy(TargetPosition, WaitInfinite());
            _userProxy = SpawnProxy(User.Position, WaitInfinite());
            // TODO: fixed casting effect so it rotates along with actor
            _userProxy.PlayEffectGroup(97385);
            _userProxy.PlayEffectGroup(134442, _targetProxy);
        }

        public override void OnChannelClose()
        {
            _targetProxy.Destroy();
            _userProxy.Destroy();
        }

        public override IEnumerable<TickTimer> RunChannel()
        {
            UsePrimaryResource(20f * RunDelay);

            Vector3D laggyPosition = new Vector3D(TargetPosition);

            yield return WaitSeconds(0.9f);

            // update proxy target delayed so animation lines up with explosions a bit better
            if (ChannelOpen)
                _targetProxy.TranslateNormal(laggyPosition, 8f);

            SpawnEffect(97821, laggyPosition);
            WeaponDamage(GetEnemiesInRange(laggyPosition, 6f), 2.00f * RunDelay, DamageType.Arcane);
        }
    }

    //bumbasher
    [ImplementsPowerSNO(Skills.Skills.Wizard.Utility.FrostNova)]
    public class WizardFrostNova : PowerScript
    {
        public const int FrostNova_Emitter = 4402;

        public override IEnumerable<TickTimer> Run()
        {
            StartCooldown(WaitSeconds(12f));

            SpawnEffect(FrostNova_Emitter, User.Position);

            IList<Actor> hits = GetEnemiesInRange(User.Position, 18);
            foreach (Actor actor in hits)
            {
                WeaponDamage(actor, 0.65f, DamageType.Cold);
            }

            yield break;
        }
    }

    [ImplementsPowerSNO(Skills.Skills.Wizard.Offensive.Blizzard)]
    public class WizardBlizzard : PowerScript
    {
        public const int Wizard_Blizzard = 0x1977;

        public override IEnumerable<TickTimer> Run()
        {
            UsePrimaryResource(45f);

            SpawnEffect(Wizard_Blizzard, TargetPosition);

            const int blizzard_duration = 3;

            for(int i = 0; i < blizzard_duration; ++i)
            {
                IList<Actor> hits = GetEnemiesInRange(TargetPosition, 18);
                foreach (Actor actor in hits)
                {
                    WeaponDamage(actor, 0.65f, DamageType.Cold);
                }

                yield return WaitSeconds(1f);
            }
        }
    }

    [ImplementsPowerSNO(Skills.Skills.Wizard.Offensive.RayOfFrost)]
    public class WizardRayOfFrost : ChanneledPower
    {
        const float BeamLength = 40f;

        private Actor _target = null;

        private void _calcTargetPosition()
        {
            // project beam end to always be a certain length
            TargetPosition = PowerMath.ProjectAndTranslate2D(User.Position, TargetPosition,
                                                             new Vector3D(User.Position.X, User.Position.Y, TargetPosition.Z),
                                                             BeamLength);
        }

        public override void OnChannelOpen()
        {
            RunDelay = 0.1f;

            // spawn target effect a little bit above the ground target
            _calcTargetPosition();
            _target = SpawnEffect(6535, TargetPosition, 0, WaitInfinite());
            User.AddComplexEffect(19327, _target);
        }

        public override void OnChannelClose()
        {
            if (_target != null)
                _target.Destroy();
        }

        public override void OnChannelUpdated()
        {
            _calcTargetPosition();
            User.TranslateFacing(TargetPosition);
            // client updates target actor position
        }
        
        public override IEnumerable<TickTimer> RunChannel()
        {
            UsePrimaryResource(29f * RunDelay);

            foreach (Actor actor in GetEnemiesInRange(User.Position, BeamLength + 10f))
            {
                if (PowerMath.PointInBeam(actor.Position, User.Position, TargetPosition, 3f))
                {
                    WeaponDamage(actor, 2.70f * RunDelay, DamageType.Cold);
                }
            }

            yield break;
        }
    }

    [ImplementsPowerSNO(Skills.Skills.Wizard.Utility.Teleport)]
    public class WizardTeleport : PowerScript
    {
        public override IEnumerable<TickTimer> Run()
        {
            UsePrimaryResource(15f);
            //StartCooldown(WaitSeconds(16f));
            SpawnProxy(User.Position).PlayEffectGroup(191848);  // alt cast efg: 170231
            yield return WaitSeconds(0.3f);
            User.Teleport(TargetPosition);
            User.PlayEffectGroup(191849);
        }
    }

    [ImplementsPowerSNO(Skills.Skills.Wizard.Signature.SpectralBlade)]
    public class WizardSpectralBlade : PowerScript
    {
        public override IEnumerable<TickTimer> Run()
        {
            UsePrimaryResource(15f);

            User.PlayEffectGroup(188941);

            // calculate hit area of effect, just in front of the user
            TargetPosition = PowerMath.ProjectAndTranslate2D(User.Position, TargetPosition, User.Position, 9f);

            for (int n = 0; n < 3; ++n)
            {
                foreach (var target in GetEnemiesInRange(TargetPosition, 9f))
                {
                    WeaponDamage(target, 0.30f, DamageType.Physical);
                }
                yield return WaitSeconds(0.2f);
            }
        }
    }

    [ImplementsPowerSNO(Skills.Skills.Wizard.Signature.ShockPulse)]
    public class WizardShockPulse : PowerScript
    {
        public override IEnumerable<TickTimer> Run()
        {
            User.PlayEffectGroup(0x0001061B); // cast effect

            for (int n = 0; n < 3; ++n)
                _SpawnBolt();

            yield break;
        }

        private void _SpawnBolt()
        {
            var eff = SpawnEffect(176247, User.Position, 0, WaitSeconds(10f));
            
            World.BroadcastIfRevealed(new ACDTranslateDetPathMessage
            {
                Id = 118,
                Field0 = (int)eff.DynamicID,
                Field1 = 1, // 0 - crashes client
                            // 1 - random scuttle (charged bolt effect)
                            // 2 - random movement, random movement pauses (toads hopping)
                            // 3 - clockwise spiral
                            // 4 - counter-clockwise spiral
                            // >=5 - nothing it seems
                Field2 = Rand.Next(), // RNG seed for style 1 and 2
                Field3 = Rand.Next(), // RNG seed for style 1 and 2
                Field4 = new Vector3D(0.0f, 0.3f, 0),  // length of this vector is amount moved for style 1 and 2, 
                Field5 = PowerMath.AngleLookAt(User.Position, TargetPosition), // facing angle
                Field6 = User.Position,
                Field7 = 1,
                Field8 = 0,
                Field9 = -1,
                Field10 = PowerSNO, // power sno
                Field11 = 0,
                Field12 = 0f, // spiral control?
                Field13 = 0f, // spiral control? for charged bolt this is facing angle again, but seems to effect nothing.
                Field14 = 0f, // spiral control? for charged bolt this is Position.X and seems to effect nothing
                Field15 = 0f  // spiral control? for charged bolt this is Position.Y and seems to effect nothing
            }, eff);

        }
    }
}
