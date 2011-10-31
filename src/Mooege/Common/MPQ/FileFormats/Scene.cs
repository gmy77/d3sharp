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
using Mooege.Common.MPQ.FileFormats.Types;
using Mooege.Core.GS.Common.Types.Collision;
using Mooege.Core.GS.Common.Types.Math;
using Mooege.Core.GS.Common.Types.SNO;

namespace Mooege.Common.MPQ.FileFormats
{
    [FileFormat(SNOGroup.Scene)]
    public class Scene : FileFormat
    {
        public Header Header { get; private set; }
        public int Int0 { get; private set; }
        public AABB AABBBounds { get; private set; }
        public AABB AABBMarketSetBounds { get; private set; }
        public NavMeshDef NavMesh { get; private set; }
        public List<int> MarkerSets = new List<int>();
        public string LookLink { get; private set; }
        public MsgTriggeredEvent MsgTriggeredEvent { get; private set; }
        public int Int1 { get; private set; }
        public NavZoneDef NavZone { get; private set; }

        public Scene(MpqFile file)
        {
            var stream = file.Open();
            this.Header = new Header(stream);

            Int0 = stream.ReadValueS32();
            this.AABBBounds = new AABB(stream);
            this.AABBMarketSetBounds = new AABB(stream);

            this.NavMesh = new NavMeshDef(stream); //load NavMeshDef
            var exclusions = stream.GetSerializedDataPointer();

            stream.Position += (14 * 4);
            var inclusions = stream.GetSerializedDataPointer();

            stream.Position += (14 * 4);
            this.MarkerSets = stream.ReadSerializedInts();

            stream.Position += (14 * 4);
            this.LookLink = stream.ReadString(64, true);

            // Maybe this is a list/array - DarkLotus
            this.MsgTriggeredEvent = stream.ReadSerializedItem<MsgTriggeredEvent>();
            this.Int1 = stream.ReadValueS32();

            stream.Position += (3 * 4);
            this.NavZone = new NavZoneDef(stream);

            stream.Close();
        }
        
        public class NavMeshDef
        {
            public int SquaresCountX { get; private set; }
            public int SquaresCountY { get; private set; }
            public int Int0 { get; private set; }
            public int NavMeshSquareCount { get; private set; }
            public float Float0 { get; private set; }
            public List<NavMeshSquare> Squares = new List<NavMeshSquare>();
            public string Filename { get; private set; }

            public NavMeshDef(MpqFileStream stream)
            {
                this.SquaresCountX = stream.ReadValueS32();
                this.SquaresCountY = stream.ReadValueS32();
                this.Int0 = stream.ReadValueS32();
                this.NavMeshSquareCount = stream.ReadValueS32();
                this.Float0 = stream.ReadValueF32();
                this.Squares = stream.ReadSerializedData<NavMeshSquare>(this.NavMeshSquareCount);

                stream.Position += (3 * 4);
                this.Filename = stream.ReadString(256, true);
            }
        }

        public class NavZoneDef
        {
            public int NavCellCount { get; private set; }
            public List<NavCell> NavCells = new List<NavCell>();
            public int NeightbourCount { get; private set; }
            public List<NavCellLookup> NavCellNeighbours = new List<NavCellLookup>();
            public float Float0 { get; private set; }
            public float Float1 { get; private set; }
            public int Int2 { get; private set; }
            public Vector2D V0 { get; private set; }
            public List<NavGridSquare> GridSquares = new List<NavGridSquare>();
            public int Int3 { get; private set; }
            public List<NavCellLookup> CellLookups = new List<NavCellLookup>();
            public int Int4 { get; private set; }
            public List<NavCellBorderData> BorderData = new List<NavCellBorderData>();

            public NavZoneDef(MpqFileStream stream)
            {
                this.NavCellCount = stream.ReadValueS32();

                stream.Position += (3 * 4);
                this.NavCells = stream.ReadSerializedData<NavCell>(this.NavCellCount);

                this.NeightbourCount = stream.ReadValueS32();
                stream.Position += (3 * 4);
                this.NavCellNeighbours = stream.ReadSerializedData<NavCellLookup>(this.NeightbourCount);

                this.Float0 = stream.ReadValueF32();
                this.Float1 = stream.ReadValueF32();
                this.Int2 = stream.ReadValueS32();
                this.V0 = new Vector2D(stream);

                stream.Position += (3 * 4);
                var pointerGridSquares = stream.GetSerializedDataPointer();
                this.GridSquares = stream.ReadSerializedData<NavGridSquare>(pointerGridSquares, pointerGridSquares.Size / 6);

                this.Int3 = stream.ReadValueS32();
                stream.Position += (3 * 4);
                var pointerCellLookups = stream.GetSerializedDataPointer();
                this.CellLookups = stream.ReadSerializedData<NavCellLookup>(pointerCellLookups, pointerCellLookups.Size / 4);

                this.Int4 = stream.ReadValueS32();
                stream.Position += (3 * 4);
                var pointerBorderData = stream.GetSerializedDataPointer();
                this.BorderData = stream.ReadSerializedData<NavCellBorderData>(pointerBorderData, pointerBorderData.Size / 4);
            }
        }

        public class NavMeshSquare : ISerializableData
        {
            public float Float0 { get; private set; }
            public int Flags { get; private set; }

            public void Read(MpqFileStream stream)
            {
                this.Float0 = stream.ReadValueF32();
                this.Flags = stream.ReadValueS32();
            }
        }

        public class NavCell : ISerializableData
        {
            public Vector3D Min { get; private set; }
            public Vector3D Max { get; private set; }
            public short Flags { get; private set; }
            public short NeighbourCount { get; private set; }
            public int NeighborsIndex { get; private set; }

            public void Read(MpqFileStream stream)
            {
                this.Min = new Vector3D(stream.ReadValueF32(), stream.ReadValueF32(), stream.ReadValueF32());
                this.Max = new Vector3D(stream.ReadValueF32(), stream.ReadValueF32(), stream.ReadValueF32());
                this.Flags = stream.ReadValueS16();
                this.NeighbourCount = stream.ReadValueS16();
                this.NeighborsIndex = stream.ReadValueS32();
            }
        }

        public class NavCellLookup : ISerializableData
        {
            public short Flags { get; private set; }
            public short WCell { get; private set; }

            public void Read(MpqFileStream stream)
            {
                this.Flags = stream.ReadValueS16();
                this.WCell = stream.ReadValueS16();
            }
        }

        public class NavGridSquare : ISerializableData
        {
            public short Flags { get; private set; }
            public short W1 { get; private set; }
            public short W2 { get; private set; }

            public void Read(MpqFileStream stream)
            {
                Flags = stream.ReadValueS16();
                W1 = stream.ReadValueS16();
                W2 = stream.ReadValueS16();
            }
        }

        public class NavCellBorderData : ISerializableData
        {
            public short W0 { get; private set; }
            public short W1 { get; private set; }

            public void Read(MpqFileStream stream)
            {
                W0 = stream.ReadValueS16();
                W1 = stream.ReadValueS16();
            }
        }
    }
}
