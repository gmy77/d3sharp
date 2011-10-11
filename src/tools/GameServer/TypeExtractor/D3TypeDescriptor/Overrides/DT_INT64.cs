using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace D3TypeDescriptor
{
    class DT_INT64 : BasicTypeDescriptor
    {
        public override void GenerateField(StringBuilder b, int pad, FieldDescriptor f)
        {
            if (f.HasMinMax)
                throw new NotImplementedException();
            b.Append(' ', pad); b.AppendLine("public long " + f.GetFieldName() + ";");
        }

        public override void GenerateParseBitBuffer(StringBuilder b, int pad, FieldDescriptor f, string bitBufferName)
        {
            if (f.HasMinMax)
                throw new NotImplementedException();
            b.Append(' ', pad); 
            b.AppendLine(f.GetFieldName() + " = " + bitBufferName + ".ReadInt64(" + f.EncodedBits + ");");
        }

        public override void GenerateEncodeBitBuffer(StringBuilder b, int pad, FieldDescriptor f, string bitBufferName)
        {
            if (f.HasMinMax)
                throw new NotImplementedException();
            b.Append(' ', pad);
            b.AppendLine(bitBufferName + ".WriteInt64(" + f.EncodedBits + ", " + f.GetFieldName() + ");");
        }

    }
}
