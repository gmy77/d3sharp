using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Data;

namespace Mooege.Common.Storage.AccountDataBase.Entities
{
    public class DBGameAccount : Entity
    {
        public new virtual ulong Id { get; set; }
        public virtual DBAccount DBAccount { get; set; }
        public virtual byte[] Banner { get; set; }
        public virtual long LastOnline { get; set; }
    }
}
