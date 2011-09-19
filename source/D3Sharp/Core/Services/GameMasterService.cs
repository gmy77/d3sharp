using System;
using D3Sharp.Net;
using D3Sharp.Net.Packets;
using D3Sharp.Utils;
using Google.ProtocolBuffers;
using bnet.protocol;
using bnet.protocol.game_master;

namespace D3Sharp.Core.Services
{
    [Service(serviceID: 0x7, serviceName: "bnet.protocol.game_master.GameMaster")]
    public class GameMasterService : GameMaster,IServerService
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
                                           .SetBucketMax(4294967296F)
                                           .SetWaitMilliseconds(1354)
                                           .SetGamesPerHour(0)
                                           .SetActiveGames(1)
                                           .SetActivePlayers(1)
                                           .SetFormingGames(0)
                                           .SetWaitingPlayers(0).Build());

            var builder =ListFactoriesResponse.CreateBuilder().AddDescription(description).SetTotalResults(1);
            done(builder.Build());
        }

        public override void FindGame(IRpcController controller, FindGameRequest request, Action<FindGameResponse> done)
        {
            throw new NotImplementedException();
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
