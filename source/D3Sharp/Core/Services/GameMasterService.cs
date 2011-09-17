using D3Sharp.Net;
using D3Sharp.Net.Packets;

namespace D3Sharp.Core.Services
{
    [Service(serviceID: 0x7, serverHash: 0x810CB195, clientHash: 0x0)]
    public class GameMasterService : Service
    {
        [ServiceMethod(0x1)]
        public void JoinGame(IClient client, Packet packetIn)
        {
            Logger.Trace("RPC:GameMaster:JoinGame() Stub");
            //var request = bnet.protocol.game_master.JoinGameRequest.ParseFrom(packetIn.Payload.ToArray());
        }
        
        [ServiceMethod(0x2)]
        public void ListFactories(IClient client, Packet packetIn)
        {
            Logger.Trace("RPC:GameMaster:ListFactories()");

            var description = bnet.protocol.game_master.GameFactoryDescription.CreateBuilder().SetId(14249086168335147635);

            var atributes = new bnet.protocol.attribute.Attribute[4]
                                {
                                    bnet.protocol.attribute.Attribute.CreateBuilder().SetName("min_players").SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetIntValue(2)).Build(),
                                    bnet.protocol.attribute.Attribute.CreateBuilder().SetName("max_players").SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetIntValue(4)).Build(),
                                    bnet.protocol.attribute.Attribute.CreateBuilder().SetName("num_teams").SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetIntValue(1)).Build(),
                                    bnet.protocol.attribute.Attribute.CreateBuilder().SetName("version").SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetStringValue("0.3.0")).Build()
                                };

            description.AddRangeAttribute(atributes);
            description.AddStatsBucket(bnet.protocol.game_master.GameStatsBucket.CreateBuilder()
                                           .SetBucketMin(0)
                                           .SetBucketMax(4294967296F)
                                           .SetWaitMilliseconds(1354)
                                           .SetGamesPerHour(0)
                                           .SetActiveGames(1)
                                           .SetActivePlayers(1)
                                           .SetFormingGames(0)
                                           .SetWaitingPlayers(0).Build());

            var response = bnet.protocol.game_master.ListFactoriesResponse.CreateBuilder().AddDescription(description).SetTotalResults(1).Build();

            var packet = new Packet(
                new Header(0xfe, 0x0, packetIn.Header.RequestID, (uint)response.SerializedSize),
                response.ToByteArray());

            client.Send(packet);
        }
        
        [ServiceMethod(0x3)]
        public void FindGame(IClient client, Packet packetIn)
        {
            Logger.Trace("RPC:GameMaster:FindGame() Stub");
            //var request = bnet.protocol.game_master.FindGameRequest.ParseFrom(packetIn.Payload.ToArray());
        }
        
        [ServiceMethod(0x4)]
        public void CancelFindGame(IClient client, Packet packetIn)
        {
            Logger.Trace("RPC:GameMaster:CancelFindGame() Stub");
            //var request = bnet.protocol.game_master.CancelFindGameRequest.ParseFrom(packetIn.Payload.ToArray());
        }
        
        [ServiceMethod(0x5)]
        public void GameEnded(IClient client, Packet packetIn)
        {
            Logger.Trace("RPC:GameMaster:GameEnded() Stub");
            //var request = bnet.protocol.game_master.GameEndedNotification.ParseFrom(packetIn.Payload.ToArray());
        }
        
        [ServiceMethod(0x6)]
        public void PlayerLeft(IClient client, Packet packetIn)
        {
            Logger.Trace("RPC:GameMaster:PlayerLeft() Stub");
            //var request = bnet.protocol.game_master.PlayerLeftNotification.ParseFrom(packetIn.Payload.ToArray());
        }
        
        [ServiceMethod(0x7)]
        public void RegisterServer(IClient client, Packet packetIn)
        {
            Logger.Trace("RPC:GameMaster:RegisterServer() Stub");
            //var request = bnet.protocol.game_master.RegisterServerRequest.ParseFrom(packetIn.Payload.ToArray());
        }
        
        [ServiceMethod(0x8)]
        public void UnregisterServer(IClient client, Packet packetIn)
        {
            Logger.Trace("RPC:GameMaster:UnregisterServer() Stub");
            //var request = bnet.protocol.game_master.UnregisterServerRequest.ParseFrom(packetIn.Payload.ToArray());
        }
        
        [ServiceMethod(0x9)]
        public void RegisterUtilities(IClient client, Packet packetIn)
        {
            Logger.Trace("RPC:GameMaster:RegisterUtilities() Stub");
            //var request = bnet.protocol.game_master.RegisterUtilitiesRequest.ParseFrom(packetIn.Payload.ToArray());
        }
        
        [ServiceMethod(0x0a)]
        public void UnregisterUtilities(IClient client, Packet packetIn)
        {
            Logger.Trace("RPC:GameMaster:UnregisterUtilities() Stub");
            //var request = bnet.protocol.game_master.UnregisterUtilitiesRequest.ParseFrom(packetIn.Payload.ToArray());
        }
        
        [ServiceMethod(0xb)]
        public void Subscribe(IClient client, Packet packetIn)
        {
            Logger.Trace("RPC:GameMaster:Subscribe() Stub");
            //var request = bnet.protocol.game_master.SubscribeRequest.ParseFrom(packetIn.Payload.ToArray());
        }
        
        [ServiceMethod(0xc)]
        public void Unsubscribe(IClient client, Packet packetIn)
        {
            Logger.Trace("RPC:GameMaster:Unsubscribe() Stub");
            //var request = bnet.protocol.game_master.UnsubscribeRequest.ParseFrom(packetIn.Payload.ToArray());
        }
        
        [ServiceMethod(0xd)]
        public void ChangeGame(IClient client, Packet packetIn)
        {
            Logger.Trace("RPC:GameMaster:ChangeGame() Stub");
            //var request = bnet.protocol.game_master.ChangeGameRequest.ParseFrom(packetIn.Payload.ToArray());
        }
        
        [ServiceMethod(0xe)]
        public void GetFactoryInfo(IClient client, Packet packetIn)
        {
            Logger.Trace("RPC:GameMaster:GetFactoryInfo() Stub");
            //var request = bnet.protocol.game_master.GetFactoryInfoRequest.ParseFrom(packetIn.Payload.ToArray());
        }
        
        [ServiceMethod(0xf)]
        public void GetGameStats(IClient client, Packet packetIn)
        {
            Logger.Trace("RPC:GameMaster:GetGameStats() Stub");
            //var request = bnet.protocol.game_master.GetGameStatsRequest.ParseFrom(packetIn.Payload.ToArray());
        }
    }
}
