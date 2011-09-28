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

using D3Sharp.Core.BNet.Accounts;
using D3Sharp.Core.Common.Toons;
using D3Sharp.Net.BNet;
using D3Sharp.Utils;
using D3Sharp.Core.Helpers;
using bnet.protocol;

// TODO: Need to do some more testing and inspection to make sure that
// responding before performing the action requested is proper

namespace D3Sharp.Core.BNet.Services
{
    [Service(serviceID: 0xb, serviceName: "bnet.protocol.presence.PresenceService")]
    public class PresenceService : bnet.protocol.presence.PresenceService,IServerService
    {
        private static readonly Logger Logger = LogManager.CreateLogger();
        public IBNetClient Client { get; set; }

        public override void Subscribe(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.presence.SubscribeRequest request, System.Action<bnet.protocol.NoData> done)
        {
            Logger.Trace("Subscribe() {0}: {1}", request.EntityId.GetHighIdType(), request.EntityId.Low);

            switch (request.EntityId.GetHighIdType())
            {
                case EntityIdHelper.HighIdType.AccountId:
                    this.Client.Account.AddSubscriber((BNetClient)this.Client, request.ObjectId);
                    break;
                case EntityIdHelper.HighIdType.ToonId:
                    var toon = ToonManager.GetToonByLowID(request.EntityId.Low);
                    // The client will send us a Subscribe with ToonId of 0 the first time it
                    // tries to create a toon with a name that already exists. Let's handle that here.
                    if (toon != null)
                        toon.AddSubscriber((BNetClient)this.Client, request.ObjectId);
                    break;
                default:
                    Logger.Warn("Recieved an unhandled Presence.Subscribe request with type {0}", request.EntityId.GetHighIdType());
                    break;
            }

            var builder = NoData.CreateBuilder();
            done(builder.Build());
        }

        public override void Unsubscribe(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.presence.UnsubscribeRequest request, System.Action<bnet.protocol.NoData> done)
        {
            Logger.Trace("Unsubscribe()");
            Logger.Debug("request:\n{0}", request.ToString());
            
            switch (request.EntityId.GetHighIdType())
            {
                case EntityIdHelper.HighIdType.AccountId:
                    var account = AccountManager.GetAccountByEntityID(request.EntityId);
                    // The client will probably make sure it doesn't unsubscribe to a null ID, but just to make sure..
                    if (account != null)
                        account.RemoveSubscriber((BNetClient)this.Client);
                    break;
                case EntityIdHelper.HighIdType.ToonId:
                    var toon = ToonManager.GetToonByLowID(request.EntityId.Low);
                    if (toon != null)
                        toon.RemoveSubscriber((BNetClient)this.Client);
                    break;
                default:
                    Logger.Warn("Recieved an unhandled Presence.Unsubscribe request with type {0}", request.EntityId.GetHighIdType());
                    break;
            }
            
            var builder = bnet.protocol.NoData.CreateBuilder();
            done(builder.Build());
        }

        public override void Update(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.presence.UpdateRequest request, System.Action<bnet.protocol.NoData> done)
        {
            Logger.Trace("Update() {0}: {1}", request.EntityId.GetHighIdType(), request.EntityId.Low);
            //Logger.Warn("request:\n{0}", request.ToString());
            // This "UpdateRequest" is not, as it may seem, a request to update the client on the state of an object,
            // but instead the *client* requesting to change fields on an object that it has subscribed to.
            // Check docs/rpc/presence.txt in branch wip-docs (or master)

            switch (request.EntityId.GetHighIdType())
            {
                case EntityIdHelper.HighIdType.AccountId:
                    var account = AccountManager.GetAccountByEntityID(request.EntityId);
                    break;
                case EntityIdHelper.HighIdType.ToonId:
                    var toon = ToonManager.GetToonByLowID(request.EntityId.Low);                    
                    break;
                default:
                    Logger.Warn("Recieved an unhandled Presence.Update request with type {0}", request.EntityId.GetHighIdType());
                    break;
            }

            var builder = NoData.CreateBuilder();
            done(builder.Build());
        }

        public override void Query(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.presence.QueryRequest request, System.Action<bnet.protocol.presence.QueryResponse> done)
        {
            Logger.Trace("Query() {0}: {1}", request.EntityId.GetHighIdType(), request.EntityId.Low);

            var builder = bnet.protocol.presence.QueryResponse.CreateBuilder();
            var toon = ToonManager.GetToonByLowID(request.EntityId.Low);                       

            foreach(var key in request.KeyList)
            {
                var field = toon.QueryField(key);
                if (field != null) builder.AddField(field);
            }

            done(builder.Build());
        }               
    }
}
