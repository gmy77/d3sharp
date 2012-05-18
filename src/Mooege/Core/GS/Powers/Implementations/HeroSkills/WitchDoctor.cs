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
using Mooege.Core.GS.Actors.Implementations.Minions;
using Mooege.Core.GS.Actors.Movement;
using Mooege.Core.GS.Common.Types.Math;
using Mooege.Core.GS.Common.Types.TagMap;
using Mooege.Core.GS.Powers.Payloads;
using Mooege.Net.GS.Message;
using Mooege.Core.GS.Ticker;

namespace Mooege.Core.GS.Powers.Implementations
{
    //Complete skill/Runes by MDZ, TODO: fix positioning of hit actors.
    #region PoisonDart
    [ImplementsPowerSNO(Skills.Skills.WitchDoctor.PhysicalRealm.PoisonDart)]
    public class WitchDoctorPoisonDart : Skill
    {
        public override IEnumerable<TickTimer> Main()
        {
            StartCooldown(EvalTag(PowerKeys.CooldownTime));
            UsePrimaryResource(EvalTag(PowerKeys.ResourceCost));
            int numProjectiles = Rune_B > 0 ? (int)ScriptFormula(4) : 1;
            for (int n = 0; n < numProjectiles; ++n)
            {
                if (Rune_B > 0)
                    yield return WaitSeconds(ScriptFormula(17));

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
            }
        }
    }
    #endregion

    //Hacky BigToad by MDZ
    //testing plague of toads attempt, does not work except for one jump, with no animation.
    #region PlagueOfToads
    [ImplementsPowerSNO(Skills.Skills.WitchDoctor.PhysicalRealm.PlagueOfToads)]
    public class WitchDoctorPlagueOfToads : PowerScript
    {
        public override IEnumerable<TickTimer> Run()
        {
            StartCooldown(EvalTag(PowerKeys.CooldownTime));
            UsePrimaryResource(EvalTag(PowerKeys.ResourceCost));

            if (Rune_C > 0)
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
            TickTimer frog = WaitSeconds(ScriptFormula(5));
            Vector3D inFrontOfminiToads = PowerMath.TranslateDirection2D(User.Position, TargetPosition, User.Position, 7f);
            var projectile = new Projectile(this, 105792, User.Position);
            projectile.LaunchArc(inFrontOfminiToads, 3f, -0.03f);
            projectile.OnCollision = (hit) =>
            {
                //destroying it while in Launcharc causes an exception.
                //projectile.Destroy();
            };
            projectile.OnArrival = () => { };
            yield return WaitSeconds(1.2f);
            projectile.LaunchArc(new Vector3D(RandomDirection(projectile.Position, 4f, 7f)), 3f, -0.03f);
            projectile.OnArrival = () => { };
            yield return WaitSeconds(1.2f);

            /*
             * for regular toads, there is a max of 3 frogs per cast, they are classifyed as projectile with a lifetime of 3.
             */
        }

        private void _SetHiddenAttribute(Actor actor, bool active)
        {
            actor.Attributes[GameAttribute.Hidden] = active;
            actor.Attributes.BroadcastChangedIfRevealed();
        }
    }
    #endregion

    //Complete, needs checking
    #region GraspOfTheDead
    [ImplementsPowerSNO(Skills.Skills.WitchDoctor.Support.GraspOfTheDead)]
    public class GraspOfTheDead : Skill
    {
        public override IEnumerable<TickTimer> Main()
        {

            StartCooldown(EvalTag(PowerKeys.CooldownTime));
            UsePrimaryResource(EvalTag(PowerKeys.ResourceCost));

            if (Rune_B > 0)
            {
                for (int i = 0; i < 4; ++i)
                {
                    var Target = GetEnemiesInRadius(TargetPosition, ScriptFormula(14)).GetClosestTo(TargetPosition);
                    if (Target != null)
                    {
                        SpawnEffect(105955, Target.Position);
                        WeaponDamage(GetEnemiesInRadius(Target.Position, ScriptFormula(15)), ScriptFormula(10), DamageType.Holy);
                        yield return WaitSeconds(ScriptFormula(13));
                    }
                }
            }
            else
            {
                var Ground = SpawnEffect(RuneSelect(69308, 105953, -1, 105956, 105957, 105958), TargetPosition, 0, WaitSeconds(ScriptFormula(8)));
                Ground.UpdateDelay = 0.5f;
                Ground.OnUpdate = () =>
                {
                    foreach (Actor enemy in GetEnemiesInRadius(TargetPosition, ScriptFormula(3)).Actors)
                    {
                        AddBuff(enemy, new DebuffSlowed(ScriptFormula(19), WaitSeconds(ScriptFormula(2))));
                        AddBuff(enemy, new DamageGroundDebuff());
                    }
                };
            }
            yield break;
        }
        [ImplementsPowerBuff(0)]
        class DamageGroundDebuff : PowerBuff
        {
            const float _damageRate = 1f;
            TickTimer _damageTimer = null;

            public override void Init()
            {
                base.Init();
                Timeout = WaitSeconds(ScriptFormula(2));
            }
            /*public override void OnPayload(Payload payload)
            {
                if (payload.Target == Target && payload is DeathPayload)
                {
                }
            }*/
            public override bool Update()
            {
                if (base.Update())
                    return true;

                if (_damageTimer == null || _damageTimer.TimedOut)
                {
                    _damageTimer = WaitSeconds(_damageRate);

                    AttackPayload attack = new AttackPayload(this);
                    attack.Targets = GetEnemiesInRadius(Target.Position, 1f); //TODO: hack, it should only be applied to individual targets, not all in 1f.
                    attack.AddWeaponDamage(ScriptFormula(0), DamageType.Physical);
                    attack.Apply();
                    attack.OnDeath = (DeathPayload) =>
                    {
                        if (Rune_E > 0)
                        {
                            if (Rand.NextDouble() < ScriptFormula(21))
                            {
                                //produce a health globe
                                //Items.ItemGenerator.CreateGlobe(User, "HealthGlobe" + 25);

                            }
                        }
                    };
                }

                return false;
            }
        }
    }
    #endregion

    //TODO:checking for all Haunts in a 90f radius. needs checking
    //TODO:also needs to check if monster already has haunt, to look for a new target or overwrite the existing one.
    //TODO: Something very buggy with complexEffect... very very buggy. (ex. cast spell, click-hold mouse move around)
    #region Haunt
    [ImplementsPowerSNO(Skills.Skills.WitchDoctor.SpiritRealm.Haunt)]
    public class Haunt : Skill
    {
        public override IEnumerable<TickTimer> Main()
        {
            //Need to check for all Haunt Buffs in this radius.
            //Max simultaneous haunts = 3 ScriptFormula(8)
            //Max Haunt Check Radius(ScriptFormula(9)) -> 90f

            UsePrimaryResource(EvalTag(PowerKeys.ResourceCost));

            if (Rune_B > 0)
            {
                if (Target == null)
                {

                    var Lingerer = SpawnEffect(111345, TargetPosition, 0, WaitSeconds(ScriptFormula(4)));
                    Lingerer.OnTimeout = () =>
                    {
                        Lingerer.World.BuffManager.RemoveAllBuffs(Lingerer);
                    };
                    AddBuff(Lingerer, new HauntLinger());
                }
            }
            else
            {
                if (Target != null)
                {
                    User.AddComplexEffect(19257, Target);
                    AddBuff(Target, new Haunt1());
                }
            }
            yield break;
        }

        [ImplementsPowerBuff(0)]
        class Haunt1 : PowerBuff
        {
            const float _damageRate = 1f;
            TickTimer _damageTimer = null;

