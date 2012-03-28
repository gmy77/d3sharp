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
using Mooege.Core.MooNet.Helpers;
using Mooege.Common.Logging;
using Mooege.Common.Storage;
using Mooege.Core.MooNet.Toons;

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

        public static ulong GetNextAvailablePersistentId()
        {
            var cmd = new SQLiteCommand("SELECT MAX(id) FROM gameaccounts", DBManager.Connection);
            try
            {
                return Convert.ToUInt64(cmd.ExecuteScalar());
            }
            catch (InvalidCastException)
            {
                return 0;
            }
        }

        static GameAccountManager()
        {
            LoadGameAccounts();
        }

        private static void LoadGameAccounts()
        {
            var query = "SELECT * FROM gameaccounts";
            var cmd = new SQLiteCommand(query, DBManager.Connection);
            var reader = cmd.ExecuteReader();

            if (!reader.HasRows) return;

            while (reader.Read())
            {
                var gameAccountId = Convert.ToUInt64(reader["id"]);
                var accountId = Convert.ToUInt64(reader["accountid"]);
                var gameAccount = new GameAccount(gameAccountId, accountId);

                #region Populate GameAccount Data

                var banner = (byte[])reader.GetValue(2);
                gameAccount.BannerConfiguration = D3.Account.BannerConfiguration.ParseFrom(banner);
                gameAccount.LastOnlineField.Value = Convert.ToInt64(reader["LastOnline"]);
                GameAccounts.Add(gameAccountId, gameAccount);

                #endregion
            }
        }

        public static GameAccount CreateGameAccount(Account account)
        {
            var gameAccount = new GameAccount(account);
            GameAccounts.Add(gameAccount.PersistentID, gameAccount);
            gameAccount.SaveToDB();
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

            if (GameAccount.DeleteFromDB()) GameAccounts.Remove(GameAccount.PersistentID);
        }

    }
}
