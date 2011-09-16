using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using D3Sharp.Utils.Extensions;

namespace D3Sharp.Net.Packets
{
    public class PacketOut:Packet
    {
        public dynamic Response { get; protected set; }

        public PacketOut(int requetID)
        {
            this.Header = new Header {RequestID = requetID};
        }

        public byte[] GetRawPacketData()
        {
            return this.Header.Data.Append(this.Payload.ToArray());
        }

        public override string ToString()
        {
            return
            string.Format(
                "\n===========[OUT]===========\nType\t: {0}\nHeader\t: {1}\nProto\t: {2}Data\t: {3}- {4}",
                this.Response.GetType(),
                this.Header,
                this.Response ?? this.Payload.HexDump(),
                this.Header.Data.HexDump(),
                this.Payload.HexDump()
            );
        }
    }
}
