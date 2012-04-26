using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mooege.Common.Storage.AccountDataBase.Entities;
using Mooege.Core.MooNet.Accounts;
using NHibernate;
using NHibernate.Linq;

namespace Mooege.Common.Storage
{
    public static class DBSessions
    {
        private static ISession _accountSession = null;
        public static ISession AccountSession
        {
            get
            {
                if (_accountSession == null || !_accountSession.IsOpen)
                {
                    _accountSession = AccountDataBase.SessionProvider.SessionFactory.OpenSession();
                }

                return _accountSession;
            }
        }
    }
}
