using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using D3Sharp.Core.Common.Items;

namespace D3Sharp.Core.Items.ItemCreation
{
    class AttributesCreatorFactory
    {

        public List<IItemAttributesCreator> create(ItemType itemType)        
        {
            List<IItemAttributesCreator> creatorList =new List<IItemAttributesCreator>();
            creatorList.Add(new DefaultAttributeCreator());

            if (Item.isWeapon(itemType)){
               creatorList.Add(new WeaponAttributesCreator());
            }else if(Item.isPotion(itemType)){
                // TODO: implement me
            }
            return creatorList;
        }

        
    }
}
