using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using D3TypeDescriptor;
using System.Xml.Linq;

namespace D3ClassGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            NetAttribute.LoadXml("attributes.xml");

            XDocument doc = XDocument.Load("typedescriptors.xml");
            int protocolHash;
            var descriptors = TypeDescriptor.LoadXml(doc.Root, out protocolHash);


            var structs = TypeDescriptor.FilterGameMessageStructures(descriptors);

            foreach (var s in structs)
            {
                StringBuilder b = new StringBuilder();
                s.GenerateClass(b, 4);
                Console.WriteLine(b.ToString());
            }

        }
    }
}
