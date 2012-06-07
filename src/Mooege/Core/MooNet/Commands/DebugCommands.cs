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

using System.Linq;
using Mooege.Common.Helpers.Hash;
using Mooege.Core.MooNet.Accounts;
using Mooege.Core.MooNet.Objects;
using Mooege.Core.MooNet.Services;
using Mooege.Net.MooNet;

namespace Mooege.Core.MooNet.Commands
{
    [CommandGroup("services", "Lists moonet services provided by the server.\nUsage: services [client]", Account.UserLevels.Owner)]
    public class ServiceDebugCommands : CommandGroup
    {
        [DefaultCommand]
        public string Services(string[] @params, MooNetClient invokerClient)
        {
            return Service.ProvidedServices.Aggregate("Provided services by server:\n",
                (current, pair) => current + string.Format("Id: 0x{0} Hash: 0x{1} [{2}]\n", pair.Value.ServiceID.ToString("X2"), pair.Value.Hash.ToString("X8"), pair.Key.Name));
        }

        [Command("client", "Shows imported service list for client.\nUsage: services client [email]")]
        public string ClientServices(string[] @params, MooNetClient invokerClient)
        {
            var client = invokerClient;

            if (client == null && @params.Count() < 1)
                return "Invalid arguments. Type 'help services client' to get help.";
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
                    output += this.ClientServices(null, gameAccount.LoggedInClient);
                }
            }
            else
            {
                output = string.Format("Imported service list for account: {0}\n", client.Account.Email);
                output = client.Services.Aggregate(output, (current, pair) =>
                    current + string.Format("Id: 0x{0} Hash: 0x{1}\n", pair.Value.ToString("X2"), pair.Key.ToString("X8")));
            }
            return output;
        }
    }

    [CommandGroup("hash", "Create hashes.", Account.UserLevels.Owner)]
    public class HashDebugCommands : CommandGroup
    {
        [Command("hashitem", "Hash case insensitive (item names)")]
        public string HashItem(string[] @params, MooNetClient invokerClient)
        {
            if (@params.Count() < 1)
                return "Invalid arguments. Type 'help hash show' to get help.";

            return StringHashHelper.HashItemName(@params[0]).ToString();
        }

        [Command("hash", "Hash case Sensitive")]
        public string HashNormal(string[] @params, MooNetClient invokerClient)
        {
            if (@params.Count() < 1)
                return "Invalid arguments. Type 'help hash show' to get help.";

            return StringHashHelper.HashNormal(@params[0]).ToString();
        }
    }

    [CommandGroup("rpcobject", "Lists rpc-objects.", Account.UserLevels.Admin)]
    public class RPCObjectDebugCommands : CommandGroup
    {
        [Command("list", "Shows lists of RPCObjects")]
        public string List(string[] @params, MooNetClient invokerClient)
        {
            return RPCObjectManager.Objects.Aggregate("RPCObjects:\n",
                (current, pair) => current + string.Format("Id: 0x{0} - {1} [{2}]\n", pair.Key.ToString("X8"), pair.Value.GetType().Name, pair.Value.ToString()));
        }

        [Command("show", "Prints detailed debug information for RPCObject.\nUsage: rpcobject show <localId>")]
        public string Show(string[] @params, MooNetClient invokerClient)
        {
            if (@params.Count() < 1)
                return "Invalid arguments. Type 'help rpcobject show' to get help.";

            ulong localId;
            var id = @params[0];
            if (!ulong.TryParse(id, out localId))
                return string.Format("Can not parse '{0}' as valid id.", id);

            if (!RPCObjectManager.Objects.ContainsKey(localId))
                return string.Format("There exists no RPCObject with dynamidId: {0}", localId);

            var rpcObject = RPCObjectManager.Objects[localId];
            var output = string.Format("[RPCObject]\nDynamicId: 0x{0}\nType: {1}\nObject: {2}\n", rpcObject.DynamicId,
                                       rpcObject.GetType().Name, rpcObject);

            output += "[Subscribers]\n";
            foreach (var client in rpcObject.Subscribers)
            {
                var remoteId = client.GetRemoteObjectId(rpcObject.DynamicId);
                output += string.Format("RemoteId: 0x{0} - {1}\n", remoteId.ToString("X8"), client.Account.Email);
            }

            return output;
        }
    }
}
