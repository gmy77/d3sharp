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
using Mooege.Net.GS.Message.Fields;

namespace Mooege.Net.GS.Message.Definitions.Misc
{
    /// <summary>
    /// Plays a single line from a conversation.
    /// Proper SNOconversation with correct LineID and correct Field4 will play everything (so far)
    /// </summary>
    public class PlayConvLineMessage : GameMessage
    {
        public int ActorID;             // The SNO of this actor is used, to get a localized "Name" of the conversation participant for chat ouput
        // MaxLength = 9
        public int[] Field1;            // looks like a list of conversation participants
        public PlayLineParams Params;
        public int Field3;              // seems to be a running number across conversationlines. StopConvLine.Field0 == PlayConvLine.Field3 == EndConvLine.Field0 == PlayConvLine.PlayLineParams.Field14 for a conversation

        public PlayConvLineMessage() : base(Opcodes.PlayConvLineMessage) {}

        public override void Parse(GameBitBuffer buffer)
        {
            ActorID = buffer.ReadInt(32);
            Field1 = new int[9];
            for (int i = 0; i < Field1.Length; i++) Field1[i] = buffer.ReadInt(32);
            Params = new PlayLineParams();
            Params.Parse(buffer);
            Field3 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, ActorID);
            for (int i = 0; i < Field1.Length; i++) buffer.WriteInt(32, Field1[i]);
            Params.Encode(buffer);
            buffer.WriteInt(32, Field3);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("PlayConvLineMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("ActorID: 0x" + ActorID.ToString("X8") + " (" + ActorID + ")");
            b.Append(' ', pad); b.AppendLine("Field1:");
            b.Append(' ', pad); b.AppendLine("{");
            for (int i = 0; i < Field1.Length; ) { b.Append(' ', pad + 1); for (int j = 0; j < 8 && i < Field1.Length; j++, i++) { b.Append("0x" + Field1[i].ToString("X8") + ", "); } b.AppendLine(); }
            b.Append(' ', pad); b.AppendLine("}"); b.AppendLine();
            Params.AsText(b, pad);
            b.Append(' ', pad); b.AppendLine("Field3: 0x" + Field3.ToString("X8") + " (" + Field3 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}