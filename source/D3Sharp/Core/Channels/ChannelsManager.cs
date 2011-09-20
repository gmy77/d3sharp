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

        // We'll start at a high value to avoid ID conflicts for now
        private static ulong _channelgen = 100000;

        public static Channel CreateNewChannel()
        {
            var channel = new Channel(_channelgen++);
            Channels.Add(channel.ID, channel);
            return channel;
        }
        
        public static Channel DeleteChannel(ulong id) {
            throw new System.NotImplementedException();
            // TODO: Mapping removal should be done in client or mayhaps the ID controller
        }
    }
}
