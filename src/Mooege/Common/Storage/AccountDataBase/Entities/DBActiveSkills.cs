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

namespace Mooege.Common.Storage.AccountDataBase.Entities
{
    public class DBActiveSkills : Entity
    {
        public virtual new ulong Id { get; protected set; }
        public virtual DBToon DBToon { get; set; }
        public virtual int Skill0 { get; set; }
        public virtual int Rune0 { get; set; }
        public virtual int Skill1 { get; set; }
        public virtual int Rune1 { get; set; }
        public virtual int Skill2 { get; set; }
        public virtual int Rune2 { get; set; }
        public virtual int Skill3 { get; set; }
        public virtual int Rune3 { get; set; }
        public virtual int Skill4 { get; set; }
        public virtual int Rune4 { get; set; }
        public virtual int Skill5 { get; set; }
        public virtual int Rune5 { get; set; }

        public virtual int Passive0 { get; set; }
        public virtual int Passive1 { get; set; }
        public virtual int Passive2 { get; set; }
    }
}
