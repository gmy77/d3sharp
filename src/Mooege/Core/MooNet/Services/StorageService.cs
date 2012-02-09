/*
 * Copyright (C) 2011 - 2012 mooege project - http://www.mooege.org
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

using System.IO;
using System.Collections.Generic;
using Gibbed.IO;
using Mooege.Common.Extensions;
using Mooege.Common.Logging;
using Mooege.Net.MooNet;
using Google.ProtocolBuffers;

namespace Mooege.Core.MooNet.Services
{
    [Service(serviceID: 0x9, serviceName: "bnet.protocol.storage.StorageService")]
    public class StorageService : bnet.protocol.storage.StorageService,IServerService
    {
        private static readonly Logger Logger = LogManager.CreateLogger();
        public MooNetClient Client { get; set; }
        public bnet.protocol.Header LastCallHeader { get; set; }

        public override void OpenTable(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.storage.OpenTableRequest request, System.Action<bnet.protocol.storage.OpenTableResponse> done)
        {
            Logger.Trace("OpenTable() {0}", this.Client);
            var builder = bnet.protocol.storage.OpenTableResponse.CreateBuilder();
            done(builder.Build());
        }

        public override void OpenColumn(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.storage.OpenColumnRequest request, System.Action<bnet.protocol.storage.OpenColumnResponse> done)
        {
            Logger.Trace("OpenColumn() {0}", this.Client);
            var builder = bnet.protocol.storage.OpenColumnResponse.CreateBuilder();
            done(builder.Build());
        }

        public override void Execute(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.storage.ExecuteRequest request, System.Action<bnet.protocol.storage.ExecuteResponse> done)
        {
            Logger.Trace("Execute() {0}", this.Client);
            bnet.protocol.storage.ExecuteResponse response = null;
            switch (request.QueryName)
            {
                case "GetGameAccountSettings":
                    response = GameAccountSettings(request);
                    break;
                case "LoadAccountDigest":
                    response = LoadAccountDigest(Client, request);
                    break;
                case "GetHeroDigests":
                    response = GetHeroDigest(Client, request);
                    break;
                case "GetToonSettings":
                    response = GetToonSettings(request);
                    break;
                //case "GetAccountProfile":
                //    response = GetAccountProfile(Client, request);
                //    break;
                //case "GetHeroProfiles":
                //    response = GetHeroProfiles(Client, request);
                //    break;
                default:
                    Logger.Warn("Unhandled query: {0}", request.QueryName);
                    response = bnet.protocol.storage.ExecuteResponse.CreateBuilder().Build();
                    break;
            }

            done(response);
        }

        private static readonly Google.ProtocolBuffers.ByteString HeroDigestColumn = Google.ProtocolBuffers.ByteString.CopyFrom(new byte[] { 0xA1, 0x81, 0xA8, 0x35, 0x68, 0x24, 0x41, 0x60, 0x09, 0x7C, 0x05, 0x1B, 0x11, 0xA8, 0x7F, 0x04 });
        private static readonly Google.ProtocolBuffers.ByteString HeroNameColumn = Google.ProtocolBuffers.ByteString.CopyFrom(new byte[] { 0xE6, 0x7A, 0xC5, 0x2B, 0x8F, 0x5F, 0x83, 0x87, 0xA6, 0x68, 0xD7, 0x2C, 0x46, 0x74, 0xEC, 0xD3 });

        private bnet.protocol.storage.ExecuteResponse GetHeroDigest(MooNetClient client, bnet.protocol.storage.ExecuteRequest request)
        {
            var results = new List<bnet.protocol.storage.OperationResult>();

            foreach(var operation in request.OperationsList)
            {
                Google.ProtocolBuffers.ByteString data = null;

                // find the requested toons entity-id.
                var stream = new MemoryStream(operation.RowId.Hash.ToByteArray());

                // contains ToonHandle in field form with one unknown field (which is not in message definition):
                // int16 unknown; uint8 realm; uint8 region; uint32 program; uint64 id;
                stream.ReadValueU16(); // unknown
                stream.ReadValueU8(); // realm
                stream.ReadValueU8(); // region 
                stream.ReadValueU32(false); // program

                var toonId = stream.ReadValueU64(false);

                if (!client.Account.CurrentGameAccount.Toons.ContainsKey(toonId))
                {
                    Logger.Error("Can't find the requested toon: {0}", toonId);
                    continue;
                }

                var toon = client.Account.CurrentGameAccount.Toons[toonId];

                if (operation.ColumnId.Hash.Equals(HeroDigestColumn))
                    data = toon.Digest.ToByteString();
                else
                    Logger.Warn("Unknown ColumndId requested: {0}", operation.ColumnId.Hash.ToByteArray().HexDump());
                //else if (operation.ColumnId.Hash.Equals(HeroNameColumn))
                //    data = toon.NameText.ToByteString();
                                 
                var operationResult = bnet.protocol.storage.OperationResult.CreateBuilder().SetTableId(operation.TableId);
                operationResult.AddData(
                    bnet.protocol.storage.Cell.CreateBuilder()
                        .SetColumnId(request.OperationsList[0].ColumnId)
                        .SetRowId(request.OperationsList[0].RowId)
                        .SetVersion(1)
                        .SetData(data)
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
                operationResult.SetErrorCode(4); //this query returns error 4 in 7728 -Egris
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

        private bnet.protocol.storage.ExecuteResponse LoadAccountDigest(MooNetClient client, bnet.protocol.storage.ExecuteRequest request)
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
                        .SetData(client.Account.CurrentGameAccount.Digest.ToByteString())
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
                operationResult.SetErrorCode(4); //this query returns error 4 in 7728 -Egris
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

    //    private bnet.protocol.storage.ExecuteResponse GetHeroProfiles(MooNetClient client, bnet.protocol.storage.ExecuteRequest request)
    //    {
    //        var results = new List<bnet.protocol.storage.OperationResult>();
    //        Logger.Trace("GetHeroProfiles()");
    //        foreach (var operation in request.OperationsList)
    //        {
    //            var stream = new MemoryStream(operation.RowId.Hash.ToByteArray());

    //            // contains ToonHandle in field form with one unknown field (which is not in message definition):
    //            // int16 unknown; uint8 realm; uint8 region; uint32 program; uint64 id;
    //            var x = stream.ReadValueU16(); // unknown
    //            var y = stream.ReadValueU8(); // realm
    //            var z = stream.ReadValueU8(); // region
    //            var p = stream.ReadValueU32(false); // program

    //            var toonId = stream.ReadValueU64(false);

    //            if (!client.CurrentGameAccount.Toons.ContainsKey(toonId))
    //            {
    //                Logger.Error("Can't find the requested toon: {0}", toonId);
    //                continue;
    //            }

    //            var toon = client.CurrentGameAccount.Toons[toonId];

    //            var operationResult = bnet.protocol.storage.OperationResult.CreateBuilder().SetTableId(operation.TableId);
    //            operationResult.AddData(
    //                bnet.protocol.storage.Cell.CreateBuilder()
    //                    .SetColumnId(request.OperationsList[0].ColumnId)
    //                    .SetRowId(request.OperationsList[0].RowId)
    //                    .SetVersion(1321757938329580)
    //                    .SetData(toon.HeroProfile)
    //                    .Build()
    //                );
    //            results.Add(operationResult.Build());
    //        }

    //        var builder = bnet.protocol.storage.ExecuteResponse.CreateBuilder();
    //        foreach (var result in results)
    //        {
    //            builder.AddResults(result);
    //        }
    //        return builder.Build();
    //    }

    //    private bnet.protocol.storage.ExecuteResponse GetAccountProfile(MooNetClient client, bnet.protocol.storage.ExecuteRequest request)
    //    {
    //        var results = new List<bnet.protocol.storage.OperationResult>();
    //        Logger.Trace("GetAccountProfile()");
    //        foreach (var operation in request.OperationsList)
    //        {
    //            var stream = new MemoryStream(operation.RowId.Hash.ToByteArray());

    //            // contains ToonHandle in field form with one unknown field (which is not in message definition):
    //            // int16 unknown; uint8 realm; uint8 region; uint32 program; uint64 id;
    //            var x = stream.ReadValueU16(); // unknown
    //            var y = stream.ReadValueU8(); // realm
    //            var z = stream.ReadValueU8(); // region
    //            var p = stream.ReadValueU32(false); // program

    //            var toonId = stream.ReadValueU64(false);

    //            if (!client.CurrentGameAccount.Toons.ContainsKey(toonId))
    //            {
    //                Logger.Error("Can't find the requested toon: {0}", toonId);
    //                continue;
    //            }

    //            var toon = client.CurrentGameAccount.Toons[toonId];

    //            var operationResult = bnet.protocol.storage.OperationResult.CreateBuilder().SetTableId(operation.TableId);
    //            operationResult.AddData(
    //                bnet.protocol.storage.Cell.CreateBuilder()
    //                    .SetColumnId(request.OperationsList[0].ColumnId)
    //                    .SetRowId(request.OperationsList[0].RowId)
    //                    .SetVersion(1321757938329580)
    //                    .SetData(client.CurrentGameAccount.AccountProfile.ToByteString())
    //                    .Build()
    //                );
    //            results.Add(operationResult.Build());
    //        }

    //        var builder = bnet.protocol.storage.ExecuteResponse.CreateBuilder();
    //        foreach (var result in results)
    //        {
    //            builder.AddResults(result);
    //        }
    //        return builder.Build();
    //    }
    }
}
