using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mooege.Net.GS.Message.Definitions.NPC
{
    [Message(Opcodes.NPCSelectConversationMessage, Consumers.SelectedNPC)]
    public class NPCSelectConversationMessage : GameMessage
    {
        public int ConversationSNO;

        public override void Parse(GameBitBuffer buffer)
        {
            ConversationSNO = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, ConversationSNO);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("NPCSelectConversationMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("ConversationSNO: 0x" + ConversationSNO.ToString("X8") + " (" + ConversationSNO + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }

    }
}
