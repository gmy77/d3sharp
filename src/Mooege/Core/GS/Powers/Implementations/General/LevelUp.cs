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
using Mooege.Core.GS.Ticker;
using Mooege.Net.GS.Message;
using Mooege.Core.GS.Players;

namespace Mooege.Core.GS.Powers.Implementations
{
    [ImplementsPowerSNO(85954)] //g_levelup.pow
    public class LevelUpBlast : PowerScript
    {
        public override IEnumerable<TickTimer> Run()
        {
            User.PlayEffectGroup(Player.LevelUpEffects[this.User.Attributes[GameAttribute.Level]]);
            yield return WaitSeconds(0.6f);
            WeaponDamage(GetEnemiesInRadius(User.Position, ScriptFormula(2)), ScriptFormula(0), DamageType.Physical);
            yield break;
        }
    }
}