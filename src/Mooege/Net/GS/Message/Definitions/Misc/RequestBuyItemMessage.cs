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

namespace Mooege.Net.GS.Message.Definitions.Misc
{
    /// <summary>
    /// Sent by the client, when the player buys an item from a vendor
    /// </summary>
    [IncomingMessage(Opcodes.RequestBuyItemMessage)]
    public class RequestBuyItemMessage : GameMessage
    {
        public int ItemActorId;

        public RequestBuyItemMessage() { }
        public RequestBuyItemMessage(int itemID)
            : base(Opcodes.RequestBuyItemMessage)
        {
            ItemActorId = itemID;
        }

        public override void Parse(GameBitBuffer buffer)
        {
            ItemActorId = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, ItemActorId);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("RequestBuyItemMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("ItemActorId: 0x" + ItemActorId.ToString("X8") + " (" + ItemActorId + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }

    }
}