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
using System.Globalization;
using System.Threading;
using Mooege.Common.Logging;
using Mooege.Common.Versions;
using Mooege.Core.MooNet.Authentication;
using Mooege.Core.MooNet.Online;
using Mooege.Net.MooNet;
using Mooege.Core.MooNet.Accounts;

namespace Mooege.Core.MooNet.Services
{
    [Service(serviceID: 0x1, serviceName: "bnet.protocol.authentication.AuthenticationServer")]
    public class AuthenticationService:bnet.protocol.authentication.AuthenticationServer, IServerService
    {
        private static readonly Logger Logger = LogManager.CreateLogger();
        public MooNetClient Client { get; set; }
        public bnet.protocol.Header LastCallHeader { get; set; }
        
        public override void Logon(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.authentication.LogonRequest request, Action<bnet.protocol.authentication.LogonResponse> done)
        {
            Logger.Trace("LogonRequest(): Email={0}", request.Email);

            if (!VersionChecker.Check(this.Client, request)) // if the client trying to connect doesn't match required version, disconnect him.
            {
                Logger.Error("Client [{0}] doesn't match required version {1}, disconnecting..", request.Email, VersionInfo.MooNet.RequiredClientVersion);
                this.Client.Connection.Disconnect(); // TODO: We should be actually notifying the client with wrong version message. /raist.
                return;
            }

            AuthManager.StartAuthentication(this.Client, request);

            var authenticationThread = new Thread(() =>
            {
                this.Client.AuthenticationCompleteSignal.WaitOne(); // wait the signal;

                if(this.Client.AuthenticationErrorCode != AuthManager.AuthenticationErrorCodes.None)
                {
                    Logger.Info("Authentication failed for {0} because of invalid credentals.", request.Email);
                    done(bnet.protocol.authentication.LogonResponse.DefaultInstance);
                    return;
                }

                Logger.Info("User {0} authenticated successfuly.", request.Email);
                var logonResponseBuilder = bnet.protocol.authentication.LogonResponse.CreateBuilder()
                    .SetAccount(this.Client.Account.BnetEntityId);

                foreach (var gameAccount in this.Client.Account.GameAccounts.Values)
                {
                    logonResponseBuilder.AddGameAccount(gameAccount.BnetEntityId);
                }

                done(logonResponseBuilder.Build());

                this.Client.EnableEncryption();

                PlayerManager.PlayerConnected(this.Client);

            }) { IsBackground = true, CurrentCulture = CultureInfo.InvariantCulture }; ;

            authenticationThread.Start();
        }

        public override void ModuleMessage(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.authentication.ModuleMessageRequest request, Action<bnet.protocol.NoData> done)
        {
            var moduleMessage = request.Message.ToByteArray();
            var command = moduleMessage[0];

            Logger.Trace("ModuleMessage(): command: {0}", command);

            done(bnet.protocol.NoData.CreateBuilder().Build());

            if(request.ModuleId==0 && command==2)
                AuthManager.HandleAuthResponse(this.Client, request.ModuleId, moduleMessage);
        }

        public override void SelectGameAccount(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.EntityId request, Action<bnet.protocol.NoData> done)
        {
            this.Client.Account.CurrentGameAccount = GameAccountManager.GetAccountByPersistentID(request.Low);
            this.Client.Account.CurrentGameAccount.LoggedInClient = this.Client;

            Logger.Trace("SelectGameAccount(): {0}", this.Client.Account.CurrentGameAccount);

            done(bnet.protocol.NoData.CreateBuilder().Build());

        }
    }
}
