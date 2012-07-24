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

using System.Text;

namespace Mooege.Net.GS.Message.Fields
{
    public class ResolvedPortalDestination
    {
        public int /* sno */ WorldSNO;
        public int StartingPointActorTag;       // in the target world is (should be!) a starting point, that is tagged with this id
        public int /* sno */ DestLevelAreaSNO;

        public void Parse(GameBitBuffer buffer)
        {
            WorldSNO = buffer.ReadInt(32);
            StartingPointActorTag = buffer.ReadInt(32);
            DestLevelAreaSNO = buffer.ReadInt(32);
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, WorldSNO);
            buffer.WriteInt(32, StartingPointActorTag);
            buffer.WriteInt(32, DestLevelAreaSNO);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("ResolvedPortalDestination:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad);
            b.AppendLine("WorldSNO: 0x" + WorldSNO.ToString("X8"));
            b.Append(' ', pad);
            b.AppendLine("StartingPointActorTag: 0x" + StartingPointActorTag.ToString("X8") + " (" + StartingPointActorTag + ")");
            b.Append(' ', pad);
            b.AppendLine("DestLevelAreaSNO: 0x" + DestLevelAreaSNO.ToString("X8"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}
