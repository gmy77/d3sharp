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

using System;
using System.Collections.Generic;
using Mooege.Common.Logging;
using Mooege.Common.MPQ;

namespace Mooege.Core.GS.Games
{
    public class QuestManager : QuestProgressHandler, IEnumerable<Quest>
    {
        private Dictionary<int, Quest> quests = new Dictionary<int, Quest>();
        private static readonly Logger Logger = new Logger("QuestManager");

        /// <summary>
        /// Accessor for quests
        /// </summary>
        /// <param name="snoQuest">snoId of the quest to retrieve</param>
        /// <returns></returns>
        public Quest this[int snoQuest]
        {
            get { return quests[snoQuest]; }
        }

        /// <summary>
        /// Creates a new QuestManager and attaches it to a game. Quests are are share among all players in a game
        /// </summary>
        /// <param name="game">The game is used to broadcast game messages to</param>
        public QuestManager(Game game)
        {
            var asset = MPQStorage.Data.Assets[Common.Types.SNO.SNOGroup.Quest];
            foreach (var quest in asset.Keys)
                quests.Add(quest, new Quest(game, quest));
        }

        /// <summary>
        /// Debug method. Advances a quest by a step
        /// </summary>
        /// <param name="snoQuest">snoID of the quest to advance</param>
        public void Advance(int snoQuest)
        {
            quests[snoQuest].Advance();
        }

        /// <summary>
        /// Call this, to trigger quest progress if a certain event has occured
        /// </summary>
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

            if (range.Start.SNOQuest == -1 || range.Start.StepID == -1)
                started = true;
            else
            {
                if (quests.ContainsKey(range.Start.SNOQuest))
                {
                    if (quests[range.Start.SNOQuest].HasStepCompleted(range.Start.StepID) || quests[range.Start.SNOQuest].CurrentStep.QuestStepID == range.Start.StepID) // rumford conversation needs current step
                        started = true;
                }
                //else logger.Warn("QuestRange {0} references unknown quest {1}", range.Header.SNOId, range.Start.SNOQuest);
            }

            if (range.End.SNOQuest == -1 || range.End.StepID < 0)
                ended = false;
            else
            {
                if (quests.ContainsKey(range.End.SNOQuest))
                {
                    if (quests[range.End.SNOQuest].HasStepCompleted(range.End.StepID))
                        ended = true;
                }
                //else logger.Warn("QuestRange {0} references unknown quest {1}", range.Header.SNOId, range.End.SNOQuest);
            }

            return started && !ended;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
