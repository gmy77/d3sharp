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

namespace Mooege.Net.GS.Message.Definitions.Effect
{
    [Message(Opcodes.ComplexEffectAddMessage)]
    public class ComplexEffectAddMessage : GameMessage
    {
        public int EffectId;
        public int Field1;  // 0=efg, 1=efg, 2=rope
        public int /* sno */ EffectSNO;
        public int SourceActorId;
        public int TargetActorId;
        public int Field5;  // 0=efg, 4=rope1, 3=rope2
        public int Field6;  // 0=efg, 1=rope1, 3=rope2

        public ComplexEffectAddMessage() : base(Opcodes.ComplexEffectAddMessage) {}

        public override void Parse(GameBitBuffer buffer)
        {
            EffectId = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(32);
            EffectSNO = buffer.ReadInt(32);
            SourceActorId = buffer.ReadInt(32);
            TargetActorId = buffer.ReadInt(32);
            Field5 = buffer.ReadInt(32);
            Field6 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, EffectId);
            buffer.WriteInt(32, Field1);
            buffer.WriteInt(32, EffectSNO);
            buffer.WriteInt(32, SourceActorId);
            buffer.WriteInt(32, TargetActorId);
            buffer.WriteInt(32, Field5);
            buffer.WriteInt(32, Field6);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("ComplexEffectAddMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("EffectId: 0x" + EffectId.ToString("X8") + " (" + EffectId + ")");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad); b.AppendLine("EffectSNO: 0x" + EffectSNO.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("SourceActorId: 0x" + SourceActorId.ToString("X8") + " (" + SourceActorId + ")");
            b.Append(' ', pad); b.AppendLine("TargetActorId: 0x" + TargetActorId.ToString("X8") + " (" + TargetActorId + ")");
            b.Append(' ', pad); b.AppendLine("Field5: 0x" + Field5.ToString("X8") + " (" + Field5 + ")");
            b.Append(' ', pad); b.AppendLine("Field6: 0x" + Field6.ToString("X8") + " (" + Field6 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}