using System.Text;
using D3Sharp.Net.Game.Messages;

namespace D3Sharp.Net.Game.Message.Fields
{
    public class Quaternion
    {
        public float Field0;
        public Vector3D Field1;

        public void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadFloat32();
            Field1 = new Vector3D();
            Field1.Parse(buffer);
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteFloat32(Field0);
            Field1.Encode(buffer);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("Quaternion:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad);
            b.AppendLine("Field0: " + Field0.ToString("G"));
            Field1.AsText(b, pad);
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}