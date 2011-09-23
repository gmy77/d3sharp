using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BNet2ProtoExtractor
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
