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
using System.Diagnostics;
using CrystalMpq;
using Mooege.Common.Extensions;
using System.Text;

namespace Mooege.Common.MPQ.FileFormats
{
    //Untested yet. - DarkLotus
    [FileFormat(SNOGroup.Power)]
    public class Power : FileFormat
    {
        public Header Header;
        string chararray1;

        char[] c0; //64
        PowerDef Powerdef;
        int i0, i1;
        char[] c1;//256
        string chararray2;
        int i2;
        ScriptFormulaDetails ScriptFormulaDetails;
        int i3;
        List<byte> CompliedScript = new List<byte>();
        int snoQuestMetaData;


        public Power(MpqFile file)
        {
            MpqFileStream stream = null;
            try
            {
                stream = file.Open();
            }
            catch
            {
                return;
            }
            
            this.Header = new Header(stream);
            byte[] buf = new byte[64];
            stream.Read(buf, 0, 64); chararray1 = Encoding.ASCII.GetString(buf);
            stream.Position += 4; // pad 1
            Powerdef = new PowerDef(stream);
            i0 = stream.ReadInt32();
            i1 = stream.ReadInt32();
            buf = new byte[256];
            stream.Read(buf, 0, 256); chararray2 = Encoding.ASCII.GetString(buf);
            i2 = stream.ReadInt32();
            ScriptFormulaDetails = stream.ReadSerializedData<ScriptFormulaDetails>();
            
            i3 = stream.ReadInt32();
            
            var serCompliedScript = stream.GetSerializedDataPointer();
            snoQuestMetaData = stream.ReadInt32();

            stream.Close();
        }
    }
    public class PowerDef
    {
        TagMap hTagMap;
        TagMap hGeneralTagMap;
        TagMap PVPGeneralTagMap;
        TagMap ContactTagMap0;
        TagMap ContactTagMap1;
        TagMap ContactTagMap2;
        TagMap ContactTagMap3;
        TagMap PVPContactTagMap0;
        TagMap PVPContactTagMap1;
        TagMap PVPContactTagMap2;
        TagMap PVPContactTagMap3;
        int i0;
        ActorCollisionFlags ActColFlags1;
        ActorCollisionFlags ActColFlags2;
        List<BuffDef> Buffs = new List<BuffDef>(); //4
        public PowerDef(MpqFileStream stream)
        {
            hTagMap = stream.ReadSerializedData<TagMap>();
            stream.Position += (2 * 4);
            hGeneralTagMap = stream.ReadSerializedData<TagMap>();
            stream.Position += (2 * 4);
            PVPGeneralTagMap = stream.ReadSerializedData<TagMap>();
            stream.Position += (2 * 4);
            ContactTagMap0 = stream.ReadSerializedData<TagMap>();
            ContactTagMap1 = stream.ReadSerializedData<TagMap>();
            ContactTagMap2 = stream.ReadSerializedData<TagMap>();
            ContactTagMap3 = stream.ReadSerializedData<TagMap>();
            stream.Position += (8 * 4);
            PVPContactTagMap0 = stream.ReadSerializedData<TagMap>();
            PVPContactTagMap1 = stream.ReadSerializedData<TagMap>();
            PVPContactTagMap2 = stream.ReadSerializedData<TagMap>();
            PVPContactTagMap3 = stream.ReadSerializedData<TagMap>();
            stream.Position += (8 * 4);
            i0 = stream.ReadInt32();
            ActColFlags1 = new ActorCollisionFlags(stream);
            ActColFlags2 = new ActorCollisionFlags(stream);
            stream.Position += 4;
            for (int i = 0; i < 4; i++)
            {
                Buffs.Add(new BuffDef(stream));
            }
            // pad +80?
        }

    }
    public class BuffDef
    {
        List<int> BuffFilterPowers = new List<int>();
        public BuffDef(MpqFileStream stream)
        {
            BuffFilterPowers = stream.ReadSerializedInts();
        }
    }
    
    
}