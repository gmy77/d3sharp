using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mooege.Core.GS.Powers
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PowerImplementationAttribute : Attribute
    {
        public int Id;

        public PowerImplementationAttribute(int id)
        {
            Id = id;
        }
    }
}
