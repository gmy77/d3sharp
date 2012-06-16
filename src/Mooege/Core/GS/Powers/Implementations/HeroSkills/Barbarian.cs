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
using System.Linq;
using System.Collections.Generic;
using Mooege.Core.GS.Actors;
using Mooege.Core.GS.Actors.Implementations.Minions;
using Mooege.Core.GS.Common.Types.Math;
using Mooege.Net.GS.Message.Definitions.ACD;
using Mooege.Core.GS.Common.Types.Misc;
using Mooege.Core.GS.Ticker;
using Mooege.Net.GS.Message;
using Mooege.Core.GS.Common.Types.TagMap;
using Mooege.Core.GS.Powers.Payloads;

namespace Mooege.Core.GS.Powers.Implementations
{
    //Complete, Rune_E is a capsule radius/dist, I use beam direction.
    #region Bash
    [ImplementsPowerSNO(Skills.Skills.Barbarian.FuryGenerators.Bash)]
    public class BarbarianBash : Skill
    {
        public override IEnumerable<TickTimer> Main()
        {
            bool hitAnything = false;
            var ShockWavePos = PowerMath.TranslateDirection2D(User.Position, TargetPosition,
                                                              User.Position,
                                                              ScriptFormula(23));
            var maxHits = ScriptFormula(1);
            for (int i = 0; i < maxHits; ++i)
            {
                AttackPayload attack = new AttackPayload(this);
                attack.Targets = GetBestMeleeEnemy();

                if (i == 0)
                    attack.AddWeaponDamage(ScriptFormula(3), DamageType.Physical);
                else
                    attack.AddWeaponDamage(ScriptFormula(12), DamageType.Physical);

                attack.OnHit = hitPayload =>
                {
                    hitAnything = true;
                    if (Rune_D > 0)
                    {
                        GeneratePrimaryResource(ScriptFormula(10));
                    }
                    if (Rune_B > 0)
                    {
                        AddBuff(User, new AddDamageBuff());
                    }

                    if (Rune_A > 0)
                    {
                    }
                    else if (Rune_C > 0)
                    {
                        if (Rand.NextDouble() < ScriptFormula(14))
                            AddBuff(hitPayload.Target, new DebuffStunned(WaitSeconds(ScriptFormula(15))));
                    }
                    else
                    {
                        if (Rand.NextDouble() < ScriptFormula(0))
                            Knockback(hitPayload.Target, ScriptFormula(5), ScriptFormula(6), ScriptFormula(7));
                    }
                };
                attack.Apply();
                yield return WaitSeconds(ScriptFormula(13));

                if (hitAnything)
                    GeneratePrimaryResource(EvalTag(PowerKeys.ResourceGainedOnFirstHit));
            }
            //Shockwave -> capsule distance, at the moment using beamdirection
            if (Rune_E > 0)
            {
                User.PlayEffectGroup(93867);
                yield return WaitSeconds(0.5f);
                WeaponDamage(GetEnemiesInBeamDirection(ShockWavePos, TargetPosition, 30f, 8f), ScriptFormula(18), DamageType.Physical);
            }
            yield break;
        }

        public override float GetContactDelay()
        {
            // seems to need this custom speed for all attacks
            return ScriptFormula(13);
        }

        [ImplementsPowerBuff(0, true)]
        public class AddDamageBuff : PowerBuff
        {
            public override void Init()
            {
                Timeout = WaitSeconds(ScriptFormula(11));
                MaxStackCount = (int)ScriptFormula(4);
            }

            public override bool Apply()
            {
                if (!base.Apply())
                    return false;
                _AddDamage();
                return true;
            }

            public override bool Stack(Buff buff)
            {
                bool stacked = StackCount < MaxStackCount;

                base.Stack(buff);

                if (stacked)
                    _AddDamage();

                return true;
            }

            public override void Remove()
            {
                base.Remove();
                User.Attributes[GameAttribute.Damage_Bonus_Min] -= StackCount * ScriptFormula(2);
                User.Attributes.BroadcastChangedIfRevealed();

            }

            private void _AddDamage()
            {
                User.Attributes[GameAttribute.Damage_Bonus_Min] += ScriptFormula(2);
                User.Attributes.BroadcastChangedIfRevealed();
            }
        }
    }
    #endregion

