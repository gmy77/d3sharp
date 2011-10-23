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

using System.Collections.Generic;
using System.Diagnostics;
using CrystalMpq;
using Mooege.Common.Extensions;

namespace Mooege.Common.MPQ.FileFormats
{
    [FileFormat(SNOGroup.Worlds)]
    public class World : FileFormat
    {
        public Header Header { get; private set; }
        public List<DRLGParams> DRLGParams = new List<DRLGParams>();
        public List<SceneParams> SceneParams = new List<SceneParams>();
        public List<int> MarkerSets = new List<int>();
        public Environment Environment { get; private set; }
        public LabelRuleSet LabelRuleSet { get; private set; }        
        public SceneClusterSet SceneClusterSet { get; private set; }
        public int[] SNONavMeshFunctions = new int[4];

        public int Int0 { get; private set; }
        public float Float0;
        public int Int1;
        public int SNOScript;
        public int Int2;

        public World(MpqFile file)
        {
            var stream = file.Open();

            this.Header = new Header(stream); // the asset header.

            var drlgParamsPointer = stream.GetSerializedDataPointer();
            this.DRLGParams = stream.ReadSerializedData<DRLGParams>(drlgParamsPointer, drlgParamsPointer.Size / 120);

            stream.Position += (3*4);
            var sceneParamsPointer = stream.GetSerializedDataPointer();
            this.SceneParams = stream.ReadSerializedData<SceneParams>(sceneParamsPointer, sceneParamsPointer.Size / 24);

            if (Header.SNOId == 71150)
            {
                foreach (var chunk in SceneParams[0].SceneChunks)
                {
                    Debug.WriteLine(chunk.SNOName.Name + "=>\t" + chunk.Position.V.X + ":" + chunk.Position.V.Y);
                }
            }

            stream.Position += (2*4);
            this.MarkerSets = stream.ReadSerializedInts();

            stream.Position += (14*4);
            this.Environment = new Environment(stream);

            LabelRuleSet = new LabelRuleSet(stream);
            this.Int0 = stream.ReadInt32();

            stream.Position += 4;
            this.SceneClusterSet = new SceneClusterSet(stream);

            for (int i = 0; i < SNONavMeshFunctions.Length; i++)
            {
                SNONavMeshFunctions[i] = stream.ReadInt32();
            }

            stream.Position += 4;
            Float0 = stream.ReadFloat();
            Int1 = stream.ReadInt32();
            SNOScript = stream.ReadInt32();
            Int2 = stream.ReadInt32();

            stream.Close();
        }
    }
}