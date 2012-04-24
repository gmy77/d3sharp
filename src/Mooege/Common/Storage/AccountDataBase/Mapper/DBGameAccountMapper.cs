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
            Id(e => e.Id).GeneratedBy.Assigned();
            HasOne(e => e.DBAccount).Constrained();
            Map(e => e.Banner).CustomSqlType("Blob");
            Map(e => e.LastOnline);
        }
    }
}
