﻿﻿/*
 * Copyright (C) 2011 mooege project
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

using System.Collections.Generic;
using Mooege.Core.GS.Map;
using Mooege.Core.GS.Common.Types.TagMap;

namespace Mooege.Core.GS.Actors.Implementations
{
    public class StartingPoint : Gizmo
    {
        public int TargetId { get; private set; }

        public StartingPoint(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
        }

        protected override void ReadTags()
        {
            if (this.Tags == null) return;

            if (this.Tags.ContainsKey(MarkerKeys.ActorTag))
                this.TargetId = this.Tags[MarkerKeys.ActorTag];
        }
    }
}
