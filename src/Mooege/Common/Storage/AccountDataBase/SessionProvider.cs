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
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using FluentNHibernate;
using FluentNHibernate.Cfg;
using Mooege.Common.Storage.AccountDataBase.Mapper;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using Mooege.Common.Helpers.IO;

namespace Mooege.Common.Storage.AccountDataBase
{
    public class SessionProvider
    {
        private static ISessionFactory _sessionFactory;
        private static Configuration _config;
        private static readonly object Lockobj = new object();
        public static ISessionFactory SessionFactory
        {
            get
            {
                lock (Lockobj)
                {
                    return _sessionFactory ?? (_sessionFactory = CreateSessionFactory());
                }
            }
        }


        public static Configuration Config
        {
            get
            {
                if (_config == null)
                {
                    _config = new Configuration();
                    _config = _config.Configure(Path.Combine(FileHelpers.AssemblyRoot, "database.Account.config"));


                    var replacedProperties = new Dictionary<string, string>();
                    foreach (var prop in _config.Properties)
                    {
                        var newvalue = prop.Value;
                        newvalue = newvalue.Replace("{$ASSETBASE}", DBManager.AssetDirectory);
                        replacedProperties.Add(prop.Key, newvalue);
                    }


                    _config = _config.SetProperties(replacedProperties);
                    _config = _config.AddMappingsFromAssembly(Assembly.GetAssembly(typeof(DBAccountMapper)));
                    if (_config.Properties.ContainsKey("dialect"))
                        if (_config.GetProperty("dialect").ToLower().Contains("sqlite"))
                            _config = _config.SetProperty("connection.release_mode", "on_close");
                }

                return _config;
            }
        }

        private static ISessionFactory CreateSessionFactory()
        {
            return Fluently.Configure(Config).ExposeConfiguration(
                cfg =>
                    new SchemaUpdate(cfg).Execute(true, true)
                ).
                BuildSessionFactory();
        }

        public static void RebuildSchema()
        {
            var schema = new SchemaUpdate(Config);
            schema.Execute(true, true);
        }
    }
}

