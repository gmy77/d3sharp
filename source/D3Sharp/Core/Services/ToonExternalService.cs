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
using D3Sharp.Net.BNet;
using D3Sharp.Utils;
using bnet.protocol.toon.external;

namespace D3Sharp.Core.Services
{
    [Service(serviceID: 0x2, serviceName: "bnet.protocol.toon.external.ToonServiceExternal")]
    public class ToonExternalService : ToonServiceExternal, IServerService
    {
        protected static readonly Logger Logger = LogManager.CreateLogger();
        public IBNetClient Client { get; set; }

        public override void ToonList(Google.ProtocolBuffers.IRpcController controller, ToonListRequest request, Action<ToonListResponse> done)
        {
            Logger.Trace("ToonList()");
            var builder = ToonListResponse.CreateBuilder();

            if (Client.Account.Toons.Count > 0)
            {
                foreach (var pair in Client.Account.Toons)
                {
                    builder.AddToons(pair.Value.BnetEntityID);
                }
            }

            done(builder.Build());
        }

        public override void SelectToon(Google.ProtocolBuffers.IRpcController controller, SelectToonRequest request, Action<SelectToonResponse> done)
        {
            Logger.Trace("SelectToon()");
            
            var builder = SelectToonResponse.CreateBuilder();
            var toon = Toons.ToonManager.GetToonByLowID(request.Toon.Low);
            this.Client.CurrentToon = toon;
            done(builder.Build());
        }

        public override void CreateToon(Google.ProtocolBuffers.IRpcController controller, CreateToonRequest request, Action<CreateToonResponse> done)
        {
            Logger.Trace("CreateToon()");
            var heroCreateParams = D3.OnlineService.HeroCreateParams.ParseFrom(request.AttributeList[0].Value.MessageValue);
            var builder = CreateToonResponse.CreateBuilder();

            var toon = new Toons.Toon(request.Name, heroCreateParams.GbidClass, heroCreateParams.IsFemale ? Toons.ToonGender.Female : Toons.ToonGender.Male, 1, (long)Client.Account.Id);
            if (Toons.ToonManager.SaveToon(toon)) builder.SetToon(toon.BnetEntityID);
            done(builder.Build());
        }

        public override void DeleteToon(Google.ProtocolBuffers.IRpcController controller, DeleteToonRequest request, Action<DeleteToonResponse> done)
        {
            Logger.Trace("DeleteToon()");
            
            var id = request.Toon.Low;
            var toon = Toons.ToonManager.GetToonByLowID(id);
            Toons.ToonManager.DeleteToon(toon);

            var builder = bnet.protocol.toon.external.DeleteToonResponse.CreateBuilder();
            done(builder.Build());
        }
    }
}
