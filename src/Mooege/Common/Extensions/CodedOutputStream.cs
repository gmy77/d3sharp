using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mooege.Common.Extensions
{
    public static class CodedOutputStreamExtensions
    {
        public static void WriteInt16NoTag(this Google.ProtocolBuffers.CodedOutputStream s, short value)
        {
            s.WriteRawBytes(BitConverter.GetBytes(value));
        }
    }
}
