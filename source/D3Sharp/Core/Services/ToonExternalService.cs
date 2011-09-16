using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using D3Sharp.Net;
using D3Sharp.Net.Packets;

namespace D3Sharp.Core.Services
{
    [Service(serviceID: 0x2, serverHash: 0x4124C31B, clientHash: 0x0)]
    public class ToonExternalService : Service
    {
        [ServiceMethod(0x1)]
        public void GetToonList(IClient client, Packet packet)
        {

        }
    }
}
