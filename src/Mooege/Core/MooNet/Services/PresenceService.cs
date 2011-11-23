/*
 * Copyright (C) 2011 mooege project
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
using Mooege.Common.Logging;
using Mooege.Core.MooNet.Accounts;
using Mooege.Core.MooNet.Helpers;
using Mooege.Core.MooNet.Toons;
using Mooege.Net.MooNet;

// TODO: Need to do some more testing and inspection to make sure that
// responding before performing the action requested is proper

namespace Mooege.Core.MooNet.Services
{
    [Service(serviceID: 0xb, serviceName: "bnet.protocol.presence.PresenceService")]
    public class PresenceService : bnet.protocol.presence.PresenceService,IServerService
    {
        private static readonly Logger Logger = LogManager.CreateLogger();
        public MooNetClient Client { get; set; }
        public bnet.protocol.Header LastCallHeader { get; set; }

        public override void Subscribe(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.presence.SubscribeRequest request, System.Action<bnet.protocol.NoData> done)
        {
            switch (request.EntityId.GetHighIdType())
            {
                case EntityIdHelper.HighIdType.AccountId:
                    var account = AccountManager.GetAccountByPersistentID(request.EntityId.Low);
                    if (account != null)
                    {
                        Logger.Trace("Subscribe() {0} {1}", this.Client, account);
                        account.AddSubscriber(this.Client, request.ObjectId);
                    }
                    break;
                case EntityIdHelper.HighIdType.ToonId:
                    var toon = ToonManager.GetToonByLowID(request.EntityId.Low);                    
                    if (toon != null) 
                    {
                        Logger.Trace("Subscribe() {0} {1}", this.Client, toon);
                        toon.AddSubscriber(this.Client, request.ObjectId); // The client will send us a Subscribe with ToonId of 0 the first time it tries to create a toon with a name that already exists. Let's handle that here.
                    }
                    break;
                default:
                    Logger.Warn("Recieved an unhandled Presence.Subscribe request with type {0}", request.EntityId.GetHighIdType());
                    break;
            }

            var builder = bnet.protocol.NoData.CreateBuilder();
            done(builder.Build());
        }

        public override void Unsubscribe(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.presence.UnsubscribeRequest request, System.Action<bnet.protocol.NoData> done)
        {
            Logger.Trace("Unsubscribe()");
            
            switch (request.EntityId.GetHighIdType())
            {
                case EntityIdHelper.HighIdType.AccountId:
                    var account = AccountManager.GetAccountByPersistentID(request.EntityId.Low);
                    // The client will probably make sure it doesn't unsubscribe to a null ID, but just to make sure..
                    if (account != null)
                    {
                        account.RemoveSubscriber(this.Client);
                        Logger.Trace("Unsubscribe() {0} {1}", this.Client, account);
                    }
                    break;
                case EntityIdHelper.HighIdType.ToonId:
                    var toon = ToonManager.GetToonByLowID(request.EntityId.Low);
                    if (toon != null)
                    {
                        toon.RemoveSubscriber(this.Client);
                        Logger.Trace("Unsubscribe() {0} {1}", this.Client, toon);
                    }
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
            //Logger.Warn("request:\n{0}", request.ToString());
            // This "UpdateRequest" is not, as it may seem, a request to update the client on the state of an object,
            // but instead the *client* requesting to change fields on an object that it has subscribed to.
            // Check docs/rpc/presence.txt in branch wip-docs (or master)

            switch (request.EntityId.GetHighIdType())
            {
                case EntityIdHelper.HighIdType.AccountId:
                    var account = AccountManager.GetAccountByPersistentID(request.EntityId.Low);
                    Logger.Trace("Update() {0} {1}", this.Client, account);
                    break;
                case EntityIdHelper.HighIdType.ToonId:
                    var toon = ToonManager.GetToonByLowID(request.EntityId.Low);
                    Logger.Trace("Update() {0} {1}", this.Client, toon);
                    foreach(var fieldOp in request.FieldOperationList)
                    {
                        toon.Update(fieldOp);
                    }
                    break;
                default:
                    Logger.Warn("Recieved an unhandled Presence.Update request with type {0}", request.EntityId.GetHighIdType());
                    break;
            }

            var builder = bnet.protocol.NoData.CreateBuilder();
            done(builder.Build());
        }

        public override void Query(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.presence.QueryRequest request, Action<bnet.protocol.presence.QueryResponse> done)
        {            
            var builder = bnet.protocol.presence.QueryResponse.CreateBuilder();

            switch (request.EntityId.GetHighIdType())
            {
                case EntityIdHelper.HighIdType.AccountId:
                    var account = AccountManager.GetAccountByPersistentID(request.EntityId.Low);
                    Logger.Trace("Query() {0} {1}", this.Client, account);
                    foreach(var key in request.KeyList)
                    {
                        var field = account.QueryField(key);
                        if (field != null) builder.AddField(field);
                    }

                    break;
                case EntityIdHelper.HighIdType.ToonId:
                    var toon = ToonManager.GetToonByLowID(request.EntityId.Low);
                    Logger.Trace("Query() {0} {1}", this.Client, toon);                    
                    foreach (var key in request.KeyList)
                    {
                        var field = toon.QueryField(key);
                        if (field != null) builder.AddField(field);
                    }

                    break;
            }

            done(builder.Build());
        }

        public override void Ownership(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.presence.OwnershipRequest request, Action<bnet.protocol.NoData> done)
        {
            throw new NotImplementedException();
        }

        public override void Heal(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.presence.UpdateRequest request, Action<bnet.protocol.NoData> done)
        {
            throw new NotImplementedException();
        }
    }
}
