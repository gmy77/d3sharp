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
using Mooege.Core.Common.Storage;

namespace Mooege.Core.MooNet.Accounts
{
    public static class AccountManager
    {
        public static readonly Dictionary<string, Account> Accounts = new Dictionary<string, Account>();

        static AccountManager()
        {
            LoadAccounts();
        }

        public static Account GetAccountByEmail(string email)
        {
            Account account;

            if (Accounts.ContainsKey(email))
                account = Accounts[email];
            else
            {
                account = new Account(email);
                Accounts.Add(email, account);
                account.SaveToDB();
            }

            return account;
        }

        public static Account GetAccountByEntityID(bnet.protocol.EntityId entityId)
        {
            return Accounts.Where(account => account.Value.BnetAccountID == entityId).Select(account => account.Value).FirstOrDefault();
        }

        public static Account GetAccountByPersistantID(ulong persistantId)
        {
            return Accounts.Where(account => account.Value.PersistentID == persistantId).Select(account => account.Value).FirstOrDefault();
        }

        private static void LoadAccounts()
        {
            var query = "SELECT * from accounts";
            var cmd = new SQLiteCommand(query, DBManager.Connection);
            var reader = cmd.ExecuteReader();

            if (!reader.HasRows) return;

            while (reader.Read())
            {
                var databaseId = (ulong)reader.GetInt64(0);
                var email = reader.GetString(1);
                var account = new Account(databaseId, email);
                Accounts.Add(email, account);
            }
        }

        public static ulong GetNextAvailablePersistantId()
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
