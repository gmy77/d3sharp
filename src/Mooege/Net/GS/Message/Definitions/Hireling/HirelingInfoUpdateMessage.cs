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

namespace Mooege.Net.GS.Message.Definitions.Hireling
{
    [Message(Opcodes.HirelingInfoUpdateMessage)]
    public class HirelingInfoUpdateMessage : GameMessage
    {
        public int HirelingIndex;
        public bool Field1;
        public int Field2;
        public int Level;

        public HirelingInfoUpdateMessage()
            : base(Opcodes.HirelingInfoUpdateMessage)
        {
        }

        public override void Parse(GameBitBuffer buffer)
        {
            HirelingIndex = buffer.ReadInt(2);
            Field1 = buffer.ReadBool();
            Field2 = buffer.ReadInt(32);
            Level = buffer.ReadInt(7);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(2, HirelingIndex);
            buffer.WriteBool(Field1);
            buffer.WriteInt(32, Field2);
            buffer.WriteInt(7, Level);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("HirelingInfoUpdateMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("HirelingIndex: 0x" + HirelingIndex.ToString("X8") + " (" + HirelingIndex + ")");
            b.Append(' ', pad); b.AppendLine("Field1: " + (Field1 ? "true" : "false"));
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', pad); b.AppendLine("Level: 0x" + Level.ToString("X8") + " (" + Level + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}