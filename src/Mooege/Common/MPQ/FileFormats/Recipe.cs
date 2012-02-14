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

using CrystalMpq;
using Mooege.Common.MPQ.FileFormats.Types;
using Mooege.Core.GS.Common.Types.SNO;

// Appears to work fine, created from snodata.xml - DarkLotus
namespace Mooege.Common.MPQ.FileFormats
{
    [FileFormat(SNOGroup.Recipe)]
    public class Recipe : FileFormat
    {
        public Header Header { get; private set; }
        public ItemSpecifierData ItemSpecifierData { get; private set; }

        public Recipe(MpqFile file)
        {
            var stream = file.Open();
            this.Header = new Header(stream);
            ItemSpecifierData = new ItemSpecifierData(stream);
            stream.Close();
        }
    }
}