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
using Mooege.Core.GS.Ticker.Helpers;
using Mooege.Net.GS.Message.Definitions.ACD;

namespace Mooege.Core.GS.Powers.Implementations
{
    [ImplementsPowerSNO(Skills.Skills.Wizard.Offensive.Meteor)]
    public class WizardMeteor : PowerImplementation
    {
        public override IEnumerable<TickTimer> Run()
        {
            UsePrimaryResource(60f);
            SpawnEffect(86790, TargetPosition);
            yield return WaitSeconds(2f); // wait for meteor to hit
            SpawnEffect(86769, TargetPosition);
            SpawnEffect(90364, TargetPosition, 0, WaitSeconds(4f));

            IList<Actor> hits = GetTargetsInRange(TargetPosition, 13f);
            WeaponDamage(hits, 3.05f, DamageType.Fire);

            // TODO: ground fire damage?
        }
    }

    [ImplementsPowerSNO(Skills.Skills.Wizard.Signature.Electrocute)]
    public class WizardElectrocute : ChanneledPowerImplementation
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

                    curTarget = GetTargetsInRange(curTarget.Position, 15f, 3).FirstOrDefault(t => !targets.Contains(t));
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
    public class WizardMagicMissile : PowerImplementation
    {
        public override IEnumerable<TickTimer> Run()
        {
            UsePrimaryResource(20f);

            User.PlayEffectGroup(19305); // cast effect
            
            var projectile = new PowerProjectile(User.World, 99567, User.Position, TargetPosition, 1f, 2000, 1f, 3f, 5f, 0f);
            projectile.OnHit = () =>
            {
                SpawnEffect(99572, projectile.getCurrentPosition()); // impact effect
                projectile.Destroy();
                WeaponDamage(projectile.hittedActor, 1.10f, DamageType.Arcane);
            };

            yield break;
        }
    }

    [ImplementsPowerSNO(Skills.Skills.Wizard.Offensive.Hydra)]
    public class WizardHydra : PowerImplementation
    {
        public override IEnumerable<TickTimer> Run()
        {
            UsePrimaryResource(60f);

            // HACK: made up demonic meteor spell, not real hydra
            SpawnEffect(185366, TargetPosition);
            yield return WaitSeconds(0.4f);

            IList<Actor> hits = GetTargetsInRange(TargetPosition, 10f);
            WeaponDamage(hits, 10f, DamageType.Fire);
        }
    }

    [ImplementsPowerSNO(Skills.Skills.Wizard.Offensive.Disintegrate)]
    public class WizardDisintegrate : ChanneledPowerImplementation
    {
        const float BeamLength = 40f;

        private Actor _target = null;

        private void _calcTargetPosition()
        {
            // project beam end to always be a certain length
            TargetPosition = PowerMath.ProjectAndTranslate2D(User.Position, TargetPosition,
                                                               User.Position, BeamLength);
            TargetPosition.Z = TargetZ;
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
            _target.TranslateSnapped(TargetPosition);
        }

        public override IEnumerable<TickTimer> RunChannel()
        {
            UsePrimaryResource(23f * RunDelay);

            foreach (Actor actor in GetTargetsInRange(User.Position, BeamLength + 10f))
            {
                if (PowerMath.PointInBeam(actor.Position, User.Position, TargetPosition, 7f))
                {  
                    actor.PlayEffectGroup(18793);
                    WeaponDamage(actor, 1.35f * RunDelay, DamageType.Arcane, true);
                }
            }

            yield break;
        }
    }

    [ImplementsPowerSNO(Skills.Skills.Wizard.Offensive.WaveOfForce)]
    public class WizardWaveOfForce : PowerImplementation
    {
        public override IEnumerable<TickTimer> Run()
        {
            UsePrimaryResource(25f);
            StartCooldown(WaitSeconds(15f));

            yield return WaitSeconds(0.350f); // wait for wizard to land
            User.PlayEffectGroup(19356);

            IList<Actor> hits = GetTargetsInRange(User.Position, 20);
            foreach (Actor actor in hits)
            {
                Knockback(actor, 5f);
                WeaponDamage(actor, 2.05f, DamageType.Physical);
            }
            yield break;
        }
    }

