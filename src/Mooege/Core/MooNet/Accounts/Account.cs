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
    public class Account : PersistentRPCObject
    {
        public D3.PartyMessage.ScreenStatus ScreenStatus { get; set; }

        //public MooNetClient LoggedInClient { get; set; }
        public bool IsOnline 
        { 
            get 
            {
                //check if anygameAccounts are online
                foreach (var gameAccount in GameAccounts)
                {
                    if (gameAccount.Value.IsOnline) return true;
                }
                return false;
            } 
        }

        public string Email { get; private set; } // I - Username
        public byte[] Salt { get; private set; }  // s- User's salt.
        public byte[] PasswordVerifier { get; private set; } // v - password verifier.
        public string Name { get; private set; }
        public int HashCode { get; private set; }
        public string BattleTag
        {
            get
            {
                return Name + "#" + HashCode.ToString("D4");
            }
            set
            {
                if (!value.Contains('#'))
                    throw new Exception("BattleTag must contain '#'");

                var split = value.Split('#');
                this.Name = split[0];
                this.HashCode = Convert.ToInt32(split[1]);
            }
        }

        public UserLevels UserLevel { get; private set; } // user level for account.

        public Dictionary<ulong, GameAccount> GameAccounts
        {
            get { return GameAccountManager.GetGameAccountsForAccount(this); }
        }

        public GameAccount CurrentGameAccount { get; set; }

        public static readonly D3.OnlineService.EntityId AccountHasNoToons =
            D3.OnlineService.EntityId.CreateBuilder().SetIdHigh(0).SetIdLow(0).Build();

        private D3.OnlineService.EntityId _lastSelectedHero = AccountHasNoToons;
        public D3.OnlineService.EntityId LastSelectedHero
        {
            get
            {
                return _lastSelectedHero;
            }
            set
            {
                _lastSelectedHero = value;
            }
        }

        private D3.OnlineService.EntityId _lastSelectedGameAccount = AccountHasNoToons;
        public D3.OnlineService.EntityId LastSelectedGameAccount
        {
            get
            {
                return _lastSelectedGameAccount;
            }
            set
            {
                _lastSelectedGameAccount = value;
            }
        }

        public Account(ulong persistentId, string email, byte[] salt, byte[] passwordVerifier, string battleTagName, int hashCode, UserLevels userLevel) // Account with given persistent ID
            : base(persistentId)
        {
            this.SetFields(email, salt, passwordVerifier, battleTagName, hashCode, userLevel);
        }

        public Account(string email, string password, string battleTagName, int hashCode, UserLevels userLevel) // Account with **newly generated** persistent ID
            : base(StringHashHelper.HashIdentity(battleTagName + "#" + hashCode.ToString("D4")))
        {
            if (password.Length > 16) password = password.Substring(0, 16); // make sure the password does not exceed 16 chars.

            var salt = SRP6a.GetRandomBytes(32);
            var passwordVerifier = SRP6a.CalculatePasswordVerifierForAccount(email, password, salt);

            this.SetFields(email, salt, passwordVerifier, battleTagName, hashCode, userLevel);
        }

        private static ulong? _persistentIdCounter = null;
        protected override ulong GenerateNewPersistentId()
        {
            if (_persistentIdCounter == null)
                _persistentIdCounter = AccountManager.GetNextAvailablePersistentId();

            return (ulong)++_persistentIdCounter;
        }

        private void SetFields(string email, byte[] salt, byte[] passwordVerifier, string battleTagName, int hashCode, UserLevels userLevel)
        {
            this.Email = email;
            this.Salt = salt;
            this.PasswordVerifier = passwordVerifier;
            this.UserLevel = userLevel;

            this.BnetEntityId = bnet.protocol.EntityId.CreateBuilder().SetHigh((ulong)EntityIdHelper.HighIdType.AccountId).SetLow(this.PersistentID).Build();

            this.Name = battleTagName;
            this.HashCode = hashCode;
        }

        public bnet.protocol.presence.Field QueryField(bnet.protocol.presence.FieldKey queryKey)
        {
            var field = bnet.protocol.presence.Field.CreateBuilder().SetKey(queryKey);

            switch ((FieldKeyHelper.Program)queryKey.Program)
            {
                //case FieldKeyHelper.Program.D3:
                //    if (queryKey.Group == 1 && queryKey.Field == 1) // Account's selected toon.
                //    {
                //        if (this.LoggedInClient != null) // check if the account is online actually.
                //            field.SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetMessageValue(this.LoggedInClient.CurrentToon.D3EntityID.ToByteString()).Build());
                //    }
                //    else
                //    {
                //        Logger.Warn("Unknown query-key: {0}, {1}, {2}", queryKey.Program, queryKey.Group, queryKey.Field);
                //    }
                //    break;
                case FieldKeyHelper.Program.BNet:
                    Logger.Warn("Unknown query-key: {0}, {1}, {2}", queryKey.Program, queryKey.Group, queryKey.Field);
                    break;
            }


            return field.HasValue ? field.Build() : null;
        }

        #region Notifications

        //account class generated
        //D3, Account,1,0 -> D3.OnlineService.EntityId: Last Played Hero
        //D3, Account,2,0 -> LastSelectedGameAccount
        //Bnet, Account,1,0 -> RealId Name
        //Bnet, Account,2,0 -> Account Online
        //Bnet, Account,4,index -> GameAccount EntityIds
        //Bnet, Account,5,0 -> BattleTag

        public override List<bnet.protocol.presence.FieldOperation> GetSubscriptionNotifications()
        {
            var operationList = new List<bnet.protocol.presence.FieldOperation>();

            if (this.LastSelectedHero != AccountHasNoToons)
                operationList.Add(GetCurrentlySelectedHeroNotification());
            if (this.LastSelectedGameAccount != AccountHasNoToons)
                operationList.Add(GetSelectedGameAccount());
            operationList.Add(GetAccountIsOnlineNotification());
            operationList.AddRange(GetGameAccountListNotification());
            operationList.Add(GetAccountBattleTagNotification());

            return operationList;
        }

        /// <summary>
        /// D3, Account, 2, 0: Selected Game Account
        /// </summary>
        /// <returns></returns>
        public bnet.protocol.presence.FieldOperation GetSelectedGameAccount()
        {
                var GameAccountKey = FieldKeyHelper.Create(FieldKeyHelper.Program.D3, FieldKeyHelper.OriginatingClass.Account, 2, 0);
                var GameAccountField = bnet.protocol.presence.Field.CreateBuilder().SetKey(GameAccountKey).SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetMessageValue(this.CurrentGameAccount.D3GameAccountId.ToByteString()).Build()).Build();
                return bnet.protocol.presence.FieldOperation.CreateBuilder().SetField(GameAccountField).Build();
        }

        /// <summary>
        /// BNet, Account, 1, 0: RealIDTag
        /// </summary>
        /// <returns></returns>
        public bnet.protocol.presence.FieldOperation GetRealIDName()
        {
            var realNameKey = FieldKeyHelper.Create(FieldKeyHelper.Program.BNet, FieldKeyHelper.OriginatingClass.Account, 1, 0);
            var realNameField = bnet.protocol.presence.Field.CreateBuilder().SetKey(realNameKey).SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetStringValue(this.Name).Build()).Build();
            return (bnet.protocol.presence.FieldOperation.CreateBuilder().SetField(realNameField).Build());
        }

        /// <summary>
        /// D3, Account, 1, 0: Currently Selected Hero
        /// </summary>
        /// <returns></returns>
        public bnet.protocol.presence.FieldOperation GetCurrentlySelectedHeroNotification()
        {
            var ToonKey = FieldKeyHelper.Create(FieldKeyHelper.Program.D3, FieldKeyHelper.OriginatingClass.Account, 1, 0);
            var ToonField = bnet.protocol.presence.Field.CreateBuilder().SetKey(ToonKey).SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetMessageValue(this.LastSelectedHero.ToByteString()).Build()).Build();
            return bnet.protocol.presence.FieldOperation.CreateBuilder().SetField(ToonField).Build();
        }

        /// <summary>
        /// BNet, Account, 2, 0: Account Online
        /// </summary>
        /// <returns></returns>
        public bnet.protocol.presence.FieldOperation GetAccountIsOnlineNotification()
        {
            var accountOnlineKey = FieldKeyHelper.Create(FieldKeyHelper.Program.BNet, FieldKeyHelper.OriginatingClass.Account, 2, 0);
            var accountOnlineField = bnet.protocol.presence.Field.CreateBuilder().SetKey(accountOnlineKey).SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetBoolValue(this.IsOnline).Build()).Build();
            return bnet.protocol.presence.FieldOperation.CreateBuilder().SetField(accountOnlineField).Build();
        }

        /// <summary>
        /// BNet, Account, 5, 0: Account BattleTag
        /// </summary>
        /// <returns></returns>
        public bnet.protocol.presence.FieldOperation GetAccountBattleTagNotification()
        {
            var tempNameKey = FieldKeyHelper.Create(FieldKeyHelper.Program.BNet, FieldKeyHelper.OriginatingClass.Account, 5, 0);
            var tempNameField = bnet.protocol.presence.Field.CreateBuilder().SetKey(tempNameKey).SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetStringValue(this.BattleTag).Build()).Build();
            return bnet.protocol.presence.FieldOperation.CreateBuilder().SetField(tempNameField).Build();
        }

        /// <summary>
        /// BNet, Account, 4, index: Account GameAccountList
        /// </summary>
        /// <returns></returns>
        public List<bnet.protocol.presence.FieldOperation> GetGameAccountListNotification()
        {
            var operationList = new List<bnet.protocol.presence.FieldOperation>();

            foreach (var pair in this.GameAccounts.Values)
            {
                var gameAccountKey = FieldKeyHelper.Create(FieldKeyHelper.Program.BNet, FieldKeyHelper.OriginatingClass.Account, 4, pair.BnetEntityId.High);
                var gameAccountField = bnet.protocol.presence.Field.CreateBuilder().SetKey(gameAccountKey).SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetEntityidValue(pair.BnetEntityId).Build()).Build();
                operationList.Add(bnet.protocol.presence.FieldOperation.CreateBuilder().SetField(gameAccountField).Build());
            }
            return operationList;
        }
        #endregion

        public bool VerifyPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                return false;

            if (password.Length < 8 || password.Length > 16)
                return false;

            var calculatedVerifier = SRP6a.CalculatePasswordVerifierForAccount(this.Email, password, this.Salt);
            return calculatedVerifier.SequenceEqual(this.PasswordVerifier);
        }

        public void SaveToDB()
        {
            try
            {
                var query = string.Format("INSERT INTO accounts (id, email, salt, passwordVerifier, battletagname, hashcode, userLevel) VALUES({0}, '{1}', @salt, @passwordVerifier, '{2}', {3}, {4})",
                        this.PersistentID, this.Email, this.Name, this.HashCode, (byte)this.UserLevel);

                using (var cmd = new SQLiteCommand(query, DBManager.Connection))
                {
                    cmd.Parameters.Add("@salt", System.Data.DbType.Binary, 32).Value = this.Salt;
                    cmd.Parameters.Add("@passwordVerifier", System.Data.DbType.Binary, 128).Value = this.PasswordVerifier;
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Logger.ErrorException(e, "SaveToDB()");
            }
        }

        public void UpdatePassword(string newPassword)
        {
            this.PasswordVerifier = SRP6a.CalculatePasswordVerifierForAccount(this.Email, newPassword, this.Salt);
            try
            {
                var query = string.Format("UPDATE accounts SET passwordVerifier=@passwordVerifier WHERE id={0}", this.PersistentID);

                using (var cmd = new SQLiteCommand(query, DBManager.Connection))
                {
                    cmd.Parameters.Add("@passwordVerifier", System.Data.DbType.Binary, 128).Value = this.PasswordVerifier;
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Logger.ErrorException(e, "UpdatePassword()");
            }
        }

        public void UpdateUserLevel(UserLevels userLevel)
        {
            this.UserLevel = userLevel;
            try
            {
                var query = string.Format("UPDATE accounts SET userLevel={0} WHERE id={1}", (byte)userLevel, this.PersistentID);
                var cmd = new SQLiteCommand(query, DBManager.Connection);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Logger.ErrorException(e, "UpdateUserLevel()");
            }
        }

        public override string ToString()
        {
            return String.Format("{{ Account: {0} [lowId: {1}] }}", this.Email, this.BnetEntityId.Low);
        }

        /// <summary>
        /// User-levels.
        /// </summary>
        public enum UserLevels : byte
        {
            User,
            GM,
            Admin,
            Owner
        }
    }
}
