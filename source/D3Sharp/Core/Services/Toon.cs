using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using D3Sharp.Net;
using D3Sharp.Net.Packets;

namespace D3Sharp.Core.Services
{
    [Service(serviceID: 0x3, serviceHash: 0x83040608)]
    public class Toon : Service
    {
        [ServiceMethod(0x1)]
        public void GetToonList(IClient client, Packet packet)
        {

        }
    }
}
