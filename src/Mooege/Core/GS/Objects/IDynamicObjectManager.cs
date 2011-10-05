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

namespace Mooege.Core.GS.Objects
{
    public interface IDynamicObjectManager
    {
        /// <summary>
        /// Get a new dynamic ID for a world.
        /// </summary>
        uint NewWorldID { get; }

        // NOTE: Dynamic IDs for scenes might be separate from global object IDs (per World)
        /// <summary>
        /// Get a new dynamic ID for a scene.
        /// </summary>
        uint NewSceneID { get; }

        /// <summary>
        /// Get a new dynamic ID for a player.
        /// </summary>
        uint NewPlayerID { get; }

        /// <summary>
        /// Get a new dynamic ID for any actor.
        /// </summary>
        uint NewActorID { get; }

        /// <summary>
        /// Get a new dynamic ID for an NPC.
        /// </summary>
        uint NewNPCID { get; }

        /// <summary>
        /// Get a new dynamic ID for a mob.
        /// </summary>
        uint NewMonsterID { get; }

        /// <summary>
        /// Get a new dynamic ID for an item.
        /// </summary>
        uint NewItemID { get; }

        bool HasWorld(uint dynamicID);
        bool HasPlayer(uint dynamicID);
        bool HasActor(uint dynamicID);
        bool HasNPC(uint dynamicID);
        bool HasMonster(uint dynamicID);
        bool HasItem(uint dynamicID);
    }
}
