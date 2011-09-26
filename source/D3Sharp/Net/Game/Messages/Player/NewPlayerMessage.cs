using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace D3Sharp.Net.Game.Messages.Player
{
    public class NewPlayerMessage : GameMessage
    {
        public int Field0;
        public string Field1;
        public string ToonName;
        public int Field3;
        public int Field4;
        public int /* sno */ snoActorPortrait;
        public int Field6;
        public HeroStateData Field7;
        public bool Field8;
        public int Field9;
        public int Field10;

        public NewPlayerMessage():base(Opcodes.NewPlayerMessage){}

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
            buffer.WriteInt(3, Field0);
            buffer.WriteCharArray(128, Field1);
            buffer.WriteCharArray(101, ToonName);
            buffer.WriteInt(5, Field3 - (-1));
            buffer.WriteInt(3, Field4 - (-1));
            buffer.WriteInt(32, snoActorPortrait);
            buffer.WriteInt(7, Field6);
            Field7.Encode(buffer);
            buffer.WriteBool(Field8);
            buffer.WriteInt(32, Field9);
            buffer.WriteInt(32, Field10);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("NewPlayerMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: \"" + Field1 + "\"");
            b.Append(' ', pad); b.AppendLine("ToonName: \"" + ToonName + "\"");
            b.Append(' ', pad); b.AppendLine("Field3: 0x" + Field3.ToString("X8") + " (" + Field3 + ")");
            b.Append(' ', pad); b.AppendLine("Field4: 0x" + Field4.ToString("X8") + " (" + Field4 + ")");
            b.Append(' ', pad); b.AppendLine("snoActorPortrait: 0x" + snoActorPortrait.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("Field6: 0x" + Field6.ToString("X8") + " (" + Field6 + ")");
            Field7.AsText(b, pad);
            b.Append(' ', pad); b.AppendLine("Field8: " + (Field8 ? "true" : "false"));
            b.Append(' ', pad); b.AppendLine("Field9: 0x" + Field9.ToString("X8") + " (" + Field9 + ")");
            b.Append(' ', pad); b.AppendLine("Field10: 0x" + Field10.ToString("X8") + " (" + Field10 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }
    }
}
