using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace D3Sharp.Net.Game
{
    public abstract class GameMessage
    {
        #region AllocateMessage
        static GameMessage AllocateMessage(int id)
        {
            switch (id)
            {
                case 47:
                    return new GameSetupMessage();
                case 46:
                    return new ConnectionEstablishedMessage();
                case 3:
                    return new QuitGameMessage();
                case 6:
                case 83:
                case 84:
                case 85:
                case 86:
                case 137:
                case 200:
                case 288:
                case 289:
                case 290:
                case 291:
                    return new DWordDataMessage();
                case 292:
                    return new BroadcastTextMessage();
                case 14:
                case 28:
                case 29:
                case 30:
                case 31:
                case 50:
                case 150:
                case 234:
                case 235:
                case 236:
                case 237:
                case 238:
                case 239:
                    return new GenericBlobMessage();
                case 18:
                    return new UInt64DataMessage();
                case 13:
                    return new VersionsMessage();
                case 42:
                case 43:
                case 162:
                case 164:
                case 251:
                case 285:
                    return new PlayerIndexMessage();
                case 49:
                    return new NewPlayerMessage();
                case 51:
                    return new EnterWorldMessage();
                case 55:
                    return new RevealWorldMessage();
                case 52:
                    return new RevealSceneMessage();
                case 53:
                    return new DestroySceneMessage();
                case 54:
                    return new SwapSceneMessage();
                case 56:
                    return new RevealTeamMessage();
                case 58:
                    return new HeroStateMessage();
                case 59:
                    return new ACDEnterKnownMessage();
                case 32:
                case 34:
                case 35:
                case 36:
                case 37:
                case 60:
                case 62:
                case 67:
                case 89:
                case 95:
                case 96:
                case 105:
                case 109:
                case 132:
                case 133:
                case 134:
                case 144:
                case 145:
                case 153:
                case 156:
                case 173:
                case 174:
                case 196:
                case 197:
                case 202:
                case 203:
                case 228:
                case 240:
                case 263:
                case 266:
                case 274:
                case 275:
                case 296:
                    return new ANNDataMessage();
                case 61:
                    return new PlayerEnterKnownMessage();
                case 63:
                    return new ACDWorldPositionMessage();
                case 64:
                    return new ACDInventoryPositionMessage();
                case 65:
                    return new ACDInventoryUpdateActorSNO();
                case 57:
                    return new PlayerActorSetInitialMessage();
                case 78:
                    return new VisualInventoryMessage();
                case 136:
                    return new ACDChangeGBHandleMessage();
                case 72:
                    return new AffixMessage();
                case 138:
                    return new LearnedSkillMessage();
                case 75:
                    return new PortalSpecifierMessage();
                case 73:
                    return new RareMonsterNamesMessage();
                case 74:
                    return new RareItemNameMessage();
                case 76:
                    return new AttributeSetValueMessage();
                case 79:
                    return new ProjectileStickMessage();
                case 77:
                    return new AttributesSetValuesMessage();
                case 88:
                case 231:
                    return new ChatMessage();
                case 178:
                    return new VictimMessage();
                case 179:
                    return new KillCountMessage();
                case 108:
                    return new PlayAnimationMessage();
                case 110:
                case 120:
                    return new ACDTranslateNormalMessage();
                case 111:
                    return new ACDTranslateSnappedMessage();
                case 112:
                case 121:
                    return new ACDTranslateFacingMessage();
                case 113:
                    return new ACDTranslateFixedMessage();
                case 114:
                    return new ACDTranslateArcMessage();
                case 115:
                    return new ACDTranslateDetPathMessage();
                case 116:
                    return new ACDTranslateDetPathSinMessage();
                case 117:
                    return new ACDTranslateDetPathSpiralMessage();
                case 118:
                    return new ACDTranslateSyncMessage();
                case 119:
                    return new ACDTranslateFixedUpdateMessage();
                case 166:
                    return new ACDCollFlagsMessage();
                case 167:
                    return new GoldModifiedMessage();
                case 122:
                    return new PlayEffectMessage();
                case 123:
                    return new PlayHitEffectMessage();
                case 124:
                    return new PlayHitEffectOverrideMessage();
                case 125:
                    return new PlayNonPositionalSoundMessage();
                case 126:
                    return new PlayErrorSoundMessage();
                case 127:
                    return new PlayMusicMessage();
                case 128:
                    return new PlayCutsceneMessage();
                case 130:
                    return new FlippyMessage();
                case 143:
                    return new NPCInteractOptionsMessage();
                case 155:
                    return new PetMessage();
                case 157:
                    return new HirelingInfoUpdateMessage();
                case 147:
                    return new QuestUpdateMessage();
                case 148:
                    return new QuestMeterMessage();
                case 149:
                    return new QuestCounterMessage();
                case 152:
                    return new PlayerLevel();
                case 131:
                    return new WaypointActivatedMessage();
                case 135:
                    return new AimTargetMessage();
                case 165:
                    return new SetIdleAnimationMessage();
                case 154:
                    return new ACDPickupFailedMessage();
                case 66:
                    return new TrickleMessage();
                case 68:
                    return new MapRevealSceneMessage();
                case 69:
                    return new SavePointInfoMessage();
                case 70:
                    return new HearthPortalInfoMessage();
                case 71:
                    return new ReturnPointInfoMessage();
                case 139:
                case 140:
                    return new DataIDDataMessage();
                case 151:
                    return new PlayerInteractMessage();
                case 160:
                case 161:
                    return new TradeMessage();
                case 168:
                    return new ActTransitionMessage();
                case 169:
                    return new InterstitialMessage();
                case 171:
                    return new RopeEffectMessageACDToACD();
                case 172:
                    return new RopeEffectMessageACDToPlace();
                case 158:
                    return new UIElementMessage();
                case 176:
                    return new ACDChangeActorMessage();
                case 177:
                    return new PlayerWarpedMessage();
                case 175:
                    return new GameSyncedDataMessage();
                case 141:
                    return new EndOfTickMessage();
                case 4:
                    return new CreateBNetGameMessage();
                case 5:
                    return new CreateBNetGameResultMessage();
                case 8:
                    return new RequestJoinBNetGameMessage();
                case 9:
                    return new BNetJoinGameRequestResultMessage();
                case 10:
                    return new JoinBNetGameMessage();
                case 11:
                    return new JoinLANGameMessage();
                case 15:
                    return new NetworkAddressMessage();
                case 17:
                    return new GameIdMessage();
                case 20:
                case 107:
                case 188:
                case 199:
                case 219:
                case 268:
                case 276:
                case 277:
                    return new IntDataMessage();
                case 22:
                    return new EntityIdMessage();
                case 23:
                    return new CreateHeroMessage();
                case 24:
                    return new CreateHeroResultMessage();
                case 26:
                    return new BlizzconCVarsMessage();
                case 38:
                case 41:
                    return new LogoutContextMessage();
                case 39:
                    return new LogoutTickTimeMessage();
                case 80:
                    return new TargetMessage();
                case 81:
                    return new SecondaryAnimationPowerMessage();
                case 82:
                case 191:
                case 192:
                case 221:
                case 282:
                case 295:
                case 300:
                    return new SNODataMessage();
                case 1:
                case 2:
                    return new TryConsoleCommand();
                case 87:
                    return new TryChatMessage();
                case 142:
                    return new TryWaypointMessage();
                case 90:
                case 92:
                    return new InventoryRequestMoveMessage();
                case 93:
                    return new InventorySplitStackMessage();
                case 94:
                    return new InventoryStackTransferMessage();
                case 91:
                    return new InventoryRequestSocketMessage();
                case 97:
                    return new InventoryRequestUseMessage();
                case 98:
                    return new SocketSpellMessage();
                case 99:
                    return new HelperDetachMessage();
                case 100:
                case 101:
                case 102:
                case 103:
                    return new AssignSkillMessage();
                case 104:
                    return new HirelingRequestLearnSkillMessage();
                case 106:
                    return new PlayerChangeHotbarButtonMessage();
                case 180:
                    return new WorldStatusMessage();
                case 181:
                    return new WeatherOverrideMessage();
                case 129:
                    return new ComplexEffectAddMessage();
                case 170:
                    return new EffectGroupACDToACDMessage();
                case 183:
                    return new ACDShearMessage();
                case 184:
                    return new ACDGroupMessage();
                case 186:
                    return new PlayConvLineMessage();
                case 187:
                    return new StopConvLineMessage();
                case 190:
                    return new EndConversationMessage();
                case 193:
                    return new HirelingSwapMessage();
                case 195:
                    return new DeathFadeTimeMessage();
                case 198:
                    return new DisplayGameTextMessage();
                case 201:
                case 270:
                    return new GBIDDataMessage();
                case 204:
                    return new ACDLookAtMessage();
                case 205:
                    return new KillCounterUpdateMessage();
                case 206:
                    return new LowHealthCombatMessage();
                case 207:
                    return new SaviorMessage();
                case 208:
                    return new FloatingNumberMessage();
                case 209:
                    return new FloatingAmountMessage();
                case 210:
                    return new RemoveRagdollMessage();
                case 211:
                    return new SNONameDataMessage();
                case 212:
                case 213:
                    return new LoreMessage();
                case 217:
                    return new WorldDeletedMessage();
                case 220:
                    return new TimedEventStartedMessage();
                case 222:
                    return new ActTransitionStartedMessage();
                case 225:
                case 226:
                    return new PlayerQuestMessage();
                case 227:
                    return new PlayerDeSyncSnapMessage();
                case 229:
                    return new SalvageResultsMessage();
                case 233:
                    return new MapMarkerInfoMessage();
                case 241:
                    return new DebugActorTooltipMessage();
                case 242:
                case 245:
                    return new BossEncounterMessage();
                case 248:
                    return new EncounterInviteStateMessage();
                case 159:
                case 260:
                    return new BoolDataMessage();
                case 256:
                    return new CameraFocusMessage();
                case 257:
                    return new CameraZoomMessage();
                case 258:
                    return new CameraYawMessage();
                case 261:
                    return new BossZoomMessage();
                case 262:
                    return new EnchantItemMessage();
                case 271:
                    return new CraftingResultsMessage();
                case 269:
                    return new DebugDrawPrimMessage();
                case 272:
                    return new CrafterLevelUpMessage();
                case 280:
                    return new GameTestingSamplingStartMessage();
                case 283:
                    return new RequestBuffCancelMessage();
                case 25:
                case 27:
                case 33:
                case 40:
                case 44:
                case 45:
                case 48:
                case 146:
                case 163:
                case 182:
                case 185:
                case 189:
                case 194:
                case 216:
                case 218:
                case 223:
                case 224:
                case 230:
                case 232:
                case 243:
                case 244:
                case 246:
                case 247:
                case 249:
                case 250:
                case 252:
                case 253:
                case 254:
                case 255:
                case 259:
                case 264:
                case 265:
                case 267:
                case 273:
                case 278:
                case 279:
                case 281:
                case 284:
                case 286:
                case 287:
                case 293:
                case 294:
                case 297:
                case 298:
                case 299:
                case 301:
                    return new SimpleMessage();
                default:
                    throw new Exception("Unknown game message id: " + id);
            }
        }
        #endregion


        public static GameMessage ParseMessage(GameBitBuffer buffer)
        {
            int id = buffer.ReadInt(9);
            var msg = AllocateMessage(id);
            msg.Id = id;
            msg.Parse(buffer);
            return msg;
        }

        public int Id;

        public abstract void VisitHandler(IGameMessageHandler handler);
        public abstract void Parse(GameBitBuffer buffer);
        public abstract void Encode(GameBitBuffer buffer);
        public abstract void AsText(StringBuilder b, int pad);
        public string AsText()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("GameMessage(0x" + Id.ToString("X4") + ")");
            AsText(builder, 0);
            return builder.ToString();
        }

        public const int ImplementedProtocolHash = 0x21EEE08D;
    }

    public interface IGameMessageHandler
    {
        void OnMessage(GameSetupMessage msg);
        void OnMessage(ConnectionEstablishedMessage msg);
        void OnMessage(QuitGameMessage msg);
        void OnMessage(DWordDataMessage msg);
        void OnMessage(BroadcastTextMessage msg);
        void OnMessage(GenericBlobMessage msg);
        void OnMessage(UInt64DataMessage msg);
        void OnMessage(VersionsMessage msg);
        void OnMessage(PlayerIndexMessage msg);
        void OnMessage(NewPlayerMessage msg);
        void OnMessage(EnterWorldMessage msg);
        void OnMessage(RevealWorldMessage msg);
        void OnMessage(RevealSceneMessage msg);
        void OnMessage(DestroySceneMessage msg);
        void OnMessage(SwapSceneMessage msg);
        void OnMessage(RevealTeamMessage msg);
        void OnMessage(HeroStateMessage msg);
        void OnMessage(ACDEnterKnownMessage msg);
        void OnMessage(ANNDataMessage msg);
        void OnMessage(PlayerEnterKnownMessage msg);
        void OnMessage(ACDWorldPositionMessage msg);
        void OnMessage(ACDInventoryPositionMessage msg);
        void OnMessage(ACDInventoryUpdateActorSNO msg);
        void OnMessage(PlayerActorSetInitialMessage msg);
        void OnMessage(VisualInventoryMessage msg);
        void OnMessage(ACDChangeGBHandleMessage msg);
        void OnMessage(AffixMessage msg);
        void OnMessage(LearnedSkillMessage msg);
        void OnMessage(PortalSpecifierMessage msg);
        void OnMessage(RareMonsterNamesMessage msg);
        void OnMessage(RareItemNameMessage msg);
        void OnMessage(AttributeSetValueMessage msg);
        void OnMessage(ProjectileStickMessage msg);
        void OnMessage(AttributesSetValuesMessage msg);
        void OnMessage(ChatMessage msg);
        void OnMessage(VictimMessage msg);
        void OnMessage(KillCountMessage msg);
        void OnMessage(PlayAnimationMessage msg);
        void OnMessage(ACDTranslateNormalMessage msg);
        void OnMessage(ACDTranslateSnappedMessage msg);
        void OnMessage(ACDTranslateFacingMessage msg);
        void OnMessage(ACDTranslateFixedMessage msg);
        void OnMessage(ACDTranslateArcMessage msg);
        void OnMessage(ACDTranslateDetPathMessage msg);
        void OnMessage(ACDTranslateDetPathSinMessage msg);
        void OnMessage(ACDTranslateDetPathSpiralMessage msg);
        void OnMessage(ACDTranslateSyncMessage msg);
        void OnMessage(ACDTranslateFixedUpdateMessage msg);
        void OnMessage(ACDCollFlagsMessage msg);
        void OnMessage(GoldModifiedMessage msg);
        void OnMessage(PlayEffectMessage msg);
        void OnMessage(PlayHitEffectMessage msg);
        void OnMessage(PlayHitEffectOverrideMessage msg);
        void OnMessage(PlayNonPositionalSoundMessage msg);
        void OnMessage(PlayErrorSoundMessage msg);
        void OnMessage(PlayMusicMessage msg);
        void OnMessage(PlayCutsceneMessage msg);
        void OnMessage(FlippyMessage msg);
        void OnMessage(NPCInteractOptionsMessage msg);
        void OnMessage(PetMessage msg);
        void OnMessage(HirelingInfoUpdateMessage msg);
        void OnMessage(QuestUpdateMessage msg);
        void OnMessage(QuestMeterMessage msg);
        void OnMessage(QuestCounterMessage msg);
        void OnMessage(PlayerLevel msg);
        void OnMessage(WaypointActivatedMessage msg);
        void OnMessage(AimTargetMessage msg);
        void OnMessage(SetIdleAnimationMessage msg);
        void OnMessage(ACDPickupFailedMessage msg);
        void OnMessage(TrickleMessage msg);
        void OnMessage(MapRevealSceneMessage msg);
        void OnMessage(SavePointInfoMessage msg);
        void OnMessage(HearthPortalInfoMessage msg);
        void OnMessage(ReturnPointInfoMessage msg);
        void OnMessage(DataIDDataMessage msg);
        void OnMessage(PlayerInteractMessage msg);
        void OnMessage(TradeMessage msg);
        void OnMessage(ActTransitionMessage msg);
        void OnMessage(InterstitialMessage msg);
        void OnMessage(RopeEffectMessageACDToACD msg);
        void OnMessage(RopeEffectMessageACDToPlace msg);
        void OnMessage(UIElementMessage msg);
        void OnMessage(ACDChangeActorMessage msg);
        void OnMessage(PlayerWarpedMessage msg);
        void OnMessage(GameSyncedDataMessage msg);
        void OnMessage(EndOfTickMessage msg);
        void OnMessage(CreateBNetGameMessage msg);
        void OnMessage(CreateBNetGameResultMessage msg);
        void OnMessage(RequestJoinBNetGameMessage msg);
        void OnMessage(BNetJoinGameRequestResultMessage msg);
        void OnMessage(JoinBNetGameMessage msg);
        void OnMessage(JoinLANGameMessage msg);
        void OnMessage(NetworkAddressMessage msg);
        void OnMessage(GameIdMessage msg);
        void OnMessage(IntDataMessage msg);
        void OnMessage(EntityIdMessage msg);
        void OnMessage(CreateHeroMessage msg);
        void OnMessage(CreateHeroResultMessage msg);
        void OnMessage(BlizzconCVarsMessage msg);
        void OnMessage(LogoutContextMessage msg);
        void OnMessage(LogoutTickTimeMessage msg);
        void OnMessage(TargetMessage msg);
        void OnMessage(SecondaryAnimationPowerMessage msg);
        void OnMessage(SNODataMessage msg);
        void OnMessage(TryConsoleCommand msg);
        void OnMessage(TryChatMessage msg);
        void OnMessage(TryWaypointMessage msg);
        void OnMessage(InventoryRequestMoveMessage msg);
        void OnMessage(InventorySplitStackMessage msg);
        void OnMessage(InventoryStackTransferMessage msg);
        void OnMessage(InventoryRequestSocketMessage msg);
        void OnMessage(InventoryRequestUseMessage msg);
        void OnMessage(SocketSpellMessage msg);
        void OnMessage(HelperDetachMessage msg);
        void OnMessage(AssignSkillMessage msg);
        void OnMessage(HirelingRequestLearnSkillMessage msg);
        void OnMessage(PlayerChangeHotbarButtonMessage msg);
        void OnMessage(WorldStatusMessage msg);
        void OnMessage(WeatherOverrideMessage msg);
        void OnMessage(ComplexEffectAddMessage msg);
        void OnMessage(EffectGroupACDToACDMessage msg);
        void OnMessage(ACDShearMessage msg);
        void OnMessage(ACDGroupMessage msg);
        void OnMessage(PlayConvLineMessage msg);
        void OnMessage(StopConvLineMessage msg);
        void OnMessage(EndConversationMessage msg);
        void OnMessage(HirelingSwapMessage msg);
        void OnMessage(DeathFadeTimeMessage msg);
        void OnMessage(DisplayGameTextMessage msg);
        void OnMessage(GBIDDataMessage msg);
        void OnMessage(ACDLookAtMessage msg);
        void OnMessage(KillCounterUpdateMessage msg);
        void OnMessage(LowHealthCombatMessage msg);
        void OnMessage(SaviorMessage msg);
        void OnMessage(FloatingNumberMessage msg);
        void OnMessage(FloatingAmountMessage msg);
        void OnMessage(RemoveRagdollMessage msg);
        void OnMessage(SNONameDataMessage msg);
        void OnMessage(LoreMessage msg);
        void OnMessage(WorldDeletedMessage msg);
        void OnMessage(TimedEventStartedMessage msg);
        void OnMessage(ActTransitionStartedMessage msg);
        void OnMessage(PlayerQuestMessage msg);
        void OnMessage(PlayerDeSyncSnapMessage msg);
        void OnMessage(SalvageResultsMessage msg);
        void OnMessage(MapMarkerInfoMessage msg);
        void OnMessage(DebugActorTooltipMessage msg);
        void OnMessage(BossEncounterMessage msg);
        void OnMessage(EncounterInviteStateMessage msg);
        void OnMessage(BoolDataMessage msg);
        void OnMessage(CameraFocusMessage msg);
        void OnMessage(CameraZoomMessage msg);
        void OnMessage(CameraYawMessage msg);
        void OnMessage(BossZoomMessage msg);
        void OnMessage(EnchantItemMessage msg);
        void OnMessage(CraftingResultsMessage msg);
        void OnMessage(DebugDrawPrimMessage msg);
        void OnMessage(CrafterLevelUpMessage msg);
        void OnMessage(GameTestingSamplingStartMessage msg);
        void OnMessage(RequestBuffCancelMessage msg);
        void OnMessage(SimpleMessage msg);
    }

}
