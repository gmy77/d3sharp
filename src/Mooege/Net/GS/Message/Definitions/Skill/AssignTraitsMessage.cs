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

namespace Mooege.Net.GS.Message.Definitions.Skill
{
    [Message(Opcodes.AssignTraitsMessage, Consumers.Player)]
    public class AssignTraitsMessage : GameMessage
    {
        public int[] /* sno */ SNOPowers;

        public override void Parse(GameBitBuffer buffer)
        {
            SNOPowers = new int[3];
            for (int i = 0; i < SNOPowers.Length; i++) SNOPowers[i] = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            for (int i = 0; i < SNOPowers.Length; i++) buffer.WriteInt(32, SNOPowers[i]);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("AssignTraitsMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            for (int i = 0; i < SNOPowers.Length; ) { b.Append(' ', pad + 1); for (int j = 0; j < 8 && i < SNOPowers.Length; j++, i++) { b.Append("0x" + SNOPowers[i].ToString("X8") + ", "); } b.AppendLine(); }
            b.Append(' ', --pad);
            b.AppendLine("}");
        }

    }
}
