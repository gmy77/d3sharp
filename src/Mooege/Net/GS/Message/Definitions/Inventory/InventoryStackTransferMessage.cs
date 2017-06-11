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

namespace Mooege.Net.GS.Message.Definitions.Inventory
{
    [Message(Opcodes.InventoryStackTransferMessage, Consumers.Inventory)]
    public class InventoryStackTransferMessage : GameMessage
    {
        public uint FromID;
        public uint ToID;
        public ulong Amount;

        public override void Parse(GameBitBuffer buffer)
        {
            FromID = buffer.ReadUInt(32);
            ToID = buffer.ReadUInt(32);
            Amount = buffer.ReadUInt64(64);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteUInt(32, FromID);
            buffer.WriteUInt(32, ToID);
            buffer.WriteUInt64(64, Amount);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("InventoryStackTransferMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("FromID: 0x" + FromID.ToString("X8") + " (" + FromID + ")");
            b.Append(' ', pad); b.AppendLine("ToID: 0x" + ToID.ToString("X8") + " (" + ToID + ")");
            b.Append(' ', pad); b.AppendLine("Amount: 0x" + Amount.ToString("X16"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}
