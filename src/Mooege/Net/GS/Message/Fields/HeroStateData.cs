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
    public class HeroStateData
    {
        public int Field0;
        public int Field1;
        public int Field2;
        public int Field3;
        public int PlayerFlags;
        public PlayerSavedData PlayerSavedData;
        public int QuestRewardHistoryEntriesCount;
        // MaxLength = 100
        public PlayerQuestRewardHistoryEntry[] tQuestRewardHistory;

        public void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(32);
            Field2 = buffer.ReadInt(32);
            Field3 = buffer.ReadInt(32);
            PlayerFlags = buffer.ReadInt(30);
            PlayerSavedData = new PlayerSavedData();
            PlayerSavedData.Parse(buffer);
            QuestRewardHistoryEntriesCount = buffer.ReadInt(32);
            tQuestRewardHistory = new PlayerQuestRewardHistoryEntry[buffer.ReadInt(7)];
            for (int i = 0; i < tQuestRewardHistory.Length; i++)
            {
                tQuestRewardHistory[i] = new PlayerQuestRewardHistoryEntry();
                tQuestRewardHistory[i].Parse(buffer);
            }
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(32, Field1);
            buffer.WriteInt(32, Field2);
            buffer.WriteInt(32, Field3);
            buffer.WriteInt(30, PlayerFlags);
            PlayerSavedData.Encode(buffer);
            buffer.WriteInt(32, QuestRewardHistoryEntriesCount);
            buffer.WriteInt(7, tQuestRewardHistory.Length);
            for (int i = 0; i < tQuestRewardHistory.Length; i++)
            {
                tQuestRewardHistory[i].Encode(buffer);
            }
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("HeroStateData:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad);
            b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad);
            b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad);
            b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', pad);
            b.AppendLine("Field3: 0x" + Field3.ToString("X8") + " (" + Field3 + ")");
            b.Append(' ', pad);
            b.AppendLine("PlayerFlags: 0x" + PlayerFlags.ToString("X8") + " (" + PlayerFlags + ")");
            PlayerSavedData.AsText(b, pad);
            b.Append(' ', pad);
            b.AppendLine("QuestRewardHistoryEntriesCount: 0x" + QuestRewardHistoryEntriesCount.ToString("X8") + " (" + QuestRewardHistoryEntriesCount + ")");
            b.Append(' ', pad);
            b.AppendLine("tQuestRewardHistory:");
            b.Append(' ', pad);
            b.AppendLine("{");
            for (int i = 0; i < tQuestRewardHistory.Length; i++)
            {
                tQuestRewardHistory[i].AsText(b, pad + 1);
                b.AppendLine();
            }
            b.Append(' ', pad);
            b.AppendLine("}");
            b.AppendLine();
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}
