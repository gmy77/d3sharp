using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace D3Sharp.Net.Game.Messages.Misc
{
    public class HirelingInfoUpdateMessage: GameMessage
    {
        public int Field0;
        public bool Field1;
        public int Field2;
        public int Field3;

        public override void Handle(GameClient client)
        {
            throw new NotImplementedException();
        }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(2);
            Field1 = buffer.ReadBool();
            Field2 = buffer.ReadInt(32);
            Field3 = buffer.ReadInt(7);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(2, Field0);
            buffer.WriteBool(Field1);
            buffer.WriteInt(32, Field2);
            buffer.WriteInt(7, Field3);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("HirelingInfoUpdateMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: " + (Field1 ? "true" : "false"));
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', pad); b.AppendLine("Field3: 0x" + Field3.ToString("X8") + " (" + Field3 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }
    }
}
