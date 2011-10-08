/*
 * Copyright (C) 2011 mooege project
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

using System.Collections.Generic;
using System.Linq;
using Mooege.Common;
using Mooege.Core.MooNet.Games;
using Mooege.Net.GS;
using Mooege.Net.GS.Message;
using Mooege.Net.GS.Message.Definitions.Game;
using Mooege.Net.GS.Message.Definitions.Act;
using Mooege.Net.GS.Message.Definitions.Player;
using Mooege.Net.GS.Message.Definitions.Connection;
using Mooege.Net.GS.Message.Definitions.Hero;
using Mooege.Net.GS.Message.Definitions.Misc;

namespace Mooege.Core.GS.Game
{
    public class PlayerManager : IMessageConsumer
    {
        static readonly Logger Logger = LogManager.CreateLogger();
        public Game Game { get; private set; }

        public PlayerManager(Game game)
        {
            this.Game = game;
        }

        public void Consume(GameClient client, GameMessage message)
        {
            if (message is JoinBNetGameMessage) OnNewPlayer(client, (JoinBNetGameMessage)message);
        }

        public void OnNewPlayer(GameClient client, JoinBNetGameMessage message)
        {
            client.BnetClient = GameManager.AvailableGames[(ulong)message.GameId].Clients.FirstOrDefault();
            if (client.BnetClient == null)
            {
                Logger.Warn("Couldn't find bnet client for joined client/player!");
                return;
            }

            client.BnetClient.InGameClient = client;

            client.SendMessageNow(new VersionsMessage(message.SNOPackHash));
            client.SendMessage(new ConnectionEstablishedMessage
            {
                Field0 = 0x00000000,
                Field1 = 0x4BB91A16,
                SNOPackHash = message.SNOPackHash,
            });
            client.SendMessage(new GameSetupMessage
            {
                Field0 = 0x00000077,
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

            var player = new Mooege.Core.GS.Player.Player(this.Game.StartWorld, client, client.BnetClient.CurrentToon);
            client.Player = player;
        }
    }
}
