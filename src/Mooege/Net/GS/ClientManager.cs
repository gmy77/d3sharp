using System.Collections.Generic;
using Mooege.Common;
using Mooege.Core.GS.Universe;

namespace Mooege.Net.GS
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
            Universes.Add(universe);

            var gameClient = new GameClient(e.Connection,universe);
            e.Connection.Client = gameClient;
        }

        public static void OnDisconnect(object sender, ConnectionEventArgs e)
        {
            Logger.Trace("Client disconnected: {0}", e.Connection.ToString());

            Universes.Remove(((GameClient) e.Connection.Client).Player.Universe);
        }
    }
}
