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
        private static Object _sessionLock=new object();
        private static ISession _accountSession = null;
        
        public static ISession AccountSession
        {
            get
            {
                lock (_sessionLock)
                {
                    if (_accountSession == null || !_accountSession.IsOpen)
                    {
                        _accountSession = AccountDataBase.SessionProvider.SessionFactory.OpenSession();
                        _accountSession.FlushMode = FlushMode.Always;
                    }
                }
                return _accountSession;
            }
        }
    }
}
