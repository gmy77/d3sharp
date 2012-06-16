/*
 * Copyright (C) 2012 mooege project
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

using Mooege.Net.GS.Message;

namespace Mooege.Core.GS.Powers.Implementations
{
    [ImplementsPowerBuff(0)]
    [ImplementsPowerSNO(224639)]
    public class ActorGhostedBuff : PowerBuff
    {
        public override void Init()
        {
            base.Init();
            Timeout = WaitSeconds(5f);  // TODO: find if this is defined anywhere
        }

        public override bool Apply()
        {
            base.Apply();
            this.Target.Attributes[GameAttribute.Look_Override] = 0x0782CAC5;
            return true;
        }

        public override void Remove()
        {
            this.Target.Attributes[GameAttribute.Look_Override] = 0;
            base.Remove();
        }
    }
}
