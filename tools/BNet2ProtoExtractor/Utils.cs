using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BNet2ProtoExtractor
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