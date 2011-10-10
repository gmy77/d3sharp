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
    public class PlayerActorSetInitialMessage : GameMessage
    {
        public uint PlayerID; // Player's DynamicID
        public int PlayerIndex;

        public PlayerActorSetInitialMessage() : base(Opcodes.PlayerActorSetInitialMessage) { }

        public override void Parse(GameBitBuffer buffer)
        {
            PlayerID = buffer.ReadUInt(32);
            PlayerIndex = buffer.ReadInt(3);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteUInt(32, PlayerID);
            buffer.WriteInt(3, PlayerIndex);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("PlayerActorSetInitialMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("PlayerID: 0x" + PlayerID.ToString("X8") + " (" + PlayerID + ")");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + PlayerIndex.ToString("X8") + " (" + PlayerIndex + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}
