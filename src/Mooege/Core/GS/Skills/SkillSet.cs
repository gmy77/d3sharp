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

            var query = string.Format("SELECT * FROM active_skills WHERE id_toon={0}", toon.PersistentID);
            var cmd = new SQLiteCommand(query, DBManager.Connection);
            var reader = cmd.ExecuteReader();
            if (!reader.HasRows)
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
                this.PassiveSkills = new int[3] { -1, -1, -1 };

                var insQuery = string.Format("INSERT INTO active_skills (id_toon,"+
                    "skill_0,skill_1,skill_2,skill_3,skill_4,skill_5,"+
                    "rune_0,rune_1,rune_2,rune_3,rune_4,rune_5,"+
                    "passive_0,passive_1,passive_2) VALUES ({0},{1},-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1 )",
                                 toon.PersistentID, ActiveSkillsList[0]);
                var insCmd = new SQLiteCommand(insQuery, DBManager.Connection);
                insCmd.ExecuteNonQuery();
            }
            else
            {
                this.ActiveSkills = new ActiveSkillSavedData[6] {
                    new ActiveSkillSavedData {  snoSkill = Convert.ToInt32(reader["skill_0"]), 
                                                snoRune  = Convert.ToInt32(reader["rune_0"]) },
                    new ActiveSkillSavedData {  snoSkill = Convert.ToInt32(reader["skill_1"]), 
                                                snoRune  = Convert.ToInt32(reader["rune_1"]) },
                    new ActiveSkillSavedData {  snoSkill = Convert.ToInt32(reader["skill_2"]), 
                                                snoRune  = Convert.ToInt32(reader["rune_2"]) },
                    new ActiveSkillSavedData {  snoSkill = Convert.ToInt32(reader["skill_3"]), 
                                                snoRune  = Convert.ToInt32(reader["rune_3"]) },
                    new ActiveSkillSavedData {  snoSkill = Convert.ToInt32(reader["skill_4"]), 
                                                snoRune  = Convert.ToInt32(reader["rune_4"]) },
                    new ActiveSkillSavedData {  snoSkill = Convert.ToInt32(reader["skill_5"]), 
                                                snoRune  = Convert.ToInt32(reader["rune_5"]) }
                };
                this.PassiveSkills = new int[3] {
                    Convert.ToInt32(reader["passive_0"]),
                    Convert.ToInt32(reader["passive_1"]),
                    Convert.ToInt32(reader["passive_2"])
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
            var query = string.Format("UPDATE active_skills SET skill_{1}={2}, rune_{1}={3} WHERE id_toon={0} ", toon.PersistentID, hotBarIndex, SNOSkill, SNORune);
            var cmd = new SQLiteCommand(query, DBManager.Connection);
            cmd.ExecuteNonQuery();
        }

        public void SwitchUpdateSkills(int oldSNOSkill, int SNOSkill, int SNORune, Toon toon)
		{
			for (int i = 0; i < this.HotBarSkills.Length; i++)
			{
				if(this.HotBarSkills[i].SNOSkill == oldSNOSkill)
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
            var query = string.Format("UPDATE active_skills SET passive_0={1}, passive_1={2}, passive_2={3} WHERE id_toon={0} ", toon.PersistentID, PassiveSkills[0], PassiveSkills[1], PassiveSkills[2]);
            var cmd = new SQLiteCommand(query, DBManager.Connection);
            cmd.ExecuteNonQuery();
        }
    }
}
