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

using System.Collections.Generic;
using CrystalMpq;
using Gibbed.IO;
using Mooege.Common.MPQ.FileFormats.Types;
using Mooege.Core.GS.Common.Types.SNO;

namespace Mooege.Common.MPQ.FileFormats
{
    [FileFormat(SNOGroup.SkillKit)]
    public class SkillKit : FileFormat
    {
        public Header Header { get; private set; }
        public List<TraitEntry> TraitEntries { get; private set; }
        public List<ActiveSkillEntry> ActiveSkillEntries { get; private set; }

        public SkillKit(MpqFile file)
        {
            var stream = file.Open();
            this.Header = new Header(stream);
            stream.Position += 12;
            this.TraitEntries = stream.ReadSerializedData<TraitEntry>();
            stream.Position += 8;
            this.ActiveSkillEntries = stream.ReadSerializedData<ActiveSkillEntry>();
            stream.Close();
        }
    }

    public class TraitEntry : ISerializableData
    {
        public int SNOPower { get; private set; }
        public int Category { get; private set; }
        public int ReqLevel { get; private set; }

        public void Read(MpqFileStream stream)
        {
            this.SNOPower = stream.ReadValueS32();
            this.Category = stream.ReadValueS32();
            this.ReqLevel = stream.ReadValueS32();
        }
    }

    public class ActiveSkillEntry : ISerializableData
    {
        public int SNOPower { get; private set; }
        public ActiveSkillCategory Category { get; private set; }
        public int SkillGroup { get; private set; }  // TODO: possible to make an enum for this, like Category has?
        public int ReqLevel { get; private set; }
        public int RuneNone_ReqLevel { get; private set; }
        public int RuneA_ReqLevel { get; private set; }
        public int RuneB_ReqLevel { get; private set; }
        public int RuneC_ReqLevel { get; private set; }
        public int RuneD_ReqLevel { get; private set; }
        public int RuneE_ReqLevel { get; private set; }

        public void Read(MpqFileStream stream)
        {
            this.SNOPower = stream.ReadValueS32();
            this.Category = (ActiveSkillCategory)stream.ReadValueS32();
            this.SkillGroup = stream.ReadValueS32();
            this.ReqLevel = stream.ReadValueS32();
            this.RuneNone_ReqLevel = stream.ReadValueS32();
            this.RuneA_ReqLevel = stream.ReadValueS32();
            this.RuneB_ReqLevel = stream.ReadValueS32();
            this.RuneC_ReqLevel = stream.ReadValueS32();
            this.RuneD_ReqLevel = stream.ReadValueS32();
            this.RuneE_ReqLevel = stream.ReadValueS32();
        }
    }

    public enum ActiveSkillCategory
    {
        FuryGenerator = 0,
        FurySpender,
        Situational,
        Signature,
        Offensive,
        Utility,
        PhysicalRealm,
        SpiritRealm,
        Support,
        HatredGenerator,
        HatredSpender,
        Discipline,
        SpiritGenerator,
        SpiritSpender,
        Mantras,
    }
}
