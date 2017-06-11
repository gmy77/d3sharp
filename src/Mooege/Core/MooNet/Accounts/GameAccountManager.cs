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
        private static readonly HashSet<GameAccount> LoadedGameAccounts = new HashSet<GameAccount>();
        private static readonly Logger Logger = LogManager.CreateLogger();

        public static int TotalAccounts
        {
            get { return DBSessions.AccountSession.Query<DBGameAccount>().Count(); }
        }

        public static GameAccount GetGameAccountByDBGameAccount(DBGameAccount dbGameAccount)
        {
            if (!LoadedGameAccounts.Any(acc => acc.DBGameAccount.Id == dbGameAccount.Id))
                LoadedGameAccounts.Add(new GameAccount(dbGameAccount));
            return LoadedGameAccounts.Single(acc => acc.DBGameAccount.Id == dbGameAccount.Id);
        }

        public static GameAccount FindLoadedGameAccountByBnetId(ulong id)
        {
            if (LoadedGameAccounts.Any(ga => ga.BnetEntityId.Low == id))
                return LoadedGameAccounts.Single(ga => ga.BnetEntityId.Low == id);
            return null;
        }

        public static List<GameAccount> GetGameAccountsForAccount(Account account)
        {
            return account.DBAccount.DBGameAccounts.Select(GetGameAccountByDBGameAccount).ToList();
        }

        //Not needed... we emulate only D3, or not?
        /*
        public static Dictionary<ulong, GameAccount> GetGameAccountsForAccountProgram(Account account, FieldKeyHelper.Program program)
        {
            
            return GameAccounts.Where(pair => pair.Value.Owner != null).Where(pair => (pair.Value.Owner.PersistentID == account.PersistentID) && (pair.Value.Program == program)).ToDictionary(pair => pair.Key, pair => pair.Value);
        }
        */
        public static GameAccount GetAccountByPersistentID(ulong persistentId)
        {
            var dbGameAccount = DBSessions.AccountSession.Get<DBGameAccount>(persistentId);
            return GetGameAccountByDBGameAccount(dbGameAccount);
        }

        public static void SaveToDB(GameAccount gameAccount)
        {
            try
            {
                DBSessions.AccountSession.SaveOrUpdate(gameAccount.DBGameAccount);
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
            return GetGameAccountByDBGameAccount(newDBGameAccount);
        }

        public static void DeleteGameAccount(GameAccount gameAccount)
        {
            if (gameAccount == null)
                return;
            if (LoadedGameAccounts.Contains(gameAccount))
                LoadedGameAccounts.Remove(gameAccount);

            //Delete all toons for game account
            foreach (var toon in ToonManager.GetToonsForGameAccount(gameAccount))

                ToonManager.DeleteToon(toon);


            var inventoryToDelete = DBSessions.AccountSession.Query<DBInventory>().Where(inv => inv.DBGameAccount.Id == gameAccount.DBGameAccount.Id);
            foreach (var inv in inventoryToDelete)
                DBSessions.AccountSession.Delete(inv);



            gameAccount.DBGameAccount.DBAccount.DBGameAccounts.Remove(gameAccount.DBGameAccount);

            DBSessions.AccountSession.Update(gameAccount.DBGameAccount.DBAccount);
            DBSessions.AccountSession.Delete(gameAccount.DBGameAccount);
            DBSessions.AccountSession.Flush();

        }

    }
}
