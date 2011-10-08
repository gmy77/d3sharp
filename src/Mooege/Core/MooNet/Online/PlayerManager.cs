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

using System;
using System.Collections.Generic;
using Mooege.Net.MooNet;

namespace Mooege.Core.MooNet.Online
{
    // probably will not need this when we actually send players from last game to recent players window.
    public static class PlayerManager
    {
        private static readonly List<MooNetClient> OnlinePlayersList = new List<MooNetClient>();

        public static List<MooNetClient> OnlinePlayers 
        {
            get { return OnlinePlayersList; } // just provide a setter so the actual list can not be modified.
        }

        public static void PlayerConnected(MooNetClient client)
        {
            OnlinePlayersList.Add(client);
        }

        public static void PlayerDisconnected(MooNetClient client)
        {
            OnlinePlayersList.Remove(client);
        }
    }
}
