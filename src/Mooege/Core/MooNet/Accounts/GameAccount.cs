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

using System;
using System.Collections.Generic;
using System.Linq;
using D3.Account;
using D3.Client;
using D3.OnlineService;
using D3.PartyMessage;
using D3.Profile;
using Google.ProtocolBuffers;
using Mooege.Common.Extensions;
using Mooege.Common.Storage.AccountDataBase.Entities;
using Mooege.Core.MooNet.Channels;
using Mooege.Core.MooNet.Helpers;
using Mooege.Core.MooNet.Objects;
using Mooege.Core.MooNet.Toons;
using Mooege.Net.MooNet;
using bnet.protocol.achievements;
using bnet.protocol.attribute;
using bnet.protocol.presence;

namespace Mooege.Core.MooNet.Accounts
{
    public class GameAccount : PersistentRPCObject
    {
        public Account Owner
        {
            get { return AccountManager.GetAccountByDBAccount(DBGameAccount.DBAccount); }
            set { DBGameAccount.DBAccount = value.DBAccount; }
        }

        public DBGameAccount DBGameAccount { get; private set; }

        public EntityId D3GameAccountId
        {
            get
            {
                return EntityId.CreateBuilder().SetIdHigh(BnetEntityId.High).SetIdLow(PersistentID).Build();
            }
        }

        public ByteStringPresenceField<BannerConfiguration> BannerConfigurationField
        {
            get
            {
                return new ByteStringPresenceField<BannerConfiguration>(FieldKeyHelper.Program.D3,
                                                                        FieldKeyHelper.OriginatingClass.GameAccount, 1,
                                                                        0, BannerConfiguration);
            }
        }


        public ByteStringPresenceField<EntityId> LastPlayedHeroIdField
        {
            get
            {
                var val = new ByteStringPresenceField<EntityId>(FieldKeyHelper.Program.D3,
                                                                FieldKeyHelper.OriginatingClass.GameAccount, 2, 0)
                              {
                                  Value = LastPlayedHeroId
                              };
                return val;
            }
        }


        public EntityId LastPlayedHeroId
        {
            get
            {
                if (this.CurrentToon == null)
                    return Toons.Count > 0 ? Toons.First().D3EntityID : AccountHasNoToons;
                return this.CurrentToon.D3EntityID;
            }
            set
            {
                this.CurrentToon = ToonManager.GetToonByLowID(value.IdLow);
            }
        }


        public IntPresenceField JoinPermissionField
            = new IntPresenceField(FieldKeyHelper.Program.D3, FieldKeyHelper.OriginatingClass.Party, 2, 0);

        public FourCCPresenceField ProgramField
            = new FourCCPresenceField(FieldKeyHelper.Program.BNet, FieldKeyHelper.OriginatingClass.GameAccount, 3, 0);

        public StringPresenceField BattleTagField
        {
            get
            {
                return new StringPresenceField(FieldKeyHelper.Program.BNet, FieldKeyHelper.OriginatingClass.GameAccount, 5, 0, Owner.BattleTag);
            }
        }


        public StringPresenceField GameAccountNameField
        {
            get
            {
                return new StringPresenceField(FieldKeyHelper.Program.BNet, FieldKeyHelper.OriginatingClass.GameAccount, 6, 0, Owner.BnetEntityId.Low.ToString() + "#1");
            }
        }

        public ByteStringPresenceField<bnet.protocol.EntityId> OwnerIdField
        {
            get
            {
                var val = new ByteStringPresenceField<bnet.protocol.EntityId>(FieldKeyHelper.Program.BNet,
                                                                  FieldKeyHelper.OriginatingClass.GameAccount, 7, 0);
                val.Value = this.Owner.BnetEntityId;
                return val;
            }
        }


        public BoolPresenceField GameAccountStatusField
            = new BoolPresenceField(FieldKeyHelper.Program.BNet, FieldKeyHelper.OriginatingClass.GameAccount, 1, 0,
                                    false);

        public IntPresenceField LastOnlineField
        {
            get
            {
                return new IntPresenceField(FieldKeyHelper.Program.BNet, FieldKeyHelper.OriginatingClass.GameAccount, 4,
                                            0, DBGameAccount.LastOnline);
            }
        }


        public FieldKeyHelper.Program Program;


