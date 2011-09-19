using System;
using System.Collections.Generic;
using System.IO;
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
        public ulong ObjectID { get; set; }
        public uint PayloadLength { get; set; }

        public Header()
        {            
            this.ObjectID = 0x00;
            this.PayloadLength = 0x00;
        }

        public Header(byte serviceId, uint methodId, int requestId, uint payloadLenght, ulong objectID)
        {
            this.SetData(serviceId, methodId, requestId, payloadLenght, objectID);
        }

        public Header(CodedInputStream stream)
        {
            var serviceId = stream.ReadRawByte();
            var methodId = stream.ReadRawVarint32();
            var requestId = stream.ReadRawByte() | (stream.ReadRawByte() << 8);

            var objectId = 0UL;
            if (serviceId != 0xfe) objectId = stream.ReadRawVarint64();
            var payloadLength = stream.ReadRawVarint32();

            this.SetData(serviceId, methodId, requestId, payloadLength, objectId);
        }

        private void SetData(byte serviceId, uint methodId, int requestId, uint payloadLenght, ulong objectId)
        {
            this.ServiceID = serviceId;
            this.MethodID = methodId;
            this.RequestID = requestId;
            this.ObjectID = objectId;
            this.PayloadLength = payloadLenght;

            this.Data = this.ServiceID != 0xfe ? new byte[6] : new byte[5];

            using (var stream = new MemoryStream())
            {
                var output = CodedOutputStream.CreateInstance(stream);
                output.WriteRawByte(this.ServiceID);
                output.WriteRawVarint32(this.MethodID);
                output.WriteRawByte((byte)(this.RequestID & 0xff));
                output.WriteRawByte((byte)(this.RequestID >> 8));
                if (serviceId != 0xfe) output.WriteRawVarint64(this.ObjectID);
                output.WriteRawVarint32(this.PayloadLength);
                output.Flush();

                this.Data = stream.ToArray();
            }
        }

        public override string ToString()
        {
            return string.Format("[S]: 0x{0}, [M]: 0x{1}, [R]: 0x{2}, [L]: 0x{3}", this.ServiceID.ToString("X2"), this.MethodID.ToString("X2"), this.RequestID.ToString("X2"), this.PayloadLength.ToString("X2"));
        }
    }
}
