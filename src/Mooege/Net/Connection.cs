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
    /// <summary>
    /// TCP Connection class.
    /// </summary>
    public class Connection : IConnection
    {
        protected static readonly Logger Logger = LogManager.CreateLogger(); // the logger.

        /// <summary>
        /// Gets underlying socket.
        /// </summary>
        public Socket Socket { get; private set; }

        /// <summary>
        /// Gets or sets bound client.
        /// </summary>
        public IClient Client { get; set; }

        /// <summary>
        /// Default buffer size.
        /// </summary>
        public static readonly int BufferSize = 16 * 1024; // 16 KB       

        /// <summary>
        /// The server that connection is bound to.
        /// </summary>
        private readonly Server _server;

        /// <summary>
        /// The recieve buffer.
        /// </summary>
        private readonly byte[] _recvBuffer = new byte[BufferSize];
                
        public Connection(Server server, Socket socket)
        {
            if (server == null) 
                throw new ArgumentNullException("server");

            if (socket == null) 
                throw new ArgumentNullException("socket");

            this._server = server;
            this.Socket = socket;
        }       

        #region socket stuff

        /// <summary>
        /// Returns true if there exists an active connection.
        /// </summary>
        public bool IsConnected
        {
            get { return Socket.Connected; }
        }

        /// <summary>
        /// Returns remote endpoint.
        /// </summary>
        public IPEndPoint RemoteEndPoint
        {
            get { return Socket.RemoteEndPoint as IPEndPoint; }
        }

        /// <summary>
        /// Returns local endpoint.
        /// </summary>
        public IPEndPoint LocalEndPoint
        {
            get { return Socket.LocalEndPoint as IPEndPoint; }
        }

        /// <summary>
        /// Returns the recieve-buffer.
        /// </summary>
        public byte[] RecvBuffer
        {
            get { return _recvBuffer; }
        }

        /// <summary>
        /// Begins recieving data async.
        /// </summary>
        /// <param name="callback">Callback function be called when recv() is complete.</param>
        /// <param name="state">State manager object.</param>
        /// <returns>Returns <see cref="IAsyncResult"/></returns>
        public IAsyncResult BeginReceive(AsyncCallback callback, object state)
        {
            return Socket.BeginReceive(_recvBuffer, 0, BufferSize, SocketFlags.None, callback, state);
        }

        public int EndReceive(IAsyncResult result)
        {
            return Socket.EndReceive(result);
        }

        /// <summary>
        /// Sends a <see cref="PacketOut"/> to remote endpoint.
        /// </summary>
        /// <param name="packet"><see cref="PacketOut"/> to send.</param>
        /// <returns>Returns count of sent bytes.</returns>
        public int Send(PacketOut packet)
        {
            if (packet == null) throw new ArgumentNullException("packet");
            return Send(packet.Data);
        }

        /// <summary>
        /// Sends byte buffer to remote endpoint.
        /// </summary>
        /// <param name="buffer">Byte buffer to send.</param>
        /// <returns>Returns count of sent bytes.</returns>
        public int Send(byte[] buffer)
        {
            if (buffer == null) throw new ArgumentNullException("buffer");
            return Send(buffer, 0, buffer.Length, SocketFlags.None);
        }

        /// <summary>
        /// Sends byte buffer to remote endpoint.
        /// </summary>
        /// <param name="buffer">Byte buffer to send.</param>
        /// <param name="flags">Sockets flags to use.</param>
        /// <returns>Returns count of sent bytes.</returns>
        public int Send(byte[] buffer, SocketFlags flags)
        {
            if (buffer == null) throw new ArgumentNullException("buffer");
            return Send(buffer, 0, buffer.Length, flags);
        }

        /// <summary>
        /// Sends byte buffer to remote endpoint.
        /// </summary>
        /// <param name="buffer">Byte buffer to send.</param>
        /// <param name="start">Start index to read from buffer.</param>
        /// <param name="count">Count of bytes to send.</param>
        /// <returns>Returns count of sent bytes.</returns>
        public int Send(byte[] buffer, int start, int count)
        {
            if (buffer == null) throw new ArgumentNullException("buffer");
            return Send(buffer, start, count, SocketFlags.None);
        }

        /// <summary>
        /// Sends byte buffer to remote endpoint.
        /// </summary>
        /// <param name="buffer">Byte buffer to send.</param>
        /// <param name="start">Start index to read from buffer.</param>
        /// <param name="count">Count of bytes to send.</param>
        /// <param name="flags">Sockets flags to use.</param>
        /// <returns>Returns count of sent bytes.</returns>
        public int Send(byte[] buffer, int start, int count, SocketFlags flags)
        {
            if (buffer == null) throw new ArgumentNullException("buffer");
            return _server.Send(this, buffer, start, count, flags);
        }

        /// <summary>
        /// Sends an enumarable byte buffer to remote endpoint.
        /// </summary>
        /// <param name="data">Enumrable byte buffer to send.</param>
        /// <returns>Returns count of sent bytes.</returns>
        public int Send(IEnumerable<byte> data)
        {
            if (data == null) throw new ArgumentNullException("data");
            return Send(data, SocketFlags.None);
        }

        /// <summary>
        /// Sends an enumarable byte buffer to remote endpoint.
        /// </summary>
        /// <param name="data">Enumrable byte buffer to send.</param>
        /// <param name="flags">Sockets flags to use.</param>
        /// <returns>Returns count of sent bytes.</returns>
        public int Send(IEnumerable<byte> data, SocketFlags flags)
        {
            if (data == null) throw new ArgumentNullException("data");
            return _server.Send(this, data, flags);
        }

        /// <summary>
        /// Kills the connection to remote endpoint.
        /// </summary>
        public void Disconnect()
        {
            if (this.IsConnected)
                _server.Disconnect(this);
        }

        /// <summary>
        /// Returns a connection state string.
        /// </summary>
        /// <returns>Connection state string.</returns>
        public override string ToString()
        {
            return Socket.RemoteEndPoint != null ? Socket.RemoteEndPoint.ToString() : "Not Connected!";
        }

        #endregion
    }
}