            public override void Init()
            {
                Timeout = WaitSeconds(ScriptFormula(1));
            }
            public override void OnPayload(Payload payload)
            {
                if (payload.Target == Target && payload is DeathPayload)
                {
                    //Need to check if monster already has a haunt on them, if it does -> check next monster.
                    var target = GetEnemiesInRadius(payload.Context.Target.Position, ScriptFormula(10))
                        .GetClosestTo(payload.Context.Target.Position);
                    if (target != null)
                    {
                        Target.AddComplexEffect(RuneSelect(19257, 111257, 111370, 113742, 111461, 111564), target);
                        AddBuff(target, new Haunt1());
                    }
                    else
                    {
                        if (Rune_B > 0)
                        {
                            var Lingerer = SpawnEffect(111345, Target.Position, 0, WaitSeconds(ScriptFormula(4)));
                            Lingerer.OnTimeout = () =>
                            {
                                Lingerer.World.BuffManager.RemoveAllBuffs(Lingerer);
                            };
                            AddBuff(Lingerer, new HauntLinger());
                        }
                    }
                }
            }
            public override bool Update()
            {
                if (base.Update())
                    return true;
                if (_damageTimer == null || _damageTimer.TimedOut)
                {
                    _damageTimer = WaitSeconds(_damageRate);

                    if (Rune_D > 0)
                    {
                        GeneratePrimaryResource(ScriptFormula(3));
                    }

                    AttackPayload attack = new AttackPayload(this);
                    attack.SetSingleTarget(Target);
                    attack.AddWeaponDamage((ScriptFormula(0) / ScriptFormula(1)), DamageType.Holy);
                    attack.AutomaticHitEffects = false;
                    attack.OnHit = hit =>
                    {
                        if (Rune_A > 0)
                        {
                            //45% of damage healed back to user
                            float healMe = ScriptFormula(2) * hit.TotalDamage;
                            //User.Attributes[GameAttribute.Hitpoints_Granted] = healMe;
                            //User.Attributes.BroadcastChangedIfRevealed();
                        }
                        if (Rune_C > 0)
                        {
                            //DebuffSlowed Target 
                            AddBuff(Target, new DebuffSlowed(ScriptFormula(5), WaitSeconds(ScriptFormula(1))));
                        }
                    };
                    attack.Apply();
                }
                return false;
            }
            public override void Remove()
            {
                base.Remove();
            }
        }
        //Rune_B
        [ImplementsPowerBuff(2)]
        class HauntLinger : PowerBuff
        {
            const float _damageRate = 1.25f;
            TickTimer _damageTimer = null;

            public override void Init()
            {
                Timeout = WaitSeconds(ScriptFormula(4));
            }

            //Search 
            public override bool Update()
            {
                if (base.Update())
                    return true;
                if (_damageTimer == null || _damageTimer.TimedOut)
                {
                    _damageTimer = WaitSeconds(_damageRate);
                    //When this finds a target, it needs to destroy the spawnactor[Lingerer]
                    if (GetEnemiesInRadius(Target.Position, ScriptFormula(10)).Actors.Count > 0)
                    {
                        var target = GetEnemiesInRadius(Target.Position, ScriptFormula(10)).GetClosestTo(Target.Position);
                        Target.AddComplexEffect(19257, target);
                        AddBuff(target, new Haunt1());
                    }

                }
                return false;
            }
            public override void Remove()
            {
                base.Remove();
            }
        }
    }
    #endregion

    //TODO:Rune_D
    #region ZombieCharger
    [ImplementsPowerSNO(Skills.Skills.WitchDoctor.PhysicalRealm.ZombieCharger)]
    public class WitchDoctorZombieCharger : Skill
    {
        public override IEnumerable<TickTimer> Main()
        {
            StartCooldown(EvalTag(PowerKeys.CooldownTime));
            UsePrimaryResource(EvalTag(PowerKeys.ResourceCost));

            if (Rune_A > 0)
            {
                Vector3D inFrontOfUser = PowerMath.TranslateDirection2D(User.Position, TargetPosition, User.Position, -20f);
                Vector3D[] projDestinations = PowerMath.GenerateSpreadPositions(User.Position, TargetPosition, 10f, 3);

                var BearProj1 = new Projectile(this, RuneSelect(74056, 105501, 105543, 105463, 105969, 105812), inFrontOfUser);
                BearProj1.Position.Z -= 3f;
                BearProj1.OnCollision = (hit) =>
                {
                    WeaponDamage(hit, ScriptFormula(4), DamageType.Poison);
                };
                BearProj1.Launch(projDestinations[1], ScriptFormula(19));

                yield return WaitSeconds(0.5f);
                var BearProj2 = new Projectile(this, RuneSelect(74056, 105501, 105543, 105463, 105969, 105812), inFrontOfUser);
                BearProj2.Position.Z -= 3f;
                BearProj2.OnCollision = (hit) =>
                {
                    WeaponDamage(hit, ScriptFormula(4), DamageType.Poison);
                };
                BearProj2.Launch(projDestinations[0], ScriptFormula(19));

                yield return WaitSeconds(0.5f);
                var BearProj3 = new Projectile(this, RuneSelect(74056, 105501, 105543, 105463, 105969, 105812), inFrontOfUser);
                BearProj3.Position.Z -= 3f;
                BearProj3.OnCollision = (hit) =>
                {
                    WeaponDamage(hit, ScriptFormula(4), DamageType.Poison);
                };
                BearProj3.Launch(projDestinations[2], ScriptFormula(19));
            }
            else if (Rune_B > 0)
            {
                Vector3D inFrontOfUser = PowerMath.TranslateDirection2D(User.Position, TargetPosition, User.Position, 3f);
                Vector3D[] projDestinations = PowerMath.GenerateSpreadPositions(User.Position, TargetPosition, ScriptFormula(10), (int)ScriptFormula(3));

                for (int i = 1; i < projDestinations.Length; i++)
                {
                    var multiproj = new Projectile(this, RuneSelect(74056, 105501, 105543, 105463, 105969, 105812), inFrontOfUser);
                    multiproj.Position.Z -= 3f;
                    multiproj.OnCollision = (hit) =>
                    {
                        WeaponDamage(hit, ScriptFormula(4), DamageType.Poison);
                    };
                    multiproj.Launch(projDestinations[i], ScriptFormula(1));
                }
            }
            else if (Rune_D > 0)
            {
                Vector3D inFrontOfUser = PowerMath.TranslateDirection2D(User.Position, TargetPosition, User.Position, 3f);
                var proj = new Projectile(this, RuneSelect(74056, 105501, 105543, 105463, 105969, 105812), inFrontOfUser);
                proj.Position.Z -= 3f;
                proj.Launch(TargetPosition, ScriptFormula(1));
                proj.OnCollision = (hit) =>
                {
                    AttackPayload attack = new AttackPayload(this);
                    attack.SetSingleTarget(hit);
                    attack.AddWeaponDamage(ScriptFormula(4), DamageType.Poison);
                    attack.Apply();
                    attack.OnDeath = DeathPayload =>
                    {
                        Vector3D inFrontOfUser2 = PowerMath.TranslateDirection2D(User.Position, TargetPosition, User.Position, 22f);
                        var proj2 = new Projectile(this, RuneSelect(74056, 105501, 105543, 105463, 105969, 105812), inFrontOfUser2);
                        proj.Launch(TargetPosition, ScriptFormula(1));
                        //TODO:need this stuff.
                        //zombie new distance (SF(13))
                        //zombie speed (SF(14))
                        //New zombie search range (SF(15))
                        //new zombie chance (SF18)
                        //max new zombie per projectile (SF24)
                        //damage scalar -> SF31
                        //damage reduction per zombie -> SF30
                    };
                };
            }
            else if (Rune_E > 0)
            {
                Vector3D inFrontOfUser = PowerMath.TranslateDirection2D(User.Position, TargetPosition, User.Position, 3f);
                var proj = new Projectile(this, RuneSelect(74056, 105501, 105543, 105463, 105969, 105812), inFrontOfUser);
                proj.Position.Z -= 3f;
                proj.OnCollision = (hit) =>
                {
                    WeaponDamage(GetEnemiesInRadius(hit.Position, ScriptFormula(11)), ScriptFormula(4), DamageType.Fire);
                };
                proj.Launch(TargetPosition, ScriptFormula(7));
            }
            else
            {
                Vector3D inFrontOfUser = PowerMath.TranslateDirection2D(User.Position, TargetPosition, User.Position, 3f);
                var proj = new Projectile(this, RuneSelect(74056, 105501, 105543, 105463, 105969, 105812), inFrontOfUser);
                proj.Position.Z -= 3f;
                proj.Launch(TargetPosition, ScriptFormula(1));
                if (Rune_C > 0)
                {
                    var Puddle = SpawnEffect(105502, proj.Position, 0, WaitSeconds(ScriptFormula(17)));
                    Puddle.UpdateDelay = 0.5f; //somehow play this more often?
                    Puddle.OnUpdate = () =>
                    {
                        AttackPayload attack = new AttackPayload(this);
                        attack.Targets = GetEnemiesInRadius(proj.Position, ScriptFormula(8));
                        attack.AddWeaponDamage(ScriptFormula(6), DamageType.Poison);
                        attack.Apply();
                    };
                }
                proj.OnCollision = (hit) =>
                {
                    WeaponDamage(hit, ScriptFormula(4), DamageType.Poison);
                };

            }

            yield break;
        }
    }
    #endregion

