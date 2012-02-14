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

using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Mooege.Net.MooNet.Packets;

namespace Mooege.Net
{
    public interface IConnection
    {
        bool IsConnected { get; }
        IPEndPoint RemoteEndPoint { get; }
        IPEndPoint LocalEndPoint { get; }
        IClient Client { get; set; }
        Socket _Socket { get; }
        
        int Send(PacketOut packet);
        int Send(IEnumerable<byte> data);
        int Send(IEnumerable<byte> data, SocketFlags flags);
        int Send(byte[] buffer);
        int Send(byte[] buffer, SocketFlags flags);
        int Send(byte[] buffer, int start, int count);
        int Send(byte[] buffer, int start, int count, SocketFlags flags);

        void Disconnect();
    }
}

