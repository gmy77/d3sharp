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
using Mooege.Common;
using Mooege.Common.Logging;
using Mooege.Net.MooNet;

// FIXME: An RPCObject will never get released at runtime because we don't remove it from
// RPCObjectManager until the dtor actually gets called. The dtor, of course, never gets
// called during runtime because the object is still referenced in RPCObjectManager.Objects.

// TODO: RPCObject should probably remove all subscribers when getting released.

namespace Mooege.Core.MooNet.Objects
{
    /// <summary>
    /// RPC objects are mapped to a remote dynamic ID for the purposes of handling subscriptions
    /// and server-side data.
    /// </summary>
    public class RPCObject
    {
        protected static readonly Logger Logger = LogManager.CreateLogger();
        
        /// <summary>
        /// The dynamic ID of the object, which is set on memory instantiation and changes over sessions.
        /// RPCObjectManager will track all dynamic IDs so that we don't get a duplicate.
        /// </summary>
        public ulong DynamicId { get; set; }

        /// <summary>
        /// List of clients that subscribed for notifications when this object updates its states.
        /// </summary>
        public List<MooNetClient> Subscribers { get; private set; }

        /// <summary>
        /// Constructor which can only be called by derived objects.
        /// </summary>
        protected RPCObject()
        {
            // Let RPCObjectManager generate a new dynamic ID for us
            RPCObjectManager.Init(this);
            this.Subscribers = new List<MooNetClient>();
        }       

        /// <summary>
        /// Adds a client subscriber to object, which will eventually be notified whenever the object changes state.
        /// </summary>
        /// <param name="client">The client to add as a subscriber.</param>
        /// <param name="remoteObjectId">The client's dynamic ID.</param>
        public void AddSubscriber(MooNetClient client, ulong remoteObjectId)
        {
            // Map the subscriber's dynamic ID to to our dynamic ID so we know how to translate later on when the object makes a notify call
            client.MapLocalObjectID(this.DynamicId, remoteObjectId);
            this.Subscribers.Add(client);
            // Since the client wasn't previously subscribed, it should not be aware of the object's state -- let's notify it
            this.NotifySubscriptionAdded(client);
        }

        /// <summary>
        /// Removes a given subscriber and unmaps the object's dynamic ID.
        /// </summary>
        /// <param name="client">The client to remove.</param>
        public void RemoveSubscriber(MooNetClient client)
        {
            if (!this.Subscribers.Contains(client))
            {
                Logger.Warn("Attempted to remove non-subscriber {0}", client.Connection.RemoteEndPoint.ToString());
                return;
            }
            // Unmap the object from the client
            client.UnmapLocalObjectId(this.DynamicId);
            this.Subscribers.Remove(client);
            // We don't need to do a notify nor respond to the client with anything since the client will ultimately act
            // like the object never existed in the first place
        }

        /// <summary>
        /// Notifies a specific subscriber about the object's present state.
        /// This methods should be actually implemented by deriving object classes.
        /// </summary>
        /// <param name="client">The subscriber.</param>
        protected virtual void NotifySubscriptionAdded(MooNetClient client) { }

        // ** We're yet not sure about this, so commenting out **
        ///// <summary>
        ///// Notifies all subscribers with the object's current state.
        ///// </summary>
        //public void NotifyAllSubscribers()
        //{
        //    foreach (var subscriber in this.Subscribers)
        //    {
        //        this.NotifySubscriptionAdded(subscriber);
        //    }
        //}

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
            if (disposing) { } // Dispose any managed resources - here we don't have any

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
