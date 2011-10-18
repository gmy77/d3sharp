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
                    effectSNO = 72331;
                    reachLength = 16;
                    reachThickness = 8;
                    break;
                default:
                    // TODO: get some logging in power implementations...
                    yield break;
            }

            // calculate end of attack reach
            TargetPosition = PowerUtils.ProjectAndTranslate2D(TargetPosition, User.Position,
                                                   User.Position, reachLength);

            User.PlayEffectToActor(effectSNO, SpawnProxy(TargetPosition));

            foreach (Actor actor in GetTargetsInRange(User.Position, reachLength + 10f))
            {
                if (PowerUtils.PointInBeam(actor.Position, User.Position, TargetPosition, reachThickness))
                {
                    actor.PlayHitEffect(5, User);
                    Damage(actor, 30f, 0);
                }
            }

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
                case 1:
                    User.PlayEffectToActor(96176, SpawnProxy(TargetPosition));
                    if (CanHitMeleeTarget(Target))
                    {
                        Target.PlayHitEffect(2, User);
                        Damage(Target, 25f, 0);
                    }
                    break;
                case 2:
                    // put target position a little bit in front of the monk. represents the lightning ball
                    TargetPosition = PowerUtils.ProjectAndTranslate2D(TargetPosition, User.Position,
                                        User.Position, 8f);

                    User.PlayEffectToActor(96178, SpawnProxy(TargetPosition));

                    foreach (Actor actor in GetTargetsInRange(TargetPosition, 7f))
                    {
                        actor.PlayHitEffect(2, User);
                        Damage(actor, 25f, 0);
                    }
                    break;
                default:
                    yield break;
            }

            yield break;
        }
    }

    [ImplementsPowerSNO(Skills.Skills.Monk.SpiritSpenders.SevenSidedStrike)]
    public class MonkSevenSidedStrike : PowerImplementation
    {
        public override IEnumerable<TickTimer> Run()
        {
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
            if (Message.Field5 == 0)
                User.PlayEffectToActor(18987, SpawnProxy(TargetPosition));
            else if (Message.Field5 == 1)
                User.PlayEffectToActor(18988, SpawnProxy(TargetPosition));
            else if (Message.Field5 == 2)
                User.PlayEffectToActor(96519, SpawnProxy(TargetPosition));

            if (Message.Field5 != 2)
            {
                if (CanHitMeleeTarget(Target))
                {
                    Target.PlayHitEffect(6, User);
                    Damage(Target, 25f, 0);
                }
            }
            else
            {
                IList<Actor> hits = GetTargetsInRange(User.Position, 10);
                foreach (Actor hit in hits)
                {
                    hit.PlayHitEffect(6, User);
                    Damage(hit, 25f, 0);
                }
            }
            yield break;
        }
    }

    [ImplementsPowerSNO(Skills.Skills.Monk.SpiritGenerator.ExplodingPalm)]
    public class MonkExplodingPalm : PowerImplementation
    {
        public override IEnumerable<TickTimer> Run()
        {
            if (Message.Field5 == 0)
                User.PlayEffectToActor(142471, SpawnProxy(TargetPosition));
            else if (Message.Field5 == 1)
                User.PlayEffectToActor(142471, SpawnProxy(TargetPosition));
            else if (Message.Field5 == 2)
                User.PlayEffectToActor(142473, SpawnProxy(TargetPosition));

            if (CanHitMeleeTarget(Target))
            {
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
            yield break;
        }
    }
}
