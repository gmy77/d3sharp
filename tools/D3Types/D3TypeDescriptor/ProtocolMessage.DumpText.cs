using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace D3TypeDescriptor
{
    public partial class ProtocolMessage
    {

    
        public void DumpAsText(TextWriter tw)
        {
            TabbedTextWriter.DumpMessage(this, tw);
        }

    }

    class TabbedTextWriter
    {
        TextWriter w;
        int tabs = 0;
        bool newLine = true;
        public bool TabMode = false;

        public TabbedTextWriter(TextWriter writer)
        {
            w = writer;
        }

        public void Begin()
        {
            tabs++;
        }
        public void End()
        {
            tabs--;
        }
        public void Write(string text)
        {
            if (newLine)
            {
                if(TabMode)
                    for (int i = 0; i < tabs; i++) w.Write("\t");
                else
                    for (int i = 0; i < tabs; i++) w.Write(" ");
                newLine = false;
            }
            w.Write(text);
        }
        public void WriteLine()
        {
            w.WriteLine();
            newLine = true;
        }
        public void WriteLine(string text)
        {
            Write(text);
            WriteLine();
        }

        public void Hexdump(byte[] blob)
        {
            for (int i = 0; i < blob.Length; i += 16)
            {
                Write(blob.Length > 0xFFFF ? i.ToString("X8") : i.ToString("X4"));
                Write(" ");

                for (int j = 0; j < 8; j++)
                {
                    int off = i + j;
                    if (off < blob.Length)
                        Write(blob[off].ToString("X2"));
                    else
                        Write("  ");
                    Write(" ");
                }
                Write(" ");

                for (int j = 0; j < 8; j++)
                {
                    int off = i + j + 8;
                    if (off < blob.Length)
                        Write(blob[off].ToString("X2"));
                    else
                        Write("  ");
                    Write(" ");
                }
                Write(" ");

                for (int j = 0; j < 8; j++)
                {
                    int off = i + j;
                    if (off < blob.Length)
                    {
                        byte b = blob[off];
                        if (b >= 0x20 && b < 0x80)
                            Write(((char)b).ToString());
                        else
                            Write(".");
                    }
                    else
                        Write(" ");
                }
                Write(" ");
                for (int j = 0; j < 8; j++)
                {
                    int off = i + j + 8;
                    if (off < blob.Length)
                    {
                        byte b = blob[off];
                        if (b >= 0x20 && b < 0x80)
                            Write(((char)b).ToString());
                        else
                            Write(".");
                    }
                    else
                        Write(" ");
                }
                WriteLine();

            }
        }


        #region DumpMessage
        internal static void DumpMessage(ProtocolMessage msg, TextWriter w)
        {
            TabbedTextWriter tw = new TabbedTextWriter(w);
            var values = msg.GetValues();
            var fields = msg.Descriptor.Fields;
            if (fields[0].Type.Name != "RequiredMessageHeader")
                throw new Exception("Expected RequiredMessageHeader");

            tw.WriteLine(string.Format("[{0}] {1}:{2:X4}", msg.Server ? "SERVER" : "CLIENT", msg.TypeName, msg.Id));
            tw.WriteLine("{");
            tw.Begin();
            for (int i = 1; i < fields.Length - 1; i++)
                DumpFieldValue(fields[i], i - 1, values[i], tw);
            if (msg.TypeName == "GenericBlobMessage")
                tw.Hexdump((byte[])values[values.Length - 1]);
            tw.End();
            tw.WriteLine("}");
            tw.WriteLine();
        }



        static void DumpFieldValue(FieldDescriptor field, int index, object value, TabbedTextWriter tw)
        {
            var fieldType = field.Type;

            if (fieldType.Name == "DT_OPTIONAL")
            {
                if (value == null)
                    return;
                fieldType = field.SubType;
            }


            tw.Write(field.GetFieldName() + ": ");

            if (fieldType.Fields != null && field.Type.Name != "DT_FIXEDARRAY")
            {
                DumpFields(field, value, tw, fieldType);
                return;
            }

            switch (fieldType.Name)
            {
                case "DT_FLOAT":
                case "DT_ANGLE":
                    tw.WriteLine(((float)value).ToString("G") + "f");
                    break;
                case "DT_INT": // could do more with min/max, checking for 1 bit integers etc
                case "DT_SNO":
                case "DT_GBID":
                case "DT_TIME":
                case "DT_ENUM":
                case "DT_DATAID":
                case "DT_SNO_GROUP":
                case "DT_SNONAME_HANDLE":
                    if ((field.Flags & 0x10) != 0)
                    {
                        if (field.Min == 0 && field.Max == 1)
                            tw.WriteLine((int)value != 0 ? "true" : "false");
                        else
                            tw.WriteLine(value.ToString());
                    }
                    else
                        tw.WriteLine("0x" + ((int)value).ToString("X8"));
                    break;
                case "DT_BYTE":
                    tw.WriteLine("0x" + ((byte)value).ToString("X2"));
                    break;
                case "DT_WORD":
                    tw.WriteLine("0x" + ((ushort)value).ToString("X4"));
                    break;
                case "DT_INT64":
                    tw.WriteLine("0x" + ((long)value).ToString("X16"));
                    break;
                case "DT_CHARARRAY":
                    tw.WriteLine("\"" + value + "\"");
                    break;
                case "DT_FIXEDARRAY":
                    tw.WriteLine();
                    if (field.SubType.Fields != null)
                    {
                        tw.WriteLine("{");
                        tw.Begin();
                        object[] array = (object[])value;
                        for (int i = 0; i < array.Length; i++)
                        {
                            tw.Write("[" + i + "]:");
                            DumpFields(field, array[i], tw, field.SubType);
                        }
                        tw.End();
                        tw.WriteLine("}");
                    }
                    else
                    {
                        var t = value.GetType();
                        if (t == typeof(byte[]))
                        {
                            tw.Hexdump((byte[])value);
                        }
                        else
                        {
                            tw.WriteLine("{");
                            tw.Begin();
                            int[] array = (int[])value;
                            for (int i = 0; i < array.Length; )
                            {
                                for (int j = 0; j < 8 && i < array.Length; j++, i++)
                                {
                                    tw.Write("0x" + array[i].ToString("X8") + ", ");
                                }
                                tw.WriteLine();
                            }
                            tw.End();
                            tw.WriteLine("}");
                        }
                    }
                    break;
                default:
                    throw new Exception("Unhandled field type: " + fieldType.Name);
            }
        }

        private static void DumpFields(FieldDescriptor field, object value, TabbedTextWriter tw, TypeDescriptor fieldType)
        {
            tw.WriteLine("// " + fieldType.Name);
            tw.WriteLine("{");
            tw.Begin();
            object[] values = (object[])value;
            var fields = fieldType.Fields;
            for (int i = 0; i < fields.Length - 1; i++)
                DumpFieldValue(fields[i], i, values[i], tw);
            if (fieldType.Name == "NetAttributeKeyValue")
            {
                object v = values[values.Length - 1];
                int id = (int)values[values.Length - 2];
                var a = NetAttribute.Attributes[id];
                tw.Write("Value: ");
                if (a.IsInteger)
                {
                    int x = (int)v;
                    if (a.EncodingType == NetAttributeEncoding.IntMinMax)
                    {
                        if (a.Min == 0 && a.Max == 1)
                            tw.Write(x != 0 ? "true" : "false");
                        else
                            tw.Write(x.ToString());
                    }
                    else
                        tw.Write("0x" + x.ToString("X8"));
                }
                else
                {
                    tw.Write(((float)v).ToString("G") + "f");
                }
                tw.WriteLine(" // " + a.Name);
            }
            tw.End();
            tw.WriteLine("}");
        }
        #endregion
    }

}
