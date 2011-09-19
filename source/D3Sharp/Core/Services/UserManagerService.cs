using System;
using D3Sharp.Net;
using D3Sharp.Net.Packets;
using D3Sharp.Utils;
using D3Sharp.Utils.Extensions;
using Google.ProtocolBuffers;
using bnet.protocol.user_manager;

namespace D3Sharp.Core.Services
{
    [Service(serviceID: 0x5, serviceName: "bnet.protocol.user_manager.UserManagerService")]
    public class UserManagerService : bnet.protocol.user_manager.UserManagerService,IServerService
    {
        protected static readonly Logger Logger = LogManager.CreateLogger();
        public IClient Client { get; set; }

        public override void SubscribeToUserManager(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.user_manager.SubscribeToUserManagerRequest request, System.Action<bnet.protocol.user_manager.SubscribeToUserManagerResponse> done)
        {
            Logger.Trace("SubscribeToUserManager()");

            const ulong accountHandle = 0x0000000000000000;
            var fakeentityid = bnet.protocol.EntityId.CreateBuilder().SetHigh(accountHandle).SetLow(0x0).Build();

            // no idea which attribute the recent player object might want
            var attribute = bnet.protocol.attribute.Attribute.CreateBuilder().SetName("fakename").SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetIntValue(0)).Build();

            var recent_player = bnet.protocol.user_manager.RecentPlayer.CreateBuilder()
                .SetPlayer(fakeentityid)
                .SetTimestampPlayed(DateTime.Now.AddDays(-2).ToUnixTime())
                .AddAttributes(attribute)
                .Build();

            var builder = bnet.protocol.user_manager.SubscribeToUserManagerResponse.CreateBuilder()
                .AddBlockedUsers(fakeentityid)
                .AddRecentPlayers(recent_player);

            done(builder.Build());
        }

        public override void ReportPlayer(IRpcController controller, ReportPlayerRequest request, Action<ReportPlayerResponse> done)
        {
            throw new NotImplementedException();
        }

        public override void BlockPlayer(IRpcController controller, BlockPlayerRequest request, Action<BlockPlayerResponse> done)
        {
            throw new NotImplementedException();
        }

        public override void RemovePlayerBlock(IRpcController controller, RemovePlayerBlockRequest request, Action<RemovePlayerBlockResponse> done)
        {
            throw new NotImplementedException();
        }

        public override void AddRecentPlayers(IRpcController controller, AddRecentPlayersRequest request, Action<AddRecentPlayersResponse> done)
        {
            throw new NotImplementedException();
        }

        public override void RemoveRecentPlayers(IRpcController controller, RemoveRecentPlayersRequest request, Action<RemoveRecentPlayersResponse> done)
        {
            throw new NotImplementedException();
        }
    }
}
