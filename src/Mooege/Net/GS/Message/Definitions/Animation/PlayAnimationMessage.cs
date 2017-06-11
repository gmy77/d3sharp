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
using Mooege.Net.GS.Message.Fields;

namespace Mooege.Net.GS.Message.Definitions.Animation
{
    [Message(Opcodes.PlayAnimationMessage)]
    public class PlayAnimationMessage : GameMessage
    {
        public uint ActorID;
        public int Field1;
        public float Field2;
        // MaxLength = 3
        public PlayAnimationMessageSpec[] tAnim;

        public PlayAnimationMessage() : base(Opcodes.PlayAnimationMessage) { }

        public override void Parse(GameBitBuffer buffer)
        {
            ActorID = buffer.ReadUInt(32);
            Field1 = buffer.ReadInt(4);
            Field2 = buffer.ReadFloat32();
            tAnim = new PlayAnimationMessageSpec[buffer.ReadInt(2)];
            for (int i = 0; i < tAnim.Length; i++) { tAnim[i] = new PlayAnimationMessageSpec(); tAnim[i].Parse(buffer); }
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteUInt(32, ActorID);
            buffer.WriteInt(4, Field1);
            buffer.WriteFloat32(Field2);
            buffer.WriteInt(2, tAnim.Length);
            for (int i = 0; i < tAnim.Length; i++) { tAnim[i].Encode(buffer); }
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("PlayAnimationMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("ActorID: 0x" + ActorID.ToString("X8") + " (" + ActorID + ")");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad); b.AppendLine("Field2: " + Field2.ToString("G"));
            b.Append(' ', pad); b.AppendLine("tAnim:");
            b.Append(' ', pad); b.AppendLine("{");
            for (int i = 0; i < tAnim.Length; i++) { tAnim[i].AsText(b, pad + 1); b.AppendLine(); }
            b.Append(' ', pad); b.AppendLine("}"); b.AppendLine();
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}
