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

using System.Collections.Generic;
using CrystalMpq;
using Gibbed.IO;
using Mooege.Common.MPQ.FileFormats.Types;
using Mooege.Core.GS.Common.Types.SNO;

namespace Mooege.Common.MPQ.FileFormats
{
    [FileFormat(SNOGroup.TreasureClass)]
    public class TreasureClass : FileFormat
    {
        public Header Header { get; private set; }
        public float Percentage { get; private set; }
        public int I0 { get; private set; }
        public int I1 { get; private set; }
        public List<LootDropModifier> ModifierList { get; private set; }

        public TreasureClass(MpqFile file)
        {
            var stream = file.Open();
            this.Header = new Header(stream);
            this.Percentage = stream.ReadValueF32();
            this.I0 = stream.ReadValueS32();
            this.I1 = stream.ReadValueS32();
            stream.Position += 8;
            this.ModifierList = stream.ReadSerializedData<LootDropModifier>();
            stream.Close();
        }

    }

    public class LootDropModifier : ISerializableData
    {
        public int I0 { get; private set; }
        public int SNOSubTreasureClass { get; private set; }
        public float Percentage { get; private set; }
        public int I1 { get; private set; }
        public int GBIdQualityClass { get; private set; }
        public int I2 { get; private set; }
        public int I3 { get; private set; }
        public int SNOCondition { get; private set; }
        public ItemSpecifierData ItemSpecifier { get; private set; }
        public int I5 { get; private set; }
        public int[] I4 { get; private set; }

        public void Read(MpqFileStream stream)
        {
            this.I0 = stream.ReadValueS32();
            this.SNOSubTreasureClass = stream.ReadValueS32();
            this.Percentage = stream.ReadValueF32();
            this.I1 = stream.ReadValueS32();
            this.GBIdQualityClass = stream.ReadValueS32();
            this.I2 = stream.ReadValueS32();
            this.I3 = stream.ReadValueS32();
            this.SNOCondition = stream.ReadValueS32();
            this.ItemSpecifier = new ItemSpecifierData(stream);
            this.I5 = stream.ReadValueS32();
            this.I4 = new int[4];
            for (int i = 0; i < 4; i++)
                this.I4[i] = stream.ReadValueS32();
        }
    }
}