        public BannerConfiguration BannerConfiguration
        {
            get
            {
                if (DBGameAccount.Banner == null || DBGameAccount.Banner.Length < 1)
                {
                    DBGameAccount.Banner = BannerConfiguration.CreateBuilder()
                        .SetBannerShape(189701627)
                        .SetSigilMain(1494901005)
                        .SetSigilAccent(3399297034)
                        .SetPatternColor(1797588777)
                        .SetBackgroundColor(1797588777)
                        .SetSigilColor(2045456409)
                        .SetSigilPlacement(1015980604)
                        .SetPattern(4173846786)
                        .SetUseSigilVariant(true)
                        .Build().ToByteArray();
                }

                return BannerConfiguration.ParseFrom(DBGameAccount.Banner);
            }
            set
            {
                DBGameAccount.Banner = value.ToByteArray();
                ChangedFields.SetPresenceFieldValue(BannerConfigurationField);
            }
        }

        private ScreenStatus _screenstatus = ScreenStatus.CreateBuilder().SetScreen(0).SetStatus(0).Build();

        public ScreenStatus ScreenStatus
        {
            get { return _screenstatus; }
            set
            {
                _screenstatus = value;
                JoinPermissionField.Value = value.Status;
                ChangedFields.SetPresenceFieldValue(JoinPermissionField);
            }
        }

        /// <summary>
        /// Selected toon for current account.
        /// </summary>

        public Toon CurrentToon
        {
            get
            {
                if (this.DBGameAccount.LastPlayedHero == null) return null;
                return ToonManager.GetToonByDBToon(this.DBGameAccount.LastPlayedHero);

            }
            set
            {

                this.DBGameAccount.LastPlayedHero = value.DBToon;
                ChangedFields.SetPresenceFieldValue(LastPlayedHeroIdField);
                ChangedFields.SetPresenceFieldValue(value.HeroClassField);
                ChangedFields.SetPresenceFieldValue(value.HeroLevelField);
                ChangedFields.SetPresenceFieldValue(value.HeroVisualEquipmentField);
                ChangedFields.SetPresenceFieldValue(value.HeroFlagsField);
                ChangedFields.SetPresenceFieldValue(value.HeroNameField);
            }
        }

        private GameAccountSettings _settings = GameAccountSettings.CreateBuilder().Build();

        public GameAccountSettings Settings
        {
            get { return _settings; }
            set { _settings = value; }
        }

        /// <summary>
        /// Away status
        /// </summary>
        public AwayStatusFlag AwayStatus { get; private set; }


        public List<AchievementUpdateRecord> Achievements { get; set; }
        public List<CriteriaUpdateRecord> AchievementCriteria { get; set; }

        public AccountProfile Profile
        {
            get
            {
                return AccountProfile.CreateBuilder()
                    .Build();
            }
        }

        public static readonly EntityId AccountHasNoToons =
            EntityId.CreateBuilder().SetIdHigh(0).SetIdLow(0).Build();

        public List<Toon> Toons
        {
            get { return ToonManager.GetToonsForGameAccount(this); }
        }



        public GameAccount(DBGameAccount dbGameAccount)
            : base(dbGameAccount.Id)
        {
            DBGameAccount = dbGameAccount;
            DBGameAccount.LastOnline = (long)DateTime.Now.ToExtendedEpoch();
            SetField();
        }


        /// <summary>
        /// Existing GameAccount
        /// </summary>
        /// <param name="persistentId"></param>
        /// <param name="accountId"></param>
        /*public GameAccount(ulong persistentId, ulong accountId)
            : base(persistentId)
        {
            SetField(AccountManager.GetAccountByPersistentID(accountId));
        }*/

        /*
        /// <summary>
        /// New GameAccount
        /// </summary>
        /// <param name="account"></param>
        public GameAccount(Account account)
            : base(account.BnetEntityId.Low)
        {
            this.SetField(account);

            this.BannerConfiguration =
                D3.Account.BannerConfiguration.CreateBuilder()
                .SetBannerShape(189701627)
                .SetSigilMain(1494901005)
                .SetSigilAccent(3399297034)
                .SetPatternColor(1797588777)
                .SetBackgroundColor(1797588777)
                .SetSigilColor(2045456409)
                .SetSigilPlacement(1015980604)
                .SetPattern(4173846786)
                .SetUseSigilVariant(true)
                .Build();
        }
        */

