using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mooege.Net.GS.Message.Definitions.Conversation;
using Mooege.Net.GS.Message.Fields;
using Mooege.Net.GS.Message;
using Mooege.Net.GS;
using Mooege.Common.MPQ.FileFormats;

namespace Mooege.Core.GS.Players
{

    class Conversation
    {
        private Mooege.Common.MPQ.FileFormats.Conversation asset;
        private int LineIndex = 0;
        private Player player;
        private ConversationManager manager;
        private int currentUniqueLineID;
        private int startTick = 0;

        private Mooege.Core.GS.Actors.Actor CurrentSpeaker
        {
            get
            {
                switch(asset.RootTreeNodes[LineIndex].Speaker1)
                {
                    case Speaker.AltNPC1: return player.World.GetActorBySNO(asset.SNOAltNpc1).First();
                    case Speaker.AltNPC2: return player.World.GetActorBySNO(asset.SNOAltNpc2).First();
                    case Speaker.AltNPC3: return player.World.GetActorBySNO(asset.SNOAltNpc3).First();
                    case Speaker.AltNPC4: return player.World.GetActorBySNO(asset.SNOAltNpc4).First();
                    case Speaker.Player: return player;
                    case Speaker.PrimaryNPC: return player.World.GetActorBySNO(asset.SNOPrimaryNpc).First();
                    case Speaker.EnchantressFollower: return null;
                    case Speaker.ScoundrelFollower: return null;
                    case Speaker.TemplarFollower: return null;
                    case Speaker.None: return null;
                }
                return null;
            }
        }
                        

        public Conversation(int snoConversation, Player player, ConversationManager manager)
        {
            asset = Mooege.Common.MPQ.MPQStorage.Data.Assets[Common.Types.SNO.SNOGroup.Conversation][snoConversation].Data as Mooege.Common.MPQ.FileFormats.Conversation;
            this.player = player;
            this.manager = manager;
        }

        public void Start()
        {
            PlayLine(LineIndex);
        }

        public void Interrupt()
        {
            StopLine(true);
        }

        public bool Update()
        {
            var node = from a in asset.RootTreeNodes[LineIndex].ChildNodes where a.I5 == player.Properties.VoiceClassID select a;
            if (node.Count() == 0)
                node = from a in asset.RootTreeNodes[LineIndex].ChildNodes where a.I5 == -1 select a;

            int duration = node.First().ConvLocalDisplayTimes.ElementAt(5).I0[player.Properties.VoiceClassID * 2 + player.Properties.Gender == 0 ? 0 : 1];



            //int duration = asset.RootTreeNodes[LineIndex].ChildNodes[0].ConvLocalDisplayTimes.ElementAt(5).I0[5];

            if (startTick + duration < player.World.Game.Tick)
            {
                StopLine(false);

                if (asset.RootTreeNodes.Count > LineIndex + 1)
                    PlayLine(++LineIndex);
                else
                {
                    EndConversation();
                    return true;
                }
            }

            return false;
        }


        private void EndConversation()
        {
            player.InGameClient.SendMessage(new EndConversationMessage()
            {
                Field0 = currentUniqueLineID,
                SNOConversation = asset.Header.SNOId,
                ActorId = player.DynamicID
            });

            player.InGameClient.SendMessage(new FinishConversationMessage(asset.Header.SNOId));
        }

        private void StopLine(bool interrupted)
        {
            player.InGameClient.SendMessage(new StopConvLineMessage()
            {
                Field0 = currentUniqueLineID,
                Interrupt = interrupted,
            });
        }


        private void PlayLine(int LineIndex)
        {
            int fooID = manager.GetNextFooID();
            currentUniqueLineID = 78; //manager.GetNextUniqueLineID();
            startTick = player.World.Game.Tick;

            player.InGameClient.SendMessage(new PlayConvLineMessage()
            {
                ActorID = CurrentSpeaker.DynamicID,
                Field1 = new uint[9]
                        {
                            player.DynamicID, asset.SNOPrimaryNpc != -1 ? player.World.GetActorBySNO(asset.SNOPrimaryNpc).First().DynamicID : 0xFFFFFFFF, 0xFFFFFFFF, 0xFFFFFFFF, 0xFFFFFFFF, 0xFFFFFFFF, 0xFFFFFFFF, 0xFFFFFFFF, 0xFFFFFFFF
                        }, // default for now since the client doesnt care - farmy

                Params = new PlayLineParams()
                {
                    SNOConversation = asset.Header.SNOId,
                    Field1 = 0x00000000, // dont know... -farmy
                    Field2 = false,      // dont know either... -farmy
                    LineID = asset.RootTreeNodes[LineIndex].LineID,
                    Field4 = (int)asset.RootTreeNodes[LineIndex].Speaker1,
                    Field5 = -1,        // dont know either... -farmy
                    TextClass = asset.RootTreeNodes[LineIndex].Speaker1 == Speaker.Player ? (Class)player.Properties.VoiceClassID : Class.None,
                    Gender = (player.Properties.Gender == 0) ? VoiceGender.Male : VoiceGender.Female,
                    AudioClass = (Class)player.Properties.VoiceClassID,
                    SNOSpeakerActor = CurrentSpeaker.SNOId,
                    Name = player.Properties.Name,
                    Field11 = 0x00000000,  // is this field I1? and if...what does it do?? 2 for level up
                    AnimationTag = asset.RootTreeNodes[LineIndex].AnimationTag,
                    Field13 = 242,
                    Field14 = currentUniqueLineID,
                    Field15 = 0x00000000        // dont know, 0x32 for level up
                },
                Field3 = 242,
            });

        }

    }


    public class ConversationManager
    {
        private Player player;
        private Dictionary<int, Conversation> openConversations = new Dictionary<int, Conversation>();
        private int linesPlayedTotal = 0;

        internal int GetNextFooID()
        {
            return linesPlayedTotal * 20 + 1;
        }

        internal int GetNextUniqueLineID()
        {
            return linesPlayedTotal++;
        }

        public ConversationManager(Player player)
        {
            this.player = player;
        }


        public void StartConversation(int snoConversation)
        {
            Conversation newConversation = new Conversation(snoConversation, player, this);
            newConversation.Start();
            openConversations.Add(snoConversation, newConversation);
        }

        public void Update(int gameTick)
        {
            List<int> removed = new List<int>();
            foreach (var conversation in openConversations)
                if (conversation.Value.Update())
                    removed.Add(conversation.Key);

            foreach (var conversation in removed)
                openConversations.Remove(conversation);
        }

        public void Consume(GameClient client, GameMessage message)
        {
            if(message is RequestCloseConversationWindowMessage)
                foreach(var conversation in openConversations.Values)
                    conversation.Interrupt();  
        }

    }
}
