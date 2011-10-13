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
using System.Linq;
using System.Text;
using System.Reflection;
using Mooege.Common;

namespace Mooege.Core.GS.Powers
{
    public abstract class PowerImplementation
    {
        static readonly Logger Logger = LogManager.CreateLogger();

        private static Dictionary<int, Type> _implementations = new Dictionary<int, Type>();

        public static PowerImplementation ImplementationForId(int id)
        {
            if (_implementations.ContainsKey(id))
            {
                return (PowerImplementation)Activator.CreateInstance(_implementations[id]);
            }
            else
            {
                Logger.Debug("Unimplemented power: {0}", id);
                return null;
            }
        }

        static PowerImplementation()
        {
            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (type.IsSubclassOf(typeof(PowerImplementation)))
                {
                    var attributes = (PowerImplementationAttribute[])type.GetCustomAttributes(typeof(PowerImplementationAttribute), true);
                    // TODO: have a class handle multiple power ids?
                    foreach (var powerAttribute in attributes)
                    {
                        _implementations[powerAttribute.Id] = type;
                    }
                }
            }
        }

        // Called to start executing a power
        // return yield an int to specify in milliseconds how long to wait before continuing.
        public abstract IEnumerable<int> Run(PowerParameters pp, PowersManager fx);
    }
}
