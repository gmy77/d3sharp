using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace D3TypeDescriptor.Overrides
{
    class AttributesSetValuesMessage : GameMessageDescriptor
    {
        public override void LoadFields(FieldDescriptor[] fields)
        {
            base.LoadFields(fields); // removes RequiredMessageHeader
            if (Fields.Length != 3 ||
                Fields[0].Type.Name != "DT_INT" ||
                Fields[1].Type.Name != "DT_FIXEDARRAY" || Fields[1].SubType.Name != "NetAttributeKeyValue" ||
                Fields[2].Type != null)
                throw new Exception("Unexpected fields in AttributesSetValuesMessage.");
        }

        public override void GenerateParseBody(StringBuilder b, int pad, string bitBufferName)
        {
            base.GenerateParseBody(b, pad, bitBufferName);
            var fieldName = Fields[1].GetFieldName();
            b.IndentAppendLine(pad, "for (int i = 0; i < " + fieldName + ".Length; i++) { " + fieldName + "[i].ParseValue(" + bitBufferName + "); };");
        }

        public override void GenerateEncodeBody(StringBuilder b, int pad, string bitBufferName)
        {
            base.GenerateEncodeBody(b, pad, bitBufferName);
            var fieldName = Fields[1].GetFieldName();
            b.IndentAppendLine(pad, "for (int i = 0; i < " + fieldName + ".Length; i++) { " + fieldName + "[i].EncodeValue(" + bitBufferName + "); };");
        }
    }
}
