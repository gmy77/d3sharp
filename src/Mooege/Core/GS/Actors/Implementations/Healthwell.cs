/*
 * Copyright (C) 2011 - 2012 mooege project - http://www.mooege.org
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

using Mooege.Core.GS.Map;
using Mooege.Core.GS.Common.Types.TagMap;
using Mooege.Net.GS.Message;

namespace Mooege.Core.GS.Actors.Implementations
{
    /// <summary>
    /// Class that implements healthwell, run power on click and change gizmo state
    /// </summary>
    class Healthwell : Gizmo
    {
        public Healthwell(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
            Attributes[GameAttribute.MinimapActive] = true;
            Attributes[GameAttribute.Gizmo_State] = 0;
        }


        public override void OnTargeted(Players.Player player, Net.GS.Message.Definitions.World.TargetMessage message)
        {
            Logger.Warn("Healthwell has no function, Powers not implemented");

            this.Attributes[GameAttribute.Gizmo_Has_Been_Operated] = true;
            this.Attributes[GameAttribute.Gizmo_Operator_ACDID] = unchecked((int)player.DynamicID);
            this.Attributes[GameAttribute.Gizmo_State] = 1;
            Attributes.BroadcastChangedIfRevealed();
        }
    }
}
