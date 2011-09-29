using System.Text;

namespace D3Sharp.Net.Game.Message.Fields
{
    public class HotbarButtonData
    {
        public int /* sno */ m_snoPower;
        public int /* gbid */ m_gbidItem;

        public void Parse(GameBitBuffer buffer)
        {
            m_snoPower = buffer.ReadInt(32);
            m_gbidItem = buffer.ReadInt(32);
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, m_snoPower);
            buffer.WriteInt(32, m_gbidItem);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("HotbarButtonData:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad);
            b.AppendLine("m_snoPower: 0x" + m_snoPower.ToString("X8"));
            b.Append(' ', pad);
            b.AppendLine("m_gbidItem: 0x" + m_gbidItem.ToString("X8"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}