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
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Mooege.Common.Extensions;
using Mooege.Common.Logging;

namespace Mooege.Net
{
    public class Server : IDisposable
    {
        public bool IsListening { get; private set; }
        public int Port { get; private set; }

        protected Socket Listener;
        protected Dictionary<Socket, IConnection> Connections = new Dictionary<Socket, IConnection>();
        protected object ConnectionLock = new object();

        public delegate void ConnectionEventHandler(object sender, ConnectionEventArgs e);
        public delegate void ConnectionDataEventHandler(object sender, ConnectionDataEventArgs e);

        public event ConnectionEventHandler OnConnect;
        public event ConnectionEventHandler OnDisconnect;
        public event ConnectionDataEventHandler DataReceived;
        public event ConnectionDataEventHandler DataSent;

        protected static readonly Logger Logger = LogManager.CreateLogger();
        private bool _disposed;

        public virtual void Run() { }

        #region listener

        public virtual bool Listen(string bindIP, int port)
        {
            // Check if the server has been disposed.
            if (_disposed) throw new ObjectDisposedException(this.GetType().Name, "Server has been disposed.");

            // Check if the server is already listening.
            if (IsListening) throw new InvalidOperationException("Server is already listening.");

            // --------------------
            // Note on IPv6 support
            // --------------------
            // First and foremost IPv6 support is EXPERIMENTAL!
            // Currently we use the approach to create a dual-socket which enables both IPv6 and IPv4 over the same socket
            // though not all operating systems support this by default. Windows Vista and 7 known to support this where Windows XP does not.
            // Also some linux distros and most BSD ones doesn't support this by default.
            // We've to eventually create two different sockets, one for IPv4 and one for IPv6 for wide-scale support for all operating systems. /raist
            // Still as D3 doesn't support IPv6 addresses to be returned with GameFactory services' bnet.protocol.game_master.ConnectInfo, 
            // IPv6 only works for moonet right now. When Blizzard fixes D3 client for so, we'll be also supporting IPv6 in game-server.

            // Create new TCP socket and set socket options.
            Listener = new Socket(NetworkingConfig.Instance.EnableIPv6 ? AddressFamily.InterNetworkV6 : AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // Setup our options:
            // * NoDelay - true - don't use packet coalescing
            // * DontLinger - true - don't keep sockets around once they've been disconnected
            // * IPv6Only - false - create a dual-socket that both supports IPv4 and IPv6 - check the IPv6 support note above.
            Listener.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.NoDelay, true);
            Listener.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, true);
			#if !__MonoCS__
            if (NetworkingConfig.Instance.EnableIPv6) Listener.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only, false);
			#endif

            try
            {
                // Bind.
                Listener.Bind(new IPEndPoint(IPAddress.Parse(bindIP), port));
                this.Port = port;
            }
            catch (SocketException)
            {
                Logger.Fatal(string.Format("{0} can not bind on {1}, server shutting down..", this.GetType().Name, bindIP));
                this.Shutdown();
                return false;
            }

            // Start listening for incoming connections.
            Listener.Listen(10);
            IsListening = true;

            // Begin accepting any incoming connections asynchronously.
            Listener.BeginAccept(AcceptCallback, null);

