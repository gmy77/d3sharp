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
using System.Text;
using System.Linq;
using D3Sharp.Core.Skills;
using D3Sharp.Net.Game.Message.Definitions.ACD;
using D3Sharp.Net.Game.Message.Definitions.Act;
using D3Sharp.Net.Game.Message.Definitions.Animation;
using D3Sharp.Net.Game.Message.Definitions.Attribute;
using D3Sharp.Net.Game.Message.Definitions.Connection;
using D3Sharp.Net.Game.Message.Definitions.Hero;
using D3Sharp.Net.Game.Message.Definitions.Inventory;
using D3Sharp.Net.Game.Message.Definitions.Map;
using D3Sharp.Net.Game.Message.Definitions.Misc;
using D3Sharp.Net.Game.Message.Definitions.Player;
using D3Sharp.Net.Game.Message.Definitions.Scene;
using D3Sharp.Net.Game.Message.Definitions.Team;
using D3Sharp.Net.Game.Messages;

namespace D3Sharp.Net.Game.Message.Definitions.Game
{
    [IncomingMessage(Opcodes.JoinBNetGameMessage)]
    public class JoinBNetGameMessage : GameMessage
    {
        public EntityId Field0;  // this *is* the toon id /raist.
        public GameId Field1;
        public int Field2; // and this is the SGameId there we set in D3Sharp.Core.Games.Game.cs when we send the connection info to client /raist.
        public long Field3;
        public int Field4;
        public int ProtocolHash;
        public int SNOPackHash;

