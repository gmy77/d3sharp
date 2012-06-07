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
using Mooege.Core.MooNet.Accounts;
using NHibernate.Linq;

namespace Mooege.Core.MooNet.Toons
{
    // Just a quick hack - not to be meant final
    public static class ToonManager
    {
        private static readonly HashSet<Toon> LoadedToons = new HashSet<Toon>();
        private static readonly Logger Logger = LogManager.CreateLogger();


        public static Toon GetToonByDBToon(DBToon dbToon)
        {
            if (!LoadedToons.Any(dbt => dbt.DBToon.Id == dbToon.Id))
                LoadedToons.Add(new Toon(dbToon));
            return LoadedToons.Single(dbt => dbt.DBToon.Id == dbToon.Id);
        }


        public static Account GetOwnerAccountByToonLowId(ulong id)
        {
            return GetToonByLowID(id).GameAccount.Owner;
        }

        public static GameAccount GetOwnerGameAccountByToonLowId(ulong id)
        {
            return GetToonByLowID(id).GameAccount;
        }



        public static Toon GetToonByLowID(ulong id)
        {
            var dbToon = DBSessions.AccountSession.Get<DBToon>(id);
            return GetToonByDBToon(dbToon);
        }

        public static Toon GetDeletedToon(GameAccount account)
        {
            var query = DBSessions.AccountSession.Query<DBToon>().Where(dbt => dbt.DBGameAccount.Id == account.PersistentID && dbt.Deleted);
            return query.Any() ? GetToonByLowID(query.First().Id) : null;
        }

        public static List<Toon> GetToonsForGameAccount(GameAccount account)
        {
            var toons = account.DBGameAccount.DBToons.Select(dbt => GetToonByLowID(dbt.Id));
            return toons.ToList();
        }


        public static int TotalToons
        {
            get { return DBSessions.AccountSession.Query<DBToon>().Count(); }
        }


        public static Toon CreateNewToon(string name, int classId, ToonFlags flags, byte level, GameAccount gameAccount)
        {
            var dbGameAccount = DBSessions.AccountSession.Get<DBGameAccount>(gameAccount.PersistentID);
            var newDBToon = new DBToon
                                {
                                    Class = @Toon.GetClassByID(classId),
                                    Name = name,
                                    /*HashCode = GetUnusedHashCodeForToonName(name),*/
                                    Flags = flags,
                                    Level = level,
                                    DBGameAccount = DBSessions.AccountSession.Get<DBGameAccount>(gameAccount.PersistentID)
                                };

            dbGameAccount.DBToons.Add(newDBToon);
            DBSessions.AccountSession.SaveOrUpdate(dbGameAccount);
            DBSessions.AccountSession.Flush();


            return GetToonByLowID(newDBToon.Id);
        }

        public static void DeleteToon(Toon toon)
        {
            if (toon == null)
                return;

            //remove toonActiveSkills
            if (toon.DBToon.DBActiveSkills != null)
            {
                DBSessions.AccountSession.Delete(toon.DBToon.DBActiveSkills);
                toon.DBToon.DBActiveSkills = null;
            }

            //remove toon inventory
            var inventoryToDelete = DBSessions.AccountSession.Query<DBInventory>().Where(inv => inv.DBToon.Id == toon.DBToon.Id);
            foreach (var inv in inventoryToDelete)
            {
                //toon.DBToon.DBGameAccount.DBInventories.Remove(inv);
                DBSessions.AccountSession.Delete(inv);
            }




            //remove lastplayed hero if it was toon
            if (toon.DBToon.DBGameAccount.LastPlayedHero != null && toon.DBToon.DBGameAccount.LastPlayedHero.Id == toon.DBToon.Id)
                toon.DBToon.DBGameAccount.LastPlayedHero = null;


            //remove toon from dbgameaccount
            while (toon.DBToon.DBGameAccount.DBToons.Contains(toon.DBToon))
                toon.DBToon.DBGameAccount.DBToons.Remove(toon.DBToon);

            //save all this thinks
            DBSessions.AccountSession.SaveOrUpdate(toon.DBToon.DBGameAccount);
            DBSessions.AccountSession.Delete(toon.DBToon);
            DBSessions.AccountSession.Flush();


            //remove toon from loadedToon list
            if (LoadedToons.Contains(toon))
                LoadedToons.Remove(toon);

            Logger.Debug("Deleting toon {0}", toon.PersistentID);
        }


        public static void Sync()
        {
            foreach (var toon in LoadedToons)
            {
                SaveToDB(toon);
            }
        }

        public static void SaveToDB(Toon toon)
        {
            try
            {
                // save character base data
                var dbToon = DBSessions.AccountSession.Get<DBToon>(toon.PersistentID);
                dbToon.Name = toon.Name;
                /*dbToon.HashCode = toon.HashCode;*/
                dbToon.Class = toon.Class;
                dbToon.Flags = toon.Flags;
                dbToon.Level = toon.Level;
                dbToon.Experience = toon.ExperienceNext;
                dbToon.DBGameAccount = DBSessions.AccountSession.Get<DBGameAccount>(toon.GameAccount.PersistentID);
                dbToon.TimePlayed = toon.TimePlayed;
                dbToon.Deleted = toon.Deleted;

                DBSessions.AccountSession.SaveOrUpdate(dbToon);
                DBSessions.AccountSession.Flush();
            }
            catch (Exception e)
            {
                Logger.ErrorException(e, "Toon.SaveToDB()");
            }
        }

    }
}
