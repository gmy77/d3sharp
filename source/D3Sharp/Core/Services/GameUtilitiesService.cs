using System;
using System.Linq;
using D3Sharp.Net;
using D3Sharp.Net.Packets;
using D3Sharp.Utils;
using Google.ProtocolBuffers;
using bnet.protocol;
using bnet.protocol.game_utilities;
using bnet.protocol.server_pool;

namespace D3Sharp.Core.Services
{
    [Service(serviceID: 0x8, serviceName: "bnet.protocol.game_utilities.GameUtilities")]
    public class GameUtilitiesService : GameUtilities,IServerService
    {
        protected static readonly Logger Logger = LogManager.CreateLogger();
        public IClient Client { get; set; }

        public override void ProcessClientRequest(IRpcController controller, ClientRequest request, Action<ClientResponse> done)
        {
            Logger.Trace("ProcessClientRequest()");

            // TODO: handle the request. this is where banner changing happens (CustomMessageId 4)
            // CustomMessage for banner change is a D3.GameMessages.SaveBannerConfiguration
            Logger.Debug("request:\n{0}", request.ToString());
            
            var builder = ClientResponse.CreateBuilder();
            done(builder.Build());
        }

        public override void CreateToon(IRpcController controller, CreateToonRequest request, Action<CreateToonResponse> done)
        {
            throw new NotImplementedException();
        }

        public override void DeleteToon(IRpcController controller, DeleteToonRequest request, Action<NoData> done)
        {
            throw new NotImplementedException();
        }

        public override void TransferToon(IRpcController controller, TransferToonRequest request, Action<NoData> done)
        {
            throw new NotImplementedException();
        }

        public override void SelectToon(IRpcController controller, SelectToonRequest request, Action<NoData> done)
        {
            throw new NotImplementedException();
        }

        public override void PresenceChannelCreated(IRpcController controller, PresenceChannelCreatedRequest request, Action<NoData> done)
        {
            throw new NotImplementedException();
        }

        public override void GetPlayerVariables(IRpcController controller, PlayerVariablesRequest request, Action<VariablesResponse> done)
        {
            throw new NotImplementedException();
        }

        public override void GetGameVariables(IRpcController controller, GameVariablesRequest request, Action<VariablesResponse> done)
        {
            throw new NotImplementedException();
        }

        public override void GetLoad(IRpcController controller, GetLoadRequest request, Action<ServerState> done)
        {
            throw new NotImplementedException();
        }
    }
}
