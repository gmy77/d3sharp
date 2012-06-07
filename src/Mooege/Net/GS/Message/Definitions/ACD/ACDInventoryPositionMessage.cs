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

namespace Mooege.Net.GS.Message.Definitions.ACD
{
    /// <summary>
    /// Sent to the client to move an item into an actors inventory
    /// </summary>
    [Message(Opcodes.ACDInventoryPositionMessage)]
    public class ACDInventoryPositionMessage : GameMessage
    {
        public uint ItemId;                                     // Item's DynamicID
        public InventoryLocationMessageData InventoryLocation;  // Target inventory
        public int Field2;                                      // have not seen != 1... need to be 1 or nothing happens - farmy

        public ACDInventoryPositionMessage()
            : base(Opcodes.ACDInventoryPositionMessage)
        { }

        public override void Parse(GameBitBuffer buffer)
        {
            ItemId = buffer.ReadUInt(32);
            InventoryLocation = new InventoryLocationMessageData();
            InventoryLocation.Parse(buffer);
            Field2 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteUInt(32, ItemId);
            if (InventoryLocation != null)
            {
                InventoryLocation.Encode(buffer);
            }
            buffer.WriteInt(32, Field2);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("ACDInventoryPositionMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("ItemId: 0x" + ItemId.ToString("X8") + " (" + ItemId + ")");
            if (InventoryLocation != null)
            {
                InventoryLocation.AsText(b, pad);
            }
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}
