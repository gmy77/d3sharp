using System;
using D3Sharp.Core.Channels;
using D3Sharp.Net;
using D3Sharp.Utils;
using Google.ProtocolBuffers;
using bnet.protocol.channel;

namespace D3Sharp.Core.Services
{
    [Service(serviceID: 0x0D, serviceName: "bnet.protocol.party.PartyService")]
    public class PartyService : bnet.protocol.party.PartyService,IServerService
    {
        protected static readonly Logger Logger = LogManager.CreateLogger();
        public IClient Client { get; set; }

        public override void CreateChannel(IRpcController controller, CreateChannelRequest request, Action<CreateChannelResponse> done)
        {
            Logger.Trace("CreateChannel()");
            //Logger.Debug("request:\n{0}", request.ToString());
            
            var newChannel = ChannelsManager.CreateNewChannel(this.Client, request.ObjectId);
            var builder = CreateChannelResponse.CreateBuilder()
                .SetObjectId(newChannel.ID)
                .SetChannelId(newChannel.BnetEntityID);

            done(builder.Build());

            newChannel.NotifyChannelState((Client)this.Client);
        }

        public override void JoinChannel(IRpcController controller, JoinChannelRequest request, Action<JoinChannelResponse> done)
        {
            throw new NotImplementedException();
        }

        public override void GetChannelInfo(IRpcController controller, GetChannelInfoRequest request, Action<GetChannelInfoResponse> done)
        {
            throw new NotImplementedException();
        }
    }
}
