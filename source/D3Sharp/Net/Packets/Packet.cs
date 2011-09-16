using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using D3Sharp.Utils.Extensions;

namespace D3Sharp.Net.Packets
{
    public class Packet
    {
        public Header Header { get; protected set; }
        public IEnumerable<byte> Payload { get; set; }

        public Packet(Header header, byte[] payload)
        {
            this.Header = header;
            this.Payload = payload;
        }

        public int Lenght
        {
            get { return this.Header.Data.Length + this.Payload.ToArray().Length; }
        }

        public byte[] GetRawPacketData()
        {
            return this.Header.Data.Append(this.Payload.ToArray());
        }

        public override string ToString()
        {
            return
            string.Format(
                "Header\t: {0}\nData\t: {1}- {2}",
                this.Header,
                this.Header.Data.HexDump(),
                this.Payload.HexDump()
            );
        }
    }
}
