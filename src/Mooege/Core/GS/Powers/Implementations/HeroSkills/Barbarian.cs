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
using Mooege.Core.GS.Common.Types.Math;
using Mooege.Net.GS.Message.Definitions.ACD;
using Mooege.Core.GS.Common.Types.Misc;
using Mooege.Core.GS.Ticker;
using Mooege.Net.GS.Message;
using Mooege.Core.GS.Common.Types.TagMap;
using Mooege.Core.GS.Powers.Payloads;
using Mooege.Core.GS.Actors.Movement;
using Mooege.Core.GS.Players;

namespace Mooege.Core.GS.Powers.Implementations
{
    #region Bash
    [ImplementsPowerSNO(Skills.Skills.Barbarian.FuryGenerators.Bash)]
    public class BarbarianBash : Skill
    {
        public override IEnumerable<TickTimer> Main()
        {
            AttackPayload attack = new AttackPayload(this);
            attack.Targets = GetBestMeleeEnemy();
            attack.AddWeaponDamage(1.45f, DamageType.Physical);
            attack.OnHit = hitPayload =>
            {
                GeneratePrimaryResource(6f);

                if (Rand.NextDouble() < 0.20)
                    Knockback(hitPayload.Target, ScriptFormula(5), ScriptFormula(6), ScriptFormula(7));
            };

            attack.Apply();

            yield break;
        }

        public override float GetContactDelay()
        {
            // seems to need this custom speed for all attacks
            return ScriptFormula(13);
        }
    }
#endregion

