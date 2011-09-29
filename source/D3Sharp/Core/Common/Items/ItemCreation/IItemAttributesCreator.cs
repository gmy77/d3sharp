using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using D3Sharp.Core.Common.Items;

namespace D3Sharp.Core.Items
{
    interface IItemAttributesCreator
    {
        void CreateAttributes(Item item);
    }
}
