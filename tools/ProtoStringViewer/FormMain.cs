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

        public FormMain()
        {
            InitializeComponent();
        }

        private void textBoxInput_TextChanged(object sender, EventArgs e)
        {
            _byteData = ProtoHelpers.Utils.Conversion.Unescape(this.textBoxInput.Text);
            this.hexBox.Text = _byteData.DumpHex(false);
            File.WriteAllBytes("proto.raw", _byteData);
        }

        private void textBoxProtoType_TextChanged(object sender, EventArgs e)
        {
            if (textBoxProtoType.Text.Trim() == string.Empty)
                return;
            var type = Type.GetType(textBoxProtoType.Text);
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
    }
}
