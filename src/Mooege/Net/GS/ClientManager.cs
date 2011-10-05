using System.Collections.Generic;
using Mooege.Common;
using Mooege.Core.GS.Game;

namespace Mooege.Net.GS
{
    public class ClientManager
    {
        protected static readonly Logger Logger = LogManager.CreateLogger();

        public static List<Game> Games = new List<Game>();

        public static void OnConnect(object sender, ConnectionEventArgs e)
        {
            Logger.Trace("Game-Client connected: {0}", e.Connection.ToString());

            // atm, just creating a game - though clients should be able to join existing ones.
            var game = new Game();
            Games.Add(game);

            var gameClient = new GameClient(e.Connection, game);
            e.Connection.Client = gameClient;
        }

        public static void OnDisconnect(object sender, ConnectionEventArgs e)
        {
            Logger.Trace("Client disconnected: {0}", e.Connection.ToString());
            Games.Remove(((GameClient) e.Connection.Client).Player.Game);
        }
    }
}
