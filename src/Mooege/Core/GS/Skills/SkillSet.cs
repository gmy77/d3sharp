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

using System.Linq;
using Mooege.Core.MooNet.Toons;
using Mooege.Net.GS.Message.Fields;

namespace Mooege.Core.GS.Skills
{
    public class SkillSet
    {
        public ToonClass @Class;

        public int[] ActiveSkillsList;
        public ActiveSkillSavedData[] ActiveSkills;
        public HotbarButtonData[] HotBarSkills;
        public int[] PassiveSkills;

        public SkillSet(ToonClass @class)
        {
            this.@Class = @class;

            this.ActiveSkillsList = Skills.GetAllActiveSkillsByClass(this.@Class).Take(6).ToArray();
            ActiveSkills = new ActiveSkillSavedData[6] {
                new ActiveSkillSavedData { snoSkill = ActiveSkillsList[0] },
                new ActiveSkillSavedData { snoSkill = ActiveSkillsList[1] },
                new ActiveSkillSavedData { snoSkill = ActiveSkillsList[2] },
                new ActiveSkillSavedData { snoSkill = ActiveSkillsList[3] },
                new ActiveSkillSavedData { snoSkill = ActiveSkillsList[4] },
                new ActiveSkillSavedData { snoSkill = ActiveSkillsList[5] }
            };

            this.HotBarSkills = new HotbarButtonData[6] {
                new HotbarButtonData { SNOSkill = ActiveSkills[4].snoSkill, ItemGBId = -1 }, // left-click
                new HotbarButtonData { SNOSkill = ActiveSkills[5].snoSkill, ItemGBId = -1 }, // right-click
                new HotbarButtonData { SNOSkill = ActiveSkills[0].snoSkill, ItemGBId = -1 }, // hidden-bar - left-click switch - which key??
                new HotbarButtonData { SNOSkill = ActiveSkills[1].snoSkill, ItemGBId = -1 }, // hidden-bar - right-click switch (press x ingame)
                new HotbarButtonData { SNOSkill = ActiveSkills[0].snoSkill, ItemGBId = -1 }, // bar-1
                new HotbarButtonData { SNOSkill = ActiveSkills[1].snoSkill, ItemGBId = -1 }, // bar-2
                //new HotbarButtonData { SNOSkill = ActiveSkills[2].snoSkill, ItemGBId = -1 }, // bar-3
                //new HotbarButtonData { SNOSkill = ActiveSkills[3].snoSkill, ItemGBId = -1 }, // bar-4 
                //new HotbarButtonData { SNOSkill = Skills.None, ItemGBId = 0x622256D4 } // bar-5 - potion
            };

            this.PassiveSkills = new int[3] { -1, -1, -1 }; // setting passive skills here crashes the client, need to figure out the reason. /raist.
        }
    }
}
