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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpPcap.LibPcap;
using SharpPcap;
using PacketDotNet;
using System.IO;

namespace GameMessageViewer
{

    /// <summary>
    /// Class that identifies a connection within a pcap dump
    /// </summary>
    class Connection
    {
        private string m_srcIp;
        public string SourceIp
        {
            get { return m_srcIp; }
        }

        private ushort m_srcPort;
        public ushort SourcePort
        {
            get { return m_srcPort; }
        }

        private string m_dstIp;
        public string DestinationIp
        {
            get { return m_dstIp; }
        }

        private ushort m_dstPort;
        public ushort DestinationPort
        {
            get { return m_dstPort; }
        }

        public Connection(string sourceIP, UInt16 sourcePort, string destinationIP, UInt16 destinationPort)
        {
            m_srcIp = sourceIP;
            m_dstIp = destinationIP;
            m_srcPort = sourcePort;
            m_dstPort = destinationPort;
        }

        public Connection(PacketDotNet.TcpPacket packet)
        {
            m_srcIp = (packet.ParentPacket as PacketDotNet.IPv4Packet).SourceAddress.ToString();
            m_dstIp = (packet.ParentPacket as PacketDotNet.IPv4Packet).DestinationAddress.ToString();
            m_srcPort = (ushort)packet.SourcePort;
            m_dstPort = (ushort)packet.DestinationPort;
        }

        /// <summary>
        /// Overrided in order to catch both sides of the connection 
        /// with the same connection object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Connection))
                return false;
            Connection con = (Connection)obj;

            bool result = ((con.SourceIp.Equals(m_srcIp)) && (con.SourcePort == m_srcPort) && (con.DestinationIp.Equals(m_dstIp)) && (con.DestinationPort == m_dstPort)) ||
                ((con.SourceIp.Equals(m_dstIp)) && (con.SourcePort == m_dstPort) && (con.DestinationIp.Equals(m_srcIp)) && (con.DestinationPort == m_srcPort));

            return result;
        }

        public override int GetHashCode()
        {
            return ((m_srcIp.GetHashCode() ^ m_srcPort.GetHashCode()) as object).GetHashCode() ^
                ((m_dstIp.GetHashCode() ^ m_dstPort.GetHashCode()) as object).GetHashCode();
        }

        public string getFileName(string path)
        {
            return string.Format("{0}{1}.{2}-{3}.{4}.tmp", path, m_srcIp, m_srcPort, m_dstIp, m_dstPort);
        }
    }





    class pCapReader
    {
        /// <summary>
        /// Reconstruct a Pcap file using TcpRecon class
        /// </summary>
        public static List<MemoryStream> ReconSingleFileSharpPcap(string capFile)
        {
            var capture = new CaptureFileReaderDevice(capFile);
            var retVal = new List<MemoryStream>();

            //Register our handler function to the 'packet arrival' event
            capture.OnPacketArrival +=
                new SharpPcap.PacketArrivalEventHandler(device_PcapOnPacketArrival);

            //Start capture 'INFINTE' number of packets
            //This method will return when EOF reached.
            capture.Capture();

            //Close the pcap device
            capture.Close();

            // Clean up
            foreach (TcpRecon tr in sharpPcapDict.Values)
            {
                retVal.Add(tr.data_out_file.BaseStream as MemoryStream);
                tr.Close();
                tr.data_out_file.BaseStream.Seek(0, SeekOrigin.Begin);
            }
            sharpPcapDict.Clear();
            return retVal;
        }



        // Holds the file streams for each tcp session in case we use SharpPcap
        static Dictionary<Connection, TcpRecon> sharpPcapDict = new Dictionary<Connection, TcpRecon>();
        static string path = "";

        // The callback function for the SharpPcap library
        private static void device_PcapOnPacketArrival(object sender, CaptureEventArgs e)
        {
            if (Packet.ParsePacket(LinkLayers.Ethernet, e.Packet.Data).PayloadPacket == null) return;

            TcpPacket tcpPacket = Packet.ParsePacket(LinkLayers.Ethernet, e.Packet.Data).PayloadPacket.PayloadPacket as TcpPacket;

            // THIS FILTERS D3 TRAFFIC, GS AS WELL AS MOONET
            if (tcpPacket != null && (tcpPacket.SourcePort == 1119 || tcpPacket.DestinationPort == 1119))
            {
                Connection c = new Connection(tcpPacket);
                if (!sharpPcapDict.ContainsKey(c))
                {
                    string fileName = c.getFileName(path);
                    TcpRecon tcpRecon = new TcpRecon(fileName);
                    sharpPcapDict.Add(c, tcpRecon);
                }

                // Use the TcpRecon class to reconstruct the session
                sharpPcapDict[c].ReassemblePacket(tcpPacket);
            }
        }
    }
}
