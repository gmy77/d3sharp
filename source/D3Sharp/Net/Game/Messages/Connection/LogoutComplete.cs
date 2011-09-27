using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace D3Sharp.Net.Game.Messages.Connection
{
    [IncomingMessage(Opcodes.LogoutComplete)]
    public class LogoutComplete:GameMessage
    {
        public override void Handle(GameClient client)
        {
            if (client.IsLoggingOut)
            {
                client.SendMessageNow(new QuitGameMessage()
                {
                    Id = 0x0003,
                    // Field0 - quit reason?
                    // 0 - logout
                    // 1 - kicked by party leader
                    // 2 - disconnected due to client-server (version?) missmatch
                    Field0 = 0,
                });
            }
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
            throw new NotImplementedException();
        }
    }
}
