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
using System.Text;
using Gibbed.IO;
using Mooege.Common.MPQ.FileFormats.Types;

namespace Mooege.Common.MPQ.FileFormats
{
    // <Descriptor Name="Quest">
    //  <Field Type="DT_ENUM" Offset="12" Min="0" Max="2" Flags="17" EncodedBits="2"></Field>
    //  <Field Type="DT_INT" Offset="16" Flags="1" EncodedBits="32" />
    //  <Field Type="DT_INT" Offset="20" Flags="1" EncodedBits="32" />
    //  <Field Type="DT_INT" Offset="24" Flags="1" EncodedBits="32" />
    //  <Field Type="DT_INT" Offset="28" Flags="1" EncodedBits="32" />
    //  <Field Type="DT_INT" Offset="32" Flags="1" EncodedBits="32" />
    //  <Field Type="DT_INT" Offset="36" Flags="1" EncodedBits="32" />
    //  <Field Type="QuestUnassignedStep" Offset="40" Flags="1" />
    //  <Field Name="serQuestSteps" Type="SerializeData" Offset="88" Flags="0" />
    //  <Field Type="DT_VARIABLEARRAY" Offset="80" Flags="33" SubType="QuestStep" />
    //  <Field Name="serQuestCompletionSteps" Type="SerializeData" Offset="104" Flags="0" />
    //  <Field Type="DT_VARIABLEARRAY" Offset="96" Flags="33" SubType="QuestCompletionStep" />
    //  <Field Type="QuestLevelRange" Offset="112" Flags="1" />
    //  <Field Type="QuestLevelRange" Offset="120" Flags="1" />
    //  <Field Type="QuestLevelRange" Offset="128" Flags="1" />
    //  <Field Type="QuestLevelRange" Offset="136" Flags="1" />
    //  <Field Type="DT_NULL" Offset="144" Flags="0" />
    // </Descriptor>
    [FileFormat(SNOGroup.Quest)]
    class Quest : FileFormat
    {
        public Header Header;
        public QuestType QuestType;
        public int I0;
        public int I1;
        public int I2;
        public int I3;
        public int I4;
        public int I5;
        public QuestUnassignedStep QuestUnassignedStep;
        public List<QuestStep> QuestSteps;
        public List<QuestCompletionStep> QuestCompletionSteps;
        public QuestLevelRange LevelRange1;
        public QuestLevelRange LevelRange2;
        public QuestLevelRange LevelRange3;
        public QuestLevelRange LevelRange4;


