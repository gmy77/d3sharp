using System;
using System.Collections.Generic;
using System.Linq;
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
