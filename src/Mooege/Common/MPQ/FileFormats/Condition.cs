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

using CrystalMpq;
using Gibbed.IO;
using Mooege.Common.MPQ.FileFormats.Types;
using Mooege.Core.GS.Common.Types.SNO;

namespace Mooege.Common.MPQ.FileFormats
{
    [FileFormat(SNOGroup.Condition)]
    public class Condition : FileFormat
    {
        public Header Header { get; private set; }
        public int I0 { get; private set; }
        public int I1 { get; private set; }
        public int[] Class { get; private set; }
        public int[] Difficulty { get; private set; }
        public LoreSubcondition[] LoreCondition { get; private set; }
        public QuestSubcondition[] QuestCondition { get; private set; }
        public ItemSubcondition[] ItemCondition { get; private set; }
        public int I4 { get; private set; }
        public int I5 { get; private set; }
        public int I6 { get; private set; }
        public int I7 { get; private set; }
        public int I8 { get; private set; }
        public int I9 { get; private set; }
        public int SNOCurrentWorld { get; private set; }
        public int SNOCurrentLevelArea { get; private set; }
        public int SNOQuestRange { get; private set; }
        public FollowerSubcondition FollowerCondition { get; private set; }
        public LabelSubcondition[] LabelCondition { get; private set; }
        public SkillSubcondition[] SkillCondition { get; private set; }
        public MonsterSubcondition[] MonsterCondition { get; private set; }
        public GameFlagSubcondition[] GameFlagCondition { get; private set; }
        public PlayerFlagSubcondition[] PlayerFlagCondition { get; private set; }

        public Condition(MpqFile file)
        {
            var stream = file.Open();
            this.Header = new Header(stream);
            this.I0 = stream.ReadValueS32();
            this.I1 = stream.ReadValueS32();
            this.Class = new int[5];
            for (int i = 0; i < 5; i++)
                this.Class[i] = stream.ReadValueS32();
            this.Difficulty = new int[4];
            for (int i = 0; i < 4; i++)
                this.Difficulty[i] = stream.ReadValueS32();
            this.LoreCondition = new LoreSubcondition[3];
            for (int i = 0; i < 3; i++)
                this.LoreCondition[i] = new LoreSubcondition(stream);
            this.QuestCondition = new QuestSubcondition[3];
            for (int i = 0; i < 3; i++)
                this.QuestCondition[i] = new QuestSubcondition(stream);
            this.ItemCondition = new ItemSubcondition[3];
            for (int i = 0; i < 3; i++)
                this.ItemCondition[i] = new ItemSubcondition(stream);
            this.I4 = stream.ReadValueS32(); //176
            this.I5 = stream.ReadValueS32();
            stream.Position += 4;
            this.I6 = stream.ReadValueS32();
            this.I7 = stream.ReadValueS32();
            this.I8 = stream.ReadValueS32();
            this.I9 = stream.ReadValueS32();
            this.SNOCurrentWorld = stream.ReadValueS32();
            this.SNOCurrentLevelArea = stream.ReadValueS32();
            this.SNOQuestRange = stream.ReadValueS32();
            this.FollowerCondition = new FollowerSubcondition(stream);
            this.LabelCondition = new LabelSubcondition[3];
            for (int i = 0; i < 3; i++)
                this.LabelCondition[i] = new LabelSubcondition(stream);
            this.SkillCondition = new SkillSubcondition[3];
            for (int i = 0; i < 3; i++)
                this.SkillCondition[i] = new SkillSubcondition(stream);
            this.MonsterCondition = new MonsterSubcondition[3];
            for (int i = 0; i < 3; i++)
                this.MonsterCondition[i] = new MonsterSubcondition(stream);
            this.GameFlagCondition = new GameFlagSubcondition[3];
            for (int i = 0; i < 3; i++)
                this.GameFlagCondition[i] = new GameFlagSubcondition(stream);
            this.PlayerFlagCondition = new PlayerFlagSubcondition[3];
            for (int i = 0; i < 3; i++)
                this.PlayerFlagCondition[i] = new PlayerFlagSubcondition(stream);
            stream.Close();
        }
    }

    public class LoreSubcondition
    {
        public int SNOLore { get; private set; }
        public int I0 { get; private set; }

        public LoreSubcondition(MpqFileStream stream)
        {
            this.SNOLore = stream.ReadValueS32();
            this.I0 = stream.ReadValueS32();
        }
    }

    public class QuestSubcondition
    {
        public int SNOQuest { get; private set; }
        public int I0 { get; private set; }
        public int I1 { get; private set; }
        public int I2 { get; private set; }

        public QuestSubcondition(MpqFileStream stream)
        {
            this.SNOQuest = stream.ReadValueS32();
            this.I0 = stream.ReadValueS32();
            this.I1 = stream.ReadValueS32();
            this.I2 = stream.ReadValueS32();
        }
    }

    public class ItemSubcondition
    {
        public int ItemGBId { get; private set; }
        public int I0 { get; private set; }
        public int I1 { get; private set; }
        public int I2 { get; private set; }

        public ItemSubcondition(MpqFileStream stream)
        {
            this.ItemGBId = stream.ReadValueS32();
            this.I0 = stream.ReadValueS32();
            this.I1 = stream.ReadValueS32();
            this.I2 = stream.ReadValueS32();
        }
    }

    public class FollowerSubcondition
    {
        public FollowerType Type { get; private set; }
        public int I0 { get; private set; }

        public FollowerSubcondition(MpqFileStream stream)
        {
            this.Type = (FollowerType)stream.ReadValueS32();
            this.I0 = stream.ReadValueS32();
        }
    }

    public class LabelSubcondition
    {
        public int LabelGBId { get; private set; }
        public int I0 { get; private set; }

        public LabelSubcondition(MpqFileStream stream)
        {
            this.LabelGBId = stream.ReadValueS32();
            this.I0 = stream.ReadValueS32();
        }
    }

    public class SkillSubcondition
    {
        public int SNOSkillPower { get; private set; }
        public int I0 { get; private set; }
        public int I1 { get; private set; }

        public SkillSubcondition(MpqFileStream stream)
        {
            this.SNOSkillPower = stream.ReadValueS32();
            this.I0 = stream.ReadValueS32();
            this.I1 = stream.ReadValueS32();
        }
    }

    public class MonsterSubcondition
    {
        public int SNOMonsterActor { get; private set; }

        public MonsterSubcondition(MpqFileStream stream)
        {
            this.SNOMonsterActor = stream.ReadValueS32();
        }
    }

    public class GameFlagSubcondition
    {
        public string S0 { get; private set; }

        public GameFlagSubcondition(MpqFileStream stream)
        {
            this.S0 = stream.ReadString(128, true);
        }
    }

    public class PlayerFlagSubcondition
    {
        public string S0 { get; private set; }

        public PlayerFlagSubcondition(MpqFileStream stream)
        {
            this.S0 = stream.ReadString(128, true);
        }
    }

    public enum FollowerType
    {
        None = 0,
        Templar,
        Scoundrel,
        Enchantress,
    }
}