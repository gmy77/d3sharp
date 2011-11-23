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
using System.Reflection;
using System.IO;


namespace GameMessageViewer
{
    public partial class CustomLinqQuery : Form
    {
        List<MessageNode> mNodes = new List<MessageNode>();
        List<BufferNode> bNodes = new List<BufferNode>();
        public IEnumerable<MessageNode> QueryResult = new List<MessageNode>();

        public CustomLinqQuery()
        {
            InitializeComponent();
        }

        class cboEntry
        {
            public Type type;
            public cboEntry(Type type) { this.type = type; }
            public override string ToString() { return type.Name; }
        }

        internal DialogResult Show(IEnumerable<BufferNode> nodes)
        {
            // Gather all Types in the AppDomain that inherit from GameMessage
            List<cboEntry> items = new List<cboEntry>();
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                foreach (Type type in assembly.GetTypes())
                    if (type.BaseType == typeof(GameMessage))
                        items.Add(new cboEntry(type));

            // show them in a combobox
            var sorted = items.OrderBy(x => x.ToString());
            foreach (cboEntry message in sorted)
                comboBox1.Items.Add(message);

            // Create a List of only MessageNodes
            foreach (BufferNode bn in nodes)
                foreach (TreeNode mn in bn.allNodes)
                    if(mn is MessageNode)
                        mNodes.Add(mn as MessageNode);

            comboBox1.SelectedIndex = 0;
            this.ShowDialog();
            return DialogResult;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //string f = "";
            //for (int i = 0; i < 40; i++)
            //{

                Type genericQuery = typeof(QueryTemplate<>);
                Type concreteQuery = genericQuery.MakeGenericType(((cboEntry)comboBox1.SelectedItem).type);
                object query = Activator.CreateInstance(concreteQuery);


                try
                {
                    QueryResult = (IEnumerable<MessageNode>)concreteQuery.GetMethod("Query").Invoke(query, new object[] { mNodes, textBox1.Text }); 
                    //QueryResult = (IEnumerable<MessageNode>)concreteQuery.GetMethod("Query").Invoke(query, new object[] { mNodes, "Field2==" + i });
                    QueryResult.Count(); // Force evaluation
                }
                catch (Exception exception)
                {
                    lblException.Text = exception.Message;
                    return;
                }
                //string s = i.ToString("X8") + "\r\n";
                //foreach (MessageNode mn in QueryResult)
                //    s += (mn.gameMessage as ACDEnterKnownMessage).ActorSNO + ":" + SNOAliases.Aliases[(mn.gameMessage as ACDEnterKnownMessage).ActorSNO.ToString()] + "\r\n";
                //f += "\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n" + s;
            //}

            //File.WriteAllText("F:\\out.txt", f);

            

            
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                button1_Click(sender, e);
        }
    }
}
