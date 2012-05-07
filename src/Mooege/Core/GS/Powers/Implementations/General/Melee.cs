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
using Mooege.Core.GS.Ticker;
using Mooege.Core.GS.Actors;
using Mooege.Net.GS.Message;


namespace Mooege.Core.GS.Powers.Implementations
{
    [ImplementsPowerSNO(30592)]  // Weapon_Melee_Instant.pow
    public class WeaponMeleeInstant : ActionTimedSkill
    {
        public override IEnumerable<TickTimer> Main()
        {
            WeaponDamage(GetBestMeleeEnemy(), 1.00f, DamageType.Physical);
            yield break;
        }

        public override float GetActionSpeed()
        {
            // for some reason the formula for _Instant.pow does not multiply by 1.1 even though it should
            // manually scale melee speed
            return base.GetActionSpeed() * 1.1f;
        }
    }
}
