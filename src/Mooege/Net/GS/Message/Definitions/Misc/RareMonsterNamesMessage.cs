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

namespace Mooege.Net.GS.Message.Definitions.Misc
{
    [Message(Opcodes.RareMonsterNamesMessage)]
    public class RareMonsterNamesMessage : GameMessage
    {
        public int Field0;
        // MaxLength = 2
        public int /* gbid */[] Field1;
        // MaxLength = 8
        public int /* gbid */[] Field2;

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = new int /* gbid */[2];
            for (int i = 0; i < Field1.Length; i++) Field1[i] = buffer.ReadInt(32);
            Field2 = new int /* gbid */[8];
            for (int i = 0; i < Field2.Length; i++) Field2[i] = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            for (int i = 0; i < Field1.Length; i++) buffer.WriteInt(32, Field1[i]);
            for (int i = 0; i < Field2.Length; i++) buffer.WriteInt(32, Field2[i]);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("RareMonsterNamesMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1:");
            b.Append(' ', pad); b.AppendLine("{");
            for (int i = 0; i < Field1.Length; ) { b.Append(' ', pad + 1); for (int j = 0; j < 8 && i < Field1.Length; j++, i++) { b.Append("0x" + Field1[i].ToString("X8") + ", "); } b.AppendLine(); }
            b.Append(' ', pad); b.AppendLine("}"); b.AppendLine();
            b.Append(' ', pad); b.AppendLine("Field2:");
            b.Append(' ', pad); b.AppendLine("{");
            for (int i = 0; i < Field2.Length; ) { b.Append(' ', pad + 1); for (int j = 0; j < 8 && i < Field2.Length; j++, i++) { b.Append("0x" + Field2[i].ToString("X8") + ", "); } b.AppendLine(); }
            b.Append(' ', pad); b.AppendLine("}"); b.AppendLine();
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}