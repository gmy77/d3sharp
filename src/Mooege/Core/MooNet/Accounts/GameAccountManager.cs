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
using Mooege.Common.Storage.AccountDataBase.Entities;
using Mooege.Core.MooNet.Helpers;
using Mooege.Common.Logging;
using Mooege.Common.Storage;
using Mooege.Core.MooNet.Toons;
using NHibernate.Linq;

namespace Mooege.Core.MooNet.Accounts
{
    class GameAccountManager
    {
        private static readonly Logger Logger = LogManager.CreateLogger();

        private static readonly Dictionary<ulong, GameAccount> GameAccounts = new Dictionary<ulong, GameAccount>();
        public static List<GameAccount> GameAccountsList { get { return GameAccounts.Values.ToList(); } }

        public static int TotalAccounts
        {
            get { return GameAccounts.Count; }
        }

        public static Dictionary<ulong, GameAccount> GetGameAccountsForAccount(Account account)
        {
            return GameAccounts.Where(pair => pair.Value.Owner != null).Where(pair => pair.Value.Owner.PersistentID == account.PersistentID).ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        public static Dictionary<ulong, GameAccount> GetGameAccountsForAccountProgram(Account account, FieldKeyHelper.Program program)
        {
            return GameAccounts.Where(pair => pair.Value.Owner != null).Where(pair => (pair.Value.Owner.PersistentID == account.PersistentID) && (pair.Value.Program == program)).ToDictionary(pair => pair.Key, pair => pair.Value);
        }
        
        public static GameAccount GetAccountByPersistentID(ulong persistentId)
        {
            return GameAccounts.Where(account => account.Value.PersistentID == persistentId).Select(account => account.Value).FirstOrDefault();
        }

        public static GameAccount GetAccountByDBGameAccount(DBGameAccount dbGameAccount)
        {
            return GetAccountByPersistentID(dbGameAccount.Id);
        }

        public static ulong GetNextAvailablePersistentId()
        {
            return !DBSessions.AccountSession.Query<DBGameAccount>().Any() ? 1
                : DBSessions.AccountSession.Query<DBGameAccount>().OrderByDescending(dba => dba.Id).Select(dba => dba.Id).First() + 1;
        }

        static GameAccountManager()
        {
            LoadGameAccounts();
        }

        private static void LoadGameAccounts()
        {
            var allDbGameAccounts = DBSessions.AccountSession.Query<DBGameAccount>().ToList();
            foreach (var dbGameAccount in allDbGameAccounts)
            {
                var gameAccountId = dbGameAccount.Id;
                var dbAcount = dbGameAccount.DBAccount;

                if (dbAcount == null)
                {
                    Logger.Error("Gameaccount without Account!");
                    continue;
                }
                var gameAccount = new GameAccount(dbGameAccount);



                #region Populate GameAccount Data

                //var banner = dbGameAccount.Banner;
                //gameAccount.BannerConfiguration = D3.Account.BannerConfiguration.ParseFrom(banner);
                //gameAccount.LastOnlineField.Value = dbGameAccount.LastOnline;
                GameAccounts.Add(gameAccountId, gameAccount);

                #endregion

            }


        }

        public static void SaveToDB(GameAccount gameAccount)
        {
            try
            {
                var dbGameAccount = DBSessions.AccountSession.Get<DBGameAccount>(gameAccount.PersistentID);
                dbGameAccount.DBAccount = DBSessions.AccountSession.Get<DBAccount>(gameAccount.Owner.PersistentID);
                dbGameAccount.LastOnline = gameAccount.LastOnlineField.Value;
                dbGameAccount.Banner = gameAccount.BannerConfiguration.ToByteArray();
                DBSessions.AccountSession.SaveOrUpdate(dbGameAccount);
                DBSessions.AccountSession.Flush();
            }
            catch (Exception e)
            {
                Logger.ErrorException(e, "GameAccount.SaveToDB()");
            }
        }

        public static GameAccount CreateGameAccount(Account account)
        {
            var newDBGameAccount = new DBGameAccount
                                       {
                                           DBAccount = DBSessions.AccountSession.Get<DBAccount>(account.PersistentID)
                                       };

            DBSessions.AccountSession.SaveOrUpdate(newDBGameAccount);
            DBSessions.AccountSession.Flush();
            var gameAccount = new GameAccount(newDBGameAccount);
            GameAccounts.Add(gameAccount.PersistentID, gameAccount);
            return gameAccount;
        }


        public static void DeleteGameAccount(GameAccount GameAccount)
        {
            if (!GameAccounts.ContainsKey(GameAccount.PersistentID))
            {
                Logger.Error("Attempting to delete game account that does not exist: {0}", GameAccount.PersistentID);
                return;
            }

            //Delete all toons for game account
            foreach (var toon in ToonManager.GetToonsForGameAccount(GameAccount).Values)
            {
                ToonManager.DeleteToon(toon);
            }


            var dbGameAccount = DBSessions.AccountSession.Get<DBGameAccount>(GameAccount.PersistentID);
            DBSessions.AccountSession.Delete(dbGameAccount);
            DBSessions.AccountSession.Flush();

            
            GameAccounts.Remove(GameAccount.PersistentID);
        }

    }
}
