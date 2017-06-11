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
using Mooege.Net.GS.Message;

namespace Mooege.Core.GS.Common.Types.Misc
{
    public class RGBAColor
    {
        public byte Red;
        public byte Green;
        public byte Blue;
        public byte Alpha;

        public RGBAColor() { }

        /// <summary>
        /// Reads RGBAColor from given MPQFileStream.
        /// </summary>
        /// <param name="stream">The MPQFileStream to read from.</param>
        public RGBAColor(MpqFileStream stream)
        {
            var buf = new byte[4];
            stream.Read(buf, 0, 4);
            Red = buf[0];
            Green = buf[1];
            Blue = buf[2];
            Alpha = buf[3];
        }

        /// <summary>
        /// Parses RGBAColor from given GameBitBuffer.
        /// </summary>
        /// <param name="buffer">The GameBitBuffer to parse from.</param>
        public void Parse(GameBitBuffer buffer)
        {
            Red = (byte)buffer.ReadInt(8);
            Green = (byte)buffer.ReadInt(8);
            Blue = (byte)buffer.ReadInt(8);
            Alpha = (byte)buffer.ReadInt(8);
        }

        /// <summary>
        /// Encodes RGBAColor to given GameBitBuffer.
        /// </summary>
        /// <param name="buffer">The GameBitBuffer to write.</param>
        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(8, Red);
            buffer.WriteInt(8, Green);
            buffer.WriteInt(8, Blue);
            buffer.WriteInt(8, Alpha);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("RGBAColor:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad);
            b.AppendLine("Reg: 0x" + Red.ToString("X2"));
            b.Append(' ', pad);
            b.AppendLine("Green: 0x" + Green.ToString("X2"));
            b.Append(' ', pad);
            b.AppendLine("Blue: 0x" + Blue.ToString("X2"));
            b.Append(' ', pad);
            b.AppendLine("Alpha: 0x" + Alpha.ToString("X2"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}