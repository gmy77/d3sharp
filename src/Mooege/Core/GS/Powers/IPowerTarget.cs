using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mooege.Core.GS.Actors;
using Mooege.Net.GS.Message.Fields;

namespace Mooege.Core.GS.Powers
{
    public interface IPowerTarget
    {
        void ReceiveDamage(Actor from, float amount, int type);
        Vector3D GetPosition();
        IList<ClientObjectId> GetIds();
    }
}
