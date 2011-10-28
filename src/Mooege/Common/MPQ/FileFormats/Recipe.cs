/*
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

using CrystalMpq;
using Gibbed.IO;
using Mooege.Common.MPQ.FileFormats.Types;
using Mooege.Core.GS.Common.Types.SNO;

// Appears to work fine, created from snodata.xml - DarkLotus
namespace Mooege.Common.MPQ.FileFormats
{
    [FileFormat(SNOGroup.Recipe)]
    public class Recipe : FileFormat
    {
        public Header Header { get; private set; }
        public int SNO { get; private set; }
        public ItemSpecifierData ItemSpecifierData { get; private set; }

        public Recipe(MpqFile file)
        {
            var stream = file.Open();
            this.Header = new Header(stream);
            this.SNO = stream.ReadValueS32();
            stream.Position += (2 * 4);
            ItemSpecifierData = new ItemSpecifierData(stream);
            stream.Close();
        }
    }

    public class ItemSpecifierData
    {
        public int ItemGBId { get; private set; }
        public int I0 { get; private set; }
        public int[] GBIdAffixes = new int[3];
        public int I1 { get; private set; }

        public ItemSpecifierData(MpqFileStream stream)
        {
            ItemGBId = stream.ReadValueS32();
            I0 = stream.ReadValueS32();
            for (int i = 0; i > 3; i++)
            {
                GBIdAffixes[i] = stream.ReadValueS32();
            }
            I1 = stream.ReadValueS32();
        }
    }
}