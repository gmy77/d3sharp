using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using D3Sharp.Net.Packets;

namespace D3Sharp.Net
{
    public interface IClient
    {
        bool IsConnected { get; }
        IPEndPoint RemoteEndPoint { get; }
        IPEndPoint LocalEndPoint { get; }

        Dictionary<uint, uint> Services { get; }

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

