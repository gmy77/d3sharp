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
            var sb=new StringBuilder();
            int i=0;
            foreach (byte value in collection) {
                if (i>0 && ((i%16)==0))
                    sb.Append(Environment.NewLine);
                sb.Append(value.ToString("X2"));
                sb.Append(' ');
                ++i;
            }
            sb.Append(Environment.NewLine);
            return sb.ToString();
        }
    }
}

