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
    [FileFormat(SNOGroup.Quest)]
    class Quest : FileFormat
    {
        public Header Header { get; private set; }
        public QuestType QuestType { get; private set; }
        public int NumberOfSteps { get; private set; }
        public int NumberOfCompletionSteps { get; private set; }
        public int I2 { get; private set; } // not uniqe..10 for like 3 quests
        public int I3 { get; private set; } // always 6 except for vendor_rescue_event which has 4
        public int I4 { get; private set; } // always -1 in beta
        public int I5 { get; private set; } // always 0 in beta
        public QuestUnassignedStep QuestUnassignedStep { get; private set; }
        public List<QuestStep> QuestSteps { get; private set; }
        public List<QuestCompletionStep> QuestCompletionSteps { get; private set; }
        public QuestLevelRange LevelRange1 { get; private set; }
        public QuestLevelRange LevelRange2 { get; private set; }
        public QuestLevelRange LevelRange3 { get; private set; }
        public QuestLevelRange LevelRange4 { get; private set; }

        public Quest(MpqFile file)
        {
            MpqFileStream stream = file.Open();

            Header = new Header(stream);
            QuestType = (QuestType)stream.ReadValueS32();
            NumberOfSteps = stream.ReadValueS32();
            NumberOfCompletionSteps = stream.ReadValueS32();
            I2 = stream.ReadValueS32();
            I3 = stream.ReadValueS32();
            I4 = stream.ReadValueS32();
            I5 = stream.ReadValueS32();

            QuestUnassignedStep = new QuestUnassignedStep(stream);
            stream.Position += 8;
            QuestSteps = stream.ReadSerializedData<QuestStep>();
            stream.Position += 8;
            QuestCompletionSteps = stream.ReadSerializedData<QuestCompletionStep>();

            LevelRange1 = new QuestLevelRange(stream);
            LevelRange2 = new QuestLevelRange(stream);
            LevelRange3 = new QuestLevelRange(stream);
            LevelRange4 = new QuestLevelRange(stream);


            stream.Close();
        }
    }


    public interface IQuestStep
    {
        int ID { get; }
        List<QuestStepObjectiveSet> StepObjectiveSets { get; }
    }

    public class QuestUnassignedStep : IQuestStep
    {
        public int ID { get; private set; }
        public List<QuestStepObjectiveSet> StepObjectiveSets { get; private set; }
        public List<QuestStepFailureConditionSet> StepFailureConditionSets { get; private set; }

        public QuestUnassignedStep(MpqFileStream stream)
        {
            ID = stream.ReadValueS32();
            stream.Position += 4;       // unaccounted in xml
            stream.Position += (2 * 4);
            StepObjectiveSets = stream.ReadSerializedData<QuestStepObjectiveSet>();

            stream.Position += (2 * 4);
            StepFailureConditionSets = stream.ReadSerializedData<QuestStepFailureConditionSet>();
        }
    }

    public class QuestStepObjectiveSet : ISerializableData
    {
        public int FollowUpStepID { get; private set; }
        public List<QuestStepObjective> StepObjectives { get; private set; }

        public void Read(MpqFileStream stream)
        {
            FollowUpStepID = stream.ReadValueS32();

            stream.Position += 4;
            stream.Position += 8;
            StepObjectives = stream.ReadSerializedData<QuestStepObjective>();
        }
    }

    public class QuestStepObjective : ISerializableData
    {
        public int I0 { get; private set; }
        public QuestStepObjectiveType ObjectiveType { get; private set; }
        public int I2 { get; private set; }
        public int CounterTarget { get; private set; }
        public SNOHandle SNOName1 { get; private set; }
        public SNOHandle SNOName2 { get; private set; }
        public int GBID1 { get; private set; }
        public int GBID2 { get; private set; }
        public string Group1Name { get; private set; }
        public string Unknown2 { get; private set; }
        public int I4 { get; private set; }              // min = 0, max = 1 unless i know what it is im not making it a bool
        public int I5 { get; private set; }
        public int GBIDItemToShow { get; private set; }

        public void Read(MpqFileStream stream)
        {
            I0 = stream.ReadValueS32();
            ObjectiveType = (QuestStepObjectiveType)stream.ReadValueS32();
            I2 = stream.ReadValueS32();
            CounterTarget = stream.ReadValueS32();
            SNOName1 = new SNOHandle(stream);
            SNOName2 = new SNOHandle(stream);
            GBID1 = stream.ReadValueS32();
            GBID2 = stream.ReadValueS32();
            Group1Name = stream.ReadString(256, true);
            Unknown2 = stream.ReadString(256, true);
            I4 = stream.ReadValueS32();
            I5 = stream.ReadValueS32();
            GBIDItemToShow = stream.ReadValueS32();
        }
    }

    public class QuestStepFailureConditionSet : ISerializableData
    {
        public List<QuestStepFailureCondition> QuestStepFailureConditions { get; private set; }

        public void Read(MpqFileStream stream)
        {
            stream.Position += 8;
            QuestStepFailureConditions = stream.ReadSerializedData<QuestStepFailureCondition>();
        }
    }

    public class QuestStepFailureCondition : ISerializableData
    {
        public QuestStepFailureConditionType ConditionType { get; private set; }
        public int I2 { get; private set; }
        public int I3 { get; private set; }
        public SNOHandle SNOName1 { get; private set; }
        public SNOHandle SNOName2 { get; private set; }
        public int GBID1 { get; private set; }
        public int GBID2 { get; private set; }
        public string Unknown1 { get; private set; }
        public string Unknown2 { get; private set; }

        public void Read(MpqFileStream stream)
        {
            ConditionType = (QuestStepFailureConditionType)stream.ReadValueS32();
            I2 = stream.ReadValueS32();
            I3 = stream.ReadValueS32();
            SNOName1 = new SNOHandle(stream);
            SNOName2 = new SNOHandle(stream);
            GBID1 = stream.ReadValueS32();
            GBID2 = stream.ReadValueS32();
            Unknown1 = stream.ReadString(256, true);
            Unknown2 = stream.ReadString(256, true);
        }
    }

    public class QuestLevelRange
    {
        public int Min { get; private set; }
        public int Max { get; private set; }

        public QuestLevelRange(MpqFileStream stream)
        {
            Min = stream.ReadValueS32();
            Max = stream.ReadValueS32();
        }
    }

    public class QuestStep : ISerializableData, IQuestStep
    {
        public string Name { get; private set; }
        public int ID { get; private set; }
        public int I1 { get; private set; }
        public int[] I2 = new int[4];
        public Enum1 Enum1 { get; private set; }
        public int[] SNORewardRecipe = new int[5];
        public int SNORewardTreasureClass { get; private set; }
        public int I3 { get; private set; }
        public int I4 { get; private set; }
        public Enum1 Enum2 { get; private set; }
        public int[] SNOReplayRewardRecipe = new int[5];
        public int SNOReplayRewardTreasureClass { get; private set; }
        public int I5 { get; private set; }
        public int I6 { get; private set; }
        public int SNOPowerGranted { get; private set; }
        public int[] SNOWaypointLevelAreas = new int[2];

        public List<QuestStepObjectiveSet> StepObjectiveSets { get; private set; }
        public List<QuestStepBonusObjectiveSet> StepBonusObjectiveSets { get; private set; }
        public List<QuestStepFailureConditionSet> StepFailureConditionSets { get; private set; }

        public void Read(MpqFileStream stream)
        {
            Name = stream.ReadString(64, true);
            ID = stream.ReadValueS32();
            I1 = stream.ReadValueS32();

            for (int i = 0; i < I2.Length; i++)
                I2[i] = stream.ReadValueS32();

            Enum1 = (Enum1)stream.ReadValueS32();

            for (int i = 0; i < SNORewardRecipe.Length; i++)
                SNORewardRecipe[i] = stream.ReadValueS32();

            SNORewardTreasureClass = stream.ReadValueS32();
            I3 = stream.ReadValueS32();
            I4 = stream.ReadValueS32();
            Enum2 = (Enum1)stream.ReadValueS32();

            for (int i = 0; i < SNOReplayRewardRecipe.Length; i++)
                SNOReplayRewardRecipe[i] = stream.ReadValueS32();

            SNOReplayRewardTreasureClass = stream.ReadValueS32();
            I5 = stream.ReadValueS32();
            I6 = stream.ReadValueS32();
            SNOPowerGranted = stream.ReadValueS32();

            for (int i = 0; i < SNOWaypointLevelAreas.Length; i++)
                SNOWaypointLevelAreas[i] = stream.ReadValueS32();

            stream.Position += 4;      // unnacounted for in the xml

            stream.Position += 8;
            StepObjectiveSets = stream.ReadSerializedData<QuestStepObjectiveSet>();

            stream.Position += 8;
            StepBonusObjectiveSets = stream.ReadSerializedData<QuestStepBonusObjectiveSet>();

            stream.Position += 8;
            StepFailureConditionSets = stream.ReadSerializedData<QuestStepFailureConditionSet>();
        }
    }

    public class QuestStepBonusObjectiveSet : ISerializableData
    {
        public int[] I0 = new int[4];
        public int I1 { get; private set; }
        public int I2 { get; private set; }
        public int I3 { get; private set; }
        public int I4 { get; private set; }
        public int I5 { get; private set; }
        public List<QuestStepObjective> StepBonusObjectives { get; private set; }

        public void Read(MpqFileStream stream)
        {
            for (int i = 0; i < I0.Length; i++)
                I0[i] = stream.ReadValueS32();

            I1 = stream.ReadValueS32();
            I2 = stream.ReadValueS32();
            I3 = stream.ReadValueS32();
            I4 = stream.ReadValueS32();
            I5 = stream.ReadValueS32();

            stream.Position += 12;
            StepBonusObjectives = stream.ReadSerializedData<QuestStepObjective>();
        }
    }

    public class QuestCompletionStep : ISerializableData, IQuestStep
    {
        public string Unknown { get; private set; }
        public int ID { get; private set; }
        public int I2 { get; private set; }

        public void Read(MpqFileStream stream)
        {
            Unknown = stream.ReadString(64, true);
            ID = stream.ReadValueS32();
            I2 = stream.ReadValueS32();
        }

        public List<QuestStepObjectiveSet> StepObjectiveSets
        {
            get { return new List<QuestStepObjectiveSet>(); }
        }
    }


    public enum Enum1
    {
        NoItem = 0,
        SharedRecipe = 1,
        ClassRecipe = 2,
        TreasureClass = 3
    }

    public enum QuestStepFailureConditionType
    {
        MonsterDied = 0,
        PlayerDied = 1,
        ActorDied = 2,
        TimedEventExpired = 3,
        ItemUsed = 4,
        GameFlagSet = 5,
        PlayerFlagSet = 6,
        EventReceived = 7
    }

    public enum QuestStepObjectiveType
    {
        HadConversation = 0,
        PossessItem = 1,
        KillMonster = 2,
        InteractWithActor = 3,
        EnterLevelArea = 4,
        EnterScene = 5,
        EnterWorld = 6,
        EnterTrigger = 7,
        CompleteQuest = 8,
        PlayerFlagSet = 9,
        TimedEventExpired = 10,
        KillGroup = 11,
        GameFlagSet = 12,
        EventReceived = 13
    }

    public enum QuestType
    {
        MainQuest = 0,
        Event = 2
    }
}
