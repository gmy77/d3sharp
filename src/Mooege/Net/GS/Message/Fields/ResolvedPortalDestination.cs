using System.Text;

namespace D3Sharp.Net.Game.Message.Fields
{
    public class ResolvedPortalDestination
    {
        public int /* sno */ snoWorld;
        public int Field1;
        public int /* sno */ snoDestLevelArea;

        public void Parse(GameBitBuffer buffer)
        {
            snoWorld = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(32);
            snoDestLevelArea = buffer.ReadInt(32);
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, snoWorld);
            buffer.WriteInt(32, Field1);
            buffer.WriteInt(32, snoDestLevelArea);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("ResolvedPortalDestination:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad);
            b.AppendLine("snoWorld: 0x" + snoWorld.ToString("X8"));
            b.Append(' ', pad);
            b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad);
            b.AppendLine("snoDestLevelArea: 0x" + snoDestLevelArea.ToString("X8"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}