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

namespace Mooege.Net.GS.Message.Fields
{
    public class VisualItem
    {
        public int /* gbid */ GbId;
        public int Field1;
        public int Field2;
        public int Field3;

        public void Parse(GameBitBuffer buffer)
        {
            GbId = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(5);
            Field2 = buffer.ReadInt(4);
            Field3 = buffer.ReadInt(5) + (-1);
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, GbId);
            buffer.WriteInt(5, Field1);
            buffer.WriteInt(4, Field2);
            buffer.WriteInt(5, Field3 - (-1));
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("VisualItem:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad);
            b.AppendLine("Field0: 0x" + GbId.ToString("X8"));
            b.Append(' ', pad);
            b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad);
            b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', pad);
            b.AppendLine("Field3: 0x" + Field3.ToString("X8") + " (" + Field3 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}