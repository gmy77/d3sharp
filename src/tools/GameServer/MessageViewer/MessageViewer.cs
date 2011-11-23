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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Mooege.Net.GS.Message;
using Mooege.Net.GS.Message.Definitions.ACD;
using System.IO;
using SharpPcap.LibPcap;
using SharpPcap;
using PacketDotNet;
using Mooege.Common.MPQ;
using Mooege.Core.GS.Common.Types.SNO;

namespace GameMessageViewer
{
    public partial class MessageViewer : Form
    {
        MessageFilter filterWindow = new MessageFilter();
        RichTextBox temp = new RichTextBox();
        List<BufferNode> allNodes = new List<BufferNode>();


        public MessageViewer()
        {
            InitializeComponent();
        }


        private void tree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if ((sender as TreeView).SelectedNode is ITextNode)
                DisplayMessage(((sender as TreeView).SelectedNode as ITextNode).AsText());
        }

        private void ApplyFilter()
        {
            tree.BeginUpdate();
            progressBar.Visible = true;
            progressBar.Value = 0;
            progressBar.Maximum = tree.Nodes.Count;

            foreach (BufferNode b in tree.Nodes)
            {
                progressBar.Value++;
                b.ApplyFilter(filterWindow.Filter);
            }

            progressBar.Visible = false;
            tree.EndUpdate();
        }


