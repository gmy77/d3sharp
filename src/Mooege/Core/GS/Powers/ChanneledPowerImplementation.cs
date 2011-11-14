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

namespace Mooege.Core.GS.Powers
{
    public abstract class ChanneledPowerImplementation : PowerImplementation
    {
        public bool ChannelOpen = false;
        public float RunDelay = 1.0f;

        public virtual void OnChannelOpen() { }
        public virtual void OnChannelClose() { }
        public virtual void OnChannelUpdated() { }
        public abstract IEnumerable<TickTimer> RunChannel();

        private TickTimer _runTimeout = null;

        public sealed override IEnumerable<TickTimer> Run()
        {
            if (_runTimeout == null || _runTimeout.TimedOut)
            {
                _runTimeout = WaitSeconds(RunDelay);
                foreach (TickTimer timeout in RunChannel())
                    yield return timeout;
            }

            yield break;
        }
    }
}
