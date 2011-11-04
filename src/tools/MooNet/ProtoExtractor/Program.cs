/*
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

// Contains code from: https://github.com/tomrus88/d3proto/blob/master/protod/Program.cs

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Google.ProtocolBuffers;
using EnumValue = Google.ProtocolBuffers.Descriptors.EnumValueDescriptor;
using FieldProto = Google.ProtocolBuffers.DescriptorProtos.FieldDescriptorProto;
using FieldType = Google.ProtocolBuffers.Descriptors.FieldType;
using FileProto = Google.ProtocolBuffers.DescriptorProtos.FileDescriptorProto;
using Label = Google.ProtocolBuffers.DescriptorProtos.FieldDescriptorProto.Types.Label;
using Type = Google.ProtocolBuffers.DescriptorProtos.FieldDescriptorProto.Types.Type;

namespace ProtoExtractor
{
    class Program
    {
        private static readonly List<FileProto> Protos = new List<FileProto>();

        private static readonly Dictionary<Label, string> Labels = new Dictionary<Label, string>
                                                                       {
                                                                           {default(Label), "unknown"},
                                                                           {Label.LABEL_OPTIONAL, "optional"},
                                                                           {Label.LABEL_REQUIRED, "required"},
                                                                           {Label.LABEL_REPEATED, "repeated"}
                                                                       };

        private static readonly Dictionary<Type, string> Types = new Dictionary<Type, string>
                                                                     {
                                                                         {default(Type), "unknown"},
                                                                         {Type.TYPE_DOUBLE, "double"},
                                                                         {Type.TYPE_FLOAT, "float"},
                                                                         {Type.TYPE_INT64, "int64"},
                                                                         {Type.TYPE_UINT64, "uint64"},
                                                                         {Type.TYPE_INT32, "int32"},
                                                                         {Type.TYPE_FIXED64, "fixed64"},
                                                                         {Type.TYPE_FIXED32, "fixed32"},
                                                                         {Type.TYPE_BOOL, "bool"},
                                                                         {Type.TYPE_STRING, "string"},
                                                                         {Type.TYPE_GROUP, "group"},
                                                                         {Type.TYPE_MESSAGE, "message"},
                                                                         {Type.TYPE_BYTES, "bytes"},
                                                                         {Type.TYPE_UINT32, "uint32"},
                                                                         {Type.TYPE_ENUM, "enum"},
                                                                         {Type.TYPE_SFIXED32, "sfixed32"},
                                                                         {Type.TYPE_SFIXED64, "sfixed64"},
                                                                         {Type.TYPE_SINT32, "sint32"},
                                                                         {Type.TYPE_SINT64, "sint64"}
                                                                     };

        static void Main(string[] args)
        {
            Console.WriteLine("ProtoBin Extractor started..");

            var files = Directory.GetFiles(".", "*.protobin", SearchOption.AllDirectories);
            Console.WriteLine("Found {0} protobin files.", files.Count());

            foreach (var file in files)
            {
                var proto = FileProto.ParseFrom(File.ReadAllBytes(file));
                Protos.Add(proto);
            }

            foreach (var proto in Protos)
                ParseProtoBin(proto);

            Console.WriteLine("Dumped {0} proto files.", files.Count());
            Console.ReadLine();
        }

        private static void ParseProtoBin(FileProto proto)
        {
            SaveAsText(proto);
            SaveDefinition(proto);
        }

        private static void SaveAsText(FileProto proto)
        {
            var path = "text\\" + proto.Name + ".txt";
            var dir = Path.GetDirectoryName(path);
            if (dir != null && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            using (var w = new StreamWriter(path))
            {
                proto.PrintTo(w);
            }
        }

        private static void SaveDefinition(FileProto proto)
        {
            var path = "definitions\\" + proto.Name;
            var dir = Path.GetDirectoryName(path);
            if (dir != null && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            using (var w = new StreamWriter(path))
            {
                foreach (var d in proto.DependencyList)
                {
                    w.WriteLine("import \"{0}\";", d);
                }

                if (proto.DependencyCount > 0)
                    w.WriteLine();

                if (proto.HasPackage)
                {
                    w.WriteLine("package {0};", proto.Package);
                    w.WriteLine();
                }

                if (proto.HasOptions)
                {
                    foreach (var o in proto.Options.AllFields)
                    {
                        if (o.Key.FieldType == FieldType.Enum)
                            w.WriteLine("option {0} = {1};", o.Key.Name, ((EnumValue)o.Value).Name);
                        else if (o.Key.FieldType == FieldType.String)
                            w.WriteLine("option {0} = \"{1}\";", o.Key.Name, o.Value);
                        else
                            w.WriteLine("option {0} = {1};", o.Key.Name, o.Value.ToString().ToLower());
                    }

                    w.WriteLine();
                }

                foreach (var m in proto.MessageTypeList)
                {
                    w.WriteLine("message {0}", m.Name);
                    w.WriteLine("{");

                    foreach (var n in m.NestedTypeList)
                    {
                        w.WriteLine("    message {0}", n.Name);
                        w.WriteLine("    {");
                        foreach (var ef in n.FieldList)
                        {
                            w.WriteLine("        {0} {1} {2} = {3};", Labels[ef.Label], GetTypeName(ef), ef.Name, ef.Number);
                        }
                        w.WriteLine("    }");
                        w.WriteLine();
                    }

                    foreach (var e in m.EnumTypeList)
                    {
                        w.WriteLine("    enum {0}", e.Name);
                        w.WriteLine("    {");
                        foreach (var ev in e.ValueList)
                        {
                            w.WriteLine("        {0} = {1};", ev.Name, ev.Number);
                        }
                        w.WriteLine("    }");
                        w.WriteLine();
                    }

                    //if (m.EnumTypeCount > 0)
                    //    w.WriteLine();

                    foreach (var f in m.FieldList)
                    {
                        if (f.HasDefaultValue)
                        {
                            w.WriteLine("    {0} {1} {2} = {3} [default = {4}];", Labels[f.Label], GetTypeName(f), f.Name, f.Number, f.Type == Type.TYPE_STRING ? string.Format("\"{0}\"", f.DefaultValue) : f.DefaultValue);
                        }
                        else
                        {
                            if (f.HasOptions && f.Options.HasPacked)
                            {
                                w.WriteLine("    {0} {1} {2} = {3} [packed={4}];", Labels[f.Label], GetTypeName(f), f.Name, f.Number, f.Options.Packed.ToString().ToLower());
                            }
                            else
                            {
                                w.WriteLine("    {0} {1} {2} = {3};", Labels[f.Label], GetTypeName(f), f.Name, f.Number);
                            }
                        }
                    }

                    //if (m.FieldCount > 0)
                    //    w.WriteLine();

                    foreach (var er in m.ExtensionRangeList)
                    {
                        w.WriteLine("    extensions {0} to {1};", er.Start,
                                    er.End == 0x20000000 ? "max" : er.End.ToString());
                    }

                    //if (m.ExtensionRangeCount > 0)
                    //    w.WriteLine();

                    foreach (var ext in m.ExtensionList)
                    {
                        w.WriteLine("    extend {0}", ext.Extendee);
                        w.WriteLine("    {");
                        {
                            w.WriteLine("        {0} {1} {2} = {3};", Labels[ext.Label], GetTypeName(ext), ext.Name, ext.Number);
                        }
                        w.WriteLine("    }");
                    }

                    w.WriteLine("}");
                    w.WriteLine();
                }

                foreach (var s in proto.ServiceList)
                {
                    w.WriteLine("service {0}", s.Name);
                    w.WriteLine("{");

                    foreach (var m in s.MethodList)
                    {
                        w.Write("    rpc {0}({1}) returns({2})", m.Name, m.InputType, m.OutputType);

                        if (m.HasOptions)
                        {
                            w.WriteLine();
                            w.WriteLine("    {");

                            foreach (var o in m.Options.UnknownFields.FieldDictionary)
                            {
                                var fdp = GetExtFieldDescriptorById(o.Key);

                                w.WriteLine("        option ({0}) = {1};", fdp.Name, GetValue(fdp, o.Value));
                            }
                            w.WriteLine("    }");
                        }
                        else
                            w.WriteLine(";");
                    }

                    w.WriteLine("}");
                    w.WriteLine();
                }

                foreach (var e in proto.EnumTypeList)
                {
                    w.WriteLine("enum {0}", e.Name);
                    w.WriteLine("{");
                    foreach (var ev in e.ValueList)
                    {
                        w.WriteLine("    {0} = {1};", ev.Name, ev.Number);
                    }
                    w.WriteLine("}");
                    w.WriteLine();
                }

                foreach (var ext in proto.ExtensionList)
                {
                    w.WriteLine("extend {0}", ext.Extendee);
                    w.WriteLine("{");
                    {
                        w.WriteLine("    {0} {1} {2} = {3};", Labels[ext.Label], GetTypeName(ext), ext.Name, ext.Number);
                    }
                    w.WriteLine("}");
                    w.WriteLine();
                }
            }
        }

        private static string GetTypeName(FieldProto ext)
        {
            return ext.HasTypeName ? ext.TypeName : Types[ext.Type];
        }

        private static object GetValue(FieldProto fdp, UnknownField unknownField)
        {
            if (unknownField.VarintList.Count > 0) return unknownField.VarintList[0];
            if (unknownField.Fixed32List.Count > 0)
            {
                if (fdp.Type == Type.TYPE_FLOAT)
                    return BitConverter.ToSingle(BitConverter.GetBytes(unknownField.Fixed32List[0]), 0);
                return unknownField.Fixed32List[0];
            }
            throw new Exception();
        }

        private static FieldProto GetExtFieldDescriptorById(int num)
        {
            return Protos.Where(p => p.ExtensionCount != 0)
                .SelectMany(p => p.ExtensionList)
                .FirstOrDefault(e => e.Number == num);
        }
    }
}
