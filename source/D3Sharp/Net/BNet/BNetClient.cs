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
using D3Sharp.Core.Channels;
using D3Sharp.Core.Toons;
using D3Sharp.Net.BNet.Packets;
using D3Sharp.Utils;
using D3Sharp.Utils.Helpers;
using Google.ProtocolBuffers;
using Google.ProtocolBuffers.Descriptors;

namespace D3Sharp.Net.BNet
{
    public sealed class BNetClient : IBNetClient 
    {
        private static readonly Logger Logger = LogManager.CreateLogger();
        public Dictionary<uint, uint> Services { get; set; }
        public Account Account { get; set; }
        private int _requestCounter = 0;

        public Toon CurrentToon { get; set; }
        public Channel CurrentChannel { get; set; }

        public IConnection Connection { get; set; }

        /// <summary>
        /// Object id map as with remote object id as key, local object id as value.
        /// </summary>
        private Dictionary<ulong, ulong> MappedObjects { get; set; }

        public BNetClient(IConnection connection)
        {
            this.Connection = connection;
            this.Services = new Dictionary<uint, uint>();
            this.MappedObjects = new Dictionary<ulong, ulong>();
        }

        // rpc to client
        public void CallMethod(MethodDescriptor method, IMessage request)
        {
            Logger.Debug("CallMethod with 0 localObjectId");
            CallMethod(method, request, 0);
        }

        public void CallMethod(MethodDescriptor method, IMessage request, ulong localObjectId)
        {
            var serviceName = method.Service.FullName;
            var serviceHash = StringHashHelper.HashString(serviceName);

            if (!this.Services.ContainsKey(serviceHash))
            {
                Logger.Error("Not bound to client service {0} [0x{1}] yet.", serviceName, serviceHash.ToString("X8"));
                return;
            }

            var serviceId = this.Services[serviceHash];
            var remoteObjectId = GetRemoteObjectID(localObjectId);

            Logger.Debug("Calling {0} localObjectId={1}, remoteObjectId={2}", method.FullName, localObjectId, remoteObjectId);

            var packet = new BNetPacket(
                new BNetHeader((byte)serviceId, (uint)(method.Index + 1), this._requestCounter++, (uint)request.SerializedSize, remoteObjectId),
                request.ToByteArray());

            this.Connection.Send(packet);
        }

        public bnet.protocol.Identity GetIdentity(bool acct, bool gameacct, bool toon)
        {
            var identityBuilder = bnet.protocol.Identity.CreateBuilder();
            if (acct) identityBuilder.SetAccountId(this.Account.BnetAccountID);
            if (gameacct) identityBuilder.SetGameAccountId(this.Account.BnetGameAccountID);
            if (toon && this.CurrentToon != null)
                identityBuilder.SetToonId(this.CurrentToon.BnetEntityID);
            return identityBuilder.Build();
        }

        public void MapLocalObjectID(ulong localObjectId, ulong remoteObjectId)
        {
            try
            {
                this.MappedObjects[localObjectId] = remoteObjectId;
            }
            catch (Exception e)
            {
                Logger.DebugException(e, "MapLocalObjectID()");
            }
        }

        public ulong GetRemoteObjectID(ulong localObjectId)
        {
            // TODO: Check for conflicts
            // TODO: Handle exceptions-- should be fatal?
            //       Maybe we can not bother once ID tracker/generator is in
            if (localObjectId == 0)
                return 0; // null/unused/unset
            else
                return this.MappedObjects[localObjectId];
        }
    }
}
