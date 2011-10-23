///*
// * Copyright (C) 2011 mooege project
// *
// * This program is free software; you can redistribute it and/or modify
// * it under the terms of the GNU General Public License as published by
// * the Free Software Foundation; either version 2 of the License, or
// * (at your option) any later version.
// *
// * This program is distributed in the hope that it will be useful,
// * but WITHOUT ANY WARRANTY; without even the implied warranty of
// * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// * GNU General Public License for more details.
// *
// * You should have received a copy of the GNU General Public License
// * along with this program; if not, write to the Free Software
// * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
// */

//using CrystalMpq;
//using Mooege.Common.Extensions;
//using Mooege.Net.GS.Message.Fields;
//using Mooege.Common.MPQ.DataTypes;

//namespace Mooege.Common.MPQ.FileFormats
//{
//    [FileFormat(SNOGroup.Scene)]
//    public class Scene : FileFormat
//    {
//        private int x;
//        private int DEADBEEF;
//        private int snoType;
//        private int unknown1, unknown2;
//        public int SceneSNO;
//        private int unknown3, unknown4;
//        private int i0;

//        public AABB_ aabbBounds;
//        public AABB_ aabbMarkerSetBounds;
//        public NavMeshDef NavMesh;
//        public int[] MarkerSets;
//        public char[] LookLink;
//        public NavZoneDef NavZone;

//        public Scene(MpqFile file)
//        {
//            var stream = file.Open();

//            long pos = 0; // x
//            DEADBEEF = stream.ReadInt32();
//            snoType = stream.ReadInt32();
//            unknown1 = stream.ReadInt32();
//            unknown2 = stream.ReadInt32();
//            SceneSNO = stream.ReadInt32();
//            unknown3 = stream.ReadInt32();
//            unknown4 = stream.ReadInt32();
//            i0 = stream.ReadInt32();
//            aabbBounds = new AABB_(stream);
//            aabbMarkerSetBounds = new AABB_(stream);

//            //load NavMeshDef
//            NavMesh = new NavMeshDef(stream);
//            // end navmeshdef
//            var serExclusions = new SerializeData(stream);
//            stream.Position += 56;
//            var serInclusions = new SerializeData(stream);
//            stream.Position += 56;

//            //MarkerSet Time
//            var serMarkerSets = new SerializeData(stream);
//            pos = stream.Position;
//            stream.Position = serMarkerSets.Offset + 16;
//            MarkerSets = new int[serMarkerSets.Size/4];
//            for (int i = 0; i < serMarkerSets.Size/4; i++)
//            {
//                MarkerSets[i] = stream.ReadInt32();
//            }
//            stream.Position = pos + 56;

//            //TODO - parse LookLink /dark
//            LookLink = new char[64];
//            for (int i = 0; i < 64; i++)
//            {
//                LookLink[i] = (char) stream.ReadByte();
//            }

//            var sermsgTriggeredEvents = new SerializeData(stream);
//            int i1 = stream.ReadInt32();
//            stream.Position += 12;

//            //navzonedef
//            NavZone = new NavZoneDef(stream);

//            stream.Close();
//        }

       

//        public class NavMeshSquare
//        {
//            private float f0;
//            public readonly int Flags;

//            public NavMeshSquare(MpqFileStream stream)
//            {
//                f0 = stream.ReadFloat();
//                Flags = stream.ReadInt32();
//            }
//        }

//        public class NavMeshDef
//        {
//            public readonly int SquaresCountX, SquaresCoountY, i0, NavMeshSquareCount;
//            public float f0;
//            private SerializeData serNavMeshArraySquares;
//            public readonly NavMeshSquare[] NavMeshArraySquares;
//            private char[] filename;

//            public string FileName
//            {
//                get { return new string(filename); }
//            }

//            public NavMeshDef(MpqFileStream stream)
//            {
//                SquaresCountX = stream.ReadInt32();
//                SquaresCoountY = stream.ReadInt32();
//                i0 = stream.ReadInt32();
//                NavMeshSquareCount = stream.ReadInt32();
//                f0 = stream.ReadFloat();
//                serNavMeshArraySquares = new SerializeData(stream);
//                long x = stream.Position;
//                stream.Position = serNavMeshArraySquares.Offset + 16;

//                NavMeshArraySquares = new NavMeshSquare[NavMeshSquareCount];
//                for (int i = 0; i < NavMeshSquareCount; i++)
//                {
//                    NavMeshArraySquares[i] = new NavMeshSquare(stream);
//                }

//                stream.Position = x;
//                stream.Position += 12;
//                filename = new char[256];

//                for (int i = 0; i < 256; i++)
//                {
//                    filename[i] = (char) stream.ReadByte(); // fix me / dark
//                }
//            }
//        }

//        public class NavCell
//        {
//            public readonly Vector3D Min, Max;
//            public readonly short Flags, NeighbourCount;
//            public readonly int NeighborsIndex;

