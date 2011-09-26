using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace D3Sharp.Net.Game.Messages.Misc
{
    public class SNONameDataMessage : GameMessage
    {
        public SNOName Field0;

        public SNONameDataMessage() { }

        public SNONameDataMessage(int id):base(id) { }

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
            Field0.Encode(buffer);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("SNONameDataMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            Field0.AsText(b, pad);
            b.Append(' ', --pad);
            b.AppendLine("}");
        }
    }
}
