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
using CrystalMpq;
using Gibbed.IO;
using Mooege.Core.GS.Common.Types.Math;
using Mooege.Net.GS.Message;
using Mooege.Common.Storage;

namespace Mooege.Core.GS.Common.Types.Scene
{
    public class SceneSpecification
    {
        [PersistentProperty("CellZ")]
        public int CellZ { get; set; } // Position.Z rounded down
        [PersistentProperty("Cell")]
        public Vector2D Cell { get; set; }
        [PersistentProperty("SNOLevelAreas", 4)]
        public int[] SNOLevelAreas { get; set; } // Area names - MaxLength = 4
        [PersistentProperty("SNOPrevWorld")]
        public int SNOPrevWorld { get; set; }
        [PersistentProperty("Unknown1")]
        public int Unknown1 { get; set; }
        [PersistentProperty("SNOPrevLevelArea")]
        public int SNOPrevLevelArea { get; set; }
        [PersistentProperty("SNONextWorld")]
        public int SNONextWorld { get; set; }
        [PersistentProperty("Unknown2")]
        public int Unknown2 { get; set; }
        [PersistentProperty("SNONextLevelArea")]
        public int SNONextLevelArea { get; set; }
        [PersistentProperty("SNOMusic")]
        public int SNOMusic { get; set; }
        [PersistentProperty("SNOCombatMusic")]
        public int SNOCombatMusic { get; set; }
        [PersistentProperty("SNOAmbient")]
        public int SNOAmbient { get; set; }
        [PersistentProperty("SNOReverb")]
        public int SNOReverb { get; set; }
        [PersistentProperty("SNOWeather")]
        public int SNOWeather { get; set; }
        [PersistentProperty("SNOPresetWorld")]
        public int SNOPresetWorld { get; set; }
        [PersistentProperty("Unknown3")]
        public int Unknown3 { get; set; }
        [PersistentProperty("Unknown4")]
        public int Unknown4 { get; set; }
        [PersistentProperty("Unknown5")]
        public int Unknown5 { get; set; }
        [PersistentProperty("ClusterID")]
        public int ClusterID { get; set; }
        [PersistentProperty("SceneCachedValues")]
        public SceneCachedValues SceneCachedValues { get; set; }

        public SceneSpecification() { }

        /// <summary>
        /// Reads SceneSpecification from given MPQFileStream.
        /// </summary>
        /// <param name="stream">The MPQFileStream to read from.</param>
        public SceneSpecification(MpqFileStream stream)
        {
            CellZ = stream.ReadValueS32();
            Cell = new Vector2D(stream);
            SNOLevelAreas = new int[4];

            for (int i = 0; i < SNOLevelAreas.Length; i++)
            {
                SNOLevelAreas[i] = stream.ReadValueS32();
            }

            SNOPrevWorld = stream.ReadValueS32();
            Unknown1 = stream.ReadValueS32();
            SNOPrevLevelArea = stream.ReadValueS32();
            SNONextWorld = stream.ReadValueS32();
            Unknown2 = stream.ReadValueS32();
            SNONextLevelArea = stream.ReadValueS32();
            SNOMusic = stream.ReadValueS32();
            SNOCombatMusic = stream.ReadValueS32();
            SNOAmbient = stream.ReadValueS32();
            SNOReverb = stream.ReadValueS32();
            SNOWeather = stream.ReadValueS32();
            SNOPresetWorld = stream.ReadValueS32();
            Unknown3 = stream.ReadValueS32();
            Unknown4 = stream.ReadValueS32();
            Unknown5 = stream.ReadValueS32();
            stream.Position += (9 * 4);
            ClusterID = stream.ReadValueS32();
            SceneCachedValues = new SceneCachedValues(stream);
        }

        /// <summary>
        /// Parses SceneSpecification from given GameBitBuffer.
        /// </summary>
        /// <param name="buffer">The GameBitBuffer to parse from.</param>
        public void Parse(GameBitBuffer buffer)
        {
            CellZ = buffer.ReadInt(32);
            Cell = new Vector2D();
            Cell.Parse(buffer);
            SNOLevelAreas = new int /* sno */[4];
            for (int i = 0; i < SNOLevelAreas.Length; i++) SNOLevelAreas[i] = buffer.ReadInt(32);
            SNOPrevWorld = buffer.ReadInt(32);
            Unknown1 = buffer.ReadInt(32);
            SNOPrevLevelArea = buffer.ReadInt(32);
            SNONextWorld = buffer.ReadInt(32);
            Unknown2 = buffer.ReadInt(32);
            SNONextLevelArea = buffer.ReadInt(32);
            SNOMusic = buffer.ReadInt(32);
            SNOCombatMusic = buffer.ReadInt(32);
            SNOAmbient = buffer.ReadInt(32);
            SNOReverb = buffer.ReadInt(32);
            SNOWeather = buffer.ReadInt(32);
            SNOPresetWorld = buffer.ReadInt(32);
            Unknown3 = buffer.ReadInt(32);
            Unknown4 = buffer.ReadInt(32);
            Unknown5 = buffer.ReadInt(32);
            ClusterID = buffer.ReadInt(32);
            SceneCachedValues = new SceneCachedValues();
            SceneCachedValues.Parse(buffer);
        }

