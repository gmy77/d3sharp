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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mooege.Core.GS.Skills;
using Mooege.Net.GS.Message.Fields;

namespace Mooege.Core.GS.Powers.Implementations
{
    [PowerImplementationAttribute(Skills.Skills.Barbarian.FuryGenerators.Bash)]
    public class BarbarianBash : PowerImplementation
    {
        public override IEnumerable<int> Run(PowerParameters pp, PowerManager fx)
        {
            fx.SpawnEffect(pp.User, 3278, pp.User.Position, fx.AngleLookAt(pp.User.Position, pp.TargetPosition), 500);
            yield return 200;
            //if (fx.CanHitMeleeTarget(pp.User, pp.Target))
            //{
                fx.PlayEffectGroupActorToActor(18663, pp.User, pp.Target);
                fx.DoKnockback(pp.User, pp.Target, 1f);
                //fx.DoDamage(pp.User, pp.Target, 35, 0);
            //}

            yield break;
        }
    }
}
