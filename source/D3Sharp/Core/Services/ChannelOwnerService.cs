using D3Sharp.Net;
using D3Sharp.Utils;

namespace D3Sharp.Core.Services
{
    [Service(serviceID: 0x11, serviceName: "bnet.protocol.channel.ChannelOwner")]
    public class ChannelOwnerService : bnet.protocol.channel.ChannelOwner, IServerService
    {
        protected static readonly Logger Logger = LogManager.CreateLogger();
        public IClient Client { get; set; }

        public override void CreateChannel(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.channel.CreateChannelRequest request, System.Action<bnet.protocol.channel.CreateChannelResponse> done)
        {
            throw new System.NotImplementedException();
        }

        public override void FindChannel(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.channel.FindChannelRequest request, System.Action<bnet.protocol.channel.FindChannelResponse> done)
        {
            throw new System.NotImplementedException();
        }

        public override void GetChannelId(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.channel.GetChannelIdRequest request, System.Action<bnet.protocol.channel.GetChannelIdResponse> done)
        {
            throw new System.NotImplementedException();
        }

        public override void GetChannelInfo(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.channel.GetChannelInfoRequest request, System.Action<bnet.protocol.channel.GetChannelInfoResponse> done)
        {
            throw new System.NotImplementedException();
        }

        public override void JoinChannel(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.channel.JoinChannelRequest request, System.Action<bnet.protocol.channel.JoinChannelResponse> done)
        {
            Logger.Trace("JoinChannel()");

            ////>>> JoinChannelRequest
            ////channel_id {
            ////  high: 433661094641971304
            ////  low: 11017467167309309688
            ////}
            ////object_id: 15


            ////>>> JoinChannelResponse
            ////object_id: 67122

            var builder = bnet.protocol.channel.JoinChannelResponse.CreateBuilder().SetObjectId(67122);
            done(builder.Build());
        }
    }
}
