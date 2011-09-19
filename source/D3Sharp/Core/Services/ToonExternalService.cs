using System;
using System.Linq;
using D3Sharp.Core.Channels;
using D3Sharp.Net;
using D3Sharp.Net.Packets;
using D3Sharp.Utils;
using Google.ProtocolBuffers;
using bnet.protocol.toon.external;

namespace D3Sharp.Core.Services
{
    [Service(serviceID: 0x2, serviceName: "bnet.protocol.toon.external.ToonServiceExternal")]
    public class ToonExternalService : ToonServiceExternal, IServerService
    {
        protected static readonly Logger Logger = LogManager.CreateLogger();
        public IClient Client { get; set; }

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
            var toon = Toons.ToonManager.GetToon(request.Toon.Low);
            this.Client.CurrentToon = toon;
            done(builder.Build());
        }

        public override void CreateToon(Google.ProtocolBuffers.IRpcController controller, CreateToonRequest request, Action<CreateToonResponse> done)
        {
            Logger.Trace("CreateToon()");
            var heroCreateParams = D3.OnlineService.HeroCreateParams.ParseFrom(request.AttributeList[0].Value.MessageValue);
            var builder = CreateToonResponse.CreateBuilder();

            var toon = new Toons.Toon(request.Name, (uint)heroCreateParams.GbidClass, heroCreateParams.IsFemale ? Toons.ToonGender.Female : Toons.ToonGender.Male, 1, (long)Client.Account.ID);
            if (Toons.ToonManager.SaveToon(toon)) builder.SetToon(toon.BnetEntityID);
            done(builder.Build());
        }

        public override void DeleteToon(Google.ProtocolBuffers.IRpcController controller, DeleteToonRequest request, Action<DeleteToonResponse> done)
        {
            Logger.Trace("DeleteToon()");
            
            var id = request.Toon.Low;
            var toon = Toons.ToonManager.GetToon(id);
            Toons.ToonManager.DeleteToon(toon);

            var builder = bnet.protocol.toon.external.DeleteToonResponse.CreateBuilder();
            done(builder.Build());
        }


        [ServiceMethod(0x4)]
        public void DeleteToon4(IClient client, Packet packetIn)
        {
            Logger.Trace("RPC:ToonExternal:DeleteToon(4)");
            var request = bnet.protocol.toon.external.DeleteToonRequest.ParseFrom(packetIn.Payload.ToArray());

            Core.Toons.ToonManager.DeleteToon(request.Toon.Low);

            var response = bnet.protocol.toon.external.DeleteToonResponse.CreateBuilder().Build();
            var packet = new Packet(
                new Header(0xfe, 0x0, packetIn.Header.RequestID, (uint)response.SerializedSize),
                response.ToByteArray());

            client.Send(packet);
        }

        [ServiceMethod(0x5)]
        public void DeleteToon5(IClient client, Packet packetIn)
        {
            Logger.Trace("RPC:ToonExternal:DeleteToon(5)");
            var response = bnet.protocol.toon.external.DeleteToonResponse.CreateBuilder().Build();

            var packet = new Packet(
                new Header(0xfe, 0x0, packetIn.Header.RequestID, (uint)response.SerializedSize),
                response.ToByteArray());

            client.Send(packet);
        }
    }
}
