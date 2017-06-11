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

namespace Mooege.Net.GS.Message.Definitions.Conversation
{
    /// <summary>
    /// Client -> Server
    /// 
    /// Sent by the client to request stopping playback of the current line
    /// and continue with the next line of the conversation
    /// (Player closes the conversation dialog window)
    /// </summary>
    [Message(Opcodes.AdvanceConvMessage, Consumers.Conversations)]
    class AdvanceConvMessage : GameMessage
    {
        /// <summary>
        /// SNO of the conversation
        /// </summary>
        public int SNOConversation;

        /// <summary>
        /// Identifier of the PlayLineParams as used in PlayConvLineMessage to start the conversation
        /// </summary>
        public int PlayLineParamsId;

        public override void Parse(GameBitBuffer buffer)
        {
            SNOConversation = buffer.ReadInt(32);
            PlayLineParamsId = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, SNOConversation);
            buffer.WriteInt(32, PlayLineParamsId);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("AdvanceConvMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("SNOConversation: 0x" + SNOConversation.ToString("X8") + " (" + SNOConversation + ")");
            b.Append(' ', pad); b.AppendLine("PlayLineParamsId: 0x" + PlayLineParamsId.ToString("X8") + " (" + PlayLineParamsId + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }
    }
}
