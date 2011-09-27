using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using D3Sharp.Net.Game.Message.Fields;
using D3Sharp.Net.Game;
using D3Sharp.Net.Game.Message.Definitions.Hero;
using D3Sharp.Net.Game.Message.Definitions.Misc;

namespace D3Sharp.Core.Skills
{
    public class Skillset
    {
        public int[] activeSkills;
        public HotbarButtonData[] hotbarSkills;
        public int[] passiveSkills;

        public Skillset()
        {
            // initialize to current convience skills
            activeSkills = new int[6] {
                (int)Skills.BasicAttack,
                (int)Skills.Wizard.Disintegrate,
                (int)Skills.Monk.BlindingFlash,
                (int)Skills.Wizard.Meteor,
                (int)Skills.Monk.SevenSidedStrike,
                (int)Skills.Wizard.MagicMissile
            };
            hotbarSkills = new HotbarButtonData[9] {
                new HotbarButtonData()
                {
                    m_snoPower = activeSkills[0],
                    m_gbidItem = -1
                },
                new HotbarButtonData()
                {
                    m_snoPower = activeSkills[1],
                    m_gbidItem = -1
                },
                new HotbarButtonData()
                {
                    m_snoPower = (int)Skills.None,
                    m_gbidItem = -1
                },
                new HotbarButtonData()
                {
                    m_snoPower = (int)Skills.None,
                    m_gbidItem = -1
                },
                new HotbarButtonData()
                {
                    m_snoPower = activeSkills[2],
                    m_gbidItem = -1
                },
                new HotbarButtonData()
                {
                    m_snoPower = activeSkills[3],
                    m_gbidItem = -1
                },
                new HotbarButtonData()
                {
                    m_snoPower = activeSkills[4],
                    m_gbidItem = -1
                },
                new HotbarButtonData()
                {
                    m_snoPower = activeSkills[5],
                    m_gbidItem = -1
                },
                new HotbarButtonData()
                {
                    m_snoPower = (int)Skills.None,
                    m_gbidItem = 0x622256D4 // potion
                }
            };
            passiveSkills = new int[3] { -1, -1, -1 };
        }

