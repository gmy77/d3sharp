using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Data;
using Mooege.Core.MooNet.Toons;

namespace Mooege.Common.Storage.AccountDataBase.Entities
{
    public class DBToon : Entity
    {
        public new virtual ulong Id { get; protected set; }
        public virtual string Name { get; set; }
        public virtual ToonClass Class { get; set; }
        public virtual ToonFlags Flags { get; set; }
        public virtual byte Level { get; set; }
        public virtual int Experience { get; set; }
        public virtual DBGameAccount DBGameAccount { get; set; }
        public virtual uint TimePlayed { get; set; }
        public virtual bool Deleted { get; set; }

        public virtual DBActiveSkills DBActiveSkills { get; set; }

    }
}