    //Complete:Check
    #region LeapAttack
    [ImplementsPowerSNO(Skills.Skills.Barbarian.FuryGenerators.LeapAttack)]
    public class BarbarianLeap : Skill
    {
        //there is a changed walking speed multiplier from 8101 patch.
        public override IEnumerable<TickTimer> Main()
        {
            if (!User.World.CheckLocationForFlag(TargetPosition, Mooege.Common.MPQ.FileFormats.Scene.NavCellFlags.AllowWalk))
                yield break;
            bool hitAnything = false;
            StartCooldown(EvalTag(PowerKeys.CooldownTime));

            if (Rune_C > 0)
            {
                AttackPayload launch = new AttackPayload(this);
                launch.Targets = GetEnemiesInRadius(User.Position, ScriptFormula(31));
                launch.AddWeaponDamage(ScriptFormula(30), DamageType.Physical);
                launch.OnHit = hitPayload =>
                {
                    hitAnything = true;
                };
                launch.Apply();
                User.PlayEffectGroup(165924); //Not sure if this is the only effect to be displayed in this case
            }

            ActorMover mover = new ActorMover(User);
            mover.MoveArc(TargetPosition, 10, -0.1f, new ACDTranslateArcMessage
            {
                //Field3 = 303110, // used for male barb leap, not needed?
                FlyingAnimationTagID = AnimationSetKeys.Attack2.ID,
                LandingAnimationTagID = -1,
                PowerSNO = PowerSNO
            });

            // wait for landing
            while (!mover.Update())
                yield return WaitTicks(1);

            // extra wait for leap to finish
            yield return WaitTicks(1);

            if (Rune_D > 0)
            {
                AddBuff(User, new LeapAttackArmorBuff());
            }

            // ground smash effect
            User.PlayEffectGroup(162811);

            AttackPayload attack = new AttackPayload(this);
            attack.Targets = GetEnemiesInRadius(TargetPosition, ScriptFormula(0));
            //ScriptFormula(1) states "% of willpower Damage", perhaps the damage should be calculated that way instead.
            attack.AddWeaponDamage(0.70f, DamageType.Physical);
            attack.OnHit = hitPayload =>
            {
                hitAnything = true;
                if (Rune_E > 0)
                {
                    if (Rand.NextDouble() < ScriptFormula(37))
                    {
                        AddBuff(hitPayload.Target, new DebuffStunned(WaitSeconds(ScriptFormula(38))));
                    }
                }
            };
            attack.Apply();

            if (hitAnything)
                GeneratePrimaryResource(EvalTag(PowerKeys.ResourceGainedOnFirstHit));

            //TODO: Eventually att visuals, and check if the current uber-drag is really intended :P
            if (Rune_A > 0)
            {
                TargetList targets = GetEnemiesInRadius(User.Position, ScriptFormula(3));
                Actor curTarget;
                int affectedTargets = 0;
                while (affectedTargets < ScriptFormula(12)) //SF(11) states  "Min number to Knockback", and is 5, what can that mean?
                {
                    curTarget = targets.GetClosestTo(User.Position);
                    if (curTarget != null)
                    {
                        targets.Actors.Remove(curTarget);

                        if (curTarget.World != null)
                        {
                            Knockback(curTarget, ScriptFormula(8), ScriptFormula(9), ScriptFormula(10));
                        }
                        affectedTargets++;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            if (Rune_B > 0)
            {
                TargetList targets = GetEnemiesInRadius(User.Position, ScriptFormula(3));
                foreach (Actor curTarget in targets.Actors)
                {
                    Knockback(curTarget, ScriptFormula(17), ScriptFormula(18), ScriptFormula(19));
                    var secondKnockBack = GetEnemiesInRadius(curTarget.Position, 1f); // this will hackfully check for collision
                    if (secondKnockBack.Actors.Count > 1)
                    {
                        foreach (Actor secondaryTarget in GetEnemiesInRadius(curTarget.Position, ScriptFormula(23)).Actors)
                        {
                            Knockback(secondaryTarget, ScriptFormula(24), ScriptFormula(25), ScriptFormula(26));
                            var thirdKnockBack = GetEnemiesInRadius(secondaryTarget.Position, 1f);
                            if (thirdKnockBack.Actors.Count > 1)
                            {
                                foreach (Actor thirdTarget in GetEnemiesInRadius(secondaryTarget.Position, ScriptFormula(23)).Actors)
                                {
                                    Knockback(thirdTarget, ScriptFormula(24), ScriptFormula(25), ScriptFormula(26));
                                }
                            }
                        }
                    }
                }
            }
            yield break;
        }
        [ImplementsPowerBuff(2)]
        class LeapAttackArmorBuff : PowerBuff
        {
            public override void Init()
            {
                Timeout = WaitSeconds(ScriptFormula(36));
            }

            public override bool Apply()
            {
                if (!base.Apply())
                    return false;

                User.Attributes[GameAttribute.Armor_Bonus_Percent] += ScriptFormula(33);
                User.Attributes.BroadcastChangedIfRevealed();

                return true;
            }

            public override void Remove()
            {
                base.Remove();

                User.Attributes[GameAttribute.Armor_Bonus_Percent] -= ScriptFormula(33);
                User.Attributes.BroadcastChangedIfRevealed();
            }
        }
    }
    #endregion

    //TODO: Runes_E
    #region WhirlWind
    [ImplementsPowerSNO(Skills.Skills.Barbarian.FurySpenders.Whirlwind)]
    public class BarbarianWhirlwind : Skill
    {
        public override IEnumerable<TickTimer> Main()
        {
            AddBuff(User, new WhirlwindEffect());
            yield break;
        }

        [ImplementsPowerBuff(0)]
        public class WhirlwindEffect : PowerBuff
        {
            private TickTimer _damageTimer;
            private TickTimer _tornadoSpawnTimer;

            public override void Init()
            {
                Timeout = WaitSeconds(ScriptFormula(0));
            }

            //This needs to be added into whirlwind, because your walking speed does become slower once whirlwind is active.
            public override bool Apply()
            {
                if (!base.Apply())
                    return false;
                User.Attributes[GameAttribute.Walking_Rate_Total] = User.Attributes[GameAttribute.Walking_Rate_Total] * EvalTag(PowerKeys.WalkingSpeedMultiplier);
                User.Attributes.BroadcastChangedIfRevealed();
                return true;
            }

            public override void Remove()
            {
                base.Remove();
                User.Attributes[GameAttribute.Walking_Rate_Total] = User.Attributes[GameAttribute.Walking_Rate_Total] / EvalTag(PowerKeys.WalkingSpeedMultiplier);
                User.Attributes.BroadcastChangedIfRevealed();
            }

            public override bool Update()
            {
                if (base.Update())
                    return true;

                if (_damageTimer == null || _damageTimer.TimedOut)
                {
                    _damageTimer = WaitSeconds(ScriptFormula(0));
                    UsePrimaryResource(EvalTag(PowerKeys.ResourceCost));

                    //TODO: THIS is where Rune_E goes. On Crit Hits, restores some HP.

                    WeaponDamage(GetEnemiesInRadius(User.Position, ScriptFormula(2)),
                                 ScriptFormula(1), Rune_A > 0 ? DamageType.Fire : DamageType.Physical);
                }

                if (Rune_B > 0)
                {
                    // spawn tornado projectiles in random directions every timed period
                    if (_tornadoSpawnTimer == null)
                        _tornadoSpawnTimer = WaitSeconds(ScriptFormula(5));

                    if (_tornadoSpawnTimer.TimedOut)
                    {
                        _tornadoSpawnTimer = WaitSeconds(ScriptFormula(5));

                        var tornado = new Projectile(this, 162386, User.Position);
                        tornado.Timeout = WaitSeconds(3f);
                        tornado.OnCollision = (hit) =>
                        {
                            WeaponDamage(hit, ScriptFormula(6), DamageType.Physical);
                        };
                        tornado.Launch(new Vector3D(User.Position.X + (float)Rand.NextDouble() - 0.5f,
                                                    User.Position.Y + (float)Rand.NextDouble() - 0.5f,
                                                    User.Position.Z), 0.25f);
                    }
                }

                return false;
            }
        }
    }
    #endregion

    //Complete:Check
    #region AncientSpear
    [ImplementsPowerSNO(Skills.Skills.Barbarian.FuryGenerators.AncientSpear)]
    public class BarbarianAncientSpear : Skill
    {
        public override IEnumerable<TickTimer> Main()
        {
            StartCooldown(EvalTag(PowerKeys.CooldownTime));
            bool hitAnything = false;

            if (Rune_B > 0)
            {
                Vector3D[] projDestinations = PowerMath.GenerateSpreadPositions(User.Position, TargetPosition, ScriptFormula(9) / 5f, (int)ScriptFormula(11));

                for (int i = 0; i < projDestinations.Length; i++)
                {
                    var proj = new Projectile(this, 161891, User.Position);
                    proj.Scale = 3f;
                    proj.Timeout = WaitSeconds(0.5f);
                    proj.OnCollision = (hit) =>
                    {
                        hitAnything = true;
                        _setupReturnProjectile(hit.Position);

                        AttackPayload attack = new AttackPayload(this);
                        attack.SetSingleTarget(hit);
                        attack.AddWeaponDamage(ScriptFormula(0), DamageType.Physical);
                        attack.OnHit = (hitPayload) =>
                        {
                            hitPayload.Target.PlayEffectGroup(79420);
                            Knockback(hitPayload.Target, -25f, ScriptFormula(3), ScriptFormula(4));
                        };
                        attack.Apply();

                        proj.Destroy();
                    };
                    proj.OnTimeout = () =>
                    {
                        _setupReturnProjectile(proj.Position);
                    };

                    proj.Launch(projDestinations[i], ScriptFormula(8));
                    User.AddRopeEffect(79402, proj);
                }
            }
            else if (Rune_E > 0)
            {
                Vector3D[] projDestinations = PowerMath.GenerateSpreadPositions(User.Position, TargetPosition, ScriptFormula(9) / 5f, (int)ScriptFormula(11));

                for (int i = 0; i < projDestinations.Length; i++)
                {
                    var proj = new Projectile(this, 161894, User.Position);
                    proj.Scale = 3f;
                    proj.Timeout = WaitSeconds(0.5f);
                    proj.OnCollision = (hit) =>
                    {
                        hitAnything = true;

                        _setupReturnProjectile(hit.Position);

                        AttackPayload attack = new AttackPayload(this);
                        attack.SetSingleTarget(hit);
                        attack.AddWeaponDamage(ScriptFormula(0), DamageType.Physical);
                        attack.OnHit = (hitPayload) =>
                        {
                            hitPayload.Target.PlayEffectGroup(79420);
                            Knockback(hitPayload.Target, 25f, ScriptFormula(3), ScriptFormula(4));
                        };
                        attack.Apply();

                        proj.Destroy();
                    };
                    proj.OnTimeout = () =>
                    {
                        _setupReturnProjectile(proj.Position);
                    };

                    proj.Launch(projDestinations[i], ScriptFormula(8));
                    User.AddRopeEffect(79402, proj);
                }
            }
            else if (Rune_A > 0)
            {
                var projectile = new Projectile(this, 161890, User.Position);
                projectile.Scale = 3f;
                projectile.Timeout = WaitSeconds(0.5f);
                projectile.OnCollision = (hit) =>
                {
                    hitAnything = true;

                    _setupReturnProjectile(hit.Position);

                    AttackPayload attack = new AttackPayload(this);
                    attack.SetSingleTarget(hit);
                    attack.AddWeaponDamage(ScriptFormula(0), DamageType.Physical);
                    attack.OnHit = (hitPayload) =>
                    {
                        for (int i = 0; i < ScriptFormula(17); ++i)
                        {
                            hitPayload.Target.PlayEffectGroup(79420);
                            Knockback(hitPayload.Target, -25f, ScriptFormula(3), ScriptFormula(4));
                        }
                    };
                    attack.Apply();

                };
                projectile.OnTimeout = () =>
                {
                    _setupReturnProjectile(projectile.Position);
                };

                projectile.Launch(TargetPosition, ScriptFormula(8));
                User.AddRopeEffect(79402, projectile);
            }
            else
            {
                var projectile = new Projectile(this, RuneSelect(74636, -1, -1, 161892, 161893, -1), User.Position);
                projectile.Scale = 3f;
                projectile.Timeout = WaitSeconds(0.5f);
                projectile.OnCollision = (hit) =>
                {
                    hitAnything = true;

                    _setupReturnProjectile(hit.Position);

                    AttackPayload attack = new AttackPayload(this);
                    attack.SetSingleTarget(hit);
                    attack.AddWeaponDamage(ScriptFormula(0), DamageType.Physical);
                    attack.OnHit = (hitPayload) =>
                    {
                        // GET OVER HERE
                        //unknown on magnitude/knockback offset?
                        hitPayload.Target.PlayEffectGroup(79420);
                        Knockback(hitPayload.Target, -25f, ScriptFormula(3), ScriptFormula(4));
                        if (Rune_C > 0)
                        {
                            float healMe = ScriptFormula(10) * hitPayload.TotalDamage;
                            //User.Attributes[GameAttribute.Hitpoints_Granted] = healMe;
                            //User.Attributes.BroadcastChangedIfRevealed();
                        }
                    };
                    attack.Apply();

                    projectile.Destroy();
                };
                projectile.OnTimeout = () =>
                {
                    _setupReturnProjectile(projectile.Position);
                };

                projectile.Launch(TargetPosition, ScriptFormula(8));
                User.AddRopeEffect(79402, projectile);
            }

            if (hitAnything)
                GeneratePrimaryResource(EvalTag(PowerKeys.ResourceGainedOnFirstHit));

            yield break;
        }

        private void _setupReturnProjectile(Vector3D spawnPosition)
        {
            Vector3D inFrontOfUser = PowerMath.TranslateDirection2D(User.Position, spawnPosition, User.Position, 5f);

            var return_proj = new Projectile(this, 79400, new Vector3D(spawnPosition.X, spawnPosition.Y, User.Position.Z));
            return_proj.Scale = 3f;
            return_proj.DestroyOnArrival = true;
            return_proj.LaunchArc(inFrontOfUser, 1f, -0.03f);
            User.AddRopeEffect(79402, return_proj);
        }
    }
    #endregion

    //TODO: A,C
    #region ThreateningShout
    [ImplementsPowerSNO(Skills.Skills.Barbarian.FurySpenders.ThreateningShout)]
    public class ThreateningShout : Skill
    {
        public override IEnumerable<TickTimer> Main()
        {
            StartCooldown(EvalTag(PowerKeys.CooldownTime));
            UsePrimaryResource(EvalTag(PowerKeys.ResourceCost));

            foreach (Actor enemy in GetEnemiesInRadius(User.Position, ScriptFormula(9)).Actors)
            {
                AddBuff(enemy, new ShoutDeBuff());
                if (Rune_D > 0)
                {
                    AddBuff(enemy, new ShoutAttackDeBuff());
                }
                if (Rune_A > 0)
                {
                    //Script(8) -> taunt duration
                    //taunted to attack you... guess more for multiple player...
                }
                if (Rune_C > 0)
                {
                    if (Rand.NextDouble() < ScriptFormula(7))
                    {
                        /*attack.OnDeath = (dead) =>
                            {
                                //dead.Target
                                //Drop another random loot :)
                            };*/
                    }
                }
                if (Rune_E > 0)
                {
                    //Script(10) -> Fear Death Effect Duration? what is this for..
                    if (Rand.NextDouble() < ScriptFormula(3))
                    {
                        AddBuff(enemy, new DebuffFeared(WaitSeconds(Rand.Next((int)ScriptFormula(5), (int)ScriptFormula(5) + (int)ScriptFormula(6)))));
                    }
                }
            }

            yield break;
        }
        [ImplementsPowerBuff(0)]
        public class ShoutDeBuff : PowerBuff
        {
            public override void Init()
            {
                Timeout = WaitSeconds(ScriptFormula(2));
            }
            public override bool Apply()
            {
                if (!base.Apply())
                    return false;
                Target.Attributes[GameAttribute.Damage_Done_Reduction_Percent] += ScriptFormula(0);
                if (Rune_B > 0)
                {
                    Target.Attributes[GameAttribute.Movement_Scalar_Reduction_Percent] += ScriptFormula(14);
                }
                Target.Attributes.BroadcastChangedIfRevealed();
                return true;
            }
            public override void Remove()
            {
                base.Remove();
                Target.Attributes[GameAttribute.Damage_Done_Reduction_Percent] -= ScriptFormula(0);
                if (Rune_B > 0)
                {
                    Target.Attributes[GameAttribute.Movement_Scalar_Reduction_Percent] -= ScriptFormula(14);
                }
                Target.Attributes.BroadcastChangedIfRevealed();
            }
        }
        [ImplementsPowerBuff(4)]
        public class ShoutAttackDeBuff : PowerBuff
        {
            public override void Init()
            {
                Timeout = WaitSeconds(ScriptFormula(17));
            }
            public override bool Apply()
            {
                if (!base.Apply())
                    return false;
                Target.Attributes[GameAttribute.Attacks_Per_Second_Percent] += ScriptFormula(4);
                Target.Attributes.BroadcastChangedIfRevealed();
                return true;
            }
            public override void Remove()
            {
                base.Remove();
                Target.Attributes[GameAttribute.Attacks_Per_Second_Percent] -= ScriptFormula(4);
                Target.Attributes.BroadcastChangedIfRevealed();
            }
        }

    }
    #endregion

    //TODO: Rune_D
    #region HammerOfTheAncients
    [ImplementsPowerSNO(Skills.Skills.Barbarian.FurySpenders.HammerOfTheAncients)]
    public class HammerOfTheAncients : Skill
    {
        public override IEnumerable<TickTimer> Main()
        {
            UsePrimaryResource(EvalTag(PowerKeys.ResourceCost));

            if (Rune_B > 0)
            {

                AttackPayload attack = new AttackPayload(this);
                attack.Targets = GetEnemiesInArcDirection(User.Position, TargetPosition, ScriptFormula(14), ScriptFormula(15));
                attack.AddWeaponDamage(ScriptFormula(23), DamageType.Physical);
                attack.Apply();
                yield break;
            }
            else
            {
                TargetPosition = PowerMath.TranslateDirection2D(User.Position, TargetPosition, User.Position, ScriptFormula(11));

                AttackPayload attack = new AttackPayload(this);
                attack.Targets = GetEnemiesInRadius(TargetPosition, ScriptFormula(11));
                attack.AddWeaponDamage(ScriptFormula(4), DamageType.Physical);
                attack.OnHit = hitPayload =>
                {
                    if (Rune_D > 0)
                    {
                        if (hitPayload.IsCriticalHit)
                        {
                            if (Rand.NextDouble() < ScriptFormula(5))
                            {
                                //drop treasure or health globes.
                            }
                        }
                    }
                    if (Rune_C > 0)
                    {
                        AddBuff(hitPayload.Target, new DebuffSlowed(ScriptFormula(8), WaitSeconds(ScriptFormula(10))));
                    }
                };
                attack.OnDeath = DeathPayload =>
                {
                    if (Rune_E > 0)
                    {
                        //if (DeathPayload.Target)? Doesn't above handle this?
                        {
                            if (Rand.NextDouble() < ScriptFormula(16))
                            {
                                AttackPayload Stunattack = new AttackPayload(this);
                                Stunattack.Targets = GetEnemiesInRadius(User.Position, ScriptFormula(18));
                                Stunattack.OnHit = stun =>
                                {
                                    AddBuff(stun.Target, new DebuffStunned(WaitSeconds(ScriptFormula(17))));
                                };
                                Stunattack.Apply();
                            }
                        }
                    }
                };
                attack.Apply();

                if (Rune_C > 0)
                {
                    var QuakeHammer = SpawnEffect(159030, User.Position, 0, WaitSeconds(ScriptFormula(10)));
                    QuakeHammer.UpdateDelay = 1f;
                    QuakeHammer.OnUpdate = () =>
                    {
                        AttackPayload TremorAttack = new AttackPayload(this);
                        TremorAttack.Targets = GetEnemiesInRadius(User.Position, ScriptFormula(7));
                        TremorAttack.AddWeaponDamage(ScriptFormula(9), DamageType.Physical);
                        TremorAttack.Apply();
                    };
                }
            }
            yield break;
        }
    }
    #endregion

    //TODO: just needs attribute check/crit/timer
    #region BattleRage
    [ImplementsPowerSNO(Skills.Skills.Barbarian.FurySpenders.BattleRage)]
    public class BattleRage : Skill
    {
        public override IEnumerable<TickTimer> Main()
        {
            StartCooldown(EvalTag(PowerKeys.CooldownTime));
            UsePrimaryResource(EvalTag(PowerKeys.ResourceCost));

            AddBuff(User, new BattleRageEffect());

            yield break;
        }

        [ImplementsPowerBuff(0)]
        public class BattleRageEffect : PowerBuff
        {
            public override void Init()
            {
                //todo: allow the addition of more seconds from Rune_B with a max of 600 seconds.
                Timeout = WaitSeconds(ScriptFormula(0));
            }

            public override bool Apply()
            {
                if (!base.Apply())
                    return false;
                //Rune_A
                //Total Damage Bonus
                User.Attributes[GameAttribute.Damage_Weapon_Percent_Bonus] += ScriptFormula(1);

                //Crit Chance Bonus
                User.Attributes[GameAttribute.Crit_Damage_Percent] += (int)ScriptFormula(2);
                User.Attributes.BroadcastChangedIfRevealed();

                return true;
            }

            public override void OnPayload(Payload payload)
            {
                if (payload is HitPayload && Target.Equals(payload.Context.User))
                {
                    //if (payload.IsCriticalHit)
                    {
                        if (Rune_B > 0)
                        {
                            if (Rand.NextDouble() < ScriptFormula(5))
                            {
                                //ScriptFormula(4) -> extends duration 
                            }
                        }
                        if (Rune_C > 0)
                        {
                            //or ScriptFormula(15)
                            if (Rand.NextDouble() < ScriptFormula(6))
                            {
                                //drop additional health globes.
                            }
                        }
                        if (Rune_D > 0)
                        {
                            GeneratePrimaryResource(ScriptFormula(3));
                        }
                        if (Rune_E > 0)
                        {
                            User.PlayEffectGroup(210321);
                            WeaponDamage(GetEnemiesInRadius(User.Position, ScriptFormula(9)), ScriptFormula(10), DamageType.Physical);
                        }
                    }
                }
            }
            public override void Remove()
            {
                base.Remove();
                //Total Damage Bonus
                User.Attributes[GameAttribute.Damage_Weapon_Percent_Bonus] -= ScriptFormula(1);

                //Crit Chance Bonus
                User.Attributes[GameAttribute.Crit_Damage_Percent] -= (int)ScriptFormula(2);
                User.Attributes.BroadcastChangedIfRevealed();
            }
        }
    }
    #endregion

    //Complete:Check
    #region Cleave
    [ImplementsPowerSNO(Skills.Skills.Barbarian.FuryGenerators.Cleave)]
    public class Cleave : Skill
    {
        public override IEnumerable<TickTimer> Main()
        {
            bool hitAnything = false;
            WeaponDamage(GetBestMeleeEnemy(), ScriptFormula(16), DamageType.Physical);
            AttackPayload attack = new AttackPayload(this);
            attack.Targets = GetEnemiesInArcDirection(User.Position, TargetPosition, ScriptFormula(27), ScriptFormula(26));
            attack.AddWeaponDamage(ScriptFormula(16), DamageType.Physical);
            attack.OnHit = hitPayload =>
            {
                hitAnything = true;
                if (Rune_B > 0)
                {
                    foreach (Actor enemy in GetEnemiesInArcDirection(User.Position, TargetPosition, ScriptFormula(27), ScriptFormula(26)).Actors)
                    {
                        //Snare Duration = SF(14)
                        //Snare Magnitude = SF(20)
                    }
                }
                if (Rune_C > 0)
                {
                    if (hitPayload.IsCriticalHit)
                    {
                        Knockback(hitPayload.Target, ScriptFormula(2), ScriptFormula(3));

                        //since its a max number of knockback jumps, but its UP TO that number, we will randomize it.
                        int Jumps = Rand.Next((int)ScriptFormula(25));
                        for (int i = 0; i < Jumps; ++i)
                        {
                            WeaponDamage(hitPayload.Target, ScriptFormula(9), DamageType.Physical);
                            Knockback(hitPayload.Target, ScriptFormula(7), ScriptFormula(8));
                        }
                    }
                }
                if (Rune_D > 0)
                {
                    //todo: is this right?
                    int StackCount = 0;
                    if (StackCount < ScriptFormula(23) + 1)
                    {
                        StackCount++;
                    }

                    GeneratePrimaryResource(ScriptFormula(22) * StackCount);
                }
                if (Rune_E > 0)
                {
                    //this doesnt work either.
                    attack.OnDeath = DeathPayload =>
                    {
                        if (Rand.NextDouble() < ScriptFormula(12))
                        {
                            AttackPayload explode = new AttackPayload(this);
                            explode.Targets = GetEnemiesInRadius(DeathPayload.Target.Position, ScriptFormula(6));
                            explode.AddWeaponDamage(ScriptFormula(4), DamageType.Physical);
                            explode.Apply();
                        }
                    };
                }
            };
            attack.Apply();

            if (hitAnything)
                GeneratePrimaryResource(EvalTag(PowerKeys.ResourceGainedOnFirstHit));

            yield break;
        }
    }
    #endregion

    //TODO: Concept written, needs review for A & E
    #region IgnorePain
    [ImplementsPowerSNO(Skills.Skills.Barbarian.Situational.IgnorePain)]
    public class IgnorePain : Skill
    {
        public override IEnumerable<TickTimer> Main()
        {
            StartCooldown(EvalTag(PowerKeys.CooldownTime));
            UsePrimaryResource(EvalTag(PowerKeys.ResourceCost));

            AddBuff(User, new IgnorePainBuff());
            if (Rune_C > 0)
            {
                foreach (Actor ally in GetAlliesInRadius(User.Position, ScriptFormula(4)).Actors)
                    AddBuff(ally, new ObsidianAlliesBuff());
            }
            yield break;
        }

        [ImplementsPowerBuff(0)]
        public class IgnorePainBuff : PowerBuff
        {
            public override void Init()
            {
                Timeout = WaitSeconds(ScriptFormula(0));
            }

            public override bool Apply()
            {
                if (!base.Apply())
                    return false;
                User.Attributes[GameAttribute.Damage_Percent_Reduction_From_Melee] += Convert.ToInt32(ScriptFormula(10));
                User.Attributes[GameAttribute.Damage_Percent_Reduction_From_Ranged] += Convert.ToInt32(ScriptFormula(10));
                Target.Attributes.BroadcastChangedIfRevealed();
                if (Rune_D > 0)
                {
                    AttackPayload attack = new AttackPayload(this);
                    attack.Targets = GetEnemiesInRadius(User.Position, ScriptFormula(8));
                    attack.AddWeaponDamage(ScriptFormula(11), DamageType.Physical);
                    attack.OnHit = hitPayload =>
                    {
                        Knockback(hitPayload.Target, ScriptFormula(6), ScriptFormula(7));
                    };
                    attack.Apply();
                }
                return true;
            }

            //OnPayload -> Rune_E
            //Calculate Hit Total from mob and
            //float healMe = ScriptFormula(1) * hitPayload.TotalDamage;
            //User.Attributes[GameAttribute.Hitpoints_Granted] = healMe;
            //User.Attributes.BroadcastChangedIfRevealed();

            //OnPayload -> Rune_A
            //float reflectBack = ScriptFormula(9) * hitPayload.TotalDamage;
            //WeaponDamage(hitPayload.Target, reflectBack, DamageType.Physical);



            public override void Remove()
            {
                base.Remove();
                User.Attributes[GameAttribute.Damage_Percent_Reduction_From_Melee] -= Convert.ToInt32(ScriptFormula(10));
                User.Attributes[GameAttribute.Damage_Percent_Reduction_From_Ranged] -= Convert.ToInt32(ScriptFormula(10));
                Target.Attributes.BroadcastChangedIfRevealed();
            }
        }
        [ImplementsPowerBuff(1)]
        public class ObsidianAlliesBuff : PowerBuff
        {
            public override void Init()
            {
                Timeout = WaitSeconds(ScriptFormula(3));
            }
            public override bool Apply()
            {
                if (!base.Apply())
                    return false;

                Target.Attributes[GameAttribute.Damage_Percent_Reduction_From_Melee] -= Convert.ToInt32(ScriptFormula(10));
                Target.Attributes[GameAttribute.Damage_Percent_Reduction_From_Ranged] -= Convert.ToInt32(ScriptFormula(10));
                Target.Attributes.BroadcastChangedIfRevealed();


                return true;
            }

            public override void Remove()
            {
                base.Remove();

                Target.Attributes[GameAttribute.Damage_Percent_Reduction_From_Melee] -= Convert.ToInt32(ScriptFormula(10));
                Target.Attributes[GameAttribute.Damage_Percent_Reduction_From_Ranged] -= Convert.ToInt32(ScriptFormula(10));
                Target.Attributes.BroadcastChangedIfRevealed();
            }
        }
    }
    #endregion

    //TODO: Rune_B | Rune_E -> Figure Out TeamID
    #region WeaponThrow
    [ImplementsPowerSNO(Skills.Skills.Barbarian.FurySpenders.WeaponThrow)]
    public class WeaponThrow : Skill
    {
        public override IEnumerable<TickTimer> Main()
        {
            StartCooldown(EvalTag(PowerKeys.CooldownTime));
            UsePrimaryResource(EvalTag(PowerKeys.ResourceCost));

            if (Rune_B > 0)
            {
                //throw projectile -> once collision with target, RopeEffect up to ScriptFromula(5) times.
            }
            else
            {
                var proj = new Projectile(this, RuneSelect(100800, 100839, 166438, 100832, 101057, 100934), User.Position);
                proj.Position.Z += 5f;  // fix height
                proj.OnCollision = (hit) =>
                {
                    hit.PlayEffectGroup(RuneSelect(18707, 166333, 16634, 166335, -1, 166339));
                    WeaponDamage(hit, ScriptFormula(15), DamageType.Physical);
                    if (Rune_C > 0)
                    {
                        if (Rand.NextDouble() < ScriptFormula(0))
                        {
                            AddBuff(hit, new DebuffStunned(WaitSeconds(ScriptFormula(7))));
                        }
                    }
                    if (Rune_D > 0)
                    {
                        float CurrentResource = User.Attributes[GameAttribute.Resource_Cur];
                        //Use remaining Fury points. Get Remaining Fury Points, use that to multiply against SF(18), and add that to total damage.
                        float DamageTotal = ScriptFormula(15) + (ScriptFormula(18) * CurrentResource);

                        WeaponDamage(GetEnemiesInRadius(hit.Position, ScriptFormula(19)), DamageTotal, DamageType.Physical);
                    }
                    if (Rune_E > 0)
                    {
                        if (Rand.NextDouble() < 1)
                        {
                            AddBuff(hit, new ConfuseDebuff());
                            //This will cause a buff on the target hit, changing their attack from the User, to other Mobs.
                        }
                    }
                    proj.Destroy();
                };
                proj.Launch(TargetPosition, 1.5f);
            }
            yield break;
        }
        [ImplementsPowerBuff(0)]
        public class ConfuseDebuff : PowerBuff
        {
            public override void Init()
            {
                Timeout = WaitSeconds(ScriptFormula(10));
            }

            public override bool Apply()
            {
                if (!base.Apply())
                    return false;
                Target.Attributes[GameAttribute.TeamID] = 0;
                Target.Attributes.BroadcastChangedIfRevealed();
                return true;
            }

            public override void Remove()
            {
                base.Remove();
                Target.Attributes[GameAttribute.TeamID] = 0;
                Target.Attributes.BroadcastChangedIfRevealed();
            }
        }
    }
    #endregion

    //Complete:Check
    #region GroundStomp
    [ImplementsPowerSNO(Skills.Skills.Barbarian.FuryGenerators.GroundStomp)]
    public class GroundStomp : Skill
    {
        public override IEnumerable<TickTimer> Main()
        {
            bool hitAnything = false;
            StartCooldown(EvalTag(PowerKeys.CooldownTime));

            User.PlayEffectGroup(RuneSelect(18685, 99685, 159415, 159416, 159397, 18685));
            //Rune_E -> when stun wears off, use slow.efg

            AttackPayload attack = new AttackPayload(this);
            attack.Targets = GetEnemiesInRadius(User.Position, ScriptFormula(0));
            attack.AddWeaponDamage(ScriptFormula(6), DamageType.Physical);
            attack.OnHit = hitPayload =>
            {
                hitAnything = true;
                if (Rune_B > 0)
                {
                    //push em away!
                    Knockback(hitPayload.Target, ScriptFormula(13), ScriptFormula(14));
                }
                if (Rune_C > 0)
                {
                    //bring em in!
                    Knockback(hitPayload.Target, ScriptFormula(11), ScriptFormula(12));
                }
                AddBuff(hitPayload.Target, new GroundStompStun());
            };
            attack.Apply();
            if (hitAnything)
                GeneratePrimaryResource(EvalTag(PowerKeys.ResourceGainedOnFirstHit) + ScriptFormula(19));

            yield break;
        }
        [ImplementsPowerBuff(0)]
        public class GroundStompStun : PowerBuff
        {
            public override void Init()
            {
                Timeout = WaitSeconds(ScriptFormula(1));
            }

            public override bool Apply()
            {
                if (!base.Apply())
                    return false;

                new DebuffStunned(WaitSeconds(ScriptFormula(1)));

                return true;
            }

            public override void Remove()
            {
                base.Remove();
                if (Rune_E > 0)
                {
                    new GroundStompSlow();
                }
            }
        }
        [ImplementsPowerBuff(0)]
        public class GroundStompSlow : PowerBuff
        {
            public override void Init()
            {
                Timeout = WaitSeconds(ScriptFormula(4));
                Target.PlayEffectGroup(159418);
            }

            public override bool Apply()
            {
                if (!base.Apply())
                    return false;
                Target.Attributes[GameAttribute.Slow] = true;
                Target.Attributes[GameAttribute.Movement_Scalar_Reduction_Percent] += ScriptFormula(5);
                Target.Attributes.BroadcastChangedIfRevealed();

                return true;
            }

            public override void Remove()
            {
                base.Remove();
                Target.Attributes[GameAttribute.Slow] = false;
                Target.Attributes[GameAttribute.Movement_Scalar_Reduction_Percent] -= ScriptFormula(5);
                Target.Attributes.BroadcastChangedIfRevealed();
            }
        }
    }
    #endregion

    //Complete:Check
    #region Rend
    [ImplementsPowerSNO(Skills.Skills.Barbarian.FurySpenders.Rend)]
    public class Rend : Skill
    {
        public override IEnumerable<TickTimer> Main()
        {
            UsePrimaryResource(EvalTag(PowerKeys.ResourceCost));

            AttackPayload attack = new AttackPayload(this);
            attack.Targets = GetEnemiesInRadius(User.Position, ScriptFormula(4));
            attack.OnHit = hitPayload =>
            {
                AddBuff(hitPayload.Target, new RendDebuff());
                if (Rune_D > 0)
                {
                    float healMe = ScriptFormula(8) * hitPayload.TotalDamage;
                    //User.Attributes[GameAttribute.Hitpoints_Granted] = healMe;
                    //User.Attributes.BroadcastChangedIfRevealed();
                }
            };
            //this work? if it dies with rend debuff, infect others.
            attack.OnDeath = DeathPayload =>
            {
                if (AddBuff(Target, new RendDebuff()))
                {
                    AttackPayload BleedAgain = new AttackPayload(this);
                    BleedAgain.Targets = GetEnemiesInRadius(Target.Position, ScriptFormula(19));
                    BleedAgain.OnHit = hitPayload2 =>
                    {
                        AddBuff(hitPayload2.Target, new RendDebuff());
                    };
                    BleedAgain.Apply();
                }
            };
            attack.Apply();
            yield break;
        }
        [ImplementsPowerBuff(0, true)]
        public class RendDebuff : PowerBuff
        {
            const float _damageRate = 1f;
            TickTimer _damageTimer = null;

            public override void Init()
            {
                Timeout = WaitSeconds(ScriptFormula(5));
                MaxStackCount = (int)ScriptFormula(10);
            }

            public override bool Apply()
            {
                if (!base.Apply())
                    return false;
                if (Rand.NextDouble() < ScriptFormula(9))
                {
                    //for rend, would we just stack updates?
                    Update();
                }
                return true;
            }
            public override bool Update()
            {
                if (base.Update())
                    return true;
                if (_damageTimer == null || _damageTimer.TimedOut)
                {
                    _damageTimer = WaitSeconds(_damageRate);
                    if (Rune_E > 0)
                    {
                        WeaponDamage(Target, ScriptFormula(7), DamageType.Physical);
                    }
                    else
                        WeaponDamage(Target, ScriptFormula(20), DamageType.Physical);
                }
                return false;
            }

            public override bool Stack(Buff buff)
            {
                bool stacked = StackCount < MaxStackCount;

                base.Stack(buff);

                if (stacked)
                    Update();

                return true;
            }

            public override void Remove()
            {
                base.Remove();

            }
        }
    }
    #endregion

    //Complete:Check
    #region Frenzy
    [ImplementsPowerSNO(Skills.Skills.Barbarian.FuryGenerators.Frenzy)]
    public class Frenzy : Skill
    {
        public override IEnumerable<TickTimer> Main()
        {
            bool hitAnything = false;
            AttackPayload attack = new AttackPayload(this);
            attack.Targets = GetBestMeleeEnemy();
            attack.AddWeaponDamage(2f, DamageType.Physical);
            attack.OnHit = hitPayload =>
            {
                hitAnything = true;
                AddBuff(User, new FrenzyBuff());
                if (Rune_C > 0)
                {
                    AddBuff(User, new ObsidianSpeedEffect());
                }
                if (Rune_B > 0)
                {
                    if (Rand.NextDouble() < ScriptFormula(17))
                    {
                        Actor target = GetEnemiesInRadius(User.Position, 15f).GetClosestTo(User.Position);

                        var proj = new Projectile(this, RuneSelect(6515, 130073, 215555, -1, 216040, 75650), User.Position);
                        proj.Position.Z += 5f;  // fix height
                        proj.OnCollision = (hit) =>
                        {
                            WeaponDamage(hit, ScriptFormula(18), DamageType.Physical);
                        };
                        proj.Launch(target.Position, 2f);
                    }
                }
                if (Rune_D > 0)
                {
                    if (Rand.NextDouble() < 1)
                    {
                        hitPayload.Target.PlayEffectGroup(163470);
                        AddBuff(hitPayload.Target, new DebuffStunned(WaitSeconds(ScriptFormula(0))));
                    }
                }
            };
            attack.OnDeath = DeathPayload =>
            {
                if (Rune_E > 0)
                {
                    User.Attributes[GameAttribute.Hitpoints_Granted_Duration] += (int)ScriptFormula(12);
                    User.Attributes[GameAttribute.Hitpoints_Granted] += ScriptFormula(10) * User.Attributes[GameAttribute.Hitpoints_Max_Total];
                    User.Attributes.BroadcastChangedIfRevealed();
                }
            };
            attack.Apply();
            if (hitAnything)
                GeneratePrimaryResource(EvalTag(PowerKeys.ResourceGainedOnFirstHit) + ScriptFormula(19));
            yield break;
        }
        [ImplementsPowerBuff(0, true)]
        class FrenzyBuff : PowerBuff
        {
            public override void Init()
            {
                Timeout = WaitSeconds(ScriptFormula(5));
                MaxStackCount = (int)ScriptFormula(3);
            }

            public override bool Apply()
            {
                if (!base.Apply())
                    return false;
                _AddFrenzy();
                return true;
            }

            public override bool Stack(Buff buff)
            {
                bool stacked = StackCount < MaxStackCount;

                base.Stack(buff);

                if (stacked)
                    _AddFrenzy();

                return true;
            }

            public override void Remove()
            {
                base.Remove();
                if (Rune_A > 0)
                {
                    User.Attributes[GameAttribute.Attack_Bonus_Percent] -= StackCount * ScriptFormula(11);
                }
                User.Attributes[GameAttribute.Attacks_Per_Second_Bonus] -= StackCount * ScriptFormula(6);
                User.Attributes.BroadcastChangedIfRevealed();

            }

            private void _AddFrenzy()
            {
                if (Rune_A > 0)
                {
                    User.Attributes[GameAttribute.Attack_Bonus_Percent] += ScriptFormula(11);
                }
                User.Attributes[GameAttribute.Attacks_Per_Second_Bonus] += ScriptFormula(6);
                User.Attributes.BroadcastChangedIfRevealed();
            }
        }
        //powerbuff(1) = Healing Over Time buff

        [ImplementsPowerBuff(3)]
        class ObsidianSpeedEffect : PowerBuff
        {
            public override void Init()
            {
                Timeout = WaitSeconds(ScriptFormula(5));
            }
            public override bool Apply()
            {
                if (!base.Apply())
                    return false;
                User.Attributes[GameAttribute.Movement_Bonus_Run_Speed] += ScriptFormula(8);
                User.Attributes.BroadcastChangedIfRevealed();

                return true;
            }
            public override void Remove()
            {
                base.Remove();
                User.Attributes[GameAttribute.Movement_Bonus_Run_Speed] -= ScriptFormula(8);
                User.Attributes.BroadcastChangedIfRevealed();

            }
        }
    }
    #endregion

    //Incomplete, very confusing how to active this with using everthing else and how to end it when pressed.
    //Possibly wait to finish this when we get Passives activated.
    #region Revenege
    [ImplementsPowerSNO(Skills.Skills.Barbarian.Situational.Revenge)]
    public class Revenge : Skill
    {
        public override IEnumerable<TickTimer> Main()
        {
            AttackPayload attack = new AttackPayload(this);
            attack.Targets = GetBestMeleeEnemy();
            attack.OnHit = hitPayload =>
            {
                if (Rand.NextDouble() < ScriptFormula(20))
                {
                    //Skill_Override? // Skill_Override_Ended //Skill_Override_Ended_Active
                    User.Attributes[GameAttribute.Skill_Override_Active] = true;
                }
            };
            yield break;
        }
    }
    #endregion

    //Complete, stupid error I made, check it.
    #region WarCry
    [ImplementsPowerSNO(Skills.Skills.Barbarian.FuryGenerators.WarCry)]
    public class WarCry : Skill
    {
        public override IEnumerable<TickTimer> Main()
        {
            StartCooldown(EvalTag(PowerKeys.CooldownTime));
            GeneratePrimaryResource(ScriptFormula(3));

            AddBuff(User, new WarCryBuff());

            foreach (Actor ally in GetAlliesInRadius(User.Position, ScriptFormula(11)).Actors)
            {
                AddBuff(User, new WarCryAllyBuff());
            }
            yield return WaitSeconds(0.5f);
        }
        //4 different powerbuffs, figure out which one is which.
        [ImplementsPowerBuff(0)]
        class WarCryBuff : PowerBuff
        {
            public override void Init()
            {
                Timeout = WaitSeconds(ScriptFormula(2));
            }

            public override bool Apply()
            {
                if (!base.Apply())
                    return false;
                if (Rune_B > 0)
                {
                    User.Attributes[GameAttribute.Dodge_Chance_Bonus] += ScriptFormula(14);
                }
                if (Rune_C > 0)
                {
                    User.Attributes[GameAttribute.Resistance_Percent] += ScriptFormula(4);
                }
                if (Rune_E > 0)
                {
                    User.Attributes[GameAttribute.Hitpoints_Max_Percent_Bonus] += ScriptFormula(5);
                    User.Attributes[GameAttribute.Hitpoints_Regen_Per_Second] += ScriptFormula(6);
                }
                User.Attributes[GameAttribute.Defense_Bonus_Percent] += ScriptFormula(0);
                User.Attributes.BroadcastChangedIfRevealed();

                return true;
            }

            public override void Remove()
            {
                base.Remove();
                if (Rune_B > 0)
                {
                    User.Attributes[GameAttribute.Dodge_Chance_Bonus] -= ScriptFormula(14);
                }
                if (Rune_C > 0)
                {
                    User.Attributes[GameAttribute.Resistance_Percent] -= ScriptFormula(4);
                }
                if (Rune_E > 0)
                {
                    User.Attributes[GameAttribute.Hitpoints_Max_Percent_Bonus] -= ScriptFormula(5);
                    User.Attributes[GameAttribute.Hitpoints_Regen_Per_Second] -= ScriptFormula(6);
                }
                User.Attributes[GameAttribute.Defense_Bonus_Percent] -= ScriptFormula(0);
                User.Attributes.BroadcastChangedIfRevealed();
            }
        }
        [ImplementsPowerBuff(2)]
        class WarCryAllyBuff : PowerBuff
        {
            public override void Init()
            {
                Timeout = WaitSeconds(ScriptFormula(2));
            }

            public override bool Apply()
            {
                if (!base.Apply())
                    return false;
                if (Rune_B > 0)
                {
                    Target.Attributes[GameAttribute.Dodge_Chance_Bonus] += ScriptFormula(14);
                }
                if (Rune_C > 0)
                {
                    Target.Attributes[GameAttribute.Resistance_Percent] += ScriptFormula(4);
                }
                if (Rune_E > 0)
                {
                    Target.Attributes[GameAttribute.Hitpoints_Max_Percent_Bonus] += ScriptFormula(5);
                    Target.Attributes[GameAttribute.Hitpoints_Regen_Per_Second] += ScriptFormula(6);
                }
                Target.Attributes[GameAttribute.Defense_Bonus_Percent] += ScriptFormula(0);
                Target.Attributes.BroadcastChangedIfRevealed();
                return true;
            }

            public override void Remove()
            {
                base.Remove();
                if (Rune_B > 0)
                {
                    Target.Attributes[GameAttribute.Dodge_Chance_Bonus] -= ScriptFormula(14);
                }
                if (Rune_C > 0)
                {
                    Target.Attributes[GameAttribute.Resistance_Percent] -= ScriptFormula(4);
                }
                if (Rune_E > 0)
                {
                    Target.Attributes[GameAttribute.Hitpoints_Max_Percent_Bonus] -= ScriptFormula(5);
                    Target.Attributes[GameAttribute.Hitpoints_Regen_Per_Second] -= ScriptFormula(6);
                }
                Target.Attributes[GameAttribute.Defense_Bonus_Percent] -= ScriptFormula(0);
                Target.Attributes.BroadcastChangedIfRevealed();
            }
        }
    }
    #endregion

    //still terrible. needs to be redone.
    #region FuriousCharge
    [ImplementsPowerSNO(Skills.Skills.Barbarian.FuryGenerators.FuriousCharge)]
    public class FuriousCharge : Skill
    {
        public override IEnumerable<TickTimer> Main()
        {
            Target = GetEnemiesInRadius(TargetPosition, 12f).GetClosestTo(TargetPosition);

            if (Target != null)
            { TargetPosition = PowerMath.TranslateDirection2D(User.Position, Target.Position, Target.Position, 5f); }
            else
            { TargetPosition = PowerMath.TranslateDirection2D(User.Position, TargetPosition, TargetPosition, 0f); }

            var dashBuff = new DashMoverBuff(TargetPosition);
            AddBuff(User, dashBuff);
            yield return dashBuff.Timeout;

            GeneratePrimaryResource(EvalTag(PowerKeys.ResourceGainedOnFirstHit)); //since its furygenerator -> this should be fine without being the first hit.
            if (Target != null && Target.World != null) // target could've died or left world
            {
                User.TranslateFacing(Target.Position, true);
                yield return WaitSeconds(0.1f);
                User.PlayEffectGroup(166193);
                foreach (Actor enemy in GetEnemiesInArcDirection(User.Position, Target.Position, ScriptFormula(13), 8f).Actors)
                {
                    WeaponDamage(enemy, ScriptFormula(14), DamageType.Physical);
                    Knockback(enemy, ScriptFormula(10));
                }
            }
            yield break;
        }

        [ImplementsPowerBuff(0)]
        class DashMoverBuff : PowerBuff
        {
            const float _damageRate = 0.05f;
            TickTimer _damageTimer = null;

            private Vector3D _destination;
            private ActorMover _mover;

            bool hitAnything = false;

            public DashMoverBuff(Vector3D destination)
            {
                _destination = destination;
            }

            public override bool Apply()
            {
                if (!base.Apply())
                    return false;

                float speed = User.Attributes[GameAttribute.Running_Rate_Total] * EvalTag(PowerKeys.WalkingSpeedMultiplier);

                User.TranslateFacing(_destination, true);
                _mover = new ActorMover(User);
                _mover.Move(_destination, speed, new ACDTranslateNormalMessage
                {
                    TurnImmediately = true,
                    AnimationTag = 69808,
                });

                // make sure buff timeout is big enough otherwise the client will sometimes ignore the visual effects.
                TickTimer minDashWait = WaitSeconds(0.15f);
                Timeout = minDashWait.TimeoutTick > _mover.ArrivalTime.TimeoutTick ? minDashWait : _mover.ArrivalTime;

                User.Attributes.BroadcastChangedIfRevealed();
                return true;
            }

            public override void Remove()
            {
                base.Remove();
                User.Attributes.BroadcastChangedIfRevealed();

                //this might not doing it for just the first hit. 
                if (hitAnything)
                    GeneratePrimaryResource(EvalTag(PowerKeys.ResourceGainedOnFirstHit));
            }

            public override bool Update()
            {
                _mover.Update();

                if (base.Update())
                    return true;
                if (_damageTimer == null || _damageTimer.TimedOut)
                {
                    _damageTimer = WaitSeconds(_damageRate);
                    AttackPayload attack = new AttackPayload(this);
                    attack.AddWeaponDamage(ScriptFormula(16), DamageType.Physical);
                    attack.Targets = GetEnemiesInRadius(User.Position, ScriptFormula(6));
                    attack.OnHit = hit =>
                    {
                        hitAnything = true;
                        Knockback(hit.Target, ScriptFormula(2));
                        if (Rune_B > 0)
                        {
                            User.Attributes[GameAttribute.Hitpoints_Granted] += ScriptFormula(21);
                            User.Attributes.BroadcastChangedIfRevealed();
                        }
                        if (Rune_D > 0)
                        {
                            GeneratePrimaryResource(ScriptFormula(11));
                        }
                        if (Rune_C > 0)
                        {
                            if (hit.IsCriticalHit)
                            {
                                AddBuff(hit.Target, new DebuffStunned(WaitSeconds(ScriptFormula(8))));
                            }
                        }
                        if (Rune_E > 0)
                        {
                            int resourceHit = 0;
                            while (resourceHit < ScriptFormula(10))
                            {
                                //maybe this cannot go here?
                            }
                        }
                    };
                    attack.Apply();
                }
                return false;
            }
        }
    }
    #endregion

    //TODO: E, B (check/fix A and C)
    #region Overpower
    [ImplementsPowerSNO(Skills.Skills.Barbarian.Situational.Overpower)]
    public class Overpower : Skill
    {
        public override IEnumerable<TickTimer> Main()
        {
            StartCooldown(EvalTag(PowerKeys.CooldownTime));

            TickTimer Cooldown = WaitSeconds(ScriptFormula(5));

            if (Rune_A > 0)
            {
                AddBuff(User, new DurationBuff());
            }
            if (Rune_E > 0)
            {
                AddBuff(User, new ReflectBuff());
            }
            AttackPayload attack = new AttackPayload(this);
            attack.Targets = GetEnemiesInRadius(User.Position, ScriptFormula(7));
            attack.AddWeaponDamage(ScriptFormula(7), DamageType.Physical);
            attack.OnHit = HitPayload =>
            {
                if (Rune_C > 0)
                {
                    //TODO: is this the intended way to heal?
                    User.Attributes[GameAttribute.Hitpoints_Cur] =
                        Math.Min(User.Attributes[GameAttribute.Hitpoints_Cur] +
                        ScriptFormula(23) * User.Attributes[GameAttribute.Hitpoints_Max],
                        User.Attributes[GameAttribute.Hitpoints_Max]);

                    User.Attributes.BroadcastChangedIfRevealed();
                }
                if (Rune_D > 0)
                {
                    GeneratePrimaryResource(ScriptFormula(28));
                }
                if (HitPayload.IsCriticalHit)
                {
                    Cooldown = WaitSeconds(0f);
                }
            };
            attack.Apply();

            if (Rune_B > 0)
            {
                //Throw up to SF(18) axes at nearby enemies which inflict SF(14) weapon damage each.
                //search radius SF(15), proj height SF(16), proj speed SF(17), proj delay time SF(19)
            }

            StartCooldown(Cooldown);
            yield break;
        }
        [ImplementsPowerBuff(0)]
        class ReflectBuff : PowerBuff
        {
            public override void Init()
            {
                Timeout = WaitSeconds(ScriptFormula(5));
            }

            public override bool Apply()
            {
                if (!base.Apply())
                    return false;
                return true;
            }

            public override void OnPayload(Payload payload)
            {
                base.OnPayload(payload);

                //reflect 45% of melee damage
            }

            public override void Remove()
            {
                base.Remove();
            }
        }
        [ImplementsPowerBuff(1)]
        class DurationBuff : PowerBuff
        {
            public override void Init()
            {
                Timeout = WaitSeconds(ScriptFormula(10));
            }

            public override bool Apply()
            {
                if (!base.Apply())
                    return false;
                //TODO: is this the intended way to increase critical hit chance?
                User.Attributes[GameAttribute.Crit_Percent_Base] += (int)ScriptFormula(9);
                User.Attributes.BroadcastChangedIfRevealed();

                return true;
            }

            public override void Remove()
            {
                base.Remove();
                User.Attributes[GameAttribute.Crit_Percent_Base] -= (int)ScriptFormula(9);
                User.Attributes.BroadcastChangedIfRevealed();
            }
        }
    }
    #endregion

    //TODO: Rune_E -> this is going to be hard..
    //Something wrong with using knockback.
    #region SiesmicSlam
    [ImplementsPowerSNO(Skills.Skills.Barbarian.FurySpenders.SiesmicSlam)]
    public class SiesmicSlam : Skill
    {
        public override IEnumerable<TickTimer> Main()
        {
            UsePrimaryResource(EvalTag(PowerKeys.ResourceCost));

            yield return WaitSeconds(0.5f);
            var proj1 = new Projectile(this, 164708, User.Position);
            proj1.Launch(TargetPosition, 1f);
            foreach (Actor target in GetEnemiesInArcDirection(User.Position, TargetPosition, 45f, ScriptFormula(14)).Actors)
            {
                WeaponDamage(target, ScriptFormula(8), DamageType.Physical);
                Knockback(target, ScriptFormula(0), ScriptFormula(1));

                if (Rune_C > 0)
                {
                    if (Rand.NextDouble() < ScriptFormula(6))
                    {
                        AddBuff(target, new DebuffStunned(WaitSeconds(ScriptFormula(7))));
                    }
                }
            }

            yield return WaitSeconds(1f);

            if (Rune_B > 0)
            {
                var aShockproj = new Projectile(this, 164788, User.Position);
                aShockproj.Launch(TargetPosition, 1f);

                foreach (Actor target in GetEnemiesInArcDirection(User.Position, TargetPosition, 45f, ScriptFormula(14)).Actors)
                {
                    WeaponDamage(target, ScriptFormula(3), DamageType.Physical);
                }
            }

            yield break;
        }
    }
    #endregion

    //SuperEarthquake.ani.
    //this is a dumpster fire.. (terrible, rewrite it)
    #region Earthquake
    [ImplementsPowerSNO(Skills.Skills.Barbarian.Situational.Earthquake)]
    public class Earthquake : Skill
    {
        public override IEnumerable<TickTimer> Main()
        {
            StartCooldown(EvalTag(PowerKeys.CooldownTime));
            UsePrimaryResource(EvalTag(PowerKeys.ResourceCost));

            User.PlayEffectGroup(55689);
            WeaponDamage(GetEnemiesInRadius(User.Position, ScriptFormula(0)), ScriptFormula(19), DamageType.Physical);
            var Quake = SpawnEffect(168440, User.Position, 0, WaitSeconds(ScriptFormula(1)));
            Quake.UpdateDelay = 0.5f;
            Quake.OnUpdate = () =>
            {
                AttackPayload attack = new AttackPayload(this);
                attack.Targets = GetEnemiesInRadius(User.Position, ScriptFormula(0));
                attack.AddWeaponDamage(ScriptFormula(17), DamageType.Physical);
                attack.Apply();
            };
            //Secondary Tremor stuff..
            yield break;
        }
    }
    #endregion

    //TODO: Rune_C and _E
    #region Sprint
    [ImplementsPowerSNO(Skills.Skills.Barbarian.FurySpenders.Sprint)]
    public class Sprint : Skill
    {
        public override IEnumerable<TickTimer> Main()
        {
            StartCooldown(EvalTag(PowerKeys.CooldownTime));
            UsePrimaryResource(EvalTag(PowerKeys.ResourceCost));

            AddBuff(User, new MovementBuff());

            if (Rune_D > 0)
            {
                foreach (Actor ally in GetAlliesInRadius(User.Position, ScriptFormula(4)).Actors)
                    AddBuff(ally, new MovementAlliesBuff());
            }
            yield break;
        }
        [ImplementsPowerBuff(0)]
        class MovementBuff : PowerBuff
        {
            public override void Init()
            {
                Timeout = WaitSeconds(ScriptFormula(0));
            }

            public override bool Apply()
            {
                if (!base.Apply())
                    return false;
                new DodgeBuff();
                //nothing seems to be working here for attributes
                User.Attributes[GameAttribute.Movement_Scalar_Uncapped_Bonus] += ScriptFormula(1);
                User.Attributes[GameAttribute.Movement_Bonus_Total] += ScriptFormula(1);
                User.Attributes.BroadcastChangedIfRevealed();
                return true;
            }

            //Rune_C : Update -> tornadoes rage in the wake of the spring, each on inflicting 40% dmg.

            //Rune_E : OnPayload? knockback and damage.

            public override void Remove()
            {
                base.Remove();
                User.Attributes[GameAttribute.Movement_Bonus_Run_Speed] -= ScriptFormula(1);
                User.Attributes.BroadcastChangedIfRevealed();
            }
        }
        [ImplementsPowerBuff(3)]
        class MovementAlliesBuff : PowerBuff
        {
            public override void Init()
            {
                Timeout = WaitSeconds(ScriptFormula(0));
            }

            public override bool Apply()
            {
                if (!base.Apply())
                    return false;
                Target.Attributes[GameAttribute.Movement_Bonus_Run_Speed] += ScriptFormula(1);
                Target.Attributes.BroadcastChangedIfRevealed();
                return true;
            }

            public override void Remove()
            {
                base.Remove();
                Target.Attributes[GameAttribute.Movement_Bonus_Run_Speed] -= ScriptFormula(1);
                Target.Attributes.BroadcastChangedIfRevealed();
            }
        }
        [ImplementsPowerBuff(4)]
        class DodgeBuff : PowerBuff
        {
            public override void Init()
            {
                Timeout = WaitSeconds(ScriptFormula(0));
            }

            public override bool Apply()
            {
                if (!base.Apply())
                    return false;
                User.Attributes[GameAttribute.Dodge_Chance_Bonus] += ScriptFormula(2);
                User.Attributes.BroadcastChangedIfRevealed();
                return true;
            }

            public override void Remove()
            {
                base.Remove();
                User.Attributes[GameAttribute.Dodge_Chance_Bonus] -= ScriptFormula(2);
                User.Attributes.BroadcastChangedIfRevealed();
            }
        }
    }
    #endregion

    //TODO: Rune_C/D
    #region WrathOfTheBerserker
    [ImplementsPowerSNO(Skills.Skills.Barbarian.Situational.WrathOfTheBerserker)]
    public class WrathOfTheBerserker : Skill
    {
        public override IEnumerable<TickTimer> Main()
        {
            StartCooldown(EvalTag(PowerKeys.CooldownTime));
            UsePrimaryResource(EvalTag(PowerKeys.ResourceCost));

            AddBuff(User, new BerserkerBuff());
            if (Rune_B > 0)
            {
                AttackPayload attack = new AttackPayload(this);
                attack.Targets = GetEnemiesInRadius(User.Position, ScriptFormula(20));
                attack.AddWeaponDamage(ScriptFormula(17), DamageType.Physical);
                attack.OnHit = HitPayload =>
                {
                    Knockback(User.Position, HitPayload.Target, ScriptFormula(18), ScriptFormula(19));
                };
            }
            if (Rune_E > 0)
            {
                AttackPayload attack = new AttackPayload(this);
                attack.Targets = GetEnemiesInRadius(User.Position, ScriptFormula(15));
                attack.OnDeath = HitPayload =>
                {
                    User.PlayEffectGroup(210319);
                    WeaponDamage(HitPayload.Target, ScriptFormula(11), DamageType.Physical);
                };
            }

            yield break;
        }
        [ImplementsPowerBuff(0)]
        class BerserkerBuff : PowerBuff
        {
            public override void Init()
            {
                Timeout = WaitSeconds(ScriptFormula(6));
            }

            public override bool Apply()
            {
                if (!base.Apply())
                    return false;
                if (Rune_A > 0)
                {
                    User.Attributes[GameAttribute.Attack_Bonus_Percent] += ScriptFormula(8);
                }
                User.Attributes[GameAttribute.Crit_Damage_Percent] += (int)ScriptFormula(0);
                User.Attributes[GameAttribute.Attacks_Per_Second_Bonus] += ScriptFormula(2);
                User.Attributes[GameAttribute.Dodge_Chance_Bonus] += ScriptFormula(3);
                User.Attributes[GameAttribute.Movement_Bonus_Run_Speed] += ScriptFormula(1);
                User.Attributes.BroadcastChangedIfRevealed();
                return true;
            }

            //Rune_D -> OnPayload? every _ fury gained adds 1 second to duration of effect.

            public override void Remove()
            {
                base.Remove();
                if (Rune_A > 0)
                {
                    User.Attributes[GameAttribute.Attack_Bonus_Percent] += ScriptFormula(8);
                }
                User.Attributes[GameAttribute.Crit_Damage_Percent] -= (int)ScriptFormula(0);
                User.Attributes[GameAttribute.Attacks_Per_Second_Bonus] -= ScriptFormula(2);
                User.Attributes[GameAttribute.Dodge_Chance_Bonus] -= ScriptFormula(3);
                User.Attributes[GameAttribute.Movement_Bonus_Run_Speed] -= ScriptFormula(1);
                User.Attributes.BroadcastChangedIfRevealed();
            }
        }
    }
    #endregion

    //Incomplete
    #region CallOfTheAncients
    [ImplementsPowerSNO(Skills.Skills.Barbarian.Situational.CallOfTheAncients)]
    public class CallOfTheAncients : Skill
    {
        public override IEnumerable<TickTimer> Main()
        {
            StartCooldown(60f);

            List<Actor> ancients = new List<Actor>();
            for (int i = 0; i < 3; i++)
            {
                if (i == 0)
                {
                    var ancient = new AncientKorlic(this.World, this, i);
                    ancient.Brain.DeActivate();
                    ancient.Position = RandomDirection(User.Position, 3f, 8f); //Kind of hacky until we get proper collisiondetection
                    ancient.Attributes[GameAttribute.Untargetable] = true;
                    ancient.EnterWorld(ancient.Position);
                    ancient.PlayActionAnimation(97105);
                    ancients.Add(ancient);
                    yield return WaitSeconds(0.2f);
                }
                if (i == 1)
                {
                    var ancient = new AncientTalic(this.World, this, i);
                    ancient.Brain.DeActivate();
                    ancient.Position = RandomDirection(User.Position, 3f, 8f); //Kind of hacky until we get proper collisiondetection
                    ancient.Attributes[GameAttribute.Untargetable] = true;
                    ancient.EnterWorld(ancient.Position);
                    ancient.PlayActionAnimation(97109);
                    ancients.Add(ancient);
                    yield return WaitSeconds(0.2f);
                }
                if (i == 2)
                {
                    var ancient = new AncientMawdawc(this.World, this, i);
                    ancient.Brain.DeActivate();
                    ancient.Position = RandomDirection(User.Position, 3f, 8f); //Kind of hacky until we get proper collisiondetection
                    ancient.Attributes[GameAttribute.Untargetable] = true;
                    ancient.EnterWorld(ancient.Position);
                    ancient.PlayActionAnimation(97107);
                    ancients.Add(ancient);
                    yield return WaitSeconds(0.2f);
                }
            }
            yield return WaitSeconds(0.8f);

            foreach (Actor ancient in ancients)
            {
                (ancient as Minion).Brain.Activate();
                ancient.Attributes[GameAttribute.Untargetable] = false;
                ancient.Attributes.BroadcastChangedIfRevealed();
            }
            yield break;
        }
    }
    #endregion
    //12 Passive Skills
}
