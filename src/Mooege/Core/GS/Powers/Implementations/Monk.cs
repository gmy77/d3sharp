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
    [PowerImplementationAttribute(Skills.Skills.Monk.SpiritGenerator.DeadlyReach)]
    public class MonkDeadlyReach : PowerImplementation
    {
        public override IEnumerable<int> Run(PowerParameters pp, PowerManager fx)
        {
            int effectSNO;
            float reachLength;
            float reachThickness;

            switch(pp.Message.Field5)
            {
                case 0:
                    effectSNO = 71921;
                    reachLength = 12;
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
            pp.TargetPosition = PowerUtils.ProjectAndTranslate2D(pp.TargetPosition, pp.User.Position,
                                                   pp.User.Position, reachLength);

            fx.PlayEffectGroupActorToActor(effectSNO, pp.User, fx.SpawnTempProxy(pp.User, pp.TargetPosition));

            foreach (Actor actor in fx.FindActorsInRange(pp.User, pp.User.Position, reachLength + 10f))
            {
                if (PowerUtils.PointInBeam(actor.Position, pp.User.Position, pp.TargetPosition, reachThickness))
                {
                    fx.PlayHitEffect(5, pp.User, actor);
                    fx.DoDamage(pp.User, actor, 30f, 0);
                }
            }

            yield break;
        }
    }

    [PowerImplementationAttribute(Skills.Skills.Monk.SpiritGenerator.FistsOfThunder)]
    public class MonkFistsOfThunder : PowerImplementation
    {
        public override IEnumerable<int> Run(PowerParameters pp, PowerManager fx)
        {
            switch (pp.Message.Field5)
            {
                case 0:
                case 1:
                    fx.PlayEffectGroupActorToActor(96176, pp.User, fx.SpawnTempProxy(pp.User, pp.TargetPosition));
                    if (fx.CanHitMeleeTarget(pp.User, pp.Target))
                    {
                        fx.PlayHitEffect(2, pp.User, pp.Target);
                        fx.DoDamage(pp.User, pp.Target, 25f, 0);
                    }
                    break;
                case 2:
                    // put target position a little bit in front of the monk. represents the lightning ball
                    pp.TargetPosition = PowerUtils.ProjectAndTranslate2D(pp.TargetPosition, pp.User.Position,
                                        pp.User.Position, 8f);

                    fx.PlayEffectGroupActorToActor(96178, pp.User, fx.SpawnTempProxy(pp.User, pp.TargetPosition));

                    foreach (Actor actor in fx.FindActorsInRange(pp.User, pp.TargetPosition, 7f))
                    {
                        fx.PlayHitEffect(2, pp.User, actor);
                        fx.DoDamage(pp.User, actor, 25f, 0);
                    }
                    break;
                default:
                    yield break;
            }

            yield break;
        }
    }

    [PowerImplementationAttribute(Skills.Skills.Monk.SpiritSpenders.SevenSidedStrike)]
    public class MonkSevenSidedStrike : PowerImplementation
    {
        public override IEnumerable<int> Run(PowerParameters pp, PowerManager fx)
        {
            Vector3D startpos;
            if (pp.Target == null)
                startpos = pp.User.Position;
            else
                startpos = pp.TargetPosition;
            
            for (int n = 0; n < 7; ++n)
            {
                IList<Actor> nearby = fx.FindActorsInRange(pp.User, startpos, 20f, 1);
                if (nearby.Count > 0)
                {
                    fx.SpawnEffect(pp.User, 99063, nearby[0].Position);
                    fx.DoDamage(pp.User, nearby[0], 100f, 0);
                    yield return 100;
                }
                else
                {
                    break;
                }
            }
        }
    }

    [PowerImplementationAttribute(Skills.Skills.Monk.SpiritGenerator.CripplingWave)]
    public class MonkCripplingWave : PowerImplementation
    {
        public override IEnumerable<int> Run(PowerParameters pp, PowerManager fx)
        {
            if (pp.Message.Field5 == 0)
                fx.PlayEffectGroupActorToActor(18987, pp.User, fx.SpawnTempProxy(pp.User, pp.TargetPosition));
            else if (pp.Message.Field5 == 1)
                fx.PlayEffectGroupActorToActor(18988, pp.User, fx.SpawnTempProxy(pp.User, pp.TargetPosition));
            else if (pp.Message.Field5 == 2)
                fx.PlayEffectGroupActorToActor(96519, pp.User, fx.SpawnTempProxy(pp.User, pp.TargetPosition));

            if (pp.Message.Field5 != 2)
            {
                if (fx.CanHitMeleeTarget(pp.User, pp.Target))
                {
                    fx.PlayHitEffect(6, pp.User, pp.Target);
                    fx.DoDamage(pp.User, pp.Target, 25f, 0);
                }
            }
            else
            {
                IList<Actor> hits = fx.FindActorsInRange(pp.User, pp.User.Position, 10);
                foreach (Actor hit in hits)
                {
                    fx.PlayHitEffect(6, pp.User, hit);
                    fx.DoDamage(pp.User, hit, 25f, 0);
                }
            }
            yield break;
        }
    }

    [PowerImplementationAttribute(Skills.Skills.Monk.SpiritGenerator.ExplodingPalm)]
    public class MonkExplodingPalm : PowerImplementation
    {
        public override IEnumerable<int> Run(PowerParameters pp, PowerManager fx)
        {
            if (pp.Message.Field5 == 0)
                fx.PlayEffectGroupActorToActor(142471, pp.User, fx.SpawnTempProxy(pp.User, pp.TargetPosition));
            else if (pp.Message.Field5 == 1)
                fx.PlayEffectGroupActorToActor(142471, pp.User, fx.SpawnTempProxy(pp.User, pp.TargetPosition));
            else if (pp.Message.Field5 == 2)
                fx.PlayEffectGroupActorToActor(142473, pp.User, fx.SpawnTempProxy(pp.User, pp.TargetPosition));

            if (fx.CanHitMeleeTarget(pp.User, pp.Target))
            {
                fx.PlayHitEffect(0, pp.User, pp.Target);
                fx.DoDamage(pp.User, pp.Target, 25f, 0);
            }

            yield break;
        }
    }

    [PowerImplementationAttribute(Skills.Skills.Monk.SpiritGenerator.SweepingWind)]
    public class MonkSweepingWind : PowerImplementation
    {
        public override IEnumerable<int> Run(PowerParameters pp, PowerManager fx)
        {
            // TODO: make buffs disappear so skill can be implemented
            //if (pp.Message.Field5 == 0)
            //    fx.PlayEffectGroupActorToActor(73953, pp.User, fx.SpawnTempProxy(pp.User, pp.TargetPosition));
            //else if (pp.Message.Field5 == 1)
            //    fx.PlayEffectGroupActorToActor(73953, pp.User, fx.SpawnTempProxy(pp.User, pp.TargetPosition));
            //else if (pp.Message.Field5 == 2)
            //    fx.PlayEffectGroupActorToActor(73953, pp.User, fx.SpawnTempProxy(pp.User, pp.TargetPosition));

            yield break;
        }
    }
}
