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
using CrystalMpq;
using Gibbed.IO;
using Mooege.Core.GS.Common.Types.Collision;
using Mooege.Net.GS.Message;
using Mooege.Common.Storage;

namespace Mooege.Core.GS.Common.Types.Scene
{
    public class SceneCachedValues
    {
        [PersistentProperty("Unknown1")]
        public int Unknown1 { get; set; }
        [PersistentProperty("Unknown2")]
        public int Unknown2 { get; set; }
        [PersistentProperty("Unknown3")]
        public int Unknown3 { get; set; }
        [PersistentProperty("AABB1")]
        public AABB AABB1 { get; set; }
        [PersistentProperty("AABB2")]
        public AABB AABB2 { get; set; }
        [PersistentProperty("Unknown4", 4)]
        public int[] Unknown4 { get; set; }    // MaxLength = 4
        [PersistentProperty("Unknown5")]
        public int Unknown5 { get; set; }

        public SceneCachedValues() { }

        /// <summary>
        /// Reads SceneCachedValues from given MPQFileStream.
        /// </summary>
        /// <param name="stream">The MPQFileStream to read from.</param>
        public SceneCachedValues(MpqFileStream stream)
        {
            Unknown1 = stream.ReadValueS32();
            Unknown2 = stream.ReadValueS32();
            Unknown3 = stream.ReadValueS32();
            AABB1 = new AABB(stream);
            AABB2 = new AABB(stream);
            Unknown4 = new int[4];
            for (int i = 0; i < Unknown4.Length; i++)
            {
                Unknown4[i] = stream.ReadValueS32();
            }
            Unknown5 = stream.ReadValueS32();
        }

        /// <summary>
        /// Parses SceneCachedValues from given GameBitBuffer.
        /// </summary>
        /// <param name="buffer">The GameBitBuffer to parse from.</param>
        public void Parse(GameBitBuffer buffer)
        {
            Unknown1 = buffer.ReadInt(32);
            Unknown2 = buffer.ReadInt(32);
            Unknown3 = buffer.ReadInt(32);
            AABB1 = new AABB();
            AABB1.Parse(buffer);
            AABB2 = new AABB();
            AABB2.Parse(buffer);
            Unknown4 = new int[4];
            for (int i = 0; i < Unknown4.Length; i++) Unknown4[i] = buffer.ReadInt(32);
            Unknown5 = buffer.ReadInt(32);
        }

        /// <summary>
        /// Encodes SceneCachedValues to given GameBitBuffer.
        /// </summary>
        /// <param name="buffer">The GameBitBuffer to write.</param>
        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Unknown1);
            buffer.WriteInt(32, Unknown2);
            buffer.WriteInt(32, Unknown3);
            AABB1.Encode(buffer);
            AABB2.Encode(buffer);
            for (int i = 0; i < Unknown4.Length; i++) buffer.WriteInt(32, Unknown4[i]);
            buffer.WriteInt(32, Unknown5);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("SceneCachedValues:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad);
            b.AppendLine("Unknown1: 0x" + Unknown1.ToString("X8") + " (" + Unknown1 + ")");
            b.Append(' ', pad);
            b.AppendLine("Unknown2: 0x" + Unknown2.ToString("X8") + " (" + Unknown2 + ")");
            b.Append(' ', pad);
            b.AppendLine("Unknown3: 0x" + Unknown3.ToString("X8") + " (" + Unknown3 + ")");
            AABB1.AsText(b, pad);
            AABB2.AsText(b, pad);
            b.Append(' ', pad);
            b.AppendLine("Unknown4:");
            b.Append(' ', pad);
            b.AppendLine("{");
            for (int i = 0; i < Unknown4.Length; )
            {
                b.Append(' ', pad + 1);
                for (int j = 0; j < 8 && i < Unknown4.Length; j++, i++)
                {
                    b.Append("0x" + Unknown4[i].ToString("X8") + ", ");
                }
                b.AppendLine();
            }
            b.Append(' ', pad);
            b.AppendLine("}");
            b.AppendLine();
            b.Append(' ', pad);
            b.AppendLine("Unknown5: 0x" + Unknown5.ToString("X8") + " (" + Unknown5 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}