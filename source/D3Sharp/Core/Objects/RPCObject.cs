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

using System.Collections.Generic;
using System.Linq;
using D3Sharp.Net.BNet;
using Google.ProtocolBuffers;

namespace D3Sharp.Core.Objects
{
    public class RPCObject
    {
        public bool Initialized { get; private set; }
        public ulong LocalObjectId { get; set; }

        public List<BNetClient> Subscribers { get; private set; }

        public RPCObject()
        {
            ObjectManager.Init(this);
            this.Initialized = true;
            this.Subscribers = new List<BNetClient>();
        }       

        public void AddSubscriber(BNetClient client, ulong remoteObjectId)
        {
            client.MapLocalObjectID(this.LocalObjectId, remoteObjectId);
            this.Subscribers.Add(client);
            this.NotifySubscriber(client);
        }

        public virtual void NotifySubscriber(BNetClient client) { }
        public virtual void NotifyAllSubscriber() { }

        public void Release()
        {
            ObjectManager.Release(this);
            this.Initialized = false;
        }
        
        ~RPCObject()
        {
            Release();
        }
    }
}
