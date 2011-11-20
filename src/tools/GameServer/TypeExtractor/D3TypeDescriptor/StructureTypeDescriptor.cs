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

using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace D3TypeDescriptor
{
    [TypeOverrideIgnore]
    public class StructureTypeDescriptor : TypeDescriptor
    {
        public FieldDescriptor[] Fields;

        public override void LoadFields(FieldDescriptor[] fields)
        {
            Fields = fields;
        }

        public override bool IsStructure
        {
            get
            {
                return true;
            }
        }

        public override System.Xml.Linq.XElement ToXml()
        {
            XElement element = base.ToXml();
            element.Name = "StructureDescriptor";

            if (this.Fields == null)
                return element;

            foreach (var field in this.Fields.OrderBy(x => x.Offset)) // order fields by offset.
                element.Add(field.ToXml());

            return element;
        }


        #region GenerateClass
        public override void GenerateClass(StringBuilder b, int pad)
        {
            b.Append(' ', pad); b.AppendLine("public class " + Name);
            b.Append(' ', pad); b.AppendLine("{");
            GenerateFieldsAndFunctions(b, pad + 4);
            b.Append(' ', pad); b.AppendLine("}");
        }

        public virtual void GenerateFieldsAndFunctions(StringBuilder b, int pad)
        {
            GenerateFields(b, pad);
            GenerateParseFunction(b, pad);
            GenerateEncodeFunction(b, pad);
        }

        public virtual void GenerateFields(StringBuilder b, int pad)
        {
            foreach (var f in Fields)
            {
                if (f.Type == null)
                    continue;
                f.Type.GenerateField(b, pad, f);
            }

            b.AppendLine();

        }

        public virtual void GenerateParseFunction(StringBuilder b, int pad)
        {
            b.Append(' ', pad); b.AppendLine("public void Parse(GameBitBuffer buffer)");
            b.Append(' ', pad); b.AppendLine("{");
            GenerateParseBody(b, pad + 4, "buffer");
            b.Append(' ', pad); b.AppendLine("}");
            b.AppendLine();
        }

        public virtual void GenerateParseBody(StringBuilder b, int pad, string bitBufferName)
        {
            foreach (var f in Fields)
            {
                if (f.Type == null)
                    continue;

                f.Type.GenerateParseBitBuffer(b, pad, f, bitBufferName);
            }
        }

        public virtual void GenerateEncodeFunction(StringBuilder b, int pad)
        {
            b.Append(' ', pad); b.AppendLine("public void Encode(GameBitBuffer buffer)");
            b.Append(' ', pad); b.AppendLine("{");
            GenerateEncodeBody(b, pad + 4, "buffer");
            b.Append(' ', pad); b.AppendLine("}");
            b.AppendLine();            
        }

        public virtual void GenerateEncodeBody(StringBuilder b, int pad, string bitBufferName)
        {
            foreach (var f in Fields)
            {
                if (f.Type == null)
                    continue;

                f.Type.GenerateEncodeBitBuffer(b, pad, f, bitBufferName);
            }
        }


        #endregion

        #region GenerateField
        public override void GenerateField(StringBuilder b, int pad, FieldDescriptor f)
        {
            b.Append(' ', pad); b.AppendLine("public " + Name + " " + f.GetFieldName() + ";");
        }

        public override void GenerateOptionalField(StringBuilder b, int pad, FieldDescriptor f)
        {
            GenerateField(b, pad, f);
        }

        public override void GenerateFixedArrayField(StringBuilder b, int pad, FieldDescriptor f)
        {
            b.Append(' ', pad); b.AppendLine(Name + "[] _" + f.GetFieldName() + ";");
            b.Append(' ', pad);
            b.AppendLine("public " + Name + "[] " + f.GetFieldName() + " { get { return _" + f.GetFieldName() + "; } set { if(value != null && value.Length " + (f.HasArrayLengthOffset ? "> " : "!= ") + f.ArrayLength + ") throw new ArgumentOutOfRangeException(); _" + f.GetFieldName() + " = value; } }");
        }

        public override void GenerateParseBitBuffer(StringBuilder b, int pad, FieldDescriptor f, string bitBufferName)
        {
            b.Append(' ', pad); b.AppendLine(f.GetFieldName() + " = new " + Name + "();");
            b.Append(' ', pad); b.AppendLine(f.GetFieldName() + ".Parse(" + bitBufferName + ");");
        }

        public override void GenerateOptionalParseBitBuffer(StringBuilder b, int pad, FieldDescriptor f, string bitBufferName)
        {
            b.Append(' ', pad); b.AppendLine("if(" + bitBufferName + ".ReadBool())");
            b.Append(' ', pad); b.AppendLine("{");
            GenerateParseBitBuffer(b, pad + 4, f, bitBufferName);
            b.Append(' ', pad); b.AppendLine("}");
        }

        public override void GenerateFixedArrayParseBitBuffer(StringBuilder b, int pad, FieldDescriptor f, string bitBufferName)
        {
            var name = f.GetFieldName();
            b.Append(' ', pad);
            if (f.HasArrayLengthOffset)
                b.AppendLine(name + " = new " + Name + "[" + bitBufferName + ".ReadInt(" + f.EncodedBits2 + ")];");
            else
                b.AppendLine(name + " = new " + Name + "[" + f.ArrayLength + "];");

            b.Append(' ', pad); b.AppendLine("for(int i = 0;i < _" + name + ".Length;i++)");
            b.Append(' ', pad); b.AppendLine("{");
            b.Append(' ', pad + 4); b.AppendLine("_" + name + "[i] = new " + Name + "();");
            b.Append(' ', pad + 4); b.AppendLine("_" + name + "[i].Parse(" + bitBufferName + ");");
            b.Append(' ', pad); b.AppendLine("}");

        }

        public override void GenerateEncodeBitBuffer(StringBuilder b, int pad, FieldDescriptor f, string bitBufferName)
        {
            b.Append(' ', pad); b.AppendLine(f.GetFieldName() + ".Encode(" + bitBufferName + ");");
        }

        public override void GenerateOptionalEncodeBitBuffer(StringBuilder b, int pad, FieldDescriptor f, string bitBufferName)
        {
            b.Append(' ', pad); b.AppendLine("if(" + f.GetFieldName() + " != null)");
            b.Append(' ', pad); b.AppendLine(f.GetFieldName() + ".Encode(" + bitBufferName + ");");
        }

        public override void GenerateFixedArrayEncodeBitBuffer(StringBuilder b, int pad, FieldDescriptor f, string bitBufferName)
        {
            var fieldName = f.GetFieldName();

            if (f.HasArrayLengthOffset)
            {
                b.Append(' ', pad);
                b.AppendLine(bitBufferName + ".WriteInt(" + f.EncodedBits2 + ", _" + f.GetFieldName() + ".Length);");
            }

            b.Append(' ', pad); b.AppendLine("for(int i = 0;i < _" + fieldName + ".Length;i++) _" + fieldName + "[i].Encode(" + bitBufferName + ");");
        }
        #endregion
    }
}
