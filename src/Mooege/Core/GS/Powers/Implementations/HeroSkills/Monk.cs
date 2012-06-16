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
using System.Text;
using Mooege.Core.GS.Actors.Implementations.Minions;
using Mooege.Core.GS.Skills;
using Mooege.Net.GS.Message.Fields;
using Mooege.Core.GS.Actors;
using Mooege.Net.GS.Message;
using Mooege.Net.GS.Message.Definitions.Actor;
using Mooege.Core.GS.Common.Types.Math;
using Mooege.Core.GS.Players;
using Mooege.Core.GS.Ticker;
using Mooege.Core.GS.Common.Types.TagMap;
using Mooege.Core.GS.Powers.Payloads;
using Mooege.Net.GS.Message.Definitions.ACD;

namespace Mooege.Core.GS.Powers.Implementations
{
    //Complete
    #region DeadlyReach
    [ImplementsPowerSNO(Skills.Skills.Monk.SpiritGenerator.DeadlyReach)]
    public class MonkDeadlyReach : ComboSkill
    {
        public override IEnumerable<TickTimer> Main()
        {
            float reachRadius;
            float reachDegrees;

            switch (ComboIndex)
            {
                case 0:
                    reachRadius = ScriptFormula(5);
                    reachDegrees = ScriptFormula(9);
                    break;
                case 1:
                    reachRadius = ScriptFormula(4);
                    reachDegrees = ScriptFormula(3);
                    break;
                case 2:
                    reachRadius = ScriptFormula(20);
                    reachDegrees = ScriptFormula(19);
                    break;
                default:
                    yield break;
            }
            if (ComboIndex == 2 && Rune_C > 0)
            {
                bool hitAnything = false;
                AttackPayload attack = new AttackPayload(this);
                attack.Targets = GetEnemiesInRadius(User.Position, ScriptFormula(11), (int)ScriptFormula(8));
                attack.AddWeaponDamage(ScriptFormula(0), DamageType.Physical);
                attack.OnHit = hitPayload =>
                {
                    hitAnything = true;
                    if (Rune_D > 0)
                    {
                        if (hitPayload.IsCriticalHit)
                        {
                            GeneratePrimaryResource(ScriptFormula(7));
                        }
                    }
                };
                attack.Apply();

                if (hitAnything)
                    GeneratePrimaryResource(EvalTag(PowerKeys.SpiritGained));
            }
            else
            {
                bool hitAnything = false;
                AttackPayload attack = new AttackPayload(this);
                attack.Targets = GetEnemiesInArcDirection(User.Position, TargetPosition, reachRadius, reachDegrees);
                attack.AddWeaponDamage(ScriptFormula(0), DamageType.Physical);
                attack.OnHit = hitPayload =>
                {
                    hitAnything = true;
                    if (Rune_D > 0)
                    {
                        if (hitPayload.IsCriticalHit)
                        {
                            GeneratePrimaryResource(ScriptFormula(7));
                        }
                    }
                };
                attack.Apply();

                if (hitAnything)
                    GeneratePrimaryResource(EvalTag(PowerKeys.SpiritGained));
            }
            yield break;
        }

        [ImplementsPowerBuff(1, true)]
        class DeadlyReach_RuneAbuff : PowerBuff
        {
            public override void Init()
            {
                base.Init();
                Timeout = WaitSeconds(ScriptFormula(14));
                MaxStackCount = (int)ScriptFormula(12);
            }

            public override bool Apply()
            {
                if (!base.Apply())
                    return false;

                _AddAmp();
                return true;
            }

            public override void Remove()
            {
                base.Remove();
                Target.Attributes[GameAttribute.Attack_Bonus_Percent] -= StackCount * ScriptFormula(13);
                Target.Attributes.BroadcastChangedIfRevealed();
            }

            public override bool Stack(Buff buff)
            {
                bool stacked = StackCount < MaxStackCount;

                base.Stack(buff);

                if (stacked)
                    _AddAmp();

                return true;
            }

            private void _AddAmp()
            {
                Target.Attributes[GameAttribute.Attack_Bonus_Percent] += ScriptFormula(13);
                Target.Attributes.BroadcastChangedIfRevealed();
            }
        }
        [ImplementsPowerBuff(0)]
        class DeadlyReach_Armorbuff : PowerBuff
        {
            public override void Init()
            {
                base.Init();
                Timeout = WaitSeconds(ScriptFormula(21));
            }

            public override bool Apply()
            {
                if (!base.Apply())
                    return false;
                Target.Attributes[GameAttribute.Armor_Bonus_Percent] += ScriptFormula(22);
                Target.Attributes.BroadcastChangedIfRevealed();
                return true;
            }

            public override void Remove()
            {
                base.Remove();
                Target.Attributes[GameAttribute.Armor_Bonus_Percent] -= ScriptFormula(22);
                Target.Attributes.BroadcastChangedIfRevealed();
            }
        }
    }
    #endregion

    //TODO Rune_A,B,C
    #region FistsOfThunder
    [ImplementsPowerSNO(Skills.Skills.Monk.SpiritGenerator.FistsOfThunder)]
    public class MonkFistsOfThunder : ComboSkill
    {
        public override IEnumerable<TickTimer> Main()
        {
            switch (TargetMessage.Field5)
            {
                case 0:
                    if (Rune_A > 0)
                    {
                        //Teleport if TargetPosition >= Minimum Distance of 27f away && Target != null (SCriptF(3))
                    }
                    MeleeStageHit();
                    break;
                case 1:
                    MeleeStageHit();
                    break;
                case 2:
                    //AddBuff(User, new ComboStage3Buff());

                    // put target position a little bit in front of the monk. represents the lightning ball
                    TargetPosition = PowerMath.TranslateDirection2D(User.Position, TargetPosition,
                                        User.Position, ScriptFormula(6));

                    bool hitAnything = false;
                    AttackPayload attack = new AttackPayload(this);
                    attack.Targets = GetEnemiesInRadius(TargetPosition, ScriptFormula(20));
                    attack.AddWeaponDamage(Rune_A > 0 ? ScriptFormula(7) : ScriptFormula(0), DamageType.Lightning);
                    attack.OnHit = hitPayload =>
                    {
                        hitAnything = true;
                        if (Rune_B > 0)
                        {
                            //Chain of Lightning
                            //SF(10) = Max L Jumps
                            //SF(11) = L Damage
                            //SF(12) = L Jump Search Radius
                        }
                        else
                        {
                            Knockback(hitPayload.Target, ScriptFormula(21));
                        }
                        if (Rune_D > 0)
                        {
                            if (hitPayload.IsCriticalHit)
                            {
                                GeneratePrimaryResource(ScriptFormula(16));
                            }
                        }
                    };
                    attack.Apply();

                    if (hitAnything)
                    {
                        GeneratePrimaryResource(EvalTag(PowerKeys.SpiritGained));
                        if (Rune_E > 0)
                        {
                            AddBuff(User, new TFists_LightningBuff());
                        }
                    }

                    break;
            }

            yield break;
        }

        private void MeleeStageHit()
        {
            AttackPayload attack = new AttackPayload(this);
            attack.Targets = GetBestMeleeEnemy();
            attack.AddWeaponDamage(Rune_A > 0 ? ScriptFormula(7) : ScriptFormula(0), DamageType.Lightning);
            attack.OnHit = hitPayload =>
            {
                GeneratePrimaryResource(EvalTag(PowerKeys.SpiritGained));
                if (Rune_D > 0)
                {
                    if (hitPayload.IsCriticalHit)
                    {
                        GeneratePrimaryResource(ScriptFormula(16));
                    }
                }
                if (Rune_E > 0)
                {
                    AddBuff(User, new TFists_LightningBuff());
                }
            };
            attack.Apply();
        }
        [ImplementsPowerBuff(2, true)]
        class TFists_LightningBuff : PowerBuff
        {
            public override void Init()
            {
                base.Init();
                Timeout = WaitSeconds(ScriptFormula(19));
                MaxStackCount = (int)ScriptFormula(17);
            }

            public override bool Apply()
            {
                if (!base.Apply())
                    return false;

                _AddAmp();
                return true;
            }

            public override void Remove()
            {
                base.Remove();
                Target.Attributes[GameAttribute.Dodge_Chance_Bonus] -= StackCount * ScriptFormula(18);
                Target.Attributes.BroadcastChangedIfRevealed();
            }

            public override bool Stack(Buff buff)
            {
                bool stacked = StackCount < MaxStackCount;

                base.Stack(buff);

                if (stacked)
                    _AddAmp();

                return true;
            }

            private void _AddAmp()
            {
                Target.Attributes[GameAttribute.Dodge_Chance_Bonus] += ScriptFormula(18);
                Target.Attributes.BroadcastChangedIfRevealed();
            }
        }

        [ImplementsPowerBuff(7)]
        class ComboStage3Buff : PowerBuff
        {
            public override void Init()
            {
                Timeout = WaitSeconds(0.5f / EvalTag(PowerKeys.ComboAttackSpeed3));
            }
        }
    }
    #endregion

