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

using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using Mooege.Common.Storage;
using Mooege.Common.Helpers.Hash;
using Mooege.Core.Cryptography;
using Mooege.Core.MooNet.Friends;
using Mooege.Core.MooNet.Helpers;
using Mooege.Core.MooNet.Objects;
using Mooege.Core.MooNet.Toons;
using Mooege.Core.MooNet.Channels;
using Mooege.Net.MooNet;

namespace Mooege.Core.MooNet.Accounts
{
    public class GameAccount : PersistentRPCObject
    {
        public Account Owner { get; set; }

        public D3.OnlineService.EntityId D3GameAccountId { get; private set; }
        public ByteStringPresenceField<D3.Account.BannerConfiguration> BannerConfigurationField
            = new ByteStringPresenceField<D3.Account.BannerConfiguration>(FieldKeyHelper.Program.D3, FieldKeyHelper.OriginatingClass.GameAccount, 1, 0);

        public ByteStringPresenceField<D3.OnlineService.EntityId> CurrentHeroIdField
            = new ByteStringPresenceField<D3.OnlineService.EntityId>(FieldKeyHelper.Program.D3, FieldKeyHelper.OriginatingClass.GameAccount, 2, 0);

        public IntPresenceField ScreenStatusField
            = new IntPresenceField(FieldKeyHelper.Program.BNet, FieldKeyHelper.OriginatingClass.GameAccount, 3, 0);

        public FourCCPresenceField ProgramField
            = new FourCCPresenceField(FieldKeyHelper.Program.BNet, FieldKeyHelper.OriginatingClass.GameAccount, 4, 0);

        public StringPresenceField BattleTagField
            = new StringPresenceField(FieldKeyHelper.Program.BNet, FieldKeyHelper.OriginatingClass.GameAccount, 6, 0);

        public StringPresenceField AccountField
            = new StringPresenceField(FieldKeyHelper.Program.BNet, FieldKeyHelper.OriginatingClass.GameAccount, 7, 0);

        public BoolPresenceField GameAccountStatusField
            = new BoolPresenceField(FieldKeyHelper.Program.BNet, FieldKeyHelper.OriginatingClass.GameAccount, 1, 0);

        public IntPresenceField GameAccountStatusIdField
            = new IntPresenceField(FieldKeyHelper.Program.BNet, FieldKeyHelper.OriginatingClass.GameAccount, 5, 0);

        public FieldKeyHelper.Program Program;

        private D3.Account.BannerConfiguration _bannerConfiguration;
        public D3.Account.BannerConfiguration BannerConfiguration
        {
            get
            {
                return _bannerConfiguration;
            }
            set
            {
                _bannerConfiguration = value;
                BannerConfigurationField.Value = value;
                this.ChangedFields.SetPresenceFieldValue(this.BannerConfigurationField);
            }
        }

        private D3.PartyMessage.ScreenStatus _screenstatus;
        public D3.PartyMessage.ScreenStatus ScreenStatus
        {
            get
            {
                return _screenstatus;
            }
            set
            {
                _screenstatus = value;
                this.ScreenStatusField.Value = value.Status;
                this.ChangedFields.SetPresenceFieldValue(this.ScreenStatusField);
            }
        }
        /// <summary>
        /// Selected toon for current account.
        /// </summary>
        private Toon _currentToon;
        public Toon CurrentToon
        {
            get
            {
                return _currentToon;
            }
            set
            {
                this._currentToon = value;
                this.CurrentHeroIdField.Value = value.D3EntityID;
                this.Owner.LastSelectedHero = value.D3EntityID;
                this.ChangedFields.SetPresenceFieldValue(this.Owner.LastSelectedHeroField);
                this.ChangedFields.SetPresenceFieldValue(this.CurrentHeroIdField);
                this.ChangedFields.SetPresenceFieldValue(value.HeroClassField);
                this.ChangedFields.SetPresenceFieldValue(value.HeroLevelField);
                this.ChangedFields.SetPresenceFieldValue(value.HeroVisualEquipmentField);
                this.ChangedFields.SetPresenceFieldValue(value.HeroFlagsField);
                this.ChangedFields.SetPresenceFieldValue(value.HeroNameField);
            }
        }

