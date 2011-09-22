/*
 * Copyright (C) 2011 D3Sharp Project
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

using System.IO;
using Google.ProtocolBuffers;

namespace BNet2ProtoExtractor.BnetPacket
{
    public class BNetHeader
    {
        public byte[] Data { get; private set; }

        public byte ServiceID { get; private set; }
        public uint MethodID { get; private set; }
        public int RequestID { get; private set; }
        public ulong ObjectID { get; private set; }
        public uint PayloadLength { get; private set; }

        public BNetHeader()
        {
            this.ObjectID = 0x00;
            this.PayloadLength = 0x00;
        }

        public BNetHeader(byte serviceId, uint methodId, int requestId, uint payloadLength, ulong objectID)
        {
            this.SetData(serviceId, methodId, requestId, payloadLength, objectID);
        }

        public BNetHeader(CodedInputStream stream)
        {
            var serviceId = stream.ReadRawByte();
            var methodId = stream.ReadRawVarint32();
            var requestId = stream.ReadRawByte() | (stream.ReadRawByte() << 8);

            var objectId = 0UL;
            if (serviceId != 0xfe) objectId = stream.ReadRawVarint64();
            var payloadLength = stream.ReadRawVarint32();

            this.SetData(serviceId, methodId, requestId, payloadLength, objectId);
        }

        private void SetData(byte serviceId, uint methodId, int requestId, uint payloadLength, ulong objectId)
        {
            this.ServiceID = serviceId;
            this.MethodID = methodId;
            this.RequestID = requestId;
            this.ObjectID = objectId;
            this.PayloadLength = payloadLength;

            using (var stream = new MemoryStream(this.ServiceID != 0xfe ? 6 : 5))
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
