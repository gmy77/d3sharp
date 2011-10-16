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
using System.Threading;
using Google.ProtocolBuffers;
using Google.ProtocolBuffers.Descriptors;
using Mooege.Common;
using Mooege.Common.Helpers;
using Mooege.Core.Common.Toons;
using Mooege.Core.MooNet.Accounts;
using Mooege.Core.MooNet.Channels;
using Mooege.Core.MooNet.Objects;
using Mooege.Net.GS;
using Mooege.Net.MooNet.Packets;
using Mooege.Net.MooNet.RPC;

namespace Mooege.Net.MooNet
{
    public sealed class MooNetClient : IClient, IRpcChannel
    {
        private static readonly Logger Logger = LogManager.CreateLogger();

        /// <summary>
        /// TCP connection.
        /// </summary>
        public IConnection Connection { get; set; }

        /// <summary>
        /// Logged in gs client if any.
        /// </summary>
        public GameClient InGameClient { get; set; }

        /// <summary>
        /// Account for logged in client.
        /// </summary>
        public Account Account { get; set; }

        /// <summary>
        /// Selected toon for current account.
        /// </summary>
        public Toon CurrentToon { get; set; }

        /// <summary>
        /// Client exported services dictionary.
        /// </summary>
        public Dictionary<uint, uint> Services { get; private set; }

        /// <summary>
        /// Allows AuthenticationService.LogonResponse to be post-poned until authentication process is done.
        /// </summary>
        public readonly AutoResetEvent AuthenticationCompleteSignal = new AutoResetEvent(false);

        /// <summary>
        /// Resulting error code for the authentication process.
        /// </summary>
        public AuthenticationErrorCodes AuthenticationErrorCode;

        /// <summary>
        /// Callback list for issued client RPCs.
        /// </summary>
        public readonly Queue<RPCCallback> RPCCallbacks = new Queue<RPCCallback>();
        
        /// <summary>
        /// Object ID map with local object ID as key and remote object ID as value.
        /// </summary>
        private Dictionary<ulong, ulong> MappedObjects { get; set; }

        /// <summary>
        /// Request counter for RPCs.
        /// </summary>
        private int _requestCounter = 0;
        
        /// <summary>
        /// Listener Id for upcoming rpc.
        /// </summary>
        private ulong _listenerId; // last targeted rpc object.
        
