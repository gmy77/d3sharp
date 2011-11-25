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

using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using Mooege.Common.Logging;
using Mooege.Common.Storage;

namespace Mooege.Core.MooNet.Accounts
{
    public static class AccountManager
    {
        private static readonly Logger Logger = LogManager.CreateLogger();

        private static readonly Dictionary<string, Account> Accounts = new Dictionary<string, Account>();
        public static List<Account> AccountsList { get { return Accounts.Values.ToList(); } }

        public static int TotalAccounts
        {
            get { return Accounts.Count; }
        }

        static AccountManager()
        {
            LoadAccounts();
        }

        public static Account GetAccountByEmail(string email)
        {
            return Accounts.ContainsKey(email) ? Accounts[email] : null;
        }

        public static Account CreateAccount(string email, string password, Account.UserLevels userLevel = Account.UserLevels.User)
        {
            var account = new Account(email, password, userLevel);
            Accounts.Add(email, account);
            account.SaveToDB();

            return account;
        }

        public static Account GetAccountByPersistentID(ulong persistentId)
        {
            return Accounts.Where(account => account.Value.PersistentID == persistentId).Select(account => account.Value).FirstOrDefault();
        }

        public static bool DeleteAccount(Account account)
        {
            if (account == null) return false;
            if (!Accounts.ContainsKey(account.Email)) return false;

            try
            {
                var query = string.Format("DELETE from accounts where id={0}", account.PersistentID);
                var cmd = new SQLiteCommand(query, DBManager.Connection);
                cmd.ExecuteNonQuery();
            }
            catch(Exception e)
            {
                Logger.ErrorException(e, "DeleteAccount()");
                return false;
            }

            Accounts.Remove(account.Email);
            // we should be also disconnecting the account if he's online. /raist.

            return true;
        }

        private static void LoadAccounts()
        {
            var query = "SELECT * from accounts";
            var cmd = new SQLiteCommand(query, DBManager.Connection);
            var reader = cmd.ExecuteReader();

            if (!reader.HasRows) return;

            while (reader.Read())
            {
                var accountId = (ulong)reader.GetInt64(0);
                var email = reader.GetString(1);

                var salt = new byte[32];
                var readBytes = reader.GetBytes(2, 0, salt, 0, 32);

                var passwordVerifier = new byte[128];
                readBytes = reader.GetBytes(3, 0, passwordVerifier, 0, 128);

                var userLevel = (byte) reader.GetByte(4);

                var account = new Account(accountId, email, salt, passwordVerifier, (Account.UserLevels)userLevel);
                Accounts.Add(email, account);
            }
        }

        public static ulong GetNextAvailablePersistentId()
        {
            var cmd = new SQLiteCommand("SELECT max(id) from accounts", DBManager.Connection);
            try
            {
                return Convert.ToUInt64(cmd.ExecuteScalar());
            }
            catch (InvalidCastException)
            {
                return 0;
            }
        }       
    }
}