    //seems complete
    #region Horrify
    [ImplementsPowerSNO(Skills.Skills.WitchDoctor.SpiritRealm.Horrify)]
    public class Horrify : Skill
    {
        public override IEnumerable<TickTimer> Main()
        {
            StartCooldown(ScriptFormula(14));
            UsePrimaryResource(EvalTag(PowerKeys.ResourceCost));

            AddBuff(User, new CastEffect());
            if (Rune_A > 0)
            {
                AddBuff(User, new CrimsonBuff());
            }
            if (Rune_E > 0)
            {
                AddBuff(User, new SprintBuff());
            }
            foreach (Actor enemy in GetEnemiesInRadius(User.Position, ScriptFormula(2)).Actors)
            {
                AddBuff(enemy, new DebuffFeared(WaitSeconds(ScriptFormula(3))));
                if (Rune_D > 0)
                {
                    GeneratePrimaryResource(ScriptFormula(8));
                }
            }
            yield break;
        }
        [ImplementsPowerBuff(0)]
        class CastEffect : PowerBuff
        {
            //switch.efg
            public override void Init()
            {
                Timeout = WaitSeconds(1f);
            }
        }
        [ImplementsPowerBuff(1)]
        class SprintBuff : PowerBuff
        {
            //spring.etf alabaster
            public override void Init()
            {
                Timeout = WaitSeconds(ScriptFormula(10));
            }
            public override bool Apply()
            {
                if (!base.Apply())
                    return false;
                Target.Attributes[GameAttribute.Movement_Bonus_Run_Speed] += ScriptFormula(9);
                Target.Attributes.BroadcastChangedIfRevealed();

                return true;
            }
            public override void Remove()
            {
                base.Remove();
                Target.Attributes[GameAttribute.Movement_Bonus_Run_Speed] -= ScriptFormula(9);
                Target.Attributes.BroadcastChangedIfRevealed();
            }
        }
        [ImplementsPowerBuff(2)]
        class CrimsonBuff : PowerBuff
        {
            //crimson buff
            public override void Init()
            {
                Timeout = WaitSeconds(ScriptFormula(12));
            }
            public override bool Apply()
            {
                if (!base.Apply())
                    return false;
                Target.Attributes[GameAttribute.Armor_Bonus_Percent] += ScriptFormula(5);
                Target.Attributes.BroadcastChangedIfRevealed();

                return true;
            }
            public override void Remove()
            {
                base.Remove();
                Target.Attributes[GameAttribute.Armor_Bonus_Percent] -= ScriptFormula(5);
                Target.Attributes.BroadcastChangedIfRevealed();
            }
        }
    }
    #endregion

    //TODO: fix resource cost and Rune_D healing
    #region Firebats
    [ImplementsPowerSNO(Skills.Skills.WitchDoctor.PhysicalRealm.Firebats)]
    public class Firebats : Skill
    {
        public override IEnumerable<TickTimer> Main()
        {
            if (Rune_A > 0)
            {
                //Projectile Giant Bat Actors
                var proj = new Projectile(this, 108238, User.Position);
                proj.Position.Z += 5f;  // unknown if this is needed
                proj.OnCollision = (hit) =>
                {
                    SpawnEffect(108389, new Vector3D(hit.Position.X, hit.Position.Y, hit.Position.Z));
                    AddBuff(hit, new BatDamage());
                    proj.Destroy();
                };
                proj.Launch(TargetPosition, ScriptFormula(8));

                yield return WaitSeconds(ScriptFormula(17));
            }
            else if (Rune_B > 0)
            {
                if (GetEnemiesInArcDirection(User.Position, TargetPosition, ScriptFormula(10), ScriptFormula(4)).Actors != null)
                {
                    var proj = new Projectile(this, 106569, User.Position);
                    proj.Position.Z += 5f;
                    proj.OnCollision = (hit) =>
                    {
                        hit.PlayEffectGroup(106575);
                        AddBuff(hit, new BatDamage());
                        proj.Destroy();
                    };
                    proj.Launch(TargetPosition, ScriptFormula(8));
                }
                yield return WaitSeconds(ScriptFormula(6));
            }
            else if (Rune_E > 0)
            {
                AddBuff(User, new FirebatCast());
                foreach (Actor actor in GetEnemiesInArcDirection(User.Position, TargetPosition, ScriptFormula(3), ScriptFormula(4)).Actors)
                {
                    AddBuff(actor, new BatDamage());
                }

                yield return WaitSeconds(ScriptFormula(20));
            }
            else
            {
                AddBuff(User, new FirebatCast());
                foreach (Actor actor in GetEnemiesInArcDirection(User.Position, TargetPosition, ScriptFormula(3), ScriptFormula(4)).Actors)
                {
                    AddBuff(actor, new BatDamage());
                }
                yield return WaitSeconds(ScriptFormula(7));
            }
            yield break;
        }
        [ImplementsPowerBuff(0)]
        class FirebatCast : PowerBuff
        {
            public override void Init()
            {
                Timeout = WaitSeconds(ScriptFormula(7));
            }
        }
        [ImplementsPowerBuff(2)]
        class BatDamage : PowerBuff
        {
            const float _damageRate = 1f;
            TickTimer _damageTimer = null;

            public override void Init()
            {
                if (Rune_C > 0)
                {
                    Timeout = WaitSeconds(3f);
                }
                else
                    Timeout = WaitSeconds(1f);
            }
            public override bool Update()
            {
                if (base.Update())
                    return true;
                if (_damageTimer == null || _damageTimer.TimedOut)
                {
                    _damageTimer = WaitSeconds(_damageRate);
                    UsePrimaryResource(ScriptFormula(0)); //testing in this area for resource. 
                    if (Rune_E > 0)
                    {
                        WeaponDamage(GetEnemiesInRadius(User.Position, ScriptFormula(21)), ScriptFormula(19), DamageType.Fire);
                    }
                    else if (Rune_A > 0)
                    {
                        AttackPayload attack = new AttackPayload(this);
                        attack.SetSingleTarget(Target);
                        attack.AddWeaponDamage(ScriptFormula(15), DamageType.Fire);
                        attack.Apply();
                    }
                    else if (Rune_B > 0)
                    {
                        AttackPayload attack = new AttackPayload(this);
                        attack.SetSingleTarget(Target);
                        attack.AddWeaponDamage(ScriptFormula(18), DamageType.Fire);
                        attack.Apply();
                    }
                    else if (Rune_C > 0)
                    {
                        AttackPayload attack = new AttackPayload(this);
                        attack.SetSingleTarget(Target);
                        attack.AddWeaponDamage(ScriptFormula(12), DamageType.Poison);
                        attack.OnHit = HitPayload =>
                        {
                        };
                        attack.Apply();
                    }
                    else
                    {
                        AttackPayload attack = new AttackPayload(this);
                        attack.SetSingleTarget(Target);
                        attack.AddWeaponDamage(ScriptFormula(1), DamageType.Fire);
                        attack.OnHit = HitPayload =>
                        {
                            //Rune_D -> Healing
                        };
                        attack.Apply();
                    }
                }
                return false;
            }
        }
    }
    #endregion

