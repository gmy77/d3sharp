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
using Mooege.Net.GS.Message.Fields;

namespace Mooege.Net.GS.Message.Definitions.Conversation
{
    /// <summary>
    /// Server -> Client
    /// 
    /// Plays a single line from a conversation.
    /// </summary>
    [Message(Opcodes.PlayConvLineMessage)]
    public class PlayConvLineMessage : GameMessage
    {
        public uint ActorID;             // The SNO of this actor is used, to get a localized "Name" of the conversation participant for chat ouput
        // MaxLength = 9
        public uint[] Field1;            // looks like a list of conversation participants - farmy
        public PlayLineParams PlayLineParams;
        public int Duration;             // playback duration in ms

        public PlayConvLineMessage() : base(Opcodes.PlayConvLineMessage) {}

        public override void Parse(GameBitBuffer buffer)
        {
            ActorID = buffer.ReadUInt(32);
            Field1 = new uint[9];
            for (int i = 0; i < Field1.Length; i++) Field1[i] = buffer.ReadUInt(32);
            PlayLineParams = new PlayLineParams();
            PlayLineParams.Parse(buffer);
            Duration = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteUInt(32, ActorID);
            for (int i = 0; i < Field1.Length; i++) buffer.WriteUInt(32, Field1[i]);
            PlayLineParams.Encode(buffer);
            buffer.WriteInt(32, Duration);
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
            PlayLineParams.AsText(b, pad);
            b.Append(' ', pad); b.AppendLine("Duration: 0x" + Duration.ToString("X8") + " (" + Duration + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}