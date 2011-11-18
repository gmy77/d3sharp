using System;
using System.Collections.Generic;
using System.IO;
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

            var writer = new StreamWriter("output.cs");

            foreach (var s in structs)
            {
                var b = new StringBuilder();
                s.GenerateClass(b, 4);
                writer.WriteLine(b.ToString());
            }

            writer.Close();

            writer = new StreamWriter("attrs.cs");
            var builder = new StringBuilder();
            NetAttribute.GenerateClass(builder);
            writer.WriteLine(builder.ToString());
            writer.Close();

            Console.WriteLine("done");
            Console.ReadLine();
        }
    }
}
