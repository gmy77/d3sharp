using System.Text;

namespace D3Sharp.Net.Game.Message.Fields
{
    public class Quaternion
    {
        public float Amount;
        public Vector3D Axis;

        public void Parse(GameBitBuffer buffer)
        {
            Amount = buffer.ReadFloat32();
            Axis = new Vector3D();
            Axis.Parse(buffer);
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteFloat32(Amount);
            Axis.Encode(buffer);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("Quaternion:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad);
            b.AppendLine("Field0: " + Amount.ToString("G"));
            Axis.AsText(b, pad);
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}