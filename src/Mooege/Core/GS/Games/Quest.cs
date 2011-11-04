using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mooege.Common.MPQ;
using Mooege.Net.GS.Message.Definitions.Quest;
using Mooege.Core.GS.Common.Types.SNO;
using Mooege.Core.GS.Games;
using Mooege.Common;

namespace Mooege.Core.GS.Players
{
    public interface QuestProgressHandler
    {
        void Notify(Mooege.Common.MPQ.FileFormats.QuestStepObjectiveType type, int value);
    }

    public class Quest : QuestProgressHandler
    {
        /// <summary>
        /// Keeps track of a single quest step
        /// </summary>
        public class QuestStep
        {
            /// <summary>
            /// Keeps track of a single quest step objective
            /// </summary>
            public class QuestObjective
            {
                private Mooege.Common.MPQ.FileFormats.QuestStepObjective _objective;
                public int Counter { get; private set; }
                protected QuestStep questStep;
                public int ID { get; private set; }
                public bool Done { get { return (_objective.CounterTarget == 0 && Counter > 0) || Counter == _objective.CounterTarget; } }

                // these are only needed to view show information in console
                public Mooege.Common.MPQ.FileFormats.QuestStepObjectiveType ObjectiveType { get { return _objective.ObjectiveType; } }
                public int ObjectiveValue { get { return _objective.SNOName1.SNOId; } }


                public QuestObjective(Mooege.Common.MPQ.FileFormats.QuestStepObjective objective, QuestStep questStep, int id)
                {
                    ID = id;
                    _objective = objective;
                    this.questStep = questStep;
                }

                public void Notify(Mooege.Common.MPQ.FileFormats.QuestStepObjectiveType type, int value)
                {
                    if (type != _objective.ObjectiveType && (int)type != -1) return;
                    switch (type)
                    {
                        case Mooege.Common.MPQ.FileFormats.QuestStepObjectiveType.EnterWorld:
                        case Mooege.Common.MPQ.FileFormats.QuestStepObjectiveType.EnterScene:
                        case Mooege.Common.MPQ.FileFormats.QuestStepObjectiveType.InteractWithActor:
                        case Mooege.Common.MPQ.FileFormats.QuestStepObjectiveType.KillMonster:
                        case Mooege.Common.MPQ.FileFormats.QuestStepObjectiveType.CompleteQuest:
                        case Mooege.Common.MPQ.FileFormats.QuestStepObjectiveType.HadConversation:
                        case Mooege.Common.MPQ.FileFormats.QuestStepObjectiveType.EnterLevelArea:
                            if (value == _objective.SNOName1.SNOId)
                            {
                                Counter++;
                                questStep.UpdateCounter(this);
                            }
                            break;
                        case (Mooege.Common.MPQ.FileFormats.QuestStepObjectiveType)(-1):
                            Counter++;
                            questStep.UpdateCounter(this);
                            break;
                    }
                }
            }

            internal struct ObjectiveSet
            {
                public List<QuestObjective> Objectives;
                public int FollowUpStepID;
            }

            internal List<ObjectiveSet> ObjectivesSets = new List<ObjectiveSet>();
            private List<List<QuestObjective>> bonusObjectives = new List<List<QuestObjective>>();
            private Mooege.Common.MPQ.FileFormats.QuestStep _questStep = null;
            private Quest _quest = null;
            public int QuestStepID { get { return _questStep.ID; } }

            private void UpdateCounter(QuestObjective objective)
            {
                if (_questStep is Mooege.Common.MPQ.FileFormats.QuestUnassignedStep == false)
                {
                    foreach (var player in _quest.game.Players.Values)
                        player.InGameClient.SendMessage(new QuestCounterMessage()
                        {
                            snoQuest = _quest.SNOName.SNOId,
                            snoLevelArea = -1,
                            StepID = _questStep.ID,
                            TaskIndex = objective.ID,
                            Counter = objective.Counter,
                            Checked = objective.Done ? 1 : 0,
                        });
                }

                var completedObjectiveList = from objectiveSet in ObjectivesSets
                                             where (from o in objectiveSet.Objectives select o.Done).Aggregate((r, o) => r && o)
                                             select objectiveSet.FollowUpStepID;
                if (completedObjectiveList.Count() > 0)
                    _quest.StepCompleted(completedObjectiveList.First());
            }

            public QuestStep(Mooege.Common.MPQ.FileFormats.QuestStep assetQuestStep, Quest quest)
            {
                _questStep = assetQuestStep;
                _quest = quest;
                int c = 0;

                foreach (var objectiveSet in assetQuestStep.StepObjectiveSets)
                    ObjectivesSets.Add(new ObjectiveSet()
                    {
                        FollowUpStepID = objectiveSet.FollowUpStepID,
                        Objectives = new List<QuestObjective>(from objective in objectiveSet.StepObjectives select new QuestObjective(objective, this, c++))
                    });
                c = 0;

                if (assetQuestStep.StepBonusObjectiveSets != null)
                    foreach (var objectiveSet in assetQuestStep.StepBonusObjectiveSets)
                        bonusObjectives.Add(new List<QuestObjective>(from objective in objectiveSet.StepBonusObjectives select new QuestObjective(objective, this, c++)));
            }


        }



