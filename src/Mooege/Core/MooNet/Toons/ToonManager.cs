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
        private static readonly List<Toon> LoadedToons = new List<Toon>();
        private static readonly IQueryable<DBToon> DBToons = DBSessions.AccountSession.Query<DBToon>();

        private static readonly Logger Logger = LogManager.CreateLogger();

        static ToonManager()
        {
            //LoadToons();
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
            if (!LoadedToons.Any(t => t.PersistentID == id))
            {
                if (!DBToons.Any(t => t.Id == id))
                    return null;
                var dbToon = DBToons.Single(t => t.Id == id);
                LoadedToons.Add(new Toon(dbToon));
            }


            return LoadedToons.Single(t => t.PersistentID == id);
        }

        public static Toon GetDeletedToon(GameAccount account)
        {
            var query = DBToons.Where(dbt => dbt.DBGameAccount.Id == account.PersistentID && dbt.Deleted);
            if (query.Any())
                return GetToonByLowID(query.First().Id);
            return null;
        }

        public static Dictionary<ulong, Toon> GetToonsForGameAccount(GameAccount account)
        {
            var dbGameAccount = DBSessions.AccountSession.Get<DBGameAccount>(account.PersistentID);
            var toons = dbGameAccount.DBToons.Select(dbt => GetToonByLowID(dbt.Id));
            return toons.ToDictionary(toon => toon.PersistentID);
        }

        //public static Dictionary<ulong, Toon> GetToonsForAccount(Account account)
        //{
        //    return Toons.Where(pair => pair.Value.GameAccount.Owner != null).Where(pair => pair.Value.GameAccount.Owner.PersistentID == account.PersistentID).ToDictionary(pair => pair.Key, pair => pair.Value);
        //}

        public static int TotalToons
        {
            get { return 0; }
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
            //if (!Toons.Any(t => t.PersistentID == toon.PersistentID))
            //{
            //    Logger.Error("Attempting to delete toon that does not exist: {0}", toon.PersistentID);
            //    return;
            //}

            var dbToon = DBToons.Single(dbt => dbt.Id == toon.PersistentID);
            dbToon.DBGameAccount.DBToons.Remove(dbToon);
            DBSessions.AccountSession.Update(dbToon.DBGameAccount);
            DBSessions.AccountSession.Delete(dbToon);
            DBSessions.AccountSession.Flush();

            if (LoadedToons.Any(t => t.PersistentID == toon.PersistentID))
                LoadedToons.RemoveAll(t => t.PersistentID == toon.PersistentID);

            Logger.Debug("Deleting toon {0}", toon.PersistentID);
        }

        private static void LoadToons()
        {
            /*
            var allDBToons = DBSessions.AccountSession.Query<DBToon>().ToList();

            foreach (var dbToon in allDBToons)
                Toons.Add(new Toon(dbToon));*/
        }

        /*
        public static int GetUnusedHashCodeForToonName(string name)
        {
            var codes = DBSessions.AccountSession.Query<DBToon>().Select(dba => dba.HashCode).ToList();
            return GenerateHashCodeNotInList(codes);
        }
        */
        public static void Sync()
        {
            foreach (var toon in LoadedToons)
            {
                SaveToDB(toon);
            }
        }

        private static int GenerateHashCodeNotInList(ICollection<int> codes)
        {
            var rnd = new Random();
            if (codes == null) return rnd.Next(1, 1000);

            int hashCode;
            do
            {
                hashCode = rnd.Next(1, 1000);
            } while (codes.Contains(hashCode));
            return hashCode;

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
