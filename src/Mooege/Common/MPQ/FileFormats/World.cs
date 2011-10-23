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
using Mooege.Common.MPQ.DataTypes;

namespace Mooege.Common.MPQ.FileFormats
{
    [FileFormat(SNOGroup.Worlds)]
    public class World : FileFormat
    {
        public Header Header;
        public int SNO;
        SerializeData serDRLGParams;
        public DRLGParams[] Param;
        SerializeData serSceneParams;
        public SceneParams Scene;
        SerializeData serMarkerSets;
        public int[] MarkerSets;

        public Environment Environment;
        public LabelRuleSet LabelRuleSet;
        public int i0;
        public SceneClusterSet SceneClusterSet5;
        public int[] arNavMeshFuncs;
        public float f0;
        public int i1;
        public int snoScript;
        public int i2;

        public World(MpqFile file)
        {
            var stream = file.Open();
            Header = new Header(stream);
            SNO = stream.ReadInt32();
            serDRLGParams = new SerializeData(stream);
            long x = stream.Position;
            if (serDRLGParams.Size > 0)
            {
                Param = new DRLGParams[serDRLGParams.Size / 120];
                stream.Position = serDRLGParams.Offset + 16;
                for (int i = 0; i < serDRLGParams.Size/120; i++)
                {
                    Param[i] = new DRLGParams(stream);
                }
            }
            stream.Position = x;
            stream.Position += (5 * 4); // This was 3 ints padding in the struct, was 8 bytes behind though so i changed it - DarkLotus

            serSceneParams = new SerializeData(stream);
            x = stream.Position;
            if (serSceneParams.Size > 0)
            {
                stream.Position = serSceneParams.Offset + 16;
                Scene = new SceneParams(stream); ;
                

            }
            stream.Position = x;
            stream.Position += (2 * 4);
            serMarkerSets = new SerializeData(stream);
            x = stream.Position;
            if (serMarkerSets.Size > 0)
            {
                MarkerSets = new int[serMarkerSets.Size / 4];
                stream.Position = serMarkerSets.Offset + 16;
                for (int i = 0; i < serMarkerSets.Size / 4; i++)
                {
                    MarkerSets[i] = stream.ReadInt32();
                }
            }
            stream.Position = x;

            stream.Position += (14 * 4);

            Environment = new DataTypes.Environment(stream);

            stream.Position += 4;
            LabelRuleSet = new DataTypes.LabelRuleSet(stream);
            i0 = stream.ReadInt32();
            stream.Position += 4;
            SceneClusterSet5 = new SceneClusterSet(stream);
            arNavMeshFuncs = new int[4];
            for (int i = 0; i < arNavMeshFuncs.Length; i++)
            {
                arNavMeshFuncs[i] = stream.ReadInt32();
            }
            stream.Position += 4;
            f0 = stream.ReadFloat();
            i1 = stream.ReadInt32();
            snoScript = stream.ReadInt32();
            i2 = stream.ReadInt32();
         
            stream.Close();
        }

      
    }
}