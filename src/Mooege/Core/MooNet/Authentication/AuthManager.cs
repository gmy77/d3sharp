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
using System.Linq;
using System.Text;
using Mooege.Net.MooNet;

namespace Mooege.Core.MooNet.Authentication
{
    public static class AuthManager
    {
        public static void HandleAuthResponse(MooNetClient client, SRP6 srp6, byte[] authMessage)
        {
            byte[] A = authMessage.Skip(1).Take(128).ToArray();
            byte[] M1 = authMessage.Skip(1 + 128).Take(32).ToArray();
            byte[] seed = authMessage.Skip(1 + 32 + 128).Take(128).ToArray();

            
        }
    }
}
