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

using System;
using System.Linq;
using Mooege.Common.Logging;
using Mooege.Net.GS.Message;
using PcapDotNet.Core;
using PcapDotNet.Packets;

namespace GSPacketDumper
{
    public static class PacketReader
    {
        private static readonly Logger Logger = LogManager.CreateLogger();

        private static readonly GameBitBuffer IncomingBuffer = new GameBitBuffer(512);
        private static readonly GameBitBuffer OutgoingBuffer = new GameBitBuffer(512);

        private const string GameServerRange = "12.129.237.0/24";
        private const string GameServerIPPattern = "12.129.237";

        public static void Read(string file)
        {
            var selectedDevice = new OfflinePacketDevice(file);

            // Open the capture file
            using (PacketCommunicator communicator =
                selectedDevice.Open(65536, // portion of the packet to capture
                // 65536 guarantees that the whole packet will be captured on all the link layers
                                    PacketDeviceOpenAttributes.Promiscuous, // promiscuous mode
                                    1000)) // read timeout
            {
                communicator.SetFilter("tcp port 1119 and ip net " + GameServerRange);

                // Read and dispatch packets until EOF is reached
                communicator.ReceivePackets(0, DispatcherHandler);
            }
        }

        private static void HandleIncomingPacket(Packet packet)
        {
            IncomingBuffer.AppendData(packet.Ethernet.IpV4.Tcp.Payload.ToArray<byte>());

            while (IncomingBuffer.IsPacketAvailable())
            {
                int end = IncomingBuffer.Position;
                end += IncomingBuffer.ReadInt(32) * 8;
                while ((end - IncomingBuffer.Position) >= 9)
                {
                    var msg = IncomingBuffer.ParseMessage();
                    if (msg == null) continue;

                    Logger.LogIncomingPacket(msg);
                }

                IncomingBuffer.Position = end;
            }

            IncomingBuffer.ConsumeData();
        }

        private static void HandleOutgoingPacket(Packet packet)
        {
            OutgoingBuffer.AppendData(packet.Ethernet.IpV4.Tcp.Payload.ToArray<byte>());

            while (OutgoingBuffer.IsPacketAvailable())
            {
                int end = OutgoingBuffer.Position;
                end += OutgoingBuffer.ReadInt(32) * 8;
                while ((end - OutgoingBuffer.Position) >= 9)
                {
                    var msg = OutgoingBuffer.ParseMessage();
                    if (msg == null) continue;

                    Logger.LogOutgoingPacket(msg);
                }

                OutgoingBuffer.Position = end;
            }

            OutgoingBuffer.ConsumeData();
        }

        private static void DispatcherHandler(Packet packet)
        {
            if (packet.Ethernet.IpV4.Tcp == null) return;
            if (packet.Ethernet.IpV4.Tcp.Payload == null) return;
            if (packet.Ethernet.IpV4.Tcp.Payload.Length == 0) return;

            Console.Write(".");

            if (packet.Ethernet.IpV4.Destination.ToString().StartsWith(GameServerIPPattern)) HandleIncomingPacket(packet);
            else HandleOutgoingPacket(packet);
        }
    }
}
