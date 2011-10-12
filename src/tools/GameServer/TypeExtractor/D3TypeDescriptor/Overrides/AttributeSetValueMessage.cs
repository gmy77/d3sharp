using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace D3TypeDescriptor.Overrides
{
    class AttributeSetValueMessage : GameMessageDescriptor
    {
        public override void LoadFields(FieldDescriptor[] fields)
        {
            base.LoadFields(fields); // removes RequiredMessageHeader
            if (this.Fields.Length != 3 ||
                this.Fields[0].Type.Name != "DT_INT" ||
                this.Fields[1].Type.Name != "NetAttributeKeyValue" ||
                this.Fields[2].Type != null)
                throw new Exception("Unexpected fields in AttributeSetValueMessage");
        }

        public override void GenerateParseBody(StringBuilder b, int pad, string bitBufferName)
        {
            base.GenerateParseBody(b, pad, bitBufferName);
            b.IndentAppendLine(pad, Fields[1].GetFieldName() + ".ParseValue(" + bitBufferName + ");");            
        }

        public override void GenerateEncodeBody(StringBuilder b, int pad, string bitBufferName)
        {
            base.GenerateEncodeBody(b, pad, bitBufferName);
            b.IndentAppendLine(pad, Fields[1].GetFieldName() + ".EncodeValue(" + bitBufferName + ");");
        }
    }
}
