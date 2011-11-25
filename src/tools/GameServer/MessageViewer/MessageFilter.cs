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
        private class Preset
        {
            public string Name;
            public Dictionary<string, bool> Filter;
            public override string ToString() { return Name; }
        }


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
                        chk.Font = new Font(chk.Font.FontFamily, 7);
                        chk.Tag = type.Name;
                        chk.Text = type.Name;
                        chk.AutoSize = true;
                        boxes.Add(chk);
                    }

            boxes.Sort((a, b) => a.Text.CompareTo(b.Text));

            int itemsPerRow = 35;
            int count = 0;
            foreach (CheckBox c in boxes)
            {
                c.Left = 20 + 220 * (count / itemsPerRow);
                c.Top = 20 + 18 * (count++ % itemsPerRow);
                Controls.Add(c);
            }

            int Width = 40 + (count / itemsPerRow + 1) * 220;
            int Height = 100 + itemsPerRow * 18;
            this.ClientSize = new System.Drawing.Size(Width, Height);
            Presets.Top = this.ClientSize.Height - Presets.Height - 20;
            Presets.Left = 20;
            cmdOk.Left = this.ClientSize.Width - cmdOk.Width - 20;
            cmdOk.Top = this.ClientSize.Height - cmdOk.Height - 20;

            foreach (Preset p in CreatePresets())
                Presets.Items.Add(p);

            Presets.SelectedIndex = 0;

        }

        new public void Show(List<TreeNode> nodes)
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

        /// <summary>
        /// Create some preset filters
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Preset> CreatePresets()
        {
            Dictionary<string, bool> All = new Dictionary<string, bool>();
            foreach (Control c in this.Controls)
                if (c is CheckBox)
                    All[c.Text] = true;
            yield return new Preset() { Name = "All", Filter = All };

            Dictionary<string, bool> None = new Dictionary<string, bool>();
            foreach (Control c in this.Controls)
                if (c is CheckBox)
                    None[c.Text] = false;
            yield return new Preset() { Name = "None", Filter = None };

            Dictionary<string, bool> LessVerbose = All.Clone();
            LessVerbose["GameTickMessage"] = false;
            LessVerbose["TrickleMessage"] = false;
            LessVerbose["ACDTranslateFacingMessage"] = false;
            yield return new Preset() { Name = "Less verbose", Filter = LessVerbose };

            Dictionary<string, bool> Questing = None.Clone();
            Questing["QuestCounterMessage"] = true;
            Questing["QuestMeterMessage"] = true;
            Questing["QuestUpdateMessage"] = true;
            Questing["WorldTargetMessage"] = true;
            yield return new Preset() { Name = "Questing", Filter = Questing };

            Dictionary<string, bool> Conversation = None.Clone();
            Conversation["PlayConvLineMessage"] = true;
            Conversation["FinishConversationMessage"] = true;
            Conversation["EndConversationMessage"] = true;
            Conversation["RequestCloseConversationWindowMessage"] = true;
            Conversation["StopConvLineMessage"] = true;
            Conversation["WorldTargetMessage"] = true;

            yield return new Preset() { Name = "Conversation", Filter = Conversation };
 
        }

        private void Presets_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadPreset(((ComboBox)sender).SelectedItem as Preset);
        }

        /// <summary>
        /// Loads a preset and sets it as selected filter
        /// </summary>
        /// <param name="p"></param>
        private void LoadPreset(Preset p)
        {
            foreach (Control c in this.Controls)
                if (c is CheckBox)
                    (c as CheckBox).Checked = p.Filter[(c as CheckBox).Text];
            this.Filter = p.Filter;
        }
    }

    /// <summary>
    /// Extension method to clone dictionaries
    /// </summary>
    public static class DictionaryClone
    {
        public static Dictionary<string, bool> Clone(this Dictionary<string, bool> original)
        {
            Dictionary<string, bool> clone = new Dictionary<string, bool>();
            foreach (string key in original.Keys)
                clone.Add(key, original[key]);
            return clone;
        }
    }
}
