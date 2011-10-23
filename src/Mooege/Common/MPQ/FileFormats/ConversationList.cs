using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CrystalMpq;
using Mooege.Common.Extensions;

namespace Mooege.Common.MPQ.FileFormats
{
    /// <summary>
    ///  There are no conversationLists in the CoreData with size > 0, so this is not used (yet)
    /// </summary>
    [FileFormat(SNOGroup.ConversationList)]
    public class ConversationList : FileFormat
    {
        public Header Header;
        public List<ConversationListEntry> ConversationListEntries;

        public ConversationList(MpqFile file)
        {
            MpqFileStream stream = file.Open();
            this.Header = new Header(stream);
            stream.Position += (2 * 4);
            ConversationListEntries = stream.ReadAllSerializedData<ConversationListEntry>();

            stream.Close();
        }

    }

    public class ConversationListEntry : ISerializableData
    {
        public int SNOConv;
        public int I0;
        public int I1;
        public int I2;
        public int GbidItem;
        public string Noname1;
        public string Noname2;
        public int SNOQuestCurrent;
        public int I3;
        public int SNOQuestAssigned;
        public int SNOQuestActive;
        public int SNOQuestComplete;
        public int SNOQuestRange;
        public int SNOLevelArea;

        public void Read(CrystalMpq.MpqFileStream stream)
        {
            SNOConv = stream.ReadInt32();
            I0 = stream.ReadInt32();
            I1 = stream.ReadInt32();
            I2 = stream.ReadInt32();
            GbidItem = stream.ReadInt32();
            Noname1 = stream.ReadString(128);
            Noname2 = stream.ReadString(128);
            SNOQuestCurrent = stream.ReadInt32();
            I3 = stream.ReadInt32();
            SNOQuestAssigned = stream.ReadInt32();
            SNOQuestActive = stream.ReadInt32();
            SNOQuestComplete = stream.ReadInt32();
            SNOQuestRange = stream.ReadInt32();
            SNOLevelArea = stream.ReadInt32();
        }
    }

}
