using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using D3Sharp.Core.Common.Items;

namespace D3Sharp.Core.Items.ItemCreation
{
    class DefaultAttributeCreator : IItemAttributesCreator
    {

        public void CreateAttributes(Item item)
        {
            item.AttributeList.Add(new NetAttribute(0x0115, 1)); // Itemquality 
            item.AttributeList.Add(new NetAttribute(0x0125, unchecked((int)2286800181))); // Seed            
        }
    }
}
