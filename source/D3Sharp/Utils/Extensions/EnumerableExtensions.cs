using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace D3Sharp.Utils.Extensions
{
    public static class EnumerableExtensions
    {
        public static string HexDump(this IEnumerable<byte> collection)
        {
            var sb = new StringBuilder();

            foreach (byte value in collection)
            {
                sb.Append("0x");
                sb.Append(value.ToString("X2"));
                sb.Append(' ');
            }

            return sb.ToString();
        }

        public static string ToEncodedString(this IEnumerable<byte> collection, Encoding encoding)
        {
            return encoding.GetString(collection.ToArray());
        }

        public static string Dump(this IEnumerable<byte> collection)
        {
            var output = Environment.NewLine;
            var hex = string.Empty; // hex buffer. 
            var text = string.Empty; // text buffer.

            int i = 0;

            foreach (byte value in collection)
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

