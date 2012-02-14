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

namespace Mooege.Net.GS.Message.Definitions.Stash
{
    /// <summary>
    /// Sent to the client, to open the inventory of a known actor as stash inventory
    /// </summary>
    [Message(Opcodes.OpenSharedStashMessage)]
    public class OpenSharedStashMessage : GameMessage
    {
        public int ActorId;

        public OpenSharedStashMessage() { }
        public OpenSharedStashMessage(int stashId)
            : base(Opcodes.OpenSharedStashMessage)
        {
            ActorId = stashId;
        }

        public override void Parse(GameBitBuffer buffer)
        {
            ActorId = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, ActorId);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("OpenSharedStackMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("ActorId: 0x" + ActorId.ToString("X8") + " (" + ActorId + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }
    }
}