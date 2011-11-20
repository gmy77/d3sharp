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
using System.Text;
using System.Xml.Linq;

namespace D3TypeDescriptor
{
    public class TypeOverrideIgnoreAttribute : Attribute
    {
    }

    public abstract class TypeDescriptor
    {
        public int Index;
        public string Name;
        public int UnkValue;

        public virtual bool IsBasicType { get { return false; } }
        public virtual bool IsStructure { get { return false; } }
        public virtual bool IsGameMessage { get { return false; } }

        static Dictionary<string, Type> _BasicOverrides = new Dictionary<string, Type>();
        static Dictionary<string, Type> _GameMessageOverrides = new Dictionary<string, Type>();
        static Dictionary<string, Type> _StructureOverrides = new Dictionary<string, Type>();

        static TypeDescriptor()
        {
            var asm = typeof(TypeDescriptor).Assembly;
            foreach (var t in asm.GetTypes())
            {
                if (!t.IsClass || !t.IsSubclassOf(typeof(TypeDescriptor)))
                    continue;
                var attribs = t.GetCustomAttributes(typeof(TypeOverrideIgnoreAttribute), false);
                if (attribs != null && attribs.Length > 0)
                    continue;

                if (t.IsSubclassOf(typeof(GameMessageDescriptor)))
                    _GameMessageOverrides.Add(t.Name, t);
                else if (t.IsSubclassOf(typeof(StructureTypeDescriptor)))
                    _StructureOverrides.Add(t.Name, t);
                else if (t.IsSubclassOf(typeof(BasicTypeDescriptor)))
                    _BasicOverrides.Add(t.Name, t);
                else
                {
                    System.Diagnostics.Debug.WriteLine("Unhandled override: " + t.Name);
                }
            }
        }

        public static BasicTypeDescriptor AllocateBasicType(string name)
        {
            Type t;
            if (!_BasicOverrides.TryGetValue(name, out t))
                throw new Exception("Unhandled basic type: " + name);
            return (BasicTypeDescriptor)Activator.CreateInstance(t);
        }

        public static GameMessageDescriptor AllocateGameMessage(string name)
        {
            Type t;
            if (!_GameMessageOverrides.TryGetValue(name, out t))
                return new GameMessageDescriptor();
            return (GameMessageDescriptor)Activator.CreateInstance(t);
        }

        public static StructureTypeDescriptor AllocateStructure(string name)
        {
            Type t;
            if (!_StructureOverrides.TryGetValue(name, out t))
                return new StructureTypeDescriptor();
            return (StructureTypeDescriptor)Activator.CreateInstance(t);
        }
        
        public static TypeDescriptor[] LoadXml(XElement root, out int protocolHash)
        {
            protocolHash = root.IntAttribute("ProtocolHash");

            Dictionary<int, TypeDescriptor> types = new Dictionary<int, TypeDescriptor>();
            foreach (var e in root.Elements())
            {
                string name = e.Attribute("Name").Value;
                TypeDescriptor desc;
                if (e.Name == "StructureDescriptor")
                    desc = AllocateStructure(name);
                else if (e.Name == "GameMessageDescriptor")
                    desc = AllocateGameMessage(name);
                else if (e.Name == "BasicDescriptor")
                    desc = AllocateBasicType(name);
                else
                    throw new Exception("Unhandled xml element: " + e.Name);

                desc.LoadXml(e);
                types.Add(desc.Index, desc);
            }

            foreach (var e in root.Elements())
            {
                var t = types[e.IntAttribute("Index")];
                int n = 0;
                var fields = e.Elements().Select(x => new FieldDescriptor(x, n++, types)).ToArray();
                if (fields.Length > 0)
                    t.LoadFields(fields);
            }
            return types.Values.ToArray();
        }

        
        static void ExploreType(StructureTypeDescriptor structure, HashSet<TypeDescriptor> explored)
        {
            var fields = structure.Fields;

            foreach (var f in fields)
            {
                if (f.Type != null && f.Type.IsStructure && explored.Add(f.Type))
                    ExploreType((StructureTypeDescriptor)f.Type, explored);
                if (f.SubType != null && f.SubType.IsStructure && explored.Add(f.SubType))
                    ExploreType((StructureTypeDescriptor)f.SubType, explored);
            }
        }

        public static TypeDescriptor[] FilterGameMessageStructures(TypeDescriptor[] types)
        {
            HashSet<TypeDescriptor> explored = new HashSet<TypeDescriptor>();

            List<GameMessageDescriptor> list = new List<GameMessageDescriptor>();
            foreach (var t in types)
            {
                if(t.IsGameMessage && explored.Add(t))
                    ExploreType((StructureTypeDescriptor)t, explored);
            }
            return explored.ToArray();
        }


        public virtual void LoadXml(XElement e)
        {
            Index = e.IntAttribute("Index");
            Name = e.Attribute("Name").Value;
            UnkValue = e.OptionalIntAttribute("UnkValue");
        }
        public virtual void LoadFields(FieldDescriptor[] fields) { throw new Exception("This type doesnt handle fields."); }

        public virtual XElement ToXml()
        {
            XElement e = new XElement("TypeDescriptor");
            e.Add(new XAttribute("Index", Index));
            e.Add(new XAttribute("Name", Name));
            if(UnkValue != 0)
                e.Add(new XAttribute("UnkValue", UnkValue));
            return e;
        }

        public virtual void GenerateClass(StringBuilder b, int pad)
        {
            throw new NotImplementedException();
        }

        public virtual void GenerateField(StringBuilder b, int pad, FieldDescriptor f) { throw new NotImplementedException();  }
        public virtual void GenerateParseBitBuffer(StringBuilder b, int pad, FieldDescriptor f, string bitBufferName) { throw new NotImplementedException(); }
        public virtual void GenerateEncodeBitBuffer(StringBuilder b, int pad, FieldDescriptor f, string bitBufferName) { throw new NotImplementedException(); }

        public virtual void GenerateOptionalField(StringBuilder b, int pad, FieldDescriptor f) { throw new NotImplementedException(); }
        public virtual void GenerateOptionalParseBitBuffer(StringBuilder b, int pad, FieldDescriptor f, string bitBufferName) { throw new NotImplementedException(); }
        public virtual void GenerateOptionalEncodeBitBuffer(StringBuilder b, int pad, FieldDescriptor f, string bitBufferName) { throw new NotImplementedException(); }

        public virtual void GenerateFixedArrayField(StringBuilder b, int pad, FieldDescriptor f) { throw new NotImplementedException(); }
        public virtual void GenerateFixedArrayParseBitBuffer(StringBuilder b, int pad, FieldDescriptor f, string bitBufferName) { throw new NotImplementedException(); }
        public virtual void GenerateFixedArrayEncodeBitBuffer(StringBuilder b, int pad, FieldDescriptor f, string bitBufferName) { throw new NotImplementedException(); }

    }
    
}
