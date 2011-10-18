using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace D3TypeDescriptor
{
    [TypeOverrideIgnore]
    public class GameMessageDescriptor : StructureTypeDescriptor
    {
        public int Size;
        public int[] NetworkIds;

        public override bool IsGameMessage
        {
            get
            {
                return true;
            }
        }

        public override void LoadXml(XElement e)
        {
            base.LoadXml(e);
            Size = e.IntAttribute("Size");
            NetworkIds = e.Attribute("NetworkIds").Value.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x)).ToArray();
        }

        public override System.Xml.Linq.XElement ToXml()
        {
            XElement e = base.ToXml();
            e.Name = "GameMessageDescriptor";            
            e.Add(new XAttribute("Size", Size));
            e.Add(new XAttribute("NetworkIds", string.Join(" ", NetworkIds)));
            return e;
        }

        public override void LoadFields(FieldDescriptor[] fields)
        {
            if(fields[0].Type.Name != "RequiredMessageHeader")
                throw new Exception("Expected RequiredMessageHeader.");
            fields = fields.Skip(1).ToArray();

            for (int i = 0; i < fields.Length; i++)
                fields[i].Index = i;

            base.LoadFields(fields);
        }

        public override void GenerateClass(StringBuilder b, int pad)
        {
            b.Append(' ', pad); b.AppendLine("public class " + Name + " : GameMessage");
            b.Append(' ', pad); b.AppendLine("{");
            GenerateFieldsAndFunctions(b, pad+4);
            b.Append(' ', pad); b.AppendLine("}");
        }

        public override void GenerateParseFunction(StringBuilder b, int pad)
        {
            b.Append(' ', pad); b.AppendLine("public override void Parse(GameBitBuffer buffer)");
            b.Append(' ', pad); b.AppendLine("{");
            GenerateParseBody(b, pad + 4, "buffer");
            b.Append(' ', pad); b.AppendLine("}");
            b.AppendLine();
        }

        public override void GenerateEncodeFunction(StringBuilder b, int pad)
        {
            b.Append(' ', pad); b.AppendLine("public override void Encode(GameBitBuffer buffer)");
            b.Append(' ', pad); b.AppendLine("{");
            GenerateEncodeBody(b, pad + 4, "buffer");
            b.Append(' ', pad); b.AppendLine("}");
            b.AppendLine();
        }
        
    }
}
