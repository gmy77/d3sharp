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
using Google.ProtocolBuffers;
using Mooege.Common.Logging;
using Mooege.Net.MooNet;
using Mooege.Core.MooNet.Toons;
using Mooege.Core.MooNet.Helpers;
using Mooege.Core.MooNet.Accounts;
using System.Collections.Generic;

namespace Mooege.Core.MooNet.Services
{
    [Service(serviceID: 0x8, serviceName: "bnet.protocol.game_utilities.GameUtilities")]
    public class GameUtilitiesService : bnet.protocol.game_utilities.GameUtilities, IServerService
    {
        private static readonly Logger Logger = LogManager.CreateLogger();
        public MooNetClient Client { get; set; }
        public bnet.protocol.Header LastCallHeader { get; set; }
        public uint Status { get; set; }

        public override void ProcessClientRequest(IRpcController controller, bnet.protocol.game_utilities.ClientRequest request, Action<bnet.protocol.game_utilities.ClientResponse> done)
        {
            var MessageId = request.GetAttribute(1).Value.IntValue;
            Logger.Trace("ProcessClientRequest() ID: {0}", MessageId);

            var builder = bnet.protocol.game_utilities.ClientResponse.CreateBuilder();

            //var version = request.GetAttribute(0).Value.StringValue; //0.5.1
            var attr = bnet.protocol.attribute.Attribute.CreateBuilder();
            switch (MessageId)
            {
                case 0: //D3.GameMessage.HeroDigestListRequest -> D3.GameMessage.HeroDigestListResponse
                    var digestList = GetHeroDigestList(D3.GameMessage.HeroDigestListRequest.ParseFrom(request.GetAttribute(2).Value.MessageValue));
                    attr.SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetMessageValue(digestList).Build());
                    break;
                case 1: //D3.GameMessage.GetAccountDigest -> D3.Account.Digest
                    var accountDigest = GetAccountDigest();
                    attr.SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetMessageValue(accountDigest).Build());
                    break;
                case 2: //CreateHero() -> D3.OnlineService.EntityId
                    var newToon = CreateHero(D3.OnlineService.HeroCreateParams.ParseFrom(request.GetAttribute(2).Value.MessageValue));
                    attr.SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetMessageValue(newToon).Build());
                    break;
                case 3: //D3.OnlineService.EntityId -> No return        Why have D3.GameMessage.DeleteHero and not use it?
                    DeleteHero(D3.OnlineService.EntityId.ParseFrom(request.GetAttribute(2).Value.MessageValue));
                    break;
                case 4: //SelectToon() -> D3.OnlineService.EntityId
                    var selectToon = SelectHero(D3.OnlineService.EntityId.ParseFrom(request.GetAttribute(2).Value.MessageValue));
                    //attr.SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetMessageValue(selectToon).Build());
                    break;
                case 5: //D3.GameMessages.SaveBannerConfiguration -> return MessageId with no Message
                    SaveBanner(D3.GameMessage.SaveBannerConfiguration.ParseFrom(request.GetAttribute(2).Value.MessageValue));
                    var attrId = bnet.protocol.attribute.Attribute.CreateBuilder()
                        .SetName("CustomMessageId")
                        .SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetIntValue(5).Build())
                        .Build();
                    builder.AddAttribute(attrId);
                    break;
                case 7: // Client doesn't care what you send here
                    attr.SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetMessageValue(ByteString.Empty).Build());
                    break;
                case 8: //D3.GameMessage.SetGameAccountSettings ->
                    var setAccountSettings = SetGameAccountSettings(D3.GameMessage.SetGameAccountSettings.ParseFrom(request.GetAttribute(2).Value.MessageValue));
                    attr.SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetMessageValue(setAccountSettings).Build());
                    break;
                case 9: //D3.GameMessage.GetToonSettings? -> D3.Client.ToonSettings
                    var getToonSettings = GetToonSettings(D3.GameMessage.GetToonSettings.ParseFrom(request.GetAttribute(2).Value.MessageValue));
                    attr.SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetMessageValue(getToonSettings).Build());
                    break;
                case 10: //D3.GameMessage.SetToonSettings?
                    var setToonSettings = SetToonSettings(D3.GameMessage.SetToonSettings.ParseFrom(request.GetAttribute(2).Value.MessageValue));
                    attr.SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetMessageValue(setToonSettings).Build());
                    break;
                case 12: //D3.GameMessage.GetHeroItems
                    var heroItems = GetHeroItems(D3.GameMessage.GetHeroItems.ParseFrom(request.GetAttribute(2).Value.MessageValue));
                    attr.SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetMessageValue(heroItems).Build());
                    break;
                case 13: //D3.GameMessage.GetAccountItems
                    var accountItems = GetAccountItems(D3.GameMessage.GetAccountItems.ParseFrom(request.GetAttribute(2).Value.MessageValue));
                    attr.SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetMessageValue(accountItems).Build());
                    var attr2 = bnet.protocol.attribute.Attribute.CreateBuilder().SetName("stashSlots");
                    attr2.SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetUintValue(14).Build());
                    builder.AddAttribute(attr2);
                    break;
                case 14: //D3.GameMessage.GetAccountProfile -> D3.Profile.AccountProfile
                    var getAccountProfile = GetAccountProfile(D3.GameMessage.GetAccountProfile.ParseFrom(request.GetAttribute(2).Value.MessageValue));
                    attr.SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetMessageValue(getAccountProfile).Build());
                    break;
                case 15: //D3.GameMessage.GetHeroProfiles -> D3.Profile.HeroProfileList
                    var getHeroProfiles = GetHeroProfiles(D3.GameMessage.GetHeroProfiles.ParseFrom(request.GetAttribute(2).Value.MessageValue));
                    attr.SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetMessageValue(getHeroProfiles).Build());
                    break;
                case 16: //? - Client expecting D3.Client.Preferences
                    attr.SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetMessageValue(D3.Client.Preferences.CreateBuilder().SetVersion(106).Build().ToByteString()).Build());
                    break;
                case 19: //D3.GameMessage.GetHeroIds -> D3.Hero.HeroList
                    var HeroList = GetHeroList(D3.GameMessage.GetHeroIds.ParseFrom(request.GetAttribute(2).Value.MessageValue));
                    attr.SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetMessageValue(HeroList).Build());
                    break;
                case 20: //D3.GameMessage.UndeleteHero -> D3.Hero.Digest
                    var UndeletedHero = UndeleteHero(D3.GameMessage.UndeleteHero.ParseFrom(request.GetAttribute(2).Value.MessageValue));
                    attr.SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetMessageValue(UndeletedHero).Build());
                    break;
                case 21: //D3.GameMessage.? (contains GameAccount EntityId) -> empty CustomMessage ByteString and CustomMessage2 ByteString
                    attr.SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetMessageValue(ByteString.Empty).Build());
                    var CustomMessage2 = bnet.protocol.attribute.Attribute.CreateBuilder().SetName("CustomMessage2");
                    CustomMessage2.SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetMessageValue(ByteString.Empty).Build());
                    break;
                case 23: //D3.GameMessage.GetDeletedHero -> D3.Hero.Digest or header status: 395003
                    var DeletedHero = GetDeletedHero();
                    if (DeletedHero != ByteString.Empty)
                        attr.SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetMessageValue(DeletedHero).Build());
                    else
                        this.Status = 395003;
                    break;
                default:
                    Logger.Warn("Unknown CustomMessageId {0}: {1}", MessageId, request.AttributeCount > 2 ? request.GetAttribute(2).Value.ToString() : "No CustomMessage?");
                    break;
            }
            if (attr.HasValue)
            {
                attr.SetName("CustomMessage");
                builder.AddAttribute(attr.Build());
            }

            done(builder.Build());
        }

        public override void PresenceChannelCreated(IRpcController controller, bnet.protocol.game_utilities.PresenceChannelCreatedRequest request, Action<bnet.protocol.NoData> done)
        {
            throw new NotImplementedException();
        }

        public override void GetPlayerVariables(IRpcController controller, bnet.protocol.game_utilities.PlayerVariablesRequest request, Action<bnet.protocol.game_utilities.VariablesResponse> done)
        {
            throw new NotImplementedException();
        }

        public override void GetGameVariables(IRpcController controller, bnet.protocol.game_utilities.GameVariablesRequest request, Action<bnet.protocol.game_utilities.VariablesResponse> done)
        {
            throw new NotImplementedException();
        }

        public override void GetLoad(IRpcController controller, bnet.protocol.server_pool.GetLoadRequest request, Action<bnet.protocol.server_pool.ServerState> done)
        {
            throw new NotImplementedException();
        }

        public override void ProcessServerRequest(IRpcController controller, bnet.protocol.game_utilities.ServerRequest request, Action<bnet.protocol.game_utilities.ServerResponse> done)
        {
            throw new NotImplementedException();
        }

        public override void NotifyGameAccountOffline(IRpcController controller, bnet.protocol.game_utilities.GameAccountOfflineNotification request, Action<bnet.protocol.NO_RESPONSE> done)
        {
            throw new NotImplementedException();
        }

        public override void NotifyGameAccountOnline(IRpcController controller, bnet.protocol.game_utilities.GameAccountOnlineNotification request, Action<bnet.protocol.NO_RESPONSE> done)
        {
            throw new NotImplementedException();
        }

        private ByteString GetHeroDigestList(D3.GameMessage.HeroDigestListRequest request)
        {
            Logger.Trace("GetHeroDigestList()");

            var ListResponse = D3.GameMessage.HeroDigestListResponse.CreateBuilder();
            foreach (var toon in request.ToonIdList)
            {
                var digest = ToonManager.GetToonByLowID(toon.IdLow).Digest;
                ListResponse.AddDigestList(
                D3.GameMessage.HeroDigestResponse.CreateBuilder()
                    .SetToonId(toon)
                    .SetSuccess(true)
                    .SetHeroDigest(digest)
                    .Build()
                    );
            }
            return ListResponse.Build().ToByteString();
        }

        private ByteString GetAccountDigest()
        {
            Logger.Trace("GetAccountDigest()");

            return this.Client.Account.CurrentGameAccount.Digest.ToByteString();
        }

        private ByteString CreateHero(D3.OnlineService.HeroCreateParams createPrams)
        {

            var newToon = ToonManager.CreateNewToon(createPrams.Name, createPrams.GbidClass, createPrams.IsFemale ? ToonFlags.Female : ToonFlags.Male, 1, Client.Account.CurrentGameAccount);
            return newToon.D3EntityID.ToByteString();
        }

        private void DeleteHero(D3.OnlineService.EntityId hero)
        {
            //Permanantly delete previously deleted hero
            var deleteHero = ToonManager.GetDeletedToon(this.Client.Account.CurrentGameAccount);
            if (deleteHero != null)
            {
                Logger.Trace("Permanantly deleting hero: {0}", deleteHero);
                ToonManager.DeleteToon(deleteHero);
            }
            //Mark hero as deleted
            var markHero = ToonManager.GetToonByLowID(hero.IdLow);
            markHero.Deleted = true;
            ToonManager.SaveToDB(markHero);

            Logger.Trace("DeleteHero(): Marked {0} as deleted.", markHero);
        }

        private ByteString SelectHero(D3.OnlineService.EntityId hero)
        {
            var oldToon = this.Client.Account.CurrentGameAccount.CurrentToon;
            var newtoon = ToonManager.GetToonByLowID(hero.IdLow);


            Logger.Trace("SelectToon() {0}", this.Client.Account.CurrentGameAccount.CurrentToon);

            if (oldToon != newtoon)
            {
                this.Client.Account.CurrentGameAccount.CurrentToon = newtoon;
                this.Client.Account.CurrentGameAccount.NotifyUpdate();
                AccountManager.SaveToDB(this.Client.Account);
            }
            return this.Client.Account.CurrentGameAccount.CurrentToon.D3EntityID.ToByteString();
        }

        private bool SaveBanner(D3.GameMessage.SaveBannerConfiguration bannerConfig)
        {
            Logger.Trace("SaveBannerConfiguration()");

            if (this.Client.Account.CurrentGameAccount.BannerConfigurationField.Value == bannerConfig.Banner)
                return false;
            else
            {
                this.Client.Account.CurrentGameAccount.BannerConfiguration = bannerConfig.Banner;
                this.Client.Account.CurrentGameAccount.NotifyUpdate();
            }
            return true;
        }

        private ByteString GetGameAccountSettings(D3.GameMessage.GetGameAccountSettings settings)
        {
            Logger.Trace("GetGameAccountSettings()");

            var gameAccount = this.Client.Account.CurrentGameAccount;
            return gameAccount.Settings.ToByteString();
            //var pref = D3.Client.Preferences.CreateBuilder().SetVersion(105).Build(); //hack since client is expecting this atm -Egris
            //return pref.ToByteString();
        }

        private ByteString SetGameAccountSettings(D3.GameMessage.SetGameAccountSettings settings)
        {
            Logger.Trace("SetGameAccountSettings()");

            this.Client.Account.CurrentGameAccount.Settings = settings.Settings;
            return ByteString.Empty;
        }

        private ByteString GetToonSettings(D3.GameMessage.GetToonSettings settings)
        {
            Logger.Trace("GetToonSettings");

            if (settings.HasHeroId)
            {
                var toon = ToonManager.GetToonByLowID(settings.HeroId.IdLow);
                return toon.Settings.ToByteString();
            }
            else
                return this.Client.Account.CurrentGameAccount.CurrentToon.Settings.ToByteString();
        }

        private ByteString SetToonSettings(D3.GameMessage.SetToonSettings settings)
        {
            Logger.Trace("SetToonSettings()");

            var toon = ToonManager.GetToonByLowID(settings.HeroId.IdLow);
            toon.Settings = settings.Settings;
            return ByteString.Empty;
        }

        private ByteString GetHeroItems(D3.GameMessage.GetHeroItems request)
        {
            Logger.Trace("GetHeroItems()");

            var itemList = D3.Items.ItemList.CreateBuilder();
            return itemList.Build().ToByteString();
        }

        private ByteString GetAccountItems(D3.GameMessage.GetAccountItems request)
        {
            Logger.Trace("GetAccountItems()");

            var itemList = D3.Items.ItemList.CreateBuilder();
            return itemList.Build().ToByteString();
        }

        private ByteString GetAccountProfile(D3.GameMessage.GetAccountProfile profile)
        {
            Logger.Trace("GetAccountProfile()");

            var account = GameAccountManager.GetAccountByPersistentID(profile.AccountId.IdLow);
            return account.Profile.ToByteString();
        }

        private ByteString GetHeroProfiles(D3.GameMessage.GetHeroProfiles profiles)
        {
            Logger.Trace("GetHeroProfiles()");

            var profileList = D3.Profile.HeroProfileList.CreateBuilder();
            if (profiles.HeroIdsCount > 0)
            {
                foreach (var hero in profiles.HeroIdsList)
                {
                    var toon = ToonManager.GetToonByLowID(hero.IdLow);
                    profileList.AddHeros(toon.Profile);
                }
            }
            else
            {
                var heroList = GameAccountManager.GetAccountByPersistentID(profiles.AccountId.IdLow).Toons;
                foreach (var hero in heroList)
                {
                    profileList.AddHeros(hero.Profile);
                }
            }

            return profileList.Build().ToByteString();
        }

        private ByteString GetHeroList(D3.GameMessage.GetHeroIds heroIds)
        {
            Logger.Trace("GetHeroList()");

            var HeroList = D3.Hero.HeroList.CreateBuilder();
            var gameAccount = GameAccountManager.GetAccountByPersistentID(heroIds.AccountId.IdLow);
            foreach (var toon in gameAccount.Toons)
            {
                if (!toon.Deleted)
                    HeroList.AddHeroIds(toon.D3EntityID);
            }
            return HeroList.Build().ToByteString();
        }

        private ByteString UndeleteHero(D3.GameMessage.UndeleteHero heroId)
        {
            var toon = ToonManager.GetToonByLowID(heroId.UndeleteHeroId.IdLow);
            if (toon != null && toon.Deleted)
            {
                toon.Deleted = false;
                ToonManager.SaveToDB(toon);
                return toon.Digest.ToByteString();
            }
            else
                return ByteString.Empty;
        }
        private ByteString GetDeletedHero()
        {
            Logger.Trace("GetDeletedHero()");
            foreach (var toon in this.Client.Account.CurrentGameAccount.Toons)
            {
                if (toon.Deleted)
                    return toon.Digest.ToByteString();
            }
            return ByteString.Empty;
        }
    }
}