        private void tree_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node is BufferNode)
                (e.Node as BufferNode).Parse();
        }

        private void groupedNode_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node is MessageNode)
                DisplayMessage((e.Node as MessageNode).gameMessage.AsText());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (TreeNode n in tree.Nodes)
                if (n is BufferNode)
                    (n as BufferNode).Parse();
        }

        private void messageFilterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            filterWindow.ShowDialog();
            ApplyFilter();
        }


        private void findAllMessagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Find filter = new Find();

            if((filter.ShowDialog() != DialogResult.OK))
                return;

            string find = filter.Filter;
            tree.BeginUpdate();

            foreach (BufferNode bn in tree.Nodes)
            {
                bn.Collapse();

                foreach (TreeNode mn in bn.Nodes)
                    if (mn is MessageNode)
                    {
                        if ((mn as MessageNode).gameMessage.GetType().Name.Contains(find))
                        {
                            bn.BackColor = Color.Yellow;
                            mn.BackColor = Color.Yellow;
                            bn.Expand();
                        }
                    }
            }
            tree.EndUpdate();
        }

        /// <summary>
        /// Close application
        /// </summary>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            (new AboutBox()).Show();
        }


        private void output_MouseMove(object sender, MouseEventArgs e)
        {
            int i = temp.GetCharIndexFromPosition(new Point(e.X, e.Y));
            temp.SelectionStart = i;
            temp.SelectionLength = 1;

            // Apparently, under mono SelectionFont property crashes when nothing is selected
            if (temp.SelectionType == RichTextBoxSelectionTypes.Empty)
                return;

            if (temp.SelectionFont.Underline)
            {
                if (output.Cursor != Cursors.Hand)
                    output.Cursor = Cursors.Hand;
            }
            else
                output.Cursor = Cursors.IBeam;
        }

        private void output_MouseClick(object sender, MouseEventArgs e)
        {
            int i = temp.GetCharIndexFromPosition(new Point(e.X, e.Y));

            temp.SelectionStart = i;
            temp.SelectionLength = 1;

            // Apparently, under mono SelectionFont property crashes when nothing is selected
            if (temp.SelectionType == RichTextBoxSelectionTypes.Empty)
                return;

            while (temp.SelectionFont.Underline)
                temp.SelectionStart--;
            temp.SelectionStart++;
            while (!temp.SelectedText.Contains(" ") && temp.SelectionStart + temp.SelectionLength < temp.Text.Length)
                temp.SelectionLength++;

            string text = temp.SelectedText;

            FindActor(text.Trim());
        }

        public void FindActor(string id)
        {
            foreach (TreeNode node in actors.Nodes)
                if (node.Tag as string == id)
                {
                    node.Expand();
                    node.BackColor = Color.Yellow;
                    node.EnsureVisible();
                    this.tabControl1.SelectTab(0);
                } else
                    node.BackColor = Color.White;
        }

        /// <summary>
        /// Underscore actor ids and add sno names
        /// </summary>
        private void DisplayMessage(string text)
        {
            temp.Text = text;
            output.Rtf = temp.Rtf;
            foreach (TreeNode tn in actors.Nodes)
            {
                int pos = temp.Find(tn.Tag as string);

                if (pos > -1)
                {
                    temp.Rtf = temp.Rtf.Replace(tn.Tag as string, tn.Text);
                    temp.Select(pos, tn.Text.Length);
                    temp.SelectionFont = new Font(output.Font, FontStyle.Underline);
                    temp.SelectionColor = Color.Blue;
                }
            }

            temp.Size = output.Size;
            temp.Location = output.Location;
            output.Rtf = temp.Rtf;

            string[] words = output.Text.Split(new string[] { "0x", " ", "\n", ", " }, StringSplitOptions.RemoveEmptyEntries); // .Split(' ');
            List<string> usedKeys = new List<string>();

            // Bruteforce replacement of snos to their aliases
            if (trySNOAliasesToolStripMenuItem.Checked)
                foreach (string word in words)
                    if (word.Length > 5)  // "for hex values 0000FDC1 etc
                    {
                        try
                        {
                            //string raw = word.Replace("\n", "").Replace("0x", "");
                            int id = 0;
                            if (Int32.TryParse(word, System.Globalization.NumberStyles.HexNumber, null, out id))
                            {
                                if (usedKeys.Contains(id.ToString()) == false)
                                {
                                    usedKeys.Add(id.ToString());
                                    string alias = "";
                                    
                                    var aliases = Mooege.Core.GS.Common.Types.TagMap.TagMap.GetKeys(id);
                                    if(aliases.Count > 0)
                                    {
                                        alias = String.Join(" or ", aliases.Select(x => x.Name));

                                        output.Rtf = output.Rtf.Replace(word, word + ": TagKey." + alias);

                                        int pos = -1;
                                        while ((pos = output.Text.IndexOf(alias, pos + 1)) > 0)
                                        {
                                            output.SelectionStart = pos;
                                            output.SelectionLength = alias.Length;
                                            output.SelectionColor = Color.OrangeRed;
                                            output.SelectionLength = 0;
                                        }
                                    }

                                    alias = SNOAliases.GetAlias(id);
                                    if (alias != "")
                                    {
                                        output.Rtf = output.Rtf.Replace(word, word + ":" + alias);

                                        int pos = -1;
                                        while((pos = output.Text.IndexOf(alias, pos + 1)) > 0)
                                        {
                                        output.SelectionStart = pos;
                                        output.SelectionLength = alias.Length;
                                        output.SelectionColor = Color.OrangeRed;
                                        output.SelectionLength = 0;
                                        }

                                    }

                                    alias = SNOAliases.GetGroup(id);
                                    if (alias != "")
                                    {
                                        output.Rtf = output.Rtf.Replace(word, word + ":" + alias);

                                        int pos = -1;
                                        while ((pos = output.Text.IndexOf(alias, pos + 1)) > 0)
                                        {
                                            output.SelectionStart = pos;
                                            output.SelectionLength = alias.Length;
                                            output.SelectionColor = Color.OrangeRed;
                                            output.SelectionLength = 0;
                                        }

                                    }
                                    


                                }
                            }
                        }
                        catch (Exception) { System.Diagnostics.Debugger.Break(); }
                    }
            output.Refresh();
        }

        private void openPreparsedDumpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Readable dumps |*.cap; *.pcap; *.log; *.hex|"+
                         "Libpcap dumpy (*.cap; *.pcap)|*.cap; *.pcap|"+
                         "Mooege dumps (*.log)|*.log|"+
                         "Wireshark hex view (*.hex)|*.hex";

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                questTree.Nodes.Clear();
                actors.Nodes.Clear();
                tree.Nodes.Clear();
                BufferNode.Reset();
                this.Text = Application.ProductName + " - " + Path.GetFileName(ofd.FileName);

                if(Path.GetExtension(ofd.FileName).ToLower().Contains("log"))
                    LoadDump(File.ReadAllText(ofd.FileName));
                if (Path.GetExtension(ofd.FileName).ToLower().Contains("cap"))
                    LoadPcap(ofd.FileName);
                //if (Path.GetExtension(ofd.FileName).ToLower().Contains("hex"))
                //    LoadWiresharkHex(File.ReadAllText(ofd.FileName));
            }
        }

        private void queryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CustomLinqQuery query = new CustomLinqQuery();
            if (query.Show(tree.Nodes.Cast<BufferNode>()) == System.Windows.Forms.DialogResult.OK)
            {
                TreeView treeQuery = new TreeView();
                treeQuery.BorderStyle = BorderStyle.None;
                treeQuery.Dock = DockStyle.Fill;
                treeQuery.AfterSelect += this.groupedNode_AfterSelect;
                TabPage tab = new TabPage("Query");
                tab.Controls.Add(treeQuery);
                tab.DoubleClick += (o,k) => MessageBox.Show("WERWER");

                treeQuery.Nodes.Clear();
                foreach (MessageNode mn in query.QueryResult)
                    treeQuery.Nodes.Add(mn.Clone());

                tabControl2.TabPages.Add(tab);
                tabControl2.SelectTab(tab);
            }
        }

        private void tabControl2_DoubleClick(object sender, EventArgs e)
        {
            if(tabControl2.SelectedIndex != 0)
                tabControl2.TabPages.Remove((TabPage)tabControl2.SelectedTab);
        }

    }
}
