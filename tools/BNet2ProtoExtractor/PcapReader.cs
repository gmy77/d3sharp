using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BNet2ProtoExtractor.BnetPacket;
using PcapDotNet.Core;
using PcapDotNet.Packets;

namespace BNet2ProtoExtractor
{
    public static class PcapReader
    {
        public static void Read(string file)
        {
            // Create the offline device
            var selectedDevice = new OfflinePacketDevice(file);

            // Open the capture file
            using (PacketCommunicator communicator =
                selectedDevice.Open(65536, // portion of the packet to capture
                                    // 65536 guarantees that the whole packet will be captured on all the link layers
                                    PacketDeviceOpenAttributes.Promiscuous, // promiscuous mode
                                    1000)) // read timeout
            {
                communicator.SetFilter("tcp port 1119");

                // Read and dispatch packets until EOF is reached
                communicator.ReceivePackets(0, DispatcherHandler);
            }
        }

        private static void DispatcherHandler(Packet packet)
        {
            if (packet.Ethernet.IpV4.Tcp == null) return;
            if (packet.Ethernet.IpV4.Tcp.Payload == null) return;
            if (packet.Ethernet.IpV4.Tcp.Payload.Length == 0) return;

            BNetRouter.ReadProto(packet.Ethernet.IpV4.Tcp.Payload);
        }
    }
}
