using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Map = Mooege.Core.GS.Map;
using Mooege.Common.MPQ.FileFormats;
using Mooege.Net.GS.Message;

namespace Mooege.Core.Common.Items.Implementations
{
    // A quick example of Type handling, HealthPotion will always override Potion (for Health potions ofc ;))!
    // fasbat
    [HandledType("HealthPotion")]
    public class HealthPotion : Potion
    {
        public HealthPotion(Map.World world, ItemTable definition)
            : base(world, definition)
        {
        }

        public override void OnRequestUse(GS.Players.Player player, Item target, int actionId, Net.GS.Message.Fields.WorldPlace worldPlace)
        {
            if (player.Attributes[GameAttribute.Hitpoints_Cur] == player.Attributes[GameAttribute.Hitpoints_Max])
                return; // TODO Error msg? /fasbat
            player.Attributes[GameAttribute.Hitpoints_Cur] =
                Math.Min(player.Attributes[GameAttribute.Hitpoints_Cur] + this.Attributes[GameAttribute.Hitpoints_Granted],
                player.Attributes[GameAttribute.Hitpoints_Max]);
            player.Attributes.SendChangedMessage(player.InGameClient, player.DynamicID); // TODO Send to all. /fasbat

            if (this.Attributes[GameAttribute.ItemStackQuantityLo] <= 1)
                player.Inventory.DestroyInventoryItem(this); // No more potions!
            else
            {
                this.Attributes[GameAttribute.ItemStackQuantityLo]--; // Just remove one
                this.Attributes.SendChangedMessage(player.InGameClient, this.DynamicID);
            }

        }
    }
}