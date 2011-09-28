using System.Text;

namespace D3Sharp.Net.Game.Message.Fields
{
    public class Vector3D
    {
        public float X;
        public float Y;
        public float Z;

        public Vector3D(){}

        public Vector3D(float x, float y, float z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public void Parse(GameBitBuffer buffer)
        {
            X = buffer.ReadFloat32();
            Y = buffer.ReadFloat32();
            Z = buffer.ReadFloat32();
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteFloat32(X);
            buffer.WriteFloat32(Y);
            buffer.WriteFloat32(Z);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("Vector3D:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad);
            b.AppendLine("Field0: " + X.ToString("G"));
            b.Append(' ', pad);
            b.AppendLine("Field1: " + Y.ToString("G"));
            b.Append(' ', pad);
            b.AppendLine("Field2: " + Z.ToString("G"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }

        public override string ToString()
        {
            return string.Format("x:{0} y:{1} z:{2}",X, Y,Z);
        }
    }
}