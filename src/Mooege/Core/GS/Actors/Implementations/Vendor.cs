/*
 * Copyright (C) 2011 mooege project
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */

using Mooege.Core.GS.Common.Types.Math;
using Mooege.Core.GS.Map;
using Mooege.Net.GS.Message;
using Mooege.Net.GS.Message.Definitions.Trade;
using Mooege.Net.GS.Message.Definitions.World;

namespace Mooege.Core.GS.Actors.Implementations
{
    [HandledSNO(178396 /* Fence_In_Town_01? */)] //TODO this is just a test, do it properly for all vendors?
    public class Vendor : InteractiveNPC
    {
        public Vendor(World world, int actorSNO, Vector3D position)
            : base(world, actorSNO, position)
        {
            this.Attributes[GameAttribute.MinimapActive] = true;
        }

        public override void OnTargeted(Mooege.Core.GS.Player.Player player, TargetMessage message)
        {
            player.InGameClient.SendMessage(new OpenTradeWindowMessage((int)this.DynamicID));
        }
    }
}
