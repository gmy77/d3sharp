﻿/*
 * Copyright (C) 2011 mooege project
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

namespace D3TypeDescriptor
{
    class DT_CHARARRAY : BasicTypeDescriptor
    {
        public override void GenerateField(StringBuilder b, int pad, FieldDescriptor f)
        {
            b.Append(' ', pad); b.AppendLine("public string _" + f.GetFieldName() + ";");
            b.Append(' ', pad); b.AppendLine("public string " + f.GetFieldName() + " { get { return _" + f.GetFieldName() + "; } set { if(value != null && value.Length > " + f.ArrayLength + ") throw new ArgumentOutOfRangeException(); _" + f.GetFieldName() + " = value; } }");
        }

        public override void GenerateOptionalField(StringBuilder b, int pad, FieldDescriptor f)
        {
            GenerateField(b, pad, f);
        }

        public override void GenerateParseBitBuffer(StringBuilder b, int pad, FieldDescriptor f, string bitBufferName)
        {
            b.Append(' ', pad);
            b.AppendLine(f.GetFieldName() + " = " + bitBufferName + ".ReadCharArray(" + f.ArrayLength + ");");
        }

        public override void GenerateOptionalParseBitBuffer(StringBuilder b, int pad, FieldDescriptor f, string bitBufferName)
        {
            b.Append(' ', pad); b.AppendLine("if(" + bitBufferName + ".ReadBool())");
            GenerateParseBitBuffer(b, pad + 4, f, bitBufferName);
        }

        public override void GenerateEncodeBitBuffer(StringBuilder b, int pad, FieldDescriptor f, string bitBufferName)
        {
            b.Append(' ', pad); b.AppendLine(bitBufferName + ".WriteCharArray(" + f.ArrayLength + ", " + f.GetFieldName() + ");");
        }

        public override void GenerateOptionalEncodeBitBuffer(StringBuilder b, int pad, FieldDescriptor f, string bitBufferName)
        {
            b.Append(' ', pad); b.AppendLine("if(" + f.GetFieldName() + " != null)");
            b.Append(' ', pad+4); b.AppendLine(bitBufferName + ".WriteCharArray(" + f.ArrayLength + ", " + f.GetFieldName() + ");");
        }
    }
}
