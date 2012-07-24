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
using Mooege.Core.GS.Common.Types.Math;

namespace Mooege.Net.GS.Message.Fields
{
    public class InventoryLocationMessageData
    {
        public uint OwnerID; // Player's DynamicID
        public int EquipmentSlot;
        public Vector2D InventoryLocation; // Row, column

        public void Parse(GameBitBuffer buffer)
        {
            OwnerID = buffer.ReadUInt(32);
            EquipmentSlot = buffer.ReadInt(5) + (-1);
            InventoryLocation = new Vector2D();
            InventoryLocation.Parse(buffer);
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteUInt(32, OwnerID);
            buffer.WriteInt(5, EquipmentSlot - (-1));
            InventoryLocation.Encode(buffer);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("InventoryLocationMessageData:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad);
            b.AppendLine("OwnerID: 0x" + OwnerID.ToString("X8") + " (" + OwnerID + ")");
            b.Append(' ', pad);
            b.AppendLine("EquipmentSlot: 0x" + EquipmentSlot.ToString("X8") + " (" + EquipmentSlot + ")");
            InventoryLocation.AsText(b, pad);
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}
