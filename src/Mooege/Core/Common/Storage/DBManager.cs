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

using System;
using System.Data.SQLite;
using Mooege.Common;
using Mooege.Common.Helpers;

namespace Mooege.Core.Common.Storage
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
                Connection = new SQLiteConnection(String.Format("Data Source={0}/{1}/account.db", FileHelpers.AssemblyRoot, Config.Instance.Root));
                Connection.Open();
            }
            catch (Exception e)
            {
                Logger.FatalException(e, "Connect()");
            }
        }
    }
}
