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
        /// <summary>
        /// The service ID -- if it's a bound service on runtime just put in -1.
        /// </summary>
        public int ServiceID { get; set; }

        /// <summary>
        /// The service hash.
        /// </summary>
        public int ServiceHash { get; set; }
        
        /// <summary>
        /// The service method.
        /// </summary>
        public byte Method { get; set; }

        /// <summary>
        /// Creates a new service attribute.
        /// </summary>
        /// <param name="serviceID">The service ID -- if it's a bound service on runtime just put in -1.</param>
        /// <param name="serviceHash">The service hash.</param>
        /// <param name="method">The service method.</param>
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
