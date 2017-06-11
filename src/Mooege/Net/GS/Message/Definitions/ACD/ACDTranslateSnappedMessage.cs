/*
 * Copyright (C) 2011 - 2012 mooege project - http://www.mooege.org
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
using Mooege.Core.GS.Common.Types.Math;

namespace Mooege.Net.GS.Message.Definitions.ACD
{
    [Message(Opcodes.ACDTranslateSnappedMessage)]
    public class ACDTranslateSnappedMessage : GameMessage
    {
        public int ActorId;
        public Vector3D Position;
        public float /* angle */ Angle;
        public bool Field3;
        public int Field4;
        public int? Field5;

        public ACDTranslateSnappedMessage() : base(Opcodes.ACDTranslateSnappedMessage) { }

        public override void Parse(GameBitBuffer buffer)
        {
            ActorId = buffer.ReadInt(32);
            Position = new Vector3D();
            Position.Parse(buffer);
            Angle = buffer.ReadFloat32();
            Field3 = buffer.ReadBool();
            Field4 = buffer.ReadInt(25);
            if (buffer.ReadBool())
            {
                Field5 = buffer.ReadInt(16);
            }
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, ActorId);
            Position.Encode(buffer);
            buffer.WriteFloat32(Angle);
            buffer.WriteBool(Field3);
            buffer.WriteInt(25, Field4);
            buffer.WriteBool(Field5.HasValue);
            if (Field5.HasValue)
            {
                buffer.WriteInt(16, Field5.Value);
            }
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("ACDTranslateSnappedMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + ActorId.ToString("X8"));
            Position.AsText(b, pad);
            b.Append(' ', pad); b.AppendLine("Angle: " + Angle.ToString("G"));
            b.Append(' ', pad); b.AppendLine("Field3: " + (Field3 ? "true" : "false"));
            b.Append(' ', pad); b.AppendLine("Field4: 0x" + Field4.ToString("X8") + " (" + Field4 + ")");
            if (Field5.HasValue)
            {
                b.Append(' ', pad); b.AppendLine("Field5.Value: 0x" + Field5.Value.ToString("X8") + " (" + Field5.Value + ")");
            }
            b.Append(' ', --pad);
            b.AppendLine("}");
        }
    }
}