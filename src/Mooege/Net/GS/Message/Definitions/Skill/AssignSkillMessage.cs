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
    [Message(new[] { Opcodes.AssignSkillMessage2, Opcodes.AssignSkillMessage3 })]
    public class AssignSkillMessage : GameMessage
    {
        public int /* sno */ SNOSkill;
        public int RuneIndex;
        public int SkillIndex;

        public override void Parse(GameBitBuffer buffer)
        {
            SNOSkill = buffer.ReadInt(32);
            RuneIndex = buffer.ReadInt(3) + (-1);
            SkillIndex = buffer.ReadInt(3);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, SNOSkill);
            buffer.WriteInt(3, RuneIndex - (-1));
            buffer.WriteInt(5, SkillIndex);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("AssignSkillMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("SNOSkill: 0x" + SNOSkill.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("RuneIndex: 0x" + RuneIndex.ToString("X8") + " (" + RuneIndex + ")");
            b.Append(' ', pad); b.AppendLine("SkillIndex: 0x" + SkillIndex.ToString("X8") + " (" + SkillIndex + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}
