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
using System.Linq;
using Google.ProtocolBuffers;
using Mooege.Common.Logging;
using Mooege.Common.Versions;
using Mooege.Core.Cryptography;
using Mooege.Core.MooNet.Accounts;
using Mooege.Net.MooNet;

namespace Mooege.Core.MooNet.Authentication
{
    public static class AuthManager
    {
        private static readonly Logger Logger = LogManager.CreateLogger();

        private static readonly Dictionary<MooNetClient, SRP6a> OngoingAuthentications = new Dictionary<MooNetClient, SRP6a>();

        public static void StartAuthentication(MooNetClient client, bnet.protocol.authentication.LogonRequest request)
        {
            InitAuthentication(client, request);
        }

        private static void InitAuthentication(MooNetClient client, bnet.protocol.authentication.LogonRequest request)
        {
            var account = AccountManager.GetAccountByEmail(request.Email.ToLower()); // check if account exists.
            
            if (account == null) // we should be returning an error to client /raist.
            {
                client.AuthenticationErrorCode = AuthenticationErrorCodes.NoGameAccount;
                client.AuthenticationCompleteSignal.Set();
                return;
            }

            var srp6a = new SRP6a(account); // create srp6 handler to process the authentication.
            OngoingAuthentications.Add(client, srp6a);

            // request client to load password.dll for authentication.
            var moduleLoadRequest = bnet.protocol.authentication.ModuleLoadRequest.CreateBuilder()
                .SetModuleHandle(bnet.protocol.ContentHandle.CreateBuilder()
                    .SetRegion(0x00005553) // us
                    .SetUsage(0x61757468) // auth - password.dll
                    .SetHash(ByteString.CopyFrom(VersionInfo.MooNet.AuthModuleHashMap[client.Platform])))
                .SetMessage(ByteString.CopyFrom(srp6a.LogonChallenge))
                .Build();

            client.MakeRPCWithListenerId(request.ListenerId, () =>
                bnet.protocol.authentication.AuthenticationClient.CreateStub(client).ModuleLoad(null, moduleLoadRequest, ModuleLoadResponse));
        }

        public static void HandleAuthResponse(MooNetClient client, int moduleId, byte[] authMessage)
        {
            if(!OngoingAuthentications.ContainsKey(client)) return; // TODO: disconnect him also. /raist.

            var srp6 = OngoingAuthentications[client];
            byte[] A = authMessage.Skip(1).Take(128).ToArray(); // client's public ephemeral
            byte[] M_client = authMessage.Skip(1 + 128).Take(32).ToArray(); // client's proof of session key.
            byte[] seed = authMessage.Skip(1 + 32 + 128).Take(128).ToArray(); // client's second challenge.

            var success = srp6.Verify(A, M_client, seed);
            if (Config.Instance.DisablePasswordChecks || success) // if authentication is sucesseful or password check's are disabled.
            {
                // send the logon proof.
                var message = bnet.protocol.authentication.ModuleMessageRequest.CreateBuilder()
                    .SetModuleId(moduleId)
                    .SetMessage(ByteString.CopyFrom(srp6.LogonProof))
                    .Build();

                client.MakeRPC(() =>
                    bnet.protocol.authentication.AuthenticationClient.CreateStub(client).ModuleMessage(null, message, callback => { }));

                client.Account = AccountManager.GetAccountByEmail(srp6.Account.Email);
                //if (client.Account.LoggedInClient != null)
                //    client.Account.LoggedInClient.Connection.Disconnect();
                //client.Account.LoggedInClient = client;
            }
            else // authentication failed because of invalid credentals.
            {
                client.AuthenticationErrorCode = AuthenticationErrorCodes.InvalidCredentials;
            }
             
            OngoingAuthentications.Remove(client);
            client.AuthenticationCompleteSignal.Set(); // signal about completion of authentication processes so we can return the response for AuthenticationService:LogonRequest.
        }

        private static void ModuleLoadResponse(IMessage response)
        {
            Logger.Trace("ModuleLoadResponse(): {0}", response.ToString());
        }

        /// <summary>
        /// Error codes for authentication process.
        /// </summary>
        public enum AuthenticationErrorCodes
        {
            None = 0,
            InvalidCredentials = 3,
            NoToonSelected = 11,
            NoGameAccount = 12,
        }
    }
}
