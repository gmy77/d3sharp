using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace D3TypeDescriptor
{
    class DT_OPTIONAL : BasicTypeDescriptor
    {
        public override void GenerateField(StringBuilder b, int pad, FieldDescriptor f)
        {
            f.SubType.GenerateOptionalField(b, pad, f);
        }        

        public override void GenerateEncodeBitBuffer(StringBuilder b, int pad, FieldDescriptor f, string bitBufferName)
        {
            f.SubType.GenerateOptionalEncodeBitBuffer(b, pad, f, bitBufferName);
        }

        public override void GenerateParseBitBuffer(StringBuilder b, int pad, FieldDescriptor f, string bitBufferName)
        {
            f.SubType.GenerateOptionalParseBitBuffer(b, pad, f, bitBufferName);
        }
    }
}
