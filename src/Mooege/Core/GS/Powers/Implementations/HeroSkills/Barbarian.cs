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

    //TODO: A,C
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

    //TODO: D,E
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

    //TODO: A,B,C
    #region BattleRage
    [ImplementsPowerSNO(Skills.Skills.Barbarian.FurySpenders.BattleRage)]
    public class BattleRage : Skill
    {
        public override IEnumerable<TickTimer> Main()
        {
            //UsePrimaryResource(5f);

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

                //Crit Chance Bonus

                return true;
            }

            public override bool Update()
            {
                if (base.Update())
                    return true;

                return false;
            }

            public override void OnPayload(Payload payload)
            {
                if (User != null && payload.Target != null
                    && payload.Context.Target != null
                    && payload.Context.User != null)
                {
                    if (payload is AttackPayload && !payload.Context.Target.Equals(User)
                        && payload.Context.User.Equals(User) && (payload.Context.PowerSNO.CompareTo(0x00007780) == 0))
                    {
                        AttackPayload lastAttack = (AttackPayload)payload;

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
            }
            public override void Remove()
            {
                base.Remove();
            }
        }
    }
    #endregion

    //Check through Skill..
    #region Cleave
    [ImplementsPowerSNO(Skills.Skills.Barbarian.FuryGenerators.Cleave)]
    public class Cleave : Skill
    {
        public override IEnumerable<TickTimer> Main()
        {
            //TODO: fix this... it plays both animations at once, play 159481 first, then 159449.
            //Switch between these two effects.
            int AnimSwitch = 0; 
            if (AnimSwitch == 1)
            {
                User.PlayEffectGroup(159449);
                //User.PlayEffectGroup(RuneSelect(18671, 159452, 159457, 159458, 159459, 159460));
                AnimSwitch = 0;
            }
            else
            {
                User.PlayEffectGroup(159481);
                //User.PlayEffectGroup(RuneSelect(18672, 159480, 159479, 159478, 159477, 159476));
                AnimSwitch = 1;
            }
            

            //Primary Target
            WeaponDamage(Target, ScriptFormula(16), DamageType.Physical);
            //Secondary Targets
            AttackPayload attack = new AttackPayload(this);
            //SF(27) is 120 degrees, SF(26) is range.
            attack.Targets = GetEnemiesInArcDirection(User.Position, TargetPosition, ScriptFormula(27), ScriptFormula(26));
            attack.AddWeaponDamage(ScriptFormula(18), DamageType.Physical);
            attack.OnHit = hitPayload =>
            {
                if (Rune_B > 0)
                {
                    //why is there knockback magnitude delta, drawn in radius, knockback magnitude, inner exclude radius
                    //gathering storm tooltip says reduces speed. but gathering storm formulas show that it brings targets closer to you.

                    AttackPayload ClosePull = new AttackPayload(this);
                    ClosePull.Targets = GetEnemiesInRadius(User.Position, ScriptFormula(14));
                    attack.OnHit = ClosePuller =>
                                            {
                                                Knockback(ClosePuller.Target, ScriptFormula(13));
                                            };
                    ClosePull.Apply();
                    AttackPayload FarPull = new AttackPayload(this);
                    FarPull.Targets = GetEnemiesInRadius(User.Position, ScriptFormula(21));
                    attack.OnHit = FarPuller =>
                                            {
                                                Knockback(FarPuller.Target, ScriptFormula(20));
                                            };
                    FarPull.Apply();
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

                    GeneratePrimaryResource(ScriptFormula(22)* StackCount);
                }
                if (Rune_E > 0)
                {
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

            yield break;
        }
    }
    #endregion

    //TODO: Rune_A -> Relfects 65% damage back to enemy
    //TODO: Rune_E -> gain 40% of all damage dealt as life.
    #region IgnorePain
    [ImplementsPowerSNO(Skills.Skills.Barbarian.Situational.IgnorePain)]
    public class IgnorePain : Skill
    {
        public override IEnumerable<TickTimer> Main()
        {
            //Rune_D and C and B
            StartCooldown(WaitSeconds(ScriptFormula(22)));
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
                User.Attributes[GameAttribute.Damage_Percent_Reduction_From_Melee] += ScriptFormula(10);
                User.Attributes[GameAttribute.Damage_Percent_Reduction_From_Ranged] += ScriptFormula(10);
                Target.Attributes.BroadcastChangedIfRevealed();
                if (Rune_D > 0)
                {
                    AttackPayload attack = new AttackPayload(this);
                    //SF(8) -> wowbox says otherwise for the radius.
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

            public override void Remove()
            {
                base.Remove();
                User.Attributes[GameAttribute.Damage_Percent_Reduction_From_Melee] -= ScriptFormula(10);
                User.Attributes[GameAttribute.Damage_Percent_Reduction_From_Ranged] -= ScriptFormula(10);
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

                Target.Attributes[GameAttribute.Damage_Percent_Reduction_From_Melee] -= ScriptFormula(10);
                Target.Attributes[GameAttribute.Damage_Percent_Reduction_From_Ranged] -= ScriptFormula(10);
                Target.Attributes.BroadcastChangedIfRevealed();


                return true;
            }

            public override void Remove()
            {
                base.Remove();

                Target.Attributes[GameAttribute.Damage_Percent_Reduction_From_Melee] -= ScriptFormula(10);
                Target.Attributes[GameAttribute.Damage_Percent_Reduction_From_Ranged] -= ScriptFormula(10);
                Target.Attributes.BroadcastChangedIfRevealed();
            }
        }
    }
#endregion

    //TODO: Rune_E -> Figure Out TeamID
    #region WeaponThrow
    [ImplementsPowerSNO(Skills.Skills.Barbarian.FurySpenders.WeaponThrow)]
    public class WeaponThrow : Skill
    {
        public override IEnumerable<TickTimer> Main()
        {
            if (Rune_B > 0)
            {
                IList<Actor> targets = new List<Actor>() { Target };
                Actor ropeSource = User;
                Actor curTarget = Target;
                float damage = ScriptFormula(15);
                while (targets.Count < ScriptFormula(5) + 1)
                {
                    if (ropeSource.World == null)
                        ropeSource = SpawnProxy(ropeSource.Position);

                    if (curTarget.World != null)
                    {
                        ropeSource.AddRopeEffect(166450, curTarget);
                        ropeSource = curTarget;

                        WeaponDamage(curTarget, damage, DamageType.Physical);
                    }
                    else
                    {
                        break;
                    }

                    curTarget = GetEnemiesInRadius(curTarget.Position, ScriptFormula(12), (int)ScriptFormula(5)).Actors.FirstOrDefault(t => !targets.Contains(t));
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

    #region GroundStomp
    [ImplementsPowerSNO(Skills.Skills.Barbarian.FuryGenerators.GroundStomp)]
    public class GroundStomp : Skill
    {
        public override IEnumerable<TickTimer> Main()
        {
            GeneratePrimaryResource(15f + ScriptFormula(19));
            User.PlayEffectGroup(RuneSelect(18685, 99685, 159415, 159416, 159397, 18685));
            //Rune_E -> when stun wears off, use slow.efg

            AttackPayload attack = new AttackPayload(this);
            attack.Targets = GetEnemiesInRadius(User.Position, ScriptFormula(0));
            attack.AddWeaponDamage(ScriptFormula(6), DamageType.Physical);
            attack.OnHit = hitPayload =>
            {
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

    //Figure out Bleed Damaging when you don't have a .acr UpdateDelay.. (inside buff)
    //TODO: Rune_D SF(8)
    //max fury chance to stack SF(9) and striking max stacks SF(10)
    #region Rend
    [ImplementsPowerSNO(Skills.Skills.Barbarian.FurySpenders.Rend)]
    public class Rend : Skill
    {
        public override IEnumerable<TickTimer> Main()
        {
            User.PlayEffectGroup(RuneSelect(70614, 161594, 100842, 161655, 161652, 161605));

            AttackPayload attack = new AttackPayload(this);
            attack.Targets = GetEnemiesInRadius(User.Position, ScriptFormula(4));
            attack.OnHit = hitPayload =>
            {
                AddBuff(hitPayload.Target, new RendDebuff());
            };
            attack.OnDeath = DeathPayload =>
                {
                    if (AddBuff(Target, new RendDebuff()))
                    {
                        AttackPayload BleedAgain = new AttackPayload(this);
                        BleedAgain.Targets = GetEnemiesInRadius(Target.Position, ScriptFormula(19));
                        BleedAgain.OnHit = hitPayload =>
                        {
                            AddBuff(hitPayload.Target, new RendDebuff());
                        };
                        BleedAgain.Apply();
                    }
                };
            attack.Apply();
            yield break;
        }
        [ImplementsPowerBuff(0)]
        public class RendDebuff : PowerBuff
        {
            public override void Init()
            {
                Timeout = WaitSeconds(ScriptFormula(5));
            }

            public override bool Apply()
            {
                if (!base.Apply())
                    return false;

                //Add this to Remove() of RendSeconds to make it 
                //do the bleed damage once every second for SF(5) seconds.

                for (int i = 0; i < ScriptFormula(5); ++i)
                {
                    new RendSeconds();
                }

                return true;
            }

            public override void Remove()
            {
                base.Remove();
            }
        }
        [ImplementsPowerBuff(0)]
        public class RendSeconds : PowerBuff
        {
            public override void Init()
            {
                Timeout = WaitSeconds(1f);
            }

            public override bool Apply()
            {
                if (!base.Apply())
                    return false;
                float Damage;
                if (Rune_E > 0)
                {
                    Damage = ScriptFormula(7);
                }
                else
                    Damage = ScriptFormula(20);

                WeaponDamage(Target, Damage, DamageType.Physical);
                return true;
            }

            public override void Remove()
            {
                base.Remove();
            }
        }
    }
#endregion

    //bash, leap attack, ancient spear, whirlwind, threateningshout, hammeroftheancients, battle rage, cleave, ignore pain, weapon throw, ground stomp, rend
}
