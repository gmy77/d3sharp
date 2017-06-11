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

namespace Mooege.Net.GS.Message.Definitions.Inventory
{
    [Message(Opcodes.InventoryRequestQuickMoveMessage, Consumers.Inventory)]
    public class InventoryRequestQuickMoveMessage : GameMessage
    {
        public uint ItemID;
        public int Field1;
        public int DestEquipmentSlot;
        public int DestRowStart;
        public int DestRowEnd;

        public override void Parse(GameBitBuffer buffer)
        {
            ItemID = buffer.ReadUInt(32);
            Field1 = buffer.ReadInt(32);
            DestEquipmentSlot = buffer.ReadInt(5) + (-1);
            DestRowStart = buffer.ReadInt(32);
            DestRowEnd = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteUInt(32, ItemID);
            buffer.WriteInt(32, Field1);
            buffer.WriteInt(5, DestEquipmentSlot - (-1));
            buffer.WriteInt(32, DestRowStart);
            buffer.WriteInt(32, DestRowEnd);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("InventoryRequestQuickMoveMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + ItemID.ToString("X8") + " (" + ItemID + ")");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + DestEquipmentSlot.ToString("X8") + " (" + DestEquipmentSlot + ")");
            b.Append(' ', pad); b.AppendLine("Field3: 0x" + DestRowStart.ToString("X8") + " (" + DestRowStart + ")");
            b.Append(' ', pad); b.AppendLine("Field4: 0x" + DestRowEnd.ToString("X8") + " (" + DestRowEnd + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}
