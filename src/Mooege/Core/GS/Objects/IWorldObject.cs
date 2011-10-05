﻿/*
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

using Mooege.Core.GS.Game;
using Mooege.Core.GS.Map;
using Mooege.Net.GS.Message.Fields;

// NOTE: This interface can probably inherit IDynamicObject

namespace Mooege.Core.GS.Objects
{
    public interface IWorldObject
    {
        /// <summary>
        /// The object's world.
        /// </summary>
        World World { get; set; }

        float Scale { get; set; }
        float RotationAmount { get; set; }
        Vector3D RotationAxis { get; set; }
        Vector3D Position { get; set; }

        void Reveal(Player player);
        void Destroy(Player player);
    }
}
