using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mooege.Net.GS.Message.Definitions.Hireling
{
    [Message(Opcodes.HirelingRetrainMessage, Consumers.SelectedNPC)]
    public class HirelingRetrainMessage : GameMessage
    {
        public uint HirelingId;

        public HirelingRetrainMessage()
            : base(Opcodes.HirelingRetrainMessage)
        {
        }

        public override void Parse(GameBitBuffer buffer)
        {
        }

        public override void Encode(GameBitBuffer buffer)
        {
            throw new NotImplementedException();
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("HirelingRetrainMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("HirelingId: 0x" + HirelingId.ToString("X8") + " (" + HirelingId + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }
    }
}
