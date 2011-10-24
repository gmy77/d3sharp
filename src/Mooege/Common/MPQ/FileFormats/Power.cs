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
        // PowerDef Powerdef;
        int i0, i1;
        char[] c1;//256
        string chararray2;
        int i2;
        ScriptFormulaDetails ScriptFormulaDetails;
        int i3;
        //<Field Name="serCompiledScript" Type="SerializeData" Offset="728" Flags="0" />
        //<Field Type="DT_VARIABLEARRAY" Offset="720" Flags="32" SubType="DT_BYTE" />
        int snoQuestMetaData;

        public Power(MpqFile file)
        {
            var stream = file.Open();
            this.Header = new Header(stream);
            byte[] buf = new byte[64];
            stream.Read(buf, 0, 64); chararray1 = Encoding.ASCII.GetString(buf);
            stream.Position += 4; // pad 1
            //Powerdef = new PowerDef(stream);
            i0 = stream.ReadInt32();
            i1 = stream.ReadInt32();
            buf = new byte[256];
            stream.Read(buf, 0, 256); chararray2 = Encoding.ASCII.GetString(buf);
            i2 = stream.ReadInt32();
            ScriptFormulaDetails = stream.ReadSerializedData<ScriptFormulaDetails>();


            snoQuestMetaData = stream.ReadInt32();
            stream.Close();
        }
    }

    //public class PowerDef
    //{
    //    // commented out because Power\trOut_LogStack_ShortDamage.pow -  ContactTagMap = stream.ReadSerializedData<TagMap>(3) line - the 3.rd tagmap seem to read a tagmapsize of 85722374.
    //    // even commenting that line out throws more out of memory exceptions - i guess we have more spots reading a errornous big count.
    //    TagMap hTagMap;
    //    TagMap hGeneralTagMap;
    //    TagMap PVPGeneralTagMap;
    //    List<TagMap> ContactTagMap = new List<TagMap>(); // 3
    //    List<TagMap> PVPContactTagMap = new List<TagMap>(); // 3
    //    int i0;
    //    ActorCollisionFlags ActColFlags1;
    //    ActorCollisionFlags ActColFlags2;
    //    List<BuffDef> Buffs = new List<BuffDef>(); //4
    //    public PowerDef(MpqFileStream stream)
    //    {
    //        hTagMap = stream.ReadSerializedData<TagMap>();
    //        hGeneralTagMap = stream.ReadSerializedData<TagMap>();
    //        PVPGeneralTagMap = stream.ReadSerializedData<TagMap>();
    //        ContactTagMap = stream.ReadSerializedData<TagMap>(3);
    //        PVPContactTagMap = stream.ReadSerializedData<TagMap>(3); 
    //        i0 = stream.ReadInt32();
    //        ActColFlags1 = new ActorCollisionFlags(stream);
    //        ActColFlags2 = new ActorCollisionFlags(stream);
            
    //        for (int i = 0; i < 4; i++)
    //        {
    //            Buffs.Add(new BuffDef(stream));
    //        }
    //        // pad +80?
    //    }
    //}

    public class BuffDef
    {
        List<int> BuffFilterPowers = new List<int>();
        public BuffDef(MpqFileStream stream)
        {
            BuffFilterPowers = stream.ReadSerializedInts();
        }
    }
    public class ActorCollisionFlags
    {
        int i0, i1, i2, i3;
        public ActorCollisionFlags(MpqFileStream stream)
        {
            this.i0 = stream.ReadInt32();
            this.i1 = stream.ReadInt32();
            this.i2 = stream.ReadInt32();
            this.i3 = stream.ReadInt32();
        }
    }
    public class ScriptFormulaDetails : ISerializableData
    {
        // Maybe should be strings? - Darklotus
        char[] c0; //256
        char[] c1; //512
        int i0, i1;
        public void Read(MpqFileStream stream)
        {
            c0 = new char[256];
            c1 = new char[512];
            byte[] buf = new byte[512];
            stream.Read(buf, 0, 256);
            for (int i = 0; i < c0.Length; i++)
            {
                c0[i] = (char)buf[i];
            }
            stream.Read(buf, 0, 512);
            for (int i = 0; i < c1.Length; i++)
            {
                c1[i] = (char)buf[i];
            }
            i0 = stream.ReadInt32();
            i1 = stream.ReadInt32();
        }
    }
}