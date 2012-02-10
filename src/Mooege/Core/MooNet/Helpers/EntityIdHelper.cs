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

namespace Mooege.Core.MooNet.Helpers
{
    public static class EntityIdHelper
    {
        /// <summary>
        /// Returns high-id type for given bnet.protocol.EntityId
        /// </summary>
        /// <param name="id">The bnet.protocol.EntityId</param>
        /// <returns><see cref="HighIdType"/></returns>
        public static HighIdType GetHighIdType(this bnet.protocol.EntityId id)
        {
            switch (id.High >> 48)
            {
                case 0x0100:
                    return HighIdType.AccountId;
                case 0x0200:
                    return HighIdType.GameAccountId;
                case 0x0000:
                    return HighIdType.ToonId;
                case 0x0600:
                    return HighIdType.ChannelId;
            }
            return HighIdType.Unknown;
        }

        /// <summary>
        /// High id types for bnet.protocol.EntityId high-id.
        /// </summary>
        public enum HighIdType : ulong
        {
            Unknown = 0x0,
            AccountId = 0x100000000000000,
            GameAccountId = 0x200000000000000,
            ToonId = 0x000000000000000,
            GameId = 0x600000000000000,
            ChannelId = 0x600000000000000
        }
    }
}
