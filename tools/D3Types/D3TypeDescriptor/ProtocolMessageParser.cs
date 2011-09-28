using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;

namespace D3TypeDescriptor
{
    public class ProtocolMessageParser
    {
        public TypeDescriptor[] Descriptors;
        public int Hash;
        public ProtocolMessageParser(XDocument doc)
        {
            var root = doc.Root;
            Hash = int.Parse(root.Attribute("Hash").Value);
            
        }

        public ProtocolMessage[] ParseMemoryWithServerByte(Stream input)
        {
            BinaryReader r = new BinaryReader(input);
            List<ProtocolMessage> list = new List<ProtocolMessage>(short.MaxValue);

            while (r.BaseStream.Position + 9 <= r.BaseStream.Length)
            {
                byte incoming = r.ReadByte();
                int length = r.ReadInt32();
                int id = r.ReadInt32();
                var desc = Descriptors[id];



                if (desc.Size != length)
                {
                    if (!(desc.Name == "GenericBlobMessage" && length >= desc.Size)
                        )
                        throw new Exception("Size mismatch for " + desc.Name + "(" + desc.Size + ") " + length);
                }

                r.BaseStream.Position -= 8;
                if (r.BaseStream.Position + length > r.BaseStream.Length)
                    break;
                byte[] data = r.ReadBytes(length);
                list.Add(new ProtocolMessage(desc, data, incoming != 0));
            }
            return list.ToArray();
        }

        public ProtocolMessage[] ParseBitBufferPacket(BitBuffer buffer, bool server)
        {
            int start = buffer.Position;
            int size = buffer.ReadInt(32) * 8;
            int end = start + size;
            List<ProtocolMessage> list = new List<ProtocolMessage>();
            while (end - buffer.Position >= 9)
            {
                start = buffer.Position;
                int id = buffer.ReadInt(9);
                buffer.Position = start;
                TypeDescriptor desc = Descriptors[id];
                    var ms = new MemoryStream();
                    ms.SetLength(desc.Size);
                    BinaryWriter w = new BinaryWriter(ms);

                    w.Write(desc.Size);
                    foreach (var f in desc.Fields)
                    {
                        w.BaseStream.Position = f.Offset;
                        f.WriteValue(buffer, w);
                    }

                    if (desc.Name == "GenericBlobMessage")
                    {
                        ms.Position = 8;
                        byte[] tmp = new byte[4];
                        ms.Read(tmp, 0, 4);
                        size = BitConverter.ToInt32(tmp, 0);
                        ms.SetLength(desc.Size + size);
                        w.BaseStream.Position = desc.Size;
                        w.Write(buffer.ReadBlobNoLength(size));
                    }
                    else if (desc.Name == "AttributeSetValueMessage")
                    {
                        byte[] tmp = new byte[4];
                        ms.Position = 12 + 8;
                        ms.Read(tmp, 0, 4);
                        id = BitConverter.ToInt32(tmp, 0);
                        var attrib = NetAttribute.Attributes[id];
                        switch (attrib.EncodingType)
                        {
                            case NetAttributeEncoding.Int:
                                w.Write(buffer.ReadInt(attrib.BitCount));
                                break;
                            case NetAttributeEncoding.IntMinMax:
                                w.Write(buffer.ReadInt(attrib.BitCount) + attrib.Min);
                                break;
                            case NetAttributeEncoding.Float16:
                                w.Write(buffer.ReadFloat16());
                                break;
                            case NetAttributeEncoding.Float16Or32:
                                w.Write(buffer.ReadBool() ? buffer.ReadFloat16() : buffer.ReadFloat32());
                                break;
                            default:
                                throw new Exception("bad voodoo");
                        }
                    }
                    else if (desc.Name == "AttributesSetValuesMessage")
                    {
                        byte[] tmp = new byte[4];
                        ms.Position = 12;
                        ms.Read(tmp, 0, 4);
                        int count = BitConverter.ToInt32(tmp, 0);
                        for (int i = 0; i < count; i++)
                        {
                            ms.Position += 8;
                            ms.Read(tmp, 0, 4);
                            id = BitConverter.ToInt32(tmp, 0);
                            var attrib = NetAttribute.Attributes[id];
                            switch (attrib.EncodingType)
                            {
                                case NetAttributeEncoding.Int:
                                    w.Write(buffer.ReadInt(attrib.BitCount));
                                    break;
                                case NetAttributeEncoding.IntMinMax:
                                    w.Write(buffer.ReadInt(attrib.BitCount) + attrib.Min);
                                    break;
                                case NetAttributeEncoding.Float16:
                                    w.Write(buffer.ReadFloat16());
                                    break;
                                case NetAttributeEncoding.Float16Or32:
                                    w.Write(buffer.ReadBool() ? buffer.ReadFloat16() : buffer.ReadFloat32());
                                    break;
                                default:
                                    throw new Exception("bad voodoo");
                            }
                        }
                    }
                    byte[] data = ms.ToArray();

                    list.Add(new ProtocolMessage(desc, data, server));
            }
            buffer.Position = end;
            return list.ToArray();
        }
    }
}
