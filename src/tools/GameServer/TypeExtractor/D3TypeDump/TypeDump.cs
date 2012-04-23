﻿/*
 * Copyright (C) 2011 mooege project
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

using System;
using System.Collections.Generic;
using System.Linq;
using D3TypeDescriptor;
using System.Diagnostics;
using System.Xml.Linq;

namespace D3TypeDump
{
    class TypeDump
    {
        const int HashAddress = 0x01318A30;
        const int HashOffset = 0x24;

        #region build 7447 addresses
        //const int OpcodeSwitch_Address = 0x008C22F0;
        //const int TypeDescriptorsAddress = 0x157F5CC;
        //const int TypeDescriptorsOffset = 40;
        //const int AttributesAddress = 0x01372420;
        //const int AttributeCount = 717;
        //const int Attribute_Int = 0x011A55D4;
        //const int Attribute_IntMinMax = 0x011A55DC;
        //const int Attribute_FloatMinMax = 0x011A55E4;
        //const int Attribute_Float16 = 0x011A55EC;
        //const int Attribute_Float16Or32 = 0x011A55F4;
        #endregion

        #region build 7728 addresses
        //const int OpcodeSwitch_Address = 0x008C22F0;
        //const int TypeDescriptorsAddress = 0x157F5CC;
        //const int TypeDescriptorsOffset = 40;
        //const int AttributesAddress = 0x01372420;
        //const int AttributeCount = 717;
        //const int Attribute_Int = 0x011A55D4;
        //const int Attribute_IntMinMax = 0x011A55DC;
        //const int Attribute_FloatMinMax = 0x011A55E4;
        //const int Attribute_Float16 = 0x011A55EC;
        //const int Attribute_Float16Or32 = 0x011A55F4;
        #endregion

        #region build 7841 addresses
        //const int OpcodeSwitch_Address = 0x008C4260;
        //const int TypeDescriptorsAddress = 0x15C9008;
        //const int TypeDescriptorsOffset = 40;
        //const int AttributesAddress = 0x013AC420;
        //const int AttributeCount = 728;
        //const int Attribute_Int = 0x11D4C5C;
        //const int Attribute_IntMinMax = 0x011D4C64;
        //const int Attribute_FloatMinMax = 0x011D4C6C;
        //const int Attribute_Float16 = 0x011D4C74;
        //const int Attribute_Float16Or32 = 0x011D4C7C;
        #endregion

        #region build 8101 addresses
        //const int OpcodeSwitch_Address = 0x008C61F0; //D3 .text

        //const int TypeDescriptorsAddress = 0x016259F8; //D3 .data
        //const int TypeDescriptorsOffset = 40;

        //const int AttributesAddress = 0x013E73B0; //D3 .data
        //const int AttributeCount = 769;
        //const int Attribute_Int = 0x12054EC; //D3 .rdata
        //const int Attribute_IntMinMax = 0x12054F4; //D3 .rdata
        //const int Attribute_FloatMinMax = 0x12054FC; //D3 .rdata
        //const int Attribute_Float16 = 0x1205504; //D3 .rdata
        //const int Attribute_Float16Or32 = 0x120550C; //D3 .rdata
        #endregion

        #region build 8296 addresses
        //const int OpcodeSwitch_Address = 0x008C05B0; //D3 .text

        //const int TypeDescriptorsAddress = 0x01435E7C; //D3 .data
        //const int TypeDescriptorsOffset = 40;

        //const int AttributesAddress = 0x0141D338; //D3 .data
        //const int AttributeCount = 784;
        //const int Attribute_Int = 0X122FF0C; //D3 .rdata
        //const int Attribute_IntMinMax = 0X122FF14; //D3 .rdata
        //const int Attribute_FloatMinMax = 0X122FF1C; //D3 .rdata
        //const int Attribute_Float16 = 0X122FF24; //D3 .rdata
        //const int Attribute_Float16Or32 = 0X122FF2C; //D3 .rdata
        #endregion

        #region build 8610 addresses
        //const int OpcodeSwitch_Address = 0x008BEA00;

        //const int TypeDescriptorsAddress = 0x016BAF60;
        //const int TypeDescriptorsOffset = 40;

        //const int AttributesAddress = 0x01482338;
        //const int AttributeCount = 819;
        //const int Attribute_Int = 0X1288354;
        //const int Attribute_IntMinMax = 0X128835C;
        //const int Attribute_FloatMinMax = 0X1288364;
        //const int Attribute_Float16 = 0X128836C;
        //const int Attribute_Float16Or32 = 0X1288374;
        #endregion

        #region build 8815 addresses
        //const int OpcodeSwitch_Address = 0x008C1350;
        //const int TypeDescriptorsAddress = 0x016EFEB0;
        //const int TypeDescriptorsOffset = 40;
        //const int AttributesAddress = 0x14A3340;
        //const int AttributeCount = 823;
        //const int Attribute_Int         = 0X12A2BD4;
        //const int Attribute_IntMinMax   = 0X12A2BDC;
        //const int Attribute_FloatMinMax = 0X12A2BE4;
        //const int Attribute_Float16     = 0X12A2BEC;
        //const int Attribute_Float16Or32 = 0X12A2BF4;
        #endregion

        #region build 8896 addresses
        //const int OpcodeSwitch_Address = 0x008C0A10;
        //const int TypeDescriptorsAddress = 0x014E561C;
        //const int TypeDescriptorsOffset = 40;
        //const int AttributesAddress = 0x14C95A8;
        //const int AttributeCount = 823;
        //const int Attribute_Int = 0X12C3004;
        //const int Attribute_IntMinMax = 0X12C300C;
        //const int Attribute_FloatMinMax = 0X12C3014;
        //const int Attribute_Float16 = 0X12C301C;
        //const int Attribute_Float16Or32 = 0X12C3024;
        #endregion

        #region build 9183 addresses
        //const int OpcodeSwitch_Address = 0x008C0770;
        //const int TypeDescriptorsAddress = 0x0151628C;
        //const int TypeDescriptorsOffset = 40;
        //const int AttributesAddress = 0x014FA3D0;
        //const int AttributeCount = 823;
        //const int Attribute_Int = 0x012ED0C4;
        //const int Attribute_IntMinMax = 0x012ED0CC;
        //const int Attribute_FloatMinMax = 0x012ED0D4;
        //const int Attribute_Float16 = 0x012ED0DC;
        //const int Attribute_Float16Or32 = 0x012ED0E4;
        #endregion

        #region build 9327 addresses
        const int OpcodeSwitch_Address = 0x008C0B00;
        const int TypeDescriptorsAddress = 0x0151828C;
        const int TypeDescriptorsOffset = 40;
        const int AttributesAddress = 0x014FC3D0;
        const int AttributeCount = 823;
        const int Attribute_Int = 0x012EF0F4;
        const int Attribute_IntMinMax = 0x012EF0FC;
        const int Attribute_FloatMinMax = 0x012EF104;
        const int Attribute_Float16 = 0x012EF10C;
        const int Attribute_Float16Or32 = 0x012EF114;
        #endregion
        // TODO: Add patterns

        class GameMessageInfo
        {
            public int Offset;
            public int Size;
            public List<int> Opcodes = new List<int>();
        }

        static Dictionary<int, GameMessageInfo> _gameMessageLookUp;

        static void GetMessageOpcodes(Mem32 m)
        {
            _gameMessageLookUp = new Dictionary<int, GameMessageInfo>();

            var func = m[OpcodeSwitch_Address];
            int maxOpcode = func[0x1B].Int32 + 1;
            int jaOffset = func[0x21].Int32;
            int defaultCase = func.Offset + 0x25 + jaOffset;
            var switchTable = func[0x28].Ptr;
            for (int i = 0; i < maxOpcode; i++)
            {
                int caseOffset = switchTable[i * 4].Int32;
                if (caseOffset == defaultCase)
                    continue;

                int offTypeDescriptor = m[caseOffset + 2].Ptr.Int32;
                int size = m[caseOffset + 13].Int32;
                int opcode = i + 1;

                GameMessageInfo gmi;
                if (_gameMessageLookUp.TryGetValue(offTypeDescriptor, out gmi))
                {
                    if (gmi.Size != size)
                        throw new Exception("Size mismatch.");
                }
                else
                {
                    gmi = new GameMessageInfo();
                    gmi.Offset = offTypeDescriptor;
                    gmi.Size = size;
                    _gameMessageLookUp.Add(offTypeDescriptor, gmi);
                }
                gmi.Opcodes.Add(opcode);
            }

        }
        #region Dump
        static TypeDescriptor DiscoverMessageDescriptors(Mem32 mem, Dictionary<int, TypeDescriptor> table, int offset)
        {
            if (offset == 0)
                return null;
            string name = mem[offset + 4].CStringPtr;
            int unkValue = mem[offset + 8].Int32;
            var fields = mem[offset + 12].Ptr;

            if (fields.Offset == 0)
            {
                var basicType = TypeDescriptor.AllocateBasicType(name);
                basicType.Name = name;
                basicType.UnkValue = unkValue;
                table.Add(offset, basicType);
                return basicType;
            }

            StructureTypeDescriptor typeDesc;

            GameMessageInfo gmi;

            if (_gameMessageLookUp.TryGetValue(offset, out gmi))
            {
                var gm = TypeDescriptor.AllocateGameMessage(name);
                gm.Size = gmi.Size;
                gm.NetworkIds = gmi.Opcodes.ToArray();
                typeDesc = gm;
            }
            else
                typeDesc = TypeDescriptor.AllocateStructure(name);
            typeDesc.Name = name;
            typeDesc.UnkValue = unkValue;

            table.Add(offset, typeDesc);

            List<FieldDescriptor> list = new List<FieldDescriptor>();
            for (; ; )
            {
                FieldDescriptor f = new FieldDescriptor();
                f.Name = fields.CStringPtr;
                int type = fields[4].Int32;
                if (mem[type + 4].CStringPtr != "DT_NULL" && !table.TryGetValue(type, out f.Type))
                    f.Type = DiscoverMessageDescriptors(mem, table, type);
                f.Offset = fields[8].Int32;
                var defaultValuePtr = fields[12].Ptr;
                f.Min = fields[0x10].Int32;
                f.Max = fields[0x14].Int32;
                f.Flags = fields[0x18].Int32;
                type = fields[0x1C].Int32;
                if (mem[type + 4].CStringPtr != "DT_NULL" && !table.TryGetValue(type, out f.SubType))
                    f.SubType = DiscoverMessageDescriptors(mem, table, type);
                f.VariableOffset = fields[0x20].Int32;
                f.ArrayLength = fields[0x24].Int32;
                f.ArrayLengthOffset = fields[0x28].Int32;
                f.EncodedBits = fields[0x2C].UInt16;
                f.EncodedBits2 = fields[0x2E].UInt16;
                f.SnoType = fields[0x30].Int32;
                f.TagMapRelated = fields[0x34].Int32;
                var enumFields = fields[0x38].Ptr;
                if (enumFields.Offset != 0)
                {
                    List<Tuple<string, int>> enums = new List<Tuple<string, int>>();
                    for (; ; )
                    {
                        if (enumFields[4].Int32 == 0)
                            break;
                        enums.Add(new Tuple<string, int>(enumFields[4].CStringPtr, enumFields.Int32));
                        enumFields = enumFields[8];
                    }
                    f.EnumFields = enums.ToArray();
                }
                f.FlagIndex = fields[0x3C].Int32;
                int funcA = fields[0x40].Int32; // TODO
                int funcB = fields[0x44].Int32; // TODO
                f.DspIndex = fields[0x48].Int32;

                var str = fields[0x4C].CString;
                // 0x4C 64 bytes, unused string
                //if (str != string.Empty)  Console.WriteLine(str);

                list.Add(f);
                if (fields.Int32 == 0)
                    break;
                fields = fields[140];

            }


            typeDesc.Fields = list.ToArray();
            return typeDesc;
        }


        static TypeDescriptor[] GetAllDescriptors(Mem32 mem)
        {
            Dictionary<int, TypeDescriptor> table = new Dictionary<int, TypeDescriptor>();
            var link = mem[TypeDescriptorsAddress].Ptr;
            int count = 0;
            while (link.Int32 != 0)
            {
                if (table.ContainsKey(link.Offset) == false)
                {
                    var desc = DiscoverMessageDescriptors(mem, table, link.Offset);
                }
                count++;
                link = link[TypeDescriptorsOffset].Ptr;
            }

            var result = table.Values.ToArray();
            for (int i = 0; i < result.Length; i++)
                result[i].Index = i;
            return result;

        }

        static void DumpAttributes(Mem32 mem)
        {
            // could get the max num from descriptor
            var attribList = mem[AttributesAddress];
            NetAttribute.Attributes = new NetAttribute[AttributeCount];
            for (int i = 0; i < AttributeCount; i++)
            {
                var attrib = attribList[i * 40];

                int id = attrib.Int32;
                int u2 = attrib[4].Int32;
                int u3 = attrib[8].Int32;
                int u4 = attrib[12].Int32;
                int u5 = attrib[16].Int32;

                string scriptA = attrib[20].CStringPtr;
                string scriptB = attrib[24].CStringPtr;
                string name = attrib[28].CStringPtr;
                var decoder = attrib[32].Ptr;
                byte u10 = attrib[36].Byte;
                switch (decoder.Int32)
                {
                    case Attribute_Int:
                        {
                            int bitCount = decoder[12].Int32;
                            NetAttribute.Attributes[id] = new NetAttribute(id, u2, u3, u4, u5, scriptA, scriptB, name, NetAttributeEncoding.Int, u10, 0, 0, bitCount);
                            break;
                        }
                    case Attribute_IntMinMax:
                        {
                            int bitCount = decoder[20].Int32;
                            int min = decoder[12].Int32;
                            int max = decoder[16].Int32;
                            NetAttribute.Attributes[id] = new NetAttribute(id, u2, u3, u4, u5, scriptA, scriptB, name, NetAttributeEncoding.IntMinMax, u10, min, max, bitCount);
                            break;
                        }
                    case Attribute_Float16: // Decode16BitFloat(bits)
                        {
                            int bitCount = decoder[12].Int32;
                            NetAttribute.Attributes[id] = new NetAttribute(id, u2, u3, u4, u5, scriptA, scriptB, name, NetAttributeEncoding.Float16, u10, 0, 0, bitCount);
                        }
                        break;
                    case Attribute_Float16Or32:
                        {
                            NetAttribute.Attributes[id] = new NetAttribute(id, u2, u3, u4, u5, scriptA, scriptB, name, NetAttributeEncoding.Float16Or32, u10, 0, 0, 0);
                        }
                        break;
                    case Attribute_FloatMinMax: // DecodeFloatMinMax
                        throw new Exception("FloatMinMax used");
                    default:
                        throw new Exception("Unknown decoder used");
                }
            }
            NetAttribute.SaveXml("attributes.xml");
        }

        public static void DumpDescriptors()
        {
            foreach (var p in Process.GetProcesses())
            {
                if (p.ProcessName == "Diablo III")
                {
                    Mem32 m = new Mem32(p);
                    GetMessageOpcodes(m);
                    DumpAttributes(m);

                    XDocument doc = new XDocument();
                    XElement root = new XElement("TypeDescriptors");
                    int hash = m[HashAddress].Ptr[HashOffset].Int32;
                    root.Add(new XAttribute("ProtocolHash", hash));

                    foreach (var desc in GetAllDescriptors(m))
                    {
                        root.Add(desc.ToXml());
                    }
                    doc.Add(root);

                    doc.Save("typedescriptors.xml");

                    m.Dispose();
                    return;
                }
            }
            Console.WriteLine("Was unable to find any 'Diablo III' process.");
        }
        #endregion
    }
}