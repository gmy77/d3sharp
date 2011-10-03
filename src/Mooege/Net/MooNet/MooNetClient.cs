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
using System.Collections.Generic;
using Google.ProtocolBuffers;
using Google.ProtocolBuffers.Descriptors;
using Mooege.Common;
using Mooege.Common.Helpers;
using Mooege.Core.Common.Toons;
using Mooege.Core.MooNet.Accounts;
using Mooege.Core.MooNet.Channels;
using Mooege.Net.GS;
using Mooege.Net.MooNet.Packets;

namespace Mooege.Net.MooNet
{
    public sealed class MooNetClient : IMooNetClient 
    {
        private static readonly Logger Logger = LogManager.CreateLogger();
        public Dictionary<uint, uint> Services { get; private set; }
        public Account Account { get; set; }
        private int _requestCounter = 0;

        public Toon CurrentToon { get; set; }
        public Channel CurrentChannel { get; set; }
        public GameClient InGameClient { get; set; }

        public IConnection Connection { get; set; }

        /// <summary>
        /// Object ID map with local object ID as key and remote object ID as value.
        /// </summary>
        private Dictionary<ulong, ulong> MappedObjects { get; set; }

        public MooNetClient(IConnection connection)
        {
            this.Connection = connection;
            this.Services = new Dictionary<uint, uint>();
            this.MappedObjects = new Dictionary<ulong, ulong>();
        }

        // rpc to client
        public void CallMethod(MethodDescriptor method, IMessage request)
        {
            CallMethod(method, request, 0);
        }

        public void CallMethod(MethodDescriptor method, IMessage request, ulong localObjectId)
        {
            var serviceName = method.Service.FullName;
            var serviceHash = StringHashHelper.HashIdentity(serviceName);

            if (!this.Services.ContainsKey(serviceHash))
            {
                Logger.Error("Not bound to client service {0} [0x{1}] yet.", serviceName, serviceHash.ToString("X8"));
                return;
            }

            var serviceId = this.Services[serviceHash];
            var remoteObjectId = GetRemoteObjectID(localObjectId);

            Logger.Trace("Calling {0} localObjectId={1}, remoteObjectId={2}", method.FullName, localObjectId, remoteObjectId);

            var packet = new Packet(
                new Header((byte)serviceId, (uint)(method.Index + 1), this._requestCounter++, (uint)request.SerializedSize, remoteObjectId),
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

        public void UnmapLocalObjectID(ulong localObjectId)
        {
            try
            {
                this.MappedObjects.Remove(localObjectId);
            }
            catch (Exception e)
            {
                Logger.DebugException(e, "UnmapLocalObjectID()");
            }
        }

        public ulong GetRemoteObjectID(ulong localObjectId)
        {
            if (localObjectId == 0)
                return 0; // null/unused/unset
            else
                return this.MappedObjects[localObjectId];
        }
    }
}
