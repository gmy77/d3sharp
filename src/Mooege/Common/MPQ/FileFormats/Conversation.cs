using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mooege.Common.Extensions;
using Mooege.Common.MPQ.FileFormats;
using CrystalMpq;

namespace Mooege.Common.MPQ.FileFormats
{
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
        public int I5;
        public List<ConversationTreeNode> RootTreeNodes;        // made it a list because it says nodeS
        public string Unknown2;
        public int I6;
        public int SNOBossEncounter;

        public Conversation(MpqFile file)
        {
            MpqFileStream stream = file.Open();

            this.Header = new Header(stream);
            this.ConversationType = (ConversationTypes)stream.ReadInt32();
            this.I0 = stream.ReadInt32();
            this.I1 = stream.ReadInt32();
            this.SNOQuest = stream.ReadInt32();
            this.I2 = stream.ReadInt32();
            this.I3 = stream.ReadInt32();
            this.SNOConvPiggyback = stream.ReadInt32();
            this.SNOConvUnlock = stream.ReadInt32();
            this.I4 = stream.ReadInt32();
            this.Unknown = stream.ReadString(128);
            this.SNOPrimaryNpc = stream.ReadInt32();
            this.SNOAltNpc1 = stream.ReadInt32();
            this.SNOAltNpc2 = stream.ReadInt32();
            this.SNOAltNpc3 = stream.ReadInt32();
            this.SNOAltNpc4 = stream.ReadInt32();
            this.I5 = stream.ReadInt32();

            stream.Position += (2 * 4);
            RootTreeNodes = stream.ReadAllSerializedData<ConversationTreeNode>();

            this.Unknown2 = stream.ReadString(256);
            this.I6 = stream.ReadInt32();

            stream.Position += (2 * 4);
            stream.Position += (2 * 4); // TODO: read serCompiledScripts instead!!

            stream.Position += 44; // these bytes are unaccounted for in the xml
            this.SNOBossEncounter = stream.ReadInt32();
            stream.Close();
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



public class ConversationTreeNode : ISerializableData
{
    public int I0;
    public int I1;
    public int I2;
    public Speaker Speaker1; //enum ??
    public Speaker Speaker2; //enum ??
    public int I3;
    public int I4;
    public int I5;
    public ConvLocalDisplayTimes[] ConvLocalDisplayTimes = new ConvLocalDisplayTimes[18];  // 18
    //byte pad0[8]; // mentioned as string in xml
    //SerializeData serComment;
    public int I6;
    public List<ConversationTreeNode> TrueNodes;
    public List<ConversationTreeNode> FalseNodes;
    public List<ConversationTreeNode> ChildNodes;

    public void Read(MpqFileStream stream)
    {
        I0 = stream.ReadInt32();
        I1 = stream.ReadInt32();
        I2 = stream.ReadInt32();
        Speaker1 = (Speaker)stream.ReadInt32();
        Speaker2 = (Speaker)stream.ReadInt32();
        I3 = stream.ReadInt32();
        I4 = stream.ReadInt32();
        I5 = stream.ReadInt32();

        for (int i = 0; i < ConvLocalDisplayTimes.Length; i++)
            ConvLocalDisplayTimes[i] = new ConvLocalDisplayTimes(stream);

        stream.Position += (2 * 4);
        stream.Position += (2 * 4); // TODO read comment from stream. this is SerializeData "Comment", points to a byte[]
        this.I6 = stream.ReadInt32();
        stream.Position += 4;       // these are unaccounted for...xml offsets just skips ahead

        stream.Position += (2 * 4);
        TrueNodes = stream.ReadAllSerializedData<ConversationTreeNode>();

        stream.Position += (2 * 4);
        FalseNodes = stream.ReadAllSerializedData<ConversationTreeNode>();

        stream.Position += (2 * 4);
        ChildNodes = stream.ReadAllSerializedData<ConversationTreeNode>();


       

    }
}


    public class ConvLocalDisplayTimes
    {
        public int[] I0 = new int[10];

        public ConvLocalDisplayTimes(CrystalMpq.MpqFileStream stream)
        {
            for (int i = 0; i < I0.Length; i++)
                I0[i] = stream.ReadInt32();
        }
    }

}
