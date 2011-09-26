using System.Text;

namespace D3Sharp.Net.Game.Messages.Misc
{
    public class AttributesSetValuesMessage : GameMessage
    {
        public int Field0;
        // MaxLength = 15
        public NetAttributeKeyValue[] atKeyVals;

        public AttributesSetValuesMessage()
            : base(Opcodes.AttributesSetValuesMessage) { }

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
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(4, atKeyVals.Length);
            for (int i = 0; i < atKeyVals.Length; i++) { atKeyVals[i].Encode(buffer); }
            for (int i = 0; i < atKeyVals.Length; i++) { atKeyVals[i].EncodeValue(buffer); }
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("AttributesSetValuesMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("atKeyVals:");
            b.Append(' ', pad); b.AppendLine("{");
            for (int i = 0; i < atKeyVals.Length; i++) { atKeyVals[i].AsText(b, pad + 1); b.AppendLine(); }
            b.Append(' ', pad); b.AppendLine("}"); b.AppendLine();
            b.Append(' ', --pad);
            b.AppendLine("}");
        }
    }
}