            return true;
        }

        private void AcceptCallback(IAsyncResult result)
        {
            if (Listener == null) return;

            try
            {
                var socket = Listener.EndAccept(result); // Finish accepting the incoming connection.
                var connection = new Connection(this, socket); // Add the new connection to the dictionary.

                lock (ConnectionLock) Connections[socket] = connection; // add the connection to list.

                OnClientConnection(new ConnectionEventArgs(connection)); // Raise the OnConnect event.

                connection.BeginReceive(ReceiveCallback, connection); // Begin receiving on the new connection connection.
                Listener.BeginAccept(AcceptCallback, null); // Continue receiving other incoming connection asynchronously.
            }
            catch (NullReferenceException) { } // we recive this after issuing server-shutdown, just ignore it.
            catch (Exception e)
            {
                Logger.DebugException(e, "AcceptCallback");
            }
        }

        private void ReceiveCallback(IAsyncResult result)
        {
            var connection = result.AsyncState as Connection; // Get the connection connection passed to the callback.
            if (connection == null) return;

            try
            {
                var bytesRecv = connection.EndReceive(result); // Finish receiving data from the socket.

                if (bytesRecv > 0)
                {
                    OnDataReceived(new ConnectionDataEventArgs(connection, connection.RecvBuffer.Enumerate(0, bytesRecv))); // Raise the DataReceived event.

                    if (connection.IsConnected) connection.BeginReceive(ReceiveCallback, connection); // Begin receiving again on the socket, if it is connected.
                    else RemoveConnection(connection, true); // else remove it from connection list.
                }
                else RemoveConnection(connection, true); // Connection was lost.
            }
            catch (SocketException)
            {
                RemoveConnection(connection, true); // An error occured while receiving, connection has disconnected.
            }
            catch (Exception e)
            {
                Logger.DebugException(e, "ReceiveCallback");
            }
        }

        public virtual int Send(Connection connection, IEnumerable<byte> data, SocketFlags flags)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (data == null) throw new ArgumentNullException("data");

            var buffer = data.ToArray();
            return Send(connection, buffer, 0, buffer.Length, SocketFlags.None);
        }

        public virtual int Send(Connection connection, byte[] buffer, int start, int count, SocketFlags flags)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (buffer == null) throw new ArgumentNullException("buffer");

            var totalBytesSent = 0;
            var bytesRemaining = buffer.Length;

            try
            {
                while (bytesRemaining > 0) // Ensure we send every byte.
                {
                    var bytesSent = connection.Socket.Send(buffer, totalBytesSent, bytesRemaining, flags); // Send the remaining data.
                    if (bytesSent > 0)
                        OnDataSent(new ConnectionDataEventArgs(connection, buffer.Enumerate(totalBytesSent, bytesSent))); // Raise the Data Sent event.

                    // Decrement bytes remaining and increment bytes sent.
                    bytesRemaining -= bytesSent;
                    totalBytesSent += bytesSent;
                }
            }
            catch (SocketException)
            {
                RemoveConnection(connection, true); // An error occured while sending, connection has disconnected.
            }
            catch (Exception e)
            {
                Logger.DebugException(e, "Send");
            }

            return totalBytesSent;
        }

        #endregion

        #region service methods

        public IEnumerable<IConnection> GetConnections()
        {
            lock (ConnectionLock)
                foreach (IConnection connection in Connections.Values)
                    yield return connection;
        }

        #endregion

        #region events

        protected virtual void OnClientConnection(ConnectionEventArgs e)
        {
            var handler = OnConnect;
            if (handler != null) handler(this, e);
        }

        protected virtual void OnClientDisconnect(ConnectionEventArgs e)
        {
            var handler = OnDisconnect;
            if (handler != null) handler(this, e);
        }

        protected virtual void OnDataReceived(ConnectionDataEventArgs e)
        {
            var handler = DataReceived;
            if (handler != null) handler(this, e);
        }

        protected virtual void OnDataSent(ConnectionDataEventArgs e)
        {
            var handler = DataSent;
            if (handler != null) handler(this, e);
        }

        #endregion

        #region disconnect & shutdown handlers

        public virtual void DisconnectAll()
        {
            lock (ConnectionLock)
            {
                foreach (var connection in Connections.Values.Cast<Connection>().Where(conn => conn.IsConnected)) // Check if the connection is connected.
                {
                    // Disconnect and raise the OnDisconnect event.
                    connection.Socket.Disconnect(false);
                    OnClientDisconnect(new ConnectionEventArgs(connection));
                }

                Connections.Clear();
            }
        }

        public virtual void Disconnect(Connection connection)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (!connection.IsConnected) return;

            connection.Socket.Disconnect(false);
            RemoveConnection(connection, true);
        }

        private void RemoveConnection(Connection connection, bool raiseEvent)
        {
            // Remove the connection from the dictionary and raise the OnDisconnection event.
            lock (ConnectionLock)
                if (Connections.Remove(connection.Socket) && raiseEvent)
                    OnClientDisconnect(new ConnectionEventArgs(connection));
        }

        public virtual void Shutdown()
        {
            // Check if the server has been disposed.
            if (_disposed) throw new ObjectDisposedException(this.GetType().Name, "Server has been disposed.");

            // Check if the server is actually listening.
            if (!IsListening) return;

            // Close the listener socket.
            if (Listener != null)
            {
                Listener.Close();
                Listener = null;
            }

            // Disconnect the clients.
            foreach(var connection in this.Connections.ToList()) // use ToList() so we don't get collection modified exception there
            {
                connection.Value.Disconnect();
            }

            Listener = null;
            IsListening = false;
        }

        #endregion

        #region de-ctor

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                Shutdown(); // Close the listener socket.
                DisconnectAll(); // Disconnect all users.
            }

            // Dispose of unmanaged resources here.

            _disposed = true;
        }

        #endregion
    }
}

