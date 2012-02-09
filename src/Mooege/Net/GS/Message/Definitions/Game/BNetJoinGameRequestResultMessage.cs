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
    [Message(Opcodes.BNetJoinGameRequestResultMessage)]
    public class BNetJoinGameRequestResultMessage : GameMessage
    {
        public int Field0;
        public GameId Field1;
        public long Field2;
        public int Field3;
        public int /* sno */ Field4;
        public int Field5;
        public int Field6;
        public short Field7;

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(3) + (-1);
            Field1 = new GameId();
            Field1.Parse(buffer);
            Field2 = buffer.ReadInt64(64);
            Field3 = buffer.ReadInt(3) + (-1);
            Field4 = buffer.ReadInt(32);
            Field5 = buffer.ReadInt(16);
            Field6 = buffer.ReadInt(32);
            Field7 = (short)buffer.ReadInt(16);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(3, Field0 - (-1));
            Field1.Encode(buffer);
            buffer.WriteInt64(64, Field2);
            buffer.WriteInt(3, Field3 - (-1));
            buffer.WriteInt(32, Field4);
            buffer.WriteInt(16, Field5);
            buffer.WriteInt(32, Field6);
            buffer.WriteInt(16, Field7);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("BNetJoinGameRequestResultMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            Field1.AsText(b, pad);
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X16"));
            b.Append(' ', pad); b.AppendLine("Field3: 0x" + Field3.ToString("X8") + " (" + Field3 + ")");
            b.Append(' ', pad); b.AppendLine("Field4: 0x" + Field4.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("Field5: 0x" + Field5.ToString("X8") + " (" + Field5 + ")");
            b.Append(' ', pad); b.AppendLine("Field6: 0x" + Field6.ToString("X8") + " (" + Field6 + ")");
            b.Append(' ', pad); b.AppendLine("Field7: 0x" + Field7.ToString("X4"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}