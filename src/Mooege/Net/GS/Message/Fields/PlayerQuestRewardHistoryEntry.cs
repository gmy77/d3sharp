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
    public class PlayerQuestRewardHistoryEntry
    {
        public int /* sno */ snoQuest;
        public int Field1;

        public enum eField2
        {
            Normal = 0,
            Nightmare = 1,
            Hell = 2,
            Inferno = 3,
        }

        public eField2 Field2;

        public void Parse(GameBitBuffer buffer)
        {
            snoQuest = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(32);
            Field2 = (eField2) buffer.ReadInt(2);
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, snoQuest);
            buffer.WriteInt(32, Field1);
            buffer.WriteInt(2, (int) Field2);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("PlayerQuestRewardHistoryEntry:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad);
            b.AppendLine("snoQuest: 0x" + snoQuest.ToString("X8"));
            b.Append(' ', pad);
            b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad);
            b.AppendLine("Field2: " + Field2.ToString());
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}