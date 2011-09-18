using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using D3Sharp.Core.Toons;
using D3Sharp.Net;
using D3Sharp.Net.Packets;
using D3Sharp.Utils.Extensions;
using D3Sharp.Core.Storage;
using Google.ProtocolBuffers;
using Gibbed.Helpers;

namespace D3Sharp.Core.Services
{
    [Service(serviceID: 0x9, serviceName: "bnet.protocol.storage.StorageService", clientHash: 0x0)]
    public class StorageService : Service
    {
        [ServiceMethod(0x2)]
        public void OpenTable(IClient client, Packet packetIn)
        {
            Logger.Trace("RPC:Storage:OpenTable()");
            var response = bnet.protocol.storage.OpenTableResponse.CreateBuilder().Build();

            var packet = new Packet(
                new Header(0xfe, 0x0, packetIn.Header.RequestID, (uint)response.SerializedSize),
                response.ToByteArray());

            client.Send(packet);
        }

        [ServiceMethod(0x3)]
        public void OpenColumn(IClient client, Packet packetIn)
        {
            Logger.Trace("RPC:Storage:OpenColumn()");
            var response = bnet.protocol.storage.OpenColumnResponse.CreateBuilder().Build();

            var packet = new Packet(
                new Header(0xfe, 0x0, packetIn.Header.RequestID, (uint)response.SerializedSize),
                response.ToByteArray());

            client.Send(packet);
        }

        [ServiceMethod(0x1)]
        public void Execute(IClient client, Packet packetIn)
        {
            Logger.Trace("RPC:Storage:Execute()");
            var request = bnet.protocol.storage.ExecuteRequest.ParseFrom(packetIn.Payload.ToArray());
            //Logger.Debug("request:\n{0}", request.ToString());
            
            bnet.protocol.storage.ExecuteResponse response = null;
            switch (request.QueryName)
            {
                case "GetGameAccountSettings":
                    response = GameAccountSettings(request);
                    break;
                case "LoadAccountDigest":
                    response = LoadAccountDigest(client, request);
                    break;
                case "GetHeroDigests":
                    response = GetHeroDigest(client, request);
                    break;
                case "GetToonSettings":
                    response = GetToonSettings(request);
                    break;
                default:
                    Logger.Warn("Unhandled query: {0}", request.QueryName);
                    response = bnet.protocol.storage.ExecuteResponse.CreateBuilder().Build();
                    break;
            }                
            
            var packet = new Packet(
                new Header(0xfe, 0x0, packetIn.Header.RequestID, (uint)response.SerializedSize),
                response.ToByteArray());

            client.Send(packet);
        }

        private bnet.protocol.storage.ExecuteResponse GetHeroDigest(IClient client, bnet.protocol.storage.ExecuteRequest request)
        {
            var results = new List<bnet.protocol.storage.OperationResult>();

            foreach(var operation in request.OperationsList)
            {
                // find the requested toons entity-id.
                var stream = new MemoryStream(operation.RowId.Hash.ToByteArray());
                
                // contains ToonHandle in field form with one unknown field (which is not in message definition):
                // int16 unknown; uint8 realm; uint8 region; uint32 program; uint64 id;
                stream.ReadValueU16(); // unknown
                stream.ReadValueU8(); // realm
                stream.ReadValueU8(); // region 
                stream.ReadValueU32(false); // program
                    
                var toonId=stream.ReadValueU64(false);

                if(!client.Account.Toons.ContainsKey(toonId))
                {
                    Logger.Error("Can't find the requested toon: " + toonId);
                    continue;
                }

                var toon = client.Account.Toons[toonId];                    
                var operationResult = bnet.protocol.storage.OperationResult.CreateBuilder().SetTableId(operation.TableId);
                operationResult.AddData(
                    bnet.protocol.storage.Cell.CreateBuilder()
                        .SetColumnId(request.OperationsList[0].ColumnId)
                        .SetRowId(request.OperationsList[0].RowId)
                        .SetVersion(1)
                        .SetData(toon.Digest.ToByteString())
                        .Build()
                    );
                results.Add(operationResult.Build());
            }

            var builder = bnet.protocol.storage.ExecuteResponse.CreateBuilder();            
            foreach(var result in results)
            {
                builder.AddResults(result);
            }            
            return builder.Build();
        }

        private bnet.protocol.storage.ExecuteResponse GetToonSettings(bnet.protocol.storage.ExecuteRequest request)
        {
            var results = new List<bnet.protocol.storage.OperationResult>();

            foreach (var operation in request.OperationsList)
            {
                var operationResult = bnet.protocol.storage.OperationResult.CreateBuilder().SetTableId(operation.TableId);
                operationResult.AddData(
                    bnet.protocol.storage.Cell.CreateBuilder()
                        .SetColumnId(request.OperationsList[0].ColumnId)
                        .SetRowId(request.OperationsList[0].RowId)
                        .SetVersion(1)
                        .Build()
                    );
                results.Add(operationResult.Build());
            }

            var builder = bnet.protocol.storage.ExecuteResponse.CreateBuilder();
            foreach (var result in results)
            {
                builder.AddResults(result);
            }
            return builder.Build();
        }

        private bnet.protocol.storage.ExecuteResponse LoadAccountDigest(IClient client, bnet.protocol.storage.ExecuteRequest request)
        {
            var results = new List<bnet.protocol.storage.OperationResult>();          

            foreach (var operation in request.OperationsList)
            {
                var operationResult = bnet.protocol.storage.OperationResult.CreateBuilder().SetTableId(operation.TableId);
                operationResult.AddData(
                    bnet.protocol.storage.Cell.CreateBuilder()
                        .SetColumnId(request.OperationsList[0].ColumnId)
                        .SetRowId(request.OperationsList[0].RowId)
                        .SetVersion(1)
                        .SetData(client.Account.Digest.ToByteString())
                        .Build());
                results.Add(operationResult.Build());
            }

            var builder = bnet.protocol.storage.ExecuteResponse.CreateBuilder();
            foreach (var result in results)
            {
                builder.AddResults(result);
            }
            return builder.Build();
        }

        private bnet.protocol.storage.ExecuteResponse GameAccountSettings(bnet.protocol.storage.ExecuteRequest request)
        {
            var results = new List<bnet.protocol.storage.OperationResult>();

            foreach (var operation in request.OperationsList)
            {
                var operationResult = bnet.protocol.storage.OperationResult.CreateBuilder().SetTableId(operation.TableId);
                operationResult.AddData(
                    bnet.protocol.storage.Cell.CreateBuilder()
                        .SetColumnId(request.OperationsList[0].ColumnId)
                        .SetRowId(request.OperationsList[0].RowId)
                        .Build());
                results.Add(operationResult.Build());
            }

            var builder = bnet.protocol.storage.ExecuteResponse.CreateBuilder();
            foreach (var result in results)
            {
                builder.AddResults(result);
            }
            return builder.Build();
        }
    }
}
