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
        public DRLGParams DRLGParams = new DRLGParams();
        public SceneParams SceneParams = new SceneParams();
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

            this.DRLGParams = stream.ReadSerializedData<DRLGParams>(); // I'm not sure if we can have a list of drlgparams. (then should be calling it with pointer.Size/120) /raist

            stream.Position += (3*4);
            this.SceneParams = stream.ReadSerializedData<SceneParams>(); // I'm not sure if we can have a list of drlgparams. (then should be calling it with pointer.Size/24) /raist

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