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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CrystalMpq;
using Mooege.Common.MPQ;

namespace Mooege.Common.Extensions
{
    public static class MpqFileStreamExtensions
    {
        public static int ReadInt32(this MpqFileStream stream)
        {
            var buffer = new byte[4];
            int result = stream.Read(buffer, 0, 4);
            return BitConverter.ToInt32(buffer, 0);
        }

        public static float ReadFloat(this MpqFileStream stream)
        {
            var buffer = new byte[4];
            int result = stream.Read(buffer, 0, 4);
            return BitConverter.ToSingle(buffer, 0);
        }

        public static Int16 ReadInt16(this MpqFileStream stream)
        {
            var buffer = new byte[2];
            int result = stream.Read(buffer, 0, 2);
            return BitConverter.ToInt16(buffer, 0);
        }

        public static SerializableDataPointer GetSerializedDataPointer(this MpqFileStream stream)
        {
            return new SerializableDataPointer(stream.ReadInt32(), stream.ReadInt32());
        }

        public static List<T> ReadSerializedData<T>(this MpqFileStream stream, SerializableDataPointer pointer, int count) where T : ISerializableData, new()
        {
            var items = new List<T>(); // read-items if any.            
            if (pointer.Size <= 0) return items;

            var oldPos = stream.Position;
            stream.Position = pointer.Offset + 16; // offset is relative to actual sno data start, so add that 16 bytes file header to get actual position. /raist

            for (int i = 0; i < count; i++)
            {
                var t = new T();
                t.Read(stream);
                items.Add(t);
            }

            stream.Position = oldPos;
            return items;
        }

        public static List<T> ReadSerializedData<T>(this MpqFileStream stream, int count) where T : ISerializableData, new()
        {
            var pointer = stream.GetSerializedDataPointer();
            return stream.ReadSerializedData<T>(pointer, count);
        }

        public static T ReadSerializedData<T>(this MpqFileStream stream) where T : ISerializableData, new()
        {
            int offset = stream.ReadInt32();
            int size = stream.ReadInt32();

            var t = new T();
            if (size <= 0) return t;

            var oldPos = stream.Position;
            stream.Position = offset + 16; // offset is relative to actual sno data start, so add that 16 bytes file header to get actual position. /raist
            t.Read(stream);
            stream.Position = oldPos;
            return t;
        }

        public static List<int> ReadSerializedInts(this MpqFileStream stream)
        {
            var items = new List<int>(); // read-items if any.
            int offset = stream.ReadInt32(); // ofset for serialized data.
            int size = stream.ReadInt32(); // size of serialized data.
            if (size <= 0) return items;

            var oldPos = stream.Position;
            stream.Position = offset + 16; // offset is relative to actual sno data start, so add that 16 bytes file header to get actual position. /raist

            while (stream.Position < offset + size + 16)
            {
                items.Add(stream.ReadInt32());
            }

            stream.Position = oldPos;
            return items;
        }
    }
}
