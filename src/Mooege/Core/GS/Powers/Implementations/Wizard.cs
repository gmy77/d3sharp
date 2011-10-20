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
using Mooege.Net.GS;
using Mooege.Net.GS.Message.Fields;
using Mooege.Net.GS.Message.Definitions.Effect;

namespace Mooege.Core.GS.Powers.Implementations
{
    [ImplementsPowerSNO(Skills.Skills.Wizard.Offensive.Meteor)]
    public class WizardMeteor : PowerImplementation
    {
        public override IEnumerable<TickTimer> Run()
        {
            UsePrimaryResource(60f);
            SpawnEffect(86790, TargetPosition);
            yield return WaitSeconds(1.9f); // wait for meteor to hit
            SpawnEffect(86769, TargetPosition);
            SpawnEffect(90364, TargetPosition, -1, WaitSeconds(4f));

            IList<Actor> hits = GetTargetsInRange(TargetPosition, 13f);
            Damage(hits, 150f, 0);
        }
    }

    [ImplementsPowerSNO(Skills.Skills.Wizard.Signature.Electrocute)]
    public class WizardElectrocute : PowerImplementation
    {
        public override IEnumerable<TickTimer> Run()
        {
            RegisterChannelingPower(WaitSeconds(0.150f));
            
            User.FacingTranslate(TargetPosition);

            // if throttling only update proxy if needed, then exit
            if (ThrottledCast)
            {
                if (Target == null)
                    GetChanneledProxy(0, TargetPosition);
                yield break;
            }

            UsePrimaryResource(6f);

            if (Target == null)
            {
                // no target, just zap the air
                User.AddRopeEffect(0x78c0, GetChanneledProxy(0, TargetPosition));
            }
            else
            {
                IList<Actor> targets = new List<Actor>() { Target };
                while (targets.Count < 3) // original target + bounce 2 times
                {
                    var bounce = GetTargetsInRange(targets.Last().Position, 15f, 3).FirstOrDefault(t => !targets.Contains(t));
                    if (bounce != null)
                        targets.Add(bounce);
                    else
                        break;
                }

                Actor ropeSource = User;
                foreach (Actor actor in targets)
                {
                    actor.PlayHitEffect(2, User);
                    ropeSource.AddRopeEffect(0x78c0, actor);

                    ropeSource = actor;
                }

                float damage = 12f;
                foreach (Actor actor in targets)
                {
                    Damage(actor, damage, 0);
                    damage *= 0.7f;
                }                
            }
            
            yield break;
        }
    }

    [ImplementsPowerSNO(Skills.Skills.Wizard.Signature.MagicMissile)]
    public class WizardMagicMissile : PowerImplementation
    {
        public override IEnumerable<TickTimer> Run()
        {
            UsePrimaryResource(20f);

            // HACK: made up spell, not real magic missile
            for (int step = 1; step < 10; ++step)
            {
                var spos = new Vector3D();
                spos.X = User.Position.X + ((TargetPosition.X - User.Position.X) * (step * 0.10f));
                spos.Y = User.Position.Y + ((TargetPosition.Y - User.Position.Y) * (step * 0.10f));
                spos.Z = User.Position.Z + ((TargetPosition.Z - User.Position.Z) * (step * 0.10f));

                SpawnEffect(61419, spos);

                IList<Actor> hits = GetTargetsInRange(spos, 6f);
                Damage(hits, 60f, 0);
                yield return WaitSeconds(0.1f);
            }
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
            Damage(hits, 100f, 0);
        }
    }

    [ImplementsPowerSNO(Skills.Skills.Wizard.Offensive.Disintegrate)]
    public class WizardDisintegrate : PowerImplementation
    {
        const float BeamLength = 40f;

        public override IEnumerable<TickTimer> Run()
        {
            RegisterChannelingPower(WaitSeconds(0.1f));
                        
            // project beam end to always be a certain length
            TargetPosition = PowerUtils.ProjectAndTranslate2D(TargetPosition, User.Position,
                                                               User.Position, BeamLength);

            if (!ThrottledCast)
            {
                UsePrimaryResource(3f);

                foreach (Actor actor in GetTargetsInRange(User.Position, BeamLength + 10f))
                {
                    if (PowerUtils.PointInBeam(actor.Position, User.Position, TargetPosition, 7f))
                    {  
                        actor.PlayHitEffect(32, User);
                        actor.PlayEffectGroup(18793);
                        Damage(actor, 10, 0);
                    }
                }
            }

            // always update effect locations
            TargetPosition.Z += 10f; // put effect a little bit above the ground target
            Effect pid = GetChanneledEffect(0, 52687, TargetPosition, true);
            if (! UserIsChanneling)
            {
                User.AddComplexEffect(18792, pid);
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
                actor.AddStunBuff(2000f);
                Damage(actor, 20, 0);
            }
            yield break;
        }
    }

    [ImplementsPowerSNO(Skills.Skills.Wizard.Offensive.ArcaneTorrent)]
    public class WizardArcaneTorrent : PowerImplementation
    {
        public override IEnumerable<TickTimer> Run()
        {
            RegisterChannelingPower(WaitSeconds(0.2f));

            if (!UserIsChanneling)
            {
                Actor targetProxy = GetChanneledProxy(0, TargetPosition);
                Actor userProxy = GetChanneledProxy(1, User.Position);
                // TODO: fixed casting effect so it rotates along with actor
                userProxy.PlayEffectGroup(97385);
                userProxy.PlayEffectGroup(134442, targetProxy);
            }

            if (!ThrottledCast)
            {
                UsePrimaryResource(10f);
                yield return WaitSeconds(0.9f);
                // update proxy target location laggy
                GetChanneledProxy(0, TargetPosition);

                SpawnEffect(97821, TargetPosition);
                Damage(GetTargetsInRange(TargetPosition, 6f), 20, 0);
            }
        }
    }

