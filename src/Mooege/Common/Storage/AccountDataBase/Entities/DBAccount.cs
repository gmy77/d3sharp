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
            this.Friends=new List<DBAccount>();
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
