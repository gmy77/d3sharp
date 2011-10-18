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

namespace Mooege.Tools.ProtoDumper
{
    public static class ArrayExtensions
    {
        public static IEnumerable<T> EnumerateFrom<T>(this T[] array, int start)
        {
            if (array == null)
                throw new ArgumentNullException("array");

            return Enumerate<T>(array, start, array.Length);
        }

        public static IEnumerable<T> Enumerate<T>(this T[] array, int start, int count)
        {
            if (array == null)
                throw new ArgumentNullException("array");

            for (int i = 0; i < count; i++)
                yield return array[start + i];
        }

        public static byte[] Append(this byte[] a, byte[] b)
        {
            var result = new byte[a.Length + b.Length];

            a.CopyTo(result, 0);
            b.CopyTo(result, a.Length);

            return result;
        }
    }

    public class StringHashHelper
    {
        // Used on the full name of a proto service for their ServiceHash and for toon names
        public static uint HashIdentity(string input)
        {
            var bytes = Encoding.ASCII.GetBytes(input);
            return bytes.Aggregate(0x811C9DC5, (current, t) => 0x1000193 * (t ^ current));
        }

        // Hash algorithm used for item names
        public static uint HashItemName(string input)
        {
            uint hash = 0;
            input = input.ToLower();
            for (int i = 0; i < input.Length; ++i)
                hash = (hash << 5) + hash + input[i];
            return hash;
        }
    }
}