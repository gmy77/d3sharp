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

namespace D3TypeDescriptor
{
    class DT_INT : BasicTypeDescriptor
    {
        public override void GenerateField(StringBuilder b, int pad, FieldDescriptor f)
        {
            if (f.HasMinMax)
            {
                if (f.Min == 0 && f.Max == 1)
                {
                    b.Append(' ', pad); b.AppendLine("public bool " + f.GetFieldName() + ";");
                }
                else
                {
                    var name = f.GetFieldName();
                    b.Append(' ', pad); b.AppendLine("int _" + name + ";");
                    b.Append(' ', pad); b.AppendLine("public int " + name + " { get { return _" + name + "; } set { if(value < " + f.Min.ToMaybeHexString(1024) + " || value > " + f.Max.ToMaybeHexString(1024) + ") throw new ArgumentOutOfRangeException(); _" + name + " = value; } }");                    
                }
            }
            else
            {
                b.Append(' ', pad); b.AppendLine("public int " + f.GetFieldName() + ";");
            }
        }

        public override void GenerateOptionalField(StringBuilder b, int pad, FieldDescriptor f)
        {
            if (f.HasMinMax)
            {
                if (f.Min == 0 && f.Max == 1)
                {
                    b.Append(' ', pad); b.AppendLine("public bool? " + f.GetFieldName() + ";");
                }
                else
                {
                    var name = f.GetFieldName();
                    b.Append(' ', pad); b.AppendLine("int? _" + name + ";");
                    b.Append(' ', pad); b.AppendLine("public int? " + name + " { get { return _" + name + "; } set { if(value.HasValue && (value.Value < " + f.Min.ToMaybeHexString(1024) + " || value.Value > " + f.Max.ToMaybeHexString(1024) + ")) throw new ArgumentOutOfRangeException(); _" + name + " = value; } }");
                }
            }
            else
            {
                b.Append(' ', pad); b.AppendLine("public int? " + f.GetFieldName() + ";");
            }
        }

        public override void GenerateFixedArrayField(StringBuilder b, int pad, FieldDescriptor f)
        {
            b.Append(' ', pad); b.AppendLine("int[] _" + f.GetFieldName() + ";");
            b.Append(' ', pad); 
            b.AppendLine("public int[] " + f.GetFieldName() + " { get { return _" + f.GetFieldName() + "; } set { if(value != null && value.Length " + (f.HasArrayLengthOffset ? "> " : "!= ") + f.ArrayLength + ") throw new ArgumentOutOfRangeException(); _" + f.GetFieldName() + " = value; } }");
        }

        public override void GenerateParseBitBuffer(StringBuilder b, int pad, FieldDescriptor f, string bitBufferName)
        {
            var fieldName = f.GetFieldName();
            b.Append(' ', pad); b.Append(fieldName + " = ");
            if (f.HasMinMax)
            {
                if (f.Min == 0 && f.Max == 1)
                {
                    b.AppendLine(bitBufferName + ".ReadBool();");
                }
                else
                {
                    if(f.Min != 0)
                        b.AppendLine(bitBufferName + ".ReadInt(" + f.EncodedBits + ") + (" + f.Min + ");");
                    else
                        b.AppendLine(bitBufferName + ".ReadInt(" + f.EncodedBits + ");");
                }
            }
            else
            {
                b.AppendLine(bitBufferName + ".ReadInt(" + f.EncodedBits + ");");
            }
        }

        public override void GenerateOptionalParseBitBuffer(StringBuilder b, int pad, FieldDescriptor f, string bitBufferName)
        {
            b.Append(' ', pad); b.AppendLine("if(" + bitBufferName + ".ReadBool())");
            GenerateParseBitBuffer(b, pad + 4, f, bitBufferName);
        }

        public override void GenerateFixedArrayParseBitBuffer(StringBuilder b, int pad, FieldDescriptor f, string bitBufferName)
        {
            if (f.HasMinMax)
                throw new NotImplementedException();

            var fieldName = f.GetFieldName();
            b.Append(' ', pad);
            if (f.HasArrayLengthOffset)
                b.AppendLine(fieldName + " = new int[" + bitBufferName + ".ReadInt(" + f.EncodedBits2 + ")];");
            else
                b.AppendLine(fieldName + " = new int[" + f.ArrayLength + "];");
            b.Append(' ', pad); b.AppendLine("for(int i = 0;i < _" + fieldName + ".Length;i++) _" + fieldName + "[i] = " + bitBufferName + ".ReadInt(" + f.EncodedBits + ");");
        }

        public override void GenerateEncodeBitBuffer(StringBuilder b, int pad, FieldDescriptor f, string bitBufferName)
        {
            if (f.HasMinMax)
            {
                if (f.Min == 0 && f.Max == 1)
                {
                    b.Append(' ', pad); b.AppendLine(bitBufferName + ".WriteBool(" + f.GetFieldName() + ");");
                }
                else
                {
                    b.Append(' ', pad); 
                    if(f.Min != 0)
                        b.AppendLine(bitBufferName + ".WriteInt(" + f.EncodedBits + ", " + f.GetFieldName() + " - (" + f.Min + "));");
                    else
                        b.AppendLine(bitBufferName + ".WriteInt(" + f.EncodedBits + ", " + f.GetFieldName() + ");");
                }
            }
            else
            {
                b.Append(' ', pad); b.AppendLine(bitBufferName + ".WriteInt(" + f.EncodedBits + ", " + f.GetFieldName() + ");");
            }            
        }

        public override void GenerateOptionalEncodeBitBuffer(StringBuilder b, int pad, FieldDescriptor f, string bitBufferName)
        {
            b.Append(' ', pad); b.AppendLine("if(" + f.GetFieldName() + ".HasValue)");
            b.Append(' ', pad + 4); 
            if (f.HasMinMax)
            {
                if (f.Min == 0 && f.Max == 1)
                {
                    b.AppendLine(bitBufferName + ".WriteBool(" + f.GetFieldName() + ".Value);");
                }
                else
                {
                    if (f.Min != 0)
                        b.AppendLine(bitBufferName + ".WriteInt(" + f.EncodedBits + ", " + f.GetFieldName() + ".Value - (" + f.Min + "));");
                    else
                        b.AppendLine(bitBufferName + ".WriteInt(" + f.EncodedBits + ", " + f.GetFieldName() + ".Value);");
                }
            }
            else
            {
                b.AppendLine(bitBufferName + ".WriteInt(" + f.EncodedBits + ", " + f.GetFieldName() + ".Value);");
            }            

        }

        public override void GenerateFixedArrayEncodeBitBuffer(StringBuilder b, int pad, FieldDescriptor f, string bitBufferName)
        {
            if (f.HasMinMax)
                throw new NotImplementedException();

            var fieldName = f.GetFieldName();

            if (f.HasArrayLengthOffset)
            {
                b.Append(' ', pad);
                b.AppendLine(bitBufferName + ".WriteInt(" + f.EncodedBits2 + ", " + f.GetFieldName() + ".Length);");
            }
            b.Append(' ', pad); b.AppendLine("for(int i = 0;i < _" + fieldName + ".Length;i++) " + bitBufferName + ".WriteInt(" + f.EncodedBits + ", _" + fieldName + "[i]);");
        }
    }
}
