using D3Sharp.Net;
using D3Sharp.Net.Packets;

﻿namespace D3Sharp.Core.Services
{
    [Service(serviceID: 0x8, serviceName: "bnet.protocol.game_utilities.GameUtilities", clientHash: 0x0)]
    public class GameUtilitiesService : Service
    {
        [ServiceMethod(0x1)]
        public void ProcessClient(IClient client, Packet packetIn)
        {
            Logger.Trace("RPC:GameMaster:ProcessClient() Stub");
            //var request = bnet.protocol.game_utilities.ProcessClient.ParseFrom(packetIn.Payload.ToArray());
        }
        
        [ServiceMethod(0x2)]
        public void CreateToon(IClient client, Packet packetIn)
        {
            Logger.Trace("RPC:GameMaster:CreateToon() Stub");
            //var request = bnet.protocol.game_utilities.CreateToonRequest.ParseFrom(packetIn.Payload.ToArray());
        }
        
        [ServiceMethod(0x3)]
        public void DeleteToon(IClient client, Packet packetIn)
        {
            Logger.Trace("RPC:GameMaster:DeleteToon() Stub");
            //var request = bnet.protocol.game_utilities.DeleteToonRequest.ParseFrom(packetIn.Payload.ToArray());
        }
        
        [ServiceMethod(0x4)]
        public void TransferToon(IClient client, Packet packetIn)
        {
            Logger.Trace("RPC:GameMaster:TransferToon() Stub");
            //var request = bnet.protocol.game_utilities.TransferToonRequest.ParseFrom(packetIn.Payload.ToArray());
        }
        
        [ServiceMethod(0x5)]
        public void SelectToon(IClient client, Packet packetIn)
        {
            Logger.Trace("RPC:GameMaster:SelectToon() Stub");
            //var request = bnet.protocol.game_utilities.SelectToonRequest.ParseFrom(packetIn.Payload.ToArray());
        }
        
        [ServiceMethod(0x6)]
        public void PresenceChannelCreated(IClient client, Packet packetIn)
        {
            Logger.Trace("RPC:GameMaster:PresenceChannelCreated() Stub");
            //var request = bnet.protocol.game_utilities.PresenceChannelCreatedRequest.ParseFrom(packetIn.Payload.ToArray());
        }
        
        [ServiceMethod(0x7)]
        public void GetPlayerVariables(IClient client, Packet packetIn)
        {
            Logger.Trace("RPC:GameMaster:GetPlayerVariables() Stub");
            //var request = bnet.protocol.game_utilities.GetPlayerVariablesRequest.ParseFrom(packetIn.Payload.ToArray());
        }
        
        [ServiceMethod(0x8)]
        public void GetGameVariables(IClient client, Packet packetIn)
        {
            Logger.Trace("RPC:GameMaster:GetGameVariables() Stub");
            //var request = bnet.protocol.game_utilities.GetGameVariablesRequest.ParseFrom(packetIn.Payload.ToArray());
        }
        
        [ServiceMethod(0x9)]
        public void GetLoad(IClient client, Packet packetIn)
        {
            Logger.Trace("RPC:GameMaster:GetLoad() Stub");
            //var request = bnet.protocol.game_utilities.GetLoadRequest.ParseFrom(packetIn.Payload.ToArray());
        }
    }
}
