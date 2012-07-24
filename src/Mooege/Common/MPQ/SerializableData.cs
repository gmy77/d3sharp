﻿/*
 * Copyright (C) 2011 - 2012 mooege project - http://www.mooege.org
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

namespace Mooege.Common.MPQ
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
    /// Provides help function for reading serialized data from MpqFileStreams.
    /// </summary>
    public static class MpqFileStreamExtensions
    {
        private static Logging.Logger logger = new Logging.Logger("MPQ deserialization");

        /// <summary>
        /// Reads all available items for given type.
        /// </summary>
        /// <typeparam name="T">Item type to read.</typeparam>
        /// <param name="stream">The MPQFileStream to read from.</param>
        /// <returns>List of items.</returns>
        public static List<T> ReadSerializedData<T>(this MpqFileStream stream) where T : ISerializableData, new()
        {
            int offset = stream.ReadValueS32(); // ofset for serialized data.
            int size = stream.ReadValueS32(); // size of serialized data.

            if (offset == 0 && size != 0)
                logger.Error("Pointer error while deserializing list of {0}. Make sure you dont read too much or too few fields!", typeof(T).Name); 

            var items = new List<T>(); // read-items if any.            
            if (size <= 0 || offset == 0) return items;

            var oldPos = stream.Position;
            stream.Position = offset + 16; // offset is relative to actual sno data start, so add that 16 bytes file header to get actual position. /raist

            while (stream.Position < offset + size + 16)
            {
                var t = new T();
                t.Read(stream);
                items.Add(t);
            }

            if (stream.Position != offset + size + 16)
                logger.Error("Size mismatch while deserializing list of {0}. Make sure you dont read too much or too few fields!", typeof(T).Name); 

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
            int offset = stream.ReadValueS32(); // ofset for serialized data.
            int size = stream.ReadValueS32(); // size of serialized data.

            var t = new T();
            if (size <= 0) return t;

            var oldPos = stream.Position;
            stream.Position = offset + 16; // offset is relative to actual sno data start, so add that 16 bytes file header to get actual position. /raist
            t.Read(stream);

            if (stream.Position != offset + size + 16)
                logger.Error("Size mismatch while deserializing single item of {0}. Make sure you dont read too much or too few fields!", typeof(T).Name); 

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
            int offset = stream.ReadValueS32(); // ofset for serialized data.
            int size = stream.ReadValueS32(); // size of serialized data.

            var items = new List<int>(); // read-items if any.
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

        /// <summary>
        /// Reads all available serialized shorts.
        /// </summary>
        /// <param name="stream">The MPQFileStream to read from.</param>
        /// <returns>The list of read shorts.</returns>
        public static List<short> ReadSerializedShorts(this MpqFileStream stream)
        {
            int offset = stream.ReadValueS32(); // ofset for serialized data.
            int size = stream.ReadValueS32(); // size of serialized data.

            var items = new List<short>(); // read-items if any.
            if (size <= 0) return items;

            var oldPos = stream.Position;
            stream.Position = offset + 16; // offset is relative to actual sno data start, so add that 16 bytes file header to get actual position. /raist

            while (stream.Position < offset + size + 16)
            {
                items.Add(stream.ReadValueS16());
            }

            stream.Position = oldPos;
            return items;
        }

        /// <summary>
        /// Reads a serialized byte array
        /// </summary>
        /// <param name="stream">The MPQFileStream to read from.</param>
        /// <returns>The serialized byte array</returns>
        public static byte[] ReadSerializedByteArray(this MpqFileStream stream)
        {
            int offset = stream.ReadValueS32(); // ofset for serialized data.
            int size = stream.ReadValueS32(); // size of serialized data.

            byte[] buffer = new byte[size];
            if (size <= 0) return buffer;

            var oldPos = stream.Position;
            stream.Position = offset + 16; // offset is relative to actual sno data start, so add that 16 bytes file header to get actual position. /raist
            stream.Read(buffer, 0, size);
            stream.Position = oldPos;
            return buffer;
        }

        /// <summary>
        /// Reads a serialized string.
        /// </summary>
        /// <param name="stream">The MPQFileStream to read from.</param>
        /// <returns>Read string.</returns>
        public static string ReadSerializedString(this MpqFileStream stream)
        {
            int offset = stream.ReadValueS32(); // ofset for serialized data.
            int size = stream.ReadValueS32(); // size of serialized data.

            var @string = string.Empty;
            if (size <= 0) return @string;

            var oldPos = stream.Position;
            stream.Position = offset + 16; // offset is relative to actual sno data start, so add that 16 bytes file header to get actual position. /raist

            @string = stream.ReadString((uint) size, true);
            stream.Position = oldPos;

            return @string;
        }
    }
}
