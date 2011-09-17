using D3Sharp.Net;
using D3Sharp.Net.Packets;

namespace D3Sharp.Core.Services
{
    [Service(serviceID: 0x7, serverHash: 0x810CB195, clientHash: 0x0)]
    public class GameMasterService : Service
    {
        [ServiceMethod(0x2)]
        public void ListFactoriesRequest(IClient client, Packet packetIn)
        {
            Logger.Trace("RPC:GameMaster:ListFactoriesRequest()");
            //var reqb = bnet.protocol.game_master.ListFactoriesRequest.ParseFrom(packetIn.Payload.ToArray());
            var varib1 = bnet.protocol.attribute.Variant.CreateBuilder();
            varib1.SetIntValue(2);
            var vari1 = varib1.Build();
            var varib2 = bnet.protocol.attribute.Variant.CreateBuilder();
            varib2.SetIntValue(4);
            var vari2 = varib2.Build();
            var varib3 = bnet.protocol.attribute.Variant.CreateBuilder();
            varib3.SetIntValue(1);
            var vari3 = varib3.Build();
            var varib4 = bnet.protocol.attribute.Variant.CreateBuilder();
            varib4.SetStringValue("0.3.0");
            var vari4 = varib4.Build();
            var attrb1 = bnet.protocol.attribute.Attribute.CreateBuilder();
            attrb1.SetName("min_players");
            attrb1.SetValue(vari1);
            var attr1 = attrb1.Build();
            var attrb2 = bnet.protocol.attribute.Attribute.CreateBuilder();
            attrb2.SetName("max_players");
            attrb2.SetValue(vari2);
            var attr2 = attrb2.Build();
            var attrb3 = bnet.protocol.attribute.Attribute.CreateBuilder();
            attrb3.SetName("num_teams");
            attrb3.SetValue(vari3);
            var attr3 = attrb3.Build();
            var attrb4 = bnet.protocol.attribute.Attribute.CreateBuilder();
            attrb4.SetName("version");
            attrb4.SetValue(vari4);
            var attr4 = attrb4.Build();
            var statsb = bnet.protocol.game_master.GameStatsBucket.CreateBuilder();
            statsb.SetBucketMin(0);
            statsb.SetBucketMax(4.2949673e+009f);
            statsb.SetWaitMilliseconds(1000);
            statsb.SetGamesPerHour(0);
            statsb.SetActiveGames(50);
            statsb.SetActivePlayers(60);
            statsb.SetFormingGames(0);
            statsb.SetWaitingPlayers(0);
            var stats = statsb.Build();
            var factb = bnet.protocol.game_master.GameFactoryDescription.CreateBuilder(); // CoopFactoryID - 14249086168335147635 was value on bnet forum error log
            factb.SetId(14249086168335147635);
            factb.AddStatsBucket(stats);
            factb.AddAttribute(attr1);
            factb.AddAttribute(attr2);
            factb.AddAttribute(attr3);
            factb.AddAttribute(attr4);
            var fact = factb.Build();
            var respb = bnet.protocol.game_master.ListFactoriesResponse.CreateBuilder();
            respb.SetTotalResults(1);
            respb.AddDescription(fact);
            var response = respb.Build(); // Seems rescount is optional

            var packet = new Packet(
                new Header(0xfe, 0x0, packetIn.Header.RequestID, (uint)response.SerializedSize),
                response.ToByteArray());
            client.Send(packet);
        }
    }
}
