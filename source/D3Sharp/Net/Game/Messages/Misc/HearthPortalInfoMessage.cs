using System.Text;

namespace D3Sharp.Net.Game.Messages.Misc
{
    public class HearthPortalInfoMessage : GameMessage
    {
        public int /* sno */ snoLevelArea;
        public int Field1;

        public HearthPortalInfoMessage():base(Opcodes.HearthPortalInfoMessage){}

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
            buffer.WriteInt(32, Field1);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("HearthPortalInfoMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("snoLevelArea: 0x" + snoLevelArea.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }
    }
}
