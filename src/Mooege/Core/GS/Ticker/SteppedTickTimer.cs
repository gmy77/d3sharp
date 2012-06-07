/*
 * Copyright (C) 2011 - 2012 mooege project - http://www.mooege.org
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

namespace Mooege.Core.GS.Ticker
{
    /// <summary>
    /// A stepped tick timer that can fire actions on given ticks per step and fire a final one on completition.
    /// </summary>
    public class SteppedTickTimer : TickTimer
    {
        /// <summary>
        /// Ticks per step.
        /// </summary>
        public int TicksPerStep { get; private set; }

        /// <summary>
        /// Action to be fired on each step.
        /// </summary>
        public Action<int> StepAction { get; private set; }

        /// <summary>
        /// The last fired steps tick.
        /// </summary>
        public int LastStepTick { get; private set; }

        /// <summary>
        /// Creates a new stepped tick timer.
        /// </summary>
        /// <param name="game">The game timer belongs to.</param>
        /// <param name="ticksPerStep">Ticks taken per step.</param>
        /// <param name="timeoutTick">Exact tick value to timeout.</param>
        /// <param name="stepCallback">The action to be called on each step.</param>
        /// <param name="completionCallback">The completition action to be called on timeout.</param>
        public SteppedTickTimer(Game game, int ticksPerStep, int timeoutTick, Action<int> stepCallback, Action<int> completionCallback = null)
            : base(game, timeoutTick, completionCallback)
        {
            if (ticksPerStep < game.TickRate)
                throw new ArgumentOutOfRangeException("ticksPerStep", string.Format("ticksPerStep value ({0}) can not be less then timer's belonging game's TickRate ({1}).", ticksPerStep, game.TickRate));

            this.TicksPerStep = ticksPerStep;
            this.StepAction = stepCallback;
            this.LastStepTick = game.TickCounter;
        }

        /// <summary>
        /// Updates the timer.
        /// </summary>
        /// <param name="tickCounter">The current tick-counter.</param>
        public override void Update(int tickCounter)
        {
            if (this.TimeoutTick == -1) // means timer is already timed-out.
                return;

            if (!this.TimedOut) // if we haven't timed out yet, check for steps.
            {
                if ((tickCounter - this.LastStepTick) >= TicksPerStep) // check if we've progressed a step.
                {
                    this.LastStepTick = tickCounter;
                    this.StepAction(tickCounter); // call the step-action.
                }
            }
            else // if we timed-out.
            {
                if (this.CompletionAction != null) // if a completition action exists.
                    this.CompletionAction(tickCounter); //call it once the timer time-outs.

                this.Stop(); // stop the timer.
            }
        }

        /// <summary>
        /// Creates a new stepped tick timer.
        /// </summary>
        /// <param name="game">The game timer belongs to.</param>
        /// <param name="ticksPerStep">Ticks taken per step.</param>
        /// <param name="ticks">Relative tick amount taken to timeout.</param>
        /// <param name="stepCallback">The action to be called on each step.</param>
        /// <param name="completionCallback">The completition action to be called on timeout.</param>
        /// <returns><see cref="SteppedTickTimer"/></returns>
        public static SteppedTickTimer WaitTicksStepped(Game game, int ticksPerStep, int ticks, Action<int> stepCallback, Action<int> completionCallback)
        {
            return new SteppedRelativeTickTimer(game, ticksPerStep, ticks, stepCallback, completionCallback);
        }

        /// <summary>
        /// Creates a new seconds based stepped tick timer.
        /// </summary>
        /// <param name="game">The game timer belongs to.</param>
        /// <param name="secondsPerStep">Seconds taken per step.</param>
        /// <param name="seconds">Seconds taken to timeout.</param>
        /// <param name="stepCallback">The action to be called on each step.</param>
        /// <param name="completionCallback">The completition action to be called on timeout.</param>
        /// <returns><see cref="SteppedTickTimer"/></returns>
        public static SteppedTickTimer WaitSecondsStepped(Game game, float secondsPerStep, float seconds, Action<int> stepCallback, Action<int> completionCallback)
        {
            return new SteppedSecondsTickTimer(game, secondsPerStep, seconds, stepCallback, completionCallback);
        }

        /// <summary>
        /// Creates a new mili-seconds based stepped tick timer.
        /// </summary>
        /// <param name="game">The game timer belongs to.</param>
        /// <param name="miliSecondsPerStep">MiliSeconds taken per step.</param>
        /// <param name="miliSeconds">MiliSeconds taken to timeout.</param>
        /// <param name="stepCallback">The action to be called on each step.</param>
        /// <param name="completionCallback">The completition action to be called on timeout.</param>
        /// <returns><see cref="SteppedTickTimer"/></returns>
        public static SteppedTickTimer WaitMiliSecondsStepped(Game game, float miliSecondsPerStep, float miliSeconds, Action<int> stepCallback, Action<int> completionCallback)
        {
            return new SteppedMiliSecondsTickTimer(game, miliSecondsPerStep, miliSeconds, stepCallback, completionCallback);
        }
    }

    /// <summary>
    /// Stepped & relative tick timer.
    /// </summary>
    public class SteppedRelativeTickTimer : SteppedTickTimer
    {
        /// <summary>
        /// Creates a new stepped tick timer.
        /// </summary>
        /// <param name="game">The game timer belongs to.</param>
        /// <param name="ticksPerStep">Ticks taken per step.</param>
        /// <param name="ticks">Relative tick amount taken to timeout.</param>
        /// <param name="stepCallback">The action to be called on each step.</param>
        /// <param name="completionCallback">The completition action to be called on timeout.</param>
        public SteppedRelativeTickTimer(Game game, int ticksPerStep, int ticks, Action<int> stepCallback, Action<int> completionCallback)
            : base(game, ticksPerStep, game.TickCounter + ticks, stepCallback, completionCallback)
        { }
    }

    /// <summary>
    /// Seconds based stepped tick timer.
    /// </summary>
    public class SteppedSecondsTickTimer : SteppedRelativeTickTimer
    {
        /// <summary>
        /// Creates a new seconds based stepped tick timer.
        /// </summary>
        /// <param name="game">The game timer belongs to.</param>
        /// <param name="secondsPerStep">Seconds taken per step.</param>
        /// <param name="seconds">Seconds taken to timeout.</param>
        /// <param name="stepCallback">The action to be called on each step.</param>
        /// <param name="completionCallback">The completition action to be called on timeout.</param>
        public SteppedSecondsTickTimer(Game game, float secondsPerStep, float seconds, Action<int> stepCallback, Action<int> completionCallback)
            : base(game, (int)(1000f / game.UpdateFrequency * game.TickRate * secondsPerStep), (int)(1000f / game.UpdateFrequency * game.TickRate * seconds), stepCallback, completionCallback)
        { }
    }

    /// <summary>
    /// Mili-seconds based stepped tick timer.
    /// </summary>
    public class SteppedMiliSecondsTickTimer : SteppedRelativeTickTimer
    {
        /// <summary>
        /// Creates a new mili-seconds based stepped tick timer.
        /// </summary>
        /// <param name="game">The game timer belongs to.</param>
        /// <param name="miliSecondsPerStep">MiliSeconds taken per step.</param>
        /// <param name="miliSeconds">MiliSeconds taken to timeout.</param>
        /// <param name="stepCallback">The action to be called on each step.</param>
        /// <param name="completionCallback">The completition action to be called on timeout.</param>
        public SteppedMiliSecondsTickTimer(Game game, float miliSecondsPerStep, float miliSeconds, Action<int> stepCallback, Action<int> completionCallback)
            : base(game, (int)((1000f / game.UpdateFrequency * game.TickRate) / 1000f * miliSecondsPerStep), (int)((1000f / game.UpdateFrequency * game.TickRate) / 1000f * miliSeconds), stepCallback, completionCallback)
        { }
    }
}
