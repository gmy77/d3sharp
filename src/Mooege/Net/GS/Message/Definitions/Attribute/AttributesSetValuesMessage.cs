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

namespace Mooege.Net.GS.Message.Definitions.Attribute
{
    [Message(Opcodes.AttributesSetValuesMessage)]
    public class AttributesSetValuesMessage : GameMessage
    {
        public uint ActorID; // Actor's DynamicID        
        public NetAttributeKeyValue[] atKeyVals; // MaxLength = 15

        public AttributesSetValuesMessage() : base(Opcodes.AttributesSetValuesMessage) { }

        public override void Parse(GameBitBuffer buffer)
        {
            ActorID = buffer.ReadUInt(32);
            atKeyVals = new NetAttributeKeyValue[buffer.ReadInt(4)];
            for (int i = 0; i < atKeyVals.Length; i++) { atKeyVals[i] = new NetAttributeKeyValue(); atKeyVals[i].Parse(buffer); }
            for (int i = 0; i < atKeyVals.Length; i++) { atKeyVals[i].ParseValue(buffer); }
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteUInt(32, ActorID);
            buffer.WriteInt(4, atKeyVals.Length);
            for (int i = 0; i < atKeyVals.Length; i++) { atKeyVals[i].Encode(buffer); }
            for (int i = 0; i < atKeyVals.Length; i++) { atKeyVals[i].EncodeValue(buffer); }
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("AttributesSetValuesMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("ActorID: 0x" + ActorID.ToString("X8") + " (" + ActorID + ")");
            b.Append(' ', pad); b.AppendLine("atKeyVals:");
            b.Append(' ', pad); b.AppendLine("{");
            for (int i = 0; i < atKeyVals.Length; i++) { atKeyVals[i].AsText(b, pad + 1); b.AppendLine(); }
            b.Append(' ', pad); b.AppendLine("}"); b.AppendLine();
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}
