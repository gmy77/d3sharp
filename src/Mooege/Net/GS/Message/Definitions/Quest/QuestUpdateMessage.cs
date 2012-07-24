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
    /// Sent to the client to inform him, that a certain step of a quest is completed and
    /// makes him display the task list for the next step
    /// </summary>
    [Message(Opcodes.QuestUpdateMessage)]
    public class QuestUpdateMessage : GameMessage
    {
        public int snoQuest;
        public int snoLevelArea;
        public int StepID;          // ID of the step, for which the task list should be displayed
        public bool Field3;         // not sure, if not set to true, nothing happens - farmy
        public bool Failed;         // Quest failed

        public QuestUpdateMessage() : base(Opcodes.QuestUpdateMessage) { }

        public override void Parse(GameBitBuffer buffer)
        {
            snoQuest = buffer.ReadInt(32);
            snoLevelArea = buffer.ReadInt(32);
            StepID = buffer.ReadInt(32);
            Field3 = buffer.ReadBool();
            Failed = buffer.ReadBool();
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, snoQuest);
            buffer.WriteInt(32, snoLevelArea);
            buffer.WriteInt(32, StepID);
            buffer.WriteBool(Field3);
            buffer.WriteBool(Failed);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("QuestUpdateMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("snoQuest: 0x" + snoQuest.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("snoLevelArea: 0x" + snoLevelArea.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("StepID: 0x" + StepID.ToString("X8") + " (" + StepID + ")");
            b.Append(' ', pad); b.AppendLine("Field3: " + (Field3 ? "true" : "false"));
            b.Append(' ', pad); b.AppendLine("Failed: " + (Failed ? "true" : "false"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}