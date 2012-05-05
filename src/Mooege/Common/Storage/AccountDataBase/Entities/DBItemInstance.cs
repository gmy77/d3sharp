using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Data;

namespace Mooege.Common.Storage.AccountDataBase.Entities
{
    public class DBItemInstance : Entity
    {
        public new virtual ulong Id { get; set; }
        public virtual int GbId { get; set; }
        public virtual string Affixes { get; set; }
        public virtual string Attributes { get; set; }
    }
}
