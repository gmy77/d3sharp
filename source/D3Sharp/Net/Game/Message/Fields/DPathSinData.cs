using System.Text;

namespace D3Sharp.Net.Game.Message.Fields
{
    public class DPathSinData
    {
        public float Field0;
        public float Field1;
        public float Field2;
        public float Field3;
        public float Field4;
        public float Field5;

        public void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadFloat32();
            Field1 = buffer.ReadFloat32();
            Field2 = buffer.ReadFloat32();
            Field3 = buffer.ReadFloat32();
            Field4 = buffer.ReadFloat32();
            Field5 = buffer.ReadFloat32();
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteFloat32(Field0);
            buffer.WriteFloat32(Field1);
            buffer.WriteFloat32(Field2);
            buffer.WriteFloat32(Field3);
            buffer.WriteFloat32(Field4);
            buffer.WriteFloat32(Field5);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("DPathSinData:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad);
            b.AppendLine("Field0: " + Field0.ToString("G"));
            b.Append(' ', pad);
            b.AppendLine("Field1: " + Field1.ToString("G"));
            b.Append(' ', pad);
            b.AppendLine("Field2: " + Field2.ToString("G"));
            b.Append(' ', pad);
            b.AppendLine("Field3: " + Field3.ToString("G"));
            b.Append(' ', pad);
            b.AppendLine("Field4: " + Field4.ToString("G"));
            b.Append(' ', pad);
            b.AppendLine("Field5: " + Field5.ToString("G"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}