/*
 * Copyright (C) 2011 D3Sharp Project
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
using System.Windows.Forms;
using Google.ProtocolBuffers;
using ProtoHelpers.Utils;

namespace ProtoStringViewer
{
    public partial class FormMain : Form
    {
        private byte[] _byteData;
        private bool _arrayize=false;

        public FormMain()
        {
            InitializeComponent();
        }

        private void UpdateProtoOutput()
        {
            string typename = textBoxProtoType.Text;
            if (typename.Trim() == string.Empty)
                return;
            var type = Type.GetType(typename + ", D3Proto");
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
            _byteData = ProtoHelpers.Utils.Conversion.Unescape(this.textBoxInput.Text);
            // Only write when it changes
            File.WriteAllBytes("proto.raw", _byteData);
            UpdateOutputs();
        }

        private void textBoxProtoType_TextChanged(object sender, EventArgs e)
        {
            UpdateProtoOutput();
        }

        private void csharpizeButton_Click(object sender, EventArgs e)
        {
            _arrayize = !_arrayize;
            if (_arrayize)
                csharpizeButton.Text = "Outputting C# array";
            else
                csharpizeButton.Text = "Ouputting raw";
            UpdateHexOutput();
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
