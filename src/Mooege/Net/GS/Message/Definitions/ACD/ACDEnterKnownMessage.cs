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
using Mooege.Net.GS.Message.Fields;

namespace Mooege.Net.GS.Message.Definitions.ACD
{
    /// <summary>
    /// Sent to client, to introduce a new actor.
    /// </summary>
    public class ACDEnterKnownMessage : GameMessage
    {
        public uint ActorID; // The actor's DynamicID
        public int /* sno */ ActorSNO;

        // For many actors, bit 0x8 is set the first time the item is introduced with ACDEnterKnown... if the item
        // is later deleted with an ANN Message and reintroduced, 0x8 is NOT set... (StartLocation, BlockingCart, MarkerLocation)
        // - farmy

        public int Field2;
        public int Field3;      // 0 = WorldLocationMessageData is set, 1 = InventoryLocationMessageData is set ...
        public WorldLocationMessageData WorldLocation;
        public InventoryLocationMessageData InventoryLocation;
        public GBHandle GBHandle;
        public int Field7;
        public int Field8;      
        public int Field9;      // Item quality if an item, otherwise 0
        public byte Field10;
        public int? /* sno */ Field11;
        public int? Field12;
        public int? Field13;    // Seems to be a running number for all actors, just a counting how many actors there already are

        public ACDEnterKnownMessage() : base(Opcodes.ACDEnterKnownMessage) { }

        public override void Parse(GameBitBuffer buffer)
        {
            ActorID = buffer.ReadUInt(32);
            ActorSNO = buffer.ReadInt(32);
            Field2 = buffer.ReadInt(5);
            Field3 = buffer.ReadInt(2) + (-1);
            if (buffer.ReadBool())
            {
                WorldLocation = new WorldLocationMessageData();
                WorldLocation.Parse(buffer);
            }
            if (buffer.ReadBool())
            {
                InventoryLocation = new InventoryLocationMessageData();
                InventoryLocation.Parse(buffer);
            }
            GBHandle = new GBHandle();
            GBHandle.Parse(buffer);
            Field7 = buffer.ReadInt(32);
            Field8 = buffer.ReadInt(32);
            Field9 = buffer.ReadInt(4) + (-1);
            Field10 = (byte)buffer.ReadInt(8);
            if (buffer.ReadBool())
            {
                Field11 = buffer.ReadInt(32);
            }
            if (buffer.ReadBool())
            {
                Field12 = buffer.ReadInt(32);
            }
            if (buffer.ReadBool())
            {
                Field13 = buffer.ReadInt(32);
            }
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteUInt(32, ActorID);
            buffer.WriteInt(32, ActorSNO);
            buffer.WriteInt(5, Field2);
            buffer.WriteInt(2, Field3 - (-1));
            buffer.WriteBool(WorldLocation != null);
            if (WorldLocation != null)
            {
                WorldLocation.Encode(buffer);
            }
            buffer.WriteBool(InventoryLocation != null);
            if (InventoryLocation != null)
            {
                InventoryLocation.Encode(buffer);
            }
            GBHandle.Encode(buffer);
            buffer.WriteInt(32, Field7);
            buffer.WriteInt(32, Field8);
            buffer.WriteInt(4, Field9 - (-1));
            buffer.WriteInt(8, Field10);
            buffer.WriteBool(Field11.HasValue);
            if (Field11.HasValue)
            {
                buffer.WriteInt(32, Field11.Value);
            }
            buffer.WriteBool(Field12.HasValue);
            if (Field12.HasValue)
            {
                buffer.WriteInt(32, Field12.Value);
            }
            buffer.WriteBool(Field13.HasValue);
            if (Field13.HasValue)
            {
                buffer.WriteInt(32, Field13.Value);
            }
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("ACDEnterKnownMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("ActorID: 0x" + ActorID.ToString("X8") + " (" + ActorID + ")");
            b.Append(' ', pad); b.AppendLine("ActorSNO: 0x" + ActorSNO.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', pad); b.AppendLine("Field3: 0x" + Field3.ToString("X8") + " (" + Field3 + ")");
            if (WorldLocation != null)
            {
                WorldLocation.AsText(b, pad);
            }
            if (InventoryLocation != null)
            {
                InventoryLocation.AsText(b, pad);
            }
            GBHandle.AsText(b, pad);
            b.Append(' ', pad); b.AppendLine("Field7: 0x" + Field7.ToString("X8") + " (" + Field7 + ")");
            b.Append(' ', pad); b.AppendLine("Field8: 0x" + Field8.ToString("X8") + " (" + Field8 + ")");
            b.Append(' ', pad); b.AppendLine("Field9: 0x" + Field9.ToString("X8") + " (" + Field9 + ")");
            b.Append(' ', pad); b.AppendLine("Field10: 0x" + Field10.ToString("X2"));
            if (Field11.HasValue)
            {
                b.Append(' ', pad); b.AppendLine("Field11.Value: 0x" + Field11.Value.ToString("X8"));
            }
            if (Field12.HasValue)
            {
                b.Append(' ', pad); b.AppendLine("Field12.Value: 0x" + Field12.Value.ToString("X8") + " (" + Field12.Value + ")");
            }
            if (Field13.HasValue)
            {
                b.Append(' ', pad); b.AppendLine("Field13.Value: 0x" + Field13.Value.ToString("X8") + " (" + Field13.Value + ")");
            }
            b.Append(' ', --pad);
            b.AppendLine("}");
        }        
    }
}