        public override void Handle(GameClient client)
        {
            if (this.Id != 0x000A)
                throw new NotImplementedException();

            // a hackish way to get client.BnetClient in context -- pretends games has only one client in. when we're done with implementing bnet completely, will get this sorted out. /raist
            client.BnetClient = Core.Games.GameManager.AvailableGames[(ulong)this.Field2].Clients.FirstOrDefault();
            if (client.BnetClient != null) client.BnetClient.InGameClient = client;

            client.SendMessageNow(new VersionsMessage()
            {
                Id = 0x000D,
                SNOPackHash = this.SNOPackHash,
                ProtocolHash = GameMessage.ImplementedProtocolHash,
                Version = "0.3.0.7333",
            });

            client.SendMessage(new ConnectionEstablishedMessage()
            {
                Id = 0x002E,
                Field0 = 0x00000000,
                Field1 = 0x4BB91A16,
                Field2 = this.SNOPackHash,
            });

            client.SendMessage(new GameSetupMessage()
            {
                Id = 0x002F,
                Field0 = 0x00000077,
            });

            client.SendMessage(new SavePointInfoMessage()
            {
                Id = 0x0045,
                snoLevelArea = -1,
            });

            client.SendMessage(new HearthPortalInfoMessage()
            {
                Id = 0x0046,
                snoLevelArea = -1,
                Field1 = -1,
            });

            client.SendMessage(new ActTransitionMessage()
            {
                Id = 0x00A8,
                Field0 = 0x00000000,
                Field1 = true,
            });

            #region NewPlayer
            client.SendMessage(new NewPlayerMessage()
            {
                Id = 0x0031,
                Field0 = 0x00000000, //Party frame (0x00000000 hide, 0x00000001 show)
                Field1 = "", //Owner name?
                ToonName = client.BnetClient.CurrentToon.Name,
                Field3 = 0x00000002, //party frame class 
                Field4 = 0x00000004, //party frame level
                snoActorPortrait = client.BnetClient.CurrentToon.ClassSNO, //party frame portrait
                Field6 = 0x00000001,
                #region HeroStateData
                Field7 = new HeroStateData()
                {
                    Field0 = 0x00000000,
                    Field1 = 0x00000000,
                    Field2 = 0x00000000,
                    Field3 = client.BnetClient.CurrentToon.Gender,
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

            client.GameWorld.ReadAndSendMap();

            System.Console.WriteLine("Positioning character at " + client.GameWorld.posx + " " + client.GameWorld.posy + " " + client.GameWorld.posz);

            #region ACDEnterKnown 0x789E00E2 PlayerId??
            client.SendMessage(new ACDEnterKnownMessage()
            {
                Id = 0x003B,
                Field0 = 0x789E00E2,
                Field1 = client.BnetClient.CurrentToon.ClassSNO, //Player model?
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
                            Field0 = client.GameWorld.posx,
                            Field1 = client.GameWorld.posy,
                            Field2 = client.GameWorld.posz,
                        },
                    },
                    Field2 = client.GameWorld.WorldID,
                },
                Field5 = null,
                Field6 = new GBHandle()
                {
                    Field0 = 0x00000007,
                    Field1 = client.BnetClient.CurrentToon.ClassID,
                },
                Field7 = -1,
                Field8 = -1,
                Field9 = 0x00000000,
                Field10 = 0x00,
            });

            client.SendMessage(new ACDCollFlagsMessage()
            {
                Id = 0x00A6,
                Field0 = 0x789E00E2,
                Field1 = 0x00000000,
            });

            client.SendMessage(new AttributesSetValuesMessage()
            {
                Id = 0x004D,
                Field0 = 0x789E00E2,
                atKeyVals = new NetAttributeKeyValue[15]
    {
         new NetAttributeKeyValue()
         {
            Attribute = GameAttribute.Attributes[0x01F8], // SkillKit 
            Int = client.BnetClient.CurrentToon.SkillKit,
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

            client.SendMessage(new AttributesSetValuesMessage()
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

            client.SendMessage(new AttributesSetValuesMessage()
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
            Field0 = client.BnetClient.CurrentToon.ResourceID,
            Attribute = GameAttribute.Attributes[0x005E], // Resource_Cur 
            Int = 0x43480000,
            Float = 0f,
         },
         new NetAttributeKeyValue()
         {
            Field0 = client.BnetClient.CurrentToon.ResourceID,
            Attribute = GameAttribute.Attributes[0x005F], // Resource_Max 
            Int = 0x00000000,
            Float = 200f,
         },
         new NetAttributeKeyValue()
         {
            Field0 = client.BnetClient.CurrentToon.ResourceID,
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
            Field0 = client.BnetClient.CurrentToon.ResourceID,
            Attribute = GameAttribute.Attributes[0x0068], // Resource_Regen_Total 
            Int = 0x00000000,
            Float = 3.051758E-05f,
         },
    },
            });

            client.SendMessage(new AttributesSetValuesMessage()
            {
                Id = 0x004D,
                Field0 = 0x789E00E2,
                atKeyVals = new NetAttributeKeyValue[15]
    {
         new NetAttributeKeyValue()
         {
            Field0 = client.BnetClient.CurrentToon.ResourceID,
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

            client.SendMessage(new AttributesSetValuesMessage()
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

            client.SendMessage(new AttributesSetValuesMessage()
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

            client.SendMessage(new AttributesSetValuesMessage()
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

            client.SendMessage(new AttributesSetValuesMessage()
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

            client.SendMessage(new AttributesSetValuesMessage()
            {
                Id = 0x004D,
                Field0 = 0x789E00E2,
                atKeyVals = new NetAttributeKeyValue[15]
    {
         new NetAttributeKeyValue()
         {
            Attribute = GameAttribute.Attributes[0x005C], // Resource_Type_Primary 
            Int = client.BnetClient.CurrentToon.ResourceID,
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

            client.SendMessage(new AttributesSetValuesMessage()
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
            Int = client.BnetClient.CurrentToon.Level,
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

            client.SendMessage(new ACDGroupMessage()
            {
                Id = 0x00B8,
                Field0 = 0x789E00E2,
                Field1 = -1,
                Field2 = -1,
            });

            client.SendMessage(new ANNDataMessage()
            {
                Id = 0x003E,
                Field0 = 0x789E00E2,
            });

            client.SendMessage(new ACDTranslateFacingMessage()
            {
                Id = 0x0070,
                Field0 = 0x789E00E2,
                Field1 = 3.022712f,
                Field2 = false,
            });

            client.SendMessage(new PlayerEnterKnownMessage()
            {
                Id = 0x003D,
                Field0 = 0x00000000,
                Field1 = 0x789E00E2,
            });

            client.SendMessage(new VisualInventoryMessage()
            {
                Id = 0x004E,
                Field0 = 0x789E00E2,
                Field1 = new VisualEquipment()
                {
                    Field0 = new VisualItem[8]
        {
             new VisualItem() //Head
             {
                Field0 = client.BnetClient.CurrentToon.Equipment.VisualItemList[0].Gbid,
                Field1 = 0x00000000,
                Field2 = 0x00000000,
                Field3 = -1,
             },
             new VisualItem() //Chest
             {
                Field0 = client.BnetClient.CurrentToon.Equipment.VisualItemList[1].Gbid,
                Field1 = 0x00000000,
                Field2 = 0x00000000,
                Field3 = -1,
             },
             new VisualItem() //Feet
             {
                Field0 = client.BnetClient.CurrentToon.Equipment.VisualItemList[2].Gbid,
                Field1 = 0x00000000,
                Field2 = 0x00000000,
                Field3 = -1,
             },
             new VisualItem() //Hands
             {
                Field0 = client.BnetClient.CurrentToon.Equipment.VisualItemList[3].Gbid,
                Field1 = 0x00000000,
                Field2 = 0x00000000,
                Field3 = -1,
             },
             new VisualItem() //Main hand
             {
                Field0 = client.BnetClient.CurrentToon.Equipment.VisualItemList[4].Gbid,
                Field1 = 0x00000000,
                Field2 = 0x00000000,
                Field3 = -1,
             },
             new VisualItem() //Offhand
             {
                Field0 = client.BnetClient.CurrentToon.Equipment.VisualItemList[5].Gbid,
                Field1 = 0x00000000,
                Field2 = 0x00000000,
                Field3 = -1,
             },
             new VisualItem() //Shoulders
             {
                Field0 = client.BnetClient.CurrentToon.Equipment.VisualItemList[6].Gbid,
                Field1 = 0x00000000,
                Field2 = 0x00000000,
                Field3 = -1,
             },
             new VisualItem() //Legs
             {
                Field0 = client.BnetClient.CurrentToon.Equipment.VisualItemList[7].Gbid,
                Field1 = 0x00000000,
                Field2 = 0x00000000,
                Field3 = -1,
             },
        },
                },
            });

            client.SendMessage(new PlayerActorSetInitialMessage()
            {
                Id = 0x0039,
                Field0 = 0x789E00E2,
                Field1 = 0x00000000,
            });
            client.SendMessage(new SNONameDataMessage()
            {
                Id = 0x00D3,
                Field0 = new SNOName()
                {
                    Field0 = 0x00000001,
                    Field1 = client.BnetClient.CurrentToon.ClassSNO,
                },
            });
            #endregion

            #region ACDEnterKnown ALL Entries
            #region ACDEnterKnown 0x789700DB
            client.SendMessage(new ACDEnterKnownMessage()
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

            client.SendMessage(new AffixMessage()
            {
                Id = 0x0048,
                Field0 = 0x789700DB,
                Field1 = 0x00000001,
                aAffixGBIDs = new int[0]
    {
    },
            });

            client.SendMessage(new AffixMessage()
            {
                Id = 0x0048,
                Field0 = 0x789700DB,
                Field1 = 0x00000002,
                aAffixGBIDs = new int[0]
    {
    },
            });

            client.SendMessage(new ACDCollFlagsMessage()
            {
                Id = 0x00A6,
                Field0 = 0x789700DB,
                Field1 = 0x00000000,
            });

            client.SendMessage(new AttributesSetValuesMessage()
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

            client.SendMessage(new ACDGroupMessage()
            {
                Id = 0x00B8,
                Field0 = 0x789700DB,
                Field1 = -1,
                Field2 = -1,
            });

            client.SendMessage(new ANNDataMessage()
            {
                Id = 0x003E,
                Field0 = 0x789700DB,
            });

            client.SendMessage(new ACDTranslateFacingMessage()
            {
                Id = 0x0070,
                Field0 = 0x789700DB,
                Field1 = 0.7359439f,
                Field2 = false,
            });

            client.SendMessage(new SNONameDataMessage()
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
            client.SendMessage(new ACDEnterKnownMessage()
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

            client.SendMessage(new AffixMessage()
            {
                Id = 0x0048,
                Field0 = 0x789800DC,
                Field1 = 0x00000001,
                aAffixGBIDs = new int[0]
    {
    },
            });

            client.SendMessage(new AffixMessage()
            {
                Id = 0x0048,
                Field0 = 0x789800DC,
                Field1 = 0x00000002,
                aAffixGBIDs = new int[0]
    {
    },
            });

            client.SendMessage(new ACDCollFlagsMessage()
            {
                Id = 0x00A6,
                Field0 = 0x789800DC,
                Field1 = 0x00000000,
            });

            client.SendMessage(new AttributesSetValuesMessage()
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

            client.SendMessage(new ACDGroupMessage()
            {
                Id = 0x00B8,
                Field0 = 0x789800DC,
                Field1 = -1,
                Field2 = -1,
            });

            client.SendMessage(new ANNDataMessage()
            {
                Id = 0x003E,
                Field0 = 0x789800DC,
            });

            client.SendMessage(new ACDTranslateFacingMessage()
            {
                Id = 0x0070,
                Field0 = 0x789800DC,
                Field1 = 3.022712f,
                Field2 = false,
            });

            client.SendMessage(new SNONameDataMessage()
            {
                Id = 0x00D3,
                Field0 = new SNOName()
                {
                    Field0 = 0x00000001,
                    Field1 = 0x0000157F,
                },
            });

            client.SendMessage(new ACDEnterKnownMessage()
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

            client.SendMessage(new AffixMessage()
            {
                Id = 0x0048,
                Field0 = 0x789900DD,
                Field1 = 0x00000001,
                aAffixGBIDs = new int[0]
    {
    },
            });

            client.SendMessage(new AffixMessage()
            {
                Id = 0x0048,
                Field0 = 0x789900DD,
                Field1 = 0x00000002,
                aAffixGBIDs = new int[0]
    {
    },
            });

            client.SendMessage(new ACDCollFlagsMessage()
            {
                Id = 0x00A6,
                Field0 = 0x789900DD,
                Field1 = 0x00000411,
            });

            client.SendMessage(new AttributesSetValuesMessage()
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

            client.SendMessage(new ACDGroupMessage()
            {
                Id = 0x00B8,
                Field0 = 0x789900DD,
                Field1 = -1,
                Field2 = -1,
            });

            client.SendMessage(new ANNDataMessage()
            {
                Id = 0x003E,
                Field0 = 0x789900DD,
            });

            client.SendMessage(new ACDTranslateFacingMessage()
            {
                Id = 0x0070,
                Field0 = 0x789900DD,
                Field1 = 0.3123385f,
                Field2 = false,
            });

            client.SendMessage(new SNONameDataMessage()
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
            client.SendMessage(new ACDEnterKnownMessage()
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

            client.SendMessage(new AffixMessage()
            {
                Id = 0x0048,
                Field0 = 0x789A00DE,
                Field1 = 0x00000001,
                aAffixGBIDs = new int[0]
    {
    },
            });

            client.SendMessage(new AffixMessage()
            {
                Id = 0x0048,
                Field0 = 0x789A00DE,
                Field1 = 0x00000002,
                aAffixGBIDs = new int[0]
    {
    },
            });

            client.SendMessage(new ACDCollFlagsMessage()
            {
                Id = 0x00A6,
                Field0 = 0x789A00DE,
                Field1 = 0x00000001,
            });

            client.SendMessage(new AttributesSetValuesMessage()
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

            client.SendMessage(new ACDGroupMessage()
            {
                Id = 0x00B8,
                Field0 = 0x789A00DE,
                Field1 = -1,
                Field2 = -1,
            });

            client.SendMessage(new ANNDataMessage()
            {
                Id = 0x003E,
                Field0 = 0x789A00DE,
            });

            client.SendMessage(new ACDTranslateFacingMessage()
            {
                Id = 0x0070,
                Field0 = 0x789A00DE,
                Field1 = 4.884459f,
                Field2 = false,
            });

            client.SendMessage(new SNONameDataMessage()
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
            client.SendMessage(new ACDEnterKnownMessage()
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

            client.SendMessage(new AffixMessage()
            {
                Id = 0x0048,
                Field0 = 0x789B00DF,
                Field1 = 0x00000001,
                aAffixGBIDs = new int[0]
    {
    },
            });

            client.SendMessage(new AffixMessage()
            {
                Id = 0x0048,
                Field0 = 0x789B00DF,
                Field1 = 0x00000002,
                aAffixGBIDs = new int[0]
    {
    },
            });

            client.SendMessage(new ACDCollFlagsMessage()
            {
                Id = 0x00A6,
                Field0 = 0x789B00DF,
                Field1 = 0x00000000,
            });

            client.SendMessage(new AttributesSetValuesMessage()
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

            client.SendMessage(new ACDGroupMessage()
            {
                Id = 0x00B8,
                Field0 = 0x789B00DF,
                Field1 = -1,
                Field2 = -1,
            });

            client.SendMessage(new ANNDataMessage()
            {
                Id = 0x003E,
                Field0 = 0x789B00DF,
            });

            client.SendMessage(new ACDTranslateFacingMessage()
            {
                Id = 0x0070,
                Field0 = 0x789B00DF,
                Field1 = 6.147496f,
                Field2 = false,
            });

            client.SendMessage(new SNONameDataMessage()
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
            client.SendMessage(new ACDEnterKnownMessage()
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

            client.SendMessage(new AffixMessage()
            {
                Id = 0x0048,
                Field0 = 0x789C00E0,
                Field1 = 0x00000001,
                aAffixGBIDs = new int[0]
    {
    },
            });

            client.SendMessage(new AffixMessage()
            {
                Id = 0x0048,
                Field0 = 0x789C00E0,
                Field1 = 0x00000002,
                aAffixGBIDs = new int[0]
    {
    },
            });

            client.SendMessage(new ACDCollFlagsMessage()
            {
                Id = 0x00A6,
                Field0 = 0x789C00E0,
                Field1 = 0x00000001,
            });

            client.SendMessage(new AttributesSetValuesMessage()
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

            client.SendMessage(new ACDGroupMessage()
            {
                Id = 0x00B8,
                Field0 = 0x789C00E0,
                Field1 = -1,
                Field2 = -1,
            });

            client.SendMessage(new ANNDataMessage()
            {
                Id = 0x003E,
                Field0 = 0x789C00E0,
            });

            client.SendMessage(new ACDTranslateFacingMessage()
            {
                Id = 0x0070,
                Field0 = 0x789C00E0,
                Field1 = 2.104887f,
                Field2 = false,
            });

            client.SendMessage(new SNONameDataMessage()
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
            client.SendMessage(new ACDEnterKnownMessage()
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
                    Field1 = client.BnetClient.CurrentToon.Equipment.VisualItemList[4].Gbid,
                },
                Field7 = -1,
                Field8 = -1,
                Field9 = 0x00000001,
                Field10 = 0x00,
            });

            client.SendMessage(new AffixMessage()
            {
                Id = 0x0048,
                Field0 = 0x789F00E3,
                Field1 = 0x00000001,
                aAffixGBIDs = new int[0]
    {
    },
            });

            client.SendMessage(new AffixMessage()
            {
                Id = 0x0048,
                Field0 = 0x789F00E3,
                Field1 = 0x00000002,
                aAffixGBIDs = new int[0]
    {
    },
            });

            client.SendMessage(new ACDCollFlagsMessage()
            {
                Id = 0x00A6,
                Field0 = 0x789F00E3,
                Field1 = 0x00000080,
            });

            client.SendMessage(new AttributesSetValuesMessage()
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

            client.SendMessage(new AttributesSetValuesMessage()
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

            client.SendMessage(new AttributesSetValuesMessage()
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

            client.SendMessage(new ACDGroupMessage()
            {
                Id = 0x00B8,
                Field0 = 0x789F00E3,
                Field1 = -1,
                Field2 = -1,
            });

            client.SendMessage(new ANNDataMessage()
            {
                Id = 0x003E,
                Field0 = 0x789F00E3,
            });

            client.SendMessage(new SNONameDataMessage()
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
            client.SendMessage(new ACDEnterKnownMessage()
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

            client.SendMessage(new AffixMessage()
            {
                Id = 0x0048,
                Field0 = 0x78A000E4,
                Field1 = 0x00000001,
                aAffixGBIDs = new int[0]
    {
    },
            });

            client.SendMessage(new AffixMessage()
            {
                Id = 0x0048,
                Field0 = 0x78A000E4,
                Field1 = 0x00000002,
                aAffixGBIDs = new int[0]
    {
    },
            });

            client.SendMessage(new ACDCollFlagsMessage()
            {
                Id = 0x00A6,
                Field0 = 0x78A000E4,
                Field1 = 0x00000080,
            });

            client.SendMessage(new AttributesSetValuesMessage()
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

            client.SendMessage(new ACDGroupMessage()
            {
                Id = 0x00B8,
                Field0 = 0x78A000E4,
                Field1 = -1,
                Field2 = -1,
            });

            client.SendMessage(new ANNDataMessage()
            {
                Id = 0x003E,
                Field0 = 0x78A000E4,
            });

            client.SendMessage(new SNONameDataMessage()
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
            client.SendMessage(new ACDEnterKnownMessage()
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

            client.SendMessage(new AffixMessage()
            {
                Id = 0x0048,
                Field0 = 0x78BE0102,
                Field1 = 0x00000001,
                aAffixGBIDs = new int[0]
    {
    },
            });

            client.SendMessage(new AffixMessage()
            {
                Id = 0x0048,
                Field0 = 0x78BE0102,
                Field1 = 0x00000002,
                aAffixGBIDs = new int[0]
    {
    },
            });

            client.SendMessage(new ACDCollFlagsMessage()
            {
                Id = 0x00A6,
                Field0 = 0x78BE0102,
                Field1 = 0x00000102,
            });

            client.SendMessage(new AttributesSetValuesMessage()
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

            client.SendMessage(new ACDGroupMessage()
            {
                Id = 0x00B8,
                Field0 = 0x78BE0102,
                Field1 = -1,
                Field2 = -1,
            });

            client.SendMessage(new ANNDataMessage()
            {
                Id = 0x003E,
                Field0 = 0x78BE0102,
            });

            client.SendMessage(new ACDTranslateFacingMessage()
            {
                Id = 0x0070,
                Field0 = 0x78BE0102,
                Field1 = 5.385688f,
                Field2 = false,
            });

            client.SendMessage(new SetIdleAnimationMessage()
            {
                Id = 0x00A5,
                Field0 = 0x78BE0102,
                Field1 = 0x00011150,
            });

            client.SendMessage(new SNONameDataMessage()
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
            client.SendMessage(new ACDEnterKnownMessage()
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

            client.SendMessage(new AffixMessage()
            {
                Id = 0x0048,
                Field0 = 0x78DD0118,
                Field1 = 0x00000001,
                aAffixGBIDs = new int[0]
    {
    },
            });

            client.SendMessage(new AffixMessage()
            {
                Id = 0x0048,
                Field0 = 0x78DD0118,
                Field1 = 0x00000002,
                aAffixGBIDs = new int[0]
    {
    },
            });

            client.SendMessage(new ACDCollFlagsMessage()
            {
                Id = 0x00A6,
                Field0 = 0x78DD0118,
                Field1 = 0x00000000,
            });

            client.SendMessage(new AttributesSetValuesMessage()
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

            client.SendMessage(new ACDGroupMessage()
            {
                Id = 0x00B8,
                Field0 = 0x78DD0118,
                Field1 = -1,
                Field2 = -1,
            });

            client.SendMessage(new ANNDataMessage()
            {
                Id = 0x003E,
                Field0 = 0x78DD0118,
            });

            client.SendMessage(new ACDTranslateFacingMessage()
            {
                Id = 0x0070,
                Field0 = 0x78DD0118,
                Field1 = 3.163388f,
                Field2 = false,
            });

            client.SendMessage(new SNONameDataMessage()
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
            client.SendMessage(new ACDEnterKnownMessage()
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

            client.SendMessage(new AffixMessage()
            {
                Id = 0x0048,
                Field0 = 0x78DE0119,
                Field1 = 0x00000001,
                aAffixGBIDs = new int[0]
    {
    },
            });

            client.SendMessage(new AffixMessage()
            {
                Id = 0x0048,
                Field0 = 0x78DE0119,
                Field1 = 0x00000002,
                aAffixGBIDs = new int[0]
    {
    },
            });

            client.SendMessage(new ACDCollFlagsMessage()
            {
                Id = 0x00A6,
                Field0 = 0x78DE0119,
                Field1 = 0x00000000,
            });

            client.SendMessage(new AttributesSetValuesMessage()
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

            client.SendMessage(new ACDGroupMessage()
            {
                Id = 0x00B8,
                Field0 = 0x78DE0119,
                Field1 = -1,
                Field2 = -1,
            });

            client.SendMessage(new ANNDataMessage()
            {
                Id = 0x003E,
                Field0 = 0x78DE0119,
            });

            client.SendMessage(new ACDTranslateFacingMessage()
            {
                Id = 0x0070,
                Field0 = 0x78DE0119,
                Field1 = 0f,
                Field2 = false,
            });

            client.SendMessage(new SNONameDataMessage()
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
            client.SendMessage(new ACDEnterKnownMessage()
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

            client.SendMessage(new AffixMessage()
            {
                Id = 0x0048,
                Field0 = 0x78DF011A,
                Field1 = 0x00000001,
                aAffixGBIDs = new int[0]
    {
    },
            });

            client.SendMessage(new AffixMessage()
            {
                Id = 0x0048,
                Field0 = 0x78DF011A,
                Field1 = 0x00000002,
                aAffixGBIDs = new int[0]
    {
    },
            });

            client.SendMessage(new ACDCollFlagsMessage()
            {
                Id = 0x00A6,
                Field0 = 0x78DF011A,
                Field1 = 0x00000001,
            });

            client.SendMessage(new AttributesSetValuesMessage()
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

            client.SendMessage(new ACDGroupMessage()
            {
                Id = 0x00B8,
                Field0 = 0x78DF011A,
                Field1 = -1,
                Field2 = -1,
            });

            client.SendMessage(new ANNDataMessage()
            {
                Id = 0x003E,
                Field0 = 0x78DF011A,
            });

            client.SendMessage(new ACDTranslateFacingMessage()
            {
                Id = 0x0070,
                Field0 = 0x78DF011A,
                Field1 = 2.888546f,
                Field2 = false,
            });

            client.SendMessage(new SetIdleAnimationMessage()
            {
                Id = 0x00A5,
                Field0 = 0x78DF011A,
                Field1 = 0x00011150,
            });

            client.SendMessage(new SNONameDataMessage()
            {
                Id = 0x00D3,
                Field0 = new SNOName()
                {
                    Field0 = 0x00000001,
                    Field1 = 0x000255BB,
                },
            });

            client.SendMessage(new HeroStateMessage()
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
                     // Left Click
                    m_snoPower = (int)Skills.Wizard.PowerHungry,
                    m_gbidItem = -1,
                 },
                 new HotbarButtonData()
                 {
                     // Right Click
                    m_snoPower = (int)Skills.Barbarian.Whirlwind,
                    m_gbidItem = -1,
                 },
                 new HotbarButtonData()
                 {
                     // Unknown - Left-Click Switch ?
                    m_snoPower = (int)Skills.None,
                    m_gbidItem = -1,
                 },
                 new HotbarButtonData()
                 {
                     // Right-Click Switch - Press X ingame
                    m_snoPower = (int)Skills.None,
                    m_gbidItem = -1,
                 },
                 new HotbarButtonData()
                 {
                     // QuickKey 1
                    m_snoPower = (int)Skills.DemonHunter.Companion,
                    m_gbidItem = -1,
                 },
                 new HotbarButtonData()
                 {
                     // QuickKey 2
                    m_snoPower = (int)Skills.Monk.ExplodingPalm,
                    m_gbidItem = -1,
                 },
                 new HotbarButtonData()
                 {
                     // QuickKey 3
                    m_snoPower = (int)Skills.WitchDoctor.SpiritWalk,
                    m_gbidItem = -1,
                 },
                 new HotbarButtonData()
                 {
                     // QuickKey 4
                    m_snoPower = (int)Skills.Wizard.Hydra,
                    m_gbidItem = -1,
                 },
                 new HotbarButtonData()
                 {
                     // QuickKey 5
                    m_snoPower = (int)Skills.None,
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

            client.SendMessage(new ANNDataMessage()
            {
                Id = 0x003C,
                Field0 = 0x78BE0102,
            });

            client.SendMessage(new ANNDataMessage()
            {
                Id = 0x003C,
                Field0 = 0x78DF011A,
            });

            client.SendMessage(new RevealTeamMessage()
            {
                Id = 0x0038,
                Field0 = 0x00000000,
                Field1 = 0x00000000,
                Field2 = -1,
            });

            client.SendMessage(new RevealTeamMessage()
            {
                Id = 0x0038,
                Field0 = 0x00000001,
                Field1 = 0x00000000,
                Field2 = -1,
            });

            client.SendMessage(new RevealTeamMessage()
            {
                Id = 0x0038,
                Field0 = 0x00000002,
                Field1 = 0x00000000,
                Field2 = -1,
            });

            client.SendMessage(new RevealTeamMessage()
            {
                Id = 0x0038,
                Field0 = 0x00000003,
                Field1 = 0x00000000,
                Field2 = -1,
            });

            client.SendMessage(new RevealTeamMessage()
            {
                Id = 0x0038,
                Field0 = 0x00000004,
                Field1 = 0x00000000,
                Field2 = -1,
            });

            client.SendMessage(new RevealTeamMessage()
            {
                Id = 0x0038,
                Field0 = 0x00000005,
                Field1 = 0x00000000,
                Field2 = -1,
            });

            client.SendMessage(new RevealTeamMessage()
            {
                Id = 0x0038,
                Field0 = 0x00000006,
                Field1 = 0x00000000,
                Field2 = -1,
            });

            client.SendMessage(new RevealTeamMessage()
            {
                Id = 0x0038,
                Field0 = 0x00000007,
                Field1 = 0x00000000,
                Field2 = -1,
            });

            client.SendMessage(new RevealTeamMessage()
            {
                Id = 0x0038,
                Field0 = 0x00000008,
                Field1 = 0x00000000,
                Field2 = -1,
            });

            client.SendMessage(new RevealTeamMessage()
            {
                Id = 0x0038,
                Field0 = 0x00000009,
                Field1 = 0x00000000,
                Field2 = -1,
            });

            client.SendMessage(new RevealTeamMessage()
            {
                Id = 0x0038,
                Field0 = 0x0000000A,
                Field1 = 0x00000002,
                Field2 = -1,
            });

            client.SendMessage(new RevealTeamMessage()
            {
                Id = 0x0038,
                Field0 = 0x0000000B,
                Field1 = 0x00000002,
                Field2 = -1,
            });

            client.SendMessage(new RevealTeamMessage()
            {
                Id = 0x0038,
                Field0 = 0x0000000C,
                Field1 = 0x00000002,
                Field2 = -1,
            });

            client.SendMessage(new RevealTeamMessage()
            {
                Id = 0x0038,
                Field0 = 0x0000000D,
                Field1 = 0x00000002,
                Field2 = -1,
            });

            client.SendMessage(new RevealTeamMessage()
            {
                Id = 0x0038,
                Field0 = 0x0000000E,
                Field1 = 0x00000000,
                Field2 = 0x00000000,
            });

            client.SendMessage(new RevealTeamMessage()
            {
                Id = 0x0038,
                Field0 = 0x0000000F,
                Field1 = 0x00000000,
                Field2 = 0x00000000,
            });

            client.SendMessage(new RevealTeamMessage()
            {
                Id = 0x0038,
                Field0 = 0x00000010,
                Field1 = 0x00000000,
                Field2 = 0x00000000,
            });

            client.SendMessage(new RevealTeamMessage()
            {
                Id = 0x0038,
                Field0 = 0x00000011,
                Field1 = 0x00000000,
                Field2 = 0x00000000,
            });

            client.SendMessage(new RevealTeamMessage()
            {
                Id = 0x0038,
                Field0 = 0x00000012,
                Field1 = 0x00000000,
                Field2 = 0x00000000,
            });

            client.SendMessage(new RevealTeamMessage()
            {
                Id = 0x0038,
                Field0 = 0x00000013,
                Field1 = 0x00000000,
                Field2 = 0x00000000,
            });

            client.SendMessage(new RevealTeamMessage()
            {
                Id = 0x0038,
                Field0 = 0x00000014,
                Field1 = 0x00000000,
                Field2 = 0x00000000,
            });

            client.SendMessage(new RevealTeamMessage()
            {
                Id = 0x0038,
                Field0 = 0x00000015,
                Field1 = 0x00000000,
                Field2 = 0x00000000,
            });

            client.SendMessage(new RevealTeamMessage()
            {
                Id = 0x0038,
                Field0 = 0x00000016,
                Field1 = 0x00000000,
                Field2 = 0x00000000,
            });

            client.SendMessage(new RevealSceneMessage()
            {
                Id = 0x0034,
                WorldID = 0x772E0000,
                SceneSpec = new SceneSpecification()
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
                ChunkID = 0x77550001,
                snoScene = 0x00008244,
                Position = new PRTransform()
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
                ParentChunkID = -1,
                snoSceneGroup = -1,
                arAppliedLabels = new int[1]
    {
        0x00360E26, 
    },
            });

            client.SendMessage(new MapRevealSceneMessage()
            {
                Id = 0x0044,
                ChunkID = 0x77550001,
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
                MiniMapVisibility = 0x00000002,
            });
            #endregion
            #region ACDEnterKnown 0x77BC0000
            client.SendMessage(new ACDEnterKnownMessage()
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

            client.SendMessage(new AffixMessage()
            {
                Id = 0x0048,
                Field0 = 0x77BC0000,
                Field1 = 0x00000001,
                aAffixGBIDs = new int[0]
    {
    },
            });

            client.SendMessage(new AffixMessage()
            {
                Id = 0x0048,
                Field0 = 0x77BC0000,
                Field1 = 0x00000002,
                aAffixGBIDs = new int[0]
    {
    },
            });

            client.SendMessage(new ACDCollFlagsMessage()
            {
                Id = 0x00A6,
                Field0 = 0x77BC0000,
                Field1 = 0x00000000,
            });

            client.SendMessage(new AttributesSetValuesMessage()
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

            client.SendMessage(new ACDGroupMessage()
            {
                Id = 0x00B8,
                Field0 = 0x77BC0000,
                Field1 = -1,
                Field2 = -1,
            });

            client.SendMessage(new ANNDataMessage()
            {
                Id = 0x003E,
                Field0 = 0x77BC0000,
            });

            client.SendMessage(new ACDTranslateFacingMessage()
            {
                Id = 0x0070,
                Field0 = 0x77BC0000,
                Field1 = 0f,
                Field2 = false,
            });
            #endregion
            #region ACDEnterKnown 0x77C10005
            client.SendMessage(new ACDEnterKnownMessage()
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

            client.SendMessage(new AffixMessage()
            {
                Id = 0x0048,
                Field0 = 0x77C10005,
                Field1 = 0x00000001,
                aAffixGBIDs = new int[0]
    {
    },
            });

            client.SendMessage(new AffixMessage()
            {
                Id = 0x0048,
                Field0 = 0x77C10005,
                Field1 = 0x00000002,
                aAffixGBIDs = new int[0]
    {
    },
            });

            client.SendMessage(new ACDCollFlagsMessage()
            {
                Id = 0x00A6,
                Field0 = 0x77C10005,
                Field1 = 0x00000080,
            });

            client.SendMessage(new AttributesSetValuesMessage()
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

            client.SendMessage(new ACDGroupMessage()
            {
                Id = 0x00B8,
                Field0 = 0x77C10005,
                Field1 = -1,
                Field2 = -1,
            });

            client.SendMessage(new ANNDataMessage()
            {
                Id = 0x003E,
                Field0 = 0x77C10005,
            });

            client.SendMessage(new ACDTranslateFacingMessage()
            {
                Id = 0x0070,
                Field0 = 0x77C10005,
                Field1 = 5.492608f,
                Field2 = false,
            });
            #endregion
            #region ACDEnterKnown 0x77C50009
            client.SendMessage(new ACDEnterKnownMessage()
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

            client.SendMessage(new AffixMessage()
            {
                Id = 0x0048,
                Field0 = 0x77C50009,
                Field1 = 0x00000001,
                aAffixGBIDs = new int[0]
    {
    },
            });

            client.SendMessage(new AffixMessage()
            {
                Id = 0x0048,
                Field0 = 0x77C50009,
                Field1 = 0x00000002,
                aAffixGBIDs = new int[0]
    {
    },
            });

            client.SendMessage(new ACDCollFlagsMessage()
            {
                Id = 0x00A6,
                Field0 = 0x77C50009,
                Field1 = 0x00000411,
            });

            client.SendMessage(new AttributesSetValuesMessage()
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

            client.SendMessage(new ACDGroupMessage()
            {
                Id = 0x00B8,
                Field0 = 0x77C50009,
                Field1 = -1,
                Field2 = -1,
            });

            client.SendMessage(new ANNDataMessage()
            {
                Id = 0x003E,
                Field0 = 0x77C50009,
            });

            client.SendMessage(new ACDTranslateFacingMessage()
            {
                Id = 0x0070,
                Field0 = 0x77C50009,
                Field1 = 0.04497663f,
                Field2 = false,
            });
            #endregion
            #region ACDEnterKnown 0x77C9000D
            client.SendMessage(new ACDEnterKnownMessage()
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

            client.SendMessage(new AffixMessage()
            {
                Id = 0x0048,
                Field0 = 0x77C9000D,
                Field1 = 0x00000001,
                aAffixGBIDs = new int[0]
    {
    },
            });

            client.SendMessage(new AffixMessage()
            {
                Id = 0x0048,
                Field0 = 0x77C9000D,
                Field1 = 0x00000002,
                aAffixGBIDs = new int[0]
    {
    },
            });

            client.SendMessage(new ACDCollFlagsMessage()
            {
                Id = 0x00A6,
                Field0 = 0x77C9000D,
                Field1 = 0x00000000,
            });

            client.SendMessage(new AttributesSetValuesMessage()
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

            client.SendMessage(new ACDGroupMessage()
            {
                Id = 0x00B8,
                Field0 = 0x77C9000D,
                Field1 = -1,
                Field2 = -1,
            });

            client.SendMessage(new ANNDataMessage()
            {
                Id = 0x003E,
                Field0 = 0x77C9000D,
            });

            client.SendMessage(new ACDTranslateFacingMessage()
            {
                Id = 0x0070,
                Field0 = 0x77C9000D,
                Field1 = 1.383674f,
                Field2 = false,
            });
            #endregion
            #region ACDEnterKnown 0x77CB000F
            client.SendMessage(new ACDEnterKnownMessage()
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

            client.SendMessage(new AffixMessage()
            {
                Id = 0x0048,
                Field0 = 0x77CB000F,
                Field1 = 0x00000001,
                aAffixGBIDs = new int[0]
    {
    },
            });

            client.SendMessage(new AffixMessage()
            {
                Id = 0x0048,
                Field0 = 0x77CB000F,
                Field1 = 0x00000002,
                aAffixGBIDs = new int[0]
    {
    },
            });

            client.SendMessage(new ACDCollFlagsMessage()
            {
                Id = 0x00A6,
                Field0 = 0x77CB000F,
                Field1 = 0x00000000,
            });

            client.SendMessage(new AttributesSetValuesMessage()
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

            client.SendMessage(new ACDGroupMessage()
            {
                Id = 0x00B8,
                Field0 = 0x77CB000F,
                Field1 = -1,
                Field2 = -1,
            });

            client.SendMessage(new ANNDataMessage()
            {
                Id = 0x003E,
                Field0 = 0x77CB000F,
            });

            client.SendMessage(new ACDTranslateFacingMessage()
            {
                Id = 0x0070,
                Field0 = 0x77CB000F,
                Field1 = 0f,
                Field2 = false,
            });
            #endregion
            #region ACDEnterKnown 0x77DA001E
            client.SendMessage(new ACDEnterKnownMessage()
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

            client.SendMessage(new AffixMessage()
            {
                Id = 0x0048,
                Field0 = 0x77DA001E,
                Field1 = 0x00000001,
                aAffixGBIDs = new int[0]
    {
    },
            });

            client.SendMessage(new AffixMessage()
            {
                Id = 0x0048,
                Field0 = 0x77DA001E,
                Field1 = 0x00000002,
                aAffixGBIDs = new int[0]
    {
    },
            });

            client.SendMessage(new ACDCollFlagsMessage()
            {
                Id = 0x00A6,
                Field0 = 0x77DA001E,
                Field1 = 0x00000001,
            });

            client.SendMessage(new AttributesSetValuesMessage()
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

            client.SendMessage(new ACDGroupMessage()
            {
                Id = 0x00B8,
                Field0 = 0x77DA001E,
                Field1 = -1,
                Field2 = -1,
            });

            client.SendMessage(new ANNDataMessage()
            {
                Id = 0x003E,
                Field0 = 0x77DA001E,
            });

            client.SendMessage(new ACDTranslateFacingMessage()
            {
                Id = 0x0070,
                Field0 = 0x77DA001E,
                Field1 = 0.02320435f,
                Field2 = false,
            });
            #endregion
            #region ACDEnterKnown 0x77DB001F
            client.SendMessage(new ACDEnterKnownMessage()
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

            client.SendMessage(new AffixMessage()
            {
                Id = 0x0048,
                Field0 = 0x77DB001F,
                Field1 = 0x00000001,
                aAffixGBIDs = new int[0]
    {
    },
            });

            client.SendMessage(new AffixMessage()
            {
                Id = 0x0048,
                Field0 = 0x77DB001F,
                Field1 = 0x00000002,
                aAffixGBIDs = new int[0]
    {
    },
            });

            client.SendMessage(new ACDCollFlagsMessage()
            {
                Id = 0x00A6,
                Field0 = 0x77DB001F,
                Field1 = 0x00000000,
            });

            client.SendMessage(new AttributesSetValuesMessage()
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

            client.SendMessage(new ACDGroupMessage()
            {
                Id = 0x00B8,
                Field0 = 0x77DB001F,
                Field1 = -1,
                Field2 = -1,
            });

            client.SendMessage(new ANNDataMessage()
            {
                Id = 0x003E,
                Field0 = 0x77DB001F,
            });

            client.SendMessage(new ACDTranslateFacingMessage()
            {
                Id = 0x0070,
                Field0 = 0x77DB001F,
                Field1 = 0f,
                Field2 = false,
            });
            #endregion
            #region ACDEnterKnown 0x77E20026
            client.SendMessage(new ACDEnterKnownMessage()
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

            client.SendMessage(new AffixMessage()
            {
                Id = 0x0048,
                Field0 = 0x77E20026,
                Field1 = 0x00000001,
                aAffixGBIDs = new int[0]
    {
    },
            });

            client.SendMessage(new AffixMessage()
            {
                Id = 0x0048,
                Field0 = 0x77E20026,
                Field1 = 0x00000002,
                aAffixGBIDs = new int[0]
    {
    },
            });

            client.SendMessage(new ACDCollFlagsMessage()
            {
                Id = 0x00A6,
                Field0 = 0x77E20026,
                Field1 = 0x00000000,
            });

            client.SendMessage(new AttributesSetValuesMessage()
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

            client.SendMessage(new ACDGroupMessage()
            {
                Id = 0x00B8,
                Field0 = 0x77E20026,
                Field1 = -1,
                Field2 = -1,
            });

            client.SendMessage(new ANNDataMessage()
            {
                Id = 0x003E,
                Field0 = 0x77E20026,
            });

            client.SendMessage(new ACDTranslateFacingMessage()
            {
                Id = 0x0070,
                Field0 = 0x77E20026,
                Field1 = 1.383674f,
                Field2 = false,
            });
            #endregion
            #region ACDEnterKnown 0x77E6002A
            client.SendMessage(new ACDEnterKnownMessage()
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

            client.SendMessage(new AffixMessage()
            {
                Id = 0x0048,
                Field0 = 0x77E6002A,
                Field1 = 0x00000001,
                aAffixGBIDs = new int[0]
    {
    },
            });

            client.SendMessage(new AffixMessage()
            {
                Id = 0x0048,
                Field0 = 0x77E6002A,
                Field1 = 0x00000002,
                aAffixGBIDs = new int[0]
    {
    },
            });

            client.SendMessage(new ACDCollFlagsMessage()
            {
                Id = 0x00A6,
                Field0 = 0x77E6002A,
                Field1 = 0x00000000,
            });

            client.SendMessage(new AttributesSetValuesMessage()
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

            client.SendMessage(new ACDGroupMessage()
            {
                Id = 0x00B8,
                Field0 = 0x77E6002A,
                Field1 = -1,
                Field2 = -1,
            });

            client.SendMessage(new ANNDataMessage()
            {
                Id = 0x003E,
                Field0 = 0x77E6002A,
            });

            client.SendMessage(new ACDTranslateFacingMessage()
            {
                Id = 0x0070,
                Field0 = 0x77E6002A,
                Field1 = 0f,
                Field2 = false,
            });
            #endregion
            #region ACDEnterKnown 0x77E8002C
            client.SendMessage(new ACDEnterKnownMessage()
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

            client.SendMessage(new AffixMessage()
            {
                Id = 0x0048,
                Field0 = 0x77E8002C,
                Field1 = 0x00000001,
                aAffixGBIDs = new int[0]
    {
    },
            });

            client.SendMessage(new AffixMessage()
            {
                Id = 0x0048,
                Field0 = 0x77E8002C,
                Field1 = 0x00000002,
                aAffixGBIDs = new int[0]
    {
    },
            });

            client.SendMessage(new ACDCollFlagsMessage()
            {
                Id = 0x00A6,
                Field0 = 0x77E8002C,
                Field1 = 0x00000000,
            });

            client.SendMessage(new AttributesSetValuesMessage()
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

            client.SendMessage(new ACDGroupMessage()
            {
                Id = 0x00B8,
                Field0 = 0x77E8002C,
                Field1 = -1,
                Field2 = -1,
            });

            client.SendMessage(new ANNDataMessage()
            {
                Id = 0x003E,
                Field0 = 0x77E8002C,
            });

            client.SendMessage(new ACDTranslateFacingMessage()
            {
                Id = 0x0070,
                Field0 = 0x77E8002C,
                Field1 = 0f,
                Field2 = false,
            });
            #endregion
            #region ACDEnterKnown 0x77EB002F
            client.SendMessage(new ACDEnterKnownMessage()
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

            client.SendMessage(new AffixMessage()
            {
                Id = 0x0048,
                Field0 = 0x77EB002F,
                Field1 = 0x00000001,
                aAffixGBIDs = new int[0]
    {
    },
            });

            client.SendMessage(new AffixMessage()
            {
                Id = 0x0048,
                Field0 = 0x77EB002F,
                Field1 = 0x00000002,
                aAffixGBIDs = new int[0]
    {
    },
            });

            client.SendMessage(new ACDCollFlagsMessage()
            {
                Id = 0x00A6,
                Field0 = 0x77EB002F,
                Field1 = 0x00000000,
            });

            client.SendMessage(new AttributesSetValuesMessage()
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

            client.SendMessage(new ACDGroupMessage()
            {
                Id = 0x00B8,
                Field0 = 0x77EB002F,
                Field1 = -1,
                Field2 = -1,
            });

            client.SendMessage(new ANNDataMessage()
            {
                Id = 0x003E,
                Field0 = 0x77EB002F,
            });

            client.SendMessage(new ACDTranslateFacingMessage()
            {
                Id = 0x0070,
                Field0 = 0x77EB002F,
                Field1 = 0f,
                Field2 = false,
            });
            #endregion
            #region ACDEnterKnown 0x77F00034
            client.SendMessage(new ACDEnterKnownMessage()
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

            client.SendMessage(new AffixMessage()
            {
                Id = 0x0048,
                Field0 = 0x77F00034,
                Field1 = 0x00000001,
                aAffixGBIDs = new int[0]
    {
    },
            });

            client.SendMessage(new AffixMessage()
            {
                Id = 0x0048,
                Field0 = 0x77F00034,
                Field1 = 0x00000002,
                aAffixGBIDs = new int[0]
    {
    },
            });

            client.SendMessage(new ACDCollFlagsMessage()
            {
                Id = 0x00A6,
                Field0 = 0x77F00034,
                Field1 = 0x00000000,
            });

            client.SendMessage(new AttributesSetValuesMessage()
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

            client.SendMessage(new ACDGroupMessage()
            {
                Id = 0x00B8,
                Field0 = 0x77F00034,
                Field1 = -1,
                Field2 = -1,
            });

            client.SendMessage(new ANNDataMessage()
            {
                Id = 0x003E,
                Field0 = 0x77F00034,
            });

            client.SendMessage(new ACDTranslateFacingMessage()
            {
                Id = 0x0070,
                Field0 = 0x77F00034,
                Field1 = 5.546649f,
                Field2 = false,
            });
            #endregion
            #region ACDEnterKnown 0x77F20036
            client.SendMessage(new ACDEnterKnownMessage()
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

            client.SendMessage(new AffixMessage()
            {
                Id = 0x0048,
                Field0 = 0x77F20036,
                Field1 = 0x00000001,
                aAffixGBIDs = new int[0]
    {
    },
            });

            client.SendMessage(new AffixMessage()
            {
                Id = 0x0048,
                Field0 = 0x77F20036,
                Field1 = 0x00000002,
                aAffixGBIDs = new int[0]
    {
    },
            });

            client.SendMessage(new PortalSpecifierMessage()
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

            client.SendMessage(new ACDCollFlagsMessage()
            {
                Id = 0x00A6,
                Field0 = 0x77F20036,
                Field1 = 0x00000001,
            });

            client.SendMessage(new AttributesSetValuesMessage()
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

            client.SendMessage(new ACDGroupMessage()
            {
                Id = 0x00B8,
                Field0 = 0x77F20036,
                Field1 = -1,
                Field2 = -1,
            });

            client.SendMessage(new ANNDataMessage()
            {
                Id = 0x003E,
                Field0 = 0x77F20036,
            });

            client.SendMessage(new ACDTranslateFacingMessage()
            {
                Id = 0x0070,
                Field0 = 0x77F20036,
                Field1 = 1.528479f,
                Field2 = false,
            });
            #endregion
            #region ACDEnterKnown 0x77F30037
            client.SendMessage(new ACDEnterKnownMessage()
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

            client.SendMessage(new AffixMessage()
            {
                Id = 0x0048,
                Field0 = 0x77F30037,
                Field1 = 0x00000001,
                aAffixGBIDs = new int[0]
    {
    },
            });

            client.SendMessage(new AffixMessage()
            {
                Id = 0x0048,
                Field0 = 0x77F30037,
                Field1 = 0x00000002,
                aAffixGBIDs = new int[0]
    {
    },
            });

            client.SendMessage(new ACDCollFlagsMessage()
            {
                Id = 0x00A6,
                Field0 = 0x77F30037,
                Field1 = 0x00000000,
            });

            client.SendMessage(new AttributesSetValuesMessage()
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

            client.SendMessage(new ACDGroupMessage()
            {
                Id = 0x00B8,
                Field0 = 0x77F30037,
                Field1 = -1,
                Field2 = -1,
            });

            client.SendMessage(new ANNDataMessage()
            {
                Id = 0x003E,
                Field0 = 0x77F30037,
            });

            client.SendMessage(new ACDTranslateFacingMessage()
            {
                Id = 0x0070,
                Field0 = 0x77F30037,
                Field1 = 0f,
                Field2 = false,
            });
            #endregion
            #region ACDEnterKnown 0x77F7003B
            client.SendMessage(new ACDEnterKnownMessage()
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

            client.SendMessage(new AffixMessage()
            {
                Id = 0x0048,
                Field0 = 0x77F7003B,
                Field1 = 0x00000001,
                aAffixGBIDs = new int[0]
    {
    },
            });

            client.SendMessage(new AffixMessage()
            {
                Id = 0x0048,
                Field0 = 0x77F7003B,
                Field1 = 0x00000002,
                aAffixGBIDs = new int[0]
    {
    },
            });

            client.SendMessage(new ACDCollFlagsMessage()
            {
                Id = 0x00A6,
                Field0 = 0x77F7003B,
                Field1 = 0x00000000,
            });

            client.SendMessage(new AttributesSetValuesMessage()
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

            client.SendMessage(new ACDGroupMessage()
            {
                Id = 0x00B8,
                Field0 = 0x77F7003B,
                Field1 = -1,
                Field2 = -1,
            });

            client.SendMessage(new ANNDataMessage()
            {
                Id = 0x003E,
                Field0 = 0x77F7003B,
            });

            client.SendMessage(new ACDTranslateFacingMessage()
            {
                Id = 0x0070,
                Field0 = 0x77F7003B,
                Field1 = 0f,
                Field2 = false,
            });
            #endregion
            #region ACDEnterKnown 0x77F8003C
            client.SendMessage(new ACDEnterKnownMessage()
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

            client.SendMessage(new AffixMessage()
            {
                Id = 0x0048,
                Field0 = 0x77F8003C,
                Field1 = 0x00000001,
                aAffixGBIDs = new int[0]
    {
    },
            });

            client.SendMessage(new AffixMessage()
            {
                Id = 0x0048,
                Field0 = 0x77F8003C,
                Field1 = 0x00000002,
                aAffixGBIDs = new int[0]
    {
    },
            });

            client.SendMessage(new ACDCollFlagsMessage()
            {
                Id = 0x00A6,
                Field0 = 0x77F8003C,
                Field1 = 0x00000000,
            });

            client.SendMessage(new AttributesSetValuesMessage()
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

            client.SendMessage(new ACDGroupMessage()
            {
                Id = 0x00B8,
                Field0 = 0x77F8003C,
                Field1 = -1,
                Field2 = -1,
            });

            client.SendMessage(new ANNDataMessage()
            {
                Id = 0x003E,
                Field0 = 0x77F8003C,
            });

            client.SendMessage(new ACDTranslateFacingMessage()
            {
                Id = 0x0070,
                Field0 = 0x77F8003C,
                Field1 = 0f,
                Field2 = false,
            });
            #endregion
            #region ACDEnterKnown 0x77F9003D
            client.SendMessage(new ACDEnterKnownMessage()
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

            client.SendMessage(new AffixMessage()
            {
                Id = 0x0048,
                Field0 = 0x77F9003D,
                Field1 = 0x00000001,
                aAffixGBIDs = new int[0]
    {
    },
            });

            client.SendMessage(new AffixMessage()
            {
                Id = 0x0048,
                Field0 = 0x77F9003D,
                Field1 = 0x00000002,
                aAffixGBIDs = new int[0]
    {
    },
            });

            client.SendMessage(new ACDCollFlagsMessage()
            {
                Id = 0x00A6,
                Field0 = 0x77F9003D,
                Field1 = 0x00000000,
            });

            client.SendMessage(new AttributesSetValuesMessage()
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

            client.SendMessage(new ACDGroupMessage()
            {
                Id = 0x00B8,
                Field0 = 0x77F9003D,
                Field1 = -1,
                Field2 = -1,
            });

            client.SendMessage(new ANNDataMessage()
            {
                Id = 0x003E,
                Field0 = 0x77F9003D,
            });

            client.SendMessage(new ACDTranslateFacingMessage()
            {
                Id = 0x0070,
                Field0 = 0x77F9003D,
                Field1 = 0f,
                Field2 = false,
            });
            #endregion
            #region ACDEnterKnown 0x77FC0040
            client.SendMessage(new ACDEnterKnownMessage()
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

            client.SendMessage(new AffixMessage()
            {
                Id = 0x0048,
                Field0 = 0x77FC0040,
                Field1 = 0x00000001,
                aAffixGBIDs = new int[0]
    {
    },
            });

            client.SendMessage(new AffixMessage()
            {
                Id = 0x0048,
                Field0 = 0x77FC0040,
                Field1 = 0x00000002,
                aAffixGBIDs = new int[0]
    {
    },
            });

            client.SendMessage(new ACDCollFlagsMessage()
            {
                Id = 0x00A6,
                Field0 = 0x77FC0040,
                Field1 = 0x00000000,
            });

            client.SendMessage(new AttributesSetValuesMessage()
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

            client.SendMessage(new ACDGroupMessage()
            {
                Id = 0x00B8,
                Field0 = 0x77FC0040,
                Field1 = -1,
                Field2 = -1,
            });

            client.SendMessage(new ANNDataMessage()
            {
                Id = 0x003E,
                Field0 = 0x77FC0040,
            });

            client.SendMessage(new ACDTranslateFacingMessage()
            {
                Id = 0x0070,
                Field0 = 0x77FC0040,
                Field1 = 0f,
                Field2 = false,
            });
            #endregion
            #region ACDEnterKnown 0x77FD0041
            client.SendMessage(new ACDEnterKnownMessage()
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

            client.SendMessage(new AffixMessage()
            {
                Id = 0x0048,
                Field0 = 0x77FD0041,
                Field1 = 0x00000001,
                aAffixGBIDs = new int[0]
    {
    },
            });

            client.SendMessage(new AffixMessage()
            {
                Id = 0x0048,
                Field0 = 0x77FD0041,
                Field1 = 0x00000002,
                aAffixGBIDs = new int[0]
    {
    },
            });

            client.SendMessage(new ACDCollFlagsMessage()
            {
                Id = 0x00A6,
                Field0 = 0x77FD0041,
                Field1 = 0x00000000,
            });

            client.SendMessage(new AttributesSetValuesMessage()
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

            client.SendMessage(new ACDGroupMessage()
            {
                Id = 0x00B8,
                Field0 = 0x77FD0041,
                Field1 = -1,
                Field2 = -1,
            });

            client.SendMessage(new ANNDataMessage()
            {
                Id = 0x003E,
                Field0 = 0x77FD0041,
            });

            client.SendMessage(new ACDTranslateFacingMessage()
            {
                Id = 0x0070,
                Field0 = 0x77FD0041,
                Field1 = 0f,
                Field2 = false,
            });
            #endregion
            #region ACDEnterKnown 0x77FE0042
            client.SendMessage(new ACDEnterKnownMessage()
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

            client.SendMessage(new AffixMessage()
            {
                Id = 0x0048,
                Field0 = 0x77FE0042,
                Field1 = 0x00000001,
                aAffixGBIDs = new int[0]
    {
    },
            });

            client.SendMessage(new AffixMessage()
            {
                Id = 0x0048,
                Field0 = 0x77FE0042,
                Field1 = 0x00000002,
                aAffixGBIDs = new int[0]
    {
    },
            });

            client.SendMessage(new ACDCollFlagsMessage()
            {
                Id = 0x00A6,
                Field0 = 0x77FE0042,
                Field1 = 0x00000411,
            });

            client.SendMessage(new AttributesSetValuesMessage()
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

            client.SendMessage(new ACDGroupMessage()
            {
                Id = 0x00B8,
                Field0 = 0x77FE0042,
                Field1 = -1,
                Field2 = -1,
            });

            client.SendMessage(new ANNDataMessage()
            {
                Id = 0x003E,
                Field0 = 0x77FE0042,
            });

            client.SendMessage(new ACDTranslateFacingMessage()
            {
                Id = 0x0070,
                Field0 = 0x77FE0042,
                Field1 = 2.988367f,
                Field2 = false,
            });
            #endregion
            #region ACDEnterKnown 0x78010045
            client.SendMessage(new ACDEnterKnownMessage()
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
                        Field1 = new Vector3D() //Cart
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

            client.SendMessage(new AffixMessage()
            {
                Id = 0x0048,
                Field0 = 0x78010045,
                Field1 = 0x00000001,
                aAffixGBIDs = new int[0]
    {
    },
            });

            client.SendMessage(new AffixMessage()
            {
                Id = 0x0048,
                Field0 = 0x78010045,
                Field1 = 0x00000002,
                aAffixGBIDs = new int[0]
    {
    },
            });

            client.SendMessage(new ACDCollFlagsMessage()
            {
                Id = 0x00A6,
                Field0 = 0x78010045,
                Field1 = 0x00000C21,
            });

            client.SendMessage(new AttributesSetValuesMessage()
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

            client.SendMessage(new ACDGroupMessage()
            {
                Id = 0x00B8,
                Field0 = 0x78010045,
                Field1 = -1,
                Field2 = -1,
            });

            client.SendMessage(new ANNDataMessage()
            {
                Id = 0x003E,
                Field0 = 0x78010045,
            });

            client.SendMessage(new ACDTranslateFacingMessage()
            {
                Id = 0x0070,
                Field0 = 0x78010045,
                Field1 = 4.99892f,
                Field2 = false,
            });
            #endregion
            #endregion

            client.FlushOutgoingBuffer();

            client.SendMessage(new DWordDataMessage() // TICK
            {
                Id = 0x0089,
                Field0 = 0x00000077,
            });

            client.FlushOutgoingBuffer();

            client.SendMessage(new AttributeSetValueMessage()
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

            client.SendMessage(new DWordDataMessage() // TICK
            {
                Id = 0x0089,
                Field0 = 0x0000007D,
            });

            client.FlushOutgoingBuffer();
        }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = new EntityId();
            Field0.Parse(buffer);
            Field1 = new GameId();
            Field1.Parse(buffer);
            Field2 = buffer.ReadInt(32);
            Field3 = buffer.ReadInt64(64);
            Field4 = buffer.ReadInt(4) + (2);
            ProtocolHash = buffer.ReadInt(32);
            SNOPackHash = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            Field0.Encode(buffer);
            Field1.Encode(buffer);
            buffer.WriteInt(32, Field2);
            buffer.WriteInt64(64, Field3);
            buffer.WriteInt(4, Field4 - (2));
            buffer.WriteInt(32, ProtocolHash);
            buffer.WriteInt(32, SNOPackHash);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("JoinBNetGameMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            Field0.AsText(b, pad);
            Field1.AsText(b, pad);
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', pad); b.AppendLine("Field3: 0x" + Field3.ToString("X16"));
            b.Append(' ', pad); b.AppendLine("Field4: 0x" + Field4.ToString("X8") + " (" + Field4 + ")");
            b.Append(' ', pad); b.AppendLine("ProtocolHash: 0x" + ProtocolHash.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("SNOPackHash: 0x" + SNOPackHash.ToString("X8"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }
    }
}