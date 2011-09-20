using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace D3Sharp.Core.Games
{
    public static class GameManager
    {
        private static readonly Dictionary<ulong, Game> AvailableGames =
            new Dictionary<ulong, Game>();

        public static Game CreateGame(ulong factoryId)
        {
            return new Game((ulong) AvailableGames.Count, factoryId);
        }
    }
}
