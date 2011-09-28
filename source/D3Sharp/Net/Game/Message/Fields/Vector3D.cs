using System.Text;

namespace D3Sharp.Net.Game.Message.Fields
{
    public class Vector3D
    {
        public float Field0;
        public float Field1;
        public float Field2;

        public void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadFloat32();
            Field1 = buffer.ReadFloat32();
            Field2 = buffer.ReadFloat32();
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteFloat32(Field0);
            buffer.WriteFloat32(Field1);
            buffer.WriteFloat32(Field2);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("Vector3D:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad);
            b.AppendLine("Field0: " + Field0.ToString("G"));
            b.Append(' ', pad);
            b.AppendLine("Field1: " + Field1.ToString("G"));
            b.Append(' ', pad);
            b.AppendLine("Field2: " + Field2.ToString("G"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}