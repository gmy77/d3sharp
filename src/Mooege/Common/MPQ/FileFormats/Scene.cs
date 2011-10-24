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
using System.Text;
using CrystalMpq;
using Mooege.Common.Extensions;
using Mooege.Net.GS.Message.Fields;

namespace Mooege.Common.MPQ.FileFormats
{
    [FileFormat(SNOGroup.Scene)]
    public class Scene : FileFormat
    {
        public Header Header { get; private set; }
        public int Int0;
        public AABB AABBBounds { get; private set; }
        public AABB AABBMarketSetBounds { get; private set; }
        public NavMeshDef NavMesh { get; private set; }
        public List<int> MarkerSets = new List<int>();
        public string LookLink { get; private set; }
        public MsgTriggeredEvent MsgTriggeredEvent { get; private set; }
        public int Int1;
        public NavZoneDef NavZone { get; private set; }

        public Scene(MpqFile file)
        {
            var stream = file.Open();
            this.Header = new Header(stream);

            Int0 = stream.ReadInt32();
            this.AABBBounds = new AABB(stream);
            this.AABBMarketSetBounds = new AABB(stream);

            this.NavMesh = new NavMeshDef(stream); //load NavMeshDef
            var exclusions = stream.GetSerializedDataPointer();

            stream.Position += (14 * 4);
            var inclusions = stream.GetSerializedDataPointer();

            stream.Position += (14 * 4);
            this.MarkerSets = stream.ReadSerializedInts();

            stream.Position += (14 * 4);
            var buf = new byte[64];
            stream.Read(buf, 0, 64);
            this.LookLink = Encoding.ASCII.GetString(buf);

            // Maybe this is a list/array - DarkLotus
            this.MsgTriggeredEvent = stream.ReadSerializedItem<MsgTriggeredEvent>();
            this.Int1 = stream.ReadInt32();

            stream.Position += (3 * 4);
            this.NavZone = new NavZoneDef(stream);

            stream.Close();
        }
        
        public class NavMeshDef
        {
            public int SquaresCountX;
            public int SquaresCountY;
            public int Int0;
            public int NavMeshSquareCount;
            public float Float0;
            public List<NavMeshSquare> Squares = new List<NavMeshSquare>();
            public string Filename;

            public NavMeshDef(MpqFileStream stream)
            {
                this.SquaresCountX = stream.ReadInt32();
                this.SquaresCountY = stream.ReadInt32();
                this.Int0 = stream.ReadInt32();
                this.NavMeshSquareCount = stream.ReadInt32();
                this.Float0 = stream.ReadFloat();
                this.Squares = stream.ReadSerializedData<NavMeshSquare>(this.NavMeshSquareCount);

                stream.Position += (3 * 4);
                var buf = new byte[256];
                stream.Read(buf, 0, 256);
                this.Filename = Encoding.ASCII.GetString(buf);
            }
        }

        public class NavZoneDef
        {
            public int NavCellCount;
            public List<NavCell> NavCells = new List<NavCell>();
            public int NeightbourCount;
            public List<NavCellLookup> NavCellNeighbours = new List<NavCellLookup>();
            public float Float0;
            public float Float1;
            public int Int2;
            public readonly Vector2D V0;
            public List<NavGridSquare> GridSquares = new List<NavGridSquare>();
            public int Int3;
            public List<NavCellLookup> CellLookups = new List<NavCellLookup>();
            public int Int4;
            public List<NavCellBorderData> BorderData = new List<NavCellBorderData>();

            public NavZoneDef(MpqFileStream stream)
            {
                this.NavCellCount = stream.ReadInt32();

                stream.Position += (3 * 4);
                this.NavCells = stream.ReadSerializedData<NavCell>(this.NavCellCount);

                this.NeightbourCount = stream.ReadInt32();
                stream.Position += (3 * 4);
                this.NavCellNeighbours = stream.ReadSerializedData<NavCellLookup>(this.NeightbourCount);

                this.Float0 = stream.ReadFloat();
                this.Float1 = stream.ReadFloat();
                this.Int2 = stream.ReadInt32();
                this.V0 = new Vector2D(stream);

                stream.Position += (3 * 4);
                var pointerGridSquares = stream.GetSerializedDataPointer();
                this.GridSquares = stream.ReadSerializedData<NavGridSquare>(pointerGridSquares, pointerGridSquares.Size / 6);

                this.Int3 = stream.ReadInt32();
                stream.Position += (3 * 4);
                var pointerCellLookups = stream.GetSerializedDataPointer();
                this.CellLookups = stream.ReadSerializedData<NavCellLookup>(pointerCellLookups, pointerCellLookups.Size / 4);

                this.Int4 = stream.ReadInt32();
                stream.Position += (3 * 4);
                var pointerBorderData = stream.GetSerializedDataPointer();
                this.BorderData = stream.ReadSerializedData<NavCellBorderData>(pointerBorderData, pointerBorderData.Size / 4);
            }
        }

        public class NavMeshSquare : ISerializableData
        {
            public float Float0;
            public int Flags;

            public void Read(MpqFileStream stream)
            {
                this.Float0 = stream.ReadFloat();
                this.Flags = stream.ReadInt32();
            }
        }

        public class NavCell : ISerializableData
        {
            public Vector3D Min, Max;
            public short Flags, NeighbourCount;
            public int NeighborsIndex;

            public void Read(MpqFileStream stream)
            {
                this.Min = new Vector3D(stream.ReadFloat(), stream.ReadFloat(), stream.ReadFloat());
                this.Max = new Vector3D(stream.ReadFloat(), stream.ReadFloat(), stream.ReadFloat());
                this.Flags = stream.ReadInt16();
                this.NeighbourCount = stream.ReadInt16();
                this.NeighborsIndex = stream.ReadInt32();
            }
        }

        public class NavCellLookup : ISerializableData
        {
            public short Flags, wCell;

            public void Read(MpqFileStream stream)
            {
                this.Flags = stream.ReadInt16();
                this.wCell = stream.ReadInt16();
            }
        }

        public class NavGridSquare : ISerializableData
        {
            public short Flags, W1, W2;

            public void Read(MpqFileStream stream)
            {
                Flags = stream.ReadInt16();
                W1 = stream.ReadInt16();
                W2 = stream.ReadInt16();
            }
        }

        public class NavCellBorderData : ISerializableData
        {
            public short W0, W1;

            public void Read(MpqFileStream stream)
            {
                W0 = stream.ReadInt16();
                W1 = stream.ReadInt16();
            }
        }
    }
}
