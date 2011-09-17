using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using D3Sharp.Core.Storage;
using D3Sharp.Utils;
using D3Sharp.Utils.Extensions;

namespace D3Sharp.Net
{
    public class Server : IDisposable
    {       
        public bool IsListening { get; private set; }
        public int Port { get; private set; }

        protected Socket Listener;
        protected Dictionary<Socket, IClient> Connections = new Dictionary<Socket, IClient>();                
        protected object ConnectionLock = new object();

        public delegate void ConnectionEventHandler(object sender, ClientEventArgs e);
        public delegate void ConnectionDataEventHandler(object sender, ClientDataEventArgs e);        

        public event ConnectionEventHandler ClientConnected;
        public event ConnectionEventHandler ClientDisconnected;
        public event ConnectionDataEventHandler DataReceived;
        public event ConnectionDataEventHandler DataSent;

        private static readonly Logger Logger = LogManager.CreateLogger();
        private bool _disposed;

        #region listener

        public virtual void Listen(int port)
        {
            // Check if the server has been disposed.
            if (_disposed) throw new ObjectDisposedException(this.GetType().Name, "Server has been disposed.");

            // Check if the server is already listening.
            if (IsListening) throw new InvalidOperationException("Server is already listening.");

            // Create new TCP socket and set socket options.
            Listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try {
                // This is failing on Linux; dunno why.
                Listener.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.NoDelay, true);
            } catch (SocketException e) {
                Logger.DebugException(e, "Listen");
            }
            Listener.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, true);

            // Bind.
            Listener.Bind(new IPEndPoint(IPAddress.Any, port));
            this.Port = port;

            // Start listening for incoming connections.
            Listener.Listen(10);
            IsListening = true;

            // Begin accepting any incoming connections asynchronously.
            Listener.BeginAccept(AcceptCallback, null);
        }

        private void AcceptCallback(IAsyncResult result)
        {
            try
            {
                var socket = Listener.EndAccept(result); // Finish accepting the incoming client.
                var connection = new Client(this, socket); // Add the new connection to the dictionary.

                lock (ConnectionLock) Connections[socket] = connection; // add the client to list.

                OnClientConnection(new ClientEventArgs(connection)); // Raise the ClientConnected event.

                connection.BeginReceive(ReceiveCallback, connection); // Begin receiving on the new client connection.
                Listener.BeginAccept(AcceptCallback, null); // Continue receiving other incoming clients asynchronously.
            }
            catch (Exception e)
            {
                Logger.DebugException(e, "AcceptCallback");
            }
        }

        private void ReceiveCallback(IAsyncResult result)
        {
            var connection = result.AsyncState as Client; // Get the client connection passed to the callback.
            if (connection == null) return;

            try
            {
                var bytesRecv = connection.EndReceive(result); // Finish receiving data from the socket.

                if (bytesRecv > 0)
                {
                    OnDataReceived(new ClientDataEventArgs(connection, connection.RecvBuffer.Enumerate(0, bytesRecv))); // Raise the DataReceived event.

                    if (connection.IsConnected) connection.BeginReceive(ReceiveCallback, connection); // Begin receiving again on the socket, if it is connected.
                    else RemoveConnection(connection, true); // else remove it from client list.
                }
                else RemoveConnection(connection, true); // Connection was lost.
            }
            catch (SocketException)
            {
                RemoveConnection(connection, true); // An error occured while receiving, client has disconnected.
            }
            catch (Exception e)
            {
                Logger.DebugException(e, "ReceiveCallback");
            }
        }

        public virtual int Send(Client connection, IEnumerable<byte> data, SocketFlags flags)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (data == null) throw new ArgumentNullException("data");

            var buffer = data.ToArray();
            return Send(connection, buffer, 0, buffer.Length, SocketFlags.None);
        }

        public virtual int Send(Client connection, byte[] buffer, int start, int count, SocketFlags flags)
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
                        OnDataSent(new ClientDataEventArgs(connection, buffer.Enumerate(totalBytesSent, bytesSent))); // Raise the Data Sent event.

                    // Decrement bytes remaining and increment bytes sent.
                    bytesRemaining -= bytesSent;
                    totalBytesSent += bytesSent;
                }
            }
            catch (SocketException)
            {
                RemoveConnection(connection, true); // An error occured while sending, client has disconnected.
            }
            catch (Exception e)
            {
                Logger.DebugException(e, "Send");
            }

            return totalBytesSent;
        }

        #endregion

        #region service methods

        public IEnumerable<IClient> GetClients()
        {
            lock (ConnectionLock)
                foreach (IClient client in Connections.Values)
                    yield return client;
        }

        #endregion

        #region events

        protected virtual void OnClientConnection(ClientEventArgs e)
        {
            var handler = ClientConnected;
            if (handler != null) handler(this, e);
        }

        protected virtual void OnClientDisconnect(ClientEventArgs e)
        {
            var handler = ClientDisconnected;
            if (handler != null) handler(this, e);
        }

        protected virtual void OnDataReceived(ClientDataEventArgs e)
        {
            var handler = DataReceived;
            if (handler != null) handler(this, e);
        }

        protected virtual void OnDataSent(ClientDataEventArgs e)
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
                foreach (var connection in Connections.Values.Cast<Client>().Where(client => client.IsConnected)) // Check if the client is connected.
                {
                    // Disconnect and raise the ClientDisconnected event.
                    connection.Socket.Disconnect(false);
                    OnClientDisconnect(new ClientEventArgs(connection));
                }

                Connections.Clear();
            }
        }

        public virtual void Disconnect(Client connection)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (!connection.IsConnected) return;

            connection.Socket.Disconnect(false);
            RemoveConnection(connection, true);
        }             
       
        private void RemoveConnection(Client connection, bool raiseEvent)
        {
            // Remove the connection from the dictionary and raise the OnDisconnection event.
            lock (ConnectionLock) 
                if (Connections.Remove(connection.Socket) && raiseEvent)
                    OnClientDisconnect(new ClientEventArgs(connection));
        }

        public virtual void Shutdown()
        {
            // Check if the server has been disposed.
            if (_disposed) throw new ObjectDisposedException(this.GetType().Name, "Server has been disposed.");

            // Check if the server is actually listening.
            if (!IsListening) return;

            // Close the listener socket.
            if (Listener != null) Listener.Close();

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

