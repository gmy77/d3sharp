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

namespace Mooege.Net.GS.Message.Definitions.ACD
{
    [Message(Opcodes.ACDChangeGBHandleMessage)]
    public class ACDChangeGBHandleMessage : GameMessage
    {
        public int Field0;
        public GBHandle Field1;

        public ACDChangeGBHandleMessage() : base(Opcodes.ACDChangeGBHandleMessage) { }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = new GBHandle();
            Field1.Parse(buffer);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            Field1.Encode(buffer);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("ACDChangeGBHandleMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            Field1.AsText(b, pad);
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}