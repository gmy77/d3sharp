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

using System.Collections.Generic;
using System.Linq;
using Google.ProtocolBuffers;
using Mooege.Common;
using Mooege.Common.Extensions;
using Mooege.Core.MooNet.Accounts;
using Mooege.Net.MooNet;

namespace Mooege.Core.MooNet.Authentication
{
    public static class AuthManager
    {
        private static readonly Logger Logger = LogManager.CreateLogger();
        private static readonly Dictionary<MooNetClient, SRP6a> OngoingAuthentications = new Dictionary<MooNetClient, SRP6a>();
        private static readonly byte[] ModuleHash = "8F52906A2C85B416A595702251570F96D3522F39237603115F2F1AB24962043C".ToByteArray(); // Password.dll

        public static void StartAuthentication(MooNetClient client, bnet.protocol.authentication.LogonRequest request)
        {
            if (Config.Instance.BypassAuthentication)
                ByPassAuthentication(client, request);
            else
                InitAuthentication(client, request);                                               
        }

        private static void ByPassAuthentication(MooNetClient client, bnet.protocol.authentication.LogonRequest request)
        {
            client.Account = AccountManager.GetAccountByEmail(request.Email) ?? AccountManager.CreateAccount(request.Email, "123");
            client.Account.LoggedInClient = client;

            client.AuthenticationCompleteSignal.Set();
        }

        private static void InitAuthentication(MooNetClient client, bnet.protocol.authentication.LogonRequest request)
        {
            var account = AccountManager.GetAccountByEmail(request.Email); // check if account exists.
            if (account == null)
            {
                client.AuthenticationErrorCode = AuthenticationErrorCode.NoGameAccount;
                client.AuthenticationCompleteSignal.Set();
            }

            var srp6a = new SRP6a(account);
            OngoingAuthentications.Add(client, srp6a);

            var moduleLoadRequest = bnet.protocol.authentication.ModuleLoadRequest.CreateBuilder()
                .SetModuleHandle(bnet.protocol.ContentHandle.CreateBuilder()
                                     .SetRegion(0x00005553) // us
                                     .SetUsage(0x61757468) // auth - password.dll
                                     .SetHash(ByteString.CopyFrom(ModuleHash)))
                .SetMessage(ByteString.CopyFrom(srp6a.LogonChallenge))
                .Build();

            client.MakeRPCWithListenerId(request.ListenerId, () =>
                bnet.protocol.authentication.AuthenticationClient.CreateStub(client).ModuleLoad(null, moduleLoadRequest, ModuleLoadResponse));
        }

        public static void HandleAuthResponse(MooNetClient client, int moduleId, byte[] authMessage)
        {
            if(!OngoingAuthentications.ContainsKey(client)) return; // TODO: disconnect him also. /raist.

            var srp6 = OngoingAuthentications[client];

            byte[] A = authMessage.Skip(1).Take(128).ToArray();
            byte[] M_client = authMessage.Skip(1 + 128).Take(32).ToArray();
            byte[] seed = authMessage.Skip(1 + 32 + 128).Take(128).ToArray();

            if(srp6.Verify(A,M_client,seed))
            {
                var message = bnet.protocol.authentication.ModuleMessageRequest.CreateBuilder()
                    .SetModuleId(moduleId)
                    .SetMessage(ByteString.CopyFrom(srp6.LogonProof))
                    .Build();

                client.MakeRPC(() =>
                    bnet.protocol.authentication.AuthenticationClient.CreateStub(client).ModuleMessage(null, message, callback => { }));

                client.Account = AccountManager.GetAccountByEmail(srp6.Account.Email);
                client.Account.LoggedInClient = client;
            }
            else
            {
                client.AuthenticationErrorCode = AuthenticationErrorCode.InvalidCredentials;
            }

            OngoingAuthentications.Remove(client);
            client.AuthenticationCompleteSignal.Set();
        }

        private static void ModuleLoadResponse(IMessage response)
        {
            Logger.Trace("{0}", response.ToString());
        }
    }
}
