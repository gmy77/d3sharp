using System;
using System.Collections.Generic;
using System.Linq;
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

using System.Text;
using Mooege.Core.GS.Skills;
using Mooege.Net.GS.Message.Fields;
using Mooege.Core.GS.Actors;
using Mooege.Net.GS.Message;
using Mooege.Net.GS.Message.Definitions.Actor;
using Mooege.Core.GS.Common.Types.Math;

namespace Mooege.Core.GS.Powers.Implementations
{
    [ImplementsPowerSNO(Skills.Skills.Monk.SpiritGenerator.DeadlyReach)]
    public class MonkDeadlyReach : PowerImplementation
    {
        public override IEnumerable<TickTimer> Run()
        {
            int effectSNO;
            float reachLength;
            float reachThickness;

            switch(Message.Field5)
            {
                case 0:
                    yield return WaitSeconds(0.1f);
                    effectSNO = 71921;
                    reachLength = 13;
                    reachThickness = 6;
                    break;
                case 1:
                    effectSNO = 72134;
                    reachLength = 14;
                    reachThickness = 8;
                    break;
                case 2:
                    yield return WaitSeconds(0.3f);
                    effectSNO = 72331;
                    reachLength = 18;
                    reachThickness = 8;
                    break;
                default:
                    yield break;
            }

            // calculate end of attack reach
            TargetPosition = PowerMath.ProjectAndTranslate2D(User.Position, TargetPosition,
                                                   User.Position, reachLength);

            User.PlayEffectGroup(effectSNO);

            bool hitAnything = false;
            foreach (Actor actor in GetTargetsInRange(User.Position, reachLength + 10f))
            {
                if (PowerMath.PointInBeam(actor.Position, User.Position, TargetPosition, reachThickness))
                {
                    hitAnything = true;
                    actor.PlayHitEffect(5, User);
                    Damage(actor, 30f, 0);
                }
            }

            if (hitAnything)
                GeneratePrimaryResource(6f);

            yield break;
        }
    }

    [ImplementsPowerSNO(Skills.Skills.Monk.SpiritGenerator.FistsOfThunder)]
    public class MonkFistsOfThunder : PowerImplementation
    {
        public override IEnumerable<TickTimer> Run()
        {
            switch (Message.Field5)
            {
                case 0:
                    yield return WaitSeconds(0.1f);
                    User.PlayEffectGroup(143570); // cast
                    User.PlayEffectGroup(96176); // projectile
                    MeleeStageHit();
                    break;
                case 1:
                    User.PlayEffectGroup(143561); // cast
                    User.PlayEffectGroup(96176); // projectile
                    MeleeStageHit();
                    break;
                case 2:
                    yield return WaitSeconds(0.3f);
                    // put target position a little bit in front of the monk. represents the lightning ball
                    TargetPosition = PowerMath.ProjectAndTranslate2D(User.Position, TargetPosition,
                                        User.Position, 8f);

                    User.PlayEffectGroup(143566); // cast
                    User.PlayEffectGroup(96178); // ball of lightning

                    bool hitAnything = false;
                    foreach (Actor actor in GetTargetsInRange(TargetPosition, 7f))
                    {
                        hitAnything = true;
                        actor.PlayHitEffect(2, User);
                        Damage(actor, 25f, 0);
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
                Target.PlayHitEffect(2, User);
                Damage(Target, 25f, 0);
            }
        }
    }

    [ImplementsPowerSNO(Skills.Skills.Monk.SpiritSpenders.SevenSidedStrike)]
    public class MonkSevenSidedStrike : PowerImplementation
    {
        public override IEnumerable<TickTimer> Run()
        {
            UsePrimaryResource(50f);
            StartCooldown(WaitSeconds(30f));

            Vector3D startpos;
            if (Target == null)
                startpos = User.Position;
            else
                startpos = TargetPosition;
            
            for (int n = 0; n < 7; ++n)
            {
                IList<Actor> nearby = GetTargetsInRange(startpos, 20f, 1);
                if (nearby.Count > 0)
                {
                    SpawnEffect(99063, nearby[0].Position);
                    Damage(nearby[0], 100f, 0);
                    yield return WaitSeconds(0.1f);
                }
                else
                {
                    break;
                }
            }
        }
    }

    [ImplementsPowerSNO(Skills.Skills.Monk.SpiritGenerator.CripplingWave)]
    public class MonkCripplingWave : PowerImplementation
    {
        public override IEnumerable<TickTimer> Run()
        {
            int effectSNO;
            switch (Message.Field5)
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
            if (Message.Field5 != 2)
            {
                if (CanHitMeleeTarget(Target))
                {
                    hitAnything = true;
                    Target.PlayHitEffect(6, User);
                    Damage(Target, 25f, 0);
                }
            }
            else
            {
                IList<Actor> hits = GetTargetsInRange(User.Position, 10f);
                foreach (Actor hit in hits)
                {
                    hitAnything = true;
                    hit.PlayHitEffect(6, User);
                    Damage(hit, 25f, 0);
                }
            }

            if (hitAnything)
                GeneratePrimaryResource(6f);

            yield break;
        }
    }

