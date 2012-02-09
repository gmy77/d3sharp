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

namespace Mooege.Net.GS.Message.Definitions.Inventory
{
    [Message( new[] {Opcodes.VoteKickMessage1, Opcodes.VoteKickMessage2})]
    class VoteKickMessage : GameMessage
    {
        public int Field0;
        public int Field1;
        public string Field2;

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(4) + (-1);
            Field1 = buffer.ReadInt(4) + (-1);
            Field2 = buffer.ReadCharArray(512);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(4, Field0 - (-1));
            buffer.WriteInt(4, Field1 - (-1));
            buffer.WriteCharArray(512, Field2);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("VoteKickMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad); b.AppendLine("Field2: " + Field2);
            b.Append(' ', --pad);
            b.AppendLine("}");
        }
    }
}