        private void SetField()
        {
            const ulong bnetGameAccountHigh = ((ulong)EntityIdHelper.HighIdType.GameAccountId) + (0x0100004433);
            BnetEntityId = bnet.protocol.EntityId.CreateBuilder().SetHigh(bnetGameAccountHigh).SetLow(PersistentID).Build();

            //TODO: Now hardcode all game account notifications to D3
            ProgramField.Value = "D3";
            Achievements = new List<AchievementUpdateRecord>();
            AchievementCriteria = new List<CriteriaUpdateRecord>();
        }

        public bool IsOnline
        {
            get { return LoggedInClient != null; }
        }

        private MooNetClient _loggedInClient;

        public MooNetClient LoggedInClient
        {
            get { return _loggedInClient; }
            set
            {
                _loggedInClient = value;

                GameAccountStatusField.Value = IsOnline;
                DBGameAccount.LastOnline = (long)DateTime.Now.ToExtendedEpoch();
                Owner.DBAccount.LastOnline = LastOnlineField.Value;

                ChangedFields.SetPresenceFieldValue(GameAccountStatusField);
                ChangedFields.SetPresenceFieldValue(LastOnlineField);
                ChangedFields.SetPresenceFieldValue(BannerConfigurationField);

                //TODO: Remove this set once delegate for set is added to presence field
                //this.Owner.AccountOnlineField.Value = this.Owner.IsOnline;
                //var operation = this.Owner.AccountOnlineField.GetFieldOperation();
                NotifyUpdate();
                //  this.UpdateSubscribers(this.Subscribers, new List<bnet.protocol.presence.FieldOperation>() { operation });
            }
        }

        public Digest Digest
        {
            get
            {
                Digest.Builder builder = Digest.CreateBuilder().SetVersion(107)
                    // 7447=>99, 7728=> 100, 8801=>102, 8296=>105, 8610=>106, 8815=>106, 8896=>106, 9183=>107
                    .SetBannerConfiguration(this.BannerConfiguration)
                    .SetFlags(2) //Enable Hardcore
                    .SetLastPlayedHeroId(LastPlayedHeroId);

                return builder.Build();
            }
        }

        #region Notifications

        public override void NotifyUpdate()
        {
            var operations = ChangedFields.GetChangedFieldList();
            ChangedFields.ClearChanged();
            base.UpdateSubscribers(Subscribers, operations);
        }

        public override List<FieldOperation> GetSubscriptionNotifications()
        {
            //for now set it here
            GameAccountStatusField.Value = IsOnline;

            var operationList = new List<FieldOperation>();

            //gameaccount
            //D3,GameAccount,1,0 -> D3.DBAccount.BannerConfiguration
            //D3,GameAccount,2,0 -> ToonId
            //D3,Hero,1,0 -> Hero Class
            //D3,Hero,2,0 -> Hero's current level
            //D3,Hero,3,0 -> D3.Hero.VisualEquipment
            //D3,Hero,4,0 -> Hero's flags
            //D3,Hero,5,0 -> Hero Name
            //D3,Hero,6,0 -> HighestUnlockedAct
            //D3,Hero,7,0 -> HighestUnlockedDifficulty
            //Bnet,GameAccount,1,0 -> GameAccount Online
            //Bnet,GameAccount,3,0 -> FourCC = "D3"
            //Bnet,GameAccount,4,0 -> Unk Int (0 if GameAccount is Offline)
            //Bnet,GameAccount,5,0 -> BattleTag
            //Bnet,GameAccount,6,0 -> DBAccount.Low + "#1"
            //Bnet,GameAccount,7,0 -> DBAccount.EntityId

            operationList.Add(BannerConfigurationField.GetFieldOperation());
            if (LastPlayedHeroId != AccountHasNoToons)
            {
                operationList.Add(LastPlayedHeroIdField.GetFieldOperation());
                if (CurrentToon != null)
                    operationList.AddRange(CurrentToon.GetSubscriptionNotifications());
            }

            operationList.Add(GameAccountStatusField.GetFieldOperation());
            operationList.Add(ProgramField.GetFieldOperation());
            operationList.Add(LastOnlineField.GetFieldOperation());
            operationList.Add(BattleTagField.GetFieldOperation());
            operationList.Add(GameAccountNameField.GetFieldOperation());
            operationList.Add(OwnerIdField.GetFieldOperation());

            return operationList;
        }

        #endregion

        public void Update(FieldOperation operation)
        {
            switch (operation.Operation)
            {
                case FieldOperation.Types.OperationType.SET:
                    DoSet(operation.Field);
                    break;
                case FieldOperation.Types.OperationType.CLEAR:
                    DoClear(operation.Field);
                    break;
                default:
                    Logger.Warn("No operation type.");
                    break;
            }
        }

