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
using System.Text;

namespace D3Sharp.Net.Game.Message.Definitions.Skill
{
    public class LearnedSkillMessage : GameMessage
    {
        // MaxLength = 128
        public int /* sno */[] aSkillSNOs;

        public override void Parse(GameBitBuffer buffer)
        {
            aSkillSNOs = new int /* sno */[buffer.ReadInt(8)];
            for (int i = 0; i < aSkillSNOs.Length; i++) aSkillSNOs[i] = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(8, aSkillSNOs.Length);
            for (int i = 0; i < aSkillSNOs.Length; i++) buffer.WriteInt(32, aSkillSNOs[i]);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("LearnedSkillMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("aSkillSNOs:");
            b.Append(' ', pad); b.AppendLine("{");
            for (int i = 0; i < aSkillSNOs.Length; ) { b.Append(' ', pad + 1); for (int j = 0; j < 8 && i < aSkillSNOs.Length; j++, i++) { b.Append("0x" + aSkillSNOs[i].ToString("X8") + ", "); } b.AppendLine(); }
            b.Append(' ', pad); b.AppendLine("}"); b.AppendLine();
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}