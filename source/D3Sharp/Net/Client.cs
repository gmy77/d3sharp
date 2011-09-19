using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using D3Sharp.Core.Accounts;
using D3Sharp.Core.Services;
using D3Sharp.Core.Toons;
using D3Sharp.Net.Packets;
using D3Sharp.Utils;
using D3Sharp.Utils.Helpers;
using Google.ProtocolBuffers;

namespace D3Sharp.Net
{
    public sealed class Client : IClient
    {
        protected static readonly Logger Logger = LogManager.CreateLogger();

        private readonly Server _server;
        private readonly Socket _socket;
        private readonly byte[] _recvBuffer = new byte[BufferSize];
        public static readonly int BufferSize = 16 * 1024; // 16 KB

        public Dictionary<uint, uint> Services { get; set; }
        public Account Account { get; set; }
        private int _requestCounter = 0;

        public Client(Server server, Socket socket)
        {
            if (server == null) throw new ArgumentNullException("server");
            if (socket == null) throw new ArgumentNullException("socket");

            this._server = server;
            this._socket = socket;
            this.Services = new Dictionary<uint, uint>();
        }

        public void RemoteCall(string remoteService, uint methodID, IMessage message)
        {
            var service = ClientService.GetDefinitionByName(remoteService);            
            if(service==null)
            {
                Logger.Error("{0} is not known client service", remoteService);
                return;
            }

            if(!this.Services.ContainsKey(service.Hash))
            {
                Logger.Error("Not bound to client service {0} [0x{1}] yet.", remoteService, service.Hash.ToString("X8"));
                return;
            }

            var serviceId = this.Services[service.Hash];
            var packet = new Packet(
                new Header((byte)serviceId, methodID, this._requestCounter++, (uint)message.SerializedSize),
                message.ToByteArray());

            this.Send(packet);
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
