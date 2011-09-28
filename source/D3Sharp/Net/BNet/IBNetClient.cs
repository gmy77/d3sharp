/*
 * Copyright (C) 2011 D3Sharp Project
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
using D3Sharp.Core.BNet.Accounts;
using D3Sharp.Core.BNet.Channels;
using D3Sharp.Core.Common.Toons;
using D3Sharp.Net.Game;
using Google.ProtocolBuffers;
using Google.ProtocolBuffers.Descriptors;

namespace D3Sharp.Net.BNet
{
    public interface IBNetClient:IClient
    {
        Dictionary<uint, uint> Services { get; }
        Account Account { get; set; }

        Toon CurrentToon { get; set; }
        Channel CurrentChannel { get; set; }
        GameClient InGameClient { get; set; }

        void CallMethod(MethodDescriptor method, IMessage request);
        void CallMethod(MethodDescriptor method, IMessage request, ulong localObjectId);

        bnet.protocol.Identity GetIdentity(bool acct, bool gameacct, bool toon);

        void MapLocalObjectID(ulong localObjectId, ulong remoteObjectId);
        void UnmapLocalObjectID(ulong localObjectId);
        ulong GetRemoteObjectID(ulong localObjectId);
    }
}