        public MooNetClient(IConnection connection)
        {
            this.Connection = connection;            
            this.Services = new Dictionary<uint, uint>();
            this.MappedObjects = new Dictionary<ulong, ulong>();
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

        #region rpc-call mechanism

        /// <summary>
        /// Allows you target an RPCObject while issuing a RPC.
        /// </summary>
        /// <param name="targetObject"><see cref="RPCObject"/></param>
        /// <param name="rpc">The rpc action.</param>
        public void MakeTargetedRPC(RPCObject targetObject, Action rpc)
        {
            this._listenerId = this.GetRemoteObjectID(targetObject.DynamicId);
            Logger.Warn("[RPC] Targeted object: {0} [localId: {1}, remoteId: {2}].", targetObject.ToString(),
                         targetObject.DynamicId, this._listenerId);

            rpc();
        }

        /// <summary>
        /// Allows you target an listener directly while issuing a RPC.
        /// </summary>
        /// <param name="listenerId">The listenerId over client.</param>
        /// <param name="rpc">The rpc action.</param>
        public void MakeRPCWithListenerId(ulong listenerId, Action rpc)
        {
            this._listenerId = listenerId;
            Logger.Trace("[RPC] Targeted listenerId: {0}.", this._listenerId);

            rpc();
        }

        /// <summary>
        /// Allows you to issue an RPC without targeting any RPCObject/Listener.
        /// </summary>
        /// <param name="rpc">The rpc action.</param>
        public void MakeRPC(Action rpc)
        {
            this._listenerId = 0;
            Logger.Trace("[RPC] with no targets.");
            rpc();
        }

        /// <summary>
        /// Makes an RPC over remote client.
        /// </summary>
        /// <param name="method">The method to call.</param>
        /// <param name="controller">The rpc controller.</param>
        /// <param name="request">The request message.</param>
        /// <param name="responsePrototype">The response message.</param>
        /// <param name="done">Action to run when client responds RPC.</param>
        public void CallMethod(MethodDescriptor method, IRpcController controller, IMessage request, IMessage responsePrototype, Action<IMessage> done)
        {
            var serviceName = method.Service.FullName;
            var serviceHash = StringHashHelper.HashIdentity(serviceName);

            if (!this.Services.ContainsKey(serviceHash))
            {
                Logger.Error("Not bound to client service {0} [0x{1}] yet.", serviceName, serviceHash.ToString("X8"));
                return;
            }

            var serviceId = this.Services[serviceHash];
            var requestId = this._requestCounter++;

            RPCCallbacks.Enqueue(new RPCCallback(done, responsePrototype.WeakToBuilder(), requestId));

            var packet = new PacketOut((byte)serviceId, MooNetRouter.GetMethodId(method), requestId, this._listenerId, request);
            this.Connection.Send(packet);
        }

        #endregion

        #region object-mapping mechanism for rpc calls

        /// <summary>
        /// Maps a given local objectId to remote one over client.
        /// </summary>
        /// <param name="localObjectId">The local objectId.</param>
        /// <param name="remoteObjectId">The remote objectId over client.</param>
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

        /// <summary>
        /// Unmaps an existing local objectId.
        /// </summary>
        /// <param name="localObjectId"></param>
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

        /// <summary>
        /// Returns the remote objectId for given localObjectId.
        /// </summary>
        /// <param name="localObjectId">The local objectId</param>
        /// <returns>The remoteobjectId</returns>
        public ulong GetRemoteObjectID(ulong localObjectId)
        {
            return localObjectId != 0 ? this.MappedObjects[localObjectId] : 0;
        }

        #endregion

        private Channel _currentChannel;
        public Channel CurrentChannel
        {
            get
            {
                return _currentChannel;
            }
            set
            {
                this._currentChannel = value;
                if (value == null) return;

                #region still trying to figure a bit below - commented meanwhile /raist. 
                // notify friends.
                //if (FriendManager.Friends[this.Account.BnetAccountID.Low].Count == 0) return; // if account has no friends just skip.

                //var fieldKey = FieldKeyHelper.Create(FieldKeyHelper.Program.D3, 4, 1, 0);
                //var field = bnet.protocol.presence.Field.CreateBuilder().SetKey(fieldKey).SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetMessageValue(value.D3EntityId.ToByteString()).Build()).Build();
                //var operation = bnet.protocol.presence.FieldOperation.CreateBuilder().SetField(field).Build();

                //var state = bnet.protocol.presence.ChannelState.CreateBuilder().SetEntityId(this.Account.BnetAccountID).AddFieldOperation(operation).Build();
                //var channelState = bnet.protocol.channel.ChannelState.CreateBuilder().SetExtension(bnet.protocol.presence.ChannelState.Presence, state);
                //var notification = bnet.protocol.channel.UpdateChannelStateNotification.CreateBuilder().SetStateChange(channelState).Build();

                //foreach (var friend in FriendManager.Friends[this.Account.BnetAccountID.Low])
                //{
                //    var account = AccountManager.GetAccountByPersistentID(friend.Id.Low);
                //    if (account == null || account.LoggedInClient == null) return; // only send to friends that are online.

                //    account.LoggedInClient.CallMethod(
                //        bnet.protocol.channel.ChannelSubscriber.Descriptor.FindMethodByName("NotifyUpdateChannelState"),
                //        notification, this.Account.DynamicId);
                //}
                #endregion
            }
        }

        /// <summary>
        /// Error codes for authentication process.
        /// </summary>
        public enum AuthenticationErrorCodes
        {
            None = 0,
            InvalidCredentials = 3,
            NoToonSelected = 11,
            NoGameAccount = 12,
        }
    }
}
