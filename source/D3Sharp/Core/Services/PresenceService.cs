using D3Sharp.Net;
using D3Sharp.Net.Packets;

namespace D3Sharp.Core.Services
{
    // bnet.protocol.presence
    [Service(serviceID: 0xb, serviceName: "bnet.protocol.presence.PresenceService", clientHash: 0x0)]
    public class PresenceService : Service
    {
        [ServiceMethod(0x1)]
        public void Subscribe(IClient client, Packet packetIn)
        {
            Logger.Trace("RPC:Presence:Subscribe()");
            
            var response = bnet.protocol.NoData.CreateBuilder().Build();            
            var packet = new Packet(
                new Header(0xfe, 0x0, packetIn.Header.RequestID, (uint)response.SerializedSize),
                response.ToByteArray());
        }
        
        [ServiceMethod(0x2)]
        public void Unsubscribe(IClient client, Packet packetIn)
        {
            Logger.Trace("RPC:Presence:Unsubscribe()");

            var response = bnet.protocol.NoData.CreateBuilder().Build();
            var packet = new Packet(
                new Header(0xfe, 0x0, packetIn.Header.RequestID, (uint)response.SerializedSize),
                response.ToByteArray());
        }
        
        [ServiceMethod(0x3)]
        public void Update(IClient client, Packet packetIn)
        {
            Logger.Trace("RPC:Presence:Update()");

            var response = bnet.protocol.NoData.CreateBuilder().Build();
            var packet = new Packet(
                new Header(0xfe, 0x0, packetIn.Header.RequestID, (uint)response.SerializedSize),
                response.ToByteArray());
        }
        
        [ServiceMethod(0x4)]
        public void Query(IClient client, Packet packetIn)
        {
            Logger.Trace("RPC:Presence:Query() Stub");
            // responds with QueryResponse
        }
    }
}