        public D3.GameMessage.SetGameAccountSettings Settings { get; set; }

        /// <summary>
        /// Away status
        /// </summary>
        public AwayStatusFlag AwayStatus { get; private set; }

        private D3.OnlineService.EntityId _lastPlayedHeroId = AccountHasNoToons;
        public D3.OnlineService.EntityId lastPlayedHeroId
        {
            get
            {
                if (_lastPlayedHeroId == AccountHasNoToons && this.Toons.Count > 0 && !this.IsOnline)
                {
                    _lastPlayedHeroId = this.CurrentHeroIdField.Value = this.Toons.First().Value.D3EntityID;
                    this._currentToon = ToonManager.GetToonByLowID(_lastPlayedHeroId.IdLow);
                }
                return _lastPlayedHeroId;
            }
            set
            {
                _lastPlayedHeroId = value;
            }
        }

        public List<bnet.protocol.achievements.AchievementUpdateRecord> Achievements { get; set; }
        public List<bnet.protocol.achievements.CriteriaUpdateRecord> AchievementCriteria { get; set; }

        public D3.Profile.AccountProfile Profile
        {
            get
            {
                return D3.Profile.AccountProfile.CreateBuilder()
                    .Build();
            }
        }

        public static readonly D3.OnlineService.EntityId AccountHasNoToons =
            D3.OnlineService.EntityId.CreateBuilder().SetIdHigh(0).SetIdLow(0).Build();

        public Dictionary<ulong, Toon> Toons
        {
            get { return ToonManager.GetToonsForGameAccount(this); }
        }

        private static ulong? _persistentIdCounter = null;

        protected override ulong GenerateNewPersistentId()
        {
            if (_persistentIdCounter == null)
                _persistentIdCounter = GameAccountManager.GetNextAvailablePersistentId();

            return (ulong)++_persistentIdCounter;
        }

        public GameAccount(ulong persistentId, ulong accountId)
            : base(persistentId)
        {
            this.SetField(AccountManager.GetAccountByPersistentID(accountId));
        }

        public GameAccount(Account account)
            : base(account.BnetEntityId.Low)
        {
            this.SetField(account);
        }

        private void SetField(Account owner)
        {
            this.Owner = owner;
            var bnetGameAccountHigh = ((ulong)EntityIdHelper.HighIdType.GameAccountId) + (0x6200004433);
            this.BnetEntityId = bnet.protocol.EntityId.CreateBuilder().SetHigh(bnetGameAccountHigh).SetLow(this.PersistentID).Build();
            this.D3GameAccountId = D3.OnlineService.EntityId.CreateBuilder().SetIdHigh(bnetGameAccountHigh).SetIdLow(this.PersistentID).Build();

            //TODO: Now hardcode all game account notifications to D3
            this.ProgramField.Value = "D3";
            this.AccountField.Value = Owner.BnetEntityId.Low.ToString() + "#1";
            this.BattleTagField.Value = this.Owner.BattleTag;
            this.BannerConfiguration =
                D3.Account.BannerConfiguration.CreateBuilder()
                .SetBannerShape(2952440006)
                .SetSigilMain(976722430)
                .SetSigilAccent(803826460)
                .SetPatternColor(1797588777)
                .SetBackgroundColor(1379006192)
                .SetSigilColor(1797588777)
                .SetSigilPlacement(3057352154)
                .SetPattern(4173846786)
                .SetUseSigilVariant(true)
                .SetEpicBanner(0)
                .Build();

            this.Achievements = new List<bnet.protocol.achievements.AchievementUpdateRecord>();
            this.AchievementCriteria = new List<bnet.protocol.achievements.CriteriaUpdateRecord>();

        }

