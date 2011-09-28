using System.Text;

namespace D3Sharp.Net.Game.Message.Fields
{
    public class PRTransform
    {
        public Quaternion Field0;
        public Vector3D Field1;

        public void Parse(GameBitBuffer buffer)
        {
            Field0 = new Quaternion();
            Field0.Parse(buffer);
            Field1 = new Vector3D();
            Field1.Parse(buffer);
        }

        public void Encode(GameBitBuffer buffer)
        {
            Field0.Encode(buffer);
            Field1.Encode(buffer);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("PRTransform:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            Field0.AsText(b, pad);
            Field1.AsText(b, pad);
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}