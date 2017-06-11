/*
 * Copyright (C) 2011 - 2012 mooege project - http://www.mooege.org
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

using System.Collections.Generic;
using Google.ProtocolBuffers;
using Mooege.Core.GS.Games;
using Mooege.Core.MooNet.Channels;
using Mooege.Core.MooNet.Helpers;
using Mooege.Net.MooNet;
using Config = Mooege.Net.GS.Config;

namespace Mooege.Core.MooNet.Games
{
    public class GameFactory : Channel
    {
        /// <summary>
        /// Ingame manager.
        /// </summary>
        public Game InGame { get; private set; }

        /// <summary>
        /// Game handle.
        /// </summary>
        public bnet.protocol.game_master.GameHandle GameHandle { get; private set; }

        public D3.OnlineService.GameCreateParams GameCreateParams { get; private set; }

        public string Version { get; private set; }

        public ulong FactoryID { get; private set; }

        public ulong RequestId { get; private set; }

        public bool Started { get; private set; }

        public GameFactory(MooNetClient owner, bnet.protocol.game_master.FindGameRequest request, ulong requestId)
            : base(owner, true)
        {
            this.Started = false;
            this.Owner = owner; //Game is really the owner Channel.Owner should maybe be EntityId instead of MooNetClient -Egris
            this.RequestId = requestId;
            this.FactoryID = request.FactoryId;
            this.BnetEntityId = bnet.protocol.EntityId.CreateBuilder().SetHigh((ulong)EntityIdHelper.HighIdType.GameId).SetLow(this.DynamicId).Build();
            this.GameHandle = bnet.protocol.game_master.GameHandle.CreateBuilder().SetFactoryId(this.FactoryID).SetGameId(this.BnetEntityId).Build();

            foreach (bnet.protocol.attribute.Attribute attribute in request.Properties.CreationAttributesList)
            {
                if (attribute.Name != "GameCreateParams")
                    Logger.Warn("FindGame(): Unknown CreationAttribute: {0}", attribute.Name);
                else
                    this.GameCreateParams = D3.OnlineService.GameCreateParams.ParseFrom(attribute.Value.MessageValue);
            }

            foreach (bnet.protocol.attribute.Attribute attribute in request.Properties.Filter.AttributeList)
            {
                if (attribute.Name != "version")
                    Logger.Warn("FindGame(): Unknown Attribute: {0}", attribute.Name);
                else
                    this.Version = attribute.Value.StringValue;
            }
        }

        public void StartGame(List<MooNetClient> clients, ulong objectId)
        {
            this.InGame = GameManager.CreateGame((int)this.DynamicId); // create the ingame.

            foreach (var client in clients) // get all clients in game.
            {
                client.MapLocalObjectID(this.DynamicId, objectId); // map remote object-id.
                this.SendConnectionInfo(client);
                client.Account.CurrentGameAccount.ScreenStatus = D3.PartyMessage.ScreenStatus.CreateBuilder().SetScreen(0).SetStatus(0).Build();
                client.Account.CurrentGameAccount.NotifyUpdate();
            }

            this.Started = true;
        }

        public void JoinGame(List<MooNetClient> clients, ulong objectId)
        {
            foreach (var client in clients) // get all clients in game.
            {
                client.MapLocalObjectID(this.DynamicId, objectId); // map remote object-id.
                this.SendConnectionInfo(client);
            }
        }

        public bnet.protocol.game_master.ConnectInfo GetConnectionInfoForClient(MooNetClient client)
        {
            //TODO: We should actually find the server's public-interface and use that /raist

            return bnet.protocol.game_master.ConnectInfo.CreateBuilder()
                .SetMemberId(client.Account.CurrentGameAccount.BnetEntityId)
                .SetHost(Net.Utils.GetGameServerIPForClient(client))
                .SetPort(Config.Instance.Port)
                .SetToken(ByteString.CopyFrom(new byte[] { 0x31, 0x33, 0x38, 0x38, 0x35, 0x34, 0x33, 0x33, 0x32, 0x30, 0x38, 0x34, 0x30, 0x30, 0x38, 0x38, 0x35, 0x37, 0x39, 0x36 }))
                .AddAttribute(bnet.protocol.attribute.Attribute.CreateBuilder()
                    .SetName("SGameId").SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetIntValue((long)this.DynamicId).Build()))
                .Build();
        }

        private void SendConnectionInfo(MooNetClient client)
        {
            // Lock party and close privacy level while entering game
            if (client.CurrentChannel != null)
            {
                var channelStatePrivacyLevel = bnet.protocol.channel.ChannelState.CreateBuilder()
                    .SetPrivacyLevel(bnet.protocol.channel.ChannelState.Types.PrivacyLevel.PRIVACY_LEVEL_CLOSED).Build();

                var notificationPrivacyLevel = bnet.protocol.channel.UpdateChannelStateNotification.CreateBuilder()
                    .SetAgentId(client.Account.CurrentGameAccount.BnetEntityId)
                    .SetStateChange(channelStatePrivacyLevel)
                    .Build();

                client.MakeTargetedRPC(client.CurrentChannel, () =>
                    bnet.protocol.channel.ChannelSubscriber.CreateStub(client).NotifyUpdateChannelState(null, notificationPrivacyLevel, callback => { }));

                var channelStatePartyLock = bnet.protocol.channel.ChannelState.CreateBuilder()
                    .AddAttribute(bnet.protocol.attribute.Attribute.CreateBuilder()
                    .SetName("D3.Party.LockReasons")
                    .SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetIntValue(1).Build())
                    .Build()).Build();

                var notificationPartyLock = bnet.protocol.channel.UpdateChannelStateNotification.CreateBuilder()
                    .SetAgentId(client.Account.CurrentGameAccount.BnetEntityId)
                    .SetStateChange(channelStatePartyLock)
                    .Build();

                client.MakeTargetedRPC(client.CurrentChannel, () =>
                    bnet.protocol.channel.ChannelSubscriber.CreateStub(client).NotifyUpdateChannelState(null, notificationPartyLock, callback => { }));
            }

            // send the notification.
            var connectionInfo = GetConnectionInfoForClient(client);

            var connectionInfoAttribute = bnet.protocol.attribute.Attribute.CreateBuilder().SetName("connection_info")
                .SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetMessageValue(connectionInfo.ToByteString()).Build())
                .Build();

            var gameHandleAttribute = bnet.protocol.attribute.Attribute.CreateBuilder().SetName("game_handle")
                .SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetMessageValue(this.GameHandle.ToByteString()).Build())
                .Build();

            var requestIdAttribute = bnet.protocol.attribute.Attribute.CreateBuilder().SetName("game_request_id")
                .SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetUintValue(this.RequestId).Build())
                .Build();

            var notificationBuilder = bnet.protocol.notification.Notification.CreateBuilder()
                .SetSenderId(bnet.protocol.EntityId.CreateBuilder().SetHigh((ulong)EntityIdHelper.HighIdType.GameAccountId).SetLow(0).Build())
                .SetTargetId(client.Account.CurrentGameAccount.BnetEntityId)
                .SetType("GAME_ENTRY")
                .AddAttribute(connectionInfoAttribute)
                .AddAttribute(gameHandleAttribute)
                .AddAttribute(requestIdAttribute)
                .Build();

            client.MakeRPC(() =>
                bnet.protocol.notification.NotificationListener.CreateStub(client).OnNotificationReceived(null, notificationBuilder, callback => { }));
        }
    }
}
