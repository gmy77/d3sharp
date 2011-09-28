/*
 * Copyright (C) 2011 D3Sharp Project
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
using D3Sharp.Utils;

namespace D3Sharp.Core.BNet.Objects
{
    public static class RPCObjectManager
    {
        private static readonly Logger Logger = LogManager.CreateLogger();

        private static ulong _nextId=0;
        private static Dictionary<ulong, RPCObject> Objects = new Dictionary<ulong, RPCObject>();

        static RPCObjectManager()
        {
            Objects[0] = null;
        }

        public static void Init(RPCObject obj)
        {
            if (obj.DynamicId != 0 && Objects.ContainsKey(obj.DynamicId))
                throw new Exception("Given object was already initialized");
            ulong id = Next();
            obj.DynamicId = id;
            Objects.Add(id, obj);
        }
        
        public static void Release(RPCObject obj)
        {
            Logger.Debug("Releasing object {0}", obj.DynamicId);
            /*if (obj.Initialized)
            {
                if (obj.DynamicId == 0 || !Objects.ContainsKey(obj.DynamicId))
                    throw new Exception("Given object was already released");
                Objects.Remove(obj.DynamicId);
            }
            else
            {
                Logger.Debug("RPCObject with ID={0} tried to release twice", obj.DynamicId);
            }*/
        }
        
        public static ulong Next()
        {
            while (Objects.ContainsKey(++_nextId));
            return _nextId;
        }
    }
}
