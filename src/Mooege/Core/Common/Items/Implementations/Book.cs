using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Map = Mooege.Core.GS.Map;
using Mooege.Common.MPQ.FileFormats;
using Mooege.Net.GS.Message;

namespace Mooege.Core.Common.Items.Implementations
{
    [HandledType("Book")]
    public class Book : Item
    {
        public Book(Map.World world, ItemTable definition)
            : base(world, definition)
        {
        }
    }
}
