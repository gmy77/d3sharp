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
    public class AuthenticationService : bnet.protocol.authentication.AuthenticationServer, IServerService
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

                // create the disconnection reason.
                var reason = bnet.protocol.connection.DisconnectNotification.CreateBuilder()
                    .SetErrorCode(3018).Build();

                // Error 3018 => A new patch for Diablo III is available. The game will now close and apply the patch automatically. You will be able to continue playing after the patch has been applied.

                // FIXME: D3 client somehow doesn't show the correct error message yet, and in debug output we only miss a message like [ Recv ] service_id: 254 token: 6 status: 28 
                // when I compare mooege's output. That could be the reason. /raist.

                // force disconnect the client as it does not satisfy required version. /raist.
                this.Client.MakeRPC(() => bnet.protocol.connection.ConnectionService.CreateStub(this.Client).ForceDisconnect(null, reason, callback => { }));
                this.Client.Connection.Disconnect();

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

                    var moduleLoadRequest = bnet.protocol.authentication.ModuleLoadRequest.CreateBuilder();
                    var moduleHandle = bnet.protocol.ContentHandle.CreateBuilder();
                    moduleHandle.SetRegion(VersionInfo.MooNet.Regions[VersionInfo.MooNet.Region])
                        .SetUsage(0x61757468); // auth - RiskFingerprint.dll or Agreement.dll

                    if (this.Client.HasAgreements())
                    {
                        moduleHandle.SetHash(ByteString.CopyFrom(VersionInfo.MooNet.AgreementHashMap[this.Client.Platform]));
                        this.Client.LastRequestedModule = MooNetClient.StreamedModule.Agreement;
                    }
                    else
                    {
                        moduleHandle.SetHash(ByteString.CopyFrom(VersionInfo.MooNet.RiskFingerprintHashMap[this.Client.Platform]));
                        moduleLoadRequest.SetMessage(ByteString.Empty);
                        this.Client.LastRequestedModule = MooNetClient.StreamedModule.RiskFingerprint;
                    }

                    moduleLoadRequest.SetModuleHandle(moduleHandle);
                    this.Client.MakeRPC(() => bnet.protocol.authentication.AuthenticationClient.CreateStub(this.Client).ModuleLoad(null, moduleLoadRequest.Build(), ModuleLoadResponse));
                    break;
                case MooNetClient.StreamedModule.RiskFingerprint:
                    Logger.Trace("Completing Authentication.");
                    this.Client.AuthenticationComplete();
                    break;
                case MooNetClient.StreamedModule.Agreement:
                    switch (this.Client.LastAgreementSent)
                    {
                        case MooNetClient.AvailableAgreements.EULA:
                            this.Client.Agreements.Add(MooNetClient.AvailableAgreements.EULA, true);
                            break;
                        case MooNetClient.AvailableAgreements.TOS:
                            this.Client.Agreements.Add(MooNetClient.AvailableAgreements.TOS, true);
                            break;
                        case MooNetClient.AvailableAgreements.RMAH:
                            this.Client.Agreements.Add(MooNetClient.AvailableAgreements.RMAH, true);
                            break;
                        default:
                            Logger.Error("Unknown agreement.");
                            break;
                    }
                    this.Client.SendAgreements();
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
            var moduleLoadRequest = bnet.protocol.authentication.ModuleLoadRequest.CreateBuilder();
            switch (this.Client.LastRequestedModule)
            {
                case MooNetClient.StreamedModule.Thumbprint:
                    moduleLoadRequest.SetModuleHandle(bnet.protocol.ContentHandle.CreateBuilder()
                        .SetRegion(VersionInfo.MooNet.Regions[VersionInfo.MooNet.Region])
                        .SetUsage(0x61757468) // auth - password.dll
                        .SetHash(ByteString.CopyFrom(VersionInfo.MooNet.PasswordHashMap[this.Client.Platform])))
                        .SetMessage(ByteString.CopyFrom(AuthManager.OngoingAuthentications[this.Client].LogonChallenge));

                    this.Client.LastRequestedModule = MooNetClient.StreamedModule.Password;
                    this.Client.MakeRPC(() => bnet.protocol.authentication.AuthenticationClient.CreateStub(this.Client).ModuleLoad(null, moduleLoadRequest.Build(), ModuleLoadResponse));
                    break;
                case MooNetClient.StreamedModule.Agreement:
                    if (this.Client.HasAgreements())
                    {
                        this.Client.SendAgreements();
                    }
                    else
                    {
                        moduleLoadRequest.SetModuleHandle(bnet.protocol.ContentHandle.CreateBuilder()
                            .SetRegion(VersionInfo.MooNet.Regions[VersionInfo.MooNet.Region])
                            .SetUsage(0x61757468) // auth - RiskFingerprint.dll
                            .SetHash(ByteString.CopyFrom(VersionInfo.MooNet.RiskFingerprintHashMap[this.Client.Platform])))
                            .SetMessage(ByteString.Empty);

                        this.Client.LastRequestedModule = MooNetClient.StreamedModule.RiskFingerprint;
                        this.Client.MakeRPC(() => bnet.protocol.authentication.AuthenticationClient.CreateStub(this.Client).ModuleLoad(null, moduleLoadRequest.Build(), ModuleLoadResponse));
                    }
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
