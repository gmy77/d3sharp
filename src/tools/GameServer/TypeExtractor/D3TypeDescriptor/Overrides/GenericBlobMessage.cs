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

 using System;
 using System.Text;

namespace D3TypeDescriptor.Overrides
{
    class GenericBlobMessage : GameMessageDescriptor
    {
        public override void LoadFields(FieldDescriptor[] fields)
        {
            base.LoadFields(fields); // removes RequiredMessageHeader

            if (Fields.Length != 2 ||
                Fields[0].Type.Name != "DT_INT" || Fields[0].HasMinMax != false && Fields[0].EncodedBits != 32 ||
                Fields[1].Type != null)
                throw new Exception("Unexpected fields in GenericBlobMessage");
        }

        public override void GenerateFields(StringBuilder b, int pad)
        {
            b.IndentAppendLine(pad, "public byte[] Data;");
        }

        public override void GenerateParseBody(StringBuilder b, int pad, string bitBufferName)
        {
            b.IndentAppendLine(pad, "Data = " + bitBufferName + ".ReadBlob(32);");
        }

        public override void GenerateEncodeBody(StringBuilder b, int pad, string bitBufferName)
        {
            b.IndentAppendLine(pad, bitBufferName + ".WriteBlob(32, Data);");
        }
    }
}
