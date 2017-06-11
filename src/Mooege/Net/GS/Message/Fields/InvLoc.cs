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

namespace Mooege.Net.GS.Message.Fields
{
    public class InvLoc
    {
        public uint OwnerID; // Owner's DynamicID
        public int EquipmentSlot;
        public int Column;
        public int Row;

        public void Parse(GameBitBuffer buffer)
        {
            OwnerID = buffer.ReadUInt(32);
            EquipmentSlot = buffer.ReadInt(5) + (-1);
            Column = buffer.ReadInt(32);
            Row = buffer.ReadInt(32);
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteUInt(32, OwnerID);
            buffer.WriteInt(5, EquipmentSlot - (-1));
            buffer.WriteInt(32, Column);
            buffer.WriteInt(32, Row);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("InvLoc:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad);
            b.AppendLine("OwnerID: 0x" + OwnerID.ToString("X8") + " (" + OwnerID + ")");
            b.Append(' ', pad);
            b.AppendLine("EquipmentSlot: 0x" + EquipmentSlot.ToString("X8") + " (" + EquipmentSlot + ")");
            b.Append(' ', pad);
            b.AppendLine("Column: 0x" + Column.ToString("X8") + " (" + Column + ")");
            b.Append(' ', pad);
            b.AppendLine("Row: 0x" + Row.ToString("X8") + " (" + Row + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}
