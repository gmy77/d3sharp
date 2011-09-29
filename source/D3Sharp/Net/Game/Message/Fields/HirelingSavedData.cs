using System.Text;

namespace D3Sharp.Net.Game.Message.Fields
{
    public class HirelingSavedData
    {
        // MaxLength = 4
        public HirelingInfo[] HirelingInfos;
        public int Field1;
        public int Field2;

        public void Parse(GameBitBuffer buffer)
        {
            HirelingInfos = new HirelingInfo[4];
            for (int i = 0; i < HirelingInfos.Length; i++)
            {
                HirelingInfos[i] = new HirelingInfo();
                HirelingInfos[i].Parse(buffer);
            }
            Field1 = buffer.ReadInt(2);
            Field2 = buffer.ReadInt(32);
        }

        public void Encode(GameBitBuffer buffer)
        {
            for (int i = 0; i < HirelingInfos.Length; i++)
            {
                HirelingInfos[i].Encode(buffer);
            }
            buffer.WriteInt(2, Field1);
            buffer.WriteInt(32, Field2);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("HirelingSavedData:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad);
            b.AppendLine("Field0:");
            b.Append(' ', pad);
            b.AppendLine("{");
            for (int i = 0; i < HirelingInfos.Length; i++)
            {
                HirelingInfos[i].AsText(b, pad + 1);
                b.AppendLine();
            }
            b.Append(' ', pad);
            b.AppendLine("}");
            b.AppendLine();
            b.Append(' ', pad);
            b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad);
            b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}