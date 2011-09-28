using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace D3TypeDescriptor
{
    class DT_BYTE : BasicTypeDescriptor
    {
        public override void GenerateField(StringBuilder b, int pad, FieldDescriptor f)
        {
            b.Append(' ', pad); b.AppendLine("public byte " + f.GetFieldName() + ";");
        }

        public override void GenerateParseBitBuffer(StringBuilder b, int pad, FieldDescriptor f, string bitBufferName)
        {
            if (f.HasMinMax)
                throw new NotImplementedException();
            b.Append(' ', pad); b.AppendLine(f.GetFieldName() + " = (byte)" + bitBufferName + ".ReadInt(" + f.EncodedBits + ");");
        }

        public override void GenerateEncodeBitBuffer(StringBuilder b, int pad, FieldDescriptor f, string bitBufferName)
        {
            if (f.HasMinMax)
                throw new NotImplementedException();
            b.Append(' ', pad); b.AppendLine(bitBufferName + ".WriteInt(" + f.EncodedBits + ", " + f.GetFieldName() + ");");
        }
    }
}
