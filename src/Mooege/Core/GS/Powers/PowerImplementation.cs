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
