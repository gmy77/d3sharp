using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace D3Sharp.Net.Game.Messages.Connection
{
    [IncomingMessage(new []{Opcodes.LogoutContextMessage1, Opcodes.LogoutContextMessage2})]
    public class LogoutContextMessage:GameMessage
    {
        public bool Field0;

        public override void Handle(GameClient client)
        {
            client.IsLoggingOut = !client.IsLoggingOut;

            if (client.IsLoggingOut)
            {
                client.SendMessageNow(new LogoutTickTimeMessage()
                {
                    Id = 0x0027,
                    Field0 = false, // true - logout with party?
                    Field1 = 600, // delay 1, make this equal to 0 for instant logout
                    Field2 = 600, // delay 2
                });
            }
        }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadBool();
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteBool(Field0);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("LogoutContextMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: " + (Field0 ? "true" : "false"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }
    }
}
