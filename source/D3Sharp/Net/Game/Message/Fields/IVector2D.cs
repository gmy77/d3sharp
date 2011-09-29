using System.Text;

namespace D3Sharp.Net.Game.Message.Fields
{
    public class IVector2D
    {
        public int x;
        public int y;

        public void Parse(GameBitBuffer buffer)
        {
            x = buffer.ReadInt(32);
            y = buffer.ReadInt(32);
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, x);
            buffer.WriteInt(32, y);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("IVector2D:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad);
            b.AppendLine("Field0: 0x" + x.ToString("X8") + " (" + x + ")");
            b.Append(' ', pad);
            b.AppendLine("Field1: 0x" + y.ToString("X8") + " (" + y + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}