    //TODO Rune_A,C
    #region SevenSidedStrike
    [ImplementsPowerSNO(Skills.Skills.Monk.SpiritSpenders.SevenSidedStrike)]
    public class MonkSevenSidedStrike : Skill
    {
        //Max Teleport Distance added in last patch 8101.
        //attack delay end SF(16)
        //A: Max Tele Dist SF(19)
        //Total Time SF(2)
        //Impact Radius? SF(13)
        //C: 7,8,9
        public override IEnumerable<TickTimer> Main()
        {
            StartCooldown(EvalTag(PowerKeys.CooldownTime));
            UsePrimaryResource(EvalTag(PowerKeys.ResourceCost));

            var groundEffect = SpawnProxy(TargetPosition, WaitInfinite());
            groundEffect.PlayEffectGroup(145041);

            for (int n = 0; n < ScriptFormula(5); ++n)
            {
                IList<Actor> nearby = GetEnemiesInRadius(TargetPosition, ScriptFormula(6)).Actors;
                if (nearby.Count > 0)
                {
                    var target = nearby[Rand.Next(0, nearby.Count)];

                    SpawnEffect(99063, target.Position, -1);
                    yield return WaitSeconds(ScriptFormula(4));

                    if (Rune_E > 0)
                    {
                        target.PlayEffectGroup(99098);
                        var splashTargets = GetEnemiesInRadius(target.Position, ScriptFormula(14));
                        splashTargets.Actors.Remove(target); // don't hit target with splash
                        WeaponDamage(splashTargets, ScriptFormula(12), DamageType.Holy);
                    }

                    WeaponDamage(target, ScriptFormula(0), DamageType.Physical);
                }
                else
                {
                    break;
                }
            }

            groundEffect.Destroy();
        }
    }
    #endregion

    //TODO Slow, Rune_C,D,E
    #region CripplingWave
    [ImplementsPowerSNO(Skills.Skills.Monk.SpiritGenerator.CripplingWave)]
    public class MonkCripplingWave : ComboSkill
    {
        public override IEnumerable<TickTimer> Main()
        {
            int effectSNO;
            switch (ComboIndex)
            {
                case 0:
                    effectSNO = 18987;
                    break;
                case 1:
                    effectSNO = 18988;
                    break;
                case 2:
                    effectSNO = 96519;
                    break;
                default:
                    yield break;
            }

            User.PlayEffectGroup(effectSNO);

            bool hitAnything = false;
            AttackPayload attack = new AttackPayload(this);
            if (ComboIndex != 2)
                attack.Targets = GetEnemiesInArcDirection(User.Position, TargetPosition, ScriptFormula(5), ScriptFormula(6));
            else
                attack.Targets = GetEnemiesInRadius(User.Position, ScriptFormula(8));
            //dmg all enemies around you, why is there Angle SF(9) - EDIT: oh because it adds them together 180+180=360

            attack.AddWeaponDamage(ScriptFormula(0), DamageType.Physical);
            attack.OnHit = hitPayload =>
            {
                hitAnything = true;
                if (ComboIndex == 2)
                {
                    //Attack Slow Amount SF(20)
                    AddBuff(hitPayload.Target, new DebuffSlowed(ScriptFormula(4), WaitSeconds(ScriptFormula(3))));
                }
                if (Rune_D > 0)
                {
                    if (hitPayload.IsCriticalHit)
                    {
                        //buff SF(15[Duration]+16[Generate Amount])
                    }
                }
                if (Rune_E > 0)
                {
                    //buff[3]
                    //Debuff to monsters that causes them to take 80% additional damage from all attacks
                }
                if (Rune_C > 0)
                {
                    //buff[1]
                    //Debuff to monsters that caused them to inflict 50% less damage
                }
            };
            attack.Apply();

            if (hitAnything)
                GeneratePrimaryResource(EvalTag(PowerKeys.SpiritGained));

            yield break;
        }
    }
    #endregion

    //TODO Rune_E
    #region ExplodingPalm
    [ImplementsPowerSNO(Skills.Skills.Monk.SpiritGenerator.ExplodingPalm)]
    public class MonkExplodingPalm : ActionTimedSkill
    {
        public override IEnumerable<TickTimer> Main()
        {
            UsePrimaryResource(EvalTag(PowerKeys.ResourceCost));

            var target = GetBestMeleeEnemy();
            if (target.Actors.Count > 0)
            {
                AddBuff(target.Actors[0], new MainDebuff());
                if (Rune_C > 0)
                    AddBuff(target.Actors[0], new RuneCDebuff());
                else if (Rune_B > 0)
                    AddBuff(target.Actors[0],
                        // DebuffSlowed also lowers attack rate, not sure if it should only be movement.
                        new DebuffSlowed(ScriptFormula(18), WaitSeconds(ScriptFormula(3))));
            }

            yield break;
        }

        [ImplementsPowerBuff(0)]
        class MainDebuff : PowerBuff
        {
            const float _damageRate = 1.0f;
            TickTimer _damageTimer = null;

            public override void Init()
            {
                Timeout = WaitSeconds(ScriptFormula(3));
            }

            public override bool Apply()
            {
                if (!base.Apply())
                    return false;

                Target.Attributes[GameAttribute.Bleeding] = true;
                Target.Attributes.BroadcastChangedIfRevealed();
                return true;
            }

            public override void Remove()
            {
                base.Remove();

                Target.Attributes[GameAttribute.Bleeding] = false;
                Target.Attributes.BroadcastChangedIfRevealed();
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
                    attack.AddWeaponDamage(ScriptFormula(6) * _damageRate, DamageType.Physical);
                    attack.AutomaticHitEffects = false;
                    attack.Apply();
                }
                return false;
            }

            public override void OnPayload(Payload payload)
            {
                if (payload.Target == Target && payload is DeathPayload)
                {
                    AttackPayload attack = new AttackPayload(this);
                    attack.Targets = GetEnemiesInRadius(Target.Position, ScriptFormula(11));
                    attack.AddDamage(ScriptFormula(9) * Target.Attributes[GameAttribute.Hitpoints_Max_Total],
                                     ScriptFormula(10), DamageType.Physical);
                    if (Rune_D > 0)
                    {
                        attack.OnHit = (hitPayload) =>
                        {
                            GeneratePrimaryResource(ScriptFormula(14));
                        };
                    }
                    attack.Apply();

                    SpawnProxy(Target.Position).PlayEffectGroup(18991);
                }
            }
        }

        [ImplementsPowerBuff(1, true)]
        class RuneCDebuff : PowerBuff
        {
            public override void Init()
            {
                base.Init();
                Timeout = WaitSeconds(ScriptFormula(25));
                MaxStackCount = (int)ScriptFormula(17);
            }

            public override bool Apply()
            {
                if (!base.Apply())
                    return false;

                _AddAmp();
                return true;
            }

            public override void Remove()
            {
                base.Remove();
                Target.Attributes[GameAttribute.Amplify_Damage_Percent] -= StackCount * ScriptFormula(15);
                Target.Attributes.BroadcastChangedIfRevealed();
            }

            public override bool Stack(Buff buff)
            {
                bool stacked = StackCount < MaxStackCount;

                base.Stack(buff);

                if (stacked)
                    _AddAmp();

                return true;
            }

