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

using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using Mooege.Common;
using Mooege.Core.Common.Storage;
using Mooege.Core.MooNet.Accounts;
using System;

namespace Mooege.Core.Common.Toons
{
    // Just a quick hack - not to be meant final
    public static class ToonManager
    {
        private static readonly Dictionary<ulong, Toon> Toons =
            new Dictionary<ulong, Toon>();

        private static readonly Logger Logger = LogManager.CreateLogger();

        static ToonManager()
        {
            LoadToons();
        }

        public static Account GetOwnerAccountByToonLowId(ulong id)
        {
            var toon = (from pair in Toons where pair.Value.PersistentID == id select pair.Value).FirstOrDefault();
            return (toon != null) ? toon.Owner : null;
        }

        public static Toon GetToonByLowID(ulong id)
        {
            return (from pair in Toons where pair.Value.PersistentID == id select pair.Value).FirstOrDefault();
        }

        public static Dictionary<ulong, Toon> GetToonsForAccount(Account account)
        {
            return Toons.Where(pair => pair.Value.Owner != null).Where(pair => pair.Value.Owner.PersistentID == account.PersistentID).ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        //Method only used when creating a Toon for the first time, ambiguous method name - Tharuler
        public static bool SaveToon(Toon toon)
        {

            if (Toons.ContainsKey(toon.PersistentID)) //this should never happen again thanks to hashcode, but lets leave it in for now - Tharuler
            {
                Logger.Error("Duplicate persistent toon id: {0}", toon.PersistentID);
                return false;
            }

            Toons.Add(toon.PersistentID, toon);
            toon.SaveToDB(); //possible concurrency problem? 2 toon created with same name at same time could introduce a race condition for the same hashcode(chance of 1 in (1000-amount of toons with that name))

            Logger.Trace("Character {0} with HashCode #{1} added to database", toon.Name, toon.HashCodeString);

            return true;
        }

        public static void DeleteToon(Toon toon)
        {
            if (!Toons.ContainsKey(toon.PersistentID))
            {
                Logger.Error("Attempting to delete toon that does not exist: {0}", toon.PersistentID);
                return;
            }

            if (toon.DeleteFromDB()) Toons.Remove(toon.PersistentID);
        }

        private static void LoadToons()
        {
            var query = "SELECT * from toons";
            var cmd = new SQLiteCommand(query, DBManager.Connection);
            var reader = cmd.ExecuteReader();

            if (!reader.HasRows) return;

            while (reader.Read())
            {
                var databaseId = (ulong)reader.GetInt64(0);
                var toon = new Toon(databaseId, reader.GetString(1), reader.GetInt32(6), reader.GetByte(2), reader.GetByte(3), reader.GetByte(4), reader.GetInt64(5));
                Toons.Add(databaseId, toon);
            }
        }

        public static int GetUnusedHashCodeForToonName(string name)
        {
            var query = string.Format("SELECT hashCode from toons WHERE name='{0}'", name);
            Logger.Trace(query);
            var cmd = new SQLiteCommand(query, DBManager.Connection);
            var reader = cmd.ExecuteReader();
            if (!reader.HasRows) return GenerateHashCodeNotInList(null);

            HashSet<int> codes = new HashSet<int>();
            while (reader.Read())
            {
                var hashCode = reader.GetInt32(0);
                codes.Add(hashCode);
            }
            return GenerateHashCodeNotInList(codes);
        }

        public static void Sync()
        {
            foreach (var pair in Toons)
            {
                pair.Value.SaveToDB();
            }
        }

        private static int GenerateHashCodeNotInList(HashSet<int> codes)
        {
            Random rnd = new Random();
            if (codes == null) return rnd.Next(1, 1000);

            int hashCode;
            do
            {
                hashCode = rnd.Next(1, 1000);
            } while (codes.Contains(hashCode)) ;
            return hashCode;

        }
    }
}
