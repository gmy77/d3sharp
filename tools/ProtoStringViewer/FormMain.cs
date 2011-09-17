using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Be.Windows.Forms;
using Google.ProtocolBuffers;

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
            this.hexBoxOutput.ByteProvider = new DynamicByteProvider(_byteData);
        }

        private void textBoxProtoType_TextChanged(object sender, EventArgs e)
        {
            if (textBoxProtoType.Text.Trim() == string.Empty) return;
            
            var type = Type.GetType(textBoxProtoType.Text);
            if (type == null)
            {
                return;
            }

            var defaultMessage = MessageUtil.GetDefaultMessage(type);

            IBuilder builder = defaultMessage.WeakCreateBuilderForType();
            if (builder == null)
            {
                return;
            }

            builder.WeakMergeFrom(ByteString.CopyFrom(_byteData));
            richTextBoxProto.Text = TextFormat.PrintToString(builder.WeakBuild());            
        }
    }
}
