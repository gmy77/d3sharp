using System.Text;

namespace D3Sharp.Net.Game.Message.Fields
{
    public class HirelingInfo
    {
        public int Field0;
        public int Field1;
        public int Field2;
        public int Field3;
        public bool Field4;
        public int Field5;
        public int Field6;
        public int Field7;
        public int Field8;

        public void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(2);
            Field1 = buffer.ReadInt(32);
            Field2 = buffer.ReadInt(7);
            Field3 = buffer.ReadInt(32);
            Field4 = buffer.ReadBool();
            Field5 = buffer.ReadInt(32);
            Field6 = buffer.ReadInt(32);
            Field7 = buffer.ReadInt(32);
            Field8 = buffer.ReadInt(32);
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(2, Field0);
            buffer.WriteInt(32, Field1);
            buffer.WriteInt(7, Field2);
            buffer.WriteInt(32, Field3);
            buffer.WriteBool(Field4);
            buffer.WriteInt(32, Field5);
            buffer.WriteInt(32, Field6);
            buffer.WriteInt(32, Field7);
            buffer.WriteInt(32, Field8);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("HirelingInfo:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad);
            b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad);
            b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad);
            b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', pad);
            b.AppendLine("Field3: 0x" + Field3.ToString("X8") + " (" + Field3 + ")");
            b.Append(' ', pad);
            b.AppendLine("Field4: " + (Field4 ? "true" : "false"));
            b.Append(' ', pad);
            b.AppendLine("Field5: 0x" + Field5.ToString("X8") + " (" + Field5 + ")");
            b.Append(' ', pad);
            b.AppendLine("Field6: 0x" + Field6.ToString("X8") + " (" + Field6 + ")");
            b.Append(' ', pad);
            b.AppendLine("Field7: 0x" + Field7.ToString("X8") + " (" + Field7 + ")");
            b.Append(' ', pad);
            b.AppendLine("Field8: 0x" + Field8.ToString("X8") + " (" + Field8 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }
    }
}