using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using D3Sharp.Core.Accounts;
using D3Sharp.Core.Toons;
using D3Sharp.Core.Channels;
using D3Sharp.Net.Packets;
using Google.ProtocolBuffers;
using Google.ProtocolBuffers.Descriptors;

namespace D3Sharp.Net
{
    public interface IClient
    {
        bool IsConnected { get; }
        IPEndPoint RemoteEndPoint { get; }
        IPEndPoint LocalEndPoint { get; }

        Dictionary<uint, uint> Services { get; }
        Account Account { get; set; }
        
        Toon CurrentToon { get; set; }
        Channel CurrentChannel { get; set; }
        bnet.protocol.Identity Identity { get; }
        
        void MapLocalObjectID(ulong localObjectId, ulong externalObjectId);
        ulong GetExternalObjectID(ulong localObjectId);
        
        void CallMethod(MethodDescriptor method, IMessage request);
        void CallMethod(MethodDescriptor method, IMessage request, ulong localObjectId);
        
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

