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
using Mooege.Net.GS.Message;
using Mooege.Core.GS.Common.Types.TagMap;

namespace Mooege.Core.GS.Actors.Implementations.Hirelings
{
    [HandledSNO(4644 /* Scoundrel.acr */)]
    public class Scoundrel : Hireling
    {
        public Scoundrel(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
            mainSNO = 4644;
            hirelingSNO = 52694;
            proxySNO = 192941;
            skillKit = 0x8AFE;
            hirelingGBID = StringHashHelper.HashItemName("Scoundrel");
            Attributes[GameAttribute.Hireling_Class] = 2;
        }

        public override Hireling CreateHireling(World world, int snoId, TagMap tags)
        {
            return new Scoundrel(world, snoId, tags);
        }
    }
}