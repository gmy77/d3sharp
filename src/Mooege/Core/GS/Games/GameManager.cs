using System.Collections.Generic;
using Mooege.Common;
using Mooege.Core.GS.Players;

namespace Mooege.Core.GS.Games
{
    public static class GameManager
    {
        static readonly Logger Logger = LogManager.CreateLogger();
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

        public static void RemovePlayerFromGame(Net.GS.GameClient gameClient)
        {
            if (gameClient == null || gameClient.Game == null) return;

            var gameId = gameClient.Game.GameId;
            if (!Games.ContainsKey(gameId)) return;

            var game = Games[gameId];
            if (!game.Players.ContainsKey(gameClient)) return;

            Player p = null;
            if (!game.Players.TryRemove(gameClient, out p))
            {
                Logger.Error("Can't remove player ({0}) from game with id: {1}", gameClient.Player.Properties.Name, gameId);
            }

            if (game.Players.Count == 0)
            {
                Games.Remove(gameId); // we should be also disposing it /raist.
            }
        }
    }
}
