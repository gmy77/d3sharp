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

using System.Collections.Generic;
using D3Sharp.Core.BNet.Objects;
using D3Sharp.Core.Helpers;
using D3Sharp.Net.BNet;
using Google.ProtocolBuffers;

namespace D3Sharp.Core.BNet.Games
{
    public class Game : RPCObject
    {
        /// <summary>
        /// Bnet EntityID encoded ID.
        /// </summary>
        public bnet.protocol.EntityId BnetEntityId { get; private set; }

        public bnet.protocol.game_master.GameHandle GameHandle { get; private set; }

        public ulong RequestID { get; private set; }
        public ulong FactoryID { get; private set; }

        public List<BNetClient> Clients = new List<BNetClient>();

        public static ulong RequestIdCounter = 0;

        public Game(ulong factoryId)
        {
            this.RequestID = ++RequestIdCounter;
            this.FactoryID = factoryId;

            this.BnetEntityId = bnet.protocol.EntityId.CreateBuilder().SetHigh((ulong)EntityIdHelper.HighIdType.GameId).SetLow(this.DynamicId).Build();
            this.GameHandle = bnet.protocol.game_master.GameHandle.CreateBuilder().SetFactoryId(this.FactoryID).SetGameId(this.BnetEntityId).Build();
        }

        public void ListenForGame(BNetClient client)
        {
            // We should actually find the server's public-interface and use that
            var connectionInfo =
                bnet.protocol.game_master.ConnectInfo.CreateBuilder().SetToonId(client.CurrentToon.BnetEntityID).SetHost
                    (Net.Utils.GetGameServerIPForClient(client)).SetPort(Net.Game.Config.Instance.Port).SetToken(ByteString.CopyFrom(new byte[] {0x07, 0x34, 0x02, 0x60, 0x91, 0x93, 0x76, 0x46, 0x28, 0x84}))
                    .AddAttribute(bnet.protocol.attribute.Attribute.CreateBuilder().SetName("SGameId").SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetIntValue((long)this.DynamicId).Build())).Build();
                 
            var builder = bnet.protocol.game_master.GameFoundNotification.CreateBuilder();
            builder.AddConnectInfo(connectionInfo);
            builder.SetRequestId(this.RequestID);
            builder.SetGameHandle(this.GameHandle);

            this.Clients.Add(client);
            client.CallMethod(bnet.protocol.game_master.GameFactorySubscriber.Descriptor.FindMethodByName("NotifyGameFound"), builder.Build(), this.DynamicId);
        }
    }
}
