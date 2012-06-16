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

using System.ServiceModel;
using Mooege.Core.MooNet.Accounts;
using Mooege.Net.WebServices;

namespace Mooege.Core.MooNet.Web
{
    [ServiceContract(Name = "Accounts")]
    public class AccountWebService : IWebService
    {
        [OperationContract]
        public bool CreateAccount(string email, string password, string battleTag)
        {
            if (string.IsNullOrEmpty(email))
                throw new FaultException(new FaultReason("Email parameter can not be null or empty."));

            if (string.IsNullOrEmpty(password))
                throw new FaultException(new FaultReason("Password parameter can not be null or empty."));

            if (string.IsNullOrEmpty(battleTag))
                throw new FaultException(new FaultReason("BattleTag parameter can not be null or empty."));

            if (password.Length < 8 || password.Length > 16)
                throw new FaultException(new FaultReason("Password should be a minimum of 8 and a maximum of 16 characters."));

            if (AccountManager.GetAccountByEmail(email.ToLower()) != null)
                throw new FaultException(new FaultReason(string.Format("An account already exists for email address {0}.", email)));


            var account = AccountManager.CreateAccount(email, password, battleTag);
            var gameAccount = GameAccountManager.CreateGameAccount(account);
            account.DBAccount.DBGameAccounts.Add(gameAccount.DBGameAccount);
            account.SaveToDB();

            return true;
        }

        [OperationContract]
        public bool AccountExists(string email)
        {
            if (string.IsNullOrEmpty(email))
                throw new FaultException(new FaultReason("Email parameter can not be null or empty."));

            return AccountManager.GetAccountByEmail(email.ToLower()) != null;
        }

        [OperationContract]
        public bool VerifyPassword(string email, string password)
        {
            if (string.IsNullOrEmpty(email))
                throw new FaultException(new FaultReason("Email parameter can not be null or empty."));

            if (string.IsNullOrEmpty(password))
                throw new FaultException(new FaultReason("Password parameter can not be null or empty."));

            if (password.Length < 8 || password.Length > 16)
                throw new FaultException(new FaultReason("Password should be a minimum of 8 and a maximum of 16 characters."));

            var account = AccountManager.GetAccountByEmail(email.ToLower());

            if (account == null)
                throw new FaultException(new FaultReason(string.Format("Account does not exist for email address {0}.", email)));

            return account.VerifyPassword(password);
        }

        [OperationContract]
        public int TotalAccounts()
        {
            return AccountManager.TotalAccounts;
        }

        [OperationContract]
        public bool ChangePassword(string email, string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new FaultException(new FaultReason("Password parameter can not be null or empty."));

            if (password.Length < 8 || password.Length > 16)
                throw new FaultException(new FaultReason("Password should be a minimum of 8 and a maximum of 16 characters."));

            var account = AccountManager.GetAccountByEmail(email.ToLower());
            if (account == null)
                throw new FaultException(new FaultReason(string.Format("Account does not exist for email address {0}.", email)));

            return account.UpdatePassword(password);

        }

        //[OperationContract]
        //public int TotalToons()
        //{
        //    return AccountManager.AccountsList.Sum(account => account.Toons.Count);
        //}
    }
}
