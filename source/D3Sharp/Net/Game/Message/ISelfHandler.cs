using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace D3Sharp.Net.Game.Message
{
    public interface ISelfHandler
    {
        void Handle(GameClient client);
    }
}
