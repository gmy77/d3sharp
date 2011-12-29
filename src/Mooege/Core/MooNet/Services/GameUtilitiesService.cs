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
using Google.ProtocolBuffers;
using Mooege.Common.Logging;
using Mooege.Net.MooNet;
using Mooege.Core.MooNet.Toons;
using Mooege.Core.MooNet.Accounts;

namespace Mooege.Core.MooNet.Services
{
    [Service(serviceID: 0x8, serviceName: "bnet.protocol.game_utilities.GameUtilities")]
    public class GameUtilitiesService : bnet.protocol.game_utilities.GameUtilities,IServerService
    {
        private static readonly Logger Logger = LogManager.CreateLogger();
        public MooNetClient Client { get; set; }
        public bnet.protocol.Header LastCallHeader { get; set; }

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
                    var ListResponse = D3.GameMessage.HeroDigestListResponse.CreateBuilder();
                    foreach (var toon in D3.GameMessage.HeroDigestListRequest.ParseFrom(request.GetAttribute(2).Value.MessageValue).ToonIdList)
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
                    attr.SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetMessageValue(ListResponse.Build().ToByteString()).Build());
                    break;
                case 1: //D3.GameMessage.GetAccountDigest -> D3.Account.Digest
                    attr.SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetMessageValue(this.Client.CurrentGameAccount.Digest.ToByteString()).Build());
                    break;
                case 2: //CreateHero()->D3.OnlineService.EntityId
                    var heroCreateParams = D3.OnlineService.HeroCreateParams.ParseFrom(request.GetAttribute(2).Value.MessageValue);
                    int hashCode = ToonManager.GetUnusedHashCodeForToonName(heroCreateParams.Name);
                    var newToon = new Toon(heroCreateParams.Name, hashCode, heroCreateParams.GbidClass, heroCreateParams.IsFemale ? ToonFlags.Female : ToonFlags.Male, 1, Client.CurrentGameAccount);
                    if (ToonManager.SaveToon(newToon))
                    {
                        Logger.Trace("CreateHero() {0}", newToon);
                        attr.SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetMessageValue(newToon.D3EntityID.ToByteString()).Build());
                    }
                    break;
                case 3: //D3.GameMessage.DeleteHero->?
                    var deleteToonId = D3.OnlineService.EntityId.ParseFrom(request.GetAttribute(2).Value.MessageValue);
                    var deleteToon = ToonManager.GetToonByLowID(deleteToonId.IdLow);
                    ToonManager.DeleteToon(deleteToon);
                    Logger.Trace("DeleteHero() {0}", deleteToon);
                    break;
                case 4: //SelectToon()->D3.OnlineService.EntityId
                    var selectToon = D3.OnlineService.EntityId.ParseFrom(request.GetAttribute(2).Value.MessageValue);
                    this.Client.CurrentToon = ToonManager.GetToonByLowID(selectToon.IdLow);
                    attr.SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetMessageValue(this.Client.CurrentToon.D3EntityID.ToByteString()).Build());
                    //this.Client.CurrentChannel = new Channels.Channel(this.Client);
                    Logger.Trace("SelectToon() {0}", this.Client.CurrentToon);
                    break;
                case 5: //D3.GameMessages.SaveBannerConfiguration -> return MessageId with no Message
                    var bannerConfig = D3.GameMessage.SaveBannerConfiguration.ParseFrom(request.GetAttribute(2).Value.MessageValue);
                    this.Client.CurrentGameAccount.BannerConfiguration = bannerConfig.Banner;
                    var attrId = bnet.protocol.attribute.Attribute.CreateBuilder()
                        .SetName("CustomMessageId")
                        .SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetIntValue(5).Build())
                        .Build();
                    builder.AddAttribute(attrId);
                    Logger.Trace("SaveBannerConifuration()");
                    break;
                case 8: //D3.GameMessage.GetGameAccountSettings? - Client expecting D3.Client.Preferences
                    //var settings = D3.Client.GameAccountSettings.CreateBuilder().SetRmtLastUsedCurrency("PTR").Build();
                    var pref = D3.Client.Preferences.CreateBuilder().SetVersion(0).Build();
                    attr.SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetMessageValue(pref.ToByteString()).Build());
                    break;
                case 9: //D3.GameMessage.SetGameAccountSettings ->
                    this.Client.CurrentGameAccount.Settings = D3.GameMessage.SetGameAccountSettings.CreateBuilder().MergeFrom(request.GetAttribute(2).Value.MessageValue).Build();
                    break;
                case 10: //D3.GameMessage.GetToonSettings? -> D3.Client.ToonSettings
                    var settings = D3.Client.ToonSettings.CreateBuilder().Build();
                    attr.SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetMessageValue(settings.ToByteString()).Build());
                    break;
                case 11: //D3.GameMessage.SetToonSettings?
                    attr.SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetMessageValue(ByteString.Empty).Build());
                    break;
                case 15: //D3.GameMessage.GetAccountProfile -> D3.Profile.AccountProfile
                    var profile = D3.Profile.AccountProfile.CreateBuilder().Build(); //Todo: Load AccountProfile from GameAccount
                    attr.SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetMessageValue(profile.ToByteString()).Build());
                    break;
                case 20: //D3.GameMessage.GetHeroIds->D3.Hero.HeroList
                    var HeroList = D3.Hero.HeroList.CreateBuilder();
                    var gameAccount = D3.GameMessage.GetHeroIds.ParseFrom(request.GetAttribute(2).Value.MessageValue).AccountId;
                    foreach (var toon in this.Client.CurrentGameAccount.Toons.Values)
                    {
                        HeroList.AddHeroIds(toon.D3EntityID);
                    }
                    attr.SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetMessageValue(HeroList.Build().ToByteString()).Build());
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
    }
}