            private void _AddAmp()
            {
                Target.Attributes[GameAttribute.Amplify_Damage_Percent] += ScriptFormula(15);
                Target.Attributes.BroadcastChangedIfRevealed();
            }
        }
    }
    #endregion

    //TODO Rune_C,D (These happen at Max Stack Count
    #region SweepingWind
    [ImplementsPowerSNO(Skills.Skills.Monk.SpiritGenerator.SweepingWind)]
    public class MonkSweepingWind : ComboSkill
    {
        //Buff0: Spirit Per second
        public override IEnumerable<TickTimer> Main()
        {
            StartCooldown(EvalTag(PowerKeys.CooldownTime));
            UsePrimaryResource(EvalTag(PowerKeys.ResourceCost));

            switch (ComboIndex)
            {
                case 0:
                    AddBuff(User, new Stage1Sweep());
                    MeleeStageHit();
                    break;
                case 1:
                    AddBuff(User, new Stage2Sweep());
                    MeleeStageHit();
                    break;
                case 2:
                    AddBuff(User, new VortexBuff());

                    break;
            }

            yield break;
        }

        private void MeleeStageHit()
        {
            AttackPayload attack = new AttackPayload(this);
            attack.Targets = GetEnemiesInRadius(User.Position, ScriptFormula(1));
            attack.AddWeaponDamage(ScriptFormula(4), DamageType.Physical);
            attack.Apply();
        }
        [ImplementsPowerBuff(1)]
        class Stage1Sweep : PowerBuff
        {
            public override void Init()
            {
                Timeout = WaitSeconds(0.5f);
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
        class Stage2Sweep : PowerBuff
        {
            public override void Init()
            {
                Timeout = WaitSeconds(0.5f);
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
        //Rune_E included
        [ImplementsPowerBuff(3, true)]
        class VortexBuff : PowerBuff
        {
            const float _damageRate = 0.25f;
            TickTimer _damageTimer = null;
            float _damagetotal = 0;
            public override void Init()
            {
                Timeout = WaitSeconds(ScriptFormula(8));
                MaxStackCount = (int)ScriptFormula(10);
            }

            public override bool Apply()
            {
                if (!base.Apply())
                    return false;

                _AddAmp();
                return true;
            }

            public override void Remove()
            {
                base.Remove();
            }

            public override bool Update()
            {
                if (base.Update())
                    return true;

                if (_damageTimer == null || _damageTimer.TimedOut)
                {
                    _damageTimer = WaitSeconds(_damageRate);

                    AttackPayload attack = new AttackPayload(this);
                    attack.Targets = GetEnemiesInRadius(User.Position, ScriptFormula(13));
                    attack.AddWeaponDamage(_damagetotal, DamageType.Physical);
                    //we divide by four because this is by second, and tick-intervals = 0.25
                    attack.AutomaticHitEffects = false;
                    attack.Apply();
                }
                return false;
            }
            public override bool Stack(Buff buff)
            {
                bool stacked = StackCount < MaxStackCount;

                base.Stack(buff);

                if (stacked)
                    _AddAmp();

                return true;
            }

            private void _AddAmp()
            {
                _damagetotal += (ScriptFormula(6) / 4f);
            }
        }
    }
    #endregion

    //TODO:Rune_A(Dash), Rune_E(Stage3), and Stage 2  //exception on third stage ?
    #region WayOfTheHundredFists
    [ImplementsPowerSNO(Skills.Skills.Monk.SpiritGenerator.WayOfTheHundredFists)]
    public class MonkWayOfTheHundredFists : ComboSkill
    {
        public override IEnumerable<TickTimer> Main()
        {
            switch (TargetMessage.Field5)
            {
                case 0:
                    if (Rune_A > 0)
                    {
                        //Dash
                    }
                    else
                    {
                        MeleeStageHit();
                    }
                    break;
                case 1:
                    //stage 2 get hit chance bonus
                    MultiHit();
                    break;
                case 2:
                    if (Rune_D > 0)
                    {
                        if (Rand.NextDouble() < ScriptFormula(25))
                        {
                            GeneratePrimaryResource(ScriptFormula(24));
                        }
                    }

                    bool hitAnything = false;
                    AttackPayload attack = new AttackPayload(this);
                    attack.Targets = GetEnemiesInArcDirection(User.Position, TargetPosition, ScriptFormula(30), ScriptFormula(31));
                    attack.AddWeaponDamage(ScriptFormula(2), DamageType.Holy);
                    attack.OnHit = hitPayload =>
                    {
                        hitAnything = true;
                        Knockback(hitPayload.Target, ScriptFormula(5), ScriptFormula(6));
                        if (Rune_A > 0)
                        {
                            AddBuff(hitPayload.Target, new RuneA_DOT_100Fists());
                        }
                    };
                    attack.Apply();

                    if (hitAnything)
                    {
                        GeneratePrimaryResource(EvalTag(PowerKeys.SpiritGained));
                        if (Rune_C > 0)
                        {
                            AddBuff(User, new RuneCbuff());
                        }
                    }
                    //TODO: Range should be only 40f ahead.
                    if (Rune_E > 0)
                    {
                        var proj = new Projectile(this, 136022, User.Position);
                        proj.Launch(TargetPosition, ScriptFormula(23));
                        proj.OnCollision = (hit) =>
                        {
                            //proj.Destroy();
                            WeaponDamage(hit, ScriptFormula(11), DamageType.Physical);
                        };
                    }

                    break;
            }

            yield break;
        }
        public override float GetContactDelay()
        {
            return 0f;
        }
        private void MeleeStageHit()
        {
            if (Rune_D > 0)
            {
                if (Rand.NextDouble() < ScriptFormula(25))
                {
                    GeneratePrimaryResource(ScriptFormula(24));
                }
            }
            bool hitAnything = false;
            AttackPayload attack = new AttackPayload(this);
            attack.Targets = GetBestMeleeEnemy();
            attack.AddWeaponDamage(ScriptFormula(0), DamageType.Physical);
            attack.OnHit = hitPayload =>
            {
                hitAnything = true;
            };
            attack.Apply();

            if (hitAnything)
            {
                GeneratePrimaryResource(EvalTag(PowerKeys.SpiritGained));
                if (Rune_C > 0)
                {
                    AddBuff(User, new RuneCbuff());
                }
            }
        }
        private void MultiHit()
        {
            if (Rune_D > 0)
            {
                if (Rand.NextDouble() < ScriptFormula(25))
                {
                    GeneratePrimaryResource(ScriptFormula(24));
                }
            }
            //TODO: this needs to be redone when I figure out how to do multiple hits to certain MAX targets..
            bool hitAnything = false;
            AttackPayload attack = new AttackPayload(this);
            attack.Targets = GetEnemiesInArcDirection(User.Position, TargetPosition, ScriptFormula(28), ScriptFormula(29));
            attack.AddWeaponDamage(ScriptFormula(1), DamageType.Physical);
            attack.OnHit = hitPayload =>
            {
                hitAnything = true;
                if (Rune_A > 0)
                {
                    AddBuff(hitPayload.Target, new RuneA_DOT_100Fists());
                }
            };
            attack.Apply();

            if (hitAnything)
            {
                GeneratePrimaryResource(EvalTag(PowerKeys.SpiritGained));
                if (Rune_C > 0)
                {
                    AddBuff(User, new RuneCbuff());
                }
            }
        }
        [ImplementsPowerBuff(3)]
        class RuneA_DOT_100Fists : PowerBuff
        {
            const float _damageRate = 1f;
            TickTimer _damageTimer = null;

            public override void Init()
            {
                base.Init();
                Timeout = WaitSeconds(ScriptFormula(15));
            }

            public override bool Update()
            {
                if (base.Update())
                    return true;

                if (_damageTimer == null || _damageTimer.TimedOut)
                {
                    _damageTimer = WaitSeconds(_damageRate);

                    WeaponDamage(Target, ScriptFormula(16), DamageType.Holy);
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
        [ImplementsPowerBuff(1, true)]
        class RuneCbuff : PowerBuff
        {
            public override void Init()
            {
                base.Init();
                Timeout = WaitSeconds(ScriptFormula(10));
                MaxStackCount = (int)ScriptFormula(9);
            }

            public override bool Apply()
            {
                if (!base.Apply())
                    return false;

                _AddAmp();
                return true;
            }

            public override void Remove()
            {
                base.Remove();
                Target.Attributes[GameAttribute.Attacks_Per_Second_Bonus] -= StackCount * ScriptFormula(7);
                Target.Attributes[GameAttribute.Movement_Bonus_Run_Speed] -= StackCount * ScriptFormula(8);
                Target.Attributes.BroadcastChangedIfRevealed();
            }

            public override bool Stack(Buff buff)
            {
                bool stacked = StackCount < MaxStackCount;

                base.Stack(buff);

                if (stacked)
                    _AddAmp();

                return true;
            }

            private void _AddAmp()
            {
                Target.Attributes[GameAttribute.Attacks_Per_Second_Bonus] += ScriptFormula(7);
                Target.Attributes[GameAttribute.Movement_Bonus_Run_Speed] += ScriptFormula(8);
                Target.Attributes.BroadcastChangedIfRevealed();
            }
        }
    }
    #endregion

    //TODO:Rune_A,B,C
    #region DashingStrike
    [ImplementsPowerSNO(Skills.Skills.Monk.SpiritSpenders.DashingStrike)]
    public class MonkDashingStrike : Skill
    {
        public override IEnumerable<TickTimer> Main()
        {
            StartCooldown(EvalTag(PowerKeys.CooldownTime));
            UsePrimaryResource(EvalTag(PowerKeys.ResourceCost));

            // dashing strike never specifies the target's id so we just search for the closest target
            Target = GetEnemiesInRadius(TargetPosition, ScriptFormula(0)).GetClosestTo(TargetPosition);

            if (Target != null)
            {
                // put dash destination just beyond target
                TargetPosition = PowerMath.TranslateDirection2D(User.Position, Target.Position, Target.Position, ScriptFormula(2));
            }
            else
            {
                // if no target, always dash fixed amount
                TargetPosition = PowerMath.TranslateDirection2D(User.Position, TargetPosition, User.Position, ScriptFormula(7));
            }

            var dashBuff = new DashMoverBuff(TargetPosition);
            AddBuff(User, dashBuff);
            yield return dashBuff.Timeout;

            if (Target != null && Target.World != null) // target could've died or left world
            {
                User.TranslateFacing(Target.Position, true);
                yield return WaitSeconds(0.1f);
                User.PlayEffectGroup(113720);
                WeaponDamage(Target, ScriptFormula(1), DamageType.Physical);
                if (Rune_E > 0)
                {
                    if (Rand.NextDouble() < ScriptFormula(32))
                    {
                        AddBuff(Target, new DebuffStunned(WaitSeconds(ScriptFormula(33))));
                    }
                }
            }
            if (Rune_C > 0)
            {
                //Dodge Chance Buff
            }
            if (Rune_B > 0)
            {
                //Movement Speed Buff
            }
        }

        [ImplementsPowerBuff(0)]
        class DashMoverBuff : PowerBuff
        {
            private Vector3D _destination;
            private ActorMover _mover;

            public DashMoverBuff(Vector3D destination)
            {
                _destination = destination;
            }

            public override bool Apply()
            {
                if (!base.Apply())
                    return false;

                // dash speed seems to always be actor speed * 10
                float speed = Target.Attributes[GameAttribute.Running_Rate_Total] * ScriptFormula(5);

                Target.TranslateFacing(_destination, true);
                _mover = new ActorMover(Target);
                _mover.Move(_destination, speed, new ACDTranslateNormalMessage
                {
                    TurnImmediately = true,
                    Field5 = 0x9206, // alt: 0x920e, not sure what this param is for.
                    AnimationTag = 69808, // dashing strike attack animation
                    Field7 = 6, // ticks to wait before playing attack animation
                });

                // make sure buff timeout is big enough otherwise the client will sometimes ignore the visual effects.
                TickTimer minDashWait = WaitSeconds(0.15f);
                Timeout = minDashWait.TimeoutTick > _mover.ArrivalTime.TimeoutTick ? minDashWait : _mover.ArrivalTime;

                Target.Attributes[GameAttribute.Hidden] = true;
                Target.Attributes.BroadcastChangedIfRevealed();
                return true;
            }

            // (TODO) Update -> While moving head first, any enemy in path Gets slowed, RUNE_A

            public override void Remove()
            {
                base.Remove();
                Target.Attributes[GameAttribute.Hidden] = false;
                Target.Attributes.BroadcastChangedIfRevealed();
            }

            public override bool Update()
            {
                _mover.Update();
                return base.Update();
            }
        }
    }
    #endregion

    //TODO RuneA,E
    #region MantraOfEvasion
    [ImplementsPowerSNO(Skills.Skills.Monk.Mantras.MantraOfEvasion)]
    public class MonkMantraOfEvasion : Skill
    {
        public override IEnumerable<TickTimer> Main()
        {
            UsePrimaryResource(EvalTag(PowerKeys.ResourceCost));

            AddBuff(User, new CasterBuff());
            AddBuff(User, new CastBonusBuff());
            foreach (Actor ally in GetAlliesInRadius(User.Position, ScriptFormula(0)).Actors)
                AddBuff(User, new CastBonusBuff());

            yield break;
        }

        class BaseDodgeBuff : PowerBuff
        {
            public override bool Apply()
            {
                if (!base.Apply())
                    return false;

                Target.Attributes[GameAttribute.Dodge_Chance_Bonus] += ScriptFormula(2);
                Target.Attributes.BroadcastChangedIfRevealed();

                return true;
            }

            public override void Remove()
            {
                base.Remove();

                Target.Attributes[GameAttribute.Dodge_Chance_Bonus] -= ScriptFormula(2);
                Target.Attributes.BroadcastChangedIfRevealed();
            }
        }

        class BaseFullEffectsBuff : BaseDodgeBuff
        {
            // TODO: Rune_E, when you or ally is under 20% life, shield around target reducing damage by 80% for 10 seconds.
            // TODO: Rune_A, Dodging an enemy's attack, creates a burst of blames in raidus.

            public override bool Apply()
            {
                if (!base.Apply())
                    return false;
                if (Rune_B > 0)
                {
                    Target.Attributes[GameAttribute.CrowdControl_Reduction] += ScriptFormula(4);
                }
                if (Rune_C > 0)
                {
                    Target.Attributes[GameAttribute.Armor_Bonus_Percent] += ScriptFormula(5);
                }
                if (Rune_D > 0)
                {
                    Target.Attributes[GameAttribute.Running_Rate_Total] += ScriptFormula(6);
                }
                return true;
            }

            public override void Remove()
            {
                base.Remove();
                if (Rune_B > 0)
                {
                    Target.Attributes[GameAttribute.CrowdControl_Reduction] -= ScriptFormula(4);
                }
                if (Rune_C > 0)
                {
                    Target.Attributes[GameAttribute.Armor_Bonus_Percent] -= ScriptFormula(5);
                }
                if (Rune_D > 0)
                {
                    Target.Attributes[GameAttribute.Running_Rate_Total] += ScriptFormula(6);
                }
            }
        }

        [ImplementsPowerBuff(0)]
        class CasterBuff : BaseFullEffectsBuff
        {
            public override void Init()
            {
                Timeout = WaitSeconds(ScriptFormula(1));
            }

            public override void Remove()
            {
                base.Remove();

                // aura fade effect
                Target.PlayEffectGroup(199677);
            }

            public override bool Update()
            {
                if (base.Update())
                    return true;

                foreach (Actor ally in GetAlliesInRadius(Target.Position, ScriptFormula(0)).Actors)
                    AddBuff(ally, new AllyBuff());

                return false;
            }
        }

        [ImplementsPowerBuff(7)]
        class CastBonusBuff : BaseDodgeBuff
        {
            public override void Init()
            {
                Timeout = WaitSeconds(ScriptFormula(13));
            }
        }

        [ImplementsPowerBuff(1)]
        class AllyBuff : BaseFullEffectsBuff
        {
            public override void Init()
            {
                Timeout = WaitSeconds(ScriptFormula(11));
            }
        }
    }
    #endregion

    //TODO Rune_C
    #region BlindingFlash
    [ImplementsPowerSNO(Skills.Skills.Monk.SpiritSpenders.BlindingFlash)]
    public class MonkBlindingFlash : Skill
    {
        //Rune_D,B,A,E = done.
        //buff[3] mass_confused -> Rune_C
        public override IEnumerable<TickTimer> Main()
        {
            StartCooldown(EvalTag(PowerKeys.CooldownTime));
            UsePrimaryResource(EvalTag(PowerKeys.ResourceCost));

            var UsedPoint = SpawnProxy(User.Position);

            AttackPayload attack = new AttackPayload(this);
            attack.Targets = GetEnemiesInRadius(User.Position, ScriptFormula(1));
            attack.OnHit = (hit) =>
            {
                TickTimer waitBuffEnd = WaitSeconds(ScriptFormula(0));

                // add main effect buff only if blind debuff took effect
                if (AddBuff(hit.Target, new DebuffBlind(waitBuffEnd)))
                    AddBuff(hit.Target, new MainEffectBuff(waitBuffEnd));
                if (Rune_C > 0)
                {
                    //chance of Charm.
                }
            };
            attack.Apply();

            if (Rune_B > 0)
            {
                yield return WaitSeconds(ScriptFormula(11));
                UsedPoint.PlayEffectGroup(139497);
                AttackPayload attack2 = new AttackPayload(this);
                attack2.Targets = GetEnemiesInRadius(UsedPoint.Position, ScriptFormula(1));
                attack2.OnHit = (hit) =>
                {
                    TickTimer waitBuffEnd = WaitSeconds(ScriptFormula(13));

                    // add main effect buff only if blind debuff took effect
                    if (AddBuff(hit.Target, new DebuffBlind(waitBuffEnd)))
                        AddBuff(hit.Target, new MainEffectBuff(waitBuffEnd));
                    AddBuff(hit.Target, new IndigoDebuff(waitBuffEnd));

                };
                attack2.Apply();
            }

            yield break;
        }

        [ImplementsPowerBuff(5)]
        class MainEffectBuff : PowerBuff
        {
            public MainEffectBuff(TickTimer timeout)
            {
                Timeout = timeout;
            }

            public override bool Apply()
            {
                if (!base.Apply())
                    return false;

                Target.Attributes[GameAttribute.Hit_Chance] -= ScriptFormula(8);
                Target.Attributes.BroadcastChangedIfRevealed();
                return true;
            }

            public override void Remove()
            {
                base.Remove();
                Target.Attributes[GameAttribute.Hit_Chance] += ScriptFormula(8);
                Target.Attributes.BroadcastChangedIfRevealed();
                if (Rune_A > 0)
                {
                    //addbuff that adds holy damage
                    AddBuff(User, new FlashingRuneABuff(WaitSeconds(ScriptFormula(3))));
                }
            }
        }
        [ImplementsPowerBuff(1)]
        class FlashingRuneABuff : PowerBuff
        {
            public FlashingRuneABuff(TickTimer timeout)
            {
                Timeout = timeout;
            }

            public override bool Apply()
            {
                if (!base.Apply())
                    return false;

                Target.Attributes[GameAttribute.Attack_Bonus_Percent] += ScriptFormula(4);
                Target.Attributes.BroadcastChangedIfRevealed();
                return true;
            }

            public override void Remove()
            {
                base.Remove();
                Target.Attributes[GameAttribute.Attack_Bonus_Percent] -= ScriptFormula(4);
                Target.Attributes.BroadcastChangedIfRevealed();
            }
        }
        [ImplementsPowerBuff(4)]
        class IndigoDebuff : PowerBuff
        {
            public IndigoDebuff(TickTimer timeout)
            {
                Timeout = timeout;
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

    //Complete
    #region LashingTailKick
    [ImplementsPowerSNO(Skills.Skills.Monk.SpiritSpenders.LashingTailKick)]
    public class LashingTailKick : Skill
    {
        public override IEnumerable<TickTimer> Main()
        {
            StartCooldown(EvalTag(PowerKeys.CooldownTime));
            UsePrimaryResource(EvalTag(PowerKeys.ResourceCost));

            if (Rune_A > 0 || Rune_D > 0)
            {
                AttackPayload attack = new AttackPayload(this);
                attack.Targets = GetEnemiesInRadius(User.Position, 10f);
                attack.AddWeaponDamage(ScriptFormula(0), Rune_A > 0 ? DamageType.Fire : DamageType.Physical);
                attack.OnHit = HitPayload =>
                {
                    Knockback(HitPayload.Target, ScriptFormula(3), ScriptFormula(4), ScriptFormula(5));
                    if (Rune_D > 0)
                    {
                        AddBuff(HitPayload.Target, new DebuffSlowed(0.6f, WaitSeconds(ScriptFormula(15))));
                    }
                };
                attack.Apply();
            }
            else if (Rune_B > 0)
            {
                var proj = new Projectile(this, 136893, User.Position);
                //max distance sf(16)
                //proj.Position.Z += 5f;  // fix height
                proj.OnCollision = (hit) =>
                {
                    hit.PlayEffectGroup(143439);
                    WeaponDamage(hit, ScriptFormula(0), DamageType.Fire);
                };
                proj.Launch(TargetPosition, 1f);
            }
            else if (Rune_C > 0)
            {
                SpawnEffect(136925, TargetPosition);
                AttackPayload attack = new AttackPayload(this);
                attack.Targets = GetEnemiesInRadius(TargetPosition, ScriptFormula(12));
                attack.AddWeaponDamage(ScriptFormula(0), DamageType.Physical);
                attack.OnHit = HitPayload =>
                {
                    AddBuff(HitPayload.Target, new DebuffSlowed(ScriptFormula(18), WaitSeconds(ScriptFormula(21))));
                };
                attack.Apply();
            }
            else
            {
                AttackPayload attack = new AttackPayload(this);
                attack.Targets = GetEnemiesInArcDirection(User.Position, TargetPosition, ScriptFormula(6), ScriptFormula(7));
                attack.AddWeaponDamage(ScriptFormula(0), DamageType.Physical);
                attack.OnHit = HitPayload =>
                {
                    if (Rune_E > 0)
                    {
                        AddBuff(HitPayload.Target, new DebuffStunned(WaitSeconds(ScriptFormula(8))));
                    }
                    else
                        Knockback(HitPayload.Target, ScriptFormula(3), ScriptFormula(4), ScriptFormula(5));
                };
                attack.Apply();
            }

            yield break;
        }
    }
    #endregion

    //Close to complete for base skill. TODO:Runes
    #region TempestRush
    [ImplementsPowerSNO(Skills.Skills.Monk.SpiritSpenders.TempestRush)]
    public class TempestRush : ChanneledSkill
    {
        private Actor _target = null;

        public override void OnChannelOpen()
        {
            EffectsPerSecond = 0.25f;
            UsePrimaryResource(EvalTag(PowerKeys.ResourceCost));
            //User.Attributes[GameAttribute.Movement_Bonus_Run_Speed] += ScriptFormula(14);
            if (Rune_C > 0)
            {
                //damage reduction
            }
        }

        public override void OnChannelClose()
        {
            if (_target != null)
                _target.Destroy();
            //User.Attributes[GameAttribute.Movement_Bonus_Run_Speed] -= ScriptFormula(14);
            if (Rune_C > 0)
            {
                //damage reduction
            }
        }

        public override void OnChannelUpdated()
        {
            UsePrimaryResource(ScriptFormula(16) / 4f);
            User.TranslateFacing(TargetPosition);
            // client updates target actor position
        }

        public override IEnumerable<TickTimer> Main()
        {
            AddBuff(User, new TempestBuff());
            AttackPayload attack = new AttackPayload(this);
            //TODO: damage offset from ground?? where does this go..
            attack.Targets = GetEnemiesInArcDirection(User.Position, TargetPosition, ScriptFormula(2), ScriptFormula(1));
            attack.AddWeaponDamage(ScriptFormula(4), DamageType.Physical);
            attack.OnHit = HitPayload =>
            {
                Knockback(HitPayload.Target, ScriptFormula(6), ScriptFormula(7), ScriptFormula(8));
                AddBuff(HitPayload.Target, new DebuffSlowed(ScriptFormula(9), WaitSeconds(ScriptFormula(10))));
                if (Rune_A > 0)
                {
                }
            };
            attack.Apply();

            yield break;
        }
        [ImplementsPowerBuff(0)]
        class TempestBuff : PowerBuff
        {
            public override void Init()
            {
                base.Init();
                Timeout = WaitSeconds(0.5f);
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

    //TODO:Actors need adjustments.
    #region WaveOfLight
    [ImplementsPowerSNO(Skills.Skills.Monk.SpiritSpenders.WaveOfLight)]
    public class WaveOfLight : Skill
    {
        public override int GetCastEffectSNO()
        {
            return base.GetCastEffectSNO();
        }
        public override int GetContactEffectSNO()
        {
            return base.GetContactEffectSNO();
        }
        //From videos: it seems you proxy the Proxy actor first, the "hit" looks like it goes with the projectile...
        public override IEnumerable<TickTimer> Main()
        {
            StartCooldown(EvalTag(PowerKeys.CooldownTime));
            //UsePrimaryResource(EvalTag(PowerKeys.ResourceCost));

            //projectile distance (50)
            if (Rune_B > 0)
            {
                Vector3D[] projDestinations = PowerMath.GenerateSpreadPositions(User.Position, TargetPosition, 45f, (int)ScriptFormula(25));

                yield return WaitSeconds(0.35f);

                for (int i = 0; i < projDestinations.Length; i++)
                {
                    var proj = new Projectile(this, 172310, User.Position);
                    proj.Launch(projDestinations[i], ScriptFormula(22));
                    proj.OnCollision = (hit) =>
                    {
                        hit.PlayEffectGroup(145443);
                        proj.Destroy();
                        WeaponDamage(hit, ScriptFormula(23), DamageType.Holy);
                    };
                }
            }
            else if (Rune_C > 0)
            {
                //add in pillar [temple.efg and hit_hp.acr and pillar_a.acr?]
                Vector3D inFrontOfUser = PowerMath.TranslateDirection2D(User.Position, TargetPosition, User.Position, ScriptFormula(8));

                yield return WaitSeconds(0.35f);

                AttackPayload attack = new AttackPayload(this);
                attack.Targets = GetEnemiesInRadius(inFrontOfUser, 20f);
                attack.AddWeaponDamage(ScriptFormula(17), DamageType.Holy);
                attack.OnHit = hit =>
                {
                    Knockback(hit.Target, ScriptFormula(5), ScriptFormula(6), ScriptFormula(7));
                    AddBuff(hit.Target, new DOTbuff(WaitSeconds(ScriptFormula(9))));
                };

            }
            else
            {
                Vector3D inFrontOfUser = PowerMath.TranslateDirection2D(User.Position, TargetPosition, User.Position, ScriptFormula(8));
                AttackPayload attack = new AttackPayload(this);
                attack.Targets = GetEnemiesInRadius(inFrontOfUser, 10f);
                attack.AddWeaponDamage(ScriptFormula(14), DamageType.Holy);
                attack.OnHit = hit =>
                {
                    if (hit.IsCriticalHit)
                    {
                        AddBuff(hit.Target, new DebuffStunned(WaitSeconds(ScriptFormula(39))));
                    }
                };

                yield return WaitSeconds(0.35f);
                var proj = new Projectile(this, 6441, User.Position);
                proj.Position.Z += 5f;  // fix height
                proj.OnCollision = (hit) =>
                {
                    hit.PlayEffectGroup(144079);
                    WeaponDamage(hit, ScriptFormula(3), DamageType.Holy);
                    Knockback(User, ScriptFormula(15), ScriptFormula(18), ScriptFormula(20));
                };
                proj.Launch(TargetPosition, ScriptFormula(2));
            }
            yield break;
        }
        [ImplementsPowerBuff(4)]
        class DOTbuff : PowerBuff
        {
            const float _damageRate = 0.5f;
            TickTimer _damageTimer = null;

            public DOTbuff(TickTimer timeout)
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

                    WeaponDamage(GetEnemiesInRadius(Target.Position, 15f), ScriptFormula(31), DamageType.Holy);
                }

                return false;
            }
        }
    }
    #endregion

    //TODO:Healing
    #region BreathOfHeaven
    [ImplementsPowerSNO(Skills.Skills.Monk.SpiritSpenders.BreathOfHeaven)]
    public class BreathOfHeaven : Skill
    {
        public override IEnumerable<TickTimer> Main()
        {
            StartCooldown(EvalTag(PowerKeys.CooldownTime));
            UsePrimaryResource(EvalTag(PowerKeys.ResourceCost));

            AttackPayload attack = new AttackPayload(this);
            attack.Targets = GetEnemiesInRadius(User.Position, ScriptFormula(4));
            attack.AddWeaponDamage(ScriptFormula(0), DamageType.Holy);
            attack.OnHit = hit =>
            {
                if (Rune_C > 0)
                {
                    AddBuff(hit.Target, new FireDamageBuff(WaitSeconds(ScriptFormula(8))));
                }
                if (Rune_E > 0)
                {
                    AddBuff(hit.Target, new DebuffFeared(WaitSeconds(ScriptFormula(13))));
                    AddBuff(hit.Target, new FearBuff());
                }
            };
            if (Rune_D > 0)
            {
                AddBuff(User, new SpiritBuff(WaitSeconds(ScriptFormula(11))));
            }
            //Heals self for Heal Min and Heal Delta
            foreach (Actor ally in GetAlliesInRadius(User.Position, ScriptFormula(4)).Actors)
            {
                //heal ally for Heal Min and Heal Delta
            }
            yield break;
        }
        [ImplementsPowerBuff(0)]
        class FireDamageBuff : PowerBuff
        {
            //firedamage
            public FireDamageBuff(TickTimer timeout)
            {
                Timeout = timeout;
            }

            public override bool Apply()
            {
                if (!base.Apply())
                    return false;

                Target.Attributes[GameAttribute.Damage_Percent_All_From_Skills] += ScriptFormula(9);
                Target.Attributes.BroadcastChangedIfRevealed();
                return true;
            }

            public override void Remove()
            {
                base.Remove();
                Target.Attributes[GameAttribute.Damage_Percent_All_From_Skills] -= ScriptFormula(9);
                Target.Attributes.BroadcastChangedIfRevealed();
            }
        }
        [ImplementsPowerBuff(1)]
        class SpiritBuff : PowerBuff
        {
            //spirit
            public SpiritBuff(TickTimer timeout)
            {
                Timeout = timeout;
            }

            public override bool Apply()
            {
                if (!base.Apply())
                    return false;

                Target.Attributes[GameAttribute.Resource_On_Hit] += ScriptFormula(12);
                Target.Attributes.BroadcastChangedIfRevealed();
                return true;
            }

            public override void Remove()
            {
                base.Remove();
                Target.Attributes[GameAttribute.Resource_On_Hit] -= ScriptFormula(12);
                Target.Attributes.BroadcastChangedIfRevealed();
            }
        }
        [ImplementsPowerBuff(2)]
        class FearBuff : PowerBuff
        {
            //Fear
            const float _damageRate = 1f;
            TickTimer _damageTimer = null;

            public override void Init()
            {
                base.Init();
                Timeout = WaitSeconds(ScriptFormula(13));
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

                if (_damageTimer == null || _damageTimer.TimedOut)
                {
                    _damageTimer = WaitSeconds(_damageRate);

                    AttackPayload attack = new AttackPayload(this);
                    attack.Targets = GetEnemiesInRadius(User.Position, ScriptFormula(4));
                    attack.AddWeaponDamage(ScriptFormula(14), DamageType.Physical);
                    attack.Apply();
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

    //TODO:instant buff, after some seconds, better buff.
    //TODO:CastGroupBuff -> when Monk removes buff, remove from other members
    #region MantraOfRetribution
    [ImplementsPowerSNO(Skills.Skills.Monk.Mantras.MantraOfRetribution)]
    public class MantraOfRetribution : Skill
    {
        public override IEnumerable<TickTimer> Main()
        {
            //No more cooldown since latest patch 8101
            UsePrimaryResource(EvalTag(PowerKeys.ResourceCost));

            AddBuff(User, new CastEffect(WaitSeconds(ScriptFormula(5))));

            foreach (Actor ally in GetAlliesInRadius(User.Position, ScriptFormula(0)).Actors)
            {
                AddBuff(ally, new CastGroupBuff(WaitSeconds(ScriptFormula(5))));
            }
            yield break;
        }
        [ImplementsPowerBuff(0)]
        class CastEffect : PowerBuff
        {
            //masterFX
            public CastEffect(TickTimer timeout)
            {
                Timeout = timeout;
            }

            public override bool Apply()
            {
                if (!base.Apply())
                    return false;
                if (Rune_B > 0)
                {
                    Target.Attributes[GameAttribute.Attacks_Per_Second_Bonus] += ScriptFormula(22);
                }
                Target.Attributes[GameAttribute.Thorns_Percent] += ScriptFormula(6);
                Target.Attributes.BroadcastChangedIfRevealed();
                return true;
            }

            public override void OnPayload(Payload payload)
            {
                if (payload.Target == Target && payload is HitPayload)
                {
                    if (Rune_C > 0)
                    {
                        if (Rand.NextDouble() < ScriptFormula(17))
                        {
                            AddBuff(payload.Context.Target, new DebuffStunned(WaitSeconds(ScriptFormula(18))));
                        }
                    }
                    if (Rune_D > 0)
                    {
                        //says there's a chance, but no formula
                        if (Rand.NextDouble() < 0.3f + (0.1f * Rune_D))
                        {
                            GeneratePrimaryResource(ScriptFormula(23));
                        }
                    }
                    if (Rune_E > 0)
                    {
                        if (Rand.NextDouble() < ScriptFormula(10))
                        {
                            WeaponDamage(GetEnemiesInRadius(payload.Context.Target.Position, ScriptFormula(20)), ScriptFormula(14), DamageType.Holy);
                        }
                    }
                }
            }

            public override void Remove()
            {
                base.Remove();
                if (Rune_B > 0)
                {
                    Target.Attributes[GameAttribute.Attacks_Per_Second_Bonus] -= ScriptFormula(22);
                }
                Target.Attributes[GameAttribute.Thorns_Percent] -= ScriptFormula(6);
                Target.Attributes.BroadcastChangedIfRevealed();

            }
        }
        [ImplementsPowerBuff(1)]
        class CastGroupBuff : PowerBuff
        {
            //grantee
            public CastGroupBuff(TickTimer timeout)
            {
                Timeout = timeout;
            }

            public override bool Apply()
            {
                if (!base.Apply())
                    return false;
                if (Rune_B > 0)
                {
                    Target.Attributes[GameAttribute.Attacks_Per_Second_Bonus] += ScriptFormula(22);
                }
                Target.Attributes[GameAttribute.Thorns_Percent] += ScriptFormula(6);
                Target.Attributes.BroadcastChangedIfRevealed();
                return true;
            }

            public override void Remove()
            {
                base.Remove();
                if (Rune_B > 0)
                {
                    Target.Attributes[GameAttribute.Attacks_Per_Second_Bonus] -= ScriptFormula(22);
                }
                Target.Attributes[GameAttribute.Thorns_Percent] -= ScriptFormula(6);
                Target.Attributes.BroadcastChangedIfRevealed();
            }
        }
    }
    #endregion

    //TODO: instant buff, after some seconds, better buff.
    #region MantraOfHealing
    [ImplementsPowerSNO(Skills.Skills.Monk.Mantras.MantraOfHealing)]
    public class MantraOfHealing : Skill
    {
        public override IEnumerable<TickTimer> Main()
        {
            //No more cooldown
            UsePrimaryResource(EvalTag(PowerKeys.ResourceCost));

            AddBuff(User, new CastEffect(WaitSeconds(ScriptFormula(4) * 60f)));
            if (Rune_B > 0)
            {
                AddBuff(User, new HealingShield(WaitSeconds(ScriptFormula(9))));
            }
            foreach (Actor ally in GetAlliesInRadius(User.Position, ScriptFormula(0)).Actors)
            {
                AddBuff(ally, new CastGroupBuff(WaitSeconds(ScriptFormula(4) * 60f)));
                if (Rune_B > 0)
                {
                    AddBuff(User, new HealingShield(WaitSeconds(ScriptFormula(9))));
                }
            }
            yield break;
        }
        [ImplementsPowerBuff(0)]
        class CastEffect : PowerBuff
        {
            //grantor
            public CastEffect(TickTimer timeout)
            {
                Timeout = timeout;
            }

            public override bool Apply()
            {
                if (!base.Apply())
                    return false;
                if (Rune_C > 0)
                {
                    Target.Attributes[GameAttribute.Vitality_Bonus_Percent] += ScriptFormula(6);
                }
                if (Rune_D > 0)
                {
                    Target.Attributes[GameAttribute.Resource_Regen_Per_Second] += ScriptFormula(7);
                }
                if (Rune_E > 0)
                {
                    Target.Attributes[GameAttribute.Resistance_All] += ScriptFormula(8);
                }
                Target.Attributes[GameAttribute.Hitpoints_Regen_Per_Second] += ScriptFormula(1);
                Target.Attributes.BroadcastChangedIfRevealed();
                return true;
            }

            public override void Remove()
            {
                base.Remove();
                if (Rune_C > 0)
                {
                    Target.Attributes[GameAttribute.Vitality_Bonus_Percent] -= ScriptFormula(6);
                }
                if (Rune_D > 0)
                {
                    Target.Attributes[GameAttribute.Resource_Regen_Per_Second] -= ScriptFormula(7);
                }
                if (Rune_E > 0)
                {
                    Target.Attributes[GameAttribute.Resistance_All] -= ScriptFormula(8);
                }
                Target.Attributes[GameAttribute.Hitpoints_Regen_Per_Second] -= ScriptFormula(1);
                Target.Attributes.BroadcastChangedIfRevealed();

            }
        }
        [ImplementsPowerBuff(2)]
        class CastGroupBuff : PowerBuff
        {
            //grantee
            public CastGroupBuff(TickTimer timeout)
            {
                Timeout = timeout;
            }

            public override bool Apply()
            {
                if (!base.Apply())
                    return false;
                if (Rune_C > 0)
                {
                    Target.Attributes[GameAttribute.Vitality_Bonus_Percent] += ScriptFormula(6);
                }
                if (Rune_D > 0)
                {
                    Target.Attributes[GameAttribute.Resource_Regen_Per_Second] += ScriptFormula(7);
                }
                if (Rune_E > 0)
                {
                    Target.Attributes[GameAttribute.Resistance_All] += ScriptFormula(8);
                }
                Target.Attributes[GameAttribute.Hitpoints_Regen_Per_Second] += ScriptFormula(1);
                Target.Attributes.BroadcastChangedIfRevealed();
                return true;
            }

            public override void Remove()
            {
                base.Remove();
                if (Rune_C > 0)
                {
                    Target.Attributes[GameAttribute.Vitality_Bonus_Percent] -= ScriptFormula(6);
                }
                if (Rune_D > 0)
                {
                    Target.Attributes[GameAttribute.Resource_Regen_Per_Second] -= ScriptFormula(7);
                }
                if (Rune_E > 0)
                {
                    Target.Attributes[GameAttribute.Resistance_All] -= ScriptFormula(8);
                }
                Target.Attributes[GameAttribute.Hitpoints_Regen_Per_Second] -= ScriptFormula(1);
                Target.Attributes.BroadcastChangedIfRevealed();
            }
        }
        [ImplementsPowerBuff(3)]
        class HealingShield : PowerBuff
        {
            //holyAuraRune_shield.efg
            public HealingShield(TickTimer timeout)
            {
                Timeout = timeout;
            }

            public override bool Apply()
            {
                if (!base.Apply())
                    return false;

                Target.Attributes[GameAttribute.Damage_Absorb_Percent] += ScriptFormula(5);
                Target.Attributes.BroadcastChangedIfRevealed();
                return true;
            }

            public override void Remove()
            {
                base.Remove();
                Target.Attributes[GameAttribute.Damage_Absorb_Percent] -= ScriptFormula(5);
                Target.Attributes.BroadcastChangedIfRevealed();
            }
        }
    }
    #endregion

    //TODO:OnPayload for Rune_D?
    //TODO: maybe while(enemy is in radius), keep the buff?
    #region MantraOfConviction
    [ImplementsPowerSNO(Skills.Skills.Monk.Mantras.MantraOfConviction)]
    public class MantraOfConviction : Skill
    {
        public override IEnumerable<TickTimer> Main()
        {
            //No more cooldown
            UsePrimaryResource(EvalTag(PowerKeys.ResourceCost));


            AddBuff(User, new ConvictionAura(WaitSeconds(ScriptFormula(0) * 60f)));

            if (Rune_D > 0)
            {
                foreach (Actor ally in GetAlliesInRadius(User.Position, ScriptFormula(0)).Actors)
                {
                    AddBuff(ally, new ReclamationAura(WaitSeconds(ScriptFormula(0) * 60f)));
                }
            }
            yield break;
        }
        [ImplementsPowerBuff(0)]
        class ConvictionAura : PowerBuff
        {
            //AuraBuff

            const float _damageRate = 0.5f;
            TickTimer _damageTimer = null;

            public ConvictionAura(TickTimer timeout)
            {
                Timeout = timeout;
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

                if (_damageTimer == null || _damageTimer.TimedOut)
                {
                    _damageTimer = WaitSeconds(_damageRate);

                    foreach (Actor Enemy in GetEnemiesInRadius(User.Position, ScriptFormula(5)).Actors)
                    {
                        AddBuff(Enemy, new DeBuff());
                        if (Rune_C > 0)
                        {
                            AddBuff(Enemy, new DebuffSlowed(ScriptFormula(7), WaitSeconds(0.5f)));
                        }
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
        class DeBuff : PowerBuff
        {
            //Debuff

            const float _damageRate = 1f;
            TickTimer _damageTimer = null;

            public override void Init()
            {
                Timeout = WaitSeconds(ScriptFormula(3));
            }

            public override bool Apply()
            {
                if (!base.Apply())
                    return false;
                if (Rune_E > 0)
                {
                    Target.Attributes[GameAttribute.Damage_Done_Reduction_Percent] += ScriptFormula(10);
                }
                Target.Attributes[GameAttribute.Defense_Reduction_Percent] += ScriptFormula(2);
                Target.Attributes.BroadcastChangedIfRevealed();
                return true;
            }
            public override bool Update()
            {
                if (base.Update())
                    return true;

                if (Rune_B > 0)
                {
                    if (_damageTimer == null || _damageTimer.TimedOut)
                    {
                        _damageTimer = WaitSeconds(_damageRate);

                        WeaponDamage(Target, ScriptFormula(12), DamageType.Holy);

                    }
                }

                return false;
            }
            public override void Remove()
            {
                base.Remove();
                if (Rune_E > 0)
                {
                    Target.Attributes[GameAttribute.Damage_Done_Reduction_Percent] -= ScriptFormula(10);
                }
                Target.Attributes[GameAttribute.Defense_Reduction_Percent] -= ScriptFormula(2);
                Target.Attributes.BroadcastChangedIfRevealed();
            }
        }
        [ImplementsPowerBuff(0)]
        class ReclamationAura : PowerBuff
        {
            //AuraBuff
            public ReclamationAura(TickTimer timeout)
            {
                Timeout = timeout;
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

    //Rune_A -> Healing
    //Rune_E -> OnPayload HitTarget collect all damage that would have been done to you, Remove() of Buff, grab that number and get (SF(4)) of damage.
    //          Deal that much damage (Max of ___  % of your max life)
    #region Serenity
    [ImplementsPowerSNO(Skills.Skills.Monk.SpiritSpenders.Serenity)]
    public class Serenity : Skill
    {
        public override IEnumerable<TickTimer> Main()
        {
            StartCooldown(EvalTag(PowerKeys.CooldownTime));
            UsePrimaryResource(EvalTag(PowerKeys.ResourceCost));

            AddBuff(User, new SerenityBuff(WaitSeconds(ScriptFormula(0))));

            if (Rune_D > 0)
            {
                foreach (Actor ally in GetAlliesInRadius(User.Position, ScriptFormula(12)).Actors)
                {
                    AddBuff(ally, new SerenityAlliesBuff(WaitSeconds(ScriptFormula(11))));
                }
            }
            yield break;
        }
        [ImplementsPowerBuff(0)]
        class SerenityBuff : PowerBuff
        {
            public SerenityBuff(TickTimer timeout)
            {
                Timeout = timeout;
            }

            public override bool Apply()
            {
                if (!base.Apply())
                    return false;

                //If(Rune_A) -> Heal ScriptFormula(1)
                if (Rune_B > 0)
                {
                    Target.Attributes[GameAttribute.Projectile_Reflect_Chance] += ScriptFormula(2);
                    Target.Attributes[GameAttribute.Thorns_Percent] += ScriptFormula(2);
                }

                Target.Attributes[GameAttribute.Gethit_Immune] = true;
                Target.Attributes[GameAttribute.Immunity] = true;
                Target.Attributes.BroadcastChangedIfRevealed();
                return true;
            }

            public override void Remove()
            {
                base.Remove();
                if (Rune_B > 0)
                {
                    Target.Attributes[GameAttribute.Projectile_Reflect_Chance] -= ScriptFormula(2);
                    Target.Attributes[GameAttribute.Thorns_Percent] -= ScriptFormula(2);
                }
                Target.Attributes[GameAttribute.Gethit_Immune] = false;
                Target.Attributes[GameAttribute.Immunity] = false;
                Target.Attributes.BroadcastChangedIfRevealed();
            }
        }
        [ImplementsPowerBuff(1)]
        class SerenityAlliesBuff : PowerBuff
        {
            public SerenityAlliesBuff(TickTimer timeout)
            {
                Timeout = timeout;
            }

            public override bool Apply()
            {
                if (!base.Apply())
                    return false;

                Target.Attributes[GameAttribute.Gethit_Immune] = true;
                Target.Attributes[GameAttribute.Immunity] = true;
                Target.Attributes.BroadcastChangedIfRevealed();
                return true;
            }

            public override void Remove()
            {
                base.Remove();
                Target.Attributes[GameAttribute.Gethit_Immune] = false;
                Target.Attributes[GameAttribute.Immunity] = false;
                Target.Attributes.BroadcastChangedIfRevealed();
            }
        }
    }
    #endregion

    //TODO: Make sure enemies cannot come back into bubble
    #region InnerSanctuary
    [ImplementsPowerSNO(Skills.Skills.Monk.SpiritSpenders.InnerSanctuary)]
    public class InnerSanctuary : Skill
    {
        public override IEnumerable<TickTimer> Main()
        {
            StartCooldown(EvalTag(PowerKeys.CooldownTime));
            UsePrimaryResource(EvalTag(PowerKeys.ResourceCost));

            var GroundSpot = SpawnProxy(User.Position);
            var Sanctuary = SpawnEffect(RuneSelect(98557, 98823, 149848, 142312, 98559, 142305), GroundSpot.Position, 0, WaitSeconds(ScriptFormula(0)));
            AttackPayload attack = new AttackPayload(this);
            attack.Targets = GetEnemiesInRadius(GroundSpot.Position, ScriptFormula(1));
            attack.OnHit = hit =>
            {
                if (Rune_A > 0)
                {
                    WeaponDamage(hit.Target, ScriptFormula(10), DamageType.Holy);
                }
                AddBuff(hit.Target, new DebuffFeared(WaitSeconds(ScriptFormula(2))));
                Knockback(hit.Target, ScriptFormula(3), ScriptFormula(4), ScriptFormula(5));
            };
            attack.Apply();
            if (Rune_D > 0 || Rune_C > 0)
            {
                Sanctuary.UpdateDelay = 0.3f;
                Sanctuary.OnUpdate = () =>
                {
                    AddBuff(User, new RegenBuff());
                    foreach (Actor ally in GetAlliesInRadius(GroundSpot.Position, ScriptFormula(1)).Actors)
                    {
                        AddBuff(User, new RegenAllyBuff());
                    }
                };
            }
            //outer proxy
            SpawnEffect(RuneSelect(142719, 142851, 149849, 142788, 142737, 142845), GroundSpot.Position, 0, WaitSeconds(ScriptFormula(7)));

            if (Rune_E > 0)
            {
                yield return WaitSeconds(ScriptFormula(0));
                var PreSanctified = SpawnEffect(149851, GroundSpot.Position, 0, WaitSeconds(ScriptFormula(31)));
                PreSanctified.UpdateDelay = 0.3f;
                PreSanctified.OnUpdate = () =>
                {
                    foreach (Actor enemy in GetEnemiesInRadius(GroundSpot.Position, ScriptFormula(1)).Actors)
                    {
                        AddBuff(enemy, new DebuffSlowed(ScriptFormula(30), WaitSeconds(ScriptFormula(31))));
                    }
                };
            }

            yield break;
        }
        [ImplementsPowerBuff(0)]
        class RegenBuff : PowerBuff
        {
            public override void Init()
            {
                base.Init();
                Timeout = WaitSeconds(ScriptFormula(13));
            }

            public override bool Apply()
            {
                if (!base.Apply())
                    return false;
                if (Rune_C > 0)
                {
                    Target.Attributes[GameAttribute.Defense_Bonus_Percent] += ScriptFormula(20);
                }
                if (Rune_D > 0)
                {
                    Target.Attributes[GameAttribute.Hitpoints_Regen_Per_Second] += ScriptFormula(25);
                }
                Target.Attributes.BroadcastChangedIfRevealed();
                return true;
            }

            public override void Remove()
            {
                base.Remove();
                if (Rune_C > 0)
                {
                    Target.Attributes[GameAttribute.Defense_Bonus_Percent] -= ScriptFormula(20);
                }
                if (Rune_D > 0)
                {
                    Target.Attributes[GameAttribute.Hitpoints_Regen_Per_Second] -= ScriptFormula(25);
                }
                Target.Attributes.BroadcastChangedIfRevealed();
            }
        }
        [ImplementsPowerBuff(1)]
        class RegenAllyBuff : PowerBuff
        {
            public override void Init()
            {
                base.Init();
                Timeout = WaitSeconds(ScriptFormula(13));
            }

            public override bool Apply()
            {
                if (!base.Apply())
                    return false;
                if (Rune_C > 0)
                {
                    Target.Attributes[GameAttribute.Defense_Bonus_Percent] += ScriptFormula(20);
                }
                if (Rune_D > 0)
                {
                    Target.Attributes[GameAttribute.Hitpoints_Regen_Per_Second] += ScriptFormula(25);
                }
                Target.Attributes.BroadcastChangedIfRevealed();

                return true;
            }

            public override void Remove()
            {
                base.Remove();
                if (Rune_C > 0)
                {
                    Target.Attributes[GameAttribute.Defense_Bonus_Percent] -= ScriptFormula(20);
                }
                if (Rune_D > 0)
                {
                    Target.Attributes[GameAttribute.Hitpoints_Regen_Per_Second] -= ScriptFormula(25);
                }
                Target.Attributes.BroadcastChangedIfRevealed();
            }
        }
    }
    #endregion

    //Pet Class
    #region MysticAlly
    [ImplementsPowerSNO(Skills.Skills.Monk.SpiritSpenders.MysticAlly)]
    public class MysticAlly : Skill
    {
        public override IEnumerable<TickTimer> Main()
        {
            UsePrimaryResource(EvalTag(PowerKeys.ResourceCost));
            var mystic = new MysticAllyMinion(this.World, this, 0);
            mystic.Brain.DeActivate();
            mystic.Position = RandomDirection(User.Position, 3f, 8f); //Kind of hacky until we get proper collisiondetection
            mystic.Attributes[GameAttribute.Untargetable] = true;
            mystic.EnterWorld(mystic.Position);
            mystic.PlayActionAnimation(130606);

            yield return WaitSeconds(0.8f);
            (mystic as Minion).Brain.Activate();
            mystic.Attributes[GameAttribute.Untargetable] = false;
            mystic.Attributes.BroadcastChangedIfRevealed();

            yield break;
        }
    }
    #endregion

    //Complete
    #region CycloneStrike
    [ImplementsPowerSNO(223473)]
    public class CycloneStrike : Skill
    {
        public override IEnumerable<TickTimer> Main()
        {
            StartCooldown(EvalTag(PowerKeys.CooldownTime));
            UsePrimaryResource(EvalTag(PowerKeys.ResourceCost));
            if (Rune_C > 0)
            {
                foreach (Actor Ally in GetAlliesInRadius(User.Position, ScriptFormula(19)).Actors)
                {
                    Ally.Attributes[GameAttribute.Hitpoints_Granted] += ScriptFormula(20);
                }
            }
            AttackPayload attack = new AttackPayload(this);
            attack.Targets = GetEnemiesInRadius(User.Position, ScriptFormula(2), (int)ScriptFormula(5));
            attack.OnHit = hit =>
            {
                Knockback(hit.Target, -25f, ScriptFormula(1), ScriptFormula(29));
            };
            attack.Apply();
            yield return WaitSeconds(0.5f);
            User.PlayEffectGroup(224247);
            WeaponDamage(GetEnemiesInRadius(User.Position, ScriptFormula(16) + ScriptFormula(17)), ScriptFormula(10), Rune_A > 0 ? DamageType.Fire : DamageType.Holy);
            if (Rune_E > 0)
            {
                AddBuff(User, new CycloneDodgeBuff(WaitSeconds(ScriptFormula(27))));
            }
            if (Rune_A > 0)
            {
                foreach (Actor Enemy in GetEnemiesInRadius(User.Position, ScriptFormula(2), (int)ScriptFormula(5)).Actors)
                {
                    AddBuff(Enemy, new DebuffFeared(WaitSeconds(ScriptFormula(13))));
                }
            }
            yield break;
        }
        [ImplementsPowerBuff(0)]
        class CycloneDodgeBuff : PowerBuff
        {
            public CycloneDodgeBuff(TickTimer timeout)
            {
                Timeout = timeout;
            }

            public override bool Apply()
            {
                if (!base.Apply())
                    return false;
                Target.Attributes[GameAttribute.Dodge_Chance_Bonus] += ScriptFormula(28);
                Target.Attributes.BroadcastChangedIfRevealed();
                return true;
            }

            public override void Remove()
            {
                base.Remove();
                Target.Attributes[GameAttribute.Dodge_Chance_Bonus] -= ScriptFormula(28);
                Target.Attributes.BroadcastChangedIfRevealed();
            }
        }
    }
    #endregion
    //11 Passives
}
