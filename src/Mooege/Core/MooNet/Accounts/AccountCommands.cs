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
using Mooege.Core.MooNet.Commands;
using Mooege.Net.MooNet;

namespace Mooege.Core.MooNet.Accounts
{
    [CommandGroup("account", "Provides account managment commands.")]
    public class AccountCommands : CommandGroup
    {
        [Command("show", "Shows information about given account\nUsage: account show <email>", Account.UserLevels.GM)]
        public string Show(string[] @params, MooNetClient invokerClient)
        {
            if (@params.Count() < 1)
                return "Invalid arguments. Type 'help account show' to get help.";

            var email = @params[0];
            var account = AccountManager.GetAccountByEmail(email);

            if (account == null)
                return string.Format("No account with email '{0}' exists.", email);

            return string.Format("Email: {0} User Level: {1}", account.Email, account.UserLevel);
        }

        [Command("add", "Allows you to add a new user account.\nUsage: account add <email> <password> <battletag> [userlevel]", Account.UserLevels.GM)]
        public string Add(string[] @params, MooNetClient invokerClient)
        {
            if (@params.Count() < 3)
                return "Invalid arguments. Type 'help account add' to get help.";

            var email = @params[0];
            var password = @params[1];
            var battleTagName = @params[2];
            var userLevel = Account.UserLevels.User;

            if (@params.Count() == 4)
            {
                var level = @params[3].ToLower();
                switch (level)
                {
                    case "owner":
                        userLevel = Account.UserLevels.Owner;
                        break;
                    case "admin":
                        userLevel = Account.UserLevels.Admin;
                        break;
                    case "gm":
                        userLevel = Account.UserLevels.GM;
                        break;
                    case "user":
                        userLevel = Account.UserLevels.User;
                        break;
                    default:
                        return level + " is not a valid user level.";
                }
            }

            if (!email.Contains('@'))
                return string.Format("'{0}' is not a valid email address.", email);

            if (battleTagName.Contains('#'))
                return "BattleTag must not contain '#' or HashCode.";

            if (password.Length < 8 || password.Length > 16)
                return "Password should be a minimum of 8 and a maximum of 16 characters.";

            if (AccountManager.GetAccountByEmail(email) != null)
                return string.Format("An account already exists for email address {0}.", email);

            var account = AccountManager.CreateAccount(email, password, battleTagName, userLevel);
            var gameAccount = GameAccountManager.CreateGameAccount(account);
            account.DBAccount.DBGameAccounts.Add(gameAccount.DBGameAccount);
            return string.Format("Created new account {0} [user-level: {1}] Full BattleTag: {2}.", account.Email, account.UserLevel, account.BattleTag);
        }

        [Command("delete", "Allows you to delete an existing account.\nUsage: account delete <email>", Account.UserLevels.GM)]
        public string Delete(string[] @params, MooNetClient invokerClient)
        {
            if (@params.Count() == 0)
                return "Invalid arguments. Type 'help account delete' to get help.";

            var account = AccountManager.GetAccountByEmail(@params[0]);

            if (account == null)
                return string.Format("No account with email '{0}' exists.", @params);

            //Delete game accounts for account
            //which in turn will delete toons for each game account
            foreach (var gameAccount in GameAccountManager.GetGameAccountsForAccount(account))
            {
                GameAccountManager.DeleteGameAccount(gameAccount);
            }

            AccountManager.DeleteAccount(account);

            return string.Format("Deleted account {0}.", @params);
        }

        [Command("setpassword", "Allows you to set a new password for account\nUsage: account setpassword <email> <password>", Account.UserLevels.GM)]
        public string SetPassword(string[] @params, MooNetClient invokerClient)
        {
            if (@params.Count() < 2)
                return "Invalid arguments. Type 'help account setpassword' to get help.";

            var email = @params[0];
            var password = @params[1];

            var account = AccountManager.GetAccountByEmail(email);

            if (account == null)
                return string.Format("No account with email '{0}' exists.", email);

            if (password.Length < 8 || password.Length > 16)
                return "Password should be a minimum of 8 and a maximum of 16 characters.";

            AccountManager.UpdatePassword(account, password);
            return string.Format("Updated password for account {0}.", email);
        }

        [Command("setuserlevel", "Allows you to set a new user level for account\nUsage: account setuserlevel <email> <user level>.\nAvailable user levels: owner, admin, gm, user.", Account.UserLevels.GM)]
        public string SetLevel(string[] @params, MooNetClient invokerClient)
        {
            if (@params.Count() < 2)
                return "Invalid arguments. Type 'help account setuserlevel' to get help.";

            var email = @params[0];
            var level = @params[1].ToLower();
            Account.UserLevels userLevel;

            var account = AccountManager.GetAccountByEmail(email);

            if (account == null)
                return string.Format("No account with email '{0}' exists.", email);

            switch (level)
            {
                case "owner":
                    userLevel = Account.UserLevels.Owner;
                    break;
                case "admin":
                    userLevel = Account.UserLevels.Admin;
                    break;
                case "gm":
                    userLevel = Account.UserLevels.GM;
                    break;
                case "user":
                    userLevel = Account.UserLevels.User;
                    break;
                default:
                    return level + " is not a valid user level.";
            }
            AccountManager.UpdateUserLevel(account, userLevel);
            return string.Format("Updated user level for account {0} [user-level: {1}].", email, userLevel);
        }
    }

    [CommandGroup("whoami", "Returns information about current logged in account.")]
    class WhoAmICommand : CommandGroup
    {
        [DefaultCommand]
        public string WhoAmI(string[] @params, MooNetClient invokerClient)
        {
            if (invokerClient == null)
                return "You can not invoke this command from console.";

            return string.Format("Email: {0} User Level: {1}", invokerClient.Account.Email, invokerClient.Account.UserLevel);
        }
    }
}
