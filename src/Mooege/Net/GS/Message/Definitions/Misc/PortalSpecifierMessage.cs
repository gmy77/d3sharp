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

namespace Mooege.Net.GS.Message.Definitions.Misc
{
    [Message(Opcodes.PortalSpecifierMessage)]
    public class PortalSpecifierMessage : GameMessage
    {
        public uint ActorID; // Portal's DynamicID
        public ResolvedPortalDestination Destination;

        public PortalSpecifierMessage() : base(Opcodes.PortalSpecifierMessage) {}

        public override void Parse(GameBitBuffer buffer)
        {
            ActorID = buffer.ReadUInt(32);
            Destination = new ResolvedPortalDestination();
            Destination.Parse(buffer);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteUInt(32, ActorID);
            Destination.Encode(buffer);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("PortalSpecifierMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("ActorID: 0x" + ActorID.ToString("X8") + " (" + ActorID + ")");
            Destination.AsText(b, pad);
            b.Append(' ', --pad);
            b.AppendLine("}");
        }
    }
}