        public Quest(MpqFile file)
        {
            MpqFileStream stream = file.Open();

            Header = new Header(stream);
            QuestType = (QuestType)stream.ReadValueS32();
            I0 = stream.ReadValueS32();
            I1 = stream.ReadValueS32();
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


    // <Descriptor Name="QuestUnassignedStep">
    //  <Field Type="DT_INT" Offset="0" Flags="1" EncodedBits="32" />
    //  <Field Name="serStepObjectiveSets" Type="SerializeData" Offset="16" Flags="0" />
    //  <Field Type="DT_VARIABLEARRAY" Offset="8" Flags="33" SubType="QuestStepObjectiveSet" />
    //  <Field Name="serStepFailureConditionSets" Type="SerializeData" Offset="32" Flags="0" />
    //  <Field Type="DT_VARIABLEARRAY" Offset="24" Flags="33" SubType="QuestStepFailureConditionSet" />
    //  <Field Type="DT_NULL" Offset="40" Flags="0" />
    // </Descriptor>
    public class QuestUnassignedStep
    {
        public int I0;
        public List<QuestStepObjectiveSet> StepObjectiveSets;
        public List<QuestStepFailureConditionSet> StepFailureConditionsSets;

        public QuestUnassignedStep(MpqFileStream stream)
        {
            I0 = stream.ReadValueS32();
            stream.Position += 4;       // unaccounted in xml
            stream.Position += (2 * 4);
            StepObjectiveSets = stream.ReadSerializedData<QuestStepObjectiveSet>();

            stream.Position += (2 * 4);
            StepFailureConditionsSets = stream.ReadSerializedData<QuestStepFailureConditionSet>();
        }
    }


    // <Descriptor Name="QuestStepObjectiveSet">
    //  <Field Type="DT_INT" Offset="0" Flags="1" EncodedBits="32" />
    //  <Field Name="serStepObjectives" Type="SerializeData" Offset="16" Flags="0" />
    //  <Field Type="DT_VARIABLEARRAY" Offset="8" Flags="33" SubType="QuestStepObjective" />
    //  <Field Type="DT_NULL" Offset="24" Flags="0" />
    // </Descriptor>
    public class QuestStepObjectiveSet : ISerializableData
    {
        public int I0;
        public List<QuestStepObjective> StepObjectives;

        public void Read(MpqFileStream stream)
        {
            I0 = stream.ReadValueS32();

            stream.Position += 4;
            stream.Position += 8;
            StepObjectives = stream.ReadSerializedData<QuestStepObjective>();
        }
    }


    // <Descriptor Name="QuestStepObjective">
    //  <Field Type="DT_INT" Offset="0" Flags="1" EncodedBits="32" />
    //  <Field Type="DT_ENUM" Offset="4" Min="0" Max="13" Flags="17" EncodedBits="4">
    //
    //  </Field>
    //  <Field Type="DT_INT" Offset="8" Flags="1" EncodedBits="32" />
    //  <Field Type="DT_INT" Offset="12" Flags="1" EncodedBits="32" />
    //  <Field Type="SNOName" Offset="16" Flags="1" />
    //  <Field Type="SNOName" Offset="24" Flags="1" />
    //  <Field Type="DT_GBID" Offset="32" Flags="1" EncodedBits="32" />
    //  <Field Type="DT_GBID" Offset="36" Flags="1" EncodedBits="32" />
    //  <Field Type="DT_CHARARRAY" Offset="40" Flags="1" ArrayLength="256" EncodedBits="9" />
    //  <Field Type="DT_CHARARRAY" Offset="296" Flags="1" ArrayLength="256" EncodedBits="9" />
    //  <Field Type="DT_INT" Offset="552" Min="0" Max="1" Flags="17" EncodedBits="1" />
    //  <Field Type="DT_INT" Offset="556" Flags="1" EncodedBits="32" />
    //  <Field Name="gbidItemToShow" Type="DT_GBID" Offset="560" Flags="1" EncodedBits="32" SnoType="2" />
    //  <Field Type="DT_NULL" Offset="564" Flags="0" />
    // </Descriptor>
    public class QuestStepObjective : ISerializableData
    {
        public int I0;
        public QuestStepObjectiveType objectiveType;
        public int I2;
        public int I3;
        public SNOName SNOName1;
        public SNOName SNOName2;
        public int GBID1;
        public int GBID2;
        public string Unknown1;
        public string Unknown2;
        public int I4;              // min = 0, max = 1 unless i know what it is im not making it a bool
        public int I5;
        public int GBIDItemToShow;

        public void Read(MpqFileStream stream)
        {
            I0 = stream.ReadValueS32();
            objectiveType = (QuestStepObjectiveType)stream.ReadValueS32();
            I2 = stream.ReadValueS32();
            I3 = stream.ReadValueS32();
            SNOName1 = new SNOName(stream);
            SNOName2 = new SNOName(stream);
            GBID1 = stream.ReadValueS32();
            GBID2 = stream.ReadValueS32();
            Unknown1 = stream.ReadString(256);
            Unknown2 = stream.ReadString(256);
            I4 = stream.ReadValueS32();
            I5 = stream.ReadValueS32();
            GBIDItemToShow = stream.ReadValueS32();
        }
    }


    // <Descriptor Name="QuestStepFailureConditionSet">
    //  <Field Name="serStepFailureConditions" Type="SerializeData" Offset="8" Flags="0" />
    //  <Field Type="DT_VARIABLEARRAY" Offset="0" Flags="33" SubType="QuestStepFailureCondition" />
    //  <Field Type="DT_NULL" Offset="16" Flags="0" />
    // </Descriptor>
    public class QuestStepFailureConditionSet : ISerializableData
    {
        public List<QuestStepFailureCondition> QuestStepFailureConditions;

        public void Read(MpqFileStream stream)
        {
            stream.Position += 8;
            QuestStepFailureConditions = stream.ReadSerializedData<QuestStepFailureCondition>();
        }
    }


    // <Descriptor Name="QuestStepFailureCondition">
    //  <Field Type="DT_ENUM" Offset="0" Min="0" Max="7" Flags="17" EncodedBits="3" />
    //  <Field Type="DT_INT" Offset="4" Flags="1" EncodedBits="32" />
    //  <Field Type="DT_INT" Offset="8" Flags="1" EncodedBits="32" />
    //  <Field Type="SNOName" Offset="12" Flags="1" />
    //  <Field Type="SNOName" Offset="20" Flags="1" />
    //  <Field Type="DT_GBID" Offset="28" Flags="1" EncodedBits="32" />
    //  <Field Type="DT_GBID" Offset="32" Flags="1" EncodedBits="32" />
    //  <Field Type="DT_CHARARRAY" Offset="36" Flags="1" ArrayLength="256" EncodedBits="9" />
    //  <Field Type="DT_CHARARRAY" Offset="292" Flags="1" ArrayLength="256" EncodedBits="9" />
    //  <Field Type="DT_NULL" Offset="548" Flags="0" />
    // </Descriptor>
    public class QuestStepFailureCondition : ISerializableData
    {
        public QuestStepFailureConditionType ConditionType;
        public int I2;
        public int I3;
        public SNOName SNOName1;
        public SNOName SNOName2;
        public int GBID1;
        public int GBID2;
        public string Unknown1;
        public string Unknown2;

        public void Read(MpqFileStream stream)
        {
            ConditionType = (QuestStepFailureConditionType)stream.ReadValueS32();
            I2 = stream.ReadValueS32();
            I3 = stream.ReadValueS32();
            SNOName1 = new SNOName(stream);
            SNOName2 = new SNOName(stream);
            GBID1 = stream.ReadValueS32();
            GBID2 = stream.ReadValueS32();
            Unknown1 = stream.ReadString(256);
            Unknown2 = stream.ReadString(256);
        }
    }


    // <Descriptor Name="QuestLevelRange">
    //  <Field Type="DT_INT" Offset="0" Flags="1" EncodedBits="32" />
    //  <Field Type="DT_INT" Offset="4" Flags="1" EncodedBits="32" />
    //  <Field Type="DT_NULL" Offset="8" Flags="0" />
    // </Descriptor>
    public class QuestLevelRange
    {
        public int I0;
        public int I1;

        public QuestLevelRange(MpqFileStream stream)
        {
            I0 = stream.ReadValueS32();
            I1 = stream.ReadValueS32();
        }
    }


    // <Descriptor Name="QuestStep">
    //  <Field Type="DT_CHARARRAY" Offset="0" Flags="1" ArrayLength="64" EncodedBits="7" />
    //  <Field Type="DT_INT" Offset="64" Flags="1" EncodedBits="32" />
    //  <Field Type="DT_INT" Offset="68" Flags="1" EncodedBits="32" />
    //  <Field Type="DT_FIXEDARRAY" Offset="72" Flags="1" SubType="DT_INT" ArrayLength="4" EncodedBits="32" />
    //  <Field Type="DT_ENUM" Offset="88" Min="0" Max="3" Flags="17" EncodedBits="2"></Field>
    //  <Field Name="snoRewardRecipe" Type="DT_FIXEDARRAY" Offset="92" Flags="1" SubType="DT_SNO" ArrayLength="5" EncodedBits="32" SnoType="49" />
    //  <Field Name="snoRewardTreasureClass" Type="DT_SNO" Offset="112" Flags="1" EncodedBits="32" SnoType="52" />
    //  <Field Type="DT_INT" Offset="116" Flags="1" EncodedBits="32" />
    //  <Field Type="DT_INT" Offset="120" Flags="1" EncodedBits="32" />
    //  <Field Type="DT_ENUM" Offset="124" Min="0" Max="3" Flags="17" EncodedBits="2"></Field>
    //  <Field Name="snoReplayRewardRecipe" Type="DT_FIXEDARRAY" Offset="128" Flags="1" SubType="DT_SNO" ArrayLength="5" EncodedBits="32" SnoType="49" />
    //  <Field Name="snoReplayRewardTreasureClass" Type="DT_SNO" Offset="148" Flags="1" EncodedBits="32" SnoType="52" />
    //  <Field Type="DT_INT" Offset="152" Flags="1" EncodedBits="32" />
    //  <Field Type="DT_INT" Offset="156" Flags="1" EncodedBits="32" />
    //  <Field Name="snoPowerGranted" Type="DT_SNO" Offset="160" Flags="1" EncodedBits="32" SnoType="29" />
    //  <Field Name="snoWaypointLevelAreas" Type="DT_FIXEDARRAY" Offset="164" Flags="1" SubType="DT_SNO" ArrayLength="2" EncodedBits="32" SnoType="22" />
    //  <Field Name="serStepObjectiveSets" Type="SerializeData" Offset="184" Flags="0" />
    //  <Field Type="DT_VARIABLEARRAY" Offset="176" Flags="33" SubType="QuestStepObjectiveSet" />
    //  <Field Name="serStepBonusObjectiveSets" Type="SerializeData" Offset="200" Flags="0" />
    //  <Field Type="DT_VARIABLEARRAY" Offset="192" Flags="33" SubType="QuestStepBonusObjectiveSet" />
    //  <Field Name="serStepFailureConditionSets" Type="SerializeData" Offset="216" Flags="0" />
    //  <Field Type="DT_VARIABLEARRAY" Offset="208" Flags="33" SubType="QuestStepFailureConditionSet" />
    //  <Field Type="DT_NULL" Offset="224" Flags="0" />
    // </Descriptor>
    public class QuestStep : ISerializableData
    {
        public string Unknown1;
        public int I0;
        public int I1;
        public int[] I2 = new int[4];
        public Enum1 Enum1;
        public int[] SNORewardRecipe = new int[5];
        public int SNORewardTreasureClass;
        public int I3;
        public int I4;
        public Enum1 Enum2;
        public int[] SNOReplayRewardRecipe = new int[5];
        public int SNOReplayRewardTreasureClass;
        public int I5;
        public int I6;
        public int SNOPowerGranted;
        public int[] SNOWaypointLevelAreas = new int[2];

        public List<QuestStepObjectiveSet> StepObjectiveSets;
        public List<QuestStepBonusObjectiveSet> StepBonusObjectiveSets;
        public List<QuestStepFailureConditionSet> StepFailureConditionSet;

        public void Read(MpqFileStream stream)
        {
            Unknown1 = stream.ReadString(64);
            I0 = stream.ReadValueS32();
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
            StepFailureConditionSet = stream.ReadSerializedData<QuestStepFailureConditionSet>();
        }
    }

    // <Descriptor Name="QuestStepBonusObjectiveSet">
    //  <Field Type="DT_FIXEDARRAY" Offset="0" Flags="1" SubType="DT_INT" ArrayLength="4" EncodedBits="32" />
    //  <Field Type="DT_INT" Offset="16" Flags="1" EncodedBits="32" />
    //  <Field Type="DT_INT" Offset="20" Flags="1" EncodedBits="32" />
    //  <Field Type="DT_INT" Offset="24" Min="0" Max="1" Flags="17" EncodedBits="1" />
    //  <Field Type="DT_INT" Offset="28" Min="0" Max="1" Flags="17" EncodedBits="1" />
    //  <Field Name="serStepBonusObjectives" Type="SerializeData" Offset="40" Flags="0" />
    //  <Field Type="DT_VARIABLEARRAY" Offset="32" Flags="33" SubType="QuestStepObjective" />
    //  <Field Type="DT_NULL" Offset="48" Flags="0" />
    // </Descriptor>
    public class QuestStepBonusObjectiveSet : ISerializableData
    {
        public int[] I0 = new int[4];
        public int I1;
        public int I2;
        public int I3;
        public int I4;
        public List<QuestStepObjective> StepBonusObjectives;

        public void Read(MpqFileStream stream)
        {
            for (int i = 0; i < I0.Length; i++)
                I0[i] = stream.ReadValueS32();

            I1 = stream.ReadValueS32();
            I2 = stream.ReadValueS32();
            I3 = stream.ReadValueS32();
            I4 = stream.ReadValueS32();

            stream.Position += 8;
            StepBonusObjectives = stream.ReadSerializedData<QuestStepObjective>();
        }
    }

    // <Descriptor Name="QuestCompletionStep">
    //  <Field Type="DT_CHARARRAY" Offset="0" Flags="1" ArrayLength="64" EncodedBits="7" />
    //  <Field Type="DT_INT" Offset="64" Flags="1" EncodedBits="32" />
    //  <Field Type="DT_INT" Offset="68" Min="0" Max="1" Flags="17" EncodedBits="1" />
    //  <Field Type="DT_NULL" Offset="72" Flags="0" />
    // </Descriptor>
    public class QuestCompletionStep : ISerializableData
    {
        public string Unknown;
        public int I1;
        public int I2;

        public void Read(MpqFileStream stream)
        {
            Unknown = stream.ReadString(64);
            I1 = stream.ReadValueS32();
            I2 = stream.ReadValueS32();
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