    #region LeapAttack
    [ImplementsPowerSNO(Skills.Skills.Barbarian.FuryGenerators.LeapAttack)]
    public class BarbarianLeap : Skill
    {
        public override IEnumerable<TickTimer> Main()
        {
            //StartCooldown(WaitSeconds(10f));

            ActorMover mover = new ActorMover(User);
            mover.MoveArc(TargetPosition, 10, -0.1f, new ACDTranslateArcMessage
            {
                //Field3 = 303110, // used for male barb leap, not needed?
                FlyingAnimationTagID = AnimationSetKeys.Attack2.ID,
                LandingAnimationTagID = -1,
                Field7 = PowerSNO
            });

            // wait for landing
            while (!mover.Update())
                yield return WaitTicks(1);

            // extra wait for leap to finish
            yield return WaitTicks(1);

            // ground smash effect
            User.PlayEffectGroup(162811);

            bool hitAnything = false;
            AttackPayload attack = new AttackPayload(this);
            attack.Targets = GetEnemiesInRadius(TargetPosition, 8f);
            attack.AddWeaponDamage(0.70f, DamageType.Physical);
            attack.OnHit = hitPayload => { hitAnything = true; };
            attack.Apply();

            if (hitAnything)
                GeneratePrimaryResource(15f);

            yield break;
        }
    }
#endregion

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
                Timeout = WaitSeconds(0.20f);
            }

            public override bool Update()
            {
                if (base.Update())
                    return true;

                if (_damageTimer == null || _damageTimer.TimedOut)
                {
                    _damageTimer = WaitSeconds(ScriptFormula(0));
                    //UsePrimaryResource(EvalTag(PowerKeys.ResourceCost));

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

    #region AncientSpear
    [ImplementsPowerSNO(Skills.Skills.Barbarian.FuryGenerators.AncientSpear)]
    public class BarbarianAncientSpear : Skill
    {
        public override IEnumerable<TickTimer> Main()
        {
            //StartCooldown(WaitSeconds(10f));

            var projectile = new Projectile(this, 74636, User.Position);
            projectile.Timeout = WaitSeconds(0.5f);
            projectile.OnCollision = (hit) =>
            {
                GeneratePrimaryResource(15f);
                
                _setupReturnProjectile(hit.Position);

                AttackPayload attack = new AttackPayload(this);
                attack.SetSingleTarget(hit);
                attack.AddWeaponDamage(1.00f, DamageType.Physical);
                attack.OnHit = (hitPayload) =>
                {
                    // GET OVER HERE
                    Knockback(hitPayload.Target, -25f, 1f, -0.03f);
                };
                attack.Apply();

                projectile.Destroy();
            };
            projectile.OnTimeout = () =>
            {
                _setupReturnProjectile(projectile.Position);
            };

            projectile.Launch(TargetPosition, 1.9f);
            User.AddRopeEffect(79402, projectile);

            yield break;
        }

        private void _setupReturnProjectile(Vector3D spawnPosition)
        {
            Vector3D inFrontOfUser = PowerMath.TranslateDirection2D(User.Position, spawnPosition, User.Position, 5f);

            var return_proj = new Projectile(this, 79400, new Vector3D(spawnPosition.X, spawnPosition.Y, User.Position.Z));
            return_proj.DestroyOnArrival = true;
            return_proj.LaunchArc(inFrontOfUser, 1f, -0.03f);
            User.AddRopeEffect(79402, return_proj);
        }
    }
#endregion

    #region ThreateningShout
    [ImplementsPowerSNO(Skills.Skills.Barbarian.FurySpenders.ThreateningShout)]
    public class ThreateningShout : Skill
    {
        public override IEnumerable<TickTimer> Main()
        {
            UsePrimaryResource(20f);
            User.PlayEffectGroup(RuneSelect(18705, 99810, 216339, 99798, 201534, 99821));
            //User.PlayEffectGroup(202891); //Yell Sound
            AttackPayload attack = new AttackPayload(this);
            attack.Targets = GetEnemiesInRadius(User.Position, ScriptFormula(9));
            attack.OnHit = (hit) =>
            {
                AddBuff(hit.Target, new ShoutReduceDamage(WaitSeconds(ScriptFormula(2))));
                if (Rune_A > 0)
                {
                    //Script(8) -> taunt duration
                    //taunted to attack you... wut? guess more for multiple player...
                }
                if (Rune_B > 0)
                {
                    AddBuff(hit.Target, new MovementDeBuff(ScriptFormula(14), WaitSeconds(ScriptFormula(2))));
                }

                if (Rune_C > 0)
                {
                    if (Rand.NextDouble() < ScriptFormula(7))
                    {
                        attack.OnDeath = (dead) =>
                            {
                                //dead.Target
                                //Drop another random loot :)
                            };
                    }
                }
                if (Rune_D > 0)
                {
                    AddBuff(hit.Target, new AttackSpeedDeBuff(ScriptFormula(4), WaitSeconds(ScriptFormula(17))));
                }
                if (Rune_E > 0)
                {
                    //Script(10) -> Fear Death Effect Duration? what is this for..
                    if (Rand.NextDouble() < ScriptFormula(3))
                    {
                        AddBuff(hit.Target, new DebuffFeared(WaitSeconds(Rand.Next((int)ScriptFormula(5), (int)ScriptFormula(5) + (int)ScriptFormula(6)))));
                    }
                }

            };
            attack.Apply();

            yield break;
        }

    }
#endregion

    #region HammerOfTheAncients
    [ImplementsPowerSNO(Skills.Skills.Barbarian.FurySpenders.HammerOfTheAncients)]
    public class HammerOfTheAncients : Skill
    {
        public override IEnumerable<TickTimer> Main()
        {
            //this does NoRune and A's area of effect
            float castAngle = MovementHelpers.GetFacingAngle(User.Position, TargetPosition);
            SpawnEffect(RuneSelect(220632, 220559, 220562, 220565, 220569, 162839), User.Position, castAngle);

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
                        AddBuff(hitPayload.Target, new MovementDeBuff(ScriptFormula(8), WaitSeconds(ScriptFormula(10))));
                    }
                };
                attack.OnDeath = DeathPayload =>
                    {
                        if (Rune_E > 0)
                        {
                            //if (DeathPayload.Target)?
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
                    var QuakeHammer = SpawnEffect(159030, User.Position, 0 , WaitSeconds(ScriptFormula(10)));
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

    //bash, leap attack, ancient spear, whirlwind, threateningshout, hammeroftheancients
}
