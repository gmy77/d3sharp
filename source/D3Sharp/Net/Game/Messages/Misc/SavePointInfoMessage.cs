using System.Text;

namespace D3Sharp.Net.Game.Messages.Misc
{
    public class SavePointInfoMessage : GameMessage
    {
        public int /* sno */ snoLevelArea;

        public SavePointInfoMessage():base(Opcodes.SavePointInfoMessage){}

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
            buffer.WriteInt(32, snoLevelArea);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("SavePointInfoMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("snoLevelArea: 0x" + snoLevelArea.ToString("X8"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }
    }
}
