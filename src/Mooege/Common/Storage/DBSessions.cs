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
        private static Object _sessionLock = new object();
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
