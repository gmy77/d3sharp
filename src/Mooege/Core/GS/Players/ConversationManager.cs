using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mooege.Net.GS.Message.Definitions.Conversation;
using Mooege.Net.GS.Message.Fields;
using Mooege.Net.GS.Message;
using Mooege.Net.GS;
using Mooege.Common.MPQ.FileFormats;
using Mooege.Common;
using Mooege.Common.Helpers;

namespace Mooege.Core.GS.Players
{
    /// <summary>
    /// Manages a single conversation.
    /// </summary>
    class Conversation
    {
        Logger logger = new Logger("Conversation");
        public event EventHandler ConversationEnded;

        public int ConvPiggyBack { get { return asset.SNOConvPiggyback; } }
        public int SNOId { get { return asset.Header.SNOId; } }

        private Mooege.Common.MPQ.FileFormats.Conversation asset;
        private int LineIndex = 0;
        private Player player;
        private ConversationManager manager;
        private int currentUniqueLineID;
        private int startTick = 0;              // start tick of the current line. used to determine, when to start the next line
        private int duration = 0;

        // This returns the dynamicID of other conversation partners. The client uses the sno of a known actor to
        // find its sound and text ressources. It also uses its position to identify where you can hear the conversation.
        // This implementation relies on there beeing exactly one actor with a given sno in the world!!
        // TODO Find a better way to get the dynamicID of actors or verify that this implementation is sound.
        private Mooege.Core.GS.Actors.Actor GetSpeaker(ConversationTreeNode node)
        {
            switch (node.Speaker1)
            {
                case Speaker.AltNPC1: return GetActorBySNO(asset.SNOAltNpc1);
                case Speaker.AltNPC2: return GetActorBySNO(asset.SNOAltNpc2);
                case Speaker.AltNPC3: return GetActorBySNO(asset.SNOAltNpc3);
                case Speaker.AltNPC4: return GetActorBySNO(asset.SNOAltNpc4);
                case Speaker.Player: return player;
                case Speaker.PrimaryNPC: return GetActorBySNO(asset.SNOPrimaryNpc);
                case Speaker.EnchantressFollower: return null;
                case Speaker.ScoundrelFollower: return null;
                case Speaker.TemplarFollower: return null;
                case Speaker.None: return null;
            }
            return null;
        }

        private Mooege.Core.GS.Actors.Actor GetActorBySNO(int sno)
        {
            var actors = (from a in player.RevealedObjects.Values where a is Mooege.Core.GS.Actors.Actor && (a as Mooege.Core.GS.Actors.Actor).SNOId == sno select a);
            if (actors.Count() > 1)
                logger.Warn("Found more than one actors in range");
            if (actors.Count() == 0)
            {
                logger.Warn("Actor not found, using player actor instead");
                return player;
            }
            return actors.First() as Mooege.Core.GS.Actors.Actor;
        }

        /// <summary>
        /// Creates a new conversation wrapper for an asset with a given sno.
        /// </summary>
        /// <param name="snoConversation">sno of the asset to wrap</param>
        /// <param name="player">player that receives messages</param>
        /// <param name="manager">the quest manager that provides ids</param>
        public Conversation(int snoConversation, Player player, ConversationManager manager)
        {
            asset = Mooege.Common.MPQ.MPQStorage.Data.Assets[Common.Types.SNO.SNOGroup.Conversation][snoConversation].Data as Mooege.Common.MPQ.FileFormats.Conversation;
            this.player = player;
            this.manager = manager;
        }

        /// <summary>
        /// Starts the conversation
        /// </summary>
        public void Start()
        {
            PlayLine(LineIndex);
        }

        /// <summary>
        /// Skips to the next line of the conversation
        /// </summary>
        public void Interrupt()
        {
            PlayNextLine(true);
        }

