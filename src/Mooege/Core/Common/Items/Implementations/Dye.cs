using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mooege.Common.Helpers;
using System.Diagnostics;
using Mooege.Net.GS.Message;
using Mooege.Common;
using Mooege.Core.GS.Players;
using Mooege.Net.GS.Message.Fields;
using Mooege.Core.GS.Map;

namespace Mooege.Core.Common.Items.Implementations
{
    // This is how Dyes should be implemented ;) /fasbat
    [HandledType("Dye")]
    public class Dye : Item
    {
        private static Dictionary<int, int> DyeColorMap = new Dictionary<int,int>();

        public Dye(World world, Mooege.Common.MPQ.FileFormats.ItemTable definition)
            : base(world, definition)
        {
        }

        public override void OnRequestUse(Player player, Item target, int actionId, WorldPlace worldPlace)
        {
            Debug.Assert(target != null);

            target.Attributes[GameAttribute.DyeType] = this.Attributes[GameAttribute.DyeType];
            player.Inventory.DestroyInventoryItem(this);
            player.Inventory.SendVisualInventory(player); // TODO: Send it to all who see! /fasbat
        }
    }
}
