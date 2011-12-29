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
using System.Linq;
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

namespace Mooege.Core.GS.Powers.Implementations
{
    [ImplementsPowerSNO(Skills.Skills.Monk.SpiritGenerator.DeadlyReach)]
    public class MonkDeadlyReach : ComboSkill
    {
        public override IEnumerable<TickTimer> Main()
        {
            float reachLength;
            float reachThickness;

            switch(ComboIndex)
            {
                case 0:
                    reachLength = 13;
                    reachThickness = 3f;
                    break;
                case 1:
                    reachLength = 14;
                    reachThickness = 4.5f;
                    break;
                case 2:
                    reachLength = 18;
                    reachThickness = 4.5f;
                    break;
                default:
                    yield break;
            }

            bool hitAnything = false;
            AttackPayload attack = new AttackPayload(this);
            attack.Targets = GetEnemiesInBeamDirection(User.Position, TargetPosition, reachLength, reachThickness);
            attack.AddWeaponDamage(1.20f, DamageType.Physical);
            attack.OnHit = hitPayload => { hitAnything = true; };
            attack.Apply();

            if (hitAnything)
                GeneratePrimaryResource(6f);

            yield break;
        }
    }

    [ImplementsPowerSNO(Skills.Skills.Monk.SpiritGenerator.FistsOfThunder)]
    public class MonkFistsOfThunder : ComboSkill
    {
        public override IEnumerable<TickTimer> Main()
        {
            switch (TargetMessage.Field5)
            {
                case 0:
                case 1:
                    MeleeStageHit();
                    break;
                case 2:
                    //AddBuff(User, new ComboStage3Buff());

                    // put target position a little bit in front of the monk. represents the lightning ball
                    TargetPosition = PowerMath.TranslateDirection2D(User.Position, TargetPosition,
                                        User.Position, 8f);

                    bool hitAnything = false;
                    AttackPayload attack = new AttackPayload(this);
                    attack.Targets = GetEnemiesInRadius(TargetPosition, 7f);
                    attack.AddWeaponDamage(1.20f, DamageType.Lightning);
                    attack.OnHit = hitPayload => {
                        hitAnything = true;
                        Knockback(hitPayload.Target, 12f);
                    };
                    attack.Apply();

                    if (hitAnything)
                        GeneratePrimaryResource(6f);

                    break;
            }

            yield break;
        }

        private void MeleeStageHit()
        {
            AttackPayload attack = new AttackPayload(this);
            attack.Targets = GetBestMeleeEnemy();
            attack.AddWeaponDamage(1.20f, DamageType.Lightning);
            attack.OnHit = hitPayload =>
            {
                GeneratePrimaryResource(6f);
            };
            attack.Apply();
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

    [ImplementsPowerSNO(Skills.Skills.Monk.SpiritSpenders.SevenSidedStrike)]
    public class MonkSevenSidedStrike : Skill
    {
        public override IEnumerable<TickTimer> Main()
        {
            //UsePrimaryResource(50f);
            //StartCooldown(WaitSeconds(30f));
            
            var groundEffect = SpawnProxy(TargetPosition, WaitInfinite());
            groundEffect.PlayEffectGroup(145041);

            for (int n = 0; n < 7; ++n)
            {
                IList<Actor> nearby = GetEnemiesInRadius(TargetPosition, 25f).Actors;
                if (nearby.Count > 0)
                {
                    var target = nearby[Rand.Next(0, nearby.Count)];

                    SpawnEffect(99063, target.Position, -1);                    
                    yield return WaitSeconds(0.2f);

                    if (Rune_E > 0)
                    {
                        target.PlayEffectGroup(99098);
                        var splashTargets = GetEnemiesInRadius(target.Position, 5f);
                        splashTargets.Actors.Remove(target); // don't hit target with splash
                        WeaponDamage(splashTargets, 0.31f, DamageType.Holy);
                    }

                    WeaponDamage(target, 1.15f, DamageType.Physical);
                }
                else
                {
                    break;
                }
            }

            groundEffect.Destroy();
        }
    }

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

            attack.AddWeaponDamage(1.10f, DamageType.Physical);
            attack.OnHit = hitPayload => { hitAnything = true; };
            attack.Apply();

            if (hitAnything)
                GeneratePrimaryResource(6f);

            yield break;
        }
    }

