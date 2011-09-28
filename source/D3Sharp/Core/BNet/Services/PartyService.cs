/*
 * Copyright (C) 2011 D3Sharp Project
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */

using System;
using D3Sharp.Core.BNet.Channels;
using D3Sharp.Net.BNet;
using D3Sharp.Utils;
using Google.ProtocolBuffers;
using bnet.protocol.channel;

namespace D3Sharp.Core.BNet.Services
{
    [Service(serviceID: 0x0D, serviceName: "bnet.protocol.party.PartyService")]
    public class PartyService : bnet.protocol.party.PartyService,IServerService
    {
        private static readonly Logger Logger = LogManager.CreateLogger();
        public IBNetClient Client { get; set; }

        // PartyService just uses ChannelService to create a new channel for the party.
        public override void CreateChannel(IRpcController controller, CreateChannelRequest request, Action<CreateChannelResponse> done)
        {
            Logger.Trace("CreateChannel()");
            var channel = ChannelManager.CreateNewChannel((BNetClient)this.Client, request.ObjectId);
            var builder = CreateChannelResponse.CreateBuilder()
                .SetObjectId(channel.DynamicId)
                .SetChannelId(channel.BnetEntityId);

            done(builder.Build());

            // Set the client that requested the creation of channel as the owner
            channel.SetOwner((BNetClient)Client);

            Logger.Warn("Created a new channel {0}:{1} for toon {2}", channel.BnetEntityId.High, channel.BnetEntityId.Low, Client.CurrentToon.Name);
        }

        public override void JoinChannel(IRpcController controller, JoinChannelRequest request, Action<JoinChannelResponse> done)
        {
            throw new NotImplementedException();
        }

        public override void GetChannelInfo(IRpcController controller, GetChannelInfoRequest request, Action<GetChannelInfoResponse> done)
        {
            Logger.Trace("GetChannelInfoRequest()");
            Logger.Warn("Request to channel {0}:{1} by toon {2}", request.ChannelId.High, request.ChannelId.Low, Client.CurrentToon.Name);

            var builder = bnet.protocol.channel.GetChannelInfoResponse.CreateBuilder();
            var channel = ChannelManager.GetChannelByEntityId(request.ChannelId);
            if (channel != null)
                builder.SetChannelInfo(channel.Info);
            else
                Logger.Warn("Channel does not exist!");

            done(builder.Build());
        }
    }
}
