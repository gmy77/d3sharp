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

using System.Text;
using D3Sharp.Net.Game.Message.Fields;

namespace D3Sharp.Net.Game.Message.Definitions.ACD
{
    [IncomingMessage(new[] { Opcodes.ACDTranslateNormalMessage1, Opcodes.ACDTranslateNormalMessage2 })]
    public class ACDTranslateNormalMessage : GameMessage, ISelfHandler
    {
        public int Field0;
        public Vector3D Position;
        public float /* angle */? Field2;
        public bool? Field3;
        public float? Field4;
        public int? Field5;
        public int? Field6;
        public int? Field7;

        public void Handle(GameClient client)
        {
            if (this.Position != null)
                client.Player.Hero.Position = this.Position;
        }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            if (buffer.ReadBool())
            {
                Position = new Vector3D();
                Position.Parse(buffer);
            }
            if (buffer.ReadBool())
            {
                Field2 = buffer.ReadFloat32();
            }
            if (buffer.ReadBool())
            {
                Field3 = buffer.ReadBool();
            }
            if (buffer.ReadBool())
            {
                Field4 = buffer.ReadFloat32();
            }
            if (buffer.ReadBool())
            {
                Field5 = buffer.ReadInt(24);
            }
            if (buffer.ReadBool())
            {
                Field6 = buffer.ReadInt(21) + (-1);
            }
            if (buffer.ReadBool())
            {
                Field7 = buffer.ReadInt(32);
            }
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteBool(Position != null);
            if (Position != null)
            {
                Position.Encode(buffer);
            }
            buffer.WriteBool(Field2.HasValue);
            if (Field2.HasValue)
            {
                buffer.WriteFloat32(Field2.Value);
            }
            buffer.WriteBool(Field3.HasValue);
            if (Field3.HasValue)
            {
                buffer.WriteBool(Field3.Value);
            }
            buffer.WriteBool(Field4.HasValue);
            if (Field4.HasValue)
            {
                buffer.WriteFloat32(Field4.Value);
            }
            buffer.WriteBool(Field5.HasValue);
            if (Field5.HasValue)
            {
                buffer.WriteInt(24, Field5.Value);
            }
            buffer.WriteBool(Field6.HasValue);
            if (Field6.HasValue)
            {
                buffer.WriteInt(21, Field6.Value - (-1));
            }
            buffer.WriteBool(Field7.HasValue);
            if (Field7.HasValue)
            {
                buffer.WriteInt(32, Field7.Value);
            }
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("ACDTranslateNormalMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8"));
            if (Position != null)
            {
                Position.AsText(b, pad);
            }
            if (Field2.HasValue)
            {
                b.Append(' ', pad); b.AppendLine("Field2.Value: " + Field2.Value.ToString("G"));
            }
            if (Field3.HasValue)
            {
                b.Append(' ', pad); b.AppendLine("Field3.Value: " + (Field3.Value ? "true" : "false"));
            }
            if (Field4.HasValue)
            {
                b.Append(' ', pad); b.AppendLine("Field4.Value: " + Field4.Value.ToString("G"));
            }
            if (Field5.HasValue)
            {
                b.Append(' ', pad); b.AppendLine("Field5.Value: 0x" + Field5.Value.ToString("X8") + " (" + Field5.Value + ")");
            }
            if (Field6.HasValue)
            {
                b.Append(' ', pad); b.AppendLine("Field6.Value: 0x" + Field6.Value.ToString("X8") + " (" + Field6.Value + ")");
            }
            if (Field7.HasValue)
            {
                b.Append(' ', pad); b.AppendLine("Field7.Value: 0x" + Field7.Value.ToString("X8") + " (" + Field7.Value + ")");
            }
            b.Append(' ', --pad);
            b.AppendLine("}");
        }
    }
}