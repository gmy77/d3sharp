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

using Mooege.Common.Helpers.Hash;
using Mooege.Core.GS.Map;
using Mooege.Core.GS.Common.Types.TagMap;

namespace Mooege.Core.GS.Actors.Implementations.Hirelings
{
    [HandledSNO(4062 /* Enchantress.acr */)]
    public class Enchantress : Hireling
    {
        public Enchantress(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
            mainSNO = 4062;
            hirelingSNO = -1;
            proxySNO = 192942;
            skillKit = 87094;
            hirelingGBID = StringHashHelper.HashItemName("Enchantress");
            Attributes[Mooege.Net.GS.Message.GameAttribute.Hireling_Class] = 3;
        }

        public override Hireling CreateHireling(World world, int snoId, TagMap tags)
        {
            return new Enchantress(world, snoId, tags);
        }
    }
}