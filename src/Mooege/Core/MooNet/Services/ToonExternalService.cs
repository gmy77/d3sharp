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
using Mooege.Common.Logging;
using Mooege.Core.MooNet.Toons;
using Mooege.Net.MooNet;

namespace Mooege.Core.MooNet.Services
{
    [Service(serviceID: 0x2, serviceName: "bnet.protocol.toon.external.ToonServiceExternal")]
    public class ToonExternalService : bnet.protocol.toon.external.ToonServiceExternal, IServerService
    {
        private static readonly Logger Logger = LogManager.CreateLogger();
        public MooNetClient Client { get; set; }
        public bnet.protocol.Header LastCallHeader { get; set; }

        public override void ToonList(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.toon.external.ToonListRequest request, Action<bnet.protocol.toon.external.ToonListResponse> done)
        {
            Logger.Trace("ToonList() {0}", this.Client.Account);
            var builder = bnet.protocol.toon.external.ToonListResponse.CreateBuilder();

            if (Client.Account.Toons.Count > 0)
            {
                foreach (var pair in Client.Account.Toons)
                {
                    builder.AddToons(pair.Value.BnetEntityID);
                }
            }

            done(builder.Build());
        }

        public override void SelectToon(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.toon.external.SelectToonRequest request, Action<bnet.protocol.toon.external.SelectToonResponse> done)
        {
            var builder = bnet.protocol.toon.external.SelectToonResponse.CreateBuilder();
            var toon = ToonManager.GetToonByLowID(request.Toon.Low);
            this.Client.CurrentToon = toon;
            done(builder.Build());

            Logger.Trace("SelectToon() {0}", toon);
        }

        public override void CreateToon(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.toon.external.CreateToonRequest request, Action<bnet.protocol.toon.external.CreateToonResponse> done)
        {            
            var heroCreateParams = D3.OnlineService.HeroCreateParams.ParseFrom(request.AttributeList[0].Value.MessageValue);
            var builder = bnet.protocol.toon.external.CreateToonResponse.CreateBuilder();

            int hashCode = ToonManager.GetUnusedHashCodeForToonName(request.Name);
            var toon = new Toon(request.Name, hashCode, heroCreateParams.GbidClass, heroCreateParams.IsFemale ? ToonFlags.Female : ToonFlags.Male, 1, Client.Account);
            if (ToonManager.SaveToon(toon))
                builder.SetToken((uint)toon.BnetEntityID.Low);
            done(builder.Build());

            var notification = bnet.protocol.toon.external.ToonCreatedNotification.CreateBuilder()
                .SetToon(toon.BnetEntityID)
                .SetToken((uint) toon.BnetEntityID.Low)
                .SetErrorCode(0).Build();

            this.Client.MakeRPC(() => bnet.protocol.toon.external.ToonNotifyExternal.CreateStub(this.Client).NotifyToonCreated(null, notification, callback => { }));

            Logger.Trace("CreateToon() {0}", toon);
        }

        public override void DeleteToon(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.toon.external.DeleteToonRequest request, Action<bnet.protocol.toon.external.DeleteToonResponse> done)
        {            
            var id = request.Toon.Low;
            var toon = ToonManager.GetToonByLowID(id);
            ToonManager.DeleteToon(toon);

            var builder = bnet.protocol.toon.external.DeleteToonResponse.CreateBuilder();
            done(builder.Build());

            Logger.Trace("DeleteToon() {0}",toon);
        }
    }
}
