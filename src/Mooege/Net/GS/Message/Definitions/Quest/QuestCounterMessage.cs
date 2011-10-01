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

namespace Mooege.Net.GS.Message.Definitions.Quest
{
    public class QuestCounterMessage : GameMessage
    {
        public int /* sno */ snoQuest;
        public int /* sno */ snoLevelArea;
        public int Field2;
        public int Field3;
        public int Field4;
        public int Field5;

        public override void Parse(GameBitBuffer buffer)
        {
            snoQuest = buffer.ReadInt(32);
            snoLevelArea = buffer.ReadInt(32);
            Field2 = buffer.ReadInt(32);
            Field3 = buffer.ReadInt(32);
            Field4 = buffer.ReadInt(32);
            Field5 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, snoQuest);
            buffer.WriteInt(32, snoLevelArea);
            buffer.WriteInt(32, Field2);
            buffer.WriteInt(32, Field3);
            buffer.WriteInt(32, Field4);
            buffer.WriteInt(32, Field5);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("QuestCounterMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("snoQuest: 0x" + snoQuest.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("snoLevelArea: 0x" + snoLevelArea.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', pad); b.AppendLine("Field3: 0x" + Field3.ToString("X8") + " (" + Field3 + ")");
            b.Append(' ', pad); b.AppendLine("Field4: 0x" + Field4.ToString("X8") + " (" + Field4 + ")");
            b.Append(' ', pad); b.AppendLine("Field5: 0x" + Field5.ToString("X8") + " (" + Field5 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}