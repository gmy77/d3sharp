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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CrystalMpq;
using Mooege.Common.MPQ.FileFormats.Types;
using Gibbed.IO;

namespace Mooege.Common.MPQ.FileFormats
{
    // <Descriptor Name="Conversation">
    //  <Field Type="DT_ENUM" Offset="12" Min="0" Max="13" Flags="17" EncodedBits="4"></Field>
    //  <Field Type="DT_INT" Offset="16" Flags="1" EncodedBits="32" />
    //  <Field Type="DT_INT" Offset="20" Flags="1" EncodedBits="32" />
    //  <Field Name="snoQuest" Type="DT_SNO" Offset="24" Flags="513" EncodedBits="32" SnoType="31" />
    //  <Field Type="DT_INT" Offset="28" Flags="1" EncodedBits="32" />
    //  <Field Type="DT_INT" Offset="32" Flags="1" EncodedBits="32" />
    //  <Field Name="snoConvPiggyback" Type="DT_SNO" Offset="36" Flags="1" EncodedBits="32" SnoType="12" />
    //  <Field Name="snoConvUnlock" Type="DT_SNO" Offset="40" Flags="1" EncodedBits="32" SnoType="12" />
    //  <Field Type="DT_INT" Offset="44" Flags="1" EncodedBits="32" />
    //  <Field Type="DT_CHARARRAY" Offset="48" Flags="1" ArrayLength="128" EncodedBits="8" />
    //  <Field Name="snoPrimaryNpc" Type="DT_SNO" Offset="176" Flags="1" EncodedBits="32" SnoType="1" />
    //  <Field Name="snoAltNpc1" Type="DT_SNO" Offset="180" Flags="1" EncodedBits="32" SnoType="1" />
    //  <Field Name="snoAltNpc2" Type="DT_SNO" Offset="184" Flags="1" EncodedBits="32" SnoType="1" />
    //  <Field Name="snoAltNpc3" Type="DT_SNO" Offset="188" Flags="1" EncodedBits="32" SnoType="1" />
    //  <Field Name="snoAltNpc4" Type="DT_SNO" Offset="192" Flags="1" EncodedBits="32" SnoType="1" />
    //  <Field Type="DT_INT" Offset="196" Flags="1" EncodedBits="32" />
    //  <Field Name="serRootTreeNodes" Type="SerializeData" Offset="208" Flags="0" />
    //  <Field Type="DT_VARIABLEARRAY" Offset="200" Flags="33" SubType="ConversationTreeNode" />
    //  <Field Type="DT_CHARARRAY" Offset="216" Flags="1" ArrayLength="256" EncodedBits="9" />
    //  <Field Type="DT_INT" Offset="472" Flags="1" EncodedBits="32" />
    //  <Field Name="serCompiledScript" Type="SerializeData" Offset="488" Flags="0" />
    //  <Field Type="DT_VARIABLEARRAY" Offset="480" Flags="32" SubType="DT_BYTE" />
    //  <Field Name="snoBossEncounter" Type="DT_SNO" Offset="536" Flags="1" EncodedBits="32" SnoType="64" />
    //  <Field Type="DT_NULL" Offset="544" Flags="0" />
    // </Descriptor>
    [FileFormat(SNOGroup.Conversation)]
    class Conversation : FileFormat
    {
        public Header Header;
        public ConversationTypes ConversationType;
        public int I0;
        public int I1;
        public int SNOQuest;
        public int I2;
        public int I3;
        public int SNOConvPiggyback;
        public int SNOConvUnlock;
        public int I4;
        public string Unknown;
        public int SNOPrimaryNpc;
        public int SNOAltNpc1;
        public int SNOAltNpc2;
        public int SNOAltNpc3;
        public int SNOAltNpc4;
        public int I5;              // not total nodes :-(
        public List<ConversationTreeNode> RootTreeNodes;
        public string Unknown2;
        public int I6;
        public byte[] CompiledScript;
        public int SNOBossEncounter;

        public Conversation(MpqFile file)
        {
            MpqFileStream stream = file.Open();

            this.Header = new Header(stream);
            this.ConversationType = (ConversationTypes)stream.ReadValueS32();
            this.I0 = stream.ReadValueS32();
            this.I1 = stream.ReadValueS32();
            this.SNOQuest = stream.ReadValueS32();
            this.I2 = stream.ReadValueS32();
            this.I3 = stream.ReadValueS32();
            this.SNOConvPiggyback = stream.ReadValueS32();
            this.SNOConvUnlock = stream.ReadValueS32();
            this.I4 = stream.ReadValueS32();
            this.Unknown = stream.ReadString(128);
            this.SNOPrimaryNpc = stream.ReadValueS32();
            this.SNOAltNpc1 = stream.ReadValueS32();
            this.SNOAltNpc2 = stream.ReadValueS32();
            this.SNOAltNpc3 = stream.ReadValueS32();
            this.SNOAltNpc4 = stream.ReadValueS32();
            this.I5 = stream.ReadValueS32();

            stream.Position += (2 * 4);
            RootTreeNodes = stream.ReadSerializedData<ConversationTreeNode>();

            this.Unknown2 = stream.ReadString(256);
            this.I6 = stream.ReadValueS32();

            stream.Position += (2 * 4);
            SerializableDataPointer compiledScriptPointer = stream.GetSerializedDataPointer();

            stream.Position += 44; // these bytes are unaccounted for in the xml
            this.SNOBossEncounter = stream.ReadValueS32();

            // reading compiled script, placed it here so i dont have to move the offset around
            CompiledScript = new byte[compiledScriptPointer.Size];
            stream.Position = compiledScriptPointer.Offset + 16;
            stream.Read(CompiledScript, 0, compiledScriptPointer.Size);

            stream.Close();
        }
    }

