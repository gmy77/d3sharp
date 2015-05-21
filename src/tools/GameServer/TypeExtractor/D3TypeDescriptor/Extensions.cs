﻿/*
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
using System.Text;
using System.Xml.Linq;

namespace D3TypeDescriptor
{
    static class Extensions
    {
        public static string ToMaybeHexString(this int value, int minHex)
        {
            if (value >= minHex || value <= -minHex)
                return "0x" + value.ToString("X");
            return value.ToString();
        }

        public static int IntAttribute(this XElement e, string name)
        {
            var a = e.Attribute(name);
            if (a == null)
                throw new Exception("Expected int attribute: " + name);
            return int.Parse(a.Value);
        }

        public static int OptionalIntAttribute(this XElement e, string name)
        {
            var a = e.Attribute(name);
            return a != null ? int.Parse(a.Value) : 0;
        }

        public static int OptionalIntAttribute(this XElement e, string name, int defaultValue)
        {
            var a = e.Attribute(name);
            return a != null ? int.Parse(a.Value) : defaultValue;
        }

        public static string OptionalStringAttribute(this XElement e, string name)
        {
            var a = e.Attribute(name);
            return a != null ? a.Value : string.Empty;
        }

        public static TypeDescriptor OptionalTypeAttribute(this XElement e, string name, Dictionary<int, TypeDescriptor> typeByIndex)
        {
            var a = e.Attribute(name);
            if (a == null)
                return null;

            int index = int.Parse(a.Value.Split('#')[1]);
            return typeByIndex[index];
        }

        public static void IndentAppendLines(this StringBuilder b, int pad, string text)
        {
            foreach (var line in text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None))
            {
                if (string.IsNullOrWhiteSpace(line))
                    b.AppendLine();
                else
                {
                    b.Append(' ', pad); b.AppendLine(line);
                }
            }
        }

        public static void IndentAppend(this StringBuilder b, int pad, string text)
        {
            b.Append(' ', pad); b.Append(text);
        }
        public static void IndentAppendLine(this StringBuilder b, int pad, string text)
        {
            b.Append(' ', pad); b.AppendLine(text);
        }

    }
}
