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
using System.Linq;
using Mooege.Common.Storage.AccountDataBase.Entities;
using Mooege.Core.MooNet.Toons;
using Mooege.Net.GS.Message.Fields;
using Mooege.Common.Logging;
using Mooege.Net.MooNet;
using System.Data.SQLite;
using Mooege.Common.Storage;

namespace Mooege.Core.GS.Skills
{
    public class SkillSet
    {
        public ToonClass @Class;
        public Toon Toon { get; private set; }

        public ActiveSkillSavedData[] ActiveSkills;
        public HotbarButtonData[] HotBarSkills;
        public int[] PassiveSkills;

        protected static readonly Logger Logger = LogManager.CreateLogger();

        public SkillSet(ToonClass @class, Toon toon)
        {
            this.@Class = @class;
            var dbToon = DBSessions.AccountSession.Get<DBToon>(toon.PersistentID);

            if (dbToon.DBActiveSkills == null)
            {
                int[] ActiveSkillsList = Skills.GetAllActiveSkillsByClass(this.@Class).Take(1).ToArray();

                this.ActiveSkills = new ActiveSkillSavedData[6] {
                    new ActiveSkillSavedData { snoSkill = ActiveSkillsList[0], snoRune = -1 },
                    new ActiveSkillSavedData { snoSkill = Skills.None, snoRune = -1 },
                    new ActiveSkillSavedData { snoSkill = Skills.None, snoRune = -1 },
                    new ActiveSkillSavedData { snoSkill = Skills.None, snoRune = -1 },
                    new ActiveSkillSavedData { snoSkill = Skills.None, snoRune = -1 },
                    new ActiveSkillSavedData { snoSkill = Skills.None, snoRune = -1 }
                };

                this.PassiveSkills = new int[3] {
                    -1,-1,-1,
                };


                dbToon.DBActiveSkills = new DBActiveSkills
                                            {
                                                Skill0 = ActiveSkillsList[0],
                                                Skill1 = -1,
                                                Skill2 = -1,
                                                Skill3 = -1,
                                                Skill4 = -1,
                                                Skill5 = -1,
                                                Rune0 = -1,
                                                Rune1 = -1,
                                                Rune2 = -1,
                                                Rune3 = -1,
                                                Rune4 = -1,
                                                Rune5 = -1,
                                                Passive0 = -1,
                                                Passive1 = -1,
                                                Passive2 = -1
                                            };
                DBSessions.AccountSession.SaveOrUpdate(dbToon.DBActiveSkills);
                DBSessions.AccountSession.Flush();

            }
            else
            {
                this.ActiveSkills = new ActiveSkillSavedData[6] {
                    new ActiveSkillSavedData {  snoSkill = dbToon.DBActiveSkills.Skill0, 
                                                snoRune  = dbToon.DBActiveSkills.Rune0 },
                    new ActiveSkillSavedData {  snoSkill = dbToon.DBActiveSkills.Skill1, 
                                                snoRune  = dbToon.DBActiveSkills.Rune1 },
                    new ActiveSkillSavedData {  snoSkill = dbToon.DBActiveSkills.Skill2, 
                                                snoRune  = dbToon.DBActiveSkills.Rune2 },
                    new ActiveSkillSavedData {  snoSkill = dbToon.DBActiveSkills.Skill3, 
                                                snoRune  = dbToon.DBActiveSkills.Rune3 },
                    new ActiveSkillSavedData {  snoSkill = dbToon.DBActiveSkills.Skill4, 
                                                snoRune  = dbToon.DBActiveSkills.Rune4 },
                    new ActiveSkillSavedData {  snoSkill = dbToon.DBActiveSkills.Skill5, 
                                                snoRune  = dbToon.DBActiveSkills.Rune5 },
                };
                this.PassiveSkills = new int[3] {
                    dbToon.DBActiveSkills.Passive0,
                    dbToon.DBActiveSkills.Passive1,
                    dbToon.DBActiveSkills.Passive2,
                };
            }

            this.HotBarSkills = new HotbarButtonData[6] {
                new HotbarButtonData { SNOSkill = ActiveSkills[0].snoSkill, Field1 = -1, ItemGBId = -1 }, // left-click
                new HotbarButtonData { SNOSkill = ActiveSkills[1].snoSkill, Field1 = -1, ItemGBId = -1 }, // right-click
                new HotbarButtonData { SNOSkill = ActiveSkills[2].snoSkill, Field1 = -1, ItemGBId = -1 }, // bar-1
                new HotbarButtonData { SNOSkill = ActiveSkills[3].snoSkill, Field1 = -1, ItemGBId = -1 }, // bar-2
                new HotbarButtonData { SNOSkill = ActiveSkills[4].snoSkill, Field1 = -1, ItemGBId = -1 }, // bar-3
                new HotbarButtonData { SNOSkill = ActiveSkills[5].snoSkill, Field1 = -1, ItemGBId = -1 }, // bar-4
            };
        }

