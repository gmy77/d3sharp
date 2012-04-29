using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Mooege.Common.Storage.AccountDataBase.Entities;

namespace Mooege.Common.Storage.AccountDataBase.Mapper
{
    public class DBToonMapper:ClassMap<DBToon>
    {
        public DBToonMapper()
        {
            Id(e => e.Id).GeneratedBy.Native();
            Map(e => e.Class);
            References(e => e.DBGameAccount);
            Map(e => e.Deleted);
            Map(e => e.Experience);
            Map(e => e.Flags);
            Map(e => e.Level);
            Map(e => e.Name);
            Map(e => e.TimePlayed);
            
            HasOne(e => e.DBActiveSkills).Cascade.All();
        }
    }
}
