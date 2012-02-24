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

namespace Mooege.Core.MooNet.Services
{
    [Service(serviceID: 0xd, serviceName: "bnet.protocol.resources.Resources")]
    public class ResourceService : bnet.protocol.resources.Resources, IServerService
    {
        private static readonly Logger Logger = LogManager.CreateLogger();
        public MooNetClient Client { get; set; }
        public bnet.protocol.Header LastCallHeader { get; set; }

        public override void GetContentHandle(IRpcController controller, bnet.protocol.resources.ContentHandleRequest request, Action<bnet.protocol.ContentHandle> done)
        {
            Logger.Trace("GetContentHandle(): ProgramId: 0x{0:X8} StreamId: 0x{1:X8} Locale: 0x{2:X8}", request.ProgramId, request.StreamId, request.Locale);
            if (request.ProgramId == 16947)
            {
                var builder = bnet.protocol.ContentHandle.CreateBuilder()
                    .SetRegion(22616)
                    .SetUsage(1346786393)
                    .SetHash(ByteString.CopyFrom("068FEC3C7426B8BA9497225A73437C6DFFAA92DE962C2B05589B5F46FBE5F5B0".ToByteArray()));

                done(builder.Build());
            }
            //Beta this returns status 4, no payload

            throw new NotImplementedException();

        }
    }
}
