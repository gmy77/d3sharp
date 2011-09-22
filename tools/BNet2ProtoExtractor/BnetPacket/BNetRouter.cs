using System;
using System.Linq;
using BNet2ProtoExtractor.Services;
using Google.ProtocolBuffers;

namespace BNet2ProtoExtractor.BnetPacket
{
    public static class BNetRouter
    {
        public static void ReadProto(PcapDotNet.Packets.Datagram payload)
        {
            Console.Write('.');
            var stream = CodedInputStream.CreateInstance(payload.ToArray());
            while (!stream.IsAtEnd)
            {
                Identify(stream);
            }
        }

        public static void Identify(CodedInputStream stream)
        {
            try
            {
                var header = new BNetHeader(stream);
                var payload = new byte[header.PayloadLength];

                payload = stream.ReadRawBytes((int) header.PayloadLength);

                var packet = new BNetPacket(header, payload);
                if (header.ServiceID == 0xfe) return;

                var service = Service.GetByID(header.ServiceID);
                if (service == null) return;

                var method =
                    service.DescriptorForType.Methods.Single(
                        m => (uint) m.Options[bnet.protocol.Rpc.MethodId.Descriptor] == header.MethodID);

                var proto = service.GetRequestPrototype(method);
                var builder = proto.WeakCreateBuilderForType();

                var message =
                    builder.WeakMergeFrom(CodedInputStream.CreateInstance(packet.Payload.ToArray())).WeakBuild();
                service.CallMethod(method, null, message, (msg => { }));
            }
            catch (UninitializedMessageException e)
            {
                Console.WriteLine("Failed to parse message: {0}", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
