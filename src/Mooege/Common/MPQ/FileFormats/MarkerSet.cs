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

using System.IO;
using CrystalMpq;
using Mooege.Common.Extensions;
using System.Text;
using Mooege.Common.MPQ;
using System.Collections.Generic;

namespace Mooege.Common.MPQ.FileFormats
{
    [FileFormat(SNOGroup.MarkerSet)]
    public class MarkerSet : FileFormat 
    {
        public Header Header;
        List<Marker> Markers = new List<Marker>();
        public List<int> Spawns = new List<int>();

        AABB aabb;
        int i0;
        string filename;
        int nLabel;
        int nSpecialIndexCount;
        public List<int> SpecialIndexList = new List<int>();


        public MarkerSet(MpqFile file)
        {
            var stream = file.Open();
            Header = new Header(stream);

            stream.ReadInt32();
            int size = stream.ReadInt32(); // hacky Didnt know how to get the size otherwise. - DarkLotus
            stream.Position += -8; 
            this.Markers = stream.ReadSerializedData<Marker>(size/208);         
            stream.Position += (15 * 4); // pad 15
            this.Spawns = stream.ReadSerializedInts();
           
            stream.Position += (14 * 4);
            aabb = new AABB(stream);
            i0 = stream.ReadInt32();
            byte[] buf = new byte[256];
            stream.Read(buf, 0, 256); filename = Encoding.ASCII.GetString(buf);
            nLabel = stream.ReadInt32();
            nSpecialIndexCount = stream.ReadInt32();
            // Only the SerializeData is read in the 010 template, not the actual payload.
            this.SpecialIndexList = stream.ReadSerializedInts();
                stream.Close();
        }

      
   }
}