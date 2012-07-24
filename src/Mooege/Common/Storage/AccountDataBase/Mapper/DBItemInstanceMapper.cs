using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Mooege.Common.Storage.AccountDataBase.Entities;

namespace Mooege.Common.Storage.AccountDataBase.Mapper
{
        public class DBItemInstanceMapper : ClassMap<DBItemInstance>
        {
            public DBItemInstanceMapper()
            {
                Id(e => e.Id).GeneratedBy.Native();
                Map(e => e.GbId);
                Map(e => e.Affixes);
                Map(e => e.Attributes);
            }
        }
    
}
