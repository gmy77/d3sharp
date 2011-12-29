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
using Mooege.Net.MooNet;

namespace Mooege.Core.MooNet.Accounts
{
    public class GameAccount : PersistentRPCObject
    {
        public Account Owner { get; set; }
        public bnet.protocol.EntityId BnetGameAccountID { get; private set; }

        public D3.Account.BannerConfiguration BannerConfiguration { get; set; }
        public D3.GameMessage.SetGameAccountSettings Settings { get; set; }

        public List<bnet.protocol.achievements.AchievementUpdateRecord> Achievements { get; set; }
        public List<bnet.protocol.achievements.CriteriaUpdateRecord> AchievementCriteria { get; set; }

        private static readonly D3.OnlineService.EntityId AccountHasNoToons =
            D3.OnlineService.EntityId.CreateBuilder().SetIdHigh(0).SetIdLow(0).Build();

        public Dictionary<ulong, Toon> Toons
        {
            get { return ToonManager.GetToonsForGameAccount(this); }
        }

        private static ulong? _persistentIdCounter = null;

        protected override ulong GenerateNewPersistentId()
        {
            if (_persistentIdCounter == null)
                _persistentIdCounter = AccountManager.GetNextAvailablePersistentId();

            return (ulong)++_persistentIdCounter;
        }

        public GameAccount(ulong persistentId, ulong accountId)
            : base(persistentId)
        {
            this.SetField(AccountManager.GetAccountByPersistentID(persistentId));
        }

        public GameAccount(Account account)
            : base(account.BnetAccountID.Low)
        {
            this.SetField(account);
        }

        private void SetField(Account owner)
        {
            this.Owner = owner;
            var bnetGameAccountHigh = ((ulong)EntityIdHelper.HighIdType.GameAccountId) + (0x6200004433);
            this.BnetGameAccountID = bnet.protocol.EntityId.CreateBuilder().SetHigh(bnetGameAccountHigh).SetLow(this.PersistentID).Build();

            this.BannerConfiguration = D3.Account.BannerConfiguration.CreateBuilder()
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
                this._loggedInClient = this.Owner.LoggedInClient = value;

                // notify friends.
                if (FriendManager.Friends[this.Owner.BnetAccountID.Low].Count == 0) return; // if account has no friends just skip.

                var fieldKey = FieldKeyHelper.Create(FieldKeyHelper.Program.BNet, 1, 2, 0);
                var field = bnet.protocol.presence.Field.CreateBuilder().SetKey(fieldKey).SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetBoolValue(this.IsOnline).Build()).Build();
                var operation = bnet.protocol.presence.FieldOperation.CreateBuilder().SetField(field).Build();

                var state = bnet.protocol.presence.ChannelState.CreateBuilder().SetEntityId(this.Owner.BnetAccountID).AddFieldOperation(operation).Build();
                var channelState = bnet.protocol.channel.ChannelState.CreateBuilder().SetExtension(bnet.protocol.presence.ChannelState.Presence, state);
                var notification = bnet.protocol.channel.UpdateChannelStateNotification.CreateBuilder().SetStateChange(channelState).Build();

