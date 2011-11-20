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
using System.Reflection;
using Mooege.Net.GS.Message;

namespace GameMessageViewer
{
    public partial class MessageFilter : Form
    {
        public Dictionary<string, bool> Filter = new Dictionary<string, bool>();


        public MessageFilter()
        {
            InitializeComponent();

            List<CheckBox> boxes = new List<CheckBox>();

            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                foreach (Type type in assembly.GetTypes())
                    if (type.BaseType == typeof(GameMessage))
                    {
                        CheckBox chk = new CheckBox();
                        chk.Tag = type.Name;
                        chk.Text = type.Name;
                        chk.AutoSize = true;
                        boxes.Add(chk);
                    }

            boxes.Sort((a, b) => a.Text.CompareTo(b.Text));

            int itemsPerRow = 6;

            int count = 0;
            foreach (CheckBox c in boxes)
            {
                bool selected;
                if (Filter.TryGetValue(c.Text, out selected))
                    c.Checked = selected;
                else
                    c.Checked = true;

                c.Left = 20 + 220 * (count % itemsPerRow);
                c.Top = 20 + 20 * (count++ / itemsPerRow);
                Controls.Add(c);
            }

            int Width = 40 + itemsPerRow * 220;
            int Height = 60 + cmdOk.Height + (count - 1) / itemsPerRow * 20;
            this.ClientSize = new System.Drawing.Size(Width, Height);


            cmdAll.Top = this.ClientSize.Height - cmdAll.Height - 20;
            cmdNone.Top = this.ClientSize.Height - cmdNone.Height - 20;

            cmdOk.Left = this.ClientSize.Width - cmdOk.Width - 20;
            cmdOk.Top = this.ClientSize.Height - cmdOk.Height - 20;

            foreach (Control c in this.Controls)
                if (c is CheckBox)
                    Filter[c.Text] = true;

        }

        new public void Show()
        {

            this.ShowDialog();
        }

        private void cmdOk_Click(object sender, EventArgs e)
        {
            foreach (Control c in this.Controls)
                if (c is CheckBox)
                    Filter[c.Text] = (c as CheckBox).Checked;

            this.Close();
        }

        private void cmdAll_Click(object sender, EventArgs e)
        {
            foreach (Control c in this.Controls)
                if (c is CheckBox)
                    (c as CheckBox).Checked = true;
        }

        private void cmdNone_Click(object sender, EventArgs e)
        {
            foreach (Control c in this.Controls)
                if (c is CheckBox)
                    (c as CheckBox).Checked = false;
        }
    }
}
