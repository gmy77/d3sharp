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
using CrystalMpq;
using Gibbed.IO;
using Mooege.Common.Extensions;
using Mooege.Common.MPQ.FileFormats.Types;

namespace Mooege.Common.MPQ.FileFormats
{
    [FileFormat(SNOGroup.LevelArea)]
    public class LevelArea : FileFormat
    {
        public Header header;
        public int i0;
        public int i1;
        public int snoLevelAreaOverrideForGizmoLocs;
        public GizmoLocSet LocSet;
        public int i2;
        public List<LevelAreaSpawnPopulation> SpawnPopulation;

        public LevelArea(MpqFile file)
        {
            var stream = file.Open();
            this.header = new Header(stream);
            this.i0 = stream.ReadValueS32();
            this.i1 = stream.ReadValueS32();
            this.snoLevelAreaOverrideForGizmoLocs = stream.ReadValueS32();
            this.LocSet = new GizmoLocSet(stream);
            this.i2 = stream.ReadValueS32();
            stream.Position += 12;
            this.SpawnPopulation = stream.ReadSerializedData<LevelAreaSpawnPopulation>();
            stream.Close();
        }
    }

    public class GizmoLocSet
    {
        public GizmoLocSpawnType[] SpawnType;

        public GizmoLocSet(MpqFileStream stream)
        {
            this.SpawnType = new GizmoLocSpawnType[26];
            for (int i = 0; i < 26; i++)
                this.SpawnType[i] = new GizmoLocSpawnType(stream);
        }
    }

    public class GizmoLocSpawnType
    {
        public List<GizmoLocSpawnEntry> SpawnEntry;
        public string s0;
        public string s1;

        public GizmoLocSpawnType(MpqFileStream stream)
        {
            stream.Position += 8;
            this.SpawnEntry = stream.ReadSerializedData<GizmoLocSpawnEntry>();
            this.s0 = stream.ReadString(80, true);
            this.s1 = stream.ReadString(256, true);
        }
    }

    public class GizmoLocSpawnEntry : ISerializableData
    {
        public int i0;
        public int i1;
        public SNOName snoName;
        public int i2;

        public void Read(MpqFileStream stream)
        {
            this.i0 = stream.ReadValueS32();
            this.i1 = stream.ReadValueS32();
            this.snoName = new SNOName(stream);
            this.i2 = stream.ReadValueS32();
        }
    }

    public class LevelAreaSpawnPopulation : ISerializableData
    {
        public string s0;
        public int i0;
        public int[] i1;
        public int i2;
        public List<LevelAreaSpawnGroup> SpawnGroup;

        public void Read(MpqFileStream stream)
        {
            this.s0 = stream.ReadString(64, true);
            this.i0 = stream.ReadValueS32();
            this.i1 = new int[4];
            for (int i = 0; i < 4; i++)
                this.i1[i] = stream.ReadValueS32();
            this.i2 = stream.ReadValueS32();
            stream.Position += 8;
            this.SpawnGroup = stream.ReadSerializedData<LevelAreaSpawnGroup>();
        }
    }

    public class LevelAreaSpawnGroup : ISerializableData
    {
        public SpawnGroupType GroupType;
        public float f0;
        public float f1;
        public int i0;
        public int i1;
        public List<LevelAreaSpawnItem> SpawnItems;
        public int i2;
        public int i3;

        public void Read(MpqFileStream stream)
        {
            this.GroupType = (SpawnGroupType)stream.ReadValueS32();
            this.f0 = stream.ReadValueF32();
            this.f1 = stream.ReadValueF32();
            this.i0 = stream.ReadValueS32();
            this.i1 = stream.ReadValueS32();
            stream.Position += 12;
            this.SpawnItems = stream.ReadSerializedData<LevelAreaSpawnItem>();
            this.i2 = stream.ReadValueS32();
            this.i3 = stream.ReadValueS32();
        }
    }

    public enum SpawnGroupType : int
    {
        Density = 0,
        Exactly = 1,
    }

    public class LevelAreaSpawnItem : ISerializableData
    {
        public SNOName snoName;
        public SpawnType spawnType;
        public int i0;
        public int i1;
        public int i2;
        public int i3;

        public void Read(MpqFileStream stream)
        {
            this.snoName = new SNOName(stream);
            this.spawnType = (SpawnType)stream.ReadValueS32();
            this.i0 = stream.ReadValueS32();
            this.i1 = stream.ReadValueS32();
            this.i2 = stream.ReadValueS32();
            this.i3 = stream.ReadValueS32();
        }
    }

    public enum SpawnType : int
    {
        Normal = 0,
        Champion,
        Rare,
        Minion,
        Unique,
        Hireling,
        Clone,
    }
}