        public bool IsOnline { get { return this.LoggedInClient != null; } }

        private MooNetClient _loggedInClient;

        public MooNetClient LoggedInClient
        {
            get
            {
                return this._loggedInClient;
            }
            set
            {
                this._loggedInClient = value;
                ChangedFields.SetPresenceFieldValue(this.GameAccountStatusField);
                ChangedFields.SetPresenceFieldValue(this.GameAccountStatusIdField);

                // notify friends.
                if (FriendManager.Friends[this.Owner.BnetEntityId.Low].Count == 0) return; // if account has no friends just skip.

                //TODO: Remove this set once delegate for set is added to presence field
                this.Owner.AccountOnlineField.Value = true;
                var operation = this.Owner.AccountOnlineField.GetFieldOperation();

                var state = bnet.protocol.presence.ChannelState.CreateBuilder().SetEntityId(this.Owner.BnetEntityId).AddFieldOperation(operation).Build();
                var channelState = bnet.protocol.channel.ChannelState.CreateBuilder().SetExtension(bnet.protocol.presence.ChannelState.Presence, state);
                var notification = bnet.protocol.channel.UpdateChannelStateNotification.CreateBuilder().SetStateChange(channelState).Build();

                foreach (var friend in FriendManager.Friends[this.Owner.BnetEntityId.Low])
                {
                    var account = AccountManager.GetAccountByPersistentID(friend.Id.Low);
                    if (account == null || !account.IsOnline) return; // only send to friends that are online.

                    // make the rpc call.
                    var d3GameAccounts = GameAccountManager.GetGameAccountsForAccountProgram(account, FieldKeyHelper.Program.D3);
                    foreach (var d3GameAccount in d3GameAccounts.Values)
                    {
                        if (d3GameAccount.IsOnline)
                        {
                            d3GameAccount.LoggedInClient.MakeTargetedRPC(this.Owner, () =>
                                bnet.protocol.channel.ChannelSubscriber.CreateStub(d3GameAccount.LoggedInClient).NotifyUpdateChannelState(null, notification, callback => { }));
                        }
                    }
                }
            }
        }

        public D3.Account.Digest Digest
        {
            get
            {
                var builder = D3.Account.Digest.CreateBuilder().SetVersion(102) // 7447=>99, 7728=> 100, 8801=>102
                    .SetBannerConfiguration(this.BannerConfigurationField.Value)
                    .SetFlags(0)
                    .SetLastPlayedHeroId(lastPlayedHeroId);

                return builder.Build();
            }
        }

        #region Notifications

        public override void NotifyUpdate()
        {
            var operations = ChangedFields.GetChangedFieldList();
            ChangedFields.ClearChanged();
            base.MakeRPC(this.LoggedInClient, operations);

            //Update everyone subscribed
            foreach (var subscriber in this.Subscribers)
            {
                var gameAccount = subscriber.Account.CurrentGameAccount;
                if (gameAccount.IsOnline) //This should never be false, subscribers should be unsubscribed if disconnected
                {
                    var state = bnet.protocol.presence.ChannelState.CreateBuilder().SetEntityId(this.BnetEntityId).AddRangeFieldOperation(operations).Build();

                    // Embed in channel.ChannelState
                    var channelState = bnet.protocol.channel.ChannelState.CreateBuilder().SetExtension(bnet.protocol.presence.ChannelState.Presence, state);

                    // Put in addnotification message
                    var notification = bnet.protocol.channel.UpdateChannelStateNotification.CreateBuilder().SetStateChange(channelState);

                    gameAccount.LoggedInClient.MakeTargetedRPC(this, () =>
                        bnet.protocol.channel.ChannelSubscriber.CreateStub(gameAccount.LoggedInClient).NotifyUpdateChannelState(null, notification.Build(), callback => { }));
                }
                else
                {
                    Logger.Warn("Subscriber: {0} not online.", subscriber.Account);
                }
            }

        }

