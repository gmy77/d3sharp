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
using System.Net;
using System.Net.Sockets;
using D3Sharp.Core.Accounts;
using D3Sharp.Core.Toons;
using D3Sharp.Core.Channels;
using D3Sharp.Net.BnetServer.Packets;
using Google.ProtocolBuffers;
using Google.ProtocolBuffers.Descriptors;

namespace D3Sharp.Net
{
    public interface IClient
    {
        bool IsConnected { get; }
        IPEndPoint RemoteEndPoint { get; }
        IPEndPoint LocalEndPoint { get; }

        Dictionary<uint, uint> Services { get; }
        Account Account { get; set; }
        
        Toon CurrentToon { get; set; }
        Channel CurrentChannel { get; set; }
        
        void CallMethod(MethodDescriptor method, IMessage request);
        void CallMethod(MethodDescriptor method, IMessage request, ulong localObjectId);

        bnet.protocol.Identity GetIdentity(bool acct, bool gameacct, bool toon);

        void MapLocalObjectID(ulong localObjectId, ulong remoteObjectId);
        ulong GetRemoteObjectID(ulong localObjectId);
        
        int Send(Packet packet);
        int Send(IEnumerable<byte> data);
        int Send(IEnumerable<byte> data, SocketFlags flags);
        int Send(byte[] buffer);
        int Send(byte[] buffer, SocketFlags flags);
        int Send(byte[] buffer, int start, int count);
        int Send(byte[] buffer, int start, int count, SocketFlags flags);

        void Disconnect();
    }
}

