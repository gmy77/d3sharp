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
using System.Linq;
using Google.ProtocolBuffers;
using Google.ProtocolBuffers.Descriptors;
using Mooege.Common.Logging;
using Mooege.Core.MooNet.Services;
using Mooege.Net.MooNet.Packets;

namespace Mooege.Net.MooNet
{
    public static class MooNetRouter
    {
        private static readonly Logger Logger = LogManager.CreateLogger();
        public const byte ServiceReply = 0xFE;

        public static void Route(ConnectionDataEventArgs e)
        {
            var client = (MooNetClient)e.Connection.Client;
            client.IncomingMooNetStream.Append(e.Data.ToArray());

            try
            {
                while (client.IncomingMooNetStream.PacketAvaliable())
                {
                    Identify(client);
                }
            }
            catch (Exception except)
            {
                Logger.Error("exception caugth on decoding loop");
                Logger.Error(except.Message);
                Logger.Error(except.StackTrace);
            }

            client.IncomingMooNetStream.Consume();
        }

        public static void Identify(MooNetClient client)
        {
            var packet = new PacketIn(client, client.IncomingMooNetStream.GetPacketHeader());

            if (packet.Header.ServiceId == ServiceReply)
                ProcessReply(client, packet);
            else
                ProcessMessage(client, packet);
        }

        private static void ProcessReply(MooNetClient client, PacketIn packet)
        {
            if (client.RPCCallbacks.ContainsKey(packet.Header.Token))
            {
                var callback = client.RPCCallbacks[packet.Header.Token];
                Logger.Trace("RPCReply => {0}", callback.Action.Target);

                callback.Action(packet.ReadMessage(callback.Builder));
                client.RPCCallbacks.Remove(packet.Header.Token);
            }
            else
            {
                Logger.Warn("RPC callback contains unexpected token: {0}", packet.Header.Token);
                client.Connection.Disconnect();
            }
        }

        private static void ProcessMessage(MooNetClient client, PacketIn packet)
        {
            var service = Service.GetByID(packet.Header.ServiceId);

            if (service == null)
            {
                Logger.Error("No service exists with id: 0x{0}", packet.Header.ServiceId.ToString("X2"));
                return;
            }

            var method = service.DescriptorForType.Methods.Single(m => GetMethodId(m) == packet.Header.MethodId);
            var proto = service.GetRequestPrototype(method);
            var builder = proto.WeakCreateBuilderForType();
            var message = packet.ReadMessage(builder);

            Logger.LogIncomingPacket(message, packet.Header);

            try
            {
                lock (service) // lock the service so that its in-context client does not get changed..
                {
                    ((IServerService)service).Client = client;
                    ((IServerService)service).LastCallHeader = packet.Header;
                    ((IServerService)service).Status = 0;
                    service.CallMethod(method, null, message, (msg => SendRPCResponse(client.Connection, packet.Header.Token, msg, ((IServerService)service).Status)));
                }
            }
            catch (NotImplementedException)
            {
                Logger.Warn("Unimplemented service method: {0}.{1}", service.GetType().Name, method.Name);
            }
            catch (UninitializedMessageException e)
            {
                Logger.Debug("Failed to parse message: {0}", e.Message);
            }
            catch (Exception e)
            {
                Logger.DebugException(e, string.Empty);
            }
        }

        public static uint GetMethodId(MethodDescriptor method)
        {
            return (uint)method.Options[bnet.protocol.Rpc.MethodId.Descriptor];
        }

        private static void SendRPCResponse(IConnection connection, uint token, IMessage message, uint status)
        {
            var packet = new PacketOut(ServiceReply, 0x0, token, message, status);
            connection.Send(packet);
        }
    }
}
