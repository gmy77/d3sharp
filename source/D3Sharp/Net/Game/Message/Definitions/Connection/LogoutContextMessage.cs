/*
 * Copyright (C) 2011 D3Sharp Project
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

namespace D3Sharp.Net.Game.Message.Definitions.Connection
{
    [IncomingMessage(new[] { Opcodes.LogoutContextMessage1, Opcodes.LogoutContextMessage2 })]
    public class LogoutContextMessage : GameMessage,ISelfHandler
    {
        public bool Field0;

        public void Handle(GameClient client)
        {
            client.IsLoggingOut = !client.IsLoggingOut;

            if (client.IsLoggingOut)
            {
                client.SendMessageNow(new LogoutTickTimeMessage()
                {
                    Id = 0x0027,
                    Field0 = false, // true - logout with party?
                    Field1 = 600, // delay 1, make this equal to 0 for instant logout
                    Field2 = 600, // delay 2
                });
            }
        }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadBool();
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteBool(Field0);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("LogoutContextMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: " + (Field0 ? "true" : "false"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }
    }
}