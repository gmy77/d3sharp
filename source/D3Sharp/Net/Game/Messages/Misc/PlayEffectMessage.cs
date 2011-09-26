using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace D3Sharp.Net.Game.Messages.Misc
{
    public class PlayEffectMessage:GameMessage
    {
        public int Field0;
        public int Field1;
        public int? Field2;

        public override void Handle(GameClient client)
        {
            throw new NotImplementedException();
        }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(7) + (-1);
            if (buffer.ReadBool())
            {
                Field2 = buffer.ReadInt(32);
            }
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(7, Field1 - (-1));
            buffer.WriteBool(Field2.HasValue);
            if (Field2.HasValue)
            {
                buffer.WriteInt(32, Field2.Value);
            }
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("PlayEffectMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            if (Field2.HasValue)
            {
                b.Append(' ', pad); b.AppendLine("Field2.Value: 0x" + Field2.Value.ToString("X8") + " (" + Field2.Value + ")");
            }
            b.Append(' ', --pad);
            b.AppendLine("}");
        }
    }
}