    //TODOs: read inside
    #region Firebomb
    [ImplementsPowerSNO(Skills.Skills.WitchDoctor.PhysicalRealm.Firebomb)]
    public class Firebomb : Skill
    {
        //NoRune:blank, A: radius, B: Bounce, C: Pool, D: Turret, E: chainLightning
        //TODO:Rune_B,D,E
        //Rune_B -> Allows skull to bounce up to two times, reduce damage by 15%
        //Rune_E -> Instead of firebomb doing AoE, each does direct damage to enemy then bounces to up to nearby enemies, reduce damage by 20%
        public override IEnumerable<TickTimer> Main()
        {
            UsePrimaryResource(EvalTag(PowerKeys.ResourceCost));

            Projectile[] grenades = new Projectile[1];
            for (int i = 0; i < grenades.Length; ++i)
            {
                var projectile = new Projectile(this, 6453, User.Position);
                grenades[i] = projectile;
            }

            float height = ScriptFormula(3);

            for (int i = 0; i < grenades.Length; ++i)
            {
                grenades[i].LaunchArc(PowerMath.TranslateDirection2D(TargetPosition, User.Position, TargetPosition,
                                                                      0f), height, ScriptFormula(2));
            }
            yield return grenades[0].ArrivalTime;

            if (Rune_D > 0)
            {
                //TODO:figure out the column animation.
                var FireColumn = new EffectActor(this, -1, TargetPosition);
                FireColumn.Timeout = WaitSeconds(ScriptFormula(14));
                FireColumn.Scale = 2f;
                FireColumn.Spawn();
                FireColumn.UpdateDelay = 0.33f; // attack every half-second
                FireColumn.OnUpdate = () =>
                {
                    var targets = GetEnemiesInRadius(FireColumn.Position, ScriptFormula(26));
                    if (targets.Actors.Count > 0 && targets != null)
                    {
                        targets.SortByDistanceFrom(FireColumn.Position);
                        var proj = new Projectile(this, 193969, FireColumn.Position);
                        proj.Position.Z += 5f;  // unknown if this is needed
                        proj.OnCollision = (hit) =>
                        {
                            WeaponDamage(hit, ScriptFormula(13), DamageType.Fire);

                            proj.Destroy();
                        };
                        FireColumn.TranslateFacing(targets.Actors[0].Position, true);
                        proj.LaunchArc(targets.Actors[0].Position, ScriptFormula(29), ScriptFormula(28));
                    }

                };
            }
            else
            {
                foreach (var grenade in grenades)
                {
                    var grenadeN = grenade;

                    SpawnEffect(6451, TargetPosition);

                    if (Rune_C > 0)
                    {
                        var pool = SpawnEffect(6483, grenade.Position, 0, WaitSeconds(ScriptFormula(12)));
                        pool.UpdateDelay = 1f;
                        pool.OnUpdate = () =>
                        {
                            WeaponDamage(GetEnemiesInRadius(grenadeN.Position, ScriptFormula(11)), ScriptFormula(10), DamageType.Fire);
                        };
                    }

                    AttackPayload attack = new AttackPayload(this);
                    attack.Targets = GetEnemiesInRadius(grenade.Position, ScriptFormula(4));
                    attack.AddWeaponDamage(ScriptFormula(0), DamageType.Fire);
                    attack.OnHit = (hitPayload) =>
                    {
                        if (Rune_A > 0)
                        {
                            SpawnEffect(193964, grenade.Position);
                            WeaponDamage(GetEnemiesInRadius(grenadeN.Position, ScriptFormula(5)), ScriptFormula(6), DamageType.Fire);
                        }
                    };
                    attack.Apply();
                }
            }
        }
    }
    #endregion

    //TODOs: Decoy HP and ID, SpiritWalk Appearance, Rune_C explosion(SNO for it?) needs to be from spawnproxy
    //decoy deathmask.acr? buff/decoydeath/decoylook.efg?
    #region SpiritWalk
    [ImplementsPowerSNO(Skills.Skills.WitchDoctor.SpiritRealm.SpiritWalk)]
    public class SpiritWalk : Skill
    {
        public override IEnumerable<TickTimer> Main()
        {
            StartCooldown(EvalTag(PowerKeys.CooldownTime));
            UsePrimaryResource(EvalTag(PowerKeys.ResourceCost));

            //Newest Patch adds Run Speed Increase = SF(16) "0.50"
            Vector3D DecoySpot = new Vector3D(User.Position);
            Actor blast = SpawnProxy(DecoySpot);

            //SpawnEffect(106584, DecoySpot, 0, WaitSeconds(ScriptFormula(0))); //Male
            SpawnEffect(107705, DecoySpot, 0, WaitSeconds(ScriptFormula(0))); //Female


            AddBuff(User, new SpiritWalkBuff());

            if (Rune_C > 0)
            {
                yield return WaitSeconds(ScriptFormula(0));
                AttackPayload attack = new AttackPayload(this);
                attack.Targets = GetEnemiesInRadius(blast.Position, ScriptFormula(8));
                attack.AddWeaponDamage(ScriptFormula(6), DamageType.Fire);
                attack.Apply();
            }
            else

                yield break;
        }
        [ImplementsPowerBuff(1)]
        class SpiritWalkBuff : PowerBuff
        {
            const float _damageRate = 0.25f;
            TickTimer _damageTimer = null;

            //Look_override ghostly appearance
            public override void Init()
            {
                Timeout = WaitSeconds(ScriptFormula(0));
            }
            public override bool Apply()
            {
                if (!base.Apply())
                    return false;
                User.Attributes[GameAttribute.Look_Override] = unchecked((int)0xF2F224EA);
                //User.Attributes[GameAttribute.Walk_Passability_Power_SNO] = ;
                User.Attributes[GameAttribute.Stealthed] = true;
                //User.Attributes[GameAttribute.Untargetable] = true;
                //User.Attributes[GameAttribute.UntargetableByPets] = true;
                if (Rune_D > 0)
                {
                    User.Attributes[GameAttribute.Resource_Regen_Percent_Per_Second] += ScriptFormula(9);
                }
                if (Rune_E > 0)
                {
                    //is this attribute by percent on its own? "Gain 16% of your maximum Life every second"
                    User.Attributes[GameAttribute.Hitpoints_Regen_Per_Second] += ScriptFormula(10);
                }
                User.Attributes.BroadcastChangedIfRevealed();
                return true;
            }
            public override void Remove()
            {
                base.Remove();
                if (Rune_D > 0)
                {
                    User.Attributes[GameAttribute.Resource_Regen_Percent_Per_Second] -= ScriptFormula(9);
                }
                if (Rune_E > 0)
                {
                    //is this attribute by percent on its own? "Gain 16% of your maximum Life every second"
                    User.Attributes[GameAttribute.Hitpoints_Regen_Per_Second] -= ScriptFormula(10);
                }
                User.Attributes[GameAttribute.Look_Override] = 0;
                //User.Attributes[GameAttribute.Walk_Passability_Power_SNO] = ;
                User.Attributes[GameAttribute.Stealthed] = false;
                //User.Attributes[GameAttribute.Untargetable] = false;
                //User.Attributes[GameAttribute.UntargetableByPets] = false;
                User.Attributes.BroadcastChangedIfRevealed();
            }

