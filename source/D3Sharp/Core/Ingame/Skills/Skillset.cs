using D3Sharp.Net.Game.Message.Fields;
using D3Sharp.Net.Game;
using D3Sharp.Net.Game.Message.Definitions.Hero;
using D3Sharp.Net.Game.Message.Definitions.Misc;
using D3Sharp.Net.Game.Message;
using D3Sharp.Net.Game.Message.Definitions.Player;
using D3Sharp.Net.Game.Message.Definitions.Skill;

namespace D3Sharp.Core.Ingame.Skills
{
    public class Skillset : IMessageConsumer
    {
        public int[] activeSkills;
        public HotbarButtonData[] hotbarSkills;
        public int[] passiveSkills;

        public Skillset()
        {
            // initialize to current convience skills
            activeSkills = new int[6] {
                (int)Skills.BasicAttack,
                (int)Skills.Wizard.ExplosiveBlast,
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

        public void Consume(GameClient client, GameMessage message)
        {
            if (message is AssignActiveSkillMessage) OnAssignActiveSkill(client, (AssignActiveSkillMessage)message);
            else if (message is AssignPassiveSkillMessage) OnAssignPassiveSkill(client, (AssignPassiveSkillMessage)message);
            else if (message is PlayerChangeHotbarButtonMessage) OnPlayerChangeHotbarButtonMessage(client, (PlayerChangeHotbarButtonMessage)message);
            else return;

            UpdateClient(client);
            client.FlushOutgoingBuffer();
        }

        private void OnPlayerChangeHotbarButtonMessage(GameClient client, PlayerChangeHotbarButtonMessage message)
        {
            hotbarSkills[message.Field0] = message.Field1;
        }

        private void OnAssignPassiveSkill(GameClient client, AssignPassiveSkillMessage message)
        {
            passiveSkills[message.Field1] = message.snoPower;
        }

        private void OnAssignActiveSkill(GameClient client, AssignActiveSkillMessage message)
        {
            foreach (HotbarButtonData button in hotbarSkills)
            {
                if (button.m_snoPower == activeSkills[message.Field1])
                    button.m_snoPower = message.snoPower;
            }

            activeSkills[message.Field1] = message.snoPower;
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
                    Gender = client.BnetClient.CurrentToon.Gender,
                    PlayerSavedData = new PlayerSavedData()
                    {
                        HotBarButtons = hotbarSkills,
                        SkilKeyMappings = new SkillKeyMapping[15]
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
                            HirelingInfos = new HirelingInfo[4]
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
                        LearnedLore = new LearnedLore()
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

            client.PacketId += 10 * 2;
            client.SendMessage(new DWordDataMessage()
            {
                Id = 0x89,
                Field0 = client.PacketId,
            });
        }
    }
}
