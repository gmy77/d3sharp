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
        private static readonly HashSet<Account> LoadedAccounts = new HashSet<Account>();
        private static readonly Logger Logger = LogManager.CreateLogger();

        public static int TotalAccounts
        {
            get { return DBSessions.AccountSession.Query<DBAccount>().Count(); }
        }


        #region AccountGetter
        public static Account CreateAccount(string email, string password, string battleTag, Account.UserLevels userLevel = Account.UserLevels.User)
        {
            if (password.Length > 16) password = password.Substring(0, 16); // make sure the password does not exceed 16 chars.
            var hashCode = GetRandomHashCodeForBattleTag();
            var salt = SRP6a.GetRandomBytes(32);
            var passwordVerifier = SRP6a.CalculatePasswordVerifierForAccount(email, password, salt);


            var newDBAccount = new DBAccount
                                   {
                                       Email = email,
                                       Salt = salt,
                                       PasswordVerifier = passwordVerifier,
                                       BattleTagName = battleTag,
                                       UserLevel = userLevel,
                                       HashCode = hashCode
                                   };


            DBSessions.AccountSession.SaveOrUpdate(newDBAccount);
            DBSessions.AccountSession.Flush();

            return GetAccountByDBAccount(newDBAccount);
        }

        public static Account GetAccountByEmail(string email)
        {
            if (DBSessions.AccountSession.Query<DBAccount>().Any(dba => dba.Email.ToLower() == email.ToLower()))
                return
                    GetAccountByDBAccount(
                        DBSessions.AccountSession.Query<DBAccount>().Single(
                            dba => dba.Email.ToLower() == email.ToLower()));
            return null;
        }

        public static Account GetAccountByBattletag(string battletag)
        {
            if (DBSessions.AccountSession.Query<DBAccount>().Any(dba => dba.BattleTagName.ToLower() == battletag.ToLower()))
                return
                    GetAccountByDBAccount(
                        DBSessions.AccountSession.Query<DBAccount>().Single(
                            dba => dba.BattleTagName.ToLower() == battletag.ToLower()));
            return null;
        }

        public static Account GetAccountByPersistentID(ulong persistentId)
        {
            var dbAccount = DBSessions.AccountSession.Get<DBAccount>(persistentId);
            return GetAccountByDBAccount(dbAccount);
        }

        public static Account GetAccountByDBAccount(DBAccount dbAccount)
        {
            if (!LoadedAccounts.Any(acc => acc.DBAccount.Id == dbAccount.Id))
                LoadedAccounts.Add(new Account(dbAccount));
            return LoadedAccounts.Single(acc => acc.DBAccount.Id == dbAccount.Id);
        }
        #endregion

        #region Managing Functions, also extending Account
        public static void SaveToDB(this Account account)
        {

            Logger.Debug("Saving account \"{0}\"", account.Email);
            try
            {
                DBSessions.AccountSession.SaveOrUpdate(account.DBAccount);
                DBSessions.AccountSession.Flush();
            }
            catch (Exception e)
            {
                Logger.ErrorException(e, "SaveToDB()");
            }
        }

        public static bool DeleteAccount(this Account account)
        {
            if (account == null)
                return false;

            if (LoadedAccounts.Contains(account))
                LoadedAccounts.Remove(account);

            DBSessions.AccountSession.Delete(account.DBAccount);
            DBSessions.AccountSession.Flush();
            // we should be also disconnecting the account if he's online. /raist.

            return true;
        }

        public static bool UpdatePassword(this Account account, string newPassword)
        {
            account.PasswordVerifier = SRP6a.CalculatePasswordVerifierForAccount(account.Email, newPassword, account.Salt);
            try
            {

                SaveToDB(account);
                return true;
            }
            catch (Exception e)
            {
                Logger.ErrorException(e, "UpdatePassword()");
                return false;
            }
        }

        public static void UpdateUserLevel(this Account account, Account.UserLevels userLevel)
        {
            account.UserLevel = userLevel;
            try
            {
                SaveToDB(account);
            }
            catch (Exception e)
            {
                Logger.ErrorException(e, "UpdateUserLevel()");
            }
        }
        #endregion



        private static int GetRandomHashCodeForBattleTag()
        {
            var rnd = new Random();
            return rnd.Next(1, 1000);
        }
    }
}