//            public NavCell(MpqFileStream stream)
//            {
//                Min = new Vector3D(stream.ReadFloat(), stream.ReadFloat(), stream.ReadFloat());
//                Max = new Vector3D(stream.ReadFloat(), stream.ReadFloat(), stream.ReadFloat());
//                Flags = stream.ReadInt16();
//                NeighbourCount = stream.ReadInt16();
//                NeighborsIndex = stream.ReadInt32();
//            }
//        }

//        public class NavCellLookup
//        {
//            public readonly short Flags, wCell;

//            public NavCellLookup(MpqFileStream stream)
//            {
//                Flags = stream.ReadInt16();
//                wCell = stream.ReadInt16();
//            }
//        }

//        public class NavGridSquare
//        {
//            public readonly short Flags, w1, w2;

//            public NavGridSquare(MpqFileStream stream)
//            {
//                Flags = stream.ReadInt16();
//                w1 = stream.ReadInt16();
//                w2 = stream.ReadInt16();
//            }
//        }

//        public class NavCellBorderData
//        {
//            public readonly short w0, w1;

//            public NavCellBorderData(MpqFileStream stream)
//            {
//                w0 = stream.ReadInt16();
//                w1 = stream.ReadInt16();

//            }
//        }

//        public class NavZoneDef
//        {
//            private float f0, f1;
//            private int i2, i3, i4;

//            public readonly NavCell[] NavCells;
//            public readonly NavCellLookup[] NavCellNeighbours;
//            public readonly Vector2D v0;
//            public readonly NavGridSquare[] GridSquares;
//            public readonly NavCellLookup[] CellLookups;
//            public readonly NavCellBorderData[] BorderData;

//            public NavZoneDef(MpqFileStream stream)
//            {
//                long x;
//                int NavCellCount = stream.ReadInt32();
//                stream.Position += 12;

//                var serNavCells = new SerializeData(stream);
//                x = stream.Position;
//                stream.Position = serNavCells.Offset + 16;

//                //Navcells
//                NavCells = new NavCell[NavCellCount];
//                for (int i = 0; i < NavCellCount; i++)
//                {
//                    NavCells[i] = new NavCell(stream);
//                }
//                stream.Position = x;

//                //NavCellLookups
//                int NeighbourCount = stream.ReadInt32();
//                stream.Position += 12;
//                var serNavCellNeighbours = new SerializeData(stream);
//                x = stream.Position;
//                stream.Position = serNavCellNeighbours.Offset + 16;
//                NavCellNeighbours = new NavCellLookup[NeighbourCount];
//                for (int i = 0; i < NeighbourCount; i++)
//                {
//                    NavCellNeighbours[i] = new NavCellLookup(stream);
//                }
//                stream.Position = x;

//                //NavGridSquares
//                float f0 = stream.ReadFloat();
//                float f1 = stream.ReadFloat();
//                int i2 = stream.ReadInt32();
//                var v0 = new Vector2D(stream);
//                stream.Position += 12;
//                var serGridSquares = new SerializeData(stream);
//                x = stream.Position;
//                stream.Position = serGridSquares.Offset + 16;
//                GridSquares = new NavGridSquare[serGridSquares.Size/6];
//                for (int i = 0; i < serGridSquares.Size/6; i++)
//                {
//                    GridSquares[i] = new NavGridSquare(stream);
//                }
//                stream.Position = x;

//                //cell lookups
//                int i3 = stream.ReadInt32();
//                stream.Position += 12;
//                var serCellLookups = new SerializeData(stream);
//                x = stream.Position;
//                stream.Position = serCellLookups.Offset + 16;
//                CellLookups = new NavCellLookup[serCellLookups.Size/4];
//                for (int i = 0; i < serCellLookups.Size/4; i++)
//                {
//                    CellLookups[i] = new NavCellLookup(stream);
//                }
//                stream.Position = x;

//                //borderdata
//                int i4 = stream.ReadInt32();
//                stream.Position += 12;
//                var serBorderData = new SerializeData(stream);
//                x = stream.Position;
//                stream.Position = serBorderData.Offset + 16;
//                BorderData = new NavCellBorderData[serBorderData.Size/4];
//                for (int i = 0; i < serBorderData.Size/4; i++)
//                {
//                    BorderData[i] = new NavCellBorderData(stream);
//                }
//            }
//        }

//        private class SerializeData
//        {
//            public readonly int Offset; // format hex? - Darklotus
//            public readonly int Size;

//            public SerializeData(MpqFileStream stream)
//            {
//                Offset = stream.ReadInt32();
//                Size = stream.ReadInt32();
//            }
//        }

//        public class Vector2D
//        {
//            public readonly int Field0, FIeld1;

//            public Vector2D(MpqFileStream stream)
//            {
//                Field0 = stream.ReadInt32();
//                FIeld1 = stream.ReadInt32();
//            }
//        }
//    }
//}
