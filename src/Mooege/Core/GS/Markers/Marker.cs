using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mooege.Core.GS.Markers
{
    public enum MarkerTypes : int
    {
        Checkpoint = 3795,
        /// <summary>
        /// Single Player start location
        /// </summary>
        Start_Location_0 = 5502,
        /// <summary>
        /// Multiplayer start location
        /// </summary>
        Start_Location_Team_0= 5503,
        Waypoint = 6442,
        Banner_Player_1 = 123714,
        Banner_Player_2 = 123715,
        Banner_Player_3 = 123716,
        Banner_Player_4 = 123717,
        Savepoint = 180941,
    }

    public enum MarkerTagTypes : int
    {
        Scale                = 524288,
        // Used for portal destination resolution
        DestinationWorld     = 526850,
        DestinationActorTag  = 526851,
        ActorTag             = 526852,  // maybe find another name
        DestinationLevelArea = 526853,
    }
}
