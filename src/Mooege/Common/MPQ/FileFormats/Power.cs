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
using Mooege.Core.GS.Common.Types.TagMap;

namespace Mooege.Common.MPQ.FileFormats
{
    [FileFormat(SNOGroup.Power)]
    public class Power : FileFormat
    {
        public Header Header { get; private set; }
        public string LuaName { get; private set; }
        public PowerDef Powerdef { get; private set; }
        public int I0 { get; private set; }
        public int I1 { get; private set; }
        public string Chararray2 { get; private set; }
        public int ScriptFormulaCount { get; private set; }
        public List<ScriptFormulaDetails> ScriptFormulaDetails = new List<ScriptFormulaDetails>();
        public int i3 { get; private set; }
        public string CompiledScript { get; private set; }
        public int SNOQuestMetaData { get; private set; }

        public Power(MpqFile file)
        {
            var stream = file.Open();
            this.Header = new Header(stream);
            LuaName = stream.ReadString(64, true);
            stream.Position += 4; // pad 1
            Powerdef = new PowerDef(stream);
            stream.Position = 440; // Seems like theres a bit of a gap - DarkLotus
            I0 = stream.ReadValueS32();
            I1 = stream.ReadValueS32();
            Chararray2 = stream.ReadString(256, true);
            ScriptFormulaCount = stream.ReadValueS32();
            ScriptFormulaDetails = stream.ReadSerializedData<ScriptFormulaDetails>();
            stream.Position += (3 * 4);
            i3 = stream.ReadValueS32();
            stream.Position += (3 * 4);

            CompiledScript = System.Text.Encoding.ASCII.GetString(stream.ReadSerializedByteArray());
            SNOQuestMetaData = stream.ReadValueS32();
            stream.Close();
        }
    }

    public class PowerDef
    {
        public TagMap TagMap { get; private set; }
        public TagMap GeneralTagMap { get; private set; }
        public TagMap PVPGeneralTagMap { get; private set; }
        public TagMap ContactTagMap0 { get; private set; }
        public TagMap ContactTagMap1 { get; private set; }
        public TagMap ContactTagMap2 { get; private set; }
        public TagMap ContactTagMap3 { get; private set; }
        public TagMap PVPContactTagMap0 { get; private set; }
        public TagMap PVPContactTagMap1 { get; private set; }
        public TagMap PVPContactTagMap2 { get; private set; }
        public TagMap PVPContactTagMap3 { get; private set; }
        public int I0 { get; private set; }
        public ActorCollisionFlags ActColFlags1 { get; private set; }
        public ActorCollisionFlags ActColFlags2 { get; private set; }
        public List<BuffDef> Buffs = new List<BuffDef>(); //4

        public PowerDef(MpqFileStream stream)
        {
            TagMap = stream.ReadSerializedItem<TagMap>();
            stream.Position += (2 * 4);
            GeneralTagMap = stream.ReadSerializedItem<TagMap>();
            stream.Position += (2 * 4);
            PVPGeneralTagMap = stream.ReadSerializedItem<TagMap>();
            stream.Position += (2 * 4);
            ContactTagMap0 = stream.ReadSerializedItem<TagMap>();
            ContactTagMap1 = stream.ReadSerializedItem<TagMap>();
            ContactTagMap2 = stream.ReadSerializedItem<TagMap>();
            ContactTagMap3 = stream.ReadSerializedItem<TagMap>();
            stream.Position += (8 * 4);
            PVPContactTagMap0 = stream.ReadSerializedItem<TagMap>();
            PVPContactTagMap1 = stream.ReadSerializedItem<TagMap>();
            PVPContactTagMap2 = stream.ReadSerializedItem<TagMap>();
            PVPContactTagMap3 = stream.ReadSerializedItem<TagMap>();
            stream.Position += (8 * 4);
            I0 = stream.ReadValueS32();
            ActColFlags1 = new ActorCollisionFlags(stream);
            ActColFlags2 = new ActorCollisionFlags(stream);
            stream.Position += 4;
            for (int i = 0; i < 4; i++)
            {
                Buffs.Add(new BuffDef(stream));
                stream.Position += (2 * 4);
            }
        }

    }
    public class ActorCollisionFlags
    {
        public int I0 { get; private set; }
        public int I1 { get; private set; }
        public int I2 { get; private set; }
        public int I3 { get; private set; }

        public ActorCollisionFlags(MpqFileStream stream)
        {
            this.I0 = stream.ReadValueS32();
            this.I1 = stream.ReadValueS32();
            this.I2 = stream.ReadValueS32();
            this.I3 = stream.ReadValueS32();
        }
    }

    public class BuffDef
    {
        public List<int> BuffFilterPowers = new List<int>();

        public BuffDef(MpqFileStream stream)
        {
            BuffFilterPowers = stream.ReadSerializedInts();
        }
    }
}