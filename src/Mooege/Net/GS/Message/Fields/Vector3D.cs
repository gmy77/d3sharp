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

namespace Mooege.Net.GS.Message.Fields
{
    public class Vector3D
    {
        public float X;
        public float Y;
        public float Z;

        public Vector3D(){}

        public Vector3D(float x, float y, float z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public void Parse(GameBitBuffer buffer)
        {
            X = buffer.ReadFloat32();
            Y = buffer.ReadFloat32();
            Z = buffer.ReadFloat32();
        }

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
            b.AppendLine("Field0: " + X.ToString("G"));
            b.Append(' ', pad);
            b.AppendLine("Field1: " + Y.ToString("G"));
            b.Append(' ', pad);
            b.AppendLine("Field2: " + Z.ToString("G"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }

        public override string ToString()
        {
            return string.Format("x:{0} y:{1} z:{2}",X, Y,Z);
        }
    }
}