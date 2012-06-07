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

namespace Mooege.Core.MooNet.Objects
{
    public static class RPCObjectManager
    {
        private static readonly Logger Logger = LogManager.CreateLogger();

        private static ulong _nextId = 0;
        public static readonly Dictionary<ulong, RPCObject> Objects = new Dictionary<ulong, RPCObject>();

        static RPCObjectManager()
        { }

        public static void Init(RPCObject obj)
        {
            if (Objects.ContainsKey(obj.DynamicId))
                throw new Exception("Given object was already initialized");
            ulong id = Next();
            obj.DynamicId = id;
            Objects.Add(id, obj);
        }

        public static void Release(RPCObject obj)
        {
            Logger.Trace("Releasing object {0}", obj.DynamicId);
            Objects.Remove(obj.DynamicId);
        }

        public static ulong Next()
        {
            while (Objects.ContainsKey(++_nextId)) ;
            return _nextId;
        }
    }
}