        private Mooege.Common.MPQ.FileFormats.Quest asset = null;
        public SNOName SNOName { get; set; }
        private Game game { get; set; }
        public QuestStep CurrentStep { get; set; }
        private List<int> completedSteps = new List<int>();           // this list has to be saved if quest progress should be saved. It is required to keep track of questranges

        public Quest(Game game, int SNOQuest, int step = -1)
        {
            this.game = game;
            //SNOId = SNOQuest;
            SNOName = new SNOName() { SNOId = SNOQuest, Group = SNOGroup.Quest };
            asset = MPQStorage.Data.Assets[Common.Types.SNO.SNOGroup.Quest][SNOQuest].Data as Mooege.Common.MPQ.FileFormats.Quest;
            if (step == -1)
                CurrentStep = new QuestStep(asset.QuestUnassignedStep, this);
        }

        // 
        public bool HasStepCompleted(int stepID)
        {
            return completedSteps.Contains(stepID); // || CurrentStep.ObjectivesSets.Select(x => x.FollowUpStepID).Contains(stepID);
        }

        public void Advance()
        {
            StepCompleted(CurrentStep.ObjectivesSets[0].FollowUpStepID);
        }

        public void StepCompleted(int FollowUpStepID)
        {
            foreach (var player in game.Players.Values)
                player.InGameClient.SendMessage(new QuestUpdateMessage()
                {
                    snoQuest = SNOName.SNOId,
                    snoLevelArea = -1,
                    StepID = FollowUpStepID,
                    Field3 = true,
                    Failed = false
                });
            completedSteps.Add(CurrentStep.QuestStepID);
            CurrentStep = (from step in asset.QuestSteps where step.ID == FollowUpStepID select new QuestStep(step, this)).FirstOrDefault();
        }

        public void Notify(Mooege.Common.MPQ.FileFormats.QuestStepObjectiveType type, int value)
        {
            if (CurrentStep != null)
                foreach (var objectiveSet in CurrentStep.ObjectivesSets)
                    foreach (var objective in objectiveSet.Objectives)
                        objective.Notify(type, value);
        }
    }


    public class QuestManager : QuestProgressHandler, IEnumerable<Quest>
    {
        private Dictionary<int, Quest> quests = new Dictionary<int, Quest>();
        private static Logger logger = new Logger("Quests");

        public Quest this[int snoQuest]
        {
            get { return quests[snoQuest]; }
        }

        public QuestManager(Game game)
        {
            var asset = MPQStorage.Data.Assets[Common.Types.SNO.SNOGroup.Quest];
            foreach (var quest in asset.Keys)
                quests.Add(quest, new Quest(game, quest, -1));

        }

        public void Advance(int snoQuest)
        {
            quests[snoQuest].Advance();
        }

        public void Notify(Mooege.Common.MPQ.FileFormats.QuestStepObjectiveType type, int value)
        {
            foreach (var quest in quests.Values)
                (quest as QuestProgressHandler).Notify(type, value);
        }

        public IEnumerator<Quest> GetEnumerator()
        {
            return quests.Values.GetEnumerator();
        }


        public bool HasCurrentQuest(int snoQuest, int Step)
        {
            if (quests.ContainsKey(snoQuest))
                if (quests[snoQuest].CurrentStep.QuestStepID == Step || Step == -1)
                    return true;

            return false;
        }


        public bool IsInQuestRange(Mooege.Common.MPQ.FileFormats.QuestRange range)
        {
            /* I assume, -1 for start sno means no starting condition and -1 for end sno means no ending of range
             * The field for the step id is sometimes set to negative values (maybe there are negative step id, -1 is maybe the unassignedstep)
             * but also set when no questID is -1. I have no idea what that means. - farmy */

            bool started = false;
            bool ended = false;

            if (range.Time0.SNOQuest == -1 || range.Time0.I0 == -1)
                started = true;
            else
            {
                if (quests.ContainsKey(range.Time0.SNOQuest))
                {
                    if (quests[range.Time0.SNOQuest].HasStepCompleted(range.Time0.I0) || quests[range.Time0.SNOQuest].CurrentStep.QuestStepID == range.Time0.I0) // rumford conversation needs current step
                        started = true;
                }
                else
                    logger.Warn("QuestRange {0} references unknown quest {1}", range.Header.SNOId, range.Time0.SNOQuest);
            }

            if (range.Time1.SNOQuest == -1 || range.Time1.I0 < 0)
                ended = false;
            else
            {
                if (quests.ContainsKey(range.Time1.SNOQuest))
                {
                    if (quests[range.Time1.SNOQuest].HasStepCompleted(range.Time1.I0))
                        ended = true;
                }
                else
                    logger.Warn("QuestRange {0} references unknown quest {1}", range.Header.SNOId, range.Time1.SNOQuest);
            }

            return started && !ended;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
