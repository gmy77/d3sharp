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

namespace Mooege.Core.MooNet.Objects
{
    /// <summary>
    /// PersistentRPCObjects are RPCObjects that are persisted in a database or other external resource.
    /// Since this derives from RPCObject, it has a dynamic ID which can be used in RPC communication for
    /// referencing the object at the remote end, additional to its persistent ID which stays same
    /// throughout sessions.
    /// </summary>
    public class PersistentRPCObject : RPCObject
    {
        /// <summary>
        /// The persistent ID for the object which will stay same through sessions, and can be used
        /// as an index in database tables.
        /// </summary>
        public ulong PersistentID { get; private set; }

        /// <summary>
        /// Parameterless ctor will **generate** a new persistent ID.
        /// </summary>
        protected PersistentRPCObject()
        {
            // We can't generate our persistent ID here but instead rely on deriving-class implementation.
            // The deriving type has to handle generation because the persistent ID is domain-specific (most of the time).
            // NOTE: Resharper or sample tools may give a warning over this line indicating: "virtual member call in constructor",
            // but in C# virtual method calls always run on the most derived type. So we're fine.
            // Check these for more information: http://goo.gl/xv7WE, http://goo.gl/x4ep2
            this.PersistentID = this.GenerateNewPersistentId();
        }

        /// <summary>
        /// Creates a new PersistentRPCObject memory instance with the given persistent ID.
        /// </summary>
        /// <param name="persistentId">The persistent ID to initialize with.</param>
        protected PersistentRPCObject(ulong persistentId)
        {
            this.PersistentID = persistentId;
        }

        /// <summary>
        /// Virtual function that must be implemented by derives classes, which should return a new unique
        /// persistent ID in the derived-class's domain -- to be used as a PersistentRPCObject's persistent ID.
        /// </summary>
        /// <returns>Returns a unique persistent ID for the derived-class's domain.</returns>
        protected virtual ulong GenerateNewPersistentId()
        {
            throw new NotImplementedException();
        }

        // Note on PersistentRPCObject derived-class ID generation strategy:
        // the derived classes first get the last persistent ID known for the class' domain and should assign values following this counter.
        // A very simple implementation would be like:
        //
        // public class SamplePersistentDerivedType : PersistentRPCObject
        // {
        //    private static ulong? _persistentIdCounter = null; // Make it nullable
        //
        //    protected override ulong GenerateNewPersistentId()
        //    {
        //        if (_persistentIdCounter == null)  // At the initial memory instantation for the type,
        //            this.SetPersistentIdCounter(); // we should get the lowest free persistent ID value for our domain
        //
        //        return (ulong)_persistentIdCounter++;
        //    }
        //
        //    private void SetPersistentIdCounter() // Get the lowest free persistent ID for the domain
        //    {
        //        _persistentIdCounter = GetFromDB(); // Get the value from the domain's table
        //    }
        // }
    }
}
