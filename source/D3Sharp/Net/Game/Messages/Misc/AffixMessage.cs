using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace D3Sharp.Net.Game.Messages.Misc
{
    public class AffixMessage : GameMessage
    {
        public int Field0;
        public int Field1;
        // MaxLength = 32
        public int /* gbid */[] aAffixGBIDs;

        public AffixMessage():base(Opcodes.AffixMessage)
        { }

        public override void Handle(GameClient client)
        {
            throw new NotImplementedException();
        }

        public override void Parse(GameBitBuffer buffer)
        {
            throw new NotImplementedException();
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(2, Field1);
            buffer.WriteInt(6, aAffixGBIDs.Length);
            for (int i = 0; i < aAffixGBIDs.Length; i++) buffer.WriteInt(32, aAffixGBIDs[i]);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("AffixMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad); b.AppendLine("aAffixGBIDs:");
            b.Append(' ', pad); b.AppendLine("{");
            for (int i = 0; i < aAffixGBIDs.Length; ) { b.Append(' ', pad + 1); for (int j = 0; j < 8 && i < aAffixGBIDs.Length; j++, i++) { b.Append("0x" + aAffixGBIDs[i].ToString("X8") + ", "); } b.AppendLine(); }
            b.Append(' ', pad); b.AppendLine("}"); b.AppendLine();
            b.Append(' ', --pad);
            b.AppendLine("}");
        }
    }
}
