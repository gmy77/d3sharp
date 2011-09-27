/*
 * Copyright (C) 2011 D3Sharp Project
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using D3Sharp.Net.Game.Messages.Animation;
using D3Sharp.Net.Game.Messages.Effect;
using D3Sharp.Net.Game.Messages.Misc.ANN;
using D3Sharp.Net.Game.Messages.Player;
using D3Sharp.Core.Helpers;

namespace D3Sharp.Net.Game.Messages.Misc
{
    [IncomingMessage(Opcodes.TargetMessage)]
    public class TargetMessage:GameMessage
    {
        public int Field0;
        public int Field1;
        public WorldPlace Field2;
        public int /* sno */ snoPower;
        public int Field4;
        public int Field5;
        public AnimPreplayData Field6;

        public override void Handle(GameClient client)
        {
            if (this.Field1 == 0x77F20036)
            {
                client.EnterInn();
                return;
            }
            else if (client.objectIdsSpawned == null || !client.objectIdsSpawned.Contains(this.Field1)) return;

            client.objectIdsSpawned.Remove(this.Field1);

            var killAni = new int[]{
                    0x2cd7,
                    0x2cd4,
                    0x01b378,
                    0x2cdc,
                    0x02f2,
                    0x2ccf,
                    0x2cd0,
                    0x2cd1,
                    0x2cd2,
                    0x2cd3,
                    0x2cd5,
                    0x01b144,
                    0x2cd6,
                    0x2cd8,
                    0x2cda,
                    0x2cd9
            };
            client.SendMessage(new PlayEffectMessage()
            {
                Id = 0x7a,
                Field0 = this.Field1,
                Field1 = 0x0,
                Field2 = 0x2,
            });
            client.SendMessage(new PlayEffectMessage()
            {
                Id = 0x7a,
                Field0 = this.Field1,
                Field1 = 0xc,
            });
            client.SendMessage(new PlayHitEffectMessage()
            {
                Id = 0x7b,
                Field0 = this.Field1,
                Field1 = 0x789E00E2,
                Field2 = 0x2,
                Field3 = false,
            });

            client.SendMessage(new FloatingNumberMessage()
            {
                Id = 0xd0,
                Field0 = this.Field1,
                Field1 = 9001.0f,
                Field2 = 0,
            });

            client.SendMessage(new ANNDataMessage()
            {
                Id = 0x6d,
                Field0 = this.Field1,
            });

            int ani = killAni[RandomHelper.Next(killAni.Length)];
            Logger.Info("Ani used: " + ani);

            client.SendMessage(new PlayAnimationMessage()
            {
                Id = 0x6c,
                Field0 = this.Field1,
                Field1 = 0xb,
                Field2 = 0,
                tAnim = new PlayAnimationMessageSpec[1]
                {
                    new PlayAnimationMessageSpec()
                    {
                        Field0 = 0x2,
                        Field1 = ani,
                        Field2 = 0x0,
                        Field3 = 1f
                    }
                }
            });

            client.packetId += 10 * 2;
            client.SendMessage(new DWordDataMessage()
            {
                Id = 0x89,
                Field0 = client.packetId,
            });

            client.SendMessage(new ANNDataMessage()
            {
                Id = 0xc5,
                Field0 = this.Field1,
            });

            client.SendMessage(new AttributeSetValueMessage
            {
                Id = 0x4c,
                Field0 = this.Field1,
                Field1 = new NetAttributeKeyValue
                {
                    Attribute = GameAttribute.Attributes[0x4d],
                    Float = 0
                }
            });

            client.SendMessage(new AttributeSetValueMessage
            {
                Id = 0x4c,
                Field0 = this.Field1,
                Field1 = new NetAttributeKeyValue
                {
                    Attribute = GameAttribute.Attributes[0x1c2],
                    Int = 1
                }
            });

            client.SendMessage(new AttributeSetValueMessage
            {
                Id = 0x4c,
                Field0 = this.Field1,
                Field1 = new NetAttributeKeyValue
                {
                    Attribute = GameAttribute.Attributes[0x1c5],
                    Int = 1
                }
            });
            client.SendMessage(new PlayEffectMessage()
            {
                Id = 0x7a,
                Field0 = this.Field1,
                Field1 = 0xc,
            });
            client.SendMessage(new PlayEffectMessage()
            {
                Id = 0x7a,
                Field0 = this.Field1,
                Field1 = 0x37,
            });
            client.SendMessage(new PlayHitEffectMessage()
            {
                Id = 0x7b,
                Field0 = this.Field1,
                Field1 = 0x789E00E2,
                Field2 = 0x2,
                Field3 = false,
            });
            client.packetId += 10 * 2;
            client.SendMessage(new DWordDataMessage()
            {
                Id = 0x89,
                Field0 = client.packetId,
            });
        }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(2) + (-1);
            Field1 = buffer.ReadInt(32);
            Field2 = new WorldPlace();
            Field2.Parse(buffer);
            snoPower = buffer.ReadInt(32);
            Field4 = buffer.ReadInt(32);
            Field5 = buffer.ReadInt(2);
            if (buffer.ReadBool())
            {
                Field6 = new AnimPreplayData();
                Field6.Parse(buffer);
            }
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(2, Field0 - (-1));
            buffer.WriteInt(32, Field1);
            Field2.Encode(buffer);
            buffer.WriteInt(32, snoPower);
            buffer.WriteInt(32, Field4);
            buffer.WriteInt(2, Field5);
            buffer.WriteBool(Field6 != null);
            if (Field6 != null)
            {
                Field6.Encode(buffer);
            }
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("TargetMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            Field2.AsText(b, pad);
            b.Append(' ', pad); b.AppendLine("snoPower: 0x" + snoPower.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("Field4: 0x" + Field4.ToString("X8") + " (" + Field4 + ")");
            b.Append(' ', pad); b.AppendLine("Field5: 0x" + Field5.ToString("X8") + " (" + Field5 + ")");
            if (Field6 != null)
            {
                Field6.AsText(b, pad);
            }
            b.Append(' ', --pad);
            b.AppendLine("}");
        }
    }
}
