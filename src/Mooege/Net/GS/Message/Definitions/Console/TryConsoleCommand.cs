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

namespace Mooege.Net.GS.Message.Definitions.Console
{
    [Message(new[]{Opcodes.TryConsoleCommand1, Opcodes.TryConsoleCommand2})]
    public class TryConsoleCommand : GameMessage
    {
        public int Field0;
        public string Field1;
        public WorldPlace Field2;
        public int Field3;

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(4) + (-1);
            Field1 = buffer.ReadCharArray(512);
            Field2 = new WorldPlace();
            Field2.Parse(buffer);
            Field3 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(4, Field0 - (-1));
            buffer.WriteCharArray(512, Field1);
            Field2.Encode(buffer);
            buffer.WriteInt(32, Field3);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("TryConsoleCommand:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: \"" + Field1 + "\"");
            Field2.AsText(b, pad);
            b.Append(' ', pad); b.AppendLine("Field3: 0x" + Field3.ToString("X8") + " (" + Field3 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}