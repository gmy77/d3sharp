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
using System.Collections.Generic;
using Mooege.Core.GS.Common.Types.SNO;

namespace Mooege.Common.MPQ.FileFormats
{
    [FileFormat(SNOGroup.EffectGroup)]
    public class EffectGroup : FileFormat
    {
        public Header Header { get; private set; }
        public int i0, i1, i2, i3, i4;
        public int SnoPower { get; private set; }
        List<EffectItem> EffectItems = new List<EffectItem>();
        public EffectGroup(MpqFile file)
        {
            var stream = file.Open();
            this.Header = new Header(stream);
            this.i0 = stream.ReadValueS32();
            this.EffectItems = stream.ReadSerializedData<EffectItem>();
            this.i1 = stream.ReadValueS32();
            stream.Position += 4; // pad 1
            this.i2 = stream.ReadValueS32();
            this.i3 = stream.ReadValueS32();
            this.i4 = stream.ReadValueS32();
            this.SnoPower = stream.ReadValueS32();
            stream.Close();
        }
    }
    public class EffectItem : ISerializableData
    {
        public int i0 { get; private set; }
        public string Name { get; private set; } // 64
        MsgTriggeredEvent TriggeredEvent = new MsgTriggeredEvent();
        public void Read(MpqFileStream stream)
        {
            this.i0 = stream.ReadValueS32();
            // Maybe this should stay a Char Array instead of a string. - DarkLotus
            this.Name = stream.ReadString(64, true);
            this.TriggeredEvent.Read(stream);
        }
    }
   
}