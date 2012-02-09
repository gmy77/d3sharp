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
    [Message(Opcodes.RequestJoinBNetGameMessage)]
    public class RequestJoinBNetGameMessage : GameMessage
    {
        public GameId Field0;
        public EntityId Field1;
        public int Field2;

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = new GameId();
            Field0.Parse(buffer);
            Field1 = new EntityId();
            Field1.Parse(buffer);
            Field2 = buffer.ReadInt(5) + (-1);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            Field0.Encode(buffer);
            Field1.Encode(buffer);
            buffer.WriteInt(5, Field2 - (-1));
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("RequestJoinBNetGameMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            Field0.AsText(b, pad);
            Field1.AsText(b, pad);
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }
    }
}