                foreach (var friend in FriendManager.Friends[this.Owner.BnetAccountID.Low])
                {
                    var account = AccountManager.GetAccountByPersistentID(friend.Id.Low);
                    if (account == null || account.LoggedInClient == null) return; // only send to friends that are online.

                    // make the rpc call.
                    account.LoggedInClient.MakeTargetedRPC(this, () =>
                        bnet.protocol.channel.ChannelSubscriber.CreateStub(account.LoggedInClient).NotifyUpdateChannelState(null, notification, callback => { }));
                }
            }
        }

        public D3.Account.Digest Digest
        {
            get
            {
                var builder = D3.Account.Digest.CreateBuilder().SetVersion(102) // 7447=>99, 7728=> 100 /raist. 
                    .SetBannerConfiguration(this.BannerConfiguration)
                    .SetFlags(0);

                D3.OnlineService.EntityId lastPlayedHeroId;
                if (Toons.Count > 0)
                {
                    lastPlayedHeroId = Toons.First().Value.D3EntityID; // we should actually hold player's last hero in database. /raist
                }
                else
                {
                    lastPlayedHeroId = AccountHasNoToons;
                }

                builder.SetLastPlayedHeroId(lastPlayedHeroId);
                return builder.Build();
            }
        }

        protected override void NotifySubscriptionAdded(MooNetClient client)
        {
            var operations = new List<bnet.protocol.presence.FieldOperation>();

            //gameaccount
            //D3,2,1,0 -> D3.Account.BannerConfiguration
            //Bnet,2,1,0 -> true
            //Bnet,2,4,0 -> FourCC = "D3"
            //Bnet,2,5,0 -> Unk Int
            //Bnet,2,6,0 -> BattleTag
            //Bnet,2,7,0 -> accountlow#1

            // Banner configuration
            var fieldKey1 = FieldKeyHelper.Create(FieldKeyHelper.Program.D3, 2, 1, 0);
            var field1 = bnet.protocol.presence.Field.CreateBuilder().SetKey(fieldKey1).SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetMessageValue(client.CurrentGameAccount.BannerConfiguration.ToByteString()).Build()).Build();
            operations.Add(bnet.protocol.presence.FieldOperation.CreateBuilder().SetField(field1).Build());

            // ??
            var fieldKey2 = FieldKeyHelper.Create(FieldKeyHelper.Program.BNet, 2, 1, 0);
            var field2 = bnet.protocol.presence.Field.CreateBuilder().SetKey(fieldKey2).SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetBoolValue(true).Build()).Build();
            operations.Add(bnet.protocol.presence.FieldOperation.CreateBuilder().SetField(field2).Build());

            // Program - FourCC "D3"
            var fieldKey3 = FieldKeyHelper.Create(FieldKeyHelper.Program.BNet, 2, 4, 0);
            var field3 = bnet.protocol.presence.Field.CreateBuilder().SetKey(fieldKey3).SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetFourccValue("D3").Build()).Build();
            operations.Add(bnet.protocol.presence.FieldOperation.CreateBuilder().SetField(field3).Build());

            // Unknown int
            var fieldKey4 = FieldKeyHelper.Create(FieldKeyHelper.Program.BNet, 2, 5, 0);
            var field4 = bnet.protocol.presence.Field.CreateBuilder().SetKey(fieldKey4).SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetIntValue(1324923597904795).Build()).Build();
            operations.Add(bnet.protocol.presence.FieldOperation.CreateBuilder().SetField(field4).Build());

            //BattleTag
            var fieldKey5 = FieldKeyHelper.Create(FieldKeyHelper.Program.BNet, 2, 6, 0);
            var field5 = bnet.protocol.presence.Field.CreateBuilder().SetKey(fieldKey5).SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetStringValue("NICKTEMPNAME").Build()).Build();
            operations.Add(bnet.protocol.presence.FieldOperation.CreateBuilder().SetField(field5).Build());

            //Account.Low + "#1"
            var fieldKey6 = FieldKeyHelper.Create(FieldKeyHelper.Program.BNet, 2, 7, 0);
            var field6 = bnet.protocol.presence.Field.CreateBuilder().SetKey(fieldKey6).SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetStringValue(Owner.BnetAccountID.Low.ToString() + "#1").Build()).Build();
            operations.Add(bnet.protocol.presence.FieldOperation.CreateBuilder().SetField(field6).Build());

            // Create a presence.ChannelState
            var state = bnet.protocol.presence.ChannelState.CreateBuilder().SetEntityId(this.BnetGameAccountID).AddRangeFieldOperation(operations).Build();

            // Embed in channel.ChannelState
            var channelState = bnet.protocol.channel.ChannelState.CreateBuilder().SetExtension(bnet.protocol.presence.ChannelState.Presence, state);

            // Put in addnotification message
            var notification = bnet.protocol.channel.AddNotification.CreateBuilder().SetChannelState(channelState);

            // Make the rpc call
            client.MakeTargetedRPC(this, () =>
                bnet.protocol.channel.ChannelSubscriber.CreateStub(client).NotifyAdd(null, notification.Build(), callback => { }));
        }

        public override string ToString()
        {
            return String.Format("{{ GameAccount: {0} [lowId: {1}] }}", this.Owner.BattleTag, this.BnetGameAccountID.Low);
        }

        public void SaveToDB()
        {
            try
            {
                if (ExistsInDB())
                {
                    var query =
                        string.Format(
                            "UPDATE gameaccount SET accountId={0} WHERE id={1}",
                            this.Owner.PersistentID, this.PersistentID);

                    var cmd = new SQLiteCommand(query, DBManager.Connection);
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    var query =
                        string.Format(
                            "INSERT INTO gameaccount (id, accountId) VALUES({0},{1})",
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

                var query = string.Format("DELETE FROM gameaccount WHERE id={0}", this.PersistentID);
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

    }
}
