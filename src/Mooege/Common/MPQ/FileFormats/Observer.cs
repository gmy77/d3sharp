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

using CrystalMpq;
using Gibbed.IO;
using Mooege.Common.MPQ.FileFormats.Types;
using Mooege.Net.GS.Message.Fields;

namespace Mooege.Common.MPQ.FileFormats
{
    [FileFormat(SNOGroup.Observer)]
    public class Observer : FileFormat
    {
        public Header Header;
        public int i0;
        public float angle0;
        public float f0;
        public float f1;
        public float velocity;
        public float angle1;
        public float angle2;
        public float f2;
        public Vector3D v0;
        public Vector3D v1;
        public float f3;
        public float f4;
        public float f5;
        public float f6;

        public Observer(MpqFile file)
        {
            var stream = file.Open();
            this.Header = new Header(stream);
            this.i0 = stream.ReadValueS32();
            this.angle0 = stream.ReadValueF32();
            this.f0 = stream.ReadValueF32();
            this.f1 = stream.ReadValueF32();
            this.velocity = stream.ReadValueF32();
            this.angle1 = stream.ReadValueF32();
            this.angle2 = stream.ReadValueF32();
            this.f2 = stream.ReadValueF32();
            this.v0 = new Vector3D(stream.ReadValueF32(), stream.ReadValueF32(), stream.ReadValueF32());
            this.v1 = new Vector3D(stream.ReadValueF32(), stream.ReadValueF32(), stream.ReadValueF32());
            this.f3 = stream.ReadValueF32();
            this.f4 = stream.ReadValueF32();
            this.f5 = stream.ReadValueF32();
            this.f6 = stream.ReadValueF32();
            stream.Close();
        }
    }
}