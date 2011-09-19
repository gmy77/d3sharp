using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using D3Sharp.Core.Accounts;
using D3Sharp.Net;

namespace D3Sharp.Core.Channels
{
    public static class ChannelsManager
    {
        private readonly static Dictionary<ulong, Channel> Channels =
            new Dictionary<ulong, Channel>();

        public static Channel CreateNewChannel(IClient client)
        {
            var channel = new Channel((ulong)Channels.Count);
            return channel;
        }
    }
}
