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
                new Header(new byte[] { 0xfe, 0x0, (byte)packetIn.Header.RequestID, 0x0, (byte)response.SerializedSize }),
                response.ToByteArray());

            Logger.Debug("RPC:Storage:OpenTableRequest()");
            client.Send(packet);
        }

        [ServiceMethod(0x3)]
        public void OpenColumnRequest(IClient client, Packet packetIn)
        {
            var response = bnet.protocol.storage.OpenColumnResponse.CreateBuilder().Build();

            var packet = new Packet(
                new Header(new byte[] { 0xfe, 0x0, (byte)packetIn.Header.RequestID, 0x0, (byte)response.SerializedSize }),
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
            }                

            var packet = new Packet(
                new Header(new byte[] { 0xfe, 0x0, (byte)packetIn.Header.RequestID, 0x0, (byte)response.SerializedSize }),
                response.ToByteArray());

            Logger.Debug("RPC:Storage:ExecuteRequest()");
            client.Send(packet);
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
                

            foreach (var operation in request.OperationsList)
            {
                var operationResult = bnet.protocol.storage.OperationResult.CreateBuilder()
                    .SetTableId(operation.TableId)
                    .AddData(
                        bnet.protocol.storage.Cell.CreateBuilder()
                            .SetColumnId(operation.ColumnId)
                            .SetRowId(operation.RowId)
                            .SetVersion(1)
                            .SetData(accountDigest.ToByteString())
                            .Build()).Build();

                builder.AddResults(operationResult);
            }

            return builder.Build();
        }

        private bnet.protocol.storage.ExecuteResponse GameAccountSettings(bnet.protocol.storage.ExecuteRequest request)
        {
            var builder = bnet.protocol.storage.ExecuteResponse.CreateBuilder();

            foreach (var operation in request.OperationsList)
            {
                var operationResult = bnet.protocol.storage.OperationResult.CreateBuilder()
                    .SetTableId(operation.TableId)
                    .AddData(
                        bnet.protocol.storage.Cell.CreateBuilder()
                            .SetColumnId(operation.ColumnId)
                            .SetRowId(operation.RowId)
                            .Build()).Build();

                builder.AddResults(operationResult);
            }

            return builder.Build();
        }
    }
}
