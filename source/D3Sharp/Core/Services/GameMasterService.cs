using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using D3Sharp.Net;
using D3Sharp.Net.Packets;

namespace D3Sharp.Core.Services
{
    [Service(serviceID: 0x7, serverHash: 0x810CB195, clientHash: 0x0)]
    public class GameMasterService : Service
    {
        [ServiceMethod(0x2)]
        public void ListFactoriesRequest(IClient client, Packet packetIn)
        {

        }
    }
}
