using System;
using System.Text;

namespace D3Sharp.Net.Game.Message.Fields
{
    public class NetAttributeKeyValue
    {
        public int? Field0;
        //public int Field1;
        public GameAttribute Attribute;
        public int Int;
        public float Float;

        public void Parse(GameBitBuffer buffer)
        {
            if (buffer.ReadBool())
            {
                Field0 = buffer.ReadInt(20);
            }
            int index = buffer.ReadInt(10) & 0xFFF;

            Attribute = GameAttribute.Attributes[index];
        }

        public void ParseValue(GameBitBuffer buffer)
        {
            switch (Attribute.EncodingType)
            {
                case GameAttributeEncoding.Int:
                    Int = buffer.ReadInt(Attribute.BitCount);
                    break;
                case GameAttributeEncoding.IntMinMax:
                    Int = buffer.ReadInt(Attribute.BitCount) + Attribute.Min;
                    break;
                case GameAttributeEncoding.Float16:
                    Float = buffer.ReadFloat16();
                    break;
                case GameAttributeEncoding.Float16Or32:
                    Float = buffer.ReadBool() ? buffer.ReadFloat16() : buffer.ReadFloat32();
                    break;
                default:
                    throw new Exception("bad voodoo");
            }
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteBool(Field0.HasValue);
            if (Field0.HasValue)
            {
                buffer.WriteInt(20, Field0.Value);
            }
            buffer.WriteInt(10, Attribute.Id);
        }

        public void EncodeValue(GameBitBuffer buffer)
        {
            switch (Attribute.EncodingType)
            {
                case GameAttributeEncoding.Int:
                    buffer.WriteInt(Attribute.BitCount, Int);
                    break;
                case GameAttributeEncoding.IntMinMax:
                    buffer.WriteInt(Attribute.BitCount, Int - Attribute.Min);
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
                    throw new Exception("bad voodoo");
            }
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("NetAttributeKeyValue:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            if (Field0.HasValue)
            {
                b.Append(' ', pad);
                b.AppendLine("Field0.Value: 0x" + Field0.Value.ToString("X8") + " (" + Field0.Value + ")");
            }
            b.Append(' ', pad);
            b.Append(Attribute.Name);
            b.Append(" (" + Attribute.Id + "): ");

            if (Attribute.IsInteger)
                b.AppendLine("0x" + Int.ToString("X8") + " (" + Int + ")");
            else
                b.AppendLine(Float.ToString("G"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}