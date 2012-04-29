using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Mooege.Common.Storage.AccountDataBase.Entities;

namespace Mooege.Common.Storage.AccountDataBase.Mapper
{
    public class DBInventoryMapper:ClassMap<DBInventory>
    {
        public DBInventoryMapper()
        {
            Id(e => e.Id).GeneratedBy.Native();
            References(e => e.DBGameAccount).Nullable();
            References(e => e.DBToon).Nullable();
            Map(e => e.EquipmentSlot);
            Map(e => e.ItemId);
            Map(e => e.LocationX);
            Map(e => e.LocationY);
            
        }
    }
}