        public override List<bnet.protocol.presence.FieldOperation> GetSubscriptionNotifications()
        {
            //for now set it here
            this.GameAccountStatusField.Value = this.IsOnline;
            this.GameAccountStatusIdField.Value = (int)(this.IsOnline == true ? 1324923597904795 : 0);

            var operationList = new List<bnet.protocol.presence.FieldOperation>();

            //gameaccount
            //D3,GameAccount,1,0 -> D3.Account.BannerConfiguration
            //D3,GameAccount,2,0 -> ToonId
            //D3,Hero,1,0 -> Hero Class
            //D3,Hero,2,0 -> Hero's current level
            //D3,Hero,3,0 -> D3.Hero.VisualEquipment
            //D3,Hero,4,0 -> Hero's flags
            //D3,Hero,5,0 -> Hero Name
            //Bnet,GameAccount,1,0 -> GameAccount Online
            //Bnet,GameAccount,4,0 -> FourCC = "D3"
            //Bnet,GameAccount,5,0 -> Unk Int (0 if GameAccount is Offline)
            //Bnet,GameAccount,6,0 -> BattleTag
            //Bnet,GameAccount,7,0 -> Account.Low + "#1"

            operationList.Add(BannerConfigurationField.GetFieldOperation());
            if (this.lastPlayedHeroId != AccountHasNoToons)
            {
                operationList.Add(this.CurrentHeroIdField.GetFieldOperation());
                operationList.AddRange(this.CurrentToon.GetSubscriptionNotifications());
            }

            operationList.Add(this.GameAccountStatusField.GetFieldOperation());
            operationList.Add(this.ProgramField.GetFieldOperation());
            operationList.Add(this.GameAccountStatusIdField.GetFieldOperation());
            operationList.Add(this.BattleTagField.GetFieldOperation());
            operationList.Add(this.AccountField.GetFieldOperation());

            return operationList;
        }

        #endregion

        public void Update(bnet.protocol.presence.FieldOperation operation)
        {
            switch (operation.Operation)
            {
                case bnet.protocol.presence.FieldOperation.Types.OperationType.SET:
                    DoSet(operation.Field);
                    break;
                case bnet.protocol.presence.FieldOperation.Types.OperationType.CLEAR:
                    DoClear(operation.Field);
                    break;
            }
        }

