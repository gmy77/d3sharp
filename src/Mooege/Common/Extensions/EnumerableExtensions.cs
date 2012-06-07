/*
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mooege.Common.Extensions
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
            if (sb.Length > 0)
                sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }

        public static string ToEncodedString(this IEnumerable<byte> collection, Encoding encoding)
        {
            return encoding.GetString(collection.ToArray());
        }

        public static string Dump(this IEnumerable<byte> collection)
        {
            var output = new StringBuilder();
            var hex = new StringBuilder();
            var text = new StringBuilder();
            int i = 0;
            foreach (byte value in collection)
            {
                if (i > 0 && ((i % 16) == 0))
                {
                    output.Append(hex);
                    output.Append(' ');
                    output.Append(text);
                    output.Append(Environment.NewLine);
                    hex.Clear(); text.Clear();
                }
                hex.Append(value.ToString("X2"));
                hex.Append(' ');
                text.Append(string.Format("{0}", (char.IsWhiteSpace((char)value) && (char)value != ' ') ? '.' : (char)value)); // prettify text
                ++i;
            }
            var hexstring = hex.ToString();
            if (text.Length < 16)
            {
                hexstring = hexstring.PadRight(48); // pad the hex representation in-case it's smaller than a regular 16 value line.
            }
            output.Append(hexstring);
            output.Append(' ');
            output.Append(text);
            return output.ToString();
        }
    }
}