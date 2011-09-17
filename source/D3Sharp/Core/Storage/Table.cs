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
    public class Table
    {
        public bnet.protocol.storage.TableId Id { get; private set; }
        public Dictionary<bnet.protocol.storage.RowId, Row> Rows { get; private set; }
        
        public Table(bnet.protocol.storage.TableId id)
        {
            this.Id = id;
            this.Rows = new Dictionary<bnet.protocol.storage.RowId, Row>();
        }
        
        public void AddRow(Row row)
        {
            /*if (Rows.ContainsKey(row.Id))
            {
                Logger.Warn("Replacing row for ID {0}", row.Id.Hash.ToByteArray().ToString());
            }*/
            Rows.Add(row.Id, row);
        }
        
        public bnet.protocol.storage.Cell SerializeCell(bnet.protocol.storage.RowId row_id, bnet.protocol.storage.ColumnId column_id)
        {
            return Rows[row_id].SerializeCell(column_id);
        }
    }
    
    public class Row
    {
        public bnet.protocol.storage.RowId Id { get; private set; }
        public Dictionary<bnet.protocol.storage.ColumnId, Cell> Cells { get; private set; }
        
        public Row(bnet.protocol.storage.RowId id)
        {
            this.Id = id;
            this.Cells = new Dictionary<bnet.protocol.storage.ColumnId, Cell>();
        }
        
        public void AddCell(Cell cell)
        {
            /*if (Cells.ContainsKey(cell.Id))
            {
                Logger.Warn("Replacing cell for ID {0}", cell.Id.Hash.ToByteArray().ToString());
            }*/
            Cells.Add(cell.Id, cell);
        }
        
        public bnet.protocol.storage.Cell SerializeCell(bnet.protocol.storage.ColumnId id)
        {
            var cell = Cells[id];
            var builder=bnet.protocol.storage.Cell.CreateBuilder();
            builder.SetRowId(Id);
            cell.Serialize(builder);
            // optional bytes row_key
            // optional fixed64 version
            return builder.Build();
        }
    }
    
    public class Cell
    {
        public bnet.protocol.storage.ColumnId Id { get; private set; }
        public ByteString Data { get; set; }
        
        public Cell(bnet.protocol.storage.ColumnId id, ByteString data)
        {
            this.Id = id;
            this.Data = data;
        }
        
        public Cell ParseFrom(bnet.protocol.storage.Cell cell)
        {
            this.Id = cell.ColumnId;
            this.Data = cell.Data;
            return this;
        }
        
        public bnet.protocol.storage.Cell.Builder Serialize(bnet.protocol.storage.Cell.Builder builder)
        {
            builder.SetColumnId(Id);
            if (Data!=null)
                builder.SetData(Data);
            else
                builder.ClearData();
            return builder;
        }
    }
}

