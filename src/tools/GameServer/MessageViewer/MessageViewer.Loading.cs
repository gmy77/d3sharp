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

        /// <summary>
        /// Hex string to byte array
        /// </summary>
        private byte[] String_To_Bytes(string strInput)
        {
            int i = 0;
            int x = 0;
            byte[] bytes = new byte[(strInput.Length) / 2];

            while (strInput.Length > i + 1)
            {
                long lngDecimal = Convert.ToInt32(strInput.Substring(i, 2), 16);
                bytes[x] = Convert.ToByte(lngDecimal);
                i = i + 2;
                ++x;
            }
            return bytes;
        }

        /// <summary>
        /// string (char array) to hex string
        /// </summary>
        private string Encode(string text)
        {
            return BitConverter.ToString(System.Text.Encoding.UTF8.GetBytes(text)).Replace("-", "");
        }

        /// <summary>
        /// Returns whether a given hex stream is a moonet stream
        /// </summary>
        public bool IsMooNetStream(string stream)
        {
            return stream.Contains(Encode("Aurora"));
        }

        /// <summary>
        /// Returns whether a given hex stream is an achievment stream
        /// </summary>
        public bool IsAchievmentStream(string stream)
        {
            return stream.Contains(Encode("Achievements_Beta:RetrieveNameFromStringlist"));
        }

        /// <summary>
        /// Returns the protocol version for a given stream if the information is available
        /// Only GS streams have that version set (i guess/hope)
        /// </summary>
        public string GetVersion(string stream)
        {
            string[] versions = new string[] { "0.5.1.8101", "0.4.0.7865", "0.3.1.7779", "0.3.0.7484", "0.3.0.7333" };


            foreach (string version in versions)
                if(stream.Contains(Encode(version)))
                    return version;

            return "unknown";
        }

        public bool HasVersion(string version, string stream)
        {
            if (stream.Contains(Encode(version)))
                return true;

            return false;
        }


        //private void LoadWiresharkHex(string text)
        //{
        //    if (text.Contains(" "))
        //    {
        //        String[] rows = text.Split('\n');
        //        String currentBuffer = "";
        //        text = "";

        //        for (int i = 0; i < rows.Length; i++)
        //        {
        //            if (i > 0 && (rows[i].StartsWith(" ") ^ rows[i - 1].StartsWith(" ")))
        //            {
        //                Buffer buffer = new Buffer(String_To_Bytes(currentBuffer));

        //                BufferNode newNode = new BufferNode(buffer, actors, questTree, "1");
        //                newNode.Start = text.Length;
        //                newNode.BackColor = rows[i].StartsWith(" ") ? newNode.BackColor = Color.LightCoral : Color.LightBlue;
        //                tree.Nodes.Add(newNode);
        //                text += currentBuffer;
        //                currentBuffer = "";
        //            }

        //            currentBuffer += (rows[i].StartsWith(" ") ? rows[i].Substring(14, 3 * 16) : rows[i].Substring(10, 3 * 16)).Trim().Replace(" ", "");
        //        }
        //    }

        //    else
        //    {
        //        Buffer buffer = new Buffer(String_To_Bytes(text));
        //        BufferNode newNode = new BufferNode(buffer, actors, questTree, "1");
        //        newNode.Parse();
        //        tree.Nodes.Add(newNode);
        //    }

        //    ApplyFilter();
        //}


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
                List<string> gsStreams = new List<string>();

                // sort the streams
                foreach (var stream in streams)
                {
                    string text = new StreamReader(stream).ReadToEnd();

                    // TODO Implement MooNet Parsing
                    // if (IsMooNetStream(text)) LoadMooNetDump(text);
                    if (IsAchievmentStream(text)) System.Console.WriteLine("Achievementstream not parsed");

                    if (!IsMooNetStream(text) && !IsAchievmentStream(text))
                        gsStreams.Add(text);
                }

                // sometimes there are other streams in the dump...
                // if only one stream is found, or more are found but only one is tagged with mooege protocol version load that one
                if (gsStreams.Count > 0)
                {
                    var correct = gsStreams.Where(x => HasVersion(Mooege.Common.Versions.VersionInfo.Ingame.MajorVersion,x)).ToList();

                    if (correct.Count() > 0)
                    {
                        if (correct.Count() == 1)
                            LoadDump(correct.First());
                        else
                        {
                            Streams StreamDialog = new Streams(correct);
                            if (StreamDialog.ShowDialog() == DialogResult.OK)
                            {
                                LoadDump(correct[StreamDialog.stream]);
                            }
                        }
                    }
                    else
                    {
                        if (gsStreams.Count() == 1)
                            LoadDump(gsStreams.First());
                        else
                        {
                            if (DialogResult.Yes == MessageBox.Show(string.Format("The dump contains more than one unidentified stream, but in none of them mooege version {0} was found. The dump is either broken or of a version Mooege does not support. Continue loading all streams? (This may take longer and messages may appear broken)", Mooege.Common.Versions.VersionInfo.Ingame.VersionString), "Multiple streams found", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                                foreach (var stream in gsStreams)
                                    LoadDump(stream);
                        }
                    }
                }

                if(streams.Count == 0)
                    MessageBox.Show("No streams found. Is this really a GS / MooNet Dump?", "FatFingerFileMisclick", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    var packet = new PacketIn(null, null);

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
            string currentDirection = "";
            progressBar.Maximum = rows.Length;
            progressBar.Value = 0;
            progressBar.Visible = true;
            allNodes = new List<BufferNode>();

            Dictionary<string, TreeNode> actors = new Dictionary<string, TreeNode>();
            Dictionary<string, TreeNode> quests = new Dictionary<string, TreeNode>();



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
            int counter = 0;
            int size = 0;
            Dictionary<string, BufferNode> lastNodesMissingData = new Dictionary<string, BufferNode>();
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
                        lastNodesMissingData.Add(clientHash + "I", null);
                        lastNodesMissingData.Add(clientHash + "O", null);
                        colors[clientHash] = trafficColors[clients.Count - 1];
                    }

                    progressBar.Value = i;

                    if (rows[i].Length > 3)
                    {
                        // Everytime the direction of data changes, the buffer is parsed and emptied
                        // this is for pcap dumps where the data stream is sent in smaller packets
                        // in mooege, data is dumped in whole
                        if (i > 0 && rows[i].Substring(0, 1) != currentDirection)
                        {
                            byte[] buf = new byte[size / 2];
                            size = 0;
                            for(int k = i - counter; k < i; k++)
                            {
                                Array.Copy(String_To_Bytes(rows[k]), 0, buf, size / 2, rows[k].Length / 2); 
                                size += rows[k].Length;
                            }

                            if (lastNodesMissingData[clientHash + currentDirection] == null)
                            {
                                BufferNode newNode = new BufferNode(actors, quests, clientHash);

                                if (newNode.Append(buf))
                                    lastNodesMissingData[clientHash + currentDirection] = newNode;
                                else
                                    lastNodesMissingData[clientHash + currentDirection] = null;

                                newNode.BackColor = currentDirection == "I" ? colors[clientHash][0] : colors[clientHash][1];
                                allNodes.Add(newNode);
                            }
                            else
                            {
                                if (false == lastNodesMissingData[clientHash + currentDirection].Append(buf))
                                    lastNodesMissingData[clientHash + currentDirection] = null;
                            }


                            counter = 0;
                            size = 0;
                            currentDirection = rows[i].Substring(0, 1);
                        }

                        if (currentDirection == "") currentDirection = rows[i].Substring(0, 1);
                        rows[i] = rows[i].Substring(13).Replace("\r", "");
                        counter++;
                        size += rows[i].Length;
                    }
                }
            }


            if (counter > 0)
            {
                byte[] buf = new byte[size / 2];
                size = 0;
                for (int k = rows.Length - counter; k < rows.Length; k++)
                {
                    Array.Copy(String_To_Bytes(rows[k]), 0, buf, size / 2, rows[k].Length / 2);
                    size += rows[k].Length;
                }

                BufferNode newNode = new BufferNode(actors, quests, clientHash);
                newNode.Append(buf);
                newNode.BackColor = currentDirection == "I" ? colors[clientHash][0] : colors[clientHash][1];
                allNodes.Add(newNode);
            }

            foreach(BufferNode bn in allNodes)
                bn.ApplyFilter(filterWindow.Filter);


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


            questTree.Nodes.AddRange(quests.Values.ToArray());
            this.actors.Nodes.AddRange(actors.Values.ToArray());


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
