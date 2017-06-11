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

using OpenSSL;

namespace Mooege.Net
{
    /// <summary>
    /// TCP Connection class.
    /// </summary>
    public class Connection : IConnection
    {
        protected static readonly Logger Logger = LogManager.CreateLogger(); // the logger.

        // [D3Inferno]
        /// Returns true if the connection is encrypted.
        public bool IsEncrypted { get; private set; }

        // Returns true is the EncryptRequest message has been sent to the client.
        // The last unencrypted packet should be the EncryptRequest NoData service response.
        public bool IsEncryptRequestSent { get; set; }

        // Returns true if the connection is trying to establish a Tls connection.
        public bool IsTlsHandshaking { get; set; }

        /// The underlying TLS stream. Required for encrypted communication.
        public SslStream TLSStream { get; private set; }

        // Notify the Connection of the encrypted TLSStream
        public void SetEncrypted(SslStream TLSStream)
        {
            this.TLSStream = TLSStream;
            this.IsEncrypted = true;
        }

        // Notify that Tls Authentication is complete (but perhaps not successful) so that it can once
        // again start listening for client data, but from now on, using the SslStream if successful.
        public void TlsAuthenticationComplete()
        {
            if (this._server == null)
            {
                Logger.Warn("Server is null in TlsAuthenticationComplete");
                return;
            }
            this._server.TlsAuthenticationComplete(this);
        }

        // Read bytes from the Socket into the buffer in a non-blocking call.
        // This allows us to read no more than the specified count number of bytes.\
        // Note that this method should only be called prior to encryption!
        public int Receive(int start, int count)
        {
            return this.Socket.Receive(_recvBuffer, start, count, SocketFlags.None);
        }

        // Wrapper for the Send method that will send the data either to the
        // Socket (unecnrypted) or to the TLSStream (encrypted).
        public int _Send(byte[] buffer, int start, int count, SocketFlags flags)
        {
            if (!IsEncrypted)
            {
                return this.Socket.Send(buffer, start, count, flags);
            }
            else
            {
                // Does this Write operation guarantee that all bytes will be written?
                this.TLSStream.Write(buffer, start, count);
                return count;
            }
        }

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
        private Server _server;

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
            get { return (Socket == null) ? false : Socket.Connected; }
        }

        /// <summary>
        /// Returns remote endpoint.
        /// </summary>
        public IPEndPoint RemoteEndPoint
        {
            get { return (Socket == null) ? null : Socket.RemoteEndPoint as IPEndPoint; }
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
            if (!IsEncrypted)
            {
                return this.Socket.BeginReceive(_recvBuffer, 0, BufferSize, SocketFlags.None, callback, state);
            }
            else
            {
                return this.TLSStream.BeginRead(_recvBuffer, 0, BufferSize, callback, state);
            }
        }

        public int EndReceive(IAsyncResult result)
        {
            if (!IsEncrypted)
            {
                return this.Socket.EndReceive(result);
            }
            else
            {
                return this.TLSStream.EndRead(result);
            }
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
            if (_server == null)
            {
                throw new Exception("[Connection] _server is null in Send");
            }
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
            if (_server == null)
            {
                throw new Exception("[Connection] _server is null in Send");
            }
            return _server.Send(this, data, flags);
        }

        /// <summary>
        /// Kills the connection to remote endpoint.
        /// </summary>
        public void Disconnect()
        {
            Logger.Trace("Disconnect() | server: " + _server);

            if (_server != null)
            {
                // Use temp assignment to preven recursion.
                Server tempServer = _server;
                _server = null;
                tempServer.Disconnect(this);
            }

            if (this.TLSStream != null)
            {
                try
                {
                    this.TLSStream.Close();
                }
                finally
                {
                    this.TLSStream.Dispose();
                    this.TLSStream = null;
                }
            }

            if (this.Socket != null)
            {
                try
                {
                    this.Socket.Shutdown(SocketShutdown.Both);
                    this.Socket.Close();
                }
                catch (Exception)
                {
                    // Ignore any exceptions that might occur during attempt to close the Socket.
                }
                finally
                {
                    this.Socket.Dispose();
                    this.Socket = null;
                }
            }
        }

        /// <summary>
        /// Returns a connection state string.
        /// </summary>
        /// <returns>Connection state string.</returns>
        public override string ToString()
        {
            if (Socket == null)
                return "No Socket!";
            else
                return Socket.RemoteEndPoint != null ? Socket.RemoteEndPoint.ToString() : "Not Connected!";
        }

        #endregion
    }
}
