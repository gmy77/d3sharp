/*
 * Copyright (C) 2011 mooege project
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

namespace Mooege.Net.GS.Message.Definitions.ACD
{
    [IncomingMessage(new[] {
        Opcodes.ACDTranslateFacingMessage1,
        Opcodes.ACDTranslateFacingMessage2
    })]
    public class ACDTranslateFacingMessage : GameMessage
    {
        public uint ActorID; // The actor's DynamicID
        public float /* angle */ Angle;
        public bool Field2;

        public ACDTranslateFacingMessage() {}
        public ACDTranslateFacingMessage(Opcodes id) : base(id) {}

        public override void Parse(GameBitBuffer buffer)
        {
            ActorID = buffer.ReadUInt(32);
            Angle = buffer.ReadFloat32();
            Field2 = buffer.ReadBool();
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteUInt(32, ActorID);
            buffer.WriteFloat32(Angle);
            buffer.WriteBool(Field2);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("ACDTranslateFacingMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("ActorID: 0x" + ActorID.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("Angle: " + Angle.ToString("G"));
            b.Append(' ', pad); b.AppendLine("Field2: " + (Field2 ? "true" : "false"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}
