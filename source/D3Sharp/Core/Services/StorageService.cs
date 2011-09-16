using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using D3Sharp.Net;
using D3Sharp.Net.Packets;
using Google.ProtocolBuffers;

namespace D3Sharp.Core.Services
{
    [Service(serviceID: 0x9, serverHash: 0xDA6E4BB9, clientHash: 0x0)]
    public class StorageService : Service
    {
        [ServiceMethod(0x2)]
        public void OpenTableRequest(IClient client, Packet packetIn)
        {
            var response = bnet.protocol.storage.OpenTableResponse.CreateBuilder().Build();

            var packet = new Packet(
                new Header(0xfe, 0x0, packetIn.Header.RequestID, (uint)response.SerializedSize),
                response.ToByteArray());

            Logger.Debug("RPC:Storage:OpenTableRequest()");
            client.Send(packet);
        }

        [ServiceMethod(0x3)]
        public void OpenColumnRequest(IClient client, Packet packetIn)
        {
            var response = bnet.protocol.storage.OpenColumnResponse.CreateBuilder().Build();

            var packet = new Packet(
                new Header(0xfe, 0x0, packetIn.Header.RequestID, (uint)response.SerializedSize),
                response.ToByteArray());

            Logger.Debug("RPC:Storage:OpenColumnRequest()");
            client.Send(packet);
        }

        [ServiceMethod(0x1)]
        public void ExecuteRequest(IClient client, Packet packetIn)
        {
            var request = bnet.protocol.storage.ExecuteRequest.CreateBuilder().MergeFrom(packetIn.Payload.ToArray()).Build();

            bnet.protocol.storage.ExecuteResponse response = null;
            switch (request.QueryName)
            {
                case "GetGameAccountSettings":
                    response = GameAccountSettings(request);
                    break;
                case "LoadAccountDigest":
                    response = LoadAccountDigest(request);
                    break;
                case "GetHeroDigests":
                    response = GetHeroDigest(request);
                    break;
                default:
                    break;
            }                
            
            var packet = new Packet(
                new Header(0xfe, 0x0, packetIn.Header.RequestID, (uint)response.SerializedSize),
                response.ToByteArray());

            Logger.Debug("RPC:Storage:ExecuteRequest()");
            client.Send(packet);
        }

        private bnet.protocol.storage.ExecuteResponse GetHeroDigest(bnet.protocol.storage.ExecuteRequest request)
        {
            var builder = bnet.protocol.storage.ExecuteResponse.CreateBuilder();

            var equipment = D3.Hero.VisualEquipment.CreateBuilder().Build();
            //var questhistory = D3.Hero.QuestHistoryEntry.CreateBuilder().Build();

            var heroDigest = D3.Hero.Digest.CreateBuilder().SetVersion(1)
                .SetHeroId(D3.OnlineService.EntityId.CreateBuilder().SetIdHigh(0x300016200004433).SetIdLow(1).Build())
                .SetHeroName("testhero")
                .SetGbidClass(1)
                .SetLevel(5)
                .SetPlayerFlags(0)
                .SetVisualEquipment(equipment)
                //.SetQuestHistory(0, questhistory)
                .SetLastPlayedAct(0)
                .SetHighestUnlockedAct(0)
                .SetLastPlayedDifficulty(0)
                .SetHighestUnlockedDifficulty(0)
                .SetLastPlayedQuest(1)
                .SetLastPlayedQuestStep(1)
                .SetTimePlayed(0).Build();

            var operationResult = bnet.protocol.storage.OperationResult.CreateBuilder()
                .SetTableId(request.OperationsList[0].TableId)
                .AddData(
                    bnet.protocol.storage.Cell.CreateBuilder()
                        .SetColumnId(request.OperationsList[0].ColumnId)
                        .SetRowId(request.OperationsList[0].RowId)
                        .SetVersion(1)
                        .SetData(heroDigest.ToByteString())
                        .Build()).Build();

            builder.AddResults(operationResult);
            return builder.Build();
        }

        private bnet.protocol.storage.ExecuteResponse LoadAccountDigest(bnet.protocol.storage.ExecuteRequest request)
        {
            var builder = bnet.protocol.storage.ExecuteResponse.CreateBuilder();

            var accountDigest = D3.Account.Digest.CreateBuilder().SetVersion(1)
                .SetLastPlayedHeroId(D3.OnlineService.EntityId.CreateBuilder().SetIdHigh(0).SetIdLow(0).Build())
                .SetBannerConfiguration(D3.Account.BannerConfiguration.CreateBuilder()
                                            .SetBackgroundColorIndex(0)
                                            .SetBannerIndex(0)
                                            .SetPattern(0)
                                            .SetPatternColorIndex(0)
                                            .SetPlacementIndex(0)
                                            .SetSigilAccent(0)
                                            .SetSigilMain(0)
                                            .SetSigilColorIndex(0)
                                            .SetUseSigilVariant(false)
                                            .Build())
                .SetFlags(0)
                .Build();

            var operationResult = bnet.protocol.storage.OperationResult.CreateBuilder()
                .SetTableId(request.OperationsList[0].TableId)
                .AddData(
                    bnet.protocol.storage.Cell.CreateBuilder()
                        .SetColumnId(request.OperationsList[0].ColumnId)
                        .SetRowId(request.OperationsList[0].RowId)
                        .SetVersion(1)
                        .SetData(accountDigest.ToByteString())
                        .Build()).Build();

            builder.AddResults(operationResult);
            return builder.Build();
        }

        private bnet.protocol.storage.ExecuteResponse GameAccountSettings(bnet.protocol.storage.ExecuteRequest request)
        {
            var builder = bnet.protocol.storage.ExecuteResponse.CreateBuilder();


            var operationResult = bnet.protocol.storage.OperationResult.CreateBuilder()
                .SetTableId(request.OperationsList[0].TableId)
                .AddData(
                    bnet.protocol.storage.Cell.CreateBuilder()
                        .SetColumnId(request.OperationsList[0].ColumnId)
                        .SetRowId(request.OperationsList[0].RowId)
                        .Build()).Build();

            builder.AddResults(operationResult);
            return builder.Build();
        }
    }
}
