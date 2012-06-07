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
namespace Mooege.Core.MooNet.Services
{
    [Service(serviceID: 0xe, serviceName: "bnet.protocol.search.SearchService")]
    public class SearchService : bnet.protocol.search.SearchService, IServerService
    {
        private static readonly Logger Logger = LogManager.CreateLogger();
        public MooNetClient Client { get; set; }
        public bnet.protocol.Header LastCallHeader { get; set; }
        public uint Status { get; set; }

        public override void FindMatches(IRpcController controller, bnet.protocol.search.FindMatchesRequest request, Action<bnet.protocol.search.FindMatchesResponse> done)
        {
            throw new NotImplementedException();
        }

        public override void SetObject(IRpcController controller, bnet.protocol.search.SetObjectRequest request, Action<bnet.protocol.NO_RESPONSE> done)
        {
            throw new NotImplementedException();
        }

        public override void RemoveObjects(IRpcController controller, bnet.protocol.search.RemoveObjectsRequest request, Action<bnet.protocol.NO_RESPONSE> done)
        {
            throw new NotImplementedException();
        }
    }
}
