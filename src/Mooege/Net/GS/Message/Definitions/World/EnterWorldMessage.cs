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
using Mooege.Core.GS.Common.Types.Math;
using Mooege.Net.GS.Message.Fields;

namespace Mooege.Net.GS.Message.Definitions.World
{
    [Message(Opcodes.EnterWorldMessage)]
    public class EnterWorldMessage : GameMessage
    {
        public Vector3D EnterPosition;
        public uint WorldID; // World's DynamicID
        public int /* sno */ WorldSNO;

        public EnterWorldMessage() : base(Opcodes.EnterWorldMessage) {}

        public override void Parse(GameBitBuffer buffer)
        {
            EnterPosition = new Vector3D();
            EnterPosition.Parse(buffer);
            WorldID = buffer.ReadUInt(32);
            WorldSNO = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            EnterPosition.Encode(buffer);
            buffer.WriteUInt(32, WorldID);
            buffer.WriteInt(32, WorldSNO);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("EnterWorldMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            EnterPosition.AsText(b, pad);
            b.Append(' ', pad); b.AppendLine("WorldID: 0x" + WorldID.ToString("X8") + " (" + WorldID + ")");
            b.Append(' ', pad); b.AppendLine("WorldSNO: 0x" + WorldSNO.ToString("X8"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }
    }
}
