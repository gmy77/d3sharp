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
using D3Sharp.Net.BNet;

namespace D3Sharp.Core.Objects
{
    /// <summary>
    /// RPC Objects are 'objects' that can be referenced over client-server communication process by remote end.
    /// They've a DynamicId field that get's generated on memory instantiation.
    /// </summary>
    public class RPCObject
    {
        /// <summary>
        /// The dnymaicId of the project which is set on memory instantiation and changes over sessions.
        /// </summary>
        public ulong DynamicId { get; set; }

        /// <summary>
        /// List of client observers that subscribed for object update notifications.
        /// </summary>
        protected List<BNetClient> Subscribers { get; private set; }

        /// <summary>
        /// RPCObject ctor which can only be called by derived objects.
        /// </summary>
        protected RPCObject()
        {
            RPCObjectManager.Init(this); // let RPCObjectManager to generate new DynamicId for us.
            this.Subscribers = new List<BNetClient>();
        }       

        /// <summary>
        /// Adds a client subscriber to object, which will eventually be notified on any object updates.
        /// </summary>
        /// <param name="client">The observer.</param>
        /// <param name="remoteObjectId">The mapped remoteId</param>
        public void AddSubscriber(BNetClient client, ulong remoteObjectId)
        {
            client.MapLocalObjectID(this.DynamicId, remoteObjectId); // map observer's Id to to our dynamic Id so any further rpc-conversation over object, can benefit it.
            this.Subscribers.Add(client); // add client to subscribers list.
            this.NotifySubscriber(client); // when we add a new subscriber, he should get an initial notification on objects present state.
        }

        /// <summary>
        /// Notifies a specific subscriber about object's present state.
        /// This methods should be actually implemented by derived object.
        /// </summary>
        /// <param name="client">The observer</param>
        protected virtual void NotifySubscriber(BNetClient client) { }

        /// <summary>
        /// Notifies all observers of the object with present state of the object.
        /// </summary>
        public void NotifyAllSubscribers()
        {
            foreach (var subscriber in this.Subscribers)
            {
                this.NotifySubscriber(subscriber);
            }
        }
        
        #region de-ctor

        // IDisposable pattern: http://msdn.microsoft.com/en-us/library/fs2xkftw%28VS.80%29.aspx

        private bool _disposed = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this); // Take object out the finalization queue to prevent finalization code for it from executing a second time.
        }

        private void Dispose(bool disposing)
        {
            if (this._disposed) return; // if it's already disposed, just return.
            if (disposing)  { } // dispose any managed resources - where we don't have any.
            
            RPCObjectManager.Release(this); // release DynamicId.

            _disposed = true;
        }

        ~RPCObject() { Dispose(false); } // finalizer called by the runtime. we should only dispose unmanaged objects and should NOT reference managed ones.
        
        #endregion
    }
}
