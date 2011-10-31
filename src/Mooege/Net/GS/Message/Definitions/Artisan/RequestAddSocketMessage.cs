using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mooege.Net.GS.Message.Definitions.Artisan
{
    [Message(Opcodes.RequestAddSocketMessage, Consumers.Player)]
    public class RequestAddSocketMessage : GameMessage
    {
        public uint ItemID;

        public override void Parse(GameBitBuffer buffer)
        {
            ItemID = buffer.ReadUInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteUInt(32, ItemID);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("OpenArtisanWindowMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("ArtisanID: 0x" + ItemID.ToString("X8") + " (" + ItemID + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }

    }

}
