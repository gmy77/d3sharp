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
using FluentNHibernate.Data;
using Mooege.Core.MooNet.Accounts;

namespace Mooege.Common.Storage.AccountDataBase.Entities
{
    public class DBAccount : Entity
    {
        public DBAccount()
        {
            this.DBGameAccounts = new List<DBGameAccount>();
            this.Friends = new List<DBAccount>();
        }

        public virtual new ulong Id { get; protected set; }
        public virtual string Email { get; set; }
        public virtual byte[] Salt { get; set; }
        public virtual byte[] PasswordVerifier { get; set; }
        public virtual string BattleTagName { get; set; }
        public virtual int HashCode { get; set; }
        public virtual Account.UserLevels UserLevel { get; set; }
        public virtual long LastOnline { get; set; }
        public virtual IList<DBGameAccount> DBGameAccounts { get; protected set; }
        public virtual IList<DBAccount> Friends { get; protected set; }
    }
}
