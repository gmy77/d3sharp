using System.Text;

namespace Mooege.Net.GS.Message.Fields
{
    public class VisualEquipment
    {
        // MaxLength = 8
        public VisualItem[] Equipments;

        public void Parse(GameBitBuffer buffer)
        {
            Equipments = new VisualItem[8];
            for (int i = 0; i < Equipments.Length; i++)
            {
                Equipments[i] = new VisualItem();
                Equipments[i].Parse(buffer);
            }
        }

        public void Encode(GameBitBuffer buffer)
        {
            for (int i = 0; i < Equipments.Length; i++)
            {
                Equipments[i].Encode(buffer);
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
            for (int i = 0; i < Equipments.Length; i++)
            {
                Equipments[i].AsText(b, pad + 1);
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