        /// <summary>
        /// Encodes SceneSpecification to given GameBitBuffer.
        /// </summary>
        /// <param name="buffer">The GameBitBuffer to write.</param>
        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, CellZ);
            Cell.Encode(buffer);
            for (int i = 0; i < SNOLevelAreas.Length; i++) buffer.WriteInt(32, SNOLevelAreas[i]);
            buffer.WriteInt(32, SNOPrevWorld);
            buffer.WriteInt(32, Unknown1);
            buffer.WriteInt(32, SNOPrevLevelArea);
            buffer.WriteInt(32, SNONextWorld);
            buffer.WriteInt(32, Unknown2);
            buffer.WriteInt(32, SNONextLevelArea);
            buffer.WriteInt(32, SNOMusic);
            buffer.WriteInt(32, SNOCombatMusic);
            buffer.WriteInt(32, SNOAmbient);
            buffer.WriteInt(32, SNOReverb);
            buffer.WriteInt(32, SNOWeather);
            buffer.WriteInt(32, SNOPresetWorld);
            buffer.WriteInt(32, Unknown3);
            buffer.WriteInt(32, Unknown4);
            buffer.WriteInt(32, Unknown5);
            buffer.WriteInt(32, ClusterID);
            SceneCachedValues.Encode(buffer);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("SceneSpecification:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad);
            b.AppendLine("CellZ: 0x" + CellZ.ToString("X8") + " (" + CellZ + ")");
            Cell.AsText(b, pad);
            b.Append(' ', pad);
            b.AppendLine("arSnoLevelAreas:");
            b.Append(' ', pad);
            b.AppendLine("{");
            for (int i = 0; i < SNOLevelAreas.Length; )
            {
                b.Append(' ', pad + 1);
                for (int j = 0; j < 8 && i < SNOLevelAreas.Length; j++, i++)
                {
                    b.Append("0x" + SNOLevelAreas[i].ToString("X8") + ", ");
                }
                b.AppendLine();
            }
            b.Append(' ', pad);
            b.AppendLine("}");
            b.AppendLine();
            b.Append(' ', pad);
            b.AppendLine("snoPrevWorld: 0x" + SNOPrevWorld.ToString("X8"));
            b.Append(' ', pad);
            b.AppendLine("Unknown1: 0x" + Unknown1.ToString("X8") + " (" + Unknown1 + ")");
            b.Append(' ', pad);
            b.AppendLine("snoPrevLevelArea: 0x" + SNOPrevLevelArea.ToString("X8"));
            b.Append(' ', pad);
            b.AppendLine("snoNextWorld: 0x" + SNONextWorld.ToString("X8"));
            b.Append(' ', pad);
            b.AppendLine("Unknown2: 0x" + Unknown2.ToString("X8") + " (" + Unknown2 + ")");
            b.Append(' ', pad);
            b.AppendLine("snoNextLevelArea: 0x" + SNONextLevelArea.ToString("X8"));
            b.Append(' ', pad);
            b.AppendLine("snoMusic: 0x" + SNOMusic.ToString("X8"));
            b.Append(' ', pad);
            b.AppendLine("snoCombatMusic: 0x" + SNOCombatMusic.ToString("X8"));
            b.Append(' ', pad);
            b.AppendLine("snoAmbient: 0x" + SNOAmbient.ToString("X8"));
            b.Append(' ', pad);
            b.AppendLine("snoReverb: 0x" + SNOReverb.ToString("X8"));
            b.Append(' ', pad);
            b.AppendLine("snoWeather: 0x" + SNOWeather.ToString("X8"));
            b.Append(' ', pad);
            b.AppendLine("snoPresetWorld: 0x" + SNOPresetWorld.ToString("X8"));
            b.Append(' ', pad);
            b.AppendLine("Unknown3: 0x" + Unknown3.ToString("X8") + " (" + Unknown3 + ")");
            b.Append(' ', pad);
            b.AppendLine("Unknown4: 0x" + Unknown4.ToString("X8") + " (" + Unknown4 + ")");
            b.Append(' ', pad);
            b.AppendLine("Unknown5: 0x" + Unknown5.ToString("X8") + " (" + Unknown5 + ")");
            b.Append(' ', pad);
            b.AppendLine("ClusterId: 0x" + ClusterID.ToString("X8") + " (" + ClusterID + ")");
            SceneCachedValues.AsText(b, pad);
            b.Append(' ', --pad);
            b.AppendLine("}");
        }
    }
}
