using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Data;

namespace Mooege.Common.Storage.AccountDataBase.Entities
{
    public class DBInventory:Entity
    {
        public new virtual ulong Id { get; set; }
        public virtual DBGameAccount DBGameAccount { get; set; }
        public virtual DBToon DBToon { get; set; }
        public virtual int LocationX { get; set; }
        public virtual int LocationY { get; set; }
        public virtual int EquipmentSlot { get; set; }
        public virtual int ItemId { get; set; }

    }
}
