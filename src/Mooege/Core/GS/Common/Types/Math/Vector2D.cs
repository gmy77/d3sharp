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
using Mooege.Net.GS.Message;
using Mooege.Common.Storage;

namespace Mooege.Core.GS.Common.Types.Math
{
    public class Vector2D
    {
        [PersistentProperty("X")]
        public int X { get; set; }

        [PersistentProperty("Y")]
        public int Y { get; set; }

        public Vector2D() { }

        /// <summary>
        /// Reads Vector2D from given MPQFileStream.
        /// </summary>
        /// <param name="stream">The MPQFileStream to read from.</param>
        public Vector2D(MpqFileStream stream)
        {
            X = stream.ReadValueS32();
            Y = stream.ReadValueS32();
        }

        public Vector2D(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// Parses Vector2D from given GameBitBuffer.
        /// </summary>
        /// <param name="buffer">The GameBitBuffer to parse from.</param>
        public void Parse(GameBitBuffer buffer)
        {
            X = buffer.ReadInt(32);
            Y = buffer.ReadInt(32);
        }

        /// <summary>
        /// Encodes Vector2D to given GameBitBuffer.
        /// </summary>        
        /// <param name="buffer">The GameBitBuffer to write.</param>
        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, X);
            buffer.WriteInt(32, Y);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("Vector2D:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad);
            b.AppendLine("X: 0x" + X.ToString("X8") + " (" + X + ")");
            b.Append(' ', pad);
            b.AppendLine("Y: 0x" + Y.ToString("X8") + " (" + Y + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }

        public override string ToString()
        {
            return string.Format("x:{0} y:{1}", X, Y);
        }
    }
}
