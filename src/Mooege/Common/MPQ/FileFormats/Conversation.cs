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
using System.Text;

namespace Mooege.Common.MPQ.FileFormats
{
    [FileFormat(SNOGroup.Conversation)]
    class Conversation : FileFormat
    {
        public Header Header { get; private set; }
        public ConversationTypes ConversationType { get; private set; }
        public int I0 { get; private set; }            // looks like the conversation icon, its 1 for important quest conversations, 0 otherwise
        public int I1 { get; private set; }
        public int SNOQuest { get; private set; }
        public int I2 { get; private set; }
        public int I3 { get; private set; }
        public int SNOConvPiggyback { get; private set; }
        public int SNOConvUnlock { get; private set; }
        public int I4 { get; private set; }
        public string Unknown { get; private set; }
        public int SNOPrimaryNpc { get; private set; }
        public int SNOAltNpc1 { get; private set; }
        public int SNOAltNpc2 { get; private set; }
        public int SNOAltNpc3 { get; private set; }
        public int SNOAltNpc4 { get; private set; }
        public int I5 { get; private set; }              // not total nodes :-(
        public List<ConversationTreeNode> RootTreeNodes { get; private set; }
        public string Unknown2 { get; private set; }
        public int I6 { get; private set; }
        public string CompiledScript { get; private set; }
        public int SNOBossEncounter { get; private set; }

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
            this.Unknown = stream.ReadString(128, true);
            this.SNOPrimaryNpc = stream.ReadValueS32();
            this.SNOAltNpc1 = stream.ReadValueS32();
            this.SNOAltNpc2 = stream.ReadValueS32();
            this.SNOAltNpc3 = stream.ReadValueS32();
            this.SNOAltNpc4 = stream.ReadValueS32();
            this.I5 = stream.ReadValueS32();

            stream.Position += (2 * 4);
            RootTreeNodes = stream.ReadSerializedData<ConversationTreeNode>();

            this.Unknown2 = stream.ReadString(256, true);
            this.I6 = stream.ReadValueS32();

            stream.Position += 12;
            CompiledScript = Encoding.ASCII.GetString(stream.ReadSerializedByteArray());
            stream.Position += 40;
            this.SNOBossEncounter = stream.ReadValueS32();
            stream.Close();
        }

