using System.Text;

namespace D3Sharp.Net.Game.Message.Fields
{
    public class RGBAColor
    {
        public byte Field0;
        public byte Field1;
        public byte Field2;
        public byte Field3;

        public void Parse(GameBitBuffer buffer)
        {
            Field0 = (byte) buffer.ReadInt(8);
            Field1 = (byte) buffer.ReadInt(8);
            Field2 = (byte) buffer.ReadInt(8);
            Field3 = (byte) buffer.ReadInt(8);
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(8, Field0);
            buffer.WriteInt(8, Field1);
            buffer.WriteInt(8, Field2);
            buffer.WriteInt(8, Field3);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("RGBAColor:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad);
            b.AppendLine("Field0: 0x" + Field0.ToString("X2"));
            b.Append(' ', pad);
            b.AppendLine("Field1: 0x" + Field1.ToString("X2"));
            b.Append(' ', pad);
            b.AppendLine("Field2: 0x" + Field2.ToString("X2"));
            b.Append(' ', pad);
            b.AppendLine("Field3: 0x" + Field3.ToString("X2"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}