        private void DoSet(Field field)
        {
            FieldOperation.Builder operation = FieldOperation.CreateBuilder();

            Field.Builder returnField = Field.CreateBuilder().SetKey(field.Key);

            switch ((FieldKeyHelper.Program)field.Key.Program)
            {
                case FieldKeyHelper.Program.D3:
                    if (field.Key.Group == 2 && field.Key.Field == 3) //CurrentActivity
                    {
                        returnField.SetValue(field.Value);
                        Logger.Trace("{0} set CurrentActivity to {1}", this, field.Value.IntValue);
                    }
                    else if (field.Key.Group == 2 && field.Key.Field == 4) //Unknown bool
                    {
                        returnField.SetValue(field.Value);
                        Logger.Trace("{0} set CurrentActivity to {1}", this, field.Value.BoolValue);
                    }
                    else if (field.Key.Group == 4 && field.Key.Field == 1) //PartyId
                    {
                        if (field.Value.HasMessageValue) //7727 Sends empty SET instead of a CLEAR -Egris
                        {
                            EntityId entityId = EntityId.ParseFrom(field.Value.MessageValue);
                            Channel channel = ChannelManager.GetChannelByEntityId(entityId);
                            if (LoggedInClient.CurrentChannel != channel)
                            {
                                LoggedInClient.CurrentChannel = channel;
                                returnField.SetValue(
                                    Variant.CreateBuilder().SetMessageValue(channel.BnetEntityId.ToByteString()).Build());
                                Logger.Trace("{0} set channel to {1}", this, channel);
                            }
                        }
                        else
                        {
                            if (LoggedInClient.CurrentChannel != null)
                            {
                                returnField.SetValue(Variant.CreateBuilder().SetMessageValue(ByteString.Empty).Build());
                                Logger.Warn("Emtpy-field: {0}, {1}, {2}", field.Key.Program, field.Key.Group,
                                            field.Key.Field);
                            }
                        }
                    }
                    else if (field.Key.Group == 4 && field.Key.Field == 2) //JoinPermission
                    {
                        //catch to stop Logger.Warn spam on client start and exit
                        // should D3.4.2 int64 Current screen (0=in-menus, 1=in-menus, 3=in-menus); see ScreenStatus sent to ChannelService.UpdateChannelState call /raist
                        if (ScreenStatus.Screen != field.Value.IntValue)
                        {
                            ScreenStatus =
                                ScreenStatus.CreateBuilder().SetScreen((int)field.Value.IntValue).SetStatus(0).Build();
                            returnField.SetValue(Variant.CreateBuilder().SetIntValue(field.Value.IntValue).Build());
                            Logger.Trace("{0} set current screen to {1}.", this, field.Value.IntValue);
                        }
                    }
                    else if (field.Key.Group == 4 && field.Key.Field == 3) //CallToArmsMessage
                    {
                        returnField.SetValue(field.Value);
                    }
                    else if (field.Key.Group == 4 && field.Key.Field == 4) //Party IsFull
                    {
                        returnField.SetValue(field.Value);
                    }
                    else if (field.Key.Group == 5 && field.Key.Field == 5) //Game IsPrivate
                    {
                        returnField.SetValue(field.Value);
                    }
                    else
                    {
                        Logger.Warn("GameAccount: Unknown set-field: {0}, {1}, {2} := {3}", field.Key.Program,
                                    field.Key.Group, field.Key.Field, field.Value);
                    }
                    break;
                case FieldKeyHelper.Program.BNet:
                    if (field.Key.Group == 2 && field.Key.Field == 2) // SocialStatus
                    {
                        AwayStatus = (AwayStatusFlag)field.Value.IntValue;
                        returnField.SetValue(Variant.CreateBuilder().SetIntValue((long)AwayStatus).Build());
                        Logger.Trace("{0} set AwayStatus to {1}.", this, AwayStatus);
                    }
                    else if (field.Key.Group == 2 && field.Key.Field == 8)
                    {
                        returnField.SetValue((field.Value));
                    }
                    else if (field.Key.Group == 2 && field.Key.Field == 10) // AFK
                    {
                        returnField.SetValue(field.Value);
                        Logger.Trace("{0} set AFK to {1}.", this, field.Value.BoolValue);
                    }
                    else
                    {
                        Logger.Warn("GameAccount: Unknown set-field: {0}, {1}, {2} := {3}", field.Key.Program,
                                    field.Key.Group, field.Key.Field, field.Value);
                    }
                    break;
            }

            //We only update subscribers on fields that actually change values.
            if (returnField.HasValue)
            {
                operation.SetField(returnField);
                UpdateSubscribers(Subscribers, new List<FieldOperation> { operation.Build() });
            }
        }

