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
using System.Xml.Linq;

namespace D3TypeDescriptor
{
    public class FieldDescriptor
    {
        public int Index;
        public string Name;
        public TypeDescriptor Type;
        public int Offset;
        public int Min;
        public int Max;
        public int Flags;
        public TypeDescriptor SubType;
        public int VariableOffset;
        public int ArrayLength = -1;
        public int ArrayLengthOffset;
        public int EncodedBits;
        public int EncodedBits2;
        public int SnoType = -1;
        public int TagMapRelated;
        public Tuple<string, int>[] EnumFields;
        public int FlagIndex;
        public string FuncA;
        public string FuncB;
        public int DspIndex = -1;

        public bool HasMinMax { get { return (Flags & 0x10) != 0; } }
        public bool HasArrayLengthOffset { get { return (Flags & 0x08) != 0; } }
        public bool Float16Encoding { get { return (Flags & 0x80) != 0; } }


        public string GetFieldName() { return string.IsNullOrEmpty(Name) ? "Field" + Index : Name; }


        public FieldDescriptor() { }
        public XElement ToXml()
        {
            XElement e = new XElement("Field");
            if (!string.IsNullOrEmpty(Name)) e.Add(new XAttribute("Name", Name));
            if (Type != null && Type.Name != "DT_NULL") e.Add(new XAttribute("Type", Type.Name + "#" + Type.Index));
            e.Add(new XAttribute("Offset", Offset));
            if (((Flags >> 4) & 1) != 0)
            {
                e.Add(new XAttribute("Min", Min));
                e.Add(new XAttribute("Max", Max));
            }
            e.Add(new XAttribute("Flags", Flags));
            if (SubType != null && SubType.Name != "DT_NULL") e.Add(new XAttribute("SubType", SubType.Name + "#" + SubType.Index));
            if (VariableOffset != 0) e.Add(new XAttribute("VariableOffset", VariableOffset));
            if (ArrayLength != -1) e.Add(new XAttribute("ArrayLength", ArrayLength));
            if (ArrayLengthOffset != 0) e.Add(new XAttribute("ArrayLengthOffset", ArrayLengthOffset));            
            if (EncodedBits != 0) e.Add(new XAttribute("EncodedBits", EncodedBits));            
            if (EncodedBits2 != 0) e.Add(new XAttribute("EncodedBits2", EncodedBits2));            
            if (SnoType != -1) e.Add(new XAttribute("SnoType", SnoType)); // TODO: Add name of SNO
            if (TagMapRelated != 0) e.Add(new XAttribute("TagMapRelated", TagMapRelated));
            if (EnumFields != null)
            {
                XElement enums = new XElement("Enum");
                foreach (var t in EnumFields)
                    enums.Add(new XElement("Entry", new XAttribute("Name", t.Item1), new XAttribute("Value", t.Item2)));
                e.Add(enums);
            }
            if (FlagIndex != 0) e.Add(new XAttribute("FlagIndex", FlagIndex));
            if (!string.IsNullOrEmpty(FuncA)) e.Add(new XAttribute("FuncA", FuncA));
            if(!string.IsNullOrEmpty(FuncB)) e.Add(new XAttribute("FuncB", FuncB));
            if(DspIndex != -1) e.Add(new XAttribute("DspIndex", DspIndex));

            return e;
        }

        public FieldDescriptor(XElement e, int index, Dictionary<int, TypeDescriptor> typesByIndex)
        {
            Index = index;
            Name = e.OptionalStringAttribute("Name");
            Type = e.OptionalTypeAttribute("Type", typesByIndex);
            Offset = e.IntAttribute("Offset");
            Min = e.OptionalIntAttribute("Min");
            Max = e.OptionalIntAttribute("Max");
            Flags = e.IntAttribute("Flags");
            SubType = e.OptionalTypeAttribute("SubType", typesByIndex);
            VariableOffset = e.OptionalIntAttribute("VariableOffset");
            ArrayLength = e.OptionalIntAttribute("ArrayLength", -1);
            ArrayLengthOffset = e.OptionalIntAttribute("ArrayLengthOffset");
            EncodedBits = e.OptionalIntAttribute("EncodedBits");
            EncodedBits2 = e.OptionalIntAttribute("EncodedBits2");
            SnoType = e.OptionalIntAttribute("SnoType", -1);
            TagMapRelated = e.OptionalIntAttribute("TagMapRelated");
            var en = e.Element("Enum");
            if (en != null)
                EnumFields = en.Elements().Select(x => new Tuple<string, int>(x.Attribute("Name").Value, int.Parse(x.Attribute("Value").Value))).ToArray();
            FlagIndex = e.OptionalIntAttribute("FlagIndex");
            FuncA = e.OptionalStringAttribute("FuncA");
            FuncB = e.OptionalStringAttribute("FuncB");
            DspIndex = e.OptionalIntAttribute("DspIndex", -1);
        }

    }

}
