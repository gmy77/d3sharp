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
using D3Sharp.Net.BNet;
using D3Sharp.Utils;
using Google.ProtocolBuffers;
using bnet.protocol;
using bnet.protocol.notification;

namespace D3Sharp.Core.Services
{
    [Service(serviceID: 0xc, serviceName: "bnet.protocol.notification.NotificationService")]
    public class NotificationService : bnet.protocol.notification.NotificationService, IServerService
    {
        protected static readonly Logger Logger = LogManager.CreateLogger();
        public IBNetClient Client { get; set; }

        public override void SendNotification(IRpcController controller, Notification request, Action<NoData> done)
        {
            throw new NotImplementedException();
        }

        public override void RegisterClient(IRpcController controller, RegisterClientRequest request, Action<NoData> done)
        {
            throw new NotImplementedException();
        }

        public override void UnregisterClient(IRpcController controller, UnregisterClientRequest request, Action<NoData> done)
        {
            throw new NotImplementedException();
        }

        public override void FindClient(IRpcController controller, FindClientRequest request, Action<FindClientResponse> done)
        {
            throw new NotImplementedException();
        }
    }
}