        /// <summary>
        /// Periodically call this method to make sure, conversation progresses depending on game ticks
        /// </summary>
        public void Update()
        {

            // TODO rotate the actor to face the player or he may not hear the dialog

            if (startTick + duration < player.World.Game.TickCounter)
                PlayNextLine(false);
        }

        /// <summary>
        /// Stops current line and starts the next if there is one, or ends the conversation
        /// </summary>
        /// <param name="interrupt">sets, whether the speaker is interrupted</param>
        private void PlayNextLine(bool interrupt)
        {
            StopLine(interrupt);

            if (asset.RootTreeNodes.Count > LineIndex + 1)
                PlayLine(++LineIndex);
            else
                EndConversation();
        }

        /// <summary>
        /// Ends the conversation, though i dont know, what it actually does. This is only through observation
        /// </summary>
        private void EndConversation()
        {
            player.InGameClient.SendMessage(new EndConversationMessage()
            {
                Field0 = currentUniqueLineID,
                SNOConversation = asset.Header.SNOId,
                ActorId = player.DynamicID
            });

            player.InGameClient.SendMessage(new FinishConversationMessage
            {
                SNOConversation = asset.Header.SNOId
            });

            if (ConversationEnded != null)
                ConversationEnded(this, null);
        }

        /// <summary>
        /// Stops readout and display of current conversation line
        /// </summary>
        /// <param name="interrupted">sets whether the speaker is interrupted or not</param>
        private void StopLine(bool interrupted)
        {
            player.InGameClient.SendMessage(new StopConvLineMessage()
            {
                Field0 = currentUniqueLineID,
                Interrupt = interrupted,
            });
        }

        /// <summary>
        /// Starts readout and display of a certain conversation line
        /// </summary>
        /// <param name="LineIndex">index of the line withing the rootnodes</param>
        private void PlayLine(int LineIndex)
        {
            ConversationTreeNode selectedNode = null;
            if (asset.RootTreeNodes[LineIndex].I0 == 6)
            {
                duration = 0;
                return; // dont know what to do with them yet -farmy
            }

            if (asset.RootTreeNodes[LineIndex].I0 == 4)
                selectedNode = asset.RootTreeNodes[LineIndex].ChildNodes[RandomHelper.Next(asset.RootTreeNodes[LineIndex].ChildNodes.Count)];
            else
                selectedNode = asset.RootTreeNodes[LineIndex];

            // Find a childnode with a matching class id, that one holds information about how long the speaker talks
            // If there is no matching childnode, there must be one with -1 which only combines all class specific into one
            var node = from a in selectedNode.ChildNodes where a.ClassFilter == player.Properties.VoiceClassID select a;
            if (node.Count() == 0)
                node = from a in selectedNode.ChildNodes where a.ClassFilter == -1 select a;

            // get duration depending on language, class and gender
            duration = node.First().ConvLocalDisplayTimes.ElementAt((int)manager.ClientLanguage).I0[player.Properties.VoiceClassID * 2 + (player.Properties.Gender == 0 ? 0 : 1)];


            int fooID = manager.GetNextFooID();
            currentUniqueLineID = manager.GetNextUniqueLineID();
            startTick = player.World.Game.TickCounter;



            player.InGameClient.SendMessage(new PlayConvLineMessage()
            {
                ActorID = GetActorBySNO(asset.SNOPrimaryNpc).DynamicID, //CurrentSpeaker.DynamicID, // 
                Field1 = new uint[9]
                        {
                            player.DynamicID, asset.SNOPrimaryNpc != -1 ? GetActorBySNO(asset.SNOPrimaryNpc).DynamicID : 0xFFFFFFFF, 0xFFFFFFFF, 0xFFFFFFFF, 0xFFFFFFFF, 0xFFFFFFFF, 0xFFFFFFFF, 0xFFFFFFFF, 0xFFFFFFFF
                        }, // default for now since the client doesnt care - farmy

                Params = new PlayLineParams()
                {
                    SNOConversation = asset.Header.SNOId,
                    Field1 = 0x00000000,
                    Field2 = false,
                    LineID = selectedNode.LineID,
                    Field4 = (int)selectedNode.Speaker1,
                    Field5 = -1,
                    TextClass = selectedNode.Speaker1 == Speaker.Player ? (Class)player.Properties.VoiceClassID : Class.None,
                    Gender = (player.Properties.Gender == 0) ? VoiceGender.Male : VoiceGender.Female,
                    AudioClass = (Class)player.Properties.VoiceClassID,
                    SNOSpeakerActor = GetSpeaker(selectedNode).SNOId,
                    Name = player.Properties.Name,
                    Field11 = 0x00000000,  // is this field I1? and if...what does it do?? 2 for level up -farmy
                    AnimationTag = selectedNode.AnimationTag,
                    Field13 = fooID,
                    Field14 = currentUniqueLineID,
                    Field15 = 0x00000032        // dont know, 0x32 for level up
                },
                Field3 = fooID,
            }, true);
        }
    }

