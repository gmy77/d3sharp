using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace D3Sharp.Net.Game.Messages.ACD
{
    public class ACDTranslateFacingMessage : GameMessage
    {
        public int Field0;
        public float /* angle */ Field1;
        public bool Field2;

        public ACDTranslateFacingMessage() { } 

        public ACDTranslateFacingMessage(int id):base(id) { }

        public override void Handle(GameClient client)
        {
            throw new NotImplementedException();
        }

        public override void Parse(GameBitBuffer buffer)
        {
            throw new NotImplementedException();
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteFloat32(Field1);
            buffer.WriteBool(Field2);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("ACDTranslateFacingMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("Field1: " + Field1.ToString("G"));
            b.Append(' ', pad); b.AppendLine("Field2: " + (Field2 ? "true" : "false"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }
    }
}
