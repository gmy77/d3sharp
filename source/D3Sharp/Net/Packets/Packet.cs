using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using D3Sharp.Utils.Extensions;

namespace D3Sharp.Net.Packets
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ServiceAttribute : Attribute
    {
        public int ServiceID { get; set; }
        public int ServiceHash { get; set; }
        public byte Method { get; set; }

        public ServiceAttribute(int serviceID, int serviceHash, byte method)
        {
            this.ServiceID = serviceID;
            this.ServiceHash = serviceHash;
            this.Method = method;
        }
    }

    public class Packet
    {
        public Header Header { get; protected set; }
        public IEnumerable<byte> Payload { get; protected set; }
    }
}
