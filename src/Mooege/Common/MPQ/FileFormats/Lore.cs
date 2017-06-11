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
using Gibbed.IO;
using Mooege.Common.MPQ.FileFormats.Types;
using Mooege.Core.GS.Common.Types.SNO;

namespace Mooege.Common.MPQ.FileFormats
{
    [FileFormat(SNOGroup.Lore)]
    public class Lore : FileFormat
    {
        public Header Header { get; private set; }
        public int I0 { get; private set; }
        public LoreCategory Category { get; private set; }
        public int I1 { get; private set; }
        public int I2 { get; private set; }
        public int SNOConversation { get; private set; }
        public int I3 { get; private set; }

        public Lore(MpqFile file)
        {
            var stream = file.Open();
            this.Header = new Header(stream);
            this.I0 = stream.ReadValueS32();
            this.Category = (LoreCategory)stream.ReadValueS32();
            this.I1 = stream.ReadValueS32();
            this.I2 = stream.ReadValueS32();
            this.SNOConversation = stream.ReadValueS32();
            this.I3 = stream.ReadValueS32();
            stream.Close();
        }
    }
    public enum LoreCategory
    {
        Quest = 0,
        World,
        People,
        Bestiary,
    };
}