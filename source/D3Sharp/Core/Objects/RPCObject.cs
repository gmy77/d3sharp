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
    /// RPC objects are mapped to a remote dynamic ID for the purposes of handling subscriptions
    /// and server-side data.
    /// </summary>
    public class RPCObject
    {
        /// <summary>
        /// The dynamic ID of the object, which is set on memory instantiation and changes over sessions.
        /// RPCObjectManager will track all dynamic IDs so that we don't get a duplicate.
        /// </summary>
        public ulong DynamicId { get; set; }

        /// <summary>
        /// List of clients that subscribed for notifications when this object updates its states.
        /// </summary>
        protected List<BNetClient> Subscribers { get; private set; }

        /// <summary>
        /// Constructor which can only be called by derived objects.
        /// </summary>
        protected RPCObject()
        {
            // Let RPCObjectManager generate new dynamid ID for us
            RPCObjectManager.Init(this);
            this.Subscribers = new List<BNetClient>();
        }       

        /// <summary>
        /// Adds a client subscriber to object, which will eventually be notified whenever the object changes state.
        /// </summary>
        /// <param name="client">The subscriber.</param>
        /// <param name="remoteObjectId">The client's dynamic ID.</param>
        public void AddSubscriber(BNetClient client, ulong remoteObjectId)
        {
            // Map the subscriber's dynamic ID to to our dynamic ID so we know how to translate later on when the object makes a notify call
            client.MapLocalObjectID(this.DynamicId, remoteObjectId);
            this.Subscribers.Add(client);
            // Since the client wasn't previously subscribed, it should not be aware of the object's state -- let's notify it
            this.NotifySubscriber(client);
        }

        /// <summary>
        /// Notifies a specific subscriber about the object's present state.
        /// This methods should be actually implemented by deriving object classes.
        /// </summary>
        /// <param name="client">The subscriber.</param>
        protected virtual void NotifySubscriber(BNetClient client) { }

        /// <summary>
        /// Notifies all subscribers with the object's current state.
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
            // Take object out the finalization queue to prevent finalization code for it from executing a second time
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (this._disposed) return; // If it's already disposed, just get out of here
            if (disposing)  { } // Dispose any managed resources - here we don't have any
            
            RPCObjectManager.Release(this); // Release our dynamic ID

            _disposed = true;
        }

        ~RPCObject()
        {
            // Finalizer called by the runtime. We should only dispose unmanaged objects and should NOT reference managed ones
            Dispose(false);
        }
        
        #endregion
    }
}
