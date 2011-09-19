using System;
using D3Sharp.Net;
using D3Sharp.Utils;
using Google.ProtocolBuffers;
using bnet.protocol;
using bnet.protocol.notification;

namespace D3Sharp.Core.Services
{
    [Service(serviceID: 0xc, serviceName: "bnet.protocol.notification.NotificationService")]
    public class NotificationService : bnet.protocol.notification.NotificationService, IServerService
    {
        protected static readonly Logger Logger = LogManager.CreateLogger();
        public IClient Client { get; set; }

        public override void SendNotification(IRpcController controller, Notification request, Action<NoData> done)
        {
            throw new NotImplementedException();
        }

        public override void RegisterClient(IRpcController controller, RegisterClientRequest request, Action<NoData> done)
        {
            throw new NotImplementedException();
        }

        public override void UnregisterClient(IRpcController controller, UnregisterClientRequest request, Action<NoData> done)
        {
            throw new NotImplementedException();
        }

        public override void FindClient(IRpcController controller, FindClientRequest request, Action<FindClientResponse> done)
        {
            throw new NotImplementedException();
        }
    }
}
