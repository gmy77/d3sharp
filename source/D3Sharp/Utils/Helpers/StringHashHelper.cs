using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace D3Sharp.Utils.Helpers
{
    public class StringHashHelper
    {
        public static uint HashString(string input)
        {
            var bytes = Encoding.ASCII.GetBytes(input);
            return bytes.Aggregate(0x811C9DC5, (current, t) => 0x1000193 * (t ^ current));
        }

        public static uint HashString2(string input)
        {
            uint hash = 0;
            input = input.ToLower();

            for (int i = 0; i < input.Length; ++i)
                hash = (hash << 5) + hash + input[i];

            return hash;
        }
    }
}
