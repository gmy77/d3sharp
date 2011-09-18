using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace D3Sharp.Core.Channels
{
    public static class ChannelsManager
    {
        private readonly static Dictionary<ulong, Channel> Channels =
            new Dictionary<ulong, Channel>();

        public static Channel CreateNewChannel(bnet.protocol.channel.ChannelState state=null)
        {
            return new Channel((ulong) Channels.Count, state);
        }
    }
}