    /// <summary>
    /// Manages conversations. Since you can (maybe?) only have one conversation at a time, this class may be merged with the player.
    /// Still, its a bit nicer this ways, considering the player class already has 1000+ loc
    /// </summary>
    public class ConversationManager
    {
        internal enum Language { Invalid, Global, enUS, enGB, enSG, esES, esMX, frFR, itIT, deDE, koKR, ptBR, ruRU, zhCN, zTW, trTR, plPL, ptPT }

        private Player player;
        private Dictionary<int, Conversation> openConversations = new Dictionary<int, Conversation>();
        private int linesPlayedTotal = 0;
        private QuestProgressHandler quests;

        internal Language ClientLanguage { get { return Language.enUS; } }

        internal int GetNextFooID()
        {
            return linesPlayedTotal * 20 + 1;
        }

        internal int GetNextUniqueLineID()
        {
            return linesPlayedTotal++;
        }

        public ConversationManager(Player player, QuestProgressHandler quests)
        {
            this.player = player;
            this.quests = quests;
        }

        /// <summary>
        /// Starts and plays a conversation
        /// </summary>
        /// <param name="snoConversation">SnoID of the conversation</param>
        public void StartConversation(int snoConversation)
        {
            Conversation newConversation = new Conversation(snoConversation, player, this);
            newConversation.Start();
            newConversation.ConversationEnded += new EventHandler(ConversationEnded);

            lock (openConversations)
            {
                openConversations.Add(snoConversation, newConversation);
            }
        }

        /// <summary>
        /// Remove conversation from the list of open conversations and start its piggyback conversation
        /// </summary>
        void ConversationEnded(object sender, EventArgs e)
        {
            Conversation conversation = sender as Conversation;
            quests.Notify(QuestStepObjectiveType.HadConversation, conversation.SNOId);

            lock (openConversations)
            {
                openConversations.Remove(conversation.SNOId);
            }

            if (conversation.ConvPiggyBack != -1)
                StartConversation(conversation.ConvPiggyBack);
        }

        /// <summary>
        /// Update all open conversations
        /// </summary>
        /// <param name="gameTick"></param>
        public void Update(int gameTick)
        {
            List<Conversation> clonedList;

            // update from a cloned list, so you can remove conversations in their ConversationEnded event
            lock (openConversations)
            {
                clonedList = (from c in openConversations select c.Value).ToList();
            }

            foreach (var conversation in clonedList)
                conversation.Update();
        }

        /// <summary>
        /// Consumes conversations related messages
        /// </summary>
        /// <param name="client"></param>
        /// <param name="message"></param>
        public void Consume(GameClient client, GameMessage message)
        {
            lock (openConversations)
            {
                if (message is RequestCloseConversationWindowMessage)
                    foreach (var conversation in openConversations.Values)
                        conversation.Interrupt();
            }
        }

    }
}
