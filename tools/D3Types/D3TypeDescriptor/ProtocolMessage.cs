using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;

namespace D3TypeDescriptor
{
    public partial class ProtocolMessage
    {
        public TypeDescriptor Descriptor;
        public byte[] Data;
        public bool Server;

        public ProtocolMessage(TypeDescriptor descriptor, byte[] data, bool server)
        {
            Descriptor = descriptor;
            Data = data;
            Server = server;
        }
        public string TypeName { get { return Descriptor.Name; } }
        public int Id { get { return BitConverter.ToInt32(Data, 4); } }


        public object[] GetValues()
        {
            BinaryReader r = new BinaryReader(new MemoryStream(Data));
            int size = r.ReadInt32();
            int id = r.ReadInt32();

            var fields = Descriptor.Fields;
            if (fields[fields.Length - 1].Type.Name != "DT_NULL")
                throw new Exception("Expected DT_NULL");
            if (Descriptor.Name == "GenericBlobMessage")
            {
                object[] values = new object[fields.Length];
                for (int i = 0; i < fields.Length - 1; i++)
                {
                    r.BaseStream.Position = fields[i].Offset;
                    values[i] = fields[i].ReadValue(r);
                }
                values[values.Length - 1] = r.ReadBytes((int)values[1]);
                return values;
            }
            else
            {
                object[] values = new object[fields.Length-1];
                for (int i = 0; i < fields.Length - 1; i++)
                {
                    r.BaseStream.Position = fields[i].Offset;
                    values[i] = fields[i].ReadValue(r);
                }
                return values;
            }
        }
    }

}
