using D3Sharp.Net;
using D3Sharp.Net.Packets;

namespace D3Sharp.Core.Services
{
    // bnet.protocol.presence
    [Service(serviceID: 0xb, serverHash: 0xFA0796FF, clientHash: 0x0)]
    public class PresenceService : Service
    {
        [ServiceMethod(0x1)]
        public void Subscribe(IClient client, Packet packetIn)
        {
            Logger.Trace("RPC:Presence:Subscribe() Stub");
            // responds with NoData
        }
        
        [ServiceMethod(0x2)]
        public void Unsubscribe(IClient client, Packet packetIn)
        {
            Logger.Trace("RPC:Presence:Unsubscribe() Stub");
            // responds with NoData
        }
        
        [ServiceMethod(0x3)]
        public void Update(IClient client, Packet packetIn)
        {
            Logger.Trace("RPC:Presence:Update() Stub");
            // responds with NoData
        }
        
        [ServiceMethod(0x4)]
        public void Query(IClient client, Packet packetIn)
        {
            Logger.Trace("RPC:Presence:Query() Stub");
            // responds with QueryResponse
        }
    }
}
