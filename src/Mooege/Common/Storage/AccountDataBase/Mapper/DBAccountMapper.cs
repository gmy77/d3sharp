using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Mooege.Common.Storage.AccountDataBase.Entities;

namespace Mooege.Common.Storage.AccountDataBase.Mapper
{
    public class DBAccountMapper:ClassMap<DBAccount>
    {
        public DBAccountMapper()
        {
            Id(e => e.Id).GeneratedBy.Increment();
            Map(e => e.Email);
            Map(e => e.Salt).CustomSqlType("VarBinary").Length(32);
            Map(e => e.PasswordVerifier).CustomSqlType("VarBinary").Length(128);
            Map(e => e.BattleTagName);
            Map(e => e.HashCode);
            Map(e => e.UserLevel);
            Map(e => e.LastOnline);
            HasMany(e => e.DBGameAccounts).Cascade.All();//Cascade all means if this Account gets deleted/saved/update ALL GameAccounts do the same :)
        }
    }
}
