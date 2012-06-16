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

using Mooege.Common.Logging;
using Mooege.Core.GS.Games;
using Mooege.Core.GS.Players;
using Mooege.Core.MooNet.Toons;
using Mooege.Net.GS.Message;
using Mooege.Net.GS.Message.Definitions.Act;
using Mooege.Net.GS.Message.Definitions.Connection;
using Mooege.Net.GS.Message.Definitions.Game;
using Mooege.Net.GS.Message.Definitions.Hero;
using Mooege.Net.GS.Message.Definitions.Misc;
using System;
using Mooege.Common.Extensions;

namespace Mooege.Net.GS
{
    public class ClientManager : IMessageConsumer
    {
        protected static readonly Logger Logger = LogManager.CreateLogger();
        private static readonly ClientManager _instance = new ClientManager();
        public static ClientManager Instance { get { return _instance; } }


        public void OnConnect(object sender, ConnectionEventArgs e)
        {
            Logger.Trace("Game-Client connected: {0}", e.Connection.ToString());

            var gameClient = new GameClient(e.Connection);
            e.Connection.Client = gameClient;
        }

        public void OnDisconnect(object sender, ConnectionEventArgs e)
        {
            Logger.Trace("Client disconnected: {0}", e.Connection.ToString());
            GameManager.RemovePlayerFromGame((GameClient)e.Connection.Client);
        }

        public void Consume(GameClient client, GameMessage message)
        {
            if (message is JoinBNetGameMessage) OnJoinGame(client, (JoinBNetGameMessage)message);
        }

        private void OnJoinGame(GameClient client, JoinBNetGameMessage message)
        {
            var game = GameManager.GetGameById(message.GameId);
            if (game == null)
            {
                Logger.Warn("Client provided message.GameId doesnt exists, dropping him..");
                client.Connection.Disconnect();
                return;
            }
            lock (game)
            {
                var toon = ToonManager.GetToonByLowID((ulong)message.ToonEntityId.Low);

                client.Game = game;

                if (toon.GameAccount.LoggedInClient == null)
                {
                    Logger.Warn("Client doesn't seem to be connected to moonet, dropping him..");
                    client.Connection.Disconnect();
                    return; // if moonet connection is lost, don't allow him to get in.
                }

                // Set references between MooNetClient and GameClient.
                client.BnetClient = toon.GameAccount.LoggedInClient;
                client.BnetClient.InGameClient = client;

                client.Player = new Player(game.StartingWorld, client, toon);
                Logger.Info("Player {0}[PlayerIndex: {1}] connected.", client.Player.Toon.Name, client.Player.PlayerIndex);

                client.SendMessage(new VersionsMessage(message.SNOPackHash));

                client.SendMessage(new ConnectionEstablishedMessage
                {
                    PlayerIndex = client.Player.PlayerIndex,
                    Field1 = 0x4BB91A16,
                    SNOPackHash = message.SNOPackHash,
                });

                client.SendMessage(new GameSetupMessage // should be the current tick for the game /raist.
                {
                    Field0 = game.TickCounter,
                });

                client.SendMessage(new SavePointInfoMessage
                {
                    snoLevelArea = -1,
                });

                client.SendMessage(new HearthPortalInfoMessage
                {
                    snoLevelArea = -1,
                    Field1 = -1,
                });

                // transition player to act so client can load act related data? /raist
                client.SendMessage(new ActTransitionMessage
                {
                    Field0 = 0x00000000,
                    Field1 = true,
                });

                toon.LoginTime = DateTimeExtensions.ToUnixTime(DateTime.UtcNow);
                Logger.Trace("Log in time:" + toon.LoginTime.ToString());

                game.Enter(client.Player);
            }
        }
    }
}