        private void DoClear(Field field)
        {
            switch ((FieldKeyHelper.Program)field.Key.Program)
            {
                case FieldKeyHelper.Program.D3:
                    Logger.Warn("GameAccount: Unknown clear-field: {0}, {1}, {2}", field.Key.Program, field.Key.Group,
                                field.Key.Field);
                    break;
                case FieldKeyHelper.Program.BNet:
                    Logger.Warn("GameAccount: Unknown clear-field: {0}, {1}, {2}", field.Key.Program, field.Key.Group,
                                field.Key.Field);
                    break;
            }
        }

        public Field QueryField(FieldKey queryKey)
        {
            Field.Builder field = Field.CreateBuilder().SetKey(queryKey);

            switch ((FieldKeyHelper.Program)queryKey.Program)
            {
                case FieldKeyHelper.Program.D3:
                    if (queryKey.Group == 2 && queryKey.Field == 1) // Banner configuration
                    {
                        field.SetValue(
                            Variant.CreateBuilder().SetMessageValue(BannerConfigurationField.Value.ToByteString()).Build
                                ());
                    }
                    else if (queryKey.Group == 2 && queryKey.Field == 2) //Hero's EntityId
                    {
                        field.SetValue(Variant.CreateBuilder().SetMessageValue(LastPlayedHeroId.ToByteString()).Build());
                    }
                    else if (queryKey.Group == 2 && queryKey.Field == 4) //Unknown Bool
                    {
                        field.SetValue(Variant.CreateBuilder().SetBoolValue(false).Build());
                    }
                    else if (queryKey.Group == 3 && queryKey.Field == 1) // Hero's class (GbidClass)
                    {
                        field.SetValue(Variant.CreateBuilder().SetIntValue(CurrentToon.ClassID).Build());
                    }
                    else if (queryKey.Group == 3 && queryKey.Field == 2) // Hero's current level
                    {
                        field.SetValue(Variant.CreateBuilder().SetIntValue(CurrentToon.Level).Build());
                    }
                    else if (queryKey.Group == 3 && queryKey.Field == 3) // Hero's visible equipment
                    {
                        field.SetValue(
                            Variant.CreateBuilder().SetMessageValue(
                                CurrentToon.HeroVisualEquipmentField.Value.ToByteString()).Build());
                    }
                    else if (queryKey.Group == 3 && queryKey.Field == 4) // Hero's flags (gender and such)
                    {
                        field.SetValue(
                            Variant.CreateBuilder().SetIntValue((uint)(CurrentToon.Flags | ToonFlags.AllUnknowns)).
                                Build());
                    }
                    else if (queryKey.Group == 3 && queryKey.Field == 5) // Toon name
                    {
                        field.SetValue(Variant.CreateBuilder().SetStringValue(CurrentToon.Name).Build());
                    }
                    else if (queryKey.Group == 3 && queryKey.Field == 6)
                    {
                        field.SetValue(Variant.CreateBuilder().SetIntValue(0).Build());
                    }
                    else if (queryKey.Group == 3 && queryKey.Field == 7)
                    {
                        field.SetValue(Variant.CreateBuilder().SetIntValue(0).Build());
                    }
                    else if (queryKey.Group == 4 && queryKey.Field == 1) // Channel ID if the client is online
                    {
                        if (LoggedInClient != null && LoggedInClient.CurrentChannel != null)
                            field.SetValue(
                                Variant.CreateBuilder().SetMessageValue(
                                    LoggedInClient.CurrentChannel.D3EntityId.ToByteString()).Build());
                        else field.SetValue(Variant.CreateBuilder().Build());
                    }
                    else if (queryKey.Group == 4 && queryKey.Field == 2)
                    // Current screen (all known values are just "in-menu"; also see ScreenStatuses sent in ChannelService.UpdateChannelState)
                    {
                        field.SetValue(Variant.CreateBuilder().SetIntValue(ScreenStatus.Screen).Build());
                    }
                    else if (queryKey.Group == 4 && queryKey.Field == 4) //Unknown Bool
                    {
                        field.SetValue(Variant.CreateBuilder().SetBoolValue(false).Build());
                    }
                    else
                    {
                        Logger.Warn("GameAccount Unknown query-key: {0}, {1}, {2}", queryKey.Program, queryKey.Group,
                                    queryKey.Field);
                    }
                    break;
                case FieldKeyHelper.Program.BNet:
                    if (queryKey.Group == 2 && queryKey.Field == 1) //GameAccount Logged in
                    {
                        field.SetValue(Variant.CreateBuilder().SetBoolValue(GameAccountStatusField.Value).Build());
                    }
                    else if (queryKey.Group == 2 && queryKey.Field == 2) // Away status
                    {
                        field.SetValue(Variant.CreateBuilder().SetIntValue((long)AwayStatus).Build());
                    }
                    else if (queryKey.Group == 2 && queryKey.Field == 3) // Program - always D3
                    {
                        field.SetValue(Variant.CreateBuilder().SetFourccValue("D3").Build());
                    }
                    else if (queryKey.Group == 2 && queryKey.Field == 5) // BattleTag
                    {
                        field.SetValue(Variant.CreateBuilder().SetStringValue(Owner.BattleTag).Build());
                    }
                    else if (queryKey.Group == 2 && queryKey.Field == 7) // DBAccount.EntityId
                    {
                        field.SetValue(Variant.CreateBuilder().SetEntityidValue(Owner.BnetEntityId).Build());
                    }
                    else if (queryKey.Group == 2 && queryKey.Field == 10) // AFK
                    {
                        field.SetValue(
                            Variant.CreateBuilder().SetBoolValue(AwayStatus != AwayStatusFlag.Available).Build());
                    }
                    else
                    {
                        Logger.Warn("GameAccount Unknown query-key: {0}, {1}, {2}", queryKey.Program, queryKey.Group,
                                    queryKey.Field);
                    }
                    break;
            }

            return field.HasValue ? field.Build() : null;
        }

