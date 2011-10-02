using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mooege.Core.Common.Items;

namespace Mooege.Core.Common.Items
{
    interface IItemAttributesCreator
    {
        void CreateAttributes(Item item);
    }
}
