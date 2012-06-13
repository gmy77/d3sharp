using System;
using System.Drawing;
using System.Reflection;
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
            this.Text = string.Format("Pony Patcher [v{0}]", Assembly.GetExecutingAssembly().GetName().Version);
            this.buttonPatch.Enabled = false;

            try
            {
                if (Patcher.IsAlreadyPatched())
                {
                    labelStatus.Text = "Process is already patched!";
                    labelStatus.ForeColor = Color.Red;
                    buttonPatch.Enabled = false;
                }
                else if (Patcher.IsClientRunning())
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
                Patcher.Cleanup();
            }
        }

        private void buttonPatch_Click(object sender, EventArgs e)
        {
            buttonPatch.Enabled = false;

            try
            {
                Patcher.Patch();
                labelStatus.Text = "Patch successful!";
                labelStatus.ForeColor = Color.DarkGreen;
            }
            catch (Exception exception)
            {
                labelStatus.Text = exception.Message;
                labelStatus.ForeColor = Color.Red;
            }
            finally
            {
                Patcher.Cleanup();
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Patcher.Cleanup();
        }
    }
}
