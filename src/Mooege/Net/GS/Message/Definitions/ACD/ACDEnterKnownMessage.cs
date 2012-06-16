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
    /// Sent to client, to introduce a new actor.
    /// </summary>
    [Message(Opcodes.ACDEnterKnownMessage)]
    public class ACDEnterKnownMessage : GameMessage
    {
        public uint ActorID; // The actor's DynamicID
        public int ActorSNOId;

        // For many actors, bit 0x8 is set the first time the item is introduced with ACDEnterKnown... if the item
        // is later deleted with an ANN Message and reintroduced, 0x8 is NOT set... (StartLocation, BlockingCart, MarkerLocation)
        // - farmy

        public int Field2;
        public int Field3;      // 0 = WorldLocationMessageData is set, 1 = InventoryLocationMessageData is set ...
        public WorldLocationMessageData WorldLocation;
        public InventoryLocationMessageData InventoryLocation;
        public GBHandle GBHandle;
        public int Field7;
        public int NameSNOId;    // Actor providing a name for the this actor, you can name zombies leah etc..  
        public int Quality;         // Item quality if an item, SpawnType for mobs, 0 otherwise
        public byte Field10;
        public int? /* sno */ Field11;
        public int? MarkerSetSNO;
        public int? MarkerSetIndex;

        public ACDEnterKnownMessage() : base(Opcodes.ACDEnterKnownMessage) { }

        public override void Parse(GameBitBuffer buffer)
        {
            ActorID = buffer.ReadUInt(32);
            ActorSNOId = buffer.ReadInt(32);
            Field2 = buffer.ReadInt(6);
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
            NameSNOId = buffer.ReadInt(32);
            Quality = buffer.ReadInt(4) + (-1);
            Field10 = (byte)buffer.ReadInt(8);
            if (buffer.ReadBool())
            {
                Field11 = buffer.ReadInt(32);
            }
            if (buffer.ReadBool())
            {
                MarkerSetSNO = buffer.ReadInt(32);
            }
            if (buffer.ReadBool())
            {
                MarkerSetIndex = buffer.ReadInt(32);
            }
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteUInt(32, ActorID);
            buffer.WriteInt(32, ActorSNOId);
            buffer.WriteInt(6, Field2);
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
            buffer.WriteInt(32, NameSNOId);
            buffer.WriteInt(4, Quality - (-1));
            buffer.WriteInt(8, Field10);
            buffer.WriteBool(Field11.HasValue);
            if (Field11.HasValue)
            {
                buffer.WriteInt(32, Field11.Value);
            }
            buffer.WriteBool(MarkerSetSNO.HasValue);
            if (MarkerSetSNO.HasValue)
            {
                buffer.WriteInt(32, MarkerSetSNO.Value);
            }
            buffer.WriteBool(MarkerSetIndex.HasValue);
            if (MarkerSetIndex.HasValue)
            {
                buffer.WriteInt(32, MarkerSetIndex.Value);
            }
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("ACDEnterKnownMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("ActorID: 0x" + ActorID.ToString("X8") + " (" + ActorID + ")");
            b.Append(' ', pad); b.AppendLine("ActorSNOId: 0x" + ActorSNOId.ToString("X8"));
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
            b.Append(' ', pad); b.AppendLine("NameSNOId: 0x" + NameSNOId.ToString("X8") + " (" + NameSNOId + ")");
            b.Append(' ', pad); b.AppendLine("Quality: 0x" + Quality.ToString("X8") + " (" + Quality + ")");
            b.Append(' ', pad); b.AppendLine("Field10: 0x" + Field10.ToString("X2"));
            if (Field11.HasValue)
            {
                b.Append(' ', pad); b.AppendLine("Field11.Value: 0x" + Field11.Value.ToString("X8"));
            }
            if (MarkerSetSNO.HasValue)
            {
                b.Append(' ', pad); b.AppendLine("MarkerSetSNO.Value: 0x" + MarkerSetSNO.Value.ToString("X8") + " (" + MarkerSetSNO.Value + ")");
            }
            if (MarkerSetIndex.HasValue)
            {
                b.Append(' ', pad); b.AppendLine("MarkerSetIndex.Value: 0x" + MarkerSetIndex.Value.ToString("X8") + " (" + MarkerSetIndex.Value + ")");
            }
            b.Append(' ', --pad);
            b.AppendLine("}");
        }
    }
}
