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

        public static void RemovePlayerFromGame(Mooege.Net.GS.GameClient gameClient)
        {
            if (gameClient != null && gameClient.Game != null)
            {
                int gameId = gameClient.Game.GameId;
                if (Games.ContainsKey(gameId))
                {
                    Game game = Games[gameId];
                    if (game.Players.ContainsKey(gameClient))
                    {
                        Player.Player p = null;
                        if (!game.Players.TryRemove(gameClient, out p))
                        {
                            //log something
                        }

                        if (game.Players.Count == 0)
                        {
                            //remove game
                            Games.Remove(gameId);
                        }
                    }
                }
            }
        }
    }
}
