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

namespace Mooege.Common.MPQ.FileFormats
{
    [FileFormat(SNOGroup.MarkerSet)]
    public class MarkerSet : FileFormat 
    {
        public MpqDataTypes.Header Header;
        public int SNO;
        private int unknown0,unknown1;

        MpqDataTypes.SerializeData serMarkers;
        MpqDataTypes.Marker[] Markers;
        MpqDataTypes.SerializeData serNoSpawns;
        MpqDataTypes.AABB aabb;
        int i0;
        string filename;
        int nLabel;
        int nSpecialIndexCount;
        MpqDataTypes.SerializeData serSpecialIndexList;


        public MarkerSet(MpqFile file)
        {
            var stream = file.Open();
            Header = new MpqDataTypes.Header(stream);
            SNO = stream.ReadInt32();
            unknown0 = stream.ReadInt32();
            unknown1 = stream.ReadInt32();
            serMarkers = new MpqDataTypes.SerializeData(stream);
            long x = stream.Position;
            Markers = new MpqDataTypes.Marker[serMarkers.Size / 208];
            stream.Position = serMarkers.Offset + 16;
            for (int i = 0; i < serMarkers.Size / 208; i++)
            {
                Markers[i] = new MpqDataTypes.Marker(stream);
            }
            stream.Position = x;
            stream.Position += (15 * 4); // pad 15
            serNoSpawns = new MpqDataTypes.SerializeData(stream);
            stream.Position += (14 * 4);
            aabb = new MpqDataTypes.AABB(stream);
            i0 = stream.ReadInt32();
            byte[] buf = new byte[256];
            stream.Read(buf, 0, 256); filename = Encoding.ASCII.GetString(buf);
            nLabel = stream.ReadInt32();
            nSpecialIndexCount = stream.ReadInt32();
            serSpecialIndexList = new MpqDataTypes.SerializeData(stream);

                stream.Close();
        }

      
    }
}