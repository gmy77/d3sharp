using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using PonyPatcher.Utilities;

namespace patcher
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
    }
}
