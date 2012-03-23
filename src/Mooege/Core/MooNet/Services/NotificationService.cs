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

using System;
using Mooege.Common.Logging;
using Mooege.Core.MooNet.Commands;
using Mooege.Core.MooNet.Helpers;
using Mooege.Core.MooNet.Accounts;
using Mooege.Net.MooNet;

namespace Mooege.Core.MooNet.Services
{
    [Service(serviceID: 0xc, serviceName: "bnet.protocol.notification.NotificationService")]
    public class NotificationService : bnet.protocol.notification.NotificationService, IServerService
    {
        private static readonly Logger Logger = LogManager.CreateLogger();
        public MooNetClient Client { get; set; }
        public bnet.protocol.Header LastCallHeader { get; set; }
        public uint Status { get; set; }

        public override void SendNotification(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.notification.Notification request, Action<bnet.protocol.NoData> done)
        {
            switch (request.GetNotificationType())
            {
                case NotificationTypeHelper.NotificationType.Whisper:

                    // NOTE: Real implementation doesn't even handle the situation where neither client knows about the other.
                    // Client requires prior knowledge of sender and target (and even then it cannot whisper by using the /whisper command).

                    var targetAccount = GameAccountManager.GetAccountByPersistentID(request.TargetId.Low);
                    Logger.Trace(string.Format("NotificationRequest.Whisper by {0} to {1}", this.Client.Account.CurrentGameAccount, targetAccount));

                    if (targetAccount.LoggedInClient == null) return;

                    if (targetAccount == this.Client.Account.CurrentGameAccount) // check if whisper targets the account itself.
                        CommandManager.TryParse(request.AttributeList[0].Value.StringValue, this.Client); // try parsing it as a command and respond it if so.
                    else
                    {
                        var notification = bnet.protocol.notification.Notification.CreateBuilder(request)
                            .SetSenderId(this.Client.Account.CurrentGameAccount.BnetEntityId)
                            .SetSenderAccountId(this.Client.Account.BnetEntityId)
                            .Build();

                        targetAccount.LoggedInClient.MakeRPC(() =>
                            bnet.protocol.notification.NotificationListener.CreateStub(targetAccount.LoggedInClient).OnNotificationReceived(controller, notification, callback => { }));
                    }
                    break;
                default:
                    Logger.Warn("Unhandled notification type: {0}", request.Type);
                    break;
            }

            var builder = bnet.protocol.NoData.CreateBuilder();
            done(builder.Build());
        }

        public override void RegisterClient(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.notification.RegisterClientRequest request, Action<bnet.protocol.NoData> done)
        {
            throw new NotImplementedException();
        }

        public override void UnregisterClient(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.notification.UnregisterClientRequest request, Action<bnet.protocol.NoData> done)
        {
            throw new NotImplementedException();
        }

        public override void FindClient(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.notification.FindClientRequest request, Action<bnet.protocol.notification.FindClientResponse> done)
        {
            throw new NotImplementedException();
        }
    }
}
