using System.Text;

namespace D3Sharp.Net.Game.Message.Fields
{
    public class VisualEquipment
    {
        // MaxLength = 8
        public VisualItem[] Field0;

        public void Parse(GameBitBuffer buffer)
        {
            Field0 = new VisualItem[8];
            for (int i = 0; i < Field0.Length; i++)
            {
                Field0[i] = new VisualItem();
                Field0[i].Parse(buffer);
            }
        }

        public void Encode(GameBitBuffer buffer)
        {
            for (int i = 0; i < Field0.Length; i++)
            {
                Field0[i].Encode(buffer);
            }
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("VisualEquipment:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad);
            b.AppendLine("Field0:");
            b.Append(' ', pad);
            b.AppendLine("{");
            for (int i = 0; i < Field0.Length; i++)
            {
                Field0[i].AsText(b, pad + 1);
                b.AppendLine();
            }
            b.Append(' ', pad);
            b.AppendLine("}");
            b.AppendLine();
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}