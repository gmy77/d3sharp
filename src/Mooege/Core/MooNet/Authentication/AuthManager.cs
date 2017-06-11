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
using Mooege.Common.Extensions;

namespace Mooege.Core.MooNet.Authentication
{
    public static class AuthManager
    {
        private static readonly Logger Logger = LogManager.CreateLogger();

        public static readonly Dictionary<MooNetClient, SRP6a> OngoingAuthentications = new Dictionary<MooNetClient, SRP6a>();

        public static void StartAuthentication(MooNetClient client, bnet.protocol.authentication.LogonRequest request)
        {
            InitAuthentication(client, request);
        }

        private static void InitAuthentication(MooNetClient client, bnet.protocol.authentication.LogonRequest request)
        {
            client.LoginEmail = request.Email;
            var account = AccountManager.GetAccountByEmail(request.Email.ToLower()); // check if account exists.

            if (account == null) // we should be returning an error to client /raist.
            {
                client.AuthenticationErrorCode = AuthenticationErrorCodes.NoGameAccount;
                client.AuthenticationComplete();
                return;
            }

            var thumbprintData = "f9513183031b3836103ac3a3bb606c0e06fd3b94ae4a6b6e405844085b794e901b0ebb1db650b85bac4b489a38a1ca9dcef2bbd13445d0cd85accfc62d84bc3a8d960b1a7a65cd8d3f72a172f41dca98459015ffbd25d766b02824a42dacb7c4cd64f4b3b9e316de23ec1dbb0153b73a4fa58ffb39c3f484b4b478a660dc8979e16e52f978a0ca2fc4184fa0f69844d73ef99f47e3ccb02cf6a636b4ae9513404eee7e0ad536dcef50cd1699e38195e6afdd3655a3a3529b4e52b33ac5d04f5fa2b15536a2c782c89c0acf133e14eac15af035abf5e44e9e1124a3397dac8f90a4ccb1717540698869fc1ba9037e099d68698ecf17ffdd36f07013176be24269fda1e5d221708181d95474fc1d74bb901062a9f6a3e24aef79e9d583d9126796d63c153a0f75f02da27ecc0971f39a46ec29087c3ae474e08fdf8f65d1445b293399bc495ff651b7d2d7a36216ba5e4400ea7bbc884dc4cf3ed27f14501b8bb7fc0f86b5089880e6889bcd851153e299a337d6c945f710559595e351995341cbef44abac379cf0f845b362d294eec390d50f1a50089d250eae1b1205cea1aff514de076516f467cd077a1fbb759b415dc6c0ea1617f31f7d764a0d60d5b67aa82b4202b2a9455eb9ca3683955ec45aaf56aba42a2f3ae9be5f3eed093a6601816d00e0569bfcb91fb7843945336c99757812d373d510d25744f9480b6cc87e8a".ToByteArray();

            var srp6a = new SRP6a(account); // create srp6 handler to process the authentication.
            OngoingAuthentications.Add(client, srp6a);

            // request client to load thumbprint.dll for authentication.
            var moduleLoadRequest = bnet.protocol.authentication.ModuleLoadRequest.CreateBuilder()
                .SetModuleHandle(bnet.protocol.ContentHandle.CreateBuilder()
                    .SetRegion(VersionInfo.MooNet.Regions[VersionInfo.MooNet.Region])
                    .SetUsage(0x61757468) // auth - thumbprint.dll
                    .SetHash(ByteString.CopyFrom(VersionInfo.MooNet.ThumbprintHashMap[client.Platform])))
                .SetMessage(ByteString.CopyFrom(thumbprintData))
                .Build();

            client.LastRequestedModule = MooNetClient.StreamedModule.Thumbprint;
            client.MakeRPC(() => bnet.protocol.authentication.AuthenticationClient.CreateStub(client).ModuleLoad(null, moduleLoadRequest, callback => { }));
        }

        public static void HandleAuthResponse(MooNetClient client, int moduleId, byte[] authMessage)
        {
            if (!OngoingAuthentications.ContainsKey(client)) return; // TODO: disconnect him also. /raist.

            var srp6 = OngoingAuthentications[client];
            byte[] A = authMessage.Skip(1).Take(128).ToArray(); // client's public ephemeral
            byte[] M_client = authMessage.Skip(1 + 128).Take(32).ToArray(); // client's proof of session key.
            byte[] seed = authMessage.Skip(1 + 32 + 128).Take(128).ToArray(); // client's second challenge.

            var success = srp6.Verify(A, M_client, seed);
            //if (Config.Instance.DisablePasswordChecks || success)
            if (success)
            {
                client.SessionKey = srp6.SessionKey;
                // send the logon proof.
                var message = bnet.protocol.authentication.ModuleMessageRequest.CreateBuilder()
                    .SetModuleId(moduleId)
                    .SetMessage(ByteString.CopyFrom(srp6.LogonProof))
                    .Build();

                client.MakeRPC(() =>
                    bnet.protocol.authentication.AuthenticationClient.CreateStub(client).ModuleMessage(null, message, callback => client.CheckAuthenticator()));

                client.Account = AccountManager.GetAccountByEmail(srp6.Account.Email);
                //if (client.Account.LoggedInClient != null)
                //    client.Account.LoggedInClient.Connection.Disconnect();
                //client.Account.LoggedInClient = client;
            }
            else // authentication failed because of invalid credentals.
            {
                client.AuthenticationErrorCode = AuthenticationErrorCodes.InvalidCredentials;
                //end authentication
                client.AuthenticationComplete();
            }

            OngoingAuthentications.Remove(client);
        }

        public static void SendAccountSettings(MooNetClient client)
        {
            var accset = new bnet.protocol.authentication.AccountSettingsNotification.Builder();
            accset.AddLicenses(new bnet.protocol.account.AccountLicense.Builder().SetId(111));
            accset.AddLicenses(new bnet.protocol.account.AccountLicense.Builder().SetId(227));
            accset.AddLicenses(new bnet.protocol.account.AccountLicense.Builder().SetId(168)); //Full Game - Removes upgrade banner
            client.MakeRPC(() => bnet.protocol.authentication.AuthenticationClient.CreateStub(client).AccountSettings(null, accset.Build(), delegate(bnet.protocol.NO_RESPONSE a) { }));
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
