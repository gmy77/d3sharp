using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mooege.Net.GS.Message.Definitions.Player
{
    /* TODO: Fixme
    [Message(Opcodes.RequestUsePowerMessage, Consumers.Player)]
    public class RequestUsePowerMessage : GameMessage
    {
        public int PowerSNOId;

        public override void Parse(GameBitBuffer buffer)
        {
            PowerSNOId = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, PowerSNOId);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("RequestUsePowerMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("PowerSNOId: 0x" + PowerSNOId.ToString("X8") + " (" + PowerSNOId + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }
    }
     */
}
