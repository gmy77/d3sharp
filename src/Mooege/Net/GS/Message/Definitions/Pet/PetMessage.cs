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

namespace Mooege.Net.GS.Message.Definitions.Pet
{
    [Message(Opcodes.PetMessage)]
    public class PetMessage : GameMessage
    {
        public int Field0;
        public int Field1;
        public uint PetId;
        public int Field3;

        public PetMessage()
            : base(Opcodes.PetMessage)
        {
        }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(3);
            Field1 = buffer.ReadInt(5);
            PetId = buffer.ReadUInt(32);
            Field3 = buffer.ReadInt(5) + (-1);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(3, Field0);
            buffer.WriteInt(5, Field1);
            buffer.WriteUInt(32, PetId);
            buffer.WriteInt(5, Field3 - (-1));
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("PetMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad); b.AppendLine("PetId: 0x" + PetId.ToString("X8") + " (" + PetId + ")");
            b.Append(' ', pad); b.AppendLine("Field3: 0x" + Field3.ToString("X8") + " (" + Field3 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }
    }
}