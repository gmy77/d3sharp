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
    public class PRTransform
    {
        public Quaternion Rotation;
        public Vector3D ReferencePoint;

        public void Parse(GameBitBuffer buffer)
        {
            Rotation = new Quaternion();
            Rotation.Parse(buffer);
            ReferencePoint = new Vector3D();
            ReferencePoint.Parse(buffer);
        }

        public void Encode(GameBitBuffer buffer)
        {
            Rotation.Encode(buffer);
            ReferencePoint.Encode(buffer);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("PRTransform:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            Rotation.AsText(b, pad);
            ReferencePoint.AsText(b, pad);
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}
