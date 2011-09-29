using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

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
