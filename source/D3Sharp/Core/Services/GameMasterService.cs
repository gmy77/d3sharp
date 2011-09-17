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

            var description = bnet.protocol.game_master.GameFactoryDescription.CreateBuilder().SetId(14249086168335147635);

            var atributes = new bnet.protocol.attribute.Attribute[4]
                                {
                                    bnet.protocol.attribute.Attribute.CreateBuilder().SetName("min_players").SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetIntValue(2)).Build(),
                                    bnet.protocol.attribute.Attribute.CreateBuilder().SetName("max_players").SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetIntValue(4)).Build(),
                                    bnet.protocol.attribute.Attribute.CreateBuilder().SetName("num_teams").SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetIntValue(1)).Build(),
                                    bnet.protocol.attribute.Attribute.CreateBuilder().SetName("version").SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetStringValue("0.3.0")).Build()
                                };

            description.AddRangeAttribute(atributes);
            description.AddStatsBucket(bnet.protocol.game_master.GameStatsBucket.CreateBuilder()
                                           .SetBucketMin(0)
                                           .SetBucketMax(4294967296F)
                                           .SetWaitMilliseconds(1354)
                                           .SetGamesPerHour(0)
                                           .SetActiveGames(1)
                                           .SetActivePlayers(1)
                                           .SetFormingGames(0)
                                           .SetWaitingPlayers(0).Build());

            var response = bnet.protocol.game_master.ListFactoriesResponse.CreateBuilder().AddDescription(description).SetTotalResults(1).Build();

            var packet = new Packet(
                new Header(0xfe, 0x0, packetIn.Header.RequestID, (uint)response.SerializedSize),
                response.ToByteArray());

            client.Send(packet);
        }
    }
}
