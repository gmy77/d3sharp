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
using CrystalMpq;
using Gibbed.IO;
using Mooege.Common.MPQ;
using Mooege.Net.GS.Message;

namespace Mooege.Core.GS.Common.Types.SNO
{
    public class SNOName
    {
        public SNOGroup Group;
        public int SNOId; /* snoname_handle */

        public SNOName() { }

        /// <summary>
        /// Reads SNOName from given MPQFileStream.
        /// </summary>
        /// <param name="stream">The MPQFileStream to read from.</param>
        public SNOName(MpqFileStream stream)
        {
            this.Group = (SNOGroup)stream.ReadValueS32();
            this.SNOId = stream.ReadValueS32();
        }

        public string Name
        {
            get
            {
                if (!MPQStorage.Data.Assets.ContainsKey(this.Group))
                    return ""; // it's here because of the SNOGroup 0, could it be the Act? /raist
                return MPQStorage.Data.Assets[this.Group].ContainsKey(this.SNOId)
                                ? MPQStorage.Data.Assets[this.Group][SNOId].Name
                                : ""; // put it here because it seems we miss loading some scenes there /raist.                
            }
        }

        /// <summary>
        /// Parses SNOName from given GameBitBuffer.
        /// </summary>
        /// <param name="buffer">The GameBitBuffer to parse from.</param>
        public void Parse(GameBitBuffer buffer)
        {
            Group = (SNOGroup)buffer.ReadInt(32);
            SNOId = buffer.ReadInt(32);
        }

        /// <summary>
        /// Encodes SNOName to given GameBitBuffer.
        /// </summary>
        /// <param name="buffer">The GameBitBuffer to write.</param>
        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, (int)Group);
            buffer.WriteInt(32, SNOId);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("SNOName:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad);
            b.AppendLine("Group: 0x" + ((int)Group).ToString("X8"));
            b.Append(' ', pad);
            b.AppendLine("SNOId: 0x" + SNOId.ToString("X8"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }

        public override string ToString()
        {
            return string.Format("[{0}] {1} - {2}", this.Group, this.SNOId, this.Name);
        }
    }
}
