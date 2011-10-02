using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mooege.Core.Common.Items;
using Mooege.Net.GS.Message.Fields;
using Mooege.Net.GS.Message;

namespace Mooege.Core.Common.Items.ItemCreation
{
    class DefaultAttributeCreator : IItemAttributesCreator
    {

        public void CreateAttributes(Item item)
        {
            item.AttributeList.Add(new NetAttributeKeyValue{ Attribute = GameAttribute.Attributes[0x0115], Int = 1,}); // Itemquality 
            item.AttributeList.Add(new NetAttributeKeyValue{ Attribute = GameAttribute.Attributes[0x0125], Int = unchecked((int)2286800181),}); // Seed            
        }
    }
}
