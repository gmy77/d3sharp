using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mooege.Core.GS.Games;

namespace Mooege.Core.GS.Powers
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
