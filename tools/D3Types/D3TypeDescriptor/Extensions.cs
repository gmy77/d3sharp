using System;
using System.Collections.Generic;
using System.Linq;
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
