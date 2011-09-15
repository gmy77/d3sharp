using System;
using System.Collections.Generic;
using System.Linq;

namespace D3Sharp.Utils.Extensions
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

        public static string Dump(this byte[] array)
        {
            var output = Environment.NewLine;
            var hex = string.Empty; // hex buffer. 
            var text = string.Empty; // text buffer.

            int i = 0;

            foreach (byte value in array)
            {
                if (i > 0 && ((i%16) == 0)) // with-in every 16 chars, put a new line.
                {
                    output += string.Format("{0} {1}", hex, text);
                    hex = text = string.Empty;
                    output += Environment.NewLine;
                }

                hex += value.ToString("X2") + " ";
                text += string.Format("{0}", (char.IsWhiteSpace((char) value) && (char) value != ' ') ? '.' : (char) value); // pretfy the text.
                i++;
            }

            if (text.Length < 16) hex = hex.PadRight(48); // pad the hex representation in-case it's smaller than a regular 16 value line.
            output += string.Format("{0} {1}", hex, text);
            output += Environment.NewLine;
            return output;
        }
    }
}

