using System.Text;
using D3Sharp.Net.Game.Messages;

namespace D3Sharp.Net.Game.Message.Fields
{
    public class PlayerSavedData
    {
        // MaxLength = 9
        public HotbarButtonData[] Field0;
        // MaxLength = 15
        public SkillKeyMapping[] Field1;
        public int /* time */ Field2;
        public int Field3;
        public HirelingSavedData Field4;
        public int Field5;
        public LearnedLore Field6;
        // MaxLength = 6
        public int /* sno */[] snoActiveSkills;
        // MaxLength = 3
        public int /* sno */[] snoTraits;
        public SavePointData Field9;
        // MaxLength = 64
        public int /* sno */[] m_SeenTutorials;

        public void Parse(GameBitBuffer buffer)
        {
            Field0 = new HotbarButtonData[9];
            for (int i = 0; i < Field0.Length; i++)
            {
                Field0[i] = new HotbarButtonData();
                Field0[i].Parse(buffer);
            }
            Field1 = new SkillKeyMapping[15];
            for (int i = 0; i < Field1.Length; i++)
            {
                Field1[i] = new SkillKeyMapping();
                Field1[i].Parse(buffer);
            }
            Field2 = buffer.ReadInt(32);
            Field3 = buffer.ReadInt(32);
            Field4 = new HirelingSavedData();
            Field4.Parse(buffer);
            Field5 = buffer.ReadInt(32);
            Field6 = new LearnedLore();
            Field6.Parse(buffer);
            snoActiveSkills = new int /* sno */[6];
            for (int i = 0; i < snoActiveSkills.Length; i++) snoActiveSkills[i] = buffer.ReadInt(32);
            snoTraits = new int /* sno */[3];
            for (int i = 0; i < snoTraits.Length; i++) snoTraits[i] = buffer.ReadInt(32);
            Field9 = new SavePointData();
            Field9.Parse(buffer);
            m_SeenTutorials = new int /* sno */[64];
            for (int i = 0; i < m_SeenTutorials.Length; i++) m_SeenTutorials[i] = buffer.ReadInt(32);
        }

        public void Encode(GameBitBuffer buffer)
        {
            for (int i = 0; i < Field0.Length; i++)
            {
                Field0[i].Encode(buffer);
            }
            for (int i = 0; i < Field1.Length; i++)
            {
                Field1[i].Encode(buffer);
            }
            buffer.WriteInt(32, Field2);
            buffer.WriteInt(32, Field3);
            Field4.Encode(buffer);
            buffer.WriteInt(32, Field5);
            Field6.Encode(buffer);
            for (int i = 0; i < snoActiveSkills.Length; i++) buffer.WriteInt(32, snoActiveSkills[i]);
            for (int i = 0; i < snoTraits.Length; i++) buffer.WriteInt(32, snoTraits[i]);
            Field9.Encode(buffer);
            for (int i = 0; i < m_SeenTutorials.Length; i++) buffer.WriteInt(32, m_SeenTutorials[i]);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("PlayerSavedData:");
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
            b.Append(' ', pad);
            b.AppendLine("Field1:");
            b.Append(' ', pad);
            b.AppendLine("{");
            for (int i = 0; i < Field1.Length; i++)
            {
                Field1[i].AsText(b, pad + 1);
                b.AppendLine();
            }
            b.Append(' ', pad);
            b.AppendLine("}");
            b.AppendLine();
            b.Append(' ', pad);
            b.AppendLine("Field2: 0x" + Field2.ToString("X8"));
            b.Append(' ', pad);
            b.AppendLine("Field3: 0x" + Field3.ToString("X8") + " (" + Field3 + ")");
            Field4.AsText(b, pad);
            b.Append(' ', pad);
            b.AppendLine("Field5: 0x" + Field5.ToString("X8") + " (" + Field5 + ")");
            Field6.AsText(b, pad);
            b.Append(' ', pad);
            b.AppendLine("snoActiveSkills:");
            b.Append(' ', pad);
            b.AppendLine("{");
            for (int i = 0; i < snoActiveSkills.Length;)
            {
                b.Append(' ', pad + 1);
                for (int j = 0; j < 8 && i < snoActiveSkills.Length; j++, i++)
                {
                    b.Append("0x" + snoActiveSkills[i].ToString("X8") + ", ");
                }
                b.AppendLine();
            }
            b.Append(' ', pad);
            b.AppendLine("}");
            b.AppendLine();
            b.Append(' ', pad);
            b.AppendLine("snoTraits:");
            b.Append(' ', pad);
            b.AppendLine("{");
            for (int i = 0; i < snoTraits.Length;)
            {
                b.Append(' ', pad + 1);
                for (int j = 0; j < 8 && i < snoTraits.Length; j++, i++)
                {
                    b.Append("0x" + snoTraits[i].ToString("X8") + ", ");
                }
                b.AppendLine();
            }
            b.Append(' ', pad);
            b.AppendLine("}");
            b.AppendLine();
            Field9.AsText(b, pad);
            b.Append(' ', pad);
            b.AppendLine("m_SeenTutorials:");
            b.Append(' ', pad);
            b.AppendLine("{");
            for (int i = 0; i < m_SeenTutorials.Length;)
            {
                b.Append(' ', pad + 1);
                for (int j = 0; j < 8 && i < m_SeenTutorials.Length; j++, i++)
                {
                    b.Append("0x" + m_SeenTutorials[i].ToString("X8") + ", ");
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