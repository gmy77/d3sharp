using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mooege.Common.MPQ;
using Mooege.Core.GS.Map;
using Mooege.Net.GS.Message.Definitions.Quest;
using Mooege.Core.GS.Common.Types.SNO;

namespace Mooege.Core.GS.Games
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
                public int Counter { get; private set;}
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
                    if(type != _objective.ObjectiveType && (int) type != -1) return;
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
                        case (Mooege.Common.MPQ.FileFormats.QuestStepObjectiveType)(- 1):
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

            private void UpdateCounter(QuestObjective objective)
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

                if(assetQuestStep.StepBonusObjectiveSets != null)
                    foreach (var objectiveSet in assetQuestStep.StepBonusObjectiveSets)
                        bonusObjectives.Add(new List<QuestObjective>(from objective in objectiveSet.StepBonusObjectives select new QuestObjective(objective, this, c++)));
            }


        }



        private Mooege.Common.MPQ.FileFormats.Quest asset = null;
        //public int SNOId { get; set; }
        public SNOName SNOName { get; set; }
        private Game game { get; set; }
        public QuestStep CurrentStep { get; set; }

        public Quest(Game game, int SNOQuest, int step = -1)
        {
            this.game = game;
            //SNOId = SNOQuest;
            SNOName = new SNOName() { SNOId = SNOQuest, Group = SNOGroup.Quest };
            asset = MPQStorage.Data.Assets[Common.Types.SNO.SNOGroup.Quest][SNOQuest].Data as Mooege.Common.MPQ.FileFormats.Quest;
            if (step == -1)
                CurrentStep = new QuestStep(asset.QuestUnassignedStep, this);
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

            CurrentStep = (from step in asset.QuestSteps where step.ID == FollowUpStepID select new QuestStep(step, this)).FirstOrDefault();
        }

        public void Notify(Mooege.Common.MPQ.FileFormats.QuestStepObjectiveType type, int value)
        {
            if(CurrentStep != null)
                foreach (var objectiveSet in CurrentStep.ObjectivesSets)
                    foreach (var objective in objectiveSet.Objectives)
                        objective.Notify(type, value);
        }
    }


    public class QuestManager: QuestProgressHandler, IEnumerable<Quest>
    {
        private Dictionary<int,Quest> quests = new Dictionary<int,Quest>();

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

        public void Notify(Mooege.Common.MPQ.FileFormats.QuestStepObjectiveType type, int value)
        {
            foreach (var quest in quests.Values)
                (quest as QuestProgressHandler).Notify(type, value);
        }

        public IEnumerator<Quest> GetEnumerator()
        {
            return quests.Values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return null;
        }
    }
}
