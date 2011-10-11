using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace D3TypeDescriptor
{
    class NetAttributeKeyValue : StructureTypeDescriptor
    {
        [TypeOverrideIgnore]
        class AttributeFieldType : BasicTypeDescriptor
        {
            public AttributeFieldType()
            {
                Index = -1;
                Name = "GameAttribute";
            }

            public static AttributeFieldType Instance = new AttributeFieldType();


            public override void GenerateField(StringBuilder b, int pad, FieldDescriptor f)
            {
                b.Append(' ', pad); b.AppendLine("public GameAttribute " + f.GetFieldName() + ";");
            }

            public override void GenerateParseBitBuffer(StringBuilder b, int pad, FieldDescriptor f, string bitBufferName)
            {
                if (!f.HasMinMax || f.Min != 0)
                    throw new NotImplementedException();                
                b.Append(' ', pad); b.AppendLine(f.GetFieldName() + " = GameAttribute.Attributes[" + bitBufferName + ".ReadInt(" + f.EncodedBits + ")];");
            }

            public override void GenerateEncodeBitBuffer(StringBuilder b, int pad, FieldDescriptor f, string bitBufferName)
            {
                b.Append(' ', pad); b.AppendLine(bitBufferName + ".WriteInt(" + f.EncodedBits + ", " + f.GetFieldName() + ".Id);");
            }
        }

        public override void LoadFields(FieldDescriptor[] fields)
        {
            if (fields.Length != 3 ||
                fields[0].Type.Name != "DT_OPTIONAL" || fields[0].SubType.Name != "DT_INT" ||
                fields[1].Type.Name != "DT_INT" ||
                fields[2].Type != null)
                throw new Exception("Unexpected fields in NetAttributeKeyValue structure.");
            fields[1].Type = AttributeFieldType.Instance;

            base.LoadFields(fields);
        }

        public override void GenerateFields(StringBuilder b, int pad)
        {
            base.GenerateFields(b, pad);
            b.Append(' ', pad); b.AppendLine("public float Float; ");
            b.Append(' ', pad); b.AppendLine("public int Int; ");
            b.AppendLine();
        }

        public override void GenerateFieldsAndFunctions(StringBuilder b, int pad)
        {
            base.GenerateFieldsAndFunctions(b, pad);

            var fieldName = Fields[1].GetFieldName();

            b.IndentAppendLines(pad, @"public void ParseValue(GameBitBuffer buffer)
{
    switch (" + fieldName + @".EncodingType)
    {
        case GameAttributeEncoding.Int:
            Int = buffer.ReadInt(" + fieldName + @".BitCount);
            break;
        case GameAttributeEncoding.IntMinMax:
            Int = buffer.ReadInt(" + fieldName + @".BitCount) + " + fieldName + @".Min;
            break;
        case GameAttributeEncoding.Float16:
            Float = buffer.ReadFloat16();
            break;
        case GameAttributeEncoding.Float16Or32:
            Float = buffer.ReadBool() ? buffer.ReadFloat16() : buffer.ReadFloat32();
            break;
        default:
            throw new Exception(""bad voodoo"");
    }
}");
            b.AppendLine();

            b.IndentAppendLines(pad, @"public void EncodeValue(GameBitBuffer buffer)
{
    switch (" + fieldName + @".EncodingType)
    {
        case GameAttributeEncoding.Int:
            buffer.WriteInt(" + fieldName + @".BitCount, Int);
            break;
        case GameAttributeEncoding.IntMinMax:
            buffer.WriteInt(" + fieldName + @".BitCount, Int - " + fieldName + @".Min);
            break;
        case GameAttributeEncoding.Float16:
            buffer.WriteFloat16(Float);
            break;
        case GameAttributeEncoding.Float16Or32:
            if (Float >= 65536.0f || -65536.0f >= Float)
            {
                buffer.WriteBool(false);
                buffer.WriteFloat32(Float);
            }
            else
            {
                buffer.WriteBool(true);
                buffer.WriteFloat16(Float);
            }
            break;
        default:
            throw new Exception(""bad voodoo"");
    }
}");


        }
    }
}
