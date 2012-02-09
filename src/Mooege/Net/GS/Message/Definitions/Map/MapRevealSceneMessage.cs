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

namespace Mooege.Net.GS.Message.Definitions.Map
{
    [Message(Opcodes.MapRevealSceneMessage)]
    public class MapRevealSceneMessage : GameMessage
    {
        public uint ChunkID;
        public int /* sno */ SceneSNO;
        public PRTransform Transform;
        public uint WorldID;
        public bool MiniMapVisibility;

        public MapRevealSceneMessage() : base(Opcodes.MapRevealSceneMessage) {}

        public override void Parse(GameBitBuffer buffer)
        {
            ChunkID = buffer.ReadUInt(32);
            SceneSNO = buffer.ReadInt(32);
            Transform = new PRTransform();
            Transform.Parse(buffer);
            WorldID = buffer.ReadUInt(32);
            MiniMapVisibility = buffer.ReadBool(); ;
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteUInt(32, ChunkID);
            buffer.WriteInt(32, SceneSNO);
            Transform.Encode(buffer);
            buffer.WriteUInt(32, WorldID);
            buffer.WriteBool(MiniMapVisibility);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("MapRevealSceneMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("ChunkID: 0x" + ChunkID.ToString("X8") + " (" + ChunkID + ")");
            b.Append(' ', pad); b.AppendLine("SceneSNO: 0x" + SceneSNO.ToString("X8"));
            Transform.AsText(b, pad);
            b.Append(' ', pad); b.AppendLine("WorldID: 0x" + WorldID.ToString("X8") + " (" + WorldID + ")");
            b.Append(' ', pad); b.AppendLine("MiniMapVisibility: " + (MiniMapVisibility ? "true" : "false"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }
    }
}
