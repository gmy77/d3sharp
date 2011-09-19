using D3Sharp.Net;
using D3Sharp.Net.Packets;
using D3Sharp.Utils;

namespace D3Sharp.Core.Services
{
    [Service(serviceID: 0x4, serviceName: "bnet.protocol.followers.FollowersService")]
    public class FollowersService : bnet.protocol.followers.FollowersService,IServerService
    {
        protected static readonly Logger Logger = LogManager.CreateLogger();
        public Client Client { get; set; }

        public override void SubscribeToFollowers(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.followers.SubscribeToFollowersRequest request, System.Action<bnet.protocol.followers.SubscribeToFollowersResponse> done)
        {
            Logger.Trace("SubscribeToFollowers()");
            var builder = bnet.protocol.followers.SubscribeToFollowersResponse.CreateBuilder();
            done(builder.Build());
        }

        public override void StartFollowing(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.followers.StartFollowingRequest request, System.Action<bnet.protocol.followers.StartFollowingResponse> done)
        {
            throw new System.NotImplementedException();
        }

        public override void StopFollowing(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.followers.StopFollowingRequest request, System.Action<bnet.protocol.followers.StopFollowingResponse> done)
        {
            throw new System.NotImplementedException();
        }

        public override void UpdateFollowerState(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.followers.UpdateFollowerStateRequest request, System.Action<bnet.protocol.followers.UpdateFollowerStateResponse> done)
        {
            throw new System.NotImplementedException();
        }
    }
}