    // <Descriptor Name="ConversationTreeNode">
    //  <Field Type="DT_INT" Offset="0" Flags="1" EncodedBits="32" />
    //  <Field Type="DT_INT" Offset="4" Flags="1" EncodedBits="32" />
    //  <Field Type="DT_INT" Offset="8" Flags="1" EncodedBits="32" />
    //  <Field Type="DT_ENUM" Offset="12" Min="-1" Max="8" Flags="17" EncodedBits="4"></Field>
    //  <Field Type="DT_ENUM" Offset="16" Min="-1" Max="8" Flags="17" EncodedBits="4"></Field>
    //  <Field Type="DT_INT" Offset="20" Flags="1" EncodedBits="32" />
    //  <Field Type="DT_INT" Offset="24" Flags="1" EncodedBits="32" />
    //  <Field Type="DT_INT" Offset="28" Flags="1" EncodedBits="32" />
    //  <Field Type="DT_FIXEDARRAY" Offset="32" Flags="1" SubType="ConvLocaleDisplayTimes" ArrayLength="18" />
    //  <Field Name="serComment" Type="SerializeData" Offset="760" Flags="0" />
    //  <Field Type="DT_CSTRING" Offset="752" Flags="33" SubType="DT_BYTE" />
    //  <Field Type="DT_INT" Offset="768" Flags="1" EncodedBits="32" />
    //  <Field Name="serTrueNodes" Type="SerializeData" Offset="784" Flags="0" />
    //  <Field Type="DT_VARIABLEARRAY" Offset="776" Flags="33" SubType="ConversationTreeNode" />
    //  <Field Name="serFalseNodes" Type="SerializeData" Offset="800" Flags="0" />
    //  <Field Type="DT_VARIABLEARRAY" Offset="792" Flags="33" SubType="ConversationTreeNode" />
    //  <Field Name="serChildNodes" Type="SerializeData" Offset="816" Flags="0" />
    //  <Field Type="DT_VARIABLEARRAY" Offset="808" Flags="33" SubType="ConversationTreeNode" />
    //  <Field Type="DT_NULL" Offset="824" Flags="0" />
    // </Descriptor>
    public class ConversationTreeNode : ISerializableData
    {
        public int I0;
        public int I1;
        public int I2;              // classid ? 
        public Speaker Speaker1;
        public Speaker Speaker2;
        public int I3;
        public int I4;
        public int I5;
        public ConvLocalDisplayTimes[] ConvLocalDisplayTimes = new ConvLocalDisplayTimes[18];
        public string Comment;
        public int I6;
        public List<ConversationTreeNode> TrueNodes;
        public List<ConversationTreeNode> FalseNodes;
        public List<ConversationTreeNode> ChildNodes;

        public void Read(MpqFileStream stream)
        {
            I0 = stream.ReadValueS32();
            I1 = stream.ReadValueS32();
            I2 = stream.ReadValueS32();
            Speaker1 = (Speaker)stream.ReadValueS32();
            Speaker2 = (Speaker)stream.ReadValueS32();
            I3 = stream.ReadValueS32();
            I4 = stream.ReadValueS32();
            I5 = stream.ReadValueS32();

            for (int i = 0; i < ConvLocalDisplayTimes.Length; i++)
                ConvLocalDisplayTimes[i] = new ConvLocalDisplayTimes(stream);

            stream.Position += (2 * 4);

            long oldPos = stream.Position;
            SerializableDataPointer commentPointer = stream.GetSerializedDataPointer();
            byte[] comment = new byte[commentPointer.Size];
            stream.Position = commentPointer.Offset + 16;
            stream.Read(comment, 0, commentPointer.Size);
            Comment = System.Text.Encoding.ASCII.GetString(comment);
            stream.Position = oldPos + 8; // dont read pointer again

            this.I6 = stream.ReadValueS32();

            stream.Position += 4;       // these are unaccounted for...xml offsets just skips ahead

            stream.Position += (2 * 4);
            TrueNodes = stream.ReadSerializedData<ConversationTreeNode>();

            stream.Position += (2 * 4);
            FalseNodes = stream.ReadSerializedData<ConversationTreeNode>();

            stream.Position += (2 * 4);
            ChildNodes = stream.ReadSerializedData<ConversationTreeNode>();
        }
    }

    // <Descriptor Name="ConvLocaleDisplayTimes">
    //  <Field Type="DT_FIXEDARRAY" Offset="0" Flags="1" SubType="DT_INT" ArrayLength="10" EncodedBits="32" />
    //  <Field Type="DT_NULL" Offset="40" Flags="0" />
    // </Descriptor>
    public class ConvLocalDisplayTimes
    {
        public int[] I0 = new int[10];

        public ConvLocalDisplayTimes(CrystalMpq.MpqFileStream stream)
        {
            for (int i = 0; i < I0.Length; i++)
                I0[i] = stream.ReadValueS32();
        }
    }


    public enum ConversationTypes
    {
        FollowerSoundset = 0,
        PlayerEmote = 1,
        AmbientFloat = 2,
        FollowerBanter = 3,
        FollowerCallout = 4,
        PlayerCallout = 5,
        GlobalChatter = 6,
        GlobalFloat = 7,
        LoreBook = 8,
        AmbientGossip = 9,
        TalkMenuGossip = 10,
        QuestStandard = 11,
        QuestFloat = 12,
        QuestEvent = 13
    }


    public enum Speaker
    {
        None = -1,
        Player = 0,
        PrimaryNPC = 1,
        AltNPC1 = 2,
        AltNPC2 = 3,
        AltNPC3 = 4,
        AltNPC4 = 5,
        TemplarFollower = 6,
        ScoundrelFollower = 7,
        EnchantressFollower = 8
    }

}
