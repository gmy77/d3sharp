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

            var query = string.Format("SELECT * from active_skills WHERE id_toon={0}", toon.D3EntityID.IdLow);
            var cmd = new SQLiteCommand(query, DBManager.Connection);
            var reader = cmd.ExecuteReader();
            if (!reader.HasRows)
            {
                int[] ActiveSkillsList = Skills.GetAllActiveSkillsByClass(this.@Class).Take(1).ToArray(); 

                this.HotBarSkills = new HotbarButtonData[6] {     
                    new HotbarButtonData { SNOSkill = ActiveSkillsList[0], ItemGBId = -1 }, // left-click
                    new HotbarButtonData { SNOSkill = Skills.None, ItemGBId = -1 }, // right-click
                    new HotbarButtonData { SNOSkill = Skills.None, ItemGBId = -1 }, // bar-1
                    new HotbarButtonData { SNOSkill = Skills.None, ItemGBId = -1 }, // bar-2
                    new HotbarButtonData { SNOSkill = Skills.None, ItemGBId = -1 }, // bar-3
                    new HotbarButtonData { SNOSkill = Skills.None, ItemGBId = -1 }, // bar-4 
                };

                this.PassiveSkills = new int[3] { -1, -1, -1 }; // setting passive skills here crashes the client, need to figure out the reason. /raist.

                var insQuery = string.Format("INSERT INTO active_skills (id_toon,"+
                    "skill_0,skill_1,skill_2,skill_3,skill_4,skill_5,"+
                    "rune_0,rune_1,rune_2,rune_3,rune_4,rune_5,"+
                    "passive_0,passive_1,passive_2) VALUES ({0},-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1 )",
                                 toon.D3EntityID.IdLow, ActiveSkillsList[0]);
                var insCmd = new SQLiteCommand(insQuery, DBManager.Connection);
                insCmd.ExecuteNonQuery();
            }
            else
            {
                this.HotBarSkills = new HotbarButtonData[6] {     
                    new HotbarButtonData { SNOSkill = (int)(Int64)reader["skill_0"], ItemGBId = -1 }, // left-click
                    new HotbarButtonData { SNOSkill = (int)(Int64)reader["skill_1"], ItemGBId = -1 }, // right-click
                    new HotbarButtonData { SNOSkill = (int)(Int64)reader["skill_2"], ItemGBId = -1 }, // bar-1
                    new HotbarButtonData { SNOSkill = (int)(Int64)reader["skill_3"], ItemGBId = -1 }, // bar-2
                    new HotbarButtonData { SNOSkill = (int)(Int64)reader["skill_4"], ItemGBId = -1 }, // bar-3
                    new HotbarButtonData { SNOSkill = (int)(Int64)reader["skill_5"], ItemGBId = -1 }, // bar-4 
                };
                this.PassiveSkills = new int[3] {
                    -1,//(int)reader["passive_0"],
                    -1,
                    -1 }; // setting passive skills here crashes the client, need to figure out the reason. /raist.
            }

            this.ActiveSkills = new ActiveSkillSavedData[6] {
                new ActiveSkillSavedData { snoSkill = this.HotBarSkills[0].SNOSkill },
                new ActiveSkillSavedData { snoSkill = this.HotBarSkills[1].SNOSkill },
                new ActiveSkillSavedData { snoSkill = this.HotBarSkills[2].SNOSkill },
                new ActiveSkillSavedData { snoSkill = this.HotBarSkills[3].SNOSkill },
                new ActiveSkillSavedData { snoSkill = this.HotBarSkills[4].SNOSkill },
                new ActiveSkillSavedData { snoSkill = this.HotBarSkills[5].SNOSkill }
            };

        }

        public void UpdateHotbarSkills(int hotBarIndex, int SNOSkill, Toon toon)
        {
            switch (hotBarIndex)
            {
                case 0:
                    Logger.Debug("Update index 0 {0}", SNOSkill);
                    var query_0 = string.Format("UPDATE active_skills SET skill_0={1} WHERE id_toon={0} ", toon.D3EntityID.IdLow, SNOSkill);
                    var cmd_0 = new SQLiteCommand(query_0, DBManager.Connection);
                    cmd_0.ExecuteReader();
                    break;
                case 1:
                    Logger.Debug("Update index 1 {0}", SNOSkill);
                    var query_1 = string.Format("UPDATE active_skills SET skill_1={1} WHERE id_toon={0} ", toon.D3EntityID.IdLow, SNOSkill);
                    var cmd_1 = new SQLiteCommand(query_1, DBManager.Connection);
                    cmd_1.ExecuteReader();
                    break;
                case 2:
                    Logger.Debug("Update index 2 {0}", SNOSkill);
                    var query_2 = string.Format("UPDATE active_skills SET skill_2={1} WHERE id_toon={0} ", toon.D3EntityID.IdLow, SNOSkill);
                    var cmd_2 = new SQLiteCommand(query_2, DBManager.Connection);
                    cmd_2.ExecuteReader();
                    break;
                case 3:
                    Logger.Debug("Update index 3 {0}", SNOSkill);
                    var query_3 = string.Format("UPDATE active_skills SET skill_3={1} WHERE id_toon={0} ", toon.D3EntityID.IdLow, SNOSkill);
                    var cmd_3 = new SQLiteCommand(query_3, DBManager.Connection);
                    cmd_3.ExecuteReader();
                    break;
                case 4:
                    Logger.Debug("Update index 4 {0}", SNOSkill);
                    var query_4 = string.Format("UPDATE active_skills SET skill_4={1} WHERE id_toon={0} ", toon.D3EntityID.IdLow, SNOSkill);
                    var cmd_4 = new SQLiteCommand(query_4, DBManager.Connection);
                    cmd_4.ExecuteReader();
                    break;
                case 5:
                    Logger.Debug("Update index 5 {0}", SNOSkill);
                    var query_5 = string.Format("UPDATE active_skills SET skill_5={1} WHERE id_toon={0} ", toon.D3EntityID.IdLow, SNOSkill);
                    var cmd_5 = new SQLiteCommand(query_5, DBManager.Connection);
                    cmd_5.ExecuteReader();
                    break;
            }
        }
		
		public void SwitchUpdateSkills(int oldSNOSkill,int SNOSkill, Toon toon)
		{
			for (int i = 0; i < this.HotBarSkills.Length; i++)
			{
				if(this.HotBarSkills[i].SNOSkill == oldSNOSkill)
				{
					Logger.Debug("SkillSet: SwitchUpdateSkill Oldskill {0} Newskill {1}", oldSNOSkill, SNOSkill);
					this.HotBarSkills[i].SNOSkill = SNOSkill;
					this.UpdateHotbarSkills(i,SNOSkill,toon);
					return;
				}
			}
		}
    }
}
