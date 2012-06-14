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
using Mooege.Core.GS.Common.Types.Math;
using Mooege.Net.GS.Message;
using Mooege.Common.Storage;

namespace Mooege.Core.GS.Common.Types.Collision
{
    public class AABB
    {
        [PersistentProperty("Min")]
        public Vector3D Min { get; private set; }
        [PersistentProperty("Max")]
        public Vector3D Max { get; private set; }

        public AABB() { }

        /// <summary>
        /// Reads AABB from given MPQFileStream.
        /// </summary>
        /// <param name="stream">The MPQFileStream to read from.</param>
        public AABB(MpqFileStream stream)
        {
            this.Min = new Vector3D(stream.ReadValueF32(), stream.ReadValueF32(), stream.ReadValueF32());
            this.Max = new Vector3D(stream.ReadValueF32(), stream.ReadValueF32(), stream.ReadValueF32());
        }

        /// <summary>
        /// Parses AABB from given GameBitBuffer.
        /// </summary>
        /// <param name="buffer">The GameBitBuffer to parse from.</param>
        public void Parse(GameBitBuffer buffer)
        {
            Min = new Vector3D();
            Min.Parse(buffer);
            Max = new Vector3D();
            Max.Parse(buffer);
        }

        /// <summary>
        /// Encodes AABB to given GameBitBuffer.
        /// </summary>
        /// <param name="buffer">The GameBitBuffer to write.</param>
        public void Encode(GameBitBuffer buffer)
        {
            Min.Encode(buffer);
            Max.Encode(buffer);
        }

        public bool IsWithin(Vector3D v)
        {
            if (v >= this.Min &&
                v <= this.Max)
            {
                return true;
            }
            return false;
        }

        public bool Intersects(AABB other)
        {
            if (// Max < o.Min
                this.Max.X < other.Min.X ||
                this.Max.Y < other.Min.Y ||
                this.Max.Z < other.Min.Z ||
                // Min > o.Max
                this.Min.X > other.Max.X ||
                this.Min.Y > other.Max.Y ||
                this.Min.Z > other.Max.Z)
            {
                return false;
            }
            return true; // Intersects if above fails
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("AABB:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            Min.AsText(b, pad);
            Max.AsText(b, pad);
            b.Append(' ', --pad);
            b.AppendLine("}");
        }

        public override string ToString()
        {
            return string.Format("AABB: min:{0} max:{1}", this.Min, this.Max);
        }
    }
}
