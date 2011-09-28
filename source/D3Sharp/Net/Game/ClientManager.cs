using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using D3Sharp.Core.Ingame.Universe;
using D3Sharp.Utils;

namespace D3Sharp.Net.Game
{
    public class ClientManager
    {
        protected static readonly Logger Logger = LogManager.CreateLogger();

        public static List<Universe> Universes = new List<Universe>();

        public static void OnConnect(object sender, ConnectionEventArgs e)
        {
            Logger.Trace("Game-Client connected: {0}", e.Connection.ToString());

            // atm, just creating a universe - though clients should be able to join existing ones.
            var universe = new Universe();
            var player = new Player(universe);
            var gameClient = new GameClient(e.Connection,player);
            player.Client = gameClient;

            e.Connection.Client = gameClient;

            Universes.Add(universe);
        }

        public static void OnDisconnect(object sender, ConnectionEventArgs e)
        {
            Logger.Trace("Client disconnected: {0}", e.Connection.ToString());

            Universes.Remove(((GameClient) e.Connection.Client).Player.Universe);
        }
    }
}
