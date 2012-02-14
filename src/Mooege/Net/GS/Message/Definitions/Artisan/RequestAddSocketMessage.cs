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

namespace Mooege.Net.GS.Message.Definitions.Artisan
{
    //[Message(Opcodes.RequestAddSocketMessage, Consumers.Player)]
    public class RequestAddSocketMessage : GameMessage
    {
        public uint ItemID;

        public override void Parse(GameBitBuffer buffer)
        {
            ItemID = buffer.ReadUInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteUInt(32, ItemID);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("RequestAddSocketMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("ItemID: 0x" + ItemID.ToString("X8") + " (" + ItemID + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }

    }

}
