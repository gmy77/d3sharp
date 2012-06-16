/*
 * Copyright (C) 2011 - 2012 mooege project - http://www.mooege.org
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */

using Mooege.Common.Logging;
using Mooege.Core.MooNet.Online;

namespace Mooege.Net.MooNet
{
    public sealed class MooNetServer : Server
    {
        private new static readonly Logger Logger = LogManager.CreateLogger(); // hide the Server.Logger so that tiny-logger can show the actual server as log source.

        public MooNetServer()
        {
            this.OnConnect += MooNetServer_OnConnect;
            this.OnDisconnect += MooNetServer_OnDisconnect;
            this.DataReceived += (sender, e) => MooNetRouter.Route(e);
            this.DataSent += (sender, e) => { };
        }

        private void MooNetServer_OnConnect(object sender, ConnectionEventArgs e)
        {
            Logger.Trace("MooNet-Client connected: {0}", e.Connection.ToString());
            e.Connection.Client = new MooNetClient(e.Connection);
        }

        private void MooNetServer_OnDisconnect(object sender, ConnectionEventArgs e)
        {
            var client = ((MooNetClient)e.Connection.Client);

            Logger.Trace("Client disconnected: {0}", e.Connection.ToString());
            if (client.Account != null && client.Account.CurrentGameAccount != null) client.Account.CurrentGameAccount.LoggedInClient = null;
            PlayerManager.PlayerDisconnected((MooNetClient)e.Connection.Client);
        }

        public override void Run()
        {
            // we can't listen for port 1119 because D3 and the launcher (agent) communicates on that port through loopback.
            // so we change our default port and start D3 with a shortcut like so:
            //   "F:\Diablo III Beta\Diablo III.exe" -launch -auroraaddress 127.0.0.1:1345

            var bindIP = NetworkingConfig.Instance.EnableIPv6 ? Config.Instance.BindIPv6 : Config.Instance.BindIP;

            if (!this.Listen(bindIP, Config.Instance.Port)) return;
            Logger.Info("MooNet-Server is listening on {0}:{1}...", bindIP, Config.Instance.Port);
        }
    }
}
