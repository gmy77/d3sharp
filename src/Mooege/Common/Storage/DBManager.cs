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
using System.Data.SQLite;
using System.IO;
using Mooege.Common.Helpers.IO;
using Mooege.Common.Logging;

namespace Mooege.Common.Storage
{
    // just a quick hack - not to be meant a final layer.
    public static class DBManager
    {
        public static SQLiteConnection MPQMirror { get; private set; }

        public static readonly Logger Logger = LogManager.CreateLogger();

        public static string AssetDirectory
        {
            get
            {
                var dataDirectory = String.Format(@"{0}/{1}", FileHelpers.AssemblyRoot, Config.Instance.Root);

                if (Path.IsPathRooted(Config.Instance.Root))
                    //Path is rooted... dont use assemblyRoot, as its absolute path.
                    dataDirectory = Config.Instance.Root;
                return dataDirectory;
            }
        }

        static DBManager()
        {
            Connect();
        }

        private static void Connect()
        {
            try
            {

                MPQMirror = new SQLiteConnection(String.Format("Data Source={0}/mpqdata.db", AssetDirectory));
                MPQMirror.Open();
            }
            catch (Exception e)
            {
                Logger.FatalException(e, "Connect()");
            }
        }
    }
}
