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
        [ImplementsPowerBuff(1)]
        class Crimson_DestablizedEffect : PowerBuff
        {
            public override void Init()
            {
                Timeout = WaitSeconds(2f);
            }
        }
    }

    //bumbasher
    [ImplementsPowerSNO(Skills.Skills.Wizard.Utility.FrostNova)]
    public class WizardFrostNova : PowerScript
    {
        //Rune_A - Enemies take 110% more damage while frozen or chilled by Frost Nova.
        //Rune_E - If Frost Nova hits at least 5 targets, you gain 45% chance to critically hit for 12 seconds
       public override IEnumerable<TickTimer> Run()
        {
            if (Rune_C > 0)
            {
                StartCooldown(WaitSeconds(ScriptFormula(3)));
                var moltenfire = SpawnEffect(RuneSelect(4402, 189047, 189048, 75631, 189049, 189050), User.Position, 0, WaitSeconds(ScriptFormula(9)));
                moltenfire.UpdateDelay = 1f;
                moltenfire.OnUpdate = () =>
                {
                    WeaponDamage(GetEnemiesInRadius(User.Position, ScriptFormula(6)), ScriptFormula(11), DamageType.Cold);
                };
            }
            else
            {
                StartCooldown(WaitSeconds(ScriptFormula(3)));
                SpawnEffect(RuneSelect(4402, 189047, 189048, 75631, 189049, 189050), User.Position);
                WeaponDamage(GetEnemiesInRadius(User.Position, ScriptFormula(6)), 0.65f, DamageType.Cold);

                if (Rune_B > 0)
                {
                    if (Rand.NextDouble() < ScriptFormula(14))
                    {
                        SpawnEffect(RuneSelect(4402, 189047, 189048, 75631, 189049, 189050), User.Position);
                        WeaponDamage(GetEnemiesInRadius(User.Position, ScriptFormula(15)), ScriptFormula(7), DamageType.Cold);
                    }
                }

                //freeze duration(ScriptFormula(2))
                //todo:while frozen or chilled enemies, Rune_A -> ScriptFormula(16)

                if (Rune_E > 0)
                {
                    //If Frost Nova hits at least 5 targets, [ScriptFormula(13)]
                    if (Rand.NextDouble() < ((Rune_E*5)+.10f))
                    {
                        //ycritically hit for 12 seconds
                        //[ScriptFormula(18) / ScriptFormula(19)]
                        //critBuff_swipe.acr [[215516]]
                    }
                }
            }
            yield break;
        }
       [ImplementsPowerBuff(5)]
       class FrostNova_Alabaster_Buff : PowerBuff
       {
           public override void Init()
           {
               Timeout = WaitSeconds(2f);
           }
       }
    }

    [ImplementsPowerSNO(Skills.Skills.Wizard.Offensive.Blizzard)]
    public class WizardBlizzard : PowerScript
    {
        //Rune_E - Enemies caught in the storm have a 52.5% chance to be frozen for 3 seconds and the critical strike chance with Blizzard is increased by 80%.
        public const int Wizard_Blizzard = 0x1977;

        public override IEnumerable<TickTimer> Run()
        {
            UsePrimaryResource(ScriptFormula(19));

            SpawnEffect(Wizard_Blizzard, TargetPosition);

            for (int i = 0; i < ScriptFormula(4); ++i)
            {
                //if (E):{critical strike chance with Blizzard is increased by 80% -> scriptformula(9)}
                
                AttackPayload attack = new AttackPayload(this);
                attack.Targets = GetEnemiesInRadius(TargetPosition, ScriptFormula(3));
                attack.AddWeaponDamage(ScriptFormula(0), DamageType.Cold);
                attack.OnHit = (hit) =>
                {
                    AddBuff(hit.Target, new DebuffChilled(WaitSeconds(3f)));
                    if (Rune_E > 0)
                    {
                        if (Rand.NextDouble() < ScriptFormula(10))
                        {
                            //scriptformula(11)
                        }
                    }
                };
                attack.Apply();

                yield return WaitSeconds(1f);
            }
            if (Rune_C > 0)
            {
                var BlizzMist = SpawnEffect(75642, User.Position, 0, WaitSeconds(ScriptFormula(7)));
                BlizzMist.UpdateDelay = 1f;
                BlizzMist.OnUpdate = () =>
                {
                    //slow enemies who enter mist
                    //ScriptFormula(8) and (12)
                };
            }
        }
    }

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
            //Total:Castin Cost vs. Resource Cost Min To Cast
            UsePrimaryResource((Math.Max(ScriptFormula(19), 8f)));

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
            //Target.playeffectgroup(149878); or on proxy?
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
            public override void Init()
            {
                Timeout = WaitSeconds(2f);
            }
        }
    }

    [ImplementsPowerSNO(Skills.Skills.Wizard.Utility.Teleport)]
    public class WizardTeleport : PowerScript
    {

        public override IEnumerable<TickTimer> Run()
        {
            UsePrimaryResource(15f);
            //StartCooldown(WaitSeconds(ScriptFormula(20)));
            
            SpawnProxy(User.Position).PlayEffectGroup(RuneSelect(170231, 205685, 205684, 191913, 192074, 192151));  // alt cast efg: 170231
            yield return WaitSeconds(0.3f);
            User.Teleport(TargetPosition);
            User.PlayEffectGroup(RuneSelect(170232, 170232, 170232, 192053, 192080, 192152));
            
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
                //Rune_C - For 16 |4second:seconds; after you appear you will take 25% less damage.
                AddBuff(User, new TeleDmgReductionBuff());
            }
            if (Rune_D > 0)
            {
                //User.PlayEffectGroup(206679); //golden_groundportal.efg
                //Rune_D - Casting Teleport again within 8 |4second:seconds; will instantly bring you back to your original location.
                //SF(18)
            }
            if (Rune_E > 0)
            {
                //User.PlayEffectGroup(192069); // -> obsidian buff
                //Rune_E - After casting Teleport there is a [2|1|] second delay before the cooldown begins.
                //E: SF(16) - runeE CoolDown Delay
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
                return true;
            }

            public override void OnPayload(Payload payload)
            {
                if (payload.Target == Target && payload is HitPayload)
                {
                    //buff -> 25% less damage[SF(14)]
                }
            }

            public override void Remove() { base.Remove(); }
        }
    }

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
                    //if Rune_A
                    //BLEED DEBUFF
                    //(SF4)A: Bleed Weapon Damage %
                    //(SF5)A: Additional Bleed Damage Modifier
                    //(SF6)A: Bleed Duration

                    if (Rune_C > 0)
                    {
                        if (Rand.NextDouble() < ScriptFormula(19))
                        {
                            Knockback(Target, ScriptFormula(14));
                            //Slow movement (SF(11) and SF(12))
                        }
                    }
                    if (Rune_D > 0)
                    {
                        GeneratePrimaryResource(ScriptFormula(13));
                    }
                    if (Rune_E > 0)
                    {
                        //E:Whenever the blades do critical damage, you are healed [1 * 100|1|]% of the damage caused.
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
                Timeout = WaitSeconds(2f);
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
                Timeout = WaitSeconds(ScriptFormula(3));
            }

            public override bool Apply()
            {
                if (!base.Apply())
                    return false;

                //Increase Armor by 50% -> Correct? -> TODO: make this a buff.
                User.Attributes[GameAttribute.Armor_Item_Percent] += ScriptFormula(2);
                User.Attributes.BroadcastChangedIfRevealed();

                return true;
            }

            public override bool Update()
            {
                if (base.Update())
                    return true;
                if (Rune_C > 0)
                {
                    // This is wrong... It needs to be the playeffectgroup which is much bigger..and shouldnt keep recreating itself.
                    var iceblade = SpawnEffect(88032, User.Position, 0, WaitInfinite());
                    iceblade.UpdateDelay = 1f;
                    iceblade.OnUpdate = () =>
                    {
                        WeaponDamage(GetEnemiesInRadius(User.Position, ScriptFormula(7)), ScriptFormula(9), DamageType.Cold);
                    };
                }
                if (Rune_B > 0)
                {
                    GetEnemiesInRadius(User.Position, ScriptFormula(7));
                    //SF(23) and SF(24)
                    //Chilled Debuff, Slow Movement and Slow Attack Speeds
                }
                return false;
            }

            public override void OnPayload(Payload payload)
            {
                if (payload.Target == Target && payload is HitPayload)
                {
                    if (Rune_D > 0)
                    {
                        //this should call a buff, each hit should give the extra buff as long as you are under the max stacks.
                        //SF(11) - max stacks
                        //sf(26) - Bonus Armor per stack
                        //sf(27) - bonus stack duration

                    }
                    if (Rune_E > 0)
                    {
                        if (Rand.NextDouble() < ScriptFormula(12))
                        {
                            SpawnEffect(4402, Target.Position);
                            WeaponDamage(GetEnemiesInRadius(Target.Position, ScriptFormula(15)), ScriptFormula(13), DamageType.Cold);
                        }
                    }
                    WeaponDamage(payload.Context.User, ScriptFormula(0), DamageType.Cold);   
                    //Debuff Chills Enemies (SF(4))
                }
            }

            public override void Remove()
            {
                base.Remove();
                User.PlayEffectGroup(19326);
                User.PlayEffectGroup(185652);
                
            }
        }
        [ImplementsPowerBuff(0)]
        class SwitchEffect : PowerBuff
        {
            public override void Init()
            {
                Timeout = WaitSeconds(2f);
            }
        }
        [ImplementsPowerBuff(2)]
        class SwitchBuff : PowerBuff
        {
            public override void Init()
            {
                Timeout = WaitSeconds(2f);
            }
        }
    }

    [ImplementsPowerSNO(Skills.Skills.Wizard.Signature.ShockPulse)]
    public class WizardShockPulse : PowerScript
    {

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
        [ImplementsPowerBuff(1)]
        class TeslaBuff : PowerBuff
        {
            public override void Init()
            {
                Timeout = WaitSeconds(2f);
            }
        }
        [ImplementsPowerBuff(2)]
        class IndigoBuff : PowerBuff
        {
            public override void Init()
            {
                Timeout = WaitSeconds(2f);
            }
        }
        [ImplementsPowerBuff(3)]
        class GoldenBuff : PowerBuff
        {
            public override void Init()
            {
                Timeout = WaitSeconds(2f);
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

        [ImplementsPowerBuff(0)]
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
        [ImplementsPowerBuff(1)]
        class StoneSkinBuff : PowerBuff
        {
            public override void Init()
            {
                Timeout = WaitSeconds(5f);
            }
        }
        [ImplementsPowerBuff(2)]
        class StoneArmorBuff : PowerBuff
        {
            public override void Init()
            {
                Timeout = WaitSeconds(5f);
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
                    if (payload.Target == Target && payload is HitPayload)
                    {
                        if (Rune_C > 0)
                        {
                            //Math.Min(Total Damage incoming/Maximum Life, 26% of Maximum Life)
                        }
                        if (Rune_D > 0)
                        {
                            //just a guess.
                            if (Rand.NextDouble() < .4f)
                            {
                                GeneratePrimaryResource(ScriptFormula(11));
                            }
                        }
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
