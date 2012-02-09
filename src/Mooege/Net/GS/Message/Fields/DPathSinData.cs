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
    public class DPathSinData
    {
        public float Field0;
        public float Field1;
        public float Field2;
        public float Field3;
        public float Field4;
        public float Field5;

        public void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadFloat32();
            Field1 = buffer.ReadFloat32();
            Field2 = buffer.ReadFloat32();
            Field3 = buffer.ReadFloat32();
            Field4 = buffer.ReadFloat32();
            Field5 = buffer.ReadFloat32();
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteFloat32(Field0);
            buffer.WriteFloat32(Field1);
            buffer.WriteFloat32(Field2);
            buffer.WriteFloat32(Field3);
            buffer.WriteFloat32(Field4);
            buffer.WriteFloat32(Field5);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("DPathSinData:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad);
            b.AppendLine("Field0: " + Field0.ToString("G"));
            b.Append(' ', pad);
            b.AppendLine("Field1: " + Field1.ToString("G"));
            b.Append(' ', pad);
            b.AppendLine("Field2: " + Field2.ToString("G"));
            b.Append(' ', pad);
            b.AppendLine("Field3: " + Field3.ToString("G"));
            b.Append(' ', pad);
            b.AppendLine("Field4: " + Field4.ToString("G"));
            b.Append(' ', pad);
            b.AppendLine("Field5: " + Field5.ToString("G"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}