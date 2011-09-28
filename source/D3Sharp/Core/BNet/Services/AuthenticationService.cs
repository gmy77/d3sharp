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
using D3Sharp.Core.BNet.Accounts;
using D3Sharp.Net.BNet;
using D3Sharp.Utils;
using bnet.protocol.authentication;

namespace D3Sharp.Core.BNet.Services
{
    [Service(serviceID: 0x1, serviceName: "bnet.protocol.authentication.AuthenticationServer")]
    public class AuthenticationService:AuthenticationServer, IServerService
    {
        private static readonly Logger Logger = LogManager.CreateLogger();
        public IBNetClient Client { get; set; }

        public override void Logon(Google.ProtocolBuffers.IRpcController controller, LogonRequest request, System.Action<LogonResponse> done)
        {
            Logger.Trace("LogonRequest(); Email={0}", request.Email);
            Client.Account = AccountManager.GetAccountByEmail(request.Email);
            Client.Account.LoggedInBNetClient = (BNetClient)Client;

            var builder = bnet.protocol.authentication.LogonResponse.CreateBuilder()
                .SetAccount(Client.Account.BnetAccountID)
                .SetGameAccount(Client.Account.BnetGameAccountID);

            done(builder.Build());

            OnlinePlayers.Players.Add((BNetClient)Client);
        }

        public override void ModuleMessage(Google.ProtocolBuffers.IRpcController controller, ModuleMessageRequest request, System.Action<bnet.protocol.NoData> done)
        {
            throw new NotImplementedException();
        }
    }
}
