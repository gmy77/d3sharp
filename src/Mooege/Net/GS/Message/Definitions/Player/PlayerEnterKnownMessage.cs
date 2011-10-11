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

namespace Mooege.Net.GS.Message.Definitions.Player
{
    [Message(Opcodes.PlayerEnterKnownMessage)]
    public class PlayerEnterKnownMessage : GameMessage
    {
        public int PlayerIndex;
        public uint PlayerID; // Player's DynamicID

        public PlayerEnterKnownMessage() : base(Opcodes.PlayerEnterKnownMessage) { }

        public override void Parse(GameBitBuffer buffer)
        {
            PlayerIndex = buffer.ReadInt(3);
            PlayerID = buffer.ReadUInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(3, PlayerIndex);
            buffer.WriteUInt(32, PlayerID);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("PlayerEnterKnownMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + PlayerIndex.ToString("X8") + " (" + PlayerIndex + ")");
            b.Append(' ', pad); b.AppendLine("PlayerID: 0x" + PlayerID.ToString("X8") + " (" + PlayerID + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}