        private void DoSet(bnet.protocol.presence.Field field)
        {
            var operation = bnet.protocol.presence.FieldOperation.CreateBuilder();

            var returnField = bnet.protocol.presence.Field.CreateBuilder().SetKey(field.Key);

            switch ((FieldKeyHelper.Program)field.Key.Program)
            {
                case FieldKeyHelper.Program.D3:
                    if (field.Key.Group == 4 && field.Key.Field == 1)
                    {
                        if (field.Value.HasMessageValue) //7727 Sends empty SET instead of a CLEAR -Egris
                        {
                            var entityId = D3.OnlineService.EntityId.ParseFrom(field.Value.MessageValue);
                            var channel = ChannelManager.GetChannelByEntityId(entityId);
                            this.LoggedInClient.CurrentChannel = channel;
                            Logger.Trace("{0} set channel to {1}", this, channel);
                        }
                        else
                        {
                            Logger.Warn("Emtpy-field: {0}, {1}, {2}", field.Key.Program, field.Key.Group, field.Key.Field);
                        }
                    }
                    else if (field.Key.Group == 4 && field.Key.Field == 2)
                    {
                        //catch to stop Logger.Warn spam on client start and exit
                        // should D3.4.2 int64 Current screen (0=in-menus, 1=in-menus, 3=in-menus); see ScreenStatus sent to ChannelService.UpdateChannelState call /raist
                    }
                    else if (field.Key.Group == 4 && field.Key.Field == 3)
                    {
                        //Looks to be the ToonFlags of the party leader/inviter when it is an int, OR the message set in an open to friends game when it is a string /dustinconrad
                    }
                    else
                    {
                        Logger.Warn("Unknown set-field: {0}, {1}, {2} := {3}", field.Key.Program, field.Key.Group, field.Key.Field, field.Value);
                    }
                    break;
                case FieldKeyHelper.Program.BNet:
                    if (field.Key.Group == 2 && field.Key.Field == 3) // Away status
                    {
                        this.AwayStatus = (AwayStatusFlag)field.Value.IntValue;
                        returnField.SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetIntValue((long)this.AwayStatus).Build()).Build();
                        Logger.Trace("{0} set AwayStatus to {1}.", this, this.AwayStatus);
                    }
                    else
                    {
                        Logger.Warn("Unknown set-field: {0}, {1}, {2} := {3}", field.Key.Program, field.Key.Group, field.Key.Field, field.Value);
                    }
                    break;
            }
            if (returnField.HasValue)
            {
                operation.SetField(returnField);
                // Create a presence.ChannelState
                var state = bnet.protocol.presence.ChannelState.CreateBuilder().SetEntityId(this.BnetEntityId).AddFieldOperation(operation).Build();

                // Embed in channel.ChannelState
                var channelState = bnet.protocol.channel.ChannelState.CreateBuilder().SetExtension(bnet.protocol.presence.ChannelState.Presence, state);

                // Put in addnotification message
                var notification = bnet.protocol.channel.UpdateChannelStateNotification.CreateBuilder().SetStateChange(channelState);

                // Make the rpc call
                this.LoggedInClient.MakeTargetedRPC(this, () =>
                    bnet.protocol.channel.ChannelSubscriber.CreateStub(this.LoggedInClient).NotifyUpdateChannelState(null, notification.Build(), callback => { }));

                //Update all online friends
                //foreach (var friend in FriendManager.Friends[this.BnetEntityId.Low])
                //{
                //    var gameAccount = GameAccountManager.GetAccountByPersistentID(friend.Id.Low);
                //    if (gameAccount.IsOnline)
                //    {
                //        gameAccount.LoggedInClient.MakeTargetedRPC(FriendManager.Instance, () =>
                //            bnet.protocol.channel.ChannelSubscriber.CreateStub(gameAccount.LoggedInClient).NotifyUpdateChannelState(null, notification.Build(), callback => { }));
                //    }
                //}

                //Update everyone subscribed
                foreach (var subscriber in this.Subscribers)
                {
                    var gameAccount = subscriber.Account.CurrentGameAccount;
                    if (gameAccount.IsOnline) //This should never be false, subscribers should be unsubscribed if disconnected
                    {
                        gameAccount.LoggedInClient.MakeTargetedRPC(this, () =>
                            bnet.protocol.channel.ChannelSubscriber.CreateStub(gameAccount.LoggedInClient).NotifyUpdateChannelState(null, notification.Build(), callback => { }));
                    }
                    else
                    {
                        Logger.Warn("Subscriber: {0} not online.", subscriber.Account);
                    }
                }
            }
        }

        private void DoClear(bnet.protocol.presence.Field field)
        {
            switch ((FieldKeyHelper.Program)field.Key.Program)
            {
                case FieldKeyHelper.Program.D3:
                    Logger.Warn("Unknown clear-field: {0}, {1}, {2}", field.Key.Program, field.Key.Group, field.Key.Field);
                    break;
                case FieldKeyHelper.Program.BNet:
                    Logger.Warn("Unknown clear-field: {0}, {1}, {2}", field.Key.Program, field.Key.Group, field.Key.Field);
                    break;
            }
        }