        public void UpdateSkills(int hotBarIndex, int SNOSkill, int SNORune, Toon toon)
        {
            Logger.Debug("Update index {0} skill {1} rune {2}", hotBarIndex, SNOSkill, SNORune);
            var dbToon = DBSessions.AccountSession.Get<DBToon>(toon.PersistentID);
            switch (hotBarIndex)
            {
                case 0:
                    dbToon.DBActiveSkills.Skill0 = SNOSkill;
                    dbToon.DBActiveSkills.Rune0 = SNORune;
                    break;
                case 1:
                    dbToon.DBActiveSkills.Skill1 = SNOSkill;
                    dbToon.DBActiveSkills.Rune1 = SNORune;
                    break;
                case 2:
                    dbToon.DBActiveSkills.Skill2 = SNOSkill;
                    dbToon.DBActiveSkills.Rune2 = SNORune;
                    break;
                case 3:
                    dbToon.DBActiveSkills.Skill3 = SNOSkill;
                    dbToon.DBActiveSkills.Rune3 = SNORune;
                    break;
                case 4:
                    dbToon.DBActiveSkills.Skill4 = SNOSkill;
                    dbToon.DBActiveSkills.Rune4 = SNORune;
                    break;
                case 5:
                    dbToon.DBActiveSkills.Skill5 = SNOSkill;
                    dbToon.DBActiveSkills.Rune5 = SNORune;
                    break;
            }
            DBSessions.AccountSession.SaveOrUpdate(dbToon.DBActiveSkills);
            DBSessions.AccountSession.Flush();

        }

        public void SwitchUpdateSkills(int oldSNOSkill, int SNOSkill, int SNORune, Toon toon)
        {
            for (int i = 0; i < this.HotBarSkills.Length; i++)
            {
                if (this.HotBarSkills[i].SNOSkill == oldSNOSkill)
                {
                    Logger.Debug("SkillSet: SwitchUpdateSkill Oldskill {0} Newskill {1}", oldSNOSkill, SNOSkill);
                    this.HotBarSkills[i].SNOSkill = SNOSkill;
                    this.UpdateSkills(i, SNOSkill, SNORune, toon);
                    return;
                }
            }
        }

        public void UpdatePassiveSkills(Toon toon)
        {
            Logger.Debug("Update passive to {0} {1} {2}", PassiveSkills[0], PassiveSkills[1], PassiveSkills[2]);
            var dbToon = DBSessions.AccountSession.Get<DBToon>(toon.PersistentID);
            dbToon.DBActiveSkills.Passive0 = PassiveSkills[0];
            dbToon.DBActiveSkills.Passive1 = PassiveSkills[1];
            dbToon.DBActiveSkills.Passive2 = PassiveSkills[2];

            DBSessions.AccountSession.SaveOrUpdate(dbToon.DBActiveSkills);
            DBSessions.AccountSession.Flush();

        }
    }
}
