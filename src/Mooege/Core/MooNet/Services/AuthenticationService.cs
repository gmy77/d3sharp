﻿/*
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
using System.Linq;
using Google.ProtocolBuffers;
using Mooege.Common.Logging;
using Mooege.Common.Versions;
using Mooege.Core.MooNet.Authentication;
using Mooege.Core.Cryptography;
using Mooege.Core.MooNet.Online;
using Mooege.Net.MooNet;
using Mooege.Core.MooNet.Accounts;
using Mooege.Common.Extensions;

namespace Mooege.Core.MooNet.Services
{
    [Service(serviceID: 0x1, serviceName: "bnet.protocol.authentication.AuthenticationServer")]
    public class AuthenticationService:bnet.protocol.authentication.AuthenticationServer, IServerService
    {
        private static readonly Logger Logger = LogManager.CreateLogger();
        public MooNetClient Client { get; set; }
        public bnet.protocol.Header LastCallHeader { get; set; }
        public uint Status { get; set; }

        public override void Logon(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.authentication.LogonRequest request, Action<bnet.protocol.NoData> done)
        {
            Logger.Trace("LogonRequest(): Email={0}", request.Email);

            if (!VersionChecker.Check(this.Client, request)) // if the client trying to connect doesn't match required version, disconnect him.
            {
                Logger.Error("Client [{0}] doesn't match required version {1}, disconnecting..", request.Email, VersionInfo.MooNet.RequiredClientVersion);
                this.Client.Connection.Disconnect(); // TODO: We should be actually notifying the client with wrong version message. /raist.
                return;
            }

            done(bnet.protocol.NoData.CreateBuilder().Build());

            AuthManager.StartAuthentication(this.Client, request);
        }

        public override void ModuleMessage(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.authentication.ModuleMessageRequest request, Action<bnet.protocol.NoData> done)
        {
            var moduleMessage = request.Message.ToByteArray();
            var command = moduleMessage[0];
            var module = this.Client.ClientModuleIds.Where(pair => pair.Value == request.ModuleId).Select(pair => pair.Key).First();

            Logger.Trace("ModuleMessage(): Module: {0} ModuleId: {1} Command: {2}", module, request.ModuleId, command);

            done(bnet.protocol.NoData.CreateBuilder().Build());

            switch (module)
            {
                case MooNetClient.StreamedModule.Password:
                    if (command == 2)
                        AuthManager.HandleAuthResponse(this.Client, request.ModuleId, moduleMessage);
                    else
                        Logger.Error("Unknown command: {0} for Password module.", command);
                    break;
                case MooNetClient.StreamedModule.Token:
                    var moduleLoadRequest = bnet.protocol.authentication.ModuleLoadRequest.CreateBuilder()
                        .SetModuleHandle(bnet.protocol.ContentHandle.CreateBuilder()
                            .SetRegion(VersionInfo.MooNet.Regions[VersionInfo.MooNet.Region])
                            .SetUsage(0x61757468) // auth - RiskFingerprint.dll
                            .SetHash(ByteString.CopyFrom(VersionInfo.MooNet.RiskFingerprintHashMap[this.Client.Platform])))
                        .SetMessage(ByteString.Empty)
                        .Build();

                    this.Client.LastRequestedModule = MooNetClient.StreamedModule.RiskFingerprint;
                    this.Client.MakeRPC(() => bnet.protocol.authentication.AuthenticationClient.CreateStub(this.Client).ModuleLoad(null, moduleLoadRequest, ModuleLoadResponse));
                    break;
                case MooNetClient.StreamedModule.RiskFingerprint:
                    Logger.Trace("Completing Authentication.");
                    this.Client.AuthenticationComplete();
                    break;
                default:
                    Logger.Error("Unknown module message data.");
                    break;
            }
        }

        public override void SelectGameAccount(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.EntityId request, Action<bnet.protocol.NoData> done)
        {
            this.Client.Account.CurrentGameAccount = GameAccountManager.GetAccountByPersistentID(request.Low);
            this.Client.Account.CurrentGameAccount.LoggedInClient = this.Client;

            Logger.Trace("SelectGameAccount(): {0}", this.Client.Account.CurrentGameAccount);

            done(bnet.protocol.NoData.CreateBuilder().Build());
        }

        public override void ModuleNotify(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.authentication.ModuleNotification request, Action<bnet.protocol.NoData> done)
        {
            Logger.Trace("ModuleNotify(): Module: {0} ModuleId: {1} Result: {2}", this.Client.LastRequestedModule.ToString(), request.ModuleId, request.Result);

            done(bnet.protocol.NoData.CreateBuilder().Build());
            this.Client.ClientModuleIds[this.Client.LastRequestedModule] = request.ModuleId;

            switch (this.Client.LastRequestedModule)
            {
                case MooNetClient.StreamedModule.Thumbprint:
                    var moduleLoadRequest = bnet.protocol.authentication.ModuleLoadRequest.CreateBuilder()
                        .SetModuleHandle(bnet.protocol.ContentHandle.CreateBuilder()
                        .SetRegion(VersionInfo.MooNet.Regions[VersionInfo.MooNet.Region])
                        .SetUsage(0x61757468) // auth - password.dll
                        .SetHash(ByteString.CopyFrom(VersionInfo.MooNet.PasswordHashMap[this.Client.Platform])))
                        .SetMessage(ByteString.CopyFrom(AuthManager.OngoingAuthentications[this.Client].LogonChallenge))
                        .Build();

                    this.Client.LastRequestedModule = MooNetClient.StreamedModule.Password;
                    this.Client.MakeRPC(() => bnet.protocol.authentication.AuthenticationClient.CreateStub(this.Client).ModuleLoad(null, moduleLoadRequest, ModuleLoadResponse));
                    break;
                case MooNetClient.StreamedModule.Password:
                case MooNetClient.StreamedModule.Token:
                case MooNetClient.StreamedModule.RiskFingerprint:
                    break;
                default:
                    Logger.Error("LastRequestModule: {0}", this.Client.LastRequestedModule.ToString());
                    break;
            }
        }

        private static void ModuleLoadResponse(IMessage response)
        {
            Logger.Trace("ModuleLoadResponse(): {0}", response.ToString());
        }

    }
}
