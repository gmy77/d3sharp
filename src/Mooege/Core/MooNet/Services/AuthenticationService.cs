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
using System.Globalization;
using System.Threading;
using Mooege.Common;
using Mooege.Core.MooNet.Authentication;
using Mooege.Core.MooNet.Online;
using Mooege.Net.MooNet;

namespace Mooege.Core.MooNet.Services
{
    [Service(serviceID: 0x1, serviceName: "bnet.protocol.authentication.AuthenticationServer")]
    public class AuthenticationService:bnet.protocol.authentication.AuthenticationServer, IServerService
    {
        private static readonly Logger Logger = LogManager.CreateLogger();
        public MooNetClient Client { get; set; }
        
        public override void Logon(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.authentication.LogonRequest request, Action<bnet.protocol.authentication.LogonResponse> done)
        {
            Logger.Trace("LogonRequest(); Email={0}", request.Email);

            // we should be also checking here version, program, locale and similar stuff /raist.

            AuthManager.StartAuthentication(this.Client, request);

            var authenticationThread = new Thread(() =>
            {
                this.Client.AuthenticationCompleteSignal.WaitOne(); // wait the signal;

                if(this.Client.AuthenticationErrorCode != MooNetClient.AuthenticationErrorCodes.None)
                {
                    Logger.Info("Authentication failed for {0} because of invalid credentals.", request.Email);
                    done(bnet.protocol.authentication.LogonResponse.DefaultInstance);
                    return;
                }

                Logger.Info("User {0} authenticated successfuly.", request.Email);
                var builder = bnet.protocol.authentication.LogonResponse.
                    CreateBuilder()
                    .SetAccount(Client.Account.BnetAccountID)
                    .SetGameAccount(Client.Account.BnetGameAccountID);

                done(builder.Build());

                PlayerManager.PlayerConnected(this.Client);

            }) { IsBackground = true, CurrentCulture = CultureInfo.InvariantCulture }; ;

            authenticationThread.Start();
        }

        public override void ModuleMessage(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.authentication.ModuleMessageRequest request, Action<bnet.protocol.NoData> done)
        {
            var moduleMessage = request.Message.ToByteArray();
            var command = moduleMessage[0];

            done(bnet.protocol.NoData.CreateBuilder().Build());

            if(request.ModuleId==0 && command==2)
                AuthManager.HandleAuthResponse(this.Client, request.ModuleId, moduleMessage);
        }
    }
}
