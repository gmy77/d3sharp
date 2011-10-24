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
using Gibbed.IO;
using Mooege.Common.Extensions;

namespace Mooege.Common.MPQ.FileFormats
{
    /// <summary>
    /// Interface for serializable data structures.
    /// </summary>
    public interface ISerializableData
    {
        /// <summary>
        /// Reads serializable type.
        /// </summary>
        /// <param name="stream">The MPQFileStream to read from.</param>
        void Read(MpqFileStream stream);
    }

    /// <summary>
    /// Serialized data pointer, contains offset and size for serialized data.
    /// </summary>
    public struct SerializableDataPointer
    {
        /// <summary>
        /// The start offset for serialized data.
        /// </summary>
        public readonly int Offset;
        /// <summary>
        /// The size of serialized data.
        /// </summary>
        public readonly int Size;

        public SerializableDataPointer(int offset, int size)
        {
            this.Offset = offset;
            this.Size = size;
        }
    }

    /// <summary>
    /// Provides help function for reading serialized data from MpqFileStreams.
    /// </summary>
    public static class MpqFileStreamExtensions
    {
        /// <summary>
        /// Reads serialized data pointer.
        /// </summary>
        /// <param name="stream">The MPQFileStream to read from.</param>
        /// <returns><see cref="SerializableDataPointer"/></returns>
        public static SerializableDataPointer GetSerializedDataPointer(this MpqFileStream stream)
        {
            return new SerializableDataPointer(stream.ReadValueS32(), stream.ReadValueS32());
        }

        /// <summary>
        /// Reads a total of n serialized items.
        /// </summary>
        /// <typeparam name="T">Item type to read.</typeparam>
        /// <param name="stream">The MPQFileStream to read from.</param>
        /// <param name="count">Number of items to read from.</param>
        /// <returns>List of items.</returns>
        public static List<T> ReadSerializedData<T>(this MpqFileStream stream, int count) where T : ISerializableData, new()
        {
            var pointer = stream.GetSerializedDataPointer();
            return stream.ReadSerializedData<T>(pointer, count);
        }

        /// <summary>
        /// Reads a total of n serialized items from given serialized-data pointer.
        /// </summary>
        /// <typeparam name="T">Item type to read.</typeparam>
        /// <param name="stream">The MPQFileStream to read from.</param>
        /// <param name="pointer">The serialized-data pointer to read from.</param>
        /// <param name="count">Number of items to read from.</param>
        /// <returns>List of items.</returns>
        public static List<T> ReadSerializedData<T>(this MpqFileStream stream, SerializableDataPointer pointer, int count) where T : ISerializableData, new()
        {
            var items = new List<T>(); // read-items if any.            
            if (pointer.Size <= 0 || count == 0) return items;

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

        /// <summary>
        /// Reads all available items for given type.
        /// </summary>
        /// <typeparam name="T">Item type to read.</typeparam>
        /// <param name="stream">The MPQFileStream to read from.</param>
        /// <returns>List of items.</returns>
        public static List<T> ReadSerializedData<T>(this MpqFileStream stream) where T : ISerializableData, new()
        {
            var pointer = stream.GetSerializedDataPointer();

            var items = new List<T>(); // read-items if any.            
            if (pointer.Size <= 0) return items;

            var oldPos = stream.Position;
            stream.Position = pointer.Offset + 16; // offset is relative to actual sno data start, so add that 16 bytes file header to get actual position. /raist

            while (stream.Position < pointer.Offset + pointer.Size + 16)
            {
                var t = new T();
                t.Read(stream);
                items.Add(t);
            }

            stream.Position = oldPos;
            return items;
        }

        /// <summary>
        /// Reads a single serialized item for given type. Warning: Use with caution.
        /// </summary>
        /// <typeparam name="T">Item type to read.</typeparam>
        /// <param name="stream">The MPQFileStream to read from.</param>
        /// <returns>The read item.</returns>
        public static T ReadSerializedItem<T>(this MpqFileStream stream) where T : ISerializableData, new()
        {
            int offset = stream.ReadValueS32();
            int size = stream.ReadValueS32();

            var t = new T();
            if (size <= 0) return t;

            var oldPos = stream.Position;
            stream.Position = offset + 16; // offset is relative to actual sno data start, so add that 16 bytes file header to get actual position. /raist
            t.Read(stream);
            stream.Position = oldPos;
            return t;
        }

        /// <summary>
        /// Reads all available serialized ints.
        /// </summary>
        /// <param name="stream">The MPQFileStream to read from.</param>
        /// <returns>The list of read ints.</returns>
        public static List<int> ReadSerializedInts(this MpqFileStream stream)
        {
            var items = new List<int>(); // read-items if any.
            int offset = stream.ReadValueS32(); // ofset for serialized data.
            int size = stream.ReadValueS32(); // size of serialized data.
            if (size <= 0) return items;

            var oldPos = stream.Position;
            stream.Position = offset + 16; // offset is relative to actual sno data start, so add that 16 bytes file header to get actual position. /raist

            while (stream.Position < offset + size + 16)
            {
                items.Add(stream.ReadValueS32());
            }

            stream.Position = oldPos;
            return items;
        }
    }
}
