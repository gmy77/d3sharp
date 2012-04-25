using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Data;

namespace Mooege.Common.Storage.AccountDataBase.Entities
{
    public class DBGameAccount : Entity
    {
        public DBGameAccount()
        {
            this.DBToons = new List<DBToon>();
            this.DBInventories=new List<DBInventory>();
        }
        public new virtual ulong Id { get; protected set; }
        public virtual DBAccount DBAccount { get; set; }
        public virtual byte[] Banner { get; set; }
        public virtual long LastOnline { get; set; }
        public virtual IList<DBToon> DBToons { get; protected set; }
        public virtual IList<DBInventory> DBInventories { get; protected set; }
    }
}
