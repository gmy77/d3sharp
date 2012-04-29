using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Mooege.Common.Storage.AccountDataBase.Entities;

namespace Mooege.Common.Storage.AccountDataBase.Mapper
{
    public class DBActiveSkillsMapper : ClassMap<DBActiveSkills>
    {
        public DBActiveSkillsMapper()
        {
            Id(e => e.Id).GeneratedBy.Native();
            HasOne(e => e.DBToon);
            Map(e => e.Rune0);
            Map(e => e.Skill0);
            Map(e => e.Rune1);
            Map(e => e.Skill1);
            Map(e => e.Rune2);
            Map(e => e.Skill2);
            Map(e => e.Rune3);
            Map(e => e.Skill3);
            Map(e => e.Rune4);
            Map(e => e.Skill4);
            Map(e => e.Rune5);
            Map(e => e.Skill5);

            Map(e => e.Passive0);
            Map(e => e.Passive1);
            Map(e => e.Passive2);
        }
    }
}
