using System.Collections.Generic;
using System.Linq;

namespace D3Sharp.Net
{
    public sealed class ClientDataEventArgs : ClientEventArgs
    {
        public IEnumerable<byte> Data { get; private set; }

        public ClientDataEventArgs(IClient client, IEnumerable<byte> data)
            : base(client)
        {
            this.Data = data ?? new byte[0];
        }

        public override string ToString()
        {
            return Client.RemoteEndPoint != null
                ? string.Format("{0}: {1} bytes", Client.RemoteEndPoint, Data.Count())
                : string.Format("Not Connected: {0} bytes", Data.Count());
        }
    }
}

