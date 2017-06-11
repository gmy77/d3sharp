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

using System.Text;
using Mooege.Net.GS.Message.Fields;

namespace Mooege.Net.GS.Message.Definitions.Game
{
    [Message(Opcodes.JoinBNetGameMessage,Consumers.ClientManager)]
    public class JoinBNetGameMessage : GameMessage
    {
        public EntityId ToonEntityId;  // this *is* the toon id /raist.
        public GameId Field1;
        public int GameId; // and this is the SGameId there we set in Mooege.Core.MooNet.Games.Game.cs when we send the connection info to client /raist.
        public long Field3;
        public int Field4;
        public int ProtocolHash;
        public int SNOPackHash;

        public override void Parse(GameBitBuffer buffer)
        {
            ToonEntityId = new EntityId();
            ToonEntityId.Parse(buffer);
            Field1 = new GameId();
            Field1.Parse(buffer);
            GameId = buffer.ReadInt(32);
            Field3 = buffer.ReadInt64(64);
            Field4 = buffer.ReadInt(4) + (2);
            ProtocolHash = buffer.ReadInt(32);
            SNOPackHash = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            ToonEntityId.Encode(buffer);
            Field1.Encode(buffer);
            buffer.WriteInt(32, GameId);
            buffer.WriteInt64(64, Field3);
            buffer.WriteInt(4, Field4 - (2));
            buffer.WriteInt(32, ProtocolHash);
            buffer.WriteInt(32, SNOPackHash);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("JoinBNetGameMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            ToonEntityId.AsText(b, pad);
            Field1.AsText(b, pad);
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + GameId.ToString("X8") + " (" + GameId + ")");
            b.Append(' ', pad); b.AppendLine("Field3: 0x" + Field3.ToString("X16"));
            b.Append(' ', pad); b.AppendLine("Field4: 0x" + Field4.ToString("X8") + " (" + Field4 + ")");
            b.Append(' ', pad); b.AppendLine("ProtocolHash: 0x" + ProtocolHash.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("SNOPackHash: 0x" + SNOPackHash.ToString("X8"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }
    }
}
