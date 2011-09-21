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
using System.Collections.Generic;
using D3Sharp.Core.Accounts;
using D3Sharp.Core.Toons;
using D3Sharp.Net.BNet;
using D3Sharp.Utils;
using D3Sharp.Core.Helpers;
using bnet.protocol;

namespace D3Sharp.Core.Services
{
    [Service(serviceID: 0xb, serviceName: "bnet.protocol.presence.PresenceService")]
    public class PresenceService : bnet.protocol.presence.PresenceService,IServerService
    {
        protected static readonly Logger Logger = LogManager.CreateLogger();
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
                    toon.AddSubscriber((BNetClient)this.Client, request.ObjectId);
                    break;
                default:
                    Logger.Warn("Recieved an unhandled Presence:Subscribe request with {0}", request.EntityId.GetHighIdType());
                    break;
            }

            var builder = NoData.CreateBuilder();
            done(builder.Build());
        }

        public override void Unsubscribe(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.presence.UnsubscribeRequest request, System.Action<bnet.protocol.NoData> done)
        {
            Logger.Trace("Unsubscribe()");
            //Logger.Debug("request:\n{0}", request.ToString());
            var builder = bnet.protocol.NoData.CreateBuilder();
            done(builder.Build());
        }

        public override void Update(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.presence.UpdateRequest request, System.Action<bnet.protocol.NoData> done)
        {
            Logger.Trace("Update() {0}: {1}", request.EntityId.GetHighIdType(), request.EntityId.Low);

            // Here the client either asks for an 'update' on some toon related objects or toon itself.
            // It maybe also setting a value on toon, like his away statys??! /raist
            // Check presence.update.request on docs.
            // We should implement it when we completely figure it out.

            switch (request.EntityId.GetHighIdType())
            {
                case EntityIdHelper.HighIdType.AccountId:
                    var account = AccountManager.GetAccountByEntityID(request.EntityId);
                    break;
                case EntityIdHelper.HighIdType.ToonId:
                    var toon = ToonManager.GetToonByLowID(request.EntityId.Low);                    
                    break;
                default:
                    Logger.Warn("Recieved an unhandled Presence:Update request with {0}", request.EntityId.GetHighIdType());
                    break;
            }

            var builder = NoData.CreateBuilder();
            done(builder.Build());
        }

        public override void Query(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.presence.QueryRequest request, System.Action<bnet.protocol.presence.QueryResponse> done)
        {
            throw new System.NotImplementedException();
        }               
    }
}
