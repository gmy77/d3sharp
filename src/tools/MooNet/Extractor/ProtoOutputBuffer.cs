using System;
using System.Collections.Generic;

namespace Extractor
{
    public static class ProtoOutputBuffer
    {
        private static List<string> _buffer = new List<string>();

        public static void Write(Type requesttype, string proto)
        {
            Console.WriteLine(requesttype.FullName);
            _buffer.Add(proto);
        }
    }
}
