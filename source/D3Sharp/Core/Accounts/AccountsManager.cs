using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using D3Sharp.Core.Storage;

namespace D3Sharp.Core.Accounts
{
    public static class AccountsManager
    {
        private static readonly Dictionary<string, Account> Accounts = new Dictionary<string, Account>();

        static AccountsManager()
        {
            LoadAccounts();
        }

        public static Account GetAccount(string email)
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

        private static void LoadAccounts()
        {
            var query = "SELECT * from accounts";
            var cmd = new SQLiteCommand(query, DBManager.Connection);
            var reader = cmd.ExecuteReader();

            if (!reader.HasRows) return;

            while (reader.Read())
            {
                var id = (ulong)reader.GetInt64(0);
                var email = reader.GetString(1);
                var account = new Account(id, email);
                Accounts.Add(email, account);
            }
        }
    }
}
