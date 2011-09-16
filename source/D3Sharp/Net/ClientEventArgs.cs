using System;

namespace D3Sharp.Net
{    
    public class ClientEventArgs : EventArgs
    {
        public IClient Client { get; private set; }

        public ClientEventArgs(IClient connection)
        {
            if (connection == null)
                throw new ArgumentNullException("connection");
            this.Client = connection;
        }

        public override string ToString()
        {
            return Client.RemoteEndPoint != null
                ? Client.RemoteEndPoint.ToString()
                : "Not Connected";
        }
    }
}

