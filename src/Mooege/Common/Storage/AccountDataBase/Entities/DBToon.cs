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
