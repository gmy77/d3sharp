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
using Google.ProtocolBuffers;
using Mooege.Common.Logging;
using Mooege.Net.MooNet;
using Mooege.Common.Extensions;
using Mooege.Common.Versions;
using Mooege.Core.MooNet.Helpers;

namespace Mooege.Core.MooNet.Services
{
    [Service(serviceID: 0xd, serviceName: "bnet.protocol.resources.Resources")]
    public class ResourceService : bnet.protocol.resources.Resources, IServerService
    {
        private static readonly Logger Logger = LogManager.CreateLogger();
        public MooNetClient Client { get; set; }
        public bnet.protocol.Header LastCallHeader { get; set; }
        public uint Status { get; set; }

        public override void GetContentHandle(IRpcController controller, bnet.protocol.resources.ContentHandleRequest request, Action<bnet.protocol.ContentHandle> done)
        {
            Logger.Trace("GetContentHandle(): ProgramId: 0x{0:X8} StreamId: 0x{1:X8} Locale: 0x{2:X8}", request.ProgramId, request.StreamId, request.Locale);
            if (request.ProgramId == (uint)FieldKeyHelper.Program.BNet)
            {
                var builder = bnet.protocol.ContentHandle.CreateBuilder()
                    .SetRegion(VersionInfo.MooNet.Regions[VersionInfo.MooNet.Region])
                    .SetUsage(0x70667479) //pfty - ProfanityFilter
                    .SetHash(ByteString.CopyFrom(VersionInfo.MooNet.Resources.ProfanityFilterHash.ToByteArray()));

                done(builder.Build());
            }
            else if (request.ProgramId == (uint)FieldKeyHelper.Program.D3)
            {
                var builder = bnet.protocol.ContentHandle.CreateBuilder()
                    .SetRegion(VersionInfo.MooNet.Regions[VersionInfo.MooNet.Region])
                    .SetUsage(0x643373); //d3s - d3 Schema
                switch (request.StreamId)
                {
                    case 0x61637473: //acts - Available Acts
                        builder.SetHash(ByteString.CopyFrom(VersionInfo.MooNet.Resources.AvailableActs.ToByteArray()));
                        break;
                    case 0x71756573: //ques - Available Quests
                        builder.SetHash(ByteString.CopyFrom(VersionInfo.MooNet.Resources.AvailableQuests.ToByteArray()));
                        break;
                    case 0x72707273: //rprs - RichPresence
                        builder.SetHash(ByteString.CopyFrom("bb41b5176f172cd217a137e7c19d19a4827c48877fe5a2ec94e2d3658612afdf".ToByteArray()));
                        break;
                    default:
                        Logger.Warn("Unknown StreamId: 0x{0:X8}", request.StreamId);
                        builder.SetHash(ByteString.Empty);
                        Status = 4;
                        break;
                }
                done(builder.Build());
            }
        }
    }
}
