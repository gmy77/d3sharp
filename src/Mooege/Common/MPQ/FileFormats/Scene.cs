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
using Mooege.Common.Extensions;
using Mooege.Net.GS.Message.Fields;
using Mooege.Common.MPQ;
using System.Collections.Generic;
using System.Text;

namespace Mooege.Common.MPQ.FileFormats
{
    [FileFormat(SNOGroup.Scene)]
    public class Scene : FileFormat
    {
        public Header Header;
        private int i0;

        public AABB aabbBounds;
        public AABB aabbMarkerSetBounds;
        public NavMeshDef NavMesh;

        public List<int> Exclusions = new List<int>();

        public List<int> Inclusions = new List<int>();

        public List<int> MarkerSets = new List<int>();
        public LookLink LookLink;
        int i1;
        public MsgTriggeredEvent MsgTriggeredEvent;
        public NavZoneDef NavZone;

        public Scene(MpqFile file)
        {
            var stream = file.Open();
            Header = new MPQ.Header(stream);
            i0 = stream.ReadInt32();
            aabbBounds = new AABB(stream);
            aabbMarkerSetBounds = new AABB(stream);

            NavMesh = new NavMeshDef(stream);
            Exclusions = stream.ReadSerializedInts();
            stream.Position += (14 * 4);
            Inclusions = stream.ReadSerializedInts();
            stream.Position += (14 * 4);
            MarkerSets = stream.ReadSerializedInts();
            stream.Position += (14 * 4);
            LookLink = new LookLink(stream);

            // Maybe this is a list/array - DarkLotus
            MsgTriggeredEvent = stream.ReadSerializedData<MsgTriggeredEvent>();
            int i1 = stream.ReadInt32();
            stream.Position += (3*4);

            NavZone = new NavZoneDef(stream);

            stream.Close();
        }



        public class NavMeshSquare : ISerializableData
        {
            private float f0;
            public int Flags;

            public void Read(MpqFileStream stream)
            {
                f0 = stream.ReadFloat();
                Flags = stream.ReadInt32();
            }
        }

        public class NavMeshDef
        {
            public readonly int SquaresCountX, SquaresCoountY, i0, NavMeshSquareCount;
            public float f0;
            //private SerializeData serNavMeshArraySquares;
            public List<NavMeshSquare> NavMeshArraySquares = new List<NavMeshSquare>();
            public string FileName;
           

            public NavMeshDef(MpqFileStream stream)
            {
                SquaresCountX = stream.ReadInt32();
                SquaresCoountY = stream.ReadInt32();
                i0 = stream.ReadInt32();
                NavMeshSquareCount = stream.ReadInt32();
                f0 = stream.ReadFloat();
                NavMeshArraySquares = stream.ReadSerializedData<NavMeshSquare>(NavMeshSquareCount);
                stream.Position += (3*4);
                byte[] buf = new byte[256];
                stream.Read(buf, 0, 256); FileName = Encoding.ASCII.GetString(buf);
            }
        }

        public class NavCell : ISerializableData
        {
            public Vector3D Min, Max;
            public short Flags, NeighbourCount;
            public int NeighborsIndex;

            public void Read(MpqFileStream stream)
            {
                Min = new Vector3D(stream.ReadFloat(), stream.ReadFloat(), stream.ReadFloat());
                Max = new Vector3D(stream.ReadFloat(), stream.ReadFloat(), stream.ReadFloat());
                Flags = stream.ReadInt16();
                NeighbourCount = stream.ReadInt16();
                NeighborsIndex = stream.ReadInt32();
            }
        }

        public class NavCellLookup : ISerializableData
        {
            public short Flags, wCell;

            public void Read(MpqFileStream stream)
            {
                Flags = stream.ReadInt16();
                wCell = stream.ReadInt16();
            }
        }

        public class NavGridSquare : ISerializableData
        {
            public short Flags, w1, w2;

            public void Read(MpqFileStream stream)
            {
                Flags = stream.ReadInt16();
                w1 = stream.ReadInt16();
                w2 = stream.ReadInt16();
            }
        }

        public class NavCellBorderData : ISerializableData
        {
            public short w0, w1;

            public void Read(MpqFileStream stream)
            {
                w0 = stream.ReadInt16();
                w1 = stream.ReadInt16();

            }
        }

        public class NavZoneDef
        {
            private float f0, f1;
            private int i2, i3, i4;

            public List<NavCell> NavCells = new List<NavCell>();
            public List<NavCellLookup> NavCellNeighbours = new List<NavCellLookup>();
            public Vector2D v0;
            public List<NavGridSquare> GridSquares = new List<NavGridSquare>();
            public List<NavCellLookup> CellLookups = new List<NavCellLookup>();
            public List<NavCellBorderData> BorderData = new List<NavCellBorderData>();

            public NavZoneDef(MpqFileStream stream)
            {             
                int NavCellCount = stream.ReadInt32();
                stream.Position += (3*4);
                NavCells = stream.ReadSerializedData<NavCell>(NavCellCount);

                int NeighbourCount = stream.ReadInt32();
                stream.Position += (3*4);
                NavCellNeighbours = stream.ReadSerializedData<NavCellLookup>(NeighbourCount);

                float f0 = stream.ReadFloat();
                float f1 = stream.ReadFloat();
                int i2 = stream.ReadInt32();
                var v0 = new Vector2D(stream);
                stream.Position += (3*4);

                //Hacky need size - DarkLotus
                stream.ReadInt32();
                int size = stream.ReadInt32();
                stream.Position += -8;
                GridSquares = stream.ReadSerializedData<NavGridSquare>(size/6);                      
                int i3 = stream.ReadInt32();
                stream.Position += (3*4);
                //Hacky need size - DarkLotus
                stream.ReadInt32();
                size = stream.ReadInt32();
                stream.Position += -8;
                CellLookups = stream.ReadSerializedData<NavCellLookup>(size/4);
                
                int i4 = stream.ReadInt32();
                stream.Position += (3*4);

                //Hacky need size - DarkLotus
                stream.ReadInt32();
                size = stream.ReadInt32();
                stream.Position += -8;
                BorderData = stream.ReadSerializedData<NavCellBorderData>(size / 4);
              
            }
        }


    }
}
