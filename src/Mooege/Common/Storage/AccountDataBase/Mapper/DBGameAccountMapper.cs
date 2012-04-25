using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Mooege.Common.Storage.AccountDataBase.Entities;

namespace Mooege.Common.Storage.AccountDataBase.Mapper
{
    public class DBGameAccountMapper : ClassMap<DBGameAccount>
    {
        public DBGameAccountMapper()
        {
            Id(e => e.Id).GeneratedBy.Increment();
            HasOne(e => e.DBAccount).Constrained();
            Map(e => e.Banner).CustomSqlType("Blob");
            Map(e => e.LastOnline);
            HasMany(e => e.DBToons).Cascade.All();
            HasMany(e => e.DBInventories).Cascade.All();
        }
    }
}