            public override bool Update()
            {
                if (base.Update())
                    return true;
                if (Rune_A > 0)
                {
                    if (_damageTimer == null || _damageTimer.TimedOut)
                    {
                        _damageTimer = WaitSeconds(_damageRate);

                        foreach (Actor enemy in GetEnemiesInRadius(User.Position, ScriptFormula(4)).Actors)
                        {
                            AddBuff(Target, new SpiritWalkDamage());
                        }
                    }
                }
                return false;
            }
        }
        /*[ImplementsPowerBuff(1)]
        class DecoyLookBuff : PowerBuff
        {
            //Brain or something?
            //Breakable shield HP? idk..
            //Dummy Health -> Script(11) * MaxHP -> once this  
            public override void Init()
            {
                Timeout = WaitSeconds(ScriptFormula(0));
            }
        }*/
        [ImplementsPowerBuff(2)]
        class SpiritWalkDamage : PowerBuff
        {
            const float _damageRate = 1f;
            TickTimer _damageTimer = null;

            public override void Init()
            {
                Timeout = WaitSeconds(1f);
            }
            public override bool Update()
            {
                if (base.Update())
                    return true;
                if (_damageTimer == null || _damageTimer.TimedOut)
                {
                    _damageTimer = WaitSeconds(_damageRate);

                    AttackPayload attack = new AttackPayload(this);
                    attack.Targets = GetEnemiesInRadius(User.Position, ScriptFormula(4));
                    attack.AddWeaponDamage(ScriptFormula(1), DamageType.Physical);
                    attack.Apply();
                }
                return false;
            }
        }
    }
    #endregion

    //Seems alright.. just needs tweaking, check on the teleportation of the character occassionally when casting.
    #region SoulHarvest
    [ImplementsPowerSNO(Skills.Skills.WitchDoctor.SpiritRealm.SoulHarvest)]
    public class SoulHarvest : Skill
    {
        public override IEnumerable<TickTimer> Main()
        {
            StartCooldown(EvalTag(PowerKeys.CooldownTime));
            UsePrimaryResource(EvalTag(PowerKeys.ResourceCost));

            User.PlayEffectGroup(19275);

            foreach (Actor enemy in GetEnemiesInRadius(User.Position, ScriptFormula(3), 5).Actors)
            {
                enemy.PlayEffectGroup(1164);
                WeaponDamage(enemy, 30f, DamageType.Physical);
                enemy.AddComplexEffect(19277, User);
                if (Rune_E > 0)
                {
                    WeaponDamage(enemy, ScriptFormula(0), DamageType.Physical);
                }
                if (Rune_C > 0)
                {
                    AddBuff(enemy, new ObsidianDebuff());
                    AddBuff(enemy, new DebuffSlowed(ScriptFormula(10), WaitSeconds(ScriptFormula(11))));
                }
                yield return WaitSeconds(0.7f);
                AddBuff(User, new soulHarvestbuff());
            }

            yield break;
        }
        [ImplementsPowerBuff(0, true)]
        class soulHarvestbuff : PowerBuff
        {
            public override void Init()
            {
                base.Init();
                Timeout = WaitSeconds(ScriptFormula(7));
                MaxStackCount = (int)ScriptFormula(13);
            }
            public override bool Apply()
            {
                if (!base.Apply())
                    return false;
                _AddHarvest();
                return true;
            }
            public override void Remove()
            {
                base.Remove();
                {
                    Target.Attributes[GameAttribute.Attack_Bonus] -= StackCount * ScriptFormula(8);
                    Target.Attributes.BroadcastChangedIfRevealed();
                }
            }
            public override bool Stack(Buff buff)
            {
                bool stacked = StackCount < MaxStackCount;

                base.Stack(buff);

                if (stacked)
                    _AddHarvest();

                return true;
            }
            private void _AddHarvest()
            {
                if (Rune_A > 0)
                {
                    //heal life SF(9)
                }
                if (Rune_D > 0)
                {
                    //Gain Mana SF(4)
                }

                Target.Attributes[GameAttribute.Attack_Bonus] += ScriptFormula(8);
                Target.Attributes.BroadcastChangedIfRevealed();
            }
        }
        [ImplementsPowerBuff(1)]
        class ObsidianDebuff : PowerBuff
        {
            public override void Init()
            {
                Timeout = WaitSeconds(ScriptFormula(11));
            }
        }
    }
    #endregion

    //SMall Portion of the skill complete
    #region LocustSwarm
    [ImplementsPowerSNO(Skills.Skills.WitchDoctor.PhysicalRealm.LocustSwarm)]
    public class LocustSwarm : Skill
    {
        //Summon a plague of locusts to assault enemies, dealing [25 * {Script Formula 18} * 100]% weapon damage per second as Poison for 3 seconds. The locusts will jump to additional nearby targets.
        public override IEnumerable<TickTimer> Main()
        {
            UsePrimaryResource(EvalTag(PowerKeys.ResourceCost));

            //cast, spread to those in radius, from there jump to other mobs in area within (__?__)
            //does not always focus the correct way.
            User.PlayEffectGroup(106765);

            //just a little wait for the animation
            yield return WaitSeconds(0.5f);


            foreach (Actor LocustTarget in GetEnemiesInArcDirection(User.Position, TargetPosition, ScriptFormula(14), 30f).Actors)
            {
                AddBuff(LocustTarget, new LocustSwarmer(WaitSeconds(ScriptFormula(1))));
            }
            yield break;
        }
        [ImplementsPowerBuff(0)]
        class LocustSwarmer : PowerBuff
        {
            const float _damageRate = 1f;
            TickTimer _damageTimer = null;

            float _jumpRate = 3f;
            TickTimer _jumpTimer = null;

            public LocustSwarmer(TickTimer timeout)
            {
                Timeout = timeout;
            }
            public override bool Update()
            {
                if (base.Update())
                    return true;
                if (_damageTimer == null || _damageTimer.TimedOut)
                {
                    _damageTimer = WaitSeconds(_damageRate);

                    AttackPayload attack = new AttackPayload(this);
                    attack.SetSingleTarget(Target);
                    attack.AddWeaponDamage(ScriptFormula(0), Rune_A > 0 ? DamageType.Fire : DamageType.Poison);
                    attack.AutomaticHitEffects = false;
                    attack.Apply();
                }

                /*while (_jumpRate > ScriptFormula(4))
                {
                    if (_jumpTimer == null || _jumpTimer.TimedOut)
                    {
                        _jumpTimer = WaitSeconds(_jumpRate);
                        //swarm jump radius
                        var newTarget = GetEnemiesInRadius(Target.Position, ScriptFormula(2)).GetClosestTo(Target.Position);
                        while (newTarget != null)
                        {
                            //Target.AddComplexEffect(106839, newTarget);
                            AddBuff(newTarget, new LocustSwarmer(WaitSeconds(ScriptFormula(1))));
                        }
                        _jumpRate *= 0.9f; //delta = Swarm Jump Time Delta
                    }
                }*/

                return false;
            }
        }
        [ImplementsPowerBuff(1)]
        class DiseaseSwarm : PowerBuff
        {
            const float _damageRate = 1f;
            TickTimer _damageTimer = null;

            public override void Init()
            {
                Timeout = WaitSeconds(ScriptFormula(11));
            }
            public override bool Update()
            {
                if (base.Update())
                    return true;
                if (_damageTimer == null || _damageTimer.TimedOut)
                {
                    _damageTimer = WaitSeconds(_damageRate);

                    AttackPayload attack = new AttackPayload(this);
                    attack.SetSingleTarget(Target);
                    attack.AddWeaponDamage(ScriptFormula(1), DamageType.Fire);
                    attack.Apply();


                }
                return false;
            }
        }
    }
    #endregion

