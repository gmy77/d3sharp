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
    public class MonkDeadlyReach : PowerScript
    {
        public override IEnumerable<TickTimer> Run()
        {
            int effectSNO;
            float reachLength;
            float reachThickness;

            switch(TargetMessage.Field5)
            {
                case 0:
                    yield return WaitSeconds(0.1f);
                    effectSNO = 140870;
                    reachLength = 13;
                    reachThickness = 3f;
                    break;
                case 1:
                    effectSNO = 140871;
                    reachLength = 14;
                    reachThickness = 4.5f;
                    break;
                case 2:
                    yield return WaitSeconds(0.3f);
                    effectSNO = 140872;
                    reachLength = 18;
                    reachThickness = 4.5f;
                    break;
                default:
                    yield break;
            }

            // calculate end of attack reach
            TargetPosition = PowerMath.ProjectAndTranslate2D(User.Position, TargetPosition,
                                                   User.Position, reachLength);

            User.PlayEffectGroup(effectSNO);

            bool hitAnything = false;
            foreach (Actor actor in GetEnemiesInRadius(User.Position, reachLength + 10f))
            {
                if (PowerMath.PointInBeam(actor.Position, User.Position, TargetPosition, reachThickness))
                {
                    hitAnything = true;
                    WeaponDamage(actor, 1.20f, DamageType.Physical);
                }
            }

            if (hitAnything)
                GeneratePrimaryResource(6f);

            yield break;
        }
    }

    [ImplementsPowerSNO(Skills.Skills.Monk.SpiritGenerator.FistsOfThunder)]
    public class MonkFistsOfThunder : PowerScript
    {
        float waitCombo(int combo)
        {
            switch (combo)
            {
                case 0: return 0.5f / EvalTag(PowerKeys.ComboAttackSpeed1);
                case 1: return 0.5f / EvalTag(PowerKeys.ComboAttackSpeed2);
                case 2: return 0.5f / EvalTag(PowerKeys.ComboAttackSpeed3);
                default: return 5.0f;
            }
        }

        public override IEnumerable<TickTimer> Run()
        {
            switch (TargetMessage.Field5)
            {
                case 0:
                    yield return WaitSeconds(waitCombo(0));
                    User.PlayEffectGroup(143516);
                    MeleeStageHit();
                    break;
                case 1:
                    yield return WaitSeconds(waitCombo(1));
                    User.PlayEffectGroup(143516);
                    MeleeStageHit();
                    break;
                case 2:
                    AddBuff(User, new ComboStage3Buff());
                    yield return WaitSeconds(waitCombo(2));
                    User.PlayEffectGroup(143518);

                    // put target position a little bit in front of the monk. represents the lightning ball
                    TargetPosition = PowerMath.ProjectAndTranslate2D(User.Position, TargetPosition,
                                        User.Position, 8f);

                    bool hitAnything = false;
                    foreach (Actor actor in GetEnemiesInRadius(TargetPosition, 7f))
                    {
                        hitAnything = true;
                        Knockback(actor, 4f);
                        WeaponDamage(actor, 1.20f, DamageType.Lightning);
                    }

                    if (hitAnything)
                        GeneratePrimaryResource(6f);

                    break;
            }

            yield break;
        }

        private void MeleeStageHit()
        {
            if (CanHitMeleeTarget(Target))
            {
                GeneratePrimaryResource(6f);
                WeaponDamage(Target, 1.20f, DamageType.Lightning);
            }
        }

        [ImplementsPowerBuff(7, false)]
        class ComboStage3Buff : PowerBuff
        {
            public override void Init()
            {
                Timeout = WaitSeconds(0.5f / EvalTag(PowerKeys.ComboAttackSpeed3));
            }
        }
    }

    [ImplementsPowerSNO(Skills.Skills.Monk.SpiritSpenders.SevenSidedStrike)]
    public class MonkSevenSidedStrike : PowerScript
    {
        public override IEnumerable<TickTimer> Run()
        {
            //UsePrimaryResource(50f);
            //StartCooldown(WaitSeconds(30f));
            
            var groundEffect = SpawnProxy(TargetPosition, WaitInfinite());
            groundEffect.PlayEffectGroup(145041);

            for (int n = 0; n < 7; ++n)
            {
                IList<Actor> nearby = GetEnemiesInRadius(TargetPosition, 25f);
                if (nearby.Count > 0)
                {
                    var target = nearby[Rand.Next(0, nearby.Count)];

                    SpawnEffect(99063, target.Position, -1);                    
                    yield return WaitSeconds(0.2f);

                    if (Rune_E > 0)
                    {
                        target.PlayEffectGroup(99098);
                        var splashTargets = GetEnemiesInRadius(target.Position, 5f);
                        splashTargets.Remove(target); // don't hit target with splash
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
    public class MonkCripplingWave : PowerScript
    {
        public override IEnumerable<TickTimer> Run()
        {
            int effectSNO;
            switch (TargetMessage.Field5)
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
            if (TargetMessage.Field5 != 2)
            {
                if (CanHitMeleeTarget(Target))
                {
                    hitAnything = true;
                    WeaponDamage(Target, 1.35f, DamageType.Physical);
                }
            }
            else
            {
                IList<Actor> hits = GetEnemiesInRadius(User.Position, 10f);
                foreach (Actor hit in hits)
                {
                    hitAnything = true;
                    WeaponDamage(hit, 1.35f, DamageType.Physical);
                }
            }

            if (hitAnything)
                GeneratePrimaryResource(6f);

            yield break;
        }
    }

    [ImplementsPowerSNO(Skills.Skills.Monk.SpiritGenerator.ExplodingPalm)]
    public class MonkExplodingPalm : PowerScript
    {
        public override IEnumerable<TickTimer> Run()
        {
            int effectSNO;
            switch (TargetMessage.Field5)
            {
                case 0:
                    effectSNO = 143841;
                    break;
                case 1:
                    effectSNO = 143841;
                    break;
                case 2:
                    effectSNO = 143473;
                    break;
                default:
                    yield break;
            }

            User.PlayEffectGroup(effectSNO);

            if (CanHitMeleeTarget(Target))
            {
                GeneratePrimaryResource(6f);
                WeaponDamage(Target, 1.00f, DamageType.Physical);
            }

            yield break;
        }
    }

    [ImplementsPowerSNO(Skills.Skills.Monk.SpiritGenerator.SweepingWind)]
    public class MonkSweepingWind : PowerScript
    {
        public override IEnumerable<TickTimer> Run()
        {
            int effectSNO;
            switch (TargetMessage.Field5)
            {
                case 0:
                    effectSNO = 196981;
                    break;
                case 1:
                    effectSNO = 196983;
                    break;
                case 2:
                    effectSNO = 196984;
                    break;
                default:
                    yield break;
            }

            User.PlayEffectGroup(effectSNO);

            if (CanHitMeleeTarget(Target))
            {
                GeneratePrimaryResource(6f);
                WeaponDamage(Target, 1.00f, DamageType.Physical);
            }

            yield break;
        }
    }

    [ImplementsPowerSNO(Skills.Skills.Monk.SpiritSpenders.DashingStrike)]
    public class MonkDashingStrike : PowerScript
    {
        public override IEnumerable<TickTimer> Run()
        {
            //UsePrimaryResource(15f);

            // dashing strike never specifies the target's id so we just search for the closest target
            // ultimately need to know the radius of each target and select the one most covered
            float min_distance = float.MaxValue;
            foreach (Actor actor in GetEnemiesInRadius(TargetPosition, 8f))
            {
                float distance = PowerMath.Distance(actor.Position, TargetPosition);
                if (distance < min_distance)
                {
                    min_distance = distance;
                    Target = actor;
                }
            }

            if (Target != null)
            {
                // put dash destination just beyond target
                TargetPosition = PowerMath.ProjectAndTranslate2D(User.Position, Target.Position, Target.Position, 7f);
            }
            else
            {
                // if no target, always dash fixed amount
                TargetPosition = PowerMath.ProjectAndTranslate2D(User.Position, TargetPosition, User.Position, 13f);
            }
            
            // dash speed seems to always be actor speed * 10
            float speed = User.Attributes[GameAttribute.Running_Rate_Total] * 10f;
            TickTimer minDashWait = WaitSeconds(0.15f);
            TickTimer waitDashEnd = new RelativeTickTimer(World.Game, (int)(PowerMath.Distance2D(User.Position, TargetPosition) / speed));
            
            // if dash ticks is too small the effect won't show at all, so always make it at least minDashWait
            waitDashEnd = minDashWait.TimeoutTick > waitDashEnd.TimeoutTick ? minDashWait : waitDashEnd;
            
            // dashing effect buff
            AddBuff(User, new DashingBuff0(waitDashEnd));
            
            // TODO: Generalize this and put it in Actor
            User.World.BroadcastInclusive(new NotifyActorMovementMessage
            {
                ActorId = (int)User.DynamicID,
                Position = TargetPosition,
                Angle = PowerMath.AngleLookAt(User.Position, TargetPosition),
                Field3 = true, // turn instantly toward target
                Speed = speed,
                Field5 = 0x9206, // alt: 0x920e, not sure what this param is for.
                AnimationTag = 69808, // dashing strike attack animation
                Field7 = 6, // ticks to wait before playing animation
            }, User);
            User.Position = TargetPosition;

            yield return waitDashEnd;

            if (Target != null && Target.World != null) // target could've died or left world
            {
                User.TranslateFacing(Target.Position, true);
                yield return WaitSeconds(0.1f);
                User.PlayEffectGroup(113720);
                WeaponDamage(Target, 1.60f, DamageType.Physical);
            }
        }

        [ImplementsPowerBuff(0)]
        class DashingBuff0 : PowerBuff
        {
            public DashingBuff0(TickTimer timeout)
            {
                Timeout = timeout;
            }

            public override bool Apply()
            {
                if (!base.Apply())
                    return false;

                User.Attributes[GameAttribute.Hidden] = true;
                User.Attributes.BroadcastChangedIfRevealed();
                return true;
            }

            public override void Remove()
            {
                base.Remove();
                User.Attributes[GameAttribute.Hidden] = false;
                User.Attributes.BroadcastChangedIfRevealed();
            }
        }
    }

    [ImplementsPowerSNO(Skills.Skills.Monk.Mantras.MantraOfEvasion)]
    public class MonkMantraOfEvasion : PowerScript
    {
        public override IEnumerable<TickTimer> Run()
        {
            // cast effect
            User.PlayEffectGroup(143964);

            AddBuff(User, new CasterBuff());
            foreach (Actor ally in GetAlliesInRadius(User.Position, ScriptFormula(0)))
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

                foreach (Actor ally in GetAlliesInRadius(Target.Position, ScriptFormula(0)))
                {
                    if (ally != Target)
                        AddBuff(ally, new AllyBuff());
                }

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
    public class MonkBlindingFlash : PowerScript
    {
        public override IEnumerable<TickTimer> Run()
        {
            User.PlayEffectGroup(137644);

            AttackPayload attack = new AttackPayload(this);
            attack.AddTargets(GetEnemiesInRadius(User.Position, ScriptFormula(1)));
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
