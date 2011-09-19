using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using D3Sharp.Core.Services;
using D3Sharp.Utils;
using D3Sharp.Utils.Extensions;
using Google.ProtocolBuffers;

namespace D3Sharp.Net.Packets
{
    public static class PacketRouter
    {
        private static readonly Logger Logger = LogManager.CreateLogger();

        public static void Route(ClientDataEventArgs e)
        {
            var stream = CodedInputStream.CreateInstance(e.Data.ToArray());
            while (!stream.IsAtEnd)
            {
                Identify(e.Client, stream);
            }
        }

        public static void Identify(IClient client, CodedInputStream stream)
        {
            var header = new Header(stream);
            var payload = new byte[header.PayloadLength];
            payload = stream.ReadRawBytes((int)header.PayloadLength);

            var packet = new Packet(header, payload);
            var service = Service.GetByID(header.ServiceID);

            if (service == null)
            {
                Logger.Error("No service exists with id: 0x{0}", header.ServiceID.ToString("X2"));
                return;
            }

            var method = service.DescriptorForType.Methods[(int)header.MethodID - 1];
            var proto = service.GetRequestPrototype(method);
            var builder = proto.WeakCreateBuilderForType();

            try
            {
                var message = builder.WeakMergeFrom(CodedInputStream.CreateInstance(packet.Payload.ToArray())).WeakBuild();

                lock (service) // lock the service so that it's in-context client does not get changed..
                {
                    //Logger.Debug("service-call data:{0}", message.ToString());
                    ((IServerService) service).Client = client;
                    service.CallMethod(method, null, message, (msg => SendResponse(client, header.RequestID, msg)));
                }
            }
            catch (NotImplementedException)
            {
                Logger.Debug(string.Format("Unimplemented service method: {0} {1}", service.GetType().Name, method.Name));
            }
            catch(Exception e)
            {
                Logger.DebugException(e,string.Empty);
            }
        }

        private static void SendResponse(IClient client, int requestId, IMessage message)
        {
            var packet = new Packet(
                new Header(0xfe, 0x0, requestId, (uint)message.SerializedSize),
                message.ToByteArray());

            client.Send(packet);
        }
    }
}
