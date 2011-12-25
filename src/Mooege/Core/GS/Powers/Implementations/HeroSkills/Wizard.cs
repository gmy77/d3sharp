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
using Mooege.Core.GS.Powers.Payloads;
using Mooege.Core.GS.Actors.Movement;
using Mooege.Core.GS.Common.Types.TagMap;
using Mooege.Net.GS.Message;
using Mooege.Core.GS.Players;

//TODO (IMPORTANT): GO BACK through and any WaitSeconds/GameAttributes must be in Buff Apply() and Remove()

namespace Mooege.Core.GS.Powers.Implementations
{
    [ImplementsPowerSNO(Skills.Skills.Wizard.Offensive.Meteor)]
    public class WizardMeteor : PowerScript
    {
        //TODO:Correct chilled/slow effect in Rune_C
        //TODO:Rune_E = crit targets from impact -> fire duration for 18 seconds
        public override IEnumerable<TickTimer> Run()
        {
            if (Rune_D > 0)
            {
                UsePrimaryResource(ScriptFormula(8) - ScriptFormula(16));
            }
            else
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
                SpawnEffect(RuneSelect(86769, 215809, 91441, 92031, 217139, 217458), impactPos);

                WeaponDamage(GetEnemiesInRadius(impactPos, ScriptFormula(3)), ScriptFormula(0),
                    RuneSelect(DamageType.Fire, DamageType.Fire, DamageType.Fire, DamageType.Cold, DamageType.Arcane, DamageType.Fire));

                WaitSeconds(ScriptFormula(4));

                if (Rune_C > 0)
                {
                    var FreezingMist = SpawnEffect(92031, impactPos, 0, WaitSeconds(ScriptFormula(5)));
                    FreezingMist.UpdateDelay = 1f;
                    FreezingMist.OnUpdate = () =>
                    {
                        WeaponDamage(GetEnemiesInRadius(impactPos, ScriptFormula(3)), ScriptFormula(2), DamageType.Cold);
                        //Target.Attributes[GameAttribute.Movement_Scalar_Reduction_Percent] += 0.60f;
                        //WaitSeconds(3f);
                        //Target.Attributes[GameAttribute.Movement_Scalar_Reduction_Percent] -= 0.60f;
                        //or
                        //AddBuff(GetEnemiesInRadius(impactPos, ScriptFormula(3)), new DebuffChilled(WaitSeconds(3f)));
                        //slow movement(60%) for three seconds

                    };
                }
                else
                {
                    var moltenfire = SpawnEffect(RuneSelect(86769, 215809, 91441, 92031, 217139, 217458), impactPos, 0, WaitSeconds(ScriptFormula(5)));
                    moltenfire.UpdateDelay = 1f;
                    moltenfire.OnUpdate = () =>
                    {
                        WeaponDamage(GetEnemiesInRadius(impactPos, ScriptFormula(3)), ScriptFormula(2), RuneSelect(DamageType.Fire, DamageType.Fire, DamageType.Fire, DamageType.Cold, DamageType.Arcane, DamageType.Fire));
                    };
                }

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
    public class WizardElectrocute : ChanneledSkill
    {
        //TODOs
        //A:Creates a streak of lightning that pierces targets, hitting all enemies in its path for 58% weapon damage as Lightning.
        //C:Blast a cone of lightning that causes 52% weapon damage as Lightning to all affected targets. 
        //E:Critical hits with this skill cause an explosion of 5 charged bolts in random directions, dealing 105% weapon damage as Lightning to any targets hit. 
        public override void OnChannelOpen()
        {
            EffectsPerSecond = 0.5f;
        }

        public override IEnumerable<TickTimer> Main()
        {
            User.TranslateFacing(TargetPosition);

            UsePrimaryResource(ScriptFormula(4));

            if (Target == null)
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
                        //Only on Rune_B does the damage reduce per target
                        if (Rune_B > 0)
                        {
                            damage *= 0.7f;
                        }
                        if (Rune_D > 0)
                        {
                            GeneratePrimaryResource(4f);
                        }
                    }
                    else
                    {
                        // early out if monster to be bounced died prematurely
                        break;
                    }

                    curTarget = GetEnemiesInRadius(curTarget.Position, ScriptFormula(2), 3).Actors.FirstOrDefault(t => !targets.Contains(t));
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
        //TODO: Pierce Chance to Target and hit another Target for Rune_C
        //      ScriptFormula(12)
        //TODO:Rune_E - ScriptFormula(10,11)
        public override IEnumerable<TickTimer> Run()
        {
            UsePrimaryResource(ScriptFormula(7));

            User.PlayEffectGroup(19305); // cast effect
            if (Rune_B > 0)
            {

                Vector3D[] projDestinations = PowerMath.GenerateSpreadPositions(User.Position, TargetPosition, ScriptFormula(8)/3f, (int)ScriptFormula(5));

                foreach (Vector3D missilePos in projDestinations)
                {
                    var proj = new Projectile(this, 99567, User.Position);
                    proj.OnCollision = (hit) =>
                    {
                        SpawnEffect(99572, new Vector3D(hit.Position.X, hit.Position.Y, hit.Position.Z + 5f)); // impact effect (fix height)
                        proj.Destroy();
                        WeaponDamage(hit, ScriptFormula(1), DamageType.Arcane);
                    };
                    proj.Launch(missilePos, ScriptFormula(4));
                }

            }
            else
            {
                var projectile = new Projectile(this, 99567, User.Position);
                projectile.OnCollision = (hit) =>
                {
                    SpawnEffect(99572, new Vector3D(hit.Position.X, hit.Position.Y, hit.Position.Z + 5f)); // impact effect (fix height)
                    projectile.Destroy();
                    //ScriptFormula(1) handles Rune_A and Rune_E damage increases
                    WeaponDamage(hit, ScriptFormula(1), DamageType.Arcane);
                    if (Rune_D > 0)
                    {
                        GeneratePrimaryResource(ScriptFormula(16));
                    }
                };
                projectile.Launch(TargetPosition, ScriptFormula(4));
            }

            yield break;
        }
    }

    [ImplementsPowerSNO(Skills.Skills.Wizard.Offensive.Hydra)]
        //No Rune = Default
        //Rune_A = Hydra_Frost
        //Rune_B = Hydra_Lightning
        //Rune_C = Hydra_Acid
        //Rune_D = Hydra_Big
        //Rune_E = Hydra_Arcane

    public class WizardHydra : Skill
    {
        public override IEnumerable<TickTimer> Main()
        {
            UsePrimaryResource(60f);
            //Spawn Hydras, Hydras face targets in radius[TODO], projectile[TODO], despawn after 9 seconds[TODO]
            //TODO: [Hard MODE!] - Rune Effects..
            // http://www.youtube.com/watch?v=twVSLGkoQqk

            Vector3D userCastPosition = new Vector3D(User.Position);
            Vector3D inFrontOfUser = PowerMath.TranslateDirection2D(User.Position, TargetPosition, User.Position, 7f);
            Vector3D[] spawnPoints = PowerMath.GenerateSpreadPositions(inFrontOfUser, RandomDirection(inFrontOfUser, 0f), 120, 3);
            //Hydra1 = angle 0, Hydra2 = angle 120, Hydra3 = angle 240. this isnt a spread but the angle of animation? think its the Y axis?
            // it may even just be spreading out the heads by mere inches(?) 

            var timeout = WaitSeconds(ScriptFormula(0));
            
            SpawnEffect(RuneSelect(81103, 83028, 81238, 77112, 83964, 81239), inFrontOfUser, 0, timeout); //Lava Pool Spawn
            
            int[] actorSNOs = new int[] {   RuneSelect(80745, 82972, 82109, 82111, 83959, 81515), 
                                            RuneSelect(80757, 83024, 81229, 81226, -1, 81231), 
                                            RuneSelect(80758, 83025, 81230, 81227, -1, 81232) };

            List<Actor> hydras = new List<Actor>();

            for (int i = 0; i < 3; ++i)
            {
                var hydra = SpawnEffect(actorSNOs[i], spawnPoints[i], 0, timeout);
                hydra.UpdateDelay = 1f; // attack every half-second
                hydra.OnUpdate = () =>
                {
                    /* 
                     * Hydras all face the same direction,
                     * attacking in the area in front of them, 
                     * not the whole 180 degrees that they are facing
                     */
                    var targets = GetEnemiesInRadius(hydra.Position, 60f);
                    if (targets.Actors.Count > 0)
                    {
                        targets.SortByDistanceFrom(hydra.Position);
                        var proj = new Projectile(this, RuneSelect(77116, 83043, -1, 77109, 86082, 77097), hydra.Position);
                        proj.Position.Z += 5f;  // fix height
                        proj.OnCollision = (hit) =>
                        {
                            // hit effect?
                                WeaponDamage(hit, 1.00f, DamageType.Fire);
         
                            proj.Destroy();
                        };
                        proj.Launch(TargetPosition, ScriptFormula(2));
                    }
                    
                };

                hydras.Add(hydra);
            }
            // wait for duration of skill
            yield return timeout;
        }
    }

    [ImplementsPowerSNO(Skills.Skills.Wizard.Offensive.ArcaneOrb)]
    public class ArcaneOrb : Skill
    {
        public override IEnumerable<TickTimer> Main()
        {
            if (Rune_D > 0)
            {
                UsePrimaryResource(ScriptFormula(0) - ScriptFormula(13));
            }
            else
            UsePrimaryResource(ScriptFormula(0));

            // cast effect - taken from DemonHuner Bola Shot
            Vector3D[] targetDirs;
            {
                targetDirs = new Vector3D[] { TargetPosition };
            }

            if (Rune_C > 0)
            {
                //Somehow set a max to only have 4 orbs at a time.
                //ScriptFormula(10) = Max Orbs -> min(( Rune_C * 1), 4)
                AddBuff(User, new Orbit1());
                AddBuff(User, new Orbit2());
                AddBuff(User, new Orbit3());
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
        [ImplementsPowerBuff(0)]
        class Orbit1 : PowerBuff
        {
            public override bool Update()
            {
                if (base.Update())
                    return true;

                var targets = GetEnemiesInRadius(Target.Position, 10f);
                if (targets.Actors.Count > 0)
                {
                    WeaponDamage(targets, ScriptFormula(3), DamageType.Arcane);
                    return true;
                }

                return false;
            }
        }

        [ImplementsPowerBuff(1)]
        class Orbit2 : PowerBuff
        {
            public override bool Update()
            {
                if (base.Update())
                    return true;

                var targets = GetEnemiesInRadius(Target.Position, 10f);
                if (targets.Actors.Count > 0)
                {
                    WeaponDamage(targets, ScriptFormula(3), DamageType.Arcane);
                    return true;
                }

                return false;
            }
        }
        [ImplementsPowerBuff(2)]
        class Orbit3 : PowerBuff
        {
            public override bool Update()
            {
                if (base.Update())
                    return true;

                var targets = GetEnemiesInRadius(Target.Position, 10f);
                if (targets.Actors.Count > 0)
                {
                    WeaponDamage(targets, ScriptFormula(3), DamageType.Arcane);
                    return true;
                }

                return false;
            }
        }

        [ImplementsPowerBuff(3)]
        class Orbit4 : PowerBuff
        {
            public override bool Update()
            {
                if (base.Update())
                    return true;

                var targets = GetEnemiesInRadius(Target.Position, 10f);
                if (targets.Actors.Count > 0)
                {
                    WeaponDamage(targets, ScriptFormula(3), DamageType.Arcane);
                    return true;
                }

                return false;
            }
        }
    }

    [ImplementsPowerSNO(Skills.Skills.Wizard.Offensive.EnergyTwister)]
    public class EnergyTwister : Skill
    {
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

    [ImplementsPowerSNO(Skills.Skills.Wizard.Offensive.Disintegrate)]
    public class WizardDisintegrate : ChanneledSkill
    {
        //TODOs
        //Rune_A- same as NoRune.. -> todo: add the increase to damage
        ///Rune-B- fatbeam
        //Rune-C- parabola.acr (93560.acr) and field.efg (93563.efg)?
        //Rune-D- Possibly Mini-Buff.efg/.rop and Dome.acr?
        //Rune-E- explode when dead [explode.efg -> 93574], explode_proxy.acr and explodeBubble.acr

        //no idea if sourceglow/pulseglow is used, most likely not.
        //unknown hitfx_override.efg
        //--------------------------------------------------------------------------------------------
        //Rune_A -> Damage increases slowly over time to inflict a maximum of 4620% weapon damage as Arcane.
             //(10) - Chargeup Time, (11) - Dmg Modifier
        //Rune_B -> Increase the width of the beam allowing it to hit more enemies for 2100% weapon damage as Arcane.
             //ScriptFormula(5) - Damage Modifier, (6) - Radius Modifier
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
            //todo: is this correct? why not just seconds?ScriptFormula(22) 
            UsePrimaryResource(23f * EffectsPerSecond);

            foreach (Actor actor in GetEnemiesInRadius(User.Position, BeamLength + 10f).Actors)
            {
                if (Rune_B > 0)
                {
                    if (PowerMath.PointInBeam(actor.Position, User.Position, TargetPosition, 6f))
                    {
                        //ScriptFormula(23)
                        WeaponDamage(actor, 1.35f * EffectsPerSecond, DamageType.Arcane);
                    }
                }
                else
                if (PowerMath.PointInBeam(actor.Position, User.Position, TargetPosition, 3f))
                {
                    //ScriptFormula(23)
                    WeaponDamage(actor, 1.35f * EffectsPerSecond, DamageType.Arcane);
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
            UsePrimaryResource(ScriptFormula(26));
            StartCooldown(WaitSeconds(ScriptFormula(30)));

            yield return WaitSeconds(0.350f); // wait for wizard to land

            //I switched the effects of obsidian and golden because in-game they are opposite
            User.PlayEffectGroup(RuneSelect(19356, 82649, 215399, 215403, 215400, 215404));

            AttackPayload attack = new AttackPayload(this);
            attack.Targets = GetEnemiesInRadius(User.Position, ScriptFormula(1));
            attack.AddWeaponDamage(ScriptFormula(2), DamageType.Physical);
            //TODO: Add Script 17 and 18 for slow movement
            //TODO: Script 6,7,8,9
            attack.OnHit = hitPayload => {
                Knockback(hitPayload.Target, ScriptFormula(0), ScriptFormula(4), ScriptFormula(5));
                if (Rune_D > 0)
                {
                    //get targets positions in user position radius
                    //for each roll random
                    if (Rand.NextDouble() < ScriptFormula(14))
                    {
                        //teleport somewhere else nearby via the minimum and maximum below.
                        //ScriptFormula(12) - teleport min, ScriptFormula(13) - tele max

                        //SpawnProxy(Target.Position).PlayEffectGroup(77975);
                        //Target.Teleport(TargetPosition);
                        //Target.PlayEffectGroup(77976);
                    }
                }
                if (Rune_E > 0)
                {
                    //TODO:Stun all enemies in radius
                    //ScriptFormula(10)
                    Knockback(hitPayload.Target, ScriptFormula(0) + ScriptFormula(11), ScriptFormula(4), ScriptFormula(5));
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

    [ImplementsPowerSNO(Skills.Skills.Wizard.Offensive.ExplosiveBlast)]
    public class ExplosiveBlast : Skill
    {
        public override IEnumerable<TickTimer> Main()
        {
            //For Damage Multipler -> I use damage modifier script formula, is that correct?
            if (Rune_D > 0)
            {
                UsePrimaryResource(ScriptFormula(15));
                StartCooldown(WaitSeconds(1f));
                User.PlayEffectGroup(89449);
            }
            else if (Rune_A > 0)
            {
                //there is no charge up effect since its instant
                UsePrimaryResource(ScriptFormula(15));
                StartCooldown(WaitSeconds(1f));
            }
            else if (Rune_C > 0)
            {
                UsePrimaryResource(ScriptFormula(15));
                StartCooldown(WaitSeconds(1f));
                SpawnProxy(User.Position).PlayEffectGroup(89449);
            }
            else
            {
                UsePrimaryResource(ScriptFormula(15));
                StartCooldown(WaitSeconds(1f));
                User.PlayEffectGroup(89449);
            }

                yield return WaitSeconds(ScriptFormula(5));

            IEnumerable<TickTimer> subScript;
            if (Rune_A > 0)
                subScript = _RuneA();
            else if (Rune_B > 0)
                subScript = _RuneB();
            else if (Rune_C > 0)
                subScript = _RuneC();
            else if (Rune_D > 0)
                subScript = _RuneD();
            else if (Rune_E > 0)
                subScript = _RuneE();
            else
                subScript = _NoRune();

            foreach (var timeout in subScript)
                yield return timeout;
        }
        IEnumerable<TickTimer> _RuneA()
        {
            SpawnEffect(61419, User.Position);
            AttackPayload attack = new AttackPayload(this);
            attack.Targets = GetEnemiesInRadius(User.Position, ScriptFormula(2));
            attack.AddWeaponDamage(ScriptFormula(0), DamageType.Physical);
            attack.Apply();
            yield break;
        }
        IEnumerable<TickTimer> _RuneB()
        {
            SpawnEffect(192210, User.Position);
            AttackPayload attack = new AttackPayload(this);
            attack.Targets = GetEnemiesInRadius(User.Position, ScriptFormula(2));
            attack.AddWeaponDamage(ScriptFormula(0), DamageType.Physical);
            attack.Apply();
            yield break;
        }
        IEnumerable<TickTimer> _RuneC()
        {
            //TODO: place spawneffect at feet - does not follow with you
            SpawnEffect(61419, User.Position);
            AttackPayload attack = new AttackPayload(this);
            attack.Targets = GetEnemiesInRadius(User.Position, ScriptFormula(2));
            attack.AddWeaponDamage(ScriptFormula(0), DamageType.Physical);
            attack.Apply();
            yield break;
        }
        IEnumerable<TickTimer> _RuneD()
        {
            SpawnEffect(192211, User.Position);
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
        IEnumerable<TickTimer> _NoRune()
        {
            SpawnEffect(61419, User.Position);
            AttackPayload attack = new AttackPayload(this);
            attack.Targets = GetEnemiesInRadius(User.Position, ScriptFormula(2));
            attack.AddWeaponDamage(ScriptFormula(0), DamageType.Physical);
            attack.Apply();
            yield break;
        }
    }

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

            //Enemies killed by Arcane Torrent have a 95% chance to fire a new missile at a 
            //nearby enemy dealing 5000% weapon damage as Arcane.
        }

        [ImplementsPowerBuff(0)]
        class CastEffect : PowerBuff
        {
            public override void Init()
            {
                Timeout = WaitSeconds(0.3f);
            }
        }
    }

    //bumbasher
    [ImplementsPowerSNO(Skills.Skills.Wizard.Utility.FrostNova)]
    public class WizardFrostNova : PowerScript
    {
        //Rune_A - Enemies take 110% more damage while frozen or chilled by Frost Nova.
        //Rune_B - A frozen enemy that is killed has a 21% chance of exploding another Frost Nova dealing 3400% weapon damage as Cold.
        //Rune_C - The Frost Nova no longer freezes enemies, but instead leaves behind a mist of frost for 8 seconds that deals 2600% weapon damage per second as Cold to enemies standing in it.
        //Rune_D - Reduce cooldown to [MAX(0,(12) - 7)|1|] seconds.
        //Rune_E - If Frost Nova hits at least 5 targets, you gain 45% chance to critically hit for 12 seconds
        public const int FrostNova_Emitter = 4402;

        public override IEnumerable<TickTimer> Run()
        {
            StartCooldown(WaitSeconds(12f));

            SpawnEffect(FrostNova_Emitter, User.Position);

            WeaponDamage(GetEnemiesInRadius(User.Position, 18f), 0.65f, DamageType.Cold);
            //todo:freeze duration to 4 seconds and check damage multiplier
            yield break;
        }
    }

    [ImplementsPowerSNO(Skills.Skills.Wizard.Offensive.Blizzard)]
    public class WizardBlizzard : PowerScript
    {
        //Rune_A - Increase the duration of the Blizzard to [10|1|] seconds.
        //Rune_B - Increase the size of the Blizzard to cover 22 yards, and deal 5800% weapon damage per second as Cold.
        //Rune_C - After the Blizzard ends, cover the ground in a low lying mist for [6|1|] seconds that slows any enemies who enters it. 
        //Rune_D - Reduce the casting cost to {Resource Cost} Arcane Power.
        //Rune_E - Enemies caught in the storm have a 52.5% chance to be frozen for 3 seconds and the critical strike chance with Blizzard is increased by 80%.
        public const int Wizard_Blizzard = 0x1977;

        public override IEnumerable<TickTimer> Run()
        {
            UsePrimaryResource(45f);

            SpawnEffect(Wizard_Blizzard, TargetPosition);

            const int blizzard_duration = 3;

            for(int i = 0; i < blizzard_duration; ++i)
            {
                AttackPayload attack = new AttackPayload(this);
                attack.Targets = GetEnemiesInRadius(TargetPosition, 18f);
                attack.AddWeaponDamage(0.65f, DamageType.Cold);
                attack.OnHit = (hit) =>
                {
                    AddBuff(hit.Target, new DebuffChilled(WaitSeconds(3f)));
                };
                attack.Apply();

                yield return WaitSeconds(1f);
            }
        }
    }

    [ImplementsPowerSNO(Skills.Skills.Wizard.Offensive.RayOfFrost)]
    public class WizardRayOfFrost : ChanneledSkill
    {
        //Rune_A - Damage increases slowly over [1.5|1|] seconds to inflict a maximum of 24000% weapon damage as Cold.
        //Rune_B - Create a swirling storm of sleet dealing [99 * 100}]% weapon damage as Cold to all enemies caught within it.
        //Rune_C - Increase enemy slow amount to 50.5% for [8.5|1|] seconds, but damage is reduced to 7400% weapon damage as Cold.
        //Rune_D - Reduce casting cost to 8.5714285714286 Arcane Power per second. 
        //Rune_E - Enemies who die leave a patch of ice on the ground that causes 64500% weapon damage as Cold to enemies moving through it over 5 seconds.

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
        
        public override IEnumerable<TickTimer> Main()
        {
            UsePrimaryResource(29f * EffectsPerSecond);

            foreach (Actor actor in GetEnemiesInRadius(User.Position, BeamLength + 10f).Actors)
            {
                if (PowerMath.PointInBeam(actor.Position, User.Position, TargetPosition, 3f))
                {
                    WeaponDamage(actor, 2.70f * EffectsPerSecond, DamageType.Cold);
                }
            }

            yield break;
        }
    }

    [ImplementsPowerSNO(Skills.Skills.Wizard.Utility.Teleport)]
    public class WizardTeleport : PowerScript
    {
        //Rune_A - Casts a low power Wave of Force on arrival dealing 8500% weapon damage as Physical to all enemies nearby.
        //Rune_B - Summon 2 mirror images for 15 |4second:seconds; on arrival.
        //Rune_C - For 16 |4second:seconds; after you appear you will take 25% less damage.
        //Rune_D - Casting Teleport again within 8 |4second:seconds; will instantly bring you back to your original location.
        //Rune_E - After casting Teleport there is a [2|1|] second delay before the cooldown begins.

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
        //A:Enemies hit by the blade will bleed for an additional [0.45 * {Script Formula 0} * 100]% weapon damage done over 3 seconds.
        //B:Extends the blades out to reach everything within 20 yards dealing 71.75% weapon damage.
        //C:Hits have a 44.6% chance to cause knockback and slow the movement of enemies by 60% for 2 seconds.
        //D:Every enemy hit grants 7 Arcane Power.
        //E:Whenever the blades do critical damage, you are healed [1 * 100|1|]% of the damage caused.
        public override IEnumerable<TickTimer> Run()
        {
            UsePrimaryResource(15f);

            User.PlayEffectGroup(188941);

            // calculate hit area of effect, just in front of the user
            TargetPosition = PowerMath.TranslateDirection2D(User.Position, TargetPosition, User.Position, 9f);

            for (int n = 0; n < 3; ++n)
            {
                WeaponDamage(GetEnemiesInRadius(TargetPosition, 9f), 0.30f, DamageType.Physical);
                yield return WaitSeconds(0.2f);
            }
        }
    }

    [ImplementsPowerSNO(Skills.Skills.Wizard.Utility.IceArmor)]
    public class IceArmor : Skill
    {
        public override IEnumerable<TickTimer> Main()
        {
            StartDefaultCooldown();
            UsePrimaryResource(25f);
            AddBuff(User, new IceArmorBuff());
            yield break;
        }

        [ImplementsPowerBuff(1)]
        class IceArmorBuff : PowerBuff
        {
            public override void Init()
            {
                Timeout = WaitSeconds(120f);
            }

            public override bool Apply()
            {
                if (!base.Apply())
                    return false;

                //Increase Armor by 50% -> Correct?
                User.Attributes[GameAttribute.Armor_Item_Percent] += ScriptFormula(2);
                User.Attributes.BroadcastChangedIfRevealed();

                //TODO: Rune Stats
                //Rune_C = IceArmorRune_IceBlade.acr/.efg

                return true;
            }

            public override void OnPayload(Payload payload)
            {
                if (payload.Target == Target && payload is HitPayload)
                {
                    //Affect monsters in radius
                    //GetEnemiesInRadius(User.Position, ScriptFormula(7));
                    //TODO:Chill for 2 Seconds? Attempted..
                    WeaponDamage(payload.Context.User,0.12f, DamageType.Cold);
                    Target.Attributes[GameAttribute.Chilled] = true;
                    WaitSeconds(2f);
                    Target.Attributes[GameAttribute.Chilled] = false;
                }
            }

            public override void Remove()
            {
                base.Remove();
                User.PlayEffectGroup(19326);
                
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

        }
    }

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
            //TODO; Unknown how Rune_E works, if attack crits -> you shock nearby enemies.
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
                    //reduce all arcane costs by 7 while storm armor is active
                    User.Attributes[GameAttribute.Resource_Cost_Reduction_Amount] += 7;
                }

                return true;
            }

            public override void OnPayload(Payload payload)
            {
                if (Rune_B > 0)
                {
                    if (payload.Target == Target && payload is HitPayload)
                    {
                        //increase movement speed 20% for 5 seconds
                        User.Attributes[GameAttribute.Movement_Bonus_Run_Speed] += ScriptFormula(14);
                        WaitSeconds(5f);
                        User.Attributes[GameAttribute.Movement_Bonus_Run_Speed] -= ScriptFormula(14);
                    }
                }
                if (Rune_C > 0)
                {
                    if (payload.Target == Target && payload is HitPayload)
                    {
                        //you have a chance to be enveloped with a lightning shield for 8 seconds 
                        //that shocks nearby enemies for 120% weapon damage as Lightning.
                    }
                }

            }

            public override bool Update()
            {
                if (base.Update())
                    return true;
                //add cooldown delay
                var targets = GetEnemiesInRadius(Target.Position, ScriptFormula(19));
                if (targets.Actors.Count > 0)
                {
                    if (Rune_A > 0)
                    {
                        WeaponDamage(targets, 2.20f, DamageType.Lightning);
                    }
                    else
                        WeaponDamage(targets, 1.00f, DamageType.Lightning);
                }
                return false;
            }

            public override void Remove()
            {
                if (Rune_D > 0)
                {
                    User.Attributes[GameAttribute.Resource_Cost_Reduction_Amount] -= 7;
                }
                base.Remove();

            }
        }
    }

    [ImplementsPowerSNO(Skills.Skills.Wizard.Utility.DiamondSkin)]
    public class DiamondSkin : Skill
    {
        //Wizard_StoneSkin.efg/stonearmor/diamondskin is also diamondskin..
        //gameattribute[Breakable Shield HP] + Invulnerable or No Damage?
        public override IEnumerable<TickTimer> Main()
        {
            StartDefaultCooldown();
            UsePrimaryResource(25f);
            AddBuff(User, new DiamondSkinBuff());
            yield break;
        }

        [ImplementsPowerBuff(2)] //TODO: check this, theres 3 groups..
        class DiamondSkinBuff : PowerBuff
        {
            public override void Init()
            {
                Timeout = WaitSeconds(5f);
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
                User.PlayEffectGroup(RuneSelect(93077, 187716, 187805, 187822, 187831, 187851));
            }
        }
    }

    [ImplementsPowerSNO(Skills.Skills.Wizard.Utility.SlowTime)]
    public class SlowTime : Skill
    {
        //TODO: make sure that when AI works, that the SlowTime Bubble is the only area that gets the effects and not from the user's position.
        public override IEnumerable<TickTimer> Main()
        {
            if (Rune_D > 0)
            {
                StartCooldown(ScriptFormula(14));
                //UsePrimaryResource(25f);
                AddBuff(User, new SlowTimeBuff());
                yield break;
            }
            else
            StartDefaultCooldown();
            //UsePrimaryResource(25f);
            AddBuff(User, new SlowTimeBuff());
            yield break;
        }

        [ImplementsPowerBuff(0)] //TODO: check this
        class SlowTimeBuff : PowerBuff
        {
            public override void Init()
            {
                Timeout = WaitSeconds(ScriptFormula(0));
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
                if (Rune_C > 0)
                {
                    if (base.Update())
                    {
                        Target.Attributes[GameAttribute.Slow] = false;
                        return true;
                    }
                    var Rune_Ctargets = GetEnemiesInRadius(User.Position, 10f);
                    if (Rune_Ctargets.Actors.Count > 0)
                    {
                        //Slowed Enemies, their Attack speed and Projectile Speeds are decreased.
                        Target.Attributes[GameAttribute.Slow] = true;

                    }
                    return false;
                }
                else if (Rune_E > 0)
                {
                    if (base.Update())
                    {
                        Target.Attributes[GameAttribute.Slow] = false;
                        return true;
                    }
                    var enemytargets = GetEnemiesInRadius(User.Position, ScriptFormula(2));
                    if (enemytargets.Actors.Count > 0)
                    {
                        //Slowed Enemies, their Attack speed and Projectile Speeds are decreased.
                        Target.Attributes[GameAttribute.Slow] = true;
                    }
                    var friendlytargets = GetAlliesInRadius(User.Position, ScriptFormula(2));
                    if (friendlytargets.Actors.Count > 0)
                    {
                        //speed up time for friendlies by increasing attack speed by 140%
                    }
                    return false;
                }
                else

                    if (base.Update())
                    {
                        Target.Attributes[GameAttribute.Slow] = false;
                        return true;
                    }
                var targets = GetEnemiesInRadius(User.Position, ScriptFormula(2));
                if (targets.Actors.Count > 0)
                {
                    if (Rune_A > 0)
                    {
                        //Slowed Enemies, their Attack speed and Projectile Speeds are decreased.
                        Target.Attributes[GameAttribute.Slow] = true;
                        //Take 140% more damage.
                    }
                    if (Rune_B > 0)
                    {
                        //Slowed Enemies, their Attack speed and Projectile Speeds are decreased.
                        Target.Attributes[GameAttribute.Slow] = true;
                        //Once Enemies out of range of bubble or bubble ended, 14 more seconds on slow time effect.
                        //maybe add this to remove()?
                    }

                        //Slowed Enemies, their Attack speed and Projectile Speeds are decreased.
                        Target.Attributes[GameAttribute.Slow] = true;
                }
                return false;
            }

            public override void Remove()
            {
                base.Remove();

            }
        }
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
                    Timeout = WaitSeconds(120f);
                }

                public override bool Apply()
                {
                    if (!base.Apply())
                        return false;
                    if (Rune_A > 0)
                    {
                        //increase resistance
                    }
                    if (Rune_B > 0)
                    {
                        //increase max resource by 40
                    }
                    if (Rune_E > 0)
                    {
                        //increase percision by 40$
                    }
                    //increasing your Defense by 20% but lowers your maximum Arcane Power by 20.
                    //--> drains __ primary resource for every 1% of your maximum Life absorbed.
                    return true;
                }

                public override void OnPayload(Payload payload)
                {
                    if (Rune_C > 0)
                    {
                        //incoming attacks that would deal more than 26% of your maximum Life 
                        //are reduced to deal 26% of your maximum Life instead.
                        //TODO: take the two numbers, and find which is the minimum.
                    }
                    if (Rune_D > 0)
                    {
                    //You have a chance to gain 7 Arcane Power whenever you are hit by a ranged or melee attack.
                    }
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
        }
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
                return true;
            }
            public override bool Update()
            {
                if(base.Update())
                    return true;
                /*
                AttackPayload attack = new AttackPayload(this);
                if (Rune_A > 0)
                {
                    attack.OnHit = (hitPayload) =>
                    {
                        //ScriptFormula(3 & 4 & 5)
                        //poison enemies for 3 seconds, dealing 70% of weapon damage
                    };
                }
                if (Rune_B > 0)
                {
                    attack.OnHit = (hitPayload) =>
                    {
                        //formulas didn't say the chance.
                        if (Rand.NextDouble() < .10)
                        {
                            //lightning arcs
                            //ScriptFormula(6 & 7)
                        }
                    };
                }
                if (Rune_C > 0)
                {
                    attack.OnHit = (hitPayload) =>
                    {
                        //damage increase bonus percent - ScriptFormula(8)
                        if (Rand.NextDouble() < ScriptFormula(9))
                        {
                            AddBuff(hitPayload.Target, new KnockbackBuff(ScriptFormula(10)));
                        }
                    };
                }
                if (Rune_D > 0)
                {
                    attack.OnHit = (hitPayload) =>
                    {
                        //formulas didn't say the chance.
                        if (Rand.NextDouble() < .10)
                        {
                            //restore arcane power ScriptFormula(11)
                        }
                    };
                }
                if (Rune_E > 0)
                {
                    attack.OnHit = (hitPayload) =>
                    {
                        //formulas didn't say the chance.
                        if (Rand.NextDouble() < .10)
                        {
                            //RuneE Leech Perc ofDamage Done - ScriptFormula(12)
                        }
                    };
                }
                //increase physical damage by 20% - ScriptFormula(0)
                attack.Apply();
                */
                return false;
            }

            public override void Remove()
            {
                base.Remove();
            }
        }
    }

        //[Hard Skills TODO] Mirror Image, Familiar, Archon
        //10 passive skills
    }
}
