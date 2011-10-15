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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Google.ProtocolBuffers;
using Mooege.Common;
using Mooege.Common.Extensions;
using Mooege.Net.MooNet;

namespace Mooege.Core.MooNet.Authentication
{
    public static class AuthManager
    {
        private static readonly Logger Logger = LogManager.CreateLogger();
        private static readonly Dictionary<MooNetClient, SRP6> OngoingAuthentications = new Dictionary<MooNetClient, SRP6>();
        private static readonly byte[] ModuleHash = "8F52906A2C85B416A595702251570F96D3522F39237603115F2F1AB24962043C".ToByteArray(); // Password.dll

        public static void StartAuthentication(MooNetClient client, bnet.protocol.authentication.LogonRequest request)
        {
            var srp6 = new SRP6(request.Email, "123");
            OngoingAuthentications.Add(client, srp6);

            var moduleLoadRequest = bnet.protocol.authentication.ModuleLoadRequest.CreateBuilder()
                .SetModuleHandle(bnet.protocol.ContentHandle.CreateBuilder()
                                     .SetRegion(0x00005553) // us
                                     .SetUsage(0x61757468) // auth - password.dll
                                     .SetHash(ByteString.CopyFrom(ModuleHash)))
                .SetMessage(ByteString.CopyFrom(srp6.LogonChallenge))
                .Build();

            client.MakeRPCWithListenerId(request.ListenerId, () =>
                bnet.protocol.authentication.AuthenticationClient.CreateStub(client).ModuleLoad(null, moduleLoadRequest, ModuleLoadResponse));
        }

        public static void HandleAuthResponse(MooNetClient client, int moduleId, byte[] authMessage)
        {
            if(!OngoingAuthentications.ContainsKey(client)) return; // TODO: disconnect him also. /raist.

            var srp6 = OngoingAuthentications[client];

            byte[] A = authMessage.Skip(1).Take(128).ToArray();
            byte[] M1 = authMessage.Skip(1 + 128).Take(32).ToArray();
            byte[] seed = authMessage.Skip(1 + 32 + 128).Take(128).ToArray();

            if(srp6.Verify(A,M1,seed))
            {
                bnet.protocol.authentication.ModuleMessageRequest.CreateBuilder()
                    .SetModuleId(moduleId)
                    .SetMessage(ByteString.CopyFrom(srp6.LogonProof)).Build();
            }

        }

        private static void ModuleLoadResponse(bnet.protocol.authentication.ModuleLoadResponse response)
        {
            Logger.Trace("ModuleLoadResponse {0}", response.ToString());
        }
    }
}
