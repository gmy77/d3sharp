using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mooege.Core.Common.Items;

namespace Mooege.Core.Common.Items.ItemCreation
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
               creatorList.Add(new PotionAttributesCreator());
            }
            return creatorList;
        }

        
    }
}
