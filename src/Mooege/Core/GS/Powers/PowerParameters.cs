using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mooege.Core.GS.Actors;
using Mooege.Net.GS.Message.Fields;
using Mooege.Net.GS.Message.Definitions.Combat;

namespace Mooege.Core.GS.Powers
{
    public class PowerParameters
    {
        public Actor User;
        public Actor Target;
        public Vector3D TargetPosition;
        public TargetMessage Message;
        public bool UserIsChanneling;
        public bool ThrottledCast;
    }
}
