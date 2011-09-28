using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace D3TypeDescriptor
{
    class DT_FLOAT : BasicTypeDescriptor
    {
        public override void GenerateField(StringBuilder b, int pad, FieldDescriptor f)
        {
            b.Append(' ', pad); b.AppendLine("public float " + f.GetFieldName() + ";");
        }

        public override void GenerateOptionalField(StringBuilder b, int pad, FieldDescriptor f)
        {
            b.Append(' ', pad); b.AppendLine("public float? " + f.GetFieldName() + ";");
        }

        public override void GenerateParseBitBuffer(StringBuilder b, int pad, FieldDescriptor f, string bitBufferName)
        {
            if (f.HasMinMax)
                throw new NotImplementedException();
            if (f.Float16Encoding)
                throw new NotImplementedException();
            if (f.EncodedBits != 32)
                throw new NotImplementedException();
            b.Append(' ', pad);
            b.AppendLine(f.GetFieldName() + " = " + bitBufferName + ".ReadFloat32();");
        }

        public override void GenerateOptionalParseBitBuffer(StringBuilder b, int pad, FieldDescriptor f, string bitBufferName)
        {
            b.Append(' ', pad); b.AppendLine("if(" + bitBufferName + ".ReadBool())");
            GenerateParseBitBuffer(b, pad + 4, f, bitBufferName);
        }

        public override void GenerateEncodeBitBuffer(StringBuilder b, int pad, FieldDescriptor f, string bitBufferName)
        {
            if (f.HasMinMax)
                throw new NotImplementedException();
            if (f.Float16Encoding)
                throw new NotImplementedException();
            if (f.EncodedBits != 32)
                throw new NotImplementedException();
            b.Append(' ', pad); b.AppendLine(bitBufferName + ".WriteFloat32(" + f.GetFieldName() + ");");
        }

        public override void GenerateOptionalEncodeBitBuffer(StringBuilder b, int pad, FieldDescriptor f, string bitBufferName)
        {
            if (f.HasMinMax)
                throw new NotImplementedException();
            if (f.Float16Encoding)
                throw new NotImplementedException();
            if (f.EncodedBits != 32)
                throw new NotImplementedException();
            var fieldName = f.GetFieldName();
            b.Append(' ', pad); b.AppendLine("if(" + fieldName + ".HasValue)");
            b.Append(' ', pad+4); b.AppendLine(bitBufferName + ".WriteFloat32(" + f.GetFieldName() + ".Value);");
        }
    }
}
