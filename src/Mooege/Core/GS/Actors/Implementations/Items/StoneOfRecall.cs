using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mooege.Core.Common.Items;
using Mooege.Common.MPQ.FileFormats;

namespace Mooege.Core.GS.Actors.Implementations.Items
{
    class StoneOfRecall : Item
    {
        public StoneOfRecall(GS.Map.World world)
            : base(world, ItemGenerator.GetItemDefinition("StoneOfRecall"))
        {
        }

        public override void OnTargeted(Players.Player player, Net.GS.Message.Definitions.World.TargetMessage message)
        {
            player.EnableStoneOfRecall();
            this.Destroy();
        }
    }
}
