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
using Mooege.Core.GS.Games;
using Mooege.Core.GS.Objects;

namespace Mooege.Core.GS.Ticker.Helpers
{
    public class TickTimer : IUpdateable
    {
        public int TimeoutTick;
        private readonly Game _game;
        private Action<int> _action;

        public TickTimer(Game game, int timeoutTick, Action<int> callback = null)
        {
            this._game = game;
            this.TimeoutTick = timeoutTick;
            this._action = callback;
        }

        public static TickTimer WaitSeconds(Game game, float seconds, Action<int> callback = null)
        {
            return new TickSecondsTimer(game, seconds, callback);
        }

        public static TickTimer WaitMiliSeconds(Game game, float miliSeconds, Action<int> callback = null)
        {
            return new TickMiliSecondsTimer(game, miliSeconds, callback);
        }

        public static TickTimer WaitTicks(Game game, int ticks, Action<int> callback = null)
        {
            return new TickRelativeTimer(game, ticks, callback);
        }

        public bool TimedOut
        {
            get { return _game.TickCounter >= TimeoutTick; }
        }

        public bool Running
        {
            get { return !this.TimedOut; }
        }

        public void Update(int tickCounter)
        {
            if (this.TimeoutTick == -1) // means timer is already fired there.
                return;

            if (this._action == null) // if we don't have an assigned action, don't care.
                return;

            if (!this.TimedOut) return; // if we haven't timed-out yet, return.

            this._action(tickCounter); // call the action.
            this.Stop();
        }

        public void Stop()
        {
            this.TimeoutTick = -1;
        }
    }

    public class TickRelativeTimer : TickTimer
    {
        public TickRelativeTimer(Game game, int ticks, Action<int> callback = null)
            : base(game, game.TickCounter + ticks, callback)
        { }
    }

    public class TickSecondsTimer : TickRelativeTimer
    {
        public TickSecondsTimer(Game game, float seconds, Action<int> callback = null)
            : base(game, (int)(1000f / game.UpdateFrequency * game.TickRate * seconds), callback)
        { }
    }

    public class TickMiliSecondsTimer : TickRelativeTimer
    {
        public TickMiliSecondsTimer(Game game, float miliSeconds, Action<int> callback = null)
            : base(game, (int)((1000f / game.UpdateFrequency * game.TickRate) / 1000f * miliSeconds), callback)
        { }
    }
}