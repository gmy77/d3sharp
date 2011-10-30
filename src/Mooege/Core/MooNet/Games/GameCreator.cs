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

using System.Linq;
using System.Collections.Generic;
using Google.ProtocolBuffers;
using Mooege.Core.MooNet.Channels;
using Mooege.Core.MooNet.Helpers;
using Mooege.Core.MooNet.Objects;
using Mooege.Net.MooNet;
using Config = Mooege.Net.GS.Config;

namespace Mooege.Core.MooNet.Games
{
    public class GameCreator : RPCObject
    {
        /// <summary>
        /// bnet.protocol.EntityId encoded Id.
        /// </summary>
        public bnet.protocol.EntityId BnetEntityId { get; private set; }

        /// <summary>
        /// The channel bound to game.
        /// </summary>
        public Channel Channel { get; private set; }

        public Core.GS.Game.Game InGame { get; private set; }

        /// <summary>
        /// Game handle.
        /// </summary>
        public bnet.protocol.game_master.GameHandle GameHandle { get; private set; }

        public D3.OnlineService.GameCreateParams GameCreateParams { get; private set; }

        public string Version { get; private set; }

        public ulong FactoryID { get; private set; }

        public ulong RequestId { get; set; }

        public static ulong RequestIdCounter = 0;

        public GameCreator(Channel channel)
        {
            this.Channel = channel;

            this.FactoryID = 14249086168335147635;
            this.BnetEntityId = bnet.protocol.EntityId.CreateBuilder().SetHigh((ulong)EntityIdHelper.HighIdType.GameId).SetLow(this.DynamicId).Build();
            this.GameHandle = bnet.protocol.game_master.GameHandle.CreateBuilder().SetFactoryId(this.FactoryID).SetGameId(this.BnetEntityId).Build();
        }

        public void StartGame(List<MooNetClient> clients, ulong objectId, D3.OnlineService.GameCreateParams gameCreateParams, string version)
        {
            this.GameCreateParams = gameCreateParams;
            this.Version = version;
            this.InGame = GS.Game.GameManager.CreateGame((int) this.DynamicId); // create the ingame.

            clients.First().MapLocalObjectID(this.DynamicId, objectId); // map remote object-id for party leader.

            foreach(var client in clients) // get all clients in game.
            {
                this.SendConnectionInfo(client);
            }
        }

        public void JoinGame(List<MooNetClient> clients, ulong objectId)
        {
            //Seems to work whether the line below is there or not.  Commenting out as it is easier to detect if something is completely missing vs wrong... /dustinconrad
            //clients.First().MapLocalObjectID(this.DynamicId, objectId);

            foreach (var client in clients) // get all clients in game.
            {
                this.SendConnectionInfo(client);
            }
        }

        private bnet.protocol.game_master.ConnectInfo GetConnectionInfoForClient(MooNetClient client)
        {
            //TODO: We should actually find the server's public-interface and use that /raist
            return bnet.protocol.game_master.ConnectInfo.CreateBuilder().SetToonId(client.CurrentToon.BnetEntityID)
                .SetHost(Net.Utils.GetGameServerIPForClient(client)).SetPort(Config.Instance.Port).SetToken(ByteString.CopyFrom(new byte[] { 0x07, 0x34, 0x02, 0x60, 0x91, 0x93, 0x76, 0x46, 0x28, 0x84 }))
                .AddAttribute(bnet.protocol.attribute.Attribute.CreateBuilder().SetName("SGameId").SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetIntValue((long)this.DynamicId).Build()))
                .Build();
        }

        private void SendConnectionInfo(MooNetClient client)
        {
            if (client == this.Channel.Owner) // we should send a GameFoundNotification to part leader
            {
                var builder = bnet.protocol.game_master.GameFoundNotification.CreateBuilder();
                builder.AddConnectInfo(GetConnectionInfoForClient(client));
                builder.SetRequestId(this.RequestId);
                builder.SetGameHandle(this.GameHandle);

                client.MakeTargetedRPC(this, () =>
                    bnet.protocol.game_master.GameFactorySubscriber.CreateStub(client).NotifyGameFound(null, builder.Build(), callback => { }));
            }
            else // where as other members should get a bnet.protocol.notification.Notification
            {
                var connectionInfo = GetConnectionInfoForClient(client);

                var connectionInfoAttribute =
                    bnet.protocol.attribute.Attribute.CreateBuilder().SetName("connection_info")
                        .SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetMessageValue(connectionInfo.ToByteString()).Build())
                        .Build();

                var gameHandleAttribute =
                    bnet.protocol.attribute.Attribute.CreateBuilder().SetName("game_handle")
                        .SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetMessageValue(this.GameHandle.ToByteString()).Build())
                        .Build();

                var builder = bnet.protocol.notification.Notification.CreateBuilder()
                    .SetSenderId(this.Channel.Owner.CurrentToon.BnetEntityID)
                    .SetTargetId(client.CurrentToon.BnetEntityID)
                    .SetType("GAME_CONNECTION_INFO")
                    .AddAttribute(connectionInfoAttribute)
                    .AddAttribute(gameHandleAttribute);

                client.MakeRPC(() => bnet.protocol.notification.NotificationListener.CreateStub(client).OnNotificationReceived(null, builder.Build(), callback => { }));
            }
        }
    }
}
