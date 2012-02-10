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
using Mooege.Common.Logging;

namespace Mooege.Core.GS.Items
{
    public class Affix
    {
        public static readonly Logger Logger = LogManager.CreateLogger();
        public int AffixGbid { get; set; }

        public Affix(int gbid)
        {
            AffixGbid = gbid;
        }

        public override String ToString()
        {
            return String.Format("{0}", AffixGbid);
        }

        public static Affix Parse(String affixString)
        {
            try
            {
                int gbid = int.Parse(affixString);
                var affix = new Affix(gbid);
                return affix;
            }
            catch (Exception e)
            {
                throw new Exception(String.Format("Affix can not be parsed: {0}", affixString), e);
            }
        }

    }
}