        public override string ToString()
        {
            return String.Format("{{ GameAccount: {0} [lowId: {1}] }}", Owner.BattleTag, BnetEntityId.Low);
        }

        //Moved DB thingies to GameAccountManager.
        /*
        public void SaveToDB()
        {
            try
            {
                if (ExistsInDB())
                {
                    var query =
                        string.Format(
                            "UPDATE gameaccounts SET accountId={0}, banner=@banner, LastOnline={1} WHERE id={2}",
                            this.Owner.PersistentID, this.LastOnlineField.Value, this.PersistentID);

                    using (var cmd = new SQLiteCommand(query, DBManager.Connection))
                    {
                        cmd.Parameters.Add("@banner", System.Data.DbType.Binary).Value = this.BannerConfiguration.ToByteArray();
                        cmd.ExecuteNonQuery();
                    }
                }
                else
                {
                    var query = string.Format("INSERT INTO gameaccounts (id, accountId, banner, LastOnline) VALUES({0},{1}, @banner, {2})", this.PersistentID, this.Owner.PersistentID, this.LastOnlineField.Value);

                    using (var cmd = new SQLiteCommand(query, DBManager.Connection))
                    {
                        cmd.Parameters.Add("@banner", System.Data.DbType.Binary).Value = this.BannerConfiguration.ToByteArray();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                Logger.ErrorException(e, "GameAccount.SaveToDB()");
            }
        }

        public bool DeleteFromDB()
        {
            try
            {
                // Remove from DB
                if (!ExistsInDB()) return false;

                var query = string.Format("DELETE FROM gameaccounts WHERE id={0}", this.PersistentID);
                var cmd = new SQLiteCommand(query, DBManager.Connection);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                Logger.ErrorException(e, "GameAccount.DeleteFromDB()");
                return false;
            }
        }

        private bool ExistsInDB()
        {
            var query = string.Format("SELECT id FROM gameaccounts where id={0}", this.PersistentID);

            var cmd = new SQLiteCommand(query, DBManager.Connection);
            var reader = cmd.ExecuteReader();
            return reader.HasRows;
        }
        */

        //TODO: figure out what 1 and 3 represent, or if it is a flag since all observed values are powers of 2 so far /dustinconrad
        public enum AwayStatusFlag : uint
        {
            Available = 0x00,
            UnknownStatus1 = 0x01,
            Away = 0x02,
            UnknownStatus2 = 0x03,
            Busy = 0x04
        }
    }
}