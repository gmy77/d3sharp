using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace D3TypeDescriptor
{
    [TypeOverrideIgnore]
    public class BasicTypeDescriptor : TypeDescriptor
    {
        public override bool IsBasicType
        {
            get
            {
                return true;
            }
        }

        public override System.Xml.Linq.XElement ToXml()
        {
            XElement e = base.ToXml();
            e.Name = "BasicDescriptor";
            return e;
        }
    }
}
