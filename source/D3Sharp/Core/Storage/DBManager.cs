using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Reflection;
using System.Text;
using D3Sharp.Utils;

namespace D3Sharp.Core.Storage
{
    // just a quick hack - not to be meant a final layer.
    public static class DBManager
    {
        public static SQLiteConnection Connection { get; private set; }
        public static readonly Logger Logger = LogManager.CreateLogger();

        static DBManager()
        {
            Connect();            
        }

        private static void Connect()
        {
            try
            {
                Connection = new SQLiteConnection(string.Format("Data Source={0}/Assets/toon.db",System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)));
                Connection.Open();
            }
            catch (Exception e)
            {
                Logger.FatalException(e, "Connect()");
            }
        }
    }
}
