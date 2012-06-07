/*
 * Copyright (C) 2011 - 2012 mooege project - http://www.mooege.org
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Mooege.Common.Storage.AccountDataBase.Entities;

namespace Mooege.Common.Storage.AccountDataBase.Mapper
{
    public class DBAccountMapper : ClassMap<DBAccount>
    {
        public DBAccountMapper()
        {
            Id(e => e.Id).GeneratedBy.Native();
            Map(e => e.Email);
            Map(e => e.Salt)/*.CustomSqlType("VarBinary(32)")*/.Length(32);
            Map(e => e.PasswordVerifier)/*.CustomSqlType("VarBinary")*/.Length(128);
            Map(e => e.BattleTagName);
            Map(e => e.HashCode);
            Map(e => e.UserLevel);
            Map(e => e.LastOnline);
            HasMany(e => e.DBGameAccounts).Cascade.All();//Cascade all means if this Account gets deleted/saved/update ALL GameAccounts do the same :)
            HasManyToMany(e => e.Friends).ParentKeyColumn("AccountAId").ChildKeyColumn("AccountBId").Cascade.SaveUpdate();
        }
    }
}
