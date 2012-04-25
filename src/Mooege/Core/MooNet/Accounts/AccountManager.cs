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
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using Mooege.Common.Logging;
using Mooege.Common.Storage;
using Mooege.Common.Storage.AccountDataBase.Entities;
using Mooege.Core.Cryptography;
using NHibernate.Linq;

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

        public static Account GetAccountByBattletag(string battletag)
        {
            foreach (var account in Accounts.Values)
            {
                if (account.AccountBattleTagField.Value == battletag)
                    return account;
            }
            return null;
        }

        public static Account CreateAccount(string email, string password, string battleTag, Account.UserLevels userLevel = Account.UserLevels.User)
        {
            if (password.Length > 16) password = password.Substring(0, 16); // make sure the password does not exceed 16 chars.
            var hashCode = AccountManager.GetRandomHashCodeForBattleTag(battleTag);
            var salt = SRP6a.GetRandomBytes(32);
            var passwordVerifier = SRP6a.CalculatePasswordVerifierForAccount(email, password, salt);


            var newDBAccount = new DBAccount
                                   {
                                       Email = email,
                                       Salt = salt,
                                       PasswordVerifier = passwordVerifier,
                                       BattleTagName = battleTag,
                                       UserLevel = (byte)userLevel,
                                       HashCode = hashCode
                                   };


            DBSessions.AccountSession.SaveOrUpdate(newDBAccount);
            DBSessions.AccountSession.Flush();

            var account = new Account(newDBAccount);
            Accounts.Add(email, account);
            return account;
        }

        public static void SaveToDB(Account account)
        {


            try
            {
                var dbAccount = DBSessions.AccountSession.Get<DBAccount>(account.PersistentID);
                dbAccount.Email = account.Email;
                dbAccount.Salt = account.Salt;
                dbAccount.PasswordVerifier = account.PasswordVerifier;
                dbAccount.BattleTagName = account.Name;
                dbAccount.HashCode = account.HashCode;
                dbAccount.UserLevel = (byte)account.UserLevel;
                dbAccount.LastOnline = account.LastOnlineField.Value;
                dbAccount.LastSelectedHeroId = account.LastSelectedHero.IdLow;
                DBSessions.AccountSession.SaveOrUpdate(dbAccount);
                DBSessions.AccountSession.Flush();


            }
            catch (Exception e)
            {
                Logger.ErrorException(e, "SaveToDB()");
            }
        }

        public static Account GetAccountByPersistentID(ulong persistentId)
        {
            return Accounts.Where(account => account.Value.PersistentID == persistentId).Select(account => account.Value).FirstOrDefault();
        }

        public static bool DeleteAccount(Account account)
        {
            if (account == null) return false;
            if (!Accounts.ContainsKey(account.Email)) return false;
            /*
            try
            {
                var query = string.Format("DELETE from accounts where id={0}", account.PersistentID);
                var cmd = new SQLiteCommand(query, DBManager.Connection);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Logger.ErrorException(e, "DeleteAccount()");
                return false;
            }
            */
            var dbAccount = DBSessions.AccountSession.Get<DBAccount>(account.PersistentID);
            DBSessions.AccountSession.Delete(dbAccount);
            DBSessions.AccountSession.Flush();
            Accounts.Remove(account.Email);
            // we should be also disconnecting the account if he's online. /raist.

            return true;
        }

        private static void LoadAccounts()
        {
            var allDbAccounts = DBSessions.AccountSession.Query<DBAccount>().ToList();
            foreach (var dbAccount in allDbAccounts)
            {
                var email = dbAccount.Email;
                var account = new Account(dbAccount);
                account.LastOnlineField.Value = dbAccount.LastOnline;
                Accounts.Add(email, account);
            }
        }

        public static int GetRandomHashCodeForBattleTag(string name)
        {
            var rnd = new Random();
            return rnd.Next(1, 1000);
        }

        public static ulong GetNextAvailablePersistentId()
        {
            return !DBSessions.AccountSession.Query<DBAccount>().Any() ? 1
                : DBSessions.AccountSession.Query<DBAccount>().OrderByDescending(dba => dba.Id).Select(dba => dba.Id).First() + 1;
        }


        public static void UpdatePassword(Account account, string newPassword)
        {
            account.PasswordVerifier = SRP6a.CalculatePasswordVerifierForAccount(account.Email, newPassword, account.Salt);
            try
            {
                /*
                var query = string.Format("UPDATE accounts SET passwordVerifier=@passwordVerifier WHERE id={0}", this.PersistentID);

                using (var cmd = new SQLiteCommand(query, DBManager.Connection))
                {
                    cmd.Parameters.Add("@passwordVerifier", System.Data.DbType.Binary, 128).Value = this.PasswordVerifier;
                    cmd.ExecuteNonQuery();
                }*/
                SaveToDB(account);
            }
            catch (Exception e)
            {
                Logger.ErrorException(e, "UpdatePassword()");
            }
        }

        public static void UpdateUserLevel(Account account, Account.UserLevels userLevel)
        {
            account.UserLevel = userLevel;
            try
            {/*
                var query = string.Format("UPDATE accounts SET userLevel={0} WHERE id={1}", (byte)userLevel, this.PersistentID);
                var cmd = new SQLiteCommand(query, DBManager.Connection);
                cmd.ExecuteNonQuery();*/
                SaveToDB(account);
            }
            catch (Exception e)
            {
                Logger.ErrorException(e, "UpdateUserLevel()");
            }
        }

    }
}
