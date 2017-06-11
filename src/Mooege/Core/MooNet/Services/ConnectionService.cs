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
using System.Collections.Generic;
using System.Linq;
using Google.ProtocolBuffers;
using Mooege.Common.Versions;
using Mooege.Common.Extensions;
using Mooege.Common.Logging;
using Mooege.Net.MooNet;

namespace Mooege.Core.MooNet.Services
{
    [Service(serviceID: 0x0, serviceHash: 0x0)]
    public class ConnectionService : bnet.protocol.connection.ConnectionService, IServerService
    {
        private static readonly Logger Logger = LogManager.CreateLogger();
        public MooNetClient Client { get; set; }
        public bnet.protocol.Header LastCallHeader { get; set; }
        public uint Status { get; set; }

        public override void Connect(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.connection.ConnectRequest request, Action<bnet.protocol.connection.ConnectResponse> done)
        {
            Logger.Trace("Connect()");

            var builder = bnet.protocol.connection.ConnectResponse.CreateBuilder()
                .SetServerId(bnet.protocol.ProcessId.CreateBuilder().SetLabel(0).SetEpoch(DateTime.Now.ToUnixTime()))
                .SetClientId(bnet.protocol.ProcessId.CreateBuilder().SetLabel(1).SetEpoch(DateTime.Now.ToUnixTime()));

            if (request.HasClientId)
                builder.SetClientId(request.ClientId);

            builder.SetContentHandleArray(bnet.protocol.connection.ConnectionMeteringContentHandles.CreateBuilder()
                .AddContentHandle(bnet.protocol.ContentHandle.CreateBuilder()
                    .SetRegion(VersionInfo.MooNet.Regions[VersionInfo.MooNet.Region])
                    .SetUsage(0x6D74727A) //mtrz
                    .SetHash(ByteString.CopyFrom("acaeab71f005567974a656cf1207f74bb9a5365c84e9f22f1f82ffec3d1367a8".ToByteArray()))));

            done(builder.Build());
        }

        public override void Bind(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.connection.BindRequest request, Action<bnet.protocol.connection.BindResponse> done)
        {
            Logger.Trace("Bind(): {0}", this.Client);

            var requestedServiceIDs = new List<uint>();

            foreach (var serviceHash in request.ImportedServiceHashList)
            {
                var serviceID = Service.GetByHash(serviceHash);
                requestedServiceIDs.Add(serviceID);

                Logger.Trace("[export] Hash: 0x{0} Id: 0x{1} Service: {2} ", serviceHash.ToString("X8"), serviceID.ToString("X2"), Service.GetByID(serviceID) != null ? Service.GetByID(serviceID).GetType().Name : "N/A");
            }

            // read services supplied by client..
            foreach (var service in request.ExportedServiceList.Where(service => !Client.Services.ContainsValue(service.Id)))
            {
                if (Client.Services.ContainsKey(service.Hash)) continue;
                Client.Services.Add(service.Hash, service.Id);

                Logger.Trace(string.Format("[import] Hash: 0x{0} Id: 0x{1}", service.Hash.ToString("X8"), service.Id.ToString("X2")));
            }

            var builder = bnet.protocol.connection.BindResponse.CreateBuilder();
            foreach (var serviceId in requestedServiceIDs) builder.AddImportedServiceId(serviceId);

            done(builder.Build());
        }

        public override void Echo(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.connection.EchoRequest request, Action<bnet.protocol.connection.EchoResponse> done)
        {
            throw new NotImplementedException();
        }

        public override void Encrypt(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.connection.EncryptRequest request, Action<bnet.protocol.NoData> done)
        {
            throw new NotImplementedException();
        }

        public override void ForceDisconnect(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.connection.DisconnectNotification request, Action<bnet.protocol.NO_RESPONSE> done)
        {
            throw new NotImplementedException();
        }

        public override void Null(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.connection.NullRequest request, Action<bnet.protocol.NO_RESPONSE> done)
        {
            throw new NotImplementedException();
        }

        public override void RequestDisconnect(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.connection.DisconnectRequest request, Action<bnet.protocol.NO_RESPONSE> done)
        {
            Logger.Trace("RequestDisconnect()");
            if (this.Client.Account != null)
            {
                Accounts.AccountManager.SaveToDB(this.Client.Account);
                if (this.Client.Account.CurrentGameAccount != null)
                {
                    Accounts.GameAccountManager.SaveToDB(this.Client.Account.CurrentGameAccount);
                    this.Client.Account.CurrentGameAccount.LoggedInClient.Connection.Disconnect();
                }
            }
        }
    }
}
