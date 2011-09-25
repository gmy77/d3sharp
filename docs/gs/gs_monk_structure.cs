public void OnMessage(JoinBNetGameMessage msg)
{
    SendMessage(new VersionsMessage()
    {
        Id = 0x000D,
        SNOPackHash = msg.SNOPackHash,
        ProtocolHash = GameMessage.ImplementedProtocolHash,
        Version = "0.3.0.7333",
    });
    FlushOutgoingBuffer();

    SendMessage(new ConnectionEstablishedMessage()
    {
        Id = 0x002E,
        Field0 = 0x00000000,
        Field1 = 0x4BB91A16,
        Field2 = msg.SNOPackHash,
    });
    SendMessage(new GameSetupMessage()
    {
        Id = 0x002F,
        Field0 = 0x00000077,
    });

    SendMessage(new SavePointInfoMessage()
    {
        Id = 0x0045,
        snoLevelArea = -1,
    });

    SendMessage(new HearthPortalInfoMessage()
    {
        Id = 0x0046,
        snoLevelArea = -1,
        Field1 = -1,
    });

    SendMessage(new ActTransitionMessage()
    {
        Id = 0x00A8,
        Field0 = 0x00000000,
        Field1 = true,
    });

    #region Quest
    SendMessage(new QuestUpdateMessage()
    {
        Id = 0x0093,
        snoQuest = 0x00015694,
        snoLevelArea = -1,
        Field2 = -1,
        Field3 = false,
        Field4 = false,
    });

    SendMessage(new QuestMeterMessage()
    {
        Id = 0x0094,
        snoQuest = 0x00015694,
        Field1 = -1,
        Field2 = -1f,
    });

    SendMessage(new QuestUpdateMessage()
    {
        Id = 0x0093,
        snoQuest = 0x0001199F,
        snoLevelArea = -1,
        Field2 = -1,
        Field3 = false,
        Field4 = false,
    });

    SendMessage(new QuestMeterMessage()
    {
        Id = 0x0094,
        snoQuest = 0x0001199F,
        Field1 = -1,
        Field2 = -1f,
    });

    SendMessage(new QuestUpdateMessage()
    {
        Id = 0x0093,
        snoQuest = 0x00011A1D,
        snoLevelArea = -1,
        Field2 = -1,
        Field3 = false,
        Field4 = false,
    });

    SendMessage(new QuestMeterMessage()
    {
        Id = 0x0094,
        snoQuest = 0x00011A1D,
        Field1 = -1,
        Field2 = -1f,
    });

    SendMessage(new QuestUpdateMessage()
    {
        Id = 0x0093,
        snoQuest = 0x0001197D,
        snoLevelArea = -1,
        Field2 = -1,
        Field3 = false,
        Field4 = false,
    });

    SendMessage(new QuestMeterMessage()
    {
        Id = 0x0094,
        snoQuest = 0x0001197D,
        Field1 = -1,
        Field2 = -1f,
    });

    SendMessage(new QuestUpdateMessage()
    {
        Id = 0x0093,
        snoQuest = 0x00011C22,
        snoLevelArea = -1,
        Field2 = -1,
        Field3 = false,
        Field4 = false,
    });

    SendMessage(new QuestMeterMessage()
    {
        Id = 0x0094,
        snoQuest = 0x00011C22,
        Field1 = -1,
        Field2 = -1f,
    });

    SendMessage(new QuestCounterMessage()
    {
        Id = 0x0095,
        snoQuest = 0x00015694,
        snoLevelArea = -1,
        Field2 = -1,
        Field3 = 0x00000000,
        Field4 = 0x00000000,
        Field5 = 0x00000000,
    });

    SendMessage(new QuestCounterMessage()
    {
        Id = 0x0095,
        snoQuest = 0x0001199F,
        snoLevelArea = -1,
        Field2 = -1,
        Field3 = 0x00000000,
        Field4 = 0x00000000,
        Field5 = 0x00000000,
    });

    SendMessage(new QuestCounterMessage()
    {
        Id = 0x0095,
        snoQuest = 0x0001199F,
        snoLevelArea = -1,
        Field2 = -1,
        Field3 = 0x00000001,
        Field4 = 0x00000000,
        Field5 = 0x00000000,
    });

    SendMessage(new QuestCounterMessage()
    {
        Id = 0x0095,
        snoQuest = 0x00011A1D,
        snoLevelArea = -1,
        Field2 = -1,
        Field3 = 0x00000000,
        Field4 = 0x00000000,
        Field5 = 0x00000000,
    });

    SendMessage(new QuestCounterMessage()
    {
        Id = 0x0095,
        snoQuest = 0x0001197D,
        snoLevelArea = -1,
        Field2 = -1,
        Field3 = 0x00000000,
        Field4 = 0x00000000,
        Field5 = 0x00000000,
    });

    SendMessage(new QuestCounterMessage()
    {
        Id = 0x0095,
        snoQuest = 0x0001197D,
        snoLevelArea = -1,
        Field2 = -1,
        Field3 = 0x00000001,
        Field4 = 0x00000000,
        Field5 = 0x00000000,
    });

    SendMessage(new QuestCounterMessage()
    {
        Id = 0x0095,
        snoQuest = 0x0001197D,
        snoLevelArea = -1,
        Field2 = -1,
        Field3 = 0x00000002,
        Field4 = 0x00000000,
        Field5 = 0x00000000,
    });

    SendMessage(new QuestCounterMessage()
    {
        Id = 0x0095,
        snoQuest = 0x00011C22,
        snoLevelArea = -1,
        Field2 = -1,
        Field3 = 0x00000000,
        Field4 = 0x00000000,
        Field5 = 0x00000000,
    });
    #endregion
    #region AssignSkill
    SendMessage(new AssignSkillMessage()
    {
        Id = 0x0066,
        snoPower = -1,
        Field1 = 0x00000000,
    });

    SendMessage(new AssignSkillMessage()
    {
        Id = 0x0066,
        snoPower = -1,
        Field1 = 0x00000001,
    });

    SendMessage(new AssignSkillMessage()
    {
        Id = 0x0066,
        snoPower = -1,
        Field1 = 0x00000002,
    });

    SendMessage(new AssignSkillMessage()
    {
        Id = 0x0066,
        snoPower = -1,
        Field1 = 0x00000003,
    });

    SendMessage(new AssignSkillMessage()
    {
        Id = 0x0066,
        snoPower = -1,
        Field1 = 0x00000004,
    });

    SendMessage(new AssignSkillMessage()
    {
        Id = 0x0066,
        snoPower = -1,
        Field1 = 0x00000005,
    });

    SendMessage(new AssignSkillMessage()
    {
        Id = 0x0067,
        snoPower = -1,
        Field1 = 0x00000000,
    });

    SendMessage(new AssignSkillMessage()
    {
        Id = 0x0067,
        snoPower = -1,
        Field1 = 0x00000001,
    });

    SendMessage(new AssignSkillMessage()
    {
        Id = 0x0067,
        snoPower = -1,
        Field1 = 0x00000002,
    });

    SendMessage(new IntDataMessage()
    {
        Id = 0x006B,
        Field0 = 0x00000000,
    });

    SendMessage(new AssignSkillMessage()
    {
        Id = 0x0066,
        snoPower = 0x000176C4,
        Field1 = 0x00000000,
    });

    SendMessage(new IntDataMessage()
    {
        Id = 0x006B,
        Field0 = 0x00000004,
    });

    SendMessage(new AssignSkillMessage()
    {
        Id = 0x0066,
        snoPower = 0x000216FA,
        Field1 = 0x00000001,
    });
    #endregion
    #region NewPlayer
    SendMessage(new NewPlayerMessage()
    {
        Id = 0x0031,
        Field0 = 0x00000000,
        Field1 = "",
        Field2 = "NEWMONK#349",
        Field3 = 0x00000002,
        Field4 = 0x00000004,
        snoActorPortrait = 0x00001271,
        Field6 = 0x00000001,
        #region HeroStateData
        Field7 = new HeroStateData()
        {
            Field0 = 0x00000000,
            Field1 = 0x00000000,
            Field2 = 0x00000000,
            Field3 = 0x00000000,
            Field4 = new PlayerSavedData()
            {
                #region HotBarButtonData
                Field0 = new HotbarButtonData[9]
    {
         new HotbarButtonData()
         {
            m_snoPower = 0x000176C4,
            m_gbidItem = -1,
         },
         new HotbarButtonData()
         {
            m_snoPower = 0x00007780,
            m_gbidItem = -1,
         },
         new HotbarButtonData()
         {
            m_snoPower = -1,
            m_gbidItem = -1,
         },
         new HotbarButtonData()
         {
            m_snoPower = 0x00007780,
            m_gbidItem = -1,
         },
         new HotbarButtonData()
         {
            m_snoPower = 0x000216FA,
            m_gbidItem = -1,
         },
         new HotbarButtonData()
         {
            m_snoPower = -1,
            m_gbidItem = -1,
         },
         new HotbarButtonData()
         {
            m_snoPower = -1,
            m_gbidItem = -1,
         },
         new HotbarButtonData()
         {
            m_snoPower = -1,
            m_gbidItem = -1,
         },
         new HotbarButtonData()
         {
            m_snoPower = -1,
            m_gbidItem = 0x622256D4,
         },
    },
                #endregion
                #region SkillKeyMapping
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
                #endregion
                Field2 = 0x00000000,
                Field3 = 0x00000001,
                #region HirelingSavedData
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
                #endregion
                Field5 = 0x00000000,
                #region LearnedLore
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
                #endregion
                #region snoActiveSkills
                snoActiveSkills = new int[6]
    {
        0x000176C4, 0x000216FA, -1, -1, -1, -1, 
    },
                #endregion
                #region snoTraits
                snoTraits = new int[3]
    {
        -1, -1, -1, 
    },
                #endregion
                #region SavePointData
                Field9 = new SavePointData()
                {
                    snoWorld = -1,
                    Field1 = -1,
                },
                #endregion
                #region SeenTutorials
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
                #endregion
            },
            Field5 = 0x00000000,
            #region PlayerQuestRewardHistoryEntry
            tQuestRewardHistory = new PlayerQuestRewardHistoryEntry[0]
    {
    },
            #endregion
        },
        #endregion
        Field8 = false,
        Field9 = 0x00000001,
        Field10 = 0x789E00E2,
    });
    #endregion
    #region GenericBlobMessages 0x0032,0x00ED,0x00EE,0x00EF
    SendMessage(new GenericBlobMessage()
    {
        Id = 0x0032,
        Data = new byte[22]
    {
    0x08, 0x00, 0x12, 0x12, 0x08, 0x08, 0x10, 0x03, 0x18, 0x04, 0x20, 0x0B, 0x28, 0x14, 0x30, 0x07, 
    0x38, 0x0B, 0x40, 0x04, 0x48, 0x01, 
    },
    });

    SendMessage(new GenericBlobMessage()
    {
        Id = 0x00ED,
        Data = new byte[11]
    {
    0x18, 0x00, 0x21, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
    },
    });

    SendMessage(new GenericBlobMessage()
    {
        Id = 0x00EE,
        Data = new byte[11]
    {
    0x18, 0x00, 0x21, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
    },
    });

    SendMessage(new GenericBlobMessage()
    {
        Id = 0x00EF,
        Data = new byte[11]
    {
    0x18, 0x00, 0x21, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
    },
    });
    #endregion
    #region GameSyncedData
    SendMessage(new GameSyncedDataMessage()
    {
        Id = 0x00AF,
        Field0 = new GameSyncedData()
        {
            Field0 = false,
            Field1 = 0x00000000,
            Field2 = 0x00000000,
            Field3 = 0x00000000,
            Field4 = 0x00000000,
            Field5 = 0x00000000,
            Field6 = new int[2]
    {
    0x00000000, 0x00000000, 
    },
            Field7 = new int[2]
    {
    0x00000000, 0x00000000, 
    },
        },
    });
    #endregion
    #region MapMarkerInfo
    SendMessage(new MapMarkerInfoMessage()
    {
        Id = 0x00E9,
        Field0 = unchecked((int)0xDF83395C),
        Field1 = new WorldPlace()
        {
            Field0 = new Vector3D()
            {
                Field0 = 2106.404f,
                Field1 = 604.0991f,
                Field2 = -4.181701f,
            },
            Field1 = 0x772E0000,
        },
        Field2 = 0x00018FB0,
        m_snoStringList = 0x0000CB2E,
        Field4 = unchecked((int)0xE14E1218),
        Field5 = 0f,
        Field6 = 0f,
        Field7 = 0f,
        Field8 = 0x00000000,
        Field9 = true,
        Field10 = false,
        Field11 = true,
        Field12 = 0x00000000,
    });

    SendMessage(new MapMarkerInfoMessage()
    {
        Id = 0x00E9,
        Field0 = unchecked((int)0xD95EA7CD),
        Field1 = new WorldPlace()
        {
            Field0 = new Vector3D()
            {
                Field0 = 1691.334f,
                Field1 = 2730.091f,
                Field2 = 37.06796f,
            },
            Field1 = 0x772E0000,
        },
        Field2 = 0x00018FB0,
        m_snoStringList = 0x0000CB2E,
        Field4 = 0x4F1F4631,
        Field5 = 0f,
        Field6 = 0f,
        Field7 = 0f,
        Field8 = 0x00000000,
        Field9 = true,
        Field10 = false,
        Field11 = true,
        Field12 = 0x00000000,
    });

    SendMessage(new MapMarkerInfoMessage()
    {
        Id = 0x00E9,
        Field0 = 0x25AC7F8A,
        Field1 = new WorldPlace()
        {
            Field0 = new Vector3D()
            {
                Field0 = 2092.469f,
                Field1 = 2715.238f,
                Field2 = 37.96085f,
            },
            Field1 = 0x772E0000,
        },
        Field2 = 0x00018FB0,
        m_snoStringList = 0x0000CB2E,
        Field4 = 0x4208C1C7,
        Field5 = 0f,
        Field6 = 0f,
        Field7 = 0f,
        Field8 = 0x00000000,
        Field9 = true,
        Field10 = false,
        Field11 = true,
        Field12 = 0x00000000,
    });

    SendMessage(new MapMarkerInfoMessage()
    {
        Id = 0x00E9,
        Field0 = unchecked((int)0xAB7BC18D),
        Field1 = new WorldPlace()
        {
            Field0 = new Vector3D()
            {
                Field0 = 2032.983f,
                Field1 = 1776.411f,
                Field2 = 1.434785f,
            },
            Field1 = 0x772E0000,
        },
        Field2 = 0x00018FB0,
        m_snoStringList = 0x0000CB2E,
        Field4 = unchecked((int)0x911000CE),
        Field5 = 0f,
        Field6 = 0f,
        Field7 = 0f,
        Field8 = 0x00000000,
        Field9 = true,
        Field10 = false,
        Field11 = true,
        Field12 = 0x00000000,
    });

    SendMessage(new MapMarkerInfoMessage()
    {
        Id = 0x00E9,
        Field0 = 0x17E66869,
        Field1 = new WorldPlace()
        {
            Field0 = new Vector3D()
            {
                Field0 = 2164.757f,
                Field1 = 2475.003f,
                Field2 = 30.89584f,
            },
            Field1 = 0x772E0000,
        },
        Field2 = 0x00018FB0,
        m_snoStringList = 0x0000CB2E,
        Field4 = 0x4717C68B,
        Field5 = 0f,
        Field6 = 0f,
        Field7 = 0f,
        Field8 = 0x00000000,
        Field9 = true,
        Field10 = false,
        Field11 = true,
        Field12 = 0x00000000,
    });

    SendMessage(new MapMarkerInfoMessage()
    {
        Id = 0x00E9,
        Field0 = 0x2DFE4150,
        Field1 = new WorldPlace()
        {
            Field0 = new Vector3D()
            {
                Field0 = 2415.898f,
                Field1 = 4024.81f,
                Field2 = -4.088893f,
            },
            Field1 = 0x772E0000,
        },
        Field2 = 0x00018FB0,
        m_snoStringList = 0x0000CB2E,
        Field4 = unchecked((int)0xE14E1218),
        Field5 = 0f,
        Field6 = 0f,
        Field7 = 0f,
        Field8 = 0x00000000,
        Field9 = true,
        Field10 = false,
        Field11 = true,
        Field12 = 0x00000000,
    });

    SendMessage(new MapMarkerInfoMessage()
    {
        Id = 0x00E9,
        Field0 = 0x57C0F73E,
        Field1 = new WorldPlace()
        {
            Field0 = new Vector3D()
            {
                Field0 = 1745.596f,
                Field1 = 353.9031f,
                Field2 = -4.620636f,
            },
            Field1 = 0x772E0000,
        },
        Field2 = 0x00018FB0,
        m_snoStringList = 0x0000CB2E,
        Field4 = unchecked((int)0xE14E1218),
        Field5 = 0f,
        Field6 = 0f,
        Field7 = 0f,
        Field8 = 0x00000000,
        Field9 = true,
        Field10 = false,
        Field11 = true,
        Field12 = 0x00000000,
    });

    SendMessage(new MapMarkerInfoMessage()
    {
        Id = 0x00E9,
        Field0 = unchecked((int)0xAEC5316E),
        Field1 = new WorldPlace()
        {
            Field0 = new Vector3D()
            {
                Field0 = 2254.563f,
                Field1 = 840.8909f,
                Field2 = 0.1090355f,
            },
            Field1 = 0x772E0000,
        },
        Field2 = 0x00018FB0,
        m_snoStringList = 0x0000CB2E,
        Field4 = 0x3FD1BF43,
        Field5 = 0f,
        Field6 = 0f,
        Field7 = 0f,
        Field8 = 0x00000000,
        Field9 = true,
        Field10 = false,
        Field11 = true,
        Field12 = 0x00000000,
    });

    SendMessage(new MapMarkerInfoMessage()
    {
        Id = 0x00E9,
        Field0 = unchecked((int)0xF8B8447D),
        Field1 = new WorldPlace()
        {
            Field0 = new Vector3D()
            {
                Field0 = 427.9307f,
                Field1 = 881.0613f,
                Field2 = 16.65557f,
            },
            Field1 = 0x772E0000,
        },
        Field2 = 0x00018FB0,
        m_snoStringList = 0x0000CB2E,
        Field4 = unchecked((int)0xE14E1218),
        Field5 = 0f,
        Field6 = 0f,
        Field7 = 0f,
        Field8 = 0x00000000,
        Field9 = true,
        Field10 = false,
        Field11 = true,
        Field12 = 0x00000000,
    });

    SendMessage(new MapMarkerInfoMessage()
    {
        Id = 0x00E9,
        Field0 = unchecked((int)0xF52F48D1),
        Field1 = new WorldPlace()
        {
            Field0 = new Vector3D()
            {
                Field0 = 1425.811f,
                Field1 = 2716.384f,
                Field2 = 44.07326f,
            },
            Field1 = 0x772E0000,
        },
        Field2 = 0x00018FB0,
        m_snoStringList = 0x0000CB2E,
        Field4 = unchecked((int)0xE83E7E16),
        Field5 = 0f,
        Field6 = 0f,
        Field7 = 0f,
        Field8 = 0x00000000,
        Field9 = true,
        Field10 = false,
        Field11 = true,
        Field12 = 0x00000000,
    });

    SendMessage(new MapMarkerInfoMessage()
    {
        Id = 0x00E9,
        Field0 = 0x79E0DAF9,
        Field1 = new WorldPlace()
        {
            Field0 = new Vector3D()
            {
                Field0 = 2981.73f,
                Field1 = 2835.009f,
                Field2 = 24.66344f,
            },
            Field1 = 0x772E0000,
        },
        Field2 = 0x0001FA21,
        m_snoStringList = 0x0000F063,
        Field4 = unchecked((int)0x9799F57B),
        Field5 = 0f,
        Field6 = 0f,
        Field7 = 0f,
        Field8 = 0x00000000,
        Field9 = true,
        Field10 = false,
        Field11 = false,
        Field12 = 0x00000000,
    });

    SendMessage(new MapMarkerInfoMessage()
    {
        Id = 0x00E9,
        Field0 = 0x096D643D,
        Field1 = new WorldPlace()
        {
            Field0 = new Vector3D()
            {
                Field0 = 1605.721f,
                Field1 = 3738.172f,
                Field2 = 50.15869f,
            },
            Field1 = 0x772E0000,
        },
        Field2 = 0x00018FB0,
        m_snoStringList = 0x0000CB2E,
        Field4 = unchecked((int)0xE14E1218),
        Field5 = 0f,
        Field6 = 0f,
        Field7 = 0f,
        Field8 = 0x00000000,
        Field9 = true,
        Field10 = false,
        Field11 = true,
        Field12 = 0x00000000,
    });

    SendMessage(new MapMarkerInfoMessage()
    {
        Id = 0x00E9,
        Field0 = 0x04A6FD7C,
        Field1 = new WorldPlace()
        {
            Field0 = new Vector3D()
            {
                Field0 = 2233.954f,
                Field1 = 1793.8f,
                Field2 = 6.619959f,
            },
            Field1 = 0x772E0000,
        },
        Field2 = 0x00018FB0,
        m_snoStringList = 0x0000CB2E,
        Field4 = unchecked((int)0x8ADED0D3),
        Field5 = 0f,
        Field6 = 0f,
        Field7 = 0f,
        Field8 = 0x00000000,
        Field9 = true,
        Field10 = false,
        Field11 = true,
        Field12 = 0x00000000,
    });

    SendMessage(new MapMarkerInfoMessage()
    {
        Id = 0x00E9,
        Field0 = unchecked((int)0x804D7E86),
        Field1 = new WorldPlace()
        {
            Field0 = new Vector3D()
            {
                Field0 = 2195.51f,
                Field1 = 4953.244f,
                Field2 = 21.84594f,
            },
            Field1 = 0x772E0000,
        },
        Field2 = 0x00018FB0,
        m_snoStringList = 0x0000CB2E,
        Field4 = unchecked((int)0xE14E1218),
        Field5 = 0f,
        Field6 = 0f,
        Field7 = 0f,
        Field8 = 0x00000000,
        Field9 = true,
        Field10 = false,
        Field11 = true,
        Field12 = 0x00000000,
    });

    SendMessage(new MapMarkerInfoMessage()
    {
        Id = 0x00E9,
        Field0 = 0x09789C6F,
        Field1 = new WorldPlace()
        {
            Field0 = new Vector3D()
            {
                Field0 = 1079.47f,
                Field1 = 3379.56f,
                Field2 = 66.53082f,
            },
            Field1 = 0x772E0000,
        },
        Field2 = 0x00018FB0,
        m_snoStringList = 0x0000CB2E,
        Field4 = unchecked((int)0xC3352792),
        Field5 = 0f,
        Field6 = 0f,
        Field7 = 0f,
        Field8 = 0x00000000,
        Field9 = true,
        Field10 = false,
        Field11 = true,
        Field12 = 0x00000000,
    });

    SendMessage(new MapMarkerInfoMessage()
    {
        Id = 0x00E9,
        Field0 = unchecked((int)0x92838DF5),
        Field1 = new WorldPlace()
        {
            Field0 = new Vector3D()
            {
                Field0 = 402.9449f,
                Field1 = 665.2502f,
                Field2 = 15.94455f,
            },
            Field1 = 0x772E0000,
        },
        Field2 = 0x00018FB0,
        m_snoStringList = 0x0000CB2E,
        Field4 = unchecked((int)0xE14E1218),
        Field5 = 0f,
        Field6 = 0f,
        Field7 = 0f,
        Field8 = 0x00000000,
        Field9 = true,
        Field10 = false,
        Field11 = true,
        Field12 = 0x00000000,
    });

    SendMessage(new MapMarkerInfoMessage()
    {
        Id = 0x00E9,
        Field0 = 0x5A31647A,
        Field1 = new WorldPlace()
        {
            Field0 = new Vector3D()
            {
                Field0 = 1092.551f,
                Field1 = 4004.261f,
                Field2 = 79.52255f,
            },
            Field1 = 0x772E0000,
        },
        Field2 = 0x00018FB0,
        m_snoStringList = 0x0000CB2E,
        Field4 = unchecked((int)0xE14E1218),
        Field5 = 0f,
        Field6 = 0f,
        Field7 = 0f,
        Field8 = 0x00000000,
        Field9 = true,
        Field10 = false,
        Field11 = true,
        Field12 = 0x00000000,
    });

    SendMessage(new MapMarkerInfoMessage()
    {
        Id = 0x00E9,
        Field0 = 0x24F55785,
        Field1 = new WorldPlace()
        {
            Field0 = new Vector3D()
            {
                Field0 = 2176.305f,
                Field1 = 1939.683f,
                Field2 = -3.581532f,
            },
            Field1 = 0x772E0000,
        },
        Field2 = 0x00018FB0,
        m_snoStringList = 0x0000CB2E,
        Field4 = unchecked((int)0x8ADED0D2),
        Field5 = 0f,
        Field6 = 0f,
        Field7 = 0f,
        Field8 = 0x00000000,
        Field9 = true,
        Field10 = false,
        Field11 = true,
        Field12 = 0x00000000,
    });

    SendMessage(new MapMarkerInfoMessage()
    {
        Id = 0x00E9,
        Field0 = unchecked((int)0x872321FD),
        Field1 = new WorldPlace()
        {
            Field0 = new Vector3D()
            {
                Field0 = 2765.661f,
                Field1 = 1974.691f,
                Field2 = -7.464648f,
            },
            Field1 = 0x772E0000,
        },
        Field2 = 0x00018FB0,
        m_snoStringList = 0x0000CB2E,
        Field4 = unchecked((int)0x86305A9E),
        Field5 = 0f,
        Field6 = 0f,
        Field7 = 0f,
        Field8 = 0x00000000,
        Field9 = true,
        Field10 = false,
        Field11 = true,
        Field12 = 0x00000000,
    });

    SendMessage(new MapMarkerInfoMessage()
    {
        Id = 0x00E9,
        Field0 = 0x7E28F29F,
        Field1 = new WorldPlace()
        {
            Field0 = new Vector3D()
            {
                Field0 = 2284.176f,
                Field1 = 2557.326f,
                Field2 = 27.7173f,
            },
            Field1 = 0x772E0000,
        },
        Field2 = 0x00018FB0,
        m_snoStringList = 0x0000CB2E,
        Field4 = 0x4208C1C6,
        Field5 = 0f,
        Field6 = 0f,
        Field7 = 0f,
        Field8 = 0x00000000,
        Field9 = true,
        Field10 = false,
        Field11 = true,
        Field12 = 0x00000000,
    });

    SendMessage(new MapMarkerInfoMessage()
    {
        Id = 0x00E9,
        Field0 = unchecked((int)0x8B988FCF),
        Field1 = new WorldPlace()
        {
            Field0 = new Vector3D()
            {
                Field0 = 2993.722f,
                Field1 = 2782.086f,
                Field2 = 24.32831f,
            },
            Field1 = 0x772E0000,
        },
        Field2 = 0x00018FB0,
        m_snoStringList = 0x0000CB2E,
        Field4 = unchecked((int)0xF0216008),
        Field5 = 0f,
        Field6 = 0f,
        Field7 = 0f,
        Field8 = 0x00000000,
        Field9 = true,
        Field10 = false,
        Field11 = true,
        Field12 = 0x00000000,
    });
    #endregion
    #region Interstitial,RevealWorld,WorldStatus,EnterWorld
    SendMessage(new InterstitialMessage()
    {
        Id = 0x00A9,
        Field0 = 0x00000004,
        Field1 = true,
    });

    SendMessage(new RevealWorldMessage()
    {
        Id = 0x0037,
        Field0 = 0x772E0000,
        Field1 = 0x000115EE,
    });

    SendMessage(new WorldStatusMessage()
    {
        Id = 0x00B4,
        Field0 = 0x772E0000,
        Field1 = false,
    });

    SendMessage(new EnterWorldMessage()
    {
        Id = 0x0033,
        Field0 = new Vector3D() { Field0 = 3143.75f, Field1 = 2828.75f, Field2 = 59.07559f },
        Field1 = 0x772E0000,
        Field2 = 0x000115EE,
    });
    #endregion
    #region RevealScene/MapRevealScen
    SendMessage(new RevealSceneMessage()
    {
        Id = 0x0034,
        Field0 = 0x772E0000,
        Field1 = new SceneSpecification()
        {
            Field0 = 0x00000000,
            Field1 = new IVector2D()
            {
                Field0 = 0x00000033,
                Field1 = 0x0000002E,
            },
            arSnoLevelAreas = new int[4]
    {
    0x00004DEB, 0x00026186, -1, -1, 
    },
            snoPrevWorld = -1,
            Field4 = 0x00000000,
            snoPrevLevelArea = -1,
            snoNextWorld = -1,
            Field7 = 0x00000000,
            snoNextLevelArea = -1,
            snoMusic = 0x000206F8,
            snoCombatMusic = -1,
            snoAmbient = 0x0002734F,
            snoReverb = 0x000153F6,
            snoWeather = 0x00013220,
            snoPresetWorld = 0x000115EE,
            Field15 = 0x00000002,
            Field16 = 0x00000002,
            Field17 = 0x00000000,
            Field18 = -1,
            tCachedValues = new SceneCachedValues()
            {
                Field0 = 0x0000003F,
                Field1 = 0x00000060,
                Field2 = 0x00000060,
                Field3 = new AABB()
                {
                    Field0 = new Vector3D()
                    {
                        Field0 = 115.9387f,
                        Field1 = 125.2578f,
                        Field2 = 43.82879f,
                    },
                    Field1 = new Vector3D()
                    {
                        Field0 = 124.0615f,
                        Field1 = 131.6655f,
                        Field2 = 57.26923f,
                    },
                },
                Field4 = new AABB()
                {
                    Field0 = new Vector3D()
                    {
                        Field0 = 115.9387f,
                        Field1 = 125.2578f,
                        Field2 = 43.82879f,
                    },
                    Field1 = new Vector3D()
                    {
                        Field0 = 124.0615f,
                        Field1 = 131.6655f,
                        Field2 = 57.26923f,
                    },
                },
                Field5 = new int[4]
    {
        0x00000000, 0x000004C8, 0x00000000, 0x00000000, 
    },
                Field6 = 0x00000009,
            },
        },
        Field2 = 0x77560002,
        snoScene = 0x00008245,
        Field4 = new PRTransform()
        {
            Field0 = new Quaternion()
            {
                Field0 = 1f,
                Field1 = new Vector3D()
                {
                    Field0 = 0f,
                    Field1 = 0f,
                    Field2 = 0f,
                },
            },
            Field1 = new Vector3D()
            {
                Field0 = 3060f,
                Field1 = 2760f,
                Field2 = 0f,
            },
        },
        Field5 = -1,
        snoSceneGroup = -1,
        arAppliedLabels = new int[0]
    {
    },
    });

    SendMessage(new MapRevealSceneMessage()
    {
        Id = 0x0044,
        Field0 = 0x77560002,
        snoScene = 0x00008245,
        Field2 = new PRTransform()
        {
            Field0 = new Quaternion()
            {
                Field0 = 1f,
                Field1 = new Vector3D()
                {
                    Field0 = 0f,
                    Field1 = 0f,
                    Field2 = 0f,
                },
            },
            Field1 = new Vector3D()
            {
                Field0 = 3060f,
                Field1 = 2760f,
                Field2 = 0f,
            },
        },
        Field3 = 0x772E0000,
        Field4 = 0x00000002,
    });

    SendMessage(new RevealSceneMessage()
    {
        Id = 0x0034,
        Field0 = 0x772E0000,
        Field1 = new SceneSpecification()
        {
            Field0 = 0x00000000,
            Field1 = new IVector2D()
            {
                Field0 = 0x0000002B,
                Field1 = 0x0000002E,
            },
            arSnoLevelAreas = new int[4]
    {
    0x000316CE, -1, 0x00004DEB, -1, 
    },
            snoPrevWorld = -1,
            Field4 = 0x00000000,
            snoPrevLevelArea = -1,
            snoNextWorld = -1,
            Field7 = 0x00000000,
            snoNextLevelArea = -1,
            snoMusic = 0x00021AF7,
            snoCombatMusic = -1,
            snoAmbient = 0x0002734F,
            snoReverb = 0x000153F6,
            snoWeather = 0x00013220,
            snoPresetWorld = 0x000115EE,
            Field15 = 0x00000000,
            Field16 = 0x00000000,
            Field17 = 0x00000000,
            Field18 = -1,
            tCachedValues = new SceneCachedValues()
            {
                Field0 = 0x0000003F,
                Field1 = 0x00000060,
                Field2 = 0x00000060,
                Field3 = new AABB()
                {
                    Field0 = new Vector3D()
                    {
                        Field0 = 119.8279f,
                        Field1 = 126.4012f,
                        Field2 = 36.26942f,
                    },
                    Field1 = new Vector3D()
                    {
                        Field0 = 130.6345f,
                        Field1 = 128.5111f,
                        Field2 = 40.50302f,
                    },
                },
                Field4 = new AABB()
                {
                    Field0 = new Vector3D()
                    {
                        Field0 = 122.8899f,
                        Field1 = 126.4012f,
                        Field2 = 36.26942f,
                    },
                    Field1 = new Vector3D()
                    {
                        Field0 = 133.6965f,
                        Field1 = 128.5111f,
                        Field2 = 40.50302f,
                    },
                },
                Field5 = new int[4]
    {
        0x00000315, 0x00000000, 0x0000009B, 0x00000000, 
    },
                Field6 = 0x00000009,
            },
        },
        Field2 = 0x77540000,
        snoScene = 0x00008243,
        Field4 = new PRTransform()
        {
            Field0 = new Quaternion()
            {
                Field0 = 1f,
                Field1 = new Vector3D()
                {
                    Field0 = 0f,
                    Field1 = 0f,
                    Field2 = 0f,
                },
            },
            Field1 = new Vector3D()
            {
                Field0 = 2580f,
                Field1 = 2760f,
                Field2 = 0f,
            },
        },
        Field5 = -1,
        snoSceneGroup = -1,
        arAppliedLabels = new int[0]
    {
    },
    });

    SendMessage(new MapRevealSceneMessage()
    {
        Id = 0x0044,
        Field0 = 0x77540000,
        snoScene = 0x00008243,
        Field2 = new PRTransform()
        {
            Field0 = new Quaternion()
            {
                Field0 = 1f,
                Field1 = new Vector3D()
                {
                    Field0 = 0f,
                    Field1 = 0f,
                    Field2 = 0f,
                },
            },
            Field1 = new Vector3D()
            {
                Field0 = 2580f,
                Field1 = 2760f,
                Field2 = 0f,
            },
        },
        Field3 = 0x772E0000,
        Field4 = 0x00000002,
    });

    SendMessage(new RevealSceneMessage()
    {
        Id = 0x0034,
        Field0 = 0x772E0000,
        Field1 = new SceneSpecification()
        {
            Field0 = 0x00000000,
            Field1 = new IVector2D()
            {
                Field0 = 0x0000002F,
                Field1 = 0x0000002A,
            },
            arSnoLevelAreas = new int[4]
    {
    0x00004DEB, -1, -1, -1, 
    },
            snoPrevWorld = -1,
            Field4 = 0x00000000,
            snoPrevLevelArea = -1,
            snoNextWorld = -1,
            Field7 = 0x00000000,
            snoNextLevelArea = -1,
            snoMusic = 0x000206F8,
            snoCombatMusic = -1,
            snoAmbient = 0x0002734F,
            snoReverb = 0x000153F6,
            snoWeather = 0x00013220,
            snoPresetWorld = 0x000115EE,
            Field15 = 0x00000036,
            Field16 = 0x00000036,
            Field17 = 0x00000000,
            Field18 = 0x00000017,
            tCachedValues = new SceneCachedValues()
            {
                Field0 = 0x0000003F,
                Field1 = 0x00000060,
                Field2 = 0x00000060,
                Field3 = new AABB()
                {
                    Field0 = new Vector3D()
                    {
                        Field0 = 120.0193f,
                        Field1 = 126.4575f,
                        Field2 = 27.89188f,
                    },
                    Field1 = new Vector3D()
                    {
                        Field0 = 128.8389f,
                        Field1 = 133.2742f,
                        Field2 = 41.33233f,
                    },
                },
                Field4 = new AABB()
                {
                    Field0 = new Vector3D()
                    {
                        Field0 = 120.0193f,
                        Field1 = 126.4575f,
                        Field2 = 26.40827f,
                    },
                    Field1 = new Vector3D()
                    {
                        Field0 = 128.8389f,
                        Field1 = 133.2742f,
                        Field2 = 42.81593f,
                    },
                },
                Field5 = new int[4]
    {
        0x0000093A, 0x00000000, 0x00000000, 0x00000000, 
    },
                Field6 = 0x00000009,
            },
        },
        Field2 = 0x778A0036,
        snoScene = 0x0000823E,
        Field4 = new PRTransform()
        {
            Field0 = new Quaternion()
            {
                Field0 = 1f,
                Field1 = new Vector3D()
                {
                    Field0 = 0f,
                    Field1 = 0f,
                    Field2 = 0f,
                },
            },
            Field1 = new Vector3D()
            {
                Field0 = 2820f,
                Field1 = 2520f,
                Field2 = 0f,
            },
        },
        Field5 = -1,
        snoSceneGroup = -1,
        arAppliedLabels = new int[0]
    {
    },
    });

    SendMessage(new MapRevealSceneMessage()
    {
        Id = 0x0044,
        Field0 = 0x778A0036,
        snoScene = 0x0000823E,
        Field2 = new PRTransform()
        {
            Field0 = new Quaternion()
            {
                Field0 = 1f,
                Field1 = new Vector3D()
                {
                    Field0 = 0f,
                    Field1 = 0f,
                    Field2 = 0f,
                },
            },
            Field1 = new Vector3D()
            {
                Field0 = 2820f,
                Field1 = 2520f,
                Field2 = 0f,
            },
        },
        Field3 = 0x772E0000,
        Field4 = 0x00000002,
    });

    SendMessage(new RevealSceneMessage()
    {
        Id = 0x0034,
        Field0 = 0x772E0000,
        Field1 = new SceneSpecification()
        {
            Field0 = 0x00000000,
            Field1 = new IVector2D()
            {
                Field0 = 0x00000033,
                Field1 = 0x0000002A,
            },
            arSnoLevelAreas = new int[4]
    {
    0x00004DEB, -1, -1, -1, 
    },
            snoPrevWorld = -1,
            Field4 = 0x00000000,
            snoPrevLevelArea = -1,
            snoNextWorld = -1,
            Field7 = 0x00000000,
            snoNextLevelArea = -1,
            snoMusic = 0x000206F8,
            snoCombatMusic = -1,
            snoAmbient = 0x0002734F,
            snoReverb = 0x000153F6,
            snoWeather = 0x00013220,
            snoPresetWorld = 0x000115EE,
            Field15 = 0x0000003C,
            Field16 = 0x0000003C,
            Field17 = 0x00000000,
            Field18 = -1,
            tCachedValues = new SceneCachedValues()
            {
                Field0 = 0x0000003F,
                Field1 = 0x00000060,
                Field2 = 0x00000060,
                Field3 = new AABB()
                {
                    Field0 = new Vector3D()
                    {
                        Field0 = 112.136f,
                        Field1 = 122.7885f,
                        Field2 = 19.60409f,
                    },
                    Field1 = new Vector3D()
                    {
                        Field0 = 127.8643f,
                        Field1 = 122.7885f,
                        Field2 = 35.97958f,
                    },
                },
                Field4 = new AABB()
                {
                    Field0 = new Vector3D()
                    {
                        Field0 = 112.136f,
                        Field1 = 122.7885f,
                        Field2 = 19.60409f,
                    },
                    Field1 = new Vector3D()
                    {
                        Field0 = 127.8643f,
                        Field1 = 122.7885f,
                        Field2 = 35.97958f,
                    },
                },
                Field5 = new int[4]
    {
        0x000000CF, 0x00000000, 0x00000000, 0x00000000, 
    },
                Field6 = 0x00000009,
            },
        },
        Field2 = 0x7790003C,
        snoScene = 0x0000823F,
        Field4 = new PRTransform()
        {
            Field0 = new Quaternion()
            {
                Field0 = 1f,
                Field1 = new Vector3D()
                {
                    Field0 = 0f,
                    Field1 = 0f,
                    Field2 = 0f,
                },
            },
            Field1 = new Vector3D()
            {
                Field0 = 3060f,
                Field1 = 2520f,
                Field2 = 0f,
            },
        },
        Field5 = -1,
        snoSceneGroup = -1,
        arAppliedLabels = new int[0]
    {
    },
    });

    SendMessage(new MapRevealSceneMessage()
    {
        Id = 0x0044,
        Field0 = 0x7790003C,
        snoScene = 0x0000823F,
        Field2 = new PRTransform()
        {
            Field0 = new Quaternion()
            {
                Field0 = 1f,
                Field1 = new Vector3D()
                {
                    Field0 = 0f,
                    Field1 = 0f,
                    Field2 = 0f,
                },
            },
            Field1 = new Vector3D()
            {
                Field0 = 3060f,
                Field1 = 2520f,
                Field2 = 0f,
            },
        },
        Field3 = 0x772E0000,
        Field4 = 0x00000002,
    });

    SendMessage(new RevealSceneMessage()
    {
        Id = 0x0034,
        Field0 = 0x772E0000,
        Field1 = new SceneSpecification()
        {
            Field0 = 0x00000000,
            Field1 = new IVector2D()
            {
                Field0 = 0x00000033,
                Field1 = 0x00000026,
            },
            arSnoLevelAreas = new int[4]
    {
    -1, -1, -1, -1, 
    },
            snoPrevWorld = -1,
            Field4 = -1,
            snoPrevLevelArea = -1,
            snoNextWorld = -1,
            Field7 = -1,
            snoNextLevelArea = -1,
            snoMusic = 0x000206F8,
            snoCombatMusic = -1,
            snoAmbient = 0x0002734F,
            snoReverb = 0x000153F6,
            snoWeather = 0x0000C575,
            snoPresetWorld = 0x000115EE,
            Field15 = 0x00000089,
            Field16 = 0x00000089,
            Field17 = 0x00000000,
            Field18 = -1,
            tCachedValues = new SceneCachedValues()
            {
                Field0 = 0x0000003F,
                Field1 = 0x00000060,
                Field2 = 0x00000060,
                Field3 = new AABB()
                {
                    Field0 = new Vector3D()
                    {
                        Field0 = 119.9998f,
                        Field1 = 120.0008f,
                        Field2 = -6.470332f,
                    },
                    Field1 = new Vector3D()
                    {
                        Field0 = 120.0005f,
                        Field1 = 120.0008f,
                        Field2 = 6.970119f,
                    },
                },
                Field4 = new AABB()
                {
                    Field0 = new Vector3D()
                    {
                        Field0 = 119.9998f,
                        Field1 = 120.0008f,
                        Field2 = -6.470333f,
                    },
                    Field1 = new Vector3D()
                    {
                        Field0 = 120.0005f,
                        Field1 = 120.0008f,
                        Field2 = 6.97012f,
                    },
                },
                Field5 = new int[4]
    {
        0x00000000, 0x00000000, 0x00000000, 0x00000000, 
    },
                Field6 = 0x00000001,
            },
        },
        Field2 = 0x77DD0089,
        snoScene = 0x0000823A,
        Field4 = new PRTransform()
        {
            Field0 = new Quaternion()
            {
                Field0 = 1f,
                Field1 = new Vector3D()
                {
                    Field0 = 0f,
                    Field1 = 0f,
                    Field2 = 0f,
                },
            },
            Field1 = new Vector3D()
            {
                Field0 = 3060f,
                Field1 = 2280f,
                Field2 = 0f,
            },
        },
        Field5 = -1,
        snoSceneGroup = -1,
        arAppliedLabels = new int[0]
    {
    },
    });

    SendMessage(new MapRevealSceneMessage()
    {
        Id = 0x0044,
        Field0 = 0x77DD0089,
        snoScene = 0x0000823A,
        Field2 = new PRTransform()
        {
            Field0 = new Quaternion()
            {
                Field0 = 1f,
                Field1 = new Vector3D()
                {
                    Field0 = 0f,
                    Field1 = 0f,
                    Field2 = 0f,
                },
            },
            Field1 = new Vector3D()
            {
                Field0 = 3060f,
                Field1 = 2280f,
                Field2 = 0f,
            },
        },
        Field3 = 0x772E0000,
        Field4 = 0x00000000,
    });
    #endregion

    #region ACDEnterKnown 0x789E00E2 PlayerId??
    SendMessage(new ACDEnterKnownMessage()
    {
        Id = 0x003B,
        Field0 = 0x789E00E2,
        Field1 = 0x00001271,
        Field2 = 0x00000009,
        Field3 = 0x00000000,
        Field4 = new WorldLocationMessageData()
        {
            Field0 = 1.43f,
            Field1 = new PRTransform()
            {
                Field0 = new Quaternion()
                {
                    Field0 = 0.05940768f,
                    Field1 = new Vector3D()
                    {
                        Field0 = 0f,
                        Field1 = 0f,
                        Field2 = 0.9982339f,
                    },
                },
                Field1 = new Vector3D()
                {
                    Field0 = 3143.75f,
                    Field1 = 2828.75f,
                    Field2 = 59.07559f,
                },
            },
            Field2 = 0x772E0000,
        },
        Field5 = null,
        Field6 = new GBHandle()
        {
            Field0 = 0x00000007,
            Field1 = 0x003DAC15,
        },
        Field7 = -1,
        Field8 = -1,
        Field9 = 0x00000000,
        Field10 = 0x00,
    });

    SendMessage(new ACDCollFlagsMessage()
    {
        Id = 0x00A6,
        Field0 = 0x789E00E2,
        Field1 = 0x00000000,
    });

    SendMessage(new AttributesSetValuesMessage()
    {
        Id = 0x004D,
        Field0 = 0x789E00E2,
        atKeyVals = new NetAttributeKeyValue[15]
    {
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x01F8], // SkillKit 
    Int = 0x00008AFA,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x00033C40,
    Attribute = GameAttribute.Attributes[0x01CC], // Buff_Active 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x00007545,
    Attribute = GameAttribute.Attributes[0x0041], // Skill 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x00007545,
    Attribute = GameAttribute.Attributes[0x0042], // Skill_Total 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x00000226,
    Attribute = GameAttribute.Attributes[0x003E], // Resistance_Total 
    Int = 0x00000000,
    Float = 0.5f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x00000226,
    Attribute = GameAttribute.Attributes[0x003C], // Resistance 
    Int = 0x00000000,
    Float = 0.5f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x00D7], // Immobolize 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x00D6], // Untargetable 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x000076B7,
    Attribute = GameAttribute.Attributes[0x0042], // Skill_Total 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x000076B7,
    Attribute = GameAttribute.Attributes[0x0041], // Skill 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x000006DF,
    Attribute = GameAttribute.Attributes[0x0041], // Skill 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x0000CE11,
    Attribute = GameAttribute.Attributes[0x01CC], // Buff_Active 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x01D2], // CantStartDisplayedPowers 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x000216FA,
    Attribute = GameAttribute.Attributes[0x0042], // Skill_Total 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x000176C4,
    Attribute = GameAttribute.Attributes[0x0041], // Skill 
    Int = 0x00000001,
    Float = 0f,
    },
    },
    });

    SendMessage(new AttributesSetValuesMessage()
    {
        Id = 0x004D,
        Field0 = 0x789E00E2,
        atKeyVals = new NetAttributeKeyValue[15]
    {
    new NetAttributeKeyValue()
    {
    Field0 = 0x000216FA,
    Attribute = GameAttribute.Attributes[0x0041], // Skill 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x000176C4,
    Attribute = GameAttribute.Attributes[0x0042], // Skill_Total 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x000006DF,
    Attribute = GameAttribute.Attributes[0x0042], // Skill_Total 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x000000DE,
    Attribute = GameAttribute.Attributes[0x003C], // Resistance 
    Int = 0x00000000,
    Float = 0.5f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x000000DE,
    Attribute = GameAttribute.Attributes[0x003E], // Resistance_Total 
    Int = 0x00000000,
    Float = 0.5f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x00C8], // Get_Hit_Recovery 
    Int = 0x00000000,
    Float = 6f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x00C7], // Get_Hit_Recovery_Per_Level 
    Int = 0x00000000,
    Float = 1f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x00C6], // Get_Hit_Recovery_Base 
    Int = 0x00000000,
    Float = 5f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x00007780,
    Attribute = GameAttribute.Attributes[0x0041], // Skill 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x00C5], // Get_Hit_Max 
    Int = 0x00000000,
    Float = 60f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x00007780,
    Attribute = GameAttribute.Attributes[0x0042], // Skill_Total 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x00C4], // Get_Hit_Max_Per_Level 
    Int = 0x00000000,
    Float = 10f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x00C3], // Get_Hit_Max_Base 
    Int = 0x00000000,
    Float = 50f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x00000000,
    Attribute = GameAttribute.Attributes[0x003E], // Resistance_Total 
    Int = 0x00000000,
    Float = 3.051758E-05f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x00000001,
    Attribute = GameAttribute.Attributes[0x003E], // Resistance_Total 
    Int = 0x00000000,
    Float = 3.051758E-05f,
    },
    },
    });

    SendMessage(new AttributesSetValuesMessage()
    {
        Id = 0x004D,
        Field0 = 0x789E00E2,
        atKeyVals = new NetAttributeKeyValue[15]
    {
    new NetAttributeKeyValue()
    {
    Field0 = 0x00000002,
    Attribute = GameAttribute.Attributes[0x003E], // Resistance_Total 
    Int = 0x00000000,
    Float = 3.051758E-05f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x00000003,
    Attribute = GameAttribute.Attributes[0x003E], // Resistance_Total 
    Int = 0x00000000,
    Float = 3.051758E-05f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x00000004,
    Attribute = GameAttribute.Attributes[0x003E], // Resistance_Total 
    Int = 0x00000000,
    Float = 3.051758E-05f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x00000005,
    Attribute = GameAttribute.Attributes[0x003E], // Resistance_Total 
    Int = 0x00000000,
    Float = 3.051758E-05f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x00000006,
    Attribute = GameAttribute.Attributes[0x003E], // Resistance_Total 
    Int = 0x00000000,
    Float = 3.051758E-05f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x00BE], // Dodge_Rating_Total 
    Int = 0x00000000,
    Float = 3.051758E-05f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x02BA], // IsTrialActor 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x000FFFFF,
    Attribute = GameAttribute.Attributes[0x01B9], // Buff_Visual_Effect 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x00A8], // Crit_Percent_Cap 
    Int = 0x3F400000,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x00000003,
    Attribute = GameAttribute.Attributes[0x005E], // Resource_Cur 
    Int = 0x43480000,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x00000003,
    Attribute = GameAttribute.Attributes[0x005F], // Resource_Max 
    Int = 0x00000000,
    Float = 200f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x00000003,
    Attribute = GameAttribute.Attributes[0x0061], // Resource_Max_Total 
    Int = 0x43480000,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x009D], // Damage_Weapon_Min_Total_All 
    Int = 0x00000000,
    Float = 2f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0099], // Damage_Weapon_Delta_Total_All 
    Int = 0x00000000,
    Float = 1f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x00000003,
    Attribute = GameAttribute.Attributes[0x0068], // Resource_Regen_Total 
    Int = 0x00000000,
    Float = 3.051758E-05f,
    },
    },
    });

    SendMessage(new AttributesSetValuesMessage()
    {
        Id = 0x004D,
        Field0 = 0x789E00E2,
        atKeyVals = new NetAttributeKeyValue[15]
    {
    new NetAttributeKeyValue()
    {
    Field0 = 0x00000003,
    Attribute = GameAttribute.Attributes[0x006B], // Resource_Effective_Max 
    Int = 0x00000000,
    Float = 200f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x000FFFFF,
    Attribute = GameAttribute.Attributes[0x0092], // Damage_Min_Subtotal 
    Int = 0x00000000,
    Float = 3.051758E-05f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x000FFFFF,
    Attribute = GameAttribute.Attributes[0x0091], // Damage_Min_Total 
    Int = 0x00000000,
    Float = 3.051758E-05f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x000FFFFF,
    Attribute = GameAttribute.Attributes[0x0190], // Damage_Weapon_Min_Total_CurrentHand 
    Int = 0x00000000,
    Float = 3.051758E-05f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x018F], // Attacks_Per_Second_Item_CurrentHand 
    Int = 0x00000000,
    Float = 1.199219f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0189], // Attacks_Per_Second_Item_Total_MainHand 
    Int = 0x00000000,
    Float = 1.199219f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0089], // Attacks_Per_Second_Total 
    Int = 0x00000000,
    Float = 1.199219f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0087], // Attacks_Per_Second 
    Int = 0x00000000,
    Float = 1f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0187], // Attacks_Per_Second_Item_MainHand 
    Int = 0x00000000,
    Float = 1.199219f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0086], // Attacks_Per_Second_Item_Total 
    Int = 0x00000000,
    Float = 1.199219f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x00033C40,
    Attribute = GameAttribute.Attributes[0x01BE], // Buff_Icon_End_Tick0 
    Int = 0x000003FB,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0084], // Attacks_Per_Second_Item_Subtotal 
    Int = 0x00000000,
    Float = 3.051758E-05f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0082], // Attacks_Per_Second_Item 
    Int = 0x00000000,
    Float = 3.051758E-05f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x00033C40,
    Attribute = GameAttribute.Attributes[0x01BA], // Buff_Icon_Start_Tick0 
    Int = 0x00000077,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0081], // Hit_Chance 
    Int = 0x00000000,
    Float = 1f,
    },
    },
    });

    SendMessage(new AttributesSetValuesMessage()
    {
        Id = 0x004D,
        Field0 = 0x789E00E2,
        atKeyVals = new NetAttributeKeyValue[15]
    {
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x007F], // Casting_Speed_Total 
    Int = 0x00000000,
    Float = 1f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x007D], // Casting_Speed 
    Int = 0x00000000,
    Float = 1f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x007B], // Movement_Scalar_Total 
    Int = 0x00000000,
    Float = 1f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x0002EC66,
    Attribute = GameAttribute.Attributes[0x0042], // Skill_Total 
    Int = 0x00000000,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0079], // Movement_Scalar_Capped_Total 
    Int = 0x00000000,
    Float = 1f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0078], // Movement_Scalar_Subtotal 
    Int = 0x00000000,
    Float = 1f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0076], // Strafing_Rate_Total 
    Int = 0x00000000,
    Float = 3.051758E-05f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0075], // Sprinting_Rate_Total 
    Int = 0x00000000,
    Float = 3.051758E-05f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0074], // Running_Rate_Total 
    Int = 0x00000000,
    Float = 0.3598633f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x00000000,
    Attribute = GameAttribute.Attributes[0x018B], // Damage_Weapon_Min_Total_MainHand 
    Int = 0x00000000,
    Float = 2f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0073], // Walking_Rate_Total 
    Int = 0x00000000,
    Float = 0.2797852f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x00000000,
    Attribute = GameAttribute.Attributes[0x018D], // Damage_Weapon_Delta_Total_MainHand 
    Int = 0x00000000,
    Float = 1f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x00000001,
    Attribute = GameAttribute.Attributes[0x008E], // Damage_Delta_Total 
    Int = 0x00000000,
    Float = 3.051758E-05f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x00000002,
    Attribute = GameAttribute.Attributes[0x008E], // Damage_Delta_Total 
    Int = 0x00000000,
    Float = 3.051758E-05f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x00000003,
    Attribute = GameAttribute.Attributes[0x008E], // Damage_Delta_Total 
    Int = 0x00000000,
    Float = 3.051758E-05f,
    },
    },
    });

    SendMessage(new AttributesSetValuesMessage()
    {
        Id = 0x004D,
        Field0 = 0x789E00E2,
        atKeyVals = new NetAttributeKeyValue[15]
    {
    new NetAttributeKeyValue()
    {
    Field0 = 0x00000004,
    Attribute = GameAttribute.Attributes[0x008E], // Damage_Delta_Total 
    Int = 0x00000000,
    Float = 3.051758E-05f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x00000005,
    Attribute = GameAttribute.Attributes[0x008E], // Damage_Delta_Total 
    Int = 0x00000000,
    Float = 3.051758E-05f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x00000006,
    Attribute = GameAttribute.Attributes[0x008E], // Damage_Delta_Total 
    Int = 0x00000000,
    Float = 3.051758E-05f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x00000000,
    Attribute = GameAttribute.Attributes[0x008E], // Damage_Delta_Total 
    Int = 0x00000000,
    Float = 1f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0070], // Running_Rate 
    Int = 0x00000000,
    Float = 0.3598633f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x00000001,
    Attribute = GameAttribute.Attributes[0x0190], // Damage_Weapon_Min_Total_CurrentHand 
    Int = 0x00000000,
    Float = 3.051758E-05f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x00000002,
    Attribute = GameAttribute.Attributes[0x0190], // Damage_Weapon_Min_Total_CurrentHand 
    Int = 0x00000000,
    Float = 3.051758E-05f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x00000003,
    Attribute = GameAttribute.Attributes[0x0190], // Damage_Weapon_Min_Total_CurrentHand 
    Int = 0x00000000,
    Float = 3.051758E-05f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x00000004,
    Attribute = GameAttribute.Attributes[0x0190], // Damage_Weapon_Min_Total_CurrentHand 
    Int = 0x00000000,
    Float = 3.051758E-05f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x00000005,
    Attribute = GameAttribute.Attributes[0x0190], // Damage_Weapon_Min_Total_CurrentHand 
    Int = 0x00000000,
    Float = 3.051758E-05f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x00000006,
    Attribute = GameAttribute.Attributes[0x0190], // Damage_Weapon_Min_Total_CurrentHand 
    Int = 0x00000000,
    Float = 3.051758E-05f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x00000000,
    Attribute = GameAttribute.Attributes[0x0190], // Damage_Weapon_Min_Total_CurrentHand 
    Int = 0x00000000,
    Float = 2f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x006F], // Walking_Rate 
    Int = 0x00000000,
    Float = 0.2797852f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x00000001,
    Attribute = GameAttribute.Attributes[0x0091], // Damage_Min_Total 
    Int = 0x00000000,
    Float = 3.051758E-05f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x00000002,
    Attribute = GameAttribute.Attributes[0x0091], // Damage_Min_Total 
    Int = 0x00000000,
    Float = 3.051758E-05f,
    },
    },
    });

    SendMessage(new AttributesSetValuesMessage()
    {
        Id = 0x004D,
        Field0 = 0x789E00E2,
        atKeyVals = new NetAttributeKeyValue[15]
    {
    new NetAttributeKeyValue()
    {
    Field0 = 0x00000003,
    Attribute = GameAttribute.Attributes[0x0091], // Damage_Min_Total 
    Int = 0x00000000,
    Float = 3.051758E-05f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x00000004,
    Attribute = GameAttribute.Attributes[0x0091], // Damage_Min_Total 
    Int = 0x00000000,
    Float = 3.051758E-05f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x00000005,
    Attribute = GameAttribute.Attributes[0x0091], // Damage_Min_Total 
    Int = 0x00000000,
    Float = 3.051758E-05f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x00000006,
    Attribute = GameAttribute.Attributes[0x0091], // Damage_Min_Total 
    Int = 0x00000000,
    Float = 3.051758E-05f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x00000001,
    Attribute = GameAttribute.Attributes[0x0191], // Damage_Weapon_Delta_Total_CurrentHand 
    Int = 0x00000000,
    Float = 3.051758E-05f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x00000002,
    Attribute = GameAttribute.Attributes[0x0191], // Damage_Weapon_Delta_Total_CurrentHand 
    Int = 0x00000000,
    Float = 3.051758E-05f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x00000003,
    Attribute = GameAttribute.Attributes[0x0191], // Damage_Weapon_Delta_Total_CurrentHand 
    Int = 0x00000000,
    Float = 3.051758E-05f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x00000004,
    Attribute = GameAttribute.Attributes[0x0191], // Damage_Weapon_Delta_Total_CurrentHand 
    Int = 0x00000000,
    Float = 3.051758E-05f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x00000005,
    Attribute = GameAttribute.Attributes[0x0191], // Damage_Weapon_Delta_Total_CurrentHand 
    Int = 0x00000000,
    Float = 3.051758E-05f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x00000006,
    Attribute = GameAttribute.Attributes[0x0191], // Damage_Weapon_Delta_Total_CurrentHand 
    Int = 0x00000000,
    Float = 3.051758E-05f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x00000000,
    Attribute = GameAttribute.Attributes[0x0091], // Damage_Min_Total 
    Int = 0x00000000,
    Float = 2f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x00000000,
    Attribute = GameAttribute.Attributes[0x0191], // Damage_Weapon_Delta_Total_CurrentHand 
    Int = 0x00000000,
    Float = 1f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x006E], // Movement_Scalar 
    Int = 0x00000000,
    Float = 1f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x00000001,
    Attribute = GameAttribute.Attributes[0x0092], // Damage_Min_Subtotal 
    Int = 0x00000000,
    Float = 3.051758E-05f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x00000002,
    Attribute = GameAttribute.Attributes[0x0092], // Damage_Min_Subtotal 
    Int = 0x00000000,
    Float = 3.051758E-05f,
    },
    },
    });

    SendMessage(new AttributesSetValuesMessage()
    {
        Id = 0x004D,
        Field0 = 0x789E00E2,
        atKeyVals = new NetAttributeKeyValue[15]
    {
    new NetAttributeKeyValue()
    {
    Field0 = 0x00000003,
    Attribute = GameAttribute.Attributes[0x0092], // Damage_Min_Subtotal 
    Int = 0x00000000,
    Float = 3.051758E-05f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x00000004,
    Attribute = GameAttribute.Attributes[0x0092], // Damage_Min_Subtotal 
    Int = 0x00000000,
    Float = 3.051758E-05f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x00000005,
    Attribute = GameAttribute.Attributes[0x0092], // Damage_Min_Subtotal 
    Int = 0x00000000,
    Float = 3.051758E-05f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x00000006,
    Attribute = GameAttribute.Attributes[0x0092], // Damage_Min_Subtotal 
    Int = 0x00000000,
    Float = 3.051758E-05f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x00000000,
    Attribute = GameAttribute.Attributes[0x0092], // Damage_Min_Subtotal 
    Int = 0x00000000,
    Float = 2f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x00000000,
    Attribute = GameAttribute.Attributes[0x0094], // Damage_Weapon_Delta 
    Int = 0x00000000,
    Float = 1f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x00000000,
    Attribute = GameAttribute.Attributes[0x0095], // Damage_Weapon_Delta_SubTotal 
    Int = 0x00000000,
    Float = 1f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x00000000,
    Attribute = GameAttribute.Attributes[0x0096], // Damage_Weapon_Max 
    Int = 0x00000000,
    Float = 3f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x00000000,
    Attribute = GameAttribute.Attributes[0x0097], // Damage_Weapon_Max_Total 
    Int = 0x00000000,
    Float = 3f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x00000000,
    Attribute = GameAttribute.Attributes[0x0098], // Damage_Weapon_Delta_Total 
    Int = 0x00000000,
    Float = 1f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x0000CE11,
    Attribute = GameAttribute.Attributes[0x027B], // Trait 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x00000000,
    Attribute = GameAttribute.Attributes[0x009B], // Damage_Weapon_Min 
    Int = 0x00000000,
    Float = 2f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x00000000,
    Attribute = GameAttribute.Attributes[0x009C], // Damage_Weapon_Min_Total 
    Int = 0x00000000,
    Float = 2f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x0000CE11,
    Attribute = GameAttribute.Attributes[0x0041], // Skill 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x0000CE11,
    Attribute = GameAttribute.Attributes[0x0042], // Skill_Total 
    Int = 0x00000001,
    Float = 0f,
    },
    },
    });

    SendMessage(new AttributesSetValuesMessage()
    {
        Id = 0x004D,
        Field0 = 0x789E00E2,
        atKeyVals = new NetAttributeKeyValue[15]
    {
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x005C], // Resource_Type_Primary 
    Int = 0x00000003,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0056], // Hitpoints_Max_Total 
    Int = 0x00000000,
    Float = 76f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0054], // Hitpoints_Max 
    Int = 0x00000000,
    Float = 40f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0051], // Hitpoints_Total_From_Level 
    Int = 0x00000000,
    Float = 3.051758E-05f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0050], // Hitpoints_Total_From_Vitality 
    Int = 0x00000000,
    Float = 36f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x004F], // Hitpoints_Factor_Vitality 
    Int = 0x00000000,
    Float = 4f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x004E], // Hitpoints_Factor_Level 
    Int = 0x00000000,
    Float = 4f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x004D], // Hitpoints_Cur 
    Int = 0x00000000,
    Float = 76f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x024C], // Disabled 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0046], // Loading 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0045], // Invulnerable 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0043], // TeamID 
    Int = 0x00000002,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x000FFFFF,
    Attribute = GameAttribute.Attributes[0x0042], // Skill_Total 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x000FFFFF,
    Attribute = GameAttribute.Attributes[0x0041], // Skill 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x0000CE11,
    Attribute = GameAttribute.Attributes[0x0230], // Buff_Icon_Count0 
    Int = 0x00000001,
    Float = 0f,
    },
    },
    });

    SendMessage(new AttributesSetValuesMessage()
    {
        Id = 0x004D,
        Field0 = 0x789E00E2,
        atKeyVals = new NetAttributeKeyValue[14]
    {
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x012C], // Hidden 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0027], // Level_Cap 
    Int = 0x0000000D,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0026], // Level 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0022], // Experience_Next 
    Int = 0x000004B0,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0021], // Experience_Granted 
    Int = 0x000003E8,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0020], // Armor_Total 
    Int = 0x00000000,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x000C], // Defense 
    Int = 0x00000000,
    Float = 10f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x00033C40,
    Attribute = GameAttribute.Attributes[0x0230], // Buff_Icon_Count0 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x000B], // Vitality 
    Int = 0x00000000,
    Float = 9f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x000A], // Precision 
    Int = 0x00000000,
    Float = 11f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0009], // Attack 
    Int = 0x00000000,
    Float = 10f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0008], // Shared_Stash_Slots 
    Int = 0x0000000E,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0007], // Backpack_Slots 
    Int = 0x0000003C,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0103], // General_Cooldown 
    Int = 0x00000000,
    Float = 0f,
    },
    },
    });

    SendMessage(new ACDGroupMessage()
    {
        Id = 0x00B8,
        Field0 = 0x789E00E2,
        Field1 = -1,
        Field2 = -1,
    });

    SendMessage(new ANNDataMessage()
    {
        Id = 0x003E,
        Field0 = 0x789E00E2,
    });

    SendMessage(new ACDTranslateFacingMessage()
    {
        Id = 0x0070,
        Field0 = 0x789E00E2,
        Field1 = 3.022712f,
        Field2 = false,
    });

    SendMessage(new PlayerEnterKnownMessage()
    {
        Id = 0x003D,
        Field0 = 0x00000000,
        Field1 = 0x789E00E2,
    });

    SendMessage(new VisualInventoryMessage()
    {
        Id = 0x004E,
        Field0 = 0x789E00E2,
        Field1 = new VisualEquipment()
        {
            Field0 = new VisualItem[8]
    {
     new VisualItem()
     {
        Field0 = -1,
        Field1 = 0x00000000,
        Field2 = 0x00000000,
        Field3 = 0x00000000,
     },
     new VisualItem()
     {
        Field0 = -1,
        Field1 = 0x00000000,
        Field2 = 0x00000000,
        Field3 = 0x00000000,
     },
     new VisualItem()
     {
        Field0 = -1,
        Field1 = 0x00000000,
        Field2 = 0x00000000,
        Field3 = 0x00000000,
     },
     new VisualItem()
     {
        Field0 = -1,
        Field1 = 0x00000000,
        Field2 = 0x00000000,
        Field3 = 0x00000000,
     },
     new VisualItem()
     {
        Field0 = 0x49B51827,
        Field1 = 0x00000000,
        Field2 = 0x00000000,
        Field3 = -1,
     },
     new VisualItem()
     {
        Field0 = -1,
        Field1 = 0x00000000,
        Field2 = 0x00000000,
        Field3 = 0x00000000,
     },
     new VisualItem()
     {
        Field0 = -1,
        Field1 = 0x00000000,
        Field2 = 0x00000000,
        Field3 = 0x00000000,
     },
     new VisualItem()
     {
        Field0 = -1,
        Field1 = 0x00000000,
        Field2 = 0x00000000,
        Field3 = 0x00000000,
     },
    },
        },
    });

    SendMessage(new PlayerActorSetInitialMessage()
    {
        Id = 0x0039,
        Field0 = 0x789E00E2,
        Field1 = 0x00000000,
    });
    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000001,
            Field1 = 0x00001271,
        },
    });
    #endregion
    #region ACDEnterKnown 0x789700DB
    SendMessage(new ACDEnterKnownMessage()
    {
        Id = 0x003B,
        Field0 = 0x789700DB,
        Field1 = 0x00001767,
        Field2 = 0x00000018,
        Field3 = 0x00000000,
        Field4 = new WorldLocationMessageData()
        {
            Field0 = 1.11f,
            Field1 = new PRTransform()
            {
                Field0 = new Quaternion()
                {
                    Field0 = 0.9330626f,
                    Field1 = new Vector3D()
                    {
                        Field0 = 0f,
                        Field1 = 0f,
                        Field2 = 0.3597142f,
                    },
                },
                Field1 = new Vector3D()
                {
                    Field0 = 3108.125f,
                    Field1 = 2882.163f,
                    Field2 = 64.49878f,
                },
            },
            Field2 = 0x772E0000,
        },
        Field5 = null,
        Field6 = new GBHandle()
        {
            Field0 = -1,
            Field1 = -1,
        },
        Field7 = 0x00000001,
        Field8 = 0x00001767,
        Field9 = 0x00000000,
        Field10 = 0x00,
        Field12 = 0x00012A04,
        Field13 = 0x00000000,
    });

    SendMessage(new AffixMessage()
    {
        Id = 0x0048,
        Field0 = 0x789700DB,
        Field1 = 0x00000001,
        aAffixGBIDs = new int[0]
    {
    },
    });

    SendMessage(new AffixMessage()
    {
        Id = 0x0048,
        Field0 = 0x789700DB,
        Field1 = 0x00000002,
        aAffixGBIDs = new int[0]
    {
    },
    });

    SendMessage(new ACDCollFlagsMessage()
    {
        Id = 0x00A6,
        Field0 = 0x789700DB,
        Field1 = 0x00000000,
    });

    SendMessage(new AttributesSetValuesMessage()
    {
        Id = 0x004D,
        Field0 = 0x789700DB,
        atKeyVals = new NetAttributeKeyValue[6]
    {
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0056], // Hitpoints_Max_Total 
    Int = 0x00000000,
    Float = 1f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0054], // Hitpoints_Max 
    Int = 0x00000000,
    Float = 0.0009994507f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0051], // Hitpoints_Total_From_Level 
    Int = 0x00000000,
    Float = 3.051758E-05f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x004D], // Hitpoints_Cur 
    Int = 0x00000000,
    Float = 0.0009994507f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0043], // TeamID 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0026], // Level 
    Int = 0x00000001,
    Float = 0f,
    },
    },
    });

    SendMessage(new ACDGroupMessage()
    {
        Id = 0x00B8,
        Field0 = 0x789700DB,
        Field1 = -1,
        Field2 = -1,
    });

    SendMessage(new ANNDataMessage()
    {
        Id = 0x003E,
        Field0 = 0x789700DB,
    });

    SendMessage(new ACDTranslateFacingMessage()
    {
        Id = 0x0070,
        Field0 = 0x789700DB,
        Field1 = 0.7359439f,
        Field2 = false,
    });

    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000001,
            Field1 = 0x00001767,
        },
    });
    #endregion
    #region ACDEnterKnown 0x789800DC
    SendMessage(new ACDEnterKnownMessage()
    {
        Id = 0x003B,
        Field0 = 0x789800DC,
        Field1 = 0x0000157F,
        Field2 = 0x00000008,
        Field3 = 0x00000000,
        Field4 = new WorldLocationMessageData()
        {
            Field0 = 1f,
            Field1 = new PRTransform()
            {
                Field0 = new Quaternion()
                {
                    Field0 = 0.05940768f,
                    Field1 = new Vector3D()
                    {
                        Field0 = 0f,
                        Field1 = 0f,
                        Field2 = 0.9982339f,
                    },
                },
                Field1 = new Vector3D()
                {
                    Field0 = 3143.104f,
                    Field1 = 2829.936f,
                    Field2 = 59.07556f,
                },
            },
            Field2 = 0x772E0000,
        },
        Field5 = null,
        Field6 = new GBHandle()
        {
            Field0 = -1,
            Field1 = -1,
        },
        Field7 = 0x00000001,
        Field8 = 0x0000157F,
        Field9 = 0x00000000,
        Field10 = 0x00,
        Field12 = 0x00012A04,
        Field13 = 0x00000001,
    });

    SendMessage(new AffixMessage()
    {
        Id = 0x0048,
        Field0 = 0x789800DC,
        Field1 = 0x00000001,
        aAffixGBIDs = new int[0]
    {
    },
    });

    SendMessage(new AffixMessage()
    {
        Id = 0x0048,
        Field0 = 0x789800DC,
        Field1 = 0x00000002,
        aAffixGBIDs = new int[0]
    {
    },
    });

    SendMessage(new ACDCollFlagsMessage()
    {
        Id = 0x00A6,
        Field0 = 0x789800DC,
        Field1 = 0x00000000,
    });

    SendMessage(new AttributesSetValuesMessage()
    {
        Id = 0x004D,
        Field0 = 0x789800DC,
        atKeyVals = new NetAttributeKeyValue[6]
    {
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0056], // Hitpoints_Max_Total 
    Int = 0x00000000,
    Float = 1f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0054], // Hitpoints_Max 
    Int = 0x00000000,
    Float = 0.0009994507f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0051], // Hitpoints_Total_From_Level 
    Int = 0x00000000,
    Float = 3.051758E-05f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x004D], // Hitpoints_Cur 
    Int = 0x00000000,
    Float = 0.0009994507f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0043], // TeamID 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0026], // Level 
    Int = 0x00000001,
    Float = 0f,
    },
    },
    });

    SendMessage(new ACDGroupMessage()
    {
        Id = 0x00B8,
        Field0 = 0x789800DC,
        Field1 = -1,
        Field2 = -1,
    });

    SendMessage(new ANNDataMessage()
    {
        Id = 0x003E,
        Field0 = 0x789800DC,
    });

    SendMessage(new ACDTranslateFacingMessage()
    {
        Id = 0x0070,
        Field0 = 0x789800DC,
        Field1 = 3.022712f,
        Field2 = false,
    });

    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000001,
            Field1 = 0x0000157F,
        },
    });

    SendMessage(new ACDEnterKnownMessage()
    {
        Id = 0x003B,
        Field0 = 0x789900DD,
        Field1 = 0x000201E5,
        Field2 = 0x00000008,
        Field3 = 0x00000000,
        Field4 = new WorldLocationMessageData()
        {
            Field0 = 1f,
            Field1 = new PRTransform()
            {
                Field0 = new Quaternion()
                {
                    Field0 = 0.9878305f,
                    Field1 = new Vector3D()
                    {
                        Field0 = 0f,
                        Field1 = 0f,
                        Field2 = 0.1555345f,
                    },
                },
                Field1 = new Vector3D()
                {
                    Field0 = 3109.516f,
                    Field1 = 2803.876f,
                    Field2 = 59.07428f,
                },
            },
            Field2 = 0x772E0000,
        },
        Field5 = null,
        Field6 = new GBHandle()
        {
            Field0 = -1,
            Field1 = -1,
        },
        Field7 = 0x00000001,
        Field8 = 0x000201E5,
        Field9 = 0x00000000,
        Field10 = 0x00,
        Field12 = 0x00012A04,
        Field13 = 0x00000002,
    });

    SendMessage(new AffixMessage()
    {
        Id = 0x0048,
        Field0 = 0x789900DD,
        Field1 = 0x00000001,
        aAffixGBIDs = new int[0]
    {
    },
    });

    SendMessage(new AffixMessage()
    {
        Id = 0x0048,
        Field0 = 0x789900DD,
        Field1 = 0x00000002,
        aAffixGBIDs = new int[0]
    {
    },
    });

    SendMessage(new ACDCollFlagsMessage()
    {
        Id = 0x00A6,
        Field0 = 0x789900DD,
        Field1 = 0x00000411,
    });

    SendMessage(new AttributesSetValuesMessage()
    {
        Id = 0x004D,
        Field0 = 0x789900DD,
        atKeyVals = new NetAttributeKeyValue[6]
    {
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0056], // Hitpoints_Max_Total 
    Int = 0x00000000,
    Float = 1f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0054], // Hitpoints_Max 
    Int = 0x00000000,
    Float = 0.0009994507f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0051], // Hitpoints_Total_From_Level 
    Int = 0x00000000,
    Float = 3.051758E-05f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x004D], // Hitpoints_Cur 
    Int = 0x00000000,
    Float = 0.0009994507f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0043], // TeamID 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0026], // Level 
    Int = 0x00000001,
    Float = 0f,
    },
    },
    });

    SendMessage(new ACDGroupMessage()
    {
        Id = 0x00B8,
        Field0 = 0x789900DD,
        Field1 = -1,
        Field2 = -1,
    });

    SendMessage(new ANNDataMessage()
    {
        Id = 0x003E,
        Field0 = 0x789900DD,
    });

    SendMessage(new ACDTranslateFacingMessage()
    {
        Id = 0x0070,
        Field0 = 0x789900DD,
        Field1 = 0.3123385f,
        Field2 = false,
    });

    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000001,
            Field1 = 0x000201E5,
        },
    });
    #endregion
    #region ACDEnterKnown 0x789A00DE
    SendMessage(new ACDEnterKnownMessage()
    {
        Id = 0x003B,
        Field0 = 0x789A00DE,
        Field1 = 0x00013871,
        Field2 = 0x00000008,
        Field3 = 0x00000000,
        Field4 = new WorldLocationMessageData()
        {
            Field0 = 1f,
            Field1 = new PRTransform()
            {
                Field0 = new Quaternion()
                {
                    Field0 = -0.7652696f,
                    Field1 = new Vector3D()
                    {
                        Field0 = 0f,
                        Field1 = 0f,
                        Field2 = 0.6437099f,
                    },
                },
                Field1 = new Vector3D()
                {
                    Field0 = 3113.659f,
                    Field1 = 2803.692f,
                    Field2 = 73.26618f,
                },
            },
            Field2 = 0x772E0000,
        },
        Field5 = null,
        Field6 = new GBHandle()
        {
            Field0 = -1,
            Field1 = -1,
        },
        Field7 = 0x00000001,
        Field8 = 0x00013871,
        Field9 = 0x00000000,
        Field10 = 0x00,
        Field12 = 0x00012A04,
        Field13 = 0x00000003,
    });

    SendMessage(new AffixMessage()
    {
        Id = 0x0048,
        Field0 = 0x789A00DE,
        Field1 = 0x00000001,
        aAffixGBIDs = new int[0]
    {
    },
    });

    SendMessage(new AffixMessage()
    {
        Id = 0x0048,
        Field0 = 0x789A00DE,
        Field1 = 0x00000002,
        aAffixGBIDs = new int[0]
    {
    },
    });

    SendMessage(new ACDCollFlagsMessage()
    {
        Id = 0x00A6,
        Field0 = 0x789A00DE,
        Field1 = 0x00000001,
    });

    SendMessage(new AttributesSetValuesMessage()
    {
        Id = 0x004D,
        Field0 = 0x789A00DE,
        atKeyVals = new NetAttributeKeyValue[6]
    {
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0056], // Hitpoints_Max_Total 
    Int = 0x00000000,
    Float = 1f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0054], // Hitpoints_Max 
    Int = 0x00000000,
    Float = 0.0009994507f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0051], // Hitpoints_Total_From_Level 
    Int = 0x00000000,
    Float = 3.051758E-05f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x004D], // Hitpoints_Cur 
    Int = 0x00000000,
    Float = 0.0009994507f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0043], // TeamID 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0026], // Level 
    Int = 0x00000001,
    Float = 0f,
    },
    },
    });

    SendMessage(new ACDGroupMessage()
    {
        Id = 0x00B8,
        Field0 = 0x789A00DE,
        Field1 = -1,
        Field2 = -1,
    });

    SendMessage(new ANNDataMessage()
    {
        Id = 0x003E,
        Field0 = 0x789A00DE,
    });

    SendMessage(new ACDTranslateFacingMessage()
    {
        Id = 0x0070,
        Field0 = 0x789A00DE,
        Field1 = 4.884459f,
        Field2 = false,
    });

    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000001,
            Field1 = 0x00013871,
        },
    });
    #endregion
    #region ACDEnterKnown 0x789B00DF
    SendMessage(new ACDEnterKnownMessage()
    {
        Id = 0x003B,
        Field0 = 0x789B00DF,
        Field1 = 0x00000D86,
        Field2 = 0x00000018,
        Field3 = 0x00000000,
        Field4 = new WorldLocationMessageData()
        {
            Field0 = 1f,
            Field1 = new PRTransform()
            {
                Field0 = new Quaternion()
                {
                    Field0 = 0.9976531f,
                    Field1 = new Vector3D()
                    {
                        Field0 = 0.009601643f,
                        Field1 = -0.0006524475f,
                        Field2 = -0.06779216f,
                    },
                },
                Field1 = new Vector3D()
                {
                    Field0 = 3127.913f,
                    Field1 = 2830.662f,
                    Field2 = 59.07558f,
                },
            },
            Field2 = 0x772E0000,
        },
        Field5 = null,
        Field6 = new GBHandle()
        {
            Field0 = -1,
            Field1 = -1,
        },
        Field7 = 0x00000001,
        Field8 = 0x00000D86,
        Field9 = 0x00000000,
        Field10 = 0x00,
        Field12 = 0x00012A04,
        Field13 = 0x00000004,
    });

    SendMessage(new AffixMessage()
    {
        Id = 0x0048,
        Field0 = 0x789B00DF,
        Field1 = 0x00000001,
        aAffixGBIDs = new int[0]
    {
    },
    });

    SendMessage(new AffixMessage()
    {
        Id = 0x0048,
        Field0 = 0x789B00DF,
        Field1 = 0x00000002,
        aAffixGBIDs = new int[0]
    {
    },
    });

    SendMessage(new ACDCollFlagsMessage()
    {
        Id = 0x00A6,
        Field0 = 0x789B00DF,
        Field1 = 0x00000000,
    });

    SendMessage(new AttributesSetValuesMessage()
    {
        Id = 0x004D,
        Field0 = 0x789B00DF,
        atKeyVals = new NetAttributeKeyValue[6]
    {
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0056], // Hitpoints_Max_Total 
    Int = 0x00000000,
    Float = 1f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0054], // Hitpoints_Max 
    Int = 0x00000000,
    Float = 0.0009994507f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0051], // Hitpoints_Total_From_Level 
    Int = 0x00000000,
    Float = 3.051758E-05f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x004D], // Hitpoints_Cur 
    Int = 0x00000000,
    Float = 0.0009994507f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0043], // TeamID 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0026], // Level 
    Int = 0x00000001,
    Float = 0f,
    },
    },
    });

    SendMessage(new ACDGroupMessage()
    {
        Id = 0x00B8,
        Field0 = 0x789B00DF,
        Field1 = -1,
        Field2 = -1,
    });

    SendMessage(new ANNDataMessage()
    {
        Id = 0x003E,
        Field0 = 0x789B00DF,
    });

    SendMessage(new ACDTranslateFacingMessage()
    {
        Id = 0x0070,
        Field0 = 0x789B00DF,
        Field1 = 6.147496f,
        Field2 = false,
    });

    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000001,
            Field1 = 0x00000D86,
        },
    });
    #endregion
    #region ACDEnterKnown 0x789C00E0
    SendMessage(new ACDEnterKnownMessage()
    {
        Id = 0x003B,
        Field0 = 0x789C00E0,
        Field1 = 0x00013870,
        Field2 = 0x00000008,
        Field3 = 0x00000000,
        Field4 = new WorldLocationMessageData()
        {
            Field0 = 1f,
            Field1 = new PRTransform()
            {
                Field0 = new Quaternion()
                {
                    Field0 = -0.4954694f,
                    Field1 = new Vector3D()
                    {
                        Field0 = 0f,
                        Field1 = 0f,
                        Field2 = -0.8686254f,
                    },
                },
                Field1 = new Vector3D()
                {
                    Field0 = 3120.037f,
                    Field1 = 2864.345f,
                    Field2 = 59.07556f,
                },
            },
            Field2 = 0x772E0000,
        },
        Field5 = null,
        Field6 = new GBHandle()
        {
            Field0 = -1,
            Field1 = -1,
        },
        Field7 = 0x00000001,
        Field8 = 0x00013870,
        Field9 = 0x00000000,
        Field10 = 0x00,
        Field12 = 0x00012A04,
        Field13 = 0x00000006,
    });

    SendMessage(new AffixMessage()
    {
        Id = 0x0048,
        Field0 = 0x789C00E0,
        Field1 = 0x00000001,
        aAffixGBIDs = new int[0]
    {
    },
    });

    SendMessage(new AffixMessage()
    {
        Id = 0x0048,
        Field0 = 0x789C00E0,
        Field1 = 0x00000002,
        aAffixGBIDs = new int[0]
    {
    },
    });

    SendMessage(new ACDCollFlagsMessage()
    {
        Id = 0x00A6,
        Field0 = 0x789C00E0,
        Field1 = 0x00000001,
    });

    SendMessage(new AttributesSetValuesMessage()
    {
        Id = 0x004D,
        Field0 = 0x789C00E0,
        atKeyVals = new NetAttributeKeyValue[6]
    {
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0056], // Hitpoints_Max_Total 
    Int = 0x00000000,
    Float = 1f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0054], // Hitpoints_Max 
    Int = 0x00000000,
    Float = 0.0009994507f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0051], // Hitpoints_Total_From_Level 
    Int = 0x00000000,
    Float = 3.051758E-05f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x004D], // Hitpoints_Cur 
    Int = 0x00000000,
    Float = 0.0009994507f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0043], // TeamID 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0026], // Level 
    Int = 0x00000001,
    Float = 0f,
    },
    },
    });

    SendMessage(new ACDGroupMessage()
    {
        Id = 0x00B8,
        Field0 = 0x789C00E0,
        Field1 = -1,
        Field2 = -1,
    });

    SendMessage(new ANNDataMessage()
    {
        Id = 0x003E,
        Field0 = 0x789C00E0,
    });

    SendMessage(new ACDTranslateFacingMessage()
    {
        Id = 0x0070,
        Field0 = 0x789C00E0,
        Field1 = 2.104887f,
        Field2 = false,
    });

    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000001,
            Field1 = 0x00013870,
        },
    });
    #endregion
    #region ACDEnterKnown 0x789F00E3
    SendMessage(new ACDEnterKnownMessage()
    {
        Id = 0x003B,
        Field0 = 0x789F00E3,
        Field1 = 0x00001025,
        Field2 = 0x0000001A,
        Field3 = 0x00000001,
        Field4 = null,
        Field5 = new InventoryLocationMessageData()
        {
            Field0 = 0x789E00E2,
            Field1 = 0x00000004,
            Field2 = new IVector2D()
            {
                Field0 = 0x00000000,
                Field1 = 0x00000000,
            },
        },
        Field6 = new GBHandle()
        {
            Field0 = 0x00000002,
            Field1 = 0x49B51827,
        },
        Field7 = -1,
        Field8 = -1,
        Field9 = 0x00000001,
        Field10 = 0x00,
    });

    SendMessage(new AffixMessage()
    {
        Id = 0x0048,
        Field0 = 0x789F00E3,
        Field1 = 0x00000001,
        aAffixGBIDs = new int[0]
    {
    },
    });

    SendMessage(new AffixMessage()
    {
        Id = 0x0048,
        Field0 = 0x789F00E3,
        Field1 = 0x00000002,
        aAffixGBIDs = new int[0]
    {
    },
    });

    SendMessage(new ACDCollFlagsMessage()
    {
        Id = 0x00A6,
        Field0 = 0x789F00E3,
        Field1 = 0x00000080,
    });

    SendMessage(new AttributesSetValuesMessage()
    {
        Id = 0x004D,
        Field0 = 0x789F00E3,
        atKeyVals = new NetAttributeKeyValue[15]
    {
    new NetAttributeKeyValue()
    {
    Field0 = 0x00007780,
    Attribute = GameAttribute.Attributes[0x0041], // Skill 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x009D], // Damage_Weapon_Min_Total_All 
    Int = 0x00000000,
    Float = 2f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x000FFFFF,
    Attribute = GameAttribute.Attributes[0x009C], // Damage_Weapon_Min_Total 
    Int = 0x00000000,
    Float = 3.051758E-05f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0099], // Damage_Weapon_Delta_Total_All 
    Int = 0x00000000,
    Float = 1f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x000FFFFF,
    Attribute = GameAttribute.Attributes[0x0097], // Damage_Weapon_Max_Total 
    Int = 0x00000000,
    Float = 3.051758E-05f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x000FFFFF,
    Attribute = GameAttribute.Attributes[0x0096], // Damage_Weapon_Max 
    Int = 0x00000000,
    Float = 3.051758E-05f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x000FFFFF,
    Attribute = GameAttribute.Attributes[0x018C], // Damage_Weapon_Min_Total_OffHand 
    Int = 0x00000000,
    Float = 3.051758E-05f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x000FFFFF,
    Attribute = GameAttribute.Attributes[0x018B], // Damage_Weapon_Min_Total_MainHand 
    Int = 0x00000000,
    Float = 3.051758E-05f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x018A], // Attacks_Per_Second_Item_Total_OffHand 
    Int = 0x00000000,
    Float = 3.051758E-05f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0089], // Attacks_Per_Second_Total 
    Int = 0x00000000,
    Float = 3.051758E-05f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0189], // Attacks_Per_Second_Item_Total_MainHand 
    Int = 0x00000000,
    Float = 1.199219f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0188], // Attacks_Per_Second_Item_OffHand 
    Int = 0x00000000,
    Float = 3.051758E-05f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0187], // Attacks_Per_Second_Item_MainHand 
    Int = 0x00000000,
    Float = 1.199219f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0086], // Attacks_Per_Second_Item_Total 
    Int = 0x00000000,
    Float = 1.199219f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0084], // Attacks_Per_Second_Item_Subtotal 
    Int = 0x00000000,
    Float = 1.199219f,
    },
    },
    });

    SendMessage(new AttributesSetValuesMessage()
    {
        Id = 0x004D,
        Field0 = 0x789F00E3,
        atKeyVals = new NetAttributeKeyValue[15]
    {
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0082], // Attacks_Per_Second_Item 
    Int = 0x00000000,
    Float = 1.199219f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x00000000,
    Attribute = GameAttribute.Attributes[0x018B], // Damage_Weapon_Min_Total_MainHand 
    Int = 0x00000000,
    Float = 2f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x00000000,
    Attribute = GameAttribute.Attributes[0x018C], // Damage_Weapon_Min_Total_OffHand 
    Int = 0x00000000,
    Float = 3.051758E-05f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x00000000,
    Attribute = GameAttribute.Attributes[0x018D], // Damage_Weapon_Delta_Total_MainHand 
    Int = 0x00000000,
    Float = 1f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x00000000,
    Attribute = GameAttribute.Attributes[0x018E], // Damage_Weapon_Delta_Total_OffHand 
    Int = 0x00000000,
    Float = 3.051758E-05f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x00000000,
    Attribute = GameAttribute.Attributes[0x0094], // Damage_Weapon_Delta 
    Int = 0x00000000,
    Float = 1f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x00000000,
    Attribute = GameAttribute.Attributes[0x0095], // Damage_Weapon_Delta_SubTotal 
    Int = 0x00000000,
    Float = 1f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x00000000,
    Attribute = GameAttribute.Attributes[0x0096], // Damage_Weapon_Max 
    Int = 0x00000000,
    Float = 3f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x00000000,
    Attribute = GameAttribute.Attributes[0x0097], // Damage_Weapon_Max_Total 
    Int = 0x00000000,
    Float = 3f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x00000000,
    Attribute = GameAttribute.Attributes[0x0098], // Damage_Weapon_Delta_Total 
    Int = 0x00000000,
    Float = 1f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x00000000,
    Attribute = GameAttribute.Attributes[0x009B], // Damage_Weapon_Min 
    Int = 0x00000000,
    Float = 2f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x00000000,
    Attribute = GameAttribute.Attributes[0x009C], // Damage_Weapon_Min_Total 
    Int = 0x00000000,
    Float = 2f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0125], // Seed 
    Int = unchecked((int)0xED34A51F),
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0124], // IdentifyCost 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0117], // Item_Equipped 
    Int = 0x00000001,
    Float = 0f,
    },
    },
    });

    SendMessage(new AttributesSetValuesMessage()
    {
        Id = 0x004D,
        Field0 = 0x789F00E3,
        atKeyVals = new NetAttributeKeyValue[3]
    {
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0115], // Item_Quality_Level 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0113], // Durability_Max 
    Int = 0x00000190,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0112], // Durability_Cur 
    Int = 0x00000190,
    Float = 0f,
    },
    },
    });

    SendMessage(new ACDGroupMessage()
    {
        Id = 0x00B8,
        Field0 = 0x789F00E3,
        Field1 = -1,
        Field2 = -1,
    });

    SendMessage(new ANNDataMessage()
    {
        Id = 0x003E,
        Field0 = 0x789F00E3,
    });

    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000001,
            Field1 = 0x00001025,
        },
    });
    #endregion
    #region ACDEnterKnown 0x78A000E4
    SendMessage(new ACDEnterKnownMessage()
    {
        Id = 0x003B,
        Field0 = 0x78A000E4,
        Field1 = 0x00001158,
        Field2 = 0x0000001A,
        Field3 = 0x00000001,
        Field4 = null,
        Field5 = new InventoryLocationMessageData()
        {
            Field0 = 0x789E00E2,
            Field1 = 0x00000000,
            Field2 = new IVector2D()
            {
                Field0 = 0x00000000,
                Field1 = 0x00000000,
            },
        },
        Field6 = new GBHandle()
        {
            Field0 = 0x00000002,
            Field1 = 0x622256D4,
        },
        Field7 = -1,
        Field8 = -1,
        Field9 = 0x00000001,
        Field10 = 0x00,
    });

    SendMessage(new AffixMessage()
    {
        Id = 0x0048,
        Field0 = 0x78A000E4,
        Field1 = 0x00000001,
        aAffixGBIDs = new int[0]
    {
    },
    });

    SendMessage(new AffixMessage()
    {
        Id = 0x0048,
        Field0 = 0x78A000E4,
        Field1 = 0x00000002,
        aAffixGBIDs = new int[0]
    {
    },
    });

    SendMessage(new ACDCollFlagsMessage()
    {
        Id = 0x00A6,
        Field0 = 0x78A000E4,
        Field1 = 0x00000080,
    });

    SendMessage(new AttributesSetValuesMessage()
    {
        Id = 0x004D,
        Field0 = 0x78A000E4,
        atKeyVals = new NetAttributeKeyValue[4]
    {
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0052], // Hitpoints_Granted 
    Int = 0x00000000,
    Float = 100f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0125], // Seed 
    Int = unchecked((int)0x884DCD35),
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0121], // ItemStackQuantityLo 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0115], // Item_Quality_Level 
    Int = 0x00000001,
    Float = 0f,
    },
    },
    });

    SendMessage(new ACDGroupMessage()
    {
        Id = 0x00B8,
        Field0 = 0x78A000E4,
        Field1 = -1,
        Field2 = -1,
    });

    SendMessage(new ANNDataMessage()
    {
        Id = 0x003E,
        Field0 = 0x78A000E4,
    });

    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000001,
            Field1 = 0x00001158,
        },
    });
    #endregion

    #region ACDEnterKnown 0x78BE0102
    SendMessage(new ACDEnterKnownMessage()
    {
        Id = 0x003B,
        Field0 = 0x78BE0102,
        Field1 = 0x0001B186,
        Field2 = 0x00000008,
        Field3 = 0x00000000,
        Field4 = new WorldLocationMessageData()
        {
            Field0 = 1.13f,
            Field1 = new PRTransform()
            {
                Field0 = new Quaternion()
                {
                    Field0 = 0.9009878f,
                    Field1 = new Vector3D()
                    {
                        Field0 = 0f,
                        Field1 = 0f,
                        Field2 = -0.4338445f,
                    },
                },
                Field1 = new Vector3D()
                {
                    Field0 = 3007.198f,
                    Field1 = 2712.854f,
                    Field2 = 23.76516f,
                },
            },
            Field2 = 0x772E0000,
        },
        Field5 = null,
        Field6 = new GBHandle()
        {
            Field0 = -1,
            Field1 = -1,
        },
        Field7 = 0x00000001,
        Field8 = 0x0001B186,
        Field9 = 0x00000000,
        Field10 = 0x00,
        Field12 = 0x0000F05D,
        Field13 = 0x00000012,
    });

    SendMessage(new AffixMessage()
    {
        Id = 0x0048,
        Field0 = 0x78BE0102,
        Field1 = 0x00000001,
        aAffixGBIDs = new int[0]
    {
    },
    });

    SendMessage(new AffixMessage()
    {
        Id = 0x0048,
        Field0 = 0x78BE0102,
        Field1 = 0x00000002,
        aAffixGBIDs = new int[0]
    {
    },
    });

    SendMessage(new ACDCollFlagsMessage()
    {
        Id = 0x00A6,
        Field0 = 0x78BE0102,
        Field1 = 0x00000102,
    });

    SendMessage(new AttributesSetValuesMessage()
    {
        Id = 0x004D,
        Field0 = 0x78BE0102,
        atKeyVals = new NetAttributeKeyValue[9]
    {
    new NetAttributeKeyValue()
    {
    Field0 = 0x00000000,
    Attribute = GameAttribute.Attributes[0x0159], // Conversation_Icon 
    Int = 0x00000000,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0156], // NPC_Is_Operatable 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0056], // Hitpoints_Max_Total 
    Int = 0x00000000,
    Float = 8.523438f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0155], // Is_NPC 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0054], // Hitpoints_Max 
    Int = 0x00000000,
    Float = 8.523438f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0051], // Hitpoints_Total_From_Level 
    Int = 0x00000000,
    Float = 3.051758E-05f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x004D], // Hitpoints_Cur 
    Int = 0x00000000,
    Float = 8.523438f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0043], // TeamID 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0026], // Level 
    Int = 0x00000001,
    Float = 0f,
    },
    },
    });

    SendMessage(new ACDGroupMessage()
    {
        Id = 0x00B8,
        Field0 = 0x78BE0102,
        Field1 = -1,
        Field2 = -1,
    });

    SendMessage(new ANNDataMessage()
    {
        Id = 0x003E,
        Field0 = 0x78BE0102,
    });

    SendMessage(new ACDTranslateFacingMessage()
    {
        Id = 0x0070,
        Field0 = 0x78BE0102,
        Field1 = 5.385688f,
        Field2 = false,
    });

    SendMessage(new SetIdleAnimationMessage()
    {
        Id = 0x00A5,
        Field0 = 0x78BE0102,
        Field1 = 0x00011150,
    });

    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000001,
            Field1 = 0x0001B186,
        },
    });
    #endregion
    #region ACDEnterKnown 0x78DD0118
    SendMessage(new ACDEnterKnownMessage()
    {
        Id = 0x003B,
        Field0 = 0x78DD0118,
        Field1 = 0x0000157E,
        Field2 = 0x00000008,
        Field3 = 0x00000000,
        Field4 = new WorldLocationMessageData()
        {
            Field0 = 1f,
            Field1 = new PRTransform()
            {
                Field0 = new Quaternion()
                {
                    Field0 = -0.01089788f,
                    Field1 = new Vector3D()
                    {
                        Field0 = 0f,
                        Field1 = 0f,
                        Field2 = 0.9999406f,
                    },
                },
                Field1 = new Vector3D()
                {
                    Field0 = 3125.888f,
                    Field1 = 2602.642f,
                    Field2 = 1.050535f,
                },
            },
            Field2 = 0x772E0000,
        },
        Field5 = null,
        Field6 = new GBHandle()
        {
            Field0 = -1,
            Field1 = -1,
        },
        Field7 = 0x00000001,
        Field8 = 0x0000157E,
        Field9 = 0x00000000,
        Field10 = 0x00,
        Field12 = 0x00011B71,
        Field13 = 0x00000000,
    });

    SendMessage(new AffixMessage()
    {
        Id = 0x0048,
        Field0 = 0x78DD0118,
        Field1 = 0x00000001,
        aAffixGBIDs = new int[0]
    {
    },
    });

    SendMessage(new AffixMessage()
    {
        Id = 0x0048,
        Field0 = 0x78DD0118,
        Field1 = 0x00000002,
        aAffixGBIDs = new int[0]
    {
    },
    });

    SendMessage(new ACDCollFlagsMessage()
    {
        Id = 0x00A6,
        Field0 = 0x78DD0118,
        Field1 = 0x00000000,
    });

    SendMessage(new AttributesSetValuesMessage()
    {
        Id = 0x004D,
        Field0 = 0x78DD0118,
        atKeyVals = new NetAttributeKeyValue[6]
    {
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0056], // Hitpoints_Max_Total 
    Int = 0x00000000,
    Float = 1f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0054], // Hitpoints_Max 
    Int = 0x00000000,
    Float = 0.0009994507f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0051], // Hitpoints_Total_From_Level 
    Int = 0x00000000,
    Float = 3.051758E-05f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x004D], // Hitpoints_Cur 
    Int = 0x00000000,
    Float = 0.0009994507f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0043], // TeamID 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0026], // Level 
    Int = 0x00000001,
    Float = 0f,
    },
    },
    });

    SendMessage(new ACDGroupMessage()
    {
        Id = 0x00B8,
        Field0 = 0x78DD0118,
        Field1 = -1,
        Field2 = -1,
    });

    SendMessage(new ANNDataMessage()
    {
        Id = 0x003E,
        Field0 = 0x78DD0118,
    });

    SendMessage(new ACDTranslateFacingMessage()
    {
        Id = 0x0070,
        Field0 = 0x78DD0118,
        Field1 = 3.163388f,
        Field2 = false,
    });

    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000001,
            Field1 = 0x0000157E,
        },
    });
    #endregion
    #region ACDEnterKnown 0x78DE0119
    SendMessage(new ACDEnterKnownMessage()
    {
        Id = 0x003B,
        Field0 = 0x78DE0119,
        Field1 = 0x00000D86,
        Field2 = 0x00000018,
        Field3 = 0x00000000,
        Field4 = new WorldLocationMessageData()
        {
            Field0 = 1f,
            Field1 = new PRTransform()
            {
                Field0 = new Quaternion()
                {
                    Field0 = 1f,
                    Field1 = new Vector3D()
                    {
                        Field0 = 0f,
                        Field1 = 0f,
                        Field2 = 0f,
                    },
                },
                Field1 = new Vector3D()
                {
                    Field0 = 3083.982f,
                    Field1 = 2603.142f,
                    Field2 = 0.4151611f,
                },
            },
            Field2 = 0x772E0000,
        },
        Field5 = null,
        Field6 = new GBHandle()
        {
            Field0 = -1,
            Field1 = -1,
        },
        Field7 = 0x00000001,
        Field8 = 0x00000D86,
        Field9 = 0x00000000,
        Field10 = 0x00,
        Field12 = 0x00011B71,
        Field13 = 0x00000001,
    });

    SendMessage(new AffixMessage()
    {
        Id = 0x0048,
        Field0 = 0x78DE0119,
        Field1 = 0x00000001,
        aAffixGBIDs = new int[0]
    {
    },
    });

    SendMessage(new AffixMessage()
    {
        Id = 0x0048,
        Field0 = 0x78DE0119,
        Field1 = 0x00000002,
        aAffixGBIDs = new int[0]
    {
    },
    });

    SendMessage(new ACDCollFlagsMessage()
    {
        Id = 0x00A6,
        Field0 = 0x78DE0119,
        Field1 = 0x00000000,
    });

    SendMessage(new AttributesSetValuesMessage()
    {
        Id = 0x004D,
        Field0 = 0x78DE0119,
        atKeyVals = new NetAttributeKeyValue[6]
    {
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0056], // Hitpoints_Max_Total 
    Int = 0x00000000,
    Float = 1f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0054], // Hitpoints_Max 
    Int = 0x00000000,
    Float = 0.0009994507f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0051], // Hitpoints_Total_From_Level 
    Int = 0x00000000,
    Float = 3.051758E-05f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x004D], // Hitpoints_Cur 
    Int = 0x00000000,
    Float = 0.0009994507f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0043], // TeamID 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0026], // Level 
    Int = 0x00000001,
    Float = 0f,
    },
    },
    });

    SendMessage(new ACDGroupMessage()
    {
        Id = 0x00B8,
        Field0 = 0x78DE0119,
        Field1 = -1,
        Field2 = -1,
    });

    SendMessage(new ANNDataMessage()
    {
        Id = 0x003E,
        Field0 = 0x78DE0119,
    });

    SendMessage(new ACDTranslateFacingMessage()
    {
        Id = 0x0070,
        Field0 = 0x78DE0119,
        Field1 = 0f,
        Field2 = false,
    });

    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000001,
            Field1 = 0x00000D86,
        },
    });
    #endregion
    #region ACDEnterKnown 0x78DF011A
    SendMessage(new ACDEnterKnownMessage()
    {
        Id = 0x003B,
        Field0 = 0x78DF011A,
        Field1 = 0x000255BB,
        Field2 = 0x00000008,
        Field3 = 0x00000000,
        Field4 = new WorldLocationMessageData()
        {
            Field0 = 1.13f,
            Field1 = new PRTransform()
            {
                Field0 = new Quaternion()
                {
                    Field0 = 0.1261874f,
                    Field1 = new Vector3D()
                    {
                        Field0 = 0f,
                        Field1 = 0f,
                        Field2 = 0.9920065f,
                    },
                },
                Field1 = new Vector3D()
                {
                    Field0 = 3131.338f,
                    Field1 = 2597.316f,
                    Field2 = 0.9298096f,
                },
            },
            Field2 = 0x772E0000,
        },
        Field5 = null,
        Field6 = new GBHandle()
        {
            Field0 = -1,
            Field1 = -1,
        },
        Field7 = 0x00000001,
        Field8 = 0x000255BB,
        Field9 = 0x00000000,
        Field10 = 0x00,
        Field12 = 0x00011B71,
        Field13 = 0x00000002,
    });

    SendMessage(new AffixMessage()
    {
        Id = 0x0048,
        Field0 = 0x78DF011A,
        Field1 = 0x00000001,
        aAffixGBIDs = new int[0]
    {
    },
    });

    SendMessage(new AffixMessage()
    {
        Id = 0x0048,
        Field0 = 0x78DF011A,
        Field1 = 0x00000002,
        aAffixGBIDs = new int[0]
    {
    },
    });

    SendMessage(new ACDCollFlagsMessage()
    {
        Id = 0x00A6,
        Field0 = 0x78DF011A,
        Field1 = 0x00000001,
    });

    SendMessage(new AttributesSetValuesMessage()
    {
        Id = 0x004D,
        Field0 = 0x78DF011A,
        atKeyVals = new NetAttributeKeyValue[13]
    {
    new NetAttributeKeyValue()
    {
    Field0 = 0x000FFFFF,
    Attribute = GameAttribute.Attributes[0x01B9], // Buff_Visual_Effect 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x00000000,
    Attribute = GameAttribute.Attributes[0x0158], // NPC_Has_Interact_Options 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x00000000,
    Attribute = GameAttribute.Attributes[0x0159], // Conversation_Icon 
    Int = 0x00000000,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x0001F972,
    Attribute = GameAttribute.Attributes[0x01CC], // Buff_Active 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x0001F972,
    Attribute = GameAttribute.Attributes[0x0230], // Buff_Icon_Count0 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0156], // NPC_Is_Operatable 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0056], // Hitpoints_Max_Total 
    Int = 0x00000000,
    Float = 14.00781f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0155], // Is_NPC 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0054], // Hitpoints_Max 
    Int = 0x00000000,
    Float = 14.00781f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0051], // Hitpoints_Total_From_Level 
    Int = 0x00000000,
    Float = 3.051758E-05f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x004D], // Hitpoints_Cur 
    Int = 0x00000000,
    Float = 14.00781f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0043], // TeamID 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0026], // Level 
    Int = 0x00000004,
    Float = 0f,
    },
    },
    });

    SendMessage(new ACDGroupMessage()
    {
        Id = 0x00B8,
        Field0 = 0x78DF011A,
        Field1 = -1,
        Field2 = -1,
    });

    SendMessage(new ANNDataMessage()
    {
        Id = 0x003E,
        Field0 = 0x78DF011A,
    });

    SendMessage(new ACDTranslateFacingMessage()
    {
        Id = 0x0070,
        Field0 = 0x78DF011A,
        Field1 = 2.888546f,
        Field2 = false,
    });

    SendMessage(new SetIdleAnimationMessage()
    {
        Id = 0x00A5,
        Field0 = 0x78DF011A,
        Field1 = 0x00011150,
    });

    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000001,
            Field1 = 0x000255BB,
        },
    });

    SendMessage(new HeroStateMessage()
    {
        Id = 0x003A,
        Field0 = new HeroStateData()
        {
            Field0 = 0x00000000,
            Field1 = 0x00000000,
            Field2 = 0x00000000,
            Field3 = 0x02000000,
            Field4 = new PlayerSavedData()
            {
                Field0 = new HotbarButtonData[9]
    {
         new HotbarButtonData()
         {
            m_snoPower = 0x000176C4,
            m_gbidItem = -1,
         },
         new HotbarButtonData()
         {
            m_snoPower = 0x00007780,
            m_gbidItem = -1,
         },
         new HotbarButtonData()
         {
            m_snoPower = -1,
            m_gbidItem = -1,
         },
         new HotbarButtonData()
         {
            m_snoPower = 0x00007780,
            m_gbidItem = -1,
         },
         new HotbarButtonData()
         {
            m_snoPower = 0x000216FA,
            m_gbidItem = -1,
         },
         new HotbarButtonData()
         {
            m_snoPower = -1,
            m_gbidItem = -1,
         },
         new HotbarButtonData()
         {
            m_snoPower = -1,
            m_gbidItem = -1,
         },
         new HotbarButtonData()
         {
            m_snoPower = -1,
            m_gbidItem = -1,
         },
         new HotbarButtonData()
         {
            m_snoPower = -1,
            m_gbidItem = 0x622256D4,
         },
    },
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
                snoActiveSkills = new int[6]
    {
        0x000176C4, 0x000216FA, -1, -1, -1, -1, 
    },
                snoTraits = new int[3]
    {
        -1, -1, -1, 
    },
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

    SendMessage(new ANNDataMessage()
    {
        Id = 0x003C,
        Field0 = 0x78BE0102,
    });

    SendMessage(new ANNDataMessage()
    {
        Id = 0x003C,
        Field0 = 0x78DF011A,
    });

    SendMessage(new RevealTeamMessage()
    {
        Id = 0x0038,
        Field0 = 0x00000000,
        Field1 = 0x00000000,
        Field2 = -1,
    });

    SendMessage(new RevealTeamMessage()
    {
        Id = 0x0038,
        Field0 = 0x00000001,
        Field1 = 0x00000000,
        Field2 = -1,
    });

    SendMessage(new RevealTeamMessage()
    {
        Id = 0x0038,
        Field0 = 0x00000002,
        Field1 = 0x00000000,
        Field2 = -1,
    });

    SendMessage(new RevealTeamMessage()
    {
        Id = 0x0038,
        Field0 = 0x00000003,
        Field1 = 0x00000000,
        Field2 = -1,
    });

    SendMessage(new RevealTeamMessage()
    {
        Id = 0x0038,
        Field0 = 0x00000004,
        Field1 = 0x00000000,
        Field2 = -1,
    });

    SendMessage(new RevealTeamMessage()
    {
        Id = 0x0038,
        Field0 = 0x00000005,
        Field1 = 0x00000000,
        Field2 = -1,
    });

    SendMessage(new RevealTeamMessage()
    {
        Id = 0x0038,
        Field0 = 0x00000006,
        Field1 = 0x00000000,
        Field2 = -1,
    });

    SendMessage(new RevealTeamMessage()
    {
        Id = 0x0038,
        Field0 = 0x00000007,
        Field1 = 0x00000000,
        Field2 = -1,
    });

    SendMessage(new RevealTeamMessage()
    {
        Id = 0x0038,
        Field0 = 0x00000008,
        Field1 = 0x00000000,
        Field2 = -1,
    });

    SendMessage(new RevealTeamMessage()
    {
        Id = 0x0038,
        Field0 = 0x00000009,
        Field1 = 0x00000000,
        Field2 = -1,
    });

    SendMessage(new RevealTeamMessage()
    {
        Id = 0x0038,
        Field0 = 0x0000000A,
        Field1 = 0x00000002,
        Field2 = -1,
    });

    SendMessage(new RevealTeamMessage()
    {
        Id = 0x0038,
        Field0 = 0x0000000B,
        Field1 = 0x00000002,
        Field2 = -1,
    });

    SendMessage(new RevealTeamMessage()
    {
        Id = 0x0038,
        Field0 = 0x0000000C,
        Field1 = 0x00000002,
        Field2 = -1,
    });

    SendMessage(new RevealTeamMessage()
    {
        Id = 0x0038,
        Field0 = 0x0000000D,
        Field1 = 0x00000002,
        Field2 = -1,
    });

    SendMessage(new RevealTeamMessage()
    {
        Id = 0x0038,
        Field0 = 0x0000000E,
        Field1 = 0x00000000,
        Field2 = 0x00000000,
    });

    SendMessage(new RevealTeamMessage()
    {
        Id = 0x0038,
        Field0 = 0x0000000F,
        Field1 = 0x00000000,
        Field2 = 0x00000000,
    });

    SendMessage(new RevealTeamMessage()
    {
        Id = 0x0038,
        Field0 = 0x00000010,
        Field1 = 0x00000000,
        Field2 = 0x00000000,
    });

    SendMessage(new RevealTeamMessage()
    {
        Id = 0x0038,
        Field0 = 0x00000011,
        Field1 = 0x00000000,
        Field2 = 0x00000000,
    });

    SendMessage(new RevealTeamMessage()
    {
        Id = 0x0038,
        Field0 = 0x00000012,
        Field1 = 0x00000000,
        Field2 = 0x00000000,
    });

    SendMessage(new RevealTeamMessage()
    {
        Id = 0x0038,
        Field0 = 0x00000013,
        Field1 = 0x00000000,
        Field2 = 0x00000000,
    });

    SendMessage(new RevealTeamMessage()
    {
        Id = 0x0038,
        Field0 = 0x00000014,
        Field1 = 0x00000000,
        Field2 = 0x00000000,
    });

    SendMessage(new RevealTeamMessage()
    {
        Id = 0x0038,
        Field0 = 0x00000015,
        Field1 = 0x00000000,
        Field2 = 0x00000000,
    });

    SendMessage(new RevealTeamMessage()
    {
        Id = 0x0038,
        Field0 = 0x00000016,
        Field1 = 0x00000000,
        Field2 = 0x00000000,
    });

    SendMessage(new RevealSceneMessage()
    {
        Id = 0x0034,
        Field0 = 0x772E0000,
        Field1 = new SceneSpecification()
        {
            Field0 = 0x00000000,
            Field1 = new IVector2D()
            {
                Field0 = 0x0000002F,
                Field1 = 0x0000002E,
            },
            arSnoLevelAreas = new int[4]
    {
    0x00004DEB, 0x00026186, 0x000316CE, -1, 
    },
            snoPrevWorld = -1,
            Field4 = 0x00000000,
            snoPrevLevelArea = -1,
            snoNextWorld = -1,
            Field7 = 0x00000000,
            snoNextLevelArea = -1,
            snoMusic = 0x000206F8,
            snoCombatMusic = -1,
            snoAmbient = 0x0002734F,
            snoReverb = 0x000153F6,
            snoWeather = 0x00013220,
            snoPresetWorld = 0x000115EE,
            Field15 = 0x00000001,
            Field16 = 0x00000001,
            Field17 = 0x00000000,
            Field18 = -1,
            tCachedValues = new SceneCachedValues()
            {
                Field0 = 0x0000003F,
                Field1 = 0x00000060,
                Field2 = 0x00000060,
                Field3 = new AABB()
                {
                    Field0 = new Vector3D()
                    {
                        Field0 = 118.3843f,
                        Field1 = 119.1316f,
                        Field2 = 43.57133f,
                    },
                    Field1 = new Vector3D()
                    {
                        Field0 = 134.2307f,
                        Field1 = 131.5811f,
                        Field2 = 43.57133f,
                    },
                },
                Field4 = new AABB()
                {
                    Field0 = new Vector3D()
                    {
                        Field0 = 113.465f,
                        Field1 = 119.1316f,
                        Field2 = 43.57133f,
                    },
                    Field1 = new Vector3D()
                    {
                        Field0 = 139.15f,
                        Field1 = 131.5811f,
                        Field2 = 43.57133f,
                    },
                },
                Field5 = new int[4]
    {
        0x0000083F, 0x00000371, 0x000001ED, 0x00000000, 
    },
                Field6 = 0x00000009,
            },
        },
        Field2 = 0x77550001,
        snoScene = 0x00008244,
        Field4 = new PRTransform()
        {
            Field0 = new Quaternion()
            {
                Field0 = 1f,
                Field1 = new Vector3D()
                {
                    Field0 = 0f,
                    Field1 = 0f,
                    Field2 = 0f,
                },
            },
            Field1 = new Vector3D()
            {
                Field0 = 2820f,
                Field1 = 2760f,
                Field2 = 0f,
            },
        },
        Field5 = -1,
        snoSceneGroup = -1,
        arAppliedLabels = new int[1]
    {
    0x00360E26, 
    },
    });

    SendMessage(new MapRevealSceneMessage()
    {
        Id = 0x0044,
        Field0 = 0x77550001,
        snoScene = 0x00008244,
        Field2 = new PRTransform()
        {
            Field0 = new Quaternion()
            {
                Field0 = 1f,
                Field1 = new Vector3D()
                {
                    Field0 = 0f,
                    Field1 = 0f,
                    Field2 = 0f,
                },
            },
            Field1 = new Vector3D()
            {
                Field0 = 2820f,
                Field1 = 2760f,
                Field2 = 0f,
            },
        },
        Field3 = 0x772E0000,
        Field4 = 0x00000002,
    });
    #endregion
    #region ACDEnterKnown 0x77BC0000
    SendMessage(new ACDEnterKnownMessage()
    {
        Id = 0x003B,
        Field0 = 0x77BC0000,
        Field1 = 0x00001243,
        Field2 = 0x00000010,
        Field3 = 0x00000000,
        Field4 = new WorldLocationMessageData()
        {
            Field0 = 1f,
            Field1 = new PRTransform()
            {
                Field0 = new Quaternion()
                {
                    Field0 = 1f,
                    Field1 = new Vector3D()
                    {
                        Field0 = 0f,
                        Field1 = 0f,
                        Field2 = 0f,
                    },
                },
                Field1 = new Vector3D()
                {
                    Field0 = 2922.286f,
                    Field1 = 2796.864f,
                    Field2 = 23.94531f,
                },
            },
            Field2 = 0x772E0000,
        },
        Field5 = null,
        Field6 = new GBHandle()
        {
            Field0 = -1,
            Field1 = -1,
        },
        Field7 = 0x00000001,
        Field8 = 0x00001243,
        Field9 = 0x00000000,
        Field10 = 0x00,
        Field12 = 0x0000DBD3,
        Field13 = 0x00000002,
    });

    SendMessage(new AffixMessage()
    {
        Id = 0x0048,
        Field0 = 0x77BC0000,
        Field1 = 0x00000001,
        aAffixGBIDs = new int[0]
    {
    },
    });

    SendMessage(new AffixMessage()
    {
        Id = 0x0048,
        Field0 = 0x77BC0000,
        Field1 = 0x00000002,
        aAffixGBIDs = new int[0]
    {
    },
    });

    SendMessage(new ACDCollFlagsMessage()
    {
        Id = 0x00A6,
        Field0 = 0x77BC0000,
        Field1 = 0x00000000,
    });

    SendMessage(new AttributesSetValuesMessage()
    {
        Id = 0x004D,
        Field0 = 0x77BC0000,
        atKeyVals = new NetAttributeKeyValue[4]
    {
    new NetAttributeKeyValue()
    {
    Field0 = 0x000FFFFF,
    Attribute = GameAttribute.Attributes[0x01B9], // Buff_Visual_Effect 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x0001F96A,
    Attribute = GameAttribute.Attributes[0x01CC], // Buff_Active 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x0001F96A,
    Attribute = GameAttribute.Attributes[0x0230], // Buff_Icon_Count0 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0043], // TeamID 
    Int = 0x00000000,
    Float = 0f,
    },
    },
    });

    SendMessage(new ACDGroupMessage()
    {
        Id = 0x00B8,
        Field0 = 0x77BC0000,
        Field1 = -1,
        Field2 = -1,
    });

    SendMessage(new ANNDataMessage()
    {
        Id = 0x003E,
        Field0 = 0x77BC0000,
    });

    SendMessage(new ACDTranslateFacingMessage()
    {
        Id = 0x0070,
        Field0 = 0x77BC0000,
        Field1 = 0f,
        Field2 = false,
    });
    #endregion
    #region ACDEnterKnown 0x77C10005
    SendMessage(new ACDEnterKnownMessage()
    {
        Id = 0x003B,
        Field0 = 0x77C10005,
        Field1 = 0x0000192A,
        Field2 = 0x00000000,
        Field3 = 0x00000000,
        Field4 = new WorldLocationMessageData()
        {
            Field0 = 1f,
            Field1 = new PRTransform()
            {
                Field0 = new Quaternion()
                {
                    Field0 = 0.9228876f,
                    Field1 = new Vector3D()
                    {
                        Field0 = 0f,
                        Field1 = 0f,
                        Field2 = -0.3850694f,
                    },
                },
                Field1 = new Vector3D()
                {
                    Field0 = 2981.73f,
                    Field1 = 2835.009f,
                    Field2 = 24.66344f,
                },
            },
            Field2 = 0x772E0000,
        },
        Field5 = null,
        Field6 = new GBHandle()
        {
            Field0 = -1,
            Field1 = -1,
        },
        Field7 = 0x00000001,
        Field8 = 0x0000192A,
        Field9 = 0x00000000,
        Field10 = 0x00,
        Field12 = 0x0000DBD3,
        Field13 = 0x00000009,
    });

    SendMessage(new AffixMessage()
    {
        Id = 0x0048,
        Field0 = 0x77C10005,
        Field1 = 0x00000001,
        aAffixGBIDs = new int[0]
    {
    },
    });

    SendMessage(new AffixMessage()
    {
        Id = 0x0048,
        Field0 = 0x77C10005,
        Field1 = 0x00000002,
        aAffixGBIDs = new int[0]
    {
    },
    });

    SendMessage(new ACDCollFlagsMessage()
    {
        Id = 0x00A6,
        Field0 = 0x77C10005,
        Field1 = 0x00000080,
    });

    SendMessage(new AttributesSetValuesMessage()
    {
        Id = 0x004D,
        Field0 = 0x77C10005,
        atKeyVals = new NetAttributeKeyValue[6]
    {
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0056], // Hitpoints_Max_Total 
    Int = 0x00000000,
    Float = 1f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0054], // Hitpoints_Max 
    Int = 0x00000000,
    Float = 0.0009994507f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0051], // Hitpoints_Total_From_Level 
    Int = 0x00000000,
    Float = 3.051758E-05f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x004D], // Hitpoints_Cur 
    Int = 0x00000000,
    Float = 0.0009994507f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0043], // TeamID 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0026], // Level 
    Int = 0x00000001,
    Float = 0f,
    },
    },
    });

    SendMessage(new ACDGroupMessage()
    {
        Id = 0x00B8,
        Field0 = 0x77C10005,
        Field1 = -1,
        Field2 = -1,
    });

    SendMessage(new ANNDataMessage()
    {
        Id = 0x003E,
        Field0 = 0x77C10005,
    });

    SendMessage(new ACDTranslateFacingMessage()
    {
        Id = 0x0070,
        Field0 = 0x77C10005,
        Field1 = 5.492608f,
        Field2 = false,
    });
    #endregion
    #region ACDEnterKnown 0x77C50009
    SendMessage(new ACDEnterKnownMessage()
    {
        Id = 0x003B,
        Field0 = 0x77C50009,
        Field1 = 0x0001FD60,
        Field2 = 0x00000000,
        Field3 = 0x00000000,
        Field4 = new WorldLocationMessageData()
        {
            Field0 = 1f,
            Field1 = new PRTransform()
            {
                Field0 = new Quaternion()
                {
                    Field0 = 0.9997472f,
                    Field1 = new Vector3D()
                    {
                        Field0 = 1.02945E-05f,
                        Field1 = 3.217819E-05f,
                        Field2 = 0.0224885f,
                    },
                },
                Field1 = new Vector3D()
                {
                    Field0 = 2970.619f,
                    Field1 = 2789.915f,
                    Field2 = 23.94531f,
                },
            },
            Field2 = 0x772E0000,
        },
        Field5 = null,
        Field6 = new GBHandle()
        {
            Field0 = -1,
            Field1 = -1,
        },
        Field7 = 0x00000001,
        Field8 = 0x0001FD60,
        Field9 = 0x00000000,
        Field10 = 0x00,
        Field12 = 0x0000DBD3,
        Field13 = 0x0000000D,
    });

    SendMessage(new AffixMessage()
    {
        Id = 0x0048,
        Field0 = 0x77C50009,
        Field1 = 0x00000001,
        aAffixGBIDs = new int[0]
    {
    },
    });

    SendMessage(new AffixMessage()
    {
        Id = 0x0048,
        Field0 = 0x77C50009,
        Field1 = 0x00000002,
        aAffixGBIDs = new int[0]
    {
    },
    });

    SendMessage(new ACDCollFlagsMessage()
    {
        Id = 0x00A6,
        Field0 = 0x77C50009,
        Field1 = 0x00000411,
    });

    SendMessage(new AttributesSetValuesMessage()
    {
        Id = 0x004D,
        Field0 = 0x77C50009,
        atKeyVals = new NetAttributeKeyValue[11]
    {
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x02BC], // MinimapActive 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x000FFFFF,
    Attribute = GameAttribute.Attributes[0x01B9], // Buff_Visual_Effect 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x0000F50B,
    Attribute = GameAttribute.Attributes[0x0230], // Buff_Icon_Count0 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x0000F50B,
    Attribute = GameAttribute.Attributes[0x01CC], // Buff_Active 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0056], // Hitpoints_Max_Total 
    Int = 0x00000000,
    Float = 1f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0054], // Hitpoints_Max 
    Int = 0x00000000,
    Float = 0.0009994507f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0051], // Hitpoints_Total_From_Level 
    Int = 0x00000000,
    Float = 3.051758E-05f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x004D], // Hitpoints_Cur 
    Int = 0x00000000,
    Float = 0.0009994507f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0045], // Invulnerable 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0043], // TeamID 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0026], // Level 
    Int = 0x00000001,
    Float = 0f,
    },
    },
    });

    SendMessage(new ACDGroupMessage()
    {
        Id = 0x00B8,
        Field0 = 0x77C50009,
        Field1 = -1,
        Field2 = -1,
    });

    SendMessage(new ANNDataMessage()
    {
        Id = 0x003E,
        Field0 = 0x77C50009,
    });

    SendMessage(new ACDTranslateFacingMessage()
    {
        Id = 0x0070,
        Field0 = 0x77C50009,
        Field1 = 0.04497663f,
        Field2 = false,
    });
    #endregion
    #region ACDEnterKnown 0x77C9000D
    SendMessage(new ACDEnterKnownMessage()
    {
        Id = 0x003B,
        Field0 = 0x77C9000D,
        Field1 = 0x0000157E,
        Field2 = 0x00000000,
        Field3 = 0x00000000,
        Field4 = new WorldLocationMessageData()
        {
            Field0 = 1f,
            Field1 = new PRTransform()
            {
                Field0 = new Quaternion()
                {
                    Field0 = 0.7700912f,
                    Field1 = new Vector3D()
                    {
                        Field0 = 0f,
                        Field1 = 0f,
                        Field2 = 0.637934f,
                    },
                },
                Field1 = new Vector3D()
                {
                    Field0 = 2922f,
                    Field1 = 2787f,
                    Field2 = 23.94531f,
                },
            },
            Field2 = 0x772E0000,
        },
        Field5 = null,
        Field6 = new GBHandle()
        {
            Field0 = -1,
            Field1 = -1,
        },
        Field7 = 0x00000001,
        Field8 = 0x0000157E,
        Field9 = 0x00000000,
        Field10 = 0x00,
        Field12 = 0x0000DBD3,
        Field13 = 0x00000013,
    });

    SendMessage(new AffixMessage()
    {
        Id = 0x0048,
        Field0 = 0x77C9000D,
        Field1 = 0x00000001,
        aAffixGBIDs = new int[0]
    {
    },
    });

    SendMessage(new AffixMessage()
    {
        Id = 0x0048,
        Field0 = 0x77C9000D,
        Field1 = 0x00000002,
        aAffixGBIDs = new int[0]
    {
    },
    });

    SendMessage(new ACDCollFlagsMessage()
    {
        Id = 0x00A6,
        Field0 = 0x77C9000D,
        Field1 = 0x00000000,
    });

    SendMessage(new AttributesSetValuesMessage()
    {
        Id = 0x004D,
        Field0 = 0x77C9000D,
        atKeyVals = new NetAttributeKeyValue[6]
    {
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0056], // Hitpoints_Max_Total 
    Int = 0x00000000,
    Float = 1f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0054], // Hitpoints_Max 
    Int = 0x00000000,
    Float = 0.0009994507f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0051], // Hitpoints_Total_From_Level 
    Int = 0x00000000,
    Float = 3.051758E-05f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x004D], // Hitpoints_Cur 
    Int = 0x00000000,
    Float = 0.0009994507f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0043], // TeamID 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0026], // Level 
    Int = 0x00000001,
    Float = 0f,
    },
    },
    });

    SendMessage(new ACDGroupMessage()
    {
        Id = 0x00B8,
        Field0 = 0x77C9000D,
        Field1 = -1,
        Field2 = -1,
    });

    SendMessage(new ANNDataMessage()
    {
        Id = 0x003E,
        Field0 = 0x77C9000D,
    });

    SendMessage(new ACDTranslateFacingMessage()
    {
        Id = 0x0070,
        Field0 = 0x77C9000D,
        Field1 = 1.383674f,
        Field2 = false,
    });
    #endregion
    #region ACDEnterKnown 0x77CB000F
    SendMessage(new ACDEnterKnownMessage()
    {
        Id = 0x003B,
        Field0 = 0x77CB000F,
        Field1 = 0x0000155A,
        Field2 = 0x00000010,
        Field3 = 0x00000000,
        Field4 = new WorldLocationMessageData()
        {
            Field0 = 1f,
            Field1 = new PRTransform()
            {
                Field0 = new Quaternion()
                {
                    Field0 = 1f,
                    Field1 = new Vector3D()
                    {
                        Field0 = 0f,
                        Field1 = 0f,
                        Field2 = 0f,
                    },
                },
                Field1 = new Vector3D()
                {
                    Field0 = 2924f,
                    Field1 = 2802f,
                    Field2 = 23.94532f,
                },
            },
            Field2 = 0x772E0000,
        },
        Field5 = null,
        Field6 = new GBHandle()
        {
            Field0 = -1,
            Field1 = -1,
        },
        Field7 = 0x00000001,
        Field8 = 0x0000155A,
        Field9 = 0x00000000,
        Field10 = 0x00,
        Field12 = 0x0000DBD3,
        Field13 = 0x00000029,
    });

    SendMessage(new AffixMessage()
    {
        Id = 0x0048,
        Field0 = 0x77CB000F,
        Field1 = 0x00000001,
        aAffixGBIDs = new int[0]
    {
    },
    });

    SendMessage(new AffixMessage()
    {
        Id = 0x0048,
        Field0 = 0x77CB000F,
        Field1 = 0x00000002,
        aAffixGBIDs = new int[0]
    {
    },
    });

    SendMessage(new ACDCollFlagsMessage()
    {
        Id = 0x00A6,
        Field0 = 0x77CB000F,
        Field1 = 0x00000000,
    });

    SendMessage(new AttributesSetValuesMessage()
    {
        Id = 0x004D,
        Field0 = 0x77CB000F,
        atKeyVals = new NetAttributeKeyValue[6]
    {
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0056], // Hitpoints_Max_Total 
    Int = 0x00000000,
    Float = 1f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0054], // Hitpoints_Max 
    Int = 0x00000000,
    Float = 0.0009994507f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0051], // Hitpoints_Total_From_Level 
    Int = 0x00000000,
    Float = 3.051758E-05f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x004D], // Hitpoints_Cur 
    Int = 0x00000000,
    Float = 0.0009994507f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0043], // TeamID 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0026], // Level 
    Int = 0x00000001,
    Float = 0f,
    },
    },
    });

    SendMessage(new ACDGroupMessage()
    {
        Id = 0x00B8,
        Field0 = 0x77CB000F,
        Field1 = -1,
        Field2 = -1,
    });

    SendMessage(new ANNDataMessage()
    {
        Id = 0x003E,
        Field0 = 0x77CB000F,
    });

    SendMessage(new ACDTranslateFacingMessage()
    {
        Id = 0x0070,
        Field0 = 0x77CB000F,
        Field1 = 0f,
        Field2 = false,
    });
    #endregion
    #region ACDEnterKnown 0x77DA001E
    SendMessage(new ACDEnterKnownMessage()
    {
        Id = 0x003B,
        Field0 = 0x77DA001E,
        Field1 = 0x00019527,
        Field2 = 0x00000000,
        Field3 = 0x00000000,
        Field4 = new WorldLocationMessageData()
        {
            Field0 = 1f,
            Field1 = new PRTransform()
            {
                Field0 = new Quaternion()
                {
                    Field0 = 0.9999328f,
                    Field1 = new Vector3D()
                    {
                        Field0 = 0f,
                        Field1 = 0f,
                        Field2 = 0.01160305f,
                    },
                },
                Field1 = new Vector3D()
                {
                    Field0 = 2981.23f,
                    Field1 = 2785.814f,
                    Field2 = 23.94531f,
                },
            },
            Field2 = 0x772E0000,
        },
        Field5 = null,
        Field6 = new GBHandle()
        {
            Field0 = -1,
            Field1 = -1,
        },
        Field7 = 0x00000001,
        Field8 = 0x00019527,
        Field9 = 0x00000000,
        Field10 = 0x00,
        Field12 = 0x0000DBD3,
        Field13 = 0x00000036,
    });

    SendMessage(new AffixMessage()
    {
        Id = 0x0048,
        Field0 = 0x77DA001E,
        Field1 = 0x00000001,
        aAffixGBIDs = new int[0]
    {
    },
    });

    SendMessage(new AffixMessage()
    {
        Id = 0x0048,
        Field0 = 0x77DA001E,
        Field1 = 0x00000002,
        aAffixGBIDs = new int[0]
    {
    },
    });

    SendMessage(new ACDCollFlagsMessage()
    {
        Id = 0x00A6,
        Field0 = 0x77DA001E,
        Field1 = 0x00000001,
    });

    SendMessage(new AttributesSetValuesMessage()
    {
        Id = 0x004D,
        Field0 = 0x77DA001E,
        atKeyVals = new NetAttributeKeyValue[7]
    {
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x01C2], // Could_Have_Ragdolled 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0056], // Hitpoints_Max_Total 
    Int = 0x00000000,
    Float = 1f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0054], // Hitpoints_Max 
    Int = 0x00000000,
    Float = 0.0009994507f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0051], // Hitpoints_Total_From_Level 
    Int = 0x00000000,
    Float = 3.051758E-05f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x004D], // Hitpoints_Cur 
    Int = 0x00000000,
    Float = 0.0009994507f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0043], // TeamID 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0026], // Level 
    Int = 0x00000001,
    Float = 0f,
    },
    },
    });

    SendMessage(new ACDGroupMessage()
    {
        Id = 0x00B8,
        Field0 = 0x77DA001E,
        Field1 = -1,
        Field2 = -1,
    });

    SendMessage(new ANNDataMessage()
    {
        Id = 0x003E,
        Field0 = 0x77DA001E,
    });

    SendMessage(new ACDTranslateFacingMessage()
    {
        Id = 0x0070,
        Field0 = 0x77DA001E,
        Field1 = 0.02320435f,
        Field2 = false,
    });
    #endregion
    #region ACDEnterKnown 0x77DB001F
    SendMessage(new ACDEnterKnownMessage()
    {
        Id = 0x003B,
        Field0 = 0x77DB001F,
        Field1 = 0x00018B03,
        Field2 = 0x00000000,
        Field3 = 0x00000000,
        Field4 = new WorldLocationMessageData()
        {
            Field0 = 1f,
            Field1 = new PRTransform()
            {
                Field0 = new Quaternion()
                {
                    Field0 = 1f,
                    Field1 = new Vector3D()
                    {
                        Field0 = 0f,
                        Field1 = 0f,
                        Field2 = 0f,
                    },
                },
                Field1 = new Vector3D()
                {
                    Field0 = 2915.467f,
                    Field1 = 2845.922f,
                    Field2 = 23.82193f,
                },
            },
            Field2 = 0x772E0000,
        },
        Field5 = null,
        Field6 = new GBHandle()
        {
            Field0 = -1,
            Field1 = -1,
        },
        Field7 = 0x00000001,
        Field8 = 0x00018B03,
        Field9 = 0x00000000,
        Field10 = 0x00,
        Field12 = 0x0000DBD3,
        Field13 = 0x00000037,
    });

    SendMessage(new AffixMessage()
    {
        Id = 0x0048,
        Field0 = 0x77DB001F,
        Field1 = 0x00000001,
        aAffixGBIDs = new int[0]
    {
    },
    });

    SendMessage(new AffixMessage()
    {
        Id = 0x0048,
        Field0 = 0x77DB001F,
        Field1 = 0x00000002,
        aAffixGBIDs = new int[0]
    {
    },
    });

    SendMessage(new ACDCollFlagsMessage()
    {
        Id = 0x00A6,
        Field0 = 0x77DB001F,
        Field1 = 0x00000000,
    });

    SendMessage(new AttributesSetValuesMessage()
    {
        Id = 0x004D,
        Field0 = 0x77DB001F,
        atKeyVals = new NetAttributeKeyValue[1]
    {
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0043], // TeamID 
    Int = 0x0000000A,
    Float = 0f,
    },
    },
    });

    SendMessage(new ACDGroupMessage()
    {
        Id = 0x00B8,
        Field0 = 0x77DB001F,
        Field1 = -1,
        Field2 = -1,
    });

    SendMessage(new ANNDataMessage()
    {
        Id = 0x003E,
        Field0 = 0x77DB001F,
    });

    SendMessage(new ACDTranslateFacingMessage()
    {
        Id = 0x0070,
        Field0 = 0x77DB001F,
        Field1 = 0f,
        Field2 = false,
    });
    #endregion
    #region ACDEnterKnown 0x77E20026
    SendMessage(new ACDEnterKnownMessage()
    {
        Id = 0x003B,
        Field0 = 0x77E20026,
        Field1 = 0x0000157E,
        Field2 = 0x00000000,
        Field3 = 0x00000000,
        Field4 = new WorldLocationMessageData()
        {
            Field0 = 1f,
            Field1 = new PRTransform()
            {
                Field0 = new Quaternion()
                {
                    Field0 = 0.7700912f,
                    Field1 = new Vector3D()
                    {
                        Field0 = 0f,
                        Field1 = 0f,
                        Field2 = 0.637934f,
                    },
                },
                Field1 = new Vector3D()
                {
                    Field0 = 2993.917f,
                    Field1 = 2791.82f,
                    Field2 = 23.94531f,
                },
            },
            Field2 = 0x772E0000,
        },
        Field5 = null,
        Field6 = new GBHandle()
        {
            Field0 = -1,
            Field1 = -1,
        },
        Field7 = 0x00000001,
        Field8 = 0x0000157E,
        Field9 = 0x00000000,
        Field10 = 0x00,
        Field12 = 0x0000DBD3,
        Field13 = 0x0000003E,
    });

    SendMessage(new AffixMessage()
    {
        Id = 0x0048,
        Field0 = 0x77E20026,
        Field1 = 0x00000001,
        aAffixGBIDs = new int[0]
    {
    },
    });

    SendMessage(new AffixMessage()
    {
        Id = 0x0048,
        Field0 = 0x77E20026,
        Field1 = 0x00000002,
        aAffixGBIDs = new int[0]
    {
    },
    });

    SendMessage(new ACDCollFlagsMessage()
    {
        Id = 0x00A6,
        Field0 = 0x77E20026,
        Field1 = 0x00000000,
    });

    SendMessage(new AttributesSetValuesMessage()
    {
        Id = 0x004D,
        Field0 = 0x77E20026,
        atKeyVals = new NetAttributeKeyValue[6]
    {
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0056], // Hitpoints_Max_Total 
    Int = 0x00000000,
    Float = 1f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0054], // Hitpoints_Max 
    Int = 0x00000000,
    Float = 0.0009994507f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0051], // Hitpoints_Total_From_Level 
    Int = 0x00000000,
    Float = 3.051758E-05f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x004D], // Hitpoints_Cur 
    Int = 0x00000000,
    Float = 0.0009994507f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0043], // TeamID 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0026], // Level 
    Int = 0x00000001,
    Float = 0f,
    },
    },
    });

    SendMessage(new ACDGroupMessage()
    {
        Id = 0x00B8,
        Field0 = 0x77E20026,
        Field1 = -1,
        Field2 = -1,
    });

    SendMessage(new ANNDataMessage()
    {
        Id = 0x003E,
        Field0 = 0x77E20026,
    });

    SendMessage(new ACDTranslateFacingMessage()
    {
        Id = 0x0070,
        Field0 = 0x77E20026,
        Field1 = 1.383674f,
        Field2 = false,
    });
    #endregion
    #region ACDEnterKnown 0x77E6002A
    SendMessage(new ACDEnterKnownMessage()
    {
        Id = 0x003B,
        Field0 = 0x77E6002A,
        Field1 = 0x00001243,
        Field2 = 0x00000010,
        Field3 = 0x00000000,
        Field4 = new WorldLocationMessageData()
        {
            Field0 = 1f,
            Field1 = new PRTransform()
            {
                Field0 = new Quaternion()
                {
                    Field0 = 1f,
                    Field1 = new Vector3D()
                    {
                        Field0 = 0f,
                        Field1 = 0f,
                        Field2 = 0f,
                    },
                },
                Field1 = new Vector3D()
                {
                    Field0 = 2990.738f,
                    Field1 = 2828.897f,
                    Field2 = 23.94533f,
                },
            },
            Field2 = 0x772E0000,
        },
        Field5 = null,
        Field6 = new GBHandle()
        {
            Field0 = -1,
            Field1 = -1,
        },
        Field7 = 0x00000001,
        Field8 = 0x00001243,
        Field9 = 0x00000000,
        Field10 = 0x00,
        Field12 = 0x0000DBD3,
        Field13 = 0x00000046,
    });

    SendMessage(new AffixMessage()
    {
        Id = 0x0048,
        Field0 = 0x77E6002A,
        Field1 = 0x00000001,
        aAffixGBIDs = new int[0]
    {
    },
    });

    SendMessage(new AffixMessage()
    {
        Id = 0x0048,
        Field0 = 0x77E6002A,
        Field1 = 0x00000002,
        aAffixGBIDs = new int[0]
    {
    },
    });

    SendMessage(new ACDCollFlagsMessage()
    {
        Id = 0x00A6,
        Field0 = 0x77E6002A,
        Field1 = 0x00000000,
    });

    SendMessage(new AttributesSetValuesMessage()
    {
        Id = 0x004D,
        Field0 = 0x77E6002A,
        atKeyVals = new NetAttributeKeyValue[4]
    {
    new NetAttributeKeyValue()
    {
    Field0 = 0x000FFFFF,
    Attribute = GameAttribute.Attributes[0x01B9], // Buff_Visual_Effect 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x0001F96A,
    Attribute = GameAttribute.Attributes[0x01CC], // Buff_Active 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x0001F96A,
    Attribute = GameAttribute.Attributes[0x0230], // Buff_Icon_Count0 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0043], // TeamID 
    Int = 0x00000000,
    Float = 0f,
    },
    },
    });

    SendMessage(new ACDGroupMessage()
    {
        Id = 0x00B8,
        Field0 = 0x77E6002A,
        Field1 = -1,
        Field2 = -1,
    });

    SendMessage(new ANNDataMessage()
    {
        Id = 0x003E,
        Field0 = 0x77E6002A,
    });

    SendMessage(new ACDTranslateFacingMessage()
    {
        Id = 0x0070,
        Field0 = 0x77E6002A,
        Field1 = 0f,
        Field2 = false,
    });
    #endregion
    #region ACDEnterKnown 0x77E8002C
    SendMessage(new ACDEnterKnownMessage()
    {
        Id = 0x003B,
        Field0 = 0x77E8002C,
        Field1 = 0x0000155A,
        Field2 = 0x00000010,
        Field3 = 0x00000000,
        Field4 = new WorldLocationMessageData()
        {
            Field0 = 1f,
            Field1 = new PRTransform()
            {
                Field0 = new Quaternion()
                {
                    Field0 = 1f,
                    Field1 = new Vector3D()
                    {
                        Field0 = 0f,
                        Field1 = 0f,
                        Field2 = 0f,
                    },
                },
                Field1 = new Vector3D()
                {
                    Field0 = 2973.246f,
                    Field1 = 2827.689f,
                    Field2 = 24.87411f,
                },
            },
            Field2 = 0x772E0000,
        },
        Field5 = null,
        Field6 = new GBHandle()
        {
            Field0 = -1,
            Field1 = -1,
        },
        Field7 = 0x00000001,
        Field8 = 0x0000155A,
        Field9 = 0x00000000,
        Field10 = 0x00,
        Field12 = 0x0000DBD3,
        Field13 = 0x00000049,
    });

    SendMessage(new AffixMessage()
    {
        Id = 0x0048,
        Field0 = 0x77E8002C,
        Field1 = 0x00000001,
        aAffixGBIDs = new int[0]
    {
    },
    });

    SendMessage(new AffixMessage()
    {
        Id = 0x0048,
        Field0 = 0x77E8002C,
        Field1 = 0x00000002,
        aAffixGBIDs = new int[0]
    {
    },
    });

    SendMessage(new ACDCollFlagsMessage()
    {
        Id = 0x00A6,
        Field0 = 0x77E8002C,
        Field1 = 0x00000000,
    });

    SendMessage(new AttributesSetValuesMessage()
    {
        Id = 0x004D,
        Field0 = 0x77E8002C,
        atKeyVals = new NetAttributeKeyValue[6]
    {
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0056], // Hitpoints_Max_Total 
    Int = 0x00000000,
    Float = 1f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0054], // Hitpoints_Max 
    Int = 0x00000000,
    Float = 0.0009994507f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0051], // Hitpoints_Total_From_Level 
    Int = 0x00000000,
    Float = 3.051758E-05f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x004D], // Hitpoints_Cur 
    Int = 0x00000000,
    Float = 0.0009994507f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0043], // TeamID 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0026], // Level 
    Int = 0x00000001,
    Float = 0f,
    },
    },
    });

    SendMessage(new ACDGroupMessage()
    {
        Id = 0x00B8,
        Field0 = 0x77E8002C,
        Field1 = -1,
        Field2 = -1,
    });

    SendMessage(new ANNDataMessage()
    {
        Id = 0x003E,
        Field0 = 0x77E8002C,
    });

    SendMessage(new ACDTranslateFacingMessage()
    {
        Id = 0x0070,
        Field0 = 0x77E8002C,
        Field1 = 0f,
        Field2 = false,
    });
    #endregion
    #region ACDEnterKnown 0x77EB002F
    SendMessage(new ACDEnterKnownMessage()
    {
        Id = 0x003B,
        Field0 = 0x77EB002F,
        Field1 = 0x000261CF,
        Field2 = 0x00000010,
        Field3 = 0x00000000,
        Field4 = new WorldLocationMessageData()
        {
            Field0 = 1f,
            Field1 = new PRTransform()
            {
                Field0 = new Quaternion()
                {
                    Field0 = 1f,
                    Field1 = new Vector3D()
                    {
                        Field0 = 0f,
                        Field1 = 0f,
                        Field2 = 0f,
                    },
                },
                Field1 = new Vector3D()
                {
                    Field0 = 3002.507f,
                    Field1 = 2843.755f,
                    Field2 = 23.94531f,
                },
            },
            Field2 = 0x772E0000,
        },
        Field5 = null,
        Field6 = new GBHandle()
        {
            Field0 = -1,
            Field1 = -1,
        },
        Field7 = 0x00000001,
        Field8 = 0x000261CF,
        Field9 = 0x00000000,
        Field10 = 0x00,
        Field12 = 0x0000DBD3,
        Field13 = 0x0000004E,
    });

    SendMessage(new AffixMessage()
    {
        Id = 0x0048,
        Field0 = 0x77EB002F,
        Field1 = 0x00000001,
        aAffixGBIDs = new int[0]
    {
    },
    });

    SendMessage(new AffixMessage()
    {
        Id = 0x0048,
        Field0 = 0x77EB002F,
        Field1 = 0x00000002,
        aAffixGBIDs = new int[0]
    {
    },
    });

    SendMessage(new ACDCollFlagsMessage()
    {
        Id = 0x00A6,
        Field0 = 0x77EB002F,
        Field1 = 0x00000000,
    });

    SendMessage(new AttributesSetValuesMessage()
    {
        Id = 0x004D,
        Field0 = 0x77EB002F,
        atKeyVals = new NetAttributeKeyValue[6]
    {
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0056], // Hitpoints_Max_Total 
    Int = 0x00000000,
    Float = 1f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0054], // Hitpoints_Max 
    Int = 0x00000000,
    Float = 0.0009994507f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0051], // Hitpoints_Total_From_Level 
    Int = 0x00000000,
    Float = 3.051758E-05f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x004D], // Hitpoints_Cur 
    Int = 0x00000000,
    Float = 0.0009994507f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0043], // TeamID 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0026], // Level 
    Int = 0x00000001,
    Float = 0f,
    },
    },
    });

    SendMessage(new ACDGroupMessage()
    {
        Id = 0x00B8,
        Field0 = 0x77EB002F,
        Field1 = -1,
        Field2 = -1,
    });

    SendMessage(new ANNDataMessage()
    {
        Id = 0x003E,
        Field0 = 0x77EB002F,
    });

    SendMessage(new ACDTranslateFacingMessage()
    {
        Id = 0x0070,
        Field0 = 0x77EB002F,
        Field1 = 0f,
        Field2 = false,
    });
    #endregion
    #region ACDEnterKnown 0x77F00034
    SendMessage(new ACDEnterKnownMessage()
    {
        Id = 0x003B,
        Field0 = 0x77F00034,
        Field1 = 0x00001243,
        Field2 = 0x00000010,
        Field3 = 0x00000000,
        Field4 = new WorldLocationMessageData()
        {
            Field0 = 1f,
            Field1 = new PRTransform()
            {
                Field0 = new Quaternion()
                {
                    Field0 = 0.9329559f,
                    Field1 = new Vector3D()
                    {
                        Field0 = 0f,
                        Field1 = 0f,
                        Field2 = -0.3599906f,
                    },
                },
                Field1 = new Vector3D()
                {
                    Field0 = 2939.892f,
                    Field1 = 2798.311f,
                    Field2 = 23.94532f,
                },
            },
            Field2 = 0x772E0000,
        },
        Field5 = null,
        Field6 = new GBHandle()
        {
            Field0 = -1,
            Field1 = -1,
        },
        Field7 = 0x00000001,
        Field8 = 0x00001243,
        Field9 = 0x00000000,
        Field10 = 0x00,
        Field12 = 0x0000DBD3,
        Field13 = 0x00000057,
    });

    SendMessage(new AffixMessage()
    {
        Id = 0x0048,
        Field0 = 0x77F00034,
        Field1 = 0x00000001,
        aAffixGBIDs = new int[0]
    {
    },
    });

    SendMessage(new AffixMessage()
    {
        Id = 0x0048,
        Field0 = 0x77F00034,
        Field1 = 0x00000002,
        aAffixGBIDs = new int[0]
    {
    },
    });

    SendMessage(new ACDCollFlagsMessage()
    {
        Id = 0x00A6,
        Field0 = 0x77F00034,
        Field1 = 0x00000000,
    });

    SendMessage(new AttributesSetValuesMessage()
    {
        Id = 0x004D,
        Field0 = 0x77F00034,
        atKeyVals = new NetAttributeKeyValue[4]
    {
    new NetAttributeKeyValue()
    {
    Field0 = 0x000FFFFF,
    Attribute = GameAttribute.Attributes[0x01B9], // Buff_Visual_Effect 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x0001F96A,
    Attribute = GameAttribute.Attributes[0x01CC], // Buff_Active 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x0001F96A,
    Attribute = GameAttribute.Attributes[0x0230], // Buff_Icon_Count0 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0043], // TeamID 
    Int = 0x00000000,
    Float = 0f,
    },
    },
    });

    SendMessage(new ACDGroupMessage()
    {
        Id = 0x00B8,
        Field0 = 0x77F00034,
        Field1 = -1,
        Field2 = -1,
    });

    SendMessage(new ANNDataMessage()
    {
        Id = 0x003E,
        Field0 = 0x77F00034,
    });

    SendMessage(new ACDTranslateFacingMessage()
    {
        Id = 0x0070,
        Field0 = 0x77F00034,
        Field1 = 5.546649f,
        Field2 = false,
    });
    #endregion
    #region ACDEnterKnown 0x77F20036
    SendMessage(new ACDEnterKnownMessage()
    {
        Id = 0x003B,
        Field0 = 0x77F20036,
        Field1 = 0x0002B875,
        Field2 = 0x00000010,
        Field3 = 0x00000000,
        Field4 = new WorldLocationMessageData()
        {
            Field0 = 0.632705f,
            Field1 = new PRTransform()
            {
                Field0 = new Quaternion()
                {
                    Field0 = 0.7219135f,
                    Field1 = new Vector3D()
                    {
                        Field0 = 0f,
                        Field1 = 0f,
                        Field2 = 0.6919833f,
                    },
                },
                Field1 = new Vector3D()
                {
                    Field0 = 2993.722f,
                    Field1 = 2782.086f,
                    Field2 = 24.32831f,
                },
            },
            Field2 = 0x772E0000,
        },
        Field5 = null,
        Field6 = new GBHandle()
        {
            Field0 = -1,
            Field1 = -1,
        },
        Field7 = 0x00000001,
        Field8 = 0x0002B875,
        Field9 = 0x00000000,
        Field10 = 0x00,
        Field12 = 0x0000DBD3,
        Field13 = 0x00000059,
    });

    SendMessage(new AffixMessage()
    {
        Id = 0x0048,
        Field0 = 0x77F20036,
        Field1 = 0x00000001,
        aAffixGBIDs = new int[0]
    {
    },
    });

    SendMessage(new AffixMessage()
    {
        Id = 0x0048,
        Field0 = 0x77F20036,
        Field1 = 0x00000002,
        aAffixGBIDs = new int[0]
    {
    },
    });

    SendMessage(new PortalSpecifierMessage()
    {
        Id = 0x004B,
        Field0 = 0x77F20036,
        Field1 = new ResolvedPortalDestination()
        {
            snoWorld = 0x0001AB32,
            Field1 = 0x000000AC,
            snoDestLevelArea = 0x0001AB91,
        },
    });

    SendMessage(new ACDCollFlagsMessage()
    {
        Id = 0x00A6,
        Field0 = 0x77F20036,
        Field1 = 0x00000001,
    });

    SendMessage(new AttributesSetValuesMessage()
    {
        Id = 0x004D,
        Field0 = 0x77F20036,
        atKeyVals = new NetAttributeKeyValue[7]
    {
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x02BC], // MinimapActive 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0056], // Hitpoints_Max_Total 
    Int = 0x00000000,
    Float = 1f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0054], // Hitpoints_Max 
    Int = 0x00000000,
    Float = 0.0009994507f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0051], // Hitpoints_Total_From_Level 
    Int = 0x00000000,
    Float = 3.051758E-05f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x004D], // Hitpoints_Cur 
    Int = 0x00000000,
    Float = 0.0009994507f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0043], // TeamID 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0026], // Level 
    Int = 0x00000001,
    Float = 0f,
    },
    },
    });

    SendMessage(new ACDGroupMessage()
    {
        Id = 0x00B8,
        Field0 = 0x77F20036,
        Field1 = -1,
        Field2 = -1,
    });

    SendMessage(new ANNDataMessage()
    {
        Id = 0x003E,
        Field0 = 0x77F20036,
    });

    SendMessage(new ACDTranslateFacingMessage()
    {
        Id = 0x0070,
        Field0 = 0x77F20036,
        Field1 = 1.528479f,
        Field2 = false,
    });
    #endregion
    #region ACDEnterKnown 0x77F30037
    SendMessage(new ACDEnterKnownMessage()
    {
        Id = 0x003B,
        Field0 = 0x77F30037,
        Field1 = 0x00001243,
        Field2 = 0x00000010,
        Field3 = 0x00000000,
        Field4 = new WorldLocationMessageData()
        {
            Field0 = 1f,
            Field1 = new PRTransform()
            {
                Field0 = new Quaternion()
                {
                    Field0 = 1f,
                    Field1 = new Vector3D()
                    {
                        Field0 = 0f,
                        Field1 = 0f,
                        Field2 = 0f,
                    },
                },
                Field1 = new Vector3D()
                {
                    Field0 = 2921.905f,
                    Field1 = 2782.769f,
                    Field2 = 24.68567f,
                },
            },
            Field2 = 0x772E0000,
        },
        Field5 = null,
        Field6 = new GBHandle()
        {
            Field0 = -1,
            Field1 = -1,
        },
        Field7 = 0x00000001,
        Field8 = 0x00001243,
        Field9 = 0x00000000,
        Field10 = 0x00,
        Field12 = 0x0000DBD3,
        Field13 = 0x0000005B,
    });

    SendMessage(new AffixMessage()
    {
        Id = 0x0048,
        Field0 = 0x77F30037,
        Field1 = 0x00000001,
        aAffixGBIDs = new int[0]
    {
    },
    });

    SendMessage(new AffixMessage()
    {
        Id = 0x0048,
        Field0 = 0x77F30037,
        Field1 = 0x00000002,
        aAffixGBIDs = new int[0]
    {
    },
    });

    SendMessage(new ACDCollFlagsMessage()
    {
        Id = 0x00A6,
        Field0 = 0x77F30037,
        Field1 = 0x00000000,
    });

    SendMessage(new AttributesSetValuesMessage()
    {
        Id = 0x004D,
        Field0 = 0x77F30037,
        atKeyVals = new NetAttributeKeyValue[4]
    {
    new NetAttributeKeyValue()
    {
    Field0 = 0x000FFFFF,
    Attribute = GameAttribute.Attributes[0x01B9], // Buff_Visual_Effect 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x0001F96A,
    Attribute = GameAttribute.Attributes[0x01CC], // Buff_Active 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x0001F96A,
    Attribute = GameAttribute.Attributes[0x0230], // Buff_Icon_Count0 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0043], // TeamID 
    Int = 0x00000000,
    Float = 0f,
    },
    },
    });

    SendMessage(new ACDGroupMessage()
    {
        Id = 0x00B8,
        Field0 = 0x77F30037,
        Field1 = -1,
        Field2 = -1,
    });

    SendMessage(new ANNDataMessage()
    {
        Id = 0x003E,
        Field0 = 0x77F30037,
    });

    SendMessage(new ACDTranslateFacingMessage()
    {
        Id = 0x0070,
        Field0 = 0x77F30037,
        Field1 = 0f,
        Field2 = false,
    });
    #endregion
    #region ACDEnterKnown 0x77F7003B
    SendMessage(new ACDEnterKnownMessage()
    {
        Id = 0x003B,
        Field0 = 0x77F7003B,
        Field1 = 0x00001243,
        Field2 = 0x00000010,
        Field3 = 0x00000000,
        Field4 = new WorldLocationMessageData()
        {
            Field0 = 1f,
            Field1 = new PRTransform()
            {
                Field0 = new Quaternion()
                {
                    Field0 = 1f,
                    Field1 = new Vector3D()
                    {
                        Field0 = 0f,
                        Field1 = 0f,
                        Field2 = 0f,
                    },
                },
                Field1 = new Vector3D()
                {
                    Field0 = 2989.97f,
                    Field1 = 2849.609f,
                    Field2 = 23.94532f,
                },
            },
            Field2 = 0x772E0000,
        },
        Field5 = null,
        Field6 = new GBHandle()
        {
            Field0 = -1,
            Field1 = -1,
        },
        Field7 = 0x00000001,
        Field8 = 0x00001243,
        Field9 = 0x00000000,
        Field10 = 0x00,
        Field12 = 0x0000DBD3,
        Field13 = 0x00000068,
    });

    SendMessage(new AffixMessage()
    {
        Id = 0x0048,
        Field0 = 0x77F7003B,
        Field1 = 0x00000001,
        aAffixGBIDs = new int[0]
    {
    },
    });

    SendMessage(new AffixMessage()
    {
        Id = 0x0048,
        Field0 = 0x77F7003B,
        Field1 = 0x00000002,
        aAffixGBIDs = new int[0]
    {
    },
    });

    SendMessage(new ACDCollFlagsMessage()
    {
        Id = 0x00A6,
        Field0 = 0x77F7003B,
        Field1 = 0x00000000,
    });

    SendMessage(new AttributesSetValuesMessage()
    {
        Id = 0x004D,
        Field0 = 0x77F7003B,
        atKeyVals = new NetAttributeKeyValue[4]
    {
    new NetAttributeKeyValue()
    {
    Field0 = 0x000FFFFF,
    Attribute = GameAttribute.Attributes[0x01B9], // Buff_Visual_Effect 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x0001F96A,
    Attribute = GameAttribute.Attributes[0x01CC], // Buff_Active 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x0001F96A,
    Attribute = GameAttribute.Attributes[0x0230], // Buff_Icon_Count0 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0043], // TeamID 
    Int = 0x00000000,
    Float = 0f,
    },
    },
    });

    SendMessage(new ACDGroupMessage()
    {
        Id = 0x00B8,
        Field0 = 0x77F7003B,
        Field1 = -1,
        Field2 = -1,
    });

    SendMessage(new ANNDataMessage()
    {
        Id = 0x003E,
        Field0 = 0x77F7003B,
    });

    SendMessage(new ACDTranslateFacingMessage()
    {
        Id = 0x0070,
        Field0 = 0x77F7003B,
        Field1 = 0f,
        Field2 = false,
    });
    #endregion
    #region ACDEnterKnown 0x77F8003C
    SendMessage(new ACDEnterKnownMessage()
    {
        Id = 0x003B,
        Field0 = 0x77F8003C,
        Field1 = 0x00001243,
        Field2 = 0x00000010,
        Field3 = 0x00000000,
        Field4 = new WorldLocationMessageData()
        {
            Field0 = 1f,
            Field1 = new PRTransform()
            {
                Field0 = new Quaternion()
                {
                    Field0 = 1f,
                    Field1 = new Vector3D()
                    {
                        Field0 = 0f,
                        Field1 = 0f,
                        Field2 = 0f,
                    },
                },
                Field1 = new Vector3D()
                {
                    Field0 = 3013.641f,
                    Field1 = 2800.703f,
                    Field2 = 23.94532f,
                },
            },
            Field2 = 0x772E0000,
        },
        Field5 = null,
        Field6 = new GBHandle()
        {
            Field0 = -1,
            Field1 = -1,
        },
        Field7 = 0x00000001,
        Field8 = 0x00001243,
        Field9 = 0x00000000,
        Field10 = 0x00,
        Field12 = 0x0000DBD3,
        Field13 = 0x00000069,
    });

    SendMessage(new AffixMessage()
    {
        Id = 0x0048,
        Field0 = 0x77F8003C,
        Field1 = 0x00000001,
        aAffixGBIDs = new int[0]
    {
    },
    });

    SendMessage(new AffixMessage()
    {
        Id = 0x0048,
        Field0 = 0x77F8003C,
        Field1 = 0x00000002,
        aAffixGBIDs = new int[0]
    {
    },
    });

    SendMessage(new ACDCollFlagsMessage()
    {
        Id = 0x00A6,
        Field0 = 0x77F8003C,
        Field1 = 0x00000000,
    });

    SendMessage(new AttributesSetValuesMessage()
    {
        Id = 0x004D,
        Field0 = 0x77F8003C,
        atKeyVals = new NetAttributeKeyValue[4]
    {
    new NetAttributeKeyValue()
    {
    Field0 = 0x000FFFFF,
    Attribute = GameAttribute.Attributes[0x01B9], // Buff_Visual_Effect 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x0001F96A,
    Attribute = GameAttribute.Attributes[0x01CC], // Buff_Active 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x0001F96A,
    Attribute = GameAttribute.Attributes[0x0230], // Buff_Icon_Count0 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0043], // TeamID 
    Int = 0x00000000,
    Float = 0f,
    },
    },
    });

    SendMessage(new ACDGroupMessage()
    {
        Id = 0x00B8,
        Field0 = 0x77F8003C,
        Field1 = -1,
        Field2 = -1,
    });

    SendMessage(new ANNDataMessage()
    {
        Id = 0x003E,
        Field0 = 0x77F8003C,
    });

    SendMessage(new ACDTranslateFacingMessage()
    {
        Id = 0x0070,
        Field0 = 0x77F8003C,
        Field1 = 0f,
        Field2 = false,
    });
    #endregion
    #region ACDEnterKnown 0x77F9003D
    SendMessage(new ACDEnterKnownMessage()
    {
        Id = 0x003B,
        Field0 = 0x77F9003D,
        Field1 = 0x00001243,
        Field2 = 0x00000010,
        Field3 = 0x00000000,
        Field4 = new WorldLocationMessageData()
        {
            Field0 = 1f,
            Field1 = new PRTransform()
            {
                Field0 = new Quaternion()
                {
                    Field0 = 1f,
                    Field1 = new Vector3D()
                    {
                        Field0 = 0f,
                        Field1 = 0f,
                        Field2 = 0f,
                    },
                },
                Field1 = new Vector3D()
                {
                    Field0 = 2972.528f,
                    Field1 = 2799.993f,
                    Field2 = 23.94532f,
                },
            },
            Field2 = 0x772E0000,
        },
        Field5 = null,
        Field6 = new GBHandle()
        {
            Field0 = -1,
            Field1 = -1,
        },
        Field7 = 0x00000001,
        Field8 = 0x00001243,
        Field9 = 0x00000000,
        Field10 = 0x00,
        Field12 = 0x0000DBD3,
        Field13 = 0x0000006A,
    });

    SendMessage(new AffixMessage()
    {
        Id = 0x0048,
        Field0 = 0x77F9003D,
        Field1 = 0x00000001,
        aAffixGBIDs = new int[0]
    {
    },
    });

    SendMessage(new AffixMessage()
    {
        Id = 0x0048,
        Field0 = 0x77F9003D,
        Field1 = 0x00000002,
        aAffixGBIDs = new int[0]
    {
    },
    });

    SendMessage(new ACDCollFlagsMessage()
    {
        Id = 0x00A6,
        Field0 = 0x77F9003D,
        Field1 = 0x00000000,
    });

    SendMessage(new AttributesSetValuesMessage()
    {
        Id = 0x004D,
        Field0 = 0x77F9003D,
        atKeyVals = new NetAttributeKeyValue[4]
    {
    new NetAttributeKeyValue()
    {
    Field0 = 0x000FFFFF,
    Attribute = GameAttribute.Attributes[0x01B9], // Buff_Visual_Effect 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x0001F96A,
    Attribute = GameAttribute.Attributes[0x01CC], // Buff_Active 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x0001F96A,
    Attribute = GameAttribute.Attributes[0x0230], // Buff_Icon_Count0 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0043], // TeamID 
    Int = 0x00000000,
    Float = 0f,
    },
    },
    });

    SendMessage(new ACDGroupMessage()
    {
        Id = 0x00B8,
        Field0 = 0x77F9003D,
        Field1 = -1,
        Field2 = -1,
    });

    SendMessage(new ANNDataMessage()
    {
        Id = 0x003E,
        Field0 = 0x77F9003D,
    });

    SendMessage(new ACDTranslateFacingMessage()
    {
        Id = 0x0070,
        Field0 = 0x77F9003D,
        Field1 = 0f,
        Field2 = false,
    });
    #endregion
    #region ACDEnterKnown 0x77FC0040
    SendMessage(new ACDEnterKnownMessage()
    {
        Id = 0x003B,
        Field0 = 0x77FC0040,
        Field1 = 0x00001243,
        Field2 = 0x00000010,
        Field3 = 0x00000000,
        Field4 = new WorldLocationMessageData()
        {
            Field0 = 1f,
            Field1 = new PRTransform()
            {
                Field0 = new Quaternion()
                {
                    Field0 = 1f,
                    Field1 = new Vector3D()
                    {
                        Field0 = 0f,
                        Field1 = 0f,
                        Field2 = 0f,
                    },
                },
                Field1 = new Vector3D()
                {
                    Field0 = 2894.062f,
                    Field1 = 2819.54f,
                    Field2 = 23.94533f,
                },
            },
            Field2 = 0x772E0000,
        },
        Field5 = null,
        Field6 = new GBHandle()
        {
            Field0 = -1,
            Field1 = -1,
        },
        Field7 = 0x00000001,
        Field8 = 0x00001243,
        Field9 = 0x00000000,
        Field10 = 0x00,
        Field12 = 0x0000DBD3,
        Field13 = 0x0000006D,
    });

    SendMessage(new AffixMessage()
    {
        Id = 0x0048,
        Field0 = 0x77FC0040,
        Field1 = 0x00000001,
        aAffixGBIDs = new int[0]
    {
    },
    });

    SendMessage(new AffixMessage()
    {
        Id = 0x0048,
        Field0 = 0x77FC0040,
        Field1 = 0x00000002,
        aAffixGBIDs = new int[0]
    {
    },
    });

    SendMessage(new ACDCollFlagsMessage()
    {
        Id = 0x00A6,
        Field0 = 0x77FC0040,
        Field1 = 0x00000000,
    });

    SendMessage(new AttributesSetValuesMessage()
    {
        Id = 0x004D,
        Field0 = 0x77FC0040,
        atKeyVals = new NetAttributeKeyValue[4]
    {
    new NetAttributeKeyValue()
    {
    Field0 = 0x000FFFFF,
    Attribute = GameAttribute.Attributes[0x01B9], // Buff_Visual_Effect 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x0001F96A,
    Attribute = GameAttribute.Attributes[0x01CC], // Buff_Active 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x0001F96A,
    Attribute = GameAttribute.Attributes[0x0230], // Buff_Icon_Count0 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0043], // TeamID 
    Int = 0x00000000,
    Float = 0f,
    },
    },
    });

    SendMessage(new ACDGroupMessage()
    {
        Id = 0x00B8,
        Field0 = 0x77FC0040,
        Field1 = -1,
        Field2 = -1,
    });

    SendMessage(new ANNDataMessage()
    {
        Id = 0x003E,
        Field0 = 0x77FC0040,
    });

    SendMessage(new ACDTranslateFacingMessage()
    {
        Id = 0x0070,
        Field0 = 0x77FC0040,
        Field1 = 0f,
        Field2 = false,
    });
    #endregion
    #region ACDEnterKnown 0x77FD0041
    SendMessage(new ACDEnterKnownMessage()
    {
        Id = 0x003B,
        Field0 = 0x77FD0041,
        Field1 = 0x00001243,
        Field2 = 0x00000010,
        Field3 = 0x00000000,
        Field4 = new WorldLocationMessageData()
        {
            Field0 = 1f,
            Field1 = new PRTransform()
            {
                Field0 = new Quaternion()
                {
                    Field0 = 1f,
                    Field1 = new Vector3D()
                    {
                        Field0 = 0f,
                        Field1 = 0f,
                        Field2 = 0f,
                    },
                },
                Field1 = new Vector3D()
                {
                    Field0 = 2938.309f,
                    Field1 = 2814.277f,
                    Field2 = 23.87319f,
                },
            },
            Field2 = 0x772E0000,
        },
        Field5 = null,
        Field6 = new GBHandle()
        {
            Field0 = -1,
            Field1 = -1,
        },
        Field7 = 0x00000001,
        Field8 = 0x00001243,
        Field9 = 0x00000000,
        Field10 = 0x00,
        Field12 = 0x0000DBD3,
        Field13 = 0x0000006E,
    });

    SendMessage(new AffixMessage()
    {
        Id = 0x0048,
        Field0 = 0x77FD0041,
        Field1 = 0x00000001,
        aAffixGBIDs = new int[0]
    {
    },
    });

    SendMessage(new AffixMessage()
    {
        Id = 0x0048,
        Field0 = 0x77FD0041,
        Field1 = 0x00000002,
        aAffixGBIDs = new int[0]
    {
    },
    });

    SendMessage(new ACDCollFlagsMessage()
    {
        Id = 0x00A6,
        Field0 = 0x77FD0041,
        Field1 = 0x00000000,
    });

    SendMessage(new AttributesSetValuesMessage()
    {
        Id = 0x004D,
        Field0 = 0x77FD0041,
        atKeyVals = new NetAttributeKeyValue[4]
    {
    new NetAttributeKeyValue()
    {
    Field0 = 0x000FFFFF,
    Attribute = GameAttribute.Attributes[0x01B9], // Buff_Visual_Effect 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x0001F96A,
    Attribute = GameAttribute.Attributes[0x01CC], // Buff_Active 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x0001F96A,
    Attribute = GameAttribute.Attributes[0x0230], // Buff_Icon_Count0 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0043], // TeamID 
    Int = 0x00000000,
    Float = 0f,
    },
    },
    });

    SendMessage(new ACDGroupMessage()
    {
        Id = 0x00B8,
        Field0 = 0x77FD0041,
        Field1 = -1,
        Field2 = -1,
    });

    SendMessage(new ANNDataMessage()
    {
        Id = 0x003E,
        Field0 = 0x77FD0041,
    });

    SendMessage(new ACDTranslateFacingMessage()
    {
        Id = 0x0070,
        Field0 = 0x77FD0041,
        Field1 = 0f,
        Field2 = false,
    });
    #endregion
    #region ACDEnterKnown 0x77FE0042
    SendMessage(new ACDEnterKnownMessage()
    {
        Id = 0x003B,
        Field0 = 0x77FE0042,
        Field1 = 0x00021463,
        Field2 = 0x00000000,
        Field3 = 0x00000000,
        Field4 = new WorldLocationMessageData()
        {
            Field0 = 1f,
            Field1 = new PRTransform()
            {
                Field0 = new Quaternion()
                {
                    Field0 = 0.07654059f,
                    Field1 = new Vector3D()
                    {
                        Field0 = 0f,
                        Field1 = 0f,
                        Field2 = 0.9970666f,
                    },
                },
                Field1 = new Vector3D()
                {
                    Field0 = 2920.87f,
                    Field1 = 2779.655f,
                    Field2 = 23.94531f,
                },
            },
            Field2 = 0x772E0000,
        },
        Field5 = null,
        Field6 = new GBHandle()
        {
            Field0 = -1,
            Field1 = -1,
        },
        Field7 = 0x00000001,
        Field8 = 0x00021463,
        Field9 = 0x00000000,
        Field10 = 0x00,
        Field12 = 0x0000DBD3,
        Field13 = 0x00000072,
    });

    SendMessage(new AffixMessage()
    {
        Id = 0x0048,
        Field0 = 0x77FE0042,
        Field1 = 0x00000001,
        aAffixGBIDs = new int[0]
    {
    },
    });

    SendMessage(new AffixMessage()
    {
        Id = 0x0048,
        Field0 = 0x77FE0042,
        Field1 = 0x00000002,
        aAffixGBIDs = new int[0]
    {
    },
    });

    SendMessage(new ACDCollFlagsMessage()
    {
        Id = 0x00A6,
        Field0 = 0x77FE0042,
        Field1 = 0x00000411,
    });

    SendMessage(new AttributesSetValuesMessage()
    {
        Id = 0x004D,
        Field0 = 0x77FE0042,
        atKeyVals = new NetAttributeKeyValue[5]
    {
    new NetAttributeKeyValue()
    {
    Field0 = 0x000FFFFF,
    Attribute = GameAttribute.Attributes[0x01B9], // Buff_Visual_Effect 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x0000F50B,
    Attribute = GameAttribute.Attributes[0x0230], // Buff_Icon_Count0 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x0000F50B,
    Attribute = GameAttribute.Attributes[0x01CC], // Buff_Active 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0045], // Invulnerable 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0043], // TeamID 
    Int = 0x00000000,
    Float = 0f,
    },
    },
    });

    SendMessage(new ACDGroupMessage()
    {
        Id = 0x00B8,
        Field0 = 0x77FE0042,
        Field1 = -1,
        Field2 = -1,
    });

    SendMessage(new ANNDataMessage()
    {
        Id = 0x003E,
        Field0 = 0x77FE0042,
    });

    SendMessage(new ACDTranslateFacingMessage()
    {
        Id = 0x0070,
        Field0 = 0x77FE0042,
        Field1 = 2.988367f,
        Field2 = false,
    });
    #endregion
    #region ACDEnterKnown 0x78010045
    SendMessage(new ACDEnterKnownMessage()
    {
        Id = 0x003B,
        Field0 = 0x78010045,
        Field1 = 0x0001B603,
        Field2 = 0x00000000,
        Field3 = 0x00000000,
        Field4 = new WorldLocationMessageData()
        {
            Field0 = 1f,
            Field1 = new PRTransform()
            {
                Field0 = new Quaternion()
                {
                    Field0 = 0.8008201f,
                    Field1 = new Vector3D()
                    {
                        Field0 = 0f,
                        Field1 = 0f,
                        Field2 = -0.598905f,
                    },
                },
                Field1 = new Vector3D()
                {
                    Field0 = 3030.794f,
                    Field1 = 2770.09f,
                    Field2 = 23.94532f,
                },
            },
            Field2 = 0x772E0000,
        },
        Field5 = null,
        Field6 = new GBHandle()
        {
            Field0 = -1,
            Field1 = -1,
        },
        Field7 = 0x00000001,
        Field8 = 0x0001B603,
        Field9 = 0x00000000,
        Field10 = 0x00,
        Field12 = 0x0000DBD3,
        Field13 = 0x00000075,
    });

    SendMessage(new AffixMessage()
    {
        Id = 0x0048,
        Field0 = 0x78010045,
        Field1 = 0x00000001,
        aAffixGBIDs = new int[0]
    {
    },
    });

    SendMessage(new AffixMessage()
    {
        Id = 0x0048,
        Field0 = 0x78010045,
        Field1 = 0x00000002,
        aAffixGBIDs = new int[0]
    {
    },
    });

    SendMessage(new ACDCollFlagsMessage()
    {
        Id = 0x00A6,
        Field0 = 0x78010045,
        Field1 = 0x00000C21,
    });

    SendMessage(new AttributesSetValuesMessage()
    {
        Id = 0x004D,
        Field0 = 0x78010045,
        atKeyVals = new NetAttributeKeyValue[4]
    {
    new NetAttributeKeyValue()
    {
    Field0 = 0x000FFFFF,
    Attribute = GameAttribute.Attributes[0x01B9], // Buff_Visual_Effect 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x0001F972,
    Attribute = GameAttribute.Attributes[0x01CC], // Buff_Active 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x0001F972,
    Attribute = GameAttribute.Attributes[0x0230], // Buff_Icon_Count0 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0043], // TeamID 
    Int = 0x00000000,
    Float = 0f,
    },
    },
    });

    SendMessage(new ACDGroupMessage()
    {
        Id = 0x00B8,
        Field0 = 0x78010045,
        Field1 = -1,
        Field2 = -1,
    });

    SendMessage(new ANNDataMessage()
    {
        Id = 0x003E,
        Field0 = 0x78010045,
    });

    SendMessage(new ACDTranslateFacingMessage()
    {
        Id = 0x0070,
        Field0 = 0x78010045,
        Field1 = 4.99892f,
        Field2 = false,
    });
    #endregion
    #region ACDEnterKnown 0x78020046
    SendMessage(new ACDEnterKnownMessage()
    {
        Id = 0x003B,
        Field0 = 0x78020046,
        Field1 = 0x0001E345,
        Field2 = 0x00000010,
        Field3 = 0x00000000,
        Field4 = new WorldLocationMessageData()
        {
            Field0 = 1f,
            Field1 = new PRTransform()
            {
                Field0 = new Quaternion()
                {
                    Field0 = 0.9238795f,
                    Field1 = new Vector3D()
                    {
                        Field0 = 2.799833E-05f,
                        Field1 = 6.323846E-05f,
                        Field2 = 0.3826835f,
                    },
                },
                Field1 = new Vector3D()
                {
                    Field0 = 2998.413f,
                    Field1 = 2835.342f,
                    Field2 = 24.36805f,
                },
            },
            Field2 = 0x772E0000,
        },
        Field5 = null,
        Field6 = new GBHandle()
        {
            Field0 = -1,
            Field1 = -1,
        },
        Field7 = 0x00000001,
        Field8 = 0x0001E345,
        Field9 = 0x00000000,
        Field10 = 0x00,
        Field12 = 0x0000DBD3,
        Field13 = 0x00000077,
    });

    SendMessage(new AffixMessage()
    {
        Id = 0x0048,
        Field0 = 0x78020046,
        Field1 = 0x00000001,
        aAffixGBIDs = new int[0]
    {
    },
    });

    SendMessage(new AffixMessage()
    {
        Id = 0x0048,
        Field0 = 0x78020046,
        Field1 = 0x00000002,
        aAffixGBIDs = new int[0]
    {
    },
    });

    SendMessage(new ACDCollFlagsMessage()
    {
        Id = 0x00A6,
        Field0 = 0x78020046,
        Field1 = 0x00000001,
    });

    SendMessage(new AttributesSetValuesMessage()
    {
        Id = 0x004D,
        Field0 = 0x78020046,
        atKeyVals = new NetAttributeKeyValue[6]
    {
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0056], // Hitpoints_Max_Total 
    Int = 0x00000000,
    Float = 1f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0054], // Hitpoints_Max 
    Int = 0x00000000,
    Float = 0.0009994507f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0051], // Hitpoints_Total_From_Level 
    Int = 0x00000000,
    Float = 3.051758E-05f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x004D], // Hitpoints_Cur 
    Int = 0x00000000,
    Float = 0.0009994507f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0043], // TeamID 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0026], // Level 
    Int = 0x00000001,
    Float = 0f,
    },
    },
    });

    SendMessage(new ACDGroupMessage()
    {
        Id = 0x00B8,
        Field0 = 0x78020046,
        Field1 = -1,
        Field2 = -1,
    });

    SendMessage(new ANNDataMessage()
    {
        Id = 0x003E,
        Field0 = 0x78020046,
    });

    SendMessage(new ACDTranslateFacingMessage()
    {
        Id = 0x0070,
        Field0 = 0x78020046,
        Field1 = 0.7854109f,
        Field2 = false,
    });
    #endregion
    #region ACDEnterKnown 0x78030047
    SendMessage(new ACDEnterKnownMessage()
    {
        Id = 0x003B,
        Field0 = 0x78030047,
        Field1 = 0x0001E344,
        Field2 = 0x00000010,
        Field3 = 0x00000000,
        Field4 = new WorldLocationMessageData()
        {
            Field0 = 1f,
            Field1 = new PRTransform()
            {
                Field0 = new Quaternion()
                {
                    Field0 = 0.9238795f,
                    Field1 = new Vector3D()
                    {
                        Field0 = 2.799833E-05f,
                        Field1 = 6.323846E-05f,
                        Field2 = 0.3826835f,
                    },
                },
                Field1 = new Vector3D()
                {
                    Field0 = 2982.224f,
                    Field1 = 2818.409f,
                    Field2 = 23.98143f,
                },
            },
            Field2 = 0x772E0000,
        },
        Field5 = null,
        Field6 = new GBHandle()
        {
            Field0 = -1,
            Field1 = -1,
        },
        Field7 = 0x00000001,
        Field8 = 0x0001E344,
        Field9 = 0x00000000,
        Field10 = 0x00,
        Field12 = 0x0000DBD3,
        Field13 = 0x00000078,
    });

    SendMessage(new AffixMessage()
    {
        Id = 0x0048,
        Field0 = 0x78030047,
        Field1 = 0x00000001,
        aAffixGBIDs = new int[0]
    {
    },
    });

    SendMessage(new AffixMessage()
    {
        Id = 0x0048,
        Field0 = 0x78030047,
        Field1 = 0x00000002,
        aAffixGBIDs = new int[0]
    {
    },
    });

    SendMessage(new ACDCollFlagsMessage()
    {
        Id = 0x00A6,
        Field0 = 0x78030047,
        Field1 = 0x00000001,
    });

    SendMessage(new AttributesSetValuesMessage()
    {
        Id = 0x004D,
        Field0 = 0x78030047,
        atKeyVals = new NetAttributeKeyValue[6]
    {
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0056], // Hitpoints_Max_Total 
    Int = 0x00000000,
    Float = 1f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0054], // Hitpoints_Max 
    Int = 0x00000000,
    Float = 0.0009994507f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0051], // Hitpoints_Total_From_Level 
    Int = 0x00000000,
    Float = 3.051758E-05f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x004D], // Hitpoints_Cur 
    Int = 0x00000000,
    Float = 0.0009994507f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0043], // TeamID 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0026], // Level 
    Int = 0x00000001,
    Float = 0f,
    },
    },
    });

    SendMessage(new ACDGroupMessage()
    {
        Id = 0x00B8,
        Field0 = 0x78030047,
        Field1 = -1,
        Field2 = -1,
    });

    SendMessage(new ANNDataMessage()
    {
        Id = 0x003E,
        Field0 = 0x78030047,
    });

    SendMessage(new ACDTranslateFacingMessage()
    {
        Id = 0x0070,
        Field0 = 0x78030047,
        Field1 = 0.7854109f,
        Field2 = false,
    });
    #endregion
    #region ACDEnterKnown 0x78040048
    SendMessage(new ACDEnterKnownMessage()
    {
        Id = 0x003B,
        Field0 = 0x78040048,
        Field1 = 0x0001E343,
        Field2 = 0x00000010,
        Field3 = 0x00000000,
        Field4 = new WorldLocationMessageData()
        {
            Field0 = 1f,
            Field1 = new PRTransform()
            {
                Field0 = new Quaternion()
                {
                    Field0 = 0.9238795f,
                    Field1 = new Vector3D()
                    {
                        Field0 = 2.799833E-05f,
                        Field1 = 6.323846E-05f,
                        Field2 = 0.3826835f,
                    },
                },
                Field1 = new Vector3D()
                {
                    Field0 = 2982.041f,
                    Field1 = 2851.404f,
                    Field2 = 24.49633f,
                },
            },
            Field2 = 0x772E0000,
        },
        Field5 = null,
        Field6 = new GBHandle()
        {
            Field0 = -1,
            Field1 = -1,
        },
        Field7 = 0x00000001,
        Field8 = 0x0001E343,
        Field9 = 0x00000000,
        Field10 = 0x00,
        Field12 = 0x0000DBD3,
        Field13 = 0x00000079,
    });

    SendMessage(new AffixMessage()
    {
        Id = 0x0048,
        Field0 = 0x78040048,
        Field1 = 0x00000001,
        aAffixGBIDs = new int[0]
    {
    },
    });

    SendMessage(new AffixMessage()
    {
        Id = 0x0048,
        Field0 = 0x78040048,
        Field1 = 0x00000002,
        aAffixGBIDs = new int[0]
    {
    },
    });

    SendMessage(new ACDCollFlagsMessage()
    {
        Id = 0x00A6,
        Field0 = 0x78040048,
        Field1 = 0x00000001,
    });

    SendMessage(new AttributesSetValuesMessage()
    {
        Id = 0x004D,
        Field0 = 0x78040048,
        atKeyVals = new NetAttributeKeyValue[6]
    {
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0056], // Hitpoints_Max_Total 
    Int = 0x00000000,
    Float = 1f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0054], // Hitpoints_Max 
    Int = 0x00000000,
    Float = 0.0009994507f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0051], // Hitpoints_Total_From_Level 
    Int = 0x00000000,
    Float = 3.051758E-05f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x004D], // Hitpoints_Cur 
    Int = 0x00000000,
    Float = 0.0009994507f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0043], // TeamID 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0026], // Level 
    Int = 0x00000001,
    Float = 0f,
    },
    },
    });

    SendMessage(new ACDGroupMessage()
    {
        Id = 0x00B8,
        Field0 = 0x78040048,
        Field1 = -1,
        Field2 = -1,
    });

    SendMessage(new ANNDataMessage()
    {
        Id = 0x003E,
        Field0 = 0x78040048,
    });

    SendMessage(new ACDTranslateFacingMessage()
    {
        Id = 0x0070,
        Field0 = 0x78040048,
        Field1 = 0.7854109f,
        Field2 = false,
    });
    #endregion
    #region ACDEnterKnown 0x78050049
    SendMessage(new ACDEnterKnownMessage()
    {
        Id = 0x003B,
        Field0 = 0x78050049,
        Field1 = 0x0001E342,
        Field2 = 0x00000010,
        Field3 = 0x00000000,
        Field4 = new WorldLocationMessageData()
        {
            Field0 = 1f,
            Field1 = new PRTransform()
            {
                Field0 = new Quaternion()
                {
                    Field0 = 0.9238795f,
                    Field1 = new Vector3D()
                    {
                        Field0 = 2.799833E-05f,
                        Field1 = 6.323846E-05f,
                        Field2 = 0.3826835f,
                    },
                },
                Field1 = new Vector3D()
                {
                    Field0 = 2965.15f,
                    Field1 = 2834.933f,
                    Field2 = 24.04564f,
                },
            },
            Field2 = 0x772E0000,
        },
        Field5 = null,
        Field6 = new GBHandle()
        {
            Field0 = -1,
            Field1 = -1,
        },
        Field7 = 0x00000001,
        Field8 = 0x0001E342,
        Field9 = 0x00000000,
        Field10 = 0x00,
        Field12 = 0x0000DBD3,
        Field13 = 0x0000007A,
    });

    SendMessage(new AffixMessage()
    {
        Id = 0x0048,
        Field0 = 0x78050049,
        Field1 = 0x00000001,
        aAffixGBIDs = new int[0]
    {
    },
    });

    SendMessage(new AffixMessage()
    {
        Id = 0x0048,
        Field0 = 0x78050049,
        Field1 = 0x00000002,
        aAffixGBIDs = new int[0]
    {
    },
    });

    SendMessage(new ACDCollFlagsMessage()
    {
        Id = 0x00A6,
        Field0 = 0x78050049,
        Field1 = 0x00000001,
    });

    SendMessage(new AttributesSetValuesMessage()
    {
        Id = 0x004D,
        Field0 = 0x78050049,
        atKeyVals = new NetAttributeKeyValue[6]
    {
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0056], // Hitpoints_Max_Total 
    Int = 0x00000000,
    Float = 1f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0054], // Hitpoints_Max 
    Int = 0x00000000,
    Float = 0.0009994507f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0051], // Hitpoints_Total_From_Level 
    Int = 0x00000000,
    Float = 3.051758E-05f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x004D], // Hitpoints_Cur 
    Int = 0x00000000,
    Float = 0.0009994507f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0043], // TeamID 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0026], // Level 
    Int = 0x00000001,
    Float = 0f,
    },
    },
    });

    SendMessage(new ACDGroupMessage()
    {
        Id = 0x00B8,
        Field0 = 0x78050049,
        Field1 = -1,
        Field2 = -1,
    });

    SendMessage(new ANNDataMessage()
    {
        Id = 0x003E,
        Field0 = 0x78050049,
    });

    SendMessage(new ACDTranslateFacingMessage()
    {
        Id = 0x0070,
        Field0 = 0x78050049,
        Field1 = 0.7854109f,
        Field2 = false,
    });
    #endregion
    #region ACDEnterKnown 0x7806004A
    SendMessage(new ACDEnterKnownMessage()
    {
        Id = 0x003B,
        Field0 = 0x7806004A,
        Field1 = 0x0002EC04,
        Field2 = 0x00000000,
        Field3 = 0x00000000,
        Field4 = new WorldLocationMessageData()
        {
            Field0 = 1f,
            Field1 = new PRTransform()
            {
                Field0 = new Quaternion()
                {
                    Field0 = 1f,
                    Field1 = new Vector3D()
                    {
                        Field0 = 0f,
                        Field1 = 0f,
                        Field2 = 0f,
                    },
                },
                Field1 = new Vector3D()
                {
                    Field0 = 2985.959f,
                    Field1 = 2795.399f,
                    Field2 = 23.94531f,
                },
            },
            Field2 = 0x772E0000,
        },
        Field5 = null,
        Field6 = new GBHandle()
        {
            Field0 = -1,
            Field1 = -1,
        },
        Field7 = 0x00000001,
        Field8 = 0x0002EC04,
        Field9 = 0x00000000,
        Field10 = 0x00,
        Field12 = 0x0000DBD3,
        Field13 = 0x00000080,
    });

    SendMessage(new AffixMessage()
    {
        Id = 0x0048,
        Field0 = 0x7806004A,
        Field1 = 0x00000001,
        aAffixGBIDs = new int[0]
    {
    },
    });

    SendMessage(new AffixMessage()
    {
        Id = 0x0048,
        Field0 = 0x7806004A,
        Field1 = 0x00000002,
        aAffixGBIDs = new int[0]
    {
    },
    });

    SendMessage(new ACDCollFlagsMessage()
    {
        Id = 0x00A6,
        Field0 = 0x7806004A,
        Field1 = 0x00000000,
    });

    SendMessage(new AttributesSetValuesMessage()
    {
        Id = 0x004D,
        Field0 = 0x7806004A,
        atKeyVals = new NetAttributeKeyValue[6]
    {
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0056], // Hitpoints_Max_Total 
    Int = 0x00000000,
    Float = 1f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0054], // Hitpoints_Max 
    Int = 0x00000000,
    Float = 0.0009994507f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0051], // Hitpoints_Total_From_Level 
    Int = 0x00000000,
    Float = 3.051758E-05f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x004D], // Hitpoints_Cur 
    Int = 0x00000000,
    Float = 0.0009994507f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0043], // TeamID 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0026], // Level 
    Int = 0x00000001,
    Float = 0f,
    },
    },
    });

    SendMessage(new ACDGroupMessage()
    {
        Id = 0x00B8,
        Field0 = 0x7806004A,
        Field1 = -1,
        Field2 = -1,
    });

    SendMessage(new ANNDataMessage()
    {
        Id = 0x003E,
        Field0 = 0x7806004A,
    });

    SendMessage(new ACDTranslateFacingMessage()
    {
        Id = 0x0070,
        Field0 = 0x7806004A,
        Field1 = 0f,
        Field2 = false,
    });
    #endregion
    #region ACDEnterKnown 0x7809004D
    SendMessage(new ACDEnterKnownMessage()
    {
        Id = 0x003B,
        Field0 = 0x7809004D,
        Field1 = 0x00001243,
        Field2 = 0x00000010,
        Field3 = 0x00000000,
        Field4 = new WorldLocationMessageData()
        {
            Field0 = 1f,
            Field1 = new PRTransform()
            {
                Field0 = new Quaternion()
                {
                    Field0 = 0.977448f,
                    Field1 = new Vector3D()
                    {
                        Field0 = 0f,
                        Field1 = 0f,
                        Field2 = 0.2111764f,
                    },
                },
                Field1 = new Vector3D()
                {
                    Field0 = 2943.416f,
                    Field1 = 2819.508f,
                    Field2 = 23.84636f,
                },
            },
            Field2 = 0x772E0000,
        },
        Field5 = null,
        Field6 = new GBHandle()
        {
            Field0 = -1,
            Field1 = -1,
        },
        Field7 = 0x00000001,
        Field8 = 0x00001243,
        Field9 = 0x00000000,
        Field10 = 0x00,
        Field12 = 0x0000DBD3,
        Field13 = 0x0000008A,
    });

    SendMessage(new AffixMessage()
    {
        Id = 0x0048,
        Field0 = 0x7809004D,
        Field1 = 0x00000001,
        aAffixGBIDs = new int[0]
    {
    },
    });

    SendMessage(new AffixMessage()
    {
        Id = 0x0048,
        Field0 = 0x7809004D,
        Field1 = 0x00000002,
        aAffixGBIDs = new int[0]
    {
    },
    });

    SendMessage(new ACDCollFlagsMessage()
    {
        Id = 0x00A6,
        Field0 = 0x7809004D,
        Field1 = 0x00000000,
    });

    SendMessage(new AttributesSetValuesMessage()
    {
        Id = 0x004D,
        Field0 = 0x7809004D,
        atKeyVals = new NetAttributeKeyValue[4]
    {
    new NetAttributeKeyValue()
    {
    Field0 = 0x000FFFFF,
    Attribute = GameAttribute.Attributes[0x01B9], // Buff_Visual_Effect 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x0001F96A,
    Attribute = GameAttribute.Attributes[0x01CC], // Buff_Active 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Field0 = 0x0001F96A,
    Attribute = GameAttribute.Attributes[0x0230], // Buff_Icon_Count0 
    Int = 0x00000001,
    Float = 0f,
    },
    new NetAttributeKeyValue()
    {
    Attribute = GameAttribute.Attributes[0x0043], // TeamID 
    Int = 0x00000000,
    Float = 0f,
    },
    },
    });

    SendMessage(new ACDGroupMessage()
    {
        Id = 0x00B8,
        Field0 = 0x7809004D,
        Field1 = -1,
        Field2 = -1,
    });

    SendMessage(new ANNDataMessage()
    {
        Id = 0x003E,
        Field0 = 0x7809004D,
    });

    SendMessage(new ACDTranslateFacingMessage()
    {
        Id = 0x0070,
        Field0 = 0x7809004D,
        Field1 = 0.4255699f,
        Field2 = false,
    });
    #endregion
    #region ACDEnterKnown 0x781C0060
    SendMessage(new ACDEnterKnownMessage()
    {
        Id = 0x003B,
        Field0 = 0x781C0060,
        Field1 = 0x0002B273,
        Field2 = 0x00000000,
        Field3 = 0x00000000,
        Field4 = new WorldLocationMessageData()
        {
            Field0 = 1f,
            Field1 = new PRTransform()
            {
                Field0 = new Quaternion()
                {
                    Field0 = 1f,
                    Field1 = new Vector3D()
                    {
                        Field0 = 0f,
                        Field1 = 0f,
                        Field2 = 0f,
                    },
                },
                Field1 = new Vector3D()
                {
                    Field0 = 3020.224f,
                    Field1 = 2861.322f,
                    Field2 = 23.94533f,
                },
            },
            Field2 = 0x772E0000,
        },
        Field5 = null,
        Field6 = new GBHandle()
        {
            Field0 = -1,
            Field1 = -1,
        },
        Field7 = 0x00000001,
        Field8 = 0x0002B273,
        Field9 = 0x00000000,
        Field10 = 0x00,
        Field12 = 0x0002B900,
        Field13 = 0x00000005,
    });

    SendMessage(new AffixMessage()
    {
        Id = 0x0048,
        Field0 = 0x781C0060,
        Field1 = 0x00000001,
        aAffixGBIDs = new int[0]
    {
    },
    });

    SendMessage(new AffixMessage()
    {
        Id = 0x0048,
        Field0 = 0x781C0060,
        Field1 = 0x00000002,
        aAffixGBIDs = new int[0]
    {
    },
    });

    SendMessage(new ACDCollFlagsMessage()
    {
        Id = 0x00A6,
        Field0 = 0x781C0060,
        Field1 = 0x00000001,
    });

    SendMessage(new ACDGroupMessage()
    {
        Id = 0x00B8,
        Field0 = 0x781C0060,
        Field1 = -1,
        Field2 = -1,
    });

    SendMessage(new ANNDataMessage()
    {
        Id = 0x003E,
        Field0 = 0x781C0060,
    });

    SendMessage(new ACDTranslateFacingMessage()
    {
        Id = 0x0070,
        Field0 = 0x781C0060,
        Field1 = 0f,
        Field2 = false,
    });
    #endregion
    #region ACDEnterKnown 0x781E0062
    SendMessage(new ACDEnterKnownMessage()
    {
        Id = 0x003B,
        Field0 = 0x781E0062,
        Field1 = 0x0002B28B,
        Field2 = 0x00000000,
        Field3 = 0x00000000,
        Field4 = new WorldLocationMessageData()
        {
            Field0 = 1f,
            Field1 = new PRTransform()
            {
                Field0 = new Quaternion()
                {
                    Field0 = -0.3870942f,
                    Field1 = new Vector3D()
                    {
                        Field0 = 0f,
                        Field1 = 0f,
                        Field2 = 0.9220403f,
                    },
                },
                Field1 = new Vector3D()
                {
                    Field0 = 3026.482f,
                    Field1 = 2856.695f,
                    Field2 = 23.94533f,
                },
            },
            Field2 = 0x772E0000,
        },
        Field5 = null,
        Field6 = new GBHandle()
        {
            Field0 = -1,
            Field1 = -1,
        },
        Field7 = 0x00000001,
        Field8 = 0x0002B28B,
        Field9 = 0x00000000,
        Field10 = 0x00,
        Field12 = 0x0002B900,
        Field13 = 0x00000007,
    });

    SendMessage(new AffixMessage()
    {
        Id = 0x0048,
        Field0 = 0x781E0062,
        Field1 = 0x00000001,
        aAffixGBIDs = new int[0]
    {
    },
    });

    SendMessage(new AffixMessage()
    {
        Id = 0x0048,
        Field0 = 0x781E0062,
        Field1 = 0x00000002,
        aAffixGBIDs = new int[0]
    {
    },
    });

    SendMessage(new ACDCollFlagsMessage()
    {
        Id = 0x00A6,
        Field0 = 0x781E0062,
        Field1 = 0x00000001,
    });

    SendMessage(new ACDGroupMessage()
    {
        Id = 0x00B8,
        Field0 = 0x781E0062,
        Field1 = -1,
        Field2 = -1,
    });

    SendMessage(new ANNDataMessage()
    {
        Id = 0x003E,
        Field0 = 0x781E0062,
    });

    SendMessage(new ACDTranslateFacingMessage()
    {
        Id = 0x0070,
        Field0 = 0x781E0062,
        Field1 = 3.936559f,
        Field2 = false,
    });
    #endregion
    #region SNONameData
    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000021,
            Field1 = 0x00008243,
        },
    });

    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000021,
            Field1 = 0x00008244,
        },
    });

    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000021,
            Field1 = 0x00008245,
        },
    });

    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000021,
            Field1 = 0x0000823E,
        },
    });

    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000021,
            Field1 = 0x00000770,
        },
    });

    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000021,
            Field1 = 0x0000823F,
        },
    });

    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000021,
            Field1 = 0x00008239,
        },
    });

    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000021,
            Field1 = 0x0000823A,
        },
    });

    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000021,
            Field1 = 0x000160A7,
        },
    });

    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000021,
            Field1 = 0x0000824D,
        },
    });

    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000001,
            Field1 = 0x00001243,
        },
    });

    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000001,
            Field1 = 0x0000157E,
        },
    });

    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000001,
            Field1 = 0x000018A8,
        },
    });

    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000001,
            Field1 = 0x0000192A,
        },
    });

    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000001,
            Field1 = 0x00001243,
        },
    });

    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000001,
            Field1 = 0x0001FD60,
        },
    });

    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000001,
            Field1 = 0x00032361,
        },
    });

    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000001,
            Field1 = 0x0000157E,
        },
    });

    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000001,
            Field1 = 0x0000155A,
        },
    });

    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000001,
            Field1 = 0x00001243,
        },
    });

    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000001,
            Field1 = 0x00001224,
        },
    });

    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000001,
            Field1 = 0x00019527,
        },
    });

    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000001,
            Field1 = 0x00018B03,
        },
    });

    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000001,
            Field1 = 0x000227BE,
        },
    });

    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000001,
            Field1 = 0x0000157E,
        },
    });

    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000001,
            Field1 = 0x00001243,
        },
    });

    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000001,
            Field1 = 0x0000155A,
        },
    });

    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000001,
            Field1 = 0x000261CF,
        },
    });

    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000001,
            Field1 = 0x00013871,
        },
    });

    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000001,
            Field1 = 0x00001243,
        },
    });

    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000001,
            Field1 = 0x0002B875,
        },
    });

    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000001,
            Field1 = 0x00001243,
        },
    });

    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000001,
            Field1 = 0x000250CC,
        },
    });

    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000001,
            Field1 = 0x0001B186,
        },
    });

    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000001,
            Field1 = 0x00001243,
        },
    });

    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000001,
            Field1 = 0x00001243,
        },
    });

    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000001,
            Field1 = 0x00001243,
        },
    });

    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000001,
            Field1 = 0x00001243,
        },
    });

    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000001,
            Field1 = 0x00001243,
        },
    });

    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000001,
            Field1 = 0x00001243,
        },
    });

    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000001,
            Field1 = 0x00001243,
        },
    });

    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000001,
            Field1 = 0x00001243,
        },
    });

    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000001,
            Field1 = 0x00021463,
        },
    });

    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000001,
            Field1 = 0x000228C4,
        },
    });

    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000001,
            Field1 = 0x0001B603,
        },
    });

    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000001,
            Field1 = 0x0001E345,
        },
    });

    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000001,
            Field1 = 0x0001E344,
        },
    });

    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000001,
            Field1 = 0x0001E343,
        },
    });

    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000001,
            Field1 = 0x0001E342,
        },
    });

    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000001,
            Field1 = 0x0002EC04,
        },
    });

    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000001,
            Field1 = 0x00001243,
        },
    });

    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000001,
            Field1 = 0x00001243,
        },
    });

    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000001,
            Field1 = 0x00001243,
        },
    });

    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000001,
            Field1 = 0x00001243,
        },
    });

    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000001,
            Field1 = 0x0002B281,
        },
    });

    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000001,
            Field1 = 0x0002B284,
        },
    });

    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000001,
            Field1 = 0x0002B289,
        },
    });

    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000001,
            Field1 = 0x0002B27C,
        },
    });

    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000001,
            Field1 = 0x0002B28B,
        },
    });

    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000001,
            Field1 = 0x0002B273,
        },
    });

    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000001,
            Field1 = 0x0002B278,
        },
    });

    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000001,
            Field1 = 0x0002B28B,
        },
    });

    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000001,
            Field1 = 0x00026989,
        },
    });

    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000001,
            Field1 = 0x0002B27B,
        },
    });

    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000001,
            Field1 = 0x00014A3E,
        },
    });

    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000001,
            Field1 = 0x000341A5,
        },
    });

    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000001,
            Field1 = 0x000341A6,
        },
    });

    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000001,
            Field1 = 0x0002AEA4,
        },
    });

    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000001,
            Field1 = 0x00001243,
        },
    });

    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000001,
            Field1 = 0x00001243,
        },
    });

    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000001,
            Field1 = 0x000314B7,
        },
    });

    SendMessage(new SNONameDataMessage()
    {
        Id = 0x00D3,
        Field0 = new SNOName()
        {
            Field0 = 0x00000001,
            Field1 = 0x000314B8,
        },
    });
    #endregion
    #region Set individual attributes of 0x789700DB
    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789700DB,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x0056], // Hitpoints_Max_Total 
            Int = 0x00000000,
            Float = 1f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789700DB,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x0054], // Hitpoints_Max 
            Int = 0x00000000,
            Float = 0.0009994507f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789700DB,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x004D], // Hitpoints_Cur 
            Int = 0x00000000,
            Float = 0.0009994507f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789700DB,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x0026], // Level 
            Int = 0x00000001,
            Float = 0f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789700DB,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x0043], // TeamID 
            Int = 0x00000001,
            Float = 0f,
        },
    });
    #endregion
    #region ACDWorldPosition 0x789700DB
    SendMessage(new ACDWorldPositionMessage()
    {
        Id = 0x003F,
        Field0 = 0x789700DB,
        Field1 = new WorldLocationMessageData()
        {
            Field0 = 1.11f,
            Field1 = new PRTransform()
            {
                Field0 = new Quaternion()
                {
                    Field0 = 0.9330626f,
                    Field1 = new Vector3D()
                    {
                        Field0 = 0f,
                        Field1 = 0f,
                        Field2 = 0.3597142f,
                    },
                },
                Field1 = new Vector3D()
                {
                    Field0 = 3108.125f,
                    Field1 = 2882.163f,
                    Field2 = 64.49878f,
                },
            },
            Field2 = 0x772E0000,
        },
    });
    #endregion
    #region Set Individual Attributes of 0x789800DC
    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789800DC,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x0056], // Hitpoints_Max_Total 
            Int = 0x00000000,
            Float = 1f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789800DC,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x0054], // Hitpoints_Max 
            Int = 0x00000000,
            Float = 0.0009994507f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789800DC,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x004D], // Hitpoints_Cur 
            Int = 0x00000000,
            Float = 0.0009994507f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789800DC,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x0026], // Level 
            Int = 0x00000001,
            Float = 0f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789800DC,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x0043], // TeamID 
            Int = 0x00000001,
            Float = 0f,
        },
    });
    #endregion
    #region ACDWorldPosition 0x789800DC
    SendMessage(new ACDWorldPositionMessage()
    {
        Id = 0x003F,
        Field0 = 0x789800DC,
        Field1 = new WorldLocationMessageData()
        {
            Field0 = 1f,
            Field1 = new PRTransform()
            {
                Field0 = new Quaternion()
                {
                    Field0 = 0.05940768f,
                    Field1 = new Vector3D()
                    {
                        Field0 = 0f,
                        Field1 = 0f,
                        Field2 = 0.9982339f,
                    },
                },
                Field1 = new Vector3D()
                {
                    Field0 = 3143.104f,
                    Field1 = 2829.936f,
                    Field2 = 59.07556f,
                },
            },
            Field2 = 0x772E0000,
        },
    });
    #endregion
    #region Set individual attributes of 0x789900DD
    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789900DD,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x0056], // Hitpoints_Max_Total 
            Int = 0x00000000,
            Float = 1f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789900DD,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x0054], // Hitpoints_Max 
            Int = 0x00000000,
            Float = 0.0009994507f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789900DD,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x004D], // Hitpoints_Cur 
            Int = 0x00000000,
            Float = 0.0009994507f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789900DD,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x0026], // Level 
            Int = 0x00000001,
            Float = 0f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789900DD,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x0043], // TeamID 
            Int = 0x00000001,
            Float = 0f,
        },
    });
    #endregion
    #region ACDWorldPositionMessage 0x789900DD
    SendMessage(new ACDWorldPositionMessage()
    {
        Id = 0x003F,
        Field0 = 0x789900DD,
        Field1 = new WorldLocationMessageData()
        {
            Field0 = 1f,
            Field1 = new PRTransform()
            {
                Field0 = new Quaternion()
                {
                    Field0 = 0.9878305f,
                    Field1 = new Vector3D()
                    {
                        Field0 = 0f,
                        Field1 = 0f,
                        Field2 = 0.1555345f,
                    },
                },
                Field1 = new Vector3D()
                {
                    Field0 = 3109.516f,
                    Field1 = 2803.876f,
                    Field2 = 59.07428f,
                },
            },
            Field2 = 0x772E0000,
        },
    });
    #endregion
    #region Attributes 0x789A00DE
    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789A00DE,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x0056], // Hitpoints_Max_Total 
            Int = 0x00000000,
            Float = 1f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789A00DE,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x0054], // Hitpoints_Max 
            Int = 0x00000000,
            Float = 0.0009994507f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789A00DE,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x004D], // Hitpoints_Cur 
            Int = 0x00000000,
            Float = 0.0009994507f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789A00DE,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x0026], // Level 
            Int = 0x00000001,
            Float = 0f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789A00DE,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x0043], // TeamID 
            Int = 0x00000001,
            Float = 0f,
        },
    });
    #endregion
    #region ACDWorldPosition 0x789A00DE
    SendMessage(new ACDWorldPositionMessage()
    {
        Id = 0x003F,
        Field0 = 0x789A00DE,
        Field1 = new WorldLocationMessageData()
        {
            Field0 = 1f,
            Field1 = new PRTransform()
            {
                Field0 = new Quaternion()
                {
                    Field0 = -0.7652696f,
                    Field1 = new Vector3D()
                    {
                        Field0 = 0f,
                        Field1 = 0f,
                        Field2 = 0.6437099f,
                    },
                },
                Field1 = new Vector3D()
                {
                    Field0 = 3113.659f,
                    Field1 = 2803.692f,
                    Field2 = 73.26618f,
                },
            },
            Field2 = 0x772E0000,
        },
    });
    #endregion
    #region Attributes 0x789B00DF
    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789B00DF,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x0056], // Hitpoints_Max_Total 
            Int = 0x00000000,
            Float = 1f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789B00DF,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x0054], // Hitpoints_Max 
            Int = 0x00000000,
            Float = 0.0009994507f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789B00DF,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x004D], // Hitpoints_Cur 
            Int = 0x00000000,
            Float = 0.0009994507f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789B00DF,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x0026], // Level 
            Int = 0x00000001,
            Float = 0f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789B00DF,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x0043], // TeamID 
            Int = 0x00000001,
            Float = 0f,
        },
    });
    #endregion
    #region ACDWorldPosition 0x789B00DF
    SendMessage(new ACDWorldPositionMessage()
    {
        Id = 0x003F,
        Field0 = 0x789B00DF,
        Field1 = new WorldLocationMessageData()
        {
            Field0 = 1f,
            Field1 = new PRTransform()
            {
                Field0 = new Quaternion()
                {
                    Field0 = 0.9976531f,
                    Field1 = new Vector3D()
                    {
                        Field0 = 0.009601643f,
                        Field1 = -0.0006524475f,
                        Field2 = -0.06779216f,
                    },
                },
                Field1 = new Vector3D()
                {
                    Field0 = 3127.913f,
                    Field1 = 2830.662f,
                    Field2 = 59.07558f,
                },
            },
            Field2 = 0x772E0000,
        },
    });
    #endregion
    #region Attributes 0x789C00E0
    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789C00E0,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x0056], // Hitpoints_Max_Total 
            Int = 0x00000000,
            Float = 1f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789C00E0,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x0054], // Hitpoints_Max 
            Int = 0x00000000,
            Float = 0.0009994507f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789C00E0,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x004D], // Hitpoints_Cur 
            Int = 0x00000000,
            Float = 0.0009994507f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789C00E0,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x0026], // Level 
            Int = 0x00000001,
            Float = 0f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789C00E0,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x0043], // TeamID 
            Int = 0x00000001,
            Float = 0f,
        },
    });
    #endregion
    #region ACDWorldPosition 0x789C00E0
    SendMessage(new ACDWorldPositionMessage()
    {
        Id = 0x003F,
        Field0 = 0x789C00E0,
        Field1 = new WorldLocationMessageData()
        {
            Field0 = 1f,
            Field1 = new PRTransform()
            {
                Field0 = new Quaternion()
                {
                    Field0 = -0.4954694f,
                    Field1 = new Vector3D()
                    {
                        Field0 = 0f,
                        Field1 = 0f,
                        Field2 = -0.8686254f,
                    },
                },
                Field1 = new Vector3D()
                {
                    Field0 = 3120.037f,
                    Field1 = 2864.345f,
                    Field2 = 59.07556f,
                },
            },
            Field2 = 0x772E0000,
        },
    });
    #endregion
    #region Attributes 0x789E00E2
    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Field0 = 0x0000CE11,
            Attribute = GameAttribute.Attributes[0x0041], // Skill 
            Int = 0x00000001,
            Float = 0f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x007F], // Casting_Speed_Total 
            Int = 0x00000000,
            Float = 1f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Field0 = 0x00000003,
            Attribute = GameAttribute.Attributes[0x0061], // Resource_Max_Total 
            Int = 0x43480000,
            Float = 0f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x009D], // Damage_Weapon_Min_Total_All 
            Int = 0x00000000,
            Float = 2f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x007D], // Casting_Speed 
            Int = 0x00000000,
            Float = 1f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Field0 = 0x0000CE11,
            Attribute = GameAttribute.Attributes[0x0042], // Skill_Total 
            Int = 0x00000001,
            Float = 0f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x005C], // Resource_Type_Primary 
            Int = 0x00000003,
            Float = 0f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x007B], // Movement_Scalar_Total 
            Int = 0x00000000,
            Float = 1f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x02BA], // IsTrialActor 
            Int = 0x00000001,
            Float = 0f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Field0 = 0x000FFFFF,
            Attribute = GameAttribute.Attributes[0x01B9], // Buff_Visual_Effect 
            Int = 0x00000001,
            Float = 0f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x0099], // Damage_Weapon_Delta_Total_All 
            Int = 0x00000000,
            Float = 1f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x0079], // Movement_Scalar_Capped_Total 
            Int = 0x00000000,
            Float = 1f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x01F8], // SkillKit 
            Int = 0x00008AFA,
            Float = 0f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x0078], // Movement_Scalar_Subtotal 
            Int = 0x00000000,
            Float = 1f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Field0 = 0x00033C40,
            Attribute = GameAttribute.Attributes[0x01CC], // Buff_Active 
            Int = 0x00000001,
            Float = 0f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x00D7], // Immobolize 
            Int = 0x00000001,
            Float = 0f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x00D6], // Untargetable 
            Int = 0x00000001,
            Float = 0f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Field0 = 0x000076B7,
            Attribute = GameAttribute.Attributes[0x0042], // Skill_Total 
            Int = 0x00000001,
            Float = 0f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x0056], // Hitpoints_Max_Total 
            Int = 0x00000000,
            Float = 76f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Field0 = 0x000076B7,
            Attribute = GameAttribute.Attributes[0x0041], // Skill 
            Int = 0x00000001,
            Float = 0f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Field0 = 0x00000003,
            Attribute = GameAttribute.Attributes[0x006B], // Resource_Effective_Max 
            Int = 0x00000000,
            Float = 200f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x0074], // Running_Rate_Total 
            Int = 0x00000000,
            Float = 0.3598633f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Field0 = 0x00000000,
            Attribute = GameAttribute.Attributes[0x018B], // Damage_Weapon_Min_Total_MainHand 
            Int = 0x00000000,
            Float = 2f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x0054], // Hitpoints_Max 
            Int = 0x00000000,
            Float = 40f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x0073], // Walking_Rate_Total 
            Int = 0x00000000,
            Float = 0.2797852f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Field0 = 0x000006DF,
            Attribute = GameAttribute.Attributes[0x0041], // Skill 
            Int = 0x00000001,
            Float = 0f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Field0 = 0x0000CE11,
            Attribute = GameAttribute.Attributes[0x01CC], // Buff_Active 
            Int = 0x00000001,
            Float = 0f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x01D2], // CantStartDisplayedPowers 
            Int = 0x00000001,
            Float = 0f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Field0 = 0x000216FA,
            Attribute = GameAttribute.Attributes[0x0042], // Skill_Total 
            Int = 0x00000001,
            Float = 0f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Field0 = 0x000176C4,
            Attribute = GameAttribute.Attributes[0x0041], // Skill 
            Int = 0x00000001,
            Float = 0f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Field0 = 0x00000000,
            Attribute = GameAttribute.Attributes[0x018D], // Damage_Weapon_Delta_Total_MainHand 
            Int = 0x00000000,
            Float = 1f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Field0 = 0x000216FA,
            Attribute = GameAttribute.Attributes[0x0041], // Skill 
            Int = 0x00000001,
            Float = 0f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Field0 = 0x000176C4,
            Attribute = GameAttribute.Attributes[0x0042], // Skill_Total 
            Int = 0x00000001,
            Float = 0f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Field0 = 0x00000000,
            Attribute = GameAttribute.Attributes[0x008E], // Damage_Delta_Total 
            Int = 0x00000000,
            Float = 1f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x0050], // Hitpoints_Total_From_Vitality 
            Int = 0x00000000,
            Float = 36f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Field0 = 0x000006DF,
            Attribute = GameAttribute.Attributes[0x0042], // Skill_Total 
            Int = 0x00000001,
            Float = 0f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x0070], // Running_Rate 
            Int = 0x00000000,
            Float = 0.3598633f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x004F], // Hitpoints_Factor_Vitality 
            Int = 0x00000000,
            Float = 4f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Field0 = 0x00000000,
            Attribute = GameAttribute.Attributes[0x0190], // Damage_Weapon_Min_Total_CurrentHand 
            Int = 0x00000000,
            Float = 2f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x006F], // Walking_Rate 
            Int = 0x00000000,
            Float = 0.2797852f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x018F], // Attacks_Per_Second_Item_CurrentHand 
            Int = 0x00000000,
            Float = 1.199219f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Field0 = 0x0000CE11,
            Attribute = GameAttribute.Attributes[0x0230], // Buff_Icon_Count0 
            Int = 0x00000001,
            Float = 0f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Field0 = 0x000000DE,
            Attribute = GameAttribute.Attributes[0x003C], // Resistance 
            Int = 0x00000000,
            Float = 0.5f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Field0 = 0x00000000,
            Attribute = GameAttribute.Attributes[0x0091], // Damage_Min_Total 
            Int = 0x00000000,
            Float = 2f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Field0 = 0x00000000,
            Attribute = GameAttribute.Attributes[0x0191], // Damage_Weapon_Delta_Total_CurrentHand 
            Int = 0x00000000,
            Float = 1f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x006E], // Movement_Scalar 
            Int = 0x00000000,
            Float = 1f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x004E], // Hitpoints_Factor_Level 
            Int = 0x00000000,
            Float = 4f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x004D], // Hitpoints_Cur 
            Int = 0x00000000,
            Float = 76f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Field0 = 0x00000000,
            Attribute = GameAttribute.Attributes[0x0092], // Damage_Min_Subtotal 
            Int = 0x00000000,
            Float = 2f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x024C], // Disabled 
            Int = 0x00000001,
            Float = 0f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x012C], // Hidden 
            Int = 0x00000001,
            Float = 0f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x000C], // Defense 
            Int = 0x00000000,
            Float = 10f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Field0 = 0x000000DE,
            Attribute = GameAttribute.Attributes[0x003E], // Resistance_Total 
            Int = 0x00000000,
            Float = 0.5f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Field0 = 0x00033C40,
            Attribute = GameAttribute.Attributes[0x0230], // Buff_Icon_Count0 
            Int = 0x00000001,
            Float = 0f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Field0 = 0x00000000,
            Attribute = GameAttribute.Attributes[0x0094], // Damage_Weapon_Delta 
            Int = 0x00000000,
            Float = 1f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x000B], // Vitality 
            Int = 0x00000000,
            Float = 9f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x000A], // Precision 
            Int = 0x00000000,
            Float = 11f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Field0 = 0x00007545,
            Attribute = GameAttribute.Attributes[0x0041], // Skill 
            Int = 0x00000001,
            Float = 0f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Field0 = 0x00000000,
            Attribute = GameAttribute.Attributes[0x0095], // Damage_Weapon_Delta_SubTotal 
            Int = 0x00000000,
            Float = 1f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x0009], // Attack 
            Int = 0x00000000,
            Float = 10f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Field0 = 0x00007545,
            Attribute = GameAttribute.Attributes[0x0042], // Skill_Total 
            Int = 0x00000001,
            Float = 0f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x0189], // Attacks_Per_Second_Item_Total_MainHand 
            Int = 0x00000000,
            Float = 1.199219f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Field0 = 0x00000000,
            Attribute = GameAttribute.Attributes[0x0096], // Damage_Weapon_Max 
            Int = 0x00000000,
            Float = 3f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x0089], // Attacks_Per_Second_Total 
            Int = 0x00000000,
            Float = 1.199219f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x0108], // Projectile_Speed 
            Int = 0x00000000,
            Float = 3.051758E-05f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x00C8], // Get_Hit_Recovery 
            Int = 0x00000000,
            Float = 6f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x00A8], // Crit_Percent_Cap 
            Int = 0x3F400000,
            Float = 0f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x0008], // Shared_Stash_Slots 
            Int = 0x0000000E,
            Float = 0f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Field0 = 0x00000000,
            Attribute = GameAttribute.Attributes[0x0097], // Damage_Weapon_Max_Total 
            Int = 0x00000000,
            Float = 3f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x0027], // Level_Cap 
            Int = 0x0000000D,
            Float = 0f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Field0 = 0x00000000,
            Attribute = GameAttribute.Attributes[0x0098], // Damage_Weapon_Delta_Total 
            Int = 0x00000000,
            Float = 1f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x00C7], // Get_Hit_Recovery_Per_Level 
            Int = 0x00000000,
            Float = 1f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x0087], // Attacks_Per_Second 
            Int = 0x00000000,
            Float = 1f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x0187], // Attacks_Per_Second_Item_MainHand 
            Int = 0x00000000,
            Float = 1.199219f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x0007], // Backpack_Slots 
            Int = 0x0000003C,
            Float = 0f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x0046], // Loading 
            Int = 0x00000001,
            Float = 0f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x0086], // Attacks_Per_Second_Item_Total 
            Int = 0x00000000,
            Float = 1.199219f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x00C6], // Get_Hit_Recovery_Base 
            Int = 0x00000000,
            Float = 5f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Field0 = 0x00007780,
            Attribute = GameAttribute.Attributes[0x0041], // Skill 
            Int = 0x00000001,
            Float = 0f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x0026], // Level 
            Int = 0x00000001,
            Float = 0f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Field0 = 0x00033C40,
            Attribute = GameAttribute.Attributes[0x01BE], // Buff_Icon_End_Tick0 
            Int = 0x000003FB,
            Float = 0f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x0045], // Invulnerable 
            Int = 0x00000001,
            Float = 0f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Field0 = 0x0000CE11,
            Attribute = GameAttribute.Attributes[0x027B], // Trait 
            Int = 0x00000001,
            Float = 0f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x00C5], // Get_Hit_Max 
            Int = 0x00000000,
            Float = 60f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Field0 = 0x00007780,
            Attribute = GameAttribute.Attributes[0x0042], // Skill_Total 
            Int = 0x00000001,
            Float = 0f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Field0 = 0x00000000,
            Attribute = GameAttribute.Attributes[0x009B], // Damage_Weapon_Min 
            Int = 0x00000000,
            Float = 2f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x00C4], // Get_Hit_Max_Per_Level 
            Int = 0x00000000,
            Float = 10f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x0103], // General_Cooldown 
            Int = 0x00000000,
            Float = 0f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Field0 = 0x00000226,
            Attribute = GameAttribute.Attributes[0x003E], // Resistance_Total 
            Int = 0x00000000,
            Float = 0.5f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Field0 = 0x00000000,
            Attribute = GameAttribute.Attributes[0x009C], // Damage_Weapon_Min_Total 
            Int = 0x00000000,
            Float = 2f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x00C3], // Get_Hit_Max_Base 
            Int = 0x00000000,
            Float = 50f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x0043], // TeamID 
            Int = 0x00000002,
            Float = 0f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Field0 = 0x000FFFFF,
            Attribute = GameAttribute.Attributes[0x0042], // Skill_Total 
            Int = 0x00000001,
            Float = 0f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x0022], // Experience_Next 
            Int = 0x000004B0,
            Float = 0f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Field0 = 0x00000003,
            Attribute = GameAttribute.Attributes[0x005E], // Resource_Cur 
            Int = 0x43480000,
            Float = 0f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Field0 = 0x000FFFFF,
            Attribute = GameAttribute.Attributes[0x0041], // Skill 
            Int = 0x00000001,
            Float = 0f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Field0 = 0x00033C40,
            Attribute = GameAttribute.Attributes[0x01BA], // Buff_Icon_Start_Tick0 
            Int = 0x00000077,
            Float = 0f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x0081], // Hit_Chance 
            Int = 0x00000000,
            Float = 1f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x0021], // Experience_Granted 
            Int = 0x000003E8,
            Float = 0f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Field0 = 0x00000226,
            Attribute = GameAttribute.Attributes[0x003C], // Resistance 
            Int = 0x00000000,
            Float = 0.5f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Field0 = 0x00000003,
            Attribute = GameAttribute.Attributes[0x005F], // Resource_Max 
            Int = 0x00000000,
            Float = 200f,
        },
    });
    #endregion
    #region VisualInventoryMessage 0x789E00E2
    SendMessage(new VisualInventoryMessage()
    {
        Id = 0x004E,
        Field0 = 0x789E00E2,
        Field1 = new VisualEquipment()
        {
            Field0 = new VisualItem[8]
    {
     new VisualItem()
     {
        Field0 = -1,
        Field1 = 0x00000000,
        Field2 = 0x00000000,
        Field3 = 0x00000000,
     },
     new VisualItem()
     {
        Field0 = -1,
        Field1 = 0x00000000,
        Field2 = 0x00000000,
        Field3 = 0x00000000,
     },
     new VisualItem()
     {
        Field0 = -1,
        Field1 = 0x00000000,
        Field2 = 0x00000000,
        Field3 = 0x00000000,
     },
     new VisualItem()
     {
        Field0 = -1,
        Field1 = 0x00000000,
        Field2 = 0x00000000,
        Field3 = 0x00000000,
     },
     new VisualItem()
     {
        Field0 = 0x49B51827,
        Field1 = 0x00000000,
        Field2 = 0x00000000,
        Field3 = -1,
     },
     new VisualItem()
     {
        Field0 = -1,
        Field1 = 0x00000000,
        Field2 = 0x00000000,
        Field3 = 0x00000000,
     },
     new VisualItem()
     {
        Field0 = -1,
        Field1 = 0x00000000,
        Field2 = 0x00000000,
        Field3 = 0x00000000,
     },
     new VisualItem()
     {
        Field0 = -1,
        Field1 = 0x00000000,
        Field2 = 0x00000000,
        Field3 = 0x00000000,
     },
    },
        },
    });
    #endregion
    #region ACDWorldPosition 0x789E00E2
    SendMessage(new ACDWorldPositionMessage()
    {
        Id = 0x003F,
        Field0 = 0x789E00E2,
        Field1 = new WorldLocationMessageData()
        {
            Field0 = 1.43f,
            Field1 = new PRTransform()
            {
                Field0 = new Quaternion()
                {
                    Field0 = 0.05940768f,
                    Field1 = new Vector3D()
                    {
                        Field0 = 0f,
                        Field1 = 0f,
                        Field2 = 0.9982339f,
                    },
                },
                Field1 = new Vector3D()
                {
                    Field0 = 3143.75f,
                    Field1 = 2828.75f,
                    Field2 = 59.07559f,
                },
            },
            Field2 = 0x772E0000,
        },
    });
    #endregion
    #region ACDCollFlags 0x789E00E2
    SendMessage(new ACDCollFlagsMessage()
    {
        Id = 0x00A6,
        Field0 = 0x789E00E2,
        Field1 = 0x00000000,
    });
    #endregion
    #region Item 0x789F00E3
    #region Attributes 0x789F00E3
    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789F00E3,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x009D], // Damage_Weapon_Min_Total_All 
            Int = 0x00000000,
            Float = 2f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789F00E3,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x0099], // Damage_Weapon_Delta_Total_All 
            Int = 0x00000000,
            Float = 1f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789F00E3,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x0117], // Item_Equipped 
            Int = 0x00000001,
            Float = 0f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789F00E3,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x0115], // Item_Quality_Level 
            Int = 0x00000001,
            Float = 0f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789F00E3,
        Field1 = new NetAttributeKeyValue()
        {
            Field0 = 0x00000000,
            Attribute = GameAttribute.Attributes[0x018B], // Damage_Weapon_Min_Total_MainHand 
            Int = 0x00000000,
            Float = 2f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789F00E3,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x0113], // Durability_Max 
            Int = 0x00000190,
            Float = 0f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789F00E3,
        Field1 = new NetAttributeKeyValue()
        {
            Field0 = 0x00000000,
            Attribute = GameAttribute.Attributes[0x018D], // Damage_Weapon_Delta_Total_MainHand 
            Int = 0x00000000,
            Float = 1f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789F00E3,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x0112], // Durability_Cur 
            Int = 0x00000190,
            Float = 0f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789F00E3,
        Field1 = new NetAttributeKeyValue()
        {
            Field0 = 0x00000000,
            Attribute = GameAttribute.Attributes[0x0094], // Damage_Weapon_Delta 
            Int = 0x00000000,
            Float = 1f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789F00E3,
        Field1 = new NetAttributeKeyValue()
        {
            Field0 = 0x00000000,
            Attribute = GameAttribute.Attributes[0x0095], // Damage_Weapon_Delta_SubTotal 
            Int = 0x00000000,
            Float = 1f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789F00E3,
        Field1 = new NetAttributeKeyValue()
        {
            Field0 = 0x00000000,
            Attribute = GameAttribute.Attributes[0x0096], // Damage_Weapon_Max 
            Int = 0x00000000,
            Float = 3f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789F00E3,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x0189], // Attacks_Per_Second_Item_Total_MainHand 
            Int = 0x00000000,
            Float = 1.199219f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789F00E3,
        Field1 = new NetAttributeKeyValue()
        {
            Field0 = 0x00000000,
            Attribute = GameAttribute.Attributes[0x0097], // Damage_Weapon_Max_Total 
            Int = 0x00000000,
            Float = 3f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789F00E3,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x0187], // Attacks_Per_Second_Item_MainHand 
            Int = 0x00000000,
            Float = 1.199219f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789F00E3,
        Field1 = new NetAttributeKeyValue()
        {
            Field0 = 0x00000000,
            Attribute = GameAttribute.Attributes[0x0098], // Damage_Weapon_Delta_Total 
            Int = 0x00000000,
            Float = 1f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789F00E3,
        Field1 = new NetAttributeKeyValue()
        {
            Field0 = 0x00007780,
            Attribute = GameAttribute.Attributes[0x0041], // Skill 
            Int = 0x00000001,
            Float = 0f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789F00E3,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x0086], // Attacks_Per_Second_Item_Total 
            Int = 0x00000000,
            Float = 1.199219f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789F00E3,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x0125], // Seed 
            Int = unchecked((int)0xED34A51F),
            Float = 0f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789F00E3,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x0124], // IdentifyCost 
            Int = 0x00000001,
            Float = 0f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789F00E3,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x0084], // Attacks_Per_Second_Item_Subtotal 
            Int = 0x00000000,
            Float = 1.199219f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789F00E3,
        Field1 = new NetAttributeKeyValue()
        {
            Field0 = 0x00000000,
            Attribute = GameAttribute.Attributes[0x009B], // Damage_Weapon_Min 
            Int = 0x00000000,
            Float = 2f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789F00E3,
        Field1 = new NetAttributeKeyValue()
        {
            Field0 = 0x00000000,
            Attribute = GameAttribute.Attributes[0x009C], // Damage_Weapon_Min_Total 
            Int = 0x00000000,
            Float = 2f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789F00E3,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x0082], // Attacks_Per_Second_Item 
            Int = 0x00000000,
            Float = 1.199219f,
        },
    });
    #endregion

    SendMessage(new PlayEffectMessage()
    {
        Id = 0x007A,
        Field0 = 0x789F00E3,
        Field1 = 0x00000027,
    });
    SendMessage(new ACDInventoryPositionMessage()
    {
        Id = 0x0040,
        Field0 = 0x789F00E3,
        Field1 = new InventoryLocationMessageData()
        {
            Field0 = 0x789E00E2,
            Field1 = 0x00000004,
            Field2 = new IVector2D()
            {
                Field0 = 0x00000000,
                Field1 = 0x00000000,
            },
        },
        Field2 = 0x00000001,
    });

    SendMessage(new ACDInventoryUpdateActorSNO()
    {
        Id = 0x0041,
        Field0 = 0x789F00E3,
        Field1 = 0x00001025,
    });
    #endregion
    #region Item 0x789F00E4
    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x78A000E4,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x0115], // Item_Quality_Level 
            Int = 0x00000001,
            Float = 0f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x78A000E4,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x0052], // Hitpoints_Granted 
            Int = 0x00000000,
            Float = 100f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x78A000E4,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x0125], // Seed 
            Int = unchecked((int)0x884DCD35),
            Float = 0f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x78A000E4,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x0121], // ItemStackQuantityLo 
            Int = 0x00000001,
            Float = 0f,
        },
    });

    SendMessage(new PlayEffectMessage()
    {
        Id = 0x007A,
        Field0 = 0x78A000E4,
        Field1 = 0x00000027,
    });

    SendMessage(new ACDInventoryPositionMessage()
    {
        Id = 0x0040,
        Field0 = 0x78A000E4,
        Field1 = new InventoryLocationMessageData()
        {
            Field0 = 0x789E00E2,
            Field1 = 0x00000000,
            Field2 = new IVector2D()
            {
                Field0 = 0x00000000,
                Field1 = 0x00000000,
            },
        },
        Field2 = 0x00000001,
    });

    SendMessage(new ACDInventoryUpdateActorSNO()
    {
        Id = 0x0041,
        Field0 = 0x78A000E4,
        Field1 = 0x00001158,
    });
    #endregion
    #region Attributes 0x78DD0118
    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x78DD0118,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x0056], // Hitpoints_Max_Total 
            Int = 0x00000000,
            Float = 1f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x78DD0118,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x0054], // Hitpoints_Max 
            Int = 0x00000000,
            Float = 0.0009994507f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x78DD0118,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x004D], // Hitpoints_Cur 
            Int = 0x00000000,
            Float = 0.0009994507f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x78DD0118,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x0026], // Level 
            Int = 0x00000001,
            Float = 0f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x78DD0118,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x0043], // TeamID 
            Int = 0x00000001,
            Float = 0f,
        },
    });
    #endregion
    #region ACDWorldPosition 0x78DD0118
    SendMessage(new ACDWorldPositionMessage()
    {
        Id = 0x003F,
        Field0 = 0x78DD0118,
        Field1 = new WorldLocationMessageData()
        {
            Field0 = 1f,
            Field1 = new PRTransform()
            {
                Field0 = new Quaternion()
                {
                    Field0 = -0.01089788f,
                    Field1 = new Vector3D()
                    {
                        Field0 = 0f,
                        Field1 = 0f,
                        Field2 = 0.9999406f,
                    },
                },
                Field1 = new Vector3D()
                {
                    Field0 = 3125.888f,
                    Field1 = 2602.642f,
                    Field2 = 1.050535f,
                },
            },
            Field2 = 0x772E0000,
        },
    });
    #endregion
    #region Attributes 0x78DE0119
    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x78DE0119,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x0056], // Hitpoints_Max_Total 
            Int = 0x00000000,
            Float = 1f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x78DE0119,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x0054], // Hitpoints_Max 
            Int = 0x00000000,
            Float = 0.0009994507f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x78DE0119,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x004D], // Hitpoints_Cur 
            Int = 0x00000000,
            Float = 0.0009994507f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x78DE0119,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x0026], // Level 
            Int = 0x00000001,
            Float = 0f,
        },
    });

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x78DE0119,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x0043], // TeamID 
            Int = 0x00000001,
            Float = 0f,
        },
    });
    #endregion
    #region ACDWorldPosition 0x78DE0119
    SendMessage(new ACDWorldPositionMessage()
    {
        Id = 0x003F,
        Field0 = 0x78DE0119,
        Field1 = new WorldLocationMessageData()
        {
            Field0 = 1f,
            Field1 = new PRTransform()
            {
                Field0 = new Quaternion()
                {
                    Field0 = 1f,
                    Field1 = new Vector3D()
                    {
                        Field0 = 0f,
                        Field1 = 0f,
                        Field2 = 0f,
                    },
                },
                Field1 = new Vector3D()
                {
                    Field0 = 3083.982f,
                    Field1 = 2603.142f,
                    Field2 = 0.4151611f,
                },
            },
            Field2 = 0x772E0000,
        },
    });
    #endregion
    #region TrickleMessage 0x789E00E2
    SendMessage(new TrickleMessage()
    {
        Id = 0x0042,
        Field0 = 0x789E00E2,
        Field1 = 0x00001271,
        Field2 = new WorldPlace()
        {
            Field0 = new Vector3D()
            {
                Field0 = 3143.75f,
                Field1 = 2828.75f,
                Field2 = 59.07559f,
            },
            Field1 = 0x772E0000,
        },
        Field3 = 0x00000000,
        Field4 = 0x00026186,
        Field5 = 1f,
        Field6 = 0x00000001,
        Field7 = 0x0000002C,
        Field10 = unchecked((int)0x8DFA5D13),
        Field12 = 0x0000F063,
    });
    #endregion
    #region TrickleMessage 0x0042

    SendMessage(new TrickleMessage()
    {
        Id = 0x0042,
        Field0 = 0x77BF0003,
        Field1 = 0x0000176E,
        Field2 = new WorldPlace()
        {
            Field0 = new Vector3D()
            {
                Field0 = 2820.838f,
                Field1 = 2912.305f,
                Field2 = 33.09947f,
            },
            Field1 = 0x772E0000,
        },
        Field4 = 0x00004DEB,
        Field5 = 1f,
        Field6 = 0x00000008,
        Field7 = 0x00000024,
        Field10 = unchecked((int)0xC206AF3C),
        Field12 = 0x0000F063,
        Field13 = 225f,
    });

    SendMessage(new TrickleMessage()
    {
        Id = 0x0042,
        Field0 = 0x77C50009,
        Field1 = 0x0001FD60,
        Field2 = new WorldPlace()
        {
            Field0 = new Vector3D()
            {
                Field0 = 2970.619f,
                Field1 = 2789.915f,
                Field2 = 23.94531f,
            },
            Field1 = 0x772E0000,
        },
        Field4 = 0x00004DEB,
        Field5 = 0.001f,
        Field6 = 0x0000000B,
        Field7 = 0x00000020,
        Field9 = 0x000315F2,
        Field10 = 0x00BEA005,
        Field12 = 0x0000F063,
        Field13 = 225f,
    });

    SendMessage(new TrickleMessage()
    {
        Id = 0x0042,
        Field0 = 0x77C6000A,
        Field1 = 0x00032361,
        Field2 = new WorldPlace()
        {
            Field0 = new Vector3D()
            {
                Field0 = 2847.292f,
                Field1 = 2845.497f,
                Field2 = 23.90378f,
            },
            Field1 = 0x772E0000,
        },
        Field4 = 0x00004DEB,
        Field5 = 1f,
        Field6 = 0x00000008,
        Field7 = 0x00000024,
        Field10 = 0x4167BF66,
        Field12 = 0x0000F063,
        Field13 = 225f,
    });

    SendMessage(new TrickleMessage()
    {
        Id = 0x0042,
        Field0 = 0x77C8000C,
        Field1 = 0x0000176E,
        Field2 = new WorldPlace()
        {
            Field0 = new Vector3D()
            {
                Field0 = 2857.057f,
                Field1 = 2913.271f,
                Field2 = 33.15442f,
            },
            Field1 = 0x772E0000,
        },
        Field4 = 0x00004DEB,
        Field5 = 1f,
        Field6 = 0x00000008,
        Field7 = 0x00000024,
        Field10 = unchecked((int)0xC206AF3C),
        Field12 = 0x0000F063,
        Field13 = 225f,
    });

    SendMessage(new TrickleMessage()
    {
        Id = 0x0042,
        Field0 = 0x77D00014,
        Field1 = 0x00001224,
        Field2 = new WorldPlace()
        {
            Field0 = new Vector3D()
            {
                Field0 = 3009.983f,
                Field1 = 2855.779f,
                Field2 = 23.85921f,
            },
            Field1 = 0x772E0000,
        },
        Field4 = 0x00004DEB,
        Field5 = 1f,
        Field6 = 0x00000008,
        Field7 = 0x00000024,
        Field10 = unchecked((int)0x9B392F4F),
        Field12 = 0x0000F063,
        Field13 = 225f,
    });

    SendMessage(new TrickleMessage()
    {
        Id = 0x0042,
        Field0 = 0x77E00024,
        Field1 = 0x000227BE,
        Field2 = new WorldPlace()
        {
            Field0 = new Vector3D()
            {
                Field0 = 2884.539f,
                Field1 = 2799.291f,
                Field2 = 23.94533f,
            },
            Field1 = 0x772E0000,
        },
        Field4 = 0x00004DEB,
        Field5 = 1f,
        Field6 = 0x00000008,
        Field7 = 0x00000024,
        Field10 = 0x52AB3C27,
        Field12 = 0x0000F063,
        Field13 = 225f,
    });

    SendMessage(new TrickleMessage()
    {
        Id = 0x0042,
        Field0 = 0x77E30027,
        Field1 = 0x00001772,
        Field2 = new WorldPlace()
        {
            Field0 = new Vector3D()
            {
                Field0 = 2847.094f,
                Field1 = 2925.771f,
                Field2 = 24.30022f,
            },
            Field1 = 0x772E0000,
        },
        Field4 = 0x000316CE,
        Field5 = 1f,
        Field6 = 0x00000008,
        Field7 = 0x00000024,
        Field10 = unchecked((int)0xE8AAA9EB),
        Field12 = 0x0000F063,
        Field13 = 225f,
    });

    SendMessage(new TrickleMessage()
    {
        Id = 0x0042,
        Field0 = 0x77E40028,
        Field1 = 0x00001772,
        Field2 = new WorldPlace()
        {
            Field0 = new Vector3D()
            {
                Field0 = 2861.172f,
                Field1 = 2935.073f,
                Field2 = 24.02267f,
            },
            Field1 = 0x772E0000,
        },
        Field4 = 0x000316CE,
        Field5 = 1f,
        Field6 = 0x00000008,
        Field7 = 0x00000024,
        Field10 = unchecked((int)0xE8AAA9EB),
        Field12 = 0x0000F063,
        Field13 = 225f,
    });

    SendMessage(new TrickleMessage()
    {
        Id = 0x0042,
        Field0 = 0x77F40038,
        Field1 = 0x000250CC,
        Field2 = new WorldPlace()
        {
            Field0 = new Vector3D()
            {
                Field0 = 2972.107f,
                Field1 = 2869.526f,
                Field2 = 23.93186f,
            },
            Field1 = 0x772E0000,
        },
        Field4 = 0x00004DEB,
        Field5 = 1f,
        Field6 = 0x00000008,
        Field7 = 0x00000024,
        Field10 = 0x5FD00378,
        Field12 = 0x0000F063,
        Field13 = 225f,
    });

    SendMessage(new TrickleMessage()
    {
        Id = 0x0042,
        Field0 = 0x77F50039,
        Field1 = 0x0001B186,
        Field2 = new WorldPlace()
        {
            Field0 = new Vector3D()
            {
                Field0 = 2979.583f,
                Field1 = 2867.992f,
                Field2 = 24.00022f,
            },
            Field1 = 0x772E0000,
        },
        Field4 = 0x00004DEB,
        Field5 = 1f,
        Field6 = 0x00000008,
        Field7 = 0x00000024,
        Field10 = 0x4C11D859,
        Field12 = 0x0000F063,
        Field13 = 225f,
    });

    SendMessage(new TrickleMessage()
    {
        Id = 0x0042,
        Field0 = 0x78000044,
        Field1 = 0x000228C4,
        Field2 = new WorldPlace()
        {
            Field0 = new Vector3D()
            {
                Field0 = 3022.005f,
                Field1 = 2778.869f,
                Field2 = 23.94533f,
            },
            Field1 = 0x772E0000,
        },
        Field4 = 0x00004DEB,
        Field5 = 1f,
        Field6 = 0x00000008,
        Field7 = 0x00000024,
        Field10 = unchecked((int)0xA8D02EDD),
        Field12 = 0x0000F063,
        Field13 = 225f,
    });

    SendMessage(new TrickleMessage()
    {
        Id = 0x0042,
        Field0 = 0x78B000F4,
        Field1 = 0x00032A7B,
        Field2 = new WorldPlace()
        {
            Field0 = new Vector3D()
            {
                Field0 = 2856.255f,
                Field1 = 2547.737f,
                Field2 = 0.4998169f,
            },
            Field1 = 0x772E0000,
        },
        Field4 = 0x00004DEB,
        Field5 = 1f,
        Field6 = 0x00000008,
        Field7 = 0x00000024,
        Field10 = unchecked((int)0xAB714BBD),
        Field12 = 0x0000F063,
        Field13 = 225f,
    });

    SendMessage(new TrickleMessage()
    {
        Id = 0x0042,
        Field0 = 0x78B200F6,
        Field1 = 0x00032A7B,
        Field2 = new WorldPlace()
        {
            Field0 = new Vector3D()
            {
                Field0 = 2873.099f,
                Field1 = 2551.124f,
                Field2 = 0.4997787f,
            },
            Field1 = 0x772E0000,
        },
        Field4 = 0x00004DEB,
        Field5 = 1f,
        Field6 = 0x00000008,
        Field7 = 0x00000024,
        Field10 = unchecked((int)0xAB714BBD),
        Field12 = 0x0000F063,
        Field13 = 225f,
    });

    SendMessage(new TrickleMessage()
    {
        Id = 0x0042,
        Field0 = 0x78B500F9,
        Field1 = 0x00014A3E,
        Field2 = new WorldPlace()
        {
            Field0 = new Vector3D()
            {
                Field0 = 2993.154f,
                Field1 = 2593.221f,
                Field2 = 0.5345764f,
            },
            Field1 = 0x772E0000,
        },
        Field4 = 0x00004DEB,
        Field5 = 1f,
        Field6 = 0x00000008,
        Field7 = 0x00000024,
        Field10 = 0x2E91310B,
        Field12 = 0x0000F063,
        Field13 = 225f,
    });

    SendMessage(new TrickleMessage()
    {
        Id = 0x0042,
        Field0 = 0x78B600FA,
        Field1 = 0x00014A40,
        Field2 = new WorldPlace()
        {
            Field0 = new Vector3D()
            {
                Field0 = 2900.319f,
                Field1 = 2574.399f,
                Field2 = 0.4997864f,
            },
            Field1 = 0x772E0000,
        },
        Field4 = 0x00004DEB,
        Field5 = 1f,
        Field6 = 0x00000008,
        Field7 = 0x00000024,
        Field10 = 0x2E91310C,
        Field12 = 0x0000F063,
        Field13 = 225f,
    });

    SendMessage(new TrickleMessage()
    {
        Id = 0x0042,
        Field0 = 0x78BE0102,
        Field1 = 0x0001B186,
        Field2 = new WorldPlace()
        {
            Field0 = new Vector3D()
            {
                Field0 = 3007.198f,
                Field1 = 2712.854f,
                Field2 = 23.76516f,
            },
            Field1 = 0x772E0000,
        },
        Field4 = 0x00004DEB,
        Field5 = 1f,
        Field6 = 0x00000008,
        Field7 = 0x00000024,
        Field10 = 0x4C11D859,
        Field12 = 0x0000F063,
        Field13 = 225f,
    });

    SendMessage(new TrickleMessage()
    {
        Id = 0x0042,
        Field0 = 0x78C40108,
        Field1 = 0x0002AEA4,
        Field2 = new WorldPlace()
        {
            Field0 = new Vector3D()
            {
                Field0 = 2982.698f,
                Field1 = 2600.461f,
                Field2 = 0.4997864f,
            },
            Field1 = 0x772E0000,
        },
        Field4 = 0x00004DEB,
        Field5 = 1f,
        Field6 = 0x00000008,
        Field7 = 0x0000002C,
        Field10 = unchecked((int)0xDCEFA44D),
        Field12 = 0x0000F063,
        Field13 = 225f,
    });

    SendMessage(new TrickleMessage()
    {
        Id = 0x0042,
        Field0 = 0x78DF011A,
        Field1 = 0x000255BB,
        Field2 = new WorldPlace()
        {
            Field0 = new Vector3D()
            {
                Field0 = 3131.338f,
                Field1 = 2597.316f,
                Field2 = 0.9298096f,
            },
            Field1 = 0x772E0000,
        },
        Field4 = 0x00004DEB,
        Field5 = 1f,
        Field6 = 0x00000008,
        Field7 = 0x00000024,
        Field10 = 0x0AF96544,
        Field12 = 0x0000F063,
    });
    #endregion
    #region ANNDataMessage 0x0043
    SendMessage(new ANNDataMessage()
    {
        Id = 0x0043,
        Field0 = 0x788500C9,
    });

    SendMessage(new ANNDataMessage()
    {
        Id = 0x0043,
        Field0 = 0x788700CB,
    });

    SendMessage(new ANNDataMessage()
    {
        Id = 0x0043,
        Field0 = 0x77C40008,
    });

    SendMessage(new ANNDataMessage()
    {
        Id = 0x0043,
        Field0 = 0x77C7000B,
    });

    SendMessage(new ANNDataMessage()
    {
        Id = 0x0043,
        Field0 = 0x77CD0011,
    });

    SendMessage(new ANNDataMessage()
    {
        Id = 0x0043,
        Field0 = 0x77E50029,
    });

    SendMessage(new ANNDataMessage()
    {
        Id = 0x0043,
        Field0 = 0x77EA002E,
    });

    SendMessage(new ANNDataMessage()
    {
        Id = 0x0043,
        Field0 = 0x77EE0032,
    });

    SendMessage(new ANNDataMessage()
    {
        Id = 0x0043,
        Field0 = 0x78120056,
    });

    SendMessage(new ANNDataMessage()
    {
        Id = 0x0043,
        Field0 = 0x78140058,
    });

    SendMessage(new ANNDataMessage()
    {
        Id = 0x0043,
        Field0 = 0x78B800FC,
    });

    SendMessage(new ANNDataMessage()
    {
        Id = 0x0043,
        Field0 = 0x78BF0103,
    });

    SendMessage(new ANNDataMessage()
    {
        Id = 0x0043,
        Field0 = 0x78C00104,
    });

    SendMessage(new ANNDataMessage()
    {
        Id = 0x0043,
        Field0 = 0x78C10105,
    });

    SendMessage(new ANNDataMessage()
    {
        Id = 0x0043,
        Field0 = 0x78C30107,
    });

    SendMessage(new ANNDataMessage()
    {
        Id = 0x0043,
        Field0 = 0x78D80113,
    });

    SendMessage(new ANNDataMessage()
    {
        Id = 0x0043,
        Field0 = 0x78D90114,
    });

    SendMessage(new ANNDataMessage()
    {
        Id = 0x0043,
        Field0 = 0x78DA0115,
    });

    SendMessage(new ANNDataMessage()
    {
        Id = 0x0043,
        Field0 = 0x78DB0116,
    });

    SendMessage(new ANNDataMessage()
    {
        Id = 0x0043,
        Field0 = 0x78DC0117,
    });
    #endregion
    SendMessage(new DWordDataMessage() // TICK
    {
        Id = 0x0089,
        Field0 = 0x00000077,
    });

    FlushOutgoingBuffer();

    SendMessage(new AttributeSetValueMessage()
    {
        Id = 0x004C,
        Field0 = 0x789E00E2,
        Field1 = new NetAttributeKeyValue()
        {
            Attribute = GameAttribute.Attributes[0x005B], // Hitpoints_Healed_Target
            Int = 0x00000000,
            Float = 76f,
        },
    });

    SendMessage(new DWordDataMessage() // TICK
    {
        Id = 0x0089,
        Field0 = 0x0000007D,
    });
}