        public bnet.protocol.presence.Field QueryField(bnet.protocol.presence.FieldKey queryKey)
        {
            var field = bnet.protocol.presence.Field.CreateBuilder().SetKey(queryKey);

            switch ((FieldKeyHelper.Program)queryKey.Program)
            {
                case FieldKeyHelper.Program.D3:
                    if (queryKey.Group == 2 && queryKey.Field == 1) // Banner configuration
                    {
                        field.SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetMessageValue(this.BannerConfigurationField.Value.ToByteString()).Build());
                    }
                    if (queryKey.Group == 2 && queryKey.Field == 2) //Hero's EntityId
                    {
                        field.SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetMessageValue(this.lastPlayedHeroId.ToByteString()).Build());
                    }
                    else if (queryKey.Group == 3 && queryKey.Field == 1) // Hero's class (GbidClass)
                    {
                        field.SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetIntValue(this.CurrentToon.ClassID).Build());
                    }
                    else if (queryKey.Group == 3 && queryKey.Field == 2) // Hero's current level
                    {
                        field.SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetIntValue(this.CurrentToon.Level).Build());
                    }
                    else if (queryKey.Group == 3 && queryKey.Field == 3) // Hero's visible equipment
                    {
                        field.SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetMessageValue(this.CurrentToon.Equipment.ToByteString()).Build());
                    }
                    else if (queryKey.Group == 3 && queryKey.Field == 4) // Hero's flags (gender and such)
                    {
                        field.SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetIntValue((uint)(this.CurrentToon.Flags | ToonFlags.AllUnknowns)).Build());
                    }
                    else if (queryKey.Group == 3 && queryKey.Field == 5) // Toon name
                    {
                        field.SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetStringValue(this.CurrentToon.Name).Build());
                    }
                    else if (queryKey.Group == 4 && queryKey.Field == 1) // Channel ID if the client is online
                    {
                        if (this.LoggedInClient != null && this.LoggedInClient.CurrentChannel != null) field.SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetMessageValue(this.LoggedInClient.CurrentChannel.D3EntityId.ToByteString()).Build());
                        else field.SetValue(bnet.protocol.attribute.Variant.CreateBuilder().Build());
                    }
                    else if (queryKey.Group == 4 && queryKey.Field == 2) // Current screen (all known values are just "in-menu"; also see ScreenStatuses sent in ChannelService.UpdateChannelState)
                    {
                        field.SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetIntValue(0).Build());
                    }
                    else
                    {
                        Logger.Warn("Unknown query-key: {0}, {1}, {2}", queryKey.Program, queryKey.Group, queryKey.Field);
                    }
                    break;
                case FieldKeyHelper.Program.BNet:
                    if (queryKey.Group == 2 && queryKey.Field == 4) // Program - always D3
                    {
                        field.SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetFourccValue("D3").Build());
                    }
                    else if (queryKey.Group == 2 && queryKey.Field == 6) // BattleTag
                    {
                        field.SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetStringValue(this.Owner.BattleTag).Build());
                    }
                    else
                    {
                        Logger.Warn("Unknown query-key: {0}, {1}, {2}", queryKey.Program, queryKey.Group, queryKey.Field);
                    }
                    break;
            }

            return field.HasValue ? field.Build() : null;
        }

        public override string ToString()
        {
            return String.Format("{{ GameAccount: {0} [lowId: {1}] }}", this.Owner.BattleTag, this.BnetEntityId.Low);
        }

        public void SaveToDB()
        {
            try
            {
                if (ExistsInDB())
                {
                    var query =
                        string.Format(
                            "UPDATE gameaccounts SET accountId={0} WHERE id={1}",
                            this.Owner.PersistentID, this.PersistentID);

                    var cmd = new SQLiteCommand(query, DBManager.Connection);
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    var query =
                        string.Format(
                            "INSERT INTO gameaccounts (id, accountId) VALUES({0},{1})",
                            this.PersistentID, this.Owner.PersistentID);

                    var cmd = new SQLiteCommand(query, DBManager.Connection);
                    cmd.ExecuteNonQuery();
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
            var query =
                string.Format(
                    "SELECT id from gameaccounts where id={0}",
                    this.PersistentID);

            var cmd = new SQLiteCommand(query, DBManager.Connection);
            var reader = cmd.ExecuteReader();
            return reader.HasRows;
        }

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
