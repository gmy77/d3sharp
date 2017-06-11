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
using Mooege.Core.GS.Common.Types.Scene;
using Mooege.Net.GS.Message.Fields;

namespace Mooege.Net.GS.Message.Definitions.Scene
{
    [Message(Opcodes.RevealSceneMessage)]
    public class RevealSceneMessage : GameMessage
    {
        public uint WorldID;
        public SceneSpecification SceneSpec;
        public uint ChunkID;
        public int /* sno */ SceneSNO;
        public PRTransform Transform;
        public uint ParentChunkID;
        public int /* sno */ SceneGroupSNO;
        // MaxLength = 256
        public int /* gbid */[] arAppliedLabels;

        public RevealSceneMessage() : base(Opcodes.RevealSceneMessage) {}

        public override void Parse(GameBitBuffer buffer)
        {
            WorldID = buffer.ReadUInt(32);
            SceneSpec = new SceneSpecification();
            SceneSpec.Parse(buffer);
            ChunkID = buffer.ReadUInt(32);
            SceneSNO = buffer.ReadInt(32);
            Transform = new PRTransform();
            Transform.Parse(buffer);
            ParentChunkID = buffer.ReadUInt(32);
            SceneGroupSNO = buffer.ReadInt(32);
            arAppliedLabels = new int /* gbid */[buffer.ReadInt(9)];
            for (int i = 0; i < arAppliedLabels.Length; i++) arAppliedLabels[i] = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteUInt(32, WorldID);
            SceneSpec.Encode(buffer);
            buffer.WriteUInt(32, ChunkID);
            buffer.WriteInt(32, SceneSNO);
            Transform.Encode(buffer);
            buffer.WriteUInt(32, ParentChunkID);
            buffer.WriteInt(32, SceneGroupSNO);
            buffer.WriteInt(9, arAppliedLabels.Length);
            for (int i = 0; i < arAppliedLabels.Length; i++) buffer.WriteInt(32, arAppliedLabels[i]);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("RevealSceneMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("WorldID: 0x" + WorldID.ToString("X8") + " (" + WorldID + ")");
            SceneSpec.AsText(b, pad);
            b.Append(' ', pad); b.AppendLine("ChunkID: 0x" + ChunkID.ToString("X8") + " (" + ChunkID + ")");
            b.Append(' ', pad); b.AppendLine("SceneSNO: 0x" + SceneSNO.ToString("X8"));
            Transform.AsText(b, pad);
            b.Append(' ', pad); b.AppendLine("ParentChunkID: 0x" + ParentChunkID.ToString("X8") + " (" + ParentChunkID + ")");
            b.Append(' ', pad); b.AppendLine("SceneGroupSNO: 0x" + SceneGroupSNO.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("arAppliedLabels:");
            b.Append(' ', pad); b.AppendLine("{");
            for (int i = 0; i < arAppliedLabels.Length; ) { b.Append(' ', pad + 1); for (int j = 0; j < 8 && i < arAppliedLabels.Length; j++, i++) { b.Append("0x" + arAppliedLabels[i].ToString("X8") + ", "); } b.AppendLine(); }
            b.Append(' ', pad); b.AppendLine("}"); b.AppendLine();
            b.Append(' ', --pad);
            b.AppendLine("}");
        }
    }
}
