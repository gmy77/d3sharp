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

        public byte ServiceID { get; set; }
        public uint MethodID { get; set; }
        public int RequestID { get; set; }
        public ulong Unknown { get; set; }
        public uint PayloadLength { get; set; }

        public Header()
        {            
            this.Unknown = 0x00;
            this.PayloadLength = 0x00;
        }

        public Header(byte[] data)
        {
            this.Data = data;

            var stream = CodedInputStream.CreateInstance(data);
            this.ServiceID = stream.ReadRawByte();
            this.MethodID = stream.ReadRawVarint32();
            this.RequestID =  stream.ReadRawByte() | (stream.ReadRawByte() << 8);
            if (ServiceID != 0xfe)
                this.Unknown = stream.ReadRawVarint64();
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
            //if (ServiceID != 0xfe)
                //stream.WriteRawVarint64(this.Unknown);
            //stream.WriteRawVarint32(this.PayloadLength);
        }

        public override string ToString()
        {
            return string.Format("[S]: 0x{0}, [M]: 0x{1}, [R]: 0x{2}, [L]: 0x{3}", this.ServiceID.ToString("X2"), this.MethodID.ToString("X2"), this.RequestID.ToString("X2"), this.PayloadLength.ToString("X2"));
        }
    }
}
