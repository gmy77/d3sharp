using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mooege.Core.Common.Items;
using Mooege.Net.GS.Message.Fields;
using Mooege.Net.GS.Message;

namespace Mooege.Core.Common.Items.ItemCreation
{
    class PotionAttributesCreator: IItemAttributesCreator
    {
        public void CreateAttributes(Item item)
        {            
            item.AttributeList.Add(new NetAttributeKeyValue { Attribute = GameAttribute.Attributes[82], Float = 250.0f, }); // Given Health 
        }
    }
}
