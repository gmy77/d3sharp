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
using Mooege.Common.MPQ;
using Mooege.Net.GS.Message;
using Mooege.Common.Storage;

namespace Mooege.Core.GS.Common.Types.Math
{
    public class Vector3D : ISerializableData
    {
        [PersistentProperty("X")]
        public float X { get; set; }
        [PersistentProperty("Y")]
        public float Y { get; set; }
        [PersistentProperty("Z")]
        public float Z { get; set; }

        public Vector3D()
        {
            this.X = 0;
            this.Y = 0;
            this.Z = 0;
        }

        public Vector3D(Vector3D vector)
        {
            this.X = vector.X;
            this.Y = vector.Y;
            this.Z = vector.Z;
        }

        public Vector3D(float x, float y, float z)
        {
            Set(x, y, z);
        }

        /// <summary>
        /// Reads Vector3D from given MPQFileStream.
        /// </summary>
        /// <param name="stream">The MPQFileStream to read from.</param>
        public Vector3D(MpqFileStream stream)
            : this(stream.ReadValueF32(), stream.ReadValueF32(), stream.ReadValueF32())
        {
        }

        public void Read(MpqFileStream stream)
        {
            X = stream.ReadValueF32();
            Y = stream.ReadValueF32();
            Z = stream.ReadValueF32();
        }

        /// <summary>
        /// Parses Vector3D from given GameBitBuffer.
        /// </summary>
        /// <param name="buffer">The GameBitBuffer to parse from.</param>
        public void Parse(GameBitBuffer buffer)
        {
            X = buffer.ReadFloat32();
            Y = buffer.ReadFloat32();
            Z = buffer.ReadFloat32();
        }

        /// <summary>
        /// Encodes Vector3D to given GameBitBuffer.
        /// </summary>        
        /// <param name="buffer">The GameBitBuffer to write.</param>
        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteFloat32(X);
            buffer.WriteFloat32(Y);
            buffer.WriteFloat32(Z);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("Vector3D:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad);
            b.AppendLine("X: " + X.ToString("G"));
            b.Append(' ', pad);
            b.AppendLine("Y: " + Y.ToString("G"));
            b.Append(' ', pad);
            b.AppendLine("Z: " + Z.ToString("G"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }

        public void Set(float x, float y, float z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        /// <summary>
        /// Calculates the distance squared from this vector to another.
        /// </summary>
        /// <param name="point">the second <see cref="Vector3" /></param>
        /// <returns>the distance squared between the vectors</returns>
        public float DistanceSquared(ref Vector3D point)
        {
            float x = point.X - X;
            float y = point.Y - Y;
            float z = point.Z - Z;

            return ((x * x) + (y * y)) + (z * z);
        }

        public static bool operator ==(Vector3D a, Vector3D b)
        {
            if (object.ReferenceEquals(null, a))
                return object.ReferenceEquals(null, b);
            return a.Equals(b);
        }

        public static bool operator !=(Vector3D a, Vector3D b)
        {
            return !(a == b);
        }

        public static bool operator >(Vector3D a, Vector3D b)
        {
            if (object.ReferenceEquals(null, a))
                return !object.ReferenceEquals(null, b);
            return a.X > b.X
                && a.Y > b.Y
                && a.Z > b.Z;
        }

        public static Vector3D operator +(Vector3D a, Vector3D b)
        {
            return new Vector3D(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static Vector3D operator -(Vector3D a, Vector3D b)
        {
            return new Vector3D(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public static bool operator <(Vector3D a, Vector3D b)
        {
            return !(a > b);
        }

        public static bool operator >=(Vector3D a, Vector3D b)
        {
            if (object.ReferenceEquals(null, a))
                return object.ReferenceEquals(null, b);
            return a.X >= b.X
                && a.Y >= b.Y
                && a.Z >= b.Z;
        }

        public static bool operator <=(Vector3D a, Vector3D b)
        {
            if (object.ReferenceEquals(null, a))
                return object.ReferenceEquals(null, b);
            return a.X <= b.X
                && a.Y <= b.Y
                && a.Z <= b.Z;
        }

        public override bool Equals(object o)
        {
            if (object.ReferenceEquals(this, o))
                return true;
            var v = o as Vector3D;
            if (v != null)
            {
                return this.X == v.X
                    && this.Y == v.Y
                    && this.Z == v.Z;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("x:{0} y:{1} z:{2}", X, Y, Z);
        }
    }
}
