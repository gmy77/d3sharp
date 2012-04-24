using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;

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
                    _accountSession = AccountDataBase.SessionProvider.SessionFactory.OpenSession();

                return _accountSession;
            }
        }
    }
}
