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

using System.Collections.Generic;
using CrystalMpq;
using Gibbed.IO;
using Mooege.Core.GS.Common.Types.Collusion;
using Mooege.Core.GS.Common.Types.Math;
using Mooege.Core.GS.Common.Types.SNO;
using Mooege.Common.MPQ.FileFormats.Types;

namespace Mooege.Common.MPQ.FileFormats
{
    [FileFormat(SNOGroup.Act)]
    public class Act : FileFormat
    {
        public Header Header;
        List<ActQuestInfo> ActQuestInfo;
        WaypointInfo[] WayPointInfo = new WaypointInfo[25];
        ResolvedPortalDestination ResolvedPortalDestination;
        ActStartLocOverride[] Field0 = new ActStartLocOverride[6];

        public Act(MpqFile file)
        {
            MpqFileStream stream = file.Open();
            Header = new Header(stream);

            ActQuestInfo = stream.ReadSerializedData<ActQuestInfo>();
            stream.Position += 12;
            for (int i = 0; i < WayPointInfo.Length; i++)
                WayPointInfo[i] = new WaypointInfo(stream);
            ResolvedPortalDestination = new ResolvedPortalDestination(stream);
            for (int i = 0; i < Field0.Length; i++)
                Field0[i] = new ActStartLocOverride(stream);

            stream.Close();
        }
    }

    public class WaypointInfo
    {
        public int SNOWorld;
        public int SNOLevelArea;
        public int I0;
        public int I1;
        public int I2;
        public int SNOQuestRange;
        public int I3;

        public WaypointInfo(MpqFileStream stream)
        {
            SNOWorld = stream.ReadValueS32();
            SNOLevelArea = stream.ReadValueS32();
            I0 = stream.ReadValueS32();
            I1 = stream.ReadValueS32();
            I2 = stream.ReadValueS32();
            SNOQuestRange = stream.ReadValueS32();
            I3 = stream.ReadValueS32();
        }
    }

    public class ActStartLocOverride
    {
        public ResolvedPortalDestination ResolvedPortalDestination;
        public int SNOQuestRange;

        public ActStartLocOverride(MpqFileStream stream)
        {
            ResolvedPortalDestination = new ResolvedPortalDestination(stream);
            SNOQuestRange = stream.ReadValueS32();
        }
    }


    public class ResolvedPortalDestination
    {
        public int SNOWorld;
        public int I0;
        public int SNODestLevelArea;

        public ResolvedPortalDestination(MpqFileStream stream)
        {
            SNOWorld = stream.ReadValueS32();
            I0 = stream.ReadValueS32();
            SNODestLevelArea = stream.ReadValueS32();
        }
    }

    public class ActQuestInfo : ISerializableData
    {
        public int SNOQuest;

        public void Read(MpqFileStream stream)
        {
            SNOQuest = stream.ReadValueS32();
        }
    }

}
