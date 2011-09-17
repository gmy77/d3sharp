using System.Linq;
using D3Sharp.Net;
using D3Sharp.Net.Packets;
using D3Sharp.Utils.Extensions;
using D3Sharp.Core.Storage;

namespace D3Sharp.Core.Services
{
    [Service(serviceID: 0x9, serverHash: 0xDA6E4BB9, clientHash: 0x0)]
    public class StorageService : Service
    {
        [ServiceMethod(0x2)]
        public void OpenTableRequest(IClient client, Packet packetIn)
        {
            Logger.Trace("RPC:Storage:OpenTableRequest()");
            var response = bnet.protocol.storage.OpenTableResponse.CreateBuilder().Build();

            var packet = new Packet(
                new Header(0xfe, 0x0, packetIn.Header.RequestID, (uint)response.SerializedSize),
                response.ToByteArray());

            client.Send(packet);
        }

        [ServiceMethod(0x3)]
        public void OpenColumnRequest(IClient client, Packet packetIn)
        {
            Logger.Trace("RPC:Storage:OpenColumnRequest()");
            var response = bnet.protocol.storage.OpenColumnResponse.CreateBuilder().Build();

            var packet = new Packet(
                new Header(0xfe, 0x0, packetIn.Header.RequestID, (uint)response.SerializedSize),
                response.ToByteArray());

            client.Send(packet);
        }

        [ServiceMethod(0x1)]
        public void ExecuteRequest(IClient client, Packet packetIn)
        {
            Logger.Trace("RPC:Storage:ExecuteRequest()");
            var request = bnet.protocol.storage.ExecuteRequest.ParseFrom(packetIn.Payload.ToArray());
            //Logger.Debug("request:\n{0}", request.ToString());
            
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
                case "GetToonSettings":
                    response = GetToonSettings(request);
                    break;
                default:
                    Logger.Warn("Unhandled ExecuteRequest: {0}", request.QueryName);
                    break;
            }                
            
            var packet = new Packet(
                new Header(0xfe, 0x0, packetIn.Header.RequestID, (uint)response.SerializedSize),
                response.ToByteArray());

            client.Send(packet);
        }

        private bnet.protocol.storage.ExecuteResponse GetToonSettings(bnet.protocol.storage.ExecuteRequest request)
        {
            var builder = bnet.protocol.storage.ExecuteResponse.CreateBuilder();

            var operationResult = bnet.protocol.storage.OperationResult.CreateBuilder()
                .SetTableId(request.OperationsList[0].TableId)
                .AddData(
                    bnet.protocol.storage.Cell.CreateBuilder()
                        .SetColumnId(request.OperationsList[0].ColumnId)
                        .SetRowId(request.OperationsList[0].RowId)
                        .SetVersion(1)
                        .Build())
                .Build();

            builder.AddResults(operationResult);
            return builder.Build();
        }

        private bnet.protocol.storage.ExecuteResponse GetHeroDigest(bnet.protocol.storage.ExecuteRequest request)
        {
            var op=request.OperationsList[0];
            var table_id=op.TableId;
            var column_id=op.ColumnId;
            var row_id=op.RowId;
            
            Logger.Debug("table_id.Hash:\n{0}", table_id.Hash.ToByteArray().Dump());
            Logger.Debug("column_id.Hash:\n{0}", column_id.Hash.ToByteArray().Dump());
            Logger.Debug("row_id.Hash:\n{0}", row_id.Hash.ToByteArray().Dump());
            
            /*try {
                var stream = CodedInputStream.CreateInstance(row_id.Hash.ToByteArray());
                stream.SkipRawBytes(2);
                var tgen=bnet.protocol.toon.ToonHandle.CreateBuilder()
                    .SetRealm(stream.ReadRawVarint32())
                    .SetRegion(stream.ReadRawVarint32())
                    .SetProgram(stream.ReadUInt32()) // "D3\0\0"
                    .SetId(stream.ReadUInt64())
                    .Build();
                Logger.Debug("generated:\n{0}", tgen.ToByteArray().Dump());
                Logger.Debug(tgen.ToString());
                //var toonhandle=bnet.protocol.toon.ToonHandle.ParseFrom(eid.ToByteArray());
                //Logger.Debug("row_id.hash as handle:\n{0}", toonhandle.ToString());
            } catch (Exception e) {
                Logger.DebugException(e, "row_id");
            }*/
            var heroDigest = D3.Hero.Digest.ParseFrom(StorageManager.Tables[table_id].Rows[row_id].Cells[column_id].Data);

            var operationResult = bnet.protocol.storage.OperationResult.CreateBuilder()
                .SetTableId(request.OperationsList[0].TableId)
                .AddData(
                    bnet.protocol.storage.Cell.CreateBuilder()
                        .SetColumnId(request.OperationsList[0].ColumnId)
                        .SetRowId(request.OperationsList[0].RowId)
                        .SetVersion(1)
                        .SetData(heroDigest.ToByteString())
                        .Build()
                ).Build();

            var builder = bnet.protocol.storage.ExecuteResponse.CreateBuilder();
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
                        .Build())
                .Build();

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
                        .Build())
                .Build();

            builder.AddResults(operationResult);
            return builder.Build();
        }
    }
}
