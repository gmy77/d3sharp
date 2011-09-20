using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace D3Sharp.Net
{
    public interface IClient
    {
        IConnection Connection { get; set; }
    }
}
