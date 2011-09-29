using System.Text;

namespace D3Sharp.Net.Game.Message.Fields
{
    public class EntityId
    {
        public long Field0;
        public long Field1;

        public void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt64(64);
            Field1 = buffer.ReadInt64(64);
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt64(64, Field0);
            buffer.WriteInt64(64, Field1);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("EntityId:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad);
            b.AppendLine("Field0: 0x" + Field0.ToString("X16"));
            b.Append(' ', pad);
            b.AppendLine("Field1: 0x" + Field1.ToString("X16"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}