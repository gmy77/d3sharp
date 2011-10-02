/*
 * Copyright (C) 2011 mooege project
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

using System.Collections.Generic;
using System.Linq;
using Mooege.Core.Common.Toons;
using Mooege.Core.GS.Actors;
using Mooege.Core.GS.Map;
using Mooege.Core.GS.Skills;
using Mooege.Net.GS;
using Mooege.Net.GS.Message;
using Mooege.Net.GS.Message.Definitions.Hero;
using Mooege.Net.GS.Message.Definitions.Misc;
using Mooege.Net.GS.Message.Definitions.Player;
using Mooege.Net.GS.Message.Definitions.Skill;
using Mooege.Net.GS.Message.Fields;
using Mooege.Core.GS.Ingame.Universe;
using Mooege.Net.GS.Message.Definitions.Inventory;

namespace Mooege.Core.GS.Universe
{
    public class Hero : Actor, IMessageConsumer // should extend actor actually?? /raist
    {
        public Toon Properties { get; private set; }
        public Universe Universe { get; private set; }

        public int CurrentWorldSNO;
        public SkillSet SkillSet;
        public Inventory Inventory;

        public GameClient InGameClient { get; private set; }


        public List<World> RevealedWorlds;
        public List<Scene> RevealedScenes;
        public List<Actor> RevealedActors;

        public Hero(GameClient client, Universe universe, Toon toon)
        {
            this.InGameClient = client;
            this.Universe = universe;
            this.Properties = toon;
            this.CurrentWorldSNO = 0x115EE;
            this.Inventory = new Inventory(this);          

            this.SkillSet = new Skills.SkillSet(this.Properties.Class);

            RevealedWorlds = new List<World>();
            RevealedScenes = new List<Scene>();
            RevealedActors = new List<Actor>();

            // actor values
            this.Id = 0x789E00E2;
            this.SnoId = this.ClassSNO;
            this.Field2 = 0x00000009;
            this.Field3 = 0x00000000;
            this.Scale = ModelScale;
            this.RotationAmount = 0.05940768f;
            this.RotationAxis = new Vector3D(0f, 0f, 0.9982339f);

            //initial world and position
            this.WorldId = 0x772E0000;

            this.Position.X = 3143.75f;
            this.Position.Y = 2828.75f;
            this.Position.Z = 59.075588f;

            //den of evil: this.Position.X = 2526.250000f; this.Position.Y = 2098.750000f; this.Position.Z = -5.381495f;
            //inn: this.Position.X = 2996.250000f; this.Position.Y = 2793.750000f; this.Position.Z = 24.045330f;
            // adrias hut: this.Position.X = 1768.750000f; this.Position.Y = 2921.250000f; this.Position.Z = 20.333143f;        
            // cemetry of forsaken: this.Position.X = 2041.250000f; this.Position.Y = 1778.750000f; this.Position.Z = 0.426203f;
            //defiled crypt level 2: this.WorldId = 2000289804; this.Position.X = 158.750000f; this.Position.Y = 76.250000f; this.Position.Z = 0.100000f;

            this.GBHandle = new GBHandle()
            {
                Field0 = 0x00000007,
                Field1 = this.Properties.ClassID,
            };

            this.Field7 = -1;
            this.Field8 = -1;
            this.Field9 = 0x00000000;
            this.Field10 = 0x0;
        }

        public void Consume(GameClient client, GameMessage message)
        {
            if (message is AssignActiveSkillMessage) OnAssignActiveSkill(client, (AssignActiveSkillMessage)message);
            else if (message is AssignPassiveSkillMessage) OnAssignPassiveSkill(client, (AssignPassiveSkillMessage)message);
            else if (message is PlayerChangeHotbarButtonMessage) OnPlayerChangeHotbarButtonMessage(client, (PlayerChangeHotbarButtonMessage)message);
            else return;

            UpdateState();
            client.FlushOutgoingBuffer();
        }

        private void OnPlayerChangeHotbarButtonMessage(GameClient client, PlayerChangeHotbarButtonMessage message)
        {
            this.SkillSet.HotBarSkills[message.BarIndex] = message.ButtonData;
        }

        private void OnAssignPassiveSkill(GameClient client, AssignPassiveSkillMessage message)
        {
            this.SkillSet.PassiveSkills[message.SkillIndex] = message.SNOSkill;
        }

        private void OnAssignActiveSkill(GameClient client, AssignActiveSkillMessage message)
        {
            var oldSNOSkill = this.SkillSet.ActiveSkills[message.SkillIndex]; // find replaced skills SNO.

            foreach (HotbarButtonData button in this.SkillSet.HotBarSkills.Where(button => button.SNOSkill == oldSNOSkill)) // loop through hotbar and replace the old skill with new one
            {
                button.SNOSkill = message.SNOSkill;
            }

            this.SkillSet.ActiveSkills[message.SkillIndex] = message.SNOSkill;
        }

        public void UpdateState()
        {
            this.InGameClient.SendMessage(new HeroStateMessage
            {
                State = this.GetStateData()
            });

            this.InGameClient.PacketId += 10 * 2;
            this.InGameClient.SendMessage(new DWordDataMessage()
            {
                Id = 0x89,
                Field0 = this.InGameClient.PacketId,
            });
        }

        public HeroStateData GetStateData()
        {
            return new HeroStateData()
            {
                Field0 = 0x00000000,
                Field1 = 0x00000000,
                Field2 = 0x00000000,
                Gender = Properties.Gender,
                PlayerSavedData = this.GetSavedData(),
                Field5 = 0x00000000,
                tQuestRewardHistory = QuestRewardHistory,
            };
        }

        private PlayerSavedData GetSavedData()
        {
            return new PlayerSavedData()
            {
                HotBarButtons = this.SkillSet.HotBarSkills,
                SkilKeyMappings = this.SkillKeyMappings,

                Field2 = 0x00000000,
                Field3 = 0x00000001,

                Field4 = new HirelingSavedData()
                {
                    HirelingInfos = this.HirelingInfo,
                    Field1 = 0x00000000,
                    Field2 = 0x00000000,
                },

                Field5 = 0x00000000,

                LearnedLore = this.LearnedLore,
                snoActiveSkills = this.SkillSet.ActiveSkills,
                snoTraits = this.SkillSet.PassiveSkills,
                Field9 = new SavePointData() { snoWorld = -1, Field1 = -1, },
                m_SeenTutorials = this.SeenTutorials,
            };
        }

        public VisualInventoryMessage GetVisualInventory()
        {
            return new VisualInventoryMessage
            {
                Field0 = 0x789E00E2,
                EquipmentList =
                    new VisualEquipment
                    {
                        Equipments =
                            Properties.Equipment.VisualItemList.Select(
                                equipment =>
                                new VisualItem
                                {
                                    GbId = equipment.Gbid,
                                    Field1 = 0x0,
                                    Field2 = 0x0,
                                    Field3 = -1
                                }).ToArray()
                    }
            };
        }

        public World CurrentWorld
        {
            get { return this.Universe.GetWorld(this.WorldId); }
        }        

        public int ClassSNO
        {
            get
            {
                if (this.Properties.Gender == 0)
                {
                    switch (this.Properties.Class)
                    {
                        case ToonClass.Barbarian:
                            return 0x0CE5;
                        case ToonClass.DemonHunter:
                            return 0x0125C7;
                        case ToonClass.Monk:
                            return 0x1271;
                        case ToonClass.WitchDoctor:
                            return 0x1955;
                        case ToonClass.Wizard:
                            return 0x1990;
                    }
                }
                else
                {
                    switch (this.Properties.Class)
                    {
                        case ToonClass.Barbarian:
                            return 0x0CD5;
                        case ToonClass.DemonHunter:
                            return 0x0123D2;
                        case ToonClass.Monk:
                            return 0x126D;
                        case ToonClass.WitchDoctor:
                            return 0x1951;
                        case ToonClass.Wizard:
                            return 0x197E;
                    }
                }
                return 0x0;
            }
        }

        public float ModelScale
        {
            get
            {
                //dummy values, need confirmation from dump
                switch (this.Properties.Class)
                {
                    case ToonClass.Barbarian:
                        return 1.22f;
                    case ToonClass.DemonHunter:
                        return 1.43f;
                    case ToonClass.Monk:
                        return 1.43f;
                    case ToonClass.WitchDoctor:
                        return 1.43f;
                    case ToonClass.Wizard:
                        return 1.43f;
                }
                return 1.43f;
            }
        }

        public int ResourceID
        {
            get
            {
                switch (this.Properties.Class)
                {
                    case ToonClass.Barbarian:
                        return 0x00000002;
                    case ToonClass.DemonHunter:
                        return 0x00000005;
                    case ToonClass.Monk:
                        return 0x00000003;
                    case ToonClass.WitchDoctor:
                        return 0x00000000;
                    case ToonClass.Wizard:
                        return 0x00000001;
                }
                return 0x00000000;
            }
        }

        public int SkillKit
        {
            get
            {
                switch (this.Properties.Class)
                {
                    case ToonClass.Barbarian:
                        return 0x00008AF4;
                    case ToonClass.DemonHunter:
                        return 0x00008AFC;
                    case ToonClass.Monk:
                        return 0x00008AFA;
                    case ToonClass.WitchDoctor:
                        return 0x00008AFF;
                    case ToonClass.Wizard:
                        return 0x00008B00;
                }
                return 0x00000001;
            }
        }

        public SkillKeyMapping[] SkillKeyMappings = new SkillKeyMapping[15]
                                                        {
                                                            new SkillKeyMapping { Power = -1, Field1 = -1, Field2 = 0x00000000, },
                                                            new SkillKeyMapping { Power = -1, Field1 = -1, Field2 = 0x00000000, },
                                                            new SkillKeyMapping { Power = -1, Field1 = -1, Field2 = 0x00000000, },
                                                            new SkillKeyMapping { Power = -1, Field1 = -1, Field2 = 0x00000000, },
                                                            new SkillKeyMapping { Power = -1, Field1 = -1, Field2 = 0x00000000, },
                                                            new SkillKeyMapping { Power = -1, Field1 = -1, Field2 = 0x00000000, },
                                                            new SkillKeyMapping { Power = -1, Field1 = -1, Field2 = 0x00000000, },
                                                            new SkillKeyMapping { Power = -1, Field1 = -1, Field2 = 0x00000000, },
                                                            new SkillKeyMapping { Power = -1, Field1 = -1, Field2 = 0x00000000, },
                                                            new SkillKeyMapping { Power = -1, Field1 = -1, Field2 = 0x00000000, },
                                                            new SkillKeyMapping { Power = -1, Field1 = -1, Field2 = 0x00000000, },
                                                            new SkillKeyMapping { Power = -1, Field1 = -1, Field2 = 0x00000000, },
                                                            new SkillKeyMapping { Power = -1, Field1 = -1, Field2 = 0x00000000, },
                                                            new SkillKeyMapping { Power = -1, Field1 = -1, Field2 = 0x00000000, },
                                                            new SkillKeyMapping { Power = -1, Field1 = -1, Field2 = 0x00000000, },
                                                        };

        public LearnedLore LearnedLore = new LearnedLore()
        {
            Field0 = 0x00000000,
            m_snoLoreLearned =
                new int[256]
                                                         {
                                                            0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,
                                                            0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,
                                                            0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,
                                                            0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,
                                                            0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,
                                                            0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,
                                                            0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,
                                                            0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,
                                                            0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,
                                                            0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,
                                                            0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,
                                                            0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,
                                                            0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,
                                                            0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,
                                                            0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,
                                                            0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,
                                                            0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,
                                                            0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,
                                                            0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,
                                                            0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,
                                                            0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,
                                                            0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,
                                                            0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,
                                                            0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,
                                                            0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,
                                                            0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,
                                                            0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,
                                                            0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,
                                                            0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,
                                                            0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,
                                                            0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,
                                                            0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000,0x00000000
                                                         },
        };

        public int[] SeenTutorials = new int[64]
                                         {
                                             -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
                                             -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
                                             -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
                                         };

        public PlayerQuestRewardHistoryEntry[] QuestRewardHistory = new PlayerQuestRewardHistoryEntry[0] { };

        public HirelingInfo[] HirelingInfo = new HirelingInfo[4]
                                                 {
                                                      new HirelingInfo { Field0 = 0x00000000, Field1 = -1, Field2 = 0x00000000, Field3 = 0x00000000, Field4 = false, Field5 = -1, Field6 = -1, Field7 = -1, Field8 = -1, },
                                                      new HirelingInfo { Field0 = 0x00000000, Field1 = -1, Field2 = 0x00000000, Field3 = 0x00000000, Field4 = false, Field5 = -1, Field6 = -1, Field7 = -1, Field8 = -1, },
                                                      new HirelingInfo { Field0 = 0x00000000, Field1 = -1, Field2 = 0x00000000, Field3 = 0x00000000, Field4 = false, Field5 = -1, Field6 = -1, Field7 = -1, Field8 = -1, },
                                                      new HirelingInfo { Field0 = 0x00000000, Field1 = -1, Field2 = 0x00000000, Field3 = 0x00000000, Field4 = false, Field5 = -1, Field6 = -1, Field7 = -1, Field8 = -1, },
                                                 };
    }
}