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
using Google.ProtocolBuffers;
using Mooege.Common;
using Mooege.Common.Extensions;
using Mooege.Core.MooNet.Accounts;
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

        private readonly byte[] _moduleHash = "8F52906A2C85B416A595702251570F96D3522F39237603115F2F1AB24962043C".ToByteArray(); // Password.dll
        private SRP6 _srp6;

        public override void Logon(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.authentication.LogonRequest request, Action<bnet.protocol.authentication.LogonResponse> done)
        {
            Logger.Trace("LogonRequest(); Email={0}", request.Email);

            // we should be also checking here version, program, locale and similar stuff /raist.

            this._srp6 = new SRP6(request.Email, "123");

            var moduleLoadRequest = bnet.protocol.authentication.ModuleLoadRequest.CreateBuilder()
                .SetModuleHandle(bnet.protocol.ContentHandle.CreateBuilder()
                    .SetRegion(0x00005553) // us
                    .SetUsage(0x61757468) // auth - password.dll
                    .SetHash(ByteString.CopyFrom(_moduleHash)))
                    .SetMessage(ByteString.CopyFrom(this._srp6.LogonChallenge))
                    .Build();

            this.Client.MakeRPCWithListenerId(request.ListenerId, () =>
                bnet.protocol.authentication.AuthenticationClient.CreateStub(this.Client).ModuleLoad(controller, moduleLoadRequest, ModuleLoadResponse));

            //this.Client.ListenerId = request.ListenerId;
            //bnet.protocol.authentication.AuthenticationClient.CreateStub(this.Client).ModuleLoad(controller, moduleLoadRequest, ModuleLoadRequestCallback);

            //var account = AccountManager.GetAccountByEmail(request.Email) ?? AccountManager.CreateAccount(request.Email); // add a config option that sets this functionality, ie AllowAccountCreationOnFirstLogin.

            //Client.Account = account;
            //Client.Account.LoggedInClient = Client;

            //var builder = bnet.protocol.authentication.LogonResponse.CreateBuilder()
            //    .SetAccount(Client.Account.BnetAccountID)
            //    .SetGameAccount(Client.Account.BnetGameAccountID);

            //done(builder.Build());

            //PlayerManager.PlayerConnected(this.Client);
        }

        public override void ModuleMessage(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.authentication.ModuleMessageRequest request, Action<bnet.protocol.NoData> done)
        {
            throw new NotImplementedException();
        }

        private static void ModuleLoadResponse(bnet.protocol.authentication.ModuleLoadResponse response)
        {
            Logger.Trace("ModuleLoadResponse {0}", response.ToString());
        }
    }
}
