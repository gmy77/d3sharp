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
    public class DBToonMapper : ClassMap<DBToon>
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