public void OnMessage(SimpleMessage msg)
{
    switch (msg.Id)
    {
        case 0x0030: // Sent with DwordDataMessage(0x0125, Value:0) and SimpleMessage(0x0125)
            {
                #region hardcoded1
                #region HirelingInfo
                SendMessage(new HirelingInfoUpdateMessage()
                {
                    Id = 0x009D,
                    Field0 = 0x00000001,
                    Field1 = false,
                    Field2 = -1,
                    Field3 = 0x00000000,
                });

                SendMessage(new HirelingInfoUpdateMessage()
                {
                    Id = 0x009D,
                    Field0 = 0x00000002,
                    Field1 = false,
                    Field2 = -1,
                    Field3 = 0x00000000,
                });

                SendMessage(new HirelingInfoUpdateMessage()
                {
                    Id = 0x009D,
                    Field0 = 0x00000003,
                    Field1 = false,
                    Field2 = -1,
                    Field3 = 0x00000000,
                });
                #endregion
                #region Attribute Values 0x789E00E2
                SendMessage(new AttributeSetValueMessage()
                {
                    Id = 0x004C,
                    Field0 = 0x789E00E2,
                    Field1 = new NetAttributeKeyValue()
                    {
                        Field0 = 0x000FFFFF,
                        Attribute = GameAttribute.Attributes[0x015B], // Banter_Cooldown
                        Int = 0x000007C9,
                        Float = 0f,
                    },
                });

                SendMessage(new AttributeSetValueMessage()
                {
                    Id = 0x004C,
                    Field0 = 0x789E00E2,
                    Field1 = new NetAttributeKeyValue()
                    {
                        Field0 = 0x00020CBE,
                        Attribute = GameAttribute.Attributes[0x01CC], // Buff_Active
                        Int = 0x00000001,
                        Float = 0f,
                    },
                });

                SendMessage(new AttributeSetValueMessage()
                {
                    Id = 0x004C,
                    Field0 = 0x789E00E2,
                    Field1 = new NetAttributeKeyValue()
                    {
                        Field0 = 0x00033C40,
                        Attribute = GameAttribute.Attributes[0x01CC], // Buff_Active
                        Int = 0x00000000,
                        Float = 0f,
                    },
                });

                SendMessage(new AttributeSetValueMessage()
                {
                    Id = 0x004C,
                    Field0 = 0x789E00E2,
                    Field1 = new NetAttributeKeyValue()
                    {
                        Attribute = GameAttribute.Attributes[0x00D7], // Immobolize
                        Int = 0x00000000,
                        Float = 0f,
                    },
                });

                SendMessage(new AttributeSetValueMessage()
                {
                    Id = 0x004C,
                    Field0 = 0x789E00E2,
                    Field1 = new NetAttributeKeyValue()
                    {
                        Attribute = GameAttribute.Attributes[0x00D6], // Untargetable
                        Int = 0x00000000,
                        Float = 0f,
                    },
                });

                SendMessage(new AttributeSetValueMessage()
                {
                    Id = 0x004C,
                    Field0 = 0x789E00E2,
                    Field1 = new NetAttributeKeyValue()
                    {
                        Attribute = GameAttribute.Attributes[0x01D2], // CantStartDisplayedPowers
                        Int = 0x00000000,
                        Float = 0f,
                    },
                });

                SendMessage(new AttributeSetValueMessage()
                {
                    Id = 0x004C,
                    Field0 = 0x789E00E2,
                    Field1 = new NetAttributeKeyValue()
                    {
                        Field0 = 0x00020CBE,
                        Attribute = GameAttribute.Attributes[0x01BA], // Buff_Icon_Start_Tick0
                        Int = 0x000000C1,
                        Float = 0f,
                    },
                });

                SendMessage(new AttributeSetValueMessage()
                {
                    Id = 0x004C,
                    Field0 = 0x789E00E2,
                    Field1 = new NetAttributeKeyValue()
                    {
                        Attribute = GameAttribute.Attributes[0x024C], // Disabled
                        Int = 0x00000000,
                        Float = 0f,
                    },
                });

                SendMessage(new AttributeSetValueMessage()
                {
                    Id = 0x004C,
                    Field0 = 0x789E00E2,
                    Field1 = new NetAttributeKeyValue()
                    {
                        Attribute = GameAttribute.Attributes[0x012C], // Hidden
                        Int = 0x00000000,
                        Float = 0f,
                    },
                });

                SendMessage(new AttributeSetValueMessage()
                {
                    Id = 0x004C,
                    Field0 = 0x789E00E2,
                    Field1 = new NetAttributeKeyValue()
                    {
                        Field0 = 0x00033C40,
                        Attribute = GameAttribute.Attributes[0x0230], // Buff_Icon_Count0
                        Int = 0x00000000,
                        Float = 0f,
                    },
                });

                SendMessage(new AttributeSetValueMessage()
                {
                    Id = 0x004C,
                    Field0 = 0x789E00E2,
                    Field1 = new NetAttributeKeyValue()
                    {
                        Field0 = 0x00020CBE,
                        Attribute = GameAttribute.Attributes[0x01BE], // Buff_Icon_End_Tick0
                        Int = 0x000007C9,
                        Float = 0f,
                    },
                });

                SendMessage(new AttributeSetValueMessage()
                {
                    Id = 0x004C,
                    Field0 = 0x789E00E2,
                    Field1 = new NetAttributeKeyValue()
                    {
                        Attribute = GameAttribute.Attributes[0x0046], // Loading
                        Int = 0x00000000,
                        Float = 0f,
                    },
                });

                SendMessage(new AttributeSetValueMessage()
                {
                    Id = 0x004C,
                    Field0 = 0x789E00E2,
                    Field1 = new NetAttributeKeyValue()
                    {
                        Field0 = 0x00033C40,
                        Attribute = GameAttribute.Attributes[0x01BE], // Buff_Icon_End_Tick0
                        Int = 0x00000000,
                        Float = 0f,
                    },
                });

                SendMessage(new AttributeSetValueMessage()
                {
                    Id = 0x004C,
                    Field0 = 0x789E00E2,
                    Field1 = new NetAttributeKeyValue()
                    {
                        Attribute = GameAttribute.Attributes[0x0045], // Invulnerable
                        Int = 0x00000000,
                        Float = 0f,
                    },
                });

                SendMessage(new AttributeSetValueMessage()
                {
                    Id = 0x004C,
                    Field0 = 0x789E00E2,
                    Field1 = new NetAttributeKeyValue()
                    {
                        Field0 = 0x00020CBE,
                        Attribute = GameAttribute.Attributes[0x0230], // Buff_Icon_Count0
                        Int = 0x00000001,
                        Float = 0f,
                    },
                });

                SendMessage(new AttributeSetValueMessage()
                {
                    Id = 0x004C,
                    Field0 = 0x789E00E2,
                    Field1 = new NetAttributeKeyValue()
                    {
                        Field0 = 0x00033C40,
                        Attribute = GameAttribute.Attributes[0x01BA], // Buff_Icon_Start_Tick0
                        Int = 0x00000000,
                        Float = 0f,
                    },
                });
                #endregion

                SendMessage(new ACDCollFlagsMessage()
                {
                    Id = 0x00A6,
                    Field0 = 0x789E00E2,
                    Field1 = 0x00000008,
                });

                SendMessage(new DWordDataMessage()
                {
                    Id = 0x0089,
                    Field0 = 0x000000C1,
                });
                #endregion
                FlushOutgoingBuffer();
                #region hardcoded2
                SendMessage(new TrickleMessage()
                {
                    Id = 0x0042,
                    Field0 = 0x789E00E2,
                    Field1 = 0x00001271,
                    Field2 = new WorldPlace()
                    {
                        Field0 = new Vector3D()
                        {
                            Field0 = 3143.75f,
                            Field1 = 2828.75f,
                            Field2 = 59.07559f,
                        },
                        Field1 = 0x772E0000,
                    },
                    Field3 = 0x00000000,
                    Field4 = 0x00026186,
                    Field5 = 1f,
                    Field6 = 0x00000001,
                    Field7 = 0x00000024,
                    Field10 = unchecked((int)0x8DFA5D13),
                    Field12 = 0x0000F063,
                });

                SendMessage(new DWordDataMessage()
                {
                    Id = 0x0089,
                    Field0 = 0x000000D1,
                });
                #endregion
                FlushOutgoingBuffer();
            }
            break;
        default:
            throw new NotImplementedException();
    }
    
}
