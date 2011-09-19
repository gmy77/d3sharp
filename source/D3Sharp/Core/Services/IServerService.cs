using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using D3Sharp.Net;

namespace D3Sharp.Core.Services
{
    public interface IServerService
    {
        IClient Client { get; set; }
    }
}
