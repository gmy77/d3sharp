﻿/*
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

using System.Text;
using Mooege.Core.GS.Games;
using Mooege.Net.GS.Message.Definitions.Game;

namespace Mooege.Net.GS.Message.Definitions.Connection
{
    [Message(Opcodes.LogoutComplete)]
    public class LogoutComplete : GameMessage,ISelfHandler
    {
        public void Handle(GameClient client)
        {
            if (client.IsLoggingOut)
            {
                client.SendMessage(new QuitGameMessage() // should be sent to all players i guess /raist.
                {
                    PlayerIndex = client.Player.PlayerIndex,
                });

                GameManager.RemovePlayerFromGame(client);
            }
        }

        public override void Parse(GameBitBuffer buffer)
        {
            
        }

        public override void Encode(GameBitBuffer buffer)
        {
            
        }

        public override void AsText(StringBuilder b, int pad)
        {
            
        }
    }
}