    //bumbasher
    [ImplementsPowerSNO(Skills.Skills.Wizard.Utility.FrostNova)]
    public class WizardFrostNova : PowerImplementation
    {
        public const int FrostNova_Emitter = 4402; //plain frost nova effect
        public const int FrostNova_Emitter_alabaster_unfreeze = 0x2e27a;
        public const int FrostNova_Emitter_crimson_addDamage = 0x2e277;
        public const int FrostNova_Emitter_golden_reduceCooldown = 0x2e279;
        public const int FrostNova_Emitter_indigo_miniFrostNovas = 0x2e278;
        public const int FrostNova_Minor_Emitter = 0x1133;

        public List<int> Effects = new List<int>
        {
            FrostNova_Emitter,
            FrostNova_Emitter_alabaster_unfreeze,
            FrostNova_Emitter_crimson_addDamage,
            FrostNova_Emitter_golden_reduceCooldown,
            FrostNova_Emitter_indigo_miniFrostNovas,
            FrostNova_Minor_Emitter
        };

        public override IEnumerable<TickTimer> Run()
        {
            StartCooldown(WaitSeconds(12f));

            SpawnEffect(FrostNova_Emitter, User.Position); //center on self

            IList<Actor> hits = GetTargetsInRange(User.Position, 18); //FIXME: is the range correct? what units?
            foreach (Actor actor in hits)
            {
                actor.AddFreezeBuff(4000); //freeze for 4 sec, TODO: use some level to increase duration or something
                Damage(actor, Rand.Next(2, 4+1), 0); //does 2-4 damage, TODO: use player DPS or something
            }

            yield break;
        }
    }

    [ImplementsPowerSNO(Skills.Skills.Wizard.Offensive.Blizzard)]
    public class WizardBlizzard : PowerImplementation
    {
        public const int Wizard_Blizzard = 0x1977;
        public const int Wizard_Blizzard_addFreeze = 0x2d53f;
        public const int Wizard_Blizzard_addSize = 0x2d53d;
        public const int wizard_blizzard_addSize_panels = 0x2d490;
        public const int Wizard_Blizzard_addTime = 0x2d53c;
        public const int wizard_blizzard_addTime_panels = 0x2d473;
        public const int wizard_blizzard_panels = 0xd28;
        public const int Wizard_Blizzard_reduceCost = 0x2d53e;
        public const int wizard_blizzard_reduceCost_panels = 0x2d4a9;
        public const int Wizard_BlizzardRune_Mist = 0x1277a;

        public static List<int> Effects = new List<int> 
        {
            Wizard_Blizzard,
            Wizard_Blizzard_addFreeze,
            Wizard_Blizzard_addSize,
            wizard_blizzard_addSize_panels,
            Wizard_Blizzard_addTime,
            wizard_blizzard_addTime_panels,
            wizard_blizzard_panels,
            Wizard_Blizzard_reduceCost,
            wizard_blizzard_reduceCost_panels,
            Wizard_BlizzardRune_Mist
        };

        //deals 12-18 DPS for 3 seconds
        public override IEnumerable<TickTimer> Run()
        {
            UsePrimaryResource(45f);

            SpawnEffect(Wizard_Blizzard, TargetPosition);

            //do damage for 3 seconds
            const int blizzard_duration = 3;

            for(int i=0; i<blizzard_duration; ++i)
            {
                IList<Actor> hits = GetTargetsInRange(TargetPosition, 18); //FIXME: is the range correct? what units?
                foreach (Actor actor in hits)
                {
                    actor.AddChillBuff(1000); //FIXME: does blizzard slows the monsters?
                    Damage(actor, Rand.Next(12, 18+1), 0);
                }

                yield return WaitSeconds(1f);
            }

            yield break;
        }
    }

    [ImplementsPowerSNO(Skills.Skills.Wizard.Offensive.RayOfFrost)]
    public class WizardRayOfFrost : PowerImplementation
    {
        const float BeamLength = 60f;

        public override IEnumerable<TickTimer> Run() //TODO: still WIP
        {
            RegisterChannelingPower(WaitSeconds(0.1f));

            User.FacingTranslate(TargetPosition);

            // project beam end to always be a certain length
            TargetPosition = PowerUtils.ProjectAndTranslate2D(TargetPosition, User.Position,
                                                               User.Position, BeamLength);

            if (!ThrottledCast)
            {
                UsePrimaryResource(3.5f);
                foreach (Actor actor in GetTargetsInRange(User.Position, BeamLength + 10f))
                {
                    if (PowerUtils.PointInBeam(actor.Position, User.Position, TargetPosition, 7f))
                    {
                        //hit effects: 8 - disintegrate, 4 - some witch doctor green crap, 2 - electric, 1 - some fire methink, 16-ice particles, 
                        actor.PlayHitEffect(64, User);
                        //FIXME: it only need to last as long as the monster is getting hit
                        // fixed? 100ms time limit seems make it a bit better /mdz
                        SpawnEffect(6535, actor.Position, 0, WaitSeconds(0.1f));
                        Damage(actor, 10, 0);
                    }
                }
            }

            // always update effect locations
            TargetPosition.Z += 10f; // put effect a little bit above the ground target
            Effect pid = GetChanneledEffect(0, 6535, TargetPosition, true);
            if (!UserIsChanneling)
            {
                User.AddComplexEffect(19327, pid);
            }

            yield break;
        }
    }
}
