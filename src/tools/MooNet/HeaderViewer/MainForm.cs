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
using System.Windows.Forms;
using Google.ProtocolBuffers;
using Mooege.Tools.Helpers;

namespace Mooege.Tools.HeaderViewer
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void inputBox_TextChanged(object sender, EventArgs e)
        {
            ParseBytes(inputBox.Text.Trim().Replace(" ", String.Empty));
        }

        private void ParseBytes(string bytestring)
        {
            if (bytestring.Length % 2 == 0 && bytestring.Length >= 10)
            {
                byte[] array = new byte[bytestring.Length / 2];
                for (int i = 0, j = 0; i < bytestring.Length - 1; i+=2, ++j)
                {
                    int b = (Conversion.ParseDigit(bytestring[i]) * 16)
                           + Conversion.ParseDigit(bytestring[i+1]);
                    array[j] = (byte)b;
                }
                ParseHeader(array);
            }
            else
            {
                outputBox.Text = "Not enough characters";
            }
        }

        private void ParseHeader(byte[] array)
        {
            CodedInputStream stream = CodedInputStream.CreateInstance(array);
            var serviceId = stream.ReadRawByte();
            var methodId = stream.ReadRawVarint32();
            var requestId = stream.ReadRawByte() | (stream.ReadRawByte() << 8);
            bool hasObjectId = false;
            ulong objectId = 0;
            if (serviceId != 0xfe)
            {
                hasObjectId = true;
                objectId = stream.ReadRawVarint64();
            }
            var payloadLength = stream.ReadRawVarint32();
            outputBox.ResetText();
            outputBox.Text = String.Format(
                "ServiceID = {0}\r\n"
              + "MethodID  = {1}\r\n"
              + "RequestID = {2}\r\n"
              + "ObjectID  = {3}\r\n"
              + "PayloadLength  = {4}\r\n"
              , serviceId, methodId, requestId, (hasObjectId) ? objectId.ToString() : "none", payloadLength);
        }
    }
}
