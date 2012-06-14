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
using Mooege.Common.Storage;

namespace Mooege.Core.GS.Common.Types.Math
{
    public class PRTransform
    {
        [PersistentProperty("Quaternion")]
        public Quaternion Quaternion { get; set; }
        [PersistentProperty("Vector3D")]
        public Vector3D Vector3D { get; set; }

        public PRTransform() { }

        /// <summary>
        /// Reads PRTransform from given MPQFileStream.
        /// </summary>
        /// <param name="stream">The MPQFileStream to read from.</param>
        public PRTransform(MpqFileStream stream)
        {
            Quaternion = new Quaternion(stream);
            Vector3D = new Vector3D(stream);
        }

        /// <summary>
        /// Reads PRTransform from given GameBitBuffer.
        /// </summary>
        /// <param name="buffer">The GameBitBuffer to parse from.</param>
        public void Parse(GameBitBuffer buffer)
        {
            Quaternion = new Quaternion();
            Quaternion.Parse(buffer);
            Vector3D = new Vector3D();
            Vector3D.Parse(buffer);
        }

        /// <summary>
        /// Encodes PRTransform to given GameBitBuffer.
        /// </summary>
        /// <param name="buffer">The GameBitBuffer to write.</param>
        public void Encode(GameBitBuffer buffer)
        {
            Quaternion.Encode(buffer);
            Vector3D.Encode(buffer);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("PRTransform:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            Quaternion.AsText(b, pad);
            Vector3D.AsText(b, pad);
            b.Append(' ', --pad);
            b.AppendLine("}");
        }
    }
}
