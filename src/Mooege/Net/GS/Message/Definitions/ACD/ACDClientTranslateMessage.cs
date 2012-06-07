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
    [Message(Opcodes.ACDClientTranslateMessage, Consumers.Player)]
    public class ACDClientTranslateMessage : GameMessage
    {
        public int Tick;
        public int Field1;
        public Vector3D Position;
        public float Angle;
        public float Speed;
        public int Field5;
        public int AnimationTag;
        public int? Field7;

        public override void Parse(GameBitBuffer buffer)
        {
            Tick = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(4);
            Position = new Vector3D();
            Position.Parse(buffer);
            Angle = buffer.ReadFloat32();
            Speed = buffer.ReadFloat32();
            Field5 = buffer.ReadInt(32);
            AnimationTag = buffer.ReadInt(21) + (-1);
            if (buffer.ReadBool())
                Field7 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Tick);
            buffer.WriteInt(4, Field1);
            Position.Encode(buffer);
            buffer.WriteFloat32(Angle);
            buffer.WriteFloat32(Speed);
            buffer.WriteInt(32, Field5);
            buffer.WriteInt(21, AnimationTag - (-1));
            if (Field7.HasValue)
                buffer.WriteInt(32, Field7.Value);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("ACDClientTranslateMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Tick: 0x" + Tick.ToString("X8") + " (" + Tick + ")");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8"));
            Position.AsText(b, pad);
            b.Append(' ', pad); b.AppendLine("Position: " + Angle.ToString("G"));
            b.Append(' ', pad); b.AppendLine("Angle: " + Speed.ToString("G"));
            b.Append(' ', pad); b.AppendLine("Field5: 0x" + Field5.ToString("X8") + " (" + Field5 + ")");
            b.Append(' ', pad); b.AppendLine("AnimationTag: 0x" + AnimationTag.ToString("X8") + " (" + AnimationTag + ")");
            if (Field7.HasValue)
            {
                b.Append(' ', pad); b.AppendLine("Field7.Value: 0x" + Field7.Value.ToString("X8") + " (" + Field7.Value + ")");
            }
            b.Append(' ', --pad);
            b.AppendLine("}");
        }
    }

}