    [ImplementsPowerSNO(Skills.Skills.Monk.SpiritGenerator.ExplodingPalm)]
    public class MonkExplodingPalm : ComboSkill
    {
        public override IEnumerable<TickTimer> Main()
        {
            AttackPayload attack = new AttackPayload(this);
            switch (ComboIndex)
            {
                case 0:
                case 1:
                    attack.Targets = GetBestMeleeEnemy();
                    attack.AddWeaponDamage(ScriptFormula(0), DamageType.Physical);
                    if (Rune_C > 0)
                        attack.AddBuffOnHit<RuneCDebuff>();
                    break;

                case 2:
                    if (Rune_B > 0)
                    {
                        attack.Targets = GetEnemiesInArcDirection(User.Position, TargetPosition, ScriptFormula(19), ScriptFormula(20));
                        int maxTargets = (int)ScriptFormula(18);
                        if (maxTargets < attack.Targets.Actors.Count)
                            attack.Targets.Actors.RemoveRange(maxTargets, attack.Targets.Actors.Count - maxTargets);
                    }
                    else
                    {
                        attack.Targets = GetBestMeleeEnemy();
                    }

                    attack.AutomaticHitEffects = false;
                    attack.AddBuffOnHit<MainDebuff>();
                    break;

                default:
                    yield break;
            }
            bool hitAnything = false;
            attack.OnHit = hitPayload => { hitAnything = true; };
            attack.Apply();

            if (hitAnything)
                GeneratePrimaryResource(6f);
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

    [ImplementsPowerSNO(Skills.Skills.Monk.SpiritGenerator.SweepingWind)]
    public class MonkSweepingWind : ComboSkill
    {
        public override IEnumerable<TickTimer> Main()
        {
            AttackPayload attack = new AttackPayload(this);
            attack.Targets = GetBestMeleeEnemy();
            attack.AddWeaponDamage(1.00f, DamageType.Physical);
            attack.Apply();

            yield break;
        }
    }

    [ImplementsPowerSNO(Skills.Skills.Monk.SpiritGenerator.WayOfTheHundredFists)]
    public class MonkWayOfTheHundredFists : ComboSkill
    {
        public override IEnumerable<TickTimer> Main()
        {
            yield break;
        }

        public override float GetContactDelay()
        {
            // no contact delay for hundred fists
            return 0f;
        }
    }

    [ImplementsPowerSNO(Skills.Skills.Monk.SpiritSpenders.DashingStrike)]
    public class MonkDashingStrike : Skill
    {
        public override IEnumerable<TickTimer> Main()
        {
            //UsePrimaryResource(15f);

            // dashing strike never specifies the target's id so we just search for the closest target
            Target = GetEnemiesInRadius(TargetPosition, 8f).GetClosestTo(TargetPosition);

            if (Target != null)
            {
                // put dash destination just beyond target
                TargetPosition = PowerMath.TranslateDirection2D(User.Position, Target.Position, Target.Position, 7f);
            }
            else
            {
                // if no target, always dash fixed amount
                TargetPosition = PowerMath.TranslateDirection2D(User.Position, TargetPosition, User.Position, 13f);
            }

            var dashBuff = new DashMoverBuff(TargetPosition);
            AddBuff(User, dashBuff);
            yield return dashBuff.Timeout;

            if (Target != null && Target.World != null) // target could've died or left world
            {
                User.TranslateFacing(Target.Position, true);
                yield return WaitSeconds(0.1f);
                User.PlayEffectGroup(113720);
                WeaponDamage(Target, 1.60f, DamageType.Physical);
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
                float speed = Target.Attributes[GameAttribute.Running_Rate_Total] * 10f;

                Target.TranslateFacing(_destination, true);
                _mover = new ActorMover(Target);
                _mover.Move(_destination, speed, new NotifyActorMovementMessage
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

    [ImplementsPowerSNO(Skills.Skills.Monk.Mantras.MantraOfEvasion)]
    public class MonkMantraOfEvasion : Skill
    {
        public override IEnumerable<TickTimer> Main()
        {
            StartDefaultCooldown();

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
            // TODO: rune buff effects and such will go here

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

    [ImplementsPowerSNO(Skills.Skills.Monk.SpiritSpenders.BlindingFlash)]
    public class MonkBlindingFlash : Skill
    {
        public override IEnumerable<TickTimer> Main()
        {
            StartDefaultCooldown();
            UsePrimaryResource(EvalTag(PowerKeys.ResourceCost));

            AttackPayload attack = new AttackPayload(this);
            attack.Targets = GetEnemiesInRadius(User.Position, ScriptFormula(1));
            attack.OnHit = (hit) =>
            {
                TickTimer waitBuffEnd = WaitSeconds(ScriptFormula(0));

                // add main effect buff only if blind debuff took effect
                if (AddBuff(hit.Target, new DebuffBlind(waitBuffEnd)))
                    AddBuff(hit.Target, new MainEffectBuff(waitBuffEnd));
            };

            attack.Apply();

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
            }
        }
    }
}
