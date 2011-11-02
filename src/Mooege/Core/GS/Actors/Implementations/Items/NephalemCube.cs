using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mooege.Core.Common.Items;
using Mooege.Common.MPQ.FileFormats;

namespace Mooege.Core.GS.Actors.Implementations.Items
{
    public class NephalemCube : Item
    {

        public NephalemCube(GS.Map.World world)
            : base(world, ItemGenerator.GetItemDefinition("NephalemCube"))
        {
        }

        public override void OnTargeted(Players.Player player, Net.GS.Message.Definitions.World.TargetMessage message)
        {
            player.EnableCubeOfNephalem();
            this.Destroy();
        }
    }
}
