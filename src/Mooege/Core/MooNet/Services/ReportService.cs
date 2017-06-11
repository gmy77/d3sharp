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
using Google.ProtocolBuffers;
using Mooege.Common.Logging;
using Mooege.Net.MooNet;
using Mooege.Core.MooNet.Accounts;

namespace Mooege.Core.MooNet.Services
{
    [Service(serviceID: 0x63, serviceName: "bnet.protocol.report.Report")]
    public class ReportService : bnet.protocol.report.ReportService, IServerService
    {
        private static readonly Logger Logger = LogManager.CreateLogger();
        public MooNetClient Client { get; set; }
        public bnet.protocol.Header LastCallHeader { get; set; }
        public uint Status { get; set; }

        public override void SendReport(IRpcController controller, bnet.protocol.report.SendReportRequest request, Action<bnet.protocol.NoData> done)
        {
            Logger.Trace("SendReport()");

            var report = request.Report;
            //TODO: Store reports against accounts
            foreach (var attribute in report.AttributeList)
            {
                switch (attribute.Name)
                {
                    case "target_toon_id": //uint GameAccount.Low
                        var reportee = GameAccountManager.GetAccountByPersistentID(attribute.Value.UintValue);
                        Logger.Trace("{0} reported {1} for \"{2}\".", this.Client.Account.CurrentGameAccount.CurrentToon, reportee, report.ReportType);
                        break;
                    //case "target_account_id": //uint Account.Low
                    //case "target_toon_name": //string
                    //case "target_toon_program": //fourcc
                    //case "target_toon_region": //string
                    //case "note": //string - not currently used in client
                }
            }

            var builder = bnet.protocol.NoData.CreateBuilder();

            done(builder.Build());
        }
    }
}
