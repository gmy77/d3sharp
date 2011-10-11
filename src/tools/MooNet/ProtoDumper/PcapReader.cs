/*
 * Copyright (C) 2011 mooege project
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

using Mooege.Tools.ProtoDumper.Packet;
using PcapDotNet.Core;

namespace Mooege.Tools.ProtoDumper
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

        private static void DispatcherHandler(PcapDotNet.Packets.Packet packet)
        {
            if (packet.Ethernet.IpV4.Tcp == null) return;
            if (packet.Ethernet.IpV4.Tcp.Payload == null) return;
            if (packet.Ethernet.IpV4.Tcp.Payload.Length == 0) return;

            MooNetRouter.ReadProto(packet.Ethernet.IpV4.Tcp.Payload);
        }
    }
}
