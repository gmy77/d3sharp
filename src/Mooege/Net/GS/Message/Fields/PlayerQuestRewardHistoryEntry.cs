using System.Text;

namespace D3Sharp.Net.Game.Message.Fields
{
    public class PlayerQuestRewardHistoryEntry
    {
        public int /* sno */ snoQuest;
        public int Field1;

        public enum eField2
        {
            Normal = 0,
            Nightmare = 1,
            Hell = 2,
            Inferno = 3,
        }

        public eField2 Field2;

        public void Parse(GameBitBuffer buffer)
        {
            snoQuest = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(32);
            Field2 = (eField2) buffer.ReadInt(2);
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, snoQuest);
            buffer.WriteInt(32, Field1);
            buffer.WriteInt(2, (int) Field2);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("PlayerQuestRewardHistoryEntry:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad);
            b.AppendLine("snoQuest: 0x" + snoQuest.ToString("X8"));
            b.Append(' ', pad);
            b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad);
            b.AppendLine("Field2: " + Field2.ToString());
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}