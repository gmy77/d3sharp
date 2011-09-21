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
using System.Linq;
using System.Text;

namespace D3Sharp.Core.Objects
{
    /// <summary>
    /// PersistentRPCObjects are 'objects' that can be referenced over client-server communication process
    /// that are persistable to a storage like database.
    /// They extend from RPCObject which will also chain them a DynamicID 
    /// (which can be used by rpc comminucation process for referencing object at the remote end)
    /// additional to it's PersistentID which stays same through sessions.
    /// </summary>
    public class PersistentRPCObject : RPCObject
    {
        /// <summary>
        /// The persistent Id for the object which will stay same through sessions
        /// and can be used for index in database tables.
        /// </summary>
        public ulong PersistentID { get; private set; }

        /// <summary>
        /// Parameterless ctor will **generate** a new PersistentId.
        /// </summary>
        protected PersistentRPCObject()
        {
            // We can't generate an PersistentId here or but instead rely on derived-types implementation.
            // So that derived type can return us a unique persistent id to use (unique in derived-types domain).
            // Note: Resharper or sample tools may give a warning over this line indicating; virtual member call in constructor.
            // But in c# virtual method calls always run on the most derived type. So we're just okay.
            // Check these for more information: http://goo.gl/xv7WE, http://goo.gl/x4ep2
            this.PersistentID = this.GenerateNewPersistentId(); 
        }

        /// <summary>
        /// Creates a new PersistentRPCObject memory instance with given persistentId.
        /// </summary>
        /// <param name="persistentId"></param>
        protected PersistentRPCObject(ulong persistentId)
        {
            this.PersistentID = persistentId;
        }

        /// <summary>
        /// Virtual function that must be implemented by derives types that should return a new unique Id on derived types-domain -- to be used as Persistent Id.
        /// </summary>
        /// <returns>Returns a unique Id for given derived-type's domain.</returns>
        protected virtual ulong GenerateNewPersistentId()
        {
            throw new NotImplementedException();
        }

        // Note on PersistentRPCObject derived types id generation strategy:
        // The derived types first get the last persistant-Id known for the type-domain and should assign values following this counter.
        // A very simple implementation would be like;
        //
        // public class SamplePersistantDerivedType : PersistentRPCObject
        // {
        //    private static ulong? _persistantIdCounter = null; // make it nullable.

        //    protected override ulong GenerateNewPersistentId()
        //    {
        //        if (_persistantIdCounter == null)  // at the very initial memory instantation for the type, 
        //            this.SetPersistantIdCounter(); // we should get the lowest free persistant-Id value in the type-domain.
        //
        //        return (ulong)_persistantIdCounter++;
        //    }
        //
        //    private void SetPersistantIdCounter() // get lowest free persistent-id value for tye type-domain.
        //    {
        //        _persistantIdCounter = getfromdb(); // get the value from type-domain table
        //    }
        // }
    }    
}
