using System;
using System.Linq;
using System.Text;
using D3Sharp.Core.Skills;
using D3Sharp.Net.Game.Messages.ACD;
using D3Sharp.Net.Game.Messages.Act;
using D3Sharp.Net.Game.Messages.Animation;
using D3Sharp.Net.Game.Messages.Hero;
using D3Sharp.Net.Game.Messages.Inventory;
using D3Sharp.Net.Game.Messages.Map;
using D3Sharp.Net.Game.Messages.Misc;
using D3Sharp.Net.Game.Messages.Misc.ANN;
using D3Sharp.Net.Game.Messages.Player;
using D3Sharp.Net.Game.Messages.Portal;
using D3Sharp.Net.Game.Messages.Scene;
using D3Sharp.Net.Game.Messages.Team;

namespace D3Sharp.Net.Game.Messages.Connection
{
    [IncomingMessage(Opcodes.JoinBNetGameMessage)]
    public class JoinBNetGameMessage : GameMessage
    {
        public EntityId Field0;  // this *is* the toon id /raist.
        public GameId Field1;
        public int Field2; // and this is the SGameId there we set in D3Sharp.Core.Games.Game.cs when we send the connection info to client /raist.
        public long Field3;
        public int Field4;
        public int ProtocolHash;
        public int SNOPackHash;

        public override void Handle(GameClient client)
        {
            if (this.Id != 0x000A)
                throw new NotImplementedException();

            // a hackish way to get client.BnetClient in context -- pretends games has only one client in. when we're done with implementing bnet completely, will get this sorted out. /raist
            client.BnetClient = Core.Games.GameManager.AvailableGames[(ulong)this.Field2].Clients.FirstOrDefault();
            if (client.BnetClient != null) client.BnetClient.InGameClient = client;

            client.SendMessageNow(new VersionsMessage()
            {
                Id = 0x000D,
                SNOPackHash = this.SNOPackHash,
                ProtocolHash = GameMessage.ImplementedProtocolHash,
                Version = "0.3.0.7333",
            });

            client.SendMessage(new ConnectionEstablishedMessage()
            {
                Id = 0x002E,
                Field0 = 0x00000000,
                Field1 = 0x4BB91A16,
                Field2 = this.SNOPackHash,
            });

            client.SendMessage(new GameSetupMessage()
            {
                Id = 0x002F,
                Field0 = 0x00000077,
            });

            client.SendMessage(new SavePointInfoMessage()
            {
                Id = 0x0045,
                snoLevelArea = -1,
            });

            client.SendMessage(new HearthPortalInfoMessage()
            {
                Id = 0x0046,
                snoLevelArea = -1,
                Field1 = -1,
            });

            client.SendMessage(new ActTransitionMessage()
            {
                Id = 0x00A8,
                Field0 = 0x00000000,
                Field1 = true,
            });

            client.GameUniverse.EnterPlayer(client);

            client.FlushOutgoingBuffer();
        }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = new EntityId();
            Field0.Parse(buffer);
            Field1 = new GameId();
            Field1.Parse(buffer);
            Field2 = buffer.ReadInt(32);
            Field3 = buffer.ReadInt64(64);
            Field4 = buffer.ReadInt(4) + (2);
            ProtocolHash = buffer.ReadInt(32);
            SNOPackHash = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            throw new NotImplementedException();
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("JoinBNetGameMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            Field0.AsText(b, pad);
            Field1.AsText(b, pad);
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', pad); b.AppendLine("Field3: 0x" + Field3.ToString("X16"));
            b.Append(' ', pad); b.AppendLine("Field4: 0x" + Field4.ToString("X8") + " (" + Field4 + ")");
            b.Append(' ', pad); b.AppendLine("ProtocolHash: 0x" + ProtocolHash.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("SNOPackHash: 0x" + SNOPackHash.ToString("X8"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }
    }
}

