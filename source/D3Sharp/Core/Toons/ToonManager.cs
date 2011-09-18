using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
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

        public static Toon GetToon(ulong id)
        {
            return (from pair in Toons where pair.Value.ID == id select pair.Value).FirstOrDefault();
        }

        public static Dictionary<ulong,Toon> GetToonsByEmail(string email)
        {
            return Toons.Where(pair => pair.Value.AccountEmail == email).ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        public static bool SaveToon(Toon toon)
        {
            if(Toons.ContainsKey(toon.ID))
            {
                Logger.Error("Duplicate toon id: " + toon.ID);
                return false;
            }

            Toons.Add(toon.ID, toon);
            toon.SaveToDB();
            return true;
        }

        public static void DeleteToon(Toon toon)
        {
            if (!Toons.ContainsKey(toon.ID))
            {
                Logger.Error("Attempting to delete toon that does not exist: " + toon.ID);
                return;
            }

            if (toon.DeleteFromDB()) Toons.Remove(toon.ID);
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
                var toon = new Toon(id, reader.GetString(1), reader.GetByte(2), reader.GetByte(3), reader.GetByte(4), reader.GetString(5));
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
