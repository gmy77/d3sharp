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

namespace Mooege.Net.GS.Message.Definitions.Conversation
{
    /// <summary>
    /// Sent to the client
    /// TODO What does this message actually do? sending it not changes nothing. - farmy
    /// </summary>
    public class EndConversationMessage : GameMessage
    {
        public int Field0;          // seems to be a running number across conversationlines. StopConvLine.Field0 == EndConvLine.Field0 == PlayConvLine.PlayLineParams.Field14 for a conversation
        public int SNOConversation;
        public int ActorID;         // Actor that begun conversation in PlayConvLine

        public EndConversationMessage() : base(Opcodes.EndConversationMessage) { }


        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            SNOConversation = buffer.ReadInt(32);
            ActorID = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(32, SNOConversation);
            buffer.WriteInt(32, ActorID);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("EndConversationMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("SNOConversation: 0x" + SNOConversation.ToString("X8") + " (" + SNOConversation + ")");
            b.Append(' ', pad); b.AppendLine("ActorID: 0x" + ActorID.ToString("X8") + " (" + ActorID + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}