    //basic skill works, Rune_E most works, other runes need fixing.
    #region SpiritBarrage
    [ImplementsPowerSNO(Skills.Skills.WitchDoctor.SpiritRealm.SpiritBarrage)]
    public class SpiritBarrage : Skill
    {
        public override IEnumerable<TickTimer> Main()
        {
            UsePrimaryResource(EvalTag(PowerKeys.ResourceCost));

            if (Rune_E > 0)
            {
                AddBuff(User, new BarrageSpirit());
            }
            else if (Rune_B > 0)
            {
                //same as regular shot just with extra spirit bolts.
                /* Missile Buff Duration
                 * Missile Buff Periodic Fire Rate
                 * Missile Damage Scalar
                 * Missile Check Radius
                 */
            }
            else if (Rune_C > 0)
            {
                //this doesnt work.
                var AOE_Ghost = SpawnEffect(181880, TargetPosition, 0, WaitSeconds(ScriptFormula(11)));
                AOE_Ghost.PlayEffectGroup(186804);
                AOE_Ghost.UpdateDelay = 1f;
                AOE_Ghost.OnUpdate = () =>
                {
                    AttackPayload attack = new AttackPayload(this);
                    attack.Targets = GetEnemiesInRadius(AOE_Ghost.Position, ScriptFormula(13));
                    attack.AddWeaponDamage(ScriptFormula(14), DamageType.Physical);
                    attack.Apply();
                };
            }
            else
            {
                var Target = GetEnemiesInRadius(TargetPosition, ScriptFormula(14)).GetClosestTo(TargetPosition);
                if (Target != null)
                {
                    User.PlayEffectGroup(175350, Target);
                    yield return WaitSeconds(ScriptFormula(2));
                    Target.PlayEffectGroup(175403);

                    WeaponDamage(Target, ScriptFormula(0), DamageType.Physical);
                    //Rune_D -> Mana Gain (SF(5))
                }
            }
            yield break;
        }
        [ImplementsPowerBuff(3)]
        class BarrageSpirit : PowerBuff
        {
            const float _damageRate = 0.6f;
            TickTimer _damageTimer = null;

            public override void Init()
            {
                Timeout = WaitSeconds(ScriptFormula(17));
            }
            public override bool Update()
            {
                if (base.Update())
                    return true;
                if (_damageTimer == null || _damageTimer.TimedOut)
                {
                    _damageTimer = WaitSeconds(_damageRate);

                    var Target = GetEnemiesInRadius(User.Position, ScriptFormula(19)).GetClosestTo(User.Position);
                    if (Target != null)
                    {
                        //needs to turn to shoot at enemies.
                        User.PlayEffectGroup(181866, Target);
                        User.Position.Z += 10f; // this doesnt change the projectile position.
                        WaitSeconds(ScriptFormula(2));
                        Target.PlayEffectGroup(181944);

                        WeaponDamage(Target, ScriptFormula(20), DamageType.Physical);

                    }
                }
                return false;
            }
        }
    }
    #endregion

    //TODOs inside
    #region AcidCloud
    [ImplementsPowerSNO(Skills.Skills.WitchDoctor.PhysicalRealm.AcidCloud)]
    public class AcidCloud : Skill
    {
        //TODO: Max Pools = 3;
        //Rune_B Splash Delay?
        public override IEnumerable<TickTimer> Main()
        {
            UsePrimaryResource(EvalTag(PowerKeys.ResourceCost));

            if (Rune_E > 0)
            {
                SpawnEffect(121908, TargetPosition);
                yield return WaitSeconds(ScriptFormula(32));
                WeaponDamage(GetEnemiesInArcDirection(User.Position, TargetPosition, ScriptFormula(31),
                    ScriptFormula(30)), ScriptFormula(1), DamageType.Poison);
            }
            else
            {
                SpawnEffect(RuneSelect(61398, 121919, 122281, 121587, 121920, -1), TargetPosition);

                if (Rune_A > 0) { yield return WaitSeconds(ScriptFormula(14)); }
                else if (Rune_B > 0) { yield return WaitSeconds(ScriptFormula(16)); }
                else { yield return WaitSeconds(ScriptFormula(0)); }

                WeaponDamage(GetEnemiesInRadius(TargetPosition, ScriptFormula(1)), ScriptFormula(1), DamageType.Poison);

                var AcidPools = SpawnEffect(6509, TargetPosition, 0, WaitSeconds(ScriptFormula(5)));
                AcidPools.UpdateDelay = 0.25f; //idk
                AcidPools.OnUpdate = () =>
                {
                    foreach (Actor enemy in GetEnemiesInRadius(TargetPosition, ScriptFormula(4)).Actors)
                    {
                        if (AddBuff(enemy, new Disease_Debuff()))
                        {
                            AddBuff(enemy, new Disease_Debuff());
                        }
                    }
                };

                //(PET CLASS)
                if (Rune_C > 0)
                {
                    //slime -> 121595.ACR
                    //this is a pet and theyre are a max of 3 allowed.
                    //spawn slime that wanders in a certain area
                }
            }
            yield break;
        }
        [ImplementsPowerBuff(2)]
        class Disease_Debuff : PowerBuff
        {
            const float _damageRate = 0.25f; //this needs to be ScriptFormula(7) = buff tickrate
            TickTimer _damageTimer = null;

            public override void Init()
            {
                Timeout = WaitSeconds(0.25f);

            }
            public override bool Update()
            {
                if (base.Update())
                    return true;
                if (_damageTimer == null || _damageTimer.TimedOut)
                {
                    _damageTimer = WaitSeconds(_damageRate);

                    WeaponDamage(Target, ScriptFormula(8), DamageType.Poison);
                }
                return false;
            }
        }
    }
    #endregion

    //TODO: confusion ID for monsters, Runes_C,E(dogs), check equations.
    #region MassConfusion
    [ImplementsPowerSNO(Skills.Skills.WitchDoctor.SpiritRealm.MassConfusion)]
    public class MassConfusion : Skill
    {
        public override IEnumerable<TickTimer> Main()
        {
            StartCooldown(EvalTag(PowerKeys.CooldownTime));
            UsePrimaryResource(EvalTag(PowerKeys.ResourceCost));

            //Target.PlayEffectGroup(184540);
            TargetList Half = GetEnemiesInRadius(TargetPosition, ScriptFormula(1));
            foreach (Actor enemy in GetEnemiesInRadius(TargetPosition, ScriptFormula(1), ((int)Half.Actors.Count / 2)).Actors)
            {
                AddBuff(enemy, new Confusion_Debuff());
            }
            WeaponDamage(GetEnemiesInRadius(TargetPosition, ScriptFormula(1)), ScriptFormula(3), DamageType.Physical);

            if (Rune_B > 0)
            {
                foreach (Actor enemy in GetEnemiesInRadius(TargetPosition, ScriptFormula(1), (int)ScriptFormula(4)).Actors)
                {
                    //if it doesnt have confusion, it gets stunned.
                    if (!AddBuff(enemy, new Confusion_Debuff()))
                    {
                        AddBuff(enemy, new DebuffStunned(WaitSeconds(ScriptFormula(6))));
                    }
                }
            }
            if (Rune_C > 0)
            {
                //could not find the correct Projectile actor for this.
                var proj = new Projectile(this, -1, User.Position);
                proj.Position.Z += 5f;  // unknown if this is needed
                proj.OnUpdate = () =>
                {
                    foreach (Actor enemy in GetEnemiesInRadius(proj.Position, ScriptFormula(10)).Actors)
                    {
                        AddBuff(enemy, new SpiritDoT());
                    }
                };
                proj.Launch(TargetPosition, ScriptFormula(16));
            }
            yield break;
        }
        [ImplementsPowerBuff(0)]
        class Confusion_Debuff : PowerBuff
        {
            public override void Init()
            {
                Timeout = WaitSeconds(ScriptFormula(0));
            }
            public override bool Apply()
            {
                if (!base.Apply())
                    return false;
                Target.Attributes[GameAttribute.Team_Override] = 0;
                return true;
            }
            public override void Remove()
            {
                base.Remove();
                Target.Attributes[GameAttribute.Team_Override] = 0;
            }
        }
        [ImplementsPowerBuff(1)]
        class SpiritDoT : PowerBuff
        {
            const float _damageRate = 1f;
            TickTimer _damageTimer = null;

