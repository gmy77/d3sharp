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
    public class NPCInteraction
    {
        public NPCInteractionType Type;
        public int ConversationSNO;
        public int Field2;
        public NPCInteractionState State;

        public void Parse(GameBitBuffer buffer)
        {
            Type = (NPCInteractionType) buffer.ReadInt(4);
            ConversationSNO = buffer.ReadInt(32);
            Field2 = buffer.ReadInt(32);
            State = (NPCInteractionState) buffer.ReadInt(2);
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(4, (int) Type);
            buffer.WriteInt(32, ConversationSNO);
            buffer.WriteInt(32, Field2);
            buffer.WriteInt(2, (int) State);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("NPCInteraction:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad);
            b.AppendLine("Type: 0x" + ((int)Type).ToString("X8") + " (" + Type + ")");
            b.Append(' ', pad);
            b.AppendLine("ConversationSNO: 0x" + ConversationSNO.ToString("X8") + " (" + ConversationSNO + ")");
            b.Append(' ', pad);
            b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', pad);
            b.AppendLine("State: 0x" + ((int)State).ToString("X8") + " (" + State + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public enum NPCInteractionType
    {
        Unknown0 = 0,
        Unknown1 = 1,
        Conversation2 = 2,  // Same as conversation, but not seen in logs? /fasbat
        Conversation = 3,
        Unknown4 = 4,
        Craft = 5,
        IdentifyAll = 6,
        Hire = 7,
        Inventory = 8
    }

    public enum NPCInteractionState
    {
        Unknown0 = 0, // Same as disabled, but should it be used? /fasbat
        New = 1,
        Disabled = 2,
        Used = 3
    }
}