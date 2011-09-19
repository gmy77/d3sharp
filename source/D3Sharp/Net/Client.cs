using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using D3Sharp.Core.Accounts;
using D3Sharp.Core.Services;
using D3Sharp.Core.Toons;
using D3Sharp.Core.Channels;
using D3Sharp.Net.Packets;
using D3Sharp.Utils;
using D3Sharp.Utils.Helpers;
using Google.ProtocolBuffers;
using Google.ProtocolBuffers.Descriptors;

namespace D3Sharp.Net
{
    public sealed class Client : IClient//, IRpcChannel
    {
        protected static readonly Logger Logger = LogManager.CreateLogger();

        private readonly Server _server;
        private readonly Socket _socket;
        private readonly byte[] _recvBuffer = new byte[BufferSize];
        public static readonly int BufferSize = 16 * 1024; // 16 KB

        public Dictionary<uint, uint> Services { get; set; }
        public Account Account { get; set; }
        private int _requestCounter = 0;
        
        public Toon CurrentToon { get; set; }
        public Channel CurrentChannel { get; set; }

        private Dictionary<ulong, ulong> MappedObjects { get; set; }

        public bnet.protocol.Identity Identity {
            get {
                var identityBuilder = bnet.protocol.Identity.CreateBuilder();
                identityBuilder.SetAccountId(this.Account.BnetAccountID);
                identityBuilder.SetGameAccountId(this.Account.BnetGameAccountID);
                if (this.CurrentToon != null)
                    identityBuilder.SetToonId(this.CurrentToon.BnetEntityID);
                return identityBuilder.Build();
            }
        }

        public Client(Server server, Socket socket)
        {
            if (server == null) throw new ArgumentNullException("server");
            if (socket == null) throw new ArgumentNullException("socket");

            this._server = server;
            this._socket = socket;
            this.Services = new Dictionary<uint, uint>();
            this.MappedObjects = new Dictionary<ulong, ulong>();
        }

        // rpc to client
        public void CallMethod(ulong localObjectId, MethodDescriptor method, IMessage request, IMessage responsePrototype, Action<IMessage> done)
        {
            var serviceName = method.Service.FullName;
            var serviceHash = StringHashHelper.HashString(serviceName);

            var externalObjectId = GetExternalObjectID(localObjectId);

            if (!this.Services.ContainsKey(serviceHash))
            {
                Logger.Error("Not bound to client service {0} [0x{1}] yet.", serviceName, serviceHash.ToString("X8"));
                return;
            }

            Logger.Trace("Calling {0} (localObjectId={1}, externalObjectId={2}) with request:", method.FullName, localObjectId, externalObjectId);
            Logger.Debug(request.ToString());
            
            var serviceId = this.Services[serviceHash];
            var header = new Header((byte) serviceId, (uint)(method.Index + 1), this._requestCounter++, (uint) request.SerializedSize);
            header.ExternalObjectID=externalObjectId;
            var packet = new Packet(header, request.ToByteArray());

            this.Send(packet);
        }

        public void MapLocalObjectID(ulong localObjectId, ulong externalObjectId)
        {
            this.MappedObjects[localObjectId] = externalObjectId;
        }

        public ulong GetExternalObjectID(ulong localObjectId)
        {
            return this.MappedObjects[localObjectId];
        }

        #region socket stuff

        public bool IsConnected
        {
            get { return _socket.Connected; }
        }

        public IPEndPoint RemoteEndPoint
        {
            get { return _socket.RemoteEndPoint as IPEndPoint; }
        }

        public IPEndPoint LocalEndPoint
        {
            get { return _socket.LocalEndPoint as IPEndPoint; }
        }

        public byte[] RecvBuffer
        {
            get { return _recvBuffer; }
        }

        public Socket Socket
        {
            get { return _socket; }
        }

        public IAsyncResult BeginReceive(AsyncCallback callback, object state)
        {
            return _socket.BeginReceive(_recvBuffer, 0, BufferSize, SocketFlags.None, callback, state);
        }

        public int EndReceive(IAsyncResult result)
        {
            return _socket.EndReceive(result);
        }

        public int Send(Packet packet)
        {
            if (packet == null) throw new ArgumentNullException("packet");
            return Send(packet.GetRawPacketData());
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
            if (_socket.RemoteEndPoint != null)
                return _socket.RemoteEndPoint.ToString();
            else
                return "Not Connected";
        }

        #endregion
    }
}
