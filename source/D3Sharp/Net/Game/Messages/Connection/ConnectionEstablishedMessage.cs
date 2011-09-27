using System.Text;

namespace D3Sharp.Net.Game.Messages.Connection
{
    [IncomingMessage(Opcodes.ConnectionEstablishedMessage)]
    public class ConnectionEstablishedMessage : GameMessage
    {
        public int Field0;
        public int Field1;
        public int Field2;

        public ConnectionEstablishedMessage() : base(Opcodes.ConnectionEstablishedMessage)
        {
        }

        public override void Handle(GameClient client)
        {
            throw new System.NotImplementedException();
        }

        public override void Parse(GameBitBuffer buffer)
        {
            throw new System.NotImplementedException();
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(3, Field0);
            buffer.WriteInt(32, Field1);
            buffer.WriteInt(32, Field2);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("ConnectionEstablishedMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }
    }
}
