using System.Text;

namespace D3Sharp.Net.Game.Message.Fields
{
    public class LearnedLore
    {
        public int Field0;
        // MaxLength = 256
        public int /* sno */[] m_snoLoreLearned;

        public void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            m_snoLoreLearned = new int /* sno */[256];
            for (int i = 0; i < m_snoLoreLearned.Length; i++) m_snoLoreLearned[i] = buffer.ReadInt(32);
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            for (int i = 0; i < m_snoLoreLearned.Length; i++) buffer.WriteInt(32, m_snoLoreLearned[i]);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("LearnedLore:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad);
            b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad);
            b.AppendLine("m_snoLoreLearned:");
            b.Append(' ', pad);
            b.AppendLine("{");
            for (int i = 0; i < m_snoLoreLearned.Length;)
            {
                b.Append(' ', pad + 1);
                for (int j = 0; j < 8 && i < m_snoLoreLearned.Length; j++, i++)
                {
                    b.Append("0x" + m_snoLoreLearned[i].ToString("X8") + ", ");
                }
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