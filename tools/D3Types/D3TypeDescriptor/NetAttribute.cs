using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace D3TypeDescriptor
{

    public enum NetAttributeEncoding
    {
        Int,
        IntMinMax,
        //FloatMinMax,
        Float16,
        Float16Or32,
    }

    public class NetAttribute
    {
        public int Id;
        public int U2;
        public int U3;
        public int U4;
        public int U5;

        public string ScriptA;
        public string ScriptB;
        public string Name;

        public NetAttributeEncoding EncodingType;

        public byte U10;

        public int Min;
        public int Max;
        public int BitCount;

        public bool IsInteger { get { return EncodingType == NetAttributeEncoding.Int || EncodingType == NetAttributeEncoding.IntMinMax; } }

        public NetAttribute()
        {
        }

        public NetAttribute(int id, int u2, int u3, int u4, int u5, string scriptA, string scriptB, string name, NetAttributeEncoding encodingType, byte u10, int min, int max, int bitCount)
        {
            Id = id;
            U2 = u2;
            U3 = u3;
            U4 = u4;
            U5 = u5;
            ScriptA = scriptA;
            ScriptB = scriptB;
            Name = name;
            EncodingType = encodingType;
            U10 = u10;

            Min = min;
            Max = max;
            BitCount = bitCount;
        }

        public static NetAttribute[] Attributes;

        public static void SaveXml(string filename)
        {
            XElement root = new XElement("Attributes");
            root.Add(new XAttribute("Count", Attributes.Length));

            for (int i = 0; i < Attributes.Length; i++)
            {
                var a = Attributes[i];
                root.Add(new XElement("Entry",
                        new XAttribute("Id", a.Id),
                        new XAttribute("U2", a.U2),
                        new XAttribute("U3", a.U3),
                        new XAttribute("U4", a.U4),
                        new XAttribute("U5", a.U5),
                        new XAttribute("ScriptA", a.ScriptA),
                        new XAttribute("ScriptB", a.ScriptB),
                        new XAttribute("Name", a.Name),
                        new XAttribute("EncodingType", a.EncodingType.ToString()),
                        new XAttribute("U10", a.U10),

                        new XAttribute("Min", a.Min),
                        new XAttribute("Max", a.Max),
                        new XAttribute("BitCount", a.BitCount)
                    ));
            }
            XDocument doc = new XDocument();
            doc.Add(root);
            doc.Save(filename);
        }

        public static void LoadXml(string filename)
        {
            XDocument doc = XDocument.Load(filename);
            var root = doc.Root;
            int count = int.Parse(root.Attribute("Count").Value);
            Attributes = new NetAttribute[count];

            foreach (var e in root.Elements())
            {
                NetAttribute a = new NetAttribute();
                a.Id = int.Parse(e.Attribute("Id").Value);
                a.U2 = int.Parse(e.Attribute("U2").Value);
                a.U2 = int.Parse(e.Attribute("U3").Value);
                a.U2 = int.Parse(e.Attribute("U4").Value);
                a.U2 = int.Parse(e.Attribute("U5").Value);

                a.ScriptA = e.Attribute("ScriptA").Value;
                a.ScriptB = e.Attribute("ScriptB").Value;
                a.Name = e.Attribute("Name").Value;

                a.EncodingType = (NetAttributeEncoding)Enum.Parse(typeof(NetAttributeEncoding), e.Attribute("EncodingType").Value);

                a.U10 = (byte)int.Parse(e.Attribute("U10").Value);
                a.Min = int.Parse(e.Attribute("Min").Value);
                a.Max = int.Parse(e.Attribute("Max").Value);
                a.BitCount = int.Parse(e.Attribute("BitCount").Value);

                Attributes[a.Id] = a;
            }
        }
    }
}
