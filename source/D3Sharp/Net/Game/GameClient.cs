/*
 * Copyright (C) 2011 D3Sharp Project
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
using System.IO;
using System.Linq;
using D3Sharp.Net.BNet;
using Gibbed.Helpers;
using System.Text;
using D3Sharp.Utils;
using System.Collections.Generic;
using D3Sharp.Net.Game;
using D3Sharp.Core.Map;

//using Gibbed.Helpers;

namespace D3Sharp.Net.Game
{
    public sealed class GameClient : IGameClient, IGameMessageHandler
    {
        static readonly Logger Logger = LogManager.CreateLogger();

        public IConnection Connection { get; set; }
        public BNetClient BnetClient { get; private set; }

        GameBitBuffer _incomingBuffer = new GameBitBuffer(512);
        GameBitBuffer _outgoingBuffer = new GameBitBuffer(ushort.MaxValue);

        public int packetId = 0x227 + 20;
        public int tick = 0;

        public World GameWorld;

        public GameClient(IConnection connection)
        {
            this.Connection = connection;
            _outgoingBuffer.WriteInt(32, 0);
            GameWorld = new World(this);
        }

        public void Parse(ConnectionDataEventArgs e)
        {
            //Console.WriteLine(e.Data.Dump());

            _incomingBuffer.AppendData(e.Data.ToArray());

            while (_incomingBuffer.IsPacketAvailable())
            {
                int end = _incomingBuffer.Position;
                end += _incomingBuffer.ReadInt(32) * 8;

                while ((end - _incomingBuffer.Position) >= 9)
                {
                    GameMessage msg = _incomingBuffer.ParseMessage();

                    //Logger.LogIncoming(msg);

                    try
                    {
                        msg.VisitHandler(this);
                    }
                    catch (NotImplementedException)
                    {
                        //Logger.Debug("Unhandled game message: 0x{0:X4} {1}", msg.Id, msg.GetType().Name);
                    }
                }

                _incomingBuffer.Position = end;
            }
            _incomingBuffer.ConsumeData();
            FlushOutgoingBuffer();
        }

        public void SendMessage(GameMessage msg)
        {
            //Logger.LogOutgoing(msg);
            _outgoingBuffer.EncodeMessage(msg);
        }

        public void FlushOutgoingBuffer()
        {
            if (_outgoingBuffer.Length > 32)
            {
                var data = _outgoingBuffer.GetPacketAndReset();
                Connection.Send(data);
            }
        }

        public void OnMessage(JoinBNetGameMessage msg)
        {
            if (msg.Id != 0x000A)
                throw new NotImplementedException();

            // a hackish way to get bnetclient in context -- pretends games has only one client in. when we're done with implementing bnet completely, will get this sorted out. /raist
            this.BnetClient = Core.Games.GameManager.AvailableGames[(ulong)msg.Field2].Clients.FirstOrDefault();
            if (this.BnetClient != null) this.BnetClient.InGameClient = this;

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

            #region NewPlayer
            SendMessage(new NewPlayerMessage()
            {
                Id = 0x0031,
                Field0 = 0x00000000, //Party frame (0x00000000 hide, 0x00000001 show)
                Field1 = "", //Owner name?
                ToonName = this.BnetClient.CurrentToon.Name,
                Field3 = 0x00000002, //party frame class 
                Field4 = 0x00000004, //party frame level
                snoActorPortrait = BnetClient.CurrentToon.ClassSNO, //party frame portrait
                Field6 = 0x00000001,
                #region HeroStateData
                Field7 = new HeroStateData()
                {
                    Field0 = 0x00000000,
                    Field1 = 0x00000000,
                    Field2 = 0x00000000,
                    Field3 = BnetClient.CurrentToon.Gender,
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
                Field8 = false, //announce party join
                Field9 = 0x00000001,
                Field10 = 0x789E00E2,
            });
            #endregion

            GameWorld.ReadAndSendMap();

            Console.WriteLine("Positioning character at " + GameWorld.posx + " " + GameWorld.posy + " " + GameWorld.posz);

            #region ACDEnterKnown 0x789E00E2 PlayerId??
            SendMessage(new ACDEnterKnownMessage()
            {
                Id = 0x003B,
                Field0 = 0x789E00E2,
                Field1 = BnetClient.CurrentToon.ClassSNO, //Player model?
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
                            Field0 = GameWorld.posx,
                            Field1 = GameWorld.posy,
                            Field2 = GameWorld.posz,
                        },
                    },
                    Field2 = GameWorld.WorldID,
                },
                Field5 = null,
                Field6 = new GBHandle()
                {
                    Field0 = 0x00000007,
                    Field1 = BnetClient.CurrentToon.ClassID,
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
            Int = BnetClient.CurrentToon.SkillKit,
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
            Field0 = BnetClient.CurrentToon.ResourceID,
            Attribute = GameAttribute.Attributes[0x005E], // Resource_Cur 
            Int = 0x43480000,
            Float = 0f,
         },
         new NetAttributeKeyValue()
         {
            Field0 = BnetClient.CurrentToon.ResourceID,
            Attribute = GameAttribute.Attributes[0x005F], // Resource_Max 
            Int = 0x00000000,
            Float = 200f,
         },
         new NetAttributeKeyValue()
         {
            Field0 = BnetClient.CurrentToon.ResourceID,
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
            Field0 = BnetClient.CurrentToon.ResourceID,
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
            Field0 = BnetClient.CurrentToon.ResourceID,
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
            Int = BnetClient.CurrentToon.ResourceID,
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
            Int = BnetClient.CurrentToon.Level,
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
             new VisualItem() //Head
             {
                Field0 = BnetClient.CurrentToon.Equipment.VisualItemList[0].Gbid,
                Field1 = 0x00000000,
                Field2 = 0x00000000,
                Field3 = -1,
             },
             new VisualItem() //Chest
             {
                Field0 = BnetClient.CurrentToon.Equipment.VisualItemList[1].Gbid,
                Field1 = 0x00000000,
                Field2 = 0x00000000,
                Field3 = -1,
             },
             new VisualItem() //Feet
             {
                Field0 = BnetClient.CurrentToon.Equipment.VisualItemList[2].Gbid,
                Field1 = 0x00000000,
                Field2 = 0x00000000,
                Field3 = -1,
             },
             new VisualItem() //Hands
             {
                Field0 = BnetClient.CurrentToon.Equipment.VisualItemList[3].Gbid,
                Field1 = 0x00000000,
                Field2 = 0x00000000,
                Field3 = -1,
             },
             new VisualItem() //Main hand
             {
                Field0 = BnetClient.CurrentToon.Equipment.VisualItemList[4].Gbid,
                Field1 = 0x00000000,
                Field2 = 0x00000000,
                Field3 = -1,
             },
             new VisualItem() //Offhand
             {
                Field0 = BnetClient.CurrentToon.Equipment.VisualItemList[5].Gbid,
                Field1 = 0x00000000,
                Field2 = 0x00000000,
                Field3 = -1,
             },
             new VisualItem() //Shoulders
             {
                Field0 = BnetClient.CurrentToon.Equipment.VisualItemList[6].Gbid,
                Field1 = 0x00000000,
                Field2 = 0x00000000,
                Field3 = -1,
             },
             new VisualItem() //Legs
             {
                Field0 = BnetClient.CurrentToon.Equipment.VisualItemList[7].Gbid,
                Field1 = 0x00000000,
                Field2 = 0x00000000,
                Field3 = -1,
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
                    Field1 = BnetClient.CurrentToon.ClassSNO,
                },
            });
            #endregion
            FlushOutgoingBuffer();

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

            FlushOutgoingBuffer();

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
                            Field1 = BnetClient.CurrentToon.ClassSNO,
                            Field2 = new WorldPlace()
                            {
                                Field0 = new Vector3D()
                                {
                                    Field0 = 3143.75f,
                                    Field1 = 2828.75f,
                                    Field2 = 59.07559f,
                                },
                                Field1 = GameWorld.WorldID,
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



        public void OnMessage(GameSetupMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(ConnectionEstablishedMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(QuitGameMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(DWordDataMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(BroadcastTextMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(GenericBlobMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(UInt64DataMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(VersionsMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(PlayerIndexMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(NewPlayerMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(EnterWorldMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(RevealWorldMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(RevealSceneMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(DestroySceneMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(SwapSceneMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(RevealTeamMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(HeroStateMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(ACDEnterKnownMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(ANNDataMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(PlayerEnterKnownMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(ACDWorldPositionMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(ACDInventoryPositionMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(ACDInventoryUpdateActorSNO msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(PlayerActorSetInitialMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(VisualInventoryMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(ACDChangeGBHandleMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(AffixMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(LearnedSkillMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(PortalSpecifierMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(RareMonsterNamesMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(RareItemNameMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(AttributeSetValueMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(ProjectileStickMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(AttributesSetValuesMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(ChatMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(VictimMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(KillCountMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(PlayAnimationMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(ACDTranslateNormalMessage msg)
        {
            //Logger.Info("player position: " + msg.Field1.Field0 + " " + msg.Field1.Field1 + " " + msg.Field1.Field2);
            throw new NotImplementedException();
        }

        public void OnMessage(ACDTranslateSnappedMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(ACDTranslateFacingMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(ACDTranslateFixedMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(ACDTranslateArcMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(ACDTranslateDetPathMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(ACDTranslateDetPathSinMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(ACDTranslateDetPathSpiralMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(ACDTranslateSyncMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(ACDTranslateFixedUpdateMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(ACDCollFlagsMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(GoldModifiedMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(PlayEffectMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(PlayHitEffectMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(PlayHitEffectOverrideMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(PlayNonPositionalSoundMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(PlayErrorSoundMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(PlayMusicMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(PlayCutsceneMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(FlippyMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(NPCInteractOptionsMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(PetMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(HirelingInfoUpdateMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(QuestUpdateMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(QuestMeterMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(QuestCounterMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(PlayerLevel msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(WaypointActivatedMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(AimTargetMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(SetIdleAnimationMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(ACDPickupFailedMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(TrickleMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(MapRevealSceneMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(SavePointInfoMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(HearthPortalInfoMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(ReturnPointInfoMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(DataIDDataMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(PlayerInteractMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(TradeMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(ActTransitionMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(InterstitialMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(RopeEffectMessageACDToACD msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(RopeEffectMessageACDToPlace msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(UIElementMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(ACDChangeActorMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(PlayerWarpedMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(GameSyncedDataMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(EndOfTickMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(CreateBNetGameMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(CreateBNetGameResultMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(RequestJoinBNetGameMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(BNetJoinGameRequestResultMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(JoinLANGameMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(NetworkAddressMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(GameIdMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(IntDataMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(EntityIdMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(CreateHeroMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(CreateHeroResultMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(BlizzconCVarsMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(LogoutContextMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(LogoutTickTimeMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(TargetMessage msg)
        {
            GameWorld.HandleTarget(msg.Field1);
            throw new NotImplementedException();
        }

        public void OnMessage(SecondaryAnimationPowerMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(SNODataMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(TryConsoleCommand msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(TryChatMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(TryWaypointMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(InventoryRequestMoveMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(InventorySplitStackMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(InventoryStackTransferMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(InventoryRequestSocketMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(InventoryRequestUseMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(SocketSpellMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(HelperDetachMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(AssignSkillMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(HirelingRequestLearnSkillMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(PlayerChangeHotbarButtonMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(WorldStatusMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(WeatherOverrideMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(ComplexEffectAddMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(EffectGroupACDToACDMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(ACDShearMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(ACDGroupMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(PlayConvLineMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(StopConvLineMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(EndConversationMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(HirelingSwapMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(DeathFadeTimeMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(DisplayGameTextMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(GBIDDataMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(ACDLookAtMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(KillCounterUpdateMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(LowHealthCombatMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(SaviorMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(FloatingNumberMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(FloatingAmountMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(RemoveRagdollMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(SNONameDataMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(LoreMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(WorldDeletedMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(TimedEventStartedMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(ActTransitionStartedMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(PlayerQuestMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(PlayerDeSyncSnapMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(SalvageResultsMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(MapMarkerInfoMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(DebugActorTooltipMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(BossEncounterMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(EncounterInviteStateMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(BoolDataMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(CameraFocusMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(CameraZoomMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(CameraYawMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(BossZoomMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(EnchantItemMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(CraftingResultsMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(DebugDrawPrimMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(CrafterLevelUpMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(GameTestingSamplingStartMessage msg)
        {
            throw new NotImplementedException();
        }

        public void OnMessage(RequestBuffCancelMessage msg)
        {
            throw new NotImplementedException();
        }
    }
}
