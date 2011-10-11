using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mooege.Core.GS.Game
{
    public static class GameManager
    {
        private static readonly Dictionary<int, Game> Games = new Dictionary<int, Game>();

        public static Game CreateGame(int gameId)
        {
            var game = new Game(gameId);
            Games.Add(gameId, game);
            return game;
        }

        public static Game GetGameById(int gameId)
        {
            return !Games.ContainsKey(gameId) ? null : Games[gameId];
        }
    }
}
