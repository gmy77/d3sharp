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
using Mooege.Common;
using Mooege.Core.Common.Toons;
using Mooege.Net.MooNet;
using bnet.protocol.toon.external;

namespace Mooege.Core.MooNet.Services
{
    [Service(serviceID: 0x2, serviceName: "bnet.protocol.toon.external.ToonServiceExternal")]
    public class ToonExternalService : ToonServiceExternal, IServerService
    {
        private static readonly Logger Logger = LogManager.CreateLogger();
        public IMooNetClient Client { get; set; }

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
            var toon = ToonManager.GetToonByLowID(request.Toon.Low);
            this.Client.CurrentToon = toon;
            done(builder.Build());
        }

        public override void CreateToon(Google.ProtocolBuffers.IRpcController controller, CreateToonRequest request, Action<CreateToonResponse> done)
        {
            Logger.Trace("CreateToon()");
            var heroCreateParams = D3.OnlineService.HeroCreateParams.ParseFrom(request.AttributeList[0].Value.MessageValue);
            var builder = CreateToonResponse.CreateBuilder();

            var toon = new Toon(request.Name, heroCreateParams.GbidClass, heroCreateParams.IsFemale ? ToonFlags.Female : ToonFlags.Male, 60, Client.Account);
            if (ToonManager.SaveToon(toon)) builder.SetToon(toon.BnetEntityID);
            done(builder.Build());
        }

        public override void DeleteToon(Google.ProtocolBuffers.IRpcController controller, DeleteToonRequest request, Action<DeleteToonResponse> done)
        {
            Logger.Trace("DeleteToon()");
            
            var id = request.Toon.Low;
            var toon = ToonManager.GetToonByLowID(id);
            ToonManager.DeleteToon(toon);

            var builder = bnet.protocol.toon.external.DeleteToonResponse.CreateBuilder();
            done(builder.Build());
        }
    }
}