    [ImplementsPowerSNO(Skills.Skills.Monk.SpiritGenerator.ExplodingPalm)]
    public class MonkExplodingPalm : PowerImplementation
    {
        public override IEnumerable<TickTimer> Run()
        {
            int effectSNO;
            switch (Message.Field5)
            {
                case 0:
                    effectSNO = 142471;
                    break;
                case 1:
                    effectSNO = 142471;
                    break;
                case 2:
                    effectSNO = 142473;
                    break;
                default:
                    yield break;
            }

            User.PlayEffectGroup(effectSNO);

            if (CanHitMeleeTarget(Target))
            {
                GeneratePrimaryResource(6f);
                Target.PlayHitEffect(0, User);
                Damage(Target, 25f, 0);
            }

            yield break;
        }
    }

    [ImplementsPowerSNO(Skills.Skills.Monk.SpiritGenerator.SweepingWind)]
    public class MonkSweepingWind : PowerImplementation
    {
        public override IEnumerable<TickTimer> Run()
        {
            int effectSNO;
            switch (Message.Field5)
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
                Target.PlayHitEffect(0, User);
                Damage(Target, 25f, 0);
            }

            yield break;
        }
    }

    [ImplementsPowerSNO(Skills.Skills.Monk.SpiritSpenders.DashingStrike)]
    public class MonkDashingStrike : PowerImplementation
    {
        public override IEnumerable<TickTimer> Run()
        {
            UsePrimaryResource(10f);

            // FIXME: some of the code currently assumes User is Player

            // dashing strike never specifies the target's id so we just search for the closest target
            // ultimately need to know the radius of each target and select the one most covered
            float min_distance = float.MaxValue;
            foreach (Actor actor in GetTargetsInRange(TargetPosition, 8f))
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
                // if no target, dash is limited in range
                if (PowerMath.Distance(User.Position, TargetPosition) > 13f)
                    TargetPosition = PowerMath.ProjectAndTranslate2D(User.Position, TargetPosition, User.Position, 13f);
            }

            _SetupAttributes(true);

            var dashTimout = WaitSeconds(0.15f);
            int dashTicks = dashTimout.TimeoutTick - User.World.Game.Tick;
            
            // TODO: Generalize this and put it in Actor
            User.World.BroadcastInclusive(new NotifyActorMovementMessage
            {
                ActorId = (int)User.DynamicID,
                Position = TargetPosition,
                Angle = PowerMath.AngleLookAt(User.Position, TargetPosition),
                Field3 = true,
                Speed = PowerMath.Distance(User.Position, TargetPosition) / dashTicks, // speed, distance per tick
                Field5 = 0x00009206, // ???
                AnimationTag = (User as Player.Player).Properties.Gender == 0 ? 69808 : 90432, // select based on gender,
                Field7 = 0x00000006 // ?
            }, User);
            User.Position.Set(TargetPosition);

            yield return dashTimout;

            _SetupAttributes(false);                                                    

            if (Target != null && Target.World != null) // target could've died or left world
            {
                User.FacingTranslate(Target.Position, true);
                Target.PlayHitEffect(2, User);
                Damage(Target, 64f, 0);
            }
        }

        private void _SetupAttributes(bool active)
        {
            int intval = active ? 1 : 0;
            // TODO: modify User.Attribute with these changes instead of just sending them.
            GameAttributeMap map = new GameAttributeMap();
            map[GameAttribute.Buff_Active, GameAttribute.Stun_Immune.Id] = active; //immune to stun during buff
            map[GameAttribute.Buff_Icon_Count0, GameAttribute.Stun_Immune.Id] = intval;
            map[GameAttribute.Buff_Active, GameAttribute.Root_Immune.Id] = active; //immune to root during buff
            map[GameAttribute.Buff_Icon_Count0, GameAttribute.Root_Immune.Id] = intval;
            map[GameAttribute.Buff_Active, GameAttribute.Fear_Immune.Id] = active; //immune to fear during buff
            map[GameAttribute.Buff_Icon_Count0, GameAttribute.Fear_Immune.Id] = intval;
            map[GameAttribute.Buff_Active, GameAttribute.Uninterruptible.Id] = active; //uninteruptable during buff
            map[GameAttribute.Buff_Icon_Count0, PowerSNO] = intval;
            map[GameAttribute.Uninterruptible, PowerSNO] = active;
            map[GameAttribute.Power_Buff_0_Visual_Effect_None, PowerSNO] = active; // switch on effect				
            map[GameAttribute.Buff_Active, PowerSNO] = active;
            map[GameAttribute.Hidden] = active;

            map.SendMessage(((Player.Player)User).InGameClient, User.DynamicID);
        }
    }
}
