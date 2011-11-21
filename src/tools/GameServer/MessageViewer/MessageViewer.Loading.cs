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

#pragma warning disable 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using Mooege.Net.GS.Message.Definitions.Player;
using Google.ProtocolBuffers;
using Mooege.Net.MooNet.Packets;
using Mooege.Core.MooNet.Services;
using Mooege.Net.MooNet;
using Mooege.Net.MooNet.RPC;

namespace GameMessageViewer
{
    public partial class MessageViewer
    {

        private byte[] String_To_Bytes(string strInput)
        {
            int i = 0;
            int x = 0;
            byte[] bytes = new byte[(strInput.Length) / 2];
            // loop through the string - 2 bytes at a time converting  
            //  it to decimal equivalent and store in byte array  
            while (strInput.Length > i + 1)
            {
                long lngDecimal = Convert.ToInt32(strInput.Substring(i, 2), 16);
                bytes[x] = Convert.ToByte(lngDecimal);
                i = i + 2;
                ++x;
            }
            return bytes;
        }


                        //        string b = Encoding.UTF8.GetString(buffer.Data, 0, buffer.Length / 8);
                        //if (b.Contains("0.3.1.7779") ||
                        //    b.Contains("0.3.0.7484") ||
                        //    b.Contains("0.3.0.7333"))
                        //{
                        //    MessageBox.Show("The dump version ({0}) is incompatible to the currently used mooege version ({1})\n Loading continues but it may take significantly longer and many messages will not be parsed correctly");
                        //}


        private void LoadWiresharkHex(string text)
        {
            if (text.Contains(" "))
            {
                String[] rows = text.Split('\n');
                String currentBuffer = "";
                text = "";

                for (int i = 0; i < rows.Length; i++)
                {
                    if (i > 0 && (rows[i].StartsWith(" ") ^ rows[i - 1].StartsWith(" ")))
                    {
                        Buffer buffer = new Buffer(String_To_Bytes(currentBuffer));

                        BufferNode newNode = new BufferNode(buffer, actors, questTree, "1");
                        newNode.Start = text.Length;
                        newNode.BackColor = rows[i].StartsWith(" ") ? newNode.BackColor = Color.LightCoral : Color.LightBlue;
                        tree.Nodes.Add(newNode);
                        text += currentBuffer;
                        currentBuffer = "";
                    }

                    currentBuffer += (rows[i].StartsWith(" ") ? rows[i].Substring(14, 3 * 16) : rows[i].Substring(10, 3 * 16)).Trim().Replace(" ", "");
                }
            }

            else
            {
                Buffer buffer = new Buffer(String_To_Bytes(text));
                BufferNode newNode = new BufferNode(buffer, actors, questTree, "1");
                newNode.Parse();
                tree.Nodes.Add(newNode);
            }

            ApplyFilter();
        }


        public bool IsMooNetStream(string stream)
        {
            return stream.Contains(BitConverter.ToString(System.Text.Encoding.UTF8.GetBytes("Aurora")).Replace("-", ""));
        }

        public bool IsAchievmentStream(string stream)
        {
            return stream.Contains(BitConverter.ToString(System.Text.Encoding.UTF8.GetBytes("Achievements_Beta:RetrieveNameFromStringlist")).Replace("-", ""));
        }



