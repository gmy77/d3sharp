/*
 * Copyright (C) 2011 D3Sharp Project
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
using D3Sharp.Core.Accounts;
using D3Sharp.Core.Storage;
using D3Sharp.Utils;

namespace D3Sharp.Core.Toons
{
    // just a quick hack - not to be meant a final.
    public static class ToonManager
    {
        private static readonly Dictionary<ulong, Toon> Toons =
            new Dictionary<ulong, Toon>();

        private static readonly Logger Logger = LogManager.CreateLogger();

        static ToonManager()
        {
            LoadToons();
        }

        public static Toon GetToonByLowID(ulong id)
        {
            return (from pair in Toons where pair.Value.Id == id select pair.Value).FirstOrDefault();
        }

        public static Dictionary<ulong, Toon> GetToonsForAccount(Account account)
        {
            return Toons.Where(pair => (ulong)pair.Value.AccountID == account.Id).ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        public static bool SaveToon(Toon toon)
        {
            if(Toons.ContainsKey(toon.Id))
            {
                Logger.Error("Duplicate toon id: " + toon.Id);
                return false;
            }

            Toons.Add(toon.Id, toon);
            toon.SaveToDB();
            return true;
        }

        public static void DeleteToon(Toon toon)
        {
            if (!Toons.ContainsKey(toon.Id))
            {
                Logger.Error("Attempting to delete toon that does not exist: " + toon.Id);
                return;
            }

            if (toon.DeleteFromDB()) Toons.Remove(toon.Id);
        }

        private static void LoadToons()
        {
            var query = "SELECT * from toons";
            var cmd = new SQLiteCommand(query, DBManager.Connection);
            var reader = cmd.ExecuteReader();

            if (!reader.HasRows) return;

            while(reader.Read())
            {
                var id = (ulong) reader.GetInt64(0);
                var toon = new Toon(id, reader.GetString(1), reader.GetByte(2), reader.GetByte(3), reader.GetByte(4), reader.GetInt64(5));
                Toons.Add(id, toon);
            }
        }

        public static void Sync()
        {
            foreach(var pair in Toons)
            {
                pair.Value.SaveToDB();
            }
        }
    }
}
