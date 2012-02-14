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

namespace Mooege.Net.GS.Message.Definitions.Trade
{
    /// <summary>
    /// Sent by the client, when the player sells an item to a vendor via trade window (not when using cauldron of jordan)
    /// </summary>
    [Message(Opcodes.RequestSellItemMessage)]
    public class RequestSellItemMessage : GameMessage
    {
        public int ItemId;

        public RequestSellItemMessage() { }
        public RequestSellItemMessage(int itemID)
            : base(Opcodes.RequestSellItemMessage)
        {
            ItemId = itemID;
        }

        public override void Parse(GameBitBuffer buffer)
        {
            ItemId = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, ItemId);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("SellItemMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("ItemId: 0x" + ItemId.ToString("X8") + " (" + ItemId + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }

    }
}