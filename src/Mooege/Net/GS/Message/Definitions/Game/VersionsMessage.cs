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
using Mooege.Common.Versions;

namespace Mooege.Net.GS.Message.Definitions.Game
{
    [Message(Opcodes.VersionsMessage)]
    public class VersionsMessage : GameMessage
    {
        public int SNOPackHash;
        public int ProtocolHash;
        public string Version;

        public VersionsMessage(int snoPacketHash):base(Opcodes.VersionsMessage)
        {
            this.SNOPackHash = snoPacketHash;
            this.ProtocolHash = VersionInfo.Ingame.ProtocolHash;
            this.Version = VersionInfo.Ingame.VersionString;
        }

        public VersionsMessage():base(Opcodes.VersionsMessage) { }

        public override void Parse(GameBitBuffer buffer)
        {
            SNOPackHash = buffer.ReadInt(32);
            ProtocolHash = buffer.ReadInt(32);
            Version = buffer.ReadCharArray(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, SNOPackHash);
            buffer.WriteInt(32, ProtocolHash);
            buffer.WriteCharArray(32, Version);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("VersionsMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("SNOPackHash: 0x" + SNOPackHash.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("ProtocolHash: 0x" + ProtocolHash.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("Version: \"" + Version + "\"");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }
    }
}