        public void UpdateClient(GameClient client)
        {
            client.SendMessage(new HeroStateMessage()
            {
                Id = 0x003A,
                Field0 = new HeroStateData()
                {
                    Field0 = 0x00000000,
                    Field1 = 0x00000000,
                    Field2 = 0x00000000,
                    Field3 = client.BnetClient.CurrentToon.Gender,
                    Field4 = new PlayerSavedData()
                    {
                        Field0 = hotbarSkills,
                        Field1 = new SkillKeyMapping[15]
                        {
                             new SkillKeyMapping()
                             {
                                Power = -1,
                                Field1 = -1,
                                Field2 = 0x00000000,
                             },
                             new SkillKeyMapping()
                             {
                                Power = -1,
                                Field1 = -1,
                                Field2 = 0x00000000,
                             },
                             new SkillKeyMapping()
                             {
                                Power = -1,
                                Field1 = -1,
                                Field2 = 0x00000000,
                             },
                             new SkillKeyMapping()
                             {
                                Power = -1,
                                Field1 = -1,
                                Field2 = 0x00000000,
                             },
                             new SkillKeyMapping()
                             {
                                Power = -1,
                                Field1 = -1,
                                Field2 = 0x00000000,
                             },
                             new SkillKeyMapping()
                             {
                                Power = -1,
                                Field1 = -1,
                                Field2 = 0x00000000,
                             },
                             new SkillKeyMapping()
                             {
                                Power = -1,
                                Field1 = -1,
                                Field2 = 0x00000000,
                             },
                             new SkillKeyMapping()
                             {
                                Power = -1,
                                Field1 = -1,
                                Field2 = 0x00000000,
                             },
                             new SkillKeyMapping()
                             {
                                Power = -1,
                                Field1 = -1,
                                Field2 = 0x00000000,
                             },
                             new SkillKeyMapping()
                             {
                                Power = -1,
                                Field1 = -1,
                                Field2 = 0x00000000,
                             },
                             new SkillKeyMapping()
                             {
                                Power = -1,
                                Field1 = -1,
                                Field2 = 0x00000000,
                             },
                             new SkillKeyMapping()
                             {
                                Power = -1,
                                Field1 = -1,
                                Field2 = 0x00000000,
                             },
                             new SkillKeyMapping()
                             {
                                Power = -1,
                                Field1 = -1,
                                Field2 = 0x00000000,
                             },
                             new SkillKeyMapping()
                             {
                                Power = -1,
                                Field1 = -1,
                                Field2 = 0x00000000,
                             },
                             new SkillKeyMapping()
                             {
                                Power = -1,
                                Field1 = -1,
                                Field2 = 0x00000000,
                             },
                        },
                        Field2 = 0x00000000,
                        Field3 = 0x00000001,
                        Field4 = new HirelingSavedData()
                        {
                            Field0 = new HirelingInfo[4]
                            {
                                 new HirelingInfo()
                                 {
                                    Field0 = 0x00000000,
                                    Field1 = -1,
                                    Field2 = 0x00000000,
                                    Field3 = 0x00000000,
                                    Field4 = false,
                                    Field5 = -1,
                                    Field6 = -1,
                                    Field7 = -1,
                                    Field8 = -1,
                                 },
                                 new HirelingInfo()
                                 {
                                    Field0 = 0x00000000,
                                    Field1 = -1,
                                    Field2 = 0x00000000,
                                    Field3 = 0x00000000,
                                    Field4 = false,
                                    Field5 = -1,
                                    Field6 = -1,
                                    Field7 = -1,
                                    Field8 = -1,
                                 },
                                 new HirelingInfo()
                                 {
                                    Field0 = 0x00000000,
                                    Field1 = -1,
                                    Field2 = 0x00000000,
                                    Field3 = 0x00000000,
                                    Field4 = false,
                                    Field5 = -1,
                                    Field6 = -1,
                                    Field7 = -1,
                                    Field8 = -1,
                                 },
                                 new HirelingInfo()
                                 {
                                    Field0 = 0x00000000,
                                    Field1 = -1,
                                    Field2 = 0x00000000,
                                    Field3 = 0x00000000,
                                    Field4 = false,
                                    Field5 = -1,
                                    Field6 = -1,
                                    Field7 = -1,
                                    Field8 = -1,
                                 },
                            },
                            Field1 = 0x00000000,
                            Field2 = 0x00000000,
                        },
                        Field5 = 0x00000000,
                        Field6 = new LearnedLore()
                        {
                            Field0 = 0x00000000,
                            m_snoLoreLearned = new int[256]
                            {
                                0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 
                                0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 
                                0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 
                                0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 
                                0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 
                                0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 
                                0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 
                                0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 
                                0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 
                                0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 
                                0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 
                                0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 
                                0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 
                                0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 
                                0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 
                                0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 
                                0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 
                                0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 
                                0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 
                                0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 
                                0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 
                                0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 
                                0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 
                                0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 
                                0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 
                                0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 
                                0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 
                                0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 
                                0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 
                                0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 
                                0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 
                                0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 
                            },
                        },
                        snoActiveSkills = activeSkills,
                        snoTraits = passiveSkills,
                        Field9 = new SavePointData()
                        {
                            snoWorld = -1,
                            Field1 = -1,
                        },
                        m_SeenTutorials = new int[64]
                        {
                            -1, -1, -1, -1, -1, -1, -1, -1, 
                            -1, -1, -1, -1, -1, -1, -1, -1, 
                            -1, -1, -1, -1, -1, -1, -1, -1, 
                            -1, -1, -1, -1, -1, -1, -1, -1, 
                            -1, -1, -1, -1, -1, -1, -1, -1, 
                            -1, -1, -1, -1, -1, -1, -1, -1, 
                            -1, -1, -1, -1, -1, -1, -1, -1, 
                            -1, -1, -1, -1, -1, -1, -1, -1, 
                        },
                    },
                    Field5 = 0x00000000,
                    tQuestRewardHistory = new PlayerQuestRewardHistoryEntry[0]
                                {
                                },
                },
            });

            client.packetId += 10 * 2;
            client.SendMessage(new DWordDataMessage()
            {
                Id = 0x89,
                Field0 = client.packetId,
            });
        }

        public void AssignActiveSkill(int slot, int code)
        {
            foreach (HotbarButtonData button in hotbarSkills)
            {
                if (button.m_snoPower == activeSkills[slot])
                    button.m_snoPower = code;
            }

            activeSkills[slot] = code;
        }

        public void AssignPassiveSkill(int slot, int code)
        {
            passiveSkills[slot] = code;
        }

        public void AssignHotbarButton(int slot, HotbarButtonData hotbarButtonData)
        {
            hotbarSkills[slot] = hotbarButtonData;
        }
    }
}
