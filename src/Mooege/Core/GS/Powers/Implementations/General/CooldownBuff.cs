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
using Mooege.Core.GS.Ticker;
using Mooege.Net.GS.Message;

namespace Mooege.Core.GS.Powers.Implementations
{
    [ImplementsPowerSNO(30176)]  // Cooldown.pow
    [ImplementsPowerBuff(0)]
    public class CooldownBuff : PowerBuff
    {
        public int TargetPowerSNO;
        private float? _seconds;

        public CooldownBuff(int targetPowerSNO, TickTimer timeout)
        {
            TargetPowerSNO = targetPowerSNO;
            Timeout = timeout;
        }

        public CooldownBuff(int targetPowerSNO, float seconds)
        {
            TargetPowerSNO = targetPowerSNO;
            _seconds = seconds;
        }

        public override void Init()
        {
            base.Init();
            if (_seconds.HasValue)
                Timeout = WaitSeconds(_seconds.Value);
        }

        public override bool Apply()
        {
            if (!base.Apply())
                return false;

            Target.Attributes[GameAttribute.Power_Cooldown_Start, TargetPowerSNO] = this.World.Game.TickCounter;
            Target.Attributes[GameAttribute.Power_Cooldown, TargetPowerSNO] = this.Timeout.TimeoutTick;
            Target.Attributes.BroadcastChangedIfRevealed();
            return true;
        }

        public override void Remove()
        {
            base.Remove();
            Target.Attributes[GameAttribute.Power_Cooldown_Start, TargetPowerSNO] = -1;
            Target.Attributes[GameAttribute.Power_Cooldown, TargetPowerSNO] = -1;
            Target.Attributes.BroadcastChangedIfRevealed();
        }

        public override bool Stack(Buff buff)
        {
            // multiple cooldowns of different target powers are allowed
            // and multiple cooldowns on the same power should never happen
            return false;
        }
    }
}