            public override void Init()
            {
                Timeout = WaitSeconds(1f);

            }
            public override bool Update()
            {
                if (base.Update())
                    return true;
                if (_damageTimer == null || _damageTimer.TimedOut)
                {
                    _damageTimer = WaitSeconds(_damageRate);

                    WeaponDamage(Target, ScriptFormula(11), DamageType.Physical);
                }
                return false;
            }
        }
    }
    #endregion

    //Needs Buff Functions fixed and Rune_E added
    #region BigBadVoodoo
    [ImplementsPowerSNO(Skills.Skills.WitchDoctor.Support.BigBadVoodoo)]
    public class BigBadVoodoo : Skill
    {
        public override IEnumerable<TickTimer> Main()
        {
            StartCooldown(EvalTag(PowerKeys.CooldownTime));
            UsePrimaryResource(EvalTag(PowerKeys.ResourceCost));

            var ShamSpot = SpawnProxy(TargetPosition, WaitSeconds(ScriptFormula(0) + 1f));
            SpawnEffect(RuneSelect(117574, 182271, 182276, 182278, 182283, 117574), TargetPosition, 0, WaitSeconds(ScriptFormula(0))).PlayEffectGroup(181291);
            AddBuff(ShamSpot, new AuraBuff());
            yield break;
        }

        [ImplementsPowerBuff(0)]
        class AuraBuff : PowerBuff
        {
            const float _damageRate = 0.5f;
            TickTimer _damageTimer = null;

            const float _healRate = 1f;
            TickTimer _healTimer = null;

