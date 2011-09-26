using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using D3Sharp.Core.NPC;

namespace D3Sharp.Net.Game.Messages.Animation
{
    [IncomingMessage(Opcodes.SecondaryAnimationPowerMessage)]
    public class SecondaryAnimationPowerMessage:GameMessage
    {
        public int /* sno */ snoPower;
        public AnimPreplayData Field1;

        public override void Handle(GameClient client)
        {
            var oldPosField1 = client.position.Field1;
            var oldPosField2 = client.position.Field2;
            for (var i = 0; i < 10; i++)
            {
                if ((i % 2) == 0)
                {
                    client.position.Field0 += (float)(client.rand.NextDouble() * 20);
                    client.position.Field1 += (float)(client.rand.NextDouble() * 20);
                }
                else
                {
                    client.position.Field0 -= (float)(client.rand.NextDouble() * 20);
                    client.position.Field1 -= (float)(client.rand.NextDouble() * 20);
                }
                System.Threading.Thread.Sleep(15); // Required to not generate the same random value twice...
                client.SpawnMob(BasicNPC.RandomNPC());
            }

            client.position.Field1 = oldPosField1;
            client.position.Field2 = oldPosField2;
        }

        public override void Parse(GameBitBuffer buffer)
        {
            snoPower = buffer.ReadInt(32);
            if (buffer.ReadBool())
            {
                Field1 = new AnimPreplayData();
                Field1.Parse(buffer);
            }
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, snoPower);
            buffer.WriteBool(Field1 != null);
            if (Field1 != null)
            {
                Field1.Encode(buffer);
            }
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("SecondaryAnimationPowerMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("snoPower: 0x" + snoPower.ToString("X8"));
            if (Field1 != null)
            {
                Field1.AsText(b, pad);
            }
            b.Append(' ', --pad);
            b.AppendLine("}");
        }
    }
}
