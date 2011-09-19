using System;
using System.Linq;
using D3Sharp.Net;
using D3Sharp.Net.Packets;
using D3Sharp.Utils;
using Google.ProtocolBuffers;
using bnet.protocol;
using bnet.protocol.game_master;

namespace D3Sharp.Core.Services
{
    [Service(serviceID: 0x7, serviceName: "bnet.protocol.game_master.GameMaster")]
    public class GameMasterService : GameMaster, IServerService
    {
        protected static readonly Logger Logger = LogManager.CreateLogger();
        public IClient Client { get; set; }

        public override void JoinGame(IRpcController controller, JoinGameRequest request, Action<JoinGameResponse> done)
        {
            throw new NotImplementedException();
        }

        public override void ListFactories(IRpcController controller, ListFactoriesRequest request, Action<ListFactoriesResponse> done)
        {
            Logger.Trace("ListFactories()");

            var description = GameFactoryDescription.CreateBuilder().SetId(14249086168335147635);
            var atributes = new bnet.protocol.attribute.Attribute[4]
                                {
                                    bnet.protocol.attribute.Attribute.CreateBuilder().SetName("min_players").SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetIntValue(2)).Build(),
                                    bnet.protocol.attribute.Attribute.CreateBuilder().SetName("max_players").SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetIntValue(4)).Build(),
                                    bnet.protocol.attribute.Attribute.CreateBuilder().SetName("num_teams").SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetIntValue(1)).Build(),
                                    bnet.protocol.attribute.Attribute.CreateBuilder().SetName("version").SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetStringValue("0.3.0")).Build()
                                };

            description.AddRangeAttribute(atributes);
            description.AddStatsBucket(GameStatsBucket.CreateBuilder()
                                           .SetBucketMin(0)
                                           .SetBucketMax(4267296)
                                           .SetWaitMilliseconds(1354)
                                           .SetGamesPerHour(0)
                                           .SetActiveGames(69)
                                           .SetActivePlayers(75)
                                           .SetFormingGames(5)
                                           .SetWaitingPlayers(0).Build());

            var builder = ListFactoriesResponse.CreateBuilder().AddDescription(description).SetTotalResults(1);
            done(builder.Build());
        }

        public override void FindGame(IRpcController controller, FindGameRequest request, Action<FindGameResponse> done)
        {
            Logger.Trace("FindGame()");

            ////>>> FindGameResponse
            ////request_id: 12526585062881647236

            var findGameResponse = bnet.protocol.game_master.FindGameResponse.CreateBuilder();
            findGameResponse.SetRequestId(12526585062881647236);

            done(findGameResponse.Build());

            var gameFoundNotification = bnet.protocol.game_master.GameFoundNotification.CreateBuilder();

            var gameHandle = bnet.protocol.game_master.GameHandle.CreateBuilder();
            gameHandle.SetFactoryId(request.FactoryId);
            gameHandle.SetGameId(bnet.protocol.EntityId.CreateBuilder().SetHigh(433661094641971304).SetLow(11017467167309309688).Build());

            var connectInfo = bnet.protocol.game_master.ConnectInfo.CreateBuilder();
            connectInfo.SetToonId(Client.Account.Toons.First().Value.BnetEntityID);
            connectInfo.SetHost("127.0.0.1");
            connectInfo.SetPort(1345);
            connectInfo.SetToken(ByteString.CopyFrom(new byte[] { 0x07, 0x34, 0x02, 0x60, 0x91, 0x93, 0x76, 0x46, 0x28, 0x84 }));
            connectInfo.AddAttribute(bnet.protocol.attribute.Attribute
                .CreateBuilder()
                .SetName("SGameId")
                .SetValue(bnet.protocol.attribute.Variant
                        .CreateBuilder()
                        .SetIntValue(2014314530)
                        .Build())
                .Build());

            gameFoundNotification.SetRequestId(12526585062881647236);
            gameFoundNotification.SetGameHandle(gameHandle.Build());
            gameFoundNotification.AddConnectInfo(connectInfo.Build());

            var c = this.Client as Client;
            c.CallMethod(GameFactorySubscriber.Descriptor.FindMethodByName("NotifyGameFound"), gameFoundNotification.Build(),1);


            ////            >>> GameFoundNotification
            ////request_id: 12526585062881647236
            ////game_handle {
            ////  factory_id: 14249086168335147635
            ////  game_id {
            ////    high: 433661094641971304
            ////    low: 11017467167309309688
            ////  }
            ////}
            ////connect_info {
            ////  toon_id {
            ////    high: 216174302532224051
            ////    low: 2345959482769161802
            ////  }
            ////  host: "12.129.237.197"
            ////  port: 1119
            ////  token: "7340260919376462884"
            ////  attribute {
            ////    name: "SGameId"
            ////    value {
            ////      int_value: 2014314530
            ////    }
            ////  }
            ////}
        }

        public override void CancelFindGame(IRpcController controller, CancelFindGameRequest request, Action<NoData> done)
        {
            throw new NotImplementedException();
        }

        public override void GameEnded(IRpcController controller, GameEndedNotification request, Action<NO_RESPONSE> done)
        {
            throw new NotImplementedException();
        }

        public override void PlayerLeft(IRpcController controller, PlayerLeftNotification request, Action<NO_RESPONSE> done)
        {
            throw new NotImplementedException();
        }

        public override void RegisterServer(IRpcController controller, RegisterServerRequest request, Action<NoData> done)
        {
            throw new NotImplementedException();
        }

        public override void UnregisterServer(IRpcController controller, UnregisterServerRequest request, Action<NO_RESPONSE> done)
        {
            throw new NotImplementedException();
        }

        public override void RegisterUtilities(IRpcController controller, RegisterUtilitiesRequest request, Action<NoData> done)
        {
            throw new NotImplementedException();
        }

        public override void UnregisterUtilities(IRpcController controller, UnregisterUtilitiesRequest request, Action<NO_RESPONSE> done)
        {
            throw new NotImplementedException();
        }

        public override void Subscribe(IRpcController controller, SubscribeRequest request, Action<SubscribeResponse> done)
        {
            throw new NotImplementedException();
        }

        public override void Unsubscribe(IRpcController controller, UnsubscribeRequest request, Action<NO_RESPONSE> done)
        {
            throw new NotImplementedException();
        }

        public override void ChangeGame(IRpcController controller, ChangeGameRequest request, Action<NoData> done)
        {
            throw new NotImplementedException();
        }

        public override void GetFactoryInfo(IRpcController controller, GetFactoryInfoRequest request, Action<GetFactoryInfoResponse> done)
        {
            throw new NotImplementedException();
        }

        public override void GetGameStats(IRpcController controller, GetGameStatsRequest request, Action<GetGameStatsResponse> done)
        {
            throw new NotImplementedException();
        }
    }
}