    [ImplementsPowerSNO(Skills.Skills.Wizard.Offensive.ArcaneTorrent)]
    public class WizardArcaneTorrent : ChanneledPowerImplementation
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
            WeaponDamage(GetTargetsInRange(laggyPosition, 6f), 2.00f * RunDelay, DamageType.Arcane);
        }
    }

    //bumbasher
    [ImplementsPowerSNO(Skills.Skills.Wizard.Utility.FrostNova)]
    public class WizardFrostNova : PowerImplementation
    {
        public const int FrostNova_Emitter = 4402;

        public override IEnumerable<TickTimer> Run()
        {
            StartCooldown(WaitSeconds(12f));

            SpawnEffect(FrostNova_Emitter, User.Position);

            IList<Actor> hits = GetTargetsInRange(User.Position, 18);
            foreach (Actor actor in hits)
            {
                WeaponDamage(actor, 0.65f, DamageType.Cold);
            }

            yield break;
        }
    }

    [ImplementsPowerSNO(Skills.Skills.Wizard.Offensive.Blizzard)]
    public class WizardBlizzard : PowerImplementation
    {
        public const int Wizard_Blizzard = 0x1977;

        public override IEnumerable<TickTimer> Run()
        {
            UsePrimaryResource(45f);

            SpawnEffect(Wizard_Blizzard, TargetPosition);

            const int blizzard_duration = 3;

            for(int i = 0; i < blizzard_duration; ++i)
            {
                IList<Actor> hits = GetTargetsInRange(TargetPosition, 18);
                foreach (Actor actor in hits)
                {
                    WeaponDamage(actor, 0.65f, DamageType.Cold);
                }

                yield return WaitSeconds(1f);
            }
        }
    }

    [ImplementsPowerSNO(Skills.Skills.Wizard.Offensive.RayOfFrost)]
    public class WizardRayOfFrost : ChanneledPowerImplementation
    {
        const float BeamLength = 40f;

        private Actor _target = null;

        private void _calcTargetPosition()
        {
            // project beam end to always be a certain length
            TargetPosition = PowerMath.ProjectAndTranslate2D(User.Position, TargetPosition,
                                                               User.Position, BeamLength);
            TargetPosition.Z = TargetZ;
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
            _target.TranslateSnapped(TargetPosition);
            User.TranslateFacing(TargetPosition);
        }
        
        public override IEnumerable<TickTimer> RunChannel()
        {
            UsePrimaryResource(29f * RunDelay);

            foreach (Actor actor in GetTargetsInRange(User.Position, BeamLength + 10f))
            {
                if (PowerMath.PointInBeam(actor.Position, User.Position, TargetPosition, 7f))
                {
                    WeaponDamage(actor, 2.70f * RunDelay, DamageType.Cold);
                }
            }

            yield break;
        }
    }

    [ImplementsPowerSNO(Skills.Skills.Wizard.Utility.Teleport)]
    public class WizardTeleport : PowerImplementation
    {
        public override IEnumerable<TickTimer> Run()
        {
            UsePrimaryResource(15f);
            //StartCooldown(WaitSeconds(16f));
            SpawnProxy(User.Position).PlayEffectGroup(19352);  // alt cast efg: 170231
            yield return WaitSeconds(0.3f);
            User.Teleport(TargetPosition);
            User.PlayEffectGroup(170232);
        }
    }

    [ImplementsPowerSNO(Skills.Skills.Wizard.Signature.SpectralBlade)]
    public class WizardSpectralBlade : PowerImplementation
    {
        public override IEnumerable<TickTimer> Run()
        {
            UsePrimaryResource(15f);

            User.PlayEffectGroup(188941);

            // calculate hit area of effect, just in front of the user
            TargetPosition = PowerMath.ProjectAndTranslate2D(User.Position, TargetPosition, User.Position, 9f);

            for (int n = 0; n < 3; ++n)
            {
                foreach (var target in GetTargetsInRange(TargetPosition, 9f))
                {
                    WeaponDamage(target, 0.30f, DamageType.Physical);
                }
                yield return WaitSeconds(0.2f);
            }
        }
    }

    [ImplementsPowerSNO(Skills.Skills.Wizard.Signature.ShockPulse)]
    public class WizardShockPulse : PowerImplementation
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
                Id = 0x73,
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
