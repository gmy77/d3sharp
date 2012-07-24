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

namespace Mooege.Net.GS.Message.Definitions.Quest
{
    /// <summary>
    /// Sent to the client to update one quest counter. Each quest consists of a sequence of steps. In each step, a list
    /// of tasks must be completed. The questcounter shows the progress of a single task.
    /// </summary>
    [Message(Opcodes.QuestCounterMessage)]
    public class QuestCounterMessage : GameMessage
    {
        public int snoQuest;
        public int snoLevelArea;
        public int StepID;          // The logical sequence of steps in a quest can be an arbitrary sequence of ids 
        public int TaskIndex;       // 0-bound index of the task to update.
        public int Counter;         // Value of the counter of the task. Used for tasks like "1 of 4 monsters slain"
        public int Checked;         // 0 = Task is unchecked, 1 = Task is checked (completed)

        public QuestCounterMessage() : base(Opcodes.QuestCounterMessage) { }

        public override void Parse(GameBitBuffer buffer)
        {
            snoQuest = buffer.ReadInt(32);
            snoLevelArea = buffer.ReadInt(32);
            StepID = buffer.ReadInt(32);
            TaskIndex = buffer.ReadInt(32);
            Counter = buffer.ReadInt(32);
            Checked = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, snoQuest);
            buffer.WriteInt(32, snoLevelArea);
            buffer.WriteInt(32, StepID);
            buffer.WriteInt(32, TaskIndex);
            buffer.WriteInt(32, Counter);
            buffer.WriteInt(32, Checked);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("QuestCounterMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("snoQuest: 0x" + snoQuest.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("snoLevelArea: 0x" + snoLevelArea.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("StepID: 0x" + StepID.ToString("X8") + " (" + StepID + ")");
            b.Append(' ', pad); b.AppendLine("TaskIndex: 0x" + TaskIndex.ToString("X8") + " (" + TaskIndex + ")");
            b.Append(' ', pad); b.AppendLine("Counter: 0x" + Counter.ToString("X8") + " (" + Counter + ")");
            b.Append(' ', pad); b.AppendLine("Checked: 0x" + Checked.ToString("X8") + " (" + Checked + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}