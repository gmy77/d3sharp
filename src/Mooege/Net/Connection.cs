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
using System.Net;
using System.Net.Sockets;
using Mooege.Common.Logging;
using Mooege.Net.MooNet.Packets;

namespace Mooege.Net
{
    public class Connection : IConnection
    {
        protected static readonly Logger Logger = LogManager.CreateLogger();

        private readonly Server _server;
        public  Socket _Socket { get; private set; }
        private readonly byte[] _recvBuffer = new byte[BufferSize];
        public static readonly int BufferSize = 16 * 1024; // 16 KB       

        public IClient Client { get; set; }

        public Connection(Server server, Socket socket)
        {
            if (server == null) throw new ArgumentNullException("server");
            if (socket == null) throw new ArgumentNullException("socket");

            this._server = server;
            this._Socket = socket;
        }       

        #region socket stuff

        public bool IsConnected
        {
            get { return _Socket.Connected; }
        }

        public IPEndPoint RemoteEndPoint
        {
            get { return _Socket.RemoteEndPoint as IPEndPoint; }
        }

        public IPEndPoint LocalEndPoint
        {
            get { return _Socket.LocalEndPoint as IPEndPoint; }
        }

        public byte[] RecvBuffer
        {
            get { return _recvBuffer; }
        }

        public Socket Socket
        {
            get { return _Socket; }
        }

        public IAsyncResult BeginReceive(AsyncCallback callback, object state)
        {
            return _Socket.BeginReceive(_recvBuffer, 0, BufferSize, SocketFlags.None, callback, state);
        }

        public int EndReceive(IAsyncResult result)
        {
            return _Socket.EndReceive(result);
        }

        public int Send(PacketOut packet)
        {
            if (packet == null) throw new ArgumentNullException("packet");
            return Send(packet.Data);
        }

        public int Send(IEnumerable<byte> data)
        {
            if (data == null) throw new ArgumentNullException("data");
            return Send(data, SocketFlags.None);
        }

        public int Send(IEnumerable<byte> data, SocketFlags flags)
        {
            if (data == null) throw new ArgumentNullException("data");
            return _server.Send(this, data, flags);
        }

        public int Send(byte[] buffer)
        {
            if (buffer == null) throw new ArgumentNullException("buffer");
            return Send(buffer, 0, buffer.Length, SocketFlags.None);
        }

        public int Send(byte[] buffer, SocketFlags flags)
        {
            if (buffer == null) throw new ArgumentNullException("buffer");
            return Send(buffer, 0, buffer.Length, flags);
        }

        public int Send(byte[] buffer, int start, int count)
        {
            if (buffer == null) throw new ArgumentNullException("buffer");
            return Send(buffer, start, count, SocketFlags.None);
        }

        public int Send(byte[] buffer, int start, int count, SocketFlags flags)
        {
            if (buffer == null) throw new ArgumentNullException("buffer");
            return _server.Send(this, buffer, start, count, flags);
        }

        public void Disconnect()
        {
            if (this.IsConnected)
                _server.Disconnect(this);
        }

        public override string ToString()
        {
            if (_Socket.RemoteEndPoint != null)
                return _Socket.RemoteEndPoint.ToString();
            else
                return "Not Connected";
        }

        #endregion
    }
}
