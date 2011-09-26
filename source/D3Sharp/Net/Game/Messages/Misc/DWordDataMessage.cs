using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace D3Sharp.Net.Game.Messages.Misc
{
    [IncomingMessage(new [] {
        Opcodes.DWordDataMessage1,Opcodes.DWordDataMessage2,Opcodes.DWordDataMessage3,Opcodes.DWordDataMessage4,Opcodes.DWordDataMessage5,Opcodes.DWordDataMessage6,
        Opcodes.DWordDataMessage7,Opcodes.DWordDataMessage8,Opcodes.DWordDataMessage9,Opcodes.DWordDataMessage10,Opcodes.DWordDataMessage11})]
    public class DWordDataMessage : GameMessage
    {
        public int Field0;

        public DWordDataMessage() {}

        public override void Handle(GameClient client)
        {
            
        }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("DWordDataMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }
    }
}
