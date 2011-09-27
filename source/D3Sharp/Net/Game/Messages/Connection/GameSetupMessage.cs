using System.Text;

namespace D3Sharp.Net.Game.Messages.Connection
{
    [IncomingMessage(Opcodes.GameSetupMessage)]
    public class GameSetupMessage : GameMessage
    {
        public int Field0;

        public GameSetupMessage() : base(Opcodes.GameSetupMessage) { }

        public override void Handle(GameClient client)
        {
            throw new System.NotImplementedException();
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
            b.AppendLine("GameSetupMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }
    }
}
