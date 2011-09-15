using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Google.ProtocolBuffers;

namespace D3Sharp.Net.Packets
{
    public class Header
    {
        public byte[] Data { get; private set; }

        public byte Service { get; set; }
        public uint Method { get; set; }
        public int RequestID { get; set; }
        public ulong Unknown { get; set; }
        public uint PayloadLength { get; set; }

        public Header()
        {            
            this.Unknown = 0x0;
            this.PayloadLength = 0x0;
        }

        public Header(byte[] data)
        {
            this.Data = data;

            var stream = CodedInputStream.CreateInstance(data);
            this.Service = stream.ReadRawByte();
            this.Method = stream.ReadRawVarint32();
            this.RequestID = stream.ReadRawByte() | (stream.ReadRawByte() << 8);
            if (Service != 0xfe) this.Unknown = stream.ReadRawVarint64();
            this.PayloadLength = stream.ReadRawVarint32();
        }

        public Header(IEnumerable<byte> data)
            : this(data.ToArray())
        {
        }

        public void Build()
        {            
            //var stream = CodedOutputStream.CreateInstance(this.Data);
            //stream.WriteRawByte(this.Service);
            //stream.WriteRawVarint32(this.Method);
            //stream.WriteRawByte((byte) this.RequestID);
            //stream.WriteRawVarint64(this.Unknown);
            //stream.WriteRawVarint32(this.PayloadLength);
        }

        public override string ToString()
        {
            return string.Format("[S]: 0x{0}, [M]: 0x{1}, [R]: 0x{2}, [L]: 0x{3}", this.Service.ToString("X2"), this.Method.ToString("X2"), this.RequestID.ToString("X2"), this.PayloadLength.ToString("X2"));
        }
    }
}