        /// <summary>
        /// Pcap loading extracts the largest session from a pcap, transforms it to dump format (in a memory stream)
        /// which can be read by LoadDump afterwards
        /// </summary>
        /// <param name="path"></param>
        private void LoadPcap(string path)
        {
            try
            {
                // This ugly thing returns a list of MemoryStreams. One for each session in the cap
                var streams = pCapReader.ReconSingleFileSharpPcap(path);

                foreach (var stream in streams)
                {
                    stream.Seek(0, SeekOrigin.Begin);
                    string text = new StreamReader(stream).ReadToEnd();

                    if (IsMooNetStream(text))
                    {
                        LoadMooNetDump(text);
                        continue;
                    }
                    if (IsAchievmentStream(text))
                    {
                        System.Console.WriteLine("Achievementstream not parsed");
                        continue;
                    }

                    LoadDump(text);
                }

            }
            catch (SharpPcap.PcapException)
            {
                MessageBox.Show("The file could not be read. Only lipcap cap files can be loaded. If you want to load a NetMon cap the README tells you how to", "Loading error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void LoadPreparsed(string text)
        {
            String[] rows = text.Split('\n');
            String currentBuffer = "";
            text = "";
            string currentDirection = "";
            progressBar.Maximum = rows.Length;
            progressBar.Value = 0;
            progressBar.Visible = true;

            for (int i = 0; i < rows.Length; i++)
            {
                progressBar.Value = i;
                Application.DoEvents();
                if (this.Visible == false)
                    break;

            }

        }


        private static readonly Dictionary<uint, MooNetCallNode> RPCCallbacksIn = new Dictionary<uint, MooNetCallNode>();
        private static readonly Dictionary<uint, MooNetCallNode> RPCCallbacksOut = new Dictionary<uint, MooNetCallNode>();
        

        private void BNetBufferAssembledCallback(byte[] buffer, Direction direction, string clientHash)
        {
            Dictionary<string, Color[]> colors = new Dictionary<string, Color[]>();


            Color[][] trafficColors = new Color[][]
            { new Color[] { Color.LightCoral , Color.LightBlue },
              new Color[] { Color.Tomato, Color.Blue },
              new Color[] { Color.Red, Color.BlueViolet },
              new Color[] { Color.PaleVioletRed, Color.CadetBlue } 
            };


            var stream = CodedInputStream.CreateInstance(buffer);
            while (!stream.IsAtEnd)
            {
                try
                {
                    var packet = new PacketIn(null, stream);

                    if (packet.Header.ServiceId == 0xFE /*ServiceReply*/)
                    {
                        //ProcessReply(client, packet);
                        var callNode = RPCCallbacksIn[packet.Header.Token];
                        RPCCallbacksIn.Remove(packet.Header.Token);
                        callNode.ReceiveReply(packet, true);
                    }
                    else
                    {
                        var service = Service.GetByID(packet.Header.ServiceId);

                        if (service == null)
                        {
                            MooNetTree.Nodes.Add(String.Format("Service not found: {0}", service));
                            return;
                        }

                        var newNode = new MooNetCallNode(packet, stream);
                        MooNetTree.Nodes.Add(newNode);
                        RPCCallbacksIn.Add(packet.Header.Token, newNode);
                    }
                }
                catch (Exception e)
                {
                    var newNode = new TreeNode(String.Format("Error parsing {0}", e.Message));
                    MooNetTree.Nodes.Add(newNode);
                
                }
            }
        }

        private void LoadMooNetDump(string text)
        {
            AssembleBuffer(text, BNetBufferAssembledCallback);
        }


        private enum Direction
        {
            Incoming,
            Outgoing
        }

        private void AssembleBuffer(string text, Action<byte[], Direction, string> bufferAssembledCallback)
        {
            String[] rows = text.Split('\n');
            string currentDirection = "";
            String currentBuffer = "";
            List<string> clients = new List<string>();
            string clientHash = "";

            // to read mooege dumps, some leading info must be removed
            // the amount of chars is fixed so its calculated once
            int removeChars = rows[0].IndexOf("Inc:");
            if (removeChars < 0)
                removeChars = rows[0].IndexOf("Out:");

            for (int i = 0; i < rows.Length; i++)
            {

                // Skip anything til the Inc/Out part (for mooege dumps), note client hash
                rows[i] = rows[i].Substring(removeChars);
                if (rows[i].Length < 8) continue;           // no content after incoming/outgoing

                clientHash = rows[i].Substring(4, 8);
                if (clients.Contains(clientHash) == false)
                {
                    clients.Add(clientHash);
                }

                if (rows[i].Length > 3)
                {
                    // Everytime the direction of data changes, the buffer is parsed and emptied
                    // this is for pcap dumps where the data stream is sent in smaller packets
                    // in mooege, data is dumped in whole
                    if (i > 0 && rows[i].Substring(0, 1) != currentDirection)
                    {
                        bufferAssembledCallback(String_To_Bytes(currentBuffer), currentDirection == "I" ? Direction.Incoming : Direction.Outgoing, clientHash);
                        
                        currentBuffer = "";
                        currentDirection = rows[i].Substring(0, 1);
                    }
                    if (currentDirection == "") currentDirection = rows[i].Substring(0, 1);
                    currentBuffer += (rows[i].Substring(13)).Replace("\r", "");
                }
            }
        }


        private void LoadDump(string text)
        {
            String[] rows = text.Split('\n');
            String currentBuffer = "";
            text = "";
            string currentDirection = "";
            progressBar.Maximum = rows.Length;
            progressBar.Value = 0;
            progressBar.Visible = true;
            allNodes = new List<BufferNode>();

            Color[][] trafficColors = new Color[][]
            { new Color[] { Color.LightCoral , Color.LightBlue },
              new Color[] { Color.Tomato, Color.Blue },
              new Color[] { Color.Red, Color.BlueViolet },
              new Color[] { Color.PaleVioletRed, Color.CadetBlue } 
            };


            List<string> clients = new List<string>();
            Dictionary<string, Color[]> colors = new Dictionary<string,Color[]>();
        
            // to read mooege dumps, some leading info must be removed
            // the amount of chars is fixed so its calculated once
            int removeChars = rows[0].IndexOf("Inc:");
            if(removeChars < 0)
                removeChars = rows[0].IndexOf("Out:");
            string clientHash = "";
            for (int i = 0; i < rows.Length; i++)
            {
                if (rows[i].Length > removeChars)
                {
                    // Skip anything til the Inc/Out part (for mooege dumps), note client hash
                    rows[i] = rows[i].Substring(removeChars);
                    clientHash = rows[i].Substring(4, 8);
                    if (clients.Contains(clientHash) == false)
                    {
                        clients.Add(clientHash);
                        colors[clientHash] = trafficColors[clients.Count - 1];
                    }

                    progressBar.Value = i;

                    //causes bugs
                    //Application.DoEvents();
                    //if (this.Visible == false)
                    //    break;

                    if (rows[i].Length > 3)
                    {
                        // Everytime the direction of data changes, the buffer is parsed and emptied
                        // this is for pcap dumps where the data stream is sent in smaller packets
                        // in mooege, data is dumped in whole
                        if (i > 0 && rows[i].Substring(0, 1) != currentDirection)
                        {
                            Buffer buffer = new Buffer(String_To_Bytes(currentBuffer));
                            BufferNode newNode = new BufferNode(buffer, actors, questTree, clientHash);
                            newNode.Start = text.Length;
                            newNode.BackColor = currentDirection == "I" ? colors[clientHash][0] : colors[clientHash][1];
                            allNodes.Add(newNode);
                            //tree.Nodes.Add(newNode);
                            newNode.ApplyFilter(filterWindow.Filter);
                            text += currentBuffer;
                            currentBuffer = "";
                            currentDirection = rows[i].Substring(0, 1);
                        }

                        if (currentDirection == "") currentDirection = rows[i].Substring(0, 1);
                        currentBuffer += (rows[i].Substring(13)).Replace("\r", "");
                    }
                }
            }


            if (currentBuffer.Length > 10)
            {
                Buffer buffer = new Buffer(String_To_Bytes(currentBuffer));
                BufferNode newNode = new BufferNode(buffer, actors, questTree, clientHash);
                newNode.Start = text.Length;
                newNode.BackColor = currentDirection == "I" ? colors[clientHash][0] : colors[clientHash][1];
                allNodes.Add(newNode);
                newNode.ApplyFilter(filterWindow.Filter);
                text += currentBuffer;
            }




            // Create a filter menu entry for every client in the stream.
            filterPlayersToolStripMenuItem.DropDownItems.Clear();
            foreach (string client in clients)
            {
                ToolStripMenuItem m = new ToolStripMenuItem(client);
                m.Tag = client;
                m.Checked = true;
                m.Click += new EventHandler(FilterPlayerClick);

                // find toon name for client hash
                foreach(BufferNode bn in allNodes)
                    if(bn.clientHash.Equals(m.Tag.ToString()))
                        foreach(TreeNode mn in bn.Nodes)
                            if(mn is MessageNode)
                            if((mn as MessageNode).gameMessage is NewPlayerMessage)
                            {
                                m.Text = ((mn as MessageNode).gameMessage as NewPlayerMessage).ToonName;
                                goto hell;
                            }

            hell:
                filterPlayersToolStripMenuItem.DropDownItems.Add(m);
            }


            tree.Nodes.AddRange(allNodes.ToArray());
            progressBar.Visible = false;
        }

        void FilterPlayerClick(object sender, EventArgs e)
        {
            Dictionary<string, bool> filter = new Dictionary<string, bool>();
            ((ToolStripMenuItem)sender).Checked = !((ToolStripMenuItem)sender).Checked;

            foreach (ToolStripMenuItem m in filterPlayersToolStripMenuItem.DropDownItems)
                filter.Add(m.Tag.ToString(), m.Checked);

            tree.Nodes.Clear();
            foreach (BufferNode bn in allNodes)
                if (filter[bn.clientHash])
                    tree.Nodes.Add(bn);

        }
    }
}
