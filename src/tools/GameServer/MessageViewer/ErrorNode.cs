using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Mooege.Net.GS.Message;
using System.Drawing;
using Mooege.Net.GS.Message.Definitions.Misc;

namespace GameMessageViewer
{
    public class ErrorNode : TreeNode, ITextNode
    {
        string description;

        public ErrorNode(string name, string description)
        {
            Text = name;
            this.description = description;
        }

        public string AsText()
        {
            return description;
        }
    }
}
