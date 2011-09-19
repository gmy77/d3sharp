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

        // Maybe it doesn't like 0 as a channel id?
        private static ulong _idGenerated=1000;

        public static Channel CreateNewChannel(Client client, ulong externalObjectId)
        {
            var channel = new Channel(_idGenerated++);
            client.MapLocalObjectID(channel.ID, externalObjectId);
            Channels.Add(channel.ID, channel);
            channel.Add(client);
            return channel;
        }
        
        public static Channel DeleteChannel(ulong id) {
            throw new System.NotImplementedException();
            // Should remove mapped client-server IDs here..
        }
    }
}
