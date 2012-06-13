/*
 * Copyright (C) 2011 - 2012 mooege project - http://www.mooege.org
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
using System.Drawing;
using System.Windows.Forms;
using PonyLib;

namespace PonyPatcher
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.Text = string.Format("Pony Patcher [v{0}]", VersionInfo.Client.RequiredClientVersion);
            this.buttonPatch.Enabled = false;

            try
            {
                if (Patcher.IsAlreadyPatched()) // check if process is already patched..
                {
                    labelStatus.Text = "Process is already patched!";
                    labelStatus.ForeColor = Color.Red;
                    buttonPatch.Enabled = false;
                }
                else if (Patcher.IsClientRunning()) // check if process is running..
                {
                    labelStatus.Text = "Client is running and ready for patching..";
                    labelStatus.ForeColor = Color.DarkGoldenrod;
                    buttonPatch.Enabled = true;
                }
                else
                {
                    labelStatus.Text = "Client is not running. Please run it first!";
                    labelStatus.ForeColor = Color.Red;
                    buttonPatch.Enabled = false;
                }
            }
            catch(Exception exception)
            {
                labelStatus.Text = exception.Message;
                labelStatus.ForeColor = Color.Red;
                buttonPatch.Enabled = false;
                Patcher.Cleanup(); // let LibPony to cleanup stuff.
            }
        }

        private void buttonPatch_Click(object sender, EventArgs e)
        {
            buttonPatch.Enabled = false;

            try
            {
                if (Patcher.Patch()) // try patching.
                {
                    labelStatus.Text = "Patch successful!";
                    labelStatus.ForeColor = Color.DarkGreen;
                }
                else
                {
                    labelStatus.Text = "Can not patch the client!";
                    labelStatus.ForeColor = Color.Red;
                }
            }
            catch (Exception exception) // catch LibPony exceptions.
            {
                labelStatus.Text = exception.Message;
                labelStatus.ForeColor = Color.Red;
            }
            finally
            {
                Patcher.Cleanup(); // we finally need to let LibPony to cleanup.
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Patcher.Cleanup(); // make sure LibPony cleanup's it's dirt.
        }

        private void buttonAbout_Click(object sender, EventArgs e)
        {
            var aboutForm = new AboutForm();
            aboutForm.ShowDialog();
        }
    }
}
