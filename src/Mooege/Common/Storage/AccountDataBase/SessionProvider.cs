using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using FluentNHibernate;
using FluentNHibernate.Cfg;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace Mooege.Common.Storage.AccountDataBase
{
    public class SessionProvider
    {
        private static ISessionFactory _sessionFactory;
        private static Configuration _config;

        public static ISessionFactory SessionFactory
        {
            get { return _sessionFactory ?? (_sessionFactory = CreateSessionFactory()); }
        }

        public static Configuration Config
        {
            get
            {
                if (_config == null)
                {
                    _config = new Configuration();
                    _config=_config.Configure(string.Format("database.Account.config"))
                        .AddMappingsFromAssembly(Assembly.GetCallingAssembly());
                }
                return _config;
            }
        }

        private static ISessionFactory CreateSessionFactory()
        {
            return Fluently.Configure(Config).ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(false, true)).
                BuildSessionFactory();
        }

        public static void RebuildSchema()
        {
            var schema = new SchemaExport(Config);
            schema.Create(true, true);
        }
    }
}

