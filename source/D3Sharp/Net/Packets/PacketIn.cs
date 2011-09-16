using System;
using System.Collections.Generic;
using System.Linq;
using D3Sharp.Utils.Extensions;

namespace D3Sharp.Net.Packets
{
    public class PacketIn:Packet
    {
        public dynamic Request { get; protected set; }

        public PacketIn(IEnumerable<byte> header, IEnumerable<byte> payload)
        {
            this.Header = new Header(header);
            this.Payload = payload;
        }

        public PacketIn(byte[] header, byte[] payload)
        {
            this.Header = new Header(header.ToArray());
            this.Payload = payload;
        }

        public PacketIn(Header header, IEnumerable<byte> payload)
        {
            this.Header = header;
            this.Payload = payload;
        }

        public PacketIn(Header header, byte[] payload)
        {
            this.Header = header;
            this.Payload = payload;
        }

        public override string ToString()
        {
            return
            string.Format(
                "\n===========[IN]===========\nType\t: {0}\nHeader\t: {1}\nProto\t: {2}Data\t: {3}- {4}",
                (this.Request != null) ? this.Request.GetType() : this.GetType(),
                this.Header,
                this.Request ?? this.Payload.HexDump(),
                this.Header.Data.HexDump(),
                this.Payload.HexDump()
            );
        }
    }
}
