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
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Google.ProtocolBuffers;
using Mooege.Tools.Helpers;
using System.Linq;

namespace Mooege.Tools.StringViewer
{
    public partial class FormMain : Form
    {
        private byte[] _byteData;
        private bool _arrayize=false;
        private System.Type[] protoTypes;

        public FormMain()
        {
            InitializeComponent();
            this.comboBoxOutputType.SelectedIndex = 0;

            var ProtoList = from types in Assembly.Load("LibMooNet").GetTypes()
                            where types.IsClass && types.Name != "Builder"
                            && types.Name != "Types" && types.Name != "Stub"
                            && types.IsAbstract == false && !types.Name.Contains("PrivateImplementation")
                            select types;
            protoTypes = ProtoList.ToArray();
        }

        private void UpdateProtoOutput()
        {
            string typename = textBoxProtoType.Text.Trim();
            if (typename.Trim() == string.Empty)
                return;
            var type = Type.GetType(typename + ", LibMooNet");
            if (type == null)
            {
                richTextBoxProto.Text = "(no type)";
                return;
            }

            try
            {
                var defaultMessage = MessageUtil.GetDefaultMessage(type);
                IBuilder builder = defaultMessage.WeakCreateBuilderForType();
                if (builder == null)
                {
                    richTextBoxProto.Text = "(no builder)";
                    return;
                }
                builder.WeakMergeFrom(ByteString.CopyFrom(_byteData));
                richTextBoxProto.Text = TextFormat.PrintToString(builder.WeakBuild());
            }
            catch (Exception ex)
            {
                richTextBoxProto.Text = ex.ToString();
            }
        }

        public void UpdateHexOutput()
        {
            this.hexBox.Text = _arrayize ? Utils.GetArrayRepresentation(_byteData) : _byteData.DumpHex(false);
        }

        public void UpdateOutputs()
        {
            UpdateHexOutput();
            UpdateProtoOutput();
        }

        private void textBoxInput_TextChanged(object sender, EventArgs e)
        {
            _byteData = Conversion.Unescape(this.textBoxInput.Text);
            // Only write when it changes
            File.WriteAllBytes("proto.raw", _byteData);
            UpdateOutputs();
        }

        private void textBoxProtoType_TextChanged(object sender, EventArgs e)
        {
            UpdateProtoOutput();
        }

        private void comboBoxOutputType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (textBoxInput.Text.Trim() == string.Empty) return;

            _arrayize = !_arrayize;
            UpdateHexOutput();
        }

        private void DetectProto()
        {
            cmdDetect.Enabled = false;
            cboProtos.Items.Clear();
            Application.UseWaitCursor = true;
            foreach (var proto in protoTypes)
            {
                Application.DoEvents();
                try
                {
                    var defaultMessage = MessageUtil.GetDefaultMessage(proto);
                    IBuilder builder = defaultMessage.WeakCreateBuilderForType();
                    builder.WeakMergeFrom(ByteString.CopyFrom(_byteData));
                    builder.WeakBuild();
                    if (builder.UnknownFields.SerializedSize == 0)
                        cboProtos.Items.Add(proto.FullName);
                }
                catch (Exception e)
                {
                    if (!e.Message.Contains("missing") && !e.Message.Contains("the middle")
                        && !e.Message.Contains("invalid wire") && !e.Message.Contains("invalid tag"))
                    {
                        MessageBox.Show("Detected possible problem:\n" + e.Message);
                    }
                }
            }
            if (cboProtos.Items.Count > 0)
            {
                cboProtos.SelectedIndex = 0;
            }
            else
                MessageBox.Show("String does not completely build into any valid prototype.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            Application.UseWaitCursor = false;
            cmdDetect.Enabled = true;
        }

        private void cboProtos_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBoxProtoType.Text = cboProtos.SelectedItem.ToString();
        }

        private void cmdDetect_Click(object sender, EventArgs e)
        {
            DetectProto();
        }
    }

    public static class Utils
    {
        public static string GetArrayRepresentation(byte[] array)
        {
            string bytes = string.Empty;
            for (int index = 0; index < array.Length; index++)
            {
                byte b = array[index];
                bytes = bytes + "0x" + b.ToString("X2");
                if (index != array.Length - 1) bytes += ", ";
            }
            return "var data = new byte[] { " + bytes + " };";
        }

    }
}