        public string AsText(string filename)
        {
            StringBuilder s = new StringBuilder();

            s.AppendLine(Header.SNOId + ":" + filename);
            s.AppendLine("ConversationType:" + ConversationType);
            s.Append("I0:" + I0 + "   ");
            s.Append("I1:" + I1 + "   ");
            s.Append("I2:" + I2 + "   ");
            s.Append("I3:" + I3 + "   ");
            s.Append("I4:" + I4 + "   ");
            s.Append("I5:" + I5 + "   ");
            s.AppendLine("I6:" + I6);

            s.AppendLine("SNOQuest:" + SNOQuest);
            s.AppendLine("SNOConvPiggyBack:" + SNOConvPiggyback);
            s.AppendLine("SNOConvUnlock:" + SNOConvUnlock);
            s.AppendLine("CompiledScript:" + (CompiledScript.Length != 0).ToString());

            foreach (var node in RootTreeNodes)
                node.AsText(s, 0);
            return s.ToString();
        }
    }


    public class ConversationTreeNode : ISerializableData
    {
        public int I0 { get; private set; }
        public int I1 { get; private set; }
        public int LineID { get; private set; }              // clasid ? 
        public Speaker Speaker1 { get; private set; }
        public Speaker Speaker2 { get; private set; }
        public int AnimationTag { get; private set; }
        public int I4 { get; private set; }
        public int ClassFilter { get; private set; }        // only used on nodes with i0 == 5, selects the displaylocalconvline
        public ConvLocalDisplayTimes[] ConvLocalDisplayTimes = new ConvLocalDisplayTimes[18];
        public string Comment { get; private set; }
        public int I6 { get; private set; }
        public List<ConversationTreeNode> TrueNodes { get; private set; }
        public List<ConversationTreeNode> FalseNodes { get; private set; }
        public List<ConversationTreeNode> ChildNodes { get; private set; }

        public void Read(MpqFileStream stream)
        {
            I0 = stream.ReadValueS32();
            I1 = stream.ReadValueS32();
            LineID = stream.ReadValueS32();
            Speaker1 = (Speaker)stream.ReadValueS32();
            Speaker2 = (Speaker)stream.ReadValueS32();
            AnimationTag = stream.ReadValueS32();
            I4 = stream.ReadValueS32();
            ClassFilter = stream.ReadValueS32();

            for (int i = 0; i < ConvLocalDisplayTimes.Length; i++)
                ConvLocalDisplayTimes[i] = new ConvLocalDisplayTimes(stream);

            stream.Position += (2 * 4);
            Comment = stream.ReadSerializedString();
            this.I6 = stream.ReadValueS32();

            stream.Position += 4;       // these are unaccounted for...xml offsets just skips ahead

            stream.Position += (2 * 4);
            TrueNodes = stream.ReadSerializedData<ConversationTreeNode>();

            stream.Position += (2 * 4);
            FalseNodes = stream.ReadSerializedData<ConversationTreeNode>();

            stream.Position += (2 * 4);
            ChildNodes = stream.ReadSerializedData<ConversationTreeNode>();
        }

        public void AsText(StringBuilder s, int pad)
        {
            s.Append(' ', pad);
            s.Append("I0:" + I0 + "   ");
            s.Append("I1:" + I1 + "   ");
            s.Append("LineID:" + LineID + "   ");
            s.Append("AnimationTag:" + AnimationTag + "   ");
            s.Append("I4:" + I4 + "   ");
            s.Append("ClassFilter:" + ClassFilter + "   ");
            s.AppendLine("I6:" + I6);
            s.Append(' ', pad); s.AppendLine("Speaker1:" + Speaker1);
            s.Append(' ', pad); s.AppendLine("Speaker2:" + Speaker2);
            s.Append(' ', pad); s.AppendLine("Comment:" + Comment);

            s.Append(' ', pad); s.AppendLine("ConvLocalDisplayTimes: not shown");
            //for (int i = 0; i < ConvLocalDisplayTimes.Length; i++)
            //    ConvLocalDisplayTimes[i].AsText(s, pad);

            if (TrueNodes.Count > 0)
            {
                s.Append(' ', pad); s.AppendLine("TrueNodes:");
                s.Append(' ', pad); s.AppendLine("{");
                foreach (var node in TrueNodes)
                    node.AsText(s, pad + 3);
                s.Append(' ', pad); s.AppendLine("}");
            }
            if (FalseNodes.Count > 0)
            {
                s.Append(' ', pad); s.AppendLine("FalseNodes:");
                s.Append(' ', pad); s.AppendLine("{");
                foreach (var node in FalseNodes)
                    node.AsText(s, pad + 3);
                s.Append(' ', pad); s.AppendLine("}");
            }
            if (ChildNodes.Count > 0)
            {
                s.Append(' ', pad); s.AppendLine("ChildNodes:");
                s.Append(' ', pad); s.AppendLine("{");
                foreach (var node in ChildNodes)
                    node.AsText(s, pad + 3);
                s.Append(' ', pad); s.AppendLine("}");
            }
        }

    }

    public class ConvLocalDisplayTimes
    {
        public int[] Languages = new int[10];

        public ConvLocalDisplayTimes(CrystalMpq.MpqFileStream stream)
        {
            for (int i = 0; i < Languages.Length; i++)
                Languages[i] = stream.ReadValueS32();
        }

        public void AsText(StringBuilder s, int pad)
        {
            s.Append(' ', pad);
            for (int i = 0; i < Languages.Length; i++)
                s.Append(Languages[i] + "  ");
            s.AppendLine();
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
