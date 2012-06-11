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
        // protected Dictionary<Socket, IConnection> Connections = new Dictionary<Socket, IConnection>();
        protected List<IConnection> Connections = new List<IConnection>();
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

                lock (ConnectionLock) Connections.Add(connection); // add the connection to list.

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

                    // [D3Inferno]
                    // Only receive again if we did not just send the EncryptRequest.
                    // If we did, then the current received packet should be the EncryptRequest NoData service response.
                    //
                    // WARNING: Probably need to explicitly test for this packet since we can get data out-of-order from the client.
                    // However, I would imagine the D3 game client has to do a flush before sending the EncryptRequest response,
                    // otherwise, anything sent after that would break since it would be handled by the TLS layer which would throw
                    // some kind of SSL error (most likely invalid version).
                    //
                    if ((connection.IsEncryptRequestSent) && (!connection.IsTlsHandshaking))
                    {
                        Logger.Trace("Waiting for the EncryptRequest NoData service response from the client.");

                        // [D3Inferno]
                        // We need to receive the EncryptRequest NoData service response from the client.
                        // However, the packet containing the response can also contain the Tls client_hello message.
                        // If we read it from the Socket, Tls Authentication will hang as the server will wait forever.
                        // So process just the next incoming message as a synchronous call.
                        if (connection.IsConnected)
                            this.ReceiveEncryptRequestServiceResponse(connection);
                    }
                    else if (!connection.IsTlsHandshaking)
                    {
                        // Begin receiving again on the socket, if it is connected.
                        if (connection.IsConnected)
                            connection.BeginReceive(ReceiveCallback, connection);
                        else
                            Logger.Trace("Connection closed:" + connection.Client);
                    }
                    else
                    {
                        Logger.Trace("No longer receiving unencrypted data.");
                    }
                }
                else RemoveConnection(connection, true); // Connection was lost.
            }
            catch (SocketException e)
            {
                RemoveConnection(connection, true); // An error occured while receiving, connection has disconnected.
                Logger.DebugException(e, "ReceiveCallback");
            }
            catch (Exception e)
            {
                RemoveConnection(connection, true); // An error occured while receiving, the connection may have been disconnected.
                Logger.DebugException(e, "ReceiveCallback");
            }
        }

        // [D3Inferno]
        // Tls authentication is complete, although it may have failed.
        // Regardless, start receiving data again.
        // If Tls was successful, it will now be handled by the SslStream.
        public void TlsAuthenticationComplete(Connection connection)
        {
            Logger.Trace("TlsAuthenticationComplete");

            // Begin receiving again on the socket, if it is connected.
            if (connection.IsConnected)
            {
                try
                {
                    connection.BeginReceive(ReceiveCallback, connection);
                }
                catch (Exception e)
                {
                    RemoveConnection(connection, true); // An error occured while receiving, the connection may have been disconnected.
                    if (e is System.IO.IOException)
                        Logger.Trace("{0} unexpectedly closed connection, client is not patched.", connection.Client);
                    else
                        Logger.DebugException(e, "TlsAuthenticationComplete()");
                }
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
                    // [D3Inferno]
                    // Use Connection wrapper to send the data so that it can be sent either over
                    // the normal NetworkStream (prior to Tls Authentication) or over the encrypted
                    // SslStream.
                    // Send the remaining data.
                    //int bytesSent = connection.Socket.Send(buffer, totalBytesSent, bytesRemaining, flags);

                    int bytesSent = connection._Send(buffer, totalBytesSent, bytesRemaining, flags);

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
                RemoveConnection(connection, true); // An error occured while sending, it is possible that the connection has a problem.
                Logger.DebugException(e, "Send");
            }

            return totalBytesSent;
        }

        #endregion

        #region Encryption Support

        // [D3Inferno]
        // This method is called once 
        private void ReceiveEncryptRequestServiceResponse(IConnection connection)
        {
            if (connection == null) return;

            int totalBytesRevc = 0;
            int pos = 0;

            int bytesRecv = connection.Receive(pos, 2);
            totalBytesRevc += bytesRecv;
            if (bytesRecv != 2)
                throw new Exception("Server Socket could not read header size bytes for EncryptRequest Service Response.");

            pos += 2;
            int headerSize = (connection.RecvBuffer[0] << 8) | connection.RecvBuffer[1];

            bytesRecv = connection.Receive(pos, headerSize);
            totalBytesRevc += bytesRecv;
            if (bytesRecv != headerSize)
                throw new Exception("Server Socket could not read header bytes (size = " + headerSize + ") for EncryptRequest Service Response.");

            // Create the bnet.protocol.Header object.
            byte[] headerBytes = new byte[headerSize];
            Array.Copy(connection.RecvBuffer, pos, headerBytes, 0, headerSize);
            var headerData = bnet.protocol.Header.ParseFrom(headerBytes);

            pos += headerSize;
            bytesRecv = connection.Receive(pos, (int)headerData.Size);
            totalBytesRevc += bytesRecv;
            if (bytesRecv != headerData.Size)
                throw new Exception("Server Socket could not read data (size = " + headerData.Size + ") for EncryptRequest Service Response.");

            // We could verify that the packet corresponds to EncryptRequest service response.
            // However, this would require looking into the MooNetClient's RPCCallbacks for the token,
            // and then verifying that it is of the correct type.

            if (totalBytesRevc > 0)
            {
                OnDataReceived(new ConnectionDataEventArgs(connection, connection.RecvBuffer.Enumerate(0, totalBytesRevc))); // Raise the DataReceived event.
            }
        }

        #endregion

        #region service methods

        public IEnumerable<IConnection> GetConnections()
        {
            lock (ConnectionLock)
                foreach (IConnection connection in Connections)
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
                foreach (var connection in Connections.Cast<Connection>()) // Check if the connection is connected.
                {
                    // Disconnect and raise the OnDisconnect event.
                    connection.Disconnect();
                    //                    connection.Socket.Disconnect(false);
                    OnClientDisconnect(new ConnectionEventArgs(connection));
                }

                Connections.Clear();
            }
        }

        public virtual void Disconnect(Connection connection)
        {
            //            if (connection == null) throw new ArgumentNullException("connection");
            //            if (!connection.IsConnected) return;

            if (connection == null)
                return;

            connection.Disconnect();

            //            connection.Socket.Disconnect(false);
            _RemoveConnection(connection, true);
        }

        private void _Disconnect(Connection connection)
        {
            if (connection == null)
                return;

            connection.Disconnect();
        }

        private void RemoveConnection(Connection connection, bool raiseEvent)
        {
            if (connection == null)
                return;

            // [D3Inferno]
            // The whole Server.Disconnect vs Server.RemoveConnection vs Connection.Disconnect is a complete mess.
            // Trying to modify the code so everything gets cleaned up properly without causing an infinite recursion.
            connection.Disconnect();

            // Remove the connection from the dictionary and raise the OnDisconnection event.
            lock (ConnectionLock)
            {
                if (Connections.Contains(connection))
                    Connections.Remove(connection);
            }

            if (raiseEvent)
                NotifyRemoveConnection(connection);
        }

        private void _RemoveConnection(Connection connection, bool raiseEvent)
        {
            if (connection == null)
                return;

            // Remove the connection from the dictionary and raise the OnDisconnection event.
            lock (ConnectionLock)
            {
                if (Connections.Contains(connection))
                    Connections.Remove(connection);
            }

            if (raiseEvent)
                NotifyRemoveConnection(connection);
        }

        private void NotifyRemoveConnection(Connection connection)
        {
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
            foreach (var connection in this.Connections.ToList()) // use ToList() so we don't get collection modified exception there
            {
                connection.Disconnect();
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

