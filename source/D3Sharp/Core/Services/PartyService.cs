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
using D3Sharp.Core.Channels;
using D3Sharp.Net.BNet;
using D3Sharp.Utils;
using Google.ProtocolBuffers;
using bnet.protocol.channel;

namespace D3Sharp.Core.Services
{
    [Service(serviceID: 0x0D, serviceName: "bnet.protocol.party.PartyService")]
    public class PartyService : bnet.protocol.party.PartyService,IServerService
    {
        protected static readonly Logger Logger = LogManager.CreateLogger();
        public IBNetClient Client { get; set; }

        public override void CreateChannel(IRpcController controller, CreateChannelRequest request, Action<CreateChannelResponse> done)
        {
            Logger.Trace("CreateChannel()");
            //Logger.Debug("request:\n{0}", request.ToString());
            
            var channel = ChannelsManager.CreateNewChannel();
            // This is an object creator, so we have to map the remote object ID
            this.Client.MapLocalObjectID(channel.LocalObjectId, request.ObjectId);
            var builder = CreateChannelResponse.CreateBuilder()
                .SetObjectId(channel.LocalObjectId)
                .SetChannelId(channel.BnetEntityID);

            done(builder.Build());

            channel.Add((BNetClient)this.Client);
            channel.NotifyChannelState((BNetClient)this.Client);
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
