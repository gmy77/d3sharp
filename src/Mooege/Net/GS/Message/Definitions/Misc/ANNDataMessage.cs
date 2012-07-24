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

namespace Mooege.Net.GS.Message.Definitions.Misc
{
    [Message(new[] {
        Opcodes.ANNDataMessage8, Opcodes.ANNDataMessage10, Opcodes.ANNDataMessage11, Opcodes.ANNDataMessage13, Opcodes.ANNDataMessage15, 
        Opcodes.ANNDataMessage16, Opcodes.ANNDataMessage17, Opcodes.ANNDataMessage18, Opcodes.ANNDataMessage21, Opcodes.ANNDataMessage23, 
        Opcodes.ANNDataMessage24, Opcodes.ANNDataMessage25, Opcodes.ANNDataMessage26, Opcodes.ANNDataMessage28, Opcodes.ANNDataMessage29, Opcodes.ANNDataMessage31, 
        Opcodes.ANNDataMessage32,
    })]
    public class ANNDataMessage : GameMessage
    {
        public uint ActorID; // Actor's DynamicID

        public ANNDataMessage(Opcodes id) : base(id) {}

        public override void Parse(GameBitBuffer buffer)
        {
            ActorID = buffer.ReadUInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteUInt(32, ActorID);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("ANNDataMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("ActorID: 0x" + ActorID.ToString("X8") + " (" + ActorID + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}
