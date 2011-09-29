using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using D3Sharp.Core.Common.Items;

namespace D3Sharp.Core.Items.ItemCreation
{
    class PotionAttributesCreator: IItemAttributesCreator
    {
        IItemAttributesCreator creator;

        PotionAttributesCreator(IItemAttributesCreator creator)
        {
            this.creator = creator;
        }

        public void CreateAttributes(Item item)
        {
            creator.CreateAttributes(item);

            item.AttributeList.Add(new NetAttribute(82,250.0f)); // points healed
        }
    }
}
