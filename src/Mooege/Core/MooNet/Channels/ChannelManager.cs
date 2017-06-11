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

using System;
using System.Collections.Generic;
using Mooege.Common.Logging;
using Mooege.Core.MooNet.Helpers;
using Mooege.Net.MooNet;

namespace Mooege.Core.MooNet.Channels
{
    public static class ChannelManager
    {
        private static readonly Logger Logger = LogManager.CreateLogger();

        public readonly static Dictionary<ulong, Channel> Channels =
            new Dictionary<ulong, Channel>();

        public static Channel CreateNewChannel(MooNetClient client, ulong remoteObjectId)
        {
            var channel = new Channel(client, false, remoteObjectId);
            Channels.Add(channel.DynamicId, channel);
            return channel;
        }

        public static void AddGameChannel(Channel channel)
        {
            Channels.Add(channel.DynamicId, channel);
        }

        public static void DissolveChannel(ulong id)
        {
            Logger.Debug("Dissolving channel {0}", id);
            if (!Channels.ContainsKey(id))
            {
                Logger.Warn("Attempted to delete a non-existent channel with ID {0}", id);
                return;
            }
            var channel = Channels[id];
            channel.RemoveAllMembers(true);
            Channels.Remove(id);
        }

        public static Channel GetChannelByEntityId(bnet.protocol.EntityId entityId)
        {
            if (entityId.GetHighIdType() == EntityIdHelper.HighIdType.ChannelId)
            {
                if (Channels.ContainsKey(entityId.Low))
                    return Channels[entityId.Low];
            }
            else
                Logger.Warn("Given entity ID doesn't look like a channel ID!");
            return null;
        }

        public static Channel GetChannelByEntityId(D3.OnlineService.EntityId entityId)
        {
            if (entityId.IdHigh == (ulong)EntityIdHelper.HighIdType.ChannelId)
            {
                if (Channels.ContainsKey(entityId.IdLow))
                    return Channels[entityId.IdLow];
            }
            else
                Logger.Warn("Given entity ID doesn't look like a channel ID!");
            return null;
        }

        public static Channel GetChannelByDynamicId(ulong dynamicId)
        {
            if (!Channels.ContainsKey(dynamicId))
                throw new Exception(string.Format("No channel exists with given dynamic id: {0}", dynamicId));
            else
                return Channels[dynamicId];
        }
    }
}
