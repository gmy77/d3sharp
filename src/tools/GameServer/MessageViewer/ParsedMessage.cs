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
    public class ParsedMessage : GameMessage
    {
        string text;

        public ParsedMessage(string text)
        {
            this.text = text;
        }


        public override void Parse(GameBitBuffer buffer)
        {
            throw new NotImplementedException();
        }

        public override void Encode(GameBitBuffer buffer)
        {
            throw new NotImplementedException();
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(text);
        }

        public override string ToString()
        {
            return text;
        }
    }
}
