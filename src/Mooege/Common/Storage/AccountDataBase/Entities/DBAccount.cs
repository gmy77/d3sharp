using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Data;

namespace Mooege.Common.Storage.AccountDataBase.Entities
{
    public class DBAccount : Entity
    {
        public new virtual ulong Id { get; set; }
        public virtual string Email { get; set; }
        public virtual byte[] Salt { get; set; }
        public virtual byte[] PasswordVerifier { get; set; }
        public virtual string BattleTagName { get; set; }
        public virtual int HashCode { get; set; }
        public virtual int UserLevel { get; set; }
        public virtual ulong LastSelectedHeroId { get; set; }
        public virtual long LastOnline { get; set; }
    }
}
