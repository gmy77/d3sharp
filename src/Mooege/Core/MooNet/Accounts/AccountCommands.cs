/*
 * Copyright (C) 2011 mooege project
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
using Mooege.Core.MooNet.Commands;
using Mooege.Net.MooNet;

namespace Mooege.Core.MooNet.Accounts
{
    [Command("AddUser")]
    public class AddUserCommand : Command
    {
        public override string Help()
        {
            return "usage: adduser email password";
        }

        public override string Invoke(string parameters, MooNetClient invokerClient = null)
        {
            var @params = parameters.Split(' ');
            if (@params.Count() < 2)
                return "Invalid arguments. Type 'help adduser' to get help.";

            var email = @params[0];
            var password = @params[1];

            if (!email.Contains('@'))
                return string.Format("'{0}' is not a valid email address.", email);

            if (password.Length < 8 || password.Length > 16)
                return "Password should be a minimum of 8 and a maximum of 16 characters.";

            if (AccountManager.GetAccountByEmail(email) != null)
                return string.Format("An account already exists for email address {0}.", email);

            var account = AccountManager.CreateAccount(email, password);
            return string.Format("Created account {0}.", email);
        }
    }

    [Command("DelUser")]
    public class DelUserCommand : Command
    {
        public override string Help()
        {
            return "usage: deluser email";
        }

        public override string Invoke(string parameters, MooNetClient invokerClient = null)
        {
            // TODO: we should be also deleting account's toons. /raist.

            parameters = parameters.Trim();

            if (parameters == string.Empty)
                return "Invalid arguments. Type 'help deluser' to get help.";

            var account = AccountManager.GetAccountByEmail(parameters);

            if (account == null)
                return string.Format("No account with email '{0}' exists.", parameters);

            AccountManager.DeleteAccount(account);
            return string.Format("Deleted account {0}.", parameters);
        }
    }

    [Command("SetPassword")]
    public class SetPasswordCommand : Command
    {
        public override string Help()
        {
            return "usage: setpassword email password";
        }

        public override string Invoke(string parameters, MooNetClient invokerClient = null)
        {
            var @params = parameters.Split(' ');

            if (@params.Count() < 2)
                return "Invalid arguments. Type 'help setpassword' to get help.";

            var email = @params[0];
            var password = @params[1];

            var account = AccountManager.GetAccountByEmail(email);

            if (account == null)
                return string.Format("No account with email '{0}' exists.", email);

            if (password.Length < 8 || password.Length > 16)
                return "Password should be a minimum of 8 and a maximum of 16 characters.";

            account.UpdatePassword(password);
            return string.Format("Updated password for user {0}.", email);
        }
    }
}