            public override void Init()
            {
                Timeout = WaitSeconds(ScriptFormula(0));

            }
            public override bool Update()
            {
                if (base.Update())
                    return true;
                if (_damageTimer == null || _damageTimer.TimedOut)
                {
                    _damageTimer = WaitSeconds(_damageRate);
                    //your character
                    //if User in Radius of Target.Position, get this buff plus Rune_D if active.
                    AddBuff(User, new FetishShamanBuff());
                    if (Rune_D > 0)
                    {
                        AddBuff(User, new Golden_ManaBuff());
                    }

                    foreach (Actor Ally in GetAlliesInRadius(Target.Position, ScriptFormula(2)).Actors)
                    {
                        AddBuff(Ally, new FetishShamanBuff());
                    }
                    if (Rune_D > 0)
                    {
                        foreach (Actor Ally in GetAlliesInRadius(Target.Position, ScriptFormula(2)).Actors)
                        {
                            AddBuff(Ally, new Golden_ManaBuff());
                        }
                    }
                }
                if (_healTimer == null || _healTimer.TimedOut)
                {
                    _healTimer = WaitSeconds(_healRate);

                    if (Rune_E > 0)
                    {
                        //User health as well
                        foreach (Actor Ally in GetAlliesInRadius(Target.Position, ScriptFormula(2)).Actors)
                        {
                            //Heal allies for ScriptFormula(4) of Max HP
                        }
                    }
                }
                return false;
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
        [ImplementsPowerBuff(2)]
        class FetishShamanBuff : PowerBuff
        {
            public override void Init()
            {
                Timeout = WaitSeconds(ScriptFormula(9));

            }
            public override bool Apply()
            {
                if (!base.Apply())
                    return false;

                if (Rune_A > 0)
                {
                    Target.Attributes[GameAttribute.Attack_Bonus_Percent] += ScriptFormula(3);
                    Target.Attributes.BroadcastChangedIfRevealed();
                }
                AddBuff(Target, new SpeedBuff(ScriptFormula(1), WaitSeconds(ScriptFormula(9))));
                AddBuff(Target, new MovementBuff(ScriptFormula(1), WaitSeconds(ScriptFormula(9))));
                return true;
            }
            public override void Remove()
            {
                base.Remove();
                if (Rune_A > 0)
                {
                    Target.Attributes[GameAttribute.Attack_Bonus_Percent] -= ScriptFormula(3);
                    Target.Attributes.BroadcastChangedIfRevealed();
                }
            }
        }
        [ImplementsPowerBuff(3)]
        class Golden_ManaBuff : PowerBuff
        {
            //Buff
            const float _damageRate = 1f;
            TickTimer _damageTimer = null;

            public override void Init()
            {
                Timeout = WaitSeconds(1f);

            }
            public override bool Update()
            {
                if (base.Update())
                    return true;
                if (_damageTimer == null || _damageTimer.TimedOut)
                {
                    _damageTimer = WaitSeconds(_damageRate);

                    //Restore ScriptFormula(5) Mana

                }
                return false;
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

    //TODO: Animation Partially complete, Wide(B) isnt complete and Creepers(A) haven't been worked on.
    //TODO: Tower(E) needs to spawn, Arc Distance, max distance from target is 15f.
    #region WallOfZombies
    [ImplementsPowerSNO(Skills.Skills.WitchDoctor.PhysicalRealm.WallOfZombies)]
    public class WallOfZombies : Skill
    {
        //TODO:Unknown how to do the width of the Wall of Zombies..
        public override IEnumerable<TickTimer> Main()
        {
            UsePrimaryResource(EvalTag(PowerKeys.ResourceCost));

            if (Rune_C > 0)
            {
                Vector3D[] projDestinations = PowerMath.GenerateSpreadPositions(User.Position, TargetPosition, 52f, (int)ScriptFormula(5));

                for (int i = 0; i < projDestinations.Length; i++)
                {
                    var proj = new Projectile(this, 183977, User.Position);
                    proj.OnCollision = (hit) =>
                    {
                        proj.Destroy();
                        WeaponDamage(hit, ScriptFormula(4), DamageType.Physical);
                        SpawnEffect(182695, new Vector3D(hit.Position.X, hit.Position.Y, hit.Position.Z));
                    };
                    proj.Launch(projDestinations[i], 0.2f);
                }
            }
            else if (Rune_B > 0)
            {
                //this needs to have double the width, 
                //at the moment this only shows the double length when pointing spell to the right.

                float castAngle = MovementHelpers.GetFacingAngle(User.Position, TargetPosition);
                Vector3D[] spawnPoints = PowerMath.GenerateSpreadPositions(TargetPosition, new Vector3D(TargetPosition.X + 10f, TargetPosition.Y, TargetPosition.Z), 180, 2);

                for (int i = 0; i < spawnPoints.Length; ++i)
                {
                    SpawnEffect(135016, spawnPoints[i], castAngle, WaitSeconds(ScriptFormula(0)));
                }

            }
            else
            {
                float castAngle = MovementHelpers.GetFacingAngle(User.Position, TargetPosition);
                var Wall = SpawnEffect(RuneSelect(131202, -1, 135016, -1, 182574, 131640), TargetPosition, castAngle, WaitSeconds(ScriptFormula(0)));
                Wall.UpdateDelay = 1f;
                Wall.OnUpdate = () =>
                {
                    //set position in front of zombies, add width. rectangle?
                    if (Rune_D > 0)
                    {
                        //slow movement of enemies
                    }
                };
            }
            yield break;
        }
    }
    #endregion

    //--------------------------Pet Classes Below----------------------//

    //Pet Class
    #region FetishArmy
    [ImplementsPowerSNO(Skills.Skills.WitchDoctor.Support.FetishArmy)]
    public class FetishArmy : Skill
    {
        public override IEnumerable<TickTimer> Main()
        {
            int maxFetishes = 5;
            List<Actor> Fetishes = new List<Actor>();
            for (int i = 0; i < maxFetishes; i++)
            {
                var Fetish = new FetishMelee(this.World, this, i);
                Fetish.Brain.DeActivate();
                Fetish.Position = RandomDirection(User.Position, 3f, 8f); //Kind of hacky until we get proper collisiondetection
                Fetish.Attributes[GameAttribute.Untargetable] = true;
                Fetish.EnterWorld(Fetish.Position);
                Fetish.PlayActionAnimation(90118);
                Fetishes.Add(Fetish);
                yield return WaitSeconds(0.2f);
            }
            yield return WaitSeconds(0.8f);
            foreach (Actor Fetish in Fetishes)
            {
                (Fetish as Minion).Brain.Activate();
                Fetish.Attributes[GameAttribute.Untargetable] = false;
                Fetish.Attributes.BroadcastChangedIfRevealed();
                Fetish.PlayActionAnimation(87190); //Not sure why this is required, but after the summon is done, it'll just be frozen otherwise.
            }

            //TODOs
            //Spawns 5 melee Fighters. (pets)
            //Rune_A = apparateDamage.efg to all spawned fetishes that deals radius damage
            //Rune_B spawns 2 more fighters
            //Rune_C spawns 2 shamans -> [213553] "shamanFireBreath.efg"
            //Rune_D just decreases cooldown
            //Rune_E spawns 2 hunters, projectile possibly[206229]"hunter.acr" and impact[206230]"hunter_impact.efg"
            yield break;
        }
    }
    #endregion

    //Pet Class
    #region Sacrifice
    [ImplementsPowerSNO(Skills.Skills.WitchDoctor.Support.Sacrifice)]
    public class Sacrifice : Skill
    {
        public override IEnumerable<TickTimer> Main()
        {
            PlayerHasDogsBuff buff = World.BuffManager.GetFirstBuff<PlayerHasDogsBuff>(this.User);
            if (buff != null)
            {
                foreach (ZombieDog dog in buff.dogs)
                {
                    dog.Kill(this);
                }
                World.BuffManager.RemoveBuffs(this.User, buff.GetType());
                StartCooldown(1f);
            }
            yield break;
        }
    }
    #endregion

    //Pet Class
    #region Gargantuan
    [ImplementsPowerSNO(Skills.Skills.WitchDoctor.Support.Gargantuan)]
    public class Gargantuan : Skill
    {
        public override IEnumerable<TickTimer> Main()
        {
            var garg = new GargantuanMinion(this.World, this, 0);
            garg.Brain.DeActivate();
            garg.Position = RandomDirection(User.Position, 3f, 8f); //Kind of hacky until we get proper collisiondetection
            garg.Attributes[GameAttribute.Untargetable] = true;
            garg.EnterWorld(garg.Position);
            garg.PlayActionAnimation(155988);
            yield return WaitSeconds(0.8f);

            (garg as Minion).Brain.Activate();
            garg.Attributes[GameAttribute.Untargetable] = false;
            garg.Attributes.BroadcastChangedIfRevealed();
            garg.PlayActionAnimation(144967); //Not sure why this is required, but after the summon is done, it'll just be frozen otherwise.

            yield break;
        }
    }
    #endregion

    //Pet Class
    #region Hex
    [ImplementsPowerSNO(Skills.Skills.WitchDoctor.Support.Hex)]
    public class Hex : Skill
    {
        public override IEnumerable<TickTimer> Main()
        {
            StartCooldown(EvalTag(PowerKeys.CooldownTime));

            var hex = new HexMinion(this.World, this, 0);
            hex.Brain.DeActivate();
            hex.Position = RandomDirection(User.Position, 3f, 8f); //Kind of hacky until we get proper collisiondetection
            hex.Attributes[GameAttribute.Untargetable] = true;
            hex.EnterWorld(hex.Position);
            hex.PlayActionAnimation(90118);
            yield return WaitSeconds(0.8f);

            (hex as Minion).Brain.Activate();
            hex.Attributes[GameAttribute.Untargetable] = false;
            hex.Attributes.BroadcastChangedIfRevealed();
            yield break;
        }
    }
    #endregion

    //Jarthrow complete, need pet spider class
    #region CorpseSpiders
    [ImplementsPowerSNO(Skills.Skills.WitchDoctor.PhysicalRealm.CorpseSpiders)]
    public class CorpseSpiders : Skill
    {
        public override IEnumerable<TickTimer> Main()
        {
            var proj = new Projectile(this, RuneSelect(106504, 215811, 215815, 215816, 215814, 215813), User.Position);
            proj.Position.Z += 5f;
            proj.LaunchArc(TargetPosition, 5f, -0.07f);
            yield return WaitSeconds(0.4f);
            proj.OnArrival = () =>
            {
                proj.Destroy();
            };
            SpawnEffect(110714, TargetPosition);

            //the rest of this is spiders, which are pets i presume?
            yield return WaitSeconds(0.05f);

            var spider = new CorpseSpider(this.World, this, 0);
            spider.Brain.DeActivate();
            spider.Position = RandomDirection(User.Position, 3f, 8f); //Kind of hacky until we get proper collisiondetection
            spider.Attributes[GameAttribute.Untargetable] = true;
            spider.EnterWorld(spider.Position);
            yield return WaitSeconds(0.05f);

            (spider as Minion).Brain.Activate();
            spider.Attributes[GameAttribute.Untargetable] = false;
            spider.Attributes.BroadcastChangedIfRevealed();

            yield break;
        }
    }
    #endregion

    //Pet Class
    //TODO: fix up
    #region SummonZombieDogs
    //TODO: This is mostly hacked together, but there are a few main points:
    //When using the Zombie Handler passive, it'll spawn 4 dogs. Need to somehow detect that that passive has switched off, and if so, kill one dog.
    //There might be problems with players using a certain rune when summoning, then switching, to get both effects. 
    //This could possibly be solved by saving the state of runes when summoning, but perhaps a OnSwitchRune could be a good way instead.
    [ImplementsPowerSNO(Skills.Skills.WitchDoctor.Support.SummonZombieDogs)]
    public class SummonZombieDogs : Skill
    {
        public override IEnumerable<TickTimer> Main()
        {
            //System.Console.Out.WriteLine("lol1");
            StartCooldown(60f);
            PlayerHasDogsBuff buff = World.BuffManager.GetFirstBuff<PlayerHasDogsBuff>(this.User);
            if (buff != null)
            {
                //System.Console.Out.WriteLine("lol");
                foreach (ZombieDog dog in buff.dogs)
                {
                    dog.Kill(this);
                }
                World.BuffManager.RemoveBuffs(this.User, buff.GetType());
            }
            //System.Console.Out.WriteLine("lol2");
            int maxDogs = (int)ScriptFormula(0);
            List<Actor> dogs = new List<Actor>();
            for (int i = 0; i < maxDogs; i++)
            {
                //System.Console.Out.WriteLine("lol3_" + i);
                var dog = new ZombieDog(this.World, this, i);
                dog.Brain.DeActivate();
                dog.Position = RandomDirection(User.Position, 3f, 8f); //Kind of hacky until we get proper collisiondetection
                dog.Attributes[GameAttribute.Untargetable] = true;
                dog.EnterWorld(dog.Position);
                dog.PlayActionAnimation(11437);
                dogs.Add(dog);
                yield return WaitSeconds(0.2f);
            }
            yield return WaitSeconds(0.8f);
            //System.Console.Out.WriteLine("lol4");
            foreach (Actor dog in dogs)
            {
                //System.Console.Out.WriteLine("lol5");
                (dog as Minion).Brain.Activate();
                dog.Attributes[GameAttribute.Untargetable] = false;
                dog.Attributes.BroadcastChangedIfRevealed();
                dog.PlayActionAnimation(11431); //Not sure why this is required, but after the summon is done, it'll just be frozen otherwise.
            }
            //System.Console.Out.WriteLine("lol6");
            AddBuff(this.User, new PlayerHasDogsBuff(dogs));

            yield break;
        }
    }
    class PlayerHasDogsBuff : PowerBuff
    {
        public List<Actor> dogs;

        public PlayerHasDogsBuff(List<Actor> dogs)
        {
            this.dogs = dogs;
        }
        public override bool Apply()
        {
            if (!base.Apply())
                return false;
            //this.User.Attributes[GameAttribute.Skill_Toggled_State, Skills.Skills.WitchDoctor.Support.Sacrifice] = true;
            //User.Attributes.BroadcastChangedIfRevealed();

            return true;
        }

        public override void OnPayload(Payload payload)
        {
            if (payload is DeathPayload)
            {

            }
        }

        public override void Remove()
        {
            base.Remove();
        }
    }
    #endregion
}
