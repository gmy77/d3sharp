using System;
using D3Sharp.Net;
using D3Sharp.Utils;
using Google.ProtocolBuffers;
using bnet.protocol;
using bnet.protocol.search;

namespace D3Sharp.Core.Services
{
    [Service(serviceID: 0xe, serviceName: "bnet.protocol.search.SearchService")]
    public class SearchService : bnet.protocol.search.SearchService,IServerService
    {
        protected static readonly Logger Logger = LogManager.CreateLogger();
        public IClient Client { get; set; }

        public override void FindMatches(IRpcController controller, FindMatchesRequest request, Action<FindMatchesResponse> done)
        {
            throw new NotImplementedException();
        }

        public override void SetObject(IRpcController controller, SetObjectRequest request, Action<NO_RESPONSE> done)
        {
            throw new NotImplementedException();
        }

        public override void RemoveObjects(IRpcController controller, RemoveObjectsRequest request, Action<NO_RESPONSE> done)
        {
            throw new NotImplementedException();
        }
    }
}
