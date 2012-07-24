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

using System.Collections.Generic;
using CrystalMpq;
using Gibbed.IO;
using Mooege.Core.GS.Common.Types.SNO;
using Mooege.Common.MPQ.FileFormats.Types;

namespace Mooege.Common.MPQ.FileFormats
{
    [FileFormat(SNOGroup.Act)]
    public class Act : FileFormat
    {
        public Header Header { get; private set; }
        public List<ActQuestInfo> ActQuestInfo { get; private set; }
        public WaypointInfo[] WayPointInfo { get; private set; }
        public ResolvedPortalDestination ResolvedPortalDestination { get; private set; }
        public ActStartLocOverride[] ActStartLocOverrides { get; private set; }

        public Act(MpqFile file)
        {
            var stream = file.Open();
            this.Header = new Header(stream);

            this.ActQuestInfo = stream.ReadSerializedData<ActQuestInfo>();
            stream.Position += 12;

            this.WayPointInfo = new WaypointInfo[25];
            for (int i = 0; i < WayPointInfo.Length; i++)
                this.WayPointInfo[i] = new WaypointInfo(stream);

            this.ResolvedPortalDestination = new ResolvedPortalDestination(stream);

            this.ActStartLocOverrides = new ActStartLocOverride[6];
            for (int i = 0; i < ActStartLocOverrides.Length; i++)
                this.ActStartLocOverrides[i] = new ActStartLocOverride(stream);

            stream.Close();
        }
    }

    public class WaypointInfo
    {
        public int SNOWorld { get; private set; }
        public int SNOLevelArea { get; private set; }
        public int I0 { get; private set; }
        public int I1 { get; private set; }
        public int I2 { get; private set; }
        public int SNOQuestRange { get; private set; }
        public int I3 { get; private set; }

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
        public ResolvedPortalDestination ResolvedPortalDestination { get; private set; }
        public int SNOQuestRange { get; private set; }

        public ActStartLocOverride(MpqFileStream stream)
        {
            ResolvedPortalDestination = new ResolvedPortalDestination(stream);
            SNOQuestRange = stream.ReadValueS32();
        }
    }


    public class ResolvedPortalDestination
    {
        public int SNOWorld { get; private set; }
        public int I0 { get; private set; }
        public int SNODestLevelArea { get; private set; }

        public ResolvedPortalDestination(MpqFileStream stream)
        {
            SNOWorld = stream.ReadValueS32();
            I0 = stream.ReadValueS32();
            SNODestLevelArea = stream.ReadValueS32();
        }
    }

    public class ActQuestInfo : ISerializableData
    {
        public int SNOQuest { get; private set; }

        public void Read(MpqFileStream stream)
        {
            SNOQuest = stream.ReadValueS32();
        }
    }
}
