using System;
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
        }

        private void buttonPatch_Click(object sender, EventArgs e)
        {
            Patcher.Patch();
        }
    }
}
