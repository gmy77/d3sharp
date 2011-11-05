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
using Mooege.Core.GS.Games;

namespace Mooege.Core.GS.Common.Types
{
    public class TickTimer
    {
        public int TimeoutTick;
        private Game _game;
        
        public TickTimer(Game game, int timeoutTick)
        {
            _game = game;
            TimeoutTick = timeoutTick;
        }

        public bool TimedOut()
        {
            return _game.Tick >= TimeoutTick;
        }
    }

    public class TickRelativeTimer : TickTimer
    {
        public TickRelativeTimer(Game game, int ticks)
            : base(game, game.Tick + ticks)
        {
        }
    }

    public class TickSecondsTimer : TickRelativeTimer
    {
        const float TickRate = 6; // Game currently doesn't expose the tick increment rate

        public TickSecondsTimer(Game game, float seconds)
            : base(game, (int)(1000f / game.UpdateFrequency * TickRate * seconds))
        {
        }
    }
}
