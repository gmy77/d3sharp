using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Map = Mooege.Core.GS.Map;
using Mooege.Common.MPQ.FileFormats;
using Mooege.Net.GS.Message;

namespace Mooege.Core.Common.Items.Implementations
{
    // A quick example of type handling. /fasbat
    [HandledType("Potion")]
    public class Potion : Item
    {
        public Potion(Map.World world, ItemTable definition)
            : base(world, definition)
        {
            Attributes[GameAttribute.ItemStackQuantityLo] = 1;
        }
    }
}
