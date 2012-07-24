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

using System;
using System.Linq;
using Mooege.Core.MooNet.Accounts;
using Mooege.Core.MooNet.Commands;
using Mooege.Net.MooNet;

namespace Mooege.Core.MooNet.Channels
{
    [CommandGroup("channels", "Lists active channels.\nUsage: channels [client]", Account.UserLevels.Owner)]
    public class ChannelCommands : CommandGroup
    {
        [DefaultCommand]
        public string Channels(string[] @params, MooNetClient invokerClient)
        {
            //return Service.ProvidedServices.Aggregate("Provided services by server:\n",
            //    (current, pair) => current + string.Format("Id: 0x{0} Hash: 0x{1} [{2}]\n", pair.Value.ServiceID.ToString("X2"), pair.Value.Hash.ToString("X8"), pair.Key.Name));

            var output = "Active Channels:\n";
            foreach (var channel in ChannelManager.Channels.Values)
            {
                output += string.Format("Id: {0}\t  Owner: {1}\n", channel.DynamicId, channel.Owner);
                foreach (var member in channel.Members.Keys)
                {
                    output += string.Format("\tMembers: {0}\n", member);
                }
            }
            return output;
            //return ChannelManager.Channels.Aggregate("Active Channels:\n",
            //    (current, pair) => current + string.Format("Id: {0} \tOwner: {1}\n", pair.Value.DynamicId, pair.Value.Owner));
        }

        [Command("state", "Show state of channel.\nUsage: channels state <channelId>", Account.UserLevels.Owner)]
        public string ChannelState(string[] @params, MooNetClient invokerClient)
        {
            if (@params.Count() < 1)
                return "Invalid arguments. Type 'help channels state' to get help.";

            UInt64 dynamicId;

            if (!UInt64.TryParse(@params[0], out dynamicId))
                return "Invalid arguments. Type 'help channels state' to get help.";

            var channel = ChannelManager.GetChannelByDynamicId(dynamicId);

            if (channel == null)
                return "Invalid Channel Id.\nType 'channels list' to get a list of active channels.";

            return channel.State.ToString();
        }

        [Command("list", "Shows list of active channels.\nUsage: channels list", Account.UserLevels.Owner)]
        public string ChannelList(string[] @params, MooNetClient invokerClient)
        {
            return ChannelManager.Channels.Keys.ToString();
        }


        [Command("client", "Shows active channels for client.\nUsage: channels client [email]")]
        public string ClientChannels(string[] @params, MooNetClient invokerClient)
        {
            var client = invokerClient;

            if (client == null && @params.Count() < 1)
                return "Invalid arguments. Type 'help channels client' to get help.";
            var output = "";


            if (client == null)
            {
                var email = @params[0];
                var account = AccountManager.GetAccountByEmail(email);

                if (account == null)
                    return string.Format("No account with email '{0}' exists.", email);

                if (!account.IsOnline)
                    return string.Format("Account '{0}' is not logged in.", email);

                var gameAccounts = GameAccountManager.GetGameAccountsForAccount(account);
                foreach (var gameAccount in gameAccounts)
                {
                    output += this.ClientChannels(null, gameAccount.LoggedInClient);
                }
            }
            else
            {
                output = string.Format("Active channels for account: {0}\n", client.Account.Email);
                output = ChannelManager.Channels.Aggregate(output, (current, pair) =>
                    current + string.Format("Id: {0} \tOwner: {1}\n", pair.Value.DynamicId, pair.Value.Owner));
            }
            return output;
        }
    }
}
