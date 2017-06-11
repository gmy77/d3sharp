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

namespace Mooege.Net.GS.Message.Fields
{
    public class HirelingInfo
    {
        public int HirelingIndex;
        public int Field1;
        public int Level;
        public int Field3;
        public bool Field4;
        public int Skill1SNOId;
        public int Skill2SNOId;
        public int Skill3SNOId;
        public int Skill4SNOId;

        public void Parse(GameBitBuffer buffer)
        {
            HirelingIndex = buffer.ReadInt(2);
            Field1 = buffer.ReadInt(32);
            Level = buffer.ReadInt(7);
            Field3 = buffer.ReadInt(32);
            Field4 = buffer.ReadBool();
            Skill1SNOId = buffer.ReadInt(32);
            Skill2SNOId = buffer.ReadInt(32);
            Skill3SNOId = buffer.ReadInt(32);
            Skill4SNOId = buffer.ReadInt(32);
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(2, HirelingIndex);
            buffer.WriteInt(32, Field1);
            buffer.WriteInt(7, Level);
            buffer.WriteInt(32, Field3);
            buffer.WriteBool(Field4);
            buffer.WriteInt(32, Skill1SNOId);
            buffer.WriteInt(32, Skill2SNOId);
            buffer.WriteInt(32, Skill3SNOId);
            buffer.WriteInt(32, Skill4SNOId);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("HirelingInfo:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad);
            b.AppendLine("HirelingIndex: 0x" + HirelingIndex.ToString("X8") + " (" + HirelingIndex + ")");
            b.Append(' ', pad);
            b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad);
            b.AppendLine("Level: 0x" + Level.ToString("X8") + " (" + Level + ")");
            b.Append(' ', pad);
            b.AppendLine("Field3: 0x" + Field3.ToString("X8") + " (" + Field3 + ")");
            b.Append(' ', pad);
            b.AppendLine("Field4: " + (Field4 ? "true" : "false"));
            b.Append(' ', pad);
            b.AppendLine("Skill1SNOId: 0x" + Skill1SNOId.ToString("X8") + " (" + Skill1SNOId + ")");
            b.Append(' ', pad);
            b.AppendLine("Skill2SNOId: 0x" + Skill2SNOId.ToString("X8") + " (" + Skill2SNOId + ")");
            b.Append(' ', pad);
            b.AppendLine("Skill3SNOId: 0x" + Skill3SNOId.ToString("X8") + " (" + Skill3SNOId + ")");
            b.Append(' ', pad);
            b.AppendLine("Skill4SNOId: 0x" + Skill4SNOId.ToString("X8") + " (" + Skill4SNOId + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }
    }
}