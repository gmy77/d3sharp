﻿/*
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
using Mooege.Core.GS.Powers.Payloads;
using Mooege.Core.GS.Actors.Movement;
using Mooege.Core.GS.Common.Types.TagMap;
using Mooege.Net.GS.Message;
using Mooege.Core.GS.Players;

//TODO (IMPORTANT): GO BACK through and any WaitSeconds/GameAttributes must be in Buff Apply() and Remove()
//TODO: ADD TO buffs. Targets in Radius receiving debuff.
//TODO any projectile that pierces through enemies needs to be inside an updater, so it doesnt constantly attack.

namespace Mooege.Core.GS.Powers.Implementations
{
    //Complete
    #region Meteor
    [ImplementsPowerSNO(Skills.Skills.Wizard.Offensive.Meteor)]
    public class WizardMeteor : PowerScript
    {
        public override IEnumerable<TickTimer> Run()
        {
            //Rune_D here as well.
            UsePrimaryResource(ScriptFormula(8));

            // cast effect
            User.PlayEffectGroup(RuneSelect(71141, 71141, 71141, 92222, 217377, 217461));

            // HACK: mooege's 100ms update rate is a little to slow for the impact to appear right on time so
            // an 100ms is shaved off the wait time
            TickTimer waitForImpact = WaitSeconds(ScriptFormula(4) - 0.1f);

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
                TickTimer poolTime = null;
                AttackPayload attack = new AttackPayload(this);
                attack.Targets = GetEnemiesInRadius(impactPos, ScriptFormula(3));
                attack.AddWeaponDamage(ScriptFormula(0), RuneSelect(DamageType.Fire, DamageType.Fire, DamageType.Fire, DamageType.Cold, DamageType.Arcane, DamageType.Fire));
                attack.OnHit = hit =>
                {
                    if (Rune_E > 0)
                    {
                        if (hit.IsCriticalHit)
                        {
                            poolTime = WaitSeconds(ScriptFormula(7));
                        }
                    }
                    else
                    {
                        poolTime = WaitSeconds(ScriptFormula(5));
                    }
                };
                attack.Apply();

                var moltenFire = SpawnEffect(RuneSelect(86769, 215809, 91441, 92031, 217139, 217458), impactPos, 0, poolTime);
                moltenFire.UpdateDelay = 1f;
                moltenFire.OnUpdate = () =>
                {
                    AttackPayload DOTattack = new AttackPayload(this);
                    DOTattack.Targets = GetEnemiesInRadius(impactPos, ScriptFormula(3));
                    DOTattack.AddWeaponDamage(ScriptFormula(2), RuneSelect(DamageType.Fire, DamageType.Fire, DamageType.Fire, DamageType.Cold, DamageType.Arcane, DamageType.Fire));
                    DOTattack.OnHit = hit =>
                    {
                        if (Rune_C > 0)
                        {
                            //Freezing Mist
                            AddBuff(hit.Target, new DebuffChilled(0.6f, WaitSeconds(3f)));
                        }
                    };
                    DOTattack.Apply();
                };

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

    #endregion

    //TODO: The charged bolts work, but regular electrocution does not.
    #region Electrocute
    [ImplementsPowerSNO(Skills.Skills.Wizard.Signature.Electrocute)]
    public class WizardElectrocute : ChanneledSkill
    {
        public override void OnChannelOpen()
        {
            EffectsPerSecond = 0.5f;
        }

        public override IEnumerable<TickTimer> Main()
        {
            User.TranslateFacing(TargetPosition);

            UsePrimaryResource(ScriptFormula(4));

            if (Rune_A > 0)
            {
                var proj = new Projectile(this, 76019, User.Position);
                proj.Position.Z += 5f;  // fix height
                proj.OnCollision = (hit) =>
                {
                    hit.PlayEffectGroup(77858);
                    WeaponDamage(GetEnemiesInRadius(proj.Position, ScriptFormula(2)), ScriptFormula(0), DamageType.Lightning);
                };
                proj.Launch(TargetPosition, 1.25f);
            }
            else if (Rune_C > 0)
            {
                User.PlayEffectGroup(77807);
                //unsure of the arc distances, but seems to be fine.
                WeaponDamage(GetEnemiesInArcDirection(User.Position, TargetPosition, ScriptFormula(2), 90f), ScriptFormula(0), DamageType.Lightning);
            }
            else if (Target == null)
            {
                // no target, just zap the air with miss effect rope
                User.AddRopeEffect(30913, TargetPosition);
            }
            else
            {
                IList<Actor> targets = new List<Actor>() { Target };
                Actor ropeSource = User;
                Actor curTarget = Target;
                float damage = ScriptFormula(0);
                while (targets.Count < ScriptFormula(9) + 1) // original target + bounce 2 times
                {
                    // replace source with proxy if it died while doing bounce delay
                    if (ropeSource.World == null)
                        ropeSource = SpawnProxy(ropeSource.Position);

                    if (curTarget.World != null)
                    {
                        ropeSource.AddRopeEffect(0x78c0, curTarget);
                        ropeSource = curTarget;

                        WeaponDamage(curTarget, damage, DamageType.Lightning);
                        /*AttackPayload attack = new AttackPayload(this);
                        attack.OnHit = HitPayload =>
                        {
                            if (Rune_E > 0)
                            {
                                if (HitPayload.IsCriticalHit)
                                {
                                    Vector3D[] projDestinations = PowerMath.GenerateSpreadPositions(User.Position, TargetPosition, 72f, (int)ScriptFormula(14));

                                    foreach (Vector3D missilePos in projDestinations)
                                    {
                                        var proj = new Projectile(this, 176247, TargetPosition);
                                        proj.OnCollision = (hit) =>
                                        {
                                            SpawnEffect(176262, new Vector3D(hit.Position.X, hit.Position.Y, hit.Position.Z + 5f)); // impact effect (fix height)
                                            proj.Destroy();
                                            WeaponDamage(hit, ScriptFormula(12), DamageType.Lightning);
                                        };
                                        proj.Launch(missilePos, 1.25f);
                                    }
                                }
                            }

                        };
                        attack.Apply();*/

                        if (Rune_B > 0)
                        {
                            damage *= 0.7f;
                        }
                        if (Rune_D > 0)
                        {
                            GeneratePrimaryResource(ScriptFormula(6));
                        }
                    }
                    else
                    {
                        // early out if monster to be bounced died prematurely
                        break;
                    }

                    curTarget = GetEnemiesInRadius(curTarget.Position, ScriptFormula(2), (int)ScriptFormula(9)).Actors.FirstOrDefault(t => !targets.Contains(t));
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
    #endregion

    //Rune_B: much better(thanks Wesko), do you also see the weird order of missiles?
    //        btw, I sped up the missiles even more to seem closer to the animation/video.
    //TODO: also figure out Rune_E homing missile
    //TODO: Proper implementation for homing missiles (RuneE) (Ussing only 1 missile and modify its trayectory till reaches target?).
    #region MagicMissile
    [ImplementsPowerSNO(Skills.Skills.Wizard.Signature.MagicMissile)]
    public class WizardMagicMissile : Skill
    {
        //TODO:Rune_E - ScriptFormula(10 -> Seek Angle Rotate(36),11 -> Seek Update Rate(.15)) -> tracks to nearest target
        public override IEnumerable<TickTimer> Main()
        {
            UsePrimaryResource(ScriptFormula(7));
            User.PlayEffectGroup(19305); // cast effect
            if (Rune_B > 0)
            {
                Vector3D[] projDestinations = PowerMath.GenerateSpreadPositions(User.Position, TargetPosition, ScriptFormula(8) / 5f, (int)ScriptFormula(5));

                var proj1 = new Projectile(this, 99567, User.Position);
                proj1.Launch(projDestinations[0], ScriptFormula(4));
                proj1.OnCollision = (hit) =>
                {
                    SpawnEffect(99572, new Vector3D(hit.Position.X, hit.Position.Y, hit.Position.Z + 5f)); // impact effect (fix height)
                    proj1.Destroy();
                    WeaponDamage(hit, ScriptFormula(1), DamageType.Arcane);
                };

                for (int i = 1; i < projDestinations.Length; i++)
                {
                    Int32 j = 1;
                    for (j = 1; j < 12000001; j++) //This will generate a 40ms delay over each projectile launch.
                    {
                        if (j % 12000000 == 0)
                        {
                            var proj = new Projectile(this, 99567, User.Position);
                            proj.Launch(projDestinations[i], ScriptFormula(4));
                            proj.OnCollision = (hit) =>
                            {
                                SpawnEffect(99572, new Vector3D(hit.Position.X, hit.Position.Y, hit.Position.Z + 5f)); // impact effect (fix height)
                                proj.Destroy();
                                WeaponDamage(hit, ScriptFormula(1), DamageType.Arcane);
                            };
                        }
                    }
                }
            }
            else if (Rune_E > 0)
            {                
                Int32 j = 1;
                var projectile = new Projectile(this, 99567, User.Position);
                var target = GetEnemiesInArcDirection(User.Position, TargetPosition, 60f, 60f).GetClosestTo(User.Position);
                projectile.Launch(TargetPosition, ScriptFormula(4));
                for (j = 1; j < 50000001; j++) //This will generate a ~150ms delay and then search for a target.
                {
                    if (j % 50000000 == 0)
                    {
                        if (target != null)
                        {
                            var projectile2 = new Projectile(this, 99567, projectile.Position);
                            projectile2.Launch(target.Position, ScriptFormula(4));
                            projectile.Destroy();
                            projectile2.OnCollision = (hit) =>
                            {
                                SpawnEffect(99572, new Vector3D(hit.Position.X, hit.Position.Y, hit.Position.Z + 5f)); // impact effect (fix height)
                                projectile2.Destroy();
                                WeaponDamage(hit, ScriptFormula(1), DamageType.Arcane);
                            };
                        }
                    }
                }
            }
            else
            {
                var projectile = new Projectile(this, 99567, User.Position);
                projectile.OnCollision = (hit) =>
                {
                    SpawnEffect(99572, new Vector3D(hit.Position.X, hit.Position.Y, hit.Position.Z + 5f)); // impact effect (fix height)
                    WeaponDamage(hit, ScriptFormula(1), DamageType.Arcane);

                    if (Rune_D > 0)
                    {
                        GeneratePrimaryResource(ScriptFormula(16));
                    }

                    if (Rune_C > 0)
                    {
                        if (Rand.NextDouble() < ScriptFormula(12))
                        {
                            //this is actually how i think it should work, pierce first target, if addition targets behind enemy, will continue to do damage to them as well.
                        }
                        projectile.Destroy();
                    }
                    else
                        projectile.Destroy();
                };
                projectile.Launch(TargetPosition, ScriptFormula(4));
            }

            yield break;
        }
    }
    #endregion

    //Very Imcomplete
    //Hydras are (most likely) Pets so this is incorrect
    #region Hydra
    [ImplementsPowerSNO(Skills.Skills.Wizard.Offensive.Hydra)]
    //No Rune = Default
    //Rune_A = Hydra_Frost
    //Rune_B = Hydra_Lightning
    //Rune_C = Hydra_Acid
    //Rune_D = Hydra_Big
    //Rune_E = Hydra_Arcane

    public class WizardHydra : Skill
    {
        const float BeamLength = 50f;

        public override IEnumerable<TickTimer> Main()
        {
            UsePrimaryResource(60f);

            //This works much better, but all three heads fire at the same target, 
            //then need to be firing off less than a second a part, like .5s.

            Vector3D userCastPosition = new Vector3D(User.Position);
            Vector3D[] spawnPoints = PowerMath.GenerateSpreadPositions(TargetPosition, new Vector3D(TargetPosition.X, TargetPosition.Y + 0.7f, TargetPosition.Z), 120, 3);

            var timeout = WaitSeconds(ScriptFormula(0));

            var lavapool = SpawnEffect(RuneSelect(81103, 83028, 81238, 77112, 83964, 81239), TargetPosition, 0, timeout); //Lava Pool Spawn
            lavapool.PlayEffectGroup(RuneSelect(81102, 82995, 82116, -1, 86328, 81301));

            int[] actorSNOs = new int[] {   RuneSelect(80745, 82972, 82109, 82111, -1, 81515), 
                                            RuneSelect(80757, 83024, 81229, 81226, -1, 81231), 
                                            RuneSelect(80758, 83025, 81230, 81227, -1, 81232) };

            if (Rune_D > 0)
            {

                //big hydra -> this throws an exception once spawned.
                var hydra1 = new EffectActor(this, 83959, spawnPoints[0]);
                hydra1.Scale = 2f;
                hydra1.Spawn();
                hydra1.UpdateDelay = 3f;
                hydra1.OnUpdate = () =>
                {
                    var target = GetEnemiesInRadius(hydra1.Position, 50f).GetClosestTo(hydra1.Position);
                    float castAngle = MovementHelpers.GetFacingAngle(hydra1.Position, target.Position);
                    hydra1.TranslateFacing(target.Position, true);
                    //timeout is set to 3, but starts while casting firewall, when it should start after firewall has spawned (1.8seconds)
                    var firewall = SpawnEffect(86082, hydra1.Position, castAngle, WaitSeconds(3f));
                    firewall.UpdateDelay = 1f;
                    firewall.OnUpdate = () =>
                    {
                        WeaponDamage(GetEnemiesInBeamDirection(hydra1.Position, target.Position, 50f, 5f), 1.00f, DamageType.Fire);
                    };

                };
            }
            else
            {
                var hydra1 = SpawnEffect(actorSNOs[0], spawnPoints[0], 0, timeout);
                hydra1.UpdateDelay = 1f; // attack every half-second
                hydra1.OnUpdate = () =>
                {
                    var targets = GetEnemiesInRadius(hydra1.Position, 60f);
                    if (targets.Actors.Count > 0 && targets != null)
                    {
                        targets.SortByDistanceFrom(hydra1.Position);
                        var proj = new Projectile(this, RuneSelect(77116, 83043, -1, 77109, 86082, 77097), hydra1.Position);
                        proj.Position.Z += 5f;  // fix height
                        proj.OnCollision = (hit) =>
                        {
                            hit.PlayEffectGroup(RuneSelect(219760, 219770, 219776, 219789, -1, 81739));
                            WeaponDamage(hit, 1.00f, DamageType.Fire);

                            proj.Destroy();
                        };
                        hydra1.TranslateFacing(targets.Actors[0].Position, true);
                        //need to fix how fast it fires -> its firing before head turns.
                        proj.Launch(targets.Actors[0].Position, ScriptFormula(2));
                    }

                };

                var hydra2 = SpawnEffect(actorSNOs[1], spawnPoints[1], 0, timeout);
                hydra2.UpdateDelay = 1f; // attack every half-second
                hydra2.OnUpdate = () =>
                {
                    var targets = GetEnemiesInRadius(hydra2.Position, 60f);
                    if (targets.Actors.Count > 0 && targets != null)
                    {
                        targets.SortByDistanceFrom(hydra2.Position);
                        var proj = new Projectile(this, RuneSelect(77116, 83043, -1, 77109, 86082, 77097), hydra2.Position);
                        proj.Position.Z += 5f;  // fix height
                        proj.OnCollision = (hit) =>
                        {
                            hit.PlayEffectGroup(RuneSelect(219760, 219770, 219776, 219789, -1, 81739));
                            WeaponDamage(hit, 1.00f, DamageType.Fire);

                            proj.Destroy();
                        };
                        //need to fix how fast it fires -> its firing before head turns.
                        hydra2.TranslateFacing(targets.Actors[0].Position, true);
                        proj.Launch(targets.Actors[0].Position, ScriptFormula(2));
                    }

                };

                var hydra3 = SpawnEffect(actorSNOs[2], spawnPoints[2], 0, timeout);
                hydra3.UpdateDelay = 1f; // attack every half-second
                hydra3.OnUpdate = () =>
                {
                    var targets = GetEnemiesInRadius(hydra3.Position, 60f);
                    if (targets.Actors.Count > 0 && targets != null)
                    {
                        targets.SortByDistanceFrom(hydra3.Position);
                        var proj = new Projectile(this, RuneSelect(77116, 83043, -1, 77109, 86082, 77097), hydra3.Position);
                        proj.Position.Z += 5f;  // fix height
                        proj.OnCollision = (hit) =>
                        {
                            hit.PlayEffectGroup(RuneSelect(219760, 219770, 219776, 219789, -1, 81739));
                            WeaponDamage(hit, 1.00f, DamageType.Fire);

                            proj.Destroy();
                        };
                        hydra3.TranslateFacing(targets.Actors[0].Position, true);
                        //need to fix how fast it fires -> its firing before head turns.
                        proj.Launch(targets.Actors[0].Position, ScriptFormula(2));
                    }

                };
            }
            // wait for duration of skill
            yield return timeout;
        }
    }
    #endregion

    //Complete
    #region ArcaneOrb
    [ImplementsPowerSNO(Skills.Skills.Wizard.Offensive.ArcaneOrb)]
    public class ArcaneOrb : Skill
    {
        public override IEnumerable<TickTimer> Main()
        {
            UsePrimaryResource(EvalTag(PowerKeys.ResourceCost));

            Vector3D[] targetDirs;
            {
                targetDirs = new Vector3D[] { TargetPosition };
            }

            if (Rune_C > 0)
            {
                User.World.BuffManager.RemoveBuffs(User, Skills.Skills.Wizard.Offensive.ArcaneOrb);
                AddBuff(User, new Orbit4());
                yield break;
            }
            else

                foreach (Vector3D position in targetDirs)
                {
                    var proj = new Projectile(this, RuneSelect(6515, 130073, 215555, -1, 216040, 75650), User.Position);
                    proj.Position.Z += 5f;  // fix height
                    proj.OnCollision = (hit) =>
                    {
                        hit.PlayEffectGroup(RuneSelect(19308, 130020, 215580, -1, 216056, -1));
                        WeaponDamage(GetEnemiesInRadius(proj.Position, ScriptFormula(5)), ScriptFormula(3), DamageType.Arcane);

                        if (Rune_E > 0)
                        {
                        }
                        else
                        {
                            proj.Destroy();
                        }
                    };
                    proj.Launch(position, ScriptFormula(2));

                    yield return WaitSeconds(2f);
                }
        }

        abstract class OrbitBase : PowerBuff
        {
            TickTimer timer;

            public override void Init()
            {
                timer = WaitSeconds(1f);
            }

            public override bool Update()
            {

                if (base.Update())
                    return true;

                if (timer.TimedOut)
                {
                    var targets = GetEnemiesInRadius(Target.Position, 10f);
                    if (targets.Actors.Count > 0)
                    {
                        WeaponDamage(targets, ScriptFormula(3), DamageType.Arcane);
                        OrbitUsed();
                        return true;
                    }
                }

                return false;
            }

            protected abstract void OrbitUsed();
        }
        [ImplementsPowerBuff(0)]
        class Orbit1 : OrbitBase
        {
            protected override void OrbitUsed()
            {
                // do nothing, orbits all used up
            }
        }
        [ImplementsPowerBuff(1)]
        class Orbit2 : OrbitBase
        {
            protected override void OrbitUsed()
            {
                AddBuff(Target, new Orbit1());
            }
        }
        [ImplementsPowerBuff(2)]
        class Orbit3 : OrbitBase
        {
            protected override void OrbitUsed()
            {
                AddBuff(Target, new Orbit2());
            }
        }
        [ImplementsPowerBuff(3)]
        class Orbit4 : OrbitBase
        {
            protected override void OrbitUsed()
            {
                AddBuff(Target, new Orbit3());
            }
        }
    }
    #endregion

    //Very Imcomplete
    #region EnergyTwister
    [ImplementsPowerSNO(Skills.Skills.Wizard.Offensive.EnergyTwister)]
    public class EnergyTwister : Skill
    {
        //http://www.youtube.com/watch?v=atIsPKXAzCU

        public override IEnumerable<TickTimer> Main()
        {
            UsePrimaryResource(ScriptFormula(15));

            // cast effect
            AddBuff(Target, new Twister());
            //SpawnEffect(RuneSelect(6560, 215311, 6560, 6560, 215324, 210804), TargetPosition);

            //Tornados need to move randomdirections at first, if the tornado is heading towards an enemy close by,
            // it will move towards the enemies.

            //Seems like it's classified as a buff (Buff Group 3)

            //and leave trail behind them (79940), think actor already does this.

            // NoRune = Unleash a twister, deals 60% weapon damage per second Arcane to everything caught within it. 
            // Rune_E = Stationary = deals damage but does not move
            // Rune_A = Increase Damage
            // Rune_C = 5 regular twisters to charge up, 
            //          then User needs to cast signature spell, then One Big Tornado
            // Rune_B = Normal Twisters, if two touch, they merge with increased AoE
            // Rune_D = Reduced cost of casting resource

            yield return WaitSeconds(2f);
        }

        [ImplementsPowerBuff(3)]
        class Twister : PowerBuff
        {
            public override void Init()
            {
                Timeout = WaitSeconds(2f);
            }

            public override bool Update()
            {
                if (base.Update())
                    return true;

                return false;
            }
        }
    }
    #endregion

    //TODO: InComplete Runes
    //Unknown if targets are supposed to seizure.. Videos dont show the seezing zombies.
    #region Disintegrate
    [ImplementsPowerSNO(Skills.Skills.Wizard.Offensive.Disintegrate)]
    public class WizardDisintegrate : ChanneledSkill
    {
        //TODOs
        //Rune_A- same as NoRune.. -> todo: add the increase to damage
        //Rune-C- parabola.acr (6523.acr) and field.efg (93563.efg)?
        //Rune-D- Possibly Mini-Buff.efg/.rop and Dome.acr?
        //Rune-E- explode when dead [explode.efg -> 93574], explode_proxy.acr and explodeBubble.acr

        //no idea if sourceglow/pulseglow is used, most likely not.
        //unknown hitfx_override.efg
        //--------------------------------------------------------------------------------------------
        //Rune_A -> Damage increases slowly over time to inflict a maximum of 4620% weapon damage as Arcane.
        //(10) - Chargeup Time, (11) - Dmg Modifier
        //Rune_C -> The beam fractures into a short ranged cone causing 239400% weapon damage per second as Arcane.
        //ScriptFormula(2) - Damage Modifier, (4) - Range, (15) - Tick Period
        //Rune_D -> When casting the beam you become charged with energy that spits out at nearby enemies doing 5700% weapon damage as Arcane.
        //(7) - AOE Weapon Dmg Scalar, (8) - AOE Radius, (21) - Cost Reduction
        //Rune_E -> Enemies killed by the beam have a 35% chance to explode causing 12800% weapon damage as Arcane to all enemies within 8 yards.
        //(9) - Weapon dmg Scalar, (12) - Chance, (25) - Explosion Radius

        const float BeamLength = 40f;

        private Actor _target = null;

        private void _calcTargetPosition()
        {
            // project beam end to always be a certain length
            TargetPosition = PowerMath.TranslateDirection2D(User.Position, TargetPosition,
                                                             new Vector3D(User.Position.X, User.Position.Y, TargetPosition.Z),
                                                             BeamLength);
        }

        public override void OnChannelOpen()
        {
            EffectsPerSecond = 0.1f;

            _calcTargetPosition();
            _target = SpawnEffect(RuneSelect(52687, 52687, 93544, -1, 52687, 215723), TargetPosition, 0, WaitInfinite());
            User.AddComplexEffect(RuneSelect(18792, 18792, 93529, -1, 93593, 216368), _target);
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

        public override IEnumerable<TickTimer> Main()
        {
            UsePrimaryResource(ScriptFormula(22) * EffectsPerSecond);

            foreach (Actor actor in GetEnemiesInRadius(User.Position, BeamLength + 10f).Actors)
            {
                if (Rune_B > 0)
                {
                    if (PowerMath.PointInBeam(actor.Position, User.Position, TargetPosition, 6f))
                    {
                        //ScriptFormula(1)
                        WeaponDamage(actor, ScriptFormula(1) * EffectsPerSecond, DamageType.Arcane);
                    }
                }
                else
                    if (PowerMath.PointInBeam(actor.Position, User.Position, TargetPosition, 3f))
                    {
                        //ScriptFormula(1)
                        WeaponDamage(actor, ScriptFormula(1) * EffectsPerSecond, DamageType.Arcane);
                    }
            }

            yield break;
        }
        [ImplementsPowerBuff(0)]
        class MiniBuff : PowerBuff
        {
            public override void Init()
            {
                Timeout = WaitSeconds(2f);
            }
        }
        [ImplementsPowerBuff(1)]
        class FieldBuff : PowerBuff
        {
            public override void Init()
            {
                Timeout = WaitSeconds(2f);
            }
        }
    }
    #endregion

    //TODO: Repelling Projectiles.
    #region WaveOfForce
    [ImplementsPowerSNO(Skills.Skills.Wizard.Offensive.WaveOfForce)]
    public class WizardWaveOfForce : PowerScript
    {
        public override IEnumerable<TickTimer> Run()
        {
            UsePrimaryResource(ScriptFormula(26));
            StartCooldown(WaitSeconds(ScriptFormula(30)));

            yield return WaitSeconds(0.350f); // wait for wizard to land

            //I switched the effects of obsidian and golden because in-game they are opposite
            User.PlayEffectGroup(RuneSelect(19356, 82649, 215399, 215403, 215400, 215404));

            AttackPayload attack = new AttackPayload(this);
            attack.Targets = GetEnemiesInRadius(User.Position, ScriptFormula(1));
            attack.AddWeaponDamage(ScriptFormula(2), DamageType.Physical);
            //TODO: Script 6,7,8,9 (repels projectiles)
            attack.OnHit = hitPayload =>
            {
                Knockback(hitPayload.Target, ScriptFormula(0), ScriptFormula(4), ScriptFormula(5));
                AddBuff(hitPayload.Target, new DebuffSlowed(ScriptFormula(18), WaitSeconds(ScriptFormula(17))));
                if (Rune_C > 0)
                {
                    if (Rand.NextDouble() < ScriptFormula(14))
                    {
                        foreach (Actor actor in GetEnemiesInRadius(User.Position, ScriptFormula(1)).Actors)
                        {
                            Vector3D targets = RandomDirection(Target.Position, ScriptFormula(12), ScriptFormula(13));
                            SpawnProxy(Target.Position).PlayEffectGroup(77975);
                            actor.Teleport(targets);
                            actor.PlayEffectGroup(77976);
                        }
                    }
                }
                if (Rune_E > 0)
                {
                    Knockback(hitPayload.Target, ScriptFormula(0) + ScriptFormula(11), ScriptFormula(4), ScriptFormula(5));
                    AddBuff(hitPayload.Target, new DebuffStunned(WaitSeconds(ScriptFormula(10))));

                }
                if (Rune_B > 0)
                {
                    if (Rand.NextDouble() < ScriptFormula(16))
                    {
                        User.PlayEffectGroup(92798);
                        attack.AddWeaponDamage(ScriptFormula(23), DamageType.Physical);
                        Knockback(hitPayload.Target, ScriptFormula(0) * ScriptFormula(15), ScriptFormula(4), ScriptFormula(5));
                    }
                }
            };
            attack.Apply();
            yield break;
        }
    }
    #endregion

    //Complete, Rune_E seems slow but correct i guess?
    #region ExplosiveBlast
    [ImplementsPowerSNO(Skills.Skills.Wizard.Offensive.ExplosiveBlast)]
    public class ExplosiveBlast : Skill
    {
        public override IEnumerable<TickTimer> Main()
        {
            Vector3D blastspot = new Vector3D(User.Position);
            Actor blast = SpawnProxy(blastspot);

            if (Rune_D > 0)
            {
                UsePrimaryResource(ScriptFormula(15));
                StartCooldown(WaitSeconds(1f));
                User.PlayEffectGroup(89449);
            }
            else if (Rune_A > 0)
            {
                UsePrimaryResource(ScriptFormula(15));
                StartCooldown(WaitSeconds(1f));
            }
            else if (Rune_C > 0)
            {
                UsePrimaryResource(ScriptFormula(15));
                StartCooldown(WaitSeconds(1f));
                blast.PlayEffectGroup(89449);
            }
            else
            {
                UsePrimaryResource(ScriptFormula(15));
                StartCooldown(WaitSeconds(1f));
                User.PlayEffectGroup(89449);
            }

            yield return WaitSeconds(ScriptFormula(5));

            if (Rune_C > 0)
            {
                SpawnEffect(61419, blastspot);
                AttackPayload attack = new AttackPayload(this);
                attack.Targets = GetEnemiesInRadius(User.Position, ScriptFormula(2));
                attack.AddWeaponDamage(ScriptFormula(0), DamageType.Physical);
                attack.Apply();
                yield break;
            }
            IEnumerable<TickTimer> subScript;
            if (Rune_E > 0)
                subScript = _RuneE();
            else
                //NoRune will actually do the animation and formulas for A,B,D,and NoRune
                subScript = _NoRune();

            foreach (var timeout in subScript)
                yield return timeout;
        }
        IEnumerable<TickTimer> _NoRune()
        {
            SpawnEffect(RuneSelect(61419, 61419, 192210, -1, 192211, -1), User.Position);
            AttackPayload attack = new AttackPayload(this);
            attack.Targets = GetEnemiesInRadius(User.Position, ScriptFormula(2));
            attack.AddWeaponDamage(ScriptFormula(0), DamageType.Physical);
            attack.Apply();
            yield break;
        }
        IEnumerable<TickTimer> _RuneE()
        {
            for (int i = 0; i < (Rune_E + 1); ++i)
            {
                SpawnEffect(61419, User.Position);
                AttackPayload attack = new AttackPayload(this);
                attack.Targets = GetEnemiesInRadius(User.Position, ScriptFormula(2));
                attack.AddWeaponDamage(ScriptFormula(0), DamageType.Physical);
                attack.Apply();
                yield return WaitSeconds(ScriptFormula(14));
            }
            yield break;
        }
    }
    #endregion

    //TODO: All Runes
    #region ArcaneTorrent
    [ImplementsPowerSNO(Skills.Skills.Wizard.Offensive.ArcaneTorrent)]
    public class WizardArcaneTorrent : ChanneledSkill
    {

        private Actor _targetProxy = null;
        private Actor _userProxy = null;

        public override void OnChannelOpen()
        {
            EffectsPerSecond = 0.2f;

            _targetProxy = SpawnEffect(RuneSelect(134595, 170443, 170285, 170830, 170590, 134595), TargetPosition, 0, WaitInfinite());
            _userProxy = SpawnProxy(User.Position, WaitInfinite());
            _userProxy.PlayEffectGroup(RuneSelect(134442, 170263, 170264, 170569, 170572, 164077), _targetProxy);
        }

        public override void OnChannelClose()
        {
            _targetProxy.Destroy();
            _userProxy.Destroy();
        }

        public override IEnumerable<TickTimer> Main()
        {
            UsePrimaryResource(20f * EffectsPerSecond);

            //if (Rune_C > 0)

            /* Instead of firing projectiles, lay Arcane mines that arm after 1 |4second:seconds;. 
             * These mines explode when an enemy approaches, dealing [114 * Casting_Speed_Total * 100]% weapon damage as Arcane. 
             * Enemies caught in the explosion have their movement and attack speeds reduced by 30% for 3 seconds. */

            //else if (Rune_E > 0)

            //Unleash the torrent beyond your control. You can no longer direct where the projectiles go 
            //but their damage is increased to 19900% weapon damage as Arcane.

            //else

            AddBuff(User, new CastEffect());

            Vector3D laggyPosition = new Vector3D(TargetPosition);

            yield return WaitSeconds(0.9f);

            // update proxy target delayed so animation lines up with explosions a bit better
            if (IsChannelOpen)
                TranslateEffect(_targetProxy, laggyPosition, 8f);

            AttackPayload attack = new AttackPayload(this);
            attack.Targets = GetEnemiesInRadius(laggyPosition, 6f);
            attack.AddWeaponDamage(2.00f * EffectsPerSecond, DamageType.Arcane);
            attack.OnHit = hitPayload =>
            {
                if (Rune_A > 0)
                {
                    //enemies get disrupted for six seconds and take 120% additional dmg from arcane attacks.
                }
                if (Rune_D > 0)
                {
                    if (Rand.NextDouble() < ScriptFormula(11))
                    {
                        //spawn a power stone that grants arcane power
                    }
                }
            };
            attack.Apply();

            /*attack.OnDeath = () =>
            {
                if (Rune_B > 0)
                {
                    var proj = new Projectile(this, 170268, hitPayload.Position);
                    proj.Position.Z += 5f;  // fix height
                    proj.OnCollision = (hit) =>
                    {
                        hit.PlayEffectGroup(RuneSelect(19308, 130020, 215580, -1, 216056, -1));
                        WeaponDamage(GetEnemiesInRadius(proj.Position, ScriptFormula(5)), ScriptFormula(3), DamageType.Arcane);
                        proj.Destroy();
                    };
                    proj.Launch(position, ScriptFormula(2));
                    //Enemies killed by Arcane Torrent have a 95% chance to fire a new missile at a nearby enemy dealing 5000% weapon damage as Arcane.
                    //SF(12 -> Spawn Chance
                    //SF(13 -> Weapon Damage
                    //SF(23 -> 
                    //SF(24 -> 25 yard radius
                }
            };*/
        }

        [ImplementsPowerBuff(0)]
        class CastEffect : PowerBuff
        {
            public override void Init()
            {
                Timeout = WaitSeconds(0.3f);
            }
        }
        [ImplementsPowerBuff(1)]
        class Crimson_DestablizedEffect : PowerBuff
        {
            public override void Init()
            {
                Timeout = WaitSeconds(2f);
            }
        }
    }
    #endregion

    //TODO: Rune_A,E
    #region FrostNova
    //bumbasher
    [ImplementsPowerSNO(Skills.Skills.Wizard.Utility.FrostNova)]
    public class WizardFrostNova : PowerScript
    {
        //Rune_A - Enemies take 110% more damage while frozen or chilled by Frost Nova.
        //Rune_B - frozen enemy that is killed has a 21% chance of exploding another frost nova.
        //Rune_E - If Frost Nova hits at least 5 targets, you gain 45% chance to critically hit for 12 seconds
        public override IEnumerable<TickTimer> Run()
        {
            if (Rune_C > 0)
            {
                StartCooldown(WaitSeconds(ScriptFormula(3)));
                var frozenMist = SpawnEffect(RuneSelect(4402, 189047, 189048, 75631, 189049, 189050), User.Position, 0, WaitSeconds(ScriptFormula(9)));
                frozenMist.UpdateDelay = 1f;
                frozenMist.OnUpdate = () =>
                {
                    AttackPayload attack = new AttackPayload(this);
                    attack.Targets = GetEnemiesInRadius(User.Position, ScriptFormula(6));
                    attack.AddWeaponDamage(ScriptFormula(11), DamageType.Cold);
                    attack.OnHit = hitPayload =>
                    {
                        AddBuff(hitPayload.Target, new DebuffChilled(ScriptFormula(5), WaitSeconds(ScriptFormula(9))));
                    };
                    attack.Apply();
                };
            }
            else
            {
                StartCooldown(WaitSeconds(ScriptFormula(3)));
                SpawnEffect(RuneSelect(4402, 189047, 189048, 75631, 189049, 189050), User.Position);
                AttackPayload attack = new AttackPayload(this);
                attack.Targets = GetEnemiesInRadius(User.Position, ScriptFormula(6));
                attack.AddWeaponDamage(0.65f, DamageType.Cold);
                attack.OnDeath = hitPayload =>
                {
                    //TODO:Add in "if Target is frozen".
                    if (Rune_B > 0)
                    {
                        if (Rand.NextDouble() < ScriptFormula(14))
                        {
                            //TODO:Does this work? Target.Position, will that get the target that dies?
                            SpawnEffect(189048, Target.Position);
                            WeaponDamage(GetEnemiesInRadius(Target.Position, ScriptFormula(15)), ScriptFormula(7), DamageType.Cold);
                        }
                    }
                };
                attack.Apply();

                //TODO: freeze duration(ScriptFormula(2))

                if (Rune_A > 0)
                {
                    //todo:while frozen or chilled enemies, Rune_A -> ScriptFormula(16)
                }

                if (Rune_E > 0)
                {
                    //TODO: Add "If Frost Nova hits at least 5 targets, [ScriptFormula(13)]"
                    if (Rand.NextDouble() < ((Rune_E * 5) + .10f))
                    {
                        //critically hit for 12 seconds
                        //[ScriptFormula(18) / ScriptFormula(19)]
                        //critBuff_swipe.acr [[215516]] -> this is possibly added when applying buff.
                    }
                }
            }
            yield break;
        }
        [ImplementsPowerBuff(5)]
        class FrostNova_Alabaster_Buff : PowerBuff
        {
            //crit hit stuff goes here.

            public override void Init()
            {
                Timeout = WaitSeconds(2f);
            }
        }
    }
    #endregion

    //Complete
    #region Blizzard
    [ImplementsPowerSNO(Skills.Skills.Wizard.Offensive.Blizzard)]
    public class WizardBlizzard : PowerScript
    {
        public const int Wizard_Blizzard = 0x1977;

        public override IEnumerable<TickTimer> Run()
        {
            UsePrimaryResource(ScriptFormula(19));

            SpawnEffect(Wizard_Blizzard, TargetPosition);

            for (int i = 0; i < ScriptFormula(4); ++i)
            {
                AttackPayload attack = new AttackPayload(this);
                attack.Targets = GetEnemiesInRadius(TargetPosition, ScriptFormula(3));
                attack.AddWeaponDamage(ScriptFormula(0), DamageType.Cold);
                attack.OnHit = (hit) =>
                {
                    //seems like this should be obvious to have in Blizzard.
                    AddBuff(hit.Target, new DebuffChilled(0.5f, WaitSeconds(3f)));

                    if (Rune_E > 0)
                    {
                        //Crit Strike Chance(ScriptFormula(9)) -> there is no Crit Strike Chance, so i've used Crit Damage Percent..
                        AddBuff(User, new BlizzardPowers(WaitSeconds(3f)));
                        if (Rand.NextDouble() < ScriptFormula(10))
                        {
                            {
                                AddBuff(hit.Target, new DebuffFrozen(WaitSeconds(3f)));
                            }
                        }
                    }
                };
                attack.Apply();

                yield return WaitSeconds(1f);
            }
            if (Rune_C > 0)
            {
                var BlizzMist = SpawnEffect(75642, User.Position, 0, WaitSeconds(ScriptFormula(7)));
                BlizzMist.UpdateDelay = 0.5f;
                BlizzMist.OnUpdate = () =>
                {
                    AttackPayload attack = new AttackPayload(this);
                    attack.Targets = GetEnemiesInRadius(TargetPosition, ScriptFormula(3));
                    attack.AddWeaponDamage(0f, DamageType.Cold);
                    attack.OnHit = (hit) =>
                    {
                        AddBuff(hit.Target, new DebuffChilled(0.75f, WaitSeconds(ScriptFormula(20))));
                    };
                };
            }
        }
    }
    #endregion

    //TODO: Rune_A,B,C,E
    #region RayOfFrost
    [ImplementsPowerSNO(Skills.Skills.Wizard.Offensive.RayOfFrost)]
    public class WizardRayOfFrost : ChanneledSkill
    {
        //Rune_B - Create a swirling storm of sleet dealing [99 * 100}]% weapon damage as Cold to all enemies caught within it.

        //We need to change how frost beam powermath works -> Beam radius (SF(10)) and Beam end Radius (SF(11)), currently the function has a weird visual
        //for other rune effects with a wider starting effect

        const float BeamLength = 40f;

        private Actor _target = null;

        private void _calcTargetPosition()
        {
            // project beam end to always be a certain length
            TargetPosition = PowerMath.TranslateDirection2D(User.Position, TargetPosition,
                                                             new Vector3D(User.Position.X, User.Position.Y, TargetPosition.Z),
                                                             BeamLength);
        }

        public override void OnChannelOpen()
        {
            EffectsPerSecond = 0.1f;
            _calcTargetPosition();
            _target = SpawnEffect(6535, TargetPosition, 0, WaitInfinite());
            User.AddComplexEffect(RuneSelect(19327, 149835, -1, 149836, 149869, 149879), _target);

            //148061 - swirling storm, is more of a user.playeffectgroup and not a rope
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

        public override IEnumerable<TickTimer> Main()
        {
            //Rune_D
            UsePrimaryResource((Math.Max(ScriptFormula(19), 8f)) * EffectsPerSecond);

            foreach (Actor actor in GetEnemiesInRadius(User.Position, BeamLength + 10f).Actors)
            {
                if (PowerMath.PointInBeam(actor.Position, User.Position, TargetPosition, 3f))
                {
                    if (Rune_A > 0)
                    {
                        //takes 1.5 seconds to reach the new maximum dmg(SF(20)) from the minimum dmg(base?)
                        //Slows targets movement by 40%
                        //targets attack speed by 30% for 5 seconds

                    }
                    else if (Rune_C > 0)
                    {
                        //WeaponDamage(actor, 2.70f * EffectsPerSecond, DamageType.Cold);
                        //Slows targets movement [ScriptFormula(4)]
                        //Chill Amount % {ScriptFormula(14)}
                        //Atk Speed Reduction % {SF(24)}
                        //Dmg Reduction {SF(25)}
                        //targets attack speed by 30% for 5 seconds
                    }
                    WeaponDamage(actor, 2.70f * EffectsPerSecond, DamageType.Cold);
                    //Slows targets movement by 40%
                    //targets attack speed by 30% for 5 seconds
                }
            }

            //Rune_E - Enemies who die leave a patch of ice on the ground that causes 64500% weapon damage as Cold to enemies moving through it over 5 seconds.
            //if enemy dies {
            //AddBuff(Hit.Target, new icyPatchBuff());
            //playeffectgroup(149878);
            //OnUpdate
            //WeaponDamage(actor, 2.70f * EffectsPerSecond, DamageType.Cold);
            //E: Ground Ice Dps (SF(3))
            //E: Ground Ice Lifetime (SF(8))
            //E: Ground Ice Chance (SF(9))
            //E: Ground Ice Refresh Interval (SF(13))
            //}

            yield break;
        }

        [ImplementsPowerBuff(1)]
        class IceDomeBuff : PowerBuff
        {
            //Rune_C
            public override void Init()
            {
                Timeout = WaitSeconds(2f);
            }
        }
    }
    #endregion

    //TODO: Rune_B -> mirror images
    #region Teleport
    [ImplementsPowerSNO(Skills.Skills.Wizard.Utility.Teleport)]
    public class WizardTeleport : PowerScript
    {
        public override IEnumerable<TickTimer> Run()
        {
            UsePrimaryResource(15f);
            if (!(Rune_E > 0 || Rune_D > 0))
            {
                StartCooldown(WaitSeconds(ScriptFormula(20)));
            }

            if (Rune_D > 0)
            {
                TeleRevertBuff buff = User.World.BuffManager.GetFirstBuff<TeleRevertBuff>(User);
                if (buff != null)
                {
                    User.PlayEffectGroup(RuneSelect(170232, 170232, 170232, 192053, 192080, 192152));
                    yield return WaitSeconds(0.3f);
                    User.Teleport(buff.OrigSpot);
                    User.PlayEffectGroup(RuneSelect(170232, 170232, 170232, 192053, 192080, 192152));
                    buff.Remove(); // Ensures that you can only revert the teleport once.
                }
                else
                {
                    Vector3D OrigSpot;
                    Actor OrigTele;
                    OrigSpot = new Vector3D(User.Position.X, User.Position.Y, User.Position.Z);
                    OrigTele = SpawnProxy(OrigSpot, WaitSeconds(ScriptFormula(18)));
                    OrigTele.PlayEffectGroup(RuneSelect(170231, 205685, 205684, 191913, 192074, 192151));
                    OrigTele.PlayEffectGroup(206679);
                    AddBuff(User, new TeleRevertBuff(OrigSpot, OrigTele));
                    yield return WaitSeconds(0.3f);
                    User.Teleport(TargetPosition);
                    User.PlayEffectGroup(RuneSelect(170232, 170232, 170232, 192053, 192080, 192152));
                }

            }
            else
            {
                SpawnProxy(User.Position).PlayEffectGroup(RuneSelect(170231, 205685, 205684, 191913, 192074, 192151));  // alt cast efg: 170231
                yield return WaitSeconds(0.3f);
                User.Teleport(TargetPosition);
                //MDZ says this might work just as 191849.
                User.PlayEffectGroup(RuneSelect(170232, 170232, 170232, 192053, 192080, 192152));
            }

            if (Rune_A > 0)
            {
                User.PlayEffectGroup(170289);
                AttackPayload attack = new AttackPayload(this);
                attack.Targets = GetEnemiesInRadius(User.Position, ScriptFormula(1));
                attack.AddWeaponDamage(ScriptFormula(2), DamageType.Physical);
                attack.OnHit = hitPayload =>
                {
                    Knockback(hitPayload.Target, ScriptFormula(4), ScriptFormula(5), ScriptFormula(6));
                };
                attack.Apply();

            }
            if (Rune_B > 0)
            {
                //Rune_B - Summon 2 mirror images for 15 |4second:seconds; on arrival.
                //SF(7,8,9,10,11)
            }
            if (Rune_C > 0)
            {
                AddBuff(User, new TeleDmgReductionBuff());
            }
            if (Rune_E > 0)
            {
                AddBuff(User, new TeleCoolDownBuff());
            }
        }
        [ImplementsPowerBuff(1)]
        class TeleDmgReductionBuff : PowerBuff
        {
            public override void Init() { Timeout = WaitSeconds(ScriptFormula(15)); }

            public override bool Apply()
            {
                if (!base.Apply())
                    return false;
                //gameattribute damage reduction, Absorb should do the same thing?
                Target.Attributes[GameAttribute.Damage_Absorb_Percent] += ScriptFormula(14);
                Target.Attributes.BroadcastChangedIfRevealed();
                return true;
            }

            public override void Remove()
            {
                base.Remove();
                Target.Attributes[GameAttribute.Damage_Absorb_Percent] -= ScriptFormula(14);
                Target.Attributes.BroadcastChangedIfRevealed();

            }
        }
        [ImplementsPowerBuff(5)]
        class TeleRevertBuff : PowerBuff
        {
            public Vector3D OrigSpot;
            public Actor OrigTele;

            public TeleRevertBuff(Vector3D OrigSpot, Actor OrigTele)
            {
                this.OrigSpot = OrigSpot;
                this.OrigTele = OrigTele;
            }

            public override void Init()
            {
                Timeout = WaitSeconds(ScriptFormula(18));
            }
            public override bool Apply()
            {
                if (!base.Apply())
                    return false;
                return true;
            }
            public override void Remove()
            {
                Timeout.Stop();
                //OrigTele.Destroy();  --   Removes the voidzone effect as though, but also throws an exception. 
                //                          Perhaps one cannot Destroy() a proxy actor, 
                //                          but is there any other way to quit the effectgroup early?
                base.Remove();
                StartCooldown(WaitSeconds(ScriptFormula(20)));
            }
        }
        [ImplementsPowerBuff(5)]
        class TeleCoolDownBuff : PowerBuff
        {
            public override void Init()
            {
                Timeout = WaitSeconds(ScriptFormula(16));
            }
            public override void Remove()
            {
                base.Remove();
                StartCooldown(WaitSeconds(ScriptFormula(20)));
            }
        }
    }
    #endregion

    //TODO: Rune_A -> time delay between bleeds? its 3 seconds, damage per second.
    //TODO: Rune_E
    #region SpectralBlade
    [ImplementsPowerSNO(Skills.Skills.Wizard.Signature.SpectralBlade)]
    public class WizardSpectralBlade : PowerScript
    {
        public override IEnumerable<TickTimer> Run()
        {

            UsePrimaryResource(15f);
            //these are changed around to actually identify with their rune color : visual effects
            User.PlayEffectGroup(RuneSelect(19343, 189477, 19343, 189413, 188944, 189362));

            // calculate hit area of effect, just in front of the user
            //(SF2)Pie Angle:60 || (SF3)Total:Pie Radius
            TargetPosition = PowerMath.TranslateDirection2D(User.Position, TargetPosition, User.Position, 9f);

            for (int n = 0; n < 3; ++n)
            {
                AttackPayload attack = new AttackPayload(this);
                attack.Targets = GetEnemiesInRadius(TargetPosition, ScriptFormula(3));
                attack.AddWeaponDamage(ScriptFormula(21), DamageType.Physical);
                attack.OnHit = hitPayload =>
                {
                    if (Rune_A > 0)
                    {
                        AddBuff(hitPayload.Target, new BleedEffect());
                    }

                    if (Rune_C > 0)
                    {
                        if (Rand.NextDouble() < ScriptFormula(19))
                        {
                            Knockback(Target, ScriptFormula(14));
                            AddBuff(hitPayload.Target, new DebuffSlowed(0.6f, WaitSeconds(ScriptFormula(11))));
                        }
                    }
                    if (Rune_D > 0)
                    {
                        GeneratePrimaryResource(ScriptFormula(13));
                    }
                    if (Rune_E > 0)
                    {
                        if (hitPayload.IsCriticalHit)
                        {
                            //hitPayload.TotalDamage
                            //E:Whenever the blades do critical damage, you are healed [1 * 100|1|]% of the damage caused.
                        }
                    }
                };
                attack.Apply();
                yield return WaitSeconds(0.2f);
            }
        }
        [ImplementsPowerBuff(2)]
        class BleedEffect : PowerBuff
        {
            public override void Init()
            {
                Timeout = WaitSeconds(ScriptFormula(6));
            }
            public override bool Update()
            {
                if (base.Update())
                    return true;

                AttackPayload bleed = new AttackPayload(this);
                //Script for damage may be off. please check.
                bleed.AddWeaponDamage(ScriptFormula(4), DamageType.Physical);
                bleed.Apply();
                return false;
            }
        }
    }
    #endregion

    //TODO: make ArmorBuff, Rune_C,E
    //TODO: auras need to Update every 1-2 seconds..
    //TODO: Someone cancel Iceblade effect, which is Rune_C
    #region Icearmor
    [ImplementsPowerSNO(Skills.Skills.Wizard.Utility.IceArmor)]
    public class IceArmor : Skill
    {
        public override IEnumerable<TickTimer> Main()
        {
            StartDefaultCooldown();
            UsePrimaryResource(25f);
            AddBuff(User, new IceArmorBuff());
            if (Rune_C > 0)
            {
                User.PlayEffectGroup(87899);
                AttackPayload chillingAura = new AttackPayload(this);
                chillingAura.Targets = GetEnemiesInRadius(User.Position, ScriptFormula(7));
                chillingAura.AddWeaponDamage(ScriptFormula(9), DamageType.Cold);
                chillingAura.OnHit = (hit) =>
                {
                    AddBuff(hit.Target, new DebuffChilled(0.5f, WaitSeconds(ScriptFormula(23))));
                };
                chillingAura.Apply();
            }
            else
            {
                AttackPayload chillingAura = new AttackPayload(this);
                chillingAura.Targets = GetEnemiesInRadius(User.Position, ScriptFormula(7));
                chillingAura.AddWeaponDamage(ScriptFormula(0), DamageType.Cold);
                chillingAura.OnHit = (hit) =>
                {
                    AddBuff(hit.Target, new DebuffChilled(0.5f, WaitSeconds(ScriptFormula(23))));
                    if (Rune_E > 0)
                    {
                        //if(hit. melee attack
                        if (Rand.NextDouble() < ScriptFormula(12))
                        {
                            hit.Target.PlayEffectGroup(19321);
                            AttackPayload frostNova = new AttackPayload(this);
                            frostNova.Targets = GetEnemiesInRadius(hit.Target.Position, ScriptFormula(6));
                            frostNova.AddWeaponDamage(ScriptFormula(19) * ScriptFormula(13), DamageType.Cold);
                            frostNova.OnHit = hitPayload =>
                            {
                                AddBuff(hitPayload.Target, new DebuffFrozen(WaitSeconds(ScriptFormula(16))));
                            };
                            frostNova.Apply();
                        }
                    }
                };
                chillingAura.Apply();
            }
            yield break;
        }

        [ImplementsPowerBuff(1)]
        class IceArmorBuff : PowerBuff
        {
            public override void Init()
            {
                Timeout = WaitSeconds(ScriptFormula(3));
                MaxStackCount = (int)ScriptFormula(11);
            }

            public override bool Apply()
            {
                if (!base.Apply())
                    return false;
                User.Attributes[GameAttribute.Armor_Item_Percent] += ScriptFormula(2);
                User.Attributes.BroadcastChangedIfRevealed();

                return true;
            }

            public override void OnPayload(Payload payload)
            {
                if (payload.Target == Target && payload is HitPayload)
                {
                    if (Rune_D > 0)
                    {
                        _AddArmor();
                    }
                    if (Rune_A > 0)
                    {
                        WeaponDamage(payload.Context.Target, ScriptFormula(0), DamageType.Cold);
                        AddBuff(payload.Context.Target, new DebuffChilled(0.5f, WaitSeconds(ScriptFormula(4))));
                    }
                }
            }
            public override bool Stack(Buff buff)
            {
                bool stacked = StackCount < MaxStackCount;

                base.Stack(buff);

                if (stacked)
                    _AddArmor();

                return true;
            }

            public override void Remove()
            {
                base.Remove();
                User.Attributes[GameAttribute.Armor_Item_Percent] -= ScriptFormula(2);
                User.Attributes.BroadcastChangedIfRevealed();
                User.PlayEffectGroup(19326);
                User.PlayEffectGroup(185652);

            }

            private void _AddArmor()
            {
                AddBuff(User, new BonusStackEffect());
            }
        }
        [ImplementsPowerBuff(0)]
        class BonusStackEffect : PowerBuff
        {
            public override void Init()
            {
                Timeout = WaitSeconds(ScriptFormula(27));
            }
            public override bool Apply()
            {
                if (!base.Apply())
                    return false;
                User.Attributes[GameAttribute.Armor_Item_Percent] += ScriptFormula(26);
                User.Attributes.BroadcastChangedIfRevealed();

                return true;
            }
            public override void Remove()
            {
                base.Remove();
                User.Attributes[GameAttribute.Armor_Item_Percent] -= StackCount * ScriptFormula(26);
                User.Attributes.BroadcastChangedIfRevealed();

            }
        }
    }
    #endregion

    //Incomplete
    #region ShockPulse
    [ImplementsPowerSNO(Skills.Skills.Wizard.Signature.ShockPulse)]
    public class WizardShockPulse : PowerScript
    {
        //A:Casts out bolts of fire to deal 195% weapon damage as Fire. 
        //B:Turn the bolts into a floating orb of static lightning that drifts directly forward, zapping up to 5 nearby enemies for 46% weapon damage as Lightning. 
        //C:Merge the bolts in a a single giant orb that oscillates forward dealing 95% weapon damage as Lightning to everything it hits with a 100% chance to pierce through enemies. 
        //DONE -> D:Every target hit by a pulse restores 7 Arcane Power. 
        //E:Slain enemies have a 100% chance to explode dealing 450% weapon damage as Lightning to every enemy within 10 yards. 
        public override IEnumerable<TickTimer> Run()
        {
            UsePrimaryResource(ScriptFormula(13));
            User.PlayEffectGroup(RuneSelect(176277, 176290, 176354, 176355, 176250, 176353)); // cast effect
            if (Rune_B > 0 || Rune_C > 0)
            {
                _SpawnBolt();
            }
            else
            {
                for (int n = 0; n < 3; ++n)
                    _SpawnBolt();
            }

            yield break;
        }

        private void _SpawnBolt()
        {
            var eff = SpawnEffect(RuneSelect(176247, 176287, 176653, 201526, 176248, 176356), User.Position, 0, WaitSeconds(ScriptFormula(25)));

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
                Field5 = MovementHelpers.GetFacingAngle(User.Position, TargetPosition), // facing angle
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

            /*
             * private void _calcTargetPosition()
        {
            // project beam end to always be a certain length
            TargetPosition = PowerMath.TranslateDirection2D(User.Position, TargetPosition,
                                                             new Vector3D(User.Position.X, User.Position.Y, TargetPosition.Z),
                                                             50f);
        }
             * _calcTargetPosition();

            
            if (Rune_B > 0 || Rune_C > 0)
            {
                bolts = 1;
            }
            else bolts = 3;

            Vector3D[] projDestinations = PowerMath.GenerateSpreadPositions(User.Position, TargetPosition, 10f, bolts);

                foreach (Vector3D missilePos in projDestinations)
                {
                    var proj = new Projectile(this, RuneSelect(176247, 176287, 176653, 201526, 176248, 176356), User.Position);
                    proj.OnCollision = (hit) =>
                    {
                        SpawnEffect(RuneSelect(176247, 176287, 176653, 201526, 176248, 176356), new Vector3D(hit.Position.X, hit.Position.Y, hit.Position.Z + 5f)); // impact effect (fix height)
                        if (Rune_B > 0)
                        {
                            WeaponDamage(GetEnemiesInRadius(proj.Position, ScriptFormula(21)), ScriptFormula(4), DamageType.Lightning);
                        }
                        else
                        {
                            WeaponDamage(hit, ScriptFormula(4), DamageType.Lightning);
                        }
                    };
                    proj.Launch(missilePos, ScriptFormula(6));

                    WaitSeconds(ScriptFormula(25));
                }*/

        }
    }
    #endregion

    //TODO: Rune_C and Rune_E
    //TODO: shock range and melee attacks for weapon damage as lightning (not in update. make your own update in Main())
    #region StormArmor
    [ImplementsPowerSNO(Skills.Skills.Wizard.Utility.StormArmor)]
    public class StormArmor : Skill
    {
        public override IEnumerable<TickTimer> Main()
        {
            StartDefaultCooldown();
            UsePrimaryResource(25f);
            AddBuff(User, new StormArmorBuff());
            yield break;
        }

        [ImplementsPowerBuff(0)]
        class StormArmorBuff : PowerBuff
        {
            public override void Init()
            {
                Timeout = WaitSeconds(120f);
            }

            public override bool Apply()
            {
                if (!base.Apply())
                    return false;

                if (Rune_D > 0)
                {
                    User.Attributes[GameAttribute.Resource_Cost_Reduction_Amount] += 7;
                    User.Attributes.BroadcastChangedIfRevealed();
                }
                return true;
            }

            public override void OnPayload(Payload payload)
            {
                if (Rune_B > 0)
                {
                    if (payload.Target == Target && payload is HitPayload)
                    {
                        AddBuff(User, new IndigoBuff());
                        AddBuff(User, new MovementBuff(ScriptFormula(14), WaitSeconds(ScriptFormula(20))));
                    }
                }
                if (Rune_C > 0)
                {
                    if (payload.Target == Target && payload is HitPayload)
                    {
                        AddBuff(User, new TeslaBuff());
                    }
                }
            }

            public override void Remove()
            {
                if (Rune_D > 0)
                {
                    User.Attributes[GameAttribute.Resource_Cost_Reduction_Amount] -= 7;
                    User.Attributes.BroadcastChangedIfRevealed();
                }
                base.Remove();
            }
        }
        [ImplementsPowerBuff(1)]
        class TeslaBuff : PowerBuff
        {
            //Rune_C

            //you have a chance to be enveloped with a lightning shield for 8 seconds 
            //that shocks nearby enemies for 120% weapon damage as Lightning.

            //TeslaStrike.acr 80600
            //tslaBolt.rop 80602
            public override void Init()
            {
                Timeout = WaitSeconds(8f);
            }
            public override bool Apply()
            {
                if (!base.Apply())
                    return false;
                return true;
            }

            public override bool Update()
            {
                if (base.Update())
                    return true;
                return false;
            }

            public override void Remove()
            {
                base.Remove();
            }
        }
        [ImplementsPowerBuff(2)]
        class IndigoBuff : PowerBuff
        {
            public override void Init()
            {
                Timeout = WaitSeconds(5f);
            }
        }
    }
    #endregion

    //Rune_B+D+E:Done
    //TODO:Rune_A -> reflects damage
    //TODO:NoRune + Rune_C -> Absorbtion, C: increases absorbtion, done within ScriptFormula(0)
    #region DiamondSkin
    [ImplementsPowerSNO(Skills.Skills.Wizard.Utility.DiamondSkin)]
    public class DiamondSkin : Skill
    {
        public override IEnumerable<TickTimer> Main()
        {
            StartDefaultCooldown();
            //UsePrimaryResource(25f);
            AddBuff(User, new DiamondSkinBuff());
            yield break;
        }

        [ImplementsPowerBuff(0)]
        class DiamondSkinBuff : PowerBuff
        {
            public override void Init()
            {
                Timeout = WaitSeconds(ScriptFormula(5));
            }

            public override bool Apply()
            {
                if (!base.Apply())
                    return false;
                User.Attributes[Mooege.Net.GS.Message.GameAttribute.Look_Override] = 0x061F7489;
                //User.Attributes[GameAttribute.Breakable_Shield_HP] += ScriptFormula(0);
                User.Attributes.BroadcastChangedIfRevealed();
                if (Rune_D > 0)
                {
                    User.Attributes[GameAttribute.Resource_Cost_Reduction_Amount] += (int)ScriptFormula(4);
                    User.Attributes.BroadcastChangedIfRevealed();
                }
                return true;
            }

            public override void OnPayload(Payload payload)
            {
                if (payload.Target == Target && payload is HitPayload)
                {
                    if (Rune_A > 0)
                    {
                        //Reflect Damage back to Target SF(1)
                    }
                    //Absorb Damge (NoRune and Rune_C) -> ScriptFormula(0) or is it ScriptFormula(39)
                }
            }

            public override void Remove()
            {
                base.Remove();
                User.Attributes[Mooege.Net.GS.Message.GameAttribute.Look_Override] = 0;
                User.PlayEffectGroup(RuneSelect(93077, 187716, 187805, 187822, 187831, 187851));
                //User.Attributes[GameAttribute.Breakable_Shield_HP] -= ScriptFormula(0);
                User.Attributes.BroadcastChangedIfRevealed();
                if (Rune_D > 0)
                {
                    User.Attributes[GameAttribute.Resource_Cost_Reduction_Amount] -= (int)ScriptFormula(4);
                    User.Attributes.BroadcastChangedIfRevealed();
                }
                if (Rune_E > 0)
                {
                    User.PlayEffectGroup(92957);
                    WeaponDamage(GetEnemiesInRadius(User.Position, ScriptFormula(6)), ScriptFormula(2), DamageType.Physical);
                }
            }
        }
        [ImplementsPowerBuff(1)]
        class StoneSkinBuff : PowerBuff
        {
            public override void Init()
            {
                Timeout = WaitSeconds(ScriptFormula(5));
            }
        }
        [ImplementsPowerBuff(2)]
        class StoneArmorBuff : PowerBuff
        {
            public override void Init()
            {
                Timeout = WaitSeconds(ScriptFormula(5));
            }
        }
    }
    #endregion

    //TODO: make sure things in Update() don't get created over and over again.
    //TODO:Rune_B: create an outer ring and only get that outer ring of targets.
    #region SlowTime
    [ImplementsPowerSNO(Skills.Skills.Wizard.Utility.SlowTime)]
    public class SlowTime : Skill
    {
        public override IEnumerable<TickTimer> Main()
        {
            if (Rune_D > 0)
            {
                StartCooldown(ScriptFormula(15) - ScriptFormula(14));
                UsePrimaryResource(25f);
                AddBuff(User, new SlowTimeBuff());
                yield break;
            }
            else
                StartCooldown(ScriptFormula(15));
            UsePrimaryResource(25f);
            AddBuff(User, new SlowTimeBuff());
            yield break;
        }

        [ImplementsPowerBuff(0)]
        class SlowTimeBuff : PowerBuff
        {
            public override void Init()
            {
                Timeout = WaitSeconds(ScriptFormula(11));
            }

            public override bool Apply()
            {
                if (!base.Apply())
                    return false;

                SpawnEffect(RuneSelect(6553, 112585, 112808, 112560, 112572, 112697), User.Position);
                return true;
            }

            public override bool Update()
            {
                if (base.Update())
                {
                    return true;
                }
                var targets = GetEnemiesInRadius(User.Position, ScriptFormula(2));
                if (targets.Actors.Count > 0)
                {
                    foreach (Actor actor in targets.Actors)
                    {
                        AddBuff(actor, new SlowTimeDebuff(ScriptFormula(3), WaitSeconds(ScriptFormula(0))));
                        if (Rune_A > 0)
                        {
                            AddBuff(actor, new AttackDamageBuff());
                        }
                    }
                }
                if (Rune_E > 0)
                {
                    var friendlytargets = GetAlliesInRadius(User.Position, ScriptFormula(2));
                    if (friendlytargets.Actors.Count > 0)
                    {
                        foreach (Actor actor in friendlytargets.Actors)
                        {
                            AddBuff(actor, new SpeedBuff(ScriptFormula(16), WaitSeconds(ScriptFormula(0))));
                        }
                    }
                }
                if (Rune_B > 0)
                {
                    var OutOfRangeTargets = GetAlliesInRadius(User.Position, ScriptFormula(2) + 2f);
                    foreach (Actor actor in OutOfRangeTargets.Actors)
                    {
                        AddBuff(actor, new SlowTimeDebuff(ScriptFormula(3), WaitSeconds(ScriptFormula(7))));
                    }
                }
                return false;
            }

            public override void Remove()
            {
                base.Remove();

            }
        }
        [ImplementsPowerBuff(1)]
        class AttackDamageBuff : PowerBuff
        {
            public override void Init()
            {
                Timeout = WaitSeconds(ScriptFormula(0));
            }

            public override bool Apply()
            {
                if (!base.Apply())
                    return false;
                Target.Attributes[GameAttribute.Attack_Bonus_Percent] += ScriptFormula(8);
                return true;
            }
            public override void Remove()
            {
                base.Remove();
                Target.Attributes[GameAttribute.Attack_Bonus_Percent] -= ScriptFormula(8);
            }
        }
    }
    #endregion

    //TODO: finish OnPayload
    #region EnergyArmor
    [ImplementsPowerSNO(Skills.Skills.Wizard.Utility.EnergyArmor)]
    public class EnergyArmor : Skill
    {
        public override IEnumerable<TickTimer> Main()
        {
            StartDefaultCooldown();
            UsePrimaryResource(25f);
            AddBuff(User, new EnergyArmorBuff());
            yield break;
        }

        [ImplementsPowerBuff(0)]
        class EnergyArmorBuff : PowerBuff
        {
            public override void Init()
            {
                Timeout = WaitSeconds(ScriptFormula(0));
            }

            public override bool Apply()
            {
                if (!base.Apply())
                    return false;
                AddBuff(User, new EnergyArmorPowers(WaitSeconds(ScriptFormula(0))));
                return true;
            }

            public override void OnPayload(Payload payload)
            {
                if (payload.Target == Target && payload is HitPayload)
                {
                    if (Rune_C > 0)
                    {
                        //ScriptFormula(8) = % of Wizard Total Health -> 0.54 - (Rune_C * 0.04)
                        //Math.Min(Total Damage incoming/Maximum Life, 26% of Maximum Life)
                        float HPamount = User.Attributes[GameAttribute.Hitpoints_Max_Total];
                        float TargetHit = payload.Context.Target.Attributes[GameAttribute.Get_Hit_Current];
                        float NewDamage = Math.Min(TargetHit / HPamount, ScriptFormula(8) / HPamount);
                    }
                    if (Rune_D > 0)
                    {
                        //just a guess.
                        if (Rand.NextDouble() < .4f)
                        {
                            GeneratePrimaryResource(ScriptFormula(11));
                        }
                    }

                    //drain __ primary resource for every 1% of your maximum Life absorbed.
                }
            }

            public override void Remove()
            {
                base.Remove();
            }
        }
        [ImplementsPowerBuff(2)]
        class EnergyReflectBuff : PowerBuff
        {
            public override void Init()
            {
                Timeout = WaitSeconds(2f);
            }
        }
        [ImplementsPowerBuff(3)]
        class EnergyResourceBuff : PowerBuff
        {
            public override void Init()
            {
                Timeout = WaitSeconds(2f);
            }
        }
        [ImplementsPowerBuff(4)]
        class EnergyAbsorbBuff : PowerBuff
        {
            public override void Init()
            {
                Timeout = WaitSeconds(2f);
            }
        }
        [ImplementsPowerBuff(5)]
        class EnergyDamageBuff : PowerBuff
        {
            public override void Init()
            {
                Timeout = WaitSeconds(2f);
            }
        }
    }
    #endregion

    //TODO: Rune_A, E
    #region MagicWeapon
    [ImplementsPowerSNO(Skills.Skills.Wizard.Utility.MagicWeapon)]
    public class MagicWeapon : Skill
    {
        public override IEnumerable<TickTimer> Main()
        {
            StartDefaultCooldown();
            UsePrimaryResource(25f);
            AddBuff(User, new MagicWeaponBuff());
            yield break;
        }

        [ImplementsPowerBuff(0)]
        class MagicWeaponBuff : PowerBuff
        {
            public override void Init()
            {
                Timeout = WaitSeconds(60f);
            }

            public override bool Apply()
            {
                if (!base.Apply())
                    return false;
                User.PlayEffectGroup(RuneSelect(218923, 219289, 219306, 219390, 219396, 219338));
                User.Attributes[GameAttribute.Damage_Weapon_Percent_Bonus] += ScriptFormula(14);
                return true;
            }

            public override void Remove()
            {
                base.Remove();
                User.Attributes[GameAttribute.Damage_Weapon_Percent_Bonus] -= ScriptFormula(14);
            }

            public override void OnPayload(Payload payload)
            {
                if (User != null && payload.Target != null
                    && payload.Context.Target != null
                    && payload.Context.User != null)
                {
                    if (payload is AttackPayload && !payload.Context.Target.Equals(User)
                        && payload.Context.User.Equals(User) && (payload.Context.PowerSNO.CompareTo(0x00007780) == 0))
                    //TODO: add detection for ranged attacks here, if Magic Weapon affects wands.
                    {
                        AttackPayload lastAttack = (AttackPayload)payload;

                        if (Rune_A > 0)
                        {
                            //TODO:
                            //ScriptFormula(3 & 4 & 5)
                            //poison enemies for 3 seconds, dealing 70% of weapon damage
                        }
                        if (Rune_B > 0) //Note: this implementation presumes that all lightning arcs will start at the soruce target, and then go only to their respective targets.
                        //If it turns out that it's instead a chain-lightning-like mechanic, just redesign so that after each iteration, ropeSource becomes curTarget.
                        {
                            //TODO: find correct chance, formulas didn't specify.
                            if (Rand.NextDouble() < 1)
                            {
                                //TODO: find correct radius for "nearby".
                                TargetList targets = GetEnemiesInRadius(lastAttack.Context.Target.Position, 10f);
                                Actor ropeSource = lastAttack.Context.Target;
                                Actor curTarget;
                                float damage = ScriptFormula(7);
                                int affectedTargets = 0;
                                while (affectedTargets < ScriptFormula(6))
                                {
                                    if (ropeSource.World == null)
                                        ropeSource = SpawnProxy(ropeSource.Position);

                                    curTarget = targets.GetClosestTo(ropeSource.Position);
                                    targets.Actors.Remove(curTarget);

                                    if (curTarget.World != null)
                                    {
                                        ropeSource.AddRopeEffect(186883, curTarget);
                                        WeaponDamage(curTarget, damage, DamageType.Lightning);
                                    }
                                    affectedTargets++;
                                }
                            }
                        }
                        if (Rune_C > 0)
                        {
                            if (Rand.NextDouble() < ScriptFormula(9))
                            {
                                AddBuff(lastAttack.Context.Target, new KnockbackBuff(ScriptFormula(10)));
                            }
                        }
                        if (Rune_D > 0)
                        {
                            //TODO: find correct chance, formulas didn't specify.
                            if (Rand.NextDouble() < 1)
                            {
                                GeneratePrimaryResource(ScriptFormula(11));
                            }
                        }
                        if (Rune_E > 0)
                        {
                            foreach (AttackPayload.DamageEntry dmg in lastAttack.DamageEntries)
                            {
                                //TODO: figure this out :)
                                //lastAttack.Context.User.Attributes[GameAttribute.Hitpoints_Cur] += Math.Min(getDamageDone(), lastAttack.Context.User.Attributes[GameAttribute.Hitpoints_Max]);
                            }
                        }
                    }
                }
            }
        }
    }
    #endregion

    //Broken
    #region Archon
    [ImplementsPowerSNO(Skills.Skills.Wizard.Utility.Archon)]
    public class Archon : Skill
    {
        /* Build up Buff Duration (SF(0))
        * Bonus duration Per Kill (SF(8))
        * 
        * 
        * 
        */
        public override IEnumerable<TickTimer> Main()
        {
            //StartDefaultCooldown();
            //UsePrimaryResource(ScriptFormula(12));
            AddBuff(User, new ArchonBuff());
            yield break;
        }
        [ImplementsPowerBuff(2)]
        class ArchonBuff : PowerBuff
        {
            public override void Init()
            {
                Timeout = WaitSeconds(15f);
            }

            public override bool Apply()
            {
                if (!base.Apply())
                    return false;
                //SF(3) armor buff
                //SF(4) resistance buff

                return true;
            }

            public override void Remove()
            {
                base.Remove();
            }
        }
    }
    #endregion


    #region MirrorImage
    [ImplementsPowerSNO(Skills.Skills.Wizard.Utility.MirrorImage)]
    public class MirrorImage : Skill
    {
        public override IEnumerable<TickTimer> Main()
        {
            //StartDefaultCooldown();
            //UsePrimaryResource(ScriptFormula(12));
            yield break;
        }
        [ImplementsPowerBuff(0)]
        class MirrorImageBuff : PowerBuff
        {
            public override void Init()
            {
                Timeout = WaitSeconds(15f);
            }

            public override bool Apply()
            {
                if (!base.Apply())
                    return false;
                return true;
            }

            public override void Remove()
            {
                base.Remove();
            }
        }
        [ImplementsPowerBuff(1)]
        class MirrorImage1Buff : PowerBuff
        {
            public override void Init()
            {
                Timeout = WaitSeconds(15f);
            }

            public override bool Apply()
            {
                if (!base.Apply())
                    return false;
                return true;
            }

            public override void Remove()
            {
                base.Remove();
            }
        }
    }
    #endregion

    //[Hard Skills TODO] Mirror Image, Familiar, Archon
    //14 passive skills
    /*
     * Power Hungry
     * Temporal Flux
     * Glass Cannon
     * Prodigy
     * Virtuoso
     * Astral Presence
     * Illusionist
     * Conflagration
     * Glavanizing Ward
     * Blur
     * Arcane Dynamo
     * Critical Mass
     * Evocation
     * Unstable Anomal
     */
}