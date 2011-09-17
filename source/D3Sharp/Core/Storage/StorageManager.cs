using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using D3Sharp.Net;
using D3Sharp.Net.Packets;
using D3Sharp.Utils;
using D3Sharp.Utils.Extensions;
using Google.ProtocolBuffers;

namespace D3Sharp.Core.Storage
{
    public class StorageManager
    {
        public static Dictionary<bnet.protocol.storage.TableId, Table> Tables { get; private set; }
        public readonly object TableLock = new object();

        // Temporary
        public static Table ToonTable = new Table(
            bnet.protocol.storage.TableId.CreateBuilder()
            .SetHash(
                ByteString.CopyFrom(
                    new byte[] {
                        0x06, 0x8B, 0x94, 0x62, 0x64, 0x14, 0xB9, 0x4A, 0x51, 0x36, 0xFD, 0x0F, 0xA6, 0xE6, 0x6C, 0xD7
                    }))
            .Build());

        // Temporary
        public static Row CharacterRow = new Row(
            bnet.protocol.storage.RowId.CreateBuilder()
            .SetHash(
                ByteString.CopyFrom(
                    new byte[] {
                        0x03, 0x00, 0x01, 0x62, 0x00, 0x00, 0x44, 0x33, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF
                    }))
            .Build());

        static StorageManager()
        {
            Tables = new Dictionary<bnet.protocol.storage.TableId, Table>();
            Tables[ToonTable.Id] = ToonTable;
            ToonTable.Rows[CharacterRow.Id] = CharacterRow;
        }